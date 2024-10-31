using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    melee,
    range
};

public class EnemyAI : MonoBehaviour, IDamageAble<float>
{
    public float currentHp = 20;
    public float maxHp = 20;

    public EnemyType enemyType;
    [Header("Range")]
    [SerializeField]
    float _detectRange = 10f;
    [SerializeField]
    float _attackRange = 5f;

    [SerializeField]
    float _movementSpeed = 10f;
    [SerializeField]
    EnemyAttack enemyAttack;
    
    public GameObject bullet;
    public Transform shotPosition;

    spawner1 spawner;
    Rigidbody rigid;
    BehaviorTreeRunner _BTRunner = null;
    Transform _detectedPlayer = null;
    Vector3 _originPos;
    Animator animator;
    public float damage = 10;
    public bool isDead = false;
    const string _MELEE_ATTACK_ANIM_STATE_NAME = "attack01";
    const string _RANGE_ATTACK_ANIM_STATE_NAME = "shot01";
    const string _ATTACK_ANIM_TRIGGER_NAME = "attack";
    const string _RANGE_ATTACK_ANIM_TRIGGER_NAME = "shot";
    DamageTextManager damagetextManager;

    float slowDelay;
    WaitForSeconds slowT;

    Material hitMaterial;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        enemyAttack = GetComponentInChildren<EnemyAttack>();
        animator = GetComponent<Animator>();
        _BTRunner = new BehaviorTreeRunner(SettingBT());
        _originPos = transform.position;
    }

    void Start()
    {
        damagetextManager = DamageTextManager.instance;
        slowT = new WaitForSeconds(slowDelay);
    }

    private void OnEnable()
    {
        //GameManager.instance.dieDelegate += Test;
        damage = 10;
        maxHp = 40;
        currentHp = maxHp;

        if (GameManager.instance.spawner.currentStage != 1)
            currentHp = (maxHp * (GameManager.instance.spawner.currentStage + GameManager.instance.spawner.currentWave * 0.2f));
        else
            currentHp = maxHp;

        if (GameManager.instance.spawner.currentStage != 1)
        {
            for (int i = 0; i < GameManager.instance.spawner.currentStage; i++)
            {
                damage *= 1.3f;
                Mathf.Round(damage);
            }
        }


        isDead = false;
        gameObject.layer = 8;
        hitMaterial = GetComponentInChildren<SkinnedMeshRenderer>().materials[1];
    }

    private void OnDisable()
    {
        foreach (var item in GetComponentsInChildren<BladeStorm>())
        {
            Debug.Log("아이템 부모 초기화");
            item.transform.SetParent(null);
            item.gameObject.SetActive(false);
        }

        //GameManager.instance.dieDelegate -= Test;
    }

    private void Update()
    {
        if (isDead) return;
        _BTRunner.Operate();
    }

    INode SettingBT()
    {
        return new SelectorNode
            (
                new List<INode>()
                {
                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(CheckMeleeAttacking),
                            new ActionNode(CheckEnemyWithinAttackRange),
                            new ActionNode(DoAttack),
                        }
                    ),
                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(CheckDetectEnemy),
                            new ActionNode(MoveToDetectEnemy),
                        }
                    ),
                    // new ActionNode(MoveToOriginPosition)
                }
            );
    }

    bool IsAniamtionRunning(string stateName)
    {
        if (animator != null)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            {
                var normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                return normalizedTime != 0 && normalizedTime < 1f;
            }
        }
        return false;
    }

    #region Attack Node
    INode.ENodeState CheckMeleeAttacking()
    {
        if (IsAniamtionRunning(_ATTACK_ANIM_TRIGGER_NAME))
        {
            return INode.ENodeState.ENS_Running;
        }
        return INode.ENodeState.ENS_Success;
    }

    INode.ENodeState CheckEnemyWithinAttackRange()
    {
        if (_detectedPlayer != null)
        {
            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position)
                < (_attackRange * _attackRange))
            {
                return INode.ENodeState.ENS_Success;
            }
        }
        return INode.ENodeState.ENS_Failure;
    }

    INode.ENodeState DoAttack()
    {
        if (_detectedPlayer != null && !isDead)
        {
            if (enemyType == EnemyType.melee)
                Attack();
            else if (enemyType == EnemyType.range)
            {
                Rotate();
                Attack();
            }
            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }
    #endregion

    #region Detect & Move Node
    INode.ENodeState CheckDetectEnemy()
    {
        var overlapColliders = Physics.OverlapSphere(transform.position, _detectRange, LayerMask.GetMask("Player"));

        if (overlapColliders != null && overlapColliders.Length > 0)
        {
            _detectedPlayer = overlapColliders[0].transform;
            Rotate();
            animator.SetFloat("moveSpeed", 1);
            return INode.ENodeState.ENS_Success;
        }

        _detectedPlayer = null;
        animator.SetFloat("moveSpeed", 0);
        return INode.ENodeState.ENS_Failure;
    }

    INode.ENodeState MoveToDetectEnemy()
    {
        if (_detectedPlayer != null && !isDead)
        {
            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_attackRange * _attackRange))
            {
                return INode.ENodeState.ENS_Success;
            }
            Rotate();
            Move();
            return INode.ENodeState.ENS_Running;
        }
        return INode.ENodeState.ENS_Failure;
    }
    #endregion

    #region  Move Origin Pos Node
    INode.ENodeState MoveToOriginPosition()
    {
        if (Vector3.SqrMagnitude(_originPos - transform.position) < float.Epsilon * float.Epsilon)
        {
            return INode.ENodeState.ENS_Success;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _originPos, Time.deltaTime * _movementSpeed);
            return INode.ENodeState.ENS_Running;
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, _detectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, _attackRange);
    }

    public void Attack()
    {
        animator.SetTrigger(_ATTACK_ANIM_TRIGGER_NAME);
    }

    public void Damage(float damageTaken)
    {
        if(isDead) return;
        
        animator.SetTrigger("hit");
        // hitSound.Play();
        currentHp -= damageTaken;

        StopCoroutine("hitMaterialChange");
        StartCoroutine("hitMaterialChange");

        Vector3 textPos = transform.position;
        textPos.y += 1.5f;
        damagetextManager.GetDamageTextObject().GetComponent<DamageText>().Init(damageTaken, textPos, false);

        Debug.Log(currentHp);
        if (currentHp <= 0)
        {
            currentHp = 0;            
            Dead();
        }
    }

    private IEnumerator hitMaterialChange()
    {
        hitMaterial.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        hitMaterial.color = Color.black;
    }

    public void PlayKnockback(Vector3 direction, float duration, float strength)
    {
        if (this.gameObject.layer == 13)
            return;

        StartCoroutine(Knockback(direction, duration, strength));
    }

    IEnumerator Knockback(Vector3 direction, float duration, float strength)
    {
        float time = 0;
        Vector3 originalPosition = transform.position;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(originalPosition, originalPosition + direction * strength, time / duration);
            yield return null;
        }
    }

    public void Dead()
    {
        GameManager.instance.dieDelegate(transform);
                
        Debug.Log("Dead 실행");
        isDead = true;
        enemyAttack.gameObject.GetComponent<BoxCollider>().enabled = false;
        animator.StopPlayback();
        // animator.SetTrigger("dead");

        animator.ResetTrigger("attack");
        animator.ResetTrigger("hit");

        // 사망 애니메이션을 강제로 재생
        animator.Play("dead");

        // 사망 애니메이션을 부드럽게 전환
        animator.CrossFade("dead", 0.2f);
        gameObject.layer = 7;

        Invoke("InActiveEnemy", 3f);
        GameManager.instance.spawner.enemies.Remove(this.gameObject);
        GameManager.instance.spawner.enemyDead();           // 스포너에 적 사망시 호출 함수        
    }

    public void InActiveEnemy()
    {
        gameObject.SetActive(false);
    }

    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _detectedPlayer.position, Time.deltaTime * _movementSpeed);
    }

    public void Rotate()
    {
        transform.LookAt(_detectedPlayer);
    }

    public void AttackStateCollider()
    {
        enemyAttack.gameObject.GetComponent<BoxCollider>().enabled =
            !enemyAttack.gameObject.GetComponent<BoxCollider>().enabled;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageAble<float> damageAble = collision.gameObject.GetComponent<IDamageAble<float>>();
        if(this.gameObject.layer == 13)
        {
            return;
        }

        if (damageAble != null && collision.gameObject.tag == "Player")
        {
            damageAble.Damage(damage);
        }
    }

    public void Shot()
    {
        animator.SetTrigger("shot");
    }

    public void Fire()
    {
        bullet.GetComponent<EnemyBullet>().targetname = "Player";
        bullet.GetComponent<EnemyBullet>().damage = damage;
        GameObject temp = Instantiate(bullet, shotPosition.position, Quaternion.identity);
        temp.transform.forward = transform.forward;
        //temp.transform.Rotate(new Vector3(90f, transform.rotation.y, 0f));
    }

    public void Slow(float slow)
    {
        float moveSpeed = _movementSpeed;
        float slowSpeed = _movementSpeed - slow;

        _movementSpeed = slowSpeed;
        
        StartCoroutine(EnemySpeedReturn(moveSpeed));
        GameObject particle = GameManager.instance.particlePoolManager.GetParticle("FreezeDeBuff");
        if (particle != null)
        {
            particle.transform.position = transform.position;
            particle.transform.SetParent(this.transform);
        }
    }

    IEnumerator EnemySpeedReturn(float speed)
    {
        float recovery = speed * 0.1f;
        slowDelay = ItemDataBase.instance.Variable(47)*0.1f;

        while (speed > _movementSpeed)
        {
            yield return slowT;
            _movementSpeed += recovery * Time.deltaTime ;
        }

        if (speed < _movementSpeed)
            _movementSpeed = speed;
    }
}

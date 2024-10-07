using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEngine.EventSystems.EventTrigger;

public enum EnemyType
{
    melee,
    range
};

public class EnemyAI : MonoBehaviour, IDamageAble<float>
{
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
    float hp = 20;
    bool isDead = false;
    const string _MELEE_ATTACK_ANIM_STATE_NAME = "attack01";
    const string _RANGE_ATTACK_ANIM_STATE_NAME = "shot01";
    const string _ATTACK_ANIM_TRIGGER_NAME = "attack";
    const string _RANGE_ATTACK_ANIM_TRIGGER_NAME = "shot";
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        enemyAttack = GetComponentInChildren<EnemyAttack>();
        animator = GetComponent<Animator>();
        _BTRunner = new BehaviorTreeRunner(SettingBT());
        _originPos = transform.position;
    }

    private void OnEnable()
    {
        spawner = FindAnyObjectByType<spawner1>();
        isDead = false;
        hp = 20f;
        gameObject.layer = 8;
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
        StartCoroutine(Knockback(transform.forward * -1f, 0.2f, 1f));
        hp -= damageTaken;
        Debug.Log(hp);
        if (hp <= 0)
        {
            hp = 0;
            Dead();
        }
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
        spawner.enemyDead();           // 스포너에 적 사망시 호출 함수
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
        if (damageAble != null && collision.gameObject.tag == "Player")
        {
            damageAble.Damage(20);
        }
    }

    public void Shot()
    {
        animator.SetTrigger("shot");
    }

    public void Fire()
    {
        bullet.GetComponent<Bullet>().targetname = "Player";
        GameObject temp = Instantiate(bullet, shotPosition.position, Quaternion.identity);
        temp.transform.forward = transform.forward;
        //temp.transform.Rotate(new Vector3(90f, transform.rotation.y, 0f));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WareWolfBoss : Boss
{
    public float currentHp = 20;
    public float maxHp = 20;

    public EnemyType enemyType;

    [SerializeField]
    WarewolfAttack enemyAttack;

    public GameObject bullet;
    public Transform shotPosition;

    Rigidbody rigid;
    BehaviorTreeRunner _BTRunner = null;
    Vector3 _originPos;
    Animator animator;
    public float damage = 10;
    public bool isDead = false;
    const string _MELEE_ATTACK_ANIM_STATE_NAME = "attack01";
    const string _ATTACK_ANIM_TRIGGER_NAME = "attack";
    const string _FIRSTPATTERN_ANIM_TRIGGER_NAME = "isFirst";
    const string _SECONDPATTERN_ANIM_TRIGGER_NAME = "isSecond";
    const string _STING_ANIM_TRIGGER_NAME = "sting";
    bool canAttack = true;
    bool canMove = true;
    bool isSting = false;

    float slowDelay;
    WaitForSeconds slowT;

    Material hitMaterial;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        enemyAttack = GetComponentInChildren<WarewolfAttack>();
        animator = GetComponent<Animator>();
        _BTRunner = new BehaviorTreeRunner(SettingBT());
        _originPos = transform.position;
    }

    void Start()
    {
        slowT = new WaitForSeconds(slowDelay);
        
        _hp = 50f;
    }

    private void OnEnable()
    {
        //GameManager.instance.dieDelegate += Test;
        damage = 10;
        maxHp = 40;
        currentHp = maxHp;

        _attackRange = 2.75f;
        _movementSpeed = 2f;

        isDead = false;
        gameObject.layer = 8;
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
        _detectedPlayer = PlayerCharacter.Instance.transform;
        FirstCooldown(Time.deltaTime);
        SecondCooldown(Time.deltaTime);
    }

    public override INode SettingBT()
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
                    )
                }
            );
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
        if (_detectedPlayer != null && !isDead && canAttack)
        {
            if (_firstCoolTime <= 0f && !isSting)
                WarewolfFirstPattern();
            else if (_secondCoolTime <= 0f && !isSting)
                WarewolfSecondPattern();
            else
                WarewolfBaseAttack();

            canAttack = false;
            StartCoroutine(AttackDelay());
            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }
    #endregion

    #region Detect & Move Node
    INode.ENodeState CheckDetectEnemy()
    {
        if (_detectedPlayer != null)
        {
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
        if (_detectedPlayer != null && !isDead && canMove)
        {
            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_attackRange * _attackRange))
            {
                animator.SetBool("isClose", true);
                return INode.ENodeState.ENS_Success;
            }
            animator.SetBool("isClose", false);
            Rotate();
            Move();
            return INode.ENodeState.ENS_Running;
        }
        return INode.ENodeState.ENS_Failure;
    }

    #endregion


    //private IEnumerator hitMaterialChange()
    //{
    //    hitMaterial.color = Color.red;
    //    yield return new WaitForSeconds(0.3f);
    //    hitMaterial.color = Color.black;
    //}

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(3f);
        canAttack = true;
    }

    public void AttackColliderChange()
    {
        enemyAttack.enabled = !enemyAttack.enabled;
    }

    public void InActiveEnemy()
    {
        gameObject.SetActive(false);
    }

    public void AttackStart()
    {
        enemyAttack.gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    public void AttackEnd()
    {
        enemyAttack.gameObject.GetComponent<BoxCollider>().enabled = false;
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
        slowDelay = ItemDataBase.instance.Variable(47) * 0.1f;

        while (speed > _movementSpeed)
        {
            yield return slowT;
            _movementSpeed += recovery * Time.deltaTime;
        }

        if (speed < _movementSpeed)
            _movementSpeed = speed;
    }

    public override INode.ENodeState EvaluatePatterns()
    {
        throw new NotImplementedException();
    }

    public void WarewolfBaseAttack()
    {
        canMove = false;
        animator.SetTrigger(_ATTACK_ANIM_TRIGGER_NAME);
    }

    public void WarewolfFirstPattern()
    {
        canMove = false;
        animator.SetTrigger(_FIRSTPATTERN_ANIM_TRIGGER_NAME);
        _firstCoolTime = 8f;
    }

    public void WarewolfSecondPattern()
    {
        canMove = false;
        animator.SetTrigger(_SECONDPATTERN_ANIM_TRIGGER_NAME);        
    }

    public void WarewolfTeleport()
    {
        GameObject instance = GameManager.instance.particlePoolManager.GetParticle("TeleportFog");
        instance.transform.position = this.transform.position;
        StartCoroutine(Teleport());
    }

    IEnumerator Teleport()
    {
        this.transform.position = new Vector3(1000f, 1000f, 1000f);
        yield return new WaitForSeconds(1f);
        this.transform.position = -_detectedPlayer.transform.forward * 5f;
        this.transform.LookAt(_detectedPlayer);
        WarewolfStingPattern();
    }

    public void WarewolfStingPattern()
    {
        isSting = true;
        animator.SetTrigger(_STING_ANIM_TRIGGER_NAME);
        _secondCoolTime = 10f;
    }


    public void EndSting()
    {
        isSting = false;
    }

    public void CanMove()
    {
        canMove = true;
    }
}

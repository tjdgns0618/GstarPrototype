using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum BossType
{
    Worm,
    Knight,
    Bishop,
    Dragon
};


public class Boss : MonoBehaviour, IDamageAble<float>
{
    public BossType _bossType;
    public float _attackRange = 10f;
    public float __attackRange;
    public float _patternRange = 12f;
    float _movementSpeed = 10f;
    bool _isDead = false;
    bool _isAttacking;
    public bool _isFirstOn;
    public bool _isSecondOn;
    public bool _isThirdOn;

    public Transform _shotpos;

    public Transform _detectedPlayer = null;
    Rigidbody _rigid;

    BehaviorTreeRunner _BTRunner = null;
    Animator animator;
    spawner1 spawner;

    const string _NormalAttack_AnimTriggerName = "isNormal";
    const string _FirstPatternAttack_AnimTriggerName = "isFirst";
    const string _SecondPatternAttack_AnimTriggerName = "isSecond";
    const string _ThirdPatternAttack_AnimTriggerName = "isThird";
    const string _Dead_AnimTriggerName = "bossDead";

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        _BTRunner = new BehaviorTreeRunner(SettingBT());
    }

    void Start()
    {
        __attackRange = _attackRange * _attackRange;
    }

    private void Update()
    {
        if (_isDead) return;
        _BTRunner.Operate();
        _detectedPlayer = PlayerCharacter.Instance.transform;
    }

    INode SettingBT()
    {
        return new SelectorNode
            (
                new List<INode>()
                {
                    new BossSequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(CheckPlayerWithinRange),
                            new AttackSelector(this),
                        }
                    ),
                    new BossSequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(MoveToDetectEnemy),
                            new ActionNode(WormDetectEnemy),
                        }
                    ),
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
    INode.ENodeState CheckPlayerWithinRange()
    {
        if (_detectedPlayer != null)
        {
            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) <= (_attackRange * _attackRange) ||
                Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) <= (_patternRange * _patternRange))
            {
                return INode.ENodeState.ENS_Success;
            }
        }
        return INode.ENodeState.ENS_Failure;
    }

    public INode.ENodeState DoNormalAttack()
    {
        if (_detectedPlayer != null && !_isDead)
        {
            _isAttacking = true;
            Attack();
            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }

    public INode.ENodeState DoFirstPattern()
    {
        if (_detectedPlayer != null && !_isDead)
        {
            FirstPatternAttack();
            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }

    public INode.ENodeState DoSecondPattern()
    {
        if (_detectedPlayer != null && !_isDead)
        {
            SecondPatternAttack();
            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }

    public INode.ENodeState DoThirdPattern()
    {
        if (_detectedPlayer != null && !_isDead)
        {
            ThirdPatternAttack();
            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }
    #endregion


    #region Detect & Move Node
    INode.ENodeState MoveToDetectEnemy()
    {
        if (_detectedPlayer != null && !_isDead)
        {
            if (_bossType == BossType.Worm)
                return INode.ENodeState.ENS_Success;

            if (!_isAttacking)
            {
                Move();
                Rotate();
            }

            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_attackRange * _attackRange))
            {
                return INode.ENodeState.ENS_Success;
            }

            return INode.ENodeState.ENS_Running;
        }
        return INode.ENodeState.ENS_Failure;
    }

    INode.ENodeState WormDetectEnemy()
    {
        if (_detectedPlayer != null && !_isDead && _bossType == BossType.Worm)
        {
            if (!_isAttacking)
            {
                if (!IsLookingAtPlayer())
                {
                    Rotate();
                    return INode.ENodeState.ENS_Running;
                }
            }
                return INode.ENodeState.ENS_Success;
        }
        return INode.ENodeState.ENS_Failure;
    }
    #endregion


    public void Rotate()
    {
        transform.LookAt(_detectedPlayer);
    }

    public bool IsLookingAtPlayer()
    {
        Vector3 directionToPlayer = (_detectedPlayer.position - transform.position).normalized;
        Vector3 forward = transform.forward;

        return Vector3.Dot(forward, directionToPlayer) > 0.95f;
    }

    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _detectedPlayer.position, Time.deltaTime * _movementSpeed);
    }


    public void Attack()
    {
        if(_bossType == BossType.Worm)
        {
            animator.SetTrigger(_NormalAttack_AnimTriggerName);
            GameObject particle = GameManager.instance.particlePoolManager.GetParticle("Poison");
            if (particle != null)
            {
                particle.transform.position = _shotpos.transform.position;
                particle.transform.rotation = transform.rotation;
            }
        }
        if(_bossType == BossType.Knight)
        {

        }
        if(_bossType == BossType.Bishop)
        {

        }
        if(_bossType == BossType.Dragon)
        {

        }
    }

    public void FirstPatternAttack()
    {
        if (_bossType == BossType.Worm)
        {
            animator.SetTrigger("isFirst");
        }
        if (_bossType == BossType.Knight)
        {

        }
        if (_bossType == BossType.Dragon)
        {

        }
    }

    public void SecondPatternAttack()
    {
        if (_bossType == BossType.Worm)
        {
            //GroundDivein anim

            int rndPosX =  Random.Range(240, 261);
            int rndPosZ =  Random.Range(-5, 26);

            Vector3 secondPos = new Vector3(rndPosX, 0, rndPosZ);
            transform.position = secondPos;
            //GroundBreakThrough anim
        }
        if (_bossType == BossType.Knight)
        {

        }
        if (_bossType == BossType.Dragon)
        {

        }
    }

    public void ThirdPatternAttack()
    {
        if (_bossType == BossType.Worm)
        {

        }
        if (_bossType == BossType.Knight)
        {

        }
        if (_bossType == BossType.Dragon)
        {

        }
    }

    public void Damage(float damage)
    {

    }

    public void Shot()
    {

    }

    public void Dead()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, _attackRange);
    }
}

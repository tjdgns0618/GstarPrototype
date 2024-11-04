using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WareWolfBoss : Boss
{
    BehaviorTreeRunner _BTRunner = null;
    const string _Idle_AnimStateName = "Idle";
    const string _Taunting_AnimStateName = "Taunting";
    const string _Run_AnimStateName = "Run";
    public BoxCollider _attackCollider;

    private void Start()
    {
        _attackRange = 3.5f;
        __attackRange = _attackRange * _attackRange;
        _movementSpeed = 1.5f;
        _isDead = false;
        _isAttacking = false;
        _hp = 150f;
        _BTRunner = new BehaviorTreeRunner(SettingBT());
    }

    private void Update()
    {
        if (_isDead) return;
        _BTRunner.Operate();
        _detectedPlayer = PlayerCharacter.Instance.transform;
        NormalCooldown(Time.deltaTime);
        FirstCooldown(Time.deltaTime);
        SecondCooldown(Time.deltaTime);
        if (IsAniamtionRunning(_Idle_AnimStateName) || IsAniamtionRunning(_Run_AnimStateName))
            _isAttacking = false;
    }

    public void AttackColliderChange()
    {
        _attackCollider.enabled = !_attackCollider.enabled;
    }

    public override INode SettingBT()
    {
        return new SelectorNode
            (
                new List<INode>()
                {
                    new BossSequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(CheckPlayerWithinCoolTime),
                            new ActionNode(EvaluatePatterns),
                        }
                    ),
                    new BossSequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(MoveToDetectEnemy),
                        }
                    ),
                }
            );
    }


    public override INode.ENodeState EvaluatePatterns()
    {
        if (CanAttack())
        {
            if (CheckPlayerWithinCoolTime() == INode.ENodeState.ENS_Success)
            {
                float playerpos = Vector3.SqrMagnitude(_detectedPlayer.position - transform.position);
                if (IsInAttackRange(playerpos) && !_isAttacking)
                {
                    if (_normalCoolTime <= 0f)
                        return DoNormalAttack();
                    else if (_firstCoolTime <= 0f)
                        return DoFirstPattern();
                    else if (_secondCoolTime <= 0f && PlayerToBossDistance())
                        return DoSecondPattern();
                }
            }
        }
        return INode.ENodeState.ENS_Failure;
    }


    #region Attack Node

    INode.ENodeState CheckPlayerWithinCoolTime()
    {
        if (CanAttack())
        {
            if (_normalCoolTime <= 0f || _firstCoolTime <= 0f || _secondCoolTime <= 0f)
            {
                return INode.ENodeState.ENS_Success; // 공격 가능
            }
        }
        return INode.ENodeState.ENS_Failure;
    }

    public INode.ENodeState DoNormalAttack()
    {
        if (CanAttack())
        {
            Attack();
            _isAttacking = true;
            _normalCoolTime = 4f;
            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }

    public INode.ENodeState DoFirstPattern()
    {
        if (CanAttack())
        {
            FirstPatternAttack();
            _isAttacking = true;
            _firstCoolTime = 5f;
            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }

    public INode.ENodeState DoSecondPattern()
    {
        if (CanAttack())
        {
            SecondPatternAttack();
            _isAttacking = true;
            _secondCoolTime = 3f;
            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }
    #endregion


    #region Detect & Move Node
    INode.ENodeState MoveToDetectEnemy()
    {
        if (CanAttack())
        {
            if (!_isAttacking)
            {
                if (!IsLookingAtPlayer())
                {
                    Rotate();
                    Move();
                    return INode.ENodeState.ENS_Running;
                }
            }
            return INode.ENodeState.ENS_Success;
        }
        return INode.ENodeState.ENS_Failure;
    }
    #endregion    
}

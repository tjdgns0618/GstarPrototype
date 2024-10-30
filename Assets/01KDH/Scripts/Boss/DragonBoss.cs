using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBoss : Boss
{
    float _patternCount;

    BehaviorTreeRunner _BTRunner = null;
    const string _Idle_AnimStateName = "Idle";
    const string _Taunting_AnimStateName = "Taunting";

    public GameObject _breath;

    private void Start()
    {
        _attackRange = 6f;
        __attackRange = _attackRange * _attackRange;
        _patternRange = 10f;
        __patternRange = _patternRange * _patternRange;
        _movementSpeed = 0f;
        _patternCount = 0f;
        _isDead = false;
        _isAttacking = false;
        _hp = 200f;
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
        if (IsAniamtionRunning(_Idle_AnimStateName) || IsAniamtionRunning(_Taunting_AnimStateName))
            _isAttacking = false;
        Debug.Log(_patternCount);
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
                if (_patternCount >= 0f)
                    return DoThirdPattern();
                if (IsInAttackRange(playerpos))
                {
                    if (_normalCoolTime <= 0f)
                        return DoNormalAttack();
                }
                if(IsInPatternRange(playerpos))
                {
                    if (_firstCoolTime <= 0f)
                        return DoFirstPattern();
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
            if (_normalCoolTime <= 0f || _firstCoolTime <= 0f || _secondCoolTime <= 0f || _patternCount >= 4f)
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
            _normalCoolTime = 3f;
            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }

    public INode.ENodeState DoFirstPattern()
    {
        if (CanAttack())
        {
            FirstPatternAttack();
            _firstCoolTime = 10f;
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
            _secondCoolTime = 12f;
            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }
    public INode.ENodeState DoThirdPattern()
    {
        if (CanAttack())
        {
            ThirdPatternAttack();
            _isAttacking = true;
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
                if(IsLookingAtPlayer() || Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (__attackRange) 
                                       || Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (__patternRange))
                {
                    return INode.ENodeState.ENS_Success;
                }
            }
        }
        return INode.ENodeState.ENS_Failure;
    }
    #endregion

    public void BreathActive()
    {
        _breath.SetActive(true);
    }

    public void FinishBreath()
    {
        _firstCoolTime = 8f;
    }

    public void FinishPattern()
    {
        _patternCount++;
        if(_patternCount >= 5f)
           _patternCount = 0;
    }

}

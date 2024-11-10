using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BishopBoss : Boss
{
    BehaviorTreeRunner _BTRunner = null;
    const string _Idle_AnimStateName = "Idle";
    const string _Taunting_AnimStateName = "Taunting";

    AudioSource _audioSource;

    public AudioClip _taunt;
    public AudioClip _normal;

    public GameObject _spear;

    private void Start()
    {
        _attackRange = 3f;
        __attackRange = _attackRange * _attackRange;
        _patternRange = 6f;
        __patternRange = _patternRange * _patternRange;
        _movementSpeed = 0f;
        _isDead = false;
        _isAttacking = false;
        _hp = 600f;
        _BTRunner = new BehaviorTreeRunner(SettingBT());
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_isDead) return;
        _BTRunner.Operate();
        _detectedPlayer = PlayerCharacter.Instance.transform;
        NormalCooldown(Time.deltaTime);
        FirstCooldown(Time.deltaTime);
        SecondCooldown(Time.deltaTime);
        if (IsAniamtionRunning(_Idle_AnimStateName))
            _isAttacking = false;
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
        if (CanAttack() && !_isAttacking)
        {
            if (CheckPlayerWithinCoolTime() == INode.ENodeState.ENS_Success)
            {
                float playerpos = Vector3.SqrMagnitude(_detectedPlayer.position - transform.position);
                if (IsInAttackRange(playerpos))
                {
                    if (_normalCoolTime <= 0f)
                        return DoNormalAttack();
                }
                if(!IsInAttackRange(playerpos) && IsInPatternRange(playerpos))
                {
                    if (_firstCoolTime <= 0f)
                        return DoFirstPattern();
                }
                if(!IsInAttackRange(playerpos) && !IsInPatternRange(playerpos))
                {
                    if (_secondCoolTime <= 0f)
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
            Fire();
            _isAttacking = true;
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
            _secondCoolTime = 15f;
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
            float playerpos = Vector3.SqrMagnitude(_detectedPlayer.position - transform.position);
            if (!_isAttacking)
            {
                if (!IsLookingAtPlayer() && IsInAttackRange(playerpos))
                {
                    Rotate();
                    return INode.ENodeState.ENS_Running;
                }
                if (!IsInAttackRange(playerpos))
                {
                    Rotate();
                    Move();
                    return INode.ENodeState.ENS_Running;
                }
                if (IsLookingAtPlayer() || Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (__attackRange)
                                       || Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (__patternRange))
                {
                    return INode.ENodeState.ENS_Success;
                }
            }
        }
        return INode.ENodeState.ENS_Failure;
    }
    #endregion

    public void SpearAttack()
    {
        _spear.SetActive(true);
    }

    public void SpearAttackEnd()
    {
        _spear.SetActive(false);
    }

    public void RingStart()
    {
        _audioSource.clip = _taunt;
        _audioSource.Play();
    }

    public void NormalAttackStart()
    {
        _audioSource.clip = _normal;
        _audioSource.Play();
    }

}


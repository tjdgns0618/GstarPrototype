using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBoss : Boss
{
    float _patternCount;

    float _meteorRadius;

    BehaviorTreeRunner _BTRunner = null;
    const string _Idle_AnimStateName = "Idle";
    const string _Taunting_AnimStateName = "Taunting";
    const string _Breath_AnimStateName = "dragonBreath";

    public GameObject _breath;
    public GameObject _head;


    AudioSource _audioSource;
    public AudioClip _meteorSound;
    public AudioClip _idleSound;
    public AudioClip _groundSound;

    private void Start()
    {
        _attackRange = 5f;
        __attackRange = _attackRange * _attackRange;
        _patternRange = 8f;
        __patternRange = _patternRange * _patternRange;
        _movementSpeed = 3f;
        _patternCount = 0f;
        _isDead = false;
        _isAttacking = false;
        _hp = 18000f;
        _BTRunner = new BehaviorTreeRunner(SettingBT());
        _audioSource = GetComponent<AudioSource>();

        GameManager.instance.spawner.bossHpSlider.maxValue = _hp;
        GameManager.instance.spawner.bossHpSlider.value = _hp;
    }

    private void Update()
    {
        if (_isDead) return;
        _BTRunner.Operate();
        _detectedPlayer = PlayerCharacter.Instance.transform;
        NormalCooldown(Time.deltaTime);
        FirstCooldown(Time.deltaTime);
        SecondCooldown(Time.deltaTime);
        if (IsAniamtionRunning("dragonBreath"))
            Rotate();
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

    public override INode.ENodeState EvaluatePatterns()
    {
        if (CanAttack()&& !_isAttacking)
        {
            if (CheckPlayerWithinCoolTime() == INode.ENodeState.ENS_Success)
            {
                float playerpos = Vector3.SqrMagnitude(_detectedPlayer.position - transform.position);
                    if (IsInAttackRange(playerpos))
                    {
                        if (_normalCoolTime <= 0f)
                            return DoNormalAttack();
                    }
                    if (IsInPatternRange(playerpos))
                    {
                        if (_firstCoolTime <= 0f)
                            return DoFirstPattern();
                        if (_secondCoolTime <= 0f && _firstCoolTime > 0f)
                            return DoSecondPattern();
                    }
                    if (_patternCount >= 4f)
                        return DoThirdPattern();
            }
        }
        return INode.ENodeState.ENS_Failure;
    }


    public INode.ENodeState DoNormalAttack()
    {
        if (CanAttack())
        {
            Attack();
            _normalCoolTime = 2f;
            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }

    public INode.ENodeState DoFirstPattern()
    {
        if (CanAttack())
        {
            FirstPatternAttack();
            _firstCoolTime = 8f;
            if (_isAttacking)
            {
                Rotate();
                return INode.ENodeState.ENS_Running;
            }
        }
        return INode.ENodeState.ENS_Failure;
    }

    public INode.ENodeState DoSecondPattern()
    {
        if (CanAttack())
        {
            SecondPatternAttack();
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
            _patternCount = 0;
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
            float playerpos = Vector3.SqrMagnitude(_detectedPlayer.position - transform.position);
            if (!_isAttacking)
            {
                if (!IsLookingAtPlayer() && IsInPatternRange(playerpos))
                {
                    Rotate();
                    return INode.ENodeState.ENS_Running;
                }
                if(!IsInPatternRange(playerpos))
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
    }

    public void HeadAttack()
    {
        _head.SetActive(true);
    }

    public void HeadAttackEnd()
    {
        _head.SetActive(false);
    }

    public void IdleSound()
    {
        _audioSource.clip = _idleSound;
        _audioSource.Play();
    }
    public void MeteorStart()
    {
        _audioSource.clip = _meteorSound;
        _audioSource.Play();
    }
    public void GroundStart()
    {
        _audioSource.clip = _groundSound;
        _audioSource.Play();
    }
}

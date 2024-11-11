using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WormBoss : Boss
{
    BehaviorTreeRunner _BTRunner = null;
    const string _Idle_AnimStateName = "Idle";
    const string _Taunting_AnimStateName = "Taunting";

    AudioSource _audioSource;
    public AudioClip _normalSound;
    public AudioClip _groundSound;
    public AudioClip _tauntSound;

    public GameObject _head;
    public GameObject _groundParticle;

    private void Start()
    {
        _attackRange = 10f;
        __attackRange = _attackRange * _attackRange;
        _movementSpeed = 0f;
        _isDead = false;
        _isAttacking = false;
        _hp = 9000f;
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
        if (IsAniamtionRunning(_Idle_AnimStateName) || IsAniamtionRunning(_Taunting_AnimStateName))
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
        if (CanAttack())
        {
            if(CheckPlayerWithinCoolTime() == INode.ENodeState.ENS_Success)
            {
                float playerpos = Vector3.SqrMagnitude(_detectedPlayer.position - transform.position);
                if(IsInAttackRange(playerpos))
                {
                    if (_firstCoolTime <= 0f)
                        return DoFirstPattern();
                }
                else
                {
                    if (_normalCoolTime <= 0f)
                        return DoNormalAttack();
                    if (_secondCoolTime <= 0f && PlayerToBossDistance())
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
            _isAttacking = true;
            _firstCoolTime = 2f;
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
                    return INode.ENodeState.ENS_Running;
                }
            }
            return INode.ENodeState.ENS_Success;
        }
        return INode.ENodeState.ENS_Failure;
    }
    #endregion


    public void HeadAttack()
    {
        _head.SetActive(true);
    }

    public void HeadAttackEnd()
    {
        _head.SetActive(false);
    }

    public void NormalStart()
    {
        _audioSource.clip = _normalSound;
        _audioSource.Play();
    }

    public void GroundStart()
    {
        _audioSource.clip = _groundSound;
        _audioSource.Play();
        _groundParticle.SetActive(true);
    }

    public void TauntStart()
    {
        _audioSource.clip = _tauntSound;
        _audioSource.Play();
    }


    protected void FirstCooldown(float deltaTime)
    {
        if (_firstCoolTime > 0)
        {
            _firstCoolTime -= deltaTime; // 쿨타임 감소
        }
    }

    protected void SecondCooldown(float deltaTime)
    {
        if (_secondCoolTime > 0)
        {
            _secondCoolTime -= deltaTime; // 쿨타임 감소
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Enemy : MonoBehaviour, IDamageAble<float>
{
    [Header("Range")]
    [SerializeField]
    float _detectRange = 10f;
    [SerializeField]
    float _meleeAttackRange = 5f;

    [SerializeField]
    float _movementSpeed = 10f;

    Rigidbody rigid;
    EnemyAttack enemyAttack;
    BehaviorTreeRunner _BTRunner = null;
    Transform _detectedPlayer = null;
    Vector3 _originPos;
    Animator animator;
    float hp = 50;

    const string _ATTACK_ANIM_STATE_NAME = "attack01";
    const string _ATTACK_ANIM_TRIGGER_NAME = "attack";
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        enemyAttack = FindAnyObjectByType<EnemyAttack>();
        animator = GetComponent<Animator>();
        _BTRunner = new BehaviorTreeRunner(SettingBT());
        _originPos = transform.position;
    }

    private void Update()
    {
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
                            new ActionNode(CheckEnemyWithinMeleeAttackRange),
                            new ActionNode(DoMeleeAttack),
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
        if (IsAniamtionRunning(_ATTACK_ANIM_STATE_NAME))
        {
            return INode.ENodeState.ENS_Running;
        }
        return INode.ENodeState.ENS_Success;
    }

    INode.ENodeState CheckEnemyWithinMeleeAttackRange()
    {
        if(_detectedPlayer != null)
        {
            if(Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) 
                < (_meleeAttackRange * _meleeAttackRange))
            {
                return INode.ENodeState.ENS_Success;
            }
        }
        return INode.ENodeState.ENS_Failure;
    }

    INode.ENodeState DoMeleeAttack()
    {
        if (_detectedPlayer != null)
        {
            animator.SetTrigger(_ATTACK_ANIM_TRIGGER_NAME);
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
            animator.SetFloat("moveSpeed", 1);
            return INode.ENodeState.ENS_Success;
        }

        _detectedPlayer = null;
        animator.SetFloat("moveSpeed", 0);
        return INode.ENodeState.ENS_Failure;
    }

    INode.ENodeState MoveToDetectEnemy()
    {
        if (_detectedPlayer != null)
        {
            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_meleeAttackRange * _meleeAttackRange))
            {
                return INode.ENodeState.ENS_Success;
            }
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
        Gizmos.DrawWireSphere(this.transform.position, _meleeAttackRange);
    }

    public void Attack(object sender, EventArgs e)
    {
        
    }

    public void Damage(float damageTaken)
    {
        animator.SetTrigger("hit");
        hp -= damageTaken;
    }

    public void Dead()
    {

    }

    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _detectedPlayer.position, Time.deltaTime * _movementSpeed);        
        transform.LookAt(_detectedPlayer);
    }

    public void AttackStateCollider()
    {
        enemyAttack.gameObject.GetComponent<MeshCollider>().enabled =
            !enemyAttack.gameObject.GetComponent<MeshCollider>().enabled;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageAble<float> damageAble = collision.gameObject.GetComponent<IDamageAble<float>>();
        if(damageAble != null)
        {
            damageAble.Damage(20);
        }
    }

    public void Shot()
    {
        throw new NotImplementedException();
    }
}

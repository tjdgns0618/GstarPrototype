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


public abstract class Boss : MonoBehaviour, IDamageAble<float>
{
    public BossType _bossType;

    protected float _attackRange;
    protected float __attackRange;
    protected float _patternRange;
    protected float __patternRange;

    protected bool _isDead;
    protected bool _isAttacking;
    protected float _hp;
    protected float _distanceT;
    protected float _movementSpeed;
    protected float _rotationSpeed = 4f;

    protected float _normalCoolTime;
    protected float _firstCoolTime;
    protected float _secondCoolTime;

    public Transform _shotpos;

    public Transform _detectedPlayer = null;
    Rigidbody _rigid;

    Animator animator;
    spawner1 spawner;

    const string _NormalAttack_AnimTriggerName = "isNormal";
    const string _FirstPatternAttack_AnimTriggerName = "isFirst";
    const string _SecondPatternAttack_AnimTriggerName = "isSecond";
    const string _ThirdPatternAttack_AnimTriggerName = "isThird";
    const string _Dead_AnimTriggerName = "bossDead";
    const string _WormBurrow_AnimTriggerName = "wormBurrow";

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    public virtual INode SettingBT()
    {
        return null;
    }

    

    public bool IsAniamtionRunning(string stateName)
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


    public void Rotate()
    {
        Vector3 direction = (_detectedPlayer.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
    }

    public bool IsLookingAtPlayer()
    {
        Vector3 directionToPlayer = (_detectedPlayer.position - transform.position).normalized;
        Vector3 forward = transform.forward;

        return Vector3.Dot(forward, directionToPlayer) > 0.99f;
    }

    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _detectedPlayer.position, Time.deltaTime * _movementSpeed);
    }


    public void Attack()
    {
        if(_bossType == BossType.Worm)
        {
            Shot();
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

    public void Fire()
    {
        GameObject particle = GameManager.instance.particlePoolManager.GetParticle("Poison");
        if (particle != null)
        {
            particle.transform.position = _shotpos.transform.position;
            particle.transform.rotation = transform.rotation;
        }
    }

    public void FirstPatternAttack()
    {
        animator.SetTrigger(_FirstPatternAttack_AnimTriggerName);
    }

    public void SecondPatternAttack()
    {
        if (_bossType == BossType.Worm)
        {
            animator.SetTrigger(_SecondPatternAttack_AnimTriggerName);
        }
        if (_bossType == BossType.Knight)
        {

        }
        if (_bossType == BossType.Bishop)
        {

        }
        if (_bossType == BossType.Dragon)
        {

        }
    }

    public void ThirdPatternAttack()
    {
        if (_bossType == BossType.Dragon)
        {

        }
    }

    public void Damage(float damage)
    {

    }

    public void Shot()
    {
        animator.SetTrigger(_NormalAttack_AnimTriggerName);
    }

    public void Dead()
    {
        _isDead = true;
        animator.SetTrigger(_Dead_AnimTriggerName);
    }

    public void UnBorrow()
    {
        Vector3 groundPos = _detectedPlayer.transform.position;
        transform.position = groundPos;
        animator.SetTrigger(_WormBurrow_AnimTriggerName);
    }

    public bool IsInAttackRange(float distance)
    {
        return distance < (__attackRange);
    }

    public bool IsInPatternRange(float distance)
    {
        return distance < (__patternRange);
    }

    public bool CanAttack()
    {
        return _detectedPlayer != null && !_isDead;
    }


    public bool PlayerToBossDistance()
    {
        float playerpos = Vector3.SqrMagnitude(_detectedPlayer.position - transform.position);
        if (CanAttack() && !IsInAttackRange(playerpos))
        {
            _distanceT += Time.deltaTime;
        }
        if (CanAttack() && IsInAttackRange(playerpos))
        {
            _distanceT = 0f;
        }
        if(_distanceT >= 8f)
        {
            _distanceT = 0f;
            return true;
        }
        return false;
    }


    protected void NormalCooldown(float deltaTime)
    {
        if (_normalCoolTime > 0)
        {
            _normalCoolTime -= deltaTime; // ÄðÅ¸ÀÓ °¨¼Ò
        }
    }

    protected void FirstCooldown(float deltaTime)
    {
        if (_firstCoolTime > 0)
        {
            _firstCoolTime -= deltaTime; // ÄðÅ¸ÀÓ °¨¼Ò
        }
    }

    protected void SecondCooldown(float deltaTime)
    {
        if (_secondCoolTime > 0)
        {
            _secondCoolTime -= deltaTime; // ÄðÅ¸ÀÓ °¨¼Ò
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, _attackRange);
    }
    public abstract INode.ENodeState EvaluatePatterns();
}

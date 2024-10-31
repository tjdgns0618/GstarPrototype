using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

    Vector3 _rndPosition;

    Animator animator;
    spawner1 spawner;
    DamageTextManager damagetextManager;

    public const string _NormalAttack_AnimTriggerName = "isNormal";
    public const string _FirstPatternAttack_AnimTriggerName = "isFirst";
    public const string _SecondPatternAttack_AnimTriggerName = "isSecond";
    public const string _ThirdPatternAttack_AnimTriggerName = "isThird";
    public const string _Dead_AnimTriggerName = "bossDead";
    public const string _WormBurrow_AnimTriggerName = "wormBurrow";

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        damagetextManager = FindAnyObjectByType<DamageTextManager>();
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
        Shot();
    }

    public void Fire()
    {
        if(_bossType == BossType.Worm)
        {
            GameObject particle = GameManager.instance.particlePoolManager.GetParticle("Poison");
            if (particle != null)
            {
                particle.transform.position = _shotpos.transform.position;
                particle.transform.rotation = transform.rotation;
            }
        }
        if (_bossType == BossType.Dragon)
        {
            GameObject particle = GameManager.instance.particlePoolManager.GetParticle("Meteor");
            if (particle != null)
            {
                particle.transform.position = _detectedPlayer.transform.position;

                for(int i = 0; i < 30; i++)
                {
                    RandomNumber();
                    GameObject particle2 = GameManager.instance.particlePoolManager.GetParticle("Meteor2");
                    particle2.transform.position = _rndPosition;
                }
            }
        }
    }

    public void FirstPatternAttack()
    {
        animator.SetTrigger(_FirstPatternAttack_AnimTriggerName);
        if (_bossType == BossType.Dragon)
        {

        }
    }

    public void SecondPatternAttack()
    {
        animator.SetTrigger(_SecondPatternAttack_AnimTriggerName);
    }

    public void ThirdPatternAttack()
    {
        animator.SetTrigger(_ThirdPatternAttack_AnimTriggerName);
    }

    public void Damage(float damage)
    {
        if (_isDead) return;

        animator.SetTrigger("hit");
        // hitSound.Play();
        _hp -= damage;


        Vector3 textPos = transform.position;
        textPos.y += 1.5f;
        damagetextManager.GetDamageTextObject().GetComponent<DamageText>().Init(damage, textPos, false);

        Debug.Log(_hp);
        if (_hp <= 0)
        {
            _hp = 0;
            Dead();
        }
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
        return distance <= (__attackRange);
    }

    public bool IsInPatternRange(float distance)
    {
        return distance <= (__patternRange);
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

    public Vector3 RandomNumber()
    {
        float rndX = Random.Range(-9f, 9f);
        float rndZ = Random.Range(-9f, 9f);

        _rndPosition = new Vector3(_detectedPlayer.transform.position.x + rndX, _detectedPlayer.transform.position.y, _detectedPlayer.transform.position.x + rndZ);
        return _rndPosition;
    }

    private IEnumerator SpawnParticles()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject particle = GameManager.instance.particlePoolManager.GetParticle("CrashFire");
            if (particle != null)
            {
                float rndY = Random.Range(0f, 180f);
                particle.transform.position = _detectedPlayer.transform.position;
                particle.transform.rotation = Quaternion.Euler(0, rndY, 0);
            }

            yield return new WaitForSeconds(0.5f); // Wait for 1 second before the next spawn
        }
    }

    public void DragonGroundPattern()
    {
        StartCoroutine(SpawnParticles());
    }

    public void PatternStart()
    {
        _isAttacking = true;
    }
    public void PatternEnd()
    {
        _isAttacking = false;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, _attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _patternRange);
    }
    public abstract INode.ENodeState EvaluatePatterns();

    public void PlayKnockback(Vector3 direction, float knockbackPower, float knockbackDuration)
    {

    }
}

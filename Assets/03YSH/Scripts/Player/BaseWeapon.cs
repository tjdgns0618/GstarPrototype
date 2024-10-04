using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public abstract class BaseWeapon : MonoBehaviour
{
    public int ComboCount { get; set; }
    public PlayerAnimationEvents playerAnimationEvents;
    public RuntimeAnimatorController WeaponAnimator { get { return weaponAnimator; } }
    public float AttackDamage { get { return attackDamage; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float AttackRange { get { return attackRange; } }

    [Header("무기 정보")]
    [SerializeField] protected RuntimeAnimatorController weaponAnimator;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;

    public delegate void Action();
    public Action ItemChance;

    public void SetWeaponData(float attackDamage, float attackSpeed, float attackRange)
    {
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed; 
        this.attackRange = attackRange;
    }


    public abstract void Attack(BaseState state);
    public abstract void Skill(BaseState state);
    public abstract void Skill2(BaseState state);
    public abstract void UltimateSkill(BaseState state);
}

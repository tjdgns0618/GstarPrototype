using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public abstract class BaseWeapon : MonoBehaviour
{
    public int ComboCount { get; set; }
    public PlayerAnimationEvents playerAnimationEvents;
    public RuntimeAnimatorController WeaponAnimator { get { return weaponAnimator; } }

    public string Name { get { return _name; } }
    public float AttackDamage { get { return attackDamage; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float AttackRange { get { return attackRange; } }

    [Header("무기 정보")]
    [SerializeField] protected RuntimeAnimatorController weaponAnimator;
    [SerializeField] protected string _name;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;

    public delegate void Action();
    public Action ItemChance;

    public void SetWeaponData(string name, float attackDamage, float attackSpeed, float attackRange)
    {
        this._name = name;
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed; 
        this.attackRange = attackRange;
    }

    public abstract void Attack(BaseState state);
    public abstract void Skill(BaseState state);
    public abstract void UltimateSkill(BaseState state);
}

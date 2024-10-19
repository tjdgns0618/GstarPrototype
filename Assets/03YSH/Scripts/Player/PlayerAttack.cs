using CharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.Collections.LowLevel.Unsafe;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.ParticleSystem;

public class PlayerAttack : BaseWeapon, IEffect
{
    public readonly int hashIsAttackAnimation = Animator.StringToHash("IsAttack");
    public readonly int hashIsSkill_Q_Animation = Animator.StringToHash("IsSkill_Q");
    public readonly int hashIsSkill_E_Animation = Animator.StringToHash("IsSkill_E");
    public readonly int hashIsSkill_R_Animation = Animator.StringToHash("IsSkill_R");
    public readonly int hashAttackAnimation = Animator.StringToHash("AttackCombo");
    public readonly int hashAttackSpeedAnimation = Animator.StringToHash("AttackSpeed");
    public readonly string hashWarriorAttackEffect = "WarriorAttackParticle";
    public readonly string hashArcherAttackEffect = "ArcherProjectile";
    public readonly string hashWizardAttackEffect = "WizardProjectile";
    private Coroutine checkAttackReInputCor;
    public GameObject[] SkillEffs;
    public PlayerCharacter pi;
    private GameManager gi;
    private ParticlePoolManager pm;

    public int skillType;

    [Header("¹ß»çÃ¼")]
    public GameObject[] projectiles;

    [SerializeField]
    GameObject effectGenerator;

    private void Start()
    {
        pi = PlayerCharacter.Instance;
        gi = GameManager.instance;
        pm = gi.particlePoolManager;
        SetWeaponData(gi._damage, gi._attackspeed,gi._range);
    }

    public override void Attack(BaseState state)
    {
        ComboCount++;
        AttackState.comboCount = ComboCount;
        pi.animator.SetFloat(hashAttackSpeedAnimation, AttackSpeed);
        pi.animator.SetBool(hashIsAttackAnimation, true);
        pi.animator.SetInteger(hashAttackAnimation, ComboCount);
        gi.activeDelegate();
        CheckAttackReInput(AttackState.CanReInputTime);
    }

    public override void Skill(BaseState state)
    {
        PlayerCharacter.Instance.isPlaySkill = true;
        Debug.Log("Q");
        skillType = 0;
        pi.animator.SetFloat(hashAttackSpeedAnimation, AttackSpeed);
        pi.animator.SetBool(hashIsSkill_Q_Animation, true);
    }

    public override void Skill2(BaseState state)
    {
        PlayerCharacter.Instance.isPlaySkill = true;
        Debug.Log("E");
        skillType = 3;
        pi.animator.SetFloat(hashAttackSpeedAnimation, AttackSpeed);
        pi.animator.SetBool(hashIsSkill_E_Animation, true);
    }

    public override void UltimateSkill(BaseState state)
    {
        Debug.Log("R");
        PlayerCharacter.Instance.isPlaySkill = true;
        skillType = 6;
        pi.animator.SetFloat(hashAttackSpeedAnimation, AttackSpeed);
        pi.animator.SetBool(hashIsSkill_R_Animation, true);
    }

    public void CheckAttackReInput(float reInputTime)
    {
        if (checkAttackReInputCor != null)
            StopCoroutine(checkAttackReInputCor);
        checkAttackReInputCor = StartCoroutine(checkAttackReInputCoroutine(reInputTime));
    }

    private IEnumerator checkAttackReInputCoroutine(float reInputTime)
    {
        float currentTime = 0f;
        while (true)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= reInputTime)
            {
                ComboCount = 0;
                AttackState.comboCount = 0;
                pi.animator.SetInteger(hashAttackAnimation, 0);
                break;
            }
            yield return null;
        }        
    }

    public void PlayComboAttackEffects()
    {
        int comboCount = Mathf.Clamp(ComboCount - 1, 0, 3);        
        if (pi.characterClass == CharacterType.Warrior)
        {
            GameObject effect = pm.GetParticle(hashWarriorAttackEffect + ComboCount);
            if (effect != null)
            {
                effect.transform.position = pi.transform.position + (Vector3.up * 1f);
                effect.transform.rotation = pi.transform.rotation;
                if (ComboCount == 3)
                {
                    effect.transform.position += (transform.forward * 1f + Vector3.up * 1f);
                    effect.transform.Rotate(new Vector3(0f, 250f, 60f));
                }
                else
                    effect.transform.Rotate(new Vector3(0f, 200f, 0f));
            }
        }
        else if(pi.characterClass == CharacterType.Archer)
        {
            GameObject arrow = pm.GetParticle(hashArcherAttackEffect + ComboCount);
            if (arrow != null)
            {
                arrow.transform.position = pi.firePoint.transform.position;
                arrow.transform.rotation = pi.transform.rotation;
            }
        }
        else if(pi.characterClass == CharacterType.Wizard)
        {
            GameObject magic = pm.GetParticle(hashWizardAttackEffect + ComboCount);
            if (magic != null)
            {
                magic.transform.position = pi.firePoint.transform.position;
                magic.transform.rotation = pi.transform.rotation;
            }
        }
    }

    public void PlaySkillEffect()
    {
        GameObject effect = Instantiate(SkillEffs[skillType + ((int)pi.characterClass)], pi.firePoint.transform.position, pi.transform.rotation);

    }

    
}

using CharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.Collections.LowLevel.Unsafe;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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

    [Header("발사체")]
    public GameObject[] projectiles;

    [SerializeField]
    GameObject effectGenerator;

    private void Start()
    {
        pi = PlayerCharacter.Instance;
        gi = GameManager.instance;
        pm = gi.particlePoolManager;
        SetWeaponData(gi._damage, gi._attackspeed, gi._range);
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
        skillType = 4;
        pi.animator.SetFloat(hashAttackSpeedAnimation, AttackSpeed);
        pi.animator.SetBool(hashIsSkill_Q_Animation, true);
    }

    public override void Skill2(BaseState state)
    {
        PlayerCharacter.Instance.isPlaySkill = true;
        Debug.Log("E");
        skillType = 5;
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
                    effect.transform.position += (transform.forward * 1.5f);
                    effect.transform.Rotate(new Vector3(0f, 250f, 60f));
                }
                else
                    effect.transform.Rotate(new Vector3(0f, 200f, 0f));
            }
        }
        else if (pi.characterClass == CharacterType.Archer)
        {
            GameObject arrow = pm.GetParticle(hashArcherAttackEffect + ComboCount);
            if (arrow != null)
            {
                arrow.transform.position = pi.firePoint.transform.position;
                arrow.transform.rotation = pi.transform.rotation;
            }
        }
        else if (pi.characterClass == CharacterType.Wizard)
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
        // GameObject effect = Instantiate(SkillEffs[skillType + ((int)pi.characterClass)], pi.firePoint.transform.position, pi.transform.rotation);
        GameObject effect;
        if (pi.characterClass == CharacterType.Warrior)
        {
            effect = gi.particlePoolManager.GetParticle(hashWarriorAttackEffect + skillType);
            effect.transform.rotation = pi.transform.rotation;
            effect.transform.position = pi.firePoint.transform.position;
            if (skillType == 5)
                effect.transform.position -= (Vector3.up * 1f) + (pi.transform.forward * -3f);
        }
        if (pi.characterClass == CharacterType.Archer)
        {
            if (skillType == 4)
            {
                effect = gi.particlePoolManager.GetParticle(hashArcherAttackEffect + skillType);
                effect.transform.position = pi.firePoint.transform.position;
                effect.transform.rotation = pi.transform.rotation;
            }
            else if (skillType == 5)
            {
                for (int i = -1; i < 2; i++)
                {
                    // 파티클을 가져옵니다.
                    effect = gi.particlePoolManager.GetParticle(hashArcherAttackEffect + skillType);

                    if (effect != null)
                    {
                        // 발사체의 회전을 설정합니다.
                        Quaternion rotation = pi.transform.rotation * Quaternion.Euler(0, 30 * i, 0);

                        // 발사체의 위치를 설정합니다.
                        effect.transform.position = pi.firePoint.transform.position;
                        effect.transform.rotation = rotation;

                        // 활성화, 비활성화 시간체크
                        // StartCoroutine(DebugObjectLifetime(effect));
                    }
                }
            }
            else if (skillType == 6)
            {
                for (int i = 0; i < 3; i++)
                {
                    effect = gi.particlePoolManager.GetParticle(hashArcherAttackEffect + skillType);
                    effect.transform.position = pi.firePoint.transform.position;
                    effect.transform.rotation = pi.transform.rotation;
                    effect.transform.position -= (Vector3.up * 1f);
                    effect.transform.position += pi.transform.forward * 3f * i;
                }
            }

        }
        if (pi.characterClass == CharacterType.Wizard)
        {
            if (skillType == 4)
            {
                effect = gi.particlePoolManager.GetParticle(hashWizardAttackEffect + skillType);
                effect.transform.position = pi.transform.position;
            }
            else if (skillType == 5)
            {
                effect = gi.particlePoolManager.GetParticle(hashWizardAttackEffect + skillType);
                effect.transform.position = pi.firePoint.transform.position;
                effect.transform.rotation = pi.transform.rotation;
            }
            else if (skillType == 6)
            {
                effect = gi.particlePoolManager.GetParticle(hashWizardAttackEffect + skillType);
                effect.transform.position = pi.firePoint.transform.position;
                effect.transform.rotation = pi.transform.rotation;
                // GameObject effect2 = gi.particlePoolManager.GetParticle(hashWizardAttackEffect + (skillType + 1));
                // effect2.transform.position = pi.transform.position + Vector3.up * 2f;
                // effect2.transform.rotation = pi.transform.rotation;
            }
        }
    }
    private IEnumerator DebugObjectLifetime(GameObject effect)
    {
        float startTime = Time.time;
        Debug.Log($"Object {effect.name} created at {startTime}");

        // 오브젝트가 비활성화될 때까지 대기합니다.
        while (effect.activeSelf)
        {
            yield return null;
        }

        float endTime = Time.time;
        Debug.Log($"Object {effect.name} destroyed at {endTime}");
        Debug.Log($"Object {effect.name} lifetime: {endTime - startTime} seconds");
    }
}

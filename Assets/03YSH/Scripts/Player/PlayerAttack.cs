using CharacterController;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.Collections.LowLevel.Unsafe;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : BaseWeapon, IEffect
{
    public readonly int hashIsAttackAnimation = Animator.StringToHash("IsAttack");
    public readonly int hashAttackAnimation = Animator.StringToHash("AttackCombo");
    public readonly int hashAttackSpeedAnimation = Animator.StringToHash("AttackSpeed");
    private Coroutine checkAttackReInputCor;
    public GameObject[] defaultAttackEffs;
    public Vector3 adjustTransform;

    [SerializeField]
    GameObject effectGenerator;

    public override void Attack(BaseState state)
    {
        ComboCount++;
        PlayerCharacter.Instance.animator.SetFloat(hashAttackSpeedAnimation, AttackSpeed);
        PlayerCharacter.Instance.animator.SetBool(hashIsAttackAnimation, true);
        PlayerCharacter.Instance.animator.SetInteger(hashAttackAnimation, ComboCount);
        CheckAttackReInput(AttackState.CanReInputTime);
    }

    public override void Skill(BaseState state)
    {
        throw new System.NotImplementedException();
    }

    public override void UltimateSkill(BaseState state)
    {
        throw new System.NotImplementedException();
    }

    public void CheckAttackReInput(float reInputTime)
    {
        if(checkAttackReInputCor != null) 
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
                break;
            yield return null;
        }

        ComboCount = 0;
        PlayerCharacter.Instance.animator.SetInteger(hashAttackAnimation, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageAble<float> damageAble = other.GetComponent<IDamageAble<float>>();
        damageAble?.Damage(attackDamage);
    }

    public void PlayComboAttackEffects()
    {
        int comboCount = Mathf.Clamp(ComboCount - 1, 0, 3);
        GameObject effect = Instantiate(defaultAttackEffs[comboCount]);
        Vector3 targetDirection = PlayerCharacter.Instance.targetDirection;

        effect.transform.position = PlayerCharacter.Instance.effectGenerator.transform.position +
                                    adjustTransform;

        // Vector3 secondAttackAdjustAngle = ComboCount == 2 ? new Vector3(0, -90f, 0f) : Vector3.zero;
        effect.transform.rotation = Quaternion.Euler(targetDirection);
        // effect.transform.eulerAngles += secondAttackAdjustAngle;
        effect.GetComponent<ParticleSystem>().Play();
    }

    public void DestroyEffect()
    {
        throw new System.NotImplementedException();
    }

    public void PlaySkillEffect()
    {
        throw new System.NotImplementedException();
    }
}

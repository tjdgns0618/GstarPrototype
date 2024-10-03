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
    public readonly int hashIsSkill_Q_Animation = Animator.StringToHash("IsSkill_Q");
    public readonly int hashIsSkill_E_Animation = Animator.StringToHash("IsSkill_E");
    public readonly int hashIsSkill_R_Animation = Animator.StringToHash("IsSkill_R");
    public readonly int hashAttackAnimation = Animator.StringToHash("AttackCombo");
    public readonly int hashAttackSpeedAnimation = Animator.StringToHash("AttackSpeed");
    private Coroutine checkAttackReInputCor;
    public GameObject[] defaultAttackEffs;
    public GameObject[] SkillEffs;

    public int skillType;

    [Header("발사체")]
    public GameObject[] projectiles;

    public Vector3 adjustTransform;

    [SerializeField]
    GameObject effectGenerator;

    public override void Attack(BaseState state)
    {
        ComboCount++;
        AttackState.comboCount = ComboCount;
        PlayerCharacter.Instance.animator.SetFloat(hashAttackSpeedAnimation, AttackSpeed);
        PlayerCharacter.Instance.animator.SetBool(hashIsAttackAnimation, true);
        PlayerCharacter.Instance.animator.SetInteger(hashAttackAnimation, ComboCount);
        CheckAttackReInput(AttackState.CanReInputTime);
        if (PlayerCharacter.Instance.characterClass != CharacterType.Warrior)
        {
            Debug.Log(projectiles[0].name);
            GameObject arrow = Instantiate(projectiles[ComboCount - 1], PlayerCharacter.Instance.firePoint.transform.position,
                PlayerCharacter.Instance.transform.rotation);
        }
    }

    public override void Skill(BaseState state)
    {
        Debug.Log("Q");
        skillType = 0;
        PlayerCharacter.Instance.animator.SetFloat(hashAttackSpeedAnimation, AttackSpeed);
        PlayerCharacter.Instance.animator.SetBool(hashIsSkill_Q_Animation, true);
    }

    public override void Skill2(BaseState state)
    {
        Debug.Log("E");
        skillType = 1;
        PlayerCharacter.Instance.animator.SetFloat(hashAttackSpeedAnimation, AttackSpeed);
        PlayerCharacter.Instance.animator.SetBool(hashIsSkill_E_Animation, true);
    }

    public override void UltimateSkill(BaseState state)
    {
        Debug.Log("R");
        skillType = 2;
        PlayerCharacter.Instance.animator.SetFloat(hashAttackSpeedAnimation, AttackSpeed);
        PlayerCharacter.Instance.animator.SetBool(hashIsSkill_R_Animation, true);
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
                PlayerCharacter.Instance.animator.SetInteger(hashAttackAnimation, 0);
                break;
            }
            yield return null;
        }

        
    }

    public void PlayComboAttackEffects()
    {
        int comboCount = Mathf.Clamp(ComboCount - 1, 0, 3);
        GameObject effect = Instantiate(defaultAttackEffs[comboCount], Vector3.zero, PlayerCharacter.Instance.transform.rotation);
        // effect.transform.SetParent(effectGenerator.transform);

        effect.transform.position = PlayerCharacter.Instance.effectGenerator.transform.position + adjustTransform;
        // Vector3 secondAttackAdjustAngle = ComboCount == 2 ? new Vector3(0, -90f, 0f) : Vector3.zero;
        effect.transform.Rotate(new Vector3(0f, 150f, 0f));

        // effect.transform.eulerAngles += secondAttackAdjustAngle;
        // effect.GetComponent<ParticleSystem>().Play();
    }

    public void DestroyEffect()
    {
        throw new System.NotImplementedException();
    }

    public void PlaySkillEffect()
    {
        Debug.Log("파티클 생성");
        GameObject effect = Instantiate(SkillEffs[skillType], PlayerCharacter.Instance.firePoint.transform.position, PlayerCharacter.Instance.transform.rotation);


        // GroundSlash groundSlashScript = effect.GetComponent<GroundSlash>();
        // effect.GetComponent<Rigidbody>().velocity = PlayerCharacter.Instance.transform.forward * groundSlashScript.speed;
    }

    
}

using CharacterController;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.Collections.LowLevel.Unsafe;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

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
    public PlayerCharacter pi;

    public int skillType;

    [Header("발사체")]
    public GameObject[] projectiles;

    public Vector3 adjustTransform;

    [SerializeField]
    GameObject effectGenerator;

    private void Start()
    {
        pi = PlayerCharacter.Instance;
    }

    public override void Attack(BaseState state)
    {
        ComboCount++;
        AttackState.comboCount = ComboCount;
        pi.animator.SetFloat(hashAttackSpeedAnimation, AttackSpeed);
        pi.animator.SetBool(hashIsAttackAnimation, true);
        pi.animator.SetInteger(hashAttackAnimation, ComboCount);
        CheckAttackReInput(AttackState.CanReInputTime);
        
    }

    public override void Skill(BaseState state)
    {
        Debug.Log("Q");
        skillType = 0;
        pi.animator.SetFloat(hashAttackSpeedAnimation, AttackSpeed);
        pi.animator.SetBool(hashIsSkill_Q_Animation, true);
    }

    public override void Skill2(BaseState state)
    {
        Debug.Log("E");
        skillType = 3;
        pi.animator.SetFloat(hashAttackSpeedAnimation, AttackSpeed);
        pi.animator.SetBool(hashIsSkill_E_Animation, true);
    }

    public override void UltimateSkill(BaseState state)
    {
        Debug.Log("R");
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
        GameObject effect = null;
        if (pi.characterClass == CharacterType.Warrior)
        {
            effect = Instantiate(defaultAttackEffs[comboCount], Vector3.zero, pi.transform.rotation);
        }
        else if(pi.characterClass == CharacterType.Archer)
        {
            effect = Instantiate(defaultAttackEffs[comboCount + 3], Vector3.zero, pi.transform.rotation);
            GameObject arrow = Instantiate(projectiles[ComboCount - 1], pi.firePoint.transform.position,
                pi.transform.rotation);
        }
        else if(pi.characterClass == CharacterType.Wizard)
        {
            effect = Instantiate(defaultAttackEffs[comboCount + 3], Vector3.zero, pi.transform.rotation);
            GameObject magic = Instantiate(projectiles[ComboCount + 2], pi.firePoint.transform.position,
                pi.transform.rotation);
            // magic.transform.Rotate(0, -90f, 0);
        }

        // effect.transform.SetParent(effectGenerator.transform);

        effect.transform.position = pi.effectGenerator.transform.position + adjustTransform;
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
        GameObject effect = Instantiate(SkillEffs[skillType + ((int)pi.characterClass)], pi.firePoint.transform.position, pi.transform.rotation);


        // GroundSlash groundSlashScript = effect.GetComponent<GroundSlash>();
        // effect.GetComponent<Rigidbody>().velocity = PlayerCharacter.Instance.transform.forward * groundSlashScript.speed;
    }

    
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CooltimeManager : MonoBehaviour
{
    public Dictionary<string, float> skillCoolDowns = new Dictionary<string, float>{
            {"WarriorQ", 0f},
            {"WarriorE", 0f},
            {"WarriorR", 0f},
            {"ArcherQ",  0f},
            {"ArcherE",  0f},
            {"ArcherR",  0f},
            {"WizardQ",  0f},
            {"WizardE",  0f},
            {"WizardR",  0f}
    };
    public Dictionary<string, float> currentCoolDowns = new Dictionary<string, float>();
    public Dictionary<string, bool> canUseSkill = new Dictionary<string, bool>();

    public PlayerCharacter playerCharacter;
    public DOTweenAnimation ultReadyAnim;
    public GameObject[] ultReadyImages;

    List<string> skillKeys;

    private void Awake()
    {
        skillKeys = new List<string>(skillCoolDowns.Keys);

        SetCoolTimes();

        foreach (var skillName in skillKeys)
        {
            currentCoolDowns[skillName] = 0;
            canUseSkill[skillName] = true;
        }
    }

    void Update()
    {
        // 딕셔너리 순회하여 쿨타임 업데이트
        foreach (var skillName in skillKeys)
        {
            if (currentCoolDowns[skillName] > 0 && (skillName == "WarriorR" || skillName == "ArcherR" || skillName == "WizardR"))
            {
                currentCoolDowns[skillName] -= Time.deltaTime;
                if (currentCoolDowns[skillName] < 0)
                {
                    currentCoolDowns[skillName] = 0;  // 쿨타임이 0 이하로 내려가면 0으로 고정
                    canUseSkill[skillName] = true;
                    if (skillName == "WarriorR")
                    {
                        ultReadyImages[0].SetActive(true);
                        ultReadyImages[1].SetActive(false);
                        ultReadyImages[2].SetActive(false);
                    }
                    else if (skillName == "ArcherR")
                    {
                        ultReadyImages[0].SetActive(false);
                        ultReadyImages[1].SetActive(true);
                        ultReadyImages[2].SetActive(false);
                    }
                    else if (skillName == "WizardR")
                    {
                        ultReadyImages[0].SetActive(false);
                        ultReadyImages[1].SetActive(false);
                        ultReadyImages[2].SetActive(true);
                    }
                    ultReadyAnim.DORestart();
                }
            }
            else if (currentCoolDowns[skillName] > 0)
            {
                currentCoolDowns[skillName] -= Time.deltaTime;
                if (currentCoolDowns[skillName] < 0)
                {
                    currentCoolDowns[skillName] = 0;  // 쿨타임이 0 이하로 내려가면 0으로 고정
                    canUseSkill[skillName] = true;
                    
                    Debug.Log($"{skillName} 스킬이 쿨타임 종료됨");
                }
            }
        }

    }

    public void SetCoolTimes()
    {
        for (int i = 0; i < skillCoolDowns.Count; i++)
        {
            skillCoolDowns[skillKeys[i]] = GameManager.instance.cooltimes[i] * GameManager.instance._skillCooltimePercent;
        }
    }

    public void UseSkill(string characterType, string skillName)
    {
        if (skillCoolDowns.ContainsKey(skillName) && currentCoolDowns[skillName] <= 0)
        {
            // 스킬 사용 가능하면 쿨타임 시작
            currentCoolDowns[skillName] = skillCoolDowns[skillName] * GameManager.instance._skillCooltimePercent;
            canUseSkill[skillName] = false;
            Debug.Log($"{characterType}의 {skillName} 스킬 사용! 쿨타임: {skillCoolDowns[skillName] * GameManager.instance._skillCooltimePercent}초");
        }
        else
        {
            Debug.LogWarning($"{characterType}의 {skillName} 스킬은 사용할 수 없습니다 (쿨타임 중이거나 잘못된 입력)");
        }
        return;
    }

}
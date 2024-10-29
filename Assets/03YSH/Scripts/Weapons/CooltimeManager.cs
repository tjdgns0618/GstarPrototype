using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CooltimeManager : MonoBehaviour
{
    public Dictionary<string, float> skillCoolDowns = new Dictionary<string, float>{
            {"WarriorQ", 3.0f},
            {"WarriorE", 5.0f},
            {"WarriorR", 10.0f},
            {"ArcherQ", 4.0f},
            {"ArcherE", 7.0f},
            {"ArcherR", 15.0f},
            {"WizardQ", 6.0f},
            {"WizardE", 9.0f},
            {"WizardR", 20.0f}
    };
    public Dictionary<string, float> currentCoolDowns = new Dictionary<string, float>();
    public Dictionary<string, bool> canUseSkill = new Dictionary<string, bool>();

    public PlayerCharacter playerCharacter;

    List<string> skillKeys;

    private void Awake()
    {
        skillKeys = new List<string>(skillCoolDowns.Keys);

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
            // 현재 쿨타임이 0보다 클 경우 쿨타임을 감소시킴
            if (currentCoolDowns[skillName] > 0)
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

    public void UseSkill(string characterType, string skillName)
    {
        if (skillCoolDowns.ContainsKey(skillName) && currentCoolDowns[skillName] <= 0)
        {
            // 스킬 사용 가능하면 쿨타임 시작
            currentCoolDowns[skillName] = skillCoolDowns[skillName];
            canUseSkill[skillName] = false;
            Debug.Log($"{characterType}의 {skillName} 스킬 사용! 쿨타임: {skillCoolDowns[skillName]}초");
        }
        else
        {
            Debug.LogWarning($"{characterType}의 {skillName} 스킬은 사용할 수 없습니다 (쿨타임 중이거나 잘못된 입력)");
        }
        return;
    }

}
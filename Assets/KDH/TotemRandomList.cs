using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Rendering;
using UnityEngine;

public class TotemRandomList : MonoBehaviour    // 석상이 스폰될 때 가중치를 받은 확률에 의거해 석상 랜덤 스폰
{
    public Totem totem;

    public bool isPrevBloodTotem;   // 전 스테이지에서 희생 토템이 떴는지 확인하고 떴다면 확률을 그대로 유지, 안 떴다면 확률 보정

    public static int TotemWeightRandom(Dictionary<int, float> weightMap)
    {
        decimal total = 0;  // 
        int maxLength = 0;  // 

        foreach(var pair in weightMap)
        {
            float weight = pair.Value;
            int length = weight.ToString().Substring(weight.ToString().IndexOf('.') + 1).Length;
            total += (decimal)weight;
            if (maxLength < length)
                maxLength = length;
        }
        int correction = (int)(total * (decimal)Mathf.Pow(10, maxLength));

        int randomN = Random.Range(1, correction + 1);
        int tempNum = 0;
        int num = 0;

        foreach (var pair in weightMap)
        {
            num = (int)(correction * (decimal)pair.Value);
            if (num + tempNum >= randomN)
            {
                return pair.Key;
            }
            tempNum += num;
        }
        return 0;
    }

    public void RandomSpawn() // 조건에 따른 토템 랜덤 스폰
    {
        Dictionary<int, float> weightMap = new Dictionary<int, float>();

        if (GameManager.instance._hp == 100)
        {
            weightMap[0] = totem._devilWeight;
            weightMap[1] = totem._angelWeight;

            int index = TotemWeightRandom(weightMap);
            Totem.RareTotemType rareTotemType = index == 0 ? Totem.RareTotemType.Devil : Totem.RareTotemType.Angel;
            GameObject totemObj = totem.GetRareTotemObject(rareTotemType);
            totemObj.SetActive(true);
            Debug.Log(totemObj);
        }
        else
        {
            weightMap[0] = totem._bloodWeight;
            weightMap[1] = totem._test1Weight;
            weightMap[2] = totem._test2Weight;


            if(!isPrevBloodTotem)   // 확률 보정
            {
                weightMap[1] += 0.1f;
                weightMap[2] += 0.1f;
            }
            int index = TotemWeightRandom(weightMap);
            Totem.TotemType totemType = index == 0 ? Totem.TotemType.Blood : (index == 1 ? Totem.TotemType.test1 : Totem.TotemType.test2);
            GameObject totemObj = totem.GetTotemObject(totemType);
            totemObj.SetActive(true);
            Debug.Log(totemObj);
            isPrevBloodTotem = (totemType == Totem.TotemType.Blood);
        }
        if(isPrevBloodTotem)    // 희생방이 떴을 경우 확률 복구
        {
            weightMap[1] -= 0.1f;
            weightMap[2] -= 0.1f;
        }
    }

    public void DeactiveAllTotems() // 스폰된 토템 제거
    {
        foreach (var rareTotemType in System.Enum.GetValues(typeof(Totem.RareTotemType)))
        {
            GameObject totemObj = totem.GetRareTotemObject((Totem.RareTotemType)rareTotemType);
            if (totemObj != null)
            {
                totemObj.SetActive(false);
            }
        }
        foreach (var totemType in System.Enum.GetValues(typeof(Totem.TotemType)))
        {
            GameObject totemObj = totem.GetTotemObject((Totem.TotemType)totemType);
            if (totemObj != null)
            {
                totemObj.SetActive(false);
            }
        }
    }
}
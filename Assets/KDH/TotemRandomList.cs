using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Rendering;
using UnityEngine;

public class TotemRandomList : MonoBehaviour    // ������ ������ �� ����ġ�� ���� Ȯ���� �ǰ��� ���� ���� ����
{
    public Totem totem;

    public bool isPrevBloodTotem;   // �� ������������ ��� ������ ������ Ȯ���ϰ� ���ٸ� Ȯ���� �״�� ����, �� ���ٸ� Ȯ�� ����

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

    public void RandomSpawn() // ���ǿ� ���� ���� ���� ����
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


            if(!isPrevBloodTotem)   // Ȯ�� ����
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
        if(isPrevBloodTotem)    // ������� ���� ��� Ȯ�� ����
        {
            weightMap[1] -= 0.1f;
            weightMap[2] -= 0.1f;
        }
    }

    public void DeactiveAllTotems() // ������ ���� ����
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
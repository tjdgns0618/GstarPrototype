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

        foreach(var pair in weightMap) // ����ġ ������ ��Ż ���
        {
            float weight = pair.Value;
            int length = weight.ToString().Substring(weight.ToString().IndexOf('.') + 1).Length;
            total += (decimal)weight;
            if (maxLength < length)
                maxLength = length;
        }
        int correction = (int)(total * (decimal)Mathf.Pow(10, maxLength));  // total�� ���������� ��ȯ�ϱ� ���� 10�� ��������

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

    public void RandomSpawn() // ���ǿ� ���� ���� ���� ���� (�������� ���� �� ȣ��)
    {
        Dictionary<int, float> weightMap = new Dictionary<int, float>();

        if (!GameManager.instance.isHit)
        {
            weightMap[0] = totem._devilWeight;
            weightMap[1] = totem._angelWeight;

            int index = TotemWeightRandom(weightMap);
            Totem.RareTotemType rareTotemType = index == 0 ? Totem.RareTotemType.DevilT : Totem.RareTotemType.AngelT;
            GameObject totemObj = totem.GetRareTotemObject(rareTotemType);
            totemObj.SetActive(true);
            Debug.Log(totemObj);
        }
        else
        {
            weightMap[0] = totem._bloodWeight;
            weightMap[1] = totem._healWeight;
            weightMap[2] = totem._diceWeight;


            if(!isPrevBloodTotem)   // Ȯ�� ����
            {
                weightMap[1] += 0.1f;
                weightMap[2] += 0.05f;
            }
            int index = TotemWeightRandom(weightMap);
            Totem.TotemType totemType = index == 0 ? Totem.TotemType.BloodT : (index == 1 ? Totem.TotemType.HealT : Totem.TotemType.DiceT);
            GameObject totemObj = totem.GetTotemObject(totemType);
            totemObj.SetActive(true);
            Debug.Log(totemObj);
            isPrevBloodTotem = (totemType == Totem.TotemType.BloodT);
        }
        if(isPrevBloodTotem)    // ������� ���� ��� Ȯ�� ����
        {
            weightMap[1] -= 0.1f;
            weightMap[2] -= 0.05f;
        }
    }

    public void DeactiveAllTotems() // ������ ���� ���� (���� ȿ�� Ȱ��ȭ �� ȣ�� �� ���� �������� ����)
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
using DuloGames.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemRandom : MonoBehaviour
{
    public float percentTotal;

    public InvenSlot[] slots;

    public int ItemWeightRandom(Dictionary<int, float> activepercent)
    {
        percentTotal = 0;  // Ȯ�� �� ��
        int maxLength = 0;  // �Ҽ��� �ڸ���

        foreach (var pair in activepercent) // ����ġ ������ ��Ż ���
        {
            float weight = pair.Value;
            int length = weight.ToString().Substring(weight.ToString().IndexOf('.') + 1).Length;
            percentTotal += weight;
            if (maxLength < length)
                maxLength = length;
        }
        int correction = (int)(percentTotal * (float)Mathf.Pow(10, maxLength));  // total�� ���������� ��ȯ�ϱ� ���� 10�� ��������

        int randomN = Random.Range(1, correction + 1);
        int tempNum = 0;
        int num = 0;

        foreach (var pair in activepercent)
        {
            num = (int)(correction * (float)pair.Value);
            if (num + tempNum >= randomN)
            {
                return pair.Key;
            }
            tempNum += num;
        }
        return 0;
    }

    public bool ItemRandomPercent(Item item, int itemcount)
    {
        Dictionary<int, float> activepercent = new Dictionary<int, float>();
        
        activepercent[0] = ItemDataBase.instance.Variable(item.itemID) * itemcount;
        activepercent[1] = percentTotal - ItemDataBase.instance.Variable(item.itemID) * itemcount;
        
        int triggerpercent = ItemWeightRandom(activepercent);

        if (triggerpercent == activepercent[0])
        {
            // Ȯ�� �ȿ� ������ �� �ߵ��Ǵ°�
            return true;
        }
        else
        {
            return false;
        }

    }
}

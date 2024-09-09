using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class RewardSlot : InvenSlot
{
    public void AddItemNoCount(Item _item, int _count = 1)         // ���� ������ �����Ǵ� ������ ������ �ϳ��̹Ƿ�
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        SetColor(1);
    }
    // ���Կ� ������ �߰� (�������� �ϳ��� �� ���)
    public void AddRewardRandomItem(Item item)
    {
        AddItemNoCount(item, 1); // ������ ������ �׻� 1�� ����
    }


}

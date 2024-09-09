using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class RewardSlot : InvenSlot
{
    public void AddItemNoCount(Item _item, int _count = 1)         // 보상 선택은 제공되는 아이템 개수가 하나이므로
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        SetColor(1);
    }
    // 슬롯에 아이템 추가 (아이템이 하나일 때 사용)
    public void AddRewardRandomItem(Item item)
    {
        AddItemNoCount(item, 1); // 아이템 개수는 항상 1로 설정
    }


}

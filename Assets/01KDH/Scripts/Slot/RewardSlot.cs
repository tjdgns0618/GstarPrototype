using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering;
using TMPro;

public class RewardSlot : InvenSlot
{
    public Button rewardBtn;
    public TextMeshProUGUI _rewardName;
    public TextMeshProUGUI _rewardEffect;

    private void Awake()
    {
        rewardBtn.onClick.AddListener(OnSlotButtonClicked); // 버튼 클릭 시 메서드 호출
    }

    private void OnSlotButtonClicked()
    {
        if (item != null)
        {
            Inventory inventory = FindObjectOfType<Inventory>(); // 인벤토리 찾아서
            inventory.AcquireItem(item, itemCount); // 아이템 추가
            ClearSlot(); // 슬롯 초기화
        }
    }

    public void AddItemNoCount(Item _item, int _count = 1)         // 보상 선택은 제공되는 아이템 개수가 하나이므로
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        _rewardName.text = _item.name;
        _rewardEffect.text = _item.itemEffect;
        
        SetColor(1);
    }
    // 슬롯에 아이템 추가 (아이템이 하나일 때 사용)
    public void AddRewardRandomItem(Item item)
    {
        AddItemNoCount(item, 1); // 아이템 개수는 항상 1로 설정
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering;
using TMPro;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class RewardSlot : InvenSlot
{
    public Button rewardBtn;
    public TextMeshProUGUI _rewardName;
    public TextMeshProUGUI _rewardEffect;

    ItemDataBase itemDatabase;

    private void Awake()
    {
        rewardBtn.onClick.AddListener(OnSlotButtonClicked); // 버튼 클릭 시 메서드 호출
    }
    private void Start()
    {
        itemDatabase = ItemDataBase.instance;
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

        float itemVariable = ItemDataBase.instance.Variable(item.itemID);
        float itemVariable2 = ItemDataBase.instance.Variable2(item.itemID);
        float itemVariable3 = ItemDataBase.instance.Variable3(item.itemID);
        float itemVariable4 = ItemDataBase.instance.Variable4(item.itemID);

        _rewardEffect.text = string.Format(_item.itemEffect, itemVariable, itemVariable2, itemVariable3, itemVariable4);

        SetColor(1);
    }
    // 슬롯에 아이템 추가 (아이템이 하나일 때 사용)
    public void AddRewardRandomItem(Item item)
    {
        AddItemNoCount(item, 1); // 아이템 개수는 항상 1로 설정
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
    }
}

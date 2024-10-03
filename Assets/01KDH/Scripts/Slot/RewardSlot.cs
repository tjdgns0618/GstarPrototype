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
        rewardBtn.onClick.AddListener(OnSlotButtonClicked); // ��ư Ŭ�� �� �޼��� ȣ��
    }
    private void Start()
    {
        itemDatabase = ItemDataBase.instance;
    }
    private void OnSlotButtonClicked()
    {
        if (item != null)
        {
            Inventory inventory = FindObjectOfType<Inventory>(); // �κ��丮 ã�Ƽ�
            inventory.AcquireItem(item, itemCount); // ������ �߰�
            ClearSlot(); // ���� �ʱ�ȭ
        }
    }

    public void AddItemNoCount(Item _item, int _count = 1)         // ���� ������ �����Ǵ� ������ ������ �ϳ��̹Ƿ�
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
    // ���Կ� ������ �߰� (�������� �ϳ��� �� ���)
    public void AddRewardRandomItem(Item item)
    {
        AddItemNoCount(item, 1); // ������ ������ �׻� 1�� ����
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
    }
}

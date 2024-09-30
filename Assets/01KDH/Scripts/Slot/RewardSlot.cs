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
        rewardBtn.onClick.AddListener(OnSlotButtonClicked); // ��ư Ŭ�� �� �޼��� ȣ��
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
        _rewardEffect.text = _item.itemEffect;
        
        SetColor(1);
    }
    // ���Կ� ������ �߰� (�������� �ϳ��� �� ���)
    public void AddRewardRandomItem(Item item)
    {
        AddItemNoCount(item, 1); // ������ ������ �׻� 1�� ����
    }
}

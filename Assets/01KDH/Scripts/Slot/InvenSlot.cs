using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Inventory _inventory;
    public Item item;
    public Item[] items;
    public SlotToolTip _slotToolTip;
    public Image itemImage;
    public Text textCount;

    public int itemCount;

    public virtual void SetColor(float _alpha)                                  // ������ ���� ����
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    public virtual void AddItem(Item _item, int _count = 1)         // ���� �������� ������ ���
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;
        textCount.text = itemCount.ToString();

        SetColor(1);
    }

    public virtual void SetSlotCount(int _count)                            // ������ ���� ������Ʈ
    {
        itemCount += _count;
        textCount.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    public virtual void ClearSlot()                                                 // �������� ���� �� ���� ����
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
    }


    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
            _slotToolTip.ShowToolTip(item, transform.position, itemCount);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        _slotToolTip.HideToolTip();
    }
}

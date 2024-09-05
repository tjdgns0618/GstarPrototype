using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemManager _item;
    public SlotToolTip _slotToolTip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_item != null)
            _slotToolTip.ShowToolTip(_item, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _slotToolTip.HideToolTip();
    }
}

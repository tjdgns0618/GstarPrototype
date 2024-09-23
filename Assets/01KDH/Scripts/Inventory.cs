using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InvenSlot[] slots;

    public bool waverewardActivate = false;

    public GameObject rewardUI;
    public GameObject slotsP;

    private void Start()
    {
        slots = slotsP.GetComponentsInChildren<InvenSlot>();
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemID == _item.itemID)
                    {
                        slots[i].SetSlotCount(_count);
                    Debug.Log("1111");
                        return;
                    }
                }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                Debug.Log("2222");
                return;
            }
        }
    }
}

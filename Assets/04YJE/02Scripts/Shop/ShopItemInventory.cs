using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemInventory : MonoBehaviour
{
    public InvenSlot[] slots;
    public InvenSlot[] shopslots;
    [SerializeField] GameObject[] slotsPr;



    public InvenSlot slot;
    public Inventory _inventory;

    private void Update()
    {
        Test();
    }
    
    void Test()
    {
        for (int j = 0; j < slots.Length; j++)
        {
            shopslots[j].item = slots[j].item;
            shopslots[j].itemCount = slots[j].itemCount;
            shopslots[j].itemImage.sprite = slots[j].itemImage.sprite;
            shopslots[j].itemImage.color = slots[j].itemImage.color;
        }
    }

    void Test2() //��ư�� ��ũ��Ʈ ���� �� �ֱ�
    {
        _inventory.EnchantItem(slot.item, 1);
    }
}

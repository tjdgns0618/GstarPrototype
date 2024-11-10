using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public GameObject[] buffUI;

    public ShopItem[] shopitem;
    public bool[] isOnBuff;

    GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }

    public void ActivateBuff(int _itemID)
    {
        int index = FindShopItemIndex(_itemID);

        gm._damage += shopitem[index].attackDamage;
        gm._critdmg += shopitem[index].criticalDamage;
        gm._critchance += shopitem[index].criticalRate;

        isOnBuff[_itemID] = true;
    }

    public void RemoveBuff(int _itemID)
    {
        int index = FindShopItemIndex(_itemID);

        if (isOnBuff[index] == true)
        {
            gm._damage -= shopitem[index].attackDamage;
            gm._critdmg -= shopitem[index].criticalDamage;
            gm._critchance -= shopitem[index].criticalRate;

            isOnBuff[_itemID] = false;
        }
    }

    public int FindShopItemIndex(int _itemID)
    {
        int findIndx = -1;

        for(int i = 0; i < shopitem.Length; i++)
        {
            if (shopitem[i].itemID == _itemID)
            {
                findIndx = i;
                break;
            }
        }

        return findIndx;
    }
}

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

    public void ActivateBuff(int itemNum)
    {
        int index = FindShopItemIndex(itemNum);

        gm._damage += shopitem[index].attackDamage;
        gm._critdmg += shopitem[index].criticalDamage;
        gm._critchance += shopitem[index].criticalRate;

        isOnBuff[itemNum] = true;
    }

    public void RemoveBuff(int itemNum)
    {
        int index = FindShopItemIndex(itemNum);

        if (isOnBuff[index] == true)
        {
            gm._damage -= shopitem[index].attackDamage;
            gm._critdmg -= shopitem[index].criticalDamage;
            gm._critchance -= shopitem[index].criticalRate;

            isOnBuff[itemNum] = false;
        }
    }

    public int FindShopItemIndex(int itemNum)
    {
        int findIndx = -1;

        for(int i = 0; i < shopitem.Length; i++)
        {
            if (shopitem[i].itemID == itemNum)
            {
                findIndx = i;
                break;
            }
        }

        return findIndx;
    }
}

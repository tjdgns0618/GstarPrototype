using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class BuffItem : MonoBehaviour
{
    public ShopItem shopitem;

    public bool isOnBuff;

    GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }

    public void ActivateBuff()
    {
        gm._damage += shopitem.attackDamage;
        gm._critdmg += shopitem.criticalDamage;
        gm._critchance += shopitem.criticalRate;

        shopitem.isItemUnbuyable = true;
        isOnBuff = true;
        gameObject.SetActive(true);
    }

    public void DeactivateBuff()
    {
        gm._damage -= shopitem.attackDamage;
        gm._critdmg -= shopitem.criticalDamage;
        gm._critchance -= shopitem.criticalRate;

        shopitem.isItemUnbuyable = false;
        isOnBuff = false;
        gameObject.SetActive(false);
    }
}


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoPotion : MonoBehaviour         //자동 회복 시켜주는 스크립트
{
    [HideInInspector] public ShopItem shopitem;
    [HideInInspector] public bool isAble;       //전투맵에서 마을로 오면 true, 전투맵에서 한번 피 회복하면 false

    private int recoveryCount;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
        shopitem = GetComponent<ShopItem>();
        InitRecoveryCount();
    }

    private void Update()
    {
        if (CheckHealthThreshold() && isAble)
        {
            //if(shopitem != null)
            //    shopitem.ActivateItemAbility();

            recoveryCount--;

            if (recoveryCount <= 0)
                SetDisablePotion();
        }
    }
    private void InitRecoveryCount()
    {
        if(shopitem != null)
            recoveryCount = shopitem.recoveryCount;
    }

    public void SetAblePotion()
    {
        isAble = true;
        shopitem.isItemUnbuyable = false;
        InitRecoveryCount();
    }

    public void SetDisablePotion()
    {
        isAble = false;
        shopitem.isItemUnbuyable = true;
    }

    public bool CheckHealthThreshold() //피가 일정 수치가 되었는지 확인
    {
        if(shopitem != null)
        {
            if (gm._hp <= gm._maxhp * 0.01f * shopitem.recoveryThreshold)
                return true;
            else
                return false;
        }
        return false;
    }
}

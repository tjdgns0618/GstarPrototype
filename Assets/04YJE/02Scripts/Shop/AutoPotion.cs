using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoPotion : MonoBehaviour         //자동 회복 시켜주는 스크립트
{
    public GameObject activeFrame;
    public GameObject disableFrame;
    public ShopItem shopitem;
    [HideInInspector] public bool isAble;       //전투맵에서 마을로 오면 true, 전투맵에서 한번 피 회복하면 false

    private int recoveryCount;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
        InitRecoveryCount();
    }

    private void Update()
    {
        if (CheckHealthThreshold() && isAble)
        {
            shopitem.ActivateItemAbility();
            recoveryCount--;

            if (recoveryCount <= 0)
                SetDisablePotion();
        }
    }
    private void InitRecoveryCount()
    {
        recoveryCount = shopitem.recoveryCount;
    }

    public void SetAblePotion()
    {
        isAble = true;
        activeFrame.SetActive(true);
        InitRecoveryCount();
    }

    public void SetDisablePotion()
    {
        isAble = false;
        activeFrame.SetActive(false);
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

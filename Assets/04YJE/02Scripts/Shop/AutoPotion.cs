using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoPotion : MonoBehaviour
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
        if (gm._hp <= HealthThreshold() && isAble)
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

    public float HealthThreshold()
    {
        return gm._maxhp * 0.01f * shopitem.recoveryThreshold;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoPotion : MonoBehaviour
{
    public GameObject activeFrame;
    public GameObject disableFrame;
    public ShopItem shopitem;
    [HideInInspector] public bool isAble;       //�����ʿ��� ������ ���� true, �����ʿ��� �ѹ� �� ȸ���ϸ� false

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
        disableFrame.SetActive(false);
        InitRecoveryCount();
    }

    public void SetDisablePotion()
    {
        isAble = false;
        activeFrame.SetActive(false);
        disableFrame.SetActive(true);
    }

    public float HealthThreshold()
    {
        return gm._maxhp * 0.01f * shopitem.recoveryThreshold;
    }
}

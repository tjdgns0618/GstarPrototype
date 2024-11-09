using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackSmithShop : MonoBehaviour
{
    public TMP_Text[] upgradeGuideText;
    public TMP_Text upGradeText;
    public TMP_Text upgradePriceText;
    public Image selItemImg;
    public GameObject messageBox;
    public TMP_Text message;
    public Inventory inventory;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private ShopItemDB shopitemDB;
    private InvenSlot selectedSlot;
    private GameManager gm;

    private void Start()
    {
        ShowUpgradeGuideText();
        gm = GameManager.instance;
    }

    private void Update()
    {
        UpdateSelectedItemUpgradeInfo();
    }

    public void ShowUpgradeGuideText()
    {
        upgradeGuideText[0].text =
           string.Format("1 -> 2 강화 성공 확률 {0}%\n"
                       + "2 -> 3 강화 성공 확률 {1}%\n"
                       + "3 -> 4 강화 성공 확률 {2}%\n"
                       + "4 -> 5 강화 성공 확률 {3}%\n",
                       shopitemDB.entities3[0].SuccessRate,
                       shopitemDB.entities3[1].SuccessRate,
                       shopitemDB.entities3[2].SuccessRate,
                       shopitemDB.entities3[3].SuccessRate);

        upgradeGuideText[1].text =
            string.Format("<sprite=0>{0}\n"
                        + "<sprite=0>{1}\n"
                        + "<sprite=0>{2}\n"
                        + "<sprite=0>{3}",
                        shopitemDB.entities3[0].Price,
                        shopitemDB.entities3[1].Price,
                        shopitemDB.entities3[2].Price,
                        shopitemDB.entities3[3].Price);
    }

    public void SelectItemToUpgrade(InvenSlot item)
    {
        selectedSlot = item;
    }

    public void UpdateSelectedItemUpgradeInfo()
    {
        if (selectedSlot != null)
        {
            selItemImg.sprite = selectedSlot.itemImage.sprite;
            if (selectedSlot.itemCount < 5)
            {
                upgradePriceText.text = string.Format("<sprite=0>{0}", shopitemDB.entities3[selectedSlot.itemCount - 1].Price);
                upGradeText.text = string.Format("{0} -> {1}", selectedSlot.itemCount, selectedSlot.itemCount + 1);
            }
            else
            {
                upgradePriceText.text = string.Format(" ");
                upGradeText.text = string.Format("5 (MAX)");
            }
        }
    }

    public void TryItemUpgrade()
    {
        if(selectedSlot != null && shopitemDB != null)
        {
            float successRate = 0;
            int price = shopitemDB.entities3[selectedSlot.itemCount - 1].Price;

            if (selectedSlot.itemCount <= 0)
                return;
            if (gm._gold < price)
            {
                OpenMessageBox("돈이 부족합니다.");
                return;
            }

            if (selectedSlot.itemCount >= 5 && !CheckItemUpgradeable(selectedSlot.item))
            {
                OpenMessageBox("더 이상 강화할 수 없습니다.");
                return;
            }
                
            successRate = shopitemDB.entities3[selectedSlot.itemCount - 1].SuccessRate;
            gm._gold -= price;

            if (successRate >= CreateRandomNumber())
                UpgradeSuccess();
            else
                UpgradeFail();
        }
    }

    public void OpenMessageBox(string text)
    {
        message.text = text;
        uiManager.OpenPopup(messageBox);
    }

    public float CreateRandomNumber()
    {
        float rand = Random.Range(0.00f, 100.00f);

        return rand;
    }

    public void UpgradeFail()
    {
        Debug.Log("업그레이드 실패");
    }

    public void UpgradeSuccess()
    {
        inventory.EnchantItem(selectedSlot.item, 1);
        OpenMessageBox("강화를 성공하였습니다!");
        UpdateSelectedItemUpgradeInfo();
    }



    public bool CheckItemUpgradeable(Item item)
    {
        bool canUpgrade = true;

        if (GameManager.instance._skillCooltimePercent <= 0.5)
        {
            canUpgrade = item.itemType2 == Item.ItemType2.cooldown ? false : true;

            if (!canUpgrade)
                return canUpgrade;
        }
        if (GameManager.instance._attackspeed >= 9)
        {
            canUpgrade = item.itemType2 == Item.ItemType2.attackspeedA ? false : true;

            if (!canUpgrade)
                return canUpgrade;
        }
        if (GameManager.instance._attackspeed <= 2.5f)
        {
            canUpgrade = item.itemType2 == Item.ItemType2.attackspeedM ? false : true;

            if (!canUpgrade)
                return canUpgrade;
        }
        if (GameManager.instance._movespeed >= 350f)
        {
            canUpgrade = item.itemType2 == Item.ItemType2.movespeedA ? false : true;

            if (!canUpgrade)
                return canUpgrade;
        }
        if (GameManager.instance._movespeed <= 250f)
        {
            canUpgrade = item.itemType2 == Item.ItemType2.movespeedM ? false : true;

            if (!canUpgrade)
                return canUpgrade;
        }
        if (GameManager.instance._maxhp >= 750f)
        {
            canUpgrade = item.itemType2 == Item.ItemType2.maxhpM ? false : true;

            if (!canUpgrade)
                return canUpgrade;
        }

        return canUpgrade;
    }
}

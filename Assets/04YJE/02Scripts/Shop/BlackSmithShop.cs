using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TickManager;

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

    public PassiveItem passive;

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

            if (selectedSlot.itemCount >= 5)
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
        if (selectedSlot.item.itemType == Item.ItemType.passive)
        {
            switch (selectedSlot.item.itemID)
            {
                case 0:
                    passive._01Pitem();
                    break;
                case 1:
                    passive._02Pitem();
                    break;
                case 2:
                    passive._03Pitem();
                    break;
                case 3:
                    passive._04Pitem();
                    break;
                case 4:
                    passive._05Pitem();
                    break;
                case 5:
                    passive._06Pitem();
                    break;
                case 6:
                    passive._07Pitem();
                    break;
                case 7:
                    passive._08Pitem();
                    break;
                case 8:
                    passive._09Pitem();
                    break;
                case 9:
                    passive._10Pitem();
                    TickManager._regenT += GameManager.instance.LifeGen;
                    break;
                case 10:
                    passive._11Pitem();
                    break;
                case 11:
                    passive._12Pitem();
                    break;
                case 12:
                    passive._13Pitem();
                    break;
                case 13:
                    passive._14Pitem();
                    break;
                case 14:
                    passive._15Pitem();
                    break;
                case 15:
                    passive._16Pitem();
                    break;
                case 16:
                    passive._17Pitem();
                    break;
                case 17:
                    passive._18Pitem();
                    break;
                case 18:
                    passive._19Pitem();
                    break;
                case 19:
                    passive._20Pitem();
                    break;
                case 20:
                    passive._21Pitem();
                    break;
                case 21:
                    passive._22Pitem();
                    break;
                case 22:
                    passive._23Pitem();
                    break;
                case 23:
                    passive._24Pitem();
                    break;
                case 24:
                    passive._25Pitem();
                    break;
                case 25:
                    passive._26Pitem();
                    break;
                case 26:
                    passive._27Pitem();
                    break;
                case 27:
                    passive._28Pitem();
                    break;
                case 28:
                    passive._29Pitem();
                    break;
                default:
                    break;
            }
        }
            OpenMessageBox("강화를 성공하였습니다!");
            UpdateSelectedItemUpgradeInfo();
    }
}

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
    public Inventory inventory;

    [SerializeField] private ShopItemDB shopitemDB;
    private InvenSlot selectedSlot;

    private void Start()
    {
        upgradeGuideText[0].text =
            string.Format("1 -> 2 ��ȭ ���� Ȯ�� {0}%\n"
                        + "2 -> 3 ��ȭ ���� Ȯ�� {1}%\n"
                        + "3 -> 4 ��ȭ ���� Ȯ�� {2}%\n"
                        + "4 -> 5 ��ȭ ���� Ȯ�� {3}%\n",
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
    
    public void ShowUpgradeGuideText()
    {

    }

    public void SelectItemToUpgrade(InvenSlot item)
    {
        selectedSlot = item;

        UpdateSelectedItemUpgradeInfo();
    }

    public void UpdateSelectedItemUpgradeInfo()
    {
        selItemImg.sprite = selectedSlot.itemImage.sprite;
        upGradeText.text = string.Format("{0} -> {1}",selectedSlot.itemCount, (selectedSlot.itemCount + 1));
        upgradePriceText.text = string.Format("<sprite=0>{0}", shopitemDB.entities3[selectedSlot.itemCount - 1].Price);
    }

    public void TryItemUpgrade()
    {
        if(selectedSlot != null && shopitemDB != null)
        {
            float successRate = 0;

            if (selectedSlot.itemCount <= 0)
                return;
            if (selectedSlot.itemCount >= 5)
            {
                Debug.Log("�� �̻� ��ȭ�� �� �����ϴ�.");
                return;
            }
                
            successRate = shopitemDB.entities3[selectedSlot.itemCount - 1].SuccessRate;

            if (successRate >= CreateRandomNumber())
                UpgradeSuccess();
            else
                UpgradeFail();
        }
    }

    public float CreateRandomNumber()
    {
        float rand = Random.Range(0.00f, 100.00f);

        return rand;
    }

    public void UpgradeFail()
    {
        Debug.Log("���׷��̵� ����");
    }

    public void UpgradeSuccess()
    {
        Debug.Log("���׷��̵� ����");
        inventory.EnchantItem(selectedSlot.item, 1);
    }
}

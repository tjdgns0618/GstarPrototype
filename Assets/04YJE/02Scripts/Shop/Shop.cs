using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopItemDB shopitemDB;
    [SerializeField] private ShopItem[] shopItems;

    [SerializeField] private GameObject messageBox;
    [SerializeField] private TMP_Text message;

    public TMP_Text gold_Txt;
    public Image showImage;

    private int selectedShopItemID;

    public AutoPotion[] autopotion;

    private GameManager gm;
    public UIManager uiManager;

    private void Start()
    {
        gm = GameManager.instance;

        int index = 0;

        for (int i = 0; i < shopitemDB.entities.Count; ++i)
        {
            //상점 아이템 데이터 불러오기 및 초기화
            if (shopItems[index].itemID == shopitemDB.entities[i].ItemID)
            {
                shopItems[index].itemName = shopitemDB.entities[i].ItemName;
                shopItems[index].itemInfo = shopitemDB.entities[i].ItemInfo;
                shopItems[index].price = shopitemDB.entities[i].Price;
                shopItems[index].maxLevel = shopitemDB.entities[i].MaxLevel;
                shopItems[index].attackDamage = shopitemDB.entities[i].AttackDamage;
                shopItems[index].isAutoPotion = shopitemDB.entities[i].IsAuto;
                shopItems[index].recoveryThreshold = shopitemDB.entities[i].RecoveryThreshold;
                shopItems[index].recoveryCount = shopitemDB.entities[i].RecoveryCount;
                shopItems[index].hp = shopitemDB.entities[i].HP;
                shopItems[index].hpRate = shopitemDB.entities[i].HPRate;
                shopItems[index].criticalDamage = shopitemDB.entities[i].CriticalDamage;
                shopItems[index].criticalRate = shopitemDB.entities[i].CriticalRate;
                shopItems[index].dashCoolTime = shopitemDB.entities[i].DashCoolTime;
                shopItems[index].itemCoolTimeDropRate = shopitemDB.entities[i].ItemCoolTimeDropRate;
                          
                shopItems[index].SetShop(this);

                index++;

                if (index == shopItems.Length)
                    break;
            }
        }
    }

    private void Update()
    {
        gold_Txt.text = gm._gold + " Gold";

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            TestGold();
        }
    }

    public void GoldTrade(int _cost_gold)   
    {
        gm._gold -= _cost_gold;
    }

    public void SelectShopItem(int shopitemID)
    {
        selectedShopItemID = shopitemID;
    }

    public void BuySelectedShopItem()
    {
        ShopItem selectedItem = FindShopItem(selectedShopItemID);

        if (selectedItem.isItemUnbuyable)
        {
            OpenMessageBox("더 이상 구매할 수 없습니다.");
            return;
        }

        //골드 줄어들기
        if (gm._gold < selectedItem.price)
        {
            OpenMessageBox("돈이 부족합니다.");
            return;
        }

        GoldTrade(selectedItem.price);

        //구매 성공 및 아이템 효과 작용
        if (!selectedItem.isAutoPotion)
            selectedItem.ActivateItemAbility();
        else
        {
            FindShopItem(selectedShopItemID).isItemUnbuyable = true;

            if (selectedShopItemID == 1100)
            {
                autopotion[0].SetAblePotion();
            }
            else if (selectedShopItemID == 1101)
            {
                autopotion[1].SetAblePotion();
            }
        }
    }

    private ShopItem FindShopItem(int itemID)
    {
        foreach(var item in shopItems)
        {
            if (item.itemID == itemID)
            { 
                return item;
            }
        }

        return null;
    }

    public void HpTrade(int _cost_hp)                           // 피 부족할 때 부족하다는 UI(자막) 추가
    {
        if (gm._hp > _cost_hp)
        {
            gm._hp -=  _cost_hp;
        }
        else
        {
            Debug.Log("체력이 부족합니다.");
        }
    }

    public void TestGold()
    {
        gm._gold += 100000;
    }

    public void TestHp()
    {
        gm._hp -= 10;
    }

    public void OpenMessageBox(string text)
    {
        message.text = text;
        uiManager.OpenPopup(messageBox);
    }
}

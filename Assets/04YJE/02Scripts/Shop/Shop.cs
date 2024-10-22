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

    [SerializeField] private int adjustIndex;

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

        for (int i = 0; i < shopItems.Length; ++i)
        {
            //상점 아이템 데이터 불러오기 및 초기화
            if (shopItems[i].itemID == shopitemDB.entities[i + adjustIndex].ItemID)
            {
                shopItems[i].itemName = shopitemDB.entities[i + adjustIndex].ItemName;
                shopItems[i].itemInfo = shopitemDB.entities[i + adjustIndex].ItemInfo;
                shopItems[i].price = shopitemDB.entities[i + adjustIndex].Price;
                shopItems[i].maxLevel = shopitemDB.entities[i + adjustIndex].MaxLevel;
                shopItems[i].attackDamage = shopitemDB.entities[i + adjustIndex].AttackDamage;
                shopItems[i].diffence = shopitemDB.entities[i + adjustIndex].Deffence;
                shopItems[i].isAutoPotion = shopitemDB.entities[i + adjustIndex].IsAuto;
                shopItems[i].recoveryThreshold = shopitemDB.entities[i + adjustIndex].RecoveryThreshold;
                shopItems[i].recoveryCount = shopitemDB.entities[i + adjustIndex].RecoveryCount;
                shopItems[i].hp = shopitemDB.entities[i + adjustIndex].HP;
                shopItems[i].hpRate = shopitemDB.entities[i + adjustIndex].HPRate;
                shopItems[i].criticalDamage = shopitemDB.entities[i + adjustIndex].CriticalDamage;
                shopItems[i].criticalRate = shopitemDB.entities[i + adjustIndex].CriticalRate;
                shopItems[i].dashCoolTime = shopitemDB.entities[i + adjustIndex].DashCoolTime;
                shopItems[i].itemCoolTimeDropRate = shopitemDB.entities[i + adjustIndex].ItemCoolTimeDropRate;

                shopItems[i].SetShop(this);
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
                autopotion[0].SetAblePotion();
            else if (selectedShopItemID == 1101)
                autopotion[1].SetAblePotion();
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

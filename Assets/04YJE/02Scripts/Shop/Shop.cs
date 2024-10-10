using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ShopItemImageData
{
    public int itemID;
    public Sprite itemImage;
}

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopItemDB shopitemDB;
    [SerializeField] private ShopItemImageData[] shopItemImages;

    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemParent;

    public TMP_Text gold_Txt;

    private int selectedShopItemID;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;

        for (int i = 0; i < shopItemImages.Length; ++i)
        {
            //상점 아이템 생성
            GameObject shopitem = Instantiate(itemPrefab, itemParent);
            ShopItem shopitemData = shopitem.GetComponent<ShopItem>();

            //상점 아이템 데이터 불러오기 및 초기화
            if (shopitemDB.entities[i].itemID == shopItemImages[i].itemID)
            shopitemData.itemID = shopItemImages[i].itemID;
            shopitemData.itemImage = shopItemImages[i].itemImage;
            shopitemData.itemName = shopitemDB.entities[i].itemName;
            shopitemData.itemInfo = shopitemDB.entities[i].itemInfo;
            shopitemData.price = shopitemDB.entities[i].price;
            shopitemData.attackDamage = shopitemDB.entities[i].attackDamage;
            shopitemData.diffence = shopitemDB.entities[i].diffence;
            shopitemData.hp = shopitemDB.entities[i].hp;
            shopitemData.hpRate = shopitemDB.entities[i].hpRate;
            shopitemData.criticalDamage = shopitemDB.entities[i].criticalDamage;
            shopitemData.criticalRate = shopitemDB.entities[i].criticalRate;
            shopitemData.SetShop(this);
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

    public void GoldTrade(int _cost_gold)                   // 돈 부족할 때 부족하다는 UI(자막) 추가
    {
        if (gm._gold >= _cost_gold)
        {
            gm._gold -= _cost_gold;
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void SelectShopItem(int shopitemID)
    {
        selectedShopItemID = shopitemID;
    }

    public void BuySelectedShopItem()
    {
        //골드 줄어들기
        GoldTrade(GetShopItemPrice(selectedShopItemID));

        //아이템 개수 추가
    }

    private int GetShopItemPrice(int itemID)
    {
        int price = -1;

        for (int i = 0; i < shopitemDB.entities.Count; ++i)
        {
            if (shopitemDB.entities[i].itemID == itemID)
                price = shopitemDB.entities[i].price;
        }

        if (price < 0)
        { 
            Debug.Log("아이템을 찾을 수 없습니다.");
            return 0;
        }

        return price;
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
        gm._gold += 500;
    }

    public void TestHp()
    {
        gm._hp -= 10;
    }
}

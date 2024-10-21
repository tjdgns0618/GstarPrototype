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

    [SerializeField] private int adjustIndex;
    
    public TMP_Text gold_Txt;

    private int selectedShopItemID;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;

        for (int i = adjustIndex; i < adjustIndex + shopItemImages.Length; ++i)
        {

            //���� ������ ������ �ҷ����� �� �ʱ�ȭ
            if (shopitemDB.entities[i].ItemID == shopItemImages[i - adjustIndex].itemID)
            {
                //���� ������ ����
                GameObject shopitem = Instantiate(itemPrefab, itemParent);
                ShopItem shopitemData = shopitem.GetComponent<ShopItem>();

                shopitemData.itemID = shopitemDB.entities[i].ItemID;
                shopitemData.itemImage = shopItemImages[i - adjustIndex].itemImage;
                shopitemData.itemName = shopitemDB.entities[i].ItemName;
                shopitemData.itemInfo = shopitemDB.entities[i].ItemInfo;
                shopitemData.price = shopitemDB.entities[i].Price;
                shopitemData.maxLevel = shopitemDB.entities[i].MaxLevel;
                shopitemData.attackDamage = shopitemDB.entities[i].AttackDamage;
                shopitemData.diffence = shopitemDB.entities[i].Deffence;
                shopitemData.hp = shopitemDB.entities[i].HP;
                shopitemData.hpRate = shopitemDB.entities[i].HPRate;
                shopitemData.criticalDamage = shopitemDB.entities[i].CriticalDamage;
                shopitemData.criticalRate = shopitemDB.entities[i].CriticalRate;
                shopitemData.dashCoolTime = shopitemDB.entities[i].DashCoolTime;
                shopitemData.itemCoolTimeDropRate = shopitemDB.entities[i].ItemCoolTimeDropRate;

                shopitemData.SetShop(this);
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
        if (gm._gold >= _cost_gold)
        {
            gm._gold -= _cost_gold;
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    public void SelectShopItem(int shopitemID)
    {
        selectedShopItemID = shopitemID;
    }

    public void BuySelectedShopItem()
    {
        //��� �پ���
        GoldTrade(GetShopItemPrice(selectedShopItemID));

        //������ ���� �߰�
    }

    private int GetShopItemPrice(int itemID)
    {
        int price = -1;

        for (int i = 0; i < shopitemDB.entities.Count; ++i)
        {
            if (shopitemDB.entities[i].ItemID == itemID)
                price = shopitemDB.entities[i].Price;
        }

        if (price < 0)
        { 
            Debug.Log("�������� ã�� �� �����ϴ�.");
            return 0;
        }

        return price;
    }

    public void HpTrade(int _cost_hp)                           // �� ������ �� �����ϴٴ� UI(�ڸ�) �߰�
    {
        if (gm._hp > _cost_hp)
        {
            gm._hp -=  _cost_hp;
        }
        else
        {
            Debug.Log("ü���� �����մϴ�.");
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

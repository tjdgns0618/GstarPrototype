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
            //���� ������ ����
            GameObject shopitem = Instantiate(itemPrefab, itemParent);
            ShopItem shopitemData = shopitem.GetComponent<ShopItem>();

            //���� ������ ������ �ҷ����� �� �ʱ�ȭ
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

    public void GoldTrade(int _cost_gold)                   // �� ������ �� �����ϴٴ� UI(�ڸ�) �߰�
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
            if (shopitemDB.entities[i].itemID == itemID)
                price = shopitemDB.entities[i].price;
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

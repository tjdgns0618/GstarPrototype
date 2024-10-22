using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopItem : MonoBehaviour
{
    public int itemID;
    public Sprite itemImage;
    public string itemName;
    public string itemInfo;
    public int price;
    public int maxLevel;
    public float attackDamage;
    public float diffence;
    public float recoveryThreshold;
    public int recoveryCount;
    public float hp;
    public float hpRate;
    public float criticalDamage;
    public float criticalRate;
    public float dashCoolTime;
    public float itemCoolTimeDropRate;
    
    protected Shop shop;
    public TMP_Text itemNameText;
    public TMP_Text itemInfoText;
    public TMP_Text itemPriceText;

    public bool isAutoPotion;
    public bool isItemUnbuyable;
    protected GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
        SetShopItemData();
        isAutoPotion = itemID == 1100 || itemID == 1101 ? true : false;
    }

    //������ �̸�, ����, ���� ǥ��
    public void SetShopItemData()
    {
        itemNameText.text = itemName;
        itemInfoText.text = itemInfo;
        itemPriceText.text = string.Format("<sprite=0>{0}",price);
    }

    //���� ���õ� ������ ǥ��
    public void ShowSelectedShopItem()
    {
        shop.showImage.sprite = itemImage;
    }

    //���õ� �������� �ٲ�
    public void SelectedShopItemChange()
    {
        shop.SelectShopItem(itemID);
    }

    //Shop ����
    public void SetShop(Shop _shop)
    {
        shop = _shop;
    }

    public void ActivateItemAbility()
    {
        gm._hp += hp;
        gm._hp += gm._maxhp / 100 * hpRate;
        gm._damage += attackDamage;
        gm._critdmg += criticalDamage;
        gm._critchance += criticalRate;
        gm._dashcount += (int)dashCoolTime;
        //������ ��Ÿ�� ����, �뽬 ��Ÿ�� ���� �߰�
    }
}

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
    public float hp;
    public float hpRate;
    public float criticalDamage;
    public float criticalRate;
    public float dashCoolTime;
    public float itemCoolTimeDropRate;

    private Image showImage;
    private Shop shop;
    public TMP_Text itemNameText;
    public TMP_Text itemInfoText;
    public TMP_Text itemPriceText;

    private void Start()
    {
        showImage = transform.parent.parent.parent.parent.GetChild(4).GetChild(1).GetComponent<Image>();
        SetShopItemData();
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
        showImage.sprite = itemImage;
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
}

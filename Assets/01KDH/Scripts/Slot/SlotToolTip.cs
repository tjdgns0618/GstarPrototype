using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotToolTip : MonoBehaviour
{
    public GameObject tBox;

    public TextMeshProUGUI txt_Itemname;
    public TextMeshProUGUI txt_Itemeffect;

    ItemDataBase itemDatabase;

    private void Start()
    {
        itemDatabase = ItemDataBase.instance;
    }

    public void ShowToolTip(Item _item, Vector3 _pos, int _count)
    {
        tBox.SetActive(true);
        _pos += new Vector3(tBox.GetComponent<RectTransform>().rect.width * 0.05f,
                           -tBox.GetComponent<RectTransform>().rect.height * 0.57f, 0);
        tBox.transform.position = _pos;
        txt_Itemname.text = _item.itemName;

        var itemData = itemDatabase.GetItem(_item.itemID);
        if(itemData.HasValue)
        {
            float itemVariable = ItemDataBase.instance.Itemtext(_item.itemID);
            float itemVariable2 = ItemDataBase.instance.Itemtext2(_item.itemID);
            float itemVariable3 = ItemDataBase.instance.Itemtext3(_item.itemID);
            float itemVariable4 = ItemDataBase.instance.Itemtext4(_item.itemID);

            txt_Itemeffect.text = string.Format(_item.itemEffect, itemVariable, itemVariable2, itemVariable3, itemVariable4);
        }
    }

    public void HideToolTip()
    {
        tBox.SetActive(false);
    }
}

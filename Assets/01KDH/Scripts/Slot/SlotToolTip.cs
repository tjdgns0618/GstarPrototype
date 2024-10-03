using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;

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
        _pos += new Vector3(tBox.GetComponent<RectTransform>().rect.width * 0.65f,
                           -tBox.GetComponent<RectTransform>().rect.height * -0.25f, 0);
        tBox.transform.position = _pos;
        txt_Itemname.text = _item.itemName;

        var itemData = itemDatabase.GetItem(_item.itemID);
        if(itemData.HasValue)
        {
            float itemVariable = itemData.Value.itemVariable_index;
            txt_Itemeffect.text = string.Format(_item.itemEffect, itemVariable * _count);
        }
    }

    public void HideToolTip()
    {
        tBox.SetActive(false);
    }
}

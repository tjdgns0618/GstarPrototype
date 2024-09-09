using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    public GameObject tBox;

    public Text txt_Itemname;
    public Text txt_Itemeffect;
    public Text txt_Iteminfo;

    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        tBox.SetActive(true);
        _pos += new Vector3(tBox.GetComponent<RectTransform>().rect.width * 0.75f,
                           -tBox.GetComponent<RectTransform>().rect.height * 0.5f, 0);
        tBox.transform.position = _pos;

        txt_Itemname.text = _item.itemName;
        txt_Itemeffect.text = _item.itemEffect;
        txt_Iteminfo.text = _item.itemInfo;
    }

    public void HideToolTip()
    {
        tBox.SetActive(false);
    }
}

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
    public TextMeshProUGUI txt_Iteminfo;

    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        tBox.SetActive(true);
        _pos += new Vector3(tBox.GetComponent<RectTransform>().rect.width * 0.65f,
                           -tBox.GetComponent<RectTransform>().rect.height * -0.25f, 0);
        tBox.transform.position = _pos;

        txt_Itemname.text = _item.itemName;
        txt_Itemeffect.text = _item.itemEffect;
    }

    public void HideToolTip()
    {
        tBox.SetActive(false);
    }
}

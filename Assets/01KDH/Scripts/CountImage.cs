using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountImage : MonoBehaviour
{
    public InvenSlot slot;
    public Image countImage;

    private void Update()
    {
        CountImageColor();
    }

    public void SetColor(float _alpha)                                  // 아이템 투명도 조절
    {
        Color color = countImage.color;
        color.a = _alpha;
        countImage.color = color;
    }

    public void CountImageColor()
    {
        if(slot.itemCount <= 1)
            SetColor(0);
        else
            SetColor(1);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingTextController : MonoBehaviour
{
    public TMP_Text damageText;

    public void InitText(string text, Color color)
    {
        damageText.text = text;
        damageText.color = color;
    }
}

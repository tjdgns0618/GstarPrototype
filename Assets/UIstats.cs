using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIstats : MonoBehaviour
{
    public TextMeshProUGUI maxhpT;
    public TextMeshProUGUI damageT;
    public TextMeshProUGUI rangeT;
    public TextMeshProUGUI coolT;
    public TextMeshProUGUI attackspeedT;
    public TextMeshProUGUI critchanceT;
    public TextMeshProUGUI critdmgT;

    void Update()
    {
        maxhpT.text = GameManager.instance._maxhp.ToString();
        damageT.text = GameManager.instance._damage.ToString();
        rangeT.text = GameManager.instance._range.ToString();
        coolT.text = GameManager.instance._cooldown.ToString();
        attackspeedT.text = GameManager.instance._attackspeed.ToString();
        critchanceT.text = GameManager.instance._critchance.ToString();
        critdmgT.text = GameManager.instance._critdmg.ToString();
    }
}

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
    public TextMeshProUGUI movespeedT;
    public TextMeshProUGUI critchanceT;
    public TextMeshProUGUI critdmgT;
    public TextMeshProUGUI regenhpT;
    public TextMeshProUGUI dashcountT;
    public TextMeshProUGUI skillcountT;
    public TextMeshProUGUI ultcountT;

    void Update()
    {
        maxhpT.text = GameManager.instance._maxhp.ToString();
        damageT.text = GameManager.instance._damage.ToString();
        rangeT.text = GameManager.instance._range.ToString() + "%";
        coolT.text = GameManager.instance._cooldown.ToString() + "%";
        attackspeedT.text = GameManager.instance._attackspeed.ToString()+"%";
        movespeedT.text = GameManager.instance._movespeed.ToString() + "%";
        critchanceT.text = GameManager.instance._critchance.ToString() + "%";
        critdmgT.text = GameManager.instance._critdmg.ToString() + "%";
        regenhpT.text = GameManager.instance._lifegen.ToString();
        dashcountT.text = "최대 " + GameManager.instance._dashcount.ToString()+"회";
        skillcountT.text = "최대 " + GameManager.instance._skillcount.ToString() + "회";
        ultcountT.text = "최대 " + GameManager.instance._ultcount.ToString() + "회";
    }
}

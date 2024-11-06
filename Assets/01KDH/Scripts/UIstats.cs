using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIstats : MonoBehaviour
{
    public TextMeshProUGUI maxhpT;
    public TextMeshProUGUI damageT;
    public TextMeshProUGUI coolT;
    public TextMeshProUGUI attackspeedT;
    public TextMeshProUGUI movespeedT;
    public TextMeshProUGUI critchanceT;
    public TextMeshProUGUI critdmgT;

    void Update()
    {
        maxhpT.text = GameManager.instance._maxhp.ToString();
        damageT.text = GameManager.instance._damage.ToString();
        coolT.text = (GameManager.instance._skillCooltimePercent*100f).ToString() + "%";
        attackspeedT.text = GameManager.instance._attackspeed.ToString();
        movespeedT.text = GameManager.instance._movespeed.ToString();
        critchanceT.text = GameManager.instance._critchance.ToString() + "%";
        critdmgT.text = (GameManager.instance._critdmg*100f).ToString() + "%";
    }
}

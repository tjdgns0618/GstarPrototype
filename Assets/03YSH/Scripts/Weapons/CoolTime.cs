using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoolTime : MonoBehaviour
{
    PlayerCharacter playerCharacter;
    public TextMeshProUGUI timer;
    public Image cooldownImage;
    public GameObject frame;

    private void Start()
    {
        playerCharacter = FindObjectOfType<PlayerCharacter>();
    }

    private void Update()
    {
        string skillKey = $"{playerCharacter.characterClass}{this.gameObject.name}";

        float cooltime = GameManager.instance.cooltimeManager.currentCoolDowns[skillKey];
        float cooltime_max = GameManager.instance.cooltimeManager.skillCoolDowns[skillKey];

        cooldownImage.fillAmount = cooltime / cooltime_max;

        string s = TimeSpan.FromSeconds(cooltime).ToString(@"ss");
        timer.text = string.Format("{0}", s);

        if(timer.text == "00" && GameManager.instance.cooltimeManager.canUseSkill[skillKey])
        {
            timer.text = "";
            frame?.SetActive(true);
        }
        else
        {
            frame?.SetActive(false);
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoolTime : MonoBehaviour
{
    float cooltime = 10f;
    float cooltime_max = 10f;
    PlayerCharacter playerCharacter;
    public TextMeshProUGUI timer;
    public Image cooldownImage;

    private void Start()
    {
        playerCharacter = FindObjectOfType<PlayerCharacter>();

        StartCoroutine(CooldownFunc());
    }

    IEnumerator CooldownFunc()
    {
        while(cooltime > 0.0f)
        {
            cooltime -= Time.deltaTime;

            cooldownImage.fillAmount = cooltime / cooltime_max;
            
            string s = TimeSpan.FromSeconds(cooltime).ToString(@"ss");
            timer.text = string.Format("{0}",s);

            yield return new WaitForFixedUpdate();
        }
    }
}

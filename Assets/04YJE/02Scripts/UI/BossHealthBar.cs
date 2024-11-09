using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider healthSlider;

    private Boss curBoss;

    private void Update()
    {
        //UpdateHealthSlider(보스 현재 체력);
    }

    public void Init(Boss boss)
    {
        //healthSlider.maxValue = 보스 최대 체력
        //healthSlider.value = maxValue;
        curBoss = boss;
    }

    public void ActiveBossHealthBar(Boss boss)
    {
        Init(boss);
        gameObject.SetActive(true);
    }

    public void UpdateHealthSlider(float hp)
    {
        if (healthSlider != null)
        {
            healthSlider.value = hp;

            if(healthSlider.value <= 0)
                gameObject.SetActive(false);
        }
    }
}

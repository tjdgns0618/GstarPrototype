using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    private Boss curBoss;
    private Slider healthSlider;

    private void Awake()
    {
        healthSlider = GetComponent<Slider>();
    }

    public void SetHp(float hp)
    {
        healthSlider.value = hp;
    }

    private void Update()
    {
        //UpdateHealthSlider(보스 현재 체력);
    }

    //public void Init(Boss boss)
    //{
    //    //healthSlider.maxValue = 보스 최대 체력
    //    //healthSlider.value = maxValue;
    //    curBoss = boss;
    //}

    //public void ActiveBossHealthBar(Boss boss)
    //{
    //    Init(boss);
    //    gameObject.SetActive(true);
    //}
}

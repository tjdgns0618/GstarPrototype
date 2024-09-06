using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth;
    public float curHealth;

    private float lerpSpeed = 0.01f;

    void Start()
    {
        if (healthSlider != null)
            healthSlider.maxValue = maxHealth;
        if (easeHealthSlider != null)
            easeHealthSlider.maxValue = maxHealth;
    }

    void Update()
    {
        UpdateHealthSlider(curHealth);
        UpdateEaseHealthSlider(healthSlider.value);

        /*
        if (Input.GetKeyUp(KeyCode.UpArrow))
            IncreaseHP(100);
        if (Input.GetKeyUp(KeyCode.DownArrow))
            DecreaseHP(100);*/

    }

    //테스트용--시작
    private void IncreaseHP(float health)
    {
        curHealth += health;
    }

    private void DecreaseHP(float health)
    {
        curHealth -= health;
    }
    //테스트용--끝

    public void UpdateHealthSlider(float hp)
    {
        if (healthSlider != null)
            healthSlider.value = hp;
    }

    public void UpdateEaseHealthSlider(float hp)
    {
        if (easeHealthSlider != null)
            if (easeHealthSlider.value > hp)
                easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, hp, lerpSpeed);
            else
                easeHealthSlider.value = hp;
    }
}

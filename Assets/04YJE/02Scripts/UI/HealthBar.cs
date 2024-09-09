using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHP;
    public float curHP;

    private float lerpSpeed = 0.01f;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.instance;
    }

    void Start()
    {
        if (healthSlider != null)
            healthSlider.maxValue = gameManager._maxhp;
        if (easeHealthSlider != null)
            easeHealthSlider.maxValue = gameManager._maxhp;
    }

    void Update()
    {
        UpdateHealthSlider(gameManager._hp);
        UpdateEaseHealthSlider(gameManager._hp);

        
        if (Input.GetKeyUp(KeyCode.UpArrow))
            IncreaseHP(10);
        if (Input.GetKeyUp(KeyCode.DownArrow))
            DecreaseHP(10);

    }

    //테스트용--시작
    private void IncreaseHP(float health)
    {
        gameManager._hp += health;
    }

    private void DecreaseHP(float health)
    {
        gameManager._hp -= health;
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

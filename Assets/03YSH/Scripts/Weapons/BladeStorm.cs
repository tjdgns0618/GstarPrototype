using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class BladeStorm : MonoBehaviour
{
    public float damagePercentage = 0.1f;
    float timer = 0;
    float duration = 0.5f;
    EnemyAI enemyAI;

    private void OnEnable()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            timer = 0;
            enemyAI.Damage(GameManager.instance._damage * damagePercentage);
        }        
    }
}
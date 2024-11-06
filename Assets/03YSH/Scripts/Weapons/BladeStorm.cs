using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeStorm : MonoBehaviour
{
    float timer = 0;
    float duration = 0.5f;
    public GameObject target;


    private void OnEnable()
    {
        timer = 0;
    }

    private void OnDisable()
    {
        timer = 0;
        target = null;
    }

    void Update()
    {
        if (target != null)
            transform.position = target.transform.position;

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            timer = 0;
            target.GetComponent<IDamageAble<float>>().Damage(GameManager.instance._damage * (ItemDataBase.instance.Variable(42) +
                                                            (ItemDataBase.instance.Variable2(42) * (GameManager.instance.FindItemCount(42) - 1))));
        }        
    }
}
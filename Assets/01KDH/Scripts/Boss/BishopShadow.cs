using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopShadow : MonoBehaviour
{
    float despawnT;
    private void Start()
    {
        despawnT = 0f;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 25f);
        DeSpawn();
        if (despawnT >= 4f)
        {
            gameObject.SetActive(false);
            despawnT = 0f;
        }
    }

    private void DeSpawn()
    {
        despawnT += Time.deltaTime;
    }
}

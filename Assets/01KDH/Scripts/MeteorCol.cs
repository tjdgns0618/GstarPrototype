using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorCol : MonoBehaviour
{
    float meteorT;
    public int meteorType;
    public GameObject mCol;
    private void OnEnable()
    {
        mCol.SetActive(false);
        meteorT = 0f;
    }

    private void Update()
    {
        meteorT += Time.deltaTime;
        if (meteorType == 0)
        {
            if (meteorT > 5f)
            {
                mCol.SetActive(true);
            }
        }
        else
        {
            if (meteorT > 6f)
            {
                mCol.SetActive(true);
            }
        }
    }
}

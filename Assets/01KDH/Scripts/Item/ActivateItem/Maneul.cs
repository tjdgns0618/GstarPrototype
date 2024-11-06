using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maneul : MonoBehaviour        // 주변 적 지속피해
{
    public GameObject col;

    WaitForSeconds coltime;

    void Start()
    {
        coltime = new WaitForSeconds(0.5f);
    }

    private void OnEnable()
    {
        StartCoroutine(ColCorutine());
    }

    IEnumerator ColCorutine()
    {
        while (true)
        {
            col.SetActive(true);
            yield return coltime;
            col.SetActive(false);
            yield return coltime;
        }
    }
}

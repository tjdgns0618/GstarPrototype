using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject col;

    WaitForSeconds coltime;

    void Start()
    {
        coltime = new WaitForSeconds(0.3f);
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

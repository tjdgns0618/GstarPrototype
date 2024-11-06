using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrashFire : MonoBehaviour
{
    public GameObject[] fires;

    private void OnEnable()
    {
        StopCoroutine(FireTrigger());
        StartCoroutine(FireTrigger());
    }
    IEnumerator FireTrigger()
    {
        yield return new WaitForSeconds(0.55f);
        for(int i = 0; i < fires.Length; i++)
        {
            fires[i].SetActive(true);
            yield return new WaitForSeconds(0.05f);
        }
    }

}

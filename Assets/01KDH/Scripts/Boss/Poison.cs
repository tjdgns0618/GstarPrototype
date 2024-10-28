using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    private Vector3 _endpos;

    private void OnEnable()
    {
        _endpos = PlayerCharacter.Instance.transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _endpos, Time.deltaTime * 10f);
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Untagged"))
        {
            gameObject.SetActive(false);
        }
        if(col.gameObject.CompareTag("Player"))
        {

        }
    }
}

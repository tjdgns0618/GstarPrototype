using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardPheonix : MonoBehaviour
{
    public GameObject boxCollider;

    void Update()
    {
        boxCollider.transform.Translate(Vector3.forward * Time.deltaTime * 25f);
    }

    private void OnEnable()
    {
        Invoke("DisableCollider", 0.5f);
        boxCollider.transform.position = Vector3.zero;
        boxCollider.GetComponent<BoxCollider>().enabled = true  ;
    }

    public void DisableCollider()
    {
        boxCollider.GetComponent<BoxCollider>().enabled = false;
    }
    
}

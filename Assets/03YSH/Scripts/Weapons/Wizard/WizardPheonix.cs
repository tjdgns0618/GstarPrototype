using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardPheonix : MonoBehaviour
{
    public GameObject boxCollider;
    bool isEnable = false;

    void Update()
    {
        if(isEnable)
            boxCollider.transform.Translate(Vector3.forward * Time.deltaTime * 25f);
    }

    private void OnEnable()
    {
        // Invoke("DisableCollider", 0.5f);
        StartCoroutine(disableCollider());
        boxCollider.transform.position = Vector3.zero;
        boxCollider.GetComponent<BoxCollider>().enabled = true;
        isEnable = true;
    }

    IEnumerator disableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        boxCollider.GetComponent<BoxCollider>().enabled = false;
    }

    public void DisableCollider()
    {
        boxCollider.GetComponent<BoxCollider>().enabled = false;
        isEnable = false;
    }
    
}

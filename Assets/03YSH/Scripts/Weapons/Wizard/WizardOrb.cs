using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardOrb : MonoBehaviour
{
    void Update()
    {
        transform.position = PlayerCharacter.Instance.transform.position + Vector3.up * 1f;
    }
}

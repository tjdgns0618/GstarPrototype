using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallBackPos : MonoBehaviour
{
    public GameObject _particle;
    private void OnParticleSystemStopped()
    {
        transform.parent.gameObject.SetActive(false);
    }
}

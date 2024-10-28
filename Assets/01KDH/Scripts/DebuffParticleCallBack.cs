using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffParticleCallBack : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        transform.parent.SetParent(null);
        transform.parent.gameObject.SetActive(false);
    }
}

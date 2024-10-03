using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCalllback : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        Destroy(transform.parent.gameObject);
    }
}

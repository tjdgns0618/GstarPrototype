using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingnalScript : MonoBehaviour
{
    public bool startMove = false;
    public float animSpeed = 2;

    private void Update()
    {
        if (startMove)
        {
            transform.position += transform.forward * Time.deltaTime * animSpeed;
        }
    }

    public void StartMove()
    {
        startMove = true;
    }

    public void TimeScaleChange(float scale)
    {
        Time.timeScale = scale;
    }

}

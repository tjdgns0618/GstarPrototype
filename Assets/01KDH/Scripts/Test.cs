using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    GameManager gm;
    void Start()
    {
        gm = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X)) 
        Debug.Log((ItemDataBase.instance.Variable2(31) * (gm.FindItemCount(31) - 1)) + "aaaaaa");
    }
}

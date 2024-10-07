using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_5 : MonoBehaviour
{ 

    public GameObject player;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            GameObject missile = ItemPoolManager.instance.Get(1);

            missile.transform.position = player.transform.position;
        }
        if(player==null)
            Debug.LogError("Player reference is not assigned!"); 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_11 : ItemEnemy
{
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            //enemy._movementSpeed = 0f;
        }
    }
}

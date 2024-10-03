using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class TestP : MonoBehaviour
{
    private void Start()
    {
        float itemVariable = ItemDataBase.instance.Variable(0);
        float itemVariable2 = ItemDataBase.instance.Variable(40);
        float itemVariable3 = ItemDataBase.instance.Variable(41);

        Debug.Log(itemVariable);
        Debug.Log(itemVariable2);
        Debug.Log(itemVariable3);
    }
}

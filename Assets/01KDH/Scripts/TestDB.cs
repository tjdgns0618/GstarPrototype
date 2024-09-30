using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDB : MonoBehaviour
{
    public PassiveItemDB testDB;

    private void Start()
    {
            Debug.Log(testDB.Sheet1[1].itemID + testDB.Sheet1[1].itemVariable);
    }
}

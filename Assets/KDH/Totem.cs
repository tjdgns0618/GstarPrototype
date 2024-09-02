using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour                                  // 토템에 상호작용 시 토템의 종류에 따라 적용되는 효과 및 재화
{
    public GameObject _bloodTotem;
    public GameObject _testTotem1;
    public GameObject _testTotem2;

    public float _devilWeight = 0.05f;
    public float _angelWeight = 0.05f;
    public float _bloodWeight = 0.5f;
    public float _test1Weight = 0.3f;
    public float _test2Weight = 0.2f;

    public void BloodTotem(int _costHp)
    {
        if (GameManager.instance._hp > _costHp)
        {
            GameManager.instance._hp -= _costHp;
            GameManager.instance._gold += 2000;
            _bloodTotem.SetActive(false);
        }
        else
        {
            Debug.Log("피없어");

         }
    }

    public void TestTotem1()
    {
        // 추후 추가될 토템
    }

    public void TestTotem2()
    {
        // 추후 추가될 토템
    }
}

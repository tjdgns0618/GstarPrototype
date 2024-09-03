using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour                                  // ���ۿ� ��ȣ�ۿ� �� ������ ������ ���� ����Ǵ� ȿ�� �� ��ȭ
{
    public enum TotemType
    {
        Blood,
        test1,
        test2
    }

    public enum RareTotemType
    {
        Devil,
        Angel
    }

    public GameObject _devilTotem;
    public GameObject _angelTotem;
    public GameObject _bloodTotem;
    public GameObject _testTotem1;
    public GameObject _testTotem2;

    public float _devilWeight = 0.5f;
    public float _angelWeight = 0.5f;
    public float _bloodWeight = 0.5f;
    public float _test1Weight = 0.3f;
    public float _test2Weight = 0.2f;

    public void BloodTotem(int _costHp)
    {
        if (GameManager.instance._hp > _costHp)
        {
            GameManager.instance._hp -= _costHp;
            GameManager.instance._gold += 2000;
            //_bloodTotem.SetActive(false);
        }
        else
        {
            Debug.Log("�Ǿ���");

         }
    }

    public void TestTotem1()
    {
        // ���� �߰��� ����
    }

    public void TestTotem2()
    {
        // ���� �߰��� ����
    }

    public GameObject GetTotemObject(TotemType type)
    {
        switch(type)
        {
            case TotemType.Blood:
                return _bloodTotem;
            case TotemType.test1:
                return _testTotem1;
            case TotemType.test2:
                return _testTotem2;
            default:
                return null;
        }
    }
    public GameObject GetRareTotemObject(RareTotemType type)
    {
        switch (type)
        {
            case RareTotemType.Devil:
                return _devilTotem;
            case RareTotemType.Angel:
                return _angelTotem;
            default:
                return null;
        }
    }
}

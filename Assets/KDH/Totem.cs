using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour                                  // ���ۿ� ��ȣ�ۿ� �� ������ ������ ���� ����Ǵ� ȿ�� �� ��ȭ
{
    public enum TotemType
    {
        BloodT,
        HealT,
        DiceT
    }

    public enum RareTotemType
    {
        DevilT,
        AngelT
    }

    public GameObject _devilTotem;
    public GameObject _angelTotem;
    public GameObject _bloodTotem;
    public GameObject _healTotem;
    public GameObject _diceTotem;

    public float _devilWeight = 0.5f;
    public float _angelWeight = 0.5f;
    public float _bloodWeight = 0.4f;
    public float _healWeight = 0.5f;
    public float _diceWeight = 0.1f;

    public void BloodTotem(int _costHp)
    {
        if (GameManager.instance._hp > _costHp)
        {
            GameManager.instance._hp -= _costHp;
            GameManager.instance._gold += 2000;
        }
        else
        {
            Debug.Log("�Ǿ���");
         }
    }

    public void HealTotem()
    {
        GameManager.instance._hp = 100;
    }

    public void DiceTotem()
    {
        int rollNum = UnityEngine.Random.Range(1, 7);
        switch(rollNum)
        {
            case 1:
                GameManager.instance._damage -= 4f;
                Debug.Log("������ ���� ������ ������ ���.");
                return;
            case 2:
                GameManager.instance._attackspeed -= 0.2f;
                GameManager.instance._cooldown -= 2f;
                Debug.Log("���� ������ �� ����.");
                 return;
                case 3:
                Debug.Log("�ƹ� �ϵ� �Ͼ�� �ʾҴ�.");
                return;
            case 4:
                GameManager.instance._attackspeed += 0.3f;
                GameManager.instance._cooldown += 4f;
                Debug.Log("���� ����������.");
                return;
            case 5:
                GameManager.instance._damage += 6f;
                Debug.Log("���� ��������.");
                return;
            case 6:
                GameManager.instance._attackspeed += 0.5f;
                GameManager.instance._cooldown += 6f;
                GameManager.instance._damage += 10f;
                Debug.Log("���� ���� ������� ����� ���.");
                return;
            default:
                return;

        }
    }

    public GameObject GetTotemObject(TotemType type)
    {
        switch(type)
        {
            case TotemType.BloodT:
                return _bloodTotem;
            case TotemType.HealT:
                return _healTotem;
            case TotemType.DiceT:
                return _diceTotem;
            default:
                return null;
        }
    }
    public GameObject GetRareTotemObject(RareTotemType type)
    {
        switch (type)
        {
            case RareTotemType.DevilT:
                return _devilTotem;
            case RareTotemType.AngelT:
                return _angelTotem;
            default:
                return null;
        }
    }
}

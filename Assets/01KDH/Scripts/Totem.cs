using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour                                  // 토템에 상호작용 시 토템의 종류에 따라 적용되는 효과 및 재화
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

    GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }

    public void BloodTotem(int _costHp)
    {
        if (gm._hp > _costHp)
        {
            gm._hp -= _costHp;
            gm._gold += 2000;
        }
        else
        {
            Debug.Log("피없어");
         }
    }

    public void HealTotem()
    {
        gm._hp = gm._maxhp;
    }

    public void DiceTotem()
    {
        int rollNum = UnityEngine.Random.Range(1, 7);
        switch(rollNum)
        {
            case 1:
                gm._damage -= 4f;
                Debug.Log("무기의 날이 무뎌진 느낌이 든다.");
                return;
            case 2:
                gm._attackspeed -= 0.2f;
                gm._cooldown -= 2f;
                Debug.Log("몸이 둔해진 것 같다.");
                 return;
                case 3:
                Debug.Log("아무 일도 일어나지 않았다.");
                return;
            case 4:
                gm._attackspeed += 0.3f;
                gm._cooldown += 4f;
                Debug.Log("몸이 가벼워졌다.");
                return;
            case 5:
                gm._damage += 6f;
                Debug.Log("힘이 강해졌다.");
                return;
            case 6:
                gm._attackspeed += 0.5f;
                gm._cooldown += 6f;
                gm._damage += 10f;
                Debug.Log("신의 힘에 가까워진 기분이 든다.");
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

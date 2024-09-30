using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    public delegate void PassiveItemDelegate();
    public PassiveItemDelegate passiveDelegate;

    GameManager gm;
    private void Start()
    {
        gm = GameManager.instance;
    }

    public void _01Pitem()
    {
        gm._cooldown -= 0.08f;
    }   
    public void _02Pitem()
    {
        gm._damage += 8f;
    }   
    public void _03Pitem()
    {
        gm._attackspeed += 0.08f;
    }  
    public void _04Pitem()
    {
        gm._movespeed += 0.04f;
    } 
    public void _05Pitem()
    {
        gm._maxhp += 10f;
    }   
    public void _06Pitem()
    {
        gm._critdmg += 5f;
    }
    public void _07Pitem()
    {
        gm._critchance += 5f;
    } 
    public void _08Pitem()
    {
        gm._damage += 5f;
        gm._attackspeed += 0.03f;
        gm._critchance += 3f;
        gm._maxhp += 5f;
    } 
    public void _09Pitem()
    {
        gm._dashcount += 1;
    }  
    public void _10Pitem()
    {
        gm._range = 0.1f;
    }
    public void _11Pitem()
    {
        gm._damage += 30f;
        gm._attackspeed -= 0.05f;
        gm._cooldown += 0.05f;
    }    
    public void _12Pitem()
    {
        gm._damage -= 25f;
        gm._attackspeed += 0.1f;
        gm._cooldown -= 0.07f;
    }   
    public void _13Pitem()
    {
        gm._skillcount += 1;
    }   
    public void _14Pitem()
    {
        gm._ultcount += 1;
    }   
    public void _15Pitem()
    {
        // ���� ������ ��
        gm._damage += 3f;
        gm._attackspeed += 0.02f;
        gm._cooldown += 0.02f;
    } 
    public void _16Pitem()
    {
        gm._damage += 40f;
        gm._maxhp -= 30f;
    }  
    public void _17Pitem()
    {
        gm._maxhp -= 20f;
        // gm._goldbonus += 0.15f;
    }   
    public void _18Pitem()
    {
        gm._maxhp -= 20f;
        gm._lifesteal += 0.15f;
    }  
    public void _19Pitem()
    {
        gm._maxhp += 30f;
        gm._movespeed -= 0.12f;
    }   
    public void _20Pitem()
    {
        gm._movespeed += 0.3f;
        // ��� ��� x
    }
    public void _21Pitem()
    {
        // ũ�� Ȯ�� ����
        // ũ�� +50%
    }    
    public void _22Pitem()
    {
        // ��ų ���� Ȯ�� ������ �ٸ� ��ų �� 4% ����
    }   
    public void _23Pitem()
    {
        gm._lifegen += 1f;
    }   
    public void _24Pitem()
    {
        // 30�� ���� �ִ�ü�� 50% ~ 200%
    }   
    public void _25Pitem()
    {
        // 30�� ���� ���������� 20% or 500%
    }
    public void _26Pitem()
    {
        // óġ�� �ִ� 100
        gm._maxhp = 1f;
    }    
    public void _27Pitem()
    {
        // óġ�� �ִ� 50
        gm._attackspeed += 0.05f;
    }    
    public void _28Pitem()
    {
        // óġ�� �ִ�100
        gm._damage += 1f;
    }   
    public void _29Pitem()
    {
        gm._revivecount += 1;
    }
}

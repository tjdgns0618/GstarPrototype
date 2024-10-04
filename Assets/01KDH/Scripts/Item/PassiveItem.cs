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
        gm._cooldown -= ItemDataBase.instance.Variable(0);
    }   
    public void _02Pitem()
    {
        gm._damage += ItemDataBase.instance.Variable(1);
    }   
    public void _03Pitem()
    {
        gm._attackspeed += ItemDataBase.instance.Variable(2);
    }  
    public void _04Pitem()
    {
        gm._movespeed += ItemDataBase.instance.Variable(3);
    } 
    public void _05Pitem()
    {
        gm._maxhp += ItemDataBase.instance.Variable(4);
    }   
    public void _06Pitem()
    {
        gm._critdmg += ItemDataBase.instance.Variable(5);
    }
    public void _07Pitem()
    {
        gm._critchance += ItemDataBase.instance.Variable(6);
    } 
    public void _08Pitem()
    {
        gm._damage += ItemDataBase.instance.Variable(7);
        gm._attackspeed += ItemDataBase.instance.Variable2(7);
        gm._critchance += ItemDataBase.instance.Variable3(7);
        gm._maxhp += ItemDataBase.instance.Variable4(7);
    } 
    public void _09Pitem()
    {
        gm._dashcount += (int)ItemDataBase.instance.Variable(8);
    }  
    public void _10Pitem()
    {
        gm._range = ItemDataBase.instance.Variable(9);
    }
    public void _11Pitem()
    {
        gm._damage += ItemDataBase.instance.Variable(10);
        gm._attackspeed -= ItemDataBase.instance.Variable2(10);
        gm._cooldown += ItemDataBase.instance.Variable3(10);
    }    
    public void _12Pitem()
    {
        gm._damage -= ItemDataBase.instance.Variable(11);
        gm._attackspeed += ItemDataBase.instance.Variable2(11);
        gm._cooldown -= ItemDataBase.instance.Variable3(11);
    }   
    public void _13Pitem()
    {
        gm._skillcount += ItemDataBase.instance.Variable(12);
    }   
    public void _14Pitem()
    {
        gm._ultcount += ItemDataBase.instance.Variable(13);
    }   
    public void _15Pitem()
    {
        // 토템 무시할 때
        gm._damage += ItemDataBase.instance.Variable(14);
        gm._attackspeed += ItemDataBase.instance.Variable2(14);
        gm._cooldown += ItemDataBase.instance.Variable3(14);
    } 
    public void _16Pitem()
    {
        gm._damage += ItemDataBase.instance.Variable(15);
        gm._maxhp -= ItemDataBase.instance.Variable2(15);
    }  
    public void _17Pitem()
    {
        gm._maxhp -= ItemDataBase.instance.Variable(16);
        // gm._goldbonus += 0.15f;
    }   
    public void _18Pitem()
    {
        gm._maxhp -= ItemDataBase.instance.Variable(17);
        gm._lifesteal += ItemDataBase.instance.Variable(17);
    }  
    public void _19Pitem()
    {
        gm._maxhp += ItemDataBase.instance.Variable(18);
        gm._movespeed -= ItemDataBase.instance.Variable2(18);
    }   
    public void _20Pitem()
    {
        gm._movespeed += ItemDataBase.instance.Variable(19);
        // 대시 사용 x
    }
    public void _21Pitem()
    {
        // 크리 확률 고정
        // 크뎀 +50%
    }    
    public void _22Pitem()
    {
        // 스킬 사용시 확률 적으로 다른 스킬 쿨 4% 감소
    }   
    public void _23Pitem()
    {
        gm._lifegen += ItemDataBase.instance.Variable(22);
    }   
    public void _24Pitem()
    {
        // 30초 마다 최대체력 50% ~ 200%
    }   
    public void _25Pitem()
    {
        // 30초 마다 최종데미지 20% or 500%
    }
    public void _26Pitem()
    {
        // 처치시 최대 100
        gm._maxhp = ItemDataBase.instance.Variable(25);
    }    
    public void _27Pitem()
    {
        // 처치시 최대 50
        gm._attackspeed += ItemDataBase.instance.Variable(26);
    }    
    public void _28Pitem()
    {
        // 처치시 최대100
        gm._damage += ItemDataBase.instance.Variable(27);
    }   
    public void _29Pitem()
    {
        gm._revivecount += ItemDataBase.instance.Variable(28);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    public delegate void PassiveItemDelegate();
    public PassiveItemDelegate passiveDelegate;

    private void Start()
    {
        passiveDelegate += _01Pitem;
        passiveDelegate += _02Pitem;
        passiveDelegate();
    }

    public void _01Pitem()
    {
        GameManager.instance._cooldown -= 0.08f;
    }   
    public void _02Pitem()
    {
        GameManager.instance._damage += 8f;
    }   
    public void _03Pitem()
    {
        GameManager.instance._attackspeed += 0.08f;
    }  
    public void _04Pitem()
    {
        GameManager.instance._movespeed += 0.04f;
    } 
    public void _05Pitem()
    {
        GameManager.instance._maxhp += 10f;
    }   
    public void _06Pitem()
    {
        GameManager.instance._critdmg += 5f;
    }  
    public void _07Pitem()
    {
        GameManager.instance._critchance += 5f;
    } 
    public void _08Pitem()
    {
        GameManager.instance._damage += 5f;
        GameManager.instance._attackspeed += 0.03f;
        GameManager.instance._critchance += 3f;
        GameManager.instance._maxhp += 5f;
    } 
    public void _09Pitem()
    {
        GameManager.instance._dashcount += 1;
    }  
    public void _10Pitem()
    {
        GameManager.instance._range = 0.1f;
    }
    public void _11Pitem()
    {
        GameManager.instance._damage += 30f;
        GameManager.instance._attackspeed -= 0.05f;
        GameManager.instance._cooldown += 0.05f;
    }    
    public void _12Pitem()
    {

    }   
    public void _13Pitem()
    {

    }   
    public void _14Pitem()
    {

    }   
    public void _15Pitem()
    {

    } 
    public void _16Pitem()
    {

    }  
    public void _17Pitem()
    {

    }   
    public void _18Pitem()
    {

    }  
    public void _19Pitem()
    {

    }   
    public void _20Pitem()
    {

    }
    public void _21Pitem()
    {

    }    
    public void _22Pitem()
    {

    }   
    public void _23Pitem()
    {

    }   
    public void _24Pitem()
    {

    }   
    public void _25Pitem()
    {

    }    
    public void _26Pitem()
    {

    }    
    public void _27Pitem()
    {

    }    
    public void _28Pitem()
    {

    }   
    public void _29Pitem()
    {

    }

}

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

    }   
    public void _02Pitem()
    {

    }   
    public void _03Pitem()
    {

    }  
    public void _04Pitem()
    {

    } 
    public void _05Pitem()
    {

    }   
    public void _06Pitem()
    {

    }  
    public void _07Pitem()
    {

    } 
    public void _08Pitem()
    {

    } 
    public void _09Pitem()
    {

    }  
    public void _10Pitem()
    {

    }
    public void _11Pitem()
    {

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

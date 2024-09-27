using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ActiveItem : MonoBehaviour
{
    //public Item _aitem;

    //public LayerMask enemyM;

    //WaitForSeconds _08item;
    //WaitForSeconds _09item;
    //WaitForSeconds _10item;
    //WaitForSeconds _11item;
    //WaitForSeconds _12item;

    //public GameObject _maneulObj;

    private void Start()
    {
        StartCoroutine(LateStart());
        //_08item = new WaitForSeconds(15f);
        //_09item = new WaitForSeconds(30f);
        //_10item = new WaitForSeconds(10f);
        //_11item = new WaitForSeconds(60f);
        //_12item = new WaitForSeconds(15f);
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(1f);
        PlayerCharacter.Instance.weaponManager.Weapon.ItemChance += _06Item;
    }

    void _01Item()
    {
        
    }    
    void _02Item()
    {
        
    }    
    void _03Item()
    {
        
    }    
    void _04Item()
    {
        
    }    
    void _05Item()
    {
        
    }    
    public void _06Item()
    {
        Debug.Log("asdfasfdfsafasfdsaf");
        Transform firework = GameManager.instance.itempools.Get(1).transform;
        firework.parent = transform;
        firework.transform.position = PlayerCharacter.Instance.transform.position + new Vector3(0, 3, 0);
    }    
    void _07Item()
    {
        
    }    
    void _08Item()
    {
        //_maneulObj.SetActive(true);
    }    
    void _09Item()
    {
        
    }   
    void _10Item()
    {
        
    } 
    void _11Item()
    {
        
    }  
    void _12Item()
    {
    }  
    void _13Item()
    {

    }    
    void _14Item()
    {
        
    }   
    void _15Item()
    {
        
    }  
    void _16Item()
    {
        
    }    
    void _17Item()
    {
        
    }   
    void _18Item()
    {
        
    } 
    void _19Item()
    {
        
    }   
    void _20Item()
    {
        
    }
}

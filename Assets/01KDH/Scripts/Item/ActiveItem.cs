using DuloGames.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.Progress;

public class ActiveItem : MonoBehaviour
{

    public float percentTotal = 100f;

    public int _itemcount;
    private Item _item;

    public InvenSlot[] slots;

    //public Item _aitem;

    //public LayerMask enemyM;

    //WaitForSeconds _08item;
    //WaitForSeconds _09item;
    //WaitForSeconds _10item;
    //WaitForSeconds _11item;
    //WaitForSeconds _12item;

    //public GameObject _maneulObj;

    GameManager gm;

    public Dictionary<string, GameObject> effectDictionary;
    public GameObject[] effectObjects;

    private void Start()
    {
        gm = GameManager.instance;

        foreach (var item in effectObjects)
        {
            effectDictionary.Add(item.name, item);
        }
        //_08item = new WaitForSeconds(15f);
        //_09item = new WaitForSeconds(30f);
        //_10item = new WaitForSeconds(10f);
        //_11item = new WaitForSeconds(60f);
        //_12item = new WaitForSeconds(15f);
    }

    private void Update()
    {

    }
    void Test()
    {
        GameObject targetObj;
        if(effectDictionary.TryGetValue("object name", out targetObj))
        {

        }
    }

    public Item FindItemData(int id)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                if (slots[i].item.itemID == id)
                {
                    _item = slots[i].item;
                }
            }
        }
        return _item;
    }

    public int FindItemCount(int id)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                if (slots[i].item.itemID == id)
                {
                    _itemcount = slots[i].itemCount;
                }
            }
        }
        return _itemcount;
    }

    public void ActiveItemEffect()
    {

    }
    
    #region effect
    void _01Item()
    {
        // ������ ���̵� 30���� ������ ����Ÿ ã��
        if(GetRandomOutcome(FindItemData(29), FindItemCount(29)))
        {
            Debug.Log("asdf");
        }
        else
        {
            Debug.Log("Failed");
        }
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
        Transform firework = gm.itempools.Get(1).transform;
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
    #endregion

    public bool GetRandomOutcome(Item item, float itemcount)
    { 
        float aProbability = ItemDataBase.instance.Variable(item.itemID) * itemcount;
        float bProbability = percentTotal - ItemDataBase.instance.Variable(item.itemID) * itemcount;

        // 0���� 100 ������ ���� ���� ����
        float randomValue = Random.Range(0f, percentTotal);
        if (randomValue < aProbability)
        {
            return true; // a�� ���õ�
        }
        else
        {
            return false; // b�� ���õ�
        }
    }
}

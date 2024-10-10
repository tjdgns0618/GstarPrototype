using DuloGames.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.Progress;

public class ActiveItem : MonoBehaviour
{
    public int _itemcount;
    private Item _item;

    public InvenSlot[] slots;

    // 공격시
    public float attackRandomValue;
    public float attackProbability;

    //  피격시
    public float hitRandomValue;
    public float hitProbability;

    // 처치시
    public float killRandomValue;
    public float killProbability;

    GameManager gm;

    public Dictionary<string, GameObject> effectDictionary;
    public GameObject[] effectObjects;

    #region 
    public ChainLightning chainLightning;
    public Item_09 _09item;
    #endregion

    private void Start()
    {
        gm = GameManager.instance;

        foreach (var item in effectObjects)
        {
            effectDictionary.Add(item.name, item);
        }
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
    public void _01Item()   //  처치 시 체력회복 오브 드랍 
    {
        if(GetKillRandom(29))
        {

        }

    }
    public void _02Item()   // 처치 시 적 폭발
    {
        if(GetKillRandom(30))
        {

        }
    }    
    public void _03Item()   // 적 피격 시 폭탄 부착
    {
        if(GetHitRandom(31))
        {

        }
    }    
    public void _04Item()   // 적 피격 시 지뢰 설치
    {
        if (GetHitRandom(32))
        {

        }
    }    
    public void _05Item()   // 플레이어 피격 시 공격 방향 반사 범위 피해
    {
        if (GetHitRandom(33))
        {

        }
    }    
    public void _06Item()   // 플레이어 피격 시 미사일 발사
    {
        if (GetHitRandom(34))
        {

        }
        //Transform firework = gm.itempools.Get(1).transform;
        //firework.parent = transform;
        //firework.transform.position = PlayerCharacter.Instance.transform.position + new Vector3(0, 3, 0);
    }    
    public void _07Item()   // 플레이어 피격 시 랜덤 효과 발동
    {
        if (GetHitRandom(35))
        {

        }
    }    
    public void _08Item()   // 일정 시간마다 마늘 효과
    {

    }    
    public void _09Item()   // 방패 공전
    {
        _09item.UseItem();
    }   
    public void _10Item()   // 일정 시간마다 적이 있는 곳에 장판
    {
        if (GetHitRandom(38))
        {

        }
    } 
    public void _11Item()   // 일정 시간마다 주변 적 정지
    {
        
    }  
    public void _12Item()   // 일정 시간마다 짧은 무적
    {
    }  
    public void _13Item()   // 공격 시 연쇄 번개
    {
        if(GetAttackRandom(41))
        {
            chainLightning.UseItem();
        }
    }    
    public void _14Item()   //  공격 시 범위 피격
    {
        if (GetAttackRandom(42))
        {

        }
    }   
    public void _15Item()   // 공격 시 범위 장판 
    {
        if (GetAttackRandom(43))
        {

        }
    }  
    public void _16Item()   // 공격 시 투사체 발사
    {
        if (GetAttackRandom(44))
        {

        }
    }    
    public void _17Item()   // 공격 시 부메랑 발사
    {
        if (GetAttackRandom(45))
        {

        }
    }   
    public void _18Item()   // 캐릭터 교체 시 범위 데미지
    {

    } 
    public void _19Item()   // 바라보는 방향 적 슬로우
    {
        
    }   
    public void _20Item()   // 공격 시 튕기는 투사체 발사
    {
        if (GetAttackRandom(48))
        {

        }
    }
    #endregion

    public bool GetAttackRandom(int id)
    {
        attackRandomValue = Random.Range(1f, 101f);  //1~100 (95~100)
        attackProbability = 100f - ItemDataBase.instance.Variable(id) * FindItemCount(id); 
        if (attackRandomValue >= attackProbability)
        {
            Debug.Log("Attack True");
            return true;
        }
        else
        {
            Debug.Log("Attack False");
            return false;
        }
     }

    public bool GetHitRandom(int id)
    {
        hitRandomValue = Random.Range(1f, 101f);  //1~100 (95~100)
        hitProbability = 100f - ItemDataBase.instance.Variable(id) * FindItemCount(id);
        if (hitRandomValue >= hitProbability)
        {
            Debug.Log("Hit True");
            return true;
        }
        else
        {
            Debug.Log("Hit False");
            return false;
        }
    }

    public bool GetKillRandom(int id)
    {
        killRandomValue = Random.Range(1f, 101f);  //1~100 (95~100)
        killProbability = 100f - ItemDataBase.instance.Variable(id) * FindItemCount(id);
        if (killRandomValue >= killProbability)
        {
            Debug.Log("Kill True");
            return true;
        }
        else
        {
            Debug.Log("Kill False");
            return false;
        }
    }
}

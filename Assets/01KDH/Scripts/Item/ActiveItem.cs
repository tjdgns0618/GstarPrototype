using DuloGames.UI;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem : MonoBehaviour
{
    private Item _item;

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

    public spawner1 spawner;
    public GameObject _player;

    #region 
    public ChainLightning chainLightning;
    public Item_09 _09item;
    #endregion

    private void Start()
    {
        gm = GameManager.instance;
    }

    #region effect
    public void _01Item(Transform transform)   //  처치 시 체력회복 오브 드랍 
    {
        //Debug.Log("01 힐링오브");
        //if (GetKillRandom(29))
        //{
            GameObject particle = gm.particlePoolManager.GetParticle("Healingorb");
            if (particle != null)
            {
                particle.transform.position = transform.position;
            }
        //}
    }
    public void _02Item(Transform transform)   // 처치 시 적 폭발
    {
        GameObject particle = gm.particlePoolManager.GetParticle("Deathboom");
        if (particle != null)
            {
                particle.transform.position = transform.position;
            }
    }

    public void _03Item()   // 적 피격 시 폭탄 부착
    {
        if (spawner.enemies.Count != 0)
        {
            GameObject particle = gm.particlePoolManager.GetParticle("ShurikenStorm");
            particle.transform.position = PlayerCharacter.Instance.transform.position + Vector3.up * 0.1f;
        }
    }

    public void _04Item(Transform transform)   // 적 피격 시 지뢰 설치
    {
        if (GetHitRandom(32))
        {

        }
    }

    public void _05Item(Transform transform)   // 플레이어 피격 시 공격 방향 반사 범위 피해
    {
        if (GetHitRandom(33))
        {

        }
    }

    public void _06Item()   // 공격 시 미사일 발사
    {
        if (GetHitRandom34(34))
        {
            if (spawner.enemies.Count > 0)
            {
                GameObject particle = gm.particlePoolManager.GetParticle("MissileUp");
                GameObject particle2 = gm.particlePoolManager.GetParticle("MissileDown");
                 if (particle != null && particle2 != null)
                 {
                     particle.transform.position = PlayerCharacter.Instance.transform.position;
                     particle2.transform.position = spawner.enemies[Random.Range(0, spawner.enemies.Count)].transform.position;
                 }
            }
        }
    }

    public void _07Item(Transform transform)   // 플레이어 피격 시 랜덤 효과 발동
    {
        if (GetHitRandom(35))
        {

        }
    }
    public void _08Item()   // 일정 시간마다 마늘 효과
    {
        GameObject particle = gm.particlePoolManager.GetParticle("Maneul");
        if (particle != null)
        {
            particle.transform.position = PlayerCharacter.Instance.transform.position;
            particle.transform.SetParent(_player.transform);
        }
    }
    public void _09Item()   // 방패 공전
    {
        _09item.UseItem();
    }
    public void _10Item()   // 일정 시간마다 적이 있는 곳에 장판
    {
        if(spawner.enemies.Count != 0)
        {
        GameObject particle = gm.particlePoolManager.GetParticle("Fire");
        if (particle != null)
            {
                if (spawner.enemies.Count > 0)
                {
                   particle.transform.position = spawner.enemies[Random.Range(0, spawner.enemies.Count)].transform.position;
                }
            }
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
        if (GetAttackRandom(41))
        {
            chainLightning.UseItem();
        }
    }
    public void _14Item(Transform transform)   //  공격 시 범위 피격
    {
        // if (GetAttackRandom(42))
        Debug.Log("14번 아이템");
        {
            GameObject particle = gm.particlePoolManager.GetParticle("BladeStorm");
            if (particle != null)
            {
                particle.transform.SetParent(transform);
                particle.transform.localPosition = Vector3.zero;
            }
        }
    }
    public void _15Item()   // 공격 시 범위 장판 
    {
        if (GetAttackRandom(43))
        {
            GameObject particle = gm.particlePoolManager.GetParticle("FireWork5");
            if (particle != null)
            {
                particle.transform.position = PlayerCharacter.Instance.transform.position + Vector3.up * 2f;
            }
        }
    }
    public void _16Item()   // 공격 시 투사체 발사
    {
        if (GetAttackRandom(44))
        {
            GameObject particle = gm.particlePoolManager.GetParticle("Wind");
            if (particle != null)
            {
                particle.transform.position = PlayerCharacter.Instance.firePoint.transform.position;
                particle.transform.rotation = PlayerCharacter.Instance.transform.rotation;
            }
        }
    }
    public void _17Item()   // 공격 시 부메랑 발사
    {
        if (GetAttackRandom(45))
        {
            GameObject particle = gm.particlePoolManager.GetParticle("Boomerang");
            if (particle != null)
            {
                particle.transform.position = PlayerCharacter.Instance.firePoint.transform.position;
                particle.transform.rotation = PlayerCharacter.Instance.transform.rotation;
            }
        }
    }
    public void _18Item()   // 캐릭터 교체 시 범위 데미지
    {

    }
    public void _19Item()   // 일정 시간마다 주변 적 슬로우
    {
        if (spawner.enemies.Count != 0)
        {
            GameObject particle = gm.particlePoolManager.GetParticle("Freeze");
            if (particle != null)
            {
                if (spawner.enemies.Count > 0)
                {
                    particle.transform.position = PlayerCharacter.Instance.transform.position;
                }
            }
        }
    }
    public void _20Item()   // 일정 시간마다 캐릭터 위치에서 팝콘이 터짐
    {
        if (spawner.enemies.Count != 0)
        {
            GameObject particle = gm.particlePoolManager.GetParticle("Popcorn");
            particle.transform.position = PlayerCharacter.Instance.transform.position + Vector3.up * 0.1f;
        }
    }
    #endregion

    public bool GetAttackRandom(int id)
    {
        attackRandomValue = Random.Range(1f, 101f);  //1~100 (95~100)
        attackProbability = 100f - ItemDataBase.instance.Variable(id) * gm.FindItemCount(id);
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
        hitProbability = 100f - ItemDataBase.instance.Variable(id) * gm.FindItemCount(id);
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
        killProbability = 100f - ItemDataBase.instance.Variable(id) * gm.FindItemCount(id);
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

    public bool GetHitRandom34(int id)
    {
        hitRandomValue = Random.Range(1f, 101f);  //1~100
        hitProbability = 100f - (ItemDataBase.instance.Variable(id) + (gm.FindItemCount(id) - 1) * 2f);
        if (hitRandomValue >= hitProbability)
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
}

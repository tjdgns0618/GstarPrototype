using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PoolManager pools;
    public ItemPoolManager itempools;

    public TMP_Text txt_gold;

    public float _maxhp = 100f;
    public float _currenthp = 100f;
    public float _hp = 100f;
    public int _gold = 0;

    public float _damage = 10f;
    public float _range = 1f;
    public float _cooldown = 0;
    public float _attackspeed = 1f;
    public float _movespeed = 1f;

    public float _critchance = 10f;
    public float _critdmg = 150f;

    public float _lifesteal = 0f;
    public float _lifegen = 0f;

    public int _dashcount = 1;
    public int _skillcount = 1;
    public int _ultcount = 1;

    public int _revivecount = 0;

    public bool isHit = false;  // 플레이어가 피격했는지 확인하는 변수
    public bool isPause = false;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        //txt_gold = _gold;
    }
}

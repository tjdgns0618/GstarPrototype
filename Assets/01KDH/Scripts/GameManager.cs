using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public InvenSlot[] slots;
    public ParticlePoolManager particlePoolManager;
    public UIManager uiManager;
    public CameraManager cameraManager;
    public EnemyPoolManager enemyPoolManager;
    public ItemPoolManager itempools;
    public CooltimeManager cooltimeManager;

    public TMP_Text txt_gold;

    public float _maxhp = 100f;
    public float _hp = 100f;
    public int _gold = 0;

    public float _damage = 10f;
    public float _range = 1f;
    public float _cooldown = 0;
    public float _changeCooldown = 3f;
    public float _attackspeed = 1f;
    public float _movespeed = 1f;
    public float _reInputTime = 1f;

    public float _critchance = 10f;
    public float _critdmg = 150f;

    public float _lifesteal = 0f;
    public float _lifegen = 0f;

    public int _dashcount = 2;
    public float _skillcount = 1;
    public float _ultcount = 1;

    public float _revivecount = 0;

    public bool isHit = false;  // 플레이어가 피격했는지 확인하는 변수
    public bool isPause = false;
    public bool isDead = false;

    private int _itemcount;

    public delegate void PlayerAttackDelegate();
    public PlayerAttackDelegate playerattackDelegate;

    public delegate void TimeActiveDelegate();   // 일정 시간마다 사용되는 아이템
    public TimeActiveDelegate timeactiveDelegate;

    public delegate void PlayerHitDelegate(Transform transform);    // 플레이어가 피격당할 때 사용되는 아이템
    public PlayerHitDelegate playerhitDelegate;

    public delegate void EnemyHitDelegate(Transform transform);   // 적이 피격당할 때 사용되는 아이템
    public EnemyHitDelegate enemyhitDelegate;

    public delegate void DieDelegate(Transform transform);   // 적이 사망할 때 사용되는 아이템
    public DieDelegate dieDelegate;

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

    private void Start()
    {
        PlayerCharacter.Instance.OnUpdateStat(_maxhp, _hp, _movespeed,_dashcount);

        //activeDelegate += Test2;
        playerhitDelegate += Test;
        playerattackDelegate += Test2;
        enemyhitDelegate += Test;
        dieDelegate += Test;
        timeactiveDelegate += Test2;
    }

    private void Update()
    {
        //txt_gold = _gold;
    }

    public void Heal(float heal)
    {
        Debug.Log(heal + "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb");
        float healValue = Mathf.Clamp(_hp + heal, _hp, _maxhp);
        _hp = healValue;
    }

    void Test(Transform transform)
    {
        return;
    }

    void Test2()
    {
        return;
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

    public void Test()
    {
        return;
    }
}

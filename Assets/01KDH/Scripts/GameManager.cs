using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public InvenSlot[] slots;
    public float[] cooltimes;
    public ParticlePoolManager particlePoolManager;
    public UIManager uiManager;
    public CameraManager cameraManager;
    public EnemyPoolManager enemyPoolManager;
    public ItemPoolManager itempools;
    public CooltimeManager cooltimeManager;
    public spawner1 spawner;

    public TMP_Text txt_gold;

    public float _maxhp;
    public float _hp;
    public float _gold;

    public float _goldBonus;

    public float _damage;
    public float _changeCooldown = 3f;
    public float _attackspeed = 1f;
    public float _movespeed = 1f;
    public float _reInputTime = 1f;

    public float _critchance = 10f;
    public float _critdmg = 1.5f;

    public float _lifegen;

    public float criticalRandomValue;
    public float criticalProbability;

    public float _skillCooltimePercent = 1f;
    [Header("대시 옵션")]
    [SerializeField, Tooltip("대쉬의 힘을 나타내는 값")]
    public float _dashPower = 3f;
    [SerializeField, Tooltip("대시 쿨타임")]
    public float _dashCool = 3f;

    public float _ultcount = 1;

    public float _revivecount = 0;

    public bool isHit = false;  // 플레이어가 피격했는지 확인하는 변수
    public bool isPause = false;
    public bool isDead = false;
    public bool isOnUI = false;

    private int _itemcount;

    public delegate void PlayerAttackDelegate();
    public PlayerAttackDelegate playerattackDelegate;

    public delegate void TimeActiveDelegate();   // 일정 시간마다 사용되는 아이템
    public TimeActiveDelegate timeactiveDelegate;

    public delegate void PlayerHitDelegate(Transform transform);    // 플레이어가 피격당할 때 사용되는 아이템
    public PlayerHitDelegate playerhitDelegate;

    public delegate void EnemyHitDelegate(GameObject target);   // 적이 피격당할 때 사용되는 아이템
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
        PlayerCharacter.Instance.OnUpdateStat(_maxhp, _hp, _movespeed);

        playerhitDelegate += Test;
        playerattackDelegate += Test2;
        enemyhitDelegate += Test3;
        dieDelegate += Test;
    }

    private void Update()
    {
        MinMaxValueStat();
    }

    public void Heal(float heal)
    {
        float healValue = Mathf.Clamp(_hp + heal, 0, _maxhp);
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
    void Test3(GameObject target)
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
    public bool Critical()
    {
        criticalRandomValue = Random.Range(1f, 101f);
        criticalProbability = 100f - _critchance;
        if (criticalRandomValue >= criticalProbability)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void MinMaxValueStat()
    {
        if (_attackspeed <= 2.5f)
            _attackspeed = 2.5f;
        if (_attackspeed >= 9f)
            _attackspeed = 9f;
        if (_skillCooltimePercent <= 0.5f)
            _skillCooltimePercent = 0.5f;
        if (_damage <= 5)
            _damage = 5;
        if (_movespeed <= 250f)
            _movespeed = 250f;
        if (_movespeed >= 500f)
            _movespeed = 500f;
        if (_reInputTime >= 3f)
            _reInputTime = 3f;
        if (_reInputTime <= 0.5f)
            _reInputTime = 0.5f;
        if (_critchance >= 100f)
            _critchance = 100f;
        if (_critchance <= 0f)
            _critchance = 0f;
    }

    public void LifeGen()
    {
        if (_maxhp > _hp)
            _hp += _lifegen;
        if (_hp > _maxhp)
            _hp = _maxhp;
    }
}

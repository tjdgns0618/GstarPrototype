using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ParticlePoolManager particlePoolManager;
    public UIManager uiManager;
    public EnemyPoolManager enemyPoolManager;
    public ItemPoolManager itempools;

    public TMP_Text txt_gold;

    public float _maxhp = 100f;
    public float _hp = 100f;
    public int _gold = 0;

    public float _damage = 10f;
    public float _range = 1f;
    public float _cooldown = 0;
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

    public delegate void ActiveDelegate();
    public ActiveDelegate activeDelegate;   // attackDelegate로 바꿔야함

    public delegate void HitDelegate();
    public HitDelegate hitDelegate;

    public delegate void DieDelegate();
    public DieDelegate dieDelegate;
    //         GameManager.instance.dieDelegate();  (EnemyAI Dead 함수에 넣기)

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
    }

    private void Update()
    {
        //txt_gold = _gold;
    }

    public void Heal(float heal)
    {
        if(_maxhp == _hp)   // 현재체력이 가득 찼을 때
        {
            return;
        }
        if (_maxhp > _hp)   // 현재체력이 최대체력보다 낮으면서
        {
            if (_hp + heal > _maxhp)    // 힐을 받으면 최대체력을 넘어가는 수치일 경우
                _hp = _maxhp;                // 현재체력은 최대체력 수치만큼
            else                                     // 힐을 받아도 최대체력을 넘어가지 않을 경우
                _hp = _hp + heal;           // 현재체력에서 회복 수치만큼 회복
        }
    }
}

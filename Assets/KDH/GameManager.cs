using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PoolManager pools;

    public float _hp = 100;
    public int _gold = 0;

    public float _damage = 10;
    public float _range = 1;
    public float _cooldown = 0;
    public float _attackspeed = 1;
    public float _maxhp;

    public bool isHit = false;  // �÷��̾ �ǰ��ߴ��� Ȯ���ϴ� ����

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
}

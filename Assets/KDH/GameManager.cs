using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PoolManager pools;

    public float _hp = 100;
    public int _gold = 0;

    float _damage = 10;
    float _range = 1;
    float _cooldown = 0;
    float _attackspeed = 1;
    float _maxhp;

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

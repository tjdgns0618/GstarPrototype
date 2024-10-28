using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    float _maneulTimer;
    float _shieldTimer;
    float _fireTimer;
    float _godTimer;
    float _freezeTimer;

    float _tickManeul;
    float _tickShield;
    float _tickFire;
    float _tickGod;
    float _tickFreeze;

    public delegate void ManeulTick();
    public delegate void ShieldTick();
    public delegate void FireTick();
    public delegate void GodTick();
    public delegate void FreezeTick();

    public static ManeulTick _maneulT;
    public static ShieldTick _shieldT;
    public static FireTick _fireT;
    public static GodTick _godT;
    public static FreezeTick _freezeT;


    void Start()
    {
        _tickManeul = ItemDataBase.instance.Variable(36);
        _tickShield = ItemDataBase.instance.Variable(37);
        _tickFire = ItemDataBase.instance.Variable(38);
        _tickGod = ItemDataBase.instance.Variable(40);
        _tickFreeze = ItemDataBase.instance.Variable(47);
    }

    void Update()
    {
        TickManeul();
        TickShield();
        TickFire();
        TickGod();
        TickFreeze();
    }

    public void TickManeul()
    {
        if (_maneulT == null) 
            return;
        _maneulTimer += Time.deltaTime;
        if (_maneulTimer >= _tickManeul)
        {
            _maneulTimer = 0;
            _maneulT?.Invoke();
        }
    }

    public void TickShield()
    {
        if (_shieldT == null)
            return;
        _shieldTimer += Time.deltaTime;
        if (_shieldTimer >= _tickShield)
        {
            _shieldTimer = 0;
            _shieldT?.Invoke();
        }
    }

    public void TickFire()
    {
        if (_fireT == null)
            return;
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= _tickFire)
        {
            _fireTimer = 0;
            _fireT?.Invoke();
        }
    }

    public void TickGod()
    {
        if (_godT == null) 
            return;
        _godTimer += Time.deltaTime;
        if (_godTimer >= _tickGod)
        {
            _godTimer = 0;
            _godT?.Invoke();
        }
    }

    public void TickFreeze()
    {
            if (_freezeT == null)
                return;
            _freezeTimer += Time.deltaTime;
            if (_freezeTimer >= _tickFreeze)
            {
                _freezeTimer = 0;
                _freezeT?.Invoke();
            }
    }
}

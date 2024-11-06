using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    GameManager gm;

    float _maneulTimer;
    float _shieldTimer;
    float _fireTimer;
    float _godTimer;
    float _freezeTimer;
    float _starTimer;
    float _shurikenTimer;

    float _tickManeul;
    float _tickShield;
    float _tickFire;
    float _tickGod;
    float _tickFreeze;
    float _tickStar;
    float _tickShuriken;

    public delegate void ManeulTick();
    public delegate void ShieldTick();
    public delegate void FireTick();
    public delegate void GodTick();
    public delegate void FreezeTick();
    public delegate void StarTick();
    public delegate void ShurikenTick();

    public static ManeulTick _maneulT;
    public static ShieldTick _shieldT;
    public static FireTick _fireT;
    public static GodTick _godT;
    public static FreezeTick _freezeT;
    public static StarTick _starT;
    public static ShurikenTick _shurikenT;


    void Start()
    {
        gm = GameManager.instance;
        _tickManeul = ItemDataBase.instance.Variable(36);
        _tickShield = ItemDataBase.instance.Variable(37);
        _tickFire = ItemDataBase.instance.Variable(38);
        _tickGod = ItemDataBase.instance.Variable(40);
        _tickFreeze = ItemDataBase.instance.Variable(47);
        _tickStar = ItemDataBase.instance.Variable(48);
        _tickShuriken = ItemDataBase.instance.Variable(31);
    }

    void Update()
    {
        TickManeul();
        TickShield();
        TickFire();
        TickGod();
        TickFreeze();
        TickPopcorn();
        TickShuriken();
    }

    public void TickManeul()
    {
        if (_maneulT == null) 
            return;
        _maneulTimer += Time.deltaTime;
        if (_maneulTimer >= _tickManeul - (ItemDataBase.instance.Variable2(36) * gm.FindItemCount(36) - 1))
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

    public void TickPopcorn()
    {
        if (_starT == null)
            return;
        _starTimer += Time.deltaTime;
        if (_starTimer >= _tickStar)
        {
            _starTimer = 0;
            _starT?.Invoke();
        }
    }

    public void TickShuriken()
    {
        if (_shurikenT == null)
            return;
        _shurikenTimer += Time.deltaTime;
        if (_shurikenTimer >= _tickShuriken - (ItemDataBase.instance.Variable2(31) * (gm.FindItemCount(31) - 1)))
        {
            _shurikenTimer = 0;
            _shurikenT?.Invoke();
        }
    }
}

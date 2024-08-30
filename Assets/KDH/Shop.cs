using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    enum _item
    {
        _damage,
        _cooldown
    }

    public Text t_gold;
    public Text t_hp;

    int _gold = 0;                           // 현재 골드
    int _hp = 100;                          // 현재 체력

    public void GoldTrade(int _cost_gold)
    {
        if (_gold >= _cost_gold)
        {
            _gold -= _cost_gold;
        }
        else
        {
            Debug.Log("돈없어");
        }
    }

    public void HpTrade(int _cost_hp)
    {
        if (_hp > _cost_hp)
        {
            _hp -=  _cost_hp;
        }
        else
        {
            Debug.Log("피없어");
        }
    }

    public void TestGold()
    {
        _gold += 500;
    }

    public void TestHp()
    {
        _hp -= 10;
    }

    public void Update()
    {
        t_gold.text = "Gold : "+ _gold;
        t_hp.text = "Hp : " + _hp;
    }
}

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

    public GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }

    public void GoldTrade(int _cost_gold)                   // �� ������ �� �����ϴٴ� UI(�ڸ�) �߰�
    {
        if (gm._gold >= _cost_gold)
        {
            gm._gold -= _cost_gold;
        }
        else
        {
            Debug.Log("������");
        }
    }

    public void HpTrade(int _cost_hp)                           // �� ������ �� �����ϴٴ� UI(�ڸ�) �߰�
    {
        if (gm._hp > _cost_hp)
        {
            gm._hp -=  _cost_hp;
        }
        else
        {
            Debug.Log("�Ǿ���");
        }
    }

    public void TestGold()
    {
        gm._gold += 500;
    }

    public void TestHp()
    {
        gm._hp -= 10;
    }

    public void Update()
    {
        t_gold.text = "Gold : "+ gm._gold;
        t_hp.text = "Hp : " + gm._hp;
    }
}

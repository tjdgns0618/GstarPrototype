using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTotem : MonoBehaviour
{
    GameManager gm;

    Rigidbody rb;

    int a;
    int b;
    int c;

    private void Start()
    {
        gm = GameManager.instance;
        rb = gameObject.GetComponent<Rigidbody>();

        Roll();
    }

    private void Update()
    {
        rb.AddTorque(new Vector3(a, b, c) * 20f);
    }

    public void DiceRoll()
    {
        int rollNum = Random.Range(1, 7);
        switch (rollNum)
        {
            case 1:
                gm._damage -= 20f;
                gm._attackspeed -= 0.4f;
                gm._critchance -= 5f;
                gm._critdmg -= 0.15f;
                Debug.Log("무언가 잘못된 것 같다.");
                return;
            case 2:
                gm._hp -= 5f;
                Debug.Log("무언가 튀어나와 나를 공격했다.");
                return;
            case 3:
                gm._attackspeed -= 0.2f;
                gm._movespeed -= 20;
                Debug.Log("몸이 둔해진 것 같다.");
                return;
            case 4:
                Debug.Log("아무 일도 일어나지 않았다.");
                return;
            case 5:
                gm._critchance += 10f;
                Debug.Log("집중력이 상승했다.");
                return;
            case 6:
                gm._attackspeed += 0.8f;
                gm._skillCooltimePercent -= 0.08f;
                gm._damage += 30f;
                Debug.Log("신의 힘에 가까워진 기분이 든다.");
                return;
            default:
                return;
        }
    }
    public void Roll()
    {
        a = Random.Range(0, 2);
        b = Random.Range(0, 2);
        c = Random.Range(0, 2);
        if (a == 0)
            a = -1;
        if(b == 0)
            b = -1;
        if (c == 0) 
            c = -1;
    }
}

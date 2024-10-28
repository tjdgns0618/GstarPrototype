using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingOrb : MonoBehaviour
{
    GameManager gm;

    void Start()
    {
        gm = GameManager.instance;
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            Debug.Log(gm._maxhp+"置企杷びびびびびびびびびびびびびびびびびびびびびび");
            Debug.Log(ItemDataBase.instance.Variable2(29) + "護 遁っっっっっっっっっっっっっっ");
            Debug.Log(gm.FindItemCount(29) + "焼戚奴 鯵呪ぬぬぬぬぬぬぬぬぬぬぬぬ");
            Debug.Log(ItemDataBase.instance.Variable2(29) * gm.FindItemCount(29) + "却戚 咽廃暗 っっっっっっっっ");
            gm.Heal(gm._maxhp * ItemDataBase.instance.Variable2(29));
            gameObject.SetActive(false);
        }
    }
}

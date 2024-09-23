using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPoolManager : MonoBehaviour
{
    public GameObject[] i_Prefabs;

    List<GameObject>[] i_Pools;

    private void Awake()
    {
        i_Pools = new List<GameObject>[i_Prefabs.Length];

        for (int index = 0; index < i_Pools.Length; index++)
        {
            i_Pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach (GameObject item in i_Pools[index])
        {
            if(!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        if (!select)
        {
            select = Instantiate(i_Prefabs[index], transform);
            i_Pools[index].Add(select);
        }
        return select;
    }

}

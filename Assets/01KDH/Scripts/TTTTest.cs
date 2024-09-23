using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTTTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            Transform firework = GameManager.instance.itempools.Get(1).transform;
            firework.parent = transform;
            firework.transform.position = PlayerCharacter.Instance.transform.position + new Vector3(0, 3, 0);
        }
    }
}

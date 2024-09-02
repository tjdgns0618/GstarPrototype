using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PoolManager poolManager;
    //public Player player;
    public GameObject test;

    void Awake()
    {
        Instance = this;
    }
}

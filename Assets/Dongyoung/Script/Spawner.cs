using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    //public Enemy enemy;

    Wave currentWave;
    int currentWaveNumber; //현재 웨이브

    int enemiesRemainingToSpawn; //남은 소환할 적의 수
    int enemiesRemainingAlive; //살아있는 적의 수
    float nextSpawnTime; //다음 소환 시간

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemiesRemainingToSpawn > 0 %% Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time - currentWave.timeBetweenSpawn;

            //Enemy spawnedEnemy = Instantiate(spawnedEnemy, Vector3.zero,Quaternion.identity);
            spawnEnemy OnDeath += Onenemy
        }
    }

    [Serializable]
    public class Wave
    {
        public int enemyCount; //소환할 적의 수
        public float timeBetweenSpawn; // 소환 딜레이
    }
}

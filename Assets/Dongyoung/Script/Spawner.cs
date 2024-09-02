using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    //public Enemy enemy;
    public GameObject test;

    Wave currentWave;
    int currentWaveNumber; //현재 웨이브

    int enemiesRemainingToSpawn; //남은 소환할 적의 수
    int enemiesRemainingAlive; //살아있는 적의 수
    float nextSpawnTime; //다음 소환 시간

    // Start is called before the first frame update
    void Start()
    {
        NextWave();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time - currentWave.timeBetweenSpawn;

            Spawn();

            //Enemy spawnedEnemy = Instantiate(spawnedEnemy, Vector3.zero,Quaternion.identity);
            //spawnEnemy OnDeath += OnEnemyDeath;
        }
    }

    void OnEnemyDeath()                     //살아있는 적의 수가 0이 되면 NextWave 함수 호출
    {
        enemiesRemainingAlive--;            

        if(enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    void NextWave()
    {
        currentWaveNumber++;
        print("wave :  "+currentWaveNumber);
        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
        }
    }

    void Spawn()
    {

    }


    [Serializable]
    public class Wave
    {
        public int enemyCount; //소환할 적의 수
        public float timeBetweenSpawn; // 소환 딜레이
    }
}

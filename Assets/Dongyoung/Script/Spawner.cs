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
    int currentWaveNumber; //���� ���̺�

    int enemiesRemainingToSpawn; //���� ��ȯ�� ���� ��
    int enemiesRemainingAlive; //����ִ� ���� ��
    float nextSpawnTime; //���� ��ȯ �ð�

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

    void OnEnemyDeath()                     //����ִ� ���� ���� 0�� �Ǹ� NextWave �Լ� ȣ��
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
        public int enemyCount; //��ȯ�� ���� ��
        public float timeBetweenSpawn; // ��ȯ ������
    }
}

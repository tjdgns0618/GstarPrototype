using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public Enemy enemy;
    public GameObject rangeSpawn;
    private BoxCollider area;
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;
    Wave currentWave;
    int currentWaveNumber; //���� ���̺�

    int enemiesRemainingToSpawn; //���� ��ȯ�� ���� ��
    int enemiesRemainingAlive; //����ִ� ���� ��
    float nextSpawnTime; //���� ��ȯ �ð�

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            SpawnRandomObject();
        }
        NextWave();
    }

   void SpawnRandomObject()
    {
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        float randomZ = Random.Range(spawnAreaMin.z, spawnAreaMax.z);

        Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);

        Instantiate(enemy, randomPosition.normalized, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time - currentWave.timeBetweenSpawn;

            Enemy spawnedEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity);
            //spawnedEnemy.OnDeath +=  OnEnemyDeath;
        }
    }
    void OnEnemyDeath()                     //����ִ� ���� ���� 0�� �Ǹ� NextWave �Լ� ȣ��
    {
        enemiesRemainingAlive--;

        if (enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    void NextWave()
    {
        currentWaveNumber++;
        print("wave :  " + currentWaveNumber);
        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
        }
    }  

    [Serializable]
    public class Wave
    {
        public int enemyCount; //��ȯ�� ���� ��
        public float timeBetweenSpawn; // ��ȯ ������
    }
}

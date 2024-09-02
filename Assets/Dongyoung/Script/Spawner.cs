using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    //public Enemy enemy;

    Wave currentWave;
    int currentWaveNumber; //���� ���̺�

    int enemiesRemainingToSpawn; //���� ��ȯ�� ���� ��
    int enemiesRemainingAlive; //����ִ� ���� ��
    float nextSpawnTime; //���� ��ȯ �ð�

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
        public int enemyCount; //��ȯ�� ���� ��
        public float timeBetweenSpawn; // ��ȯ ������
    }
}

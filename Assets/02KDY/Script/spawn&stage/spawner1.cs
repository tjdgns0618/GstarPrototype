using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class spawner1 : MonoBehaviour
{
    public Transform spawnPos; //���� �߽� ��ġ
    public GameObject portal;
    public GameObject rewardUI;
    public GameObject waveClear;
    public GameObject stageClear;
    //public GameObject bossPrefab;
    //public GameObject playerPrefab;
    public TMP_Text waveInfoText;
    public TMP_Text stageInfoText;
    public TMP_Text waveCountText;

    public float spawnRadius = 20f; // �÷��̾�κ��� enemy�� ������ �� �ִ� �ִ� �Ÿ�
    public float minDistancefromPlayer = 5f; // �÷��̾�� enemy ���� �ּ� �Ÿ�
    public int firstWaveEnemy = 10; //ù��° ���̺꿡 ������ enemy�� ��
    public float spawnInterval = 3f; //enemy ���� �ֱ�
    public float waveInterval = 5f; //���̺� �� ��� �ð�
    public int maxWaves = 5; //�ִ� ���̺� ��
    public int currentWave = 0; //���� ���̺�

    public int currentStage = 1; //���� ��������

    private int enemyPerSpawn; //�� ���� �ֱ⿡ ������ enemy ��
    private int spawnedCount = 0; //������ enemy ī��Ʈ
    private int totalEnemiesInWave; //���� ���̺꿡�� ������ enemy�� ��ü ��
    private int enemiesLeft;

    public string[] enemyNames;

    public delegate void Action();
    public Action enemyDead;

    WaitForSeconds wInterval;

    private void Awake()
    {
        enemyDead += OnEnemyDeath;     // �̺�Ʈ ���
    }

    void Start()
    {
        wInterval= new WaitForSeconds(spawnInterval);
        StartWave();
    }

    void StartWave()
    {
        Time.timeScale = 1;
        StartCoroutine(WaveSystem());
    }

    void OnEnemyDeath()                     //����ִ� ���� ���� 0�� �Ǹ� NextWave �Լ� ȣ��
    {
        spawnedCount--;
        Debug.Log(enemiesLeft);
        UpdateWaveInfoUI();
        OnEnemyDestroyed();
        if (enemiesLeft == 0)
        {
            if(currentWave == maxWaves)
            {           
                stageClear.SetActive(true);
                Invoke("StageClear", 2f);
                portal.SetActive(true);
                Time.timeScale = 1;
                //���� �߰� ����   
            }
            else
            {
                waveClear.SetActive(true);
                Invoke("RewardTerm", 2f);
                //rewardUI.AddRewardRandomItems(rewardUI.allItems);
            }
        }
    }

    public void RewardTerm()
    {
        Time.timeScale = 0;
        rewardUI.SetActive(true);
        waveClear.SetActive(false);
    }
    public void StageClear()
    {
        stageClear.SetActive(false);
    }
    IEnumerator WaveSystem() //���̺� �ý���
    {
        if (currentWave < maxWaves)
        {
            currentWave++; //���� ���̺�� �Ѿ
            SetupWave(); //���̺� ����
            yield return wInterval;
            yield return StartCoroutine(SpawnEnemy()); //���� ����
        }
        Debug.Log("All waves completed");
    }

    public void SetupWave()
    {
        enemyPerSpawn = firstWaveEnemy * currentWave * currentStage; //���̺긶�� ������ ���� �� ���� ex)firstWaveEnemy�� 5�� ��� 1�������� 1���̺� 5����(5*1*1)
        totalEnemiesInWave = enemyPerSpawn; 
        spawnedCount = 0;
        enemiesLeft = totalEnemiesInWave;
        UpdateWaveInfoUI();
        Debug.Log($"{currentWave}���̺� ����! �� {totalEnemiesInWave}�� ����");
        waveCountText.text = $"WAVE {currentWave}";
    }

    IEnumerator SpawnEnemy() // ���� �ð� �������� ���� ����
    {
        int enemiesToSpawn = totalEnemiesInWave; // ���� ���� ��
        int enemiesInThisBatch; // �̹� ��ġ���� ������ ���� ��

        while (enemiesToSpawn > 0)
        {
            enemiesInThisBatch = Mathf.Min(firstWaveEnemy, enemiesToSpawn);

            for (int i = 0; i < enemiesInThisBatch; i++)
            {
                if (spawnedCount >= totalEnemiesInWave) break;

                Vector3 randomPosition = GetRandomPosition(); // �÷��̾� �ֺ� ���� ��ġ ����

                if (Vector3.Distance(randomPosition, spawnPos.position) >= minDistancefromPlayer)
                {
                    if (!IsPositionOccupied(randomPosition))
                    {
                        // enemyPrefab ����Ʈ���� ������ ������ ����
                        string randomEnemyName = enemyNames[Random.Range(0, enemyNames.Length)];

                        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

                        // Instantiate ��� ������Ʈ Ǯ������ ����
                        GameObject enemy = GameManager.instance.enemyPoolManager.GetEnemyPool(randomEnemyName);
                        enemy.transform.position = randomPosition;
                        enemy.transform.rotation = randomRotation;

                        // ObjectPoolManager.instance.SpawnFromPool(randomEnemyPrefab, randomPosition, randomRotation);
                        spawnedCount++;
                        enemiesToSpawn--;
                    }
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }



    // �ٸ� ������� �Ÿ��� ����Ͽ� ��ġ�� �ʵ��� üũ�ϴ� �Լ�
    bool IsPositionOccupied(Vector3 position)
    {
        float minDistanceBetweenEnemies = 3f; // ���� ���� �ּ� �Ÿ� ����

        GameObject[] spawnedEnemies = GameObject.FindGameObjectsWithTag("Enemy"); // �̹� ������ ������ ã��

        foreach (GameObject enemy in spawnedEnemies)
        {
            if (Vector3.Distance(position, enemy.transform.position) < minDistanceBetweenEnemies)
            {
                return true; // �ٸ� ���� �ʹ� ������ ��ġ�� ����� �� ����
            }
        }

        return false; // ����� ������ ������ ��ġ�� ����� �� ����
    }


    //spawnPos �ֺ��� ������ ��ġ�� ��ȯ�ϴ� �Լ�
    Vector3 GetRandomPosition()
    {
        //���� ���� ������ ������ ��ġ ����
        Vector2 randomCirclePoint = Random.insideUnitSphere * spawnRadius; //���� ���� ������ ������ 2D��ǥ
        Vector3 randomPosition = new Vector3(randomCirclePoint.x, 0, randomCirclePoint.y); //y�� 0���� �����Ͽ� ��鿡�� ����

        // spawnPos�� ��ġ �����ؼ� ������ ����
        randomPosition += spawnPos.position;

        return randomPosition;
    }
    // Update is called once per frame

    void OnDrawGizmosSelected() //����׿� �ڵ�(���� ���� �ð�ȭ)
    {
        Gizmos.color = Color.green;
        if (spawnPos != null)
        {
            //spawnPos�� �߽��ؼ� ���� �ݰ� ǥ��
            Gizmos.DrawWireSphere(spawnPos.position, spawnRadius);
        }
    }
    public void OnEnemyDestroyed()
    {
        enemiesLeft--;
        UpdateWaveInfoUI();
    }
    void UpdateWaveInfoUI()
    {
        waveInfoText.text = $"���� ��  {enemiesLeft}";
    }
    public void increaseStage()
    {
        portal.SetActive(false);
        currentStage++;
        stageInfoText.text = $"STAGE {currentStage}";
        currentWave = 0;
        Debug.Log($"stage increase to {currentStage}");
        StartWave();
    }
}

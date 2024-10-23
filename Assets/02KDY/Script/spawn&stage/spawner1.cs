using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class spawner1 : MonoBehaviour
{
    public Transform spawnPos; //스폰 중심 위치
    public GameObject portal;
    public GameObject rewardUI;
    public GameObject waveClear;
    public GameObject stageClear;
    //public GameObject bossPrefab;
    //public GameObject playerPrefab;
    public TMP_Text waveInfoText;
    public TMP_Text stageInfoText;
    public TMP_Text waveCountText;

    public float spawnRadius = 20f; // 플레이어로부터 enemy가 생성될 수 있는 최대 거리
    public float minDistancefromPlayer = 5f; // 플레이어와 enemy 간의 최소 거리
    public int firstWaveEnemy = 10; //첫번째 웨이브에 나오는 enemy의 수
    public float spawnInterval = 3f; //enemy 생성 주기
    public float waveInterval = 5f; //웨이브 간 대기 시간
    public int maxWaves = 5; //최대 웨이브 수
    public int currentWave = 0; //현재 웨이브

    public int currentStage = 1; //현재 스테이지

    private int enemyPerSpawn; //한 번의 주기에 생성할 enemy 수
    private int spawnedCount = 0; //생성된 enemy 카운트
    private int totalEnemiesInWave; //현재 웨이브에서 생성할 enemy의 전체 수
    private int enemiesLeft;

    public string[] enemyNames;

    public delegate void Action();
    public Action enemyDead;

    WaitForSeconds wInterval;

    private void Awake()
    {
        enemyDead += OnEnemyDeath;     // 이벤트 등록
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

    void OnEnemyDeath()                     //살아있는 적의 수가 0이 되면 NextWave 함수 호출
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
                //토템 추가 예정   
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
    IEnumerator WaveSystem() //웨이브 시스템
    {
        if (currentWave < maxWaves)
        {
            currentWave++; //다음 웨이브로 넘어감
            SetupWave(); //웨이브 설정
            yield return wInterval;
            yield return StartCoroutine(SpawnEnemy()); //몬스터 생성
        }
        Debug.Log("All waves completed");
    }

    public void SetupWave()
    {
        enemyPerSpawn = firstWaveEnemy * currentWave * currentStage; //웨이브마다 생성할 몬스터 수 증가 ex)firstWaveEnemy가 5인 경우 1스테이지 1웨이브 5마리(5*1*1)
        totalEnemiesInWave = enemyPerSpawn; 
        spawnedCount = 0;
        enemiesLeft = totalEnemiesInWave;
        UpdateWaveInfoUI();
        Debug.Log($"{currentWave}웨이브 시작! 적 {totalEnemiesInWave}개 생성");
        waveCountText.text = $"WAVE {currentWave}";
    }

    IEnumerator SpawnEnemy() // 일정 시간 간격으로 몬스터 생성
    {
        int enemiesToSpawn = totalEnemiesInWave; // 남은 적의 수
        int enemiesInThisBatch; // 이번 배치에서 생성할 적의 수

        while (enemiesToSpawn > 0)
        {
            enemiesInThisBatch = Mathf.Min(firstWaveEnemy, enemiesToSpawn);

            for (int i = 0; i < enemiesInThisBatch; i++)
            {
                if (spawnedCount >= totalEnemiesInWave) break;

                Vector3 randomPosition = GetRandomPosition(); // 플레이어 주변 랜덤 위치 생성

                if (Vector3.Distance(randomPosition, spawnPos.position) >= minDistancefromPlayer)
                {
                    if (!IsPositionOccupied(randomPosition))
                    {
                        // enemyPrefab 리스트에서 랜덤한 프리팹 선택
                        string randomEnemyName = enemyNames[Random.Range(0, enemyNames.Length)];

                        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

                        // Instantiate 대신 오브젝트 풀링에서 스폰
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



    // 다른 적들과의 거리를 계산하여 겹치지 않도록 체크하는 함수
    bool IsPositionOccupied(Vector3 position)
    {
        float minDistanceBetweenEnemies = 3f; // 적들 간의 최소 거리 설정

        GameObject[] spawnedEnemies = GameObject.FindGameObjectsWithTag("Enemy"); // 이미 생성된 적들을 찾음

        foreach (GameObject enemy in spawnedEnemies)
        {
            if (Vector3.Distance(position, enemy.transform.position) < minDistanceBetweenEnemies)
            {
                return true; // 다른 적과 너무 가까우면 위치를 사용할 수 없음
            }
        }

        return false; // 충분히 떨어져 있으면 위치를 사용할 수 있음
    }


    //spawnPos 주변의 랜덤한 위치를 반환하는 함수
    Vector3 GetRandomPosition()
    {
        //스폰 영역 내에서 랜덤한 위치 생성
        Vector2 randomCirclePoint = Random.insideUnitSphere * spawnRadius; //원형 범위 내에서 랜덤한 2D좌표
        Vector3 randomPosition = new Vector3(randomCirclePoint.x, 0, randomCirclePoint.y); //y를 0으로 설정하여 평면에서 생성

        // spawnPos의 위치 기준해서 오프셋 적용
        randomPosition += spawnPos.position;

        return randomPosition;
    }
    // Update is called once per frame

    void OnDrawGizmosSelected() //디버그용 코드(스폰 범위 시각화)
    {
        Gizmos.color = Color.green;
        if (spawnPos != null)
        {
            //spawnPos를 중심해서 스폰 반경 표시
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
        waveInfoText.text = $"남은 적  {enemiesLeft}";
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

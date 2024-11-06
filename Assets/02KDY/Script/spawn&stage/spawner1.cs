using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class spawner1 : MonoBehaviour
{
    public Transform spawnPos; //스폰 중심 위치
    public GameObject portal;
    public GameObject rewardUI;
    public GameObject waveClear;
    public GameObject stageClear;
    public GameObject bossPrefab;
    public GameObject bossPrefab2;
    public GameObject bossPrefab3;
    public GameObject bossPrefab4;
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

    public int initialStage = 0;
    public int currentStage = 1; //현재 스테이지

    private int enemyPerSpawn; //한 번의 주기에 생성할 enemy 수
    private int spawnedCount = 0; //생성된 enemy 카운트
    public int totalEnemiesInWave; //현재 웨이브에서 생성할 enemy의 전체 수
    private int enemiesLeft;

    SoundManager soundManager;

    public string[] enemyNames;

    public List<GameObject> enemies = new List<GameObject>();

    public int _randomnum;

    public delegate void Action();
    public Action enemyDead;

    WaitForSeconds wInterval;
    SceneChanger sceneChanger;

    private void Awake()
    {
        enemyDead += OnEnemyDeath;     // 이벤트 등록
    }

    private void Start()
    {
        soundManager = SoundManager.instance;
        wInterval = new WaitForSeconds(spawnInterval);
        sceneChanger = FindObjectOfType<SceneChanger>();
        // StartWave();
    }

    void StartWave()
    {
        Time.timeScale = 1;
        StartCoroutine(WaveSystem());
    }

    void OnEnemyDeath()                     //살아있는 적의 수가 0이 되면 NextWave 함수 호출
    {
        spawnedCount--;
        UpdateWaveInfoUI();
        OnEnemyDestroyed();
        if (enemiesLeft == 0)
        {
            if (currentWave == maxWaves)
            {
                stageClear.SetActive(true);
                Invoke("StageClear", 2f);
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
        portal.SetActive(true);
        Time.timeScale = 0;
        rewardUI.SetActive(true);
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
        if (currentStage == 2 && currentWave == maxWaves) //보스 스테이지 조정
        {
            StartCoroutine(BossWave());
            UpdateWaveInfoUI();
            waveCountText.text = $"WAVE {currentWave}";
            Debug.Log("보스 웨이브이므로 일반 몬스터 생성 안함.");
            soundManager.PlayMusic(4);
            return;
        }
        else if (currentStage == 4 && currentWave == maxWaves)
        {
            StartCoroutine(BossWave());
            UpdateWaveInfoUI();
            waveCountText.text = $"WAVE {currentWave}";
            Debug.Log("보스 웨이브이므로 일반 몬스터 생성 안함.");
            soundManager.PlayMusic(4);
            return;
        }
        else if (currentStage == 6 && currentWave == maxWaves)
        {
            StartCoroutine(BossWave());
            UpdateWaveInfoUI();
            waveCountText.text = $"WAVE {currentWave}";
            Debug.Log("보스 웨이브이므로 일반 몬스터 생성 안함.");
            soundManager.PlayMusic(4);
            return;
        }
        else if (currentStage == 8 && currentWave == maxWaves)
        {
            StartCoroutine(BossWave());
            UpdateWaveInfoUI();
            waveCountText.text = $"WAVE {currentWave}";
            Debug.Log("보스 웨이브이므로 일반 몬스터 생성 안함.");
            soundManager.PlayMusic(4);
            return;
        }
        if(currentStage != 7 || currentStage != 8)
            enemyPerSpawn = (firstWaveEnemy + currentStage * currentWave * 4) % 2 == 0 ?
                (firstWaveEnemy + currentStage * currentWave * 4) : (firstWaveEnemy + currentStage * currentWave * 4) - 1; //웨이브마다 생성할 몬스터 수 증가 ex)firstWaveEnemy가 5인 경우 1스테이지 1웨이브 5마리(5*1*1)
        totalEnemiesInWave = enemyPerSpawn;
        spawnedCount = 0;
        enemiesLeft = totalEnemiesInWave;
        UpdateWaveInfoUI();
        Debug.Log($"{currentWave}웨이브 시작! 적 {totalEnemiesInWave}개 생성");
        waveCountText.text = $"WAVE {currentWave}";
    }

    IEnumerator SpawnEnemy()
    {
        if (currentStage == 2 && currentWave == maxWaves) // 보스 스테이지 조정
        {
            yield break;
        }
        else if (currentStage == 4 && currentWave == maxWaves)
        {
            yield break;
        }
        else if (currentStage == 6 && currentWave == maxWaves)
        {
            yield break;
        }
        else if (currentStage == 8 && currentWave == maxWaves)
        {
            yield break;
        }
        if ((currentStage == 3 || currentStage == 5 || currentStage == 7) && currentWave == 1)
        {
            soundManager.PlayMusic(2);
        }
        int enemiesToSpawn = totalEnemiesInWave;
        int halfEnemies = enemiesToSpawn / 2; // 한 번에 나올 몬스터 수를 반으로 나눔

        for (int batch = 0; batch < 2; batch++) // 두 번에 나누어 생성
        {
            int enemiesInThisBatch = halfEnemies; // 이번 배치에서 생성할 적의 수

            while (enemiesInThisBatch > 0)
            {
                if (spawnedCount >= totalEnemiesInWave) break;

                Vector3 randomPosition = GetRandomPosition(); // 플레이어 주변 랜덤 위치 생성

                if (Vector3.Distance(randomPosition, spawnPos.position) >= minDistancefromPlayer)
                {
                    if (!IsPositionOccupied(randomPosition))
                    {
                        string randomEnemyName = enemyNames[Random.Range(0, enemyNames.Length)];
                        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

                        GameObject enemy = GameManager.instance.enemyPoolManager.GetEnemyPool(randomEnemyName);
                        enemies.Add(enemy);
                        enemy.transform.position = randomPosition;
                        enemy.transform.rotation = randomRotation;

                        spawnedCount++;
                        enemiesInThisBatch--;
                    }
                }
            }

            // 다음 배치 전 대기 시간 추가
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator BossWave() //보스 웨이브
    {
        yield return new WaitForSeconds(spawnInterval);
        Debug.Log("보스 소환 준비 중...");
        SpawnBoss();
        UpdateWaveInfoUI();
        yield return new WaitUntil(() => enemiesLeft == 0);

        Debug.Log("보스 처치 완료. 스테이지 클리어");
        if (currentStage == 8 && currentWave == maxWaves)
        {
            sceneChanger.LoadEndingCutScene();
        }
    }

    void SpawnBoss() //보스 소환
    {
        if (currentStage == 2 && currentWave == maxWaves) //2스테이지
        {
            Vector3 bossSpawnPosition = GetRandomPosition();
            GameObject boss = Instantiate(bossPrefab2, bossSpawnPosition, Quaternion.identity);
            enemiesLeft = 1; // 보스 몬스터 1마리
            enemies.Add(boss);
            Debug.Log($"보스 몬스터가 {bossSpawnPosition} 위치에 소환되었습니다!");
        }
        else if (currentStage == 4 && currentWave == maxWaves) //4스테이지
        {
            Vector3 bossSpawnPosition = GetRandomPosition();
            GameObject boss = Instantiate(bossPrefab2, bossSpawnPosition, Quaternion.identity);
            enemiesLeft = 1; // 보스 몬스터 1마리
            enemies.Add(boss);
            Debug.Log($"보스 몬스터가 {bossSpawnPosition} 위치에 소환되었습니다!");
        }
        else if (currentStage == 6 && currentWave == maxWaves) //6스테이지
        {
            Vector3 bossSpawnPosition = GetRandomPosition();
            GameObject boss = Instantiate(bossPrefab3, bossSpawnPosition, Quaternion.identity);
            enemiesLeft = 1; // 보스 몬스터 1마리
            enemies.Add(boss);
            Debug.Log($"보스 몬스터가 {bossSpawnPosition} 위치에 소환되었습니다!");
        }
        else if (currentStage == 8 && currentWave == maxWaves) //8스테이지
        {
            Vector3 bossSpawnPosition = GetRandomPosition();
            GameObject boss = Instantiate(bossPrefab4, bossSpawnPosition, Quaternion.identity);
            enemiesLeft = 1; // 보스 몬스터 1마리
            enemies.Add(boss);
            Debug.Log($"보스 몬스터가 {bossSpawnPosition} 위치에 소환되었습니다!");
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
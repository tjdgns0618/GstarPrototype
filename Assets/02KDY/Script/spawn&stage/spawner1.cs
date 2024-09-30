using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class spawner1 : MonoBehaviour
{
    public List<GameObject> enemyPrefab; //enemy프리팹
    public Transform spawnPos; //스폰 중심 위치
    public GameObject portal;
    public GameObject rewardUI;
    public GameObject bossprefab;
    //public GameObject playerPrefab;
    public TMP_Text waveInfoText;
    public TMP_Text stageInfoText;

    public float spawnRadius = 20f; // 플레이어로부터 enemy가 생성될 수 있는 최대 거리
    public float minDistancefromPlayer = 5f; // 플레이어와 enemy 간의 최소 거리
    public int firstWaveEnemy = 10; //첫번째 웨이브에 나오는 enemy의 수
    public float spawnInterval = 3f; //enemy 생성 주기
    public float waveInterval = 5f; //웨이브 간 대기 시간
    public int maxWaves = 5; //최대 웨이브 수
    public int currentWave = 0; //현재 웨이브

    public int currentStage = 1; //현재 스테이지
    public int stagePerEnemy = 3; //스테이지마다 늘어나는 enemy의 배수

    private int enemyPerSpawn; //한 번의 주기에 생성할 enemy 수
    private int spawnedCount = 0; //생성된 enemy 카운트
    private int totalEnemiesInWave; //현재 웨이브에서 생성할 enemy의 전체 수
    private int enemiesLeft;

//    [SerializeField]
//    private List<EnemyPoolManager.Pool> pools = new List<EnemyPoolManager.Pool>
//{
//    new EnemyPoolManager.Pool { tag = "Enemy", prefab = enemyPrefab, size = 20 },
//};

    public delegate void Action();
    public Action enemyDead;

    WaitForSeconds wInterval;
    void Start()
    {
        wInterval= new WaitForSeconds(spawnInterval);
        StartWave();
        enemyDead += OnEnemyDeath;     // 이벤트 등록
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

        if (spawnedCount == 0)
        {
            if(currentWave == maxWaves)
            {
                portal.SetActive(true);
                Time.timeScale = 1;
                //토템 추가 예정
            }
            else
            {
                Time.timeScale = 0;
                //rewardUI.AddRewardRandomItems(rewardUI.allItems);
                rewardUI.SetActive(true);
            }
        }
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
        enemyPerSpawn = firstWaveEnemy * currentWave * currentStage; //웨이브마다 생성할 몬스터 수 증가
        totalEnemiesInWave = enemyPerSpawn; //웨이브마다 총 생성할 몬스터의 수
        spawnedCount = 0;
        enemiesLeft = totalEnemiesInWave;
        UpdateWaveInfoUI();
        Debug.Log($"{currentWave}웨이브 시작! 적 {totalEnemiesInWave}개 생성");
    }

    IEnumerator SpawnEnemy() //일정 시간 간격으로 몬스터 생성
    {
        while (spawnedCount < totalEnemiesInWave)
        {
            for (int i = 0; i < enemyPerSpawn; i++)
            {
                if (spawnedCount >= totalEnemiesInWave) break; // 현재 웨이브의 총 몬스터 수 초과 방지

                Vector3 randomPosition = GetRandomPosition(); // 플레이어 주변 랜덤 위치 생성

                // 플레이어와의 최소 거리가 유지되는지 확인
                if (Vector3.Distance(randomPosition, spawnPos.position) >= minDistancefromPlayer)
                {
                    // 기존에 생성된 적과의 거리가 충분한지 확인
                    if (!IsPositionOccupied(randomPosition))
                    {
                        GameObject randomEnemyPrefab = enemyPrefab[Random.Range(0, enemyPrefab.Count)]; // 랜덤한 enemy프리팹 선택

                        // Y축 기준 랜덤 회전 생성 (0도 ~ 360도)
                        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

                        // 적을 랜덤한 위치와 회전으로 생성
                        Instantiate(randomEnemyPrefab, randomPosition, randomRotation);
                        spawnedCount++;
                    }
                }
            }
            yield return null;
        }
    }

    // 다른 적들과의 거리를 계산하여 겹치지 않도록 체크하는 함수
    bool IsPositionOccupied(Vector3 position)
    {
        float minDistanceBetweenEnemies = 2f; // 적들 간의 최소 거리 설정

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
        waveInfoText.text = $" 남은 적 : {enemiesLeft}";
    }
    public void increaseStage()
    {
        portal.SetActive(false);
        currentStage++;
        stageInfoText.text = $"{currentStage} Stage";
        currentWave = 0;
        Debug.Log($"stage increase to {currentStage}");
        StartWave();
    }
}

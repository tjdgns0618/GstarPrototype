using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class spawner1 : MonoBehaviour
{
    public GameObject enemyprefab; //enemy프리팹
    public Transform player; // 플레이어의 위치

    public float spawnRadius = 20f; // 플레이어로부터 enemy가 생성될 수 있는 최대 거리
    public float minDistancefromPlayer = 5f; // 플레이어와 enemy 간의 최소 거리
    public int firstWaveEnemy = 5; //첫번째 웨이브에 나오는 enemy의 수
    public float spawnInterval = 3f; //enemy 생성 주기
    public float waveInterval = 5f; //웨이브 간 대기 시간
    public int maxWaves = 5; //최대 웨이브 수

    private int currentWave = 0; //현재 웨이브
    private int enemyPerSpawn; //한 번의 주기에 생성할 enemy 수
    private int spawnedCount = 0; //생성된 enemy 카운트
    private int totalEnemiesInWave; //현재 웨이브에서 생성할 enemy의 전체 수
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaveSystem());
    }

    IEnumerator WaveSystem() //웨이브 시스템
    {
        while (currentWave < maxWaves)
        {
            currentWave++; //다음 웨이브로 넘어감
            SetupWave(); //웨이브 설정
            yield return StartCoroutine(SpawnEnemy()); //몬스터 생성
            yield return new WaitForSeconds(waveInterval); //웨이브 간 대기 시간
        }

        Debug.Log("All waves completed");
    }

    void SetupWave()
    {
        enemyPerSpawn = firstWaveEnemy + (currentWave - 1) * 2; //웨이브마다 생성할 몬스터 수 증가
        totalEnemiesInWave = enemyPerSpawn * 2; //웨이브마다 총 생성할 몬스터의 수(지금은 한 주기당 2배)
        spawnedCount = 0;
        Debug.Log($"{currentWave}웨이브 시작! 적 {totalEnemiesInWave}개 생성");
    }

    IEnumerator SpawnEnemy() //일정 시간 간격으로 몬스터 생성
    {
        while (spawnedCount < totalEnemiesInWave)
        {
            for (int i = 0; i < enemyPerSpawn; i++)
            {
                if (spawnedCount >= totalEnemiesInWave) break; //현재 웨이브의 총 몬스터 수 초과 방지

                Vector3 randomPosition = GetRandomPosition(); //플레이어 주변 랜덤 위치 생성

                if(Vector3.Distance(randomPosition, player.position) >= minDistancefromPlayer) //플레이어가 최소 거리보다 멀리 떨어진 경우 생성
                {
                    Instantiate(enemyprefab, randomPosition, Quaternion.identity);
                    spawnedCount++;
                }
            }
            
            yield return new WaitForSeconds(spawnInterval); //지정된 시간 만큼 대기
        }
    }

    //플레이어 주변의 랜덤한 위치를 반환하는 함수
    Vector3 GetRandomPosition()
    {
        //스폰 영역 내에서 랜덤한 위치 생성
        Vector2 randomCirclePoint = Random.insideUnitSphere * spawnRadius; //원형 범위 내에서 랜덤한 2D좌표
        Vector3 randomPosition = new Vector3(randomCirclePoint.x, 0, randomCirclePoint.y); //y를 0으로 설정하여 평면에서 생성

        // 플레이어의 위치 기준해서 오프셋 적용
        randomPosition += player.position;

        return randomPosition;
    }
    // Update is called once per frame

    void OnDrawGizmosSelected() //디버그용 코드(스폰 범위 시각화)
    {
        Gizmos.color = Color.green;
        if(player != null)
        {
            //플레이어 위치를 중심해서 스폰 반경 표시
            Gizmos.DrawWireSphere(player.position, spawnRadius);
        }
    }
}

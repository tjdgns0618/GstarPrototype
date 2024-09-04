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
    public int numberOfMonster = 10; //생성할 몬스터 수

    public float spawntime = 3f; // enemy 생성 주기

    public int mostersperspawn = 3; //한번의 주기에 생성할 enemy 수

    private int spawnedCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnMonster());
    }

    IEnumerator SpawnMonster()
    {
        while (spawnedCount < numberOfMonster)
        {
            for(int i = 0; i < mostersperspawn; i++)
            {
                if (spawnedCount >= numberOfMonster) break; //전체 몬스터 수 초과 방지

                Vector3 randomPosition = GetRandomPosition(); //플레이어 주변 랜덤 위치 생성

                //플레이어와의 거리를 확인하여 최소 거리보다 멀리 떨어진 경우 생성
                if(Vector3.Distance(randomPosition, player.position) >= minDistancefromPlayer)
                {
                    Instantiate(enemyprefab, randomPosition, Quaternion.identity);
                    spawnedCount++;
                }
            }
            // 지정된 시간 만큼 대기
            yield return new WaitForSeconds(spawntime);
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

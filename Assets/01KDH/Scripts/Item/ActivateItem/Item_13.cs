using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Item_13 : MonoBehaviour
{
    public Transform player;
    public Transform firstEnemy;
    public float targetRange;
    public int maxTargets = 5;
    public float attackDelay;

    private LineRenderer lineRenderer;

    public GameObject particleSystemPrefab;
    public GameObject particle2SystemPrefab;

    Vector3 chainPos;

    private void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f; // 선의 시작 두께
        lineRenderer.endWidth = 0.1f; // 선의 끝 두께
        lineRenderer.startColor = Color.red; // 선의 시작 색상
        lineRenderer.endColor = Color.red; // 선의 끝 색상
        lineRenderer.positionCount = 0; // 초기 포지션 수
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            List<EnemyAI> enemies = FindEnemiesInRange();
            StartCoroutine(TargetEnemies(enemies));
        }
    }

    List<EnemyAI> FindEnemiesInRange()
    {
        return FindObjectsOfType<EnemyAI>()
           .Where(enemy => Vector3.Distance(player.position, enemy.transform.position) <= targetRange)
           .ToList();
    }

    IEnumerator TargetEnemies(List<EnemyAI> enemies)
    {
        List<EnemyAI> targetedEnemies = new List<EnemyAI>();

        while (targetedEnemies.Count < maxTargets && enemies.Count > 0)
        {
            // 현재 적 리스트에서 공격하지 않은 적만 필터링
            var availableEnemies = enemies.Where(enemy => enemy != null && !targetedEnemies.Contains(enemy)).ToList();
            if (availableEnemies.Count == 0)
                break; // 더 이상 공격할 적이 없으면 종료

            EnemyAI currentTarget = availableEnemies.OrderBy(enemy => Vector3.Distance(player.position, enemy.transform.position))
            .FirstOrDefault();

            if (currentTarget != null)
            {
                if (targetedEnemies.Count == 0)
                {
                    chainPos = currentTarget.transform.position;
                    // 첫 번째 타겟: 플레이어와 가장 가까운 적
                    currentTarget = availableEnemies.OrderBy(enemy => Vector3.Distance(player.position, enemy.transform.position)).First();
                    
                    GameObject particle = Instantiate(particleSystemPrefab, chainPos, Quaternion.identity); // 체인 파티클
                    GameObject particle2 = Instantiate(particle2SystemPrefab, chainPos, Quaternion.identity);   // 적 피격시 파티클
                    DrawLineToTarget(player.position, chainPos);
                }
                else
                {
                    if (targetedEnemies.Count > 0) // 리스트가 비어 있지 않음을 확인
                    {
                        chainPos = currentTarget.transform.position;
                        // 두 번째 타겟부터: 이전 타겟과 가장 가까운 적
                        currentTarget = availableEnemies.OrderBy(enemy => Vector3.Distance(targetedEnemies.Last().transform.position, enemy.transform.position)).First();
                        DrawLineToTarget(targetedEnemies.Last().transform.position, chainPos);
                    }
                    else
                    {
                        yield break;
                    }
                }
                currentTarget.Damage(3f);
                targetedEnemies.Add(currentTarget); // 공격한 적 추가
                enemies.Remove(currentTarget); // 공격한 적을 enemies 리스트에서도 제거
            }
            yield return new WaitForSeconds(attackDelay); // 공격 사이에 대기
        }
        lineRenderer.positionCount = 0;
    }

    void DrawLineToTarget(Vector3 from, Vector3 to)
    {
        lineRenderer.positionCount += 2; // 포지션 수 증가
        lineRenderer.SetPosition(lineRenderer.positionCount - 2, from); // 이전 타겟 위치
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, to); // 현재 타겟 위치
    }

    void Test()
    {

    }
}
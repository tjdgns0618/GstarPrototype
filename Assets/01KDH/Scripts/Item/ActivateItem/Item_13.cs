using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Item_13 : MonoBehaviour
{
    public Transform player;
    public float targetRange = 10f;
    public int maxTargets = 5;

    private void Update()
    {
        List<EnemyAI> enemies = FindEnemiesInRange();
        TargetEnemies(enemies);
    }

    List<EnemyAI> FindEnemiesInRange()      //LINQ를 이용해 연쇄 번개 효과
    {
        return FindObjectsOfType<EnemyAI>()
           .Where(enemy => Vector3.Distance(player.position, enemy.transform.position) <= targetRange)  // 플레이어와 각 적의 거리 계산 및 최대 사거리 내 적 선택
           .OrderBy(enemy => Vector3.Distance(player.position, enemy.transform.position))   // 플레이어와 각 적의 거리를 계산해 가까운 적 부터 먼 적 순서로 정렬
           .Take(maxTargets)    // 정렬된 적 리스트에서 최대 타겟 수 만큼 적 선택
           .ToList();   
    }

    void TargetEnemies(List<EnemyAI> enemies)
    {
        foreach (var enemy in enemies)
        {
            //enemy.hp -= 10f;
        }
    }
}

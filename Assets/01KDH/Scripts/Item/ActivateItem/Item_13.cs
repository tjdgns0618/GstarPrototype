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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
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
            var availableEnemies = enemies.Where(enemy => !targetedEnemies.Contains(enemy)).ToList();

            if (availableEnemies.Count == 0)
                break; // 더 이상 공격할 적이 없으면 종료

            EnemyAI currentTarget;

            if (targetedEnemies.Count == 0)
            {
                // 첫 번째 타겟: 플레이어와 가장 가까운 적
                currentTarget = availableEnemies.OrderBy(enemy => Vector3.Distance(player.position, enemy.transform.position)).First();
            }
            else
            {
                // 두 번째 타겟부터: 이전 타겟과 가장 가까운 적
                currentTarget = availableEnemies.OrderBy(enemy => Vector3.Distance(targetedEnemies.Last().transform.position, enemy.transform.position)).First();
            }

            currentTarget.Damage(100f);
            targetedEnemies.Add(currentTarget); // 공격한 적 추가
            enemies.Remove(currentTarget); // 공격한 적을 enemies 리스트에서도 제거

            yield return new WaitForSeconds(attackDelay); // 공격 사이에 대기
        }
    }
}
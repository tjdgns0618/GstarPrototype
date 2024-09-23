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
            // ���� �� ����Ʈ���� �������� ���� ���� ���͸�
            var availableEnemies = enemies.Where(enemy => !targetedEnemies.Contains(enemy)).ToList();

            if (availableEnemies.Count == 0)
                break; // �� �̻� ������ ���� ������ ����

            EnemyAI currentTarget;

            if (targetedEnemies.Count == 0)
            {
                // ù ��° Ÿ��: �÷��̾�� ���� ����� ��
                currentTarget = availableEnemies.OrderBy(enemy => Vector3.Distance(player.position, enemy.transform.position)).First();
            }
            else
            {
                // �� ��° Ÿ�ٺ���: ���� Ÿ�ٰ� ���� ����� ��
                currentTarget = availableEnemies.OrderBy(enemy => Vector3.Distance(targetedEnemies.Last().transform.position, enemy.transform.position)).First();
            }

            currentTarget.Damage(100f);
            targetedEnemies.Add(currentTarget); // ������ �� �߰�
            enemies.Remove(currentTarget); // ������ ���� enemies ����Ʈ������ ����

            yield return new WaitForSeconds(attackDelay); // ���� ���̿� ���
        }
    }
}
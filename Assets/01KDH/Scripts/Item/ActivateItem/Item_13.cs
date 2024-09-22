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

    List<EnemyAI> FindEnemiesInRange()      //LINQ�� �̿��� ���� ���� ȿ��
    {
        return FindObjectsOfType<EnemyAI>()
           .Where(enemy => Vector3.Distance(player.position, enemy.transform.position) <= targetRange)  // �÷��̾�� �� ���� �Ÿ� ��� �� �ִ� ��Ÿ� �� �� ����
           .OrderBy(enemy => Vector3.Distance(player.position, enemy.transform.position))   // �÷��̾�� �� ���� �Ÿ��� ����� ����� �� ���� �� �� ������ ����
           .Take(maxTargets)    // ���ĵ� �� ����Ʈ���� �ִ� Ÿ�� �� ��ŭ �� ����
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

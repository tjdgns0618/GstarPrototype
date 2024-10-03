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
    public Material lineMat;
    //public ParticleSystem lineParticle;
    //public ParticleSystem sparkParticle;

    private void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f; // ���� ���� �β�
        lineRenderer.endWidth = 0.1f; // ���� �� �β�
        lineRenderer.material = lineMat; // ���� ���� ����
        lineRenderer.startColor = Color.red; // ���� ���� ����
        lineRenderer.endColor = Color.red; // ���� �� ����
        lineRenderer.positionCount = 0; // �ʱ� ������ ��
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
            // ���� �� ����Ʈ���� �������� ���� ���� ���͸�
            var availableEnemies = enemies.Where(enemy => enemy != null && !targetedEnemies.Contains(enemy)).ToList();
            if (availableEnemies.Count == 0)
                break; // �� �̻� ������ ���� ������ ����

            EnemyAI currentTarget = availableEnemies.OrderBy(enemy => Vector3.Distance(player.position, enemy.transform.position))
            .FirstOrDefault();

            if (currentTarget != null)
            {
                if (targetedEnemies.Count == 0)
                {
                    // ù ��° Ÿ��: �÷��̾�� ���� ����� ��
                    currentTarget = availableEnemies.OrderBy(enemy => Vector3.Distance(player.position, enemy.transform.position)).First();
                    DrawLineToTarget(player.position, currentTarget.transform.position);
                }
                else
                {
                    if (targetedEnemies.Count > 0) // ����Ʈ�� ��� ���� ������ Ȯ��
                    {
                        // �� ��° Ÿ�ٺ���: ���� Ÿ�ٰ� ���� ����� ��
                        currentTarget = availableEnemies.OrderBy(enemy => Vector3.Distance(targetedEnemies.Last().transform.position, enemy.transform.position)).First();
                        DrawLineToTarget(targetedEnemies.Last().transform.position, currentTarget.transform.position);
                    }
                    else
                    {
                        yield break;
                    }
                }
                currentTarget.Damage(100f);
                targetedEnemies.Add(currentTarget); // ������ �� �߰�
                enemies.Remove(currentTarget); // ������ ���� enemies ����Ʈ������ ����
            }
            yield return new WaitForSeconds(attackDelay); // ���� ���̿� ���
        }
        lineRenderer.positionCount = 0;
    }

    void DrawLineToTarget(Vector3 from, Vector3 to)
    {
        lineRenderer.positionCount += 2; // ������ �� ����
        lineRenderer.SetPosition(lineRenderer.positionCount - 2, from); // ���� Ÿ�� ��ġ
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, to); // ���� Ÿ�� ��ġ
    }
}
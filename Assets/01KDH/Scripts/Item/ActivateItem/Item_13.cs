using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class Item_13 : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public float targetRange; // ���� ����
    public int maxTargets = 5; // �ִ� Ÿ�� ��
    public float attackDelay; // ���� ������ ������

    public GameObject hitParticlePrefab; // �ǰ� ��ƼŬ ������

    private List<EnemyAI> targetedEnemies = new List<EnemyAI>(); // Ÿ�� �� ����Ʈ
    private LineRenderer lineRenderer; // ���� ������ ������Ʈ
    //public LineRenderer linePrefab;
    private Dictionary<EnemyAI, GameObject> activeParticles = new Dictionary<EnemyAI, GameObject>(); // ���� ����� ��ƼŬ

    private void Start()
    {
        // ���� ������ ������Ʈ�� �߰��ϰ� �ʱ� ����
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.9f; // ���� �β�
        lineRenderer.endWidth = 0.9f; // �� �β�
        lineRenderer.positionCount = 0; // �ʱ� ������ ��
        lineRenderer.material = new Material(Shader.Find("Hovl/Particles/testS")); // �⺻ ���� ����
        lineRenderer.startColor = new Color(97f / 255f, 157f / 255f, 255f / 255f);
        lineRenderer.endColor = new Color(97f / 255f, 157f / 255f, 255f / 255f);
        Texture2D texture = Resources.Load<Texture2D>("Trail6"); // �ؽ�ó ���
        lineRenderer.material.mainTexture = texture;
        Texture2D noiseTexture = Resources.Load<Texture2D>("Noise34"); // ������ �ؽ�ó ���
        if (lineRenderer.material.HasProperty("_Noise")) // ���̴��� ������Ƽ�� �ִ��� Ȯ��
        {
            lineRenderer.material.SetTexture("_Noise", noiseTexture); // �ؽ�ó ����
        }
    }

    private void Update()
    {
        // 'P' Ű�� ������ �� Ÿ�� ���� ã�� ����
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(TargetEnemies()); // Ÿ�� �� �ڷ�ƾ ����
        }
    }

    private IEnumerator TargetEnemies()
    {
        // ���� �� ���� ã��
        List<EnemyAI> enemies = FindEnemiesInRange();

        // ���� 5�� �̸��̸� ����
        if (enemies.Count < 1)
        {
            yield break; // ���� �����ϸ� �ڷ�ƾ ����
        }

        // �ִ� Ÿ�� ���� �����ϰų� ���� ���� ������ �ݺ�
        while (targetedEnemies.Count < maxTargets && enemies.Count > 0)
        {
            EnemyAI currentTarget; // ���� Ÿ���� �� ����

            if (targetedEnemies.Count > 0)
            {
                // ���� Ÿ���� ��ƼŬ ����
                DestroyParticles(targetedEnemies.Last());
            }

            if (targetedEnemies.Count == 0)
            {
                // ù ��° Ÿ��: �÷��̾�� ���� ����� ��
                currentTarget = enemies.OrderBy(enemy => Vector3.Distance(player.position, enemy.transform.position)).FirstOrDefault();
            }
            else
            {
                // �� ��° Ÿ�ٺ���: ���� Ÿ�ٰ� ���� ����� ��
                currentTarget = enemies.OrderBy(enemy => Vector3.Distance(targetedEnemies.Last().transform.position, enemy.transform.position)).FirstOrDefault();
            }

            if (currentTarget == null)
            {
                yield break; // Ÿ���� null�̸� ����
            }

            // ���� Ÿ�ٰ� �����ϴ� ���� �׸���
            DrawLine(targetedEnemies.Count == 0 ? player.position : targetedEnemies.Last().transform.position, currentTarget.transform.position);

            currentTarget.Damage(100f); // ������ 3�� ���ظ� ����
            GameObject particle = Instantiate(hitParticlePrefab, currentTarget.transform.position, Quaternion.identity); // �ǰ� ��ƼŬ ����
            activeParticles[currentTarget] = particle; // ���� ���� ��ƼŬ ����

            targetedEnemies.Add(currentTarget); // Ÿ�� ����Ʈ�� ���� �� �߰�
            enemies.Remove(currentTarget); // ������ ���� ����Ʈ���� ����

            yield return new WaitForSeconds(attackDelay); // ���� ���� ���
        }

        // ��� Ÿ�ٿ� ���� ��ƼŬ ����
        foreach (var enemy in targetedEnemies)
        {
            DestroyParticles(enemy); // �� Ÿ�ٿ� ���� ��ƼŬ ����
        }

        // ������ ���� ������ ����
        lineRenderer.positionCount = 0; // ���� �������� ������ ���� 0���� �����Ͽ� ����

        // ��� Ÿ�� ����Ʈ �ʱ�ȭ
        targetedEnemies.Clear(); // Ÿ�� ����Ʈ �ʱ�ȭ
        activeParticles.Clear(); // Ȱ��ȭ�� ��ƼŬ ����Ʈ �ʱ�ȭ
    }

    private List<EnemyAI> FindEnemiesInRange()
    {
        // �÷��̾�� ������ ���� ���� ������ ã��
        return FindObjectsOfType<EnemyAI>()
            .Where(enemy => enemy != null && Vector3.Distance(player.position, enemy.transform.position) <= targetRange) // ���� ���� �� ���͸�
            .ToList(); // ����Ʈ�� ��ȯ
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        // ���� �������� ����� ���ο� ������ ���� ����
        lineRenderer.positionCount = 0; // ���� ������ ���� 0���� �����Ͽ� ����

        // ������ ���� 2�� �����Ͽ� ���ο� ������ �׸� �غ�
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start); // ���� ��ġ ����
        lineRenderer.SetPosition(1, end); // �� ��ġ ����
    }

    private void DestroyParticles(EnemyAI enemy)
    {
        // ���� Ÿ�ٿ� �ش��ϴ� ��ƼŬ ����
        if (activeParticles.TryGetValue(enemy, out GameObject particle))
        {
            Destroy(particle); // ��ƼŬ ����
            activeParticles.Remove(enemy); // ����Ʈ���� ����
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class spawner1 : MonoBehaviour
{
    public List<GameObject> enemyprefab; //enemy������
    public Transform player; // �÷��̾��� ��ġ
    public GameObject portal;
    public GameObject rewardUI;
    public TMP_Text waveUI;

    public float spawnRadius = 20f; // �÷��̾�κ��� enemy�� ������ �� �ִ� �ִ� �Ÿ�
    public float minDistancefromPlayer = 5f; // �÷��̾�� enemy ���� �ּ� �Ÿ�
    public int firstWaveEnemy = 5; //ù��° ���̺꿡 ������ enemy�� ��
    public float spawnInterval = 3f; //enemy ���� �ֱ�
    public float waveInterval = 5f; //���̺� �� ��� �ð�
    public int maxWaves = 5; //�ִ� ���̺� ��
    public int currentWave = 0; //���� ���̺�

    private int enemyPerSpawn; //�� ���� �ֱ⿡ ������ enemy ��
    private int spawnedCount = 0; //������ enemy ī��Ʈ
    private int totalEnemiesInWave; //���� ���̺꿡�� ������ enemy�� ��ü ��

    public delegate void Action();
    public Action enemyDead;

    void Start()
    {
        StartWave();
        enemyDead += OnEnemyDeath;     // �̺�Ʈ ���
    }

    void StartWave()
    {
        Time.timeScale = 1;
        StartCoroutine(WaveSystem());
    }

    void OnEnemyDeath()                     //����ִ� ���� ���� 0�� �Ǹ� NextWave �Լ� ȣ��
    {
        spawnedCount--;
        Debug.LogError(spawnedCount);

        if (spawnedCount == 0)
        {
            Time.timeScale = 0;
            rewardUI.SetActive(true);
        }
    }

    //void EndStage()
    //{
    //    if (currentWave == maxWaves && spawnedCount == 0)
    //    {
    //        // ��Ż Ȱ��ȭ
    //        // ���� Ȱ��ȭ
    //    }
    //}
    IEnumerator WaveSystem() //���̺� �ý���
    {
        if (currentWave < maxWaves)
        {
            currentWave++; //���� ���̺�� �Ѿ
            SetupWave(); //���̺� ����
            yield return new WaitForSeconds(spawnInterval);
            yield return StartCoroutine(SpawnEnemy()); //���� ����
        }

        Debug.Log("All waves completed");
    }

    void SetupWave()
    {
        enemyPerSpawn = firstWaveEnemy + (currentWave - 1) * 2; //���̺긶�� ������ ���� �� ����
        totalEnemiesInWave = enemyPerSpawn * 2; //���̺긶�� �� ������ ������ ��(������ ���̺� �� 4������ ���� ex.10->14->18)
        spawnedCount = 0;
        Debug.Log($"{currentWave}���̺� ����! �� {totalEnemiesInWave}�� ����");
    }

    public void WaveCount(int num)
    {
        waveUI.text = string.Format("Wave{0}", num);
    }

    IEnumerator SpawnEnemy() //���� �ð� �������� ���� ����
    {
        while (spawnedCount < totalEnemiesInWave)
        {
            for (int i = 0; i < enemyPerSpawn; i++)
            {
                if (spawnedCount >= totalEnemiesInWave) break; //���� ���̺��� �� ���� �� �ʰ� ����

                Vector3 randomPosition = GetRandomPosition(); //�÷��̾� �ֺ� ���� ��ġ ����

                if (Vector3.Distance(randomPosition, player.position) >= minDistancefromPlayer) //�÷��̾ �ּ� �Ÿ����� �ָ� ������ ��� ����
                {
                    GameObject randomEnemyPrefab = enemyprefab[Random.Range(0, enemyprefab.Count)]; //������ enemy������ ����

                    Instantiate(randomEnemyPrefab, randomPosition, Quaternion.identity);
                    spawnedCount++;
                }
            }
        }
        yield return null;
    }

    //�÷��̾� �ֺ��� ������ ��ġ�� ��ȯ�ϴ� �Լ�
    Vector3 GetRandomPosition()
    {
        //���� ���� ������ ������ ��ġ ����
        Vector2 randomCirclePoint = Random.insideUnitSphere * spawnRadius; //���� ���� ������ ������ 2D��ǥ
        Vector3 randomPosition = new Vector3(randomCirclePoint.x, 0, randomCirclePoint.y); //y�� 0���� �����Ͽ� ��鿡�� ����

        // �÷��̾��� ��ġ �����ؼ� ������ ����
        randomPosition += player.position;

        return randomPosition;
    }
    // Update is called once per frame

    void OnDrawGizmosSelected() //����׿� �ڵ�(���� ���� �ð�ȭ)
    {
        Gizmos.color = Color.green;
        if (player != null)
        {
            //�÷��̾� ��ġ�� �߽��ؼ� ���� �ݰ� ǥ��
            Gizmos.DrawWireSphere(player.position, spawnRadius);
        }
    }
}

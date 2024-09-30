using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class spawner1 : MonoBehaviour
{
    public List<GameObject> enemyPrefab; //enemy������
    public Transform spawnPos; //���� �߽� ��ġ
    public GameObject portal;
    public GameObject rewardUI;
    public GameObject bossprefab;
    //public GameObject playerPrefab;
    public TMP_Text waveInfoText;
    public TMP_Text stageInfoText;

    public float spawnRadius = 20f; // �÷��̾�κ��� enemy�� ������ �� �ִ� �ִ� �Ÿ�
    public float minDistancefromPlayer = 5f; // �÷��̾�� enemy ���� �ּ� �Ÿ�
    public int firstWaveEnemy = 10; //ù��° ���̺꿡 ������ enemy�� ��
    public float spawnInterval = 3f; //enemy ���� �ֱ�
    public float waveInterval = 5f; //���̺� �� ��� �ð�
    public int maxWaves = 5; //�ִ� ���̺� ��
    public int currentWave = 0; //���� ���̺�

    public int currentStage = 1; //���� ��������
    public int stagePerEnemy = 3; //������������ �þ�� enemy�� ���

    private int enemyPerSpawn; //�� ���� �ֱ⿡ ������ enemy ��
    private int spawnedCount = 0; //������ enemy ī��Ʈ
    private int totalEnemiesInWave; //���� ���̺꿡�� ������ enemy�� ��ü ��
    private int enemiesLeft;

//    [SerializeField]
//    private List<EnemyPoolManager.Pool> pools = new List<EnemyPoolManager.Pool>
//{
//    new EnemyPoolManager.Pool { tag = "Enemy", prefab = enemyPrefab, size = 20 },
//};

    public delegate void Action();
    public Action enemyDead;

    WaitForSeconds wInterval;
    void Start()
    {
        wInterval= new WaitForSeconds(spawnInterval);
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

        UpdateWaveInfoUI();

        if (spawnedCount == 0)
        {
            if(currentWave == maxWaves)
            {
                portal.SetActive(true);
                Time.timeScale = 1;
                //���� �߰� ����
            }
            else
            {
                Time.timeScale = 0;
                //rewardUI.AddRewardRandomItems(rewardUI.allItems);
                rewardUI.SetActive(true);
            }
        }
    }

    IEnumerator WaveSystem() //���̺� �ý���
    {
        if (currentWave < maxWaves)
        {
            currentWave++; //���� ���̺�� �Ѿ
            SetupWave(); //���̺� ����
            yield return wInterval;
            yield return StartCoroutine(SpawnEnemy()); //���� ����
        }

        Debug.Log("All waves completed");
    }

    public void SetupWave()
    {
        enemyPerSpawn = firstWaveEnemy * currentWave * currentStage; //���̺긶�� ������ ���� �� ����
        totalEnemiesInWave = enemyPerSpawn; //���̺긶�� �� ������ ������ ��
        spawnedCount = 0;
        enemiesLeft = totalEnemiesInWave;
        UpdateWaveInfoUI();
        Debug.Log($"{currentWave}���̺� ����! �� {totalEnemiesInWave}�� ����");
    }

    IEnumerator SpawnEnemy() //���� �ð� �������� ���� ����
    {
        while (spawnedCount < totalEnemiesInWave)
        {
            for (int i = 0; i < enemyPerSpawn; i++)
            {
                if (spawnedCount >= totalEnemiesInWave) break; // ���� ���̺��� �� ���� �� �ʰ� ����

                Vector3 randomPosition = GetRandomPosition(); // �÷��̾� �ֺ� ���� ��ġ ����

                // �÷��̾���� �ּ� �Ÿ��� �����Ǵ��� Ȯ��
                if (Vector3.Distance(randomPosition, spawnPos.position) >= minDistancefromPlayer)
                {
                    // ������ ������ ������ �Ÿ��� ������� Ȯ��
                    if (!IsPositionOccupied(randomPosition))
                    {
                        GameObject randomEnemyPrefab = enemyPrefab[Random.Range(0, enemyPrefab.Count)]; // ������ enemy������ ����

                        // Y�� ���� ���� ȸ�� ���� (0�� ~ 360��)
                        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

                        // ���� ������ ��ġ�� ȸ������ ����
                        Instantiate(randomEnemyPrefab, randomPosition, randomRotation);
                        spawnedCount++;
                    }
                }
            }
            yield return null;
        }
    }

    // �ٸ� ������� �Ÿ��� ����Ͽ� ��ġ�� �ʵ��� üũ�ϴ� �Լ�
    bool IsPositionOccupied(Vector3 position)
    {
        float minDistanceBetweenEnemies = 2f; // ���� ���� �ּ� �Ÿ� ����

        GameObject[] spawnedEnemies = GameObject.FindGameObjectsWithTag("Enemy"); // �̹� ������ ������ ã��

        foreach (GameObject enemy in spawnedEnemies)
        {
            if (Vector3.Distance(position, enemy.transform.position) < minDistanceBetweenEnemies)
            {
                return true; // �ٸ� ���� �ʹ� ������ ��ġ�� ����� �� ����
            }
        }

        return false; // ����� ������ ������ ��ġ�� ����� �� ����
    }

    //spawnPos �ֺ��� ������ ��ġ�� ��ȯ�ϴ� �Լ�
    Vector3 GetRandomPosition()
    {
        //���� ���� ������ ������ ��ġ ����
        Vector2 randomCirclePoint = Random.insideUnitSphere * spawnRadius; //���� ���� ������ ������ 2D��ǥ
        Vector3 randomPosition = new Vector3(randomCirclePoint.x, 0, randomCirclePoint.y); //y�� 0���� �����Ͽ� ��鿡�� ����

        // spawnPos�� ��ġ �����ؼ� ������ ����
        randomPosition += spawnPos.position;

        return randomPosition;
    }
    // Update is called once per frame

    void OnDrawGizmosSelected() //����׿� �ڵ�(���� ���� �ð�ȭ)
    {
        Gizmos.color = Color.green;
        if (spawnPos != null)
        {
            //spawnPos�� �߽��ؼ� ���� �ݰ� ǥ��
            Gizmos.DrawWireSphere(spawnPos.position, spawnRadius);
        }
    }
    public void OnEnemyDestroyed()
    {
        enemiesLeft--;
        UpdateWaveInfoUI();
    }
    void UpdateWaveInfoUI()
    {
        waveInfoText.text = $" ���� �� : {enemiesLeft}";
    }
    public void increaseStage()
    {
        portal.SetActive(false);
        currentStage++;
        stageInfoText.text = $"{currentStage} Stage";
        currentWave = 0;
        Debug.Log($"stage increase to {currentStage}");
        StartWave();
    }
}

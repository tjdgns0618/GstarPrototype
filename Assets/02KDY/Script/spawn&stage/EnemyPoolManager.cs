using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    // ���� ������ ��ƼŬ ������
    public List<GameObject> enemyPrefabs;

    // ��ƼŬ Ǯ�� ��ųʸ��� ���� (�̸� -> ��ƼŬ ����Ʈ)
    private Dictionary<string, List<GameObject>> enemyPools;

    // �� Ǯ�� ũ��
    public int poolSize = 10;

    void Start()
    {
        // ��ųʸ� �ʱ�ȭ
        enemyPools = new Dictionary<string, List<GameObject>>();

        // �� ��ƼŬ �����տ� ���� Ǯ ����
        foreach (GameObject prefab in enemyPrefabs)
        {
            // Ǯ�� �����ϰ� ��ųʸ��� �߰�
            List<GameObject> pool = new List<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject enemy = Instantiate(prefab);
                enemy.SetActive(false);
                pool.Add(enemy);
            }

            enemyPools.Add(prefab.name, pool);  // ��ƼŬ �̸��� key�� ���
        }
    }

    // Ư�� �̸��� ��ƼŬ ��������
    public GameObject GetEnemyPool(string enemyName)
    {
        if (enemyPools.ContainsKey(enemyName))
        {
            // �ش� �̸��� Ǯ���� ��Ȱ��ȭ�� ��ƼŬ�� ã�� ��ȯ
            List<GameObject> pool = enemyPools[enemyName];

            foreach (GameObject enemy in pool)
            {
                if (!enemy.activeInHierarchy)
                {
                    enemy.SetActive(true);
                    return enemy;
                }
            }

            // ��� ������ ��ƼŬ�� ���ٸ�, ���� �����Ͽ� Ǯ�� �߰�
            GameObject newEnemy = Instantiate(enemyPrefabs.Find(p => p.name == enemyName));
            newEnemy.SetActive(true);

            pool.Add(newEnemy);
            return newEnemy;
        }

        Debug.LogWarning("�ش� �̸��� ��ƼŬ�� ã�� �� �����ϴ�: " + enemyName);
        return null;
    }

    // ��ƼŬ�� ��ȯ (��Ȱ��ȭ)
    public void ReturnPool(GameObject enemy)
    {
        enemy.SetActive(false);
    }

    /*
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab; // �� ������
        public int size; // �̸� ������ ��
    }

    public static ObjectPoolManager Instance;

    public List<Pool> pools; // ���� �����տ� ���� Ǯ
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary; // �������� Ű�� �ϴ� ��ųʸ�

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

        // �� �����տ� ���� �̸� ������ �� ��ŭ ����
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.prefab, objectPool);
        }
    }

    // �������� �޾Ƽ� Ǯ���� ������Ʈ�� ������ �Լ�
    public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning($"Pool with prefab {prefab.name} doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[prefab].Dequeue();

        objectToSpawn.SetActive(true); // Ȱ��ȭ
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[prefab].Enqueue(objectToSpawn); // ������ ���� �ٽ� ť�� ����

        return objectToSpawn;
    }
    */
}

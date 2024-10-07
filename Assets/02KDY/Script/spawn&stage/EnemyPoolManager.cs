using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    // 여러 종류의 파티클 프리팹
    public List<GameObject> enemyPrefabs;

    // 파티클 풀을 딕셔너리로 관리 (이름 -> 파티클 리스트)
    private Dictionary<string, List<GameObject>> enemyPools;

    // 각 풀의 크기
    public int poolSize = 10;

    void Start()
    {
        // 딕셔너리 초기화
        enemyPools = new Dictionary<string, List<GameObject>>();

        // 각 파티클 프리팹에 대해 풀 생성
        foreach (GameObject prefab in enemyPrefabs)
        {
            // 풀을 생성하고 딕셔너리에 추가
            List<GameObject> pool = new List<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject enemy = Instantiate(prefab);
                enemy.SetActive(false);
                pool.Add(enemy);
            }

            enemyPools.Add(prefab.name, pool);  // 파티클 이름을 key로 사용
        }
    }

    // 특정 이름의 파티클 가져오기
    public GameObject GetEnemyPool(string enemyName)
    {
        if (enemyPools.ContainsKey(enemyName))
        {
            // 해당 이름의 풀에서 비활성화된 파티클을 찾아 반환
            List<GameObject> pool = enemyPools[enemyName];

            foreach (GameObject enemy in pool)
            {
                if (!enemy.activeInHierarchy)
                {
                    enemy.SetActive(true);
                    return enemy;
                }
            }

            // 사용 가능한 파티클이 없다면, 새로 생성하여 풀에 추가
            GameObject newEnemy = Instantiate(enemyPrefabs.Find(p => p.name == enemyName));
            newEnemy.SetActive(true);

            pool.Add(newEnemy);
            return newEnemy;
        }

        Debug.LogWarning("해당 이름의 파티클을 찾을 수 없습니다: " + enemyName);
        return null;
    }

    // 파티클을 반환 (비활성화)
    public void ReturnPool(GameObject enemy)
    {
        enemy.SetActive(false);
    }

    /*
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab; // 적 프리팹
        public int size; // 미리 생성할 수
    }

    public static ObjectPoolManager Instance;

    public List<Pool> pools; // 여러 프리팹에 대한 풀
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary; // 프리팹을 키로 하는 딕셔너리

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

        // 각 프리팹에 대해 미리 설정된 수 만큼 생성
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false); // 초기에는 비활성화
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.prefab, objectPool);
        }
    }

    // 프리팹을 받아서 풀에서 오브젝트를 꺼내는 함수
    public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning($"Pool with prefab {prefab.name} doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[prefab].Dequeue();

        objectToSpawn.SetActive(true); // 활성화
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[prefab].Enqueue(objectToSpawn); // 재사용을 위해 다시 큐에 넣음

        return objectToSpawn;
    }
    */
}

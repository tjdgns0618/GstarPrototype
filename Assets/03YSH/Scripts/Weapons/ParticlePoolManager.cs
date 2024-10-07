using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolManager : MonoBehaviour
{
    // 여러 종류의 파티클 프리팹
    public List<GameObject> particlePrefabs;

    // 파티클 풀을 딕셔너리로 관리 (이름 -> 파티클 리스트)
    private Dictionary<string, List<GameObject>> particlePools;

    // 각 풀의 크기
    public int poolSize = 10;

    void Start()
    {
        // 딕셔너리 초기화
        particlePools = new Dictionary<string, List<GameObject>>();

        // 각 파티클 프리팹에 대해 풀 생성
        foreach (GameObject prefab in particlePrefabs)
        {
            // 풀을 생성하고 딕셔너리에 추가
            List<GameObject> pool = new List<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject particle = Instantiate(prefab);
                particle.SetActive(false);
                pool.Add(particle);
            }

            particlePools.Add(prefab.name, pool);  // 파티클 이름을 key로 사용
        }
    }

    // 특정 이름의 파티클 가져오기
    public GameObject GetParticle(string particleName)
    {
        if (particlePools.ContainsKey(particleName))
        {
            // 해당 이름의 풀에서 비활성화된 파티클을 찾아 반환
            List<GameObject> pool = particlePools[particleName];

            foreach (GameObject particle in pool)
            {
                if (!particle.activeInHierarchy)
                {
                    ClearTrails(particle);
                    particle.SetActive(true);
                    return particle;
                }
            }

            // 사용 가능한 파티클이 없다면, 새로 생성하여 풀에 추가
            GameObject newParticle = Instantiate(particlePrefabs.Find(p => p.name == particleName));
            ClearTrails(newParticle);
            newParticle.SetActive(true);

            pool.Add(newParticle);
            return newParticle;
        }

        Debug.LogWarning("해당 이름의 파티클을 찾을 수 없습니다: " + particleName);
        return null;
    }

    // 파티클을 반환 (비활성화)
    public void ReturnParticle(GameObject particle)
    {
        ClearTrails(particle);
        particle.transform.position = PlayerCharacter.Instance.transform.position;
        particle.SetActive(false);
    }

    private void ClearTrails(GameObject particle)
    {
        // 부모 및 자식의 모든 TrailRenderer를 가져와서 초기화
        TrailRenderer[] trails = particle.GetComponentsInChildren<TrailRenderer>();

        foreach (TrailRenderer trail in trails)
        {
            trail.Clear();  // 트레일 초기화
        }
    }
}

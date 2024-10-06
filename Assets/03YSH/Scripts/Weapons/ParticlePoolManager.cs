using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolManager : MonoBehaviour
{
    // ���� ������ ��ƼŬ ������
    public List<GameObject> particlePrefabs;

    // ��ƼŬ Ǯ�� ��ųʸ��� ���� (�̸� -> ��ƼŬ ����Ʈ)
    private Dictionary<string, List<GameObject>> particlePools;

    // �� Ǯ�� ũ��
    public int poolSize = 10;

    void Start()
    {
        // ��ųʸ� �ʱ�ȭ
        particlePools = new Dictionary<string, List<GameObject>>();

        // �� ��ƼŬ �����տ� ���� Ǯ ����
        foreach (GameObject prefab in particlePrefabs)
        {
            // Ǯ�� �����ϰ� ��ųʸ��� �߰�
            List<GameObject> pool = new List<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject particle = Instantiate(prefab);
                particle.SetActive(false);
                pool.Add(particle);
            }

            particlePools.Add(prefab.name, pool);  // ��ƼŬ �̸��� key�� ���
        }
    }

    // Ư�� �̸��� ��ƼŬ ��������
    public GameObject GetParticle(string particleName)
    {
        if (particlePools.ContainsKey(particleName))
        {
            // �ش� �̸��� Ǯ���� ��Ȱ��ȭ�� ��ƼŬ�� ã�� ��ȯ
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

            // ��� ������ ��ƼŬ�� ���ٸ�, ���� �����Ͽ� Ǯ�� �߰�
            GameObject newParticle = Instantiate(particlePrefabs.Find(p => p.name == particleName));
            ClearTrails(newParticle);
            newParticle.SetActive(true);

            pool.Add(newParticle);
            return newParticle;
        }

        Debug.LogWarning("�ش� �̸��� ��ƼŬ�� ã�� �� �����ϴ�: " + particleName);
        return null;
    }

    // ��ƼŬ�� ��ȯ (��Ȱ��ȭ)
    public void ReturnParticle(GameObject particle)
    {
        ClearTrails(particle);
        particle.transform.position = PlayerCharacter.Instance.transform.position;
        particle.SetActive(false);
    }

    private void ClearTrails(GameObject particle)
    {
        // �θ� �� �ڽ��� ��� TrailRenderer�� �����ͼ� �ʱ�ȭ
        TrailRenderer[] trails = particle.GetComponentsInChildren<TrailRenderer>();

        foreach (TrailRenderer trail in trails)
        {
            trail.Clear();  // Ʈ���� �ʱ�ȭ
        }
    }
}

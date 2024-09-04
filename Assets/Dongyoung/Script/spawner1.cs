using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class spawner1 : MonoBehaviour
{
    public GameObject enemyprefab; //enemy������
    public Transform player; // �÷��̾��� ��ġ

    public float spawnRadius = 20f; // �÷��̾�κ��� enemy�� ������ �� �ִ� �ִ� �Ÿ�
    public float minDistancefromPlayer = 5f; // �÷��̾�� enemy ���� �ּ� �Ÿ�
    public int numberOfMonster = 10; //������ ���� ��

    public float spawntime = 3f; // enemy ���� �ֱ�

    public int mostersperspawn = 3; //�ѹ��� �ֱ⿡ ������ enemy ��

    private int spawnedCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnMonster());
    }

    IEnumerator SpawnMonster()
    {
        while (spawnedCount < numberOfMonster)
        {
            for(int i = 0; i < mostersperspawn; i++)
            {
                if (spawnedCount >= numberOfMonster) break; //��ü ���� �� �ʰ� ����

                Vector3 randomPosition = GetRandomPosition(); //�÷��̾� �ֺ� ���� ��ġ ����

                //�÷��̾���� �Ÿ��� Ȯ���Ͽ� �ּ� �Ÿ����� �ָ� ������ ��� ����
                if(Vector3.Distance(randomPosition, player.position) >= minDistancefromPlayer)
                {
                    Instantiate(enemyprefab, randomPosition, Quaternion.identity);
                    spawnedCount++;
                }
            }
            // ������ �ð� ��ŭ ���
            yield return new WaitForSeconds(spawntime);
        }
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
        if(player != null)
        {
            //�÷��̾� ��ġ�� �߽��ؼ� ���� �ݰ� ǥ��
            Gizmos.DrawWireSphere(player.position, spawnRadius);
        }
    }
}

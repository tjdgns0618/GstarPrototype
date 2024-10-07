using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class Item_13 : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public float targetRange; // 공격 범위
    public int maxTargets = 5; // 최대 타겟 수
    public float attackDelay; // 공격 사이의 딜레이

    public GameObject hitParticlePrefab; // 피격 파티클 프리팹

    private List<EnemyAI> targetedEnemies = new List<EnemyAI>(); // 타겟 적 리스트
    private LineRenderer lineRenderer; // 라인 렌더러 컴포넌트
    //public LineRenderer linePrefab;
    private Dictionary<EnemyAI, GameObject> activeParticles = new Dictionary<EnemyAI, GameObject>(); // 적과 연결된 파티클

    private void Start()
    {
        // 라인 렌더러 컴포넌트를 추가하고 초기 설정
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.9f; // 시작 두께
        lineRenderer.endWidth = 0.9f; // 끝 두께
        lineRenderer.positionCount = 0; // 초기 포지션 수
        lineRenderer.material = new Material(Shader.Find("Hovl/Particles/testS")); // 기본 재질 설정
        lineRenderer.startColor = new Color(97f / 255f, 157f / 255f, 255f / 255f);
        lineRenderer.endColor = new Color(97f / 255f, 157f / 255f, 255f / 255f);
        Texture2D texture = Resources.Load<Texture2D>("Trail6"); // 텍스처 경로
        lineRenderer.material.mainTexture = texture;
        Texture2D noiseTexture = Resources.Load<Texture2D>("Noise34"); // 노이즈 텍스처 경로
        if (lineRenderer.material.HasProperty("_Noise")) // 쉐이더에 프로퍼티가 있는지 확인
        {
            lineRenderer.material.SetTexture("_Noise", noiseTexture); // 텍스처 설정
        }
    }

    private void Update()
    {
        // 'P' 키를 눌렀을 때 타겟 적을 찾기 시작
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(TargetEnemies()); // 타겟 적 코루틴 시작
        }
    }

    private IEnumerator TargetEnemies()
    {
        // 범위 내 적을 찾음
        List<EnemyAI> enemies = FindEnemiesInRange();

        // 적이 5명 미만이면 종료
        if (enemies.Count < 1)
        {
            yield break; // 적이 부족하면 코루틴 종료
        }

        // 최대 타겟 수에 도달하거나 적이 없을 때까지 반복
        while (targetedEnemies.Count < maxTargets && enemies.Count > 0)
        {
            EnemyAI currentTarget; // 현재 타겟할 적 변수

            if (targetedEnemies.Count > 0)
            {
                // 이전 타겟의 파티클 삭제
                DestroyParticles(targetedEnemies.Last());
            }

            if (targetedEnemies.Count == 0)
            {
                // 첫 번째 타겟: 플레이어와 가장 가까운 적
                currentTarget = enemies.OrderBy(enemy => Vector3.Distance(player.position, enemy.transform.position)).FirstOrDefault();
            }
            else
            {
                // 두 번째 타겟부터: 이전 타겟과 가장 가까운 적
                currentTarget = enemies.OrderBy(enemy => Vector3.Distance(targetedEnemies.Last().transform.position, enemy.transform.position)).FirstOrDefault();
            }

            if (currentTarget == null)
            {
                yield break; // 타겟이 null이면 종료
            }

            // 현재 타겟과 연결하는 라인 그리기
            DrawLine(targetedEnemies.Count == 0 ? player.position : targetedEnemies.Last().transform.position, currentTarget.transform.position);

            currentTarget.Damage(100f); // 적에게 3의 피해를 입힘
            GameObject particle = Instantiate(hitParticlePrefab, currentTarget.transform.position, Quaternion.identity); // 피격 파티클 생성
            activeParticles[currentTarget] = particle; // 현재 적과 파티클 연결

            targetedEnemies.Add(currentTarget); // 타겟 리스트에 현재 적 추가
            enemies.Remove(currentTarget); // 공격한 적을 리스트에서 제거

            yield return new WaitForSeconds(attackDelay); // 공격 사이 대기
        }

        // 모든 타겟에 대한 파티클 삭제
        foreach (var enemy in targetedEnemies)
        {
            DestroyParticles(enemy); // 각 타겟에 대해 파티클 삭제
        }

        // 마지막 라인 렌더러 삭제
        lineRenderer.positionCount = 0; // 라인 렌더러의 포지션 수를 0으로 설정하여 삭제

        // 모든 타겟 리스트 초기화
        targetedEnemies.Clear(); // 타겟 리스트 초기화
        activeParticles.Clear(); // 활성화된 파티클 리스트 초기화
    }

    private List<EnemyAI> FindEnemiesInRange()
    {
        // 플레이어와 지정된 범위 내의 적들을 찾음
        return FindObjectsOfType<EnemyAI>()
            .Where(enemy => enemy != null && Vector3.Distance(player.position, enemy.transform.position) <= targetRange) // 범위 내의 적 필터링
            .ToList(); // 리스트로 반환
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        // 이전 포지션을 지우고 새로운 포지션 수를 설정
        lineRenderer.positionCount = 0; // 이전 포지션 수를 0으로 설정하여 지움

        // 포지션 수를 2로 설정하여 새로운 라인을 그릴 준비
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start); // 시작 위치 설정
        lineRenderer.SetPosition(1, end); // 끝 위치 설정
    }

    private void DestroyParticles(EnemyAI enemy)
    {
        // 이전 타겟에 해당하는 파티클 삭제
        if (activeParticles.TryGetValue(enemy, out GameObject particle))
        {
            Destroy(particle); // 파티클 삭제
            activeParticles.Remove(enemy); // 리스트에서 제거
        }
    }
}
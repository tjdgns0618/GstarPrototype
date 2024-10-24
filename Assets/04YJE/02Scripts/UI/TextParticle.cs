using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextParticle : MonoBehaviour
{
    public TMP_Text textMeshPro; // TextMeshPro 컴포넌트를 참조합니다.
    public ParticleSystem textParticleSystem; // 파티클 시스템을 참조합니다.
    private ParticleSystemRenderer rendererSystem; // 파티클 시스템의 렌더러를 저장할 변수입니다.

    void Start()
    {
        // 원하는 텍스트 설정
        textMeshPro.text = "여기에 원하는 텍스트 입력";

        // 파티클 시스템의 렌더러를 가져옵니다.
        rendererSystem = textParticleSystem.GetComponent<ParticleSystemRenderer>();

        // TextMeshPro의 메시를 파티클 시스템 렌더러의 메시에 설정합니다.
        rendererSystem.mesh = textMeshPro.mesh;

        // TextMeshPro 메시를 사용하여 렌더링 설정
        var renderer = textParticleSystem.GetComponent<ParticleSystemRenderer>();
        renderer.mesh = textMeshPro.mesh; // 전체 메시를 사용하는 대신 개별적으로 설정 필요
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextParticle : MonoBehaviour
{
    public TMP_Text textMeshPro; // TextMeshPro ������Ʈ�� �����մϴ�.
    public ParticleSystem textParticleSystem; // ��ƼŬ �ý����� �����մϴ�.
    private ParticleSystemRenderer rendererSystem; // ��ƼŬ �ý����� �������� ������ �����Դϴ�.

    void Start()
    {
        // ���ϴ� �ؽ�Ʈ ����
        textMeshPro.text = "���⿡ ���ϴ� �ؽ�Ʈ �Է�";

        // ��ƼŬ �ý����� �������� �����ɴϴ�.
        rendererSystem = textParticleSystem.GetComponent<ParticleSystemRenderer>();

        // TextMeshPro�� �޽ø� ��ƼŬ �ý��� �������� �޽ÿ� �����մϴ�.
        rendererSystem.mesh = textMeshPro.mesh;

        // TextMeshPro �޽ø� ����Ͽ� ������ ����
        var renderer = textParticleSystem.GetComponent<ParticleSystemRenderer>();
        renderer.mesh = textMeshPro.mesh; // ��ü �޽ø� ����ϴ� ��� ���������� ���� �ʿ�
    }
}

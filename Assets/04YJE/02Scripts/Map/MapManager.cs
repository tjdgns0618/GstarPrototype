using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Transform player;            //���� �÷��̾� ��ġ
    public Transform villageSpawnPos;   //���� ���� ��ġ
    public Transform fieldSpawnPos;     //�ʵ� ���� ��ġ

    public CameraManager cameraManager; 
    public UIManager uiManager;
    public spawner1 waveManager;

    public Map currentMap;

    public enum Map
    {
        Village,
        Field,
    }

    private void Start()
    {
        if (currentMap == Map.Village)
            uiManager.SetUIActive(uiManager.fieldUI, false);
        else if (currentMap == Map.Field)
            uiManager.SetUIActive(uiManager.fieldUI, true);
    }

    public void OnVillage()
    {
        //���� ������
        currentMap = Map.Village;
        //��

        //ĳ���� ��ġ �̵�
        player.SetPositionAndRotation(villageSpawnPos.position, villageSpawnPos.rotation);
        //ī�޶� ��ȯ
        cameraManager.ShowVillageView();
        //WAVE UI ����
        uiManager.SetUIActive(uiManager.fieldUI, false);
        //WaveManager ����
        waveManager.gameObject.SetActive(false);
        //����
    }

    public void OnField()
    {
        //���� ������
        currentMap = Map.Field;
        //��

        //ĳ���� ��ġ �̵�
        player.SetPositionAndRotation(fieldSpawnPos.position, fieldSpawnPos.rotation);
        //ī�޶� ��ȯ
        cameraManager.ShowFieldView();
        //WAVE UI �ѱ�
        uiManager.SetUIActive(uiManager.fieldUI, true);
        //WaveManager �ѱ�
        waveManager.gameObject.SetActive(true);
        //����
    }
}

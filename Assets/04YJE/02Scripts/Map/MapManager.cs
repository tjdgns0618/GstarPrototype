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

    public void OnVillage()
    {
        //��

        //ĳ���� ��ġ �̵�
        player.SetPositionAndRotation(villageSpawnPos.position, villageSpawnPos.rotation);
        //ī�޶� ��ȯ
        cameraManager.ShowVillageView();
        //WAVE UI ����
        uiManager.SetUIActive(uiManager.waveUI, false);
        //����
    }

    public void OnField()
    {
        //��

        //ĳ���� ��ġ �̵�
        player.SetPositionAndRotation(fieldSpawnPos.position, fieldSpawnPos.rotation);
        //ī�޶� ��ȯ
        cameraManager.ShowFieldView();
        //WAVE UI �ѱ�
        uiManager.SetUIActive(uiManager.waveUI, true);
        //����
    }
}

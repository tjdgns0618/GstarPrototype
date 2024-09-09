using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Transform player;            //현재 플레이어 위치
    public Transform villageSpawnPos;   //마을 스폰 위치
    public Transform fieldSpawnPos;     //필드 스폰 위치

    public CameraManager cameraManager; 
    public UIManager uiManager;

    public void OnVillage()
    {
        //텀

        //캐릭터 위치 이동
        player.SetPositionAndRotation(villageSpawnPos.position, villageSpawnPos.rotation);
        //카메라 전환
        cameraManager.ShowVillageView();
        //WAVE UI 끄기
        uiManager.SetUIActive(uiManager.waveUI, false);
        //사운드
    }

    public void OnField()
    {
        //텀

        //캐릭터 위치 이동
        player.SetPositionAndRotation(fieldSpawnPos.position, fieldSpawnPos.rotation);
        //카메라 전환
        cameraManager.ShowFieldView();
        //WAVE UI 켜기
        uiManager.SetUIActive(uiManager.waveUI, true);
        //사운드
    }
}

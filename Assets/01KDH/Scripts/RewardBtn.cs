using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBtn : MonoBehaviour
{
    public GameObject waveStart;
    public GameObject rewardUI;

    public RewardUI reUI;
    public Item[] items;

    //public Inventory _inventory;
    public RewardSlot _rewardslot;
    // public InvenSlot _slot;
    
    public spawner1 _spawn;


    public void NextWave()
    {
        // _inventory.AcquireItem(_rewardslot.item, _rewardslot.itemCount);    // 리워드 슬롯에 있는 아이템 획득 (아이템 개수 최신화)
        if (_spawn.currentWave < _spawn.maxWaves)   // 아직 스테이지가 진행 중 이라면
        {
            rewardUI.SetActive(false);    // 보상 창을 닫고
            waveStart.SendMessage("StartWave");   // StartWave 함수 실행
            reUI.AddRewardRandomItems(items);
        }
        else  // 스테이지가 끝났다면,
            rewardUI.SetActive(false);    // 창만 닫기
        //_rewardslot.ClearSlot();    // 다음 웨이브 보상 때 새로운 랜덤 보상으로 바꿔야 하기에 슬롯 초기화
    }
}

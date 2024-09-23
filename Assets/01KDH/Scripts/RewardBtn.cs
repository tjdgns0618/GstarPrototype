using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBtn : MonoBehaviour
{
    public GameObject waveStart;
    public GameObject rewardUI;

    public RewardUI reUI;
    public Item[] items;

    public Inventory _inventory;
    public RewardSlot _rewardslot;
    // public InvenSlot _slot;
    
    public spawner1 _spawn;


    public void NextWave()
    {
        if (_spawn.currentWave < _spawn.maxWaves)   // 아직 스테이지가 진행 중 이라면
        {
            if (_rewardslot.item != null) // 슬롯에 아이템이 있을 경우
            {
                _inventory.AcquireItem(_rewardslot.item, _rewardslot.itemCount); // 인벤토리에 아이템 추가
                _rewardslot.ClearSlot();    // 다음 웨이브 보상 때 새로운 랜덤 보상으로 바꿔야 하기에 슬롯 초기화
            }
        rewardUI.SetActive(false);    // 보상 창을 닫고
        waveStart.SendMessage("StartWave");   // StartWave 함수 실행
        reUI.AddRewardRandomItems(items);   // 새로운 아이템을 보상 슬롯에 추가
        }
        else  // 스테이지가 끝났다면,
            rewardUI.SetActive(false);    // 창만 닫기
    }
}

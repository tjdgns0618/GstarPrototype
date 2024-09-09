using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    public spawner1 _spawn;
    public RewardSlot _rewardslot;
    public Inventory _inventory;

    public RewardSlot[] rewardSlots;
    public Item[] allItems;

    private void Update()
    {
        AddRewardRandomItems(allItems);
    }

    public void AddRewardRandomItems(Item[] items)      // RewardUI가 열릴 때 실행(wave가 끝날 때)
    {
        List<Item> selectedItems = GetRewardRandomItems(items, 5);        // 중복되지 않는 아이템 5개를 담을 리스트

        for (int i = 0; i < rewardSlots.Length; i++)
        {
            if (i < selectedItems.Count)
            {
                rewardSlots[i].AddRewardRandomItem(selectedItems[i]);
            }
        }
    }

    // 중복되지 않는 아이템을 랜덤으로 선택하는 메서드
    private List<Item> GetRewardRandomItems(Item[] items, int count)    
    {
        List<Item> rewarditems = new List<Item>();        // 보상 아이템을 담을 리스트

        List<Item> itemList = new List<Item>(items);       // 선택 가능한 아이템 목록을 리스트로 변환


        while (rewarditems.Count < count && itemList.Count > 0)        // 중복되지 않는 랜덤 아이템을 선택
        {
            int index = Random.Range(0, itemList.Count);
            Item selectedItem = itemList[index];
            if (!rewarditems.Contains(selectedItem))
            {
                rewarditems.Add(selectedItem);
            }
            itemList.RemoveAt(index); // 선택된 아이템은 목록에서 제거
        }
        return rewarditems;
    }

    public void NextWave()
    {
        _inventory.AcquireItem(_rewardslot.item, _rewardslot.itemCount);    // 리워드 슬롯에 있는 아이템 획득 (아이템 개수 최신화)
        //if (_spawn.currentWave < _spawn.maxWaves)   // 아직 스테이지가 진행 중 이라면
        //{
        //    rewardUI.SetActive(false);    // 보상 창을 닫고
        //    waveStart.SendMessage("StartWave");   // StartWave 함수 실행
        //}
        //else  // 스테이지가 끝났다면,
        //    rewardUI.SetActive(false);    // 창만 닫기
        _rewardslot.ClearSlot();    // 다음 웨이브 보상 때 새로운 랜덤 보상으로 바꿔야 하기에 슬롯 초기화
    }


}

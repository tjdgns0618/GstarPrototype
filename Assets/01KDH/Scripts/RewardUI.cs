using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    public spawner1 _spawn;
    //public RewardSlot _rewardslot;
    //public Inventory _inventory;

    public RewardSlot[] rewardSlots;
    public Item[] allItems;

    private void Start()
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

}

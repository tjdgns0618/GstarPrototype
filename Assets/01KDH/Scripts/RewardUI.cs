using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class RewardUI : MonoBehaviour
{
    public spawner1 _spawn;
    public RewardSlot[] rewardSlots;
    public Item[] allItems;

    private void OnEnable()
    {
        AddRewardRandomItems(allItems);
    }

    public void AddRewardRandomItems(Item[] items)      // RewardUI�� ���� �� ����(wave�� ���� ��)
    {
        List<Item> selectedItems = GetRewardRandomItems(items, rewardSlots.Length);        // �ߺ����� �ʴ� ������ 5���� ���� ����Ʈ

        for (int i = 0; i < rewardSlots.Length; i++)
        {
            if (i < selectedItems.Count)
            {
                rewardSlots[i].AddRewardRandomItem(selectedItems[i]);
            }
        }
    }

    // �ߺ����� �ʴ� �������� �������� �����ϴ� �޼���
    private List<Item> GetRewardRandomItems(Item[] items, int count)    
    {
        List<Item> rewarditems = new List<Item>();        // ���� �������� ���� ����Ʈ
        List<Item> itemList = new List<Item>(items);       // ���� ������ ������ ����� ����Ʈ�� ��ȯ

        while (rewarditems.Count < count && itemList.Count > 0)        // �ߺ����� �ʴ� ���� �������� ����
        {
            int index = Random.Range(0, itemList.Count);
            Item selectedItem = itemList[index];
            if (!rewarditems.Contains(selectedItem))
            {
                rewarditems.Add(selectedItem);
            }
            itemList.RemoveAt(index); // ���õ� �������� ��Ͽ��� ����
        }
        return rewarditems;
    }

}

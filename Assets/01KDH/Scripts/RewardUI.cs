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

    public void AddRewardRandomItems(Item[] items)      // RewardUI�� ���� �� ����(wave�� ���� ��)
    {
        List<Item> selectedItems = GetRewardRandomItems(items, 5);        // �ߺ����� �ʴ� ������ 5���� ���� ����Ʈ

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

    public void NextWave()
    {
        _inventory.AcquireItem(_rewardslot.item, _rewardslot.itemCount);    // ������ ���Կ� �ִ� ������ ȹ�� (������ ���� �ֽ�ȭ)
        //if (_spawn.currentWave < _spawn.maxWaves)   // ���� ���������� ���� �� �̶��
        //{
        //    rewardUI.SetActive(false);    // ���� â�� �ݰ�
        //    waveStart.SendMessage("StartWave");   // StartWave �Լ� ����
        //}
        //else  // ���������� �����ٸ�,
        //    rewardUI.SetActive(false);    // â�� �ݱ�
        _rewardslot.ClearSlot();    // ���� ���̺� ���� �� ���ο� ���� �������� �ٲ�� �ϱ⿡ ���� �ʱ�ȭ
    }


}

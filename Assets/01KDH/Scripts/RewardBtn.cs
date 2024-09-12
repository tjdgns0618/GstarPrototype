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
        // _inventory.AcquireItem(_rewardslot.item, _rewardslot.itemCount);    // ������ ���Կ� �ִ� ������ ȹ�� (������ ���� �ֽ�ȭ)
        if (_spawn.currentWave < _spawn.maxWaves)   // ���� ���������� ���� �� �̶��
        {
            rewardUI.SetActive(false);    // ���� â�� �ݰ�
            waveStart.SendMessage("StartWave");   // StartWave �Լ� ����
            reUI.AddRewardRandomItems(items);
        }
        else  // ���������� �����ٸ�,
            rewardUI.SetActive(false);    // â�� �ݱ�
        //_rewardslot.ClearSlot();    // ���� ���̺� ���� �� ���ο� ���� �������� �ٲ�� �ϱ⿡ ���� �ʱ�ȭ
    }
}

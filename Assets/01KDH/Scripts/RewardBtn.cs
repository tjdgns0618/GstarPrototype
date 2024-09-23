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
        if (_spawn.currentWave < _spawn.maxWaves)   // ���� ���������� ���� �� �̶��
        {
            if (_rewardslot.item != null) // ���Կ� �������� ���� ���
            {
                _inventory.AcquireItem(_rewardslot.item, _rewardslot.itemCount); // �κ��丮�� ������ �߰�
                _rewardslot.ClearSlot();    // ���� ���̺� ���� �� ���ο� ���� �������� �ٲ�� �ϱ⿡ ���� �ʱ�ȭ
            }
        rewardUI.SetActive(false);    // ���� â�� �ݰ�
        waveStart.SendMessage("StartWave");   // StartWave �Լ� ����
        reUI.AddRewardRandomItems(items);   // ���ο� �������� ���� ���Կ� �߰�
        }
        else  // ���������� �����ٸ�,
            rewardUI.SetActive(false);    // â�� �ݱ�
    }
}

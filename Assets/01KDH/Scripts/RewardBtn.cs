using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RewardBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject waveStart;
    public GameObject rewardUI;

    public RewardUI reUI;
    public Item[] items;

    public Inventory _inventory;
    public RewardSlot _rewardslot;

    public Animator uiAnim;

    private Image image;

    public spawner1 _spawn;

    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void NextWave()
    {
        _rectTransform.anchoredPosition = new Vector3(-356, _rectTransform.anchoredPosition.y, 0);
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
        {
            rewardUI.SetActive(false);    // â�� �ݱ�
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        uiAnim.SetBool("isOn", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiAnim.SetBool("isOn", false);
        uiAnim.SetTrigger("isOnTrigger");
    }

}

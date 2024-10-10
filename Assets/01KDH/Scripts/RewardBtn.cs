using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class RewardBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject waveStart;
    public GameObject rewardUI;

    public RewardUI reUI;
    public Item[] items;
    public Item _item;

    public Inventory _inventory;
    public RewardSlot _rewardslot;
    public PassiveItem _passiveItem;
    public ActiveItem _activeItem;

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
        if (_spawn.currentWave < _spawn.maxWaves)   // 아직 스테이지가 진행 중 이라면
        {
            if (_rewardslot.item != null) // 슬롯에 아이템이 있을 경우
            {
                _inventory.AcquireItem(_rewardslot.item, _rewardslot.itemCount); // 인벤토리에 아이템 추가
                if (_rewardslot.item.itemType == Item.ItemType.passive)
                {
                    switch (_rewardslot.item.itemID)
                    {
                        case 0:
                            _passiveItem._01Pitem();
                            break;
                        case 1:
                            _passiveItem._02Pitem();
                            break;
                        case 2:
                            _passiveItem._03Pitem();
                            break;
                        case 3:
                            _passiveItem._04Pitem();
                            break;
                        case 4:
                            _passiveItem._05Pitem();
                            break;
                        case 5:
                            _passiveItem._06Pitem();
                            break;
                        case 6:
                            _passiveItem._07Pitem();
                            break;
                        case 7:
                            _passiveItem._08Pitem();
                            break;
                        case 8:
                            _passiveItem._09Pitem();
                            break;
                        case 9:
                            _passiveItem._10Pitem();
                            break;
                        case 10:
                            _passiveItem._11Pitem();
                            break;
                        case 11:
                            _passiveItem._12Pitem();
                            break;
                        case 12:
                            _passiveItem._13Pitem();
                            break;
                        case 13:
                            _passiveItem._14Pitem();
                            break;
                        case 14:
                            _passiveItem._15Pitem();
                            break;
                        case 15:
                            _passiveItem._16Pitem();
                            break;
                        case 16:
                            _passiveItem._17Pitem();
                            break;
                        case 17:
                            _passiveItem._18Pitem();
                            break;
                        case 18:
                            _passiveItem._19Pitem();
                            break;
                        case 19:
                            _passiveItem._20Pitem();
                            break;
                        case 20:
                            _passiveItem._21Pitem();
                            break;
                        case 21:
                            _passiveItem._22Pitem();
                            break;
                        case 22:
                            _passiveItem._23Pitem();
                            break;
                        case 23:
                            _passiveItem._24Pitem();
                            break;
                        case 24:
                            _passiveItem._25Pitem();
                            break;
                        case 25:
                            _passiveItem._26Pitem();
                            break;
                        case 26:
                            _passiveItem._27Pitem();
                            break;
                        case 27:
                            _passiveItem._28Pitem();
                            break;
                        case 28:
                            _passiveItem._29Pitem();
                            break;
                        default:
                            break;
                    }

                }
                if (_rewardslot.item.itemType == Item.ItemType.active)
                {
                    switch (_rewardslot.item.itemID)
                    {
                        case 29:
                            GameManager.instance.activeDelegate += _activeItem._01Item;
                            break;
                        case 30:
                            GameManager.instance.activeDelegate += _activeItem._02Item;
                            break;
                        case 31:
                            GameManager.instance.activeDelegate += _activeItem._03Item;
                            break;
                        case 32:
                            GameManager.instance.activeDelegate += _activeItem._04Item;
                            break;
                        case 33:
                            GameManager.instance.activeDelegate += _activeItem._05Item;
                            break;
                        case 34:
                            GameManager.instance.activeDelegate += _activeItem._06Item;
                            break;
                        case 35:
                            GameManager.instance.activeDelegate += _activeItem._07Item;
                            break;
                        case 36:
                            GameManager.instance.activeDelegate += _activeItem._08Item;
                            break;
                        case 37:
                            GameManager.instance.activeDelegate += _activeItem._09Item;
                            break;
                        case 38:
                            GameManager.instance.activeDelegate += _activeItem._10Item;
                            break;
                        case 39:
                            GameManager.instance.activeDelegate += _activeItem._11Item;
                            break;
                        case 40:
                            GameManager.instance.activeDelegate += _activeItem._12Item;
                            break;
                        case 41:
                            GameManager.instance.activeDelegate += _activeItem._13Item;
                            break;
                        case 42:
                            GameManager.instance.activeDelegate += _activeItem._14Item;
                            break;
                        case 43:
                            GameManager.instance.activeDelegate += _activeItem._15Item;
                            break;
                        case 44:
                            GameManager.instance.activeDelegate += _activeItem._16Item;
                            break;
                        case 45:
                            GameManager.instance.activeDelegate += _activeItem._17Item;
                            break;
                        case 46:
                            GameManager.instance.activeDelegate += _activeItem._18Item;
                            break;
                        case 47:
                            GameManager.instance.activeDelegate += _activeItem._19Item;
                            break;
                        case 48:
                            GameManager.instance.activeDelegate += _activeItem._20Item;
                            break;
                        default:
                            break;
                    }
                }
                _rewardslot.ClearSlot();    // 다음 웨이브 보상 때 새로운 랜덤 보상으로 바꿔야 하기에 슬롯 초기화
            }
        rewardUI.SetActive(false);    // 보상 창을 닫고
        waveStart.SendMessage("StartWave");   // StartWave 함수 실행
        reUI.AddRewardRandomItems(items);   // 새로운 아이템을 보상 슬롯에 추가
        }
        else  // 스테이지가 끝났다면,
        {
            rewardUI.SetActive(false);    // 창만 닫기
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> openPopups = new List<GameObject>();

    public GameObject statWindow;
    public GameObject pauseWindow;
    public GameObject fieldUI;

    //캐릭터 선택 UI
    [Header("Select_Character_UI")]
    public GameObject[] characterSelectionUI;   //선택된 캐릭터를 표시되는 프레임 이미지(오브젝트)
    public GameObject[] charactersUltimateUI;   //선택되지 않은 캐릭터들의 궁극기 스킬 UI(오브젝트)
    private int[] chargeCount;                  //캐릭터별 충전된 궁극기 횟수
    //private float[] skillCoolTime;              
    private GameObject[] chargeCountParent;     //스킬 충전 UI 오브젝트 (부모)
    //public GameObject[] skillCoolTimeParent;
    //private TMP_Text[] skillCoolTime_Txt;

    //현재 캐릭터 스킬 UI
    public GameObject[] skillActiveFrame;       //활성화된 현재 캐릭터 스킬의 프레임 UI
    public GameObject[] skillCoolTimeUI;        //현재 캐릭터 스킬 쿨타임 UI(오브젝트)
    public GameObject[] skillChargeUI;          //현재 캐릭터 궁극기 충전 UI(오브젝트)
    public TMP_Text[] skillCoolTimeText;        //현재 캐릭터 스킬 쿨타임 Text

    public enum Skill
    {
        Q = 0,
        E = 1,
        R = 2,
    }

    //상점 UI
    private GameObject prevShowImage;

    [Header("Text_UI")]
    public TMP_Text wave_Txt;
    public TMP_Text health_Txt;
    public TMP_Text coin_Txt;

    [Header("Manager")]
    public spawner1 spawner;
    private GameManager gameManager;

    [Header("AutoPotion")]
    public AutoPotion[] autoPotion;

    private void Start()
    {
        gameManager = GameManager.instance;

        chargeCount = new int[characterSelectionUI.Length];                  //임의로 한 것. 나중에 캐릭터 개수만큼 배열 생성
        chargeCountParent = new GameObject[characterSelectionUI.Length];     //임의로 한 것. 나중에 캐릭터 개수만큼 배열 생성


        for (int index = 0; index < characterSelectionUI.Length; index++)
        {
            chargeCountParent[index] = charactersUltimateUI[index].transform.GetChild(0).GetChild(2).gameObject;
        }

        //for (int index = 0; index < charactersUltimateUI.Length; index++)
        //{
        //    skillCoolTimeParent[index] = charactersUltimateUI[index].transform.GetChild(0).GetChild(1).gameObject;
        //}
    }

    void Update()
    {
        SetWaveText();
        SetHPText();
        SetCoinText();

        //팝업창 닫기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (openPopups.Count > 0)
                CloseLastOpenedPopup();
            else
            {
                OpenPopup(pauseWindow);
            }
        }

        //플레이어 정보 열고 닫기
        if (Input.GetKey(KeyCode.Tab))
        {
            SetUIActive(statWindow, true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            SetUIActive(statWindow, false);
        }
    }

    //UI팝업창 열기
    public void OpenPopup(GameObject popup)
    {
        if (popup != null && !openPopups.Contains(popup))
        {
            SetUIActive(popup, true);
            openPopups.Add(popup);
            gameManager.isPause = true;
        }

        if (popup == pauseWindow)
        {
            Time.timeScale = 0f;
            gameManager.isPause = true;
        }
    }

    //UI팝업창 닫기
    public void ClosePopup(GameObject popup)
    {
        if (popup != null && openPopups.Contains(popup))
        {
            SetUIActive(popup, false);
            openPopups.Remove(popup);
            gameManager.isPause = false;
        }

        if (popup == pauseWindow)
        {
            Time.timeScale = 1f;
        }

        if (openPopups.Count <= 0)
            gameManager.isPause = false;
    }

    //가장 최근에 열린 팝업창 닫기
    public void CloseLastOpenedPopup()
    {
        if (openPopups.Count > 0)
        {
            ClosePopup(openPopups.Last());
        }
    }

    //모든 팝업창 닫기
    public void CloseAllOpenPopups()
    {
        while (openPopups.Count > 0)
        {
            ClosePopup(openPopups.Last());
        }
    }

    //UI Active 설정
    public void SetUIActive(GameObject uiObject, bool isActive)
    {
        if (uiObject != null)
        {
            uiObject.SetActive(isActive);
        }
    }

    public void SetWaveText()
    {
        //wave_Txt.text = string.Format("WAVE {0}", spawner.);
    }

    public void SetHPText()
    {
        health_Txt.text = string.Format("{0}/{1}", gameManager._hp, gameManager._maxhp);
    }

    public void SetCoinText()
    {
        coin_Txt.text = string.Format("{0} Gold", gameManager._gold);
    }

    //선택된 캐릭터 UI 표시
    public void ChangeCharacterUI(int num)
    {
        SetUIActive(characterSelectionUI[num], true);
        SetUIActive(charactersUltimateUI[num], false);

        for (int index = 0; index < characterSelectionUI.Length; index++)  //선택 가능한 캐릭터 개수로 변경할 것
        {
            if (index == num)
                continue;

            SetUIActive(characterSelectionUI[index], false);
            SetUIActive(charactersUltimateUI[index], true);
        }
    }

    //스킬  충전
    public void ChargingSkillCount(int index)
    {
        chargeCount[index] = Mathf.Clamp(++chargeCount[index], 0, 3);
        SetUIActive(chargeCountParent[index].transform.GetChild(chargeCount[index] - 1).gameObject, true);
    }

    //스킬 사용
    public void UseSkillCount(int index)
    {
        chargeCount[index] = Mathf.Clamp(--chargeCount[index], 0, 3);
        SetUIActive(chargeCountParent[index].transform.GetChild(chargeCount[index]).gameObject, false);
    }

    //상점 스크롤바 초기화
    public void InitScrollbarValue(Scrollbar scrollbar)
    {
        if (scrollbar != null)
            scrollbar.value = 1;
    }
}

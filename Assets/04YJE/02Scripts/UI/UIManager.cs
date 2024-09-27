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

    public Image innShowObj;
    public Image wizardShowObj;
    public Image blacksmithShowObj;

    //캐릭터 선택 UI
    [Header("Select_Character_UI")]
    public GameObject[] characterSelectionUI;
    public GameObject[] charactersUltimateUI;
    private int[] chargeCount;
    private float[] skillCoolTime;
    public GameObject[] chargeCountParent;
    public GameObject[] skillCoolTimeParent;
    private Text[] skillCoolTime_Txt;

    //상점 UI
    private GameObject prevShowImage;

    [Header("Text_UI")]
    public TMP_Text wave_Txt;
    public TMP_Text health_Txt;
    public TMP_Text coin_Txt;

    [Header("Manager")]
    public spawner1 spawner;
    private GameManager gameManager;

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
        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            SetUIActive(statWindow, false);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeCharacterUI(0);
        }
        else if( Input.GetKeyUp(KeyCode.Alpha2))
        {
            ChangeCharacterUI(1);
        }
        else if( Input.GetKeyUp(KeyCode.Alpha3))
        {
            ChangeCharacterUI(2);
        }
    }

    //UI팝업창 열기
    public void OpenPopup(GameObject popup) 
    {
        if(popup != null && !openPopups.Contains(popup))
        {
            SetUIActive(popup, true);
            openPopups.Add(popup);
        }

        if (popup == pauseWindow)
            Time.timeScale = 0f;
    }

    //UI팝업창 닫기
    public void ClosePopup(GameObject popup)
    {
        if(popup != null && openPopups.Contains(popup))
        {
            SetUIActive(popup, false);
            openPopups.Remove(popup);
        }

        if (popup == pauseWindow && !pauseWindow.activeSelf)
            Time.timeScale = 1f;
    }

    //가장 최근에 열린 팝업창 닫기
    public void CloseLastOpenedPopup()
    {
        if(openPopups.Count > 0)
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

        for(int index = 0; index < characterSelectionUI.Length; index++)  //선택 가능한 캐릭터 개수로 변경할 것
        {
            if (index == num)
                continue;

            SetUIActive(characterSelectionUI[index], false);
            SetUIActive(charactersUltimateUI[index], true);
        }
    }

    //사용 가능 스킬 횟수 충전
    public void ChargingSkillCount(int index)
    {
        chargeCount[index] = Mathf.Clamp(++chargeCount[index], 0, 3);
        Debug.Log(chargeCount[index]);
        SetUIActive(chargeCountParent[index].transform.GetChild(chargeCount[index] -1).gameObject, true);
    }

    public void UseSkillCount(int index)
    {
        chargeCount[index] = Mathf.Clamp(--chargeCount[index],0,3);
        
        Debug.Log(chargeCount[index]);
        SetUIActive(chargeCountParent[index].transform.GetChild(chargeCount[index]).gameObject, false);
    }

    public void InitScrollbarValue(Scrollbar scrollbar)
    {
        if(scrollbar != null)
            scrollbar.value = 1;
    }
}

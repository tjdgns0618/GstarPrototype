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

    //ĳ���� ���� UI
    [Header("Select_Character_UI")]
    public GameObject[] characterSelectionUI;   //���õ� ĳ���͸� ǥ�õǴ� ������ �̹���(������Ʈ)
    public GameObject[] charactersUltimateUI;   //���õ��� ���� ĳ���͵��� �ñر� ��ų UI(������Ʈ)
    private int[] chargeCount;                  //ĳ���ͺ� ������ �ñر� Ƚ��
    //private float[] skillCoolTime;              
    private GameObject[] chargeCountParent;     //��ų ���� UI ������Ʈ (�θ�)
    //public GameObject[] skillCoolTimeParent;
    //private TMP_Text[] skillCoolTime_Txt;

    //���� ĳ���� ��ų UI
    public GameObject[] skillActiveFrame;       //Ȱ��ȭ�� ���� ĳ���� ��ų�� ������ UI
    public GameObject[] skillCoolTimeUI;        //���� ĳ���� ��ų ��Ÿ�� UI(������Ʈ)
    public GameObject[] skillChargeUI;          //���� ĳ���� �ñر� ���� UI(������Ʈ)
    public TMP_Text[] skillCoolTimeText;        //���� ĳ���� ��ų ��Ÿ�� Text

    public enum Skill
    {
        Q = 0,
        E = 1,
        R = 2,
    }

    //���� UI
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

        chargeCount = new int[characterSelectionUI.Length];                  //���Ƿ� �� ��. ���߿� ĳ���� ������ŭ �迭 ����
        chargeCountParent = new GameObject[characterSelectionUI.Length];     //���Ƿ� �� ��. ���߿� ĳ���� ������ŭ �迭 ����


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

        //�˾�â �ݱ�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (openPopups.Count > 0)
                CloseLastOpenedPopup();
            else
            {
                OpenPopup(pauseWindow);
            }
        }

        //�÷��̾� ���� ���� �ݱ�
        if (Input.GetKey(KeyCode.Tab))
        {
            SetUIActive(statWindow, true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            SetUIActive(statWindow, false);
        }
    }

    //UI�˾�â ����
    public void OpenPopup(GameObject popup)
    {
        if (popup != null && !openPopups.Contains(popup))
        {
            SetUIActive(popup, true);
            openPopups.Add(popup);
        }

        if (popup == pauseWindow)
            Time.timeScale = 0f;
    }

    //UI�˾�â �ݱ�
    public void ClosePopup(GameObject popup)
    {
        if (popup != null && openPopups.Contains(popup))
        {
            SetUIActive(popup, false);
            openPopups.Remove(popup);
        }

        if (popup == pauseWindow && !pauseWindow.activeSelf)
            Time.timeScale = 1f;
    }

    //���� �ֱٿ� ���� �˾�â �ݱ�
    public void CloseLastOpenedPopup()
    {
        if (openPopups.Count > 0)
        {
            ClosePopup(openPopups.Last());
        }
    }

    //��� �˾�â �ݱ�
    public void CloseAllOpenPopups()
    {
        while (openPopups.Count > 0)
        {
            ClosePopup(openPopups.Last());
        }
    }

    //UI Active ����
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

    //���õ� ĳ���� UI ǥ��
    public void ChangeCharacterUI(int num)
    {
        SetUIActive(characterSelectionUI[num], true);
        SetUIActive(charactersUltimateUI[num], false);

        for (int index = 0; index < characterSelectionUI.Length; index++)  //���� ������ ĳ���� ������ ������ ��
        {
            if (index == num)
                continue;

            SetUIActive(characterSelectionUI[index], false);
            SetUIActive(charactersUltimateUI[index], true);
        }
    }

    //��ų  ����
    public void ChargingSkillCount(int index)
    {
        chargeCount[index] = Mathf.Clamp(++chargeCount[index], 0, 3);
        SetUIActive(chargeCountParent[index].transform.GetChild(chargeCount[index] - 1).gameObject, true);
    }

    //��ų ���
    public void UseSkillCount(int index)
    {
        chargeCount[index] = Mathf.Clamp(--chargeCount[index], 0, 3);
        SetUIActive(chargeCountParent[index].transform.GetChild(chargeCount[index]).gameObject, false);
    }

    //���� ��ũ�ѹ� �ʱ�ȭ
    public void InitScrollbarValue(Scrollbar scrollbar)
    {
        if (scrollbar != null)
            scrollbar.value = 1;
    }
}

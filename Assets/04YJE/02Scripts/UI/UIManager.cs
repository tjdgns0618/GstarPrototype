using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> openPopups = new List<GameObject>();

    public GameObject statWindow;
    public GameObject pauseWindow;
    public GameObject waveUI;
    public GameObject[] characterSelectionUI;
    public GameObject[] charactersUltimateUI;
    private GameObject prevShowImage;

    [Header("Text_UI")]
    public TMP_Text wave_Txt;
    public TMP_Text health_Txt;
    public TMP_Text coin_Txt;




    public spawner1 spawner;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
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
                OpenPopup(pauseWindow);
        }

        //�÷��̾� ���� ���� �ݱ�
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

    //UI�˾�â ����
    public void OpenPopup(GameObject popup) 
    {
        if(popup != null && !openPopups.Contains(popup))
        {
            SetUIActive(popup, true);
            openPopups.Add(popup);
        }
    }

    //UI�˾�â �ݱ�
    public void ClosePopup(GameObject popup)
    {
        if(popup != null && openPopups.Contains(popup))
        {
            SetUIActive(popup, false);
            openPopups.Remove(popup);
        }
    }

    //���� �ֱٿ� ���� �˾�â �ݱ�
    public void CloseLastOpenedPopup()
    {
        if(openPopups.Count > 0)
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

        for(int index = 0; index < characterSelectionUI.Length; index++)  //���� ������ ĳ���� ������ ������ ��
        {
            if (index == num)
                continue;

            SetUIActive(characterSelectionUI[index], false);
            SetUIActive(charactersUltimateUI[index], true);
        }
    }
}

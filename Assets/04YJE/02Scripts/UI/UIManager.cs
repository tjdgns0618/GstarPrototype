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

        //ÆË¾÷Ã¢ ´Ý±â
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (openPopups.Count > 0)
                CloseLastOpenedPopup();
            else
                OpenPopup(pauseWindow);
        }

        //ÇÃ·¹ÀÌ¾î Á¤º¸ ¿­°í ´Ý±â
        if (Input.GetKey(KeyCode.Tab))
        {
            SetUIActive(statWindow, true);
        }
        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            SetUIActive(statWindow, false);
        }
    }

    //UIÆË¾÷Ã¢ ¿­±â
    public void OpenPopup(GameObject popup) 
    {
        if(popup != null && !openPopups.Contains(popup))
        {
            SetUIActive(popup, true);
            openPopups.Add(popup);
        }
    }

    //UIÆË¾÷Ã¢ ´Ý±â
    public void ClosePopup(GameObject popup)
    {
        if(popup != null && openPopups.Contains(popup))
        {
            SetUIActive(popup, false);
            openPopups.Remove(popup);
        }
    }

    //°¡Àå ÃÖ±Ù¿¡ ¿­¸° ÆË¾÷Ã¢ ´Ý±â
    public void CloseLastOpenedPopup()
    {
        if(openPopups.Count > 0)
        {
            ClosePopup(openPopups.Last());
        }
    }

    //¸ðµç ÆË¾÷Ã¢ ´Ý±â
    public void CloseAllOpenPopups()
    {
        while (openPopups.Count > 0)
        {
            ClosePopup(openPopups.Last());
        }
    }

    //UI Active ¼³Á¤
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
}

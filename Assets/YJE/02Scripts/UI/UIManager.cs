using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> openPopups = new List<GameObject>();

    [SerializeField] private GameObject statWindow;
    [SerializeField] private GameObject pauseWindow;

    void Update()
    {
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
        if(popup != null)
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
}

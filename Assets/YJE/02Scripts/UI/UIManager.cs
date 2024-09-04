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
    }

    //UI�˾�â ����
    public void OpenPopup(GameObject popup) 
    {
        if(popup != null)
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
}

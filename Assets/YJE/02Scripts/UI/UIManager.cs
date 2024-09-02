using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();

                if (instance == null)
                    Debug.LogError("UIManager가 씬에 존재하지 않습니다.");
            }

            return instance;
        }
    }

    private Stack<UIPopup> openPopups = new Stack<UIPopup>();
    private Queue<UIPopup> pendingPopups = new Queue<UIPopup>();


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseLastOpenedPopup();
        }
    }

    public void OpenPopup(UIPopup popup)
    {
        if(popup != null)
        {
            //새로운 팝업 열기
            openPopups.Push(popup);
        }
    }

    public void ClosePopup(UIPopup popup)
    {
        if(popup != null && openPopups.Contains(popup))
        {
            //팝업 닫기
            openPopups.Pop();
        }
    }

    public void CloseLastOpenedPopup()
    {
        if(openPopups.Count > 0)
        {
            ClosePopup(openPopups.Peek());
        }
    }

    public void CloseAllOpenPopups()
    {
        while (openPopups.Count > 0)
        {
            ClosePopup(openPopups.Peek());
        }
    }
}

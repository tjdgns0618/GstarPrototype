using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    static public SceneChanger instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("YJE_MainScene");
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}

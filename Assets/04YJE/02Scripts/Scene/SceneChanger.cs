using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    SoundManager soundManager;

    private void Start()
    {
        soundManager = SoundManager.instance;
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(1);
        soundManager.PlayMusic(1,2f);
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
        soundManager.PlayMusic(0, 2f);
    }
}

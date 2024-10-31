using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    SoundManager soundManager;
    FadeInOut fadeInOut;
    WaitForSeconds fadeTime = new WaitForSeconds(2f);

    private void Start()
    {
        soundManager = SoundManager.instance;
        fadeInOut = FindObjectOfType<FadeInOut>();
    }

    public void LoadMainScene()
    {
        fadeInOut.FadeOut();
        StartCoroutine(_LoadMainScene());
    }

    public void LoadTitleScene()
    {
        fadeInOut.FadeOut();
        StartCoroutine(_LoadTitleScene());
    }

    public void LoadTutorialScene()
    {

    }

    IEnumerator _LoadMainScene()
    {
        yield return fadeTime;

        SceneManager.LoadScene(1);
        soundManager.PlayMusic(1, 2f);
    }

    IEnumerator _LoadTitleScene()
    {
        yield return fadeTime;

        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
        soundManager.PlayMusic(0, 2f);
    }

    IEnumerator _LoadTutorialScene()
    {
        yield return fadeTime;

        //로드씬함수 호출
    }
}

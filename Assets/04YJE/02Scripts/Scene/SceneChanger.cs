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
        fadeInOut.FadeOut();
        StartCoroutine(_LoadTutorialScene());
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

        SceneManager.LoadScene(1);
        soundManager.PlayMusic(1, 2f);
        //로드씬함수 호출
    }

    IEnumerator _LoadMainScene()
    {
        yield return fadeTime;

        SceneManager.LoadScene(2);
        soundManager.PlayMusic(1, 2f);
    }

    public void Quit()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

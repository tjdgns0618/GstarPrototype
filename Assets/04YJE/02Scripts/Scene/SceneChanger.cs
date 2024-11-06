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

    bool isLoading = false;

    private void Start()
    {
        isLoading = false;
        soundManager = SoundManager.instance;
        fadeInOut = FindObjectOfType<FadeInOut>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha8))
            LoadEndingCutScene();
    }

    public void LoadMainScene()
    {
        if (isLoading)
            return;

        fadeInOut.FadeOut();
        StartCoroutine(_LoadMainScene());
    }

    public void LoadTitleScene()
    {
        if (isLoading)
            return;

        Time.timeScale = 1.0f;
        fadeInOut.FadeOut();
        StartCoroutine(_LoadTitleScene());
    }

    public void LoadTutorialScene()
    {
        if (isLoading)
            return;

        fadeInOut.FadeOut();
        StartCoroutine(_LoadTutorialScene());
    }

    public void LoadEndingCutScene()
    {
        if (isLoading)
            return;

        fadeInOut = FindObjectOfType<FadeInOut>();
        SceneManager.LoadScene(3);
        soundManager.PlayMusic(5, 2f);
    }

    IEnumerator _LoadTitleScene()
    {
        isLoading = true;
        yield return fadeTime;

        SceneManager.LoadScene(0);
        soundManager.PlayMusic(0, 2f);
    }
    IEnumerator _LoadTutorialScene()
    {
        isLoading = true;
        yield return fadeTime;

        SceneManager.LoadScene(1);
        soundManager.PlayMusic(1, 2f);
        //로드씬함수 호출
    }

    IEnumerator _LoadMainScene()
    {
        isLoading = true;
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

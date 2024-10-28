using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioMixer audioMixer;

    public AudioSource bgmSource;
    public AudioClip[] bgmLists;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    

    public void PlayMusic(int index)
    {
        bgmSource.clip = bgmLists[index];
    }

    public void SetBGMVolume(float value)
    {
        if (audioMixer != null)
            audioMixer.SetFloat("BGM", value);
    }

    public void SetSFXVolume(float value)
    {
        if (audioMixer != null)
            audioMixer.SetFloat("SFX", value);
    }
}

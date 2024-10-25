using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioMixer audioMixer;

    public AudioSource bgmSource;
    public AudioClip[] bgmLists;

    private void Awake()
    {
        if(instance == null)
            instance = this;        

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetSFXVolume(10);
        SetBGMVolume(-80);
    }

    public void PlayMusic(int index, float fadeDuration = 0.5f)
    {
        bgmSource.clip = bgmLists[index];
    }

    public void MuteBGM()
    {
        bgmSource.mute = true;
    }

    public void UnmuteBGM()
    {
        bgmSource.mute = false;
    }

    public void MuteSFX()
    {
    }

    public void SetSFXVolume(float volume)
    {
        if (audioMixer != null)
            audioMixer.SetFloat("SFX", volume);
    }

    public void SetBGMVolume(float volume)
    {
        if(audioMixer != null)
            audioMixer.SetFloat("BGM", volume);
    }
}

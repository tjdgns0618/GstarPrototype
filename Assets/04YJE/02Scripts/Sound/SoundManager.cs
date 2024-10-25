using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

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
    

    public void PlayMusic(int index)
    {
        bgmSource.clip = bgmLists[index];
    }

    public void SetBGMVolumeFromSlider(Slider slider)
    {
        if(slider != null)
            SetBGMVolume(slider.value);
    }

    public void SetSFXVolumeFromSlider(Slider slider)
    {
        if(slider != null)
            SetSFXVolume(slider.value);
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

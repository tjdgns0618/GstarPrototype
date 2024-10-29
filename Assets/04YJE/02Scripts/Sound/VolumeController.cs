using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    private SoundManager soundManager;

    public AudioMixer audioMixer;

    public Slider[] slider;                 //0: BGM, 1:SFX

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        slider[0].value = GetBGMVolume();
        slider[1].value = GetSFXVolume();
    }

    public void SetBGMVolumeFromSlider()
    {
        SetBGMVolume(slider[0].value);
    }

    public void SetSFXVolumeFromSlider()
    {
        SetSFXVolume(slider[1].value);
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

    public float GetBGMVolume()
    {
        float volume;
        audioMixer.GetFloat("BGM", out volume);

        return volume;
    }

    public float GetSFXVolume()
    {
        float volume;
        audioMixer.GetFloat("SFX", out volume);

        return volume;
    }
}

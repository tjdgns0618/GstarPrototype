using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private SoundManager soundManager;

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void CallSetBGMVolumeFunc(Slider slider)
    {
        soundManager.SetBGMVolume(slider.value);
    }

    public void CallSetSFXVolumeFunc(Slider slider)
    {
        soundManager.SetSFXVolume(slider.value);
    }
}

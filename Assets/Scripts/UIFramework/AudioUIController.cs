using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore;
using UnityEngine.UI;
using Universal.AudioSystem;

public class AudioUIController : MonoBehaviour
{
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;

    private void OnEnable()
    {
        bgmSlider.value = SoundManager.Instance.BgmSource.volume;
        sfxSlider.value = SoundManager.Instance.SfxSource.volume;
    }

    public void SetBGMVolume()
    {
        SoundManager.Instance.BgmSource.volume = bgmSlider.value;
    }

    public void SetSFXVolume()
    {
        SoundManager.Instance.SfxSource.volume = sfxSlider.value;
    }
}

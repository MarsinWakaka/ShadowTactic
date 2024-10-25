using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using Utilities;

public class VideoUIController : MonoBehaviour
{
    [SerializeField] Slider FrameRateSlider;
    [SerializeField] TextMeshProUGUI FrameRateTextBox;
    [SerializeField] Toggle styleToggle;

    private void OnEnable()
    {
        FrameRateSlider.value = GameManager.Instance.targetFrameRate;
        //Debug.Log($"{DateTime.Now}styleToggle.isOn:{styleToggle.isOn} <= FilmStyleActive:{VolumeController.Instance.IsFilmStyleEnabled()}");
        styleToggle.isOn = VolumeController.Instance.IsFilmStyleEnabled();
    }

    public void SetFrameRate()
    {
        int rate = (int)FrameRateSlider.value;
        GameManager.Instance.SetFrameRate(rate);
        FrameRateTextBox.text = rate.ToString();
    }

    public void UpdateVideoStyle()
    {
        bool state = styleToggle.isOn;
        //Debug.Log($"{ DateTime.Now}styleToggle.isOn:{ styleToggle.isOn} => FilmStyleActive:{ VolumeController.Instance.IsFilmStyleEnabled()}");
        VolumeController.Instance.SetFilmStyle(state);
    }
}
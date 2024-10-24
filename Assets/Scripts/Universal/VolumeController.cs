using Assets.Scripts.Universal;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeController : PersistentSingleton<VolumeController>
{
    Volume volume_Global;

    [SerializeField] float maxVigIntensity;
    float targetValue;

    Vignette vignette;
    Tonemapping tonemapping;

    Coroutine _coroutine;

    void Start()
    {
        volume_Global = GetComponent<Volume>();
            
        if(!volume_Global.profile.TryGet<Vignette>(out vignette))
        {
            //Debug.LogWarning("未找到Volume");
        }
        if(!volume_Global.profile.TryGet<Tonemapping>(out tonemapping))
        {
            //Debug.LogWarning("未找到Tonemapping");
        }

        if (vignette.intensity.value != 0f)
        {
            targetValue = maxVigIntensity - targetValue;
            SetVignetteInstensity(0);
        }
    }

    //private void Update()
    //{
    //    SetVignetteInstensity(Mathf.Sin(Time.time));
    //}

    public void SetVignetteInstensity(float x)
    {
        vignette.intensity.SetValue(new FloatParameter(x));
    }

    public void ChangeHidenEffect()
    {
        if (_coroutine == null)
        {
            _coroutine = StartCoroutine(ZoomOutVignette());
        }
        else
        {
            StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(ZoomOutVignette());
        }
    }

    private IEnumerator ZoomOutVignette()
    {
        targetValue = maxVigIntensity - targetValue;
        float currentValue = vignette.intensity.value;

        float timer = 0;

        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            float intensity = Mathf.Lerp(currentValue, targetValue, timer * 2);
            SetVignetteInstensity(intensity);
            yield return null;
        }
        _coroutine = null;
    }

    public void GameOverFX()
    {

    }

    public void TurnOnFilmStyle()
    {
        tonemapping.active = true;
    }

    public void TurnOffFilmStyle()
    {
        tonemapping.active = false;
    }

    public void SetFilmStyle(bool state)
    {
        if (state)
        {
            // 启用 TonemappingMode.ACES
            tonemapping.mode.overrideState = true;
            tonemapping.mode.value = TonemappingMode.ACES;
        }
        else
        {
            // 禁用 TonemappingMode
            tonemapping.mode.overrideState = false;
        }
    }

    public bool IsFilmStyleEnabled()
    {
        return tonemapping.mode.overrideState;
    }
}

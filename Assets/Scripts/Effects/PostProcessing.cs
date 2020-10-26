using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessing : MonoBehaviour
{
    [SerializeField] VolumeProfile profile;
    Vignette vignette;
    SplitToning toning;
    WhiteBalance balance;
    ColorAdjustments adjustment;

    static PostProcessing post;

    private void Start()
    {
        post = this;
        profile.TryGet<Vignette>(out vignette);
        profile.TryGet<SplitToning>(out toning);
        profile.TryGet<WhiteBalance>(out balance);
        profile.TryGet<ColorAdjustments>(out adjustment);
    }

    public static void SetVignette(float smoothness)
    {
        post.vignette.smoothness = new ClampedFloatParameter(smoothness, 0, 1);
    }

    public static void SetTemperature(float temperature)
    {
        post.balance.temperature = new ClampedFloatParameter(temperature, -100, 100);
    }

    public static void SetContrast(float contrast)
    {
        post.adjustment.contrast = new ClampedFloatParameter(contrast, -100, 100);
    }

    public static void SetSaturation(float saturation)
    {
        post.adjustment.saturation = new ClampedFloatParameter(saturation, -100, 100);
    }
}

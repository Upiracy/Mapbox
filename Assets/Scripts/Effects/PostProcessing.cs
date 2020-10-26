using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessing : MonoBehaviour
{
    [SerializeField] Light directionLight;
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

    public static void SetVignette(float intensity, float smoothness)
    {
        post.vignette.intensity = new ClampedFloatParameter(intensity, 0, 1);
        post.vignette.smoothness = new ClampedFloatParameter(smoothness, 0, 1);
    }

    public static void SetTemperature(float temperature)
    {
        post.balance.temperature = new ClampedFloatParameter(temperature, -100, 100);
    }

    public static void SetTint(float tint)
    {
        post.balance.tint = new ClampedFloatParameter(tint, -100, 100);
    }

    public static void SetContrast(float contrast)
    {
        post.adjustment.contrast = new ClampedFloatParameter(contrast, -100, 100);
    }

    public static void SetSaturation(float saturation)
    {
        post.adjustment.saturation = new ClampedFloatParameter(saturation, -100, 100);
    }

    public static void SetGlobalLight(float intensity)
    {
        post.directionLight.intensity = intensity;
    }

    public static void SetAmbientLight(float intensity, Color color)
    {
        RenderSettings.ambientIntensity = intensity;
        RenderSettings.ambientLight = color;
    }
    

}

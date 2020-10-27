using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    static float intensity, smoothness, temperature, tint, contrast, saturation, lightValue;

    private void Awake()
    {
        post = this;
        profile.TryGet<Vignette>(out vignette);
        profile.TryGet<SplitToning>(out toning);
        profile.TryGet<WhiteBalance>(out balance);
        profile.TryGet<ColorAdjustments>(out adjustment);
    }

    public static void SetVignette(float intensity, float smoothness)
    {
        //post.vignette.intensity = new ClampedFloatParameter(intensity, 0, 1);
        //post.vignette.smoothness = new ClampedFloatParameter(smoothness, 0, 1);
        PostProcessing.intensity = intensity;
        PostProcessing.smoothness = smoothness;
        //post.vignette.intensity.Override(intensity);
        //post.vignette.smoothness.Override(smoothness);
    }

    public static void SetTemperature(float temperature)
    {
        //post.balance.temperature = new ClampedFloatParameter(temperature, -100, 100);
        PostProcessing.temperature = temperature;
        //post.balance.temperature.Override(temperature);
    }

    public static void SetTint(float tint)
    {
        //post.balance.tint = new ClampedFloatParameter(tint, -100, 100);
        PostProcessing.tint = tint;
        //post.balance.tint.Override(tint);
    }

    public static void SetContrast(float contrast)
    {
        //post.adjustment.contrast = new ClampedFloatParameter(contrast, -100, 100);
        PostProcessing.contrast = contrast;
        //post.adjustment.contrast.Override(contrast);
    }

    public static void SetSaturation(float saturation)
    {
        //post.adjustment.saturation = new ClampedFloatParameter(saturation, -100, 100);
        PostProcessing.saturation = saturation;
        //post.adjustment.saturation.Override(saturation);
    }

    public static void SetGlobalLight(float intensity)
    {
        PostProcessing.lightValue = intensity;
        //post.directionLight.intensity = intensity;
    }

    public static void SetAmbientLight(float intensity, Color color)
    {
        RenderSettings.ambientIntensity = intensity;
        RenderSettings.ambientLight = color;
    }

    private void Update()
    {
        float intensity, smoothness, temperature, tint, contrast, saturation, lightValue;
        intensity = post.vignette.intensity.value;
        smoothness = post.vignette.smoothness.value;
        temperature = post.balance.temperature.value;
        tint = post.balance.tint.value;
        contrast = post.adjustment.contrast.value;
        saturation = post.adjustment.saturation.value;
        lightValue = post.directionLight.intensity;

        if (PostProcessing.intensity > intensity + 0.005f) post.vignette.intensity.Override(intensity + 0.01f);
        else if(PostProcessing.intensity < intensity - 0.005f) post.vignette.intensity.Override(intensity - 0.01f);

        if (PostProcessing.smoothness > smoothness + 0.005f) post.vignette.smoothness.Override(smoothness + 0.01f);
        else if (PostProcessing.smoothness < smoothness - 0.005f) post.vignette.smoothness.Override(smoothness - 0.01f);

        if (PostProcessing.temperature > temperature) post.balance.temperature.Override(temperature + 1);
        else if (PostProcessing.temperature < temperature) post.balance.temperature.Override(temperature - 1);

        if (PostProcessing.tint > tint) post.balance.tint.Override(tint + 1);
        else if (PostProcessing.tint < tint) post.balance.tint.Override(tint - 1);

        if (PostProcessing.contrast > contrast) post.adjustment.contrast.Override(contrast + 1);
        else if (PostProcessing.contrast < contrast) post.adjustment.contrast.Override(contrast - 1);

        if (PostProcessing.saturation > saturation) post.adjustment.saturation.Override(saturation + 1);
        else if (PostProcessing.saturation < saturation) post.adjustment.saturation.Override(saturation - 1);

        if (PostProcessing.lightValue > lightValue + 0.01f) post.directionLight.intensity = lightValue + 0.01f;
        else if (PostProcessing.lightValue < lightValue - 0.01f) post.directionLight.intensity = lightValue - 0.01f;
    }

}

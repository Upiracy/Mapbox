#ifndef UNIVERSAL_LIT_INPUT_INCLUDED
#define UNIVERSAL_LIT_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Assets/AkilliMum/SRP/Mirror/Base/URP/SurfaceInput.hlsl"

CBUFFER_START(UnityPerMaterial)
float4 _BaseMap_ST;
half4 _BaseColor;
half4 _SpecColor;
half4 _EmissionColor;
half _Cutoff;
half _Smoothness;   
half _Metallic;
half _BumpScale;
half _OcclusionStrength;




float4 _ReflectionTex_ST;
float4 _ReflectionTexOther_ST;
float4 _ReflectionTexDepth_ST;
float4 _ReflectionTexOtherDepth_ST;
float4 _RefractionTex_ST;
float4 _MaskTex_ST;
float4 _RippleTex_ST;
float4 _WaveNoiseTex_ST;

half _EnableDepthBlur;

half _EnableSimpleDepth;
float _SimpleDepthCutoff;
//float _DepthBlur;
float _NearClip;
float _FarClip;

half _ReflectionIntensity;
float _LODLevel;
//float _WetLevel;
float _MixBlackColor;

half _EnableRefraction;
half _ReflectionRefraction;

half _EnableMask;
half _MaskCutoff;
half _MaskEdgeDarkness;
half4 _MaskTiling;

//half _UseOpaqueCamImage; //todo:

half _EnableWave;
half _WaveSize;
half _WaveDistortion;
half _WaveSpeed;

half _EnableRipple;
half _RippleSize;
half _RippleRefraction;
half _RippleDensity;
half _RippleSpeed;

half _Surface;
float _WorkType;
float _DeviceType;

CBUFFER_END

TEXTURE2D(_CameraDepthTexture); SAMPLER(sampler_CameraDepthTexture);
TEXTURE2D(_CameraOpaqueTexture); SAMPLER(sampler_CameraOpaqueTexture_linear_clamp);
			
TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);
TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);

TEXTURE2D(_RefractionTex);       SAMPLER(sampler_RefractionTex);

TEXTURE2D(_ReflectionTex);       SAMPLER(sampler_ReflectionTex);
TEXTURE2D(_ReflectionTexOther);       SAMPLER(sampler_ReflectionTexOther);
TEXTURE2D(_ReflectionTexDepth);       SAMPLER(sampler_ReflectionTexDepth);
TEXTURE2D(_ReflectionTexOtherDepth);       SAMPLER(sampler_ReflectionTexOtherDepth);

TEXTURE2D(_MaskTex);       SAMPLER(sampler_MaskTex);

TEXTURE2D(_RippleTex);       SAMPLER(sampler_RippleTex);

TEXTURE2D(_WaveNoiseTex);       SAMPLER(sampler_WaveNoiseTex);


#ifdef _SPECULAR_SETUP
    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_SpecGlossMap, sampler_SpecGlossMap, uv)
#else
    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv)
#endif

half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
{
    half4 specGloss;

#ifdef _METALLICSPECGLOSSMAP
    specGloss = SAMPLE_METALLICSPECULAR(uv);
    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
        specGloss.a = albedoAlpha * _Smoothness;
    #else
        specGloss.a *= _Smoothness;
    #endif
#else // _METALLICSPECGLOSSMAP
    #if _SPECULAR_SETUP
        specGloss.rgb = _SpecColor.rgb;
    #else
        specGloss.rgb = _Metallic.rrr;
    #endif

    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
        specGloss.a = albedoAlpha * _Smoothness;
    #else
        specGloss.a = _Smoothness;
    #endif
#endif

    return specGloss;
}

half SampleOcclusion(float2 uv)
{
#ifdef _OCCLUSIONMAP
// TODO: Controls things like these by exposing SHADER_QUALITY levels (low, medium, high)
#if defined(SHADER_API_GLES)
    return SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
#else
    half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
    return LerpWhiteTo(occ, _OcclusionStrength);
#endif
#else
    return 1.0;
#endif
}

#define FLT_MAX 3.402823466e+38
#define FLT_MIN 1.175494351e-38
#define DBL_MAX 1.7976931348623158e+308
#define DBL_MIN 2.2250738585072014e-308

inline void InitializeStandardLitSurfaceData(float2 uv, out SurfaceData outSurfaceData)
{
    half4 albedoAlpha = SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
    outSurfaceData.alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);

    half4 specGloss = SampleMetallicSpecGloss(uv, albedoAlpha.a);
    outSurfaceData.albedo = albedoAlpha.rgb * _BaseColor.rgb;
       
#if _SPECULAR_SETUP
    outSurfaceData.metallic = 1.0h;
    outSurfaceData.specular = specGloss.rgb;
#else
    outSurfaceData.metallic = specGloss.r;
    outSurfaceData.specular = half3(0.0h, 0.0h, 0.0h);
#endif

    outSurfaceData.smoothness = specGloss.a;
    outSurfaceData.normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
    outSurfaceData.occlusion = SampleOcclusion(uv);
    outSurfaceData.emission = SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
}

#endif // UNIVERSAL_INPUT_SURFACE_PBR_INCLUDED

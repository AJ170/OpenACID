// Shared helpers for the ported AC Identity shaders.
// Every formula here mirrors the Unity 5.2.3 GLES programs found in the rip.
#ifndef ACID_COMMON_INCLUDED
#define ACID_COMMON_INCLUDED

#include "UnityCG.cginc"

// Globals pushed by game code through Shader.SetGlobal*.
half4 acid_Fog;                 // x: fog start, y: fog end, z: max fog amount
fixed4 _LightDir;               // xyz: light direction, w: directional light weight
fixed4 _LightParameters;        // x: SH scale, y: desaturation, z: AO strength
fixed4 _RimColor;
half2 _Shadows;                 // realtime shadow remap: max(x + atten * y, 0)
sampler2D _NoiseTexture;
sampler2D _RasterTex;

// Per-vertex fog weights: x = scene colour weight, y = fog colour weight.
inline half2 ACID_FogFactors(float4 vertex)
{
    float dist = length(UnityObjectToViewPos(vertex.xyz));
    half f = (1.0 - saturate((acid_Fog.y - dist) / (acid_Fog.y - acid_Fog.x))) * acid_Fog.z;
    return half2(1.0 - f, f);
}

inline half3 ACID_ApplyFog(half3 c, half2 fog)
{
    return unity_FogColor.rgb * fog.y + c * fog.x;
}

// L2 spherical harmonics, scaled, then converted linear -> gamma like the original.
inline half3 ACID_SHAmbient(half3 n, half scale)
{
    half4 n4 = half4(n, 1.0);
    half3 x1 = half3(dot(unity_SHAr, n4), dot(unity_SHAg, n4), dot(unity_SHAb, n4));
    half4 vB = n.xyzz * n.yzzx;
    half3 x2 = half3(dot(unity_SHBr, vB), dot(unity_SHBg, vB), dot(unity_SHBb, vB));
    half3 x3 = unity_SHC.rgb * (n.x * n.x - n.y * n.y);
    half3 sh = max((x1 + x2 + x3) * scale, half3(0.0, 0.0, 0.0));
    return max(1.055 * pow(sh, half3(0.4166667, 0.4166667, 0.4166667)) - 0.055, half3(0.0, 0.0, 0.0));
}

inline half3 ACID_Desaturate(half3 c, half amount)
{
    half lum = dot(c, half3(1.0, 1.0, 1.0)) * 0.3333333;
    return lerp(c, half3(lum, lum, lum), amount);
}

inline half ACID_ShadowFactor(half atten)
{
    return max(_Shadows.x + atten * _Shadows.y, 0.0);
}

// Noise driven dissolve band used by FX_NOISEGLOW and FANCY_FADE.
inline half ACID_NoiseBand(half noise, half layer)
{
    return layer - (0.9375 * noise + 0.03125);
}

inline half ACID_NoiseGlow(half noise, half layer)
{
    return saturate(1.0 - 32.0 * abs(ACID_NoiseBand(noise, layer)));
}

// RASTER_FADE: screen-space wipe. screen = ComputeScreenPos(pos).xyw. Discards
// the hidden part and returns the glow amount for the visible edge.
inline half ACID_RasterFade(float3 screen, half fadeAlpha)
{
    float2 uv = screen.xy / screen.z;
    half t = 1.5 * fadeAlpha - uv.y;
    clip(t);
    half glow = saturate(1.0 - t * 2.5);
    half r = tex2D(_RasterTex, float2(uv.x, saturate(glow - uv.y))).z;
    clip(r - 0.4);
    return glow;
}

// NGUI 2.x soft clipping factor.
inline half ACID_SoftClip(float2 clipPos, float2 sharpness)
{
    float2 f = (1.0 - abs(clipPos)) * sharpness;
    return saturate(min(f.x, f.y));
}

// The original shaders nudge clip-space z assuming OpenGL's [-w, w] range.
// Convert to that convention and back so the offsets stay identical on
// reversed-Z (D3D11 editor) and on PS Vita.
inline float ACID_ClipZToGL(float4 p)
{
#if defined(UNITY_REVERSED_Z)
    return p.w - 2.0 * p.z;
#elif defined(SHADER_API_WIIU) || defined(SHADER_API_D3D11_9X)
    return 2.0 * p.z - p.w;
#else
    return p.z;
#endif
}

inline float ACID_GLZToClip(float glz, float w)
{
#if defined(UNITY_REVERSED_Z)
    return (w - glz) * 0.5;
#elif defined(SHADER_API_WIIU) || defined(SHADER_API_D3D11_9X)
    return (glz + w) * 0.5;
#else
    return glz;
#endif
}

#endif // ACID_COMMON_INCLUDED

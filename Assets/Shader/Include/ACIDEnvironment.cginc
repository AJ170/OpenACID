// Environment/* shaders (static level geometry).
//
// Options:
//   ACID_ENV_BUMP     _BumpMap, lighting per pixel
//   ACID_ENV_CUTOUT   alpha test against _Cutoff
//   ACID_ENV_FADE     SIMPLE_FADE / FANCY_FADE / RASTER_FADE keywords (no realtime shadows,
//                     like the original)
// Keywords read: SHADOWS_SCREEN and the fade keywords. The original FX_HIGHLIGHT
// variants produced identical pixels, so that keyword is not compiled here.
//
// Without ACID_ENV_BUMP the normal only varies per vertex, so every lighting term
// (SH ambient, N.L, rim) is evaluated in the vertex shader. This is the bulk of
// the screen on Vita and leaves one texture fetch plus a few MADs per pixel.
#ifndef ACID_ENVIRONMENT_INCLUDED
#define ACID_ENVIRONMENT_INCLUDED

#include "ACIDCommon.cginc"
#include "AutoLight.cginc"

sampler2D _MainTex;
float4 _MainTex_ST;
sampler2D _BumpMap;
half _Cutoff;
half _ApplyRimIntensity;
fixed4 _FadeColor;

#if defined(SHADOWS_SCREEN) && !defined(ACID_ENV_FADE)
    #define ACID_ENV_SHADOWS 1
#endif

#if defined(ACID_ENV_FADE) && defined(RASTER_FADE)
    #define ACID_ENV_SCREENPOS 1
#endif

struct appdata_env
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float4 tangent : TANGENT;
    float4 texcoord : TEXCOORD0;
};

struct v2f_env
{
    float4 pos : SV_POSITION;
    half2 uv : TEXCOORD0;
    half2 fog : TEXCOORD1;
#if defined(ACID_ENV_BUMP)
    half3 normal : TEXCOORD2;
    half3 viewDir : TEXCOORD3;
    half4 tangent : TEXCOORD4;   // w: tangent sign
#else
    half3 ambient : TEXCOORD2;   // SH ambient * directional term
    half3 light : TEXCOORD3;     // x: N.L, y: rim (1 - N.V)
#endif
#if defined(ACID_ENV_SHADOWS)
    SHADOW_COORDS(5)
#endif
#if defined(ACID_ENV_SCREENPOS)
    float3 screen : TEXCOORD6;
#endif
};

v2f_env vertEnv(appdata_env v)
{
    v2f_env o;
    UNITY_INITIALIZE_OUTPUT(v2f_env, o);
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);

    float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
    half3 viewDir = normalize(_WorldSpaceCameraPos - worldPos);
    half3 worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
#if defined(ACID_ENV_BUMP)
    o.normal = worldNormal;
    o.viewDir = viewDir;
    o.tangent = half4(normalize(mul((float3x3)unity_ObjectToWorld, v.tangent.xyz)), v.tangent.w);
#else
    half ndl = max(dot(worldNormal, -_LightDir.xyz), 0.0);
    o.ambient = ACID_SHAmbient(worldNormal, _LightParameters.x) * (ndl * _LightDir.w + (1.0 - _LightDir.w));
    o.light = half3(ndl, 1.0 - dot(worldNormal, viewDir), 0.0);
#endif

    o.fog = ACID_FogFactors(v.vertex);
#if defined(ACID_ENV_SHADOWS)
    TRANSFER_SHADOW(o);
#endif
#if defined(ACID_ENV_SCREENPOS)
    o.screen = ComputeScreenPos(o.pos).xyw;
#endif
    return o;
}

fixed4 fragEnv(v2f_env i) : SV_Target
{
    fixed4 tex = tex2D(_MainTex, i.uv);
#if defined(ACID_ENV_CUTOUT)
    clip(tex.a - _Cutoff);
#endif

#if defined(ACID_ENV_BUMP)
    half3 tn = UnpackNormal(tex2D(_BumpMap, i.uv));
    half3 binormal = cross(i.normal, i.tangent.xyz) * i.tangent.w;
    half3 N = tn.x * i.tangent.xyz + tn.y * binormal + tn.z * i.normal;
    half ndl = max(dot(N, -_LightDir.xyz), 0.0);
    half rim = 1.0 - dot(N, i.viewDir);
    half3 lit = ACID_SHAmbient(N, _LightParameters.x) * tex.rgb * (ndl * _LightDir.w + (1.0 - _LightDir.w));
#else
    half ndl = i.light.x;
    half rim = i.light.y;
    half3 lit = i.ambient * tex.rgb;
#endif

#if defined(ACID_ENV_SHADOWS)
    lit *= (ACID_ShadowFactor(SHADOW_ATTENUATION(i)) * _LightDir.w + 1.0) - _LightDir.w;
#endif
    lit = ACID_Desaturate(lit, ndl * _LightParameters.y);

#if defined(ACID_ENV_FADE)
    fixed4 c = fixed4(lit, 1.0);
  #if defined(SIMPLE_FADE)
    c *= _FadeColor;
  #elif defined(FANCY_FADE)
    c.rgb = _FadeColor.rgb * (ACID_NoiseGlow(tex2D(_NoiseTexture, i.uv).y, _FadeColor.a) + c.rgb);
  #elif defined(RASTER_FADE)
    c.rgb += _FadeColor.rgb * ACID_RasterFade(i.screen, _FadeColor.a);
  #endif
#else
    fixed4 c = fixed4(lit, _RimColor.a);
#endif

    c.rgb += (rim * _ApplyRimIntensity + (1.0 - _ApplyRimIntensity)) * _RimColor.rgb;
    c.rgb = ACID_ApplyFog(c.rgb, i.fog);
    return c;
}

#endif // ACID_ENVIRONMENT_INCLUDED

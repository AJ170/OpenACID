// Character/* shaders (body, hair, head, props, tinted variants, fades).
//
// Pick exactly one surface type:
//   ACID_CHAR_BODY   _MaskTex: z specular mask, w AO
//   ACID_CHAR_HAIR   albedo only
//   ACID_CHAR_HEAD   _MainTex alpha is AO
//   ACID_CHAR_PROP   _MainTex alpha is the specular mask, _PropGlossiness
//   ACID_CHAR_2COL   BODY + _Color0/_Color1 tint from _MaskTex.rg, x5 specular
//                    (times the _HiSpecCube reflection when ACID_CHAR_BUMP)
//   ACID_CHAR_4COL   4 tint colours selected by _MainTex.b, detail maps A/B
// Options:
//   ACID_CHAR_BUMP         normal mapped (the non _LQ shaders), lighting per pixel
//   ACID_CHAR_FACE_UV      vertex alpha > 0 switches to the *_Face UV offsets
//   ACID_CHAR_CUSTOMLIGHT  use the material's _LightParametersLocal instead of the globals
//   ACID_CHAR_NO_SHADOWS   the original ignored realtime shadows
//   ACID_CHAR_FADE         SIMPLE_FADE / FANCY_FADE / RASTER_FADE keywords
// Keywords read: SHADOWS_SCREEN, FX_HIGHLIGHT, FX_NOISEGLOW and the fade keywords.
//
// Without ACID_CHAR_BUMP the normal is per vertex, so the SH ambient term is
// evaluated in the vertex shader (cheaper on Vita, same result for smooth SH).
#ifndef ACID_CHARACTER_INCLUDED
#define ACID_CHARACTER_INCLUDED

#include "ACIDCommon.cginc"
#include "AutoLight.cginc"

sampler2D _MainTex;
float4 _MainTex_ST;
sampler2D _BumpMap;
float4 _BumpMap_ST;
sampler2D _MaskTex;
float4 _MaskTex_ST;
fixed4 _CharacterShadowColor;
half4 _Glossiness;
half _MatGlossiness;
half4 _PropGlossiness;
fixed4 _Color0;
fixed4 _Color1;
fixed4 _Color2;
fixed4 _Color3;
sampler2D _DetailA;
sampler2D _DetailB;
float4 _DetailScale;
samplerCUBE _HiSpecCube;
half4 _NoiseGlow;
fixed4 _FadeColor;
fixed4 _LightParametersLocal;
half4 _MainTex_Face;
half4 _BumpMap_Face;
half4 _MaskTex_Face;

#if defined(ACID_CHAR_CUSTOMLIGHT)
    #define ACID_LP _LightParametersLocal
    #define ACID_LIGHT_WEIGHT _LightParametersLocal.w
#else
    #define ACID_LP _LightParameters
    #define ACID_LIGHT_WEIGHT _LightDir.w
#endif

#if defined(SHADOWS_SCREEN) && !defined(ACID_CHAR_NO_SHADOWS)
    #define ACID_CHAR_SHADOWS 1
#endif

#if defined(ACID_CHAR_FADE) && defined(RASTER_FADE)
    #define ACID_CHAR_SCREENPOS 1
#endif

struct appdata_char
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float4 tangent : TANGENT;
    float4 texcoord : TEXCOORD0;
    fixed4 color : COLOR;
};

struct v2f_char
{
    float4 pos : SV_POSITION;
    half4 uv0 : TEXCOORD0;      // xy: _MainTex, zw: _MaskTex
    half4 uv1 : TEXCOORD1;      // xy: _BumpMap, zw: raw uv (noise)
    half2 fog : TEXCOORD2;
#if defined(ACID_CHAR_BUMP)
    half4 tSpace0 : TEXCOORD3;  // xyz: view dir,     w: binormal.x
    half4 tSpace1 : TEXCOORD4;  // xyz: world normal, w: binormal.y
    half4 tSpace2 : TEXCOORD5;  // xyz: tangent,      w: binormal.z
#else
    half3 viewDir : TEXCOORD3;
    half3 normal : TEXCOORD4;
    half3 ambient : TEXCOORD5;
#endif
#if defined(ACID_CHAR_SHADOWS)
    SHADOW_COORDS(6)
#endif
#if defined(ACID_CHAR_SCREENPOS)
    float3 screen : TEXCOORD7;
#endif
};

v2f_char vertChar(appdata_char v)
{
    v2f_char o;
    UNITY_INITIALIZE_OUTPUT(v2f_char, o);
    o.pos = UnityObjectToClipPos(v.vertex);

    float2 uv = v.texcoord.xy;
    o.uv0.xy = TRANSFORM_TEX(uv, _MainTex);
    o.uv0.zw = TRANSFORM_TEX(uv, _MaskTex);
    o.uv1.xy = TRANSFORM_TEX(uv, _BumpMap);
    o.uv1.zw = uv;
#if defined(ACID_CHAR_FACE_UV)
    if (v.color.a > 0.0)
    {
        o.uv0.xy = uv * _MainTex_Face.xy + _MainTex_Face.zw;
        o.uv0.zw = uv * _MaskTex_Face.xy + _MaskTex_Face.zw;
        o.uv1.xy = uv * _BumpMap_Face.xy + _BumpMap_Face.zw;
    }
#endif

    float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
    half3 viewDir = normalize(_WorldSpaceCameraPos - worldPos);
    // The original transformed normals with the object->world matrix (not its inverse transpose).
    half3 worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
#if defined(ACID_CHAR_BUMP)
    half3 worldTangent = normalize(mul((float3x3)unity_ObjectToWorld, v.tangent.xyz));
    half3 worldBinormal = normalize(cross(worldNormal, worldTangent) * v.tangent.w);
    o.tSpace0 = half4(viewDir, worldBinormal.x);
    o.tSpace1 = half4(worldNormal, worldBinormal.y);
    o.tSpace2 = half4(worldTangent, worldBinormal.z);
#else
    o.viewDir = viewDir;
    o.normal = worldNormal;
    o.ambient = ACID_SHAmbient(worldNormal, ACID_LP.x);
#endif

    o.fog = ACID_FogFactors(v.vertex);
#if defined(ACID_CHAR_SHADOWS)
    TRANSFER_SHADOW(o);
#endif
#if defined(ACID_CHAR_SCREENPOS)
    o.screen = ComputeScreenPos(o.pos).xyw;
#endif
    return o;
}

fixed4 fragChar(v2f_char i) : SV_Target
{
    half3 L = -_LightDir.xyz;
#if defined(ACID_CHAR_BUMP)
    half3 V = i.tSpace0.xyz;
    half3 B = half3(i.tSpace0.w, i.tSpace1.w, i.tSpace2.w);
    half3 tn = UnpackNormal(tex2D(_BumpMap, i.uv1.xy));
    half3 N = tn.x * i.tSpace2.xyz + tn.y * B + tn.z * i.tSpace1.xyz;
    half3 amb = ACID_SHAmbient(N, ACID_LP.x);
#else
    half3 V = i.viewDir;
    half3 N = i.normal;
    half3 amb = i.ambient;
#endif

    fixed4 mainTex = tex2D(_MainTex, i.uv0.xy);
    half3 albedo = mainTex.rgb;
    half ao = 1.0;
    half spec = 0.0;
#if defined(ACID_CHAR_BODY) || defined(ACID_CHAR_2COL)
    fixed4 mask = tex2D(_MaskTex, i.uv0.zw);
    spec = mask.z;
    ao = 1.0 - (1.0 - mask.w) * ACID_LP.z;
  #if defined(ACID_CHAR_2COL)
    albedo *= lerp(half3(1.0, 1.0, 1.0), _Color0.rgb, mask.x);
    albedo *= lerp(half3(1.0, 1.0, 1.0), _Color1.rgb, mask.y);
  #endif
#elif defined(ACID_CHAR_HEAD)
    ao = 1.0 - (1.0 - mainTex.a) * ACID_LP.z;
#elif defined(ACID_CHAR_PROP)
    spec = mainTex.a;
#elif defined(ACID_CHAR_4COL)
    // _MainTex.b picks one of four tints (one-hot); A/B detail maps ride on tints 3 and 0.
    half sel0 = step(0.75, mainTex.b);
    half r = mainTex.b - sel0 * 2.0;
    half sel1 = step(0.5, r);
    r -= sel1 * 2.0;
    half sel2 = step(0.25, r);
    r -= sel2 * 2.0;
    half sel3 = step(0.0, r);
    float4 detailUV = _DetailScale * i.uv0.xyxy;
    half4 d0 = tex2D(_DetailA, detailUV.xy);
    half4 d1 = tex2D(_DetailB, detailUV.zw);
    half2 detail = (half2(d0.x, d0.y) - 0.5) * sel3 + (half2(d1.x, d1.y) - 0.5) * sel0;
    half3 tint = sel0 * _Color0.rgb + sel1 * _Color1.rgb + sel2 * _Color2.rgb + sel3 * _Color3.rgb;
    albedo = mainTex.r * tint + detail.x;
    spec = detail.y + mainTex.g;
#endif

    half ndl = max(dot(N, L), 0.0) * ACID_LIGHT_WEIGHT + (1.0 - ACID_LIGHT_WEIGHT);
    half3 shadowMix = lerp(_CharacterShadowColor.rgb, half3(ndl, ndl, ndl), min(1.0, ndl * 1.5));
    half3 lit = amb * albedo * ao * (1.0 - spec) * shadowMix;

#if defined(ACID_CHAR_BODY) || defined(ACID_CHAR_PROP) || defined(ACID_CHAR_2COL) || defined(ACID_CHAR_4COL)
    half specView = max(dot(V - 2.0 * dot(N, V) * N, -N), 0.25);
  #if defined(ACID_CHAR_PROP)
    lit += specView * (_PropGlossiness.w * 10.0) * _PropGlossiness.rgb * amb * spec * albedo;
  #else
    half3 gloss = specView * _Glossiness.w * (10.0 * _MatGlossiness) * _Glossiness.rgb;
    #if defined(ACID_CHAR_2COL) && defined(ACID_CHAR_BUMP)
    half3 cube = texCUBE(_HiSpecCube, reflect(-V, N)).rgb;
    lit += gloss * amb * cube * (spec * ao * 5.0) * albedo;
    #elif defined(ACID_CHAR_2COL)
    lit += gloss * amb * (spec * ao * 5.0) * albedo;  // _LQ variants skip the cubemap
    #else
    lit += gloss * amb * (spec * ao) * albedo;
    #endif
  #endif
#endif

#if defined(ACID_CHAR_SHADOWS)
    lit *= ACID_ShadowFactor(SHADOW_ATTENUATION(i));
#endif
    lit = ACID_Desaturate(lit, ndl * ACID_LP.y);

    fixed4 c = fixed4(lit, _RimColor.a);

#if defined(ACID_CHAR_FADE)
  #if defined(SIMPLE_FADE)
    c *= _FadeColor;
  #elif defined(FANCY_FADE)
    half band = ACID_NoiseBand(tex2D(_NoiseTexture, i.uv1.zw).y, _FadeColor.a);
    c.rgb = _FadeColor.rgb * (saturate(1.0 - 32.0 * abs(band)) + c.rgb);
    c.a = 32.0 * band + 0.9375;
  #elif defined(RASTER_FADE)
    c.rgb += _FadeColor.rgb * ACID_RasterFade(i.screen, _FadeColor.a);
  #endif
#endif

#if defined(FX_HIGHLIGHT)
    c.rgb += (1.0 - dot(N, V)) * _RimColor.rgb;
#endif
#if defined(FX_NOISEGLOW)
    c.rgb = _NoiseGlow.rgb * (ACID_NoiseGlow(tex2D(_NoiseTexture, i.uv1.zw).y, _NoiseGlow.a) + c.rgb);
#endif

    c.rgb = ACID_ApplyFog(c.rgb, i.fog);
    return c;
}

#endif // ACID_CHARACTER_INCLUDED

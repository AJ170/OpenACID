// Generated/* shaders (lightmapped level geometry of the city levels) and the Hidden/Shadow/Wind* casters.
//
// Options (define before including):
//   ACID_GEN_ATLAS    Tex2DBias: the texture is an atlas tile; vertex colour holds the tile rect, the UV tiles inside
//                     it (frac), and a -20 mip bias near tile edges hides the seam frac() puts into the derivatives
//   ACID_GEN_BLEND    TextureBlend: a second atlas layer (_BlendTex) blended by vertex alpha and texture alpha
//   ACID_GEN_BUMP     _BumpMap relief: colour * (1 + N.L - L.z), light direction in tangent space
//   ACID_GEN_SPEC     _SpecTex * _LightColor0 * Blinn-Phong(_Shininess), added to the albedo before lighting
//   ACID_GEN_WIND     vertex sway from WindSystem's globals (_WindParams, _WindDir), weighted by vertex colour
//   ACID_GEN_CUTOUT   alpha test against _Cutoff; lightmapped dark areas are pulled towards _ShadowColor
//   ACID_GEN_ALPHA    output the texture alpha (alpha blended pass)
// Keywords read: LIGHTMAP_ON, SHADOWS_SCREEN.
//
// Lighting is the baked lightmap (dLDR: 2 * rgb, like the GLES originals) times the albedo; realtime shadows
// darken towards _EnvAmbientColor. Without a lightmap the originals use the albedo unlit.
#ifndef ACID_GENERATED_INCLUDED
#define ACID_GENERATED_INCLUDED

#include "ACIDCommon.cginc"
#include "AutoLight.cginc"

sampler2D _MainTex;
sampler2D _BumpMap;
sampler2D _SpecTex;
sampler2D _BlendTex;
half _Shininess;
half _Cutoff;
fixed _ShadowBrightening;
fixed4 _ShadowColor;
fixed4 _EnvAmbientColor;
fixed4 _LightColor0;
half _PixelBlenderBias;
half _PixelBlenderScale;
half _Layer0UvScale;
half _Layer1UvScale;
fixed4 _Layer0Color;
fixed4 _Layer1Color;
float4 _WindParams;
float4 _WindDir;
float _WindDirFromObjNormalOrGlobal;
half4 _WindFrequency;
half4 _WindAmplitude;

#if defined(ACID_GEN_BLEND)
    #define ACID_GEN_ATLAS 1
#endif
#if defined(SHADOWS_SCREEN)
    #define ACID_GEN_SHADOWS 1
#endif

struct appdata_gen
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float4 tangent : TANGENT;
    float4 texcoord : TEXCOORD0;
    float4 texcoord1 : TEXCOORD1;
    float4 texcoord2 : TEXCOORD2;
    fixed4 color : COLOR;
};

struct v2f_gen
{
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
#if defined(ACID_GEN_BLEND)
    float4 uv1 : TEXCOORD1;      // xy: second layer UV, zw: second layer atlas offset
#endif
    half4 fog : TEXCOORD2;       // xy: fog weights; zw: blend weight / blend bias sign (ACID_GEN_BLEND)
#if defined(ACID_GEN_ATLAS)
    float4 atlas : TEXCOORD3;    // tile rect: xy min, zw max
#endif
#if defined(LIGHTMAP_ON)
    float2 lmuv : TEXCOORD4;
#endif
#if defined(ACID_GEN_BUMP) || defined(ACID_GEN_SPEC)
    half3 lightDir : TEXCOORD5;  // tangent space (bump) or object space
#endif
#if defined(ACID_GEN_SPEC)
    half3 viewDir : TEXCOORD6;
  #if !defined(ACID_GEN_BUMP)
    half3 normal : TEXCOORD7;
  #endif
#endif
#if defined(ACID_GEN_BLEND)
    fixed4 color0 : COLOR0;
    fixed4 color1 : COLOR1;
#endif
#if defined(ACID_GEN_SHADOWS)
    SHADOW_COORDS(8)
#endif
};

// WindSystem: per-object phase (colour r), per-leaf phase (g), bend weight (b), normal sway weight (a).
inline float4 ACID_Wind(float4 vertex, float3 normal, fixed4 color)
{
    float4 wavesIn = _WindParams.xxxy * _WindFrequency + color.r;
    wavesIn.xyz = wavesIn.xyz * (0.5 + color.g * 0.5) + color.g;
    float4 w = abs(frac(frac(wavesIn) * 2.0 - 1.0 + 0.5) * 2.0 - 1.0);
    float4 waves = w * w * (3.0 - 2.0 * w) * _WindAmplitude;
    vertex.xyz += normal * (dot(waves.xyz, float3(1.0, 1.0, 1.0)) * _WindParams.w) * color.a;
    float3 dir = normalize(lerp(mul((float3x3)unity_WorldToObject, _WindDir.xyz), normal, _WindDirFromObjNormalOrGlobal));
    vertex.xyz += dir * (_WindDir.w * color.b * waves.w + color.b * _WindParams.z);
    return vertex;
}

v2f_gen vertGen(appdata_gen v)
{
    v2f_gen o;
    UNITY_INITIALIZE_OUTPUT(v2f_gen, o);
#if defined(ACID_GEN_WIND)
    v.vertex = ACID_Wind(v.vertex, v.normal, v.color);
#endif
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.texcoord.xy;
    o.fog.xy = ACID_FogFactors(v.vertex);

#if defined(ACID_GEN_ATLAS)
    o.atlas.xy = v.color.xy;
  #if defined(ACID_GEN_BLEND)
    o.atlas.zw = v.color.xy + v.color.zz;
  #else
    o.atlas.zw = v.color.xy + v.color.zw;
  #endif
    o.atlas = o.atlas * 0.9960938;
    o.atlas.zw += 0.00390625;
#endif
#if defined(ACID_GEN_BLEND)
    half val = v.color.a * 2.0 - 1.0;
    half t = saturate(val * 512.0);
    o.fog.z = abs(val);
    o.fog.w = t * 2.0 - 1.0;
    o.uv = v.texcoord.xy * lerp(_Layer1UvScale, _Layer0UvScale, t);
    o.uv1 = float4(v.texcoord.xy * lerp(_Layer0UvScale, _Layer1UvScale, t), v.texcoord2.xy);
    o.color0 = lerp(_Layer1Color, _Layer0Color, t);
    o.color1 = lerp(_Layer0Color, _Layer1Color, t);
#endif
#if defined(LIGHTMAP_ON)
    o.lmuv = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif

#if defined(ACID_GEN_BUMP) || defined(ACID_GEN_SPEC)
    half3 lightDir = mul((float3x3)unity_WorldToObject, -_LightDir.xyz);
  #if defined(ACID_GEN_SPEC)
    half3 viewDir = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1.0)).xyz - v.vertex.xyz;
  #endif
  #if defined(ACID_GEN_BUMP)
    float3 binormal = cross(v.normal, v.tangent.xyz) * v.tangent.w;
    float3x3 rotation = float3x3(v.tangent.xyz, binormal, v.normal);
    o.lightDir = mul(rotation, lightDir);
    #if defined(ACID_GEN_SPEC)
    o.viewDir = mul(rotation, viewDir);
    #endif
  #else
    o.lightDir = lightDir;
    o.viewDir = viewDir;
    o.normal = v.normal;
  #endif
#endif
#if defined(ACID_GEN_SHADOWS)
    TRANSFER_SHADOW(o);
#endif
    return o;
}

fixed4 fragGen(v2f_gen i) : SV_Target
{
#if defined(ACID_GEN_ATLAS)
    float2 f = frac(i.uv);
    float4 edge = float4(f, 1.0 - f);
    float bias = min(dot(max(float4(0.0, 0.0, 0.0, 0.0), (edge - 0.93) * 14.2857), float4(1.0, 1.0, 1.0, 1.0)), 1.0) * -20.0;
    fixed4 albedo = tex2Dbias(_MainTex, float4(lerp(i.atlas.xy, i.atlas.zw, f), 0.0, bias));
  #if defined(ACID_GEN_BLEND)
    fixed4 second = tex2Dbias(_BlendTex, float4(lerp(i.atlas.xy + i.uv1.zw, i.atlas.zw + i.uv1.zw, frac(i.uv1.xy)), 0.0, bias));
    fixed4 first = albedo * i.color0;
    second *= i.color1;
    half t = saturate(i.fog.z + _PixelBlenderScale * ((i.fog.w * _PixelBlenderBias + ((second.a - first.a) * 0.5 + i.fog.z)) - 0.5));
    albedo.rgb = lerp(first.rgb, second.rgb, t);
  #endif
  #if defined(ACID_GEN_SPEC)
    fixed4 specTex = tex2Dbias(_SpecTex, float4(lerp(i.atlas.xy, i.atlas.zw, f), 0.0, bias));
  #endif
#else
    fixed4 albedo = tex2D(_MainTex, i.uv);
  #if defined(ACID_GEN_SPEC)
    fixed4 specTex = tex2D(_SpecTex, i.uv);
  #endif
#endif

#if defined(ACID_GEN_CUTOUT)
    clip(albedo.a - _Cutoff);
#endif

    fixed3 c = albedo.rgb;

#if defined(ACID_GEN_BUMP)
  #if defined(ACID_GEN_ATLAS)
    half3 n = tex2Dbias(_BumpMap, float4(lerp(i.atlas.xy, i.atlas.zw, f), 0.0, bias)).xyz * 2.0 - 1.0;
  #else
    half3 n = tex2D(_BumpMap, i.uv).xyz * 2.0 - 1.0;
  #endif
    half3 L = normalize(i.lightDir);
#endif
#if defined(ACID_GEN_SPEC)
  #if defined(ACID_GEN_BUMP)
    half nh = dot(n, normalize(normalize(i.viewDir) + L));
  #else
    half nh = dot(normalize(i.normal), normalize(normalize(i.viewDir) + normalize(i.lightDir)));
  #endif
    c += specTex.rgb * _LightColor0.rgb * pow(max(0.0, nh), _Shininess);
#endif

#if defined(ACID_GEN_SHADOWS)
    fixed shadow = SHADOW_ATTENUATION(i);
#endif
#if defined(LIGHTMAP_ON)
    half3 lm = 2.0 * UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmuv).rgb;
  #if defined(ACID_GEN_SHADOWS)
    c *= min(lm, lerp(_EnvAmbientColor.rgb, lm, shadow));
  #elif defined(ACID_GEN_CUTOUT)
    half brightness = min(1.0, dot(lm, half3(0.3, 0.59, 0.11)));
    c *= lerp(lm, _ShadowColor.rgb, (1.0 - brightness * brightness * brightness) * _ShadowBrightening);
  #else
    c *= lm;
  #endif
#elif defined(ACID_GEN_SHADOWS)
    c *= _EnvAmbientColor.rgb * (1.0 - shadow) + shadow;
#endif

#if defined(ACID_GEN_BUMP)
    c *= 1.0 + (dot(L, n) - L.z);
#endif

    c = ACID_ApplyFog(c, i.fog.xy);
#if defined(ACID_GEN_ALPHA) || defined(ACID_GEN_CUTOUT)
    return fixed4(c, albedo.a);
#else
    return fixed4(c, 1.0);
#endif
}

// Shadow casters with the same wind sway (Hidden/Shadow/Wind, Hidden/Shadow/WindCutout).
struct v2f_gen_shadow
{
    V2F_SHADOW_CASTER;
    float2 uv : TEXCOORD1;
};

v2f_gen_shadow vertGenShadow(appdata_gen v)
{
    v2f_gen_shadow o;
    v.vertex = ACID_Wind(v.vertex, v.normal, v.color);
    TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
    o.uv = v.texcoord.xy;
    return o;
}

float4 fragGenShadow(v2f_gen_shadow i) : SV_Target
{
#if defined(ACID_GEN_CUTOUT)
    clip(tex2D(_MainTex, i.uv).a - _Cutoff);
#endif
    SHADOW_CASTER_FRAGMENT(i)
}

#endif // ACID_GENERATED_INCLUDED

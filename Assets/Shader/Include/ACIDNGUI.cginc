// NGUI "Unlit/*" shaders as shipped in the game (mix of NGUI 2.x and 3.x clipping).
//
// NGUI_COLOR (required):
//   NGUI_COLORED   tex * vertex colour
//   NGUI_TEXT      vertex rgb, vertex alpha * tex alpha
//   NGUI_PREMUL    tex * vertex colour, clipping also scales rgb
//   NGUI_PACKED    font atlas packed into rgba, channel picked by vertex colour
// NGUI_CLIP (optional):
//   NGUI_CLIP_ALPHA     NGUI 2 hard clip, region from _MainTex_ST
//   NGUI_CLIP_SOFT      NGUI 2 soft clip, _MainTex_ST + _ClipSharpness
//   NGUI_CLIP_TEX       NGUI 3 texture clip, _ClipRange0 + _ClipTex
//   NGUI_CLIP_TEX_ST    NGUI 2 texture clip, _MainTex_ST + _ClipTex
//   NGUI_CLIP_RECT 1|2|3  NGUI 3 panel clipping (_ClipRangeN / _ClipArgsN)
// NGUI_UV_ST: apply _MainTex_ST to the texture coordinates.
#ifndef ACID_NGUI_INCLUDED
#define ACID_NGUI_INCLUDED

#include "UnityCG.cginc"

sampler2D _MainTex;
float4 _MainTex_ST;
sampler2D _ClipTex;
float2 _ClipSharpness;
float4 _ClipRange0;
float4 _ClipArgs0;
float4 _ClipRange1;
float4 _ClipArgs1;
float4 _ClipRange2;
float4 _ClipArgs2;

#if defined(NGUI_CLIP_RECT) || defined(NGUI_CLIP_ALPHA) || defined(NGUI_CLIP_SOFT) || defined(NGUI_CLIP_TEX) || defined(NGUI_CLIP_TEX_ST)
    #define NGUI_HAS_CLIP 1
#endif

struct appdata_ngui
{
    float4 vertex : POSITION;
    fixed4 color : COLOR;
    float2 texcoord : TEXCOORD0;
};

struct v2f_ngui
{
    float4 pos : SV_POSITION;
    fixed4 color : COLOR;
    float2 uv : TEXCOORD0;
#if defined(NGUI_HAS_CLIP)
    float4 clip01 : TEXCOORD1;   // xy: first clip region (or clip texture uv), zw: second
#endif
#if defined(NGUI_CLIP_RECT) && NGUI_CLIP_RECT >= 3
    float2 clip2 : TEXCOORD2;
#endif
};

inline float2 NGUIRotate(float2 v, float2 rot)
{
    return float2(v.x * rot.y - v.y * rot.x, v.x * rot.x + v.y * rot.y);
}

inline float NGUIRectFactor(float2 clipPos, float2 sharpness)
{
    float2 f = (1.0 - abs(clipPos)) * sharpness;
    return min(f.x, f.y);
}

v2f_ngui vertNGUI(appdata_ngui v)
{
    v2f_ngui o;
    UNITY_INITIALIZE_OUTPUT(v2f_ngui, o);
    o.pos = UnityObjectToClipPos(v.vertex);
    o.color = v.color;
#if defined(NGUI_UV_ST)
    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
#else
    o.uv = v.texcoord;
#endif
#if defined(NGUI_CLIP_ALPHA) || defined(NGUI_CLIP_SOFT)
    o.clip01 = float4(v.vertex.xy * _MainTex_ST.xy + _MainTex_ST.zw, 0.0, 0.0);
#elif defined(NGUI_CLIP_TEX_ST)
    o.clip01 = float4((v.vertex.xy * _MainTex_ST.xy + _MainTex_ST.zw) * 0.5 + 0.5, 0.0, 0.0);
#elif defined(NGUI_CLIP_TEX)
    o.clip01 = float4((v.vertex.xy * _ClipRange0.zw + _ClipRange0.xy) * 0.5 + 0.5, 0.0, 0.0);
#elif defined(NGUI_CLIP_RECT)
    o.clip01.xy = v.vertex.xy * _ClipRange0.zw + _ClipRange0.xy;
    o.clip01.zw = float2(0.0, 0.0);
  #if NGUI_CLIP_RECT >= 2
    o.clip01.zw = NGUIRotate(v.vertex.xy, _ClipArgs1.zw) * _ClipRange1.zw + _ClipRange1.xy;
  #endif
  #if NGUI_CLIP_RECT >= 3
    o.clip2 = NGUIRotate(v.vertex.xy, _ClipArgs2.zw) * _ClipRange2.zw + _ClipRange2.xy;
  #endif
#endif
    return o;
}

fixed4 fragNGUI(v2f_ngui i) : SV_Target
{
    fixed4 tex = tex2D(_MainTex, i.uv);
#if defined(NGUI_TEXT)
    fixed4 col = fixed4(i.color.rgb, i.color.a * tex.a);
#elif defined(NGUI_PACKED)
    half4 sel = saturate(ceil(i.color - 0.5));
    fixed4 col = saturate((sel * 0.51 - i.color) / -0.49);
    half4 mask = tex * sel;
    half packedAlpha = (mask.x + mask.y) + (mask.z + mask.w);
#else
    fixed4 col = tex * i.color;
#endif

#if defined(NGUI_CLIP_ALPHA)
    float2 a = abs(i.clip01.xy);
    if (1.0 - max(a.x, a.y) < 0.0)
    {
  #if defined(NGUI_PREMUL)
        col = fixed4(0.0, 0.0, 0.0, 0.0);
  #else
        col.a = 0.0;
  #endif
    }
#elif defined(NGUI_HAS_CLIP)
  #if defined(NGUI_CLIP_SOFT)
    float factor = saturate(NGUIRectFactor(i.clip01.xy, _ClipSharpness));
  #elif defined(NGUI_CLIP_TEX) || defined(NGUI_CLIP_TEX_ST)
    float factor = tex2D(_ClipTex, i.clip01.xy).a;
  #else
    float factor = NGUIRectFactor(i.clip01.xy, _ClipArgs0.xy);
    #if NGUI_CLIP_RECT >= 2
    factor = min(factor, NGUIRectFactor(i.clip01.zw, _ClipArgs1.xy));
    #endif
    #if NGUI_CLIP_RECT >= 3
    factor = min(factor, NGUIRectFactor(i.clip2, _ClipArgs2.xy));
    #endif
    factor = saturate(factor);
  #endif
    col.a *= factor;
  #if defined(NGUI_PREMUL)
    col.rgb *= factor;
  #endif
#endif

#if defined(NGUI_PACKED)
    col.a *= packedAlpha;
#endif
    return col;
}

#endif // ACID_NGUI_INCLUDED

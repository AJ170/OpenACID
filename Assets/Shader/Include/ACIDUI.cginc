// Game specific UI/* shaders. Define one ACID_UI_* selector before including;
// ACID_SOFTCLIP adds NGUI 2 soft clipping (the "(SoftClip)" variants).
#ifndef ACID_UI_INCLUDED
#define ACID_UI_INCLUDED

#include "ACIDCommon.cginc"

sampler2D _MainTex;
float4 _MainTex_ST;
sampler2D _MaskTex;
float4 _MaskTex_ST;
sampler2D _Foreground;
sampler2D _Stencil;
sampler2D _FrameTex;
sampler2D _InsigniaItem;
sampler2D _HeroicFrame;
float4 _HeroicFrame_ST;
sampler2D _HeroicStencil;
fixed4 _Color;
fixed4 _Color0;
fixed4 _Color1;
fixed4 _Rarity;
half _Desaturation;
half _Alpha;
half _ShowInsigniaItem;
half2 _YOffsetVec;
float4 _Region;
float2 _ClipSharpness;

struct appdata_ui
{
    float4 vertex : POSITION;
    fixed4 color : COLOR;
    float2 texcoord : TEXCOORD0;
};

struct v2f_ui
{
    float4 pos : SV_POSITION;
    fixed4 color : COLOR;
    float4 uv : TEXCOORD0;
#if defined(ACID_SOFTCLIP)
    float2 clipPos : TEXCOORD1;
#endif
};

inline fixed3 ACID_UIDesaturate(fixed3 c, half amount)
{
    half lum = dot(half3(0.222, 0.707, 0.071), c);
    return lerp(c, fixed3(lum, lum, lum), amount);
}

v2f_ui vertUI(appdata_ui v)
{
    v2f_ui o;
    UNITY_INITIALIZE_OUTPUT(v2f_ui, o);
    o.pos = UnityObjectToClipPos(v.vertex);
    o.color = v.color;
    float2 uv = v.texcoord;
#if defined(ACID_UI_TEXT_WHITE) || defined(ACID_UI_TEXT_OUTLINE)
    o.color = v.color * _Color;
    o.uv = float4(TRANSFORM_TEX(uv, _MainTex), 0.0, 0.0);
#elif defined(ACID_UI_EVENTBANNER)
    o.uv = float4(uv, uv + _YOffsetVec);
#elif defined(ACID_UI_MINIMAP) || defined(ACID_UI_MINIMAP_RESTRICTED)
    o.uv = float4(uv * _Region.zw + _Region.xy, uv);
#elif defined(ACID_UI_PORTRAIT_HEROIC)
    o.uv = float4(TRANSFORM_TEX(uv, _MaskTex), TRANSFORM_TEX(uv, _HeroicFrame));
#else
    o.uv = float4(uv, 0.0, 0.0);
#endif
#if defined(ACID_SOFTCLIP)
    o.clipPos = v.vertex.xy * _MainTex_ST.xy + _MainTex_ST.zw;
#endif
    return o;
}

fixed4 fragUI(v2f_ui i) : SV_Target
{
    fixed4 c;
#if defined(ACID_UI_TEXT_WHITE)
    c = fixed4(i.color.rgb, i.color.a * tex2D(_MainTex, i.uv.xy).a);
#elif defined(ACID_UI_TEXT_OUTLINE)
    fixed a = tex2D(_MainTex, i.uv.xy).a;
    c = fixed4(i.color.rgb * (2.0 * a - 1.0), i.color.a * (a * a));
#elif defined(ACID_UI_EVENTBANNER)
    c = fixed4(tex2D(_MainTex, i.uv.zw).rgb, _Color0.a * tex2D(_Stencil, i.uv.xy).r);
#elif defined(ACID_UI_MINIMAP)
    fixed4 icon = tex2D(_MainTex, i.uv.xy);
    c = fixed4(icon.aaa, i.color.a) * tex2D(_MaskTex, i.uv.zw).g;
#elif defined(ACID_UI_MINIMAP_RESTRICTED)
    c = tex2D(_MainTex, i.uv.xy) * i.color.a * tex2D(_MaskTex, i.uv.zw).g;
#elif defined(ACID_UI_ACHIEVEMENT)
    fixed4 icon = tex2D(_MainTex, i.uv.xy);
    fixed4 frame = tex2D(_FrameTex, i.uv.xy);
    c.rgb = lerp(icon.rgb, frame.rgb, frame.a);
    c.a = _Alpha * max(icon.a, frame.a);
    c.rgb = ACID_UIDesaturate(c.rgb, _Desaturation);
#elif defined(ACID_UI_ITEMCOLORING)
    fixed4 background = tex2D(_MainTex, i.uv.xy);
    fixed4 mask = tex2D(_MaskTex, i.uv.xy);
    fixed4 foreground = tex2D(_Foreground, i.uv.xy);
    foreground.rgb *= lerp(fixed3(1.0, 1.0, 1.0), _Color0.rgb, mask.r);
    foreground.rgb *= lerp(fixed3(1.0, 1.0, 1.0), _Color1.rgb, mask.g);
  #if defined(BACKGROUND_ON)
    background.rgb = lerp(lerp(background.rgb, background.rgb * _Rarity.rgb, background.a), _Rarity.rgb, mask.b);
    c = fixed4(lerp(background.rgb, foreground.rgb, foreground.a), _Color0.a);
  #else
    c = fixed4(lerp(_Rarity.rgb, foreground.rgb, foreground.a), max(mask.b, foreground.a) * _Color0.a);
  #endif
    c.rgb = ACID_UIDesaturate(c.rgb, _Desaturation);
#elif defined(ACID_UI_PORTRAIT) || defined(ACID_UI_PORTRAIT_HEROIC)
    fixed4 background = tex2D(_MainTex, i.uv.xy);
    fixed3 mask = tex2D(_MaskTex, i.uv.xy).rgb;
    fixed4 foreground = tex2D(_Foreground, i.uv.xy);
    fixed4 insignia = tex2D(_InsigniaItem, i.uv.xy);
    foreground.rgb *= lerp(fixed3(1.0, 1.0, 1.0), _Color0.rgb, mask.r);
    foreground.rgb *= lerp(fixed3(1.0, 1.0, 1.0), _Color1.rgb, mask.g);
    background.rgb = lerp(background.rgb, insignia.rgb, insignia.a * _ShowInsigniaItem);
    c = fixed4(lerp(background.rgb, foreground.rgb, mask.b), _Color0.a * foreground.a);
  #if defined(ACID_UI_PORTRAIT_HEROIC)
    c = lerp(tex2D(_HeroicFrame, i.uv.zw), c, c.a * tex2D(_HeroicStencil, i.uv.zw).r);
  #endif
#endif

#if defined(ACID_SOFTCLIP)
    c.a *= ACID_SoftClip(i.clipPos, _ClipSharpness);
#endif
    return c;
}

#endif // ACID_UI_INCLUDED

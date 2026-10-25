// Effects/Alpha, Effects/Additive, Effects/Additive (Overlay) and Effects/Godray.
// Keywords: TINT_COLOR_ON (2x _TintColor), VERTEX_COLOR_ON, FOG_ON (game fog).
// Define ACID_GODRAY for the godray fade terms.
#ifndef ACID_EFFECTS_INCLUDED
#define ACID_EFFECTS_INCLUDED

#include "ACIDCommon.cginc"

sampler2D _MainTex;
float4 _MainTex_ST;
fixed4 _TintColor;
half _BoostAlpha;
half _CamHeightOffset;
half _FadeDistance;

#if defined(TINT_COLOR_ON) || defined(VERTEX_COLOR_ON)
    #define ACID_FX_COLOR 1
#endif

struct appdata_fx
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    fixed4 color : COLOR;
    float2 texcoord : TEXCOORD0;
};

struct v2f_fx
{
    float4 pos : SV_POSITION;
    half2 uv : TEXCOORD0;
#if defined(ACID_FX_COLOR)
    fixed4 color : TEXCOORD1;
#endif
#if defined(FOG_ON)
    half2 fog : TEXCOORD2;
#endif
#if defined(ACID_GODRAY)
    half normalY : TEXCOORD3;
    float camDist : TEXCOORD4;
    float camHeight : TEXCOORD5;
#endif
};

v2f_fx vertFX(appdata_fx v)
{
    v2f_fx o;
    UNITY_INITIALIZE_OUTPUT(v2f_fx, o);
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
#if defined(TINT_COLOR_ON)
    o.color = fixed4(_TintColor.rgb * 2.0, _TintColor.a);
  #if defined(VERTEX_COLOR_ON)
    o.color *= v.color;
  #endif
#elif defined(VERTEX_COLOR_ON)
    o.color = v.color;
#endif
#if defined(FOG_ON)
    o.fog = ACID_FogFactors(v.vertex);
#endif
#if defined(ACID_GODRAY)
    float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
    o.normalY = UnityObjectToWorldNormal(v.normal).y;
    o.camDist = length(_WorldSpaceCameraPos - worldPos);
    o.camHeight = _WorldSpaceCameraPos.y - worldPos.y;
#endif
    return o;
}

fixed4 fragFX(v2f_fx i) : SV_Target
{
    fixed4 c = tex2D(_MainTex, i.uv);
#if defined(ACID_FX_COLOR)
    c *= i.color;
#endif
#if defined(ACID_GODRAY)
    c.a *= saturate(_CamHeightOffset - i.camHeight);
    c.a *= i.normalY;
    c.a *= sqrt(1.0 - saturate((i.camDist - 1.0) / (_FadeDistance - 1.0))) * _BoostAlpha;
  #if defined(FOG_ON)
    c.rgb *= i.fog.x;  // additive: fade to black instead of to the fog colour
  #endif
#elif defined(FOG_ON)
    c.rgb = ACID_ApplyFog(c.rgb, i.fog);
#endif
    return c;
}

#endif // ACID_EFFECTS_INCLUDED

// 7 tap separable gaussian used by Hidden/FastBlur (Standard Assets MobileBlur)
// and Hidden/LqBlurIteration. The "SGX" path precomputes every tap position in
// the vertex shader, which avoids dependent texture reads on PowerVR SGX GPUs
// (the PS Vita GPU), so prefer it there.
#ifndef ACID_BLUR_INCLUDED
#define ACID_BLUR_INCLUDED

#include "UnityCG.cginc"

sampler2D _MainTex;
half4 _MainTex_TexelSize;

static const half4 ACID_BlurCurve4[7] =
{
    half4(0.0205, 0.0205, 0.0205, 0.0),
    half4(0.0855, 0.0855, 0.0855, 0.0),
    half4(0.232, 0.232, 0.232, 0.0),
    half4(0.324, 0.324, 0.324, 1.0),
    half4(0.232, 0.232, 0.232, 0.0),
    half4(0.0855, 0.0855, 0.0855, 0.0),
    half4(0.0205, 0.0205, 0.0205, 0.0)
};

struct v2f_blur8
{
    float4 pos : SV_POSITION;
    half2 uv : TEXCOORD0;
    half2 offs : TEXCOORD1;
};

struct v2f_blurSGX
{
    float4 pos : SV_POSITION;
    half2 uv : TEXCOORD0;
    half4 offs0 : TEXCOORD1;   // uv -/+ 3 taps
    half4 offs1 : TEXCOORD2;   // uv -/+ 2 taps
    half4 offs2 : TEXCOORD3;   // uv -/+ 1 tap
};

v2f_blur8 ACID_VertBlur8(appdata_img v, half2 step)
{
    v2f_blur8 o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.texcoord.xy;
    o.offs = step;
    return o;
}

half4 ACID_FragBlur8(v2f_blur8 i)
{
    half2 coords = i.uv - i.offs * 3.0;
    half4 color = half4(0.0, 0.0, 0.0, 0.0);
    for (int l = 0; l < 7; l++)
    {
        color += tex2D(_MainTex, coords) * ACID_BlurCurve4[l];
        coords += i.offs;
    }
    return color;
}

v2f_blurSGX ACID_VertBlurSGX(appdata_img v, half2 step)
{
    v2f_blurSGX o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.texcoord.xy;
    half4 uv4 = v.texcoord.xyxy;
    half4 s = step.xyxy * half4(1.0, 1.0, -1.0, -1.0);
    o.offs0 = uv4 - s * 3.0;
    o.offs1 = uv4 - s * 2.0;
    o.offs2 = uv4 - s;
    return o;
}

half4 ACID_FragBlurSGX(v2f_blurSGX i)
{
    half4 color = tex2D(_MainTex, i.uv) * ACID_BlurCurve4[3];
    color += (tex2D(_MainTex, i.offs0.xy) + tex2D(_MainTex, i.offs0.zw)) * ACID_BlurCurve4[0];
    color += (tex2D(_MainTex, i.offs1.xy) + tex2D(_MainTex, i.offs1.zw)) * ACID_BlurCurve4[1];
    color += (tex2D(_MainTex, i.offs2.xy) + tex2D(_MainTex, i.offs2.zw)) * ACID_BlurCurve4[2];
    return color;
}

#endif // ACID_BLUR_INCLUDED

// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/MaskShaderAdditiveUV2.shader.cg, not this file.
Shader "Effects/AlphaMaskAdditiveUV2" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "black" { }
 _Mask ("Alpha (A)", 2D) = "white" { }
 _TintColor ("Color Tint", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "Queue"="Overlay" "IgnoreProjector"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "Queue"="Overlay" "IgnoreProjector"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend One One
  ColorMask RGB
  Offset -1, -1
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #define ACID_MASK_UV2
  #include "UnityCG.cginc"
  sampler2D _MainTex;
  float4 _MainTex_ST;
  sampler2D _Mask;
  float4 _Mask_ST;
  fixed4 _TintColor;
  struct v2f { float4 pos : SV_POSITION; half4 uv : TEXCOORD0; };
  v2f vert(appdata_full v)
  {
      v2f o;
      o.pos = UnityObjectToClipPos(v.vertex);
      o.uv.xy = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
  #if defined(ACID_MASK_UV2)
      o.uv.zw = TRANSFORM_TEX(v.texcoord1.xy, _Mask);
  #else
      o.uv.zw = TRANSFORM_TEX(v.texcoord.xy, _Mask);
  #endif
      return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
      fixed4 col = tex2D(_MainTex, i.uv.xy) * _TintColor;
      col.rgb *= tex2D(_Mask, i.uv.zw).rgb;
      return col;
  }
  ENDCG
 }
}
Fallback "Particles/Alpha Blended"
}

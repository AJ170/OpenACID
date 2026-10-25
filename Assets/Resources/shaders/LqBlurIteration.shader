// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/LqBlurIteration.shader.cg, not this file.
Shader "Hidden/LqBlurIteration" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
}
SubShader { 
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #pragma multi_compile SGX_OFF SGX_ON
  #include "../../Shader/Include/ACIDBlur.cginc"
  half _Parameter;
  #if defined(SGX_ON)
      #define ACID_BLUR_V2F v2f_blurSGX
      #define ACID_BLUR_VERT ACID_VertBlurSGX
      #define ACID_BLUR_FRAG ACID_FragBlurSGX
  #else
      #define ACID_BLUR_V2F v2f_blur8
      #define ACID_BLUR_VERT ACID_VertBlur8
      #define ACID_BLUR_FRAG ACID_FragBlur8
  #endif
  half4 frag(ACID_BLUR_V2F i) : SV_Target { return ACID_BLUR_FRAG(i); }
  ACID_BLUR_V2F vert(appdata_img v) { return ACID_BLUR_VERT(v, _MainTex_TexelSize.xy * half2(0.0, 1.0) * _Parameter); }
  ENDCG
 }
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #pragma multi_compile SGX_OFF SGX_ON
  #include "../../Shader/Include/ACIDBlur.cginc"
  half _Parameter;
  #if defined(SGX_ON)
      #define ACID_BLUR_V2F v2f_blurSGX
      #define ACID_BLUR_VERT ACID_VertBlurSGX
      #define ACID_BLUR_FRAG ACID_FragBlurSGX
  #else
      #define ACID_BLUR_V2F v2f_blur8
      #define ACID_BLUR_VERT ACID_VertBlur8
      #define ACID_BLUR_FRAG ACID_FragBlur8
  #endif
  half4 frag(ACID_BLUR_V2F i) : SV_Target { return ACID_BLUR_FRAG(i); }
  ACID_BLUR_V2F vert(appdata_img v) { return ACID_BLUR_VERT(v, _MainTex_TexelSize.xy * half2(1.0, 0.0) * _Parameter); }
  ENDCG
 }
}
Fallback "Diffuse"
}

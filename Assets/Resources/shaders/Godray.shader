// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/Godray.shader.cg, not this file.
Shader "Effects/Godray" {
Properties {
 _MainTex ("Particle Texture", 2D) = "white" { }
 _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,1)
 _BoostAlpha ("Boost Alpha", Range(0,10)) = 1
 _CamHeightOffset ("Camera Height Offset", Range(-10,10)) = 4
 _FadeDistance ("Camera Fade Distance", Range(0,60)) = 30
}
SubShader { 
 Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha One
  ColorMask RGB
  CGPROGRAM
  #pragma vertex vertFX
  #pragma fragment fragFX
  #pragma multi_compile FOG_OFF FOG_ON
  #pragma multi_compile TINT_COLOR_OFF TINT_COLOR_ON
  #pragma multi_compile VERTEX_COLOR_OFF VERTEX_COLOR_ON
  #define ACID_GODRAY
  #include "../../Shader/Include/ACIDEffects.cginc"
  ENDCG
 }
}
}

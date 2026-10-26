// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Generated/Transparent/Cutout/DiffuseCutoutCullOff.shader.cg, not this file.
Shader "Generated/Transparent/Cutout/DiffuseCutoutCullOff" {
Properties {
 _MainTex ("Albedo (RGB) Alpha (A)", 2D) = "white" { }
 _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
 _ShadowBrightening ("ShadowColor Usage", Range(0,1)) = 0
 _ShadowColor ("Shadow Color", Color) = (0,0,0,0)
 _Padding ("Padding (Atlas)", Float) = 0
}
SubShader { 
 LOD 99
 Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="AlphaTest" "RenderType"="TransparentCutout" }
  Cull Off
  ColorMask RGB
  CGPROGRAM
  #pragma vertex vertGen
  #pragma fragment fragGen
  #pragma multi_compile __ LIGHTMAP_ON
  #pragma multi_compile __ SHADOWS_SCREEN
  #define ACID_GEN_CUTOUT 1
  #include "../../../../Include/ACIDGenerated.cginc"
  ENDCG
 }
}
Fallback "Hidden/Shadow/Cutout"
}

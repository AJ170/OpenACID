// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Generated/Transparent/Cutout/DiffuseCutoutCullOnWind.shader.cg, not this file.
Shader "Generated/Transparent/Cutout/DiffuseCutoutCullOnWind" {
Properties {
 _MainTex ("Albedo (RGB) Alpha (A)", 2D) = "white" { }
 _WindFrequency ("Flutter Frequency (XYZ) Bend Frequency (W)", Vector) = (1.975,0.793,0.375,0.193)
 _WindAmplitude ("Flutter Amplitude (XYZ) Bend Amplitude (W)", Vector) = (1,1,1,1)
 _WindDirFromObjNormalOrGlobal ("Wind Direction from Global Windsystem or Objectspace Normal", Range(0,1)) = 0
 _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
 _ShadowBrightening ("ShadowColor Usage", Range(0,1)) = 0
 _ShadowColor ("Shadow Color", Color) = (0,0,0,0)
}
SubShader { 
 LOD 99
 Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="AlphaTest" "RenderType"="TransparentCutout" }
  ColorMask RGB
  CGPROGRAM
  #pragma vertex vertGen
  #pragma fragment fragGen
  #pragma multi_compile __ LIGHTMAP_ON
  #pragma multi_compile __ SHADOWS_SCREEN
  #define ACID_GEN_CUTOUT 1
  #define ACID_GEN_WIND 1
  #include "../../../../Include/ACIDGenerated.cginc"
  ENDCG
 }
}
Fallback "Hidden/Shadow/WindCutout"
}

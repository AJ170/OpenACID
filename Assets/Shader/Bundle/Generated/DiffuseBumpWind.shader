// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Generated/DiffuseBumpWind.shader.cg, not this file.
Shader "Generated/DiffuseBumpWind" {
Properties {
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
 _BumpMap ("Normalmap", 2D) = "bump" { }
 _WindFrequency ("Flutter Frequency (XYZ) Bend Frequency (W)", Vector) = (1.975,0.793,0.375,0.193)
 _WindAmplitude ("Flutter Amplitude (XYZ) Bend Amplitude (W)", Vector) = (1,1,1,1)
 _WindDirFromObjNormalOrGlobal ("Wind Direction from Global Windsystem or Objectspace Normal", Range(0,1)) = 0
}
SubShader { 
 LOD 499
 Tags { "Queue"="Geometry" "RenderType"="Opaque" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="Geometry" "RenderType"="Opaque" }
  CGPROGRAM
  #pragma vertex vertGen
  #pragma fragment fragGen
  #pragma multi_compile __ LIGHTMAP_ON
  #pragma multi_compile __ SHADOWS_SCREEN
  #define ACID_GEN_BUMP 1
  #define ACID_GEN_WIND 1
  #include "../../Include/ACIDGenerated.cginc"
  ENDCG
 }
}
Fallback "Generated/DiffuseWind"
}

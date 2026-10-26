// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Generated/DiffuseTex2DLOD.shader.cg, not this file.
Shader "Generated/DiffuseTex2DLOD" {
Properties {
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
 _AtlasRes ("Atlas Resolution", Float) = 1024
}
SubShader { 
 LOD 100
 Tags { "Queue"="Geometry" "RenderType"="Opaque" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="Geometry" "RenderType"="Opaque" }
  CGPROGRAM
  // Only reached as a fallback; samples the atlas like Tex2DBias instead of the original manual LOD.
  #pragma vertex vertGen
  #pragma fragment fragGen
  #pragma multi_compile __ LIGHTMAP_ON
  #pragma multi_compile __ SHADOWS_SCREEN
  #define ACID_GEN_ATLAS 1
  #include "../../Include/ACIDGenerated.cginc"
  ENDCG
 }
}
Fallback "Generated/DiffuseTex2DBias"
}

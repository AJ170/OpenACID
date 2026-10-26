// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Generated/DiffuseBumpTex2DBias.shader.cg, not this file.
Shader "Generated/DiffuseBumpTex2DBias" {
Properties {
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
 _BumpMap ("Normalmap", 2D) = "bump" { }
}
SubShader { 
 LOD 299
 Tags { "Queue"="Geometry" "RenderType"="Opaque" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="Geometry" "RenderType"="Opaque" }
  CGPROGRAM
  #pragma vertex vertGen
  #pragma fragment fragGen
  #pragma multi_compile __ LIGHTMAP_ON
  #pragma multi_compile __ SHADOWS_SCREEN
  #define ACID_GEN_BUMP 1
  #define ACID_GEN_ATLAS 1
  #include "../../Include/ACIDGenerated.cginc"
  ENDCG
 }
}
Fallback "Generated/DiffuseTex2DLOD"
}

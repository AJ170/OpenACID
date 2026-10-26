// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Generated/DiffuseBump.shader.cg, not this file.
Shader "Generated/DiffuseBump" {
Properties {
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
 _BumpMap ("Normalmap", 2D) = "bump" { }
 _Padding ("Padding (Atlas)", Float) = 0
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
  #include "../../Include/ACIDGenerated.cginc"
  ENDCG
 }
}
Fallback "Generated/Diffuse"
}

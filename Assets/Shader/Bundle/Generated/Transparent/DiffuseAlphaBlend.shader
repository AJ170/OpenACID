// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Generated/Transparent/DiffuseAlphaBlend.shader.cg, not this file.
Shader "Generated/Transparent/DiffuseAlphaBlend" {
Properties {
 _MainTex ("Albedo (RGB) Alpha (A)", 2D) = "white" { }
 _Padding ("Padding (Atlas)", Float) = 0
}
SubShader { 
 LOD 99
 Tags { "Queue"="AlphaTest" "RenderType"="Transparent" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="AlphaTest" "RenderType"="Transparent" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  CGPROGRAM
  #pragma vertex vertGen
  #pragma fragment fragGen
  #pragma multi_compile __ LIGHTMAP_ON
  #pragma multi_compile __ SHADOWS_SCREEN
  #define ACID_GEN_ALPHA 1
  #include "../../../Include/ACIDGenerated.cginc"
  ENDCG
 }
}
Fallback "Hidden/Shadow/Cutout"
}

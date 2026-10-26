// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Generated/DiffuseTextureBlendTex2DBias.shader.cg, not this file.
Shader "Generated/DiffuseTextureBlendTex2DBias" {
Properties {
 _MainTex ("Albedo (RGBA)", 2D) = "white" { }
 _BlendTex ("Albedo (RGBA)", 2D) = "white" { }
 _PixelBlenderBias ("Pixel Blend Bias", Float) = 0
 _PixelBlenderScale ("Pixel Blend Scale", Float) = 0.5
 _Layer0UvScale ("Tiling MainTex", Float) = 1
 _Layer1UvScale ("Tiling BlendTex", Float) = 1
 _Layer0Color ("MainTex Color", Color) = (1,1,1,1)
 _Layer1Color ("BlendTex Color", Color) = (1,1,1,1)
}
SubShader { 
 LOD 199
 Tags { "Queue"="Geometry" "RenderType"="Opaque" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="Geometry" "RenderType"="Opaque" }
  CGPROGRAM
  #pragma vertex vertGen
  #pragma fragment fragGen
  #pragma multi_compile __ LIGHTMAP_ON
  #pragma multi_compile __ SHADOWS_SCREEN
  #define ACID_GEN_BLEND 1
  #include "../../Include/ACIDGenerated.cginc"
  ENDCG
 }
}
Fallback "Hidden/Shadow/Opaque"
}

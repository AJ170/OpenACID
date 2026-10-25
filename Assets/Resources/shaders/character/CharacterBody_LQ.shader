// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/character/CharacterBody_LQ.shader.cg, not this file.
Shader "Character/CharacterBody_LQ" {
Properties {
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
 _MaskTex ("Unused (RG) Specular Mask (B)", 2D) = "white" { }
 _MatGlossiness ("Glossiness", Float) = 1
}
SubShader { 
 LOD 99
 Tags { "LightMode"="ForwardBase" "Queue"="Geometry" "RenderType"="Opaque" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="Geometry" "RenderType"="Opaque" }
  CGPROGRAM
  #pragma vertex vertChar
  #pragma fragment fragChar
  #pragma multi_compile __ SHADOWS_SCREEN
  #pragma multi_compile FX_OFF FX_NOISEGLOW FX_HIGHLIGHT
  #define ACID_CHAR_BODY
  #include "../../../Shader/Include/ACIDCharacter.cginc"
  ENDCG
 }
}
Fallback "Hidden/Shadow/Opaque"
}

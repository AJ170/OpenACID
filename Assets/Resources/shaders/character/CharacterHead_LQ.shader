// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/character/CharacterHead_LQ.shader.cg, not this file.
Shader "Character/CharacterHead_LQ" {
Properties {
 _MainTex ("Albedo (RGB) AO (A)", 2D) = "white" { }
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
  #define ACID_CHAR_HEAD
  #include "../../../Shader/Include/ACIDCharacter.cginc"
  ENDCG
 }
}
Fallback "Hidden/Shadow/Opaque"
}

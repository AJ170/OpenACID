// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/character/CharacterHair_LQ.shader.cg, not this file.
Shader "Character/CharacterHair_LQ" {
Properties {
 _MainTex ("Albedo (RGB) Alpha (A)", 2D) = "white" { }
}
SubShader { 
 LOD 99
 Tags { "LightMode"="ForwardBase" "Queue"="Transparent" "RenderType"="Transparent" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="Transparent" "RenderType"="Transparent" }
  CGPROGRAM
  #pragma vertex vertChar
  #pragma fragment fragChar
  #pragma multi_compile __ SHADOWS_SCREEN
  #pragma multi_compile FX_OFF FX_NOISEGLOW FX_HIGHLIGHT
  #define ACID_CHAR_HAIR
  #include "../../../Shader/Include/ACIDCharacter.cginc"
  ENDCG
 }
}
Fallback "Hidden/Shadow/Opaque"
}

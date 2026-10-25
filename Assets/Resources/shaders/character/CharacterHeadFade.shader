// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/character/CharacterHeadFade.shader.cg, not this file.
Shader "Character/CharacterHeadFade" {
Properties {
 _MainTex ("Albedo (RGB) AO (A)", 2D) = "white" { }
 _BumpMap ("Normalmap", 2D) = "bump" { }
 _FadeColor ("Fade Color", Color) = (1,1,1,1)
 _RimColor ("RimColor, will be set from code", Color) = (1,0,1,1)
}
SubShader { 
 LOD 99
 Tags { "LightMode"="ForwardBase" "Queue"="AlphaTest+2" "RenderType"="Opaque" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="AlphaTest+2" "RenderType"="Opaque" }
  ZTest Less
  ColorMask 0
  CGPROGRAM
  #pragma vertex vertDepth
  #pragma fragment fragDepth
  #include "../../../Shader/Include/ACIDDepthOnly.cginc"
  ENDCG
 }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="AlphaTest+2" "RenderType"="Opaque" }
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  CGPROGRAM
  #pragma vertex vertChar
  #pragma fragment fragChar
  #pragma multi_compile __ SHADOWS_SCREEN
  #pragma multi_compile SIMPLE_FADE FANCY_FADE RASTER_FADE
  #define ACID_CHAR_HEAD
  #define ACID_CHAR_FADE
  #define ACID_CHAR_BUMP
  #include "../../../Shader/Include/ACIDCharacter.cginc"
  ENDCG
 }
}
Fallback "Hidden/Shadow/Opaque"
}

// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/character/CharacterBody2ColFade_LQ.shader.cg, not this file.
Shader "Character/CharacterBody2ColFade_LQ" {
Properties {
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
 _MaskTex ("Colorize (RG) Specular Mask (B)", 2D) = "white" { }
 _Color0 ("Color 0", Color) = (1,1,1,1)
 _Color1 ("Color 1", Color) = (1,1,1,1)
 _NoiseGlow ("Noise glow color and layer", Color) = (1,1,1,0)
 _FadeColor ("Fade Color", Color) = (1,1,1,1)
 _MatGlossiness ("Glossiness", Float) = 1
 _RimColor ("RimColor, will be set from code", Color) = (1,0,1,1)
}
SubShader { 
 LOD 99
 Tags { "LightMode"="ForwardBase" "Queue"="AlphaTest+2" "RenderType"="Opaque" "EagleVision"="humanoid" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="AlphaTest+2" "RenderType"="Opaque" "EagleVision"="humanoid" }
  ZTest Less
  ColorMask 0
  CGPROGRAM
  #pragma vertex vertDepth
  #pragma fragment fragDepth
  #include "../../../Shader/Include/ACIDDepthOnly.cginc"
  ENDCG
 }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="AlphaTest+2" "RenderType"="Opaque" "EagleVision"="humanoid" }
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  CGPROGRAM
  #pragma vertex vertChar
  #pragma fragment fragChar
  #pragma multi_compile __ SHADOWS_SCREEN
  #pragma multi_compile SIMPLE_FADE FANCY_FADE RASTER_FADE
  #define ACID_CHAR_2COL
  #define ACID_CHAR_FADE
  #include "../../../Shader/Include/ACIDCharacter.cginc"
  ENDCG
 }
}
Fallback "Hidden/Shadow/Opaque"
}

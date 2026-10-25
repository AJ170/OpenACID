// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/character/CharacterBody2ColCustomLight.shader.cg, not this file.
Shader "Character/CharacterCustomLight" {
Properties {
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
 _BumpMap ("Normalmap", 2D) = "bump" { }
 _MaskTex ("Colorize (RG) Specular Mask (B)", 2D) = "white" { }
 _Color0 ("Color 0", Color) = (1,1,1,1)
 _Color1 ("Color 1", Color) = (1,1,1,1)
 _MatGlossiness ("Glossiness", Float) = 1
 _NoiseGlow ("Noise glow color and layer", Color) = (1,1,1,0)
[HideInInspector]  _RimColor ("RimColor, will be set from code", Color) = (1,0,1,1)
 _LightParametersLocal ("Light Params (x: SH, y: Desat, z: AO, w: LightDimmer", Vector) = (1,0,1,0)
}
SubShader { 
 LOD 99
 Tags { "LightMode"="ForwardBase" "Queue"="Geometry" "RenderType"="Opaque" "EagleVision"="humanoid" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="Geometry" "RenderType"="Opaque" "EagleVision"="humanoid" }
  CGPROGRAM
  #pragma vertex vertChar
  #pragma fragment fragChar
  #pragma multi_compile __ SHADOWS_SCREEN
  #pragma multi_compile FX_OFF FX_NOISEGLOW FX_HIGHLIGHT
  #define ACID_CHAR_2COL
  #define ACID_CHAR_CUSTOMLIGHT
  #define ACID_CHAR_BUMP
  #include "../../../Shader/Include/ACIDCharacter.cginc"
  ENDCG
 }
}
Fallback "Hidden/Shadow/Opaque"
}

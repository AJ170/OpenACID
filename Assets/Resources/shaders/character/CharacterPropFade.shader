// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/character/CharacterPropFade.shader.cg, not this file.
Shader "Character/CharacterPropFade" {
Properties {
 _MainTex ("Albedo (RGB) Specular Mask (A)", 2D) = "white" { }
 _BumpMap ("Normalmap", 2D) = "bump" { }
 _RampTex ("Ramp Texture", 2D) = "bump" { }
 _FadeColor ("Fade Color", Color) = (1,1,1,1)
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
  #pragma multi_compile SIMPLE_FADE FANCY_FADE RASTER_FADE
  #define ACID_CHAR_PROP
  #define ACID_CHAR_FADE
  #define ACID_CHAR_BUMP
  #include "../../../Shader/Include/ACIDCharacter.cginc"
  ENDCG
 }
}
Fallback "Hidden/Shadow/Opaque"
}

// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/character/CharacterBody4Col_LQ.shader.cg, not this file.
Shader "Character/CharacterBody4Col_LQ" {
Properties {
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
 _DetailA ("DetailA", 2D) = "bump" { }
 _DetailB ("DetailB", 2D) = "bump" { }
 _DetailScale ("DetailScale", Vector) = (2,2,3,3)
 _Color0 ("ColorMask White + Detail B", Color) = (1,1,1,1)
 _Color1 ("ColorMask Grey 150", Color) = (1,1,1,1)
 _Color2 ("ColorMask Grey 80", Color) = (1,1,1,1)
 _Color3 ("ColorMask Black + Detail A", Color) = (1,1,1,1)
 _MatGlossiness ("Glossiness", Float) = 1
 _NoiseGlow ("Noise glow color and layer", Color) = (1,1,1,0)
 _RimColor ("RimColor, will be set from code", Color) = (1,0,1,1)
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
  #define ACID_CHAR_4COL
  #include "../../../Shader/Include/ACIDCharacter.cginc"
  ENDCG
 }
}
Fallback "Hidden/Shadow/Opaque"
}

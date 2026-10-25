// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/GenericEffect-Mobile-Additive.shader.cg, not this file.
Shader "Effects/Additive" {
Properties {
 _MainTex ("Particle Texture", 2D) = "white" { }
 _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,1)
}
SubShader { 
 Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha One
  ColorMask RGB
  CGPROGRAM
  #pragma vertex vertFX
  #pragma fragment fragFX
  #pragma multi_compile FOG_OFF FOG_ON
  #pragma multi_compile TINT_COLOR_OFF TINT_COLOR_ON
  #pragma multi_compile VERTEX_COLOR_OFF VERTEX_COLOR_ON
  #include "Include/ACIDEffects.cginc"
  ENDCG
 }
}
}

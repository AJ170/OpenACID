// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Effects/Alpha (Overlay).shader.cg, not this file.
Shader "Effects/Alpha (Overlay)" {
Properties {
 _MainTex ("Particle Texture", 2D) = "white" { }
 _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,1)
}
SubShader { 
 Tags { "Queue"="Overlay" "IgnoreProjector"="true" "RenderType"="Overlay" }
 Pass {
  Tags { "Queue"="Overlay" "IgnoreProjector"="true" "RenderType"="Overlay" }
  ZTest Always
  ZWrite Off
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  CGPROGRAM
  #pragma vertex vertFX
  #pragma fragment fragFX
  #pragma multi_compile FOG_OFF FOG_ON
  #pragma multi_compile TINT_COLOR_OFF TINT_COLOR_ON
  #pragma multi_compile VERTEX_COLOR_OFF VERTEX_COLOR_ON
  #include "../../Include/ACIDEffects.cginc"
  ENDCG
 }
}
}

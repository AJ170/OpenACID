// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/Unlit - Transparent Colored 2.shader.cg, not this file.
Shader "Hidden/Unlit/Transparent Colored 2" {
Properties {
 _MainTex ("Base (RGB), Alpha (A)", 2D) = "black" { }
}
SubShader { 
 Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  Offset -1, -1
  CGPROGRAM
  #pragma vertex vertNGUI
  #pragma fragment fragNGUI
  #define NGUI_COLORED
  #define NGUI_CLIP_RECT 2
  #include "../../Shader/Include/ACIDNGUI.cginc"
  ENDCG
 }
}
}

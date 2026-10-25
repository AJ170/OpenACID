// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/Unlit - Text 1.shader.cg, not this file.
Shader "Hidden/Unlit/Text 1" {
Properties {
 _MainTex ("Alpha (A)", 2D) = "white" { }
}
SubShader { 
 Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
  Offset -1, -1
  CGPROGRAM
  #pragma vertex vertNGUI
  #pragma fragment fragNGUI
  #define NGUI_TEXT
  #define NGUI_CLIP_RECT 1
  #include "../../Shader/Include/ACIDNGUI.cginc"
  ENDCG
 }
}
Fallback "Unlit/Text"
}

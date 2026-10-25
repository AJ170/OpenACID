// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/Unlit - Premultiplied Colored.shader.cg, not this file.
Shader "Unlit/Premultiplied Colored" {
Properties {
 _MainTex ("Base (RGB), Alpha (A)", 2D) = "black" { }
}
SubShader { 
 Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend One OneMinusSrcAlpha
  ColorMask RGB
  Offset -1, -1
  CGPROGRAM
  #pragma vertex vertNGUI
  #pragma fragment fragNGUI
  #define NGUI_PREMUL
  #include "../../Shader/Include/ACIDNGUI.cginc"
  ENDCG
 }
}
}

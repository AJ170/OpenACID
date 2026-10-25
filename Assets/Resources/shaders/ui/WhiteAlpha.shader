// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/ui/WhiteAlpha.shader.cg, not this file.
Shader "UI/White Alpha Shader" {
Properties {
 _MainTex ("Font Texture", 2D) = "white" { }
 _Color ("Text Color", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" "PreviewType"="Plane" }
 Pass {
  Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" "PreviewType"="Plane" }
  ZTest Always
  ZWrite Off
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
  CGPROGRAM
  #pragma vertex vertUI
  #pragma fragment fragUI
  #define ACID_UI_TEXT_WHITE
  #include "../../../Shader/Include/ACIDUI.cginc"
  ENDCG
 }
}
}

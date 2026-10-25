// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/ui/AchievmentIcon.shader.cg, not this file.
Shader "UI/AchievmentIcon" {
Properties {
 _MainTex ("Icon (ARGB)", 2D) = "white" { }
 _FrameTex ("Frame (ARGB)", 2D) = "white" { }
 _Desaturation ("Desaturation Amount", Range(0,1)) = 0
 _Alpha ("Alpha", Range(0,1)) = 1
}
SubShader { 
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "RenderType"="Opaque" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  CGPROGRAM
  #pragma vertex vertUI
  #pragma fragment fragUI
  #define ACID_UI_ACHIEVEMENT
  #include "../../../Shader/Include/ACIDUI.cginc"
  ENDCG
 }
}
}

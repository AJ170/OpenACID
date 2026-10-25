// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/ui/EventBanner.shader.cg, not this file.
Shader "UI/EventBanner" {
Properties {
 _MainTex ("Main Texture", 2D) = "white" { }
 _Stencil ("Stencil Texture", 2D) = "white" { }
 _Color0 ("Color1", Color) = (1,1,1,1)
 _YOffsetVec ("YOffset", Vector) = (0,0,0,0)
}
SubShader { 
 Pass {
  Blend SrcAlpha OneMinusSrcAlpha
  CGPROGRAM
  #pragma vertex vertUI
  #pragma fragment fragUI
  #define ACID_UI_EVENTBANNER
  #include "../../../Shader/Include/ACIDUI.cginc"
  ENDCG
 }
}
}

// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/ui/MiniMapMaskRestricted.shader.cg, not this file.
Shader "UI/Minimap/MiniMapBackgroundRestricted" {
Properties {
 _MainTex ("Icon (RGBA)", 2D) = "white" { }
 _MaskTex ("Masking Texture", 2D) = "white" { }
 _Region ("Region (x,y,w,h)", Vector) = (0,0,1,1)
}
SubShader { 
 Tags { "Queue"="Overlay" }
 Pass {
  Tags { "Queue"="Overlay" }
  Blend SrcAlpha OneMinusSrcAlpha
  CGPROGRAM
  #pragma vertex vertUI
  #pragma fragment fragUI
  #define ACID_UI_MINIMAP_RESTRICTED
  #include "../../../Shader/Include/ACIDUI.cginc"
  ENDCG
 }
}
}

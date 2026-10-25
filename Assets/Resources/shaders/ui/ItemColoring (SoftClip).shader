// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/ui/ItemColoring (SoftClip).shader.cg, not this file.
Shader "UI/ItemColoring (SoftClip)" {
Properties {
 _MainTex ("Background Texture", 2D) = "white" { }
 _MaskTex ("Masking Texture", 2D) = "white" { }
 _Foreground ("Foreground Texture", 2D) = "white" { }
 _Color0 ("Color1", Color) = (1,1,1,1)
 _Color1 ("Color2", Color) = (1,1,1,1)
 _Rarity ("Rarity color", Color) = (1,1,1,1)
 _Desaturation ("Desaturation Amount", Range(0,1)) = 0
}
SubShader { 
 Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  CGPROGRAM
  #pragma vertex vertUI
  #pragma fragment fragUI
  #pragma multi_compile BACKGROUND_OFF BACKGROUND_ON
  #define ACID_UI_ITEMCOLORING
  #define ACID_SOFTCLIP
  #include "../../../Shader/Include/ACIDUI.cginc"
  ENDCG
 }
}
}

// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/ui/PortraitColoringAssassinHeroic (SoftClip).shader.cg, not this file.
Shader "UI/PortraitColoringAssassinHeroic (SoftClip)" {
Properties {
 _MainTex ("Background Texture", 2D) = "white" { }
 _MaskTex ("Masking Texture", 2D) = "white" { }
 _Foreground ("Foreground Texture", 2D) = "white" { }
 _HeroicFrame ("Heroic Frame Texture", 2D) = "white" { }
 _HeroicStencil ("HF Stencil Texture", 2D) = "white" { }
 _ShowInsigniaItem ("Show Insignia Item", Float) = 0
 _InsigniaItem ("Insignia Item Texture", 2D) = "white" { }
 _Color0 ("Color1", Color) = (1,1,1,1)
 _Color1 ("Color2", Color) = (1,1,1,1)
}
SubShader { 
 Pass {
  Blend SrcAlpha OneMinusSrcAlpha
  CGPROGRAM
  #pragma vertex vertUI
  #pragma fragment fragUI
  #define ACID_UI_PORTRAIT_HEROIC
  #define ACID_SOFTCLIP
  #include "../../../Shader/Include/ACIDUI.cginc"
  ENDCG
 }
}
}

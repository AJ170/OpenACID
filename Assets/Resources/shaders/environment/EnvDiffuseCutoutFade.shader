// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/environment/EnvDiffuseCutoutFade.shader.cg, not this file.
Shader "Environment/EnvDiffuseCutoutFade" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
 _FadeColor ("Fade Color", Color) = (1,1,1,1)
}
SubShader { 
 LOD 99
 Tags { "LightMode"="ForwardBase" "Queue"="AlphaTest" "RenderType"="TransparentCutout" "EagleVision"="humanoid" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="AlphaTest" "RenderType"="TransparentCutout" "EagleVision"="humanoid" }
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
  CGPROGRAM
  #pragma vertex vertEnv
  #pragma fragment fragEnv
  #pragma multi_compile SIMPLE_FADE FANCY_FADE RASTER_FADE
  #define ACID_ENV_CUTOUT
  #define ACID_ENV_FADE
  #include "../../../Shader/Include/ACIDEnvironment.cginc"
  ENDCG
 }
}
Fallback "Environment/EnvDiffuseCutout"
}

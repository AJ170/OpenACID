// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/environment/EnvDiffuseBumpedFade.shader.cg, not this file.
Shader "Environment/EnvDiffuseBumpedFade" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _BumpMap ("Normalmap", 2D) = "bump" { }
 _FadeColor ("Fade Color", Color) = (1,1,1,1)
}
SubShader { 
 LOD 99
 Tags { "LightMode"="ForwardBase" "Queue"="AlphaTest" "RenderType"="Opaque" "EagleVision"="humanoid" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="AlphaTest" "RenderType"="Opaque" "EagleVision"="humanoid" }
  ZTest Less
  ColorMask 0
  CGPROGRAM
  #pragma vertex vertDepth
  #pragma fragment fragDepth
  #include "../../../Shader/Include/ACIDDepthOnly.cginc"
  ENDCG
 }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="AlphaTest" "RenderType"="Opaque" "EagleVision"="humanoid" }
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  CGPROGRAM
  #pragma vertex vertEnv
  #pragma fragment fragEnv
  #pragma multi_compile SIMPLE_FADE FANCY_FADE RASTER_FADE
  #define ACID_ENV_BUMP
  #define ACID_ENV_FADE
  #include "../../../Shader/Include/ACIDEnvironment.cginc"
  ENDCG
 }
}
Fallback "Environment/EnvDiffuseBumped"
}

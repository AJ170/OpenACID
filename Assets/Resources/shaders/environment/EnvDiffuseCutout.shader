// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/environment/EnvDiffuseCutout.shader.cg, not this file.
Shader "Environment/EnvDiffuseCutout" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
 _ApplyRimIntensity ("ApplyRimIntensity", Float) = 0
}
SubShader { 
 LOD 99
 Tags { "LightMode"="ForwardBase" "Queue"="AlphaTest" "RenderType"="TransparentCutout" "EagleVision"="humanoid" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="AlphaTest" "RenderType"="TransparentCutout" "EagleVision"="humanoid" }
  Cull Off
  CGPROGRAM
  #pragma vertex vertEnv
  #pragma fragment fragEnv
  #pragma multi_compile __ SHADOWS_SCREEN
  #define ACID_ENV_CUTOUT
  #include "../../../Shader/Include/ACIDEnvironment.cginc"
  ENDCG
 }
}
Fallback "Hidden/Shadow/Cutout"
}

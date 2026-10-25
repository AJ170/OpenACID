// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/environment/EnvDiffuse.shader.cg, not this file.
Shader "Environment/EnvDiffuse" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}
SubShader { 
 LOD 99
 Tags { "LightMode"="ForwardBase" "Queue"="Geometry" "RenderType"="Opaque" "EagleVision"="humanoid" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="Geometry" "RenderType"="Opaque" "EagleVision"="humanoid" }
  CGPROGRAM
  #pragma vertex vertEnv
  #pragma fragment fragEnv
  #pragma multi_compile __ SHADOWS_SCREEN
  #include "../../../Shader/Include/ACIDEnvironment.cginc"
  ENDCG
 }
}
Fallback "Hidden/Shadow/Opaque"
}

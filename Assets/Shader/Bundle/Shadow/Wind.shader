// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Shadow/Wind.shader.cg, not this file.
Shader "Hidden/Shadow/Wind" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _SpecColor ("Spec Color", Color) = (1,1,1,1)
 _Emission ("Emissive Color", Color) = (0,0,0,0)
 _Shininess ("Shininess", Range(0.01,1)) = 0.7
 _MainTex ("Base (RGB)", 2D) = "white" { }
}
SubShader { 
 Pass {
  Name "SHADOWCASTER"
  Tags { "LightMode"="ShadowCaster" }
  Cull Off
  Offset 1, 1
  CGPROGRAM
  #pragma vertex vertGenShadow
  #pragma fragment fragGenShadow
  #pragma multi_compile_shadowcaster
  #include "../../Include/ACIDGenerated.cginc"
  ENDCG
 }
}
}

// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/character/Outlines.shader.cg, not this file.
Shader "Character/Outlines" {
Properties {
 _OutlineColor ("Outline Color", Color) = (0,0,0,1)
 _Outline ("Outline width", Range(0.002,0.03)) = 0.005
 _Offset ("Offset", Float) = 0.05
}
SubShader { 
 LOD 99
 Tags { "LightMode"="ForwardBase" "Queue"="Transparent+2" "RenderType"="Opaque" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="Transparent+2" "RenderType"="Opaque" }
  ZTest Greater
  Cull Off
  ColorMask 0
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "../../../Shader/Include/ACIDCommon.cginc"
  float _Offset;
  float4 vert(float4 vertex : POSITION) : SV_POSITION
  {
      float4 p = UnityObjectToClipPos(vertex);
      float z = ACID_ClipZToGL(p);
      p.z = ACID_GLZToClip(z + (z / p.w) * _Offset, p.w);
      return p;
  }
  fixed4 frag() : SV_Target
  {
      return fixed4(0.0, 0.0, 0.0, 0.0);
  }
  ENDCG
 }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="Transparent+2" "RenderType"="Opaque" }
  ZTest Greater
  Cull Off
  ColorMask RGB
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "../../../Shader/Include/ACIDCommon.cginc"
  float _Outline;
  float _Offset;
  float4 _OutlineColor;
  float4 vert(float4 vertex : POSITION, float3 normal : NORMAL) : SV_POSITION
  {
      float4 p = UnityObjectToClipPos(vertex);
      float z = ACID_ClipZToGL(p);
      float3 viewNormal = mul((float3x3)UNITY_MATRIX_IT_MV, normal);
      p.xy += mul((float2x2)UNITY_MATRIX_P, viewNormal.xy) * z * _Outline;
      p.z = ACID_GLZToClip(z - (z / p.w) * _Offset, p.w);
      return p;
  }
  fixed4 frag() : SV_Target
  {
      return _OutlineColor;
  }
  ENDCG
 }
}
}

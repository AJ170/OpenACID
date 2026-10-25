// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/debug/MeshColliderFront.shader.cg, not this file.
Shader "Hidden/Debug/MeshCollider (Front)" {
SubShader { 
 Tags { "LightMode"="Always" "Queue"="Transparent+2" "RenderType"="Transparent" }
 Pass {
  Tags { "LightMode"="Always" "Queue"="Transparent+2" "RenderType"="Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask 0
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "../../../Shader/Include/ACIDCommon.cginc"
  float4 vert(float4 vertex : POSITION) : SV_POSITION
  {
      float4 p = UnityObjectToClipPos(vertex);
      p.z = ACID_GLZToClip(ACID_ClipZToGL(p) - 4e-05, p.w);
      return p;
  }
  fixed4 frag() : SV_Target { return fixed4(0.0, 0.0, 0.0, 0.0); }
  ENDCG
 }
 Pass {
  Tags { "LightMode"="Always" "Queue"="Transparent+2" "RenderType"="Transparent" }
  ZTest Equal
  ZWrite Off
  Stencil {
   Ref 255
   Pass Replace
  }
  Blend SrcAlpha OneMinusSrcAlpha
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "../../../Shader/Include/ACIDCommon.cginc"
  float4 vert(float4 vertex : POSITION) : SV_POSITION
  {
      float4 p = UnityObjectToClipPos(vertex);
      p.z = ACID_GLZToClip(ACID_ClipZToGL(p) - 4e-05, p.w);
      return p;
  }
  fixed4 frag() : SV_Target { return fixed4(0.0, 1.0, 0.0, 0.3); }
  ENDCG
 }
}
Fallback "Mobile/VertexLit"
}

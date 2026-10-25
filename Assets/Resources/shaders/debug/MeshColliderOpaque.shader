// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/debug/MeshColliderOpaque.shader.cg, not this file.
Shader "Hidden/Debug/MeshCollider (Opaque)" {
SubShader { 
 Tags { "LightMode"="Always" "Queue"="Transparent-1" "RenderType"="Opaque" }
 Pass {
  Tags { "LightMode"="Always" "Queue"="Transparent-1" "RenderType"="Opaque" }
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc"
  struct v2f { float4 pos : SV_POSITION; float3 worldPos : TEXCOORD0; };
  v2f vert(float4 vertex : POSITION)
  {
      v2f o;
      o.pos = UnityObjectToClipPos(vertex);
      o.worldPos = mul(unity_ObjectToWorld, vertex).xyz;
      return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
      float3 s = pow(abs(sin(i.worldPos * 2.0)), float3(4.0, 4.0, 4.0)) * 0.5;
      float g = s.x + s.y + s.z;
      return fixed4(g, g, g, g);
  }
  ENDCG
 }
}
Fallback "Mobile/VertexLit"
}

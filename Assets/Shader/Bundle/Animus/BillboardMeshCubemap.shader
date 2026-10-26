// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Animus/BillboardMeshCubemap.shader.cg, not this file.
Shader "Animus/BillboardMeshCubemap" {
Properties {
 _MainTex ("Billboard Texture", 2D) = "white" { }
 _FadeOffset ("Fade Offset", Float) = 0.25
}
SubShader { 
 Tags { "Queue"="Background" "IgnoreProjector"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "Queue"="Background" "IgnoreProjector"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha One
  ColorMask RGB
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc"
  sampler2D _MainTex;
  float _FadeOffset;
  struct v2f
  {
      float4 pos : SV_POSITION;
      float2 uv : TEXCOORD0;
      float4 color : TEXCOORD1;
  };
  // Sky particles around the camera (the mesh is centred on it). Each quad drifts and circles;
  // texcoord1 holds its corner offset, whose angle sets the circling phase.
  v2f vert(appdata_full v)
  {
      v2f o;
      float4 wpos = mul(unity_ObjectToWorld, v.vertex);
      wpos.xyz += _WorldSpaceCameraPos;
      float4 vpos = mul(UNITY_MATRIX_V, wpos);
      float t = v.vertex.x * v.vertex.y * v.vertex.z + _Time.z * 0.25;
      float swirl = sin(t * 0.05) * 10.0;
      float angle = atan2(v.texcoord1.x, v.texcoord1.y);
      float radius = 1.0 + sin(t) * 0.25;
      vpos.x += sin(t * 0.5) * 0.25 + sin(swirl + angle) * radius;
      vpos.y += -0.25 - sin(t) * 0.1 + cos(swirl + angle) * radius;
      o.color = v.color;
      o.color.a = vpos.y - _FadeOffset;
      o.pos = mul(UNITY_MATRIX_P, vpos);
      o.uv = v.texcoord.xy;
      return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
      return tex2D(_MainTex, i.uv) * i.color;
  }
  ENDCG
 }
}
}

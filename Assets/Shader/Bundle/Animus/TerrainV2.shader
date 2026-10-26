// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Animus/TerrainV2.shader.cg, not this file.
Shader "Animus/TerrainV2" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Billboard Texture", 2D) = "white" { }
}
SubShader { 
 Tags { "Queue"="Background" "IgnoreProjector"="true" }
 Pass {
  Tags { "Queue"="Background" "IgnoreProjector"="true" }
  ZWrite Off
  Cull Off
  Blend One One
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc"
  half4 _Color;
  sampler2D _MainTex;
  struct v2f
  {
      float4 pos : SV_POSITION;
      half4 color : COLOR;
      half2 uv : TEXCOORD0;
  };
  // Swaying billboards: vertex colour alpha and normal.z weight two wave sets, texcoord1 holds the
  // quad corner offset in view space.
  v2f vert(appdata_full v)
  {
      v2f o;
      float4 wpos = mul(unity_ObjectToWorld, v.vertex);
      float zx = wpos.z * wpos.x;
      float deg = zx * 0.0075 + _Time.z * 0.33;
      float deg3 = zx * 0.75 + _Time.z;
      float waveInverse = 1.0 - v.normal.z;
      float4 p = wpos;
      p.x += cos(deg * 0.5) * v.color.a * 2.0 * waveInverse;
      p.y += sin(deg * 0.5) * v.color.a * 4.0 * waveInverse;
      p.x += cos(deg3 * 0.75) * v.normal.z * v.color.a * 0.5;
      p.y += sin(deg3 * 0.75) * v.normal.z * v.color.a * 0.5;
      float4 vpos = mul(UNITY_MATRIX_V, p);
      half4 corner = v.texcoord1 * (1.05 + sin((zx * 0.0075 + _Time.z * 0.2) * 3.5) * 0.35 * v.color.a);
      corner *= 1.05 + sin(deg3 * 0.5) * 0.55 * v.normal.z;
      o.pos = mul(UNITY_MATRIX_P, vpos + corner);
      o.color = v.color;
      o.uv = v.texcoord.xy;
      return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
      return tex2D(_MainTex, i.uv) * i.color * _Color;
  }
  ENDCG
 }
}
}

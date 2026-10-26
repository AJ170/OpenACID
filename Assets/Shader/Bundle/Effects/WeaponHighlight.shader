// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Effects/WeaponHighlight.shader.cg, not this file.
Shader "Effects/WeaponHighlight" {
Properties {
 _NoiseA ("Night (RGB)", 2D) = "white" { }
 _Scale ("Scale", Range(0,0.1)) = 0.01
 _Contrast ("Contrast", Range(1,4)) = 1
 _Color ("Color", Color) = (1,1,1,1)
 _Offsets ("Offsets (x,y = noise a)(z,w = noise b)", Vector) = (0,0,0,0)
}
SubShader { 
 Tags { "Queue"="Transparent" }
 Pass {
  Tags { "Queue"="Transparent" }
  ZWrite Off
  Blend One One
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc"
  sampler2D _NoiseA;
  float4 _Offsets;
  float _Scale;
  float _Contrast;
  fixed4 _Color;
  struct appdata
  {
      float4 vertex : POSITION;
      float3 normal : NORMAL;
      float4 texcoord : TEXCOORD0;
      float4 texcoord1 : TEXCOORD1;
      fixed4 color : COLOR;
  };
  struct v2f
  {
      float4 pos : SV_POSITION;
      fixed4 color : COLOR;
      half2 uv0 : TEXCOORD0;
      half2 uv1 : TEXCOORD1;
  };
  v2f vert(appdata v)
  {
      v2f o;
      o.pos = UnityObjectToClipPos(float4(v.vertex.xyz + v.normal * _Scale, 1.0));
      o.color = v.color;
      o.uv0 = v.texcoord.xy + _Offsets.xy;
      o.uv1 = v.texcoord1.xy + _Offsets.zw;
      return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
      fixed noise = tex2D(_NoiseA, i.uv0).x + tex2D(_NoiseA, i.uv1).y - 1.0;
      fixed4 c = _Color * _Contrast * (noise * i.color);
      return c * (_Color.a * i.color.a);
  }
  ENDCG
 }
}
}

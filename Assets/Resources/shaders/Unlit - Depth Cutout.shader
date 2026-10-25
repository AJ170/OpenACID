// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/Unlit - Depth Cutout.shader.cg, not this file.
Shader "Unlit/Depth Cutout" {
Properties {
 _MainTex ("Base (RGB), Alpha (A)", 2D) = "white" { }
}
SubShader { 
 Tags { "Queue"="Background" "IgnoreProjector"="true" }
 Pass {
  Tags { "Queue"="Background" "IgnoreProjector"="true" }
  ZTest Less
  Cull Off
  ColorMask 0
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc"
  sampler2D _MainTex;
  float4 _MainTex_ST;
  struct v2f { float4 pos : SV_POSITION; fixed4 color : COLOR; float2 uv : TEXCOORD0; };
  v2f vert(appdata_full v)
  {
      v2f o;
      o.pos = UnityObjectToClipPos(v.vertex);
      o.color = saturate(v.color);
      o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
      return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
      fixed4 col = tex2D(_MainTex, i.uv) * i.color;
      if (col.a <= 0.99) discard;  // AlphaTest Greater 0.99
      return col;
  }
  ENDCG
 }
}
}

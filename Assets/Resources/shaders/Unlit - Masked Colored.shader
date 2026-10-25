// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/Unlit - Masked Colored.shader.cg, not this file.
Shader "Unlit/Masked Colored" {
Properties {
 _MainTex ("Base (RGB) Mask (A)", 2D) = "black" { }
 _Color ("Tint Color", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  ColorMask RGB
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc"
  sampler2D _MainTex;
  float4 _MainTex_ST;
  fixed4 _Color;
  struct v2f { float4 pos : SV_POSITION; fixed4 color : COLOR; float2 uv : TEXCOORD0; };
  v2f vert(appdata_full v)
  {
      v2f o;
      o.pos = UnityObjectToClipPos(v.vertex);
      o.color = v.color;
      o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
      return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
      fixed4 col = tex2D(_MainTex, i.uv) * i.color;
      col.rgb = lerp(col.rgb, col.rgb * _Color.rgb, col.a);
      return col;
  }
  ENDCG
 }
}
}

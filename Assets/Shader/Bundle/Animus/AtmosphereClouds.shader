// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Animus/AtmosphereClouds.shader.cg, not this file.
Shader "Animus/AtmosphereClouds" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _Color ("Color", Color) = (1,1,1,0.5)
}
SubShader { 
 Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
  Lighting On
  ZWrite Off
  Cull Off
  Blend SrcAlpha One
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc"
  sampler2D _MainTex;
  float4 _Color;
  struct v2f
  {
      float4 pos : SV_POSITION;
      float2 uv : TEXCOORD0;
      float4 color : COLOR;
  };
  v2f vert(appdata_base v)
  {
      v2f o;
      float3 wpos = mul(unity_ObjectToWorld, v.vertex).xyz;
      half fade = saturate(distance(_WorldSpaceCameraPos, wpos) * 0.6 - 2.0);
      float3 n = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
      o.color.rgb = _Color.rgb * (1.5 * max(0.0, dot(n, normalize(_WorldSpaceLightPos0.xyz))));
      o.color.a = fade;
      o.pos = UnityObjectToClipPos(v.vertex);
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

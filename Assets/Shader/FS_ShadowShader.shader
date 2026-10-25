// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/FS_ShadowShader.shader.cg, not this file.
Shader "Hidden/FS_ShadowShader/UnlitAlphaBlend" {
Properties {
 _MainTex ("Base (RGB), Alpha (A)", 2D) = "white" { }
}
SubShader { 
 Tags { "Queue"="Transparent-1" "IgnoreProjector"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "Queue"="Transparent-1" "IgnoreProjector"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  Offset -1, -1
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "Include/ACIDCommon.cginc"
  sampler2D _MainTex;
  float4 _MainTex_ST;
  float4 _EnvAmbientColor;
  struct v2f { float4 pos : SV_POSITION; fixed4 color : COLOR; half2 fog : TEXCOORD2; float2 uv : TEXCOORD0; };
  v2f vert(appdata_full v)
  {
      v2f o;
      o.pos = UnityObjectToClipPos(v.vertex);
      o.color = v.color * _EnvAmbientColor;
      o.fog = ACID_FogFactors(v.vertex);
      o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
      return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
      fixed4 c = tex2D(_MainTex, i.uv) * i.color;
      c.rgb = ACID_ApplyFog(c.rgb, i.fog);
      return c;
  }
  ENDCG
 }
}
}

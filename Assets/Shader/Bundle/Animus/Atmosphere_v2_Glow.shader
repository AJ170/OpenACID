// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Animus/Atmosphere_v2_Glow.shader.cg, not this file.
Shader "Animus/Atmosphere_v2_Glow" {
Properties {
 _Color ("Color", Color) = (1,1,1,0.5)
 _Amplitude ("Amplitude", Float) = 1
 _Offset ("Offset", Float) = 0
 _Amplitude2 ("Amplitude2", Float) = 1
 _Offset2 ("Offset2", Float) = 0
}
SubShader { 
 Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend One One
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc"
  #include "Lighting.cginc"
  float4 _Color;
  float _Amplitude;
  float _Offset;
  float _Amplitude2;
  float _Offset2;
  struct v2f
  {
      float4 pos : SV_POSITION;
      float3 normal : TEXCOORD0;
      float3 viewDir : TEXCOORD1;
      float3 color : COLOR;
  };
  v2f vert(appdata_base v)
  {
      v2f o;
      float3 wpos = mul(unity_ObjectToWorld, v.vertex).xyz;
      half fade = saturate(distance(_WorldSpaceCameraPos, wpos) - 2.0);
      o.normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
      o.viewDir = normalize(_WorldSpaceCameraPos - wpos);
      o.color = _LightColor0.rgb * _Color.rgb * max(0.0, dot(o.normal, normalize(_WorldSpaceLightPos0.xyz))) * fade;
      o.pos = UnityObjectToClipPos(v.vertex);
      return o;
  }
  float4 frag(v2f i) : SV_Target
  {
      // rim band: two sine lobes over the view/normal angle
      float t = min(1.0, _Color.a * abs(dot(normalize(i.viewDir), normalize(i.normal))));
      return float4(i.color * (sin(t * _Amplitude + _Offset) * sin(t * _Amplitude2 + _Offset2)), 1.0);
  }
  ENDCG
 }
}
}

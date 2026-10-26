// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Barrier/Beam.shader.cg, not this file.
Shader "Barrier/Beam" {
Properties {
 _MainTex ("Billboard Texture", 2D) = "white" { }
 _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,1)
 _Thickness ("Thickness", Float) = 1
 _Frequency ("Frequency", Vector) = (0.3,0.35,0.25,0.25)
 _Amplitude ("Amplitude", Vector) = (1,1,1,1)
 _FadeDistance ("Fade Distance", Float) = 25
}
SubShader { 
 Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
  ZWrite Off
  Blend One One
  ColorMask RGB
  CGPROGRAM
  // a: position along it), additive and pulsing
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc"
  sampler2D _MainTex;
  float4 _TintColor;
  float4 _Frequency;
  float4 _Amplitude;
  float _Thickness;
  float _FadeDistance;
  struct appdata
  {
      float4 vertex : POSITION;
      float3 normal : NORMAL;
      float4 tangent : TANGENT;
      float4 texcoord : TEXCOORD0;
      float4 texcoord1 : TEXCOORD1;
      fixed4 color : COLOR;
  };
  struct v2f
  {
      float4 pos : SV_POSITION;
      float2 uv : TEXCOORD0;
      half4 color : TEXCOORD1;
  };
  v2f vert(appdata v)
  {
      v2f o;
      const float3 up = float3(0.0, 1.0, 0.0);
      float3 pos = mul(unity_ObjectToWorld, v.vertex).xyz;
      float3 normal = mul((float3x3)unity_ObjectToWorld, v.normal);
      float hv = v.color.y;
      float4 s = sin(normal.y * 6.283185 + (_Frequency * _Time.zzzz) * (0.5 + normal.x * 0.5)) * _Amplitude;
      float3 sway = lerp(float3(s.xy + s.zw, 0.0), float3(s.x + s.z, 0.0, s.y + s.w), hv);
      float3 right = mul((float3x3)unity_ObjectToWorld, v.tangent.xyz);
      right = normalize(float3(right.x, 0.0, right.z));
      float3 forward = cross(right, up);
      pos += sway.x * forward + sway.y * up;
      float3 toCam = _WorldSpaceCameraPos - pos;
      float3 local = right * toCam.z + up * toCam.y + forward * toCam.x;
      float camDist = length(lerp(local.xy, local.xz, v.color.yy));
      float thickness = _Thickness * min(1.0, camDist * 0.1);
      float2 size = lerp(float2(v.tangent.w, thickness), float2(thickness, v.tangent.w), hv) * (v.texcoord.xy * 2.0 - 1.0);
      float3 viewDir = normalize(toCam);
      float center = v.color.w - 0.4980392;
      float3 horizontal = right * size.x + normalize(cross(right, viewDir)) * size.y + right * center * _FadeDistance;
      float3 vertical = normalize(cross(viewDir, up)) * size.x + up * size.y + up * center * _FadeDistance;
      pos += lerp(horizontal, vertical, hv);
      o.pos = mul(UNITY_MATRIX_VP, float4(pos, 1.0));
      o.uv = lerp(v.texcoord1, v.texcoord, hv).xy;
      float pulse = sin(normal.x * 6.283185 + _Time.z * (0.5 + normal.y * 0.5));
      o.color = 2.0 * _TintColor * (1.0 + pulse) * (1.0 - abs(center) * 2.0);
      float3 a = abs(local);
      half fadeV = max(min(1.0, pow(12.5 / a.x, 4.0)) - saturate((a.z - v.tangent.w) * 0.05), 0.0);
      half fadeH = min(1.0, pow(17.5 / camDist, 4.0));
      o.color.rgb *= lerp(fadeV, fadeH, saturate(v.color.y));
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

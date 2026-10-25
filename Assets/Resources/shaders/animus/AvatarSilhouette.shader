// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/animus/AvatarSilhouette.shader.cg, not this file.
Shader "Animus/AvatarSilhouette" {
Properties {
 _Color ("Main Color", Color) = (0.1,0.1,0.1,1)
 _FadeColor ("Fade Color", Color) = (1,1,1,1)
}
SubShader { 
 LOD 99
 Tags { "LightMode"="Always" "Queue"="Geometry" "ForceNoShadowCasting"="true" "RenderType"="Opaque" }
 Pass {
  Tags { "LightMode"="Always" "Queue"="Geometry" "ForceNoShadowCasting"="true" "RenderType"="Opaque" }
  ZTest Less
  ColorMask 0
  CGPROGRAM
  #pragma vertex vertDepth
  #pragma fragment fragDepth
  #include "../../../Shader/Include/ACIDDepthOnly.cginc"
  ENDCG
 }
 Pass {
  Tags { "LightMode"="Always" "Queue"="Geometry" "ForceNoShadowCasting"="true" "RenderType"="Opaque" }
  Blend SrcAlpha OneMinusSrcAlpha
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "../../../Shader/Include/ACIDCommon.cginc"
  float4 _Color;
  float4 _FadeColor;
  struct v2f { float4 pos : SV_POSITION; float2 viewXY : TEXCOORD1; };
  v2f vert(float4 vertex : POSITION)
  {
      v2f o;
      o.pos = UnityObjectToClipPos(vertex);
      o.viewXY = UnityObjectToViewPos(vertex.xyz).xy;
      return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
      half band = _FadeColor.a - (0.875 * tex2D(_NoiseTexture, i.viewXY).y + 0.0625);
      fixed4 c;
      c.rgb = _FadeColor.rgb * (saturate(1.0 - 16.0 * abs(band)) + _Color.rgb);
      c.a = saturate(16.0 * band + 0.875) * _Color.a;
      return c;
  }
  ENDCG
 }
}
Fallback "Mobile/VertexLit"
}

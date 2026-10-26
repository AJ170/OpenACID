// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Animus/GlobeFadeOut.shader.cg, not this file.
Shader "Animus/GlobeFadeOut" {
Properties {
 _MainTex ("Main Texture", 2D) = "white" { }
 _ZoomFade ("Zoom Fade", Float) = 0.25
}
SubShader { 
 Tags { "LightMode"="ForwardBase" "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" }
  Lighting On
  Blend SrcAlpha OneMinusSrcAlpha
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc"
  #include "Lighting.cginc"
  sampler2D _MainTex;
  half _ZoomFade;
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
      // fades towards _ZoomFade as the camera comes within 3 units
      half fade = saturate(distance(_WorldSpaceCameraPos, wpos) - 2.0);
      float3 n = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
      o.color.rgb = _LightColor0.rgb * max(0.0, dot(n, normalize(_WorldSpaceLightPos0.xyz)));
      o.color.a = _ZoomFade + fade * (1.0 - _ZoomFade);
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

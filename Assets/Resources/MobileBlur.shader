// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/MobileBlur.shader.cg, not this file.
Shader "Hidden/FastBlur" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _Bloom ("Bloom (RGB)", 2D) = "black" { }
}
SubShader { 
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  CGPROGRAM
  #include "../Shader/Include/ACIDBlur.cginc"
  half4 _Parameter;
  #pragma vertex vert
  #pragma fragment frag
  struct v2f_tap { float4 pos : SV_POSITION; half2 uv0 : TEXCOORD0; half2 uv1 : TEXCOORD1; half2 uv2 : TEXCOORD2; half2 uv3 : TEXCOORD3; };
  v2f_tap vert(appdata_img v)
  {
      v2f_tap o;
      o.pos = UnityObjectToClipPos(v.vertex);
      o.uv0 = v.texcoord.xy + _MainTex_TexelSize.xy;
      o.uv1 = v.texcoord.xy + _MainTex_TexelSize.xy * half2(-0.5, -0.5);
      o.uv2 = v.texcoord.xy + _MainTex_TexelSize.xy * half2(0.5, -0.5);
      o.uv3 = v.texcoord.xy + _MainTex_TexelSize.xy * half2(-0.5, 0.5);
      return o;
  }
  fixed4 frag(v2f_tap i) : SV_Target
  {
      fixed4 color = tex2D(_MainTex, i.uv0) + tex2D(_MainTex, i.uv1);
      color += tex2D(_MainTex, i.uv2);
      color += tex2D(_MainTex, i.uv3);
      return color / 4.0;
  }
  ENDCG
 }
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  CGPROGRAM
  #include "../Shader/Include/ACIDBlur.cginc"
  half4 _Parameter;
  #pragma vertex vert
  #pragma fragment frag
  v2f_blur8 vert(appdata_img v) { return ACID_VertBlur8(v, _MainTex_TexelSize.xy * half2(0.0, 1.0) * _Parameter.x); }
  half4 frag(v2f_blur8 i) : SV_Target { return ACID_FragBlur8(i); }
  ENDCG
 }
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  CGPROGRAM
  #include "../Shader/Include/ACIDBlur.cginc"
  half4 _Parameter;
  #pragma vertex vert
  #pragma fragment frag
  v2f_blur8 vert(appdata_img v) { return ACID_VertBlur8(v, _MainTex_TexelSize.xy * half2(1.0, 0.0) * _Parameter.x); }
  half4 frag(v2f_blur8 i) : SV_Target { return ACID_FragBlur8(i); }
  ENDCG
 }
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  CGPROGRAM
  #include "../Shader/Include/ACIDBlur.cginc"
  half4 _Parameter;
  #pragma vertex vert
  #pragma fragment frag
  v2f_blurSGX vert(appdata_img v) { return ACID_VertBlurSGX(v, _MainTex_TexelSize.xy * half2(0.0, 1.0) * _Parameter.x); }
  half4 frag(v2f_blurSGX i) : SV_Target { return ACID_FragBlurSGX(i); }
  ENDCG
 }
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  CGPROGRAM
  #include "../Shader/Include/ACIDBlur.cginc"
  half4 _Parameter;
  #pragma vertex vert
  #pragma fragment frag
  v2f_blurSGX vert(appdata_img v) { return ACID_VertBlurSGX(v, _MainTex_TexelSize.xy * half2(1.0, 0.0) * _Parameter.x); }
  half4 frag(v2f_blurSGX i) : SV_Target { return ACID_FragBlurSGX(i); }
  ENDCG
 }
}
Fallback Off
}

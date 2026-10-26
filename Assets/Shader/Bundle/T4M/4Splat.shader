// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/T4M/4Splat.shader.cg, not this file.
Shader "T4M/4Splat" {
Properties {
 _Splat0 ("Layer 1", 2D) = "white" { }
 _Splat1 ("Layer 2", 2D) = "white" { }
 _Splat2 ("Layer 3", 2D) = "white" { }
 _Splat3 ("Layer 4", 2D) = "white" { }
 _Control ("Control (RGBA)", 2D) = "white" { }
}
SubShader { 
 LOD 99
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="Geometry" }
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #pragma multi_compile __ LIGHTMAP_ON
  #pragma multi_compile __ SHADOWS_SCREEN
  #include "../../Include/ACIDCommon.cginc"
  #include "AutoLight.cginc"
  sampler2D _Control;
  sampler2D _Splat0;
  sampler2D _Splat1;
  sampler2D _Splat2;
  sampler2D _Splat3;
  float4 _Splat0_ST;
  float4 _Splat1_ST;
  float4 _Splat2_ST;
  float4 _Splat3_ST;
  fixed4 _EnvAmbientColor;
  struct appdata
  {
      float4 vertex : POSITION;
      float4 texcoord : TEXCOORD0;
      float4 texcoord1 : TEXCOORD1;
  };
  struct v2f
  {
      float4 pos : SV_POSITION;
      float4 uv : TEXCOORD0;      // xy: control UV, zw: fog weights
      float4 uv01 : TEXCOORD1;
      float4 uv23 : TEXCOORD2;
  #if defined(LIGHTMAP_ON)
      float2 lmuv : TEXCOORD3;
  #endif
  #if defined(SHADOWS_SCREEN)
      SHADOW_COORDS(4)
  #endif
  };
  v2f vert(appdata v)
  {
      v2f o;
      UNITY_INITIALIZE_OUTPUT(v2f, o);
      o.pos = UnityObjectToClipPos(v.vertex);
      o.uv = float4(v.texcoord.xy, ACID_FogFactors(v.vertex));
      o.uv01 = float4(TRANSFORM_TEX(v.texcoord.xy, _Splat0), TRANSFORM_TEX(v.texcoord.xy, _Splat1));
      o.uv23 = float4(TRANSFORM_TEX(v.texcoord.xy, _Splat2), TRANSFORM_TEX(v.texcoord.xy, _Splat3));
  #if defined(LIGHTMAP_ON)
      o.lmuv = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
  #endif
  #if defined(SHADOWS_SCREEN)
      TRANSFER_SHADOW(o);
  #endif
      return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
      half4 control = tex2D(_Control, i.uv.xy);
      control /= dot(control, half4(1.0, 1.0, 1.0, 1.0));
      half3 c = tex2D(_Splat0, i.uv01.xy).rgb * control.x + tex2D(_Splat1, i.uv01.zw).rgb * control.y
              + tex2D(_Splat2, i.uv23.xy).rgb * control.z + tex2D(_Splat3, i.uv23.zw).rgb * control.w;
  #if defined(SHADOWS_SCREEN)
      fixed shadow = SHADOW_ATTENUATION(i);
  #endif
  #if defined(LIGHTMAP_ON)
      half3 lm = 2.0 * UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmuv).rgb;
    #if defined(SHADOWS_SCREEN)
      c *= min(lm, lerp(_EnvAmbientColor.rgb, lm, shadow));
    #else
      c *= lm;
    #endif
  #elif defined(SHADOWS_SCREEN)
      c *= _EnvAmbientColor.rgb * (1.0 - shadow) + shadow;
  #endif
      return fixed4(ACID_ApplyFog(c, i.uv.zw), 1.0);
  }
  ENDCG
 }
}
Fallback "Diffuse"
}

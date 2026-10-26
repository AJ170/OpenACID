// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Effects/Additive (Eagle Vision).shader.cg, not this file.
Shader "Effects/Additive (Eagle Vision)" {
Properties {
 _MainTex ("Particle Texture", 2D) = "white" { }
 _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,1)
[HideInInspector]  _RimColor ("RimColor", Color) = (0,0,0,0)
}
SubShader { 
 Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" "EagleVision"="particles" }
 Pass {
  Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" "EagleVision"="particles" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha One
  ColorMask RGB
  CGPROGRAM
  #pragma vertex vertFX
  #pragma fragment fragFX
  #pragma multi_compile FOG_OFF FOG_ON
  #pragma multi_compile TINT_COLOR_OFF TINT_COLOR_ON
  #pragma multi_compile VERTEX_COLOR_OFF VERTEX_COLOR_ON
  #define ACID_FX_FOG_TO_BLACK 1
  #include "../../Include/ACIDEffects.cginc"
  ENDCG
 }
 Pass {
  Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" "EagleVision"="particles" }
  ZWrite Off
  Cull Off
  ColorMask A
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #pragma multi_compile EV_OFF EV_FRIENDLY EV_ENEMY EV_MISSION EV_SELECTED
  #include "UnityCG.cginc"
  sampler2D _MainTex;
  float4 _MainTex_ST;
  struct v2f
  {
      float4 pos : SV_POSITION;
      float2 uv : TEXCOORD0;
  };
  v2f vert(appdata_base v)
  {
      v2f o;
      o.pos = UnityObjectToClipPos(v.vertex);
      o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
      return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
      fixed4 c = tex2D(_MainTex, i.uv);
      clip(c.a - 0.05);
  #if defined(EV_FRIENDLY)
      c.a = 0.09375;
  #elif defined(EV_ENEMY)
      c.a = 0.34375;
  #elif defined(EV_MISSION)
      c.a = 0.59375;
  #elif defined(EV_SELECTED)
      c.a = 0.96875;
  #else
      c.a = 0.0;
  #endif
      return c;
  }
  ENDCG
 }
}
}

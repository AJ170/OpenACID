// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/ShadowOpaque.shader.cg, not this file.
Shader "Hidden/Shadow/Opaque" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _SpecColor ("Spec Color", Color) = (1,1,1,1)
 _Emission ("Emissive Color", Color) = (0,0,0,0)
 _Shininess ("Shininess", Range(0.01,1)) = 0.7
 _MainTex ("Base (RGB)", 2D) = "white" { }
}
SubShader { 
 Pass {
  Name "SHADOWCASTER"
  Tags { "LightMode"="ShadowCaster" }
  Cull Off
  Offset 1, 1
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #pragma multi_compile_shadowcaster
  #include "UnityCG.cginc"
  struct v2f { V2F_SHADOW_CASTER; };
  v2f vert(appdata_base v)
  {
      v2f o;
      TRANSFER_SHADOW_CASTER(o)
      return o;
  }
  float4 frag(v2f i) : SV_Target
  {
      SHADOW_CASTER_FRAGMENT(i)
  }
  ENDCG
 }
}
}

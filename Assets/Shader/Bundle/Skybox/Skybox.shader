// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Skybox/Skybox.shader.cg, not this file.
Shader "Skybox/Skybox" {
Properties {
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
[HideInInspector]  _Fog ("Fog (Set by code)", Color) = (0,0,0,0)
}
SubShader { 
 LOD 99
 Tags { "LightMode"="Always" "Queue"="AlphaTest+1" "RenderType"="Background" }
 Pass {
  Tags { "LightMode"="Always" "Queue"="AlphaTest+1" "RenderType"="Background" }
  ZWrite Off
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #pragma multi_compile FOG_OFF FOG_ON
  #include "UnityCG.cginc"
  sampler2D _MainTex;
  half4 _Fog;
  struct v2f
  {
      float4 pos : SV_POSITION;
      float2 uv : TEXCOORD0;
  };
  v2f vert(appdata_base v)
  {
      v2f o;
      o.pos = UnityObjectToClipPos(v.vertex);
      o.uv = v.texcoord.xy;
      return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
      fixed4 c = tex2D(_MainTex, i.uv);
  #if defined(FOG_ON)
      c.rgb = lerp(c.rgb, _Fog.rgb, _Fog.a);
  #endif
      return c;
  }
  ENDCG
 }
}
Fallback Off
}

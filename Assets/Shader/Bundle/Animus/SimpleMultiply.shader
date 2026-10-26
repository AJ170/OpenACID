// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Animus/SimpleMultiply.shader.cg, not this file.
Shader "Animus/SimpleMultiply" {
Properties {
 _Darken ("Dark Amount", Float) = 0.5
}
SubShader { 
 Tags { "Queue"="Geometry" }
 Pass {
  Tags { "Queue"="Geometry" }
  Blend DstColor Zero
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc"
  half _Darken;
  float4 vert(float4 vertex : POSITION) : SV_POSITION
  {
      return UnityObjectToClipPos(vertex);
  }
  fixed4 frag() : SV_Target
  {
      // Blend DstColor Zero: darkens what is behind by _Darken.
      return fixed4(_Darken, _Darken, _Darken, 1.0);
  }
  ENDCG
 }
}
}

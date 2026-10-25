// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/Unlit - Depth.shader.cg, not this file.
Shader "Unlit/Depth" {
SubShader { 
 Tags { "Queue"="Geometry+1" "RenderType"="Opaque" }
 Pass {
  Tags { "Queue"="Geometry+1" "RenderType"="Opaque" }
  ColorMask 0
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc"
  struct v2f { float4 pos : SV_POSITION; fixed4 color : COLOR; };
  v2f vert(appdata_full v)
  {
      v2f o;
      o.pos = UnityObjectToClipPos(v.vertex);
      o.color = saturate(v.color);
      return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
      return i.color;
  }
  ENDCG
 }
}
}

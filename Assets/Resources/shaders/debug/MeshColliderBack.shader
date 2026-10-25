// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/debug/MeshColliderBack.shader.cg, not this file.
Shader "Hidden/Debug/MeshCollider (Back)" {
SubShader { 
 Tags { "LightMode"="Always" "Queue"="Transparent+3" "RenderType"="Transparent" }
 Pass {
  Tags { "LightMode"="Always" "Queue"="Transparent+3" "RenderType"="Transparent" }
  ZWrite Off
  Stencil {
   Ref 255
   Comp NotEqual
  }
  Blend SrcAlpha OneMinusSrcAlpha
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc"
  float4 vert(float4 vertex : POSITION) : SV_POSITION { return UnityObjectToClipPos(vertex); }
  fixed4 frag() : SV_Target { return fixed4(1.0, 0.0, 0.0, 0.3); }
  ENDCG
 }
}
Fallback "Mobile/VertexLit"
}

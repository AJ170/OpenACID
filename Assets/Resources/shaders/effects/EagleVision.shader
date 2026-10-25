// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/effects/EagleVision.shader.cg, not this file.
Shader "Hidden/Effects/Special/EagleVision" {
Properties {
 _RimColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
 _MainTex ("Particle Texture", 2D) = "white" { }
}
SubShader { 
 Tags { "LightMode"="ForwardBase" "Queue"="Geometry" "IgnoreProjector"="true" "RenderType"="Opaque" "EagleVision"="humanoid" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="Geometry" "IgnoreProjector"="true" "RenderType"="Opaque" "EagleVision"="humanoid" }
  ZWrite Off
  Cull Off
  Blend OneMinusDstAlpha DstAlpha, One Zero
  CGPROGRAM
  // Replacement shader: SubShaders are picked by the "EagleVision" tag.
  #include "UnityCG.cginc"
  sampler2D _MainTex;
  float4 _MainTex_ST;
  float4 _RimColor;
  float _EagleVisionAmount;
  #pragma vertex vert
  #pragma fragment frag
  float4 vert(float4 vertex : POSITION) : SV_POSITION { return UnityObjectToClipPos(vertex); }
  fixed4 frag() : SV_Target { return fixed4(_RimColor.rgb * _EagleVisionAmount, 0.95); }
  ENDCG
 }
}
SubShader { 
 Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" "EagleVision"="particles" }
 Pass {
  Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" "EagleVision"="particles" }
  ZWrite Off
  Cull Off
  Blend OneMinusDstAlpha DstAlpha
  CGPROGRAM
  // Replacement shader: SubShaders are picked by the "EagleVision" tag.
  #include "UnityCG.cginc"
  sampler2D _MainTex;
  float4 _MainTex_ST;
  float4 _RimColor;
  float _EagleVisionAmount;
  #pragma vertex vert
  #pragma fragment frag
  struct v2f { float4 pos : SV_POSITION; fixed4 color : COLOR; float2 uv : TEXCOORD0; };
  v2f vert(appdata_full v)
  {
      v2f o;
      o.pos = UnityObjectToClipPos(v.vertex);
      o.color = v.color;
      o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
      return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
      return fixed4(((2.0 * i.color) * (_EagleVisionAmount * tex2D(_MainTex, i.uv))).rgb, 1.0);
  }
  ENDCG
 }
}
SubShader { 
 Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" "EagleVision"="asda" }
 Pass {
  Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" "EagleVision"="asda" }
  ZWrite Off
  Cull Off
  Blend One One
  CGPROGRAM
  // Replacement shader: SubShaders are picked by the "EagleVision" tag.
  #include "UnityCG.cginc"
  sampler2D _MainTex;
  float4 _MainTex_ST;
  float4 _RimColor;
  float _EagleVisionAmount;
  #pragma vertex vert
  #pragma fragment frag
  float _RedShiftX;
  float _RedShiftY;
  float _GreenShiftX;
  float _GreenShiftY;
  float _BlueShiftX;
  float _BlueShiftY;
  struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };
  v2f vert(appdata_full v)
  {
      v2f o;
      o.pos = UnityObjectToClipPos(v.vertex);
      o.uv = v.texcoord.xy;
      return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
      return fixed4(tex2D(_MainTex, i.uv + float2(_RedShiftX, _RedShiftY)).r,
                    tex2D(_MainTex, i.uv + float2(_GreenShiftX, _GreenShiftY)).g,
                    tex2D(_MainTex, i.uv + float2(_BlueShiftX, _BlueShiftY)).b, 0.0);
  }
  ENDCG
 }
}
}

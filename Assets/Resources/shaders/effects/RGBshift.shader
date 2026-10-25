// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/effects/RGBshift.shader.cg, not this file.
Shader "Effects/Special/RGBshift" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _RedShiftX ("Red Shift X", Float) = 0
 _RedShiftY ("Red Shift Y", Float) = 0
 _GreenShiftX ("Green Shift X", Float) = 0
 _GreenShiftY ("Green Shift Y", Float) = 0
 _BlueShiftX ("Blue Shift X", Float) = 0
 _BlueShiftY ("Blue Shift Y", Float) = 0
}
SubShader { 
 Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" "EagleVision"="rgbshift" }
 Pass {
  Tags { "Queue"="Transparent" "IgnoreProjector"="true" "RenderType"="Transparent" "EagleVision"="rgbshift" }
  Cull Off
  Blend One One
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc"
  sampler2D _MainTex;
  float _RedShiftX;
  float _RedShiftY;
  float _GreenShiftX;
  float _GreenShiftY;
  float _BlueShiftX;
  float _BlueShiftY;
  struct v2f { float4 pos : SV_POSITION; float2 uvR : TEXCOORD0; float2 uvG : TEXCOORD1; float2 uvB : TEXCOORD2; };
  v2f vert(appdata_full v)
  {
      v2f o;
      o.pos = UnityObjectToClipPos(v.vertex);
      o.uvR = v.texcoord.xy + float2(_RedShiftX, _RedShiftY);
      o.uvG = v.texcoord.xy + float2(_GreenShiftX, _GreenShiftY);
      o.uvB = v.texcoord.xy + float2(_BlueShiftX, _BlueShiftY);
      return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
      return fixed4(tex2D(_MainTex, i.uvR).r * 1.01, tex2D(_MainTex, i.uvG).g, tex2D(_MainTex, i.uvB).b * 0.99, 1.0);
  }
  ENDCG
 }
}
}

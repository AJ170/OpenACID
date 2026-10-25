// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/shaders/PostFxComboPass.shader.cg, not this file.
Shader "Hidden/PostFxComboPass" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
}
SubShader { 
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  CGPROGRAM
  // Single full screen pass: bloom add (before or after grading), LUT colour
  // correction, eagle vision aware desaturation and the thunderstorm curve.
  #pragma vertex vert
  #pragma fragment frag
  #pragma multi_compile BLOOM_OFF BLOOM_FIRST BLOOM_LAST
  #pragma multi_compile COLORCORRECT_OFF COLORCORRECT_ON COLORCORRECT_FADING_ON
  #pragma multi_compile DESATURATE_OFF DESATURATE_ON
  #pragma multi_compile THUNDER_OFF THUNDER_ON
  #include "UnityCG.cginc"
  sampler2D _MainTex;
  sampler2D _BloomTex;
  sampler2D _LutTex;
  half4 _DesaturationWeightsAndMix;
  half4 _ThunderstormPreAdd;
  half4 _ThunderstormExp;
  half _ColorCorrectionIntensity;
  struct v2f { float4 pos : SV_POSITION; half2 uv : TEXCOORD0; };
  v2f vert(appdata_img v)
  {
      v2f o;
      o.pos = UnityObjectToClipPos(v.vertex);
      o.uv = v.texcoord.xy;
      return o;
  }
  half4 frag(v2f i) : SV_Target
  {
      half4 c = tex2D(_MainTex, i.uv);
  #if defined(BLOOM_FIRST)
      c.rgb += tex2D(_BloomTex, i.uv).rgb;
  #endif
  #if defined(COLORCORRECT_ON) || defined(COLORCORRECT_FADING_ON)
      // 2D LUT: red/green from (r,g), blue from (a,b).
      half3 corrected = half3(tex2D(_LutTex, c.rg).rg, tex2D(_LutTex, c.ab).a);
    #if defined(COLORCORRECT_FADING_ON)
      c.rgb = lerp(c.rgb, corrected, _ColorCorrectionIntensity);
    #else
      c.rgb = corrected;
    #endif
  #endif
  #if defined(DESATURATE_ON)
      half lum = dot(_DesaturationWeightsAndMix.rgb, c.rgb);
      half weight = c.a > 0.95 ? 1.0 : 0.0;   // keep eagle vision targets coloured
      c.rgb = lerp(c.rgb, half3(lum, lum, lum), _DesaturationWeightsAndMix.a * weight);
  #endif
  #if defined(BLOOM_LAST)
      c.rgb += tex2D(_BloomTex, i.uv).rgb;
  #endif
  #if defined(THUNDER_ON)
      c.rgb = pow(c.rgb + _ThunderstormPreAdd.rgb, _ThunderstormExp.rgb);
  #endif
      return half4(c.rgb, 1.0);
  }
  ENDCG
 }
}
Fallback Off
}

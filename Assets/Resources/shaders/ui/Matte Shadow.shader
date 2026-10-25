// Rebuilt from the Unity 5.2.3 compiled surface shader (the well known "Matte Shadow"
// shadow-only material): albedo lerp(1, _Color, _ShadowInt), alpha test at _Cutoff,
// lit areas stay white, shadowed areas tint the destination through Blend Zero SrcColor.
Shader "Custom/Matte Shadow" {
Properties {
 _Color ("Shadow Color", Color) = (1,1,1,1)
 _ShadowInt ("Shadow Intensity", Range(0,1)) = 1
 _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}
SubShader {
 Tags { "Queue"="AlphaTest" "IgnoreProjector"="true" "RenderType"="TransparentCutout" }
 LOD 200
 ZWrite Off
 Blend Zero SrcColor
 ColorMask RGB
 CGPROGRAM
 #pragma surface surf ShadowOnly alphatest:_Cutoff nolightmap nodynlightmap nodirlightmap nometa
 fixed4 _Color;
 float _ShadowInt;
 struct Input { float2 uv_MainTex; };
 void surf(Input IN, inout SurfaceOutput o)
 {
     o.Albedo = lerp(float3(1.0, 1.0, 1.0), _Color.rgb, _ShadowInt);
     o.Alpha = 1.0;
 }
 inline fixed4 LightingShadowOnly(SurfaceOutput s, fixed3 lightDir, fixed atten)
 {
     fixed4 c;
     c.rgb = lerp(s.Albedo, float3(1.0, 1.0, 1.0), atten);
     c.a = 1.0 - atten;
     return c;
 }
 ENDCG
}
Fallback "Transparent/Cutout/VertexLit"
}

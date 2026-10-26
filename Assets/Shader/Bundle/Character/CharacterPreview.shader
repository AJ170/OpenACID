// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Character/CharacterPreview.shader.cg, not this file.
Shader "Character/CharacterPreview" {
Properties {
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
 _BumpMap ("Normalmap", 2D) = "bump" { }
 _MatGlossiness ("Glossiness", Float) = 1
}
SubShader { 
 LOD 99
 Tags { "LightMode"="ForwardBase" "Queue"="Geometry" "RenderType"="Opaque" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="Geometry" "RenderType"="Opaque" }
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "../../Include/ACIDCommon.cginc"
  sampler2D _MainTex;
  sampler2D _BumpMap;
  half4 _MainTex_ST;
  half4 _BumpMap_ST;
  fixed4 _CharacterShadowColor;
  struct v2f
  {
      float4 pos : SV_POSITION;
      half2 uvMain : TEXCOORD0;
      half2 uvBump : TEXCOORD1;
      half2 fog : TEXCOORD2;
      half3 tangent : TEXCOORD3;
      half3 binormal : TEXCOORD4;
      half3 normal : TEXCOORD5;
  };
  // The assassin model in the Animus menus: bumped SH ambient with the game's light direction,
  // shadow colour, AO from albedo alpha, desaturation and fog (same globals as the character shaders).
  v2f vert(appdata_full v)
  {
      v2f o;
      o.pos = UnityObjectToClipPos(v.vertex);
      o.uvMain = TRANSFORM_TEX(v.texcoord, _MainTex);
      o.uvBump = TRANSFORM_TEX(v.texcoord, _BumpMap);
      o.normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
      o.tangent = normalize(mul((float3x3)unity_ObjectToWorld, v.tangent.xyz));
      o.binormal = normalize(cross(o.normal, o.tangent) * v.tangent.w);
      o.fog = ACID_FogFactors(v.vertex);
      return o;
  }
  fixed4 frag(v2f i) : SV_Target
  {
      half4 albedo = tex2D(_MainTex, i.uvMain);
      half ao = 1.0 - (1.0 - albedo.a) * _LightParameters.z;
      half3 tn = tex2D(_BumpMap, i.uvBump).xyz * 2.0 - 1.0;
      half3 n = i.tangent * tn.x + i.binormal * tn.y + i.normal * tn.z;
      half lit = max(dot(n, -_LightDir.xyz), 0.0) * _LightDir.w + (1.0 - _LightDir.w);
      half3 c = ACID_SHAmbient(n, _LightParameters.x) * albedo.rgb * ao * lerp(_CharacterShadowColor.rgb, half3(lit, lit, lit), min(1.0, lit * 1.5));
      c = ACID_Desaturate(c, lit * _LightParameters.y);
      return fixed4(ACID_ApplyFog(c, i.fog), 1.0);
  }
  ENDCG
 }
}
Fallback "Hidden/Shadow/Opaque"
}

// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Shader/Bundle/Animus/MatCap_AnimusFloor.shader.cg, not this file.
Shader "Animus/MatCap_AnimusFloor" {
Properties {
 _Color ("Main Color", Color) = (0.5,0.5,0.5,1)
 _ShadowColor ("Shadow Color", Color) = (0,0,0,0.5)
 _BumpMap ("Bumpmap (RGB)", 2D) = "bump" { }
 _MatCap ("MatCap (RGB)", 2D) = "white" { }
 _Amplitude ("Amplitude", Float) = 0.05
 _ReflIntensity ("Reflection Intensity", Float) = 0.25
 _EffectCube ("Reflection Map", CUBE) = "" { }
}
SubShader { 
 Tags { "LightMode"="ForwardBase" "Queue"="Background+1" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="Background+1" }
  Lighting On
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #pragma multi_compile __ SHADOWS_SCREEN
  #include "UnityCG.cginc"
  #include "AutoLight.cginc"
  half4 _Color;
  half4 _ShadowColor;
  sampler2D _BumpMap;
  sampler2D _MatCap;
  float4 _MatCap_ST;
  samplerCUBE _EffectCube;
  float _Amplitude;
  float _ReflIntensity;
  struct v2f
  {
      float4 pos : SV_POSITION;
      float2 uv : TEXCOORD0;
      float2 fade : TEXCOORD1;
      float3 matcapX : TEXCOORD2;
      float3 matcapY : TEXCOORD3;
      float3 normal : TEXCOORD4;
      float3 viewVec : TEXCOORD5;
  #if defined(SHADOWS_SCREEN)
      SHADOW_COORDS(6)
  #endif
  };
  // Animus floor tiles: they bob (vertex colour g = amplitude, r = phase), blend from a flat colour up
  // close (fade by distance + texcoord1.y) to a matcap with a cube reflection further out.
  v2f vert(appdata_full v)
  {
      v2f o;
      float4 wpos = mul(unity_ObjectToWorld, v.vertex);
      float deg = wpos.z * wpos.x * 0.0075 + _Time.z * 0.33;
      float amp = _Amplitude * v.color.g;
      float wave = sin(deg + v.color.r * 12.0);
      float4 bobbed = wpos;
      bobbed.y = wpos.y - (amp + wave * amp);
      o.fade = float2(saturate(distance(_WorldSpaceCameraPos, bobbed.xyz) * 0.15 - 1.0), v.texcoord1.y);
      o.pos = UnityObjectToClipPos(mul(unity_WorldToObject, bobbed));
      float3 binormal = cross(v.normal, v.tangent.xyz) * v.tangent.w;
      float3x3 rotation = float3x3(v.tangent.xyz, binormal, v.normal);
      o.matcapX = mul(rotation, UNITY_MATRIX_MV[0].xyz);
      o.matcapY = mul(rotation, UNITY_MATRIX_MV[1].xyz);
      o.normal = normalize(mul(v.normal, (float3x3)unity_WorldToObject)) + wave * 0.0125 * v.color.g;
      o.viewVec = wpos.xyz - _WorldSpaceCameraPos;
      o.uv = v.texcoord.xy;
  #if defined(SHADOWS_SCREEN)
      TRANSFER_SHADOW(o);
  #endif
      return o;
  }
  half4 frag(v2f i) : SV_Target
  {
      // raw *2-1 unpack like the original (the normal map is imported as a plain texture)
      half3 n = tex2D(_BumpMap, i.uv).xyz * 2.0 - 1.0;
      half2 vn = half2(dot(i.matcapX, n), dot(i.matcapY, n));
      float3 reflected = reflect(i.viewVec, normalize(i.normal));
      half4 matcap = tex2D(_MatCap, vn * 0.5 + 0.5 + _MatCap_ST.zw);
      half baseFade = saturate(i.fade.x + i.fade.y);
      half4 c = _Color * baseFade + matcap * _Color * 2.0 * (1.0 - baseFade);
      c += texCUBE(_EffectCube, reflected) * _ReflIntensity * (1.0 - baseFade);
  #if defined(SHADOWS_SCREEN)
      c.rgb = lerp(c.rgb, _ShadowColor.rgb, (1.0 - SHADOW_ATTENUATION(i)) * _ShadowColor.a);
  #endif
      return c;
  }
  ENDCG
 }
}
Fallback "Hidden/Shadow/Opaque"
}

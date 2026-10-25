// Ported from the Unity 5.2.3 compiled shader by Tools/ShaderPort/port_shaders.py.
// Edit Tools/ShaderPort/specs/Resources/materials/water/WaterFlowFoamTileHidingFakeLight_Metal.shader.cg, not this file.
Shader "Water/FlowFoamTileHidingFakeLight_Metal" {
Properties {
 _DistFoamTex ("DistortionFoam", 2D) = "white" { }
 _GradientTex ("Gradient", 2D) = "white" { }
 _DistortParams ("Distortions (World Normal, Reflection, Specular, Unused)", Vector) = (1,1,2,1.15)
 _BumpTiling ("Bump Tiling (Tiling x, Tiling z, Blend factor x, Blend factor z", Vector) = (1,1,0.12,0.12)
 _Shininess ("Shininess", Range(0,1000)) = 400
 _CrestFoamParameters ("Foam Parameters (Crest foam intensity, Shore foam intensity, Unused, Unused)", Vector) = (2,1,1,0)
 _OverlayFoamIntensity ("Overlay-Foam Intensity", Range(0,1)) = 1
 _GerstnerIntensity ("Per vertex displacement", Float) = 1
 _GAmplitude ("Wave Amplitude", Vector) = (0.3,0.35,0.25,0.25)
 _GFrequency ("Wave Frequency", Vector) = (1.3,1.35,1.25,1.25)
 _GSteepness ("Wave Steepness", Vector) = (1,1,1,1)
 _GSpeed ("Wave Speed", Vector) = (1.2,1.375,1.1,1.5)
 _GDirectionAB ("Wave Direction", Vector) = (0.3,0.85,0.85,0.25)
 _GDirectionCD ("Wave Direction", Vector) = (0.1,0.9,0.5,0.5)
 _DebugReflectionPower ("Reflection Power (DEBUG)", Range(0,1)) = 0
 _DebugReflectionDamping ("Reflection Vector Damping", Range(0,1)) = 0
 _DebugReflectionIntensity ("Reflection Intensity", Float) = 1
}
SubShader { 
 Tags { "LightMode"="ForwardBase" "Queue"="Geometry+2" "IgnoreProjector"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "LightMode"="ForwardBase" "Queue"="Geometry+2" "IgnoreProjector"="true" "RenderType"="Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  CGPROGRAM
  // Gerstner wave water with flow-mapped distortion/foam, planar reflection, fake
  // specular light and the game's fog. Interpolators are packed (9 -> 6) for Vita.
  #pragma vertex vert
  #pragma fragment frag
  #include "../../../Shader/Include/ACIDCommon.cginc"
  sampler2D _DistFoamTex;
  sampler2D _GradientTex;
  sampler2D _ReflectionTex;
  fixed4 _ReflColor;
  half4 _DistortParams;
  half _Shininess;
  fixed4 _SpecularColor;
  half _DebugReflectionPower;
  half _DebugReflectionDamping;
  half _DebugReflectionIntensity;
  fixed _OverlayFoamIntensity;
  half4 _fakeLightDir;
  float4 _flowPhasesLerp;
  half2 _flowSpeed;
  half _GerstnerIntensity;
  half4 _BumpTiling;
  half4 _GAmplitude;
  half4 _GFrequency;
  half4 _GSteepness;
  half4 _GSpeed;
  half4 _GDirectionAB;
  half4 _GDirectionCD;
  half4 _CrestFoamParameters;
  
  struct v2f
  {
      float4 pos : SV_POSITION;
      float4 uvFlow : TEXCOORD0;      // xy: flow phase A, zw: flow phase B
      float4 uvFlowDetail : TEXCOORD1;
      fixed4 fogFoam : TEXCOORD2;     // xy: fog weights, z: crest/shore foam
      half4 normalAlpha : TEXCOORD3;  // xyz: wave normal, w: vertex alpha
      half4 viewGradient : TEXCOORD4; // xyz: view vector, w: gradient u (vertex green)
      half3 screen : TEXCOORD5;
  };
  
  v2f vert(appdata_full v)
  {
      v2f o;
      float3 worldVertex0 = mul(unity_ObjectToWorld, v.vertex).xyz;
      half waterBaseHeight = worldVertex0.y;
      float2 xz = worldVertex0.xz;
  
      // Gerstner displacement (4 waves)
      half4 AB = _GSteepness.xxyy * _GAmplitude.xxyy * _GDirectionAB;
      half4 CD = _GSteepness.zzww * _GAmplitude.zzww * _GDirectionCD;
      half4 dotABCD = _GFrequency * half4(dot(_GDirectionAB.xy, xz), dot(_GDirectionAB.zw, xz),
                                          dot(_GDirectionCD.xy, xz), dot(_GDirectionCD.zw, xz));
      half4 TIME = _Time.yyyy * _GSpeed;
      half4 COS = cos(dotABCD + TIME);
      float3 offsets;
      offsets.x = dot(COS, half4(AB.xz, CD.xz));
      offsets.z = dot(COS, half4(AB.yw, CD.yw));
      offsets.y = dot(sin(dotABCD + TIME), _GAmplitude);
  
      // Gerstner normal at the displaced position
      float2 xzVtx = xz + offsets.xz;
      half4 AB2 = _GFrequency.xxyy * _GAmplitude.xxyy * _GDirectionAB;
      half4 CD2 = _GFrequency.zzww * _GAmplitude.zzww * _GDirectionCD;
      half4 dot2 = _GFrequency * half4(dot(_GDirectionAB.xy, xzVtx), dot(_GDirectionAB.zw, xzVtx),
                                       dot(_GDirectionCD.xy, xzVtx), dot(_GDirectionCD.zw, xzVtx));
      half4 COS2 = cos(dot2 + TIME);
      half3 nrml = half3(-dot(COS2, half4(AB2.xz, CD2.xz)), 2.0, -dot(COS2, half4(AB2.yw, CD2.yw)));
      nrml.xz *= _GerstnerIntensity;
      nrml = normalize(nrml);
  
      float4 vtx = float4(v.vertex.xyz + offsets, v.vertex.w);
      float3 worldVertex = mul(unity_ObjectToWorld, vtx).xyz;
      float3 viewVector = _WorldSpaceCameraPos - worldVertex;
      float viewDirLen = length(viewVector);
  
      o.pos = UnityObjectToClipPos(vtx);
      o.screen = ComputeScreenPos(o.pos).xyw;
      o.normalAlpha = half4(nrml, v.color.a);
      o.viewGradient = half4(viewVector / viewDirLen, v.color.g);
  
      half crest = saturate((worldVertex.y - waterBaseHeight) / (_CrestFoamParameters.x - waterBaseHeight));
      half foamShore = 1.0 - v.color.a * v.color.a;
      half f = (1.0 - saturate((acid_Fog.y - viewDirLen) / (acid_Fog.y - acid_Fog.x))) * acid_Fog.z;
      o.fogFoam = fixed4(1.0 - f, f, max(crest * _CrestFoamParameters.y, foamShore * _CrestFoamParameters.z), 0.0);
  
      float2 uv = worldVertex.xz * _BumpTiling.xy;
      float2 flow = (v.color.xz * 2.0 - 1.0) * _flowSpeed;
      o.uvFlow = float4(uv + flow * _flowPhasesLerp.x, uv + flow * _flowPhasesLerp.y);
      o.uvFlowDetail = o.uvFlow * _BumpTiling.zwzw;
      return o;
  }
  
  fixed4 frag(v2f i) : SV_Target
  {
      half3 bump0 = tex2D(_DistFoamTex, i.uvFlow.xy).rgb + tex2D(_DistFoamTex, i.uvFlowDetail.xy).rgb;
      half3 bump1 = tex2D(_DistFoamTex, i.uvFlow.zw).rgb + tex2D(_DistFoamTex, i.uvFlowDetail.zw).rgb;
      half3 combined = lerp(bump0, bump1, _flowPhasesLerp.z);
      combined.xy = (combined.xy - 1.0) * _DistortParams.z;
  
      half3 specNormal = normalize(half3(i.normalAlpha.x + combined.x, i.normalAlpha.y, i.normalAlpha.z + combined.y));
      fixed4 gradient = tex2D(_GradientTex, half2(i.viewGradient.w, dot(specNormal, i.viewGradient.xyz)));
      half3 refl = _DebugReflectionIntensity * tex2D(_ReflectionTex, (i.screen.xy + specNormal.xz * _DebugReflectionDamping) / i.screen.z).rgb;
      half spec = pow(abs(dot(specNormal, normalize(i.viewGradient.xyz + _fakeLightDir.xyz))), _Shininess);
  
      half foam = combined.z;
      half foamAdd = pow(foam, 5.0) * _OverlayFoamIntensity + foam * i.fogFoam.z;
      fixed3 c = gradient.rgb + (foamAdd + (1.0 - foamAdd) * (refl * _ReflColor.rgb * gradient.a + spec * _SpecularColor.rgb));
      c = lerp(c, refl * _ReflColor.rgb, _DebugReflectionPower);
      return fixed4(ACID_ApplyFog(c, i.fogFoam.xy), i.normalAlpha.w);
  }
  ENDCG
 }
}
Fallback "Mobile/VertexLit"
}

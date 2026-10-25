Shader "Character/CharacterBody4Col" {
Properties {
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
 _BumpMap ("Normalmap", 2D) = "bump" { }
 _DetailA ("DetailA", 2D) = "bump" { }
 _DetailB ("DetailB", 2D) = "bump" { }
 _DetailScale ("DetailScale", Vector) = (2,2,3,3)
 _Color0 ("ColorMask White + Detail B", Color) = (1,1,1,1)
 _Color1 ("ColorMask Grey 150", Color) = (1,1,1,1)
 _Color2 ("ColorMask Grey 80", Color) = (1,1,1,1)
 _Color3 ("ColorMask Black + Detail A", Color) = (1,1,1,1)
 _MatGlossiness ("Glossiness", Float) = 1
 _NoiseGlow ("Noise glow color and layer", Color) = (1,1,1,0)
 _RimColor ("RimColor, will be set from code", Color) = (1,0,1,1)
}
SubShader { 
 LOD 99
 Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry" "RenderType"="Opaque" "EagleVision"="humanoid" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry" "RenderType"="Opaque" "EagleVision"="humanoid" }
  GpuProgramID 52005
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _DetailA;
uniform sampler2D _DetailB;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _Color2;
uniform lowp vec4 _Color3;
uniform highp vec4 _DetailScale;
mediump vec3 tmpvar_1;
mediump vec2 tmpvar_2;
mediump float tmpvar_3;
mediump vec3 tmpvar_4;
mediump vec3 tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  tmpvar_2 = xlv_TEXCOORD0.xy;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  highp float _selector_10;
  _selector_10 = tmpvar_9.z;
  highp float ref_11;
  highp vec4 result_12;
  result_12.x = float((_selector_10 >= 0.75));
  ref_11 = (_selector_10 - (result_12.x * 2.0));
  result_12.y = float((ref_11 >= 0.5));
  ref_11 = (ref_11 - (result_12.y * 2.0));
  result_12.z = float((ref_11 >= 0.25));
  ref_11 = (ref_11 - (result_12.z * 2.0));
  result_12.w = float((ref_11 >= 0.0));
  highp vec2 _uv_13;
  _uv_13 = tmpvar_2;
  highp vec2 result_14;
  highp vec4 d1_15;
  highp vec4 d0_16;
  highp vec4 tmpvar_17;
  tmpvar_17 = (_DetailScale * _uv_13.xyxy);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_DetailA, tmpvar_17.xy);
  d0_16 = tmpvar_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_DetailB, tmpvar_17.zw);
  d1_15 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20.zw = vec2(0.0, 0.0);
  tmpvar_20.x = d0_16.x;
  tmpvar_20.y = d1_15.x;
  result_14.x = dot ((tmpvar_20 - vec4(0.5, 0.5, 0.0, 0.0)), result_12.wxxx);
  highp vec4 tmpvar_21;
  tmpvar_21.zw = vec2(0.0, 0.0);
  tmpvar_21.x = d0_16.y;
  tmpvar_21.y = d1_15.y;
  result_14.y = dot ((tmpvar_21 - vec4(0.5, 0.5, 0.0, 0.0)), result_12.wxxx);
  highp vec4 tmpvar_22;
  highp mat4 tmpvar_23;
  lowp mat4 tmpvar_24;
  tmpvar_24[0].x = _Color0.x;
  tmpvar_24[0].y = _Color1.x;
  tmpvar_24[0].z = _Color2.x;
  tmpvar_24[0].w = _Color3.x;
  tmpvar_24[1].x = _Color0.y;
  tmpvar_24[1].y = _Color1.y;
  tmpvar_24[1].z = _Color2.y;
  tmpvar_24[1].w = _Color3.y;
  tmpvar_24[2].x = _Color0.z;
  tmpvar_24[2].y = _Color1.z;
  tmpvar_24[2].z = _Color2.z;
  tmpvar_24[2].w = _Color3.z;
  tmpvar_24[3].x = _Color0.w;
  tmpvar_24[3].y = _Color1.w;
  tmpvar_24[3].z = _Color2.w;
  tmpvar_24[3].w = _Color3.w;
  tmpvar_23 = tmpvar_24;
  tmpvar_22 = (result_12 * tmpvar_23);
  tmpvar_4 = ((tmpvar_9.xxx * tmpvar_22.xyz) + result_14.xxx);
  tmpvar_3 = (result_14.y + tmpvar_9.y);
  mediump vec3 tangentNormal_25;
  mediump vec3 tmpvar_26;
  tmpvar_26.x = xlv_TEXCOORD3.w;
  tmpvar_26.y = xlv_TEXCOORD4.w;
  tmpvar_26.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_27;
  mediump vec3 tmpvar_28;
  tmpvar_27 = xlv_TEXCOORD5.xyz;
  tmpvar_28 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_29;
  tmpvar_29[0].x = tmpvar_27.x;
  tmpvar_29[0].y = tmpvar_26.x;
  tmpvar_29[0].z = tmpvar_28.x;
  tmpvar_29[1].x = tmpvar_27.y;
  tmpvar_29[1].y = tmpvar_26.y;
  tmpvar_29[1].z = tmpvar_28.y;
  tmpvar_29[2].x = tmpvar_27.z;
  tmpvar_29[2].y = tmpvar_26.z;
  tmpvar_29[2].z = tmpvar_28.z;
  lowp vec3 tmpvar_30;
  tmpvar_30 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_25 = tmpvar_30;
  tmpvar_5 = (tangentNormal_25 * tmpvar_29);
  mediump vec4 tmpvar_31;
  tmpvar_31.w = 1.0;
  tmpvar_31.xyz = tmpvar_5;
  mediump vec3 x2_32;
  mediump vec3 x1_33;
  x1_33.x = dot (unity_SHAr, tmpvar_31);
  x1_33.y = dot (unity_SHAg, tmpvar_31);
  x1_33.z = dot (unity_SHAb, tmpvar_31);
  mediump vec4 tmpvar_34;
  tmpvar_34 = (tmpvar_5.xyzz * tmpvar_5.yzzx);
  x2_32.x = dot (unity_SHBr, tmpvar_34);
  x2_32.y = dot (unity_SHBg, tmpvar_34);
  x2_32.z = dot (unity_SHBb, tmpvar_34);
  mediump vec3 tmpvar_35;
  tmpvar_35 = max (((1.055 * 
    pow ((((x2_32 + 
      (unity_SHC.xyz * ((tmpvar_5.x * tmpvar_5.x) - (tmpvar_5.y * tmpvar_5.y)))
    ) + x1_33) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = ((max (
    dot (tmpvar_5, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_35 * tmpvar_4)
   * 
    (1.0 - tmpvar_3)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_6), vec3(
    min (1.0, (tmpvar_6 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (tmpvar_5, xlv_TEXCOORD3.xyz) * tmpvar_5)
      )), -(tmpvar_5)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_35)
   * tmpvar_3) * tmpvar_4));
  mediump vec3 tmpvar_36;
  tmpvar_36 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_6 * _LightParameters.y)));
  c_7.xyz = tmpvar_36;
  c_7.w = _RimColor.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (c_7.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_OFF" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _DetailA;
uniform lowp sampler2D _DetailB;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec4 t0;
highp vec4 t1;
mediump vec4 t16_1;
lowp vec3 t10_1;
highp float t2;
mediump vec4 t16_2;
lowp vec4 t10_2;
highp vec4 t3;
highp vec3 t4;
mediump vec4 t16_5;
lowp vec3 t10_6;
mediump vec3 t16_7;
mediump vec3 t16_8;
highp float t9;
mediump vec3 t16_16;
highp float t19;
bvec2 tb19;
bool tb28;
mediump float t16_32;
void main()
{
    t0 = vs_TEXCOORD0.xyxy * _DetailScale;
    t1.xz = texture(_DetailA, t0.xy).xy;
    t1.yw = texture(_DetailB, t0.zw).xy;
    t0 = t1 + vec4(-0.5, -0.5, -0.5, -0.5);
    t10_1.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    tb28 = t10_1.z>=0.75;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.x = tb28 ? 1.0 : float(0.0);
    t19 = t10_1.z + t2;
    tb28 = t19>=0.5;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.y = tb28 ? 1.0 : float(0.0);
    t19 = t19 + t2;
    tb19.y = t19>=0.25;
    t2 = (tb19.y) ? -2.0 : -0.0;
    t19 = t19 + t2;
    tb19.x = t19>=0.0;
    t3.zw = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb19.yx));
    t0.x = dot(t0.yx, t3.xw);
    t9 = dot(t0.wz, t3.xw);
    t9 = t10_1.y + t9;
    t10_2.x = _Color0.x;
    t10_2.y = _Color1.x;
    t10_2.z = _Color2.x;
    t10_2.w = _Color3.x;
    t4.x = dot(t3, t10_2);
    t10_2.x = _Color0.y;
    t10_2.y = _Color1.y;
    t10_2.z = _Color2.y;
    t10_2.w = _Color3.y;
    t4.y = dot(t3, t10_2);
    t10_2.x = _Color0.z;
    t10_2.y = _Color1.z;
    t10_2.z = _Color2.z;
    t10_2.w = _Color3.z;
    t4.z = dot(t3, t10_2);
    t0.xzw = t10_1.xxx * t4.xyz + t0.xxx;
    t16_5.x = vs_TEXCOORD5.x;
    t16_5.y = vs_TEXCOORD3.w;
    t16_5.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_6.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = vs_TEXCOORD5.y;
    t16_5.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = t16_1.y * t16_1.y;
    t16_5.x = t16_1.x * t16_1.x + (-t16_5.x);
    t16_7.xy = vs_TEXCOORD5.zw;
    t16_7.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_6.xyz, t16_7.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_7.x = dot(unity_SHBr, t16_2);
    t16_7.y = dot(unity_SHBg, t16_2);
    t16_7.z = dot(unity_SHBb, t16_2);
    t16_5.xyz = unity_SHC.xyz * t16_5.xxx + t16_7.xyz;
    t16_1.w = 1.0;
    t16_7.x = dot(unity_SHAr, t16_1);
    t16_7.y = dot(unity_SHAg, t16_1);
    t16_7.z = dot(unity_SHAb, t16_1);
    t16_5.xyz = t16_5.xyz + t16_7.xyz;
    t16_5.xyz = t16_5.xyz * _LightParameters.xxx;
    t16_5.xyz = log2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_5.xyz = exp2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_32 = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_32 = t16_32 + t16_32;
    t16_7.xyz = t16_1.xyz * (-vec3(t16_32)) + vs_TEXCOORD3.xyz;
    t16_5.w = dot(t16_7.xyz, (-t16_1.xyz));
    t16_7.x = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_7.x = max(t16_7.x, 0.0);
    t16_5 = max(t16_5, vec4(0.0, 0.0, 0.0, 0.25));
    t16_32 = t16_5.w * _Glossiness.w;
    t16_32 = t16_32 * _MatGlossiness;
    t16_32 = t16_32 * 10.0;
    t16_16.xyz = vec3(t16_32) * _Glossiness.xyz;
    t16_16.xyz = t16_5.xyz * t16_16.xyz;
    t16_5.xyz = t0.xzw * t16_5.xyz;
    t16_16.xyz = vec3(t9) * t16_16.xyz;
    t16_32 = (-t9) + 1.0;
    t16_5.xyz = vec3(t16_32) * t16_5.xyz;
    t16_16.xyz = t0.xzw * t16_16.xyz;
    t10_6.x = (-_LightDir.w) + 1.0;
    t16_32 = t16_7.x * _LightDir.w + t10_6.x;
    t16_7.x = t16_32 * 1.5;
    t16_7.x = min(t16_7.x, 1.0);
    t16_8.xyz = vec3(t16_32) + (-_CharacterShadowColor.xyz);
    t16_32 = t16_32 * _LightParameters.y;
    t16_8.xyz = t16_7.xxx * t16_8.xyz + _CharacterShadowColor.xyz;
    t16_5.xyz = t16_5.xyz * t16_8.xyz + t16_16.xyz;
    t16_7.x = dot(t16_5.xyz, vec3(1.0, 1.0, 1.0));
    t16_7.xyz = t16_7.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_5.xyz);
    t16_5.xyz = vec3(t16_32) * t16_7.xyz + t16_5.xyz;
    t16_5.xyz = t16_5.xyz * vs_TEXCOORD2.xxx;
    t16_5.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_5.xyz;
    SV_Target0.xyz = t16_5.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _DetailA;
uniform sampler2D _DetailB;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _Color2;
uniform lowp vec4 _Color3;
uniform highp vec4 _DetailScale;
mediump vec3 tmpvar_1;
mediump vec2 tmpvar_2;
mediump float tmpvar_3;
mediump vec3 tmpvar_4;
mediump vec3 tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  tmpvar_2 = xlv_TEXCOORD0.xy;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  highp float _selector_10;
  _selector_10 = tmpvar_9.z;
  highp float ref_11;
  highp vec4 result_12;
  result_12.x = float((_selector_10 >= 0.75));
  ref_11 = (_selector_10 - (result_12.x * 2.0));
  result_12.y = float((ref_11 >= 0.5));
  ref_11 = (ref_11 - (result_12.y * 2.0));
  result_12.z = float((ref_11 >= 0.25));
  ref_11 = (ref_11 - (result_12.z * 2.0));
  result_12.w = float((ref_11 >= 0.0));
  highp vec2 _uv_13;
  _uv_13 = tmpvar_2;
  highp vec2 result_14;
  highp vec4 d1_15;
  highp vec4 d0_16;
  highp vec4 tmpvar_17;
  tmpvar_17 = (_DetailScale * _uv_13.xyxy);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_DetailA, tmpvar_17.xy);
  d0_16 = tmpvar_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_DetailB, tmpvar_17.zw);
  d1_15 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20.zw = vec2(0.0, 0.0);
  tmpvar_20.x = d0_16.x;
  tmpvar_20.y = d1_15.x;
  result_14.x = dot ((tmpvar_20 - vec4(0.5, 0.5, 0.0, 0.0)), result_12.wxxx);
  highp vec4 tmpvar_21;
  tmpvar_21.zw = vec2(0.0, 0.0);
  tmpvar_21.x = d0_16.y;
  tmpvar_21.y = d1_15.y;
  result_14.y = dot ((tmpvar_21 - vec4(0.5, 0.5, 0.0, 0.0)), result_12.wxxx);
  highp vec4 tmpvar_22;
  highp mat4 tmpvar_23;
  lowp mat4 tmpvar_24;
  tmpvar_24[0].x = _Color0.x;
  tmpvar_24[0].y = _Color1.x;
  tmpvar_24[0].z = _Color2.x;
  tmpvar_24[0].w = _Color3.x;
  tmpvar_24[1].x = _Color0.y;
  tmpvar_24[1].y = _Color1.y;
  tmpvar_24[1].z = _Color2.y;
  tmpvar_24[1].w = _Color3.y;
  tmpvar_24[2].x = _Color0.z;
  tmpvar_24[2].y = _Color1.z;
  tmpvar_24[2].z = _Color2.z;
  tmpvar_24[2].w = _Color3.z;
  tmpvar_24[3].x = _Color0.w;
  tmpvar_24[3].y = _Color1.w;
  tmpvar_24[3].z = _Color2.w;
  tmpvar_24[3].w = _Color3.w;
  tmpvar_23 = tmpvar_24;
  tmpvar_22 = (result_12 * tmpvar_23);
  tmpvar_4 = ((tmpvar_9.xxx * tmpvar_22.xyz) + result_14.xxx);
  tmpvar_3 = (result_14.y + tmpvar_9.y);
  mediump vec3 tangentNormal_25;
  mediump vec3 tmpvar_26;
  tmpvar_26.x = xlv_TEXCOORD3.w;
  tmpvar_26.y = xlv_TEXCOORD4.w;
  tmpvar_26.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_27;
  mediump vec3 tmpvar_28;
  tmpvar_27 = xlv_TEXCOORD5.xyz;
  tmpvar_28 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_29;
  tmpvar_29[0].x = tmpvar_27.x;
  tmpvar_29[0].y = tmpvar_26.x;
  tmpvar_29[0].z = tmpvar_28.x;
  tmpvar_29[1].x = tmpvar_27.y;
  tmpvar_29[1].y = tmpvar_26.y;
  tmpvar_29[1].z = tmpvar_28.y;
  tmpvar_29[2].x = tmpvar_27.z;
  tmpvar_29[2].y = tmpvar_26.z;
  tmpvar_29[2].z = tmpvar_28.z;
  lowp vec3 tmpvar_30;
  tmpvar_30 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_25 = tmpvar_30;
  tmpvar_5 = (tangentNormal_25 * tmpvar_29);
  mediump vec4 tmpvar_31;
  tmpvar_31.w = 1.0;
  tmpvar_31.xyz = tmpvar_5;
  mediump vec3 x2_32;
  mediump vec3 x1_33;
  x1_33.x = dot (unity_SHAr, tmpvar_31);
  x1_33.y = dot (unity_SHAg, tmpvar_31);
  x1_33.z = dot (unity_SHAb, tmpvar_31);
  mediump vec4 tmpvar_34;
  tmpvar_34 = (tmpvar_5.xyzz * tmpvar_5.yzzx);
  x2_32.x = dot (unity_SHBr, tmpvar_34);
  x2_32.y = dot (unity_SHBg, tmpvar_34);
  x2_32.z = dot (unity_SHBb, tmpvar_34);
  mediump vec3 tmpvar_35;
  tmpvar_35 = max (((1.055 * 
    pow ((((x2_32 + 
      (unity_SHC.xyz * ((tmpvar_5.x * tmpvar_5.x) - (tmpvar_5.y * tmpvar_5.y)))
    ) + x1_33) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = ((max (
    dot (tmpvar_5, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_35 * tmpvar_4)
   * 
    (1.0 - tmpvar_3)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_6), vec3(
    min (1.0, (tmpvar_6 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (tmpvar_5, xlv_TEXCOORD3.xyz) * tmpvar_5)
      )), -(tmpvar_5)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_35)
   * tmpvar_3) * tmpvar_4));
  mediump vec3 tmpvar_36;
  tmpvar_36 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_6 * _LightParameters.y)));
  c_7.xyz = tmpvar_36;
  c_7.w = _RimColor.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (c_7.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _DetailA;
uniform lowp sampler2D _DetailB;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec4 t0;
highp vec4 t1;
mediump vec4 t16_1;
lowp vec3 t10_1;
highp float t2;
mediump vec4 t16_2;
lowp vec4 t10_2;
highp vec4 t3;
highp vec3 t4;
mediump vec4 t16_5;
lowp vec3 t10_6;
mediump vec3 t16_7;
mediump vec3 t16_8;
highp float t9;
mediump vec3 t16_16;
highp float t19;
bvec2 tb19;
bool tb28;
mediump float t16_32;
void main()
{
    t0 = vs_TEXCOORD0.xyxy * _DetailScale;
    t1.xz = texture(_DetailA, t0.xy).xy;
    t1.yw = texture(_DetailB, t0.zw).xy;
    t0 = t1 + vec4(-0.5, -0.5, -0.5, -0.5);
    t10_1.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    tb28 = t10_1.z>=0.75;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.x = tb28 ? 1.0 : float(0.0);
    t19 = t10_1.z + t2;
    tb28 = t19>=0.5;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.y = tb28 ? 1.0 : float(0.0);
    t19 = t19 + t2;
    tb19.y = t19>=0.25;
    t2 = (tb19.y) ? -2.0 : -0.0;
    t19 = t19 + t2;
    tb19.x = t19>=0.0;
    t3.zw = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb19.yx));
    t0.x = dot(t0.yx, t3.xw);
    t9 = dot(t0.wz, t3.xw);
    t9 = t10_1.y + t9;
    t10_2.x = _Color0.x;
    t10_2.y = _Color1.x;
    t10_2.z = _Color2.x;
    t10_2.w = _Color3.x;
    t4.x = dot(t3, t10_2);
    t10_2.x = _Color0.y;
    t10_2.y = _Color1.y;
    t10_2.z = _Color2.y;
    t10_2.w = _Color3.y;
    t4.y = dot(t3, t10_2);
    t10_2.x = _Color0.z;
    t10_2.y = _Color1.z;
    t10_2.z = _Color2.z;
    t10_2.w = _Color3.z;
    t4.z = dot(t3, t10_2);
    t0.xzw = t10_1.xxx * t4.xyz + t0.xxx;
    t16_5.x = vs_TEXCOORD5.x;
    t16_5.y = vs_TEXCOORD3.w;
    t16_5.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_6.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = vs_TEXCOORD5.y;
    t16_5.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = t16_1.y * t16_1.y;
    t16_5.x = t16_1.x * t16_1.x + (-t16_5.x);
    t16_7.xy = vs_TEXCOORD5.zw;
    t16_7.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_6.xyz, t16_7.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_7.x = dot(unity_SHBr, t16_2);
    t16_7.y = dot(unity_SHBg, t16_2);
    t16_7.z = dot(unity_SHBb, t16_2);
    t16_5.xyz = unity_SHC.xyz * t16_5.xxx + t16_7.xyz;
    t16_1.w = 1.0;
    t16_7.x = dot(unity_SHAr, t16_1);
    t16_7.y = dot(unity_SHAg, t16_1);
    t16_7.z = dot(unity_SHAb, t16_1);
    t16_5.xyz = t16_5.xyz + t16_7.xyz;
    t16_5.xyz = t16_5.xyz * _LightParameters.xxx;
    t16_5.xyz = log2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_5.xyz = exp2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_32 = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_32 = t16_32 + t16_32;
    t16_7.xyz = t16_1.xyz * (-vec3(t16_32)) + vs_TEXCOORD3.xyz;
    t16_5.w = dot(t16_7.xyz, (-t16_1.xyz));
    t16_7.x = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_7.x = max(t16_7.x, 0.0);
    t16_5 = max(t16_5, vec4(0.0, 0.0, 0.0, 0.25));
    t16_32 = t16_5.w * _Glossiness.w;
    t16_32 = t16_32 * _MatGlossiness;
    t16_32 = t16_32 * 10.0;
    t16_16.xyz = vec3(t16_32) * _Glossiness.xyz;
    t16_16.xyz = t16_5.xyz * t16_16.xyz;
    t16_5.xyz = t0.xzw * t16_5.xyz;
    t16_16.xyz = vec3(t9) * t16_16.xyz;
    t16_32 = (-t9) + 1.0;
    t16_5.xyz = vec3(t16_32) * t16_5.xyz;
    t16_16.xyz = t0.xzw * t16_16.xyz;
    t10_6.x = (-_LightDir.w) + 1.0;
    t16_32 = t16_7.x * _LightDir.w + t10_6.x;
    t16_7.x = t16_32 * 1.5;
    t16_7.x = min(t16_7.x, 1.0);
    t16_8.xyz = vec3(t16_32) + (-_CharacterShadowColor.xyz);
    t16_32 = t16_32 * _LightParameters.y;
    t16_8.xyz = t16_7.xxx * t16_8.xyz + _CharacterShadowColor.xyz;
    t16_5.xyz = t16_5.xyz * t16_8.xyz + t16_16.xyz;
    t16_7.x = dot(t16_5.xyz, vec3(1.0, 1.0, 1.0));
    t16_7.xyz = t16_7.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_5.xyz);
    t16_5.xyz = vec3(t16_32) * t16_7.xyz + t16_5.xyz;
    t16_5.xyz = t16_5.xyz * vs_TEXCOORD2.xxx;
    t16_5.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_5.xyz;
    SV_Target0.xyz = t16_5.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _DetailA;
uniform sampler2D _DetailB;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _Color2;
uniform lowp vec4 _Color3;
uniform highp vec4 _DetailScale;
mediump vec3 tmpvar_1;
mediump vec2 tmpvar_2;
mediump float tmpvar_3;
mediump vec3 tmpvar_4;
mediump vec3 tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  tmpvar_2 = xlv_TEXCOORD0.xy;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  highp float _selector_10;
  _selector_10 = tmpvar_9.z;
  highp float ref_11;
  highp vec4 result_12;
  result_12.x = float((_selector_10 >= 0.75));
  ref_11 = (_selector_10 - (result_12.x * 2.0));
  result_12.y = float((ref_11 >= 0.5));
  ref_11 = (ref_11 - (result_12.y * 2.0));
  result_12.z = float((ref_11 >= 0.25));
  ref_11 = (ref_11 - (result_12.z * 2.0));
  result_12.w = float((ref_11 >= 0.0));
  highp vec2 _uv_13;
  _uv_13 = tmpvar_2;
  highp vec2 result_14;
  highp vec4 d1_15;
  highp vec4 d0_16;
  highp vec4 tmpvar_17;
  tmpvar_17 = (_DetailScale * _uv_13.xyxy);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_DetailA, tmpvar_17.xy);
  d0_16 = tmpvar_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_DetailB, tmpvar_17.zw);
  d1_15 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20.zw = vec2(0.0, 0.0);
  tmpvar_20.x = d0_16.x;
  tmpvar_20.y = d1_15.x;
  result_14.x = dot ((tmpvar_20 - vec4(0.5, 0.5, 0.0, 0.0)), result_12.wxxx);
  highp vec4 tmpvar_21;
  tmpvar_21.zw = vec2(0.0, 0.0);
  tmpvar_21.x = d0_16.y;
  tmpvar_21.y = d1_15.y;
  result_14.y = dot ((tmpvar_21 - vec4(0.5, 0.5, 0.0, 0.0)), result_12.wxxx);
  highp vec4 tmpvar_22;
  highp mat4 tmpvar_23;
  lowp mat4 tmpvar_24;
  tmpvar_24[0].x = _Color0.x;
  tmpvar_24[0].y = _Color1.x;
  tmpvar_24[0].z = _Color2.x;
  tmpvar_24[0].w = _Color3.x;
  tmpvar_24[1].x = _Color0.y;
  tmpvar_24[1].y = _Color1.y;
  tmpvar_24[1].z = _Color2.y;
  tmpvar_24[1].w = _Color3.y;
  tmpvar_24[2].x = _Color0.z;
  tmpvar_24[2].y = _Color1.z;
  tmpvar_24[2].z = _Color2.z;
  tmpvar_24[2].w = _Color3.z;
  tmpvar_24[3].x = _Color0.w;
  tmpvar_24[3].y = _Color1.w;
  tmpvar_24[3].z = _Color2.w;
  tmpvar_24[3].w = _Color3.w;
  tmpvar_23 = tmpvar_24;
  tmpvar_22 = (result_12 * tmpvar_23);
  tmpvar_4 = ((tmpvar_9.xxx * tmpvar_22.xyz) + result_14.xxx);
  tmpvar_3 = (result_14.y + tmpvar_9.y);
  mediump vec3 tangentNormal_25;
  mediump vec3 tmpvar_26;
  tmpvar_26.x = xlv_TEXCOORD3.w;
  tmpvar_26.y = xlv_TEXCOORD4.w;
  tmpvar_26.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_27;
  mediump vec3 tmpvar_28;
  tmpvar_27 = xlv_TEXCOORD5.xyz;
  tmpvar_28 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_29;
  tmpvar_29[0].x = tmpvar_27.x;
  tmpvar_29[0].y = tmpvar_26.x;
  tmpvar_29[0].z = tmpvar_28.x;
  tmpvar_29[1].x = tmpvar_27.y;
  tmpvar_29[1].y = tmpvar_26.y;
  tmpvar_29[1].z = tmpvar_28.y;
  tmpvar_29[2].x = tmpvar_27.z;
  tmpvar_29[2].y = tmpvar_26.z;
  tmpvar_29[2].z = tmpvar_28.z;
  lowp vec3 tmpvar_30;
  tmpvar_30 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_25 = tmpvar_30;
  tmpvar_5 = (tangentNormal_25 * tmpvar_29);
  mediump vec4 tmpvar_31;
  tmpvar_31.w = 1.0;
  tmpvar_31.xyz = tmpvar_5;
  mediump vec3 x2_32;
  mediump vec3 x1_33;
  x1_33.x = dot (unity_SHAr, tmpvar_31);
  x1_33.y = dot (unity_SHAg, tmpvar_31);
  x1_33.z = dot (unity_SHAb, tmpvar_31);
  mediump vec4 tmpvar_34;
  tmpvar_34 = (tmpvar_5.xyzz * tmpvar_5.yzzx);
  x2_32.x = dot (unity_SHBr, tmpvar_34);
  x2_32.y = dot (unity_SHBg, tmpvar_34);
  x2_32.z = dot (unity_SHBb, tmpvar_34);
  mediump vec3 tmpvar_35;
  tmpvar_35 = max (((1.055 * 
    pow ((((x2_32 + 
      (unity_SHC.xyz * ((tmpvar_5.x * tmpvar_5.x) - (tmpvar_5.y * tmpvar_5.y)))
    ) + x1_33) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = ((max (
    dot (tmpvar_5, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_35 * tmpvar_4)
   * 
    (1.0 - tmpvar_3)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_6), vec3(
    min (1.0, (tmpvar_6 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (tmpvar_5, xlv_TEXCOORD3.xyz) * tmpvar_5)
      )), -(tmpvar_5)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_35)
   * tmpvar_3) * tmpvar_4));
  mediump vec3 atten_36;
  lowp float tmpvar_37;
  highp float tmpvar_38;
  tmpvar_38 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_37 = tmpvar_38;
  lowp vec3 tmpvar_39;
  tmpvar_39 = vec3(tmpvar_37);
  atten_36 = tmpvar_39;
  lit_color_8 = (lit_color_8 * max ((_Shadows.x + 
    (atten_36 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_40;
  tmpvar_40 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_6 * _LightParameters.y)));
  c_7.xyz = tmpvar_40;
  c_7.w = _RimColor.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (c_7.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _DetailA;
uniform sampler2D _DetailB;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _Color2;
uniform lowp vec4 _Color3;
uniform highp vec4 _DetailScale;
mediump vec3 tmpvar_1;
mediump vec2 tmpvar_2;
mediump float tmpvar_3;
mediump vec3 tmpvar_4;
mediump vec3 tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  tmpvar_2 = xlv_TEXCOORD0.xy;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  highp float _selector_10;
  _selector_10 = tmpvar_9.z;
  highp float ref_11;
  highp vec4 result_12;
  result_12.x = float((_selector_10 >= 0.75));
  ref_11 = (_selector_10 - (result_12.x * 2.0));
  result_12.y = float((ref_11 >= 0.5));
  ref_11 = (ref_11 - (result_12.y * 2.0));
  result_12.z = float((ref_11 >= 0.25));
  ref_11 = (ref_11 - (result_12.z * 2.0));
  result_12.w = float((ref_11 >= 0.0));
  highp vec2 _uv_13;
  _uv_13 = tmpvar_2;
  highp vec2 result_14;
  highp vec4 d1_15;
  highp vec4 d0_16;
  highp vec4 tmpvar_17;
  tmpvar_17 = (_DetailScale * _uv_13.xyxy);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_DetailA, tmpvar_17.xy);
  d0_16 = tmpvar_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_DetailB, tmpvar_17.zw);
  d1_15 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20.zw = vec2(0.0, 0.0);
  tmpvar_20.x = d0_16.x;
  tmpvar_20.y = d1_15.x;
  result_14.x = dot ((tmpvar_20 - vec4(0.5, 0.5, 0.0, 0.0)), result_12.wxxx);
  highp vec4 tmpvar_21;
  tmpvar_21.zw = vec2(0.0, 0.0);
  tmpvar_21.x = d0_16.y;
  tmpvar_21.y = d1_15.y;
  result_14.y = dot ((tmpvar_21 - vec4(0.5, 0.5, 0.0, 0.0)), result_12.wxxx);
  highp vec4 tmpvar_22;
  highp mat4 tmpvar_23;
  lowp mat4 tmpvar_24;
  tmpvar_24[0].x = _Color0.x;
  tmpvar_24[0].y = _Color1.x;
  tmpvar_24[0].z = _Color2.x;
  tmpvar_24[0].w = _Color3.x;
  tmpvar_24[1].x = _Color0.y;
  tmpvar_24[1].y = _Color1.y;
  tmpvar_24[1].z = _Color2.y;
  tmpvar_24[1].w = _Color3.y;
  tmpvar_24[2].x = _Color0.z;
  tmpvar_24[2].y = _Color1.z;
  tmpvar_24[2].z = _Color2.z;
  tmpvar_24[2].w = _Color3.z;
  tmpvar_24[3].x = _Color0.w;
  tmpvar_24[3].y = _Color1.w;
  tmpvar_24[3].z = _Color2.w;
  tmpvar_24[3].w = _Color3.w;
  tmpvar_23 = tmpvar_24;
  tmpvar_22 = (result_12 * tmpvar_23);
  tmpvar_4 = ((tmpvar_9.xxx * tmpvar_22.xyz) + result_14.xxx);
  tmpvar_3 = (result_14.y + tmpvar_9.y);
  mediump vec3 tangentNormal_25;
  mediump vec3 tmpvar_26;
  tmpvar_26.x = xlv_TEXCOORD3.w;
  tmpvar_26.y = xlv_TEXCOORD4.w;
  tmpvar_26.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_27;
  mediump vec3 tmpvar_28;
  tmpvar_27 = xlv_TEXCOORD5.xyz;
  tmpvar_28 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_29;
  tmpvar_29[0].x = tmpvar_27.x;
  tmpvar_29[0].y = tmpvar_26.x;
  tmpvar_29[0].z = tmpvar_28.x;
  tmpvar_29[1].x = tmpvar_27.y;
  tmpvar_29[1].y = tmpvar_26.y;
  tmpvar_29[1].z = tmpvar_28.y;
  tmpvar_29[2].x = tmpvar_27.z;
  tmpvar_29[2].y = tmpvar_26.z;
  tmpvar_29[2].z = tmpvar_28.z;
  lowp vec3 tmpvar_30;
  tmpvar_30 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_25 = tmpvar_30;
  tmpvar_5 = (tangentNormal_25 * tmpvar_29);
  mediump vec4 tmpvar_31;
  tmpvar_31.w = 1.0;
  tmpvar_31.xyz = tmpvar_5;
  mediump vec3 x2_32;
  mediump vec3 x1_33;
  x1_33.x = dot (unity_SHAr, tmpvar_31);
  x1_33.y = dot (unity_SHAg, tmpvar_31);
  x1_33.z = dot (unity_SHAb, tmpvar_31);
  mediump vec4 tmpvar_34;
  tmpvar_34 = (tmpvar_5.xyzz * tmpvar_5.yzzx);
  x2_32.x = dot (unity_SHBr, tmpvar_34);
  x2_32.y = dot (unity_SHBg, tmpvar_34);
  x2_32.z = dot (unity_SHBb, tmpvar_34);
  mediump vec3 tmpvar_35;
  tmpvar_35 = max (((1.055 * 
    pow ((((x2_32 + 
      (unity_SHC.xyz * ((tmpvar_5.x * tmpvar_5.x) - (tmpvar_5.y * tmpvar_5.y)))
    ) + x1_33) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = ((max (
    dot (tmpvar_5, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_35 * tmpvar_4)
   * 
    (1.0 - tmpvar_3)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_6), vec3(
    min (1.0, (tmpvar_6 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (tmpvar_5, xlv_TEXCOORD3.xyz) * tmpvar_5)
      )), -(tmpvar_5)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_35)
   * tmpvar_3) * tmpvar_4));
  mediump vec3 atten_36;
  lowp float shadow_37;
  shadow_37 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_38;
  tmpvar_38 = vec3(shadow_37);
  atten_36 = tmpvar_38;
  lit_color_8 = (lit_color_8 * max ((_Shadows.x + 
    (atten_36 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_39;
  tmpvar_39 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_6 * _LightParameters.y)));
  c_7.xyz = tmpvar_39;
  c_7.w = _RimColor.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (c_7.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1 = t0.yyyy * unity_World2Shadow[0][1];
    t1 = unity_World2Shadow[0][0] * t0.xxxx + t1;
    t1 = unity_World2Shadow[0][2] * t0.zzzz + t1;
    t0 = unity_World2Shadow[0][3] * t0.wwww + t1;
    vs_TEXCOORD8 = t0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _DetailA;
uniform lowp sampler2D _DetailB;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec4 t0;
highp vec4 t1;
mediump vec4 t16_1;
lowp vec3 t10_1;
highp float t2;
mediump vec4 t16_2;
lowp vec4 t10_2;
highp vec4 t3;
highp vec3 t4;
mediump vec4 t16_5;
lowp vec3 t10_6;
mediump vec3 t16_7;
mediump vec3 t16_8;
highp float t9;
mediump vec3 t16_16;
highp float t19;
bvec2 tb19;
bool tb28;
mediump float t16_32;
void main()
{
    t0 = vs_TEXCOORD0.xyxy * _DetailScale;
    t1.xz = texture(_DetailA, t0.xy).xy;
    t1.yw = texture(_DetailB, t0.zw).xy;
    t0 = t1 + vec4(-0.5, -0.5, -0.5, -0.5);
    t10_1.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    tb28 = t10_1.z>=0.75;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.x = tb28 ? 1.0 : float(0.0);
    t19 = t10_1.z + t2;
    tb28 = t19>=0.5;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.y = tb28 ? 1.0 : float(0.0);
    t19 = t19 + t2;
    tb19.y = t19>=0.25;
    t2 = (tb19.y) ? -2.0 : -0.0;
    t19 = t19 + t2;
    tb19.x = t19>=0.0;
    t3.zw = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb19.yx));
    t0.x = dot(t0.yx, t3.xw);
    t9 = dot(t0.wz, t3.xw);
    t9 = t10_1.y + t9;
    t10_2.x = _Color0.x;
    t10_2.y = _Color1.x;
    t10_2.z = _Color2.x;
    t10_2.w = _Color3.x;
    t4.x = dot(t3, t10_2);
    t10_2.x = _Color0.y;
    t10_2.y = _Color1.y;
    t10_2.z = _Color2.y;
    t10_2.w = _Color3.y;
    t4.y = dot(t3, t10_2);
    t10_2.x = _Color0.z;
    t10_2.y = _Color1.z;
    t10_2.z = _Color2.z;
    t10_2.w = _Color3.z;
    t4.z = dot(t3, t10_2);
    t0.xzw = t10_1.xxx * t4.xyz + t0.xxx;
    t16_5.x = vs_TEXCOORD5.x;
    t16_5.y = vs_TEXCOORD3.w;
    t16_5.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_6.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = vs_TEXCOORD5.y;
    t16_5.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = t16_1.y * t16_1.y;
    t16_5.x = t16_1.x * t16_1.x + (-t16_5.x);
    t16_7.xy = vs_TEXCOORD5.zw;
    t16_7.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_6.xyz, t16_7.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_7.x = dot(unity_SHBr, t16_2);
    t16_7.y = dot(unity_SHBg, t16_2);
    t16_7.z = dot(unity_SHBb, t16_2);
    t16_5.xyz = unity_SHC.xyz * t16_5.xxx + t16_7.xyz;
    t16_1.w = 1.0;
    t16_7.x = dot(unity_SHAr, t16_1);
    t16_7.y = dot(unity_SHAg, t16_1);
    t16_7.z = dot(unity_SHAb, t16_1);
    t16_5.xyz = t16_5.xyz + t16_7.xyz;
    t16_5.xyz = t16_5.xyz * _LightParameters.xxx;
    t16_5.xyz = log2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_5.xyz = exp2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_32 = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_32 = t16_32 + t16_32;
    t16_7.xyz = t16_1.xyz * (-vec3(t16_32)) + vs_TEXCOORD3.xyz;
    t16_5.w = dot(t16_7.xyz, (-t16_1.xyz));
    t16_7.x = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_7.x = max(t16_7.x, 0.0);
    t16_5 = max(t16_5, vec4(0.0, 0.0, 0.0, 0.25));
    t16_32 = t16_5.w * _Glossiness.w;
    t16_32 = t16_32 * _MatGlossiness;
    t16_32 = t16_32 * 10.0;
    t16_16.xyz = vec3(t16_32) * _Glossiness.xyz;
    t16_16.xyz = t16_5.xyz * t16_16.xyz;
    t16_5.xyz = t0.xzw * t16_5.xyz;
    t16_16.xyz = vec3(t9) * t16_16.xyz;
    t16_32 = (-t9) + 1.0;
    t16_5.xyz = vec3(t16_32) * t16_5.xyz;
    t16_16.xyz = t0.xzw * t16_16.xyz;
    t10_6.x = (-_LightDir.w) + 1.0;
    t16_32 = t16_7.x * _LightDir.w + t10_6.x;
    t16_7.x = t16_32 * 1.5;
    t16_7.x = min(t16_7.x, 1.0);
    t16_8.xyz = vec3(t16_32) + (-_CharacterShadowColor.xyz);
    t16_32 = t16_32 * _LightParameters.y;
    t16_8.xyz = t16_7.xxx * t16_8.xyz + _CharacterShadowColor.xyz;
    t16_5.xyz = t16_5.xyz * t16_8.xyz + t16_16.xyz;
    vec3 txVec30 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_7.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec30, 0.0);
    t16_16.x = (-_LightShadowData.x) + 1.0;
    t16_7.x = t16_7.x * t16_16.x + _LightShadowData.x;
    t16_7.x = t16_7.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_7.x = max(t16_7.x, 0.0);
    t16_5.xyz = t16_5.xyz * t16_7.xxx;
    t16_7.x = dot(t16_5.xyz, vec3(1.0, 1.0, 1.0));
    t16_7.xyz = t16_7.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_5.xyz);
    t16_5.xyz = vec3(t16_32) * t16_7.xyz + t16_5.xyz;
    t16_5.xyz = t16_5.xyz * vs_TEXCOORD2.xxx;
    t16_5.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_5.xyz;
    SV_Target0.xyz = t16_5.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _DetailA;
uniform sampler2D _DetailB;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _Color2;
uniform lowp vec4 _Color3;
uniform highp vec4 _DetailScale;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump vec2 tmpvar_2;
mediump float tmpvar_3;
mediump vec3 tmpvar_4;
mediump vec3 tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  tmpvar_2 = xlv_TEXCOORD0.xy;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  highp float _selector_10;
  _selector_10 = tmpvar_9.z;
  highp float ref_11;
  highp vec4 result_12;
  result_12.x = float((_selector_10 >= 0.75));
  ref_11 = (_selector_10 - (result_12.x * 2.0));
  result_12.y = float((ref_11 >= 0.5));
  ref_11 = (ref_11 - (result_12.y * 2.0));
  result_12.z = float((ref_11 >= 0.25));
  ref_11 = (ref_11 - (result_12.z * 2.0));
  result_12.w = float((ref_11 >= 0.0));
  highp vec2 _uv_13;
  _uv_13 = tmpvar_2;
  highp vec2 result_14;
  highp vec4 d1_15;
  highp vec4 d0_16;
  highp vec4 tmpvar_17;
  tmpvar_17 = (_DetailScale * _uv_13.xyxy);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_DetailA, tmpvar_17.xy);
  d0_16 = tmpvar_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_DetailB, tmpvar_17.zw);
  d1_15 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20.zw = vec2(0.0, 0.0);
  tmpvar_20.x = d0_16.x;
  tmpvar_20.y = d1_15.x;
  result_14.x = dot ((tmpvar_20 - vec4(0.5, 0.5, 0.0, 0.0)), result_12.wxxx);
  highp vec4 tmpvar_21;
  tmpvar_21.zw = vec2(0.0, 0.0);
  tmpvar_21.x = d0_16.y;
  tmpvar_21.y = d1_15.y;
  result_14.y = dot ((tmpvar_21 - vec4(0.5, 0.5, 0.0, 0.0)), result_12.wxxx);
  highp vec4 tmpvar_22;
  highp mat4 tmpvar_23;
  lowp mat4 tmpvar_24;
  tmpvar_24[0].x = _Color0.x;
  tmpvar_24[0].y = _Color1.x;
  tmpvar_24[0].z = _Color2.x;
  tmpvar_24[0].w = _Color3.x;
  tmpvar_24[1].x = _Color0.y;
  tmpvar_24[1].y = _Color1.y;
  tmpvar_24[1].z = _Color2.y;
  tmpvar_24[1].w = _Color3.y;
  tmpvar_24[2].x = _Color0.z;
  tmpvar_24[2].y = _Color1.z;
  tmpvar_24[2].z = _Color2.z;
  tmpvar_24[2].w = _Color3.z;
  tmpvar_24[3].x = _Color0.w;
  tmpvar_24[3].y = _Color1.w;
  tmpvar_24[3].z = _Color2.w;
  tmpvar_24[3].w = _Color3.w;
  tmpvar_23 = tmpvar_24;
  tmpvar_22 = (result_12 * tmpvar_23);
  tmpvar_4 = ((tmpvar_9.xxx * tmpvar_22.xyz) + result_14.xxx);
  tmpvar_3 = (result_14.y + tmpvar_9.y);
  mediump vec3 tangentNormal_25;
  mediump vec3 tmpvar_26;
  tmpvar_26.x = xlv_TEXCOORD3.w;
  tmpvar_26.y = xlv_TEXCOORD4.w;
  tmpvar_26.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_27;
  mediump vec3 tmpvar_28;
  tmpvar_27 = xlv_TEXCOORD5.xyz;
  tmpvar_28 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_29;
  tmpvar_29[0].x = tmpvar_27.x;
  tmpvar_29[0].y = tmpvar_26.x;
  tmpvar_29[0].z = tmpvar_28.x;
  tmpvar_29[1].x = tmpvar_27.y;
  tmpvar_29[1].y = tmpvar_26.y;
  tmpvar_29[1].z = tmpvar_28.y;
  tmpvar_29[2].x = tmpvar_27.z;
  tmpvar_29[2].y = tmpvar_26.z;
  tmpvar_29[2].z = tmpvar_28.z;
  lowp vec3 tmpvar_30;
  tmpvar_30 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_25 = tmpvar_30;
  tmpvar_5 = (tangentNormal_25 * tmpvar_29);
  mediump vec4 tmpvar_31;
  tmpvar_31.w = 1.0;
  tmpvar_31.xyz = tmpvar_5;
  mediump vec3 x2_32;
  mediump vec3 x1_33;
  x1_33.x = dot (unity_SHAr, tmpvar_31);
  x1_33.y = dot (unity_SHAg, tmpvar_31);
  x1_33.z = dot (unity_SHAb, tmpvar_31);
  mediump vec4 tmpvar_34;
  tmpvar_34 = (tmpvar_5.xyzz * tmpvar_5.yzzx);
  x2_32.x = dot (unity_SHBr, tmpvar_34);
  x2_32.y = dot (unity_SHBg, tmpvar_34);
  x2_32.z = dot (unity_SHBb, tmpvar_34);
  mediump vec3 tmpvar_35;
  tmpvar_35 = max (((1.055 * 
    pow ((((x2_32 + 
      (unity_SHC.xyz * ((tmpvar_5.x * tmpvar_5.x) - (tmpvar_5.y * tmpvar_5.y)))
    ) + x1_33) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = ((max (
    dot (tmpvar_5, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_35 * tmpvar_4)
   * 
    (1.0 - tmpvar_3)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_6), vec3(
    min (1.0, (tmpvar_6 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (tmpvar_5, xlv_TEXCOORD3.xyz) * tmpvar_5)
      )), -(tmpvar_5)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_35)
   * tmpvar_3) * tmpvar_4));
  mediump vec3 tmpvar_36;
  tmpvar_36 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_6 * _LightParameters.y)));
  c_7.xyz = tmpvar_36;
  c_7.w = _RimColor.w;
  lowp vec4 _color_37;
  _color_37.w = c_7.w;
  lowp vec4 tmpvar_38;
  tmpvar_38 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_39;
  tmpvar_39 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_38.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_37.xyz = (_NoiseGlow.xyz * (tmpvar_39 + c_7.xyz));
  c_7.w = _color_37.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_37.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _DetailA;
uniform lowp sampler2D _DetailB;
uniform lowp sampler2D _NoiseTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec4 t0;
lowp float t10_0;
highp vec4 t1;
mediump vec4 t16_1;
lowp vec3 t10_1;
highp float t2;
mediump vec4 t16_2;
lowp vec4 t10_2;
highp vec4 t3;
highp vec3 t4;
mediump vec4 t16_5;
lowp vec3 t10_6;
mediump vec3 t16_7;
mediump vec3 t16_8;
highp float t9;
mediump vec3 t16_16;
highp float t19;
bvec2 tb19;
bool tb28;
mediump float t16_32;
void main()
{
    t0 = vs_TEXCOORD0.xyxy * _DetailScale;
    t1.xz = texture(_DetailA, t0.xy).xy;
    t1.yw = texture(_DetailB, t0.zw).xy;
    t0 = t1 + vec4(-0.5, -0.5, -0.5, -0.5);
    t10_1.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    tb28 = t10_1.z>=0.75;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.x = tb28 ? 1.0 : float(0.0);
    t19 = t10_1.z + t2;
    tb28 = t19>=0.5;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.y = tb28 ? 1.0 : float(0.0);
    t19 = t19 + t2;
    tb19.y = t19>=0.25;
    t2 = (tb19.y) ? -2.0 : -0.0;
    t19 = t19 + t2;
    tb19.x = t19>=0.0;
    t3.zw = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb19.yx));
    t0.x = dot(t0.yx, t3.xw);
    t9 = dot(t0.wz, t3.xw);
    t9 = t10_1.y + t9;
    t10_2.x = _Color0.x;
    t10_2.y = _Color1.x;
    t10_2.z = _Color2.x;
    t10_2.w = _Color3.x;
    t4.x = dot(t3, t10_2);
    t10_2.x = _Color0.y;
    t10_2.y = _Color1.y;
    t10_2.z = _Color2.y;
    t10_2.w = _Color3.y;
    t4.y = dot(t3, t10_2);
    t10_2.x = _Color0.z;
    t10_2.y = _Color1.z;
    t10_2.z = _Color2.z;
    t10_2.w = _Color3.z;
    t4.z = dot(t3, t10_2);
    t0.xzw = t10_1.xxx * t4.xyz + t0.xxx;
    t16_5.x = vs_TEXCOORD5.x;
    t16_5.y = vs_TEXCOORD3.w;
    t16_5.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_6.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = vs_TEXCOORD5.y;
    t16_5.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = t16_1.y * t16_1.y;
    t16_5.x = t16_1.x * t16_1.x + (-t16_5.x);
    t16_7.xy = vs_TEXCOORD5.zw;
    t16_7.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_6.xyz, t16_7.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_7.x = dot(unity_SHBr, t16_2);
    t16_7.y = dot(unity_SHBg, t16_2);
    t16_7.z = dot(unity_SHBb, t16_2);
    t16_5.xyz = unity_SHC.xyz * t16_5.xxx + t16_7.xyz;
    t16_1.w = 1.0;
    t16_7.x = dot(unity_SHAr, t16_1);
    t16_7.y = dot(unity_SHAg, t16_1);
    t16_7.z = dot(unity_SHAb, t16_1);
    t16_5.xyz = t16_5.xyz + t16_7.xyz;
    t16_5.xyz = t16_5.xyz * _LightParameters.xxx;
    t16_5.xyz = log2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_5.xyz = exp2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_32 = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_32 = t16_32 + t16_32;
    t16_7.xyz = t16_1.xyz * (-vec3(t16_32)) + vs_TEXCOORD3.xyz;
    t16_5.w = dot(t16_7.xyz, (-t16_1.xyz));
    t16_7.x = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_7.x = max(t16_7.x, 0.0);
    t16_5 = max(t16_5, vec4(0.0, 0.0, 0.0, 0.25));
    t16_32 = t16_5.w * _Glossiness.w;
    t16_32 = t16_32 * _MatGlossiness;
    t16_32 = t16_32 * 10.0;
    t16_16.xyz = vec3(t16_32) * _Glossiness.xyz;
    t16_16.xyz = t16_5.xyz * t16_16.xyz;
    t16_5.xyz = t0.xzw * t16_5.xyz;
    t16_16.xyz = vec3(t9) * t16_16.xyz;
    t16_32 = (-t9) + 1.0;
    t16_5.xyz = vec3(t16_32) * t16_5.xyz;
    t16_16.xyz = t0.xzw * t16_16.xyz;
    t10_6.x = (-_LightDir.w) + 1.0;
    t16_32 = t16_7.x * _LightDir.w + t10_6.x;
    t16_7.x = t16_32 * 1.5;
    t16_7.x = min(t16_7.x, 1.0);
    t16_8.xyz = vec3(t16_32) + (-_CharacterShadowColor.xyz);
    t16_32 = t16_32 * _LightParameters.y;
    t16_8.xyz = t16_7.xxx * t16_8.xyz + _CharacterShadowColor.xyz;
    t16_5.xyz = t16_5.xyz * t16_8.xyz + t16_16.xyz;
    t16_7.x = dot(t16_5.xyz, vec3(1.0, 1.0, 1.0));
    t16_7.xyz = t16_7.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_5.xyz);
    t16_5.xyz = vec3(t16_32) * t16_7.xyz + t16_5.xyz;
    t10_0 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_32 = t10_0 * 0.9375 + 0.03125;
    t16_32 = (-t16_32) + _NoiseGlow.w;
    t16_32 = -abs(t16_32) * 32.0 + 1.0;
    t16_32 = max(t16_32, 0.0);
    t16_5.xyz = t16_5.xyz + vec3(t16_32);
    t16_5.xyz = t16_5.xyz * _NoiseGlow.xyz;
    t16_5.xyz = t16_5.xyz * vs_TEXCOORD2.xxx;
    t16_5.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_5.xyz;
    SV_Target0.xyz = t16_5.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _DetailA;
uniform sampler2D _DetailB;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _Color2;
uniform lowp vec4 _Color3;
uniform highp vec4 _DetailScale;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump vec2 tmpvar_2;
mediump float tmpvar_3;
mediump vec3 tmpvar_4;
mediump vec3 tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  tmpvar_2 = xlv_TEXCOORD0.xy;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  highp float _selector_10;
  _selector_10 = tmpvar_9.z;
  highp float ref_11;
  highp vec4 result_12;
  result_12.x = float((_selector_10 >= 0.75));
  ref_11 = (_selector_10 - (result_12.x * 2.0));
  result_12.y = float((ref_11 >= 0.5));
  ref_11 = (ref_11 - (result_12.y * 2.0));
  result_12.z = float((ref_11 >= 0.25));
  ref_11 = (ref_11 - (result_12.z * 2.0));
  result_12.w = float((ref_11 >= 0.0));
  highp vec2 _uv_13;
  _uv_13 = tmpvar_2;
  highp vec2 result_14;
  highp vec4 d1_15;
  highp vec4 d0_16;
  highp vec4 tmpvar_17;
  tmpvar_17 = (_DetailScale * _uv_13.xyxy);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_DetailA, tmpvar_17.xy);
  d0_16 = tmpvar_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_DetailB, tmpvar_17.zw);
  d1_15 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20.zw = vec2(0.0, 0.0);
  tmpvar_20.x = d0_16.x;
  tmpvar_20.y = d1_15.x;
  result_14.x = dot ((tmpvar_20 - vec4(0.5, 0.5, 0.0, 0.0)), result_12.wxxx);
  highp vec4 tmpvar_21;
  tmpvar_21.zw = vec2(0.0, 0.0);
  tmpvar_21.x = d0_16.y;
  tmpvar_21.y = d1_15.y;
  result_14.y = dot ((tmpvar_21 - vec4(0.5, 0.5, 0.0, 0.0)), result_12.wxxx);
  highp vec4 tmpvar_22;
  highp mat4 tmpvar_23;
  lowp mat4 tmpvar_24;
  tmpvar_24[0].x = _Color0.x;
  tmpvar_24[0].y = _Color1.x;
  tmpvar_24[0].z = _Color2.x;
  tmpvar_24[0].w = _Color3.x;
  tmpvar_24[1].x = _Color0.y;
  tmpvar_24[1].y = _Color1.y;
  tmpvar_24[1].z = _Color2.y;
  tmpvar_24[1].w = _Color3.y;
  tmpvar_24[2].x = _Color0.z;
  tmpvar_24[2].y = _Color1.z;
  tmpvar_24[2].z = _Color2.z;
  tmpvar_24[2].w = _Color3.z;
  tmpvar_24[3].x = _Color0.w;
  tmpvar_24[3].y = _Color1.w;
  tmpvar_24[3].z = _Color2.w;
  tmpvar_24[3].w = _Color3.w;
  tmpvar_23 = tmpvar_24;
  tmpvar_22 = (result_12 * tmpvar_23);
  tmpvar_4 = ((tmpvar_9.xxx * tmpvar_22.xyz) + result_14.xxx);
  tmpvar_3 = (result_14.y + tmpvar_9.y);
  mediump vec3 tangentNormal_25;
  mediump vec3 tmpvar_26;
  tmpvar_26.x = xlv_TEXCOORD3.w;
  tmpvar_26.y = xlv_TEXCOORD4.w;
  tmpvar_26.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_27;
  mediump vec3 tmpvar_28;
  tmpvar_27 = xlv_TEXCOORD5.xyz;
  tmpvar_28 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_29;
  tmpvar_29[0].x = tmpvar_27.x;
  tmpvar_29[0].y = tmpvar_26.x;
  tmpvar_29[0].z = tmpvar_28.x;
  tmpvar_29[1].x = tmpvar_27.y;
  tmpvar_29[1].y = tmpvar_26.y;
  tmpvar_29[1].z = tmpvar_28.y;
  tmpvar_29[2].x = tmpvar_27.z;
  tmpvar_29[2].y = tmpvar_26.z;
  tmpvar_29[2].z = tmpvar_28.z;
  lowp vec3 tmpvar_30;
  tmpvar_30 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_25 = tmpvar_30;
  tmpvar_5 = (tangentNormal_25 * tmpvar_29);
  mediump vec4 tmpvar_31;
  tmpvar_31.w = 1.0;
  tmpvar_31.xyz = tmpvar_5;
  mediump vec3 x2_32;
  mediump vec3 x1_33;
  x1_33.x = dot (unity_SHAr, tmpvar_31);
  x1_33.y = dot (unity_SHAg, tmpvar_31);
  x1_33.z = dot (unity_SHAb, tmpvar_31);
  mediump vec4 tmpvar_34;
  tmpvar_34 = (tmpvar_5.xyzz * tmpvar_5.yzzx);
  x2_32.x = dot (unity_SHBr, tmpvar_34);
  x2_32.y = dot (unity_SHBg, tmpvar_34);
  x2_32.z = dot (unity_SHBb, tmpvar_34);
  mediump vec3 tmpvar_35;
  tmpvar_35 = max (((1.055 * 
    pow ((((x2_32 + 
      (unity_SHC.xyz * ((tmpvar_5.x * tmpvar_5.x) - (tmpvar_5.y * tmpvar_5.y)))
    ) + x1_33) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = ((max (
    dot (tmpvar_5, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_35 * tmpvar_4)
   * 
    (1.0 - tmpvar_3)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_6), vec3(
    min (1.0, (tmpvar_6 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (tmpvar_5, xlv_TEXCOORD3.xyz) * tmpvar_5)
      )), -(tmpvar_5)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_35)
   * tmpvar_3) * tmpvar_4));
  mediump vec3 tmpvar_36;
  tmpvar_36 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_6 * _LightParameters.y)));
  c_7.xyz = tmpvar_36;
  c_7.w = _RimColor.w;
  lowp vec4 _color_37;
  _color_37.w = c_7.w;
  lowp vec4 tmpvar_38;
  tmpvar_38 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_39;
  tmpvar_39 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_38.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_37.xyz = (_NoiseGlow.xyz * (tmpvar_39 + c_7.xyz));
  c_7.w = _color_37.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_37.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _DetailA;
uniform lowp sampler2D _DetailB;
uniform lowp sampler2D _NoiseTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec4 t0;
lowp float t10_0;
highp vec4 t1;
mediump vec4 t16_1;
lowp vec3 t10_1;
highp float t2;
mediump vec4 t16_2;
lowp vec4 t10_2;
highp vec4 t3;
highp vec3 t4;
mediump vec4 t16_5;
lowp vec3 t10_6;
mediump vec3 t16_7;
mediump vec3 t16_8;
highp float t9;
mediump vec3 t16_16;
highp float t19;
bvec2 tb19;
bool tb28;
mediump float t16_32;
void main()
{
    t0 = vs_TEXCOORD0.xyxy * _DetailScale;
    t1.xz = texture(_DetailA, t0.xy).xy;
    t1.yw = texture(_DetailB, t0.zw).xy;
    t0 = t1 + vec4(-0.5, -0.5, -0.5, -0.5);
    t10_1.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    tb28 = t10_1.z>=0.75;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.x = tb28 ? 1.0 : float(0.0);
    t19 = t10_1.z + t2;
    tb28 = t19>=0.5;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.y = tb28 ? 1.0 : float(0.0);
    t19 = t19 + t2;
    tb19.y = t19>=0.25;
    t2 = (tb19.y) ? -2.0 : -0.0;
    t19 = t19 + t2;
    tb19.x = t19>=0.0;
    t3.zw = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb19.yx));
    t0.x = dot(t0.yx, t3.xw);
    t9 = dot(t0.wz, t3.xw);
    t9 = t10_1.y + t9;
    t10_2.x = _Color0.x;
    t10_2.y = _Color1.x;
    t10_2.z = _Color2.x;
    t10_2.w = _Color3.x;
    t4.x = dot(t3, t10_2);
    t10_2.x = _Color0.y;
    t10_2.y = _Color1.y;
    t10_2.z = _Color2.y;
    t10_2.w = _Color3.y;
    t4.y = dot(t3, t10_2);
    t10_2.x = _Color0.z;
    t10_2.y = _Color1.z;
    t10_2.z = _Color2.z;
    t10_2.w = _Color3.z;
    t4.z = dot(t3, t10_2);
    t0.xzw = t10_1.xxx * t4.xyz + t0.xxx;
    t16_5.x = vs_TEXCOORD5.x;
    t16_5.y = vs_TEXCOORD3.w;
    t16_5.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_6.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = vs_TEXCOORD5.y;
    t16_5.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = t16_1.y * t16_1.y;
    t16_5.x = t16_1.x * t16_1.x + (-t16_5.x);
    t16_7.xy = vs_TEXCOORD5.zw;
    t16_7.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_6.xyz, t16_7.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_7.x = dot(unity_SHBr, t16_2);
    t16_7.y = dot(unity_SHBg, t16_2);
    t16_7.z = dot(unity_SHBb, t16_2);
    t16_5.xyz = unity_SHC.xyz * t16_5.xxx + t16_7.xyz;
    t16_1.w = 1.0;
    t16_7.x = dot(unity_SHAr, t16_1);
    t16_7.y = dot(unity_SHAg, t16_1);
    t16_7.z = dot(unity_SHAb, t16_1);
    t16_5.xyz = t16_5.xyz + t16_7.xyz;
    t16_5.xyz = t16_5.xyz * _LightParameters.xxx;
    t16_5.xyz = log2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_5.xyz = exp2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_32 = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_32 = t16_32 + t16_32;
    t16_7.xyz = t16_1.xyz * (-vec3(t16_32)) + vs_TEXCOORD3.xyz;
    t16_5.w = dot(t16_7.xyz, (-t16_1.xyz));
    t16_7.x = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_7.x = max(t16_7.x, 0.0);
    t16_5 = max(t16_5, vec4(0.0, 0.0, 0.0, 0.25));
    t16_32 = t16_5.w * _Glossiness.w;
    t16_32 = t16_32 * _MatGlossiness;
    t16_32 = t16_32 * 10.0;
    t16_16.xyz = vec3(t16_32) * _Glossiness.xyz;
    t16_16.xyz = t16_5.xyz * t16_16.xyz;
    t16_5.xyz = t0.xzw * t16_5.xyz;
    t16_16.xyz = vec3(t9) * t16_16.xyz;
    t16_32 = (-t9) + 1.0;
    t16_5.xyz = vec3(t16_32) * t16_5.xyz;
    t16_16.xyz = t0.xzw * t16_16.xyz;
    t10_6.x = (-_LightDir.w) + 1.0;
    t16_32 = t16_7.x * _LightDir.w + t10_6.x;
    t16_7.x = t16_32 * 1.5;
    t16_7.x = min(t16_7.x, 1.0);
    t16_8.xyz = vec3(t16_32) + (-_CharacterShadowColor.xyz);
    t16_32 = t16_32 * _LightParameters.y;
    t16_8.xyz = t16_7.xxx * t16_8.xyz + _CharacterShadowColor.xyz;
    t16_5.xyz = t16_5.xyz * t16_8.xyz + t16_16.xyz;
    t16_7.x = dot(t16_5.xyz, vec3(1.0, 1.0, 1.0));
    t16_7.xyz = t16_7.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_5.xyz);
    t16_5.xyz = vec3(t16_32) * t16_7.xyz + t16_5.xyz;
    t10_0 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_32 = t10_0 * 0.9375 + 0.03125;
    t16_32 = (-t16_32) + _NoiseGlow.w;
    t16_32 = -abs(t16_32) * 32.0 + 1.0;
    t16_32 = max(t16_32, 0.0);
    t16_5.xyz = t16_5.xyz + vec3(t16_32);
    t16_5.xyz = t16_5.xyz * _NoiseGlow.xyz;
    t16_5.xyz = t16_5.xyz * vs_TEXCOORD2.xxx;
    t16_5.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_5.xyz;
    SV_Target0.xyz = t16_5.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_NOISEGLOW" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _DetailA;
uniform sampler2D _DetailB;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _Color2;
uniform lowp vec4 _Color3;
uniform highp vec4 _DetailScale;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump vec2 tmpvar_2;
mediump float tmpvar_3;
mediump vec3 tmpvar_4;
mediump vec3 tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  tmpvar_2 = xlv_TEXCOORD0.xy;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  highp float _selector_10;
  _selector_10 = tmpvar_9.z;
  highp float ref_11;
  highp vec4 result_12;
  result_12.x = float((_selector_10 >= 0.75));
  ref_11 = (_selector_10 - (result_12.x * 2.0));
  result_12.y = float((ref_11 >= 0.5));
  ref_11 = (ref_11 - (result_12.y * 2.0));
  result_12.z = float((ref_11 >= 0.25));
  ref_11 = (ref_11 - (result_12.z * 2.0));
  result_12.w = float((ref_11 >= 0.0));
  highp vec2 _uv_13;
  _uv_13 = tmpvar_2;
  highp vec2 result_14;
  highp vec4 d1_15;
  highp vec4 d0_16;
  highp vec4 tmpvar_17;
  tmpvar_17 = (_DetailScale * _uv_13.xyxy);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_DetailA, tmpvar_17.xy);
  d0_16 = tmpvar_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_DetailB, tmpvar_17.zw);
  d1_15 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20.zw = vec2(0.0, 0.0);
  tmpvar_20.x = d0_16.x;
  tmpvar_20.y = d1_15.x;
  result_14.x = dot ((tmpvar_20 - vec4(0.5, 0.5, 0.0, 0.0)), result_12.wxxx);
  highp vec4 tmpvar_21;
  tmpvar_21.zw = vec2(0.0, 0.0);
  tmpvar_21.x = d0_16.y;
  tmpvar_21.y = d1_15.y;
  result_14.y = dot ((tmpvar_21 - vec4(0.5, 0.5, 0.0, 0.0)), result_12.wxxx);
  highp vec4 tmpvar_22;
  highp mat4 tmpvar_23;
  lowp mat4 tmpvar_24;
  tmpvar_24[0].x = _Color0.x;
  tmpvar_24[0].y = _Color1.x;
  tmpvar_24[0].z = _Color2.x;
  tmpvar_24[0].w = _Color3.x;
  tmpvar_24[1].x = _Color0.y;
  tmpvar_24[1].y = _Color1.y;
  tmpvar_24[1].z = _Color2.y;
  tmpvar_24[1].w = _Color3.y;
  tmpvar_24[2].x = _Color0.z;
  tmpvar_24[2].y = _Color1.z;
  tmpvar_24[2].z = _Color2.z;
  tmpvar_24[2].w = _Color3.z;
  tmpvar_24[3].x = _Color0.w;
  tmpvar_24[3].y = _Color1.w;
  tmpvar_24[3].z = _Color2.w;
  tmpvar_24[3].w = _Color3.w;
  tmpvar_23 = tmpvar_24;
  tmpvar_22 = (result_12 * tmpvar_23);
  tmpvar_4 = ((tmpvar_9.xxx * tmpvar_22.xyz) + result_14.xxx);
  tmpvar_3 = (result_14.y + tmpvar_9.y);
  mediump vec3 tangentNormal_25;
  mediump vec3 tmpvar_26;
  tmpvar_26.x = xlv_TEXCOORD3.w;
  tmpvar_26.y = xlv_TEXCOORD4.w;
  tmpvar_26.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_27;
  mediump vec3 tmpvar_28;
  tmpvar_27 = xlv_TEXCOORD5.xyz;
  tmpvar_28 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_29;
  tmpvar_29[0].x = tmpvar_27.x;
  tmpvar_29[0].y = tmpvar_26.x;
  tmpvar_29[0].z = tmpvar_28.x;
  tmpvar_29[1].x = tmpvar_27.y;
  tmpvar_29[1].y = tmpvar_26.y;
  tmpvar_29[1].z = tmpvar_28.y;
  tmpvar_29[2].x = tmpvar_27.z;
  tmpvar_29[2].y = tmpvar_26.z;
  tmpvar_29[2].z = tmpvar_28.z;
  lowp vec3 tmpvar_30;
  tmpvar_30 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_25 = tmpvar_30;
  tmpvar_5 = (tangentNormal_25 * tmpvar_29);
  mediump vec4 tmpvar_31;
  tmpvar_31.w = 1.0;
  tmpvar_31.xyz = tmpvar_5;
  mediump vec3 x2_32;
  mediump vec3 x1_33;
  x1_33.x = dot (unity_SHAr, tmpvar_31);
  x1_33.y = dot (unity_SHAg, tmpvar_31);
  x1_33.z = dot (unity_SHAb, tmpvar_31);
  mediump vec4 tmpvar_34;
  tmpvar_34 = (tmpvar_5.xyzz * tmpvar_5.yzzx);
  x2_32.x = dot (unity_SHBr, tmpvar_34);
  x2_32.y = dot (unity_SHBg, tmpvar_34);
  x2_32.z = dot (unity_SHBb, tmpvar_34);
  mediump vec3 tmpvar_35;
  tmpvar_35 = max (((1.055 * 
    pow ((((x2_32 + 
      (unity_SHC.xyz * ((tmpvar_5.x * tmpvar_5.x) - (tmpvar_5.y * tmpvar_5.y)))
    ) + x1_33) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = ((max (
    dot (tmpvar_5, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_35 * tmpvar_4)
   * 
    (1.0 - tmpvar_3)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_6), vec3(
    min (1.0, (tmpvar_6 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (tmpvar_5, xlv_TEXCOORD3.xyz) * tmpvar_5)
      )), -(tmpvar_5)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_35)
   * tmpvar_3) * tmpvar_4));
  mediump vec3 atten_36;
  lowp float tmpvar_37;
  highp float tmpvar_38;
  tmpvar_38 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_37 = tmpvar_38;
  lowp vec3 tmpvar_39;
  tmpvar_39 = vec3(tmpvar_37);
  atten_36 = tmpvar_39;
  lit_color_8 = (lit_color_8 * max ((_Shadows.x + 
    (atten_36 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_40;
  tmpvar_40 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_6 * _LightParameters.y)));
  c_7.xyz = tmpvar_40;
  c_7.w = _RimColor.w;
  lowp vec4 _color_41;
  _color_41.w = c_7.w;
  lowp vec4 tmpvar_42;
  tmpvar_42 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_43;
  tmpvar_43 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_42.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_41.xyz = (_NoiseGlow.xyz * (tmpvar_43 + c_7.xyz));
  c_7.w = _color_41.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_41.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _DetailA;
uniform sampler2D _DetailB;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _Color2;
uniform lowp vec4 _Color3;
uniform highp vec4 _DetailScale;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump vec2 tmpvar_2;
mediump float tmpvar_3;
mediump vec3 tmpvar_4;
mediump vec3 tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  tmpvar_2 = xlv_TEXCOORD0.xy;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  highp float _selector_10;
  _selector_10 = tmpvar_9.z;
  highp float ref_11;
  highp vec4 result_12;
  result_12.x = float((_selector_10 >= 0.75));
  ref_11 = (_selector_10 - (result_12.x * 2.0));
  result_12.y = float((ref_11 >= 0.5));
  ref_11 = (ref_11 - (result_12.y * 2.0));
  result_12.z = float((ref_11 >= 0.25));
  ref_11 = (ref_11 - (result_12.z * 2.0));
  result_12.w = float((ref_11 >= 0.0));
  highp vec2 _uv_13;
  _uv_13 = tmpvar_2;
  highp vec2 result_14;
  highp vec4 d1_15;
  highp vec4 d0_16;
  highp vec4 tmpvar_17;
  tmpvar_17 = (_DetailScale * _uv_13.xyxy);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_DetailA, tmpvar_17.xy);
  d0_16 = tmpvar_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_DetailB, tmpvar_17.zw);
  d1_15 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20.zw = vec2(0.0, 0.0);
  tmpvar_20.x = d0_16.x;
  tmpvar_20.y = d1_15.x;
  result_14.x = dot ((tmpvar_20 - vec4(0.5, 0.5, 0.0, 0.0)), result_12.wxxx);
  highp vec4 tmpvar_21;
  tmpvar_21.zw = vec2(0.0, 0.0);
  tmpvar_21.x = d0_16.y;
  tmpvar_21.y = d1_15.y;
  result_14.y = dot ((tmpvar_21 - vec4(0.5, 0.5, 0.0, 0.0)), result_12.wxxx);
  highp vec4 tmpvar_22;
  highp mat4 tmpvar_23;
  lowp mat4 tmpvar_24;
  tmpvar_24[0].x = _Color0.x;
  tmpvar_24[0].y = _Color1.x;
  tmpvar_24[0].z = _Color2.x;
  tmpvar_24[0].w = _Color3.x;
  tmpvar_24[1].x = _Color0.y;
  tmpvar_24[1].y = _Color1.y;
  tmpvar_24[1].z = _Color2.y;
  tmpvar_24[1].w = _Color3.y;
  tmpvar_24[2].x = _Color0.z;
  tmpvar_24[2].y = _Color1.z;
  tmpvar_24[2].z = _Color2.z;
  tmpvar_24[2].w = _Color3.z;
  tmpvar_24[3].x = _Color0.w;
  tmpvar_24[3].y = _Color1.w;
  tmpvar_24[3].z = _Color2.w;
  tmpvar_24[3].w = _Color3.w;
  tmpvar_23 = tmpvar_24;
  tmpvar_22 = (result_12 * tmpvar_23);
  tmpvar_4 = ((tmpvar_9.xxx * tmpvar_22.xyz) + result_14.xxx);
  tmpvar_3 = (result_14.y + tmpvar_9.y);
  mediump vec3 tangentNormal_25;
  mediump vec3 tmpvar_26;
  tmpvar_26.x = xlv_TEXCOORD3.w;
  tmpvar_26.y = xlv_TEXCOORD4.w;
  tmpvar_26.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_27;
  mediump vec3 tmpvar_28;
  tmpvar_27 = xlv_TEXCOORD5.xyz;
  tmpvar_28 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_29;
  tmpvar_29[0].x = tmpvar_27.x;
  tmpvar_29[0].y = tmpvar_26.x;
  tmpvar_29[0].z = tmpvar_28.x;
  tmpvar_29[1].x = tmpvar_27.y;
  tmpvar_29[1].y = tmpvar_26.y;
  tmpvar_29[1].z = tmpvar_28.y;
  tmpvar_29[2].x = tmpvar_27.z;
  tmpvar_29[2].y = tmpvar_26.z;
  tmpvar_29[2].z = tmpvar_28.z;
  lowp vec3 tmpvar_30;
  tmpvar_30 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_25 = tmpvar_30;
  tmpvar_5 = (tangentNormal_25 * tmpvar_29);
  mediump vec4 tmpvar_31;
  tmpvar_31.w = 1.0;
  tmpvar_31.xyz = tmpvar_5;
  mediump vec3 x2_32;
  mediump vec3 x1_33;
  x1_33.x = dot (unity_SHAr, tmpvar_31);
  x1_33.y = dot (unity_SHAg, tmpvar_31);
  x1_33.z = dot (unity_SHAb, tmpvar_31);
  mediump vec4 tmpvar_34;
  tmpvar_34 = (tmpvar_5.xyzz * tmpvar_5.yzzx);
  x2_32.x = dot (unity_SHBr, tmpvar_34);
  x2_32.y = dot (unity_SHBg, tmpvar_34);
  x2_32.z = dot (unity_SHBb, tmpvar_34);
  mediump vec3 tmpvar_35;
  tmpvar_35 = max (((1.055 * 
    pow ((((x2_32 + 
      (unity_SHC.xyz * ((tmpvar_5.x * tmpvar_5.x) - (tmpvar_5.y * tmpvar_5.y)))
    ) + x1_33) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = ((max (
    dot (tmpvar_5, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_35 * tmpvar_4)
   * 
    (1.0 - tmpvar_3)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_6), vec3(
    min (1.0, (tmpvar_6 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (tmpvar_5, xlv_TEXCOORD3.xyz) * tmpvar_5)
      )), -(tmpvar_5)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_35)
   * tmpvar_3) * tmpvar_4));
  mediump vec3 atten_36;
  lowp float shadow_37;
  shadow_37 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_38;
  tmpvar_38 = vec3(shadow_37);
  atten_36 = tmpvar_38;
  lit_color_8 = (lit_color_8 * max ((_Shadows.x + 
    (atten_36 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_39;
  tmpvar_39 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_6 * _LightParameters.y)));
  c_7.xyz = tmpvar_39;
  c_7.w = _RimColor.w;
  lowp vec4 _color_40;
  _color_40.w = c_7.w;
  lowp vec4 tmpvar_41;
  tmpvar_41 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_42;
  tmpvar_42 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_41.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_40.xyz = (_NoiseGlow.xyz * (tmpvar_42 + c_7.xyz));
  c_7.w = _color_40.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_40.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1 = t0.yyyy * unity_World2Shadow[0][1];
    t1 = unity_World2Shadow[0][0] * t0.xxxx + t1;
    t1 = unity_World2Shadow[0][2] * t0.zzzz + t1;
    t0 = unity_World2Shadow[0][3] * t0.wwww + t1;
    vs_TEXCOORD8 = t0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _DetailA;
uniform lowp sampler2D _DetailB;
uniform lowp sampler2D _NoiseTexture;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec4 t0;
lowp float t10_0;
highp vec4 t1;
mediump vec4 t16_1;
lowp vec3 t10_1;
highp float t2;
mediump vec4 t16_2;
lowp vec4 t10_2;
highp vec4 t3;
highp vec3 t4;
mediump vec4 t16_5;
lowp vec3 t10_6;
mediump vec3 t16_7;
mediump vec3 t16_8;
highp float t9;
mediump vec3 t16_16;
highp float t19;
bvec2 tb19;
bool tb28;
mediump float t16_32;
void main()
{
    t0 = vs_TEXCOORD0.xyxy * _DetailScale;
    t1.xz = texture(_DetailA, t0.xy).xy;
    t1.yw = texture(_DetailB, t0.zw).xy;
    t0 = t1 + vec4(-0.5, -0.5, -0.5, -0.5);
    t10_1.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    tb28 = t10_1.z>=0.75;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.x = tb28 ? 1.0 : float(0.0);
    t19 = t10_1.z + t2;
    tb28 = t19>=0.5;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.y = tb28 ? 1.0 : float(0.0);
    t19 = t19 + t2;
    tb19.y = t19>=0.25;
    t2 = (tb19.y) ? -2.0 : -0.0;
    t19 = t19 + t2;
    tb19.x = t19>=0.0;
    t3.zw = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb19.yx));
    t0.x = dot(t0.yx, t3.xw);
    t9 = dot(t0.wz, t3.xw);
    t9 = t10_1.y + t9;
    t10_2.x = _Color0.x;
    t10_2.y = _Color1.x;
    t10_2.z = _Color2.x;
    t10_2.w = _Color3.x;
    t4.x = dot(t3, t10_2);
    t10_2.x = _Color0.y;
    t10_2.y = _Color1.y;
    t10_2.z = _Color2.y;
    t10_2.w = _Color3.y;
    t4.y = dot(t3, t10_2);
    t10_2.x = _Color0.z;
    t10_2.y = _Color1.z;
    t10_2.z = _Color2.z;
    t10_2.w = _Color3.z;
    t4.z = dot(t3, t10_2);
    t0.xzw = t10_1.xxx * t4.xyz + t0.xxx;
    t16_5.x = vs_TEXCOORD5.x;
    t16_5.y = vs_TEXCOORD3.w;
    t16_5.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_6.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = vs_TEXCOORD5.y;
    t16_5.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = t16_1.y * t16_1.y;
    t16_5.x = t16_1.x * t16_1.x + (-t16_5.x);
    t16_7.xy = vs_TEXCOORD5.zw;
    t16_7.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_6.xyz, t16_7.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_7.x = dot(unity_SHBr, t16_2);
    t16_7.y = dot(unity_SHBg, t16_2);
    t16_7.z = dot(unity_SHBb, t16_2);
    t16_5.xyz = unity_SHC.xyz * t16_5.xxx + t16_7.xyz;
    t16_1.w = 1.0;
    t16_7.x = dot(unity_SHAr, t16_1);
    t16_7.y = dot(unity_SHAg, t16_1);
    t16_7.z = dot(unity_SHAb, t16_1);
    t16_5.xyz = t16_5.xyz + t16_7.xyz;
    t16_5.xyz = t16_5.xyz * _LightParameters.xxx;
    t16_5.xyz = log2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_5.xyz = exp2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_32 = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_32 = t16_32 + t16_32;
    t16_7.xyz = t16_1.xyz * (-vec3(t16_32)) + vs_TEXCOORD3.xyz;
    t16_5.w = dot(t16_7.xyz, (-t16_1.xyz));
    t16_7.x = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_7.x = max(t16_7.x, 0.0);
    t16_5 = max(t16_5, vec4(0.0, 0.0, 0.0, 0.25));
    t16_32 = t16_5.w * _Glossiness.w;
    t16_32 = t16_32 * _MatGlossiness;
    t16_32 = t16_32 * 10.0;
    t16_16.xyz = vec3(t16_32) * _Glossiness.xyz;
    t16_16.xyz = t16_5.xyz * t16_16.xyz;
    t16_5.xyz = t0.xzw * t16_5.xyz;
    t16_16.xyz = vec3(t9) * t16_16.xyz;
    t16_32 = (-t9) + 1.0;
    t16_5.xyz = vec3(t16_32) * t16_5.xyz;
    t16_16.xyz = t0.xzw * t16_16.xyz;
    t10_6.x = (-_LightDir.w) + 1.0;
    t16_32 = t16_7.x * _LightDir.w + t10_6.x;
    t16_7.x = t16_32 * 1.5;
    t16_7.x = min(t16_7.x, 1.0);
    t16_8.xyz = vec3(t16_32) + (-_CharacterShadowColor.xyz);
    t16_32 = t16_32 * _LightParameters.y;
    t16_8.xyz = t16_7.xxx * t16_8.xyz + _CharacterShadowColor.xyz;
    t16_5.xyz = t16_5.xyz * t16_8.xyz + t16_16.xyz;
    vec3 txVec29 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_7.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec29, 0.0);
    t16_16.x = (-_LightShadowData.x) + 1.0;
    t16_7.x = t16_7.x * t16_16.x + _LightShadowData.x;
    t16_7.x = t16_7.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_7.x = max(t16_7.x, 0.0);
    t16_5.xyz = t16_5.xyz * t16_7.xxx;
    t16_7.x = dot(t16_5.xyz, vec3(1.0, 1.0, 1.0));
    t16_7.xyz = t16_7.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_5.xyz);
    t16_5.xyz = vec3(t16_32) * t16_7.xyz + t16_5.xyz;
    t10_0 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_32 = t10_0 * 0.9375 + 0.03125;
    t16_32 = (-t16_32) + _NoiseGlow.w;
    t16_32 = -abs(t16_32) * 32.0 + 1.0;
    t16_32 = max(t16_32, 0.0);
    t16_5.xyz = t16_5.xyz + vec3(t16_32);
    t16_5.xyz = t16_5.xyz * _NoiseGlow.xyz;
    t16_5.xyz = t16_5.xyz * vs_TEXCOORD2.xxx;
    t16_5.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_5.xyz;
    SV_Target0.xyz = t16_5.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _DetailA;
uniform sampler2D _DetailB;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _Color2;
uniform lowp vec4 _Color3;
uniform highp vec4 _DetailScale;
mediump vec3 tmpvar_1;
mediump vec2 tmpvar_2;
mediump float tmpvar_3;
mediump vec3 tmpvar_4;
mediump vec3 tmpvar_5;
mediump float tmpvar_6;
mediump float tmpvar_7;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_8;
  mediump vec3 lit_color_9;
  tmpvar_1 = -(_LightDir.xyz);
  tmpvar_2 = xlv_TEXCOORD0.xy;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  highp float _selector_11;
  _selector_11 = tmpvar_10.z;
  highp float ref_12;
  highp vec4 result_13;
  result_13.x = float((_selector_11 >= 0.75));
  ref_12 = (_selector_11 - (result_13.x * 2.0));
  result_13.y = float((ref_12 >= 0.5));
  ref_12 = (ref_12 - (result_13.y * 2.0));
  result_13.z = float((ref_12 >= 0.25));
  ref_12 = (ref_12 - (result_13.z * 2.0));
  result_13.w = float((ref_12 >= 0.0));
  highp vec2 _uv_14;
  _uv_14 = tmpvar_2;
  highp vec2 result_15;
  highp vec4 d1_16;
  highp vec4 d0_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_DetailScale * _uv_14.xyxy);
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_DetailA, tmpvar_18.xy);
  d0_17 = tmpvar_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_DetailB, tmpvar_18.zw);
  d1_16 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.zw = vec2(0.0, 0.0);
  tmpvar_21.x = d0_17.x;
  tmpvar_21.y = d1_16.x;
  result_15.x = dot ((tmpvar_21 - vec4(0.5, 0.5, 0.0, 0.0)), result_13.wxxx);
  highp vec4 tmpvar_22;
  tmpvar_22.zw = vec2(0.0, 0.0);
  tmpvar_22.x = d0_17.y;
  tmpvar_22.y = d1_16.y;
  result_15.y = dot ((tmpvar_22 - vec4(0.5, 0.5, 0.0, 0.0)), result_13.wxxx);
  highp vec4 tmpvar_23;
  highp mat4 tmpvar_24;
  lowp mat4 tmpvar_25;
  tmpvar_25[0].x = _Color0.x;
  tmpvar_25[0].y = _Color1.x;
  tmpvar_25[0].z = _Color2.x;
  tmpvar_25[0].w = _Color3.x;
  tmpvar_25[1].x = _Color0.y;
  tmpvar_25[1].y = _Color1.y;
  tmpvar_25[1].z = _Color2.y;
  tmpvar_25[1].w = _Color3.y;
  tmpvar_25[2].x = _Color0.z;
  tmpvar_25[2].y = _Color1.z;
  tmpvar_25[2].z = _Color2.z;
  tmpvar_25[2].w = _Color3.z;
  tmpvar_25[3].x = _Color0.w;
  tmpvar_25[3].y = _Color1.w;
  tmpvar_25[3].z = _Color2.w;
  tmpvar_25[3].w = _Color3.w;
  tmpvar_24 = tmpvar_25;
  tmpvar_23 = (result_13 * tmpvar_24);
  tmpvar_4 = ((tmpvar_10.xxx * tmpvar_23.xyz) + result_15.xxx);
  tmpvar_3 = (result_15.y + tmpvar_10.y);
  mediump vec3 tangentNormal_26;
  mediump vec3 tmpvar_27;
  tmpvar_27.x = xlv_TEXCOORD3.w;
  tmpvar_27.y = xlv_TEXCOORD4.w;
  tmpvar_27.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_28;
  mediump vec3 tmpvar_29;
  tmpvar_28 = xlv_TEXCOORD5.xyz;
  tmpvar_29 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_30;
  tmpvar_30[0].x = tmpvar_28.x;
  tmpvar_30[0].y = tmpvar_27.x;
  tmpvar_30[0].z = tmpvar_29.x;
  tmpvar_30[1].x = tmpvar_28.y;
  tmpvar_30[1].y = tmpvar_27.y;
  tmpvar_30[1].z = tmpvar_29.y;
  tmpvar_30[2].x = tmpvar_28.z;
  tmpvar_30[2].y = tmpvar_27.z;
  tmpvar_30[2].z = tmpvar_29.z;
  lowp vec3 tmpvar_31;
  tmpvar_31 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_26 = tmpvar_31;
  tmpvar_5 = (tangentNormal_26 * tmpvar_30);
  mediump vec4 tmpvar_32;
  tmpvar_32.w = 1.0;
  tmpvar_32.xyz = tmpvar_5;
  mediump vec3 x2_33;
  mediump vec3 x1_34;
  x1_34.x = dot (unity_SHAr, tmpvar_32);
  x1_34.y = dot (unity_SHAg, tmpvar_32);
  x1_34.z = dot (unity_SHAb, tmpvar_32);
  mediump vec4 tmpvar_35;
  tmpvar_35 = (tmpvar_5.xyzz * tmpvar_5.yzzx);
  x2_33.x = dot (unity_SHBr, tmpvar_35);
  x2_33.y = dot (unity_SHBg, tmpvar_35);
  x2_33.z = dot (unity_SHBb, tmpvar_35);
  mediump vec3 tmpvar_36;
  tmpvar_36 = max (((1.055 * 
    pow ((((x2_33 + 
      (unity_SHC.xyz * ((tmpvar_5.x * tmpvar_5.x) - (tmpvar_5.y * tmpvar_5.y)))
    ) + x1_34) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_7 = (1.0 - dot (tmpvar_5, xlv_TEXCOORD3.xyz));
  tmpvar_6 = ((max (
    dot (tmpvar_5, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_9 = (((
    (tmpvar_36 * tmpvar_4)
   * 
    (1.0 - tmpvar_3)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_6), vec3(
    min (1.0, (tmpvar_6 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (tmpvar_5, xlv_TEXCOORD3.xyz) * tmpvar_5)
      )), -(tmpvar_5)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_36)
   * tmpvar_3) * tmpvar_4));
  mediump vec3 tmpvar_37;
  tmpvar_37 = mix (lit_color_9, vec3((dot (lit_color_9, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_6 * _LightParameters.y)));
  c_8.xyz = tmpvar_37;
  c_8.w = _RimColor.w;
  lowp vec4 _color_38;
  _color_38.w = c_8.w;
  _color_38.xyz = (c_8.xyz + (tmpvar_7 * _RimColor.xyz));
  c_8.w = _color_38.w;
  c_8.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_38.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_8;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _DetailA;
uniform lowp sampler2D _DetailB;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec4 t0;
highp vec4 t1;
mediump vec4 t16_1;
lowp vec3 t10_1;
highp float t2;
mediump vec4 t16_2;
lowp vec4 t10_2;
highp vec4 t3;
highp vec3 t4;
mediump vec3 t16_5;
lowp vec3 t10_6;
mediump vec4 t16_7;
mediump vec3 t16_8;
highp float t9;
mediump float t16_16;
mediump vec3 t16_17;
highp float t19;
bvec2 tb19;
bool tb28;
mediump float t16_32;
void main()
{
    t0 = vs_TEXCOORD0.xyxy * _DetailScale;
    t1.xz = texture(_DetailA, t0.xy).xy;
    t1.yw = texture(_DetailB, t0.zw).xy;
    t0 = t1 + vec4(-0.5, -0.5, -0.5, -0.5);
    t10_1.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    tb28 = t10_1.z>=0.75;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.x = tb28 ? 1.0 : float(0.0);
    t19 = t10_1.z + t2;
    tb28 = t19>=0.5;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.y = tb28 ? 1.0 : float(0.0);
    t19 = t19 + t2;
    tb19.y = t19>=0.25;
    t2 = (tb19.y) ? -2.0 : -0.0;
    t19 = t19 + t2;
    tb19.x = t19>=0.0;
    t3.zw = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb19.yx));
    t0.x = dot(t0.yx, t3.xw);
    t9 = dot(t0.wz, t3.xw);
    t9 = t10_1.y + t9;
    t10_2.x = _Color0.x;
    t10_2.y = _Color1.x;
    t10_2.z = _Color2.x;
    t10_2.w = _Color3.x;
    t4.x = dot(t3, t10_2);
    t10_2.x = _Color0.y;
    t10_2.y = _Color1.y;
    t10_2.z = _Color2.y;
    t10_2.w = _Color3.y;
    t4.y = dot(t3, t10_2);
    t10_2.x = _Color0.z;
    t10_2.y = _Color1.z;
    t10_2.z = _Color2.z;
    t10_2.w = _Color3.z;
    t4.z = dot(t3, t10_2);
    t0.xzw = t10_1.xxx * t4.xyz + t0.xxx;
    t16_5.x = vs_TEXCOORD5.x;
    t16_5.y = vs_TEXCOORD3.w;
    t16_5.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_6.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = vs_TEXCOORD5.y;
    t16_5.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = t16_1.y * t16_1.y;
    t16_5.x = t16_1.x * t16_1.x + (-t16_5.x);
    t16_7.xy = vs_TEXCOORD5.zw;
    t16_7.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_6.xyz, t16_7.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_7.x = dot(unity_SHBr, t16_2);
    t16_7.y = dot(unity_SHBg, t16_2);
    t16_7.z = dot(unity_SHBb, t16_2);
    t16_5.xyz = unity_SHC.xyz * t16_5.xxx + t16_7.xyz;
    t16_1.w = 1.0;
    t16_7.x = dot(unity_SHAr, t16_1);
    t16_7.y = dot(unity_SHAg, t16_1);
    t16_7.z = dot(unity_SHAb, t16_1);
    t16_5.xyz = t16_5.xyz + t16_7.xyz;
    t16_5.xyz = t16_5.xyz * _LightParameters.xxx;
    t16_5.xyz = log2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_5.xyz = exp2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_5.xyz = max(t16_5.xyz, vec3(0.0, 0.0, 0.0));
    t16_32 = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_7.x = t16_32 + t16_32;
    t16_32 = (-t16_32) + 1.0;
    t16_7.xyz = t16_1.xyz * (-t16_7.xxx) + vs_TEXCOORD3.xyz;
    t16_7.x = dot(t16_7.xyz, (-t16_1.xyz));
    t16_7.y = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_7.xy = max(t16_7.xy, vec2(0.25, 0.0));
    t16_7.x = t16_7.x * _Glossiness.w;
    t16_7.x = t16_7.x * _MatGlossiness;
    t16_7.x = t16_7.x * 10.0;
    t16_7.xzw = t16_7.xxx * _Glossiness.xyz;
    t16_7.xzw = t16_5.xyz * t16_7.xzw;
    t16_5.xyz = t0.xzw * t16_5.xyz;
    t16_7.xzw = vec3(t9) * t16_7.xzw;
    t16_8.x = (-t9) + 1.0;
    t16_5.xyz = t16_5.xyz * t16_8.xxx;
    t16_7.xzw = t0.xzw * t16_7.xzw;
    t10_6.x = (-_LightDir.w) + 1.0;
    t16_16 = t16_7.y * _LightDir.w + t10_6.x;
    t16_8.x = t16_16 * 1.5;
    t16_8.x = min(t16_8.x, 1.0);
    t16_17.xyz = vec3(t16_16) + (-_CharacterShadowColor.xyz);
    t16_16 = t16_16 * _LightParameters.y;
    t16_8.xyz = t16_8.xxx * t16_17.xyz + _CharacterShadowColor.xyz;
    t16_5.xyz = t16_5.xyz * t16_8.xyz + t16_7.xzw;
    t16_7.x = dot(t16_5.xyz, vec3(1.0, 1.0, 1.0));
    t16_7.xzw = t16_7.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_5.xyz);
    t16_5.xyz = vec3(t16_16) * t16_7.xzw + t16_5.xyz;
    t16_5.xyz = vec3(t16_32) * _RimColor.xyz + t16_5.xyz;
    t16_5.xyz = t16_5.xyz * vs_TEXCOORD2.xxx;
    t16_5.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_5.xyz;
    SV_Target0.xyz = t16_5.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _DetailA;
uniform sampler2D _DetailB;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _Color2;
uniform lowp vec4 _Color3;
uniform highp vec4 _DetailScale;
mediump vec3 tmpvar_1;
mediump vec2 tmpvar_2;
mediump float tmpvar_3;
mediump vec3 tmpvar_4;
mediump vec3 tmpvar_5;
mediump float tmpvar_6;
mediump float tmpvar_7;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_8;
  mediump vec3 lit_color_9;
  tmpvar_1 = -(_LightDir.xyz);
  tmpvar_2 = xlv_TEXCOORD0.xy;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  highp float _selector_11;
  _selector_11 = tmpvar_10.z;
  highp float ref_12;
  highp vec4 result_13;
  result_13.x = float((_selector_11 >= 0.75));
  ref_12 = (_selector_11 - (result_13.x * 2.0));
  result_13.y = float((ref_12 >= 0.5));
  ref_12 = (ref_12 - (result_13.y * 2.0));
  result_13.z = float((ref_12 >= 0.25));
  ref_12 = (ref_12 - (result_13.z * 2.0));
  result_13.w = float((ref_12 >= 0.0));
  highp vec2 _uv_14;
  _uv_14 = tmpvar_2;
  highp vec2 result_15;
  highp vec4 d1_16;
  highp vec4 d0_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_DetailScale * _uv_14.xyxy);
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_DetailA, tmpvar_18.xy);
  d0_17 = tmpvar_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_DetailB, tmpvar_18.zw);
  d1_16 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.zw = vec2(0.0, 0.0);
  tmpvar_21.x = d0_17.x;
  tmpvar_21.y = d1_16.x;
  result_15.x = dot ((tmpvar_21 - vec4(0.5, 0.5, 0.0, 0.0)), result_13.wxxx);
  highp vec4 tmpvar_22;
  tmpvar_22.zw = vec2(0.0, 0.0);
  tmpvar_22.x = d0_17.y;
  tmpvar_22.y = d1_16.y;
  result_15.y = dot ((tmpvar_22 - vec4(0.5, 0.5, 0.0, 0.0)), result_13.wxxx);
  highp vec4 tmpvar_23;
  highp mat4 tmpvar_24;
  lowp mat4 tmpvar_25;
  tmpvar_25[0].x = _Color0.x;
  tmpvar_25[0].y = _Color1.x;
  tmpvar_25[0].z = _Color2.x;
  tmpvar_25[0].w = _Color3.x;
  tmpvar_25[1].x = _Color0.y;
  tmpvar_25[1].y = _Color1.y;
  tmpvar_25[1].z = _Color2.y;
  tmpvar_25[1].w = _Color3.y;
  tmpvar_25[2].x = _Color0.z;
  tmpvar_25[2].y = _Color1.z;
  tmpvar_25[2].z = _Color2.z;
  tmpvar_25[2].w = _Color3.z;
  tmpvar_25[3].x = _Color0.w;
  tmpvar_25[3].y = _Color1.w;
  tmpvar_25[3].z = _Color2.w;
  tmpvar_25[3].w = _Color3.w;
  tmpvar_24 = tmpvar_25;
  tmpvar_23 = (result_13 * tmpvar_24);
  tmpvar_4 = ((tmpvar_10.xxx * tmpvar_23.xyz) + result_15.xxx);
  tmpvar_3 = (result_15.y + tmpvar_10.y);
  mediump vec3 tangentNormal_26;
  mediump vec3 tmpvar_27;
  tmpvar_27.x = xlv_TEXCOORD3.w;
  tmpvar_27.y = xlv_TEXCOORD4.w;
  tmpvar_27.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_28;
  mediump vec3 tmpvar_29;
  tmpvar_28 = xlv_TEXCOORD5.xyz;
  tmpvar_29 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_30;
  tmpvar_30[0].x = tmpvar_28.x;
  tmpvar_30[0].y = tmpvar_27.x;
  tmpvar_30[0].z = tmpvar_29.x;
  tmpvar_30[1].x = tmpvar_28.y;
  tmpvar_30[1].y = tmpvar_27.y;
  tmpvar_30[1].z = tmpvar_29.y;
  tmpvar_30[2].x = tmpvar_28.z;
  tmpvar_30[2].y = tmpvar_27.z;
  tmpvar_30[2].z = tmpvar_29.z;
  lowp vec3 tmpvar_31;
  tmpvar_31 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_26 = tmpvar_31;
  tmpvar_5 = (tangentNormal_26 * tmpvar_30);
  mediump vec4 tmpvar_32;
  tmpvar_32.w = 1.0;
  tmpvar_32.xyz = tmpvar_5;
  mediump vec3 x2_33;
  mediump vec3 x1_34;
  x1_34.x = dot (unity_SHAr, tmpvar_32);
  x1_34.y = dot (unity_SHAg, tmpvar_32);
  x1_34.z = dot (unity_SHAb, tmpvar_32);
  mediump vec4 tmpvar_35;
  tmpvar_35 = (tmpvar_5.xyzz * tmpvar_5.yzzx);
  x2_33.x = dot (unity_SHBr, tmpvar_35);
  x2_33.y = dot (unity_SHBg, tmpvar_35);
  x2_33.z = dot (unity_SHBb, tmpvar_35);
  mediump vec3 tmpvar_36;
  tmpvar_36 = max (((1.055 * 
    pow ((((x2_33 + 
      (unity_SHC.xyz * ((tmpvar_5.x * tmpvar_5.x) - (tmpvar_5.y * tmpvar_5.y)))
    ) + x1_34) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_7 = (1.0 - dot (tmpvar_5, xlv_TEXCOORD3.xyz));
  tmpvar_6 = ((max (
    dot (tmpvar_5, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_9 = (((
    (tmpvar_36 * tmpvar_4)
   * 
    (1.0 - tmpvar_3)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_6), vec3(
    min (1.0, (tmpvar_6 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (tmpvar_5, xlv_TEXCOORD3.xyz) * tmpvar_5)
      )), -(tmpvar_5)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_36)
   * tmpvar_3) * tmpvar_4));
  mediump vec3 tmpvar_37;
  tmpvar_37 = mix (lit_color_9, vec3((dot (lit_color_9, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_6 * _LightParameters.y)));
  c_8.xyz = tmpvar_37;
  c_8.w = _RimColor.w;
  lowp vec4 _color_38;
  _color_38.w = c_8.w;
  _color_38.xyz = (c_8.xyz + (tmpvar_7 * _RimColor.xyz));
  c_8.w = _color_38.w;
  c_8.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_38.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_8;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _DetailA;
uniform lowp sampler2D _DetailB;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec4 t0;
highp vec4 t1;
mediump vec4 t16_1;
lowp vec3 t10_1;
highp float t2;
mediump vec4 t16_2;
lowp vec4 t10_2;
highp vec4 t3;
highp vec3 t4;
mediump vec3 t16_5;
lowp vec3 t10_6;
mediump vec4 t16_7;
mediump vec3 t16_8;
highp float t9;
mediump float t16_16;
mediump vec3 t16_17;
highp float t19;
bvec2 tb19;
bool tb28;
mediump float t16_32;
void main()
{
    t0 = vs_TEXCOORD0.xyxy * _DetailScale;
    t1.xz = texture(_DetailA, t0.xy).xy;
    t1.yw = texture(_DetailB, t0.zw).xy;
    t0 = t1 + vec4(-0.5, -0.5, -0.5, -0.5);
    t10_1.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    tb28 = t10_1.z>=0.75;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.x = tb28 ? 1.0 : float(0.0);
    t19 = t10_1.z + t2;
    tb28 = t19>=0.5;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.y = tb28 ? 1.0 : float(0.0);
    t19 = t19 + t2;
    tb19.y = t19>=0.25;
    t2 = (tb19.y) ? -2.0 : -0.0;
    t19 = t19 + t2;
    tb19.x = t19>=0.0;
    t3.zw = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb19.yx));
    t0.x = dot(t0.yx, t3.xw);
    t9 = dot(t0.wz, t3.xw);
    t9 = t10_1.y + t9;
    t10_2.x = _Color0.x;
    t10_2.y = _Color1.x;
    t10_2.z = _Color2.x;
    t10_2.w = _Color3.x;
    t4.x = dot(t3, t10_2);
    t10_2.x = _Color0.y;
    t10_2.y = _Color1.y;
    t10_2.z = _Color2.y;
    t10_2.w = _Color3.y;
    t4.y = dot(t3, t10_2);
    t10_2.x = _Color0.z;
    t10_2.y = _Color1.z;
    t10_2.z = _Color2.z;
    t10_2.w = _Color3.z;
    t4.z = dot(t3, t10_2);
    t0.xzw = t10_1.xxx * t4.xyz + t0.xxx;
    t16_5.x = vs_TEXCOORD5.x;
    t16_5.y = vs_TEXCOORD3.w;
    t16_5.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_6.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = vs_TEXCOORD5.y;
    t16_5.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = t16_1.y * t16_1.y;
    t16_5.x = t16_1.x * t16_1.x + (-t16_5.x);
    t16_7.xy = vs_TEXCOORD5.zw;
    t16_7.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_6.xyz, t16_7.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_7.x = dot(unity_SHBr, t16_2);
    t16_7.y = dot(unity_SHBg, t16_2);
    t16_7.z = dot(unity_SHBb, t16_2);
    t16_5.xyz = unity_SHC.xyz * t16_5.xxx + t16_7.xyz;
    t16_1.w = 1.0;
    t16_7.x = dot(unity_SHAr, t16_1);
    t16_7.y = dot(unity_SHAg, t16_1);
    t16_7.z = dot(unity_SHAb, t16_1);
    t16_5.xyz = t16_5.xyz + t16_7.xyz;
    t16_5.xyz = t16_5.xyz * _LightParameters.xxx;
    t16_5.xyz = log2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_5.xyz = exp2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_5.xyz = max(t16_5.xyz, vec3(0.0, 0.0, 0.0));
    t16_32 = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_7.x = t16_32 + t16_32;
    t16_32 = (-t16_32) + 1.0;
    t16_7.xyz = t16_1.xyz * (-t16_7.xxx) + vs_TEXCOORD3.xyz;
    t16_7.x = dot(t16_7.xyz, (-t16_1.xyz));
    t16_7.y = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_7.xy = max(t16_7.xy, vec2(0.25, 0.0));
    t16_7.x = t16_7.x * _Glossiness.w;
    t16_7.x = t16_7.x * _MatGlossiness;
    t16_7.x = t16_7.x * 10.0;
    t16_7.xzw = t16_7.xxx * _Glossiness.xyz;
    t16_7.xzw = t16_5.xyz * t16_7.xzw;
    t16_5.xyz = t0.xzw * t16_5.xyz;
    t16_7.xzw = vec3(t9) * t16_7.xzw;
    t16_8.x = (-t9) + 1.0;
    t16_5.xyz = t16_5.xyz * t16_8.xxx;
    t16_7.xzw = t0.xzw * t16_7.xzw;
    t10_6.x = (-_LightDir.w) + 1.0;
    t16_16 = t16_7.y * _LightDir.w + t10_6.x;
    t16_8.x = t16_16 * 1.5;
    t16_8.x = min(t16_8.x, 1.0);
    t16_17.xyz = vec3(t16_16) + (-_CharacterShadowColor.xyz);
    t16_16 = t16_16 * _LightParameters.y;
    t16_8.xyz = t16_8.xxx * t16_17.xyz + _CharacterShadowColor.xyz;
    t16_5.xyz = t16_5.xyz * t16_8.xyz + t16_7.xzw;
    t16_7.x = dot(t16_5.xyz, vec3(1.0, 1.0, 1.0));
    t16_7.xzw = t16_7.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_5.xyz);
    t16_5.xyz = vec3(t16_16) * t16_7.xzw + t16_5.xyz;
    t16_5.xyz = vec3(t16_32) * _RimColor.xyz + t16_5.xyz;
    t16_5.xyz = t16_5.xyz * vs_TEXCOORD2.xxx;
    t16_5.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_5.xyz;
    SV_Target0.xyz = t16_5.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_HIGHLIGHT" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _DetailA;
uniform sampler2D _DetailB;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _Color2;
uniform lowp vec4 _Color3;
uniform highp vec4 _DetailScale;
mediump vec3 tmpvar_1;
mediump vec2 tmpvar_2;
mediump float tmpvar_3;
mediump vec3 tmpvar_4;
mediump vec3 tmpvar_5;
mediump float tmpvar_6;
mediump float tmpvar_7;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_8;
  mediump vec3 lit_color_9;
  tmpvar_1 = -(_LightDir.xyz);
  tmpvar_2 = xlv_TEXCOORD0.xy;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  highp float _selector_11;
  _selector_11 = tmpvar_10.z;
  highp float ref_12;
  highp vec4 result_13;
  result_13.x = float((_selector_11 >= 0.75));
  ref_12 = (_selector_11 - (result_13.x * 2.0));
  result_13.y = float((ref_12 >= 0.5));
  ref_12 = (ref_12 - (result_13.y * 2.0));
  result_13.z = float((ref_12 >= 0.25));
  ref_12 = (ref_12 - (result_13.z * 2.0));
  result_13.w = float((ref_12 >= 0.0));
  highp vec2 _uv_14;
  _uv_14 = tmpvar_2;
  highp vec2 result_15;
  highp vec4 d1_16;
  highp vec4 d0_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_DetailScale * _uv_14.xyxy);
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_DetailA, tmpvar_18.xy);
  d0_17 = tmpvar_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_DetailB, tmpvar_18.zw);
  d1_16 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.zw = vec2(0.0, 0.0);
  tmpvar_21.x = d0_17.x;
  tmpvar_21.y = d1_16.x;
  result_15.x = dot ((tmpvar_21 - vec4(0.5, 0.5, 0.0, 0.0)), result_13.wxxx);
  highp vec4 tmpvar_22;
  tmpvar_22.zw = vec2(0.0, 0.0);
  tmpvar_22.x = d0_17.y;
  tmpvar_22.y = d1_16.y;
  result_15.y = dot ((tmpvar_22 - vec4(0.5, 0.5, 0.0, 0.0)), result_13.wxxx);
  highp vec4 tmpvar_23;
  highp mat4 tmpvar_24;
  lowp mat4 tmpvar_25;
  tmpvar_25[0].x = _Color0.x;
  tmpvar_25[0].y = _Color1.x;
  tmpvar_25[0].z = _Color2.x;
  tmpvar_25[0].w = _Color3.x;
  tmpvar_25[1].x = _Color0.y;
  tmpvar_25[1].y = _Color1.y;
  tmpvar_25[1].z = _Color2.y;
  tmpvar_25[1].w = _Color3.y;
  tmpvar_25[2].x = _Color0.z;
  tmpvar_25[2].y = _Color1.z;
  tmpvar_25[2].z = _Color2.z;
  tmpvar_25[2].w = _Color3.z;
  tmpvar_25[3].x = _Color0.w;
  tmpvar_25[3].y = _Color1.w;
  tmpvar_25[3].z = _Color2.w;
  tmpvar_25[3].w = _Color3.w;
  tmpvar_24 = tmpvar_25;
  tmpvar_23 = (result_13 * tmpvar_24);
  tmpvar_4 = ((tmpvar_10.xxx * tmpvar_23.xyz) + result_15.xxx);
  tmpvar_3 = (result_15.y + tmpvar_10.y);
  mediump vec3 tangentNormal_26;
  mediump vec3 tmpvar_27;
  tmpvar_27.x = xlv_TEXCOORD3.w;
  tmpvar_27.y = xlv_TEXCOORD4.w;
  tmpvar_27.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_28;
  mediump vec3 tmpvar_29;
  tmpvar_28 = xlv_TEXCOORD5.xyz;
  tmpvar_29 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_30;
  tmpvar_30[0].x = tmpvar_28.x;
  tmpvar_30[0].y = tmpvar_27.x;
  tmpvar_30[0].z = tmpvar_29.x;
  tmpvar_30[1].x = tmpvar_28.y;
  tmpvar_30[1].y = tmpvar_27.y;
  tmpvar_30[1].z = tmpvar_29.y;
  tmpvar_30[2].x = tmpvar_28.z;
  tmpvar_30[2].y = tmpvar_27.z;
  tmpvar_30[2].z = tmpvar_29.z;
  lowp vec3 tmpvar_31;
  tmpvar_31 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_26 = tmpvar_31;
  tmpvar_5 = (tangentNormal_26 * tmpvar_30);
  mediump vec4 tmpvar_32;
  tmpvar_32.w = 1.0;
  tmpvar_32.xyz = tmpvar_5;
  mediump vec3 x2_33;
  mediump vec3 x1_34;
  x1_34.x = dot (unity_SHAr, tmpvar_32);
  x1_34.y = dot (unity_SHAg, tmpvar_32);
  x1_34.z = dot (unity_SHAb, tmpvar_32);
  mediump vec4 tmpvar_35;
  tmpvar_35 = (tmpvar_5.xyzz * tmpvar_5.yzzx);
  x2_33.x = dot (unity_SHBr, tmpvar_35);
  x2_33.y = dot (unity_SHBg, tmpvar_35);
  x2_33.z = dot (unity_SHBb, tmpvar_35);
  mediump vec3 tmpvar_36;
  tmpvar_36 = max (((1.055 * 
    pow ((((x2_33 + 
      (unity_SHC.xyz * ((tmpvar_5.x * tmpvar_5.x) - (tmpvar_5.y * tmpvar_5.y)))
    ) + x1_34) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_7 = (1.0 - dot (tmpvar_5, xlv_TEXCOORD3.xyz));
  tmpvar_6 = ((max (
    dot (tmpvar_5, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_9 = (((
    (tmpvar_36 * tmpvar_4)
   * 
    (1.0 - tmpvar_3)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_6), vec3(
    min (1.0, (tmpvar_6 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (tmpvar_5, xlv_TEXCOORD3.xyz) * tmpvar_5)
      )), -(tmpvar_5)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_36)
   * tmpvar_3) * tmpvar_4));
  mediump vec3 atten_37;
  lowp float tmpvar_38;
  highp float tmpvar_39;
  tmpvar_39 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_38 = tmpvar_39;
  lowp vec3 tmpvar_40;
  tmpvar_40 = vec3(tmpvar_38);
  atten_37 = tmpvar_40;
  lit_color_9 = (lit_color_9 * max ((_Shadows.x + 
    (atten_37 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_41;
  tmpvar_41 = mix (lit_color_9, vec3((dot (lit_color_9, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_6 * _LightParameters.y)));
  c_8.xyz = tmpvar_41;
  c_8.w = _RimColor.w;
  lowp vec4 _color_42;
  _color_42.w = c_8.w;
  _color_42.xyz = (c_8.xyz + (tmpvar_7 * _RimColor.xyz));
  c_8.w = _color_42.w;
  c_8.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_42.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_8;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _DetailA;
uniform sampler2D _DetailB;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _Color2;
uniform lowp vec4 _Color3;
uniform highp vec4 _DetailScale;
mediump vec3 tmpvar_1;
mediump vec2 tmpvar_2;
mediump float tmpvar_3;
mediump vec3 tmpvar_4;
mediump vec3 tmpvar_5;
mediump float tmpvar_6;
mediump float tmpvar_7;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_8;
  mediump vec3 lit_color_9;
  tmpvar_1 = -(_LightDir.xyz);
  tmpvar_2 = xlv_TEXCOORD0.xy;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  highp float _selector_11;
  _selector_11 = tmpvar_10.z;
  highp float ref_12;
  highp vec4 result_13;
  result_13.x = float((_selector_11 >= 0.75));
  ref_12 = (_selector_11 - (result_13.x * 2.0));
  result_13.y = float((ref_12 >= 0.5));
  ref_12 = (ref_12 - (result_13.y * 2.0));
  result_13.z = float((ref_12 >= 0.25));
  ref_12 = (ref_12 - (result_13.z * 2.0));
  result_13.w = float((ref_12 >= 0.0));
  highp vec2 _uv_14;
  _uv_14 = tmpvar_2;
  highp vec2 result_15;
  highp vec4 d1_16;
  highp vec4 d0_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_DetailScale * _uv_14.xyxy);
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_DetailA, tmpvar_18.xy);
  d0_17 = tmpvar_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_DetailB, tmpvar_18.zw);
  d1_16 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.zw = vec2(0.0, 0.0);
  tmpvar_21.x = d0_17.x;
  tmpvar_21.y = d1_16.x;
  result_15.x = dot ((tmpvar_21 - vec4(0.5, 0.5, 0.0, 0.0)), result_13.wxxx);
  highp vec4 tmpvar_22;
  tmpvar_22.zw = vec2(0.0, 0.0);
  tmpvar_22.x = d0_17.y;
  tmpvar_22.y = d1_16.y;
  result_15.y = dot ((tmpvar_22 - vec4(0.5, 0.5, 0.0, 0.0)), result_13.wxxx);
  highp vec4 tmpvar_23;
  highp mat4 tmpvar_24;
  lowp mat4 tmpvar_25;
  tmpvar_25[0].x = _Color0.x;
  tmpvar_25[0].y = _Color1.x;
  tmpvar_25[0].z = _Color2.x;
  tmpvar_25[0].w = _Color3.x;
  tmpvar_25[1].x = _Color0.y;
  tmpvar_25[1].y = _Color1.y;
  tmpvar_25[1].z = _Color2.y;
  tmpvar_25[1].w = _Color3.y;
  tmpvar_25[2].x = _Color0.z;
  tmpvar_25[2].y = _Color1.z;
  tmpvar_25[2].z = _Color2.z;
  tmpvar_25[2].w = _Color3.z;
  tmpvar_25[3].x = _Color0.w;
  tmpvar_25[3].y = _Color1.w;
  tmpvar_25[3].z = _Color2.w;
  tmpvar_25[3].w = _Color3.w;
  tmpvar_24 = tmpvar_25;
  tmpvar_23 = (result_13 * tmpvar_24);
  tmpvar_4 = ((tmpvar_10.xxx * tmpvar_23.xyz) + result_15.xxx);
  tmpvar_3 = (result_15.y + tmpvar_10.y);
  mediump vec3 tangentNormal_26;
  mediump vec3 tmpvar_27;
  tmpvar_27.x = xlv_TEXCOORD3.w;
  tmpvar_27.y = xlv_TEXCOORD4.w;
  tmpvar_27.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_28;
  mediump vec3 tmpvar_29;
  tmpvar_28 = xlv_TEXCOORD5.xyz;
  tmpvar_29 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_30;
  tmpvar_30[0].x = tmpvar_28.x;
  tmpvar_30[0].y = tmpvar_27.x;
  tmpvar_30[0].z = tmpvar_29.x;
  tmpvar_30[1].x = tmpvar_28.y;
  tmpvar_30[1].y = tmpvar_27.y;
  tmpvar_30[1].z = tmpvar_29.y;
  tmpvar_30[2].x = tmpvar_28.z;
  tmpvar_30[2].y = tmpvar_27.z;
  tmpvar_30[2].z = tmpvar_29.z;
  lowp vec3 tmpvar_31;
  tmpvar_31 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_26 = tmpvar_31;
  tmpvar_5 = (tangentNormal_26 * tmpvar_30);
  mediump vec4 tmpvar_32;
  tmpvar_32.w = 1.0;
  tmpvar_32.xyz = tmpvar_5;
  mediump vec3 x2_33;
  mediump vec3 x1_34;
  x1_34.x = dot (unity_SHAr, tmpvar_32);
  x1_34.y = dot (unity_SHAg, tmpvar_32);
  x1_34.z = dot (unity_SHAb, tmpvar_32);
  mediump vec4 tmpvar_35;
  tmpvar_35 = (tmpvar_5.xyzz * tmpvar_5.yzzx);
  x2_33.x = dot (unity_SHBr, tmpvar_35);
  x2_33.y = dot (unity_SHBg, tmpvar_35);
  x2_33.z = dot (unity_SHBb, tmpvar_35);
  mediump vec3 tmpvar_36;
  tmpvar_36 = max (((1.055 * 
    pow ((((x2_33 + 
      (unity_SHC.xyz * ((tmpvar_5.x * tmpvar_5.x) - (tmpvar_5.y * tmpvar_5.y)))
    ) + x1_34) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_7 = (1.0 - dot (tmpvar_5, xlv_TEXCOORD3.xyz));
  tmpvar_6 = ((max (
    dot (tmpvar_5, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_9 = (((
    (tmpvar_36 * tmpvar_4)
   * 
    (1.0 - tmpvar_3)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_6), vec3(
    min (1.0, (tmpvar_6 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (tmpvar_5, xlv_TEXCOORD3.xyz) * tmpvar_5)
      )), -(tmpvar_5)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_36)
   * tmpvar_3) * tmpvar_4));
  mediump vec3 atten_37;
  lowp float shadow_38;
  shadow_38 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_39;
  tmpvar_39 = vec3(shadow_38);
  atten_37 = tmpvar_39;
  lit_color_9 = (lit_color_9 * max ((_Shadows.x + 
    (atten_37 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_40;
  tmpvar_40 = mix (lit_color_9, vec3((dot (lit_color_9, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_6 * _LightParameters.y)));
  c_8.xyz = tmpvar_40;
  c_8.w = _RimColor.w;
  lowp vec4 _color_41;
  _color_41.w = c_8.w;
  _color_41.xyz = (c_8.xyz + (tmpvar_7 * _RimColor.xyz));
  c_8.w = _color_41.w;
  c_8.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_41.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_8;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1 = t0.yyyy * unity_World2Shadow[0][1];
    t1 = unity_World2Shadow[0][0] * t0.xxxx + t1;
    t1 = unity_World2Shadow[0][2] * t0.zzzz + t1;
    t0 = unity_World2Shadow[0][3] * t0.wwww + t1;
    vs_TEXCOORD8 = t0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
};
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	vec4 unity_WorldTransformParams;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _Color2;
uniform 	lowp vec4 _Color3;
uniform 	vec4 _DetailScale;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _DetailA;
uniform lowp sampler2D _DetailB;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec4 t0;
highp vec4 t1;
mediump vec4 t16_1;
lowp vec3 t10_1;
highp float t2;
mediump vec4 t16_2;
lowp vec4 t10_2;
highp vec4 t3;
highp vec3 t4;
mediump vec3 t16_5;
lowp vec3 t10_6;
mediump vec4 t16_7;
mediump vec3 t16_8;
highp float t9;
mediump float t16_16;
mediump vec3 t16_17;
highp float t19;
bvec2 tb19;
mediump float t16_25;
bool tb28;
mediump float t16_32;
void main()
{
    t0 = vs_TEXCOORD0.xyxy * _DetailScale;
    t1.xz = texture(_DetailA, t0.xy).xy;
    t1.yw = texture(_DetailB, t0.zw).xy;
    t0 = t1 + vec4(-0.5, -0.5, -0.5, -0.5);
    t10_1.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    tb28 = t10_1.z>=0.75;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.x = tb28 ? 1.0 : float(0.0);
    t19 = t10_1.z + t2;
    tb28 = t19>=0.5;
    t2 = (tb28) ? -2.0 : -0.0;
    t3.y = tb28 ? 1.0 : float(0.0);
    t19 = t19 + t2;
    tb19.y = t19>=0.25;
    t2 = (tb19.y) ? -2.0 : -0.0;
    t19 = t19 + t2;
    tb19.x = t19>=0.0;
    t3.zw = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb19.yx));
    t0.x = dot(t0.yx, t3.xw);
    t9 = dot(t0.wz, t3.xw);
    t9 = t10_1.y + t9;
    t10_2.x = _Color0.x;
    t10_2.y = _Color1.x;
    t10_2.z = _Color2.x;
    t10_2.w = _Color3.x;
    t4.x = dot(t3, t10_2);
    t10_2.x = _Color0.y;
    t10_2.y = _Color1.y;
    t10_2.z = _Color2.y;
    t10_2.w = _Color3.y;
    t4.y = dot(t3, t10_2);
    t10_2.x = _Color0.z;
    t10_2.y = _Color1.z;
    t10_2.z = _Color2.z;
    t10_2.w = _Color3.z;
    t4.z = dot(t3, t10_2);
    t0.xzw = t10_1.xxx * t4.xyz + t0.xxx;
    t16_5.x = vs_TEXCOORD5.x;
    t16_5.y = vs_TEXCOORD3.w;
    t16_5.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_6.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = vs_TEXCOORD5.y;
    t16_5.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_6.xyz, t16_5.xyz);
    t16_5.x = t16_1.y * t16_1.y;
    t16_5.x = t16_1.x * t16_1.x + (-t16_5.x);
    t16_7.xy = vs_TEXCOORD5.zw;
    t16_7.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_6.xyz, t16_7.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_7.x = dot(unity_SHBr, t16_2);
    t16_7.y = dot(unity_SHBg, t16_2);
    t16_7.z = dot(unity_SHBb, t16_2);
    t16_5.xyz = unity_SHC.xyz * t16_5.xxx + t16_7.xyz;
    t16_1.w = 1.0;
    t16_7.x = dot(unity_SHAr, t16_1);
    t16_7.y = dot(unity_SHAg, t16_1);
    t16_7.z = dot(unity_SHAb, t16_1);
    t16_5.xyz = t16_5.xyz + t16_7.xyz;
    t16_5.xyz = t16_5.xyz * _LightParameters.xxx;
    t16_5.xyz = log2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_5.xyz = exp2(t16_5.xyz);
    t16_5.xyz = t16_5.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_5.xyz = max(t16_5.xyz, vec3(0.0, 0.0, 0.0));
    t16_32 = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_7.x = t16_32 + t16_32;
    t16_32 = (-t16_32) + 1.0;
    t16_7.xyz = t16_1.xyz * (-t16_7.xxx) + vs_TEXCOORD3.xyz;
    t16_7.x = dot(t16_7.xyz, (-t16_1.xyz));
    t16_7.y = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_7.xy = max(t16_7.xy, vec2(0.25, 0.0));
    t16_7.x = t16_7.x * _Glossiness.w;
    t16_7.x = t16_7.x * _MatGlossiness;
    t16_7.x = t16_7.x * 10.0;
    t16_7.xzw = t16_7.xxx * _Glossiness.xyz;
    t16_7.xzw = t16_5.xyz * t16_7.xzw;
    t16_5.xyz = t0.xzw * t16_5.xyz;
    t16_7.xzw = vec3(t9) * t16_7.xzw;
    t16_8.x = (-t9) + 1.0;
    t16_5.xyz = t16_5.xyz * t16_8.xxx;
    t16_7.xzw = t0.xzw * t16_7.xzw;
    t10_6.x = (-_LightDir.w) + 1.0;
    t16_16 = t16_7.y * _LightDir.w + t10_6.x;
    t16_8.x = t16_16 * 1.5;
    t16_8.x = min(t16_8.x, 1.0);
    t16_17.xyz = vec3(t16_16) + (-_CharacterShadowColor.xyz);
    t16_16 = t16_16 * _LightParameters.y;
    t16_8.xyz = t16_8.xxx * t16_17.xyz + _CharacterShadowColor.xyz;
    t16_5.xyz = t16_5.xyz * t16_8.xyz + t16_7.xzw;
    vec3 txVec39 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_7.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec39, 0.0);
    t16_25 = (-_LightShadowData.x) + 1.0;
    t16_7.x = t16_7.x * t16_25 + _LightShadowData.x;
    t16_7.x = t16_7.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_7.x = max(t16_7.x, 0.0);
    t16_5.xyz = t16_5.xyz * t16_7.xxx;
    t16_7.x = dot(t16_5.xyz, vec3(1.0, 1.0, 1.0));
    t16_7.xzw = t16_7.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_5.xyz);
    t16_5.xyz = vec3(t16_16) * t16_7.xzw + t16_5.xyz;
    t16_5.xyz = vec3(t16_32) * _RimColor.xyz + t16_5.xyz;
    t16_5.xyz = t16_5.xyz * vs_TEXCOORD2.xxx;
    t16_5.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_5.xyz;
    SV_Target0.xyz = t16_5.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_OFF" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_NOISEGLOW" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_HIGHLIGHT" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" }
"!!GLES3"
}
}
 }
}
Fallback "Hidden/Shadow/Opaque"
}
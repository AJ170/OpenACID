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
 Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Background+1" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Background+1" }
  Lighting On
  GpuProgramID 10721
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp float _Amplitude;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  highp vec2 tmpvar_2;
  tmpvar_2.y = _glesMultiTexCoord1.y;
  mediump float distance_3;
  highp float mDeg_4;
  highp vec4 wPos_5;
  highp vec4 tmpvar_6;
  tmpvar_6 = (_Object2World * _glesVertex);
  wPos_5.xzw = tmpvar_6.xzw;
  highp vec4 tmpvar_7;
  tmpvar_7.w = 0.0;
  tmpvar_7.xyz = tmpvar_1;
  mDeg_4 = (((tmpvar_6.z * tmpvar_6.x) * 0.0075) + (_Time.z * 0.33));
  highp float tmpvar_8;
  tmpvar_8 = (_Amplitude * _glesColor.y);
  mediump float tmpvar_9;
  tmpvar_9 = (_glesColor.x * 12.0);
  wPos_5.y = (tmpvar_6.y - (tmpvar_8 + (
    sin((mDeg_4 + tmpvar_9))
   * tmpvar_8)));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_WorldSpaceCameraPos - wPos_5.xyz);
  highp float tmpvar_11;
  tmpvar_11 = sqrt(dot (tmpvar_10, tmpvar_10));
  distance_3 = tmpvar_11;
  mediump float tmpvar_12;
  tmpvar_12 = clamp ((-1.0 + (distance_3 * 0.15)), 0.0, 1.0);
  tmpvar_2.x = tmpvar_12;
  wPos_5 = (_World2Object * wPos_5);
  highp vec3 tmpvar_13;
  highp vec3 tmpvar_14;
  tmpvar_13 = _glesTANGENT.xyz;
  tmpvar_14 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_15;
  tmpvar_15[0].x = tmpvar_13.x;
  tmpvar_15[0].y = tmpvar_14.x;
  tmpvar_15[0].z = tmpvar_1.x;
  tmpvar_15[1].x = tmpvar_13.y;
  tmpvar_15[1].y = tmpvar_14.y;
  tmpvar_15[1].z = tmpvar_1.y;
  tmpvar_15[2].x = tmpvar_13.z;
  tmpvar_15[2].y = tmpvar_14.z;
  tmpvar_15[2].z = tmpvar_1.z;
  highp vec4 v_16;
  v_16.x = glstate_matrix_modelview0[0].x;
  v_16.y = glstate_matrix_modelview0[1].x;
  v_16.z = glstate_matrix_modelview0[2].x;
  v_16.w = glstate_matrix_modelview0[3].x;
  highp vec4 v_17;
  v_17.x = glstate_matrix_modelview0[0].y;
  v_17.y = glstate_matrix_modelview0[1].y;
  v_17.z = glstate_matrix_modelview0[2].y;
  v_17.w = glstate_matrix_modelview0[3].y;
  gl_Position = (glstate_matrix_mvp * wPos_5);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = (tmpvar_15 * v_16.xyz);
  xlv_TEXCOORD3 = (tmpvar_15 * v_17.xyz);
  xlv_TEXCOORD4 = (normalize((tmpvar_7 * _World2Object).xyz) + ((
    sin((mDeg_4 + tmpvar_9))
   * 0.0125) * _glesColor.y));
  xlv_TEXCOORD5 = (tmpvar_6.xyz - _WorldSpaceCameraPos);
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _MatCap_ST;
uniform mediump vec4 _Color;
uniform sampler2D _BumpMap;
uniform sampler2D _MatCap;
uniform lowp samplerCube _EffectCube;
uniform highp float _ReflIntensity;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
void main ()
{
  mediump vec4 finalColor_1;
  mediump vec4 baseFade_2;
  mediump vec3 reflectedDir_3;
  mediump vec2 vn_4;
  highp vec3 normal_5;
  lowp vec3 tmpvar_6;
  tmpvar_6 = ((texture2D (_BumpMap, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  normal_5 = tmpvar_6;
  highp float tmpvar_7;
  tmpvar_7 = dot (xlv_TEXCOORD2, normal_5);
  vn_4.x = tmpvar_7;
  highp float tmpvar_8;
  tmpvar_8 = dot (xlv_TEXCOORD3, normal_5);
  vn_4.y = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize(xlv_TEXCOORD4);
  highp vec3 tmpvar_10;
  tmpvar_10 = (xlv_TEXCOORD5 - (2.0 * (
    dot (tmpvar_9, xlv_TEXCOORD5)
   * tmpvar_9)));
  reflectedDir_3 = tmpvar_10;
  lowp vec4 tmpvar_11;
  highp vec2 P_12;
  P_12 = (((vn_4 * 0.5) + 0.5) + _MatCap_ST.zw);
  tmpvar_11 = texture2D (_MatCap, P_12);
  highp vec4 tmpvar_13;
  tmpvar_13 = vec4(clamp ((xlv_TEXCOORD1.x + xlv_TEXCOORD1.y), 0.0, 1.0));
  baseFade_2 = tmpvar_13;
  finalColor_1 = ((_Color * baseFade_2) + ((
    (tmpvar_11 * _Color)
   * 2.0) * (1.0 - baseFade_2)));
  lowp vec4 tmpvar_14;
  tmpvar_14 = textureCube (_EffectCube, reflectedDir_3);
  finalColor_1 = (finalColor_1 + ((tmpvar_14 * _ReflIntensity) * (1.0 - baseFade_2)));
  gl_FragData[0] = finalColor_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
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
uniform 	vec4 _MatCap_ST;
uniform 	mediump vec4 _Color;
uniform 	mediump vec4 _ShadowColor;
uniform 	float _Amplitude;
uniform 	float _ReflIntensity;
in highp vec4 in_POSITION0;
in mediump vec3 in_COLOR0;
in highp vec2 in_TEXCOORD0;
in highp vec2 in_TEXCOORD1;
in highp vec3 in_NORMAL0;
in highp vec4 in_TANGENT0;
out highp vec2 vs_TEXCOORD0;
out highp vec2 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
out highp vec3 vs_TEXCOORD4;
out highp vec3 vs_TEXCOORD5;
highp vec4 t0;
highp vec4 t1;
highp vec3 t2;
highp vec4 t3;
highp vec4 t4;
mediump float t16_5;
highp float t6;
highp float t12;
void main()
{
    t0.x = in_COLOR0.y * _Amplitude;
    t6 = _Time.z * 0.330000013;
    t1 = in_POSITION0.yyyy * _Object2World[1];
    t1 = _Object2World[0] * in_POSITION0.xxxx + t1;
    t1 = _Object2World[2] * in_POSITION0.zzzz + t1;
    t1 = _Object2World[3] * in_POSITION0.wwww + t1;
    t12 = t1.x * t1.z;
    t6 = t12 * 0.00749999983 + t6;
    t6 = in_COLOR0.x * 12.0 + t6;
    t6 = sin(t6);
    t0.x = t6 * t0.x + t0.x;
    t6 = t6 * 0.0125000002;
    t6 = t6 * in_COLOR0.y;
    t2.y = (-t0.x) + t1.y;
    t3 = t2.yyyy * _World2Object[1];
    t3 = _World2Object[0] * t1.xxxx + t3;
    t3 = _World2Object[2] * t1.zzzz + t3;
    t3 = _World2Object[3] * t1.wwww + t3;
    t4 = t3.yyyy * glstate_matrix_mvp[1];
    t4 = glstate_matrix_mvp[0] * t3.xxxx + t4;
    t4 = glstate_matrix_mvp[2] * t3.zzzz + t4;
    gl_Position = glstate_matrix_mvp[3] * t3.wwww + t4;
    t2.xz = t1.xz;
    vs_TEXCOORD5.xyz = t1.xyz + (-_WorldSpaceCameraPos.xyzx.xyz);
    t0.xzw = (-t2.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t0.x = dot(t0.xzw, t0.xzw);
    t0.x = sqrt(t0.x);
    t16_5 = t0.x * 0.150000006 + -1.0;
    t16_5 = clamp(t16_5, 0.0, 1.0);
    vs_TEXCOORD1.x = t16_5;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    vs_TEXCOORD1.y = in_TEXCOORD1.y;
    t0.xzw = in_NORMAL0.zxy * in_TANGENT0.yzx;
    t0.xzw = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t0.xzw);
    t0.xzw = t0.xzw * in_TANGENT0.www;
    t1.x = glstate_matrix_modelview0[0].x;
    t1.y = glstate_matrix_modelview0[1].x;
    t1.z = glstate_matrix_modelview0[2].x;
    vs_TEXCOORD2.y = dot(t0.xzw, t1.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t1.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t1.xyz);
    t1.x = glstate_matrix_modelview0[0].y;
    t1.y = glstate_matrix_modelview0[1].y;
    t1.z = glstate_matrix_modelview0[2].y;
    vs_TEXCOORD3.y = dot(t0.xzw, t1.xyz);
    vs_TEXCOORD3.x = dot(in_TANGENT0.xyz, t1.xyz);
    vs_TEXCOORD3.z = dot(in_NORMAL0.xyz, t1.xyz);
    t1.x = dot(in_NORMAL0.xyz, _World2Object[0].xyz);
    t1.y = dot(in_NORMAL0.xyz, _World2Object[1].xyz);
    t1.z = dot(in_NORMAL0.xyz, _World2Object[2].xyz);
    t0.x = dot(t1.xyz, t1.xyz);
    t0.x = inversesqrt(t0.x);
    vs_TEXCOORD4.xyz = t1.xyz * t0.xxx + vec3(t6);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
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
uniform 	vec4 _MatCap_ST;
uniform 	mediump vec4 _Color;
uniform 	mediump vec4 _ShadowColor;
uniform 	float _Amplitude;
uniform 	float _ReflIntensity;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _MatCap;
uniform lowp samplerCube _EffectCube;
in highp vec2 vs_TEXCOORD0;
in highp vec2 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
in highp vec3 vs_TEXCOORD4;
in highp vec3 vs_TEXCOORD5;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec4 t0;
lowp vec4 t10_0;
highp vec2 t1;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec2 t16_3;
highp float t4;
highp float t15;
void main()
{
    t0.x = dot(vs_TEXCOORD4.xyz, vs_TEXCOORD4.xyz);
    t0.x = inversesqrt(t0.x);
    t0.xyz = t0.xxx * vs_TEXCOORD4.xyz;
    t15 = dot(vs_TEXCOORD5.xyz, t0.xyz);
    t15 = t15 + t15;
    t0.xyz = t0.xyz * (-vec3(t15)) + vs_TEXCOORD5.xyz;
    t10_0 = texture(_EffectCube, t0.xyz);
    t0 = t10_0 * vec4(vec4(_ReflIntensity, _ReflIntensity, _ReflIntensity, _ReflIntensity));
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD0.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t1.x = dot(vs_TEXCOORD2.xyz, t10_2.xyz);
    t1.y = dot(vs_TEXCOORD3.xyz, t10_2.xyz);
    t16_3.xy = t1.xy * vec2(0.5, 0.5) + vec2(0.5, 0.5);
    t1.xy = vec2(t16_3.x + _MatCap_ST.z, t16_3.y + _MatCap_ST.w);
    t10_1 = texture(_MatCap, t1.xy);
    t16_1 = t10_1 * _Color;
    t16_1 = t16_1 + t16_1;
    t4 = vs_TEXCOORD1.y + vs_TEXCOORD1.x;
    t4 = clamp(t4, 0.0, 1.0);
    t16_3.x = (-t4) + 1.0;
    t16_1 = t16_1 * t16_3.xxxx;
    t16_1 = _Color * vec4(t4) + t16_1;
    t0 = t0 * t16_3.xxxx + t16_1;
    SV_Target0 = t0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp float _Amplitude;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  highp vec2 tmpvar_2;
  tmpvar_2.y = _glesMultiTexCoord1.y;
  mediump float distance_3;
  highp float mDeg_4;
  highp vec4 wPos_5;
  highp vec4 tmpvar_6;
  tmpvar_6 = (_Object2World * _glesVertex);
  wPos_5.xzw = tmpvar_6.xzw;
  highp vec4 tmpvar_7;
  tmpvar_7.w = 0.0;
  tmpvar_7.xyz = tmpvar_1;
  mDeg_4 = (((tmpvar_6.z * tmpvar_6.x) * 0.0075) + (_Time.z * 0.33));
  highp float tmpvar_8;
  tmpvar_8 = (_Amplitude * _glesColor.y);
  mediump float tmpvar_9;
  tmpvar_9 = (_glesColor.x * 12.0);
  wPos_5.y = (tmpvar_6.y - (tmpvar_8 + (
    sin((mDeg_4 + tmpvar_9))
   * tmpvar_8)));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_WorldSpaceCameraPos - wPos_5.xyz);
  highp float tmpvar_11;
  tmpvar_11 = sqrt(dot (tmpvar_10, tmpvar_10));
  distance_3 = tmpvar_11;
  mediump float tmpvar_12;
  tmpvar_12 = clamp ((-1.0 + (distance_3 * 0.15)), 0.0, 1.0);
  tmpvar_2.x = tmpvar_12;
  wPos_5 = (_World2Object * wPos_5);
  highp vec3 tmpvar_13;
  highp vec3 tmpvar_14;
  tmpvar_13 = _glesTANGENT.xyz;
  tmpvar_14 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_15;
  tmpvar_15[0].x = tmpvar_13.x;
  tmpvar_15[0].y = tmpvar_14.x;
  tmpvar_15[0].z = tmpvar_1.x;
  tmpvar_15[1].x = tmpvar_13.y;
  tmpvar_15[1].y = tmpvar_14.y;
  tmpvar_15[1].z = tmpvar_1.y;
  tmpvar_15[2].x = tmpvar_13.z;
  tmpvar_15[2].y = tmpvar_14.z;
  tmpvar_15[2].z = tmpvar_1.z;
  highp vec4 v_16;
  v_16.x = glstate_matrix_modelview0[0].x;
  v_16.y = glstate_matrix_modelview0[1].x;
  v_16.z = glstate_matrix_modelview0[2].x;
  v_16.w = glstate_matrix_modelview0[3].x;
  highp vec4 v_17;
  v_17.x = glstate_matrix_modelview0[0].y;
  v_17.y = glstate_matrix_modelview0[1].y;
  v_17.z = glstate_matrix_modelview0[2].y;
  v_17.w = glstate_matrix_modelview0[3].y;
  gl_Position = (glstate_matrix_mvp * wPos_5);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = (tmpvar_15 * v_16.xyz);
  xlv_TEXCOORD3 = (tmpvar_15 * v_17.xyz);
  xlv_TEXCOORD4 = (normalize((tmpvar_7 * _World2Object).xyz) + ((
    sin((mDeg_4 + tmpvar_9))
   * 0.0125) * _glesColor.y));
  xlv_TEXCOORD5 = (tmpvar_6.xyz - _WorldSpaceCameraPos);
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _MatCap_ST;
uniform mediump vec4 _Color;
uniform sampler2D _BumpMap;
uniform sampler2D _MatCap;
uniform lowp samplerCube _EffectCube;
uniform highp float _ReflIntensity;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
void main ()
{
  mediump vec4 finalColor_1;
  mediump vec4 baseFade_2;
  mediump vec3 reflectedDir_3;
  mediump vec2 vn_4;
  highp vec3 normal_5;
  lowp vec3 tmpvar_6;
  tmpvar_6 = ((texture2D (_BumpMap, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  normal_5 = tmpvar_6;
  highp float tmpvar_7;
  tmpvar_7 = dot (xlv_TEXCOORD2, normal_5);
  vn_4.x = tmpvar_7;
  highp float tmpvar_8;
  tmpvar_8 = dot (xlv_TEXCOORD3, normal_5);
  vn_4.y = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize(xlv_TEXCOORD4);
  highp vec3 tmpvar_10;
  tmpvar_10 = (xlv_TEXCOORD5 - (2.0 * (
    dot (tmpvar_9, xlv_TEXCOORD5)
   * tmpvar_9)));
  reflectedDir_3 = tmpvar_10;
  lowp vec4 tmpvar_11;
  highp vec2 P_12;
  P_12 = (((vn_4 * 0.5) + 0.5) + _MatCap_ST.zw);
  tmpvar_11 = texture2D (_MatCap, P_12);
  highp vec4 tmpvar_13;
  tmpvar_13 = vec4(clamp ((xlv_TEXCOORD1.x + xlv_TEXCOORD1.y), 0.0, 1.0));
  baseFade_2 = tmpvar_13;
  finalColor_1 = ((_Color * baseFade_2) + ((
    (tmpvar_11 * _Color)
   * 2.0) * (1.0 - baseFade_2)));
  lowp vec4 tmpvar_14;
  tmpvar_14 = textureCube (_EffectCube, reflectedDir_3);
  finalColor_1 = (finalColor_1 + ((tmpvar_14 * _ReflIntensity) * (1.0 - baseFade_2)));
  gl_FragData[0] = finalColor_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
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
uniform 	vec4 _MatCap_ST;
uniform 	mediump vec4 _Color;
uniform 	mediump vec4 _ShadowColor;
uniform 	float _Amplitude;
uniform 	float _ReflIntensity;
in highp vec4 in_POSITION0;
in mediump vec3 in_COLOR0;
in highp vec2 in_TEXCOORD0;
in highp vec2 in_TEXCOORD1;
in highp vec3 in_NORMAL0;
in highp vec4 in_TANGENT0;
out highp vec2 vs_TEXCOORD0;
out highp vec2 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
out highp vec3 vs_TEXCOORD4;
out highp vec3 vs_TEXCOORD5;
highp vec4 t0;
highp vec4 t1;
highp vec3 t2;
highp vec4 t3;
highp vec4 t4;
mediump float t16_5;
highp float t6;
highp float t12;
void main()
{
    t0.x = in_COLOR0.y * _Amplitude;
    t6 = _Time.z * 0.330000013;
    t1 = in_POSITION0.yyyy * _Object2World[1];
    t1 = _Object2World[0] * in_POSITION0.xxxx + t1;
    t1 = _Object2World[2] * in_POSITION0.zzzz + t1;
    t1 = _Object2World[3] * in_POSITION0.wwww + t1;
    t12 = t1.x * t1.z;
    t6 = t12 * 0.00749999983 + t6;
    t6 = in_COLOR0.x * 12.0 + t6;
    t6 = sin(t6);
    t0.x = t6 * t0.x + t0.x;
    t6 = t6 * 0.0125000002;
    t6 = t6 * in_COLOR0.y;
    t2.y = (-t0.x) + t1.y;
    t3 = t2.yyyy * _World2Object[1];
    t3 = _World2Object[0] * t1.xxxx + t3;
    t3 = _World2Object[2] * t1.zzzz + t3;
    t3 = _World2Object[3] * t1.wwww + t3;
    t4 = t3.yyyy * glstate_matrix_mvp[1];
    t4 = glstate_matrix_mvp[0] * t3.xxxx + t4;
    t4 = glstate_matrix_mvp[2] * t3.zzzz + t4;
    gl_Position = glstate_matrix_mvp[3] * t3.wwww + t4;
    t2.xz = t1.xz;
    vs_TEXCOORD5.xyz = t1.xyz + (-_WorldSpaceCameraPos.xyzx.xyz);
    t0.xzw = (-t2.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t0.x = dot(t0.xzw, t0.xzw);
    t0.x = sqrt(t0.x);
    t16_5 = t0.x * 0.150000006 + -1.0;
    t16_5 = clamp(t16_5, 0.0, 1.0);
    vs_TEXCOORD1.x = t16_5;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    vs_TEXCOORD1.y = in_TEXCOORD1.y;
    t0.xzw = in_NORMAL0.zxy * in_TANGENT0.yzx;
    t0.xzw = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t0.xzw);
    t0.xzw = t0.xzw * in_TANGENT0.www;
    t1.x = glstate_matrix_modelview0[0].x;
    t1.y = glstate_matrix_modelview0[1].x;
    t1.z = glstate_matrix_modelview0[2].x;
    vs_TEXCOORD2.y = dot(t0.xzw, t1.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t1.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t1.xyz);
    t1.x = glstate_matrix_modelview0[0].y;
    t1.y = glstate_matrix_modelview0[1].y;
    t1.z = glstate_matrix_modelview0[2].y;
    vs_TEXCOORD3.y = dot(t0.xzw, t1.xyz);
    vs_TEXCOORD3.x = dot(in_TANGENT0.xyz, t1.xyz);
    vs_TEXCOORD3.z = dot(in_NORMAL0.xyz, t1.xyz);
    t1.x = dot(in_NORMAL0.xyz, _World2Object[0].xyz);
    t1.y = dot(in_NORMAL0.xyz, _World2Object[1].xyz);
    t1.z = dot(in_NORMAL0.xyz, _World2Object[2].xyz);
    t0.x = dot(t1.xyz, t1.xyz);
    t0.x = inversesqrt(t0.x);
    vs_TEXCOORD4.xyz = t1.xyz * t0.xxx + vec3(t6);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
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
uniform 	vec4 _MatCap_ST;
uniform 	mediump vec4 _Color;
uniform 	mediump vec4 _ShadowColor;
uniform 	float _Amplitude;
uniform 	float _ReflIntensity;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _MatCap;
uniform lowp samplerCube _EffectCube;
in highp vec2 vs_TEXCOORD0;
in highp vec2 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
in highp vec3 vs_TEXCOORD4;
in highp vec3 vs_TEXCOORD5;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec4 t0;
lowp vec4 t10_0;
highp vec2 t1;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec2 t16_3;
highp float t4;
highp float t15;
void main()
{
    t0.x = dot(vs_TEXCOORD4.xyz, vs_TEXCOORD4.xyz);
    t0.x = inversesqrt(t0.x);
    t0.xyz = t0.xxx * vs_TEXCOORD4.xyz;
    t15 = dot(vs_TEXCOORD5.xyz, t0.xyz);
    t15 = t15 + t15;
    t0.xyz = t0.xyz * (-vec3(t15)) + vs_TEXCOORD5.xyz;
    t10_0 = texture(_EffectCube, t0.xyz);
    t0 = t10_0 * vec4(vec4(_ReflIntensity, _ReflIntensity, _ReflIntensity, _ReflIntensity));
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD0.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t1.x = dot(vs_TEXCOORD2.xyz, t10_2.xyz);
    t1.y = dot(vs_TEXCOORD3.xyz, t10_2.xyz);
    t16_3.xy = t1.xy * vec2(0.5, 0.5) + vec2(0.5, 0.5);
    t1.xy = vec2(t16_3.x + _MatCap_ST.z, t16_3.y + _MatCap_ST.w);
    t10_1 = texture(_MatCap, t1.xy);
    t16_1 = t10_1 * _Color;
    t16_1 = t16_1 + t16_1;
    t4 = vs_TEXCOORD1.y + vs_TEXCOORD1.x;
    t4 = clamp(t4, 0.0, 1.0);
    t16_3.x = (-t4) + 1.0;
    t16_1 = t16_1 * t16_3.xxxx;
    t16_1 = _Color * vec4(t4) + t16_1;
    t0 = t0 * t16_3.xxxx + t16_1;
    SV_Target0 = t0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp float _Amplitude;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  highp vec2 tmpvar_2;
  tmpvar_2.y = _glesMultiTexCoord1.y;
  mediump float distance_3;
  highp float mDeg_4;
  highp vec4 wPos_5;
  mediump vec4 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7 = (_Object2World * _glesVertex);
  wPos_5.xzw = tmpvar_7.xzw;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 0.0;
  tmpvar_8.xyz = tmpvar_1;
  mDeg_4 = (((tmpvar_7.z * tmpvar_7.x) * 0.0075) + (_Time.z * 0.33));
  highp float tmpvar_9;
  tmpvar_9 = (_Amplitude * _glesColor.y);
  mediump float tmpvar_10;
  tmpvar_10 = (_glesColor.x * 12.0);
  wPos_5.y = (tmpvar_7.y - (tmpvar_9 + (
    sin((mDeg_4 + tmpvar_10))
   * tmpvar_9)));
  highp vec3 tmpvar_11;
  tmpvar_11 = (_WorldSpaceCameraPos - wPos_5.xyz);
  highp float tmpvar_12;
  tmpvar_12 = sqrt(dot (tmpvar_11, tmpvar_11));
  distance_3 = tmpvar_12;
  mediump float tmpvar_13;
  tmpvar_13 = clamp ((-1.0 + (distance_3 * 0.15)), 0.0, 1.0);
  tmpvar_2.x = tmpvar_13;
  wPos_5 = (_World2Object * wPos_5);
  highp vec3 tmpvar_14;
  highp vec3 tmpvar_15;
  tmpvar_14 = _glesTANGENT.xyz;
  tmpvar_15 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_16;
  tmpvar_16[0].x = tmpvar_14.x;
  tmpvar_16[0].y = tmpvar_15.x;
  tmpvar_16[0].z = tmpvar_1.x;
  tmpvar_16[1].x = tmpvar_14.y;
  tmpvar_16[1].y = tmpvar_15.y;
  tmpvar_16[1].z = tmpvar_1.y;
  tmpvar_16[2].x = tmpvar_14.z;
  tmpvar_16[2].y = tmpvar_15.z;
  tmpvar_16[2].z = tmpvar_1.z;
  highp vec4 v_17;
  v_17.x = glstate_matrix_modelview0[0].x;
  v_17.y = glstate_matrix_modelview0[1].x;
  v_17.z = glstate_matrix_modelview0[2].x;
  v_17.w = glstate_matrix_modelview0[3].x;
  highp vec4 v_18;
  v_18.x = glstate_matrix_modelview0[0].y;
  v_18.y = glstate_matrix_modelview0[1].y;
  v_18.z = glstate_matrix_modelview0[2].y;
  v_18.w = glstate_matrix_modelview0[3].y;
  tmpvar_6 = (unity_World2Shadow[0] * tmpvar_7);
  gl_Position = (glstate_matrix_mvp * wPos_5);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = (tmpvar_16 * v_17.xyz);
  xlv_TEXCOORD3 = (tmpvar_16 * v_18.xyz);
  xlv_TEXCOORD4 = (normalize((tmpvar_8 * _World2Object).xyz) + ((
    sin((mDeg_4 + tmpvar_10))
   * 0.0125) * _glesColor.y));
  xlv_TEXCOORD5 = (tmpvar_7.xyz - _WorldSpaceCameraPos);
  xlv_TEXCOORD6 = tmpvar_6;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
uniform highp vec4 _MatCap_ST;
uniform mediump vec4 _Color;
uniform mediump vec4 _ShadowColor;
uniform sampler2D _BumpMap;
uniform sampler2D _MatCap;
uniform lowp samplerCube _EffectCube;
uniform highp float _ReflIntensity;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  mediump vec4 finalColor_1;
  mediump vec4 baseFade_2;
  mediump vec3 reflectedDir_3;
  mediump vec2 vn_4;
  highp vec3 normal_5;
  lowp vec3 tmpvar_6;
  tmpvar_6 = ((texture2D (_BumpMap, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  normal_5 = tmpvar_6;
  highp float tmpvar_7;
  tmpvar_7 = dot (xlv_TEXCOORD2, normal_5);
  vn_4.x = tmpvar_7;
  highp float tmpvar_8;
  tmpvar_8 = dot (xlv_TEXCOORD3, normal_5);
  vn_4.y = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize(xlv_TEXCOORD4);
  highp vec3 tmpvar_10;
  tmpvar_10 = (xlv_TEXCOORD5 - (2.0 * (
    dot (tmpvar_9, xlv_TEXCOORD5)
   * tmpvar_9)));
  reflectedDir_3 = tmpvar_10;
  lowp vec4 tmpvar_11;
  highp vec2 P_12;
  P_12 = (((vn_4 * 0.5) + 0.5) + _MatCap_ST.zw);
  tmpvar_11 = texture2D (_MatCap, P_12);
  highp vec4 tmpvar_13;
  tmpvar_13 = vec4(clamp ((xlv_TEXCOORD1.x + xlv_TEXCOORD1.y), 0.0, 1.0));
  baseFade_2 = tmpvar_13;
  finalColor_1 = ((_Color * baseFade_2) + ((
    (tmpvar_11 * _Color)
   * 2.0) * (1.0 - baseFade_2)));
  lowp vec4 tmpvar_14;
  tmpvar_14 = textureCube (_EffectCube, reflectedDir_3);
  finalColor_1 = (finalColor_1 + ((tmpvar_14 * _ReflIntensity) * (1.0 - baseFade_2)));
  lowp float tmpvar_15;
  highp float tmpvar_16;
  tmpvar_16 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD6).x > 
    (xlv_TEXCOORD6.z / xlv_TEXCOORD6.w)
  )), _LightShadowData.x);
  tmpvar_15 = tmpvar_16;
  lowp float tmpvar_17;
  tmpvar_17 = (1.0 - tmpvar_15);
  finalColor_1.xyz = mix (finalColor_1.xyz, _ShadowColor.xyz, vec3((tmpvar_17 * _ShadowColor.w)));
  gl_FragData[0] = finalColor_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp float _Amplitude;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  highp vec2 tmpvar_2;
  tmpvar_2.y = _glesMultiTexCoord1.y;
  mediump float distance_3;
  highp float mDeg_4;
  highp vec4 wPos_5;
  mediump vec4 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7 = (_Object2World * _glesVertex);
  wPos_5.xzw = tmpvar_7.xzw;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 0.0;
  tmpvar_8.xyz = tmpvar_1;
  mDeg_4 = (((tmpvar_7.z * tmpvar_7.x) * 0.0075) + (_Time.z * 0.33));
  highp float tmpvar_9;
  tmpvar_9 = (_Amplitude * _glesColor.y);
  mediump float tmpvar_10;
  tmpvar_10 = (_glesColor.x * 12.0);
  wPos_5.y = (tmpvar_7.y - (tmpvar_9 + (
    sin((mDeg_4 + tmpvar_10))
   * tmpvar_9)));
  highp vec3 tmpvar_11;
  tmpvar_11 = (_WorldSpaceCameraPos - wPos_5.xyz);
  highp float tmpvar_12;
  tmpvar_12 = sqrt(dot (tmpvar_11, tmpvar_11));
  distance_3 = tmpvar_12;
  mediump float tmpvar_13;
  tmpvar_13 = clamp ((-1.0 + (distance_3 * 0.15)), 0.0, 1.0);
  tmpvar_2.x = tmpvar_13;
  wPos_5 = (_World2Object * wPos_5);
  highp vec3 tmpvar_14;
  highp vec3 tmpvar_15;
  tmpvar_14 = _glesTANGENT.xyz;
  tmpvar_15 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_16;
  tmpvar_16[0].x = tmpvar_14.x;
  tmpvar_16[0].y = tmpvar_15.x;
  tmpvar_16[0].z = tmpvar_1.x;
  tmpvar_16[1].x = tmpvar_14.y;
  tmpvar_16[1].y = tmpvar_15.y;
  tmpvar_16[1].z = tmpvar_1.y;
  tmpvar_16[2].x = tmpvar_14.z;
  tmpvar_16[2].y = tmpvar_15.z;
  tmpvar_16[2].z = tmpvar_1.z;
  highp vec4 v_17;
  v_17.x = glstate_matrix_modelview0[0].x;
  v_17.y = glstate_matrix_modelview0[1].x;
  v_17.z = glstate_matrix_modelview0[2].x;
  v_17.w = glstate_matrix_modelview0[3].x;
  highp vec4 v_18;
  v_18.x = glstate_matrix_modelview0[0].y;
  v_18.y = glstate_matrix_modelview0[1].y;
  v_18.z = glstate_matrix_modelview0[2].y;
  v_18.w = glstate_matrix_modelview0[3].y;
  tmpvar_6 = (unity_World2Shadow[0] * tmpvar_7);
  gl_Position = (glstate_matrix_mvp * wPos_5);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = (tmpvar_16 * v_17.xyz);
  xlv_TEXCOORD3 = (tmpvar_16 * v_18.xyz);
  xlv_TEXCOORD4 = (normalize((tmpvar_8 * _World2Object).xyz) + ((
    sin((mDeg_4 + tmpvar_10))
   * 0.0125) * _glesColor.y));
  xlv_TEXCOORD5 = (tmpvar_7.xyz - _WorldSpaceCameraPos);
  xlv_TEXCOORD6 = tmpvar_6;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform highp vec4 _MatCap_ST;
uniform mediump vec4 _Color;
uniform mediump vec4 _ShadowColor;
uniform sampler2D _BumpMap;
uniform sampler2D _MatCap;
uniform lowp samplerCube _EffectCube;
uniform highp float _ReflIntensity;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying highp vec3 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  mediump vec4 finalColor_1;
  mediump vec4 baseFade_2;
  mediump vec3 reflectedDir_3;
  mediump vec2 vn_4;
  highp vec3 normal_5;
  lowp vec3 tmpvar_6;
  tmpvar_6 = ((texture2D (_BumpMap, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  normal_5 = tmpvar_6;
  highp float tmpvar_7;
  tmpvar_7 = dot (xlv_TEXCOORD2, normal_5);
  vn_4.x = tmpvar_7;
  highp float tmpvar_8;
  tmpvar_8 = dot (xlv_TEXCOORD3, normal_5);
  vn_4.y = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize(xlv_TEXCOORD4);
  highp vec3 tmpvar_10;
  tmpvar_10 = (xlv_TEXCOORD5 - (2.0 * (
    dot (tmpvar_9, xlv_TEXCOORD5)
   * tmpvar_9)));
  reflectedDir_3 = tmpvar_10;
  lowp vec4 tmpvar_11;
  highp vec2 P_12;
  P_12 = (((vn_4 * 0.5) + 0.5) + _MatCap_ST.zw);
  tmpvar_11 = texture2D (_MatCap, P_12);
  highp vec4 tmpvar_13;
  tmpvar_13 = vec4(clamp ((xlv_TEXCOORD1.x + xlv_TEXCOORD1.y), 0.0, 1.0));
  baseFade_2 = tmpvar_13;
  finalColor_1 = ((_Color * baseFade_2) + ((
    (tmpvar_11 * _Color)
   * 2.0) * (1.0 - baseFade_2)));
  lowp vec4 tmpvar_14;
  tmpvar_14 = textureCube (_EffectCube, reflectedDir_3);
  finalColor_1 = (finalColor_1 + ((tmpvar_14 * _ReflIntensity) * (1.0 - baseFade_2)));
  lowp float shadow_15;
  shadow_15 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD6.xyz) * (1.0 - _LightShadowData.x)));
  lowp float tmpvar_16;
  tmpvar_16 = (1.0 - shadow_15);
  finalColor_1.xyz = mix (finalColor_1.xyz, _ShadowColor.xyz, vec3((tmpvar_16 * _ShadowColor.w)));
  gl_FragData[0] = finalColor_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
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
uniform 	vec4 _MatCap_ST;
uniform 	mediump vec4 _Color;
uniform 	mediump vec4 _ShadowColor;
uniform 	float _Amplitude;
uniform 	float _ReflIntensity;
in highp vec4 in_POSITION0;
in mediump vec3 in_COLOR0;
in highp vec2 in_TEXCOORD0;
in highp vec2 in_TEXCOORD1;
in highp vec3 in_NORMAL0;
in highp vec4 in_TANGENT0;
out highp vec2 vs_TEXCOORD0;
out highp vec2 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
out highp vec3 vs_TEXCOORD4;
out highp vec3 vs_TEXCOORD5;
out mediump vec4 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
highp vec3 t2;
highp vec4 t3;
highp vec4 t4;
mediump float t16_5;
highp float t6;
highp float t12;
void main()
{
    t0.x = in_COLOR0.y * _Amplitude;
    t6 = _Time.z * 0.330000013;
    t1 = in_POSITION0.yyyy * _Object2World[1];
    t1 = _Object2World[0] * in_POSITION0.xxxx + t1;
    t1 = _Object2World[2] * in_POSITION0.zzzz + t1;
    t1 = _Object2World[3] * in_POSITION0.wwww + t1;
    t12 = t1.x * t1.z;
    t6 = t12 * 0.00749999983 + t6;
    t6 = in_COLOR0.x * 12.0 + t6;
    t6 = sin(t6);
    t0.x = t6 * t0.x + t0.x;
    t6 = t6 * 0.0125000002;
    t6 = t6 * in_COLOR0.y;
    t2.y = (-t0.x) + t1.y;
    t3 = t2.yyyy * _World2Object[1];
    t3 = _World2Object[0] * t1.xxxx + t3;
    t3 = _World2Object[2] * t1.zzzz + t3;
    t3 = _World2Object[3] * t1.wwww + t3;
    t4 = t3.yyyy * glstate_matrix_mvp[1];
    t4 = glstate_matrix_mvp[0] * t3.xxxx + t4;
    t4 = glstate_matrix_mvp[2] * t3.zzzz + t4;
    gl_Position = glstate_matrix_mvp[3] * t3.wwww + t4;
    t2.xz = t1.xz;
    t0.xzw = (-t2.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t0.x = dot(t0.xzw, t0.xzw);
    t0.x = sqrt(t0.x);
    t16_5 = t0.x * 0.150000006 + -1.0;
    t16_5 = clamp(t16_5, 0.0, 1.0);
    vs_TEXCOORD1.x = t16_5;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    vs_TEXCOORD1.y = in_TEXCOORD1.y;
    t0.xzw = in_NORMAL0.zxy * in_TANGENT0.yzx;
    t0.xzw = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t0.xzw);
    t0.xzw = t0.xzw * in_TANGENT0.www;
    t3.x = glstate_matrix_modelview0[0].x;
    t3.y = glstate_matrix_modelview0[1].x;
    t3.z = glstate_matrix_modelview0[2].x;
    vs_TEXCOORD2.y = dot(t0.xzw, t3.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t3.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t3.xyz);
    t3.x = glstate_matrix_modelview0[0].y;
    t3.y = glstate_matrix_modelview0[1].y;
    t3.z = glstate_matrix_modelview0[2].y;
    vs_TEXCOORD3.y = dot(t0.xzw, t3.xyz);
    vs_TEXCOORD3.x = dot(in_TANGENT0.xyz, t3.xyz);
    vs_TEXCOORD3.z = dot(in_NORMAL0.xyz, t3.xyz);
    t3.x = dot(in_NORMAL0.xyz, _World2Object[0].xyz);
    t3.y = dot(in_NORMAL0.xyz, _World2Object[1].xyz);
    t3.z = dot(in_NORMAL0.xyz, _World2Object[2].xyz);
    t0.x = dot(t3.xyz, t3.xyz);
    t0.x = inversesqrt(t0.x);
    vs_TEXCOORD4.xyz = t3.xyz * t0.xxx + vec3(t6);
    vs_TEXCOORD5.xyz = t1.xyz + (-_WorldSpaceCameraPos.xyzx.xyz);
    t0 = t1.yyyy * unity_World2Shadow[0][1];
    t0 = unity_World2Shadow[0][0] * t2.xxxx + t0;
    t0 = unity_World2Shadow[0][2] * t2.zzzz + t0;
    t0 = unity_World2Shadow[0][3] * t1.wwww + t0;
    vs_TEXCOORD6 = t0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
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
uniform 	vec4 _MatCap_ST;
uniform 	mediump vec4 _Color;
uniform 	mediump vec4 _ShadowColor;
uniform 	float _Amplitude;
uniform 	float _ReflIntensity;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _MatCap;
uniform lowp samplerCube _EffectCube;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in highp vec2 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
in highp vec3 vs_TEXCOORD4;
in highp vec3 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD6;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec4 t0;
lowp vec4 t10_0;
highp vec2 t1;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
highp float t4;
mediump float t16_5;
highp float t18;
mediump float t16_21;
void main()
{
    t0.x = dot(vs_TEXCOORD4.xyz, vs_TEXCOORD4.xyz);
    t0.x = inversesqrt(t0.x);
    t0.xyz = t0.xxx * vs_TEXCOORD4.xyz;
    t18 = dot(vs_TEXCOORD5.xyz, t0.xyz);
    t18 = t18 + t18;
    t0.xyz = t0.xyz * (-vec3(t18)) + vs_TEXCOORD5.xyz;
    t10_0 = texture(_EffectCube, t0.xyz);
    t0 = t10_0 * vec4(vec4(_ReflIntensity, _ReflIntensity, _ReflIntensity, _ReflIntensity));
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD0.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t1.x = dot(vs_TEXCOORD2.xyz, t10_2.xyz);
    t1.y = dot(vs_TEXCOORD3.xyz, t10_2.xyz);
    t16_3.xy = t1.xy * vec2(0.5, 0.5) + vec2(0.5, 0.5);
    t1.xy = vec2(t16_3.x + _MatCap_ST.z, t16_3.y + _MatCap_ST.w);
    t10_1 = texture(_MatCap, t1.xy);
    t16_1 = t10_1 * _Color;
    t16_1 = t16_1 + t16_1;
    t4 = vs_TEXCOORD1.y + vs_TEXCOORD1.x;
    t4 = clamp(t4, 0.0, 1.0);
    t16_3.x = (-t4) + 1.0;
    t16_1 = t16_1 * t16_3.xxxx;
    t16_1 = _Color * vec4(t4) + t16_1;
    t0 = t0 * t16_3.xxxx + t16_1;
    t16_3.xyz = (-t0.xyz) + _ShadowColor.xyz;
    vec3 txVec0 = vec3(vs_TEXCOORD6.xy,vs_TEXCOORD6.z);
    t16_21 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec0, 0.0);
    t16_5 = (-_LightShadowData.x) + 1.0;
    t16_21 = t16_21 * t16_5 + _LightShadowData.x;
    t10_2.x = (-t16_21) + 1.0;
    t16_21 = t10_2.x * _ShadowColor.w;
    SV_Target0.xyz = vec3(t16_21) * t16_3.xyz + t0.xyz;
    SV_Target0.w = t0.w;
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES3"
}
}
 }
}
Fallback "Hidden/Shadow/Opaque"
}
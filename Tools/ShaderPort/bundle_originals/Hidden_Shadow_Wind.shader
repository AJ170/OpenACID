Shader "Hidden/Shadow/Wind" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _SpecColor ("Spec Color", Color) = (1,1,1,1)
 _Emission ("Emissive Color", Color) = (0,0,0,0)
 _Shininess ("Shininess", Range(0.01,1)) = 0.7
 _MainTex ("Base (RGB)", 2D) = "white" { }
}
SubShader { 
 Pass {
  Name "SHADOWCASTER"
  Tags { "LIGHTMODE"="SHADOWCASTER" "SHADOWSUPPORT"="true" }
  Cull Off
  Offset 1, 1
  GpuProgramID 18165
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_DEPTH" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
uniform highp vec4 unity_LightShadowBias;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 _WindParams;
uniform highp vec4 _WindDir;
uniform highp float _WindDirFromObjNormalOrGlobal;
uniform mediump vec4 _WindFrequency;
uniform mediump vec4 _WindAmplitude;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  highp vec4 tmpvar_2;
  highp vec4 v_position_3;
  v_position_3.w = _glesVertex.w;
  highp vec4 frequency_4;
  frequency_4 = _WindFrequency;
  highp vec4 amplitude_5;
  amplitude_5 = _WindAmplitude;
  highp vec4 waves_6;
  highp vec4 wavesIn_7;
  highp float leafPhase_8;
  highp float objPhase_9;
  lowp float tmpvar_10;
  tmpvar_10 = tmpvar_1.x;
  objPhase_9 = tmpvar_10;
  lowp float tmpvar_11;
  tmpvar_11 = tmpvar_1.y;
  leafPhase_8 = tmpvar_11;
  wavesIn_7 = ((_WindParams.xxxy * frequency_4) + vec4(objPhase_9));
  wavesIn_7.xyz = (wavesIn_7.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_8) * vec3(0.5, 0.5, 0.5))));
  wavesIn_7.xyz = (wavesIn_7.xyz + vec3(leafPhase_8));
  highp vec4 tmpvar_12;
  tmpvar_12 = abs(((
    fract((((
      fract(wavesIn_7)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_6 = (((tmpvar_12 * tmpvar_12) * (3.0 - 
    (2.0 * tmpvar_12)
  )) * amplitude_5);
  v_position_3.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_6.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * _glesColor.w));
  highp vec4 tmpvar_13;
  tmpvar_13.w = 0.0;
  tmpvar_13.xyz = _WindDir.xyz;
  v_position_3.xyz = (v_position_3.xyz + (normalize(
    mix ((_World2Object * tmpvar_13).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * _glesColor.z) * waves_6.w)
   + 
    (_glesColor.z * _WindParams.z)
  )));
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = v_position_3.xyz;
  tmpvar_2 = (glstate_matrix_mvp * tmpvar_14);
  highp vec4 clipPos_15;
  clipPos_15.xyw = tmpvar_2.xyw;
  clipPos_15.z = (tmpvar_2.z + clamp ((unity_LightShadowBias.x / tmpvar_2.w), 0.0, 1.0));
  clipPos_15.z = mix (clipPos_15.z, max (clipPos_15.z, -(tmpvar_2.w)), unity_LightShadowBias.y);
  tmpvar_2 = clipPos_15;
  gl_Position = clipPos_15;
}


#endif
#ifdef FRAGMENT
void main ()
{
  gl_FragData[0] = vec4(0.0, 0.0, 0.0, 0.0);
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_DEPTH" }
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in lowp vec4 in_COLOR0;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
highp float t6;
highp float t9;
void main()
{
    t0.xyz = _World2Object[1].xyz * _WindDir.yyy;
    t0.xyz = _World2Object[0].xyz * _WindDir.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WindDir.zzz + t0.xyz;
    t1.xyz = (-t0.xyz) + in_NORMAL0.xyz;
    t0.xyz = vec3(_WindDirFromObjNormalOrGlobal) * t1.xyz + t0.xyz;
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    t9 = in_COLOR0.y * 0.5 + 0.5;
    t1 = _WindParams.xxxy * _WindFrequency + in_COLOR0.xxxx;
    t1.xyz = t1.xyz * vec3(t9) + in_COLOR0.yyy;
    t2 = fract(t1);
    t1 = t2 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-0.5, -0.5, -0.5, -0.5);
    t1 = fract(t1);
    t1 = t1 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-1.0, -1.0, -1.0, -1.0);
    t2 = abs(t1) * abs(t1);
    t1 = -abs(t1) * vec4(2.0, 2.0, 2.0, 2.0) + vec4(3.0, 3.0, 3.0, 3.0);
    t1 = t1 * t2;
    t1 = t1 * _WindAmplitude;
    t9 = dot(t1.xyz, vec3(1.0, 1.0, 1.0));
    t9 = t9 * _WindParams.w;
    t1.xyz = vec3(t9) * in_NORMAL0.xyz;
    t1.xyz = t1.xyz * in_COLOR0.www + in_POSITION0.xyz;
    t9 = in_COLOR0.z * _WindDir.w;
    t2.x = in_COLOR0.z * _WindParams.z;
    t9 = t9 * t1.w + t2.x;
    t0.xyz = t0.xyz * vec3(t9) + t1.xyz;
    t1 = t0.yyyy * glstate_matrix_mvp[1];
    t1 = glstate_matrix_mvp[0] * t0.xxxx + t1;
    t0 = glstate_matrix_mvp[2] * t0.zzzz + t1;
    t0 = t0 + glstate_matrix_mvp[3];
    t1.x = unity_LightShadowBias.x / t0.w;
    t1.x = clamp(t1.x, 0.0, 1.0);
    t6 = t0.z + t1.x;
    t1.x = max((-t0.w), t6);
    gl_Position.xyw = t0.xyw;
    t0.x = (-t6) + t1.x;
    gl_Position.z = unity_LightShadowBias.y * t0.x + t6;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
layout(location = 0) out mediump vec4 SV_Target0;
void main()
{
    SV_Target0 = vec4(0.0, 0.0, 0.0, 0.0);
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_CUBE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
uniform highp vec4 _LightPositionRange;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _WindParams;
uniform highp vec4 _WindDir;
uniform highp float _WindDirFromObjNormalOrGlobal;
uniform mediump vec4 _WindFrequency;
uniform mediump vec4 _WindAmplitude;
varying highp vec3 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  highp vec4 v_position_2;
  v_position_2.w = _glesVertex.w;
  highp vec4 frequency_3;
  frequency_3 = _WindFrequency;
  highp vec4 amplitude_4;
  amplitude_4 = _WindAmplitude;
  highp vec4 waves_5;
  highp vec4 wavesIn_6;
  highp float leafPhase_7;
  highp float objPhase_8;
  lowp float tmpvar_9;
  tmpvar_9 = tmpvar_1.x;
  objPhase_8 = tmpvar_9;
  lowp float tmpvar_10;
  tmpvar_10 = tmpvar_1.y;
  leafPhase_7 = tmpvar_10;
  wavesIn_6 = ((_WindParams.xxxy * frequency_3) + vec4(objPhase_8));
  wavesIn_6.xyz = (wavesIn_6.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_7) * vec3(0.5, 0.5, 0.5))));
  wavesIn_6.xyz = (wavesIn_6.xyz + vec3(leafPhase_7));
  highp vec4 tmpvar_11;
  tmpvar_11 = abs(((
    fract((((
      fract(wavesIn_6)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_5 = (((tmpvar_11 * tmpvar_11) * (3.0 - 
    (2.0 * tmpvar_11)
  )) * amplitude_4);
  v_position_2.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_5.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * _glesColor.w));
  highp vec4 tmpvar_12;
  tmpvar_12.w = 0.0;
  tmpvar_12.xyz = _WindDir.xyz;
  v_position_2.xyz = (v_position_2.xyz + (normalize(
    mix ((_World2Object * tmpvar_12).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * _glesColor.z) * waves_5.w)
   + 
    (_glesColor.z * _WindParams.z)
  )));
  xlv_TEXCOORD0 = ((_Object2World * v_position_2).xyz - _LightPositionRange.xyz);
  gl_Position = (glstate_matrix_mvp * v_position_2);
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _LightPositionRange;
varying highp vec3 xlv_TEXCOORD0;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 enc_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = fract((vec4(1.0, 255.0, 65025.0, 1.658138e+07) * min (
    (sqrt(dot (xlv_TEXCOORD0, xlv_TEXCOORD0)) * _LightPositionRange.w)
  , 0.999)));
  enc_2 = (tmpvar_3 - (tmpvar_3.yzww * 0.003921569));
  tmpvar_1 = enc_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_CUBE" }
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in lowp vec4 in_COLOR0;
out highp vec3 vs_TEXCOORD0;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
highp float t9;
void main()
{
    t0.xyz = _World2Object[1].xyz * _WindDir.yyy;
    t0.xyz = _World2Object[0].xyz * _WindDir.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WindDir.zzz + t0.xyz;
    t1.xyz = (-t0.xyz) + in_NORMAL0.xyz;
    t0.xyz = vec3(_WindDirFromObjNormalOrGlobal) * t1.xyz + t0.xyz;
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    t9 = in_COLOR0.y * 0.5 + 0.5;
    t1 = _WindParams.xxxy * _WindFrequency + in_COLOR0.xxxx;
    t1.xyz = t1.xyz * vec3(t9) + in_COLOR0.yyy;
    t2 = fract(t1);
    t1 = t2 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-0.5, -0.5, -0.5, -0.5);
    t1 = fract(t1);
    t1 = t1 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-1.0, -1.0, -1.0, -1.0);
    t2 = abs(t1) * abs(t1);
    t1 = -abs(t1) * vec4(2.0, 2.0, 2.0, 2.0) + vec4(3.0, 3.0, 3.0, 3.0);
    t1 = t1 * t2;
    t1 = t1 * _WindAmplitude;
    t9 = dot(t1.xyz, vec3(1.0, 1.0, 1.0));
    t9 = t9 * _WindParams.w;
    t1.xyz = vec3(t9) * in_NORMAL0.xyz;
    t1.xyz = t1.xyz * in_COLOR0.www + in_POSITION0.xyz;
    t9 = in_COLOR0.z * _WindDir.w;
    t2.x = in_COLOR0.z * _WindParams.z;
    t9 = t9 * t1.w + t2.x;
    t0.xyz = t0.xyz * vec3(t9) + t1.xyz;
    t1.xyz = t0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * t0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * t0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    vs_TEXCOORD0.xyz = t1.xyz + (-_LightPositionRange.xyz);
    t1 = t0.yyyy * glstate_matrix_mvp[1];
    t1 = glstate_matrix_mvp[0] * t0.xxxx + t1;
    t0 = glstate_matrix_mvp[2] * t0.zzzz + t1;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
in highp vec3 vs_TEXCOORD0;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec4 t0;
void main()
{
    t0.x = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD0.xyz);
    t0.x = sqrt(t0.x);
    t0.x = t0.x * _LightPositionRange.w;
    t0.x = min(t0.x, 0.999000013);
    t0 = t0.xxxx * vec4(1.0, 255.0, 65025.0, 16581375.0);
    t0 = fract(t0);
    t0 = (-t0.yzww) * vec4(0.00392156886, 0.00392156886, 0.00392156886, 0.00392156886) + t0;
    SV_Target0 = t0;
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SHADOWS_DEPTH" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_DEPTH" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_CUBE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_CUBE" }
"!!GLES3"
}
}
 }
 Pass {
  Name "SHADOWCOLLECTOR"
  Tags { "LIGHTMODE"="SHADOWCOLLECTOR" }
  GpuProgramID 80224
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_NONATIVE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _WindParams;
uniform highp vec4 _WindDir;
uniform highp float _WindDirFromObjNormalOrGlobal;
uniform mediump vec4 _WindFrequency;
uniform mediump vec4 _WindAmplitude;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  highp vec4 tmpvar_2;
  highp vec4 v_position_3;
  v_position_3.w = _glesVertex.w;
  highp vec4 frequency_4;
  frequency_4 = _WindFrequency;
  highp vec4 amplitude_5;
  amplitude_5 = _WindAmplitude;
  highp vec4 waves_6;
  highp vec4 wavesIn_7;
  highp float leafPhase_8;
  highp float objPhase_9;
  lowp float tmpvar_10;
  tmpvar_10 = tmpvar_1.x;
  objPhase_9 = tmpvar_10;
  lowp float tmpvar_11;
  tmpvar_11 = tmpvar_1.y;
  leafPhase_8 = tmpvar_11;
  wavesIn_7 = ((_WindParams.xxxy * frequency_4) + vec4(objPhase_9));
  wavesIn_7.xyz = (wavesIn_7.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_8) * vec3(0.5, 0.5, 0.5))));
  wavesIn_7.xyz = (wavesIn_7.xyz + vec3(leafPhase_8));
  highp vec4 tmpvar_12;
  tmpvar_12 = abs(((
    fract((((
      fract(wavesIn_7)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_6 = (((tmpvar_12 * tmpvar_12) * (3.0 - 
    (2.0 * tmpvar_12)
  )) * amplitude_5);
  v_position_3.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_6.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * _glesColor.w));
  highp vec4 tmpvar_13;
  tmpvar_13.w = 0.0;
  tmpvar_13.xyz = _WindDir.xyz;
  v_position_3.xyz = (v_position_3.xyz + (normalize(
    mix ((_World2Object * tmpvar_13).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * _glesColor.z) * waves_6.w)
   + 
    (_glesColor.z * _WindParams.z)
  )));
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Object2World * v_position_3);
  tmpvar_2.xyz = tmpvar_14.xyz;
  tmpvar_2.w = -((glstate_matrix_modelview0 * v_position_3).z);
  gl_Position = (glstate_matrix_mvp * v_position_3);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_14).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_14).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_14).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_14).xyz;
  xlv_TEXCOORD4 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 zFar_4;
  highp vec4 zNear_5;
  bvec4 tmpvar_6;
  tmpvar_6 = greaterThanEqual (xlv_TEXCOORD4.wwww, _LightSplitsNear);
  lowp vec4 tmpvar_7;
  tmpvar_7 = vec4(tmpvar_6);
  zNear_5 = tmpvar_7;
  bvec4 tmpvar_8;
  tmpvar_8 = lessThan (xlv_TEXCOORD4.wwww, _LightSplitsFar);
  lowp vec4 tmpvar_9;
  tmpvar_9 = vec4(tmpvar_8);
  zFar_4 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (zNear_5 * zFar_4);
  highp float tmpvar_11;
  tmpvar_11 = clamp (((xlv_TEXCOORD4.w * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = (((
    (xlv_TEXCOORD0 * tmpvar_10.x)
   + 
    (xlv_TEXCOORD1 * tmpvar_10.y)
  ) + (xlv_TEXCOORD2 * tmpvar_10.z)) + (xlv_TEXCOORD3 * tmpvar_10.w));
  highp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_ShadowMapTexture, tmpvar_12.xy);
  mediump float tmpvar_14;
  if ((tmpvar_13.x < tmpvar_12.z)) {
    tmpvar_14 = 0.0;
  } else {
    tmpvar_14 = 1.0;
  };
  shadow_3 = (_LightShadowData.x + (tmpvar_14 * (1.0 - _LightShadowData.x)));
  res_2.x = clamp ((shadow_3 + tmpvar_11), 0.0, 1.0);
  res_2.y = 1.0;
  highp vec2 enc_15;
  highp vec2 tmpvar_16;
  tmpvar_16 = fract((vec2(1.0, 255.0) * (1.0 - 
    (xlv_TEXCOORD4.w * _ProjectionParams.w)
  )));
  enc_15.y = tmpvar_16.y;
  enc_15.x = (tmpvar_16.x - (tmpvar_16.y * 0.003921569));
  res_2.zw = enc_15;
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_NATIVE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _WindParams;
uniform highp vec4 _WindDir;
uniform highp float _WindDirFromObjNormalOrGlobal;
uniform mediump vec4 _WindFrequency;
uniform mediump vec4 _WindAmplitude;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  highp vec4 tmpvar_2;
  highp vec4 v_position_3;
  v_position_3.w = _glesVertex.w;
  highp vec4 frequency_4;
  frequency_4 = _WindFrequency;
  highp vec4 amplitude_5;
  amplitude_5 = _WindAmplitude;
  highp vec4 waves_6;
  highp vec4 wavesIn_7;
  highp float leafPhase_8;
  highp float objPhase_9;
  lowp float tmpvar_10;
  tmpvar_10 = tmpvar_1.x;
  objPhase_9 = tmpvar_10;
  lowp float tmpvar_11;
  tmpvar_11 = tmpvar_1.y;
  leafPhase_8 = tmpvar_11;
  wavesIn_7 = ((_WindParams.xxxy * frequency_4) + vec4(objPhase_9));
  wavesIn_7.xyz = (wavesIn_7.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_8) * vec3(0.5, 0.5, 0.5))));
  wavesIn_7.xyz = (wavesIn_7.xyz + vec3(leafPhase_8));
  highp vec4 tmpvar_12;
  tmpvar_12 = abs(((
    fract((((
      fract(wavesIn_7)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_6 = (((tmpvar_12 * tmpvar_12) * (3.0 - 
    (2.0 * tmpvar_12)
  )) * amplitude_5);
  v_position_3.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_6.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * _glesColor.w));
  highp vec4 tmpvar_13;
  tmpvar_13.w = 0.0;
  tmpvar_13.xyz = _WindDir.xyz;
  v_position_3.xyz = (v_position_3.xyz + (normalize(
    mix ((_World2Object * tmpvar_13).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * _glesColor.z) * waves_6.w)
   + 
    (_glesColor.z * _WindParams.z)
  )));
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Object2World * v_position_3);
  tmpvar_2.xyz = tmpvar_14.xyz;
  tmpvar_2.w = -((glstate_matrix_modelview0 * v_position_3).z);
  gl_Position = (glstate_matrix_mvp * v_position_3);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_14).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_14).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_14).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_14).xyz;
  xlv_TEXCOORD4 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 zFar_4;
  highp vec4 zNear_5;
  bvec4 tmpvar_6;
  tmpvar_6 = greaterThanEqual (xlv_TEXCOORD4.wwww, _LightSplitsNear);
  lowp vec4 tmpvar_7;
  tmpvar_7 = vec4(tmpvar_6);
  zNear_5 = tmpvar_7;
  bvec4 tmpvar_8;
  tmpvar_8 = lessThan (xlv_TEXCOORD4.wwww, _LightSplitsFar);
  lowp vec4 tmpvar_9;
  tmpvar_9 = vec4(tmpvar_8);
  zFar_4 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (zNear_5 * zFar_4);
  highp vec4 tmpvar_11;
  tmpvar_11.w = 1.0;
  tmpvar_11.xyz = (((
    (xlv_TEXCOORD0 * tmpvar_10.x)
   + 
    (xlv_TEXCOORD1 * tmpvar_10.y)
  ) + (xlv_TEXCOORD2 * tmpvar_10.z)) + (xlv_TEXCOORD3 * tmpvar_10.w));
  lowp float tmpvar_12;
  tmpvar_12 = shadow2DEXT (_ShadowMapTexture, tmpvar_11.xyz);
  mediump float tmpvar_13;
  tmpvar_13 = tmpvar_12;
  shadow_3 = (_LightShadowData.x + (tmpvar_13 * (1.0 - _LightShadowData.x)));
  res_2.x = clamp ((shadow_3 + clamp (
    ((xlv_TEXCOORD4.w * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  res_2.y = 1.0;
  highp vec2 enc_14;
  highp vec2 tmpvar_15;
  tmpvar_15 = fract((vec2(1.0, 255.0) * (1.0 - 
    (xlv_TEXCOORD4.w * _ProjectionParams.w)
  )));
  enc_14.y = tmpvar_15.y;
  enc_14.x = (tmpvar_15.x - (tmpvar_15.y * 0.003921569));
  res_2.zw = enc_14;
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_NATIVE" }
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in lowp vec4 in_COLOR0;
out highp vec3 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
out highp vec4 vs_TEXCOORD4;
highp vec3 t0;
highp vec4 t1;
highp vec4 t2;
highp float t3;
highp float t9;
void main()
{
    t0.xyz = _World2Object[1].xyz * _WindDir.yyy;
    t0.xyz = _World2Object[0].xyz * _WindDir.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WindDir.zzz + t0.xyz;
    t1.xyz = (-t0.xyz) + in_NORMAL0.xyz;
    t0.xyz = vec3(_WindDirFromObjNormalOrGlobal) * t1.xyz + t0.xyz;
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    t9 = in_COLOR0.y * 0.5 + 0.5;
    t1 = _WindParams.xxxy * _WindFrequency + in_COLOR0.xxxx;
    t1.xyz = t1.xyz * vec3(t9) + in_COLOR0.yyy;
    t2 = fract(t1);
    t1 = t2 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-0.5, -0.5, -0.5, -0.5);
    t1 = fract(t1);
    t1 = t1 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-1.0, -1.0, -1.0, -1.0);
    t2 = abs(t1) * abs(t1);
    t1 = -abs(t1) * vec4(2.0, 2.0, 2.0, 2.0) + vec4(3.0, 3.0, 3.0, 3.0);
    t1 = t1 * t2;
    t1 = t1 * _WindAmplitude;
    t9 = dot(t1.xyz, vec3(1.0, 1.0, 1.0));
    t9 = t9 * _WindParams.w;
    t1.xyz = vec3(t9) * in_NORMAL0.xyz;
    t1.xyz = t1.xyz * in_COLOR0.www + in_POSITION0.xyz;
    t9 = in_COLOR0.z * _WindDir.w;
    t2.x = in_COLOR0.z * _WindParams.z;
    t9 = t9 * t1.w + t2.x;
    t0.xyz = t0.xyz * vec3(t9) + t1.xyz;
    t1 = t0.yyyy * glstate_matrix_mvp[1];
    t1 = glstate_matrix_mvp[0] * t0.xxxx + t1;
    t1 = glstate_matrix_mvp[2] * t0.zzzz + t1;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t1;
    t1 = t0.yyyy * _Object2World[1];
    t1 = _Object2World[0] * t0.xxxx + t1;
    t1 = _Object2World[2] * t0.zzzz + t1;
    t1 = _Object2World[3] * in_POSITION0.wwww + t1;
    t2.xyz = t1.yyy * unity_World2Shadow[0][1].xyz;
    t2.xyz = unity_World2Shadow[0][0].xyz * t1.xxx + t2.xyz;
    t2.xyz = unity_World2Shadow[0][2].xyz * t1.zzz + t2.xyz;
    vs_TEXCOORD0.xyz = unity_World2Shadow[0][3].xyz * t1.www + t2.xyz;
    t2.xyz = t1.yyy * unity_World2Shadow[1][1].xyz;
    t2.xyz = unity_World2Shadow[1][0].xyz * t1.xxx + t2.xyz;
    t2.xyz = unity_World2Shadow[1][2].xyz * t1.zzz + t2.xyz;
    vs_TEXCOORD1.xyz = unity_World2Shadow[1][3].xyz * t1.www + t2.xyz;
    t2.xyz = t1.yyy * unity_World2Shadow[2][1].xyz;
    t2.xyz = unity_World2Shadow[2][0].xyz * t1.xxx + t2.xyz;
    t2.xyz = unity_World2Shadow[2][2].xyz * t1.zzz + t2.xyz;
    vs_TEXCOORD2.xyz = unity_World2Shadow[2][3].xyz * t1.www + t2.xyz;
    t2.xyz = t1.yyy * unity_World2Shadow[3][1].xyz;
    t2.xyz = unity_World2Shadow[3][0].xyz * t1.xxx + t2.xyz;
    t2.xyz = unity_World2Shadow[3][2].xyz * t1.zzz + t2.xyz;
    vs_TEXCOORD3.xyz = unity_World2Shadow[3][3].xyz * t1.www + t2.xyz;
    vs_TEXCOORD4.xyz = t1.xyz;
    t3 = t0.y * glstate_matrix_modelview0[1].z;
    t0.x = glstate_matrix_modelview0[0].z * t0.x + t3;
    t0.x = glstate_matrix_modelview0[2].z * t0.z + t0.x;
    t0.x = glstate_matrix_modelview0[3].z * in_POSITION0.w + t0.x;
    vs_TEXCOORD4.w = (-t0.x);
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec3 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
in highp vec4 vs_TEXCOORD4;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec4 t0;
lowp float t10_0;
bvec4 tb0;
highp vec4 t1;
bvec4 tb1;
mediump float t16_2;
void main()
{
    tb0 = greaterThanEqual(vs_TEXCOORD4.wwww, _LightSplitsNear);
    t0 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tb0));
    tb1 = lessThan(vs_TEXCOORD4.wwww, _LightSplitsFar);
    t1 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tb1));
    t0 = t0 * t1;
    t1.xyz = t0.yyy * vs_TEXCOORD1.xyz;
    t1.xyz = vs_TEXCOORD0.xyz * t0.xxx + t1.xyz;
    t0.xyz = vs_TEXCOORD2.xyz * t0.zzz + t1.xyz;
    t0.xyz = vs_TEXCOORD3.xyz * t0.www + t0.xyz;
    vec3 txVec14 = vec3(t0.xy,t0.z);
    t10_0 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec14, 0.0);
    t16_2 = (-_LightShadowData.x) + 1.0;
    t16_2 = t10_0 * t16_2 + _LightShadowData.x;
    t0.x = vs_TEXCOORD4.w * _LightShadowData.z + _LightShadowData.w;
    t0.x = clamp(t0.x, 0.0, 1.0);
    t0.x = t0.x + t16_2;
    t0.x = clamp(t0.x, 0.0, 1.0);
    t1.x = (-vs_TEXCOORD4.w) * _ProjectionParams.w + 1.0;
    t1.xy = t1.xx * vec2(1.0, 255.0);
    t1.xy = fract(t1.xy);
    t0.z = (-t1.y) * 0.00392156886 + t1.x;
    t0.w = t1.y;
    t0.y = 1.0;
    SV_Target0 = t0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NONATIVE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _WindParams;
uniform highp vec4 _WindDir;
uniform highp float _WindDirFromObjNormalOrGlobal;
uniform mediump vec4 _WindFrequency;
uniform mediump vec4 _WindAmplitude;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  highp vec4 tmpvar_2;
  highp vec4 v_position_3;
  v_position_3.w = _glesVertex.w;
  highp vec4 frequency_4;
  frequency_4 = _WindFrequency;
  highp vec4 amplitude_5;
  amplitude_5 = _WindAmplitude;
  highp vec4 waves_6;
  highp vec4 wavesIn_7;
  highp float leafPhase_8;
  highp float objPhase_9;
  lowp float tmpvar_10;
  tmpvar_10 = tmpvar_1.x;
  objPhase_9 = tmpvar_10;
  lowp float tmpvar_11;
  tmpvar_11 = tmpvar_1.y;
  leafPhase_8 = tmpvar_11;
  wavesIn_7 = ((_WindParams.xxxy * frequency_4) + vec4(objPhase_9));
  wavesIn_7.xyz = (wavesIn_7.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_8) * vec3(0.5, 0.5, 0.5))));
  wavesIn_7.xyz = (wavesIn_7.xyz + vec3(leafPhase_8));
  highp vec4 tmpvar_12;
  tmpvar_12 = abs(((
    fract((((
      fract(wavesIn_7)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_6 = (((tmpvar_12 * tmpvar_12) * (3.0 - 
    (2.0 * tmpvar_12)
  )) * amplitude_5);
  v_position_3.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_6.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * _glesColor.w));
  highp vec4 tmpvar_13;
  tmpvar_13.w = 0.0;
  tmpvar_13.xyz = _WindDir.xyz;
  v_position_3.xyz = (v_position_3.xyz + (normalize(
    mix ((_World2Object * tmpvar_13).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * _glesColor.z) * waves_6.w)
   + 
    (_glesColor.z * _WindParams.z)
  )));
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Object2World * v_position_3);
  tmpvar_2.xyz = tmpvar_14.xyz;
  tmpvar_2.w = -((glstate_matrix_modelview0 * v_position_3).z);
  gl_Position = (glstate_matrix_mvp * v_position_3);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_14).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_14).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_14).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_14).xyz;
  xlv_TEXCOORD4 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp sampler2D _ShadowMapTexture;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 cascadeWeights_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[0].xyz);
  highp vec3 tmpvar_6;
  tmpvar_6 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[1].xyz);
  highp vec3 tmpvar_7;
  tmpvar_7 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[2].xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[3].xyz);
  highp vec4 tmpvar_9;
  tmpvar_9.x = dot (tmpvar_5, tmpvar_5);
  tmpvar_9.y = dot (tmpvar_6, tmpvar_6);
  tmpvar_9.z = dot (tmpvar_7, tmpvar_7);
  tmpvar_9.w = dot (tmpvar_8, tmpvar_8);
  bvec4 tmpvar_10;
  tmpvar_10 = lessThan (tmpvar_9, unity_ShadowSplitSqRadii);
  lowp vec4 tmpvar_11;
  tmpvar_11 = vec4(tmpvar_10);
  cascadeWeights_4 = tmpvar_11;
  cascadeWeights_4.yzw = clamp ((cascadeWeights_4.yzw - cascadeWeights_4.xyz), 0.0, 1.0);
  highp vec3 tmpvar_12;
  tmpvar_12 = (xlv_TEXCOORD4.xyz - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_13;
  tmpvar_13 = clamp (((
    sqrt(dot (tmpvar_12, tmpvar_12))
   * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = (((
    (xlv_TEXCOORD0 * cascadeWeights_4.x)
   + 
    (xlv_TEXCOORD1 * cascadeWeights_4.y)
  ) + (xlv_TEXCOORD2 * cascadeWeights_4.z)) + (xlv_TEXCOORD3 * cascadeWeights_4.w));
  highp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_ShadowMapTexture, tmpvar_14.xy);
  mediump float tmpvar_16;
  if ((tmpvar_15.x < tmpvar_14.z)) {
    tmpvar_16 = 0.0;
  } else {
    tmpvar_16 = 1.0;
  };
  shadow_3 = (_LightShadowData.x + (tmpvar_16 * (1.0 - _LightShadowData.x)));
  res_2.x = clamp ((shadow_3 + tmpvar_13), 0.0, 1.0);
  res_2.y = 1.0;
  highp vec2 enc_17;
  highp vec2 tmpvar_18;
  tmpvar_18 = fract((vec2(1.0, 255.0) * (1.0 - 
    (xlv_TEXCOORD4.w * _ProjectionParams.w)
  )));
  enc_17.y = tmpvar_18.y;
  enc_17.x = (tmpvar_18.x - (tmpvar_18.y * 0.003921569));
  res_2.zw = enc_17;
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _WindParams;
uniform highp vec4 _WindDir;
uniform highp float _WindDirFromObjNormalOrGlobal;
uniform mediump vec4 _WindFrequency;
uniform mediump vec4 _WindAmplitude;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  highp vec4 tmpvar_2;
  highp vec4 v_position_3;
  v_position_3.w = _glesVertex.w;
  highp vec4 frequency_4;
  frequency_4 = _WindFrequency;
  highp vec4 amplitude_5;
  amplitude_5 = _WindAmplitude;
  highp vec4 waves_6;
  highp vec4 wavesIn_7;
  highp float leafPhase_8;
  highp float objPhase_9;
  lowp float tmpvar_10;
  tmpvar_10 = tmpvar_1.x;
  objPhase_9 = tmpvar_10;
  lowp float tmpvar_11;
  tmpvar_11 = tmpvar_1.y;
  leafPhase_8 = tmpvar_11;
  wavesIn_7 = ((_WindParams.xxxy * frequency_4) + vec4(objPhase_9));
  wavesIn_7.xyz = (wavesIn_7.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_8) * vec3(0.5, 0.5, 0.5))));
  wavesIn_7.xyz = (wavesIn_7.xyz + vec3(leafPhase_8));
  highp vec4 tmpvar_12;
  tmpvar_12 = abs(((
    fract((((
      fract(wavesIn_7)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_6 = (((tmpvar_12 * tmpvar_12) * (3.0 - 
    (2.0 * tmpvar_12)
  )) * amplitude_5);
  v_position_3.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_6.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * _glesColor.w));
  highp vec4 tmpvar_13;
  tmpvar_13.w = 0.0;
  tmpvar_13.xyz = _WindDir.xyz;
  v_position_3.xyz = (v_position_3.xyz + (normalize(
    mix ((_World2Object * tmpvar_13).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * _glesColor.z) * waves_6.w)
   + 
    (_glesColor.z * _WindParams.z)
  )));
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Object2World * v_position_3);
  tmpvar_2.xyz = tmpvar_14.xyz;
  tmpvar_2.w = -((glstate_matrix_modelview0 * v_position_3).z);
  gl_Position = (glstate_matrix_mvp * v_position_3);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_14).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_14).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_14).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_14).xyz;
  xlv_TEXCOORD4 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform lowp sampler2DShadow _ShadowMapTexture;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 cascadeWeights_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[0].xyz);
  highp vec3 tmpvar_6;
  tmpvar_6 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[1].xyz);
  highp vec3 tmpvar_7;
  tmpvar_7 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[2].xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[3].xyz);
  highp vec4 tmpvar_9;
  tmpvar_9.x = dot (tmpvar_5, tmpvar_5);
  tmpvar_9.y = dot (tmpvar_6, tmpvar_6);
  tmpvar_9.z = dot (tmpvar_7, tmpvar_7);
  tmpvar_9.w = dot (tmpvar_8, tmpvar_8);
  bvec4 tmpvar_10;
  tmpvar_10 = lessThan (tmpvar_9, unity_ShadowSplitSqRadii);
  lowp vec4 tmpvar_11;
  tmpvar_11 = vec4(tmpvar_10);
  cascadeWeights_4 = tmpvar_11;
  cascadeWeights_4.yzw = clamp ((cascadeWeights_4.yzw - cascadeWeights_4.xyz), 0.0, 1.0);
  highp vec3 tmpvar_12;
  tmpvar_12 = (xlv_TEXCOORD4.xyz - unity_ShadowFadeCenterAndType.xyz);
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (((
    (xlv_TEXCOORD0 * cascadeWeights_4.x)
   + 
    (xlv_TEXCOORD1 * cascadeWeights_4.y)
  ) + (xlv_TEXCOORD2 * cascadeWeights_4.z)) + (xlv_TEXCOORD3 * cascadeWeights_4.w));
  lowp float tmpvar_14;
  tmpvar_14 = shadow2DEXT (_ShadowMapTexture, tmpvar_13.xyz);
  mediump float tmpvar_15;
  tmpvar_15 = tmpvar_14;
  shadow_3 = (_LightShadowData.x + (tmpvar_15 * (1.0 - _LightShadowData.x)));
  res_2.x = clamp ((shadow_3 + clamp (
    ((sqrt(dot (tmpvar_12, tmpvar_12)) * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  res_2.y = 1.0;
  highp vec2 enc_16;
  highp vec2 tmpvar_17;
  tmpvar_17 = fract((vec2(1.0, 255.0) * (1.0 - 
    (xlv_TEXCOORD4.w * _ProjectionParams.w)
  )));
  enc_16.y = tmpvar_17.y;
  enc_16.x = (tmpvar_17.x - (tmpvar_17.y * 0.003921569));
  res_2.zw = enc_16;
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" }
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in lowp vec4 in_COLOR0;
out highp vec3 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
out highp vec4 vs_TEXCOORD4;
highp vec3 t0;
highp vec4 t1;
highp vec4 t2;
highp float t3;
highp float t9;
void main()
{
    t0.xyz = _World2Object[1].xyz * _WindDir.yyy;
    t0.xyz = _World2Object[0].xyz * _WindDir.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WindDir.zzz + t0.xyz;
    t1.xyz = (-t0.xyz) + in_NORMAL0.xyz;
    t0.xyz = vec3(_WindDirFromObjNormalOrGlobal) * t1.xyz + t0.xyz;
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    t9 = in_COLOR0.y * 0.5 + 0.5;
    t1 = _WindParams.xxxy * _WindFrequency + in_COLOR0.xxxx;
    t1.xyz = t1.xyz * vec3(t9) + in_COLOR0.yyy;
    t2 = fract(t1);
    t1 = t2 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-0.5, -0.5, -0.5, -0.5);
    t1 = fract(t1);
    t1 = t1 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-1.0, -1.0, -1.0, -1.0);
    t2 = abs(t1) * abs(t1);
    t1 = -abs(t1) * vec4(2.0, 2.0, 2.0, 2.0) + vec4(3.0, 3.0, 3.0, 3.0);
    t1 = t1 * t2;
    t1 = t1 * _WindAmplitude;
    t9 = dot(t1.xyz, vec3(1.0, 1.0, 1.0));
    t9 = t9 * _WindParams.w;
    t1.xyz = vec3(t9) * in_NORMAL0.xyz;
    t1.xyz = t1.xyz * in_COLOR0.www + in_POSITION0.xyz;
    t9 = in_COLOR0.z * _WindDir.w;
    t2.x = in_COLOR0.z * _WindParams.z;
    t9 = t9 * t1.w + t2.x;
    t0.xyz = t0.xyz * vec3(t9) + t1.xyz;
    t1 = t0.yyyy * glstate_matrix_mvp[1];
    t1 = glstate_matrix_mvp[0] * t0.xxxx + t1;
    t1 = glstate_matrix_mvp[2] * t0.zzzz + t1;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t1;
    t1 = t0.yyyy * _Object2World[1];
    t1 = _Object2World[0] * t0.xxxx + t1;
    t1 = _Object2World[2] * t0.zzzz + t1;
    t1 = _Object2World[3] * in_POSITION0.wwww + t1;
    t2.xyz = t1.yyy * unity_World2Shadow[0][1].xyz;
    t2.xyz = unity_World2Shadow[0][0].xyz * t1.xxx + t2.xyz;
    t2.xyz = unity_World2Shadow[0][2].xyz * t1.zzz + t2.xyz;
    vs_TEXCOORD0.xyz = unity_World2Shadow[0][3].xyz * t1.www + t2.xyz;
    t2.xyz = t1.yyy * unity_World2Shadow[1][1].xyz;
    t2.xyz = unity_World2Shadow[1][0].xyz * t1.xxx + t2.xyz;
    t2.xyz = unity_World2Shadow[1][2].xyz * t1.zzz + t2.xyz;
    vs_TEXCOORD1.xyz = unity_World2Shadow[1][3].xyz * t1.www + t2.xyz;
    t2.xyz = t1.yyy * unity_World2Shadow[2][1].xyz;
    t2.xyz = unity_World2Shadow[2][0].xyz * t1.xxx + t2.xyz;
    t2.xyz = unity_World2Shadow[2][2].xyz * t1.zzz + t2.xyz;
    vs_TEXCOORD2.xyz = unity_World2Shadow[2][3].xyz * t1.www + t2.xyz;
    t2.xyz = t1.yyy * unity_World2Shadow[3][1].xyz;
    t2.xyz = unity_World2Shadow[3][0].xyz * t1.xxx + t2.xyz;
    t2.xyz = unity_World2Shadow[3][2].xyz * t1.zzz + t2.xyz;
    vs_TEXCOORD3.xyz = unity_World2Shadow[3][3].xyz * t1.www + t2.xyz;
    vs_TEXCOORD4.xyz = t1.xyz;
    t3 = t0.y * glstate_matrix_modelview0[1].z;
    t0.x = glstate_matrix_modelview0[0].z * t0.x + t3;
    t0.x = glstate_matrix_modelview0[2].z * t0.z + t0.x;
    t0.x = glstate_matrix_modelview0[3].z * in_POSITION0.w + t0.x;
    vs_TEXCOORD4.w = (-t0.x);
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec3 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
in highp vec4 vs_TEXCOORD4;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec4 t0;
lowp float t10_0;
bvec4 tb0;
highp vec3 t1;
mediump float t16_2;
highp vec3 t3;
void main()
{
    t0.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowSplitSpheres[0].xyz);
    t0.x = dot(t0.xyz, t0.xyz);
    t1.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowSplitSpheres[1].xyz);
    t0.y = dot(t1.xyz, t1.xyz);
    t1.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowSplitSpheres[2].xyz);
    t0.z = dot(t1.xyz, t1.xyz);
    t1.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowSplitSpheres[3].xyz);
    t0.w = dot(t1.xyz, t1.xyz);
    tb0 = lessThan(t0, unity_ShadowSplitSqRadii);
    t1.x = (tb0.x) ? float(-1.0) : float(-0.0);
    t1.y = (tb0.y) ? float(-1.0) : float(-0.0);
    t1.z = (tb0.z) ? float(-1.0) : float(-0.0);
    t0 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tb0));
    t3.xyz = vec3(t1.x + t0.y, t1.y + t0.z, t1.z + t0.w);
    t3.xyz = max(t3.xyz, vec3(0.0, 0.0, 0.0));
    t1.xyz = t3.xxx * vs_TEXCOORD1.xyz;
    t1.xyz = vs_TEXCOORD0.xyz * t0.xxx + t1.xyz;
    t0.xyz = vs_TEXCOORD2.xyz * t3.yyy + t1.xyz;
    t0.xyz = vs_TEXCOORD3.xyz * t3.zzz + t0.xyz;
    vec3 txVec13 = vec3(t0.xy,t0.z);
    t10_0 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec13, 0.0);
    t16_2 = (-_LightShadowData.x) + 1.0;
    t16_2 = t10_0 * t16_2 + _LightShadowData.x;
    t0.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowFadeCenterAndType.xyz);
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t0.x = t0.x * _LightShadowData.z + _LightShadowData.w;
    t0.x = clamp(t0.x, 0.0, 1.0);
    t0.x = t0.x + t16_2;
    t0.x = clamp(t0.x, 0.0, 1.0);
    t1.x = (-vs_TEXCOORD4.w) * _ProjectionParams.w + 1.0;
    t1.xy = t1.xx * vec2(1.0, 255.0);
    t1.xy = fract(t1.xy);
    t0.z = (-t1.y) * 0.00392156886 + t1.x;
    t0.w = t1.y;
    t0.y = 1.0;
    SV_Target0 = t0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SINGLE_CASCADE" "SHADOWS_NONATIVE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _WindParams;
uniform highp vec4 _WindDir;
uniform highp float _WindDirFromObjNormalOrGlobal;
uniform mediump vec4 _WindFrequency;
uniform mediump vec4 _WindAmplitude;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  highp vec4 tmpvar_2;
  highp vec4 v_position_3;
  v_position_3.w = _glesVertex.w;
  highp vec4 frequency_4;
  frequency_4 = _WindFrequency;
  highp vec4 amplitude_5;
  amplitude_5 = _WindAmplitude;
  highp vec4 waves_6;
  highp vec4 wavesIn_7;
  highp float leafPhase_8;
  highp float objPhase_9;
  lowp float tmpvar_10;
  tmpvar_10 = tmpvar_1.x;
  objPhase_9 = tmpvar_10;
  lowp float tmpvar_11;
  tmpvar_11 = tmpvar_1.y;
  leafPhase_8 = tmpvar_11;
  wavesIn_7 = ((_WindParams.xxxy * frequency_4) + vec4(objPhase_9));
  wavesIn_7.xyz = (wavesIn_7.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_8) * vec3(0.5, 0.5, 0.5))));
  wavesIn_7.xyz = (wavesIn_7.xyz + vec3(leafPhase_8));
  highp vec4 tmpvar_12;
  tmpvar_12 = abs(((
    fract((((
      fract(wavesIn_7)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_6 = (((tmpvar_12 * tmpvar_12) * (3.0 - 
    (2.0 * tmpvar_12)
  )) * amplitude_5);
  v_position_3.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_6.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * _glesColor.w));
  highp vec4 tmpvar_13;
  tmpvar_13.w = 0.0;
  tmpvar_13.xyz = _WindDir.xyz;
  v_position_3.xyz = (v_position_3.xyz + (normalize(
    mix ((_World2Object * tmpvar_13).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * _glesColor.z) * waves_6.w)
   + 
    (_glesColor.z * _WindParams.z)
  )));
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Object2World * v_position_3);
  tmpvar_2.xyz = tmpvar_14.xyz;
  tmpvar_2.w = -((glstate_matrix_modelview0 * v_position_3).z);
  gl_Position = (glstate_matrix_mvp * v_position_3);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_14).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_14).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_14).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_14).xyz;
  xlv_TEXCOORD4 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 zFar_4;
  highp vec4 zNear_5;
  bvec4 tmpvar_6;
  tmpvar_6 = greaterThanEqual (xlv_TEXCOORD4.wwww, _LightSplitsNear);
  lowp vec4 tmpvar_7;
  tmpvar_7 = vec4(tmpvar_6);
  zNear_5 = tmpvar_7;
  bvec4 tmpvar_8;
  tmpvar_8 = lessThan (xlv_TEXCOORD4.wwww, _LightSplitsFar);
  lowp vec4 tmpvar_9;
  tmpvar_9 = vec4(tmpvar_8);
  zFar_4 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (zNear_5 * zFar_4);
  highp float tmpvar_11;
  tmpvar_11 = clamp (((xlv_TEXCOORD4.w * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = (((
    (xlv_TEXCOORD0 * tmpvar_10.x)
   + 
    (xlv_TEXCOORD1 * tmpvar_10.y)
  ) + (xlv_TEXCOORD2 * tmpvar_10.z)) + (xlv_TEXCOORD3 * tmpvar_10.w));
  highp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_ShadowMapTexture, tmpvar_12.xy);
  mediump float tmpvar_14;
  if ((tmpvar_13.x < tmpvar_12.z)) {
    tmpvar_14 = 0.0;
  } else {
    tmpvar_14 = 1.0;
  };
  shadow_3 = (_LightShadowData.x + (tmpvar_14 * (1.0 - _LightShadowData.x)));
  res_2.x = clamp ((shadow_3 + tmpvar_11), 0.0, 1.0);
  res_2.y = 1.0;
  highp vec2 enc_15;
  highp vec2 tmpvar_16;
  tmpvar_16 = fract((vec2(1.0, 255.0) * (1.0 - 
    (xlv_TEXCOORD4.w * _ProjectionParams.w)
  )));
  enc_15.y = tmpvar_16.y;
  enc_15.x = (tmpvar_16.x - (tmpvar_16.y * 0.003921569));
  res_2.zw = enc_15;
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _WindParams;
uniform highp vec4 _WindDir;
uniform highp float _WindDirFromObjNormalOrGlobal;
uniform mediump vec4 _WindFrequency;
uniform mediump vec4 _WindAmplitude;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  highp vec4 tmpvar_2;
  highp vec4 v_position_3;
  v_position_3.w = _glesVertex.w;
  highp vec4 frequency_4;
  frequency_4 = _WindFrequency;
  highp vec4 amplitude_5;
  amplitude_5 = _WindAmplitude;
  highp vec4 waves_6;
  highp vec4 wavesIn_7;
  highp float leafPhase_8;
  highp float objPhase_9;
  lowp float tmpvar_10;
  tmpvar_10 = tmpvar_1.x;
  objPhase_9 = tmpvar_10;
  lowp float tmpvar_11;
  tmpvar_11 = tmpvar_1.y;
  leafPhase_8 = tmpvar_11;
  wavesIn_7 = ((_WindParams.xxxy * frequency_4) + vec4(objPhase_9));
  wavesIn_7.xyz = (wavesIn_7.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_8) * vec3(0.5, 0.5, 0.5))));
  wavesIn_7.xyz = (wavesIn_7.xyz + vec3(leafPhase_8));
  highp vec4 tmpvar_12;
  tmpvar_12 = abs(((
    fract((((
      fract(wavesIn_7)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_6 = (((tmpvar_12 * tmpvar_12) * (3.0 - 
    (2.0 * tmpvar_12)
  )) * amplitude_5);
  v_position_3.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_6.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * _glesColor.w));
  highp vec4 tmpvar_13;
  tmpvar_13.w = 0.0;
  tmpvar_13.xyz = _WindDir.xyz;
  v_position_3.xyz = (v_position_3.xyz + (normalize(
    mix ((_World2Object * tmpvar_13).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * _glesColor.z) * waves_6.w)
   + 
    (_glesColor.z * _WindParams.z)
  )));
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Object2World * v_position_3);
  tmpvar_2.xyz = tmpvar_14.xyz;
  tmpvar_2.w = -((glstate_matrix_modelview0 * v_position_3).z);
  gl_Position = (glstate_matrix_mvp * v_position_3);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_14).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_14).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_14).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_14).xyz;
  xlv_TEXCOORD4 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 zFar_4;
  highp vec4 zNear_5;
  bvec4 tmpvar_6;
  tmpvar_6 = greaterThanEqual (xlv_TEXCOORD4.wwww, _LightSplitsNear);
  lowp vec4 tmpvar_7;
  tmpvar_7 = vec4(tmpvar_6);
  zNear_5 = tmpvar_7;
  bvec4 tmpvar_8;
  tmpvar_8 = lessThan (xlv_TEXCOORD4.wwww, _LightSplitsFar);
  lowp vec4 tmpvar_9;
  tmpvar_9 = vec4(tmpvar_8);
  zFar_4 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (zNear_5 * zFar_4);
  highp vec4 tmpvar_11;
  tmpvar_11.w = 1.0;
  tmpvar_11.xyz = (((
    (xlv_TEXCOORD0 * tmpvar_10.x)
   + 
    (xlv_TEXCOORD1 * tmpvar_10.y)
  ) + (xlv_TEXCOORD2 * tmpvar_10.z)) + (xlv_TEXCOORD3 * tmpvar_10.w));
  lowp float tmpvar_12;
  tmpvar_12 = shadow2DEXT (_ShadowMapTexture, tmpvar_11.xyz);
  mediump float tmpvar_13;
  tmpvar_13 = tmpvar_12;
  shadow_3 = (_LightShadowData.x + (tmpvar_13 * (1.0 - _LightShadowData.x)));
  res_2.x = clamp ((shadow_3 + clamp (
    ((xlv_TEXCOORD4.w * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  res_2.y = 1.0;
  highp vec2 enc_14;
  highp vec2 tmpvar_15;
  tmpvar_15 = fract((vec2(1.0, 255.0) * (1.0 - 
    (xlv_TEXCOORD4.w * _ProjectionParams.w)
  )));
  enc_14.y = tmpvar_15.y;
  enc_14.x = (tmpvar_15.x - (tmpvar_15.y * 0.003921569));
  res_2.zw = enc_14;
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in lowp vec4 in_COLOR0;
out highp vec3 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
out highp vec4 vs_TEXCOORD4;
highp vec3 t0;
highp vec4 t1;
highp vec4 t2;
highp float t3;
highp float t9;
void main()
{
    t0.xyz = _World2Object[1].xyz * _WindDir.yyy;
    t0.xyz = _World2Object[0].xyz * _WindDir.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WindDir.zzz + t0.xyz;
    t1.xyz = (-t0.xyz) + in_NORMAL0.xyz;
    t0.xyz = vec3(_WindDirFromObjNormalOrGlobal) * t1.xyz + t0.xyz;
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    t9 = in_COLOR0.y * 0.5 + 0.5;
    t1 = _WindParams.xxxy * _WindFrequency + in_COLOR0.xxxx;
    t1.xyz = t1.xyz * vec3(t9) + in_COLOR0.yyy;
    t2 = fract(t1);
    t1 = t2 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-0.5, -0.5, -0.5, -0.5);
    t1 = fract(t1);
    t1 = t1 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-1.0, -1.0, -1.0, -1.0);
    t2 = abs(t1) * abs(t1);
    t1 = -abs(t1) * vec4(2.0, 2.0, 2.0, 2.0) + vec4(3.0, 3.0, 3.0, 3.0);
    t1 = t1 * t2;
    t1 = t1 * _WindAmplitude;
    t9 = dot(t1.xyz, vec3(1.0, 1.0, 1.0));
    t9 = t9 * _WindParams.w;
    t1.xyz = vec3(t9) * in_NORMAL0.xyz;
    t1.xyz = t1.xyz * in_COLOR0.www + in_POSITION0.xyz;
    t9 = in_COLOR0.z * _WindDir.w;
    t2.x = in_COLOR0.z * _WindParams.z;
    t9 = t9 * t1.w + t2.x;
    t0.xyz = t0.xyz * vec3(t9) + t1.xyz;
    t1 = t0.yyyy * glstate_matrix_mvp[1];
    t1 = glstate_matrix_mvp[0] * t0.xxxx + t1;
    t1 = glstate_matrix_mvp[2] * t0.zzzz + t1;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t1;
    t1 = t0.yyyy * _Object2World[1];
    t1 = _Object2World[0] * t0.xxxx + t1;
    t1 = _Object2World[2] * t0.zzzz + t1;
    t1 = _Object2World[3] * in_POSITION0.wwww + t1;
    t2.xyz = t1.yyy * unity_World2Shadow[0][1].xyz;
    t2.xyz = unity_World2Shadow[0][0].xyz * t1.xxx + t2.xyz;
    t2.xyz = unity_World2Shadow[0][2].xyz * t1.zzz + t2.xyz;
    vs_TEXCOORD0.xyz = unity_World2Shadow[0][3].xyz * t1.www + t2.xyz;
    t2.xyz = t1.yyy * unity_World2Shadow[1][1].xyz;
    t2.xyz = unity_World2Shadow[1][0].xyz * t1.xxx + t2.xyz;
    t2.xyz = unity_World2Shadow[1][2].xyz * t1.zzz + t2.xyz;
    vs_TEXCOORD1.xyz = unity_World2Shadow[1][3].xyz * t1.www + t2.xyz;
    t2.xyz = t1.yyy * unity_World2Shadow[2][1].xyz;
    t2.xyz = unity_World2Shadow[2][0].xyz * t1.xxx + t2.xyz;
    t2.xyz = unity_World2Shadow[2][2].xyz * t1.zzz + t2.xyz;
    vs_TEXCOORD2.xyz = unity_World2Shadow[2][3].xyz * t1.www + t2.xyz;
    t2.xyz = t1.yyy * unity_World2Shadow[3][1].xyz;
    t2.xyz = unity_World2Shadow[3][0].xyz * t1.xxx + t2.xyz;
    t2.xyz = unity_World2Shadow[3][2].xyz * t1.zzz + t2.xyz;
    vs_TEXCOORD3.xyz = unity_World2Shadow[3][3].xyz * t1.www + t2.xyz;
    vs_TEXCOORD4.xyz = t1.xyz;
    t3 = t0.y * glstate_matrix_modelview0[1].z;
    t0.x = glstate_matrix_modelview0[0].z * t0.x + t3;
    t0.x = glstate_matrix_modelview0[2].z * t0.z + t0.x;
    t0.x = glstate_matrix_modelview0[3].z * in_POSITION0.w + t0.x;
    vs_TEXCOORD4.w = (-t0.x);
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec3 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
in highp vec4 vs_TEXCOORD4;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec4 t0;
lowp float t10_0;
bvec4 tb0;
highp vec4 t1;
bvec4 tb1;
mediump float t16_2;
void main()
{
    tb0 = greaterThanEqual(vs_TEXCOORD4.wwww, _LightSplitsNear);
    t0 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tb0));
    tb1 = lessThan(vs_TEXCOORD4.wwww, _LightSplitsFar);
    t1 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tb1));
    t0 = t0 * t1;
    t1.xyz = t0.yyy * vs_TEXCOORD1.xyz;
    t1.xyz = vs_TEXCOORD0.xyz * t0.xxx + t1.xyz;
    t0.xyz = vs_TEXCOORD2.xyz * t0.zzz + t1.xyz;
    t0.xyz = vs_TEXCOORD3.xyz * t0.www + t0.xyz;
    vec3 txVec8 = vec3(t0.xy,t0.z);
    t10_0 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec8, 0.0);
    t16_2 = (-_LightShadowData.x) + 1.0;
    t16_2 = t10_0 * t16_2 + _LightShadowData.x;
    t0.x = vs_TEXCOORD4.w * _LightShadowData.z + _LightShadowData.w;
    t0.x = clamp(t0.x, 0.0, 1.0);
    t0.x = t0.x + t16_2;
    t0.x = clamp(t0.x, 0.0, 1.0);
    t1.x = (-vs_TEXCOORD4.w) * _ProjectionParams.w + 1.0;
    t1.xy = t1.xx * vec2(1.0, 255.0);
    t1.xy = fract(t1.xy);
    t0.z = (-t1.y) * 0.00392156886 + t1.x;
    t0.w = t1.y;
    t0.y = 1.0;
    SV_Target0 = t0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_SINGLE_CASCADE" "SHADOWS_NONATIVE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _WindParams;
uniform highp vec4 _WindDir;
uniform highp float _WindDirFromObjNormalOrGlobal;
uniform mediump vec4 _WindFrequency;
uniform mediump vec4 _WindAmplitude;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  highp vec4 tmpvar_2;
  highp vec4 v_position_3;
  v_position_3.w = _glesVertex.w;
  highp vec4 frequency_4;
  frequency_4 = _WindFrequency;
  highp vec4 amplitude_5;
  amplitude_5 = _WindAmplitude;
  highp vec4 waves_6;
  highp vec4 wavesIn_7;
  highp float leafPhase_8;
  highp float objPhase_9;
  lowp float tmpvar_10;
  tmpvar_10 = tmpvar_1.x;
  objPhase_9 = tmpvar_10;
  lowp float tmpvar_11;
  tmpvar_11 = tmpvar_1.y;
  leafPhase_8 = tmpvar_11;
  wavesIn_7 = ((_WindParams.xxxy * frequency_4) + vec4(objPhase_9));
  wavesIn_7.xyz = (wavesIn_7.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_8) * vec3(0.5, 0.5, 0.5))));
  wavesIn_7.xyz = (wavesIn_7.xyz + vec3(leafPhase_8));
  highp vec4 tmpvar_12;
  tmpvar_12 = abs(((
    fract((((
      fract(wavesIn_7)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_6 = (((tmpvar_12 * tmpvar_12) * (3.0 - 
    (2.0 * tmpvar_12)
  )) * amplitude_5);
  v_position_3.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_6.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * _glesColor.w));
  highp vec4 tmpvar_13;
  tmpvar_13.w = 0.0;
  tmpvar_13.xyz = _WindDir.xyz;
  v_position_3.xyz = (v_position_3.xyz + (normalize(
    mix ((_World2Object * tmpvar_13).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * _glesColor.z) * waves_6.w)
   + 
    (_glesColor.z * _WindParams.z)
  )));
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Object2World * v_position_3);
  tmpvar_2.xyz = tmpvar_14.xyz;
  tmpvar_2.w = -((glstate_matrix_modelview0 * v_position_3).z);
  gl_Position = (glstate_matrix_mvp * v_position_3);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_14).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_14).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_14).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_14).xyz;
  xlv_TEXCOORD4 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp sampler2D _ShadowMapTexture;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 cascadeWeights_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[0].xyz);
  highp vec3 tmpvar_6;
  tmpvar_6 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[1].xyz);
  highp vec3 tmpvar_7;
  tmpvar_7 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[2].xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[3].xyz);
  highp vec4 tmpvar_9;
  tmpvar_9.x = dot (tmpvar_5, tmpvar_5);
  tmpvar_9.y = dot (tmpvar_6, tmpvar_6);
  tmpvar_9.z = dot (tmpvar_7, tmpvar_7);
  tmpvar_9.w = dot (tmpvar_8, tmpvar_8);
  bvec4 tmpvar_10;
  tmpvar_10 = lessThan (tmpvar_9, unity_ShadowSplitSqRadii);
  lowp vec4 tmpvar_11;
  tmpvar_11 = vec4(tmpvar_10);
  cascadeWeights_4 = tmpvar_11;
  cascadeWeights_4.yzw = clamp ((cascadeWeights_4.yzw - cascadeWeights_4.xyz), 0.0, 1.0);
  highp vec3 tmpvar_12;
  tmpvar_12 = (xlv_TEXCOORD4.xyz - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_13;
  tmpvar_13 = clamp (((
    sqrt(dot (tmpvar_12, tmpvar_12))
   * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = (((
    (xlv_TEXCOORD0 * cascadeWeights_4.x)
   + 
    (xlv_TEXCOORD1 * cascadeWeights_4.y)
  ) + (xlv_TEXCOORD2 * cascadeWeights_4.z)) + (xlv_TEXCOORD3 * cascadeWeights_4.w));
  highp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_ShadowMapTexture, tmpvar_14.xy);
  mediump float tmpvar_16;
  if ((tmpvar_15.x < tmpvar_14.z)) {
    tmpvar_16 = 0.0;
  } else {
    tmpvar_16 = 1.0;
  };
  shadow_3 = (_LightShadowData.x + (tmpvar_16 * (1.0 - _LightShadowData.x)));
  res_2.x = clamp ((shadow_3 + tmpvar_13), 0.0, 1.0);
  res_2.y = 1.0;
  highp vec2 enc_17;
  highp vec2 tmpvar_18;
  tmpvar_18 = fract((vec2(1.0, 255.0) * (1.0 - 
    (xlv_TEXCOORD4.w * _ProjectionParams.w)
  )));
  enc_17.y = tmpvar_18.y;
  enc_17.x = (tmpvar_18.x - (tmpvar_18.y * 0.003921569));
  res_2.zw = enc_17;
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _WindParams;
uniform highp vec4 _WindDir;
uniform highp float _WindDirFromObjNormalOrGlobal;
uniform mediump vec4 _WindFrequency;
uniform mediump vec4 _WindAmplitude;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  highp vec4 tmpvar_2;
  highp vec4 v_position_3;
  v_position_3.w = _glesVertex.w;
  highp vec4 frequency_4;
  frequency_4 = _WindFrequency;
  highp vec4 amplitude_5;
  amplitude_5 = _WindAmplitude;
  highp vec4 waves_6;
  highp vec4 wavesIn_7;
  highp float leafPhase_8;
  highp float objPhase_9;
  lowp float tmpvar_10;
  tmpvar_10 = tmpvar_1.x;
  objPhase_9 = tmpvar_10;
  lowp float tmpvar_11;
  tmpvar_11 = tmpvar_1.y;
  leafPhase_8 = tmpvar_11;
  wavesIn_7 = ((_WindParams.xxxy * frequency_4) + vec4(objPhase_9));
  wavesIn_7.xyz = (wavesIn_7.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_8) * vec3(0.5, 0.5, 0.5))));
  wavesIn_7.xyz = (wavesIn_7.xyz + vec3(leafPhase_8));
  highp vec4 tmpvar_12;
  tmpvar_12 = abs(((
    fract((((
      fract(wavesIn_7)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_6 = (((tmpvar_12 * tmpvar_12) * (3.0 - 
    (2.0 * tmpvar_12)
  )) * amplitude_5);
  v_position_3.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_6.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * _glesColor.w));
  highp vec4 tmpvar_13;
  tmpvar_13.w = 0.0;
  tmpvar_13.xyz = _WindDir.xyz;
  v_position_3.xyz = (v_position_3.xyz + (normalize(
    mix ((_World2Object * tmpvar_13).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * _glesColor.z) * waves_6.w)
   + 
    (_glesColor.z * _WindParams.z)
  )));
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Object2World * v_position_3);
  tmpvar_2.xyz = tmpvar_14.xyz;
  tmpvar_2.w = -((glstate_matrix_modelview0 * v_position_3).z);
  gl_Position = (glstate_matrix_mvp * v_position_3);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_14).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_14).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_14).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_14).xyz;
  xlv_TEXCOORD4 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform lowp sampler2DShadow _ShadowMapTexture;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 cascadeWeights_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[0].xyz);
  highp vec3 tmpvar_6;
  tmpvar_6 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[1].xyz);
  highp vec3 tmpvar_7;
  tmpvar_7 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[2].xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[3].xyz);
  highp vec4 tmpvar_9;
  tmpvar_9.x = dot (tmpvar_5, tmpvar_5);
  tmpvar_9.y = dot (tmpvar_6, tmpvar_6);
  tmpvar_9.z = dot (tmpvar_7, tmpvar_7);
  tmpvar_9.w = dot (tmpvar_8, tmpvar_8);
  bvec4 tmpvar_10;
  tmpvar_10 = lessThan (tmpvar_9, unity_ShadowSplitSqRadii);
  lowp vec4 tmpvar_11;
  tmpvar_11 = vec4(tmpvar_10);
  cascadeWeights_4 = tmpvar_11;
  cascadeWeights_4.yzw = clamp ((cascadeWeights_4.yzw - cascadeWeights_4.xyz), 0.0, 1.0);
  highp vec3 tmpvar_12;
  tmpvar_12 = (xlv_TEXCOORD4.xyz - unity_ShadowFadeCenterAndType.xyz);
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (((
    (xlv_TEXCOORD0 * cascadeWeights_4.x)
   + 
    (xlv_TEXCOORD1 * cascadeWeights_4.y)
  ) + (xlv_TEXCOORD2 * cascadeWeights_4.z)) + (xlv_TEXCOORD3 * cascadeWeights_4.w));
  lowp float tmpvar_14;
  tmpvar_14 = shadow2DEXT (_ShadowMapTexture, tmpvar_13.xyz);
  mediump float tmpvar_15;
  tmpvar_15 = tmpvar_14;
  shadow_3 = (_LightShadowData.x + (tmpvar_15 * (1.0 - _LightShadowData.x)));
  res_2.x = clamp ((shadow_3 + clamp (
    ((sqrt(dot (tmpvar_12, tmpvar_12)) * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  res_2.y = 1.0;
  highp vec2 enc_16;
  highp vec2 tmpvar_17;
  tmpvar_17 = fract((vec2(1.0, 255.0) * (1.0 - 
    (xlv_TEXCOORD4.w * _ProjectionParams.w)
  )));
  enc_16.y = tmpvar_17.y;
  enc_16.x = (tmpvar_17.x - (tmpvar_17.y * 0.003921569));
  res_2.zw = enc_16;
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in lowp vec4 in_COLOR0;
out highp vec3 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
out highp vec4 vs_TEXCOORD4;
highp vec3 t0;
highp vec4 t1;
highp vec4 t2;
highp float t3;
highp float t9;
void main()
{
    t0.xyz = _World2Object[1].xyz * _WindDir.yyy;
    t0.xyz = _World2Object[0].xyz * _WindDir.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WindDir.zzz + t0.xyz;
    t1.xyz = (-t0.xyz) + in_NORMAL0.xyz;
    t0.xyz = vec3(_WindDirFromObjNormalOrGlobal) * t1.xyz + t0.xyz;
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    t9 = in_COLOR0.y * 0.5 + 0.5;
    t1 = _WindParams.xxxy * _WindFrequency + in_COLOR0.xxxx;
    t1.xyz = t1.xyz * vec3(t9) + in_COLOR0.yyy;
    t2 = fract(t1);
    t1 = t2 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-0.5, -0.5, -0.5, -0.5);
    t1 = fract(t1);
    t1 = t1 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-1.0, -1.0, -1.0, -1.0);
    t2 = abs(t1) * abs(t1);
    t1 = -abs(t1) * vec4(2.0, 2.0, 2.0, 2.0) + vec4(3.0, 3.0, 3.0, 3.0);
    t1 = t1 * t2;
    t1 = t1 * _WindAmplitude;
    t9 = dot(t1.xyz, vec3(1.0, 1.0, 1.0));
    t9 = t9 * _WindParams.w;
    t1.xyz = vec3(t9) * in_NORMAL0.xyz;
    t1.xyz = t1.xyz * in_COLOR0.www + in_POSITION0.xyz;
    t9 = in_COLOR0.z * _WindDir.w;
    t2.x = in_COLOR0.z * _WindParams.z;
    t9 = t9 * t1.w + t2.x;
    t0.xyz = t0.xyz * vec3(t9) + t1.xyz;
    t1 = t0.yyyy * glstate_matrix_mvp[1];
    t1 = glstate_matrix_mvp[0] * t0.xxxx + t1;
    t1 = glstate_matrix_mvp[2] * t0.zzzz + t1;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t1;
    t1 = t0.yyyy * _Object2World[1];
    t1 = _Object2World[0] * t0.xxxx + t1;
    t1 = _Object2World[2] * t0.zzzz + t1;
    t1 = _Object2World[3] * in_POSITION0.wwww + t1;
    t2.xyz = t1.yyy * unity_World2Shadow[0][1].xyz;
    t2.xyz = unity_World2Shadow[0][0].xyz * t1.xxx + t2.xyz;
    t2.xyz = unity_World2Shadow[0][2].xyz * t1.zzz + t2.xyz;
    vs_TEXCOORD0.xyz = unity_World2Shadow[0][3].xyz * t1.www + t2.xyz;
    t2.xyz = t1.yyy * unity_World2Shadow[1][1].xyz;
    t2.xyz = unity_World2Shadow[1][0].xyz * t1.xxx + t2.xyz;
    t2.xyz = unity_World2Shadow[1][2].xyz * t1.zzz + t2.xyz;
    vs_TEXCOORD1.xyz = unity_World2Shadow[1][3].xyz * t1.www + t2.xyz;
    t2.xyz = t1.yyy * unity_World2Shadow[2][1].xyz;
    t2.xyz = unity_World2Shadow[2][0].xyz * t1.xxx + t2.xyz;
    t2.xyz = unity_World2Shadow[2][2].xyz * t1.zzz + t2.xyz;
    vs_TEXCOORD2.xyz = unity_World2Shadow[2][3].xyz * t1.www + t2.xyz;
    t2.xyz = t1.yyy * unity_World2Shadow[3][1].xyz;
    t2.xyz = unity_World2Shadow[3][0].xyz * t1.xxx + t2.xyz;
    t2.xyz = unity_World2Shadow[3][2].xyz * t1.zzz + t2.xyz;
    vs_TEXCOORD3.xyz = unity_World2Shadow[3][3].xyz * t1.www + t2.xyz;
    vs_TEXCOORD4.xyz = t1.xyz;
    t3 = t0.y * glstate_matrix_modelview0[1].z;
    t0.x = glstate_matrix_modelview0[0].z * t0.x + t3;
    t0.x = glstate_matrix_modelview0[2].z * t0.z + t0.x;
    t0.x = glstate_matrix_modelview0[3].z * in_POSITION0.w + t0.x;
    vs_TEXCOORD4.w = (-t0.x);
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec3 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
in highp vec4 vs_TEXCOORD4;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec4 t0;
lowp float t10_0;
bvec4 tb0;
highp vec3 t1;
mediump float t16_2;
highp vec3 t3;
void main()
{
    t0.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowSplitSpheres[0].xyz);
    t0.x = dot(t0.xyz, t0.xyz);
    t1.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowSplitSpheres[1].xyz);
    t0.y = dot(t1.xyz, t1.xyz);
    t1.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowSplitSpheres[2].xyz);
    t0.z = dot(t1.xyz, t1.xyz);
    t1.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowSplitSpheres[3].xyz);
    t0.w = dot(t1.xyz, t1.xyz);
    tb0 = lessThan(t0, unity_ShadowSplitSqRadii);
    t1.x = (tb0.x) ? float(-1.0) : float(-0.0);
    t1.y = (tb0.y) ? float(-1.0) : float(-0.0);
    t1.z = (tb0.z) ? float(-1.0) : float(-0.0);
    t0 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tb0));
    t3.xyz = vec3(t1.x + t0.y, t1.y + t0.z, t1.z + t0.w);
    t3.xyz = max(t3.xyz, vec3(0.0, 0.0, 0.0));
    t1.xyz = t3.xxx * vs_TEXCOORD1.xyz;
    t1.xyz = vs_TEXCOORD0.xyz * t0.xxx + t1.xyz;
    t0.xyz = vs_TEXCOORD2.xyz * t3.yyy + t1.xyz;
    t0.xyz = vs_TEXCOORD3.xyz * t3.zzz + t0.xyz;
    vec3 txVec14 = vec3(t0.xy,t0.z);
    t10_0 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec14, 0.0);
    t16_2 = (-_LightShadowData.x) + 1.0;
    t16_2 = t10_0 * t16_2 + _LightShadowData.x;
    t0.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowFadeCenterAndType.xyz);
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t0.x = t0.x * _LightShadowData.z + _LightShadowData.w;
    t0.x = clamp(t0.x, 0.0, 1.0);
    t0.x = t0.x + t16_2;
    t0.x = clamp(t0.x, 0.0, 1.0);
    t1.x = (-vs_TEXCOORD4.w) * _ProjectionParams.w + 1.0;
    t1.xy = t1.xx * vec2(1.0, 255.0);
    t1.xy = fract(t1.xy);
    t0.z = (-t1.y) * 0.00392156886 + t1.x;
    t0.w = t1.y;
    t0.y = 1.0;
    SV_Target0 = t0;
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SHADOWS_NONATIVE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_NATIVE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_NATIVE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NONATIVE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_SINGLE_CASCADE" "SHADOWS_NONATIVE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_SINGLE_CASCADE" "SHADOWS_NONATIVE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
"!!GLES3"
}
}
 }
}
}
Shader "Effects/Godray" {
Properties {
 _MainTex ("Particle Texture", 2D) = "white" { }
 _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,1)
 _BoostAlpha ("Boost Alpha", Range(0,10)) = 1
 _CamHeightOffset ("Camera Height Offset", Range(-10,10)) = 4
 _FadeDistance ("Camera Fade Distance", Range(0,60)) = 30
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha One
  ColorMask RGB
  GpuProgramID 41959
Program "vp" {
SubProgram "gles " {
Keywords { "FOG_OFF" "TINT_COLOR_OFF" "VERTEX_COLOR_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump vec4 _MainTex_ST;
varying mediump vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD3;
varying highp float xlv_TEXCOORD4;
varying highp float xlv_TEXCOORD5;
void main ()
{
  mediump vec2 tmpvar_1;
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec4 tmpvar_2;
  tmpvar_2.w = 0.0;
  tmpvar_2.xyz = _glesNormal;
  highp vec4 tmpvar_3;
  tmpvar_3 = (_Object2World * _glesVertex);
  highp vec3 tmpvar_4;
  tmpvar_4 = (_WorldSpaceCameraPos - tmpvar_3.xyz);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD3 = normalize((tmpvar_2 * _World2Object).xyz);
  xlv_TEXCOORD4 = sqrt(dot (tmpvar_4, tmpvar_4));
  xlv_TEXCOORD5 = (_WorldSpaceCameraPos.y - tmpvar_3.y);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform highp float _BoostAlpha;
uniform highp float _CamHeightOffset;
uniform highp float _FadeDistance;
varying mediump vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD3;
varying highp float xlv_TEXCOORD4;
varying highp float xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.xyz = tmpvar_2.xyz;
  highp float tmpvar_3;
  tmpvar_3 = clamp ((_CamHeightOffset - xlv_TEXCOORD5), 0.0, 1.0);
  c_1.w = (tmpvar_2.w * tmpvar_3);
  c_1.w = (c_1.w * xlv_TEXCOORD3.y);
  highp float tmpvar_4;
  tmpvar_4 = sqrt((1.0 - clamp (
    ((xlv_TEXCOORD4 - 1.0) / (_FadeDistance - 1.0))
  , 0.0, 1.0)));
  c_1.w = (c_1.w * (tmpvar_4 * _BoostAlpha));
  gl_FragData[0] = c_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "FOG_OFF" "TINT_COLOR_OFF" "VERTEX_COLOR_OFF" }
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	lowp vec4 _TintColor;
uniform 	float _BoostAlpha;
uniform 	float _CamHeightOffset;
uniform 	float _FadeDistance;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec2 vs_TEXCOORD0;
mediump  vec4 phase0_Output0_1;
out highp float vs_TEXCOORD4;
out highp float vs_TEXCOORD5;
out highp vec3 vs_TEXCOORD3;
highp vec4 t0;
highp float t1;
highp float t3;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    phase0_Output0_1.xy = t0.xy;
    t0.x = dot(in_NORMAL0.xyz, _World2Object[0].xyz);
    t0.y = dot(in_NORMAL0.xyz, _World2Object[1].xyz);
    t0.z = dot(in_NORMAL0.xyz, _World2Object[2].xyz);
    t3 = dot(t0.xyz, t0.xyz);
    t3 = inversesqrt(t3);
    vs_TEXCOORD3.xyz = vec3(t3) * t0.xyz;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.yzw = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t1 = dot(t0.yzw, t0.yzw);
    t0.x = sqrt(t1);
    phase0_Output0_1 = t0.xz;
vs_TEXCOORD0 = phase0_Output0_1.xy;
vs_TEXCOORD4 = phase0_Output0_1.z;
vs_TEXCOORD5 = phase0_Output0_1.w;
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	lowp vec4 _TintColor;
uniform 	float _BoostAlpha;
uniform 	float _CamHeightOffset;
uniform 	float _FadeDistance;
uniform lowp sampler2D _MainTex;
in mediump vec2 vs_TEXCOORD0;
in highp float vs_TEXCOORD4;
in highp float vs_TEXCOORD5;
in highp vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
highp float t0;
highp vec4 t1;
highp float t2;
void main()
{
    t0 = vs_TEXCOORD4 + -1.0;
    t2 = _FadeDistance + -1.0;
    t0 = t0 / t2;
    t0 = clamp(t0, 0.0, 1.0);
    t0 = (-t0) + 1.0;
    t0 = sqrt(t0);
    t0 = t0 * _BoostAlpha;
    t2 = vs_TEXCOORD5 + (-_CamHeightOffset);
    t2 = (-t2);
    t2 = clamp(t2, 0.0, 1.0);
    t1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t2 = t2 * t1.w;
    t2 = t2 * vs_TEXCOORD3.y;
    t1.w = t0 * t2;
    SV_Target0 = t1;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "TINT_COLOR_OFF" "VERTEX_COLOR_OFF" "FOG_ON" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 acid_Fog;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp float xlv_TEXCOORD4;
varying highp float xlv_TEXCOORD5;
void main ()
{
  mediump float fogpos_1;
  mediump vec2 tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_2 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp float tmpvar_4;
  highp vec3 x_5;
  x_5 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_4 = sqrt(dot (x_5, x_5));
  fogpos_1 = tmpvar_4;
  tmpvar_3.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_3.x = (1.0 - tmpvar_3.y);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 0.0;
  tmpvar_6.xyz = _glesNormal;
  highp vec4 tmpvar_7;
  tmpvar_7 = (_Object2World * _glesVertex);
  highp vec3 tmpvar_8;
  tmpvar_8 = (_WorldSpaceCameraPos - tmpvar_7.xyz);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
  xlv_TEXCOORD3 = normalize((tmpvar_6 * _World2Object).xyz);
  xlv_TEXCOORD4 = sqrt(dot (tmpvar_8, tmpvar_8));
  xlv_TEXCOORD5 = (_WorldSpaceCameraPos.y - tmpvar_7.y);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform highp float _BoostAlpha;
uniform highp float _CamHeightOffset;
uniform highp float _FadeDistance;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp float xlv_TEXCOORD4;
varying highp float xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.xyz = (tmpvar_2.xyz * xlv_TEXCOORD2.x);
  highp float tmpvar_3;
  tmpvar_3 = clamp ((_CamHeightOffset - xlv_TEXCOORD5), 0.0, 1.0);
  c_1.w = (tmpvar_2.w * tmpvar_3);
  c_1.w = (c_1.w * xlv_TEXCOORD3.y);
  highp float tmpvar_4;
  tmpvar_4 = sqrt((1.0 - clamp (
    ((xlv_TEXCOORD4 - 1.0) / (_FadeDistance - 1.0))
  , 0.0, 1.0)));
  c_1.w = (c_1.w * (tmpvar_4 * _BoostAlpha));
  gl_FragData[0] = c_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "TINT_COLOR_OFF" "VERTEX_COLOR_OFF" "FOG_ON" }
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	lowp vec4 _TintColor;
uniform 	mediump vec4 acid_Fog;
uniform 	float _BoostAlpha;
uniform 	float _CamHeightOffset;
uniform 	float _FadeDistance;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
out highp float vs_TEXCOORD4;
out highp float vs_TEXCOORD5;
highp vec4 t0;
mediump float t16_1;
mediump float t16_3;
highp float t6;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_3 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_3;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_3 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_3;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD0.xy = t0.xy;
    t0.x = dot(in_NORMAL0.xyz, _World2Object[0].xyz);
    t0.y = dot(in_NORMAL0.xyz, _World2Object[1].xyz);
    t0.z = dot(in_NORMAL0.xyz, _World2Object[2].xyz);
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    vs_TEXCOORD3.xyz = vec3(t6) * t0.xyz;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    vs_TEXCOORD5 = t0.y;
    vs_TEXCOORD4 = sqrt(t0.x);
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	lowp vec4 _TintColor;
uniform 	mediump vec4 acid_Fog;
uniform 	float _BoostAlpha;
uniform 	float _CamHeightOffset;
uniform 	float _FadeDistance;
uniform lowp sampler2D _MainTex;
in mediump vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
in highp float vs_TEXCOORD4;
in highp float vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
highp float t0;
lowp vec4 t10_1;
mediump vec3 t16_2;
highp float t3;
void main()
{
    t0 = vs_TEXCOORD4 + -1.0;
    t3 = _FadeDistance + -1.0;
    t0 = t0 / t3;
    t0 = clamp(t0, 0.0, 1.0);
    t0 = (-t0) + 1.0;
    t0 = sqrt(t0);
    t0 = t0 * _BoostAlpha;
    t3 = vs_TEXCOORD5 + (-_CamHeightOffset);
    t3 = (-t3);
    t3 = clamp(t3, 0.0, 1.0);
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t3 = t3 * t10_1.w;
    t16_2.xyz = t10_1.xyz * vs_TEXCOORD2.xxx;
    SV_Target0.xyz = t16_2.xyz;
    t3 = t3 * vs_TEXCOORD3.y;
    t0 = t0 * t3;
    SV_Target0.w = t0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "FOG_OFF" "VERTEX_COLOR_ON" "TINT_COLOR_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump vec4 _MainTex_ST;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD3;
varying highp float xlv_TEXCOORD4;
varying highp float xlv_TEXCOORD5;
void main ()
{
  mediump vec2 tmpvar_1;
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec4 tmpvar_2;
  tmpvar_2.w = 0.0;
  tmpvar_2.xyz = _glesNormal;
  highp vec4 tmpvar_3;
  tmpvar_3 = (_Object2World * _glesVertex);
  highp vec3 tmpvar_4;
  tmpvar_4 = (_WorldSpaceCameraPos - tmpvar_3.xyz);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = _glesColor;
  xlv_TEXCOORD3 = normalize((tmpvar_2 * _World2Object).xyz);
  xlv_TEXCOORD4 = sqrt(dot (tmpvar_4, tmpvar_4));
  xlv_TEXCOORD5 = (_WorldSpaceCameraPos.y - tmpvar_3.y);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform highp float _BoostAlpha;
uniform highp float _CamHeightOffset;
uniform highp float _FadeDistance;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD3;
varying highp float xlv_TEXCOORD4;
varying highp float xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  c_1 = (texture2D (_MainTex, xlv_TEXCOORD0) * xlv_TEXCOORD1);
  highp float tmpvar_2;
  tmpvar_2 = clamp ((_CamHeightOffset - xlv_TEXCOORD5), 0.0, 1.0);
  c_1.w = (c_1.w * tmpvar_2);
  c_1.w = (c_1.w * xlv_TEXCOORD3.y);
  highp float tmpvar_3;
  tmpvar_3 = sqrt((1.0 - clamp (
    ((xlv_TEXCOORD4 - 1.0) / (_FadeDistance - 1.0))
  , 0.0, 1.0)));
  c_1.w = (c_1.w * (tmpvar_3 * _BoostAlpha));
  gl_FragData[0] = c_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "FOG_OFF" "VERTEX_COLOR_ON" "TINT_COLOR_OFF" }
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	lowp vec4 _TintColor;
uniform 	float _BoostAlpha;
uniform 	float _CamHeightOffset;
uniform 	float _FadeDistance;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in lowp vec4 in_COLOR0;
out mediump vec2 vs_TEXCOORD0;
mediump  vec4 phase0_Output0_1;
out highp float vs_TEXCOORD4;
out highp float vs_TEXCOORD5;
out lowp vec4 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD3;
highp vec4 t0;
highp float t1;
highp float t3;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    phase0_Output0_1.xy = t0.xy;
    vs_TEXCOORD1 = in_COLOR0;
    t0.x = dot(in_NORMAL0.xyz, _World2Object[0].xyz);
    t0.y = dot(in_NORMAL0.xyz, _World2Object[1].xyz);
    t0.z = dot(in_NORMAL0.xyz, _World2Object[2].xyz);
    t3 = dot(t0.xyz, t0.xyz);
    t3 = inversesqrt(t3);
    vs_TEXCOORD3.xyz = vec3(t3) * t0.xyz;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.yzw = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t1 = dot(t0.yzw, t0.yzw);
    t0.x = sqrt(t1);
    phase0_Output0_1 = t0.xz;
vs_TEXCOORD0 = phase0_Output0_1.xy;
vs_TEXCOORD4 = phase0_Output0_1.z;
vs_TEXCOORD5 = phase0_Output0_1.w;
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	lowp vec4 _TintColor;
uniform 	float _BoostAlpha;
uniform 	float _CamHeightOffset;
uniform 	float _FadeDistance;
uniform lowp sampler2D _MainTex;
in mediump vec2 vs_TEXCOORD0;
in highp float vs_TEXCOORD4;
in highp float vs_TEXCOORD5;
in lowp vec4 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
highp float t0;
lowp vec4 t10_1;
highp float t2;
void main()
{
    t0 = vs_TEXCOORD4 + -1.0;
    t2 = _FadeDistance + -1.0;
    t0 = t0 / t2;
    t0 = clamp(t0, 0.0, 1.0);
    t0 = (-t0) + 1.0;
    t0 = sqrt(t0);
    t0 = t0 * _BoostAlpha;
    t2 = vs_TEXCOORD5 + (-_CamHeightOffset);
    t2 = (-t2);
    t2 = clamp(t2, 0.0, 1.0);
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t10_1 = t10_1 * vs_TEXCOORD1;
    t2 = t2 * t10_1.w;
    SV_Target0.xyz = t10_1.xyz;
    t2 = t2 * vs_TEXCOORD3.y;
    t0 = t0 * t2;
    SV_Target0.w = t0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "VERTEX_COLOR_ON" "TINT_COLOR_OFF" "FOG_ON" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 acid_Fog;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp float xlv_TEXCOORD4;
varying highp float xlv_TEXCOORD5;
void main ()
{
  mediump float fogpos_1;
  mediump vec2 tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_2 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp float tmpvar_4;
  highp vec3 x_5;
  x_5 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_4 = sqrt(dot (x_5, x_5));
  fogpos_1 = tmpvar_4;
  tmpvar_3.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_3.x = (1.0 - tmpvar_3.y);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 0.0;
  tmpvar_6.xyz = _glesNormal;
  highp vec4 tmpvar_7;
  tmpvar_7 = (_Object2World * _glesVertex);
  highp vec3 tmpvar_8;
  tmpvar_8 = (_WorldSpaceCameraPos - tmpvar_7.xyz);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_2;
  xlv_TEXCOORD1 = _glesColor;
  xlv_TEXCOORD2 = tmpvar_3;
  xlv_TEXCOORD3 = normalize((tmpvar_6 * _World2Object).xyz);
  xlv_TEXCOORD4 = sqrt(dot (tmpvar_8, tmpvar_8));
  xlv_TEXCOORD5 = (_WorldSpaceCameraPos.y - tmpvar_7.y);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform highp float _BoostAlpha;
uniform highp float _CamHeightOffset;
uniform highp float _FadeDistance;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp float xlv_TEXCOORD4;
varying highp float xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  c_1 = (texture2D (_MainTex, xlv_TEXCOORD0) * xlv_TEXCOORD1);
  c_1.xyz = (c_1.xyz * xlv_TEXCOORD2.x);
  highp float tmpvar_2;
  tmpvar_2 = clamp ((_CamHeightOffset - xlv_TEXCOORD5), 0.0, 1.0);
  c_1.w = (c_1.w * tmpvar_2);
  c_1.w = (c_1.w * xlv_TEXCOORD3.y);
  highp float tmpvar_3;
  tmpvar_3 = sqrt((1.0 - clamp (
    ((xlv_TEXCOORD4 - 1.0) / (_FadeDistance - 1.0))
  , 0.0, 1.0)));
  c_1.w = (c_1.w * (tmpvar_3 * _BoostAlpha));
  gl_FragData[0] = c_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "VERTEX_COLOR_ON" "TINT_COLOR_OFF" "FOG_ON" }
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	lowp vec4 _TintColor;
uniform 	mediump vec4 acid_Fog;
uniform 	float _BoostAlpha;
uniform 	float _CamHeightOffset;
uniform 	float _FadeDistance;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in lowp vec4 in_COLOR0;
out mediump vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD2;
out lowp vec4 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD3;
out highp float vs_TEXCOORD4;
out highp float vs_TEXCOORD5;
highp vec4 t0;
mediump float t16_1;
mediump float t16_3;
highp float t6;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_3 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_3;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_3 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_3;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD0.xy = t0.xy;
    vs_TEXCOORD1 = in_COLOR0;
    t0.x = dot(in_NORMAL0.xyz, _World2Object[0].xyz);
    t0.y = dot(in_NORMAL0.xyz, _World2Object[1].xyz);
    t0.z = dot(in_NORMAL0.xyz, _World2Object[2].xyz);
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    vs_TEXCOORD3.xyz = vec3(t6) * t0.xyz;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    vs_TEXCOORD5 = t0.y;
    vs_TEXCOORD4 = sqrt(t0.x);
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	lowp vec4 _TintColor;
uniform 	mediump vec4 acid_Fog;
uniform 	float _BoostAlpha;
uniform 	float _CamHeightOffset;
uniform 	float _FadeDistance;
uniform lowp sampler2D _MainTex;
in mediump vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD2;
in lowp vec4 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD3;
in highp float vs_TEXCOORD4;
in highp float vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
highp float t0;
lowp vec4 t10_1;
mediump vec3 t16_2;
highp float t3;
void main()
{
    t0 = vs_TEXCOORD4 + -1.0;
    t3 = _FadeDistance + -1.0;
    t0 = t0 / t3;
    t0 = clamp(t0, 0.0, 1.0);
    t0 = (-t0) + 1.0;
    t0 = sqrt(t0);
    t0 = t0 * _BoostAlpha;
    t3 = vs_TEXCOORD5 + (-_CamHeightOffset);
    t3 = (-t3);
    t3 = clamp(t3, 0.0, 1.0);
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t10_1 = t10_1 * vs_TEXCOORD1;
    t3 = t3 * t10_1.w;
    t16_2.xyz = t10_1.xyz * vs_TEXCOORD2.xxx;
    SV_Target0.xyz = t16_2.xyz;
    t3 = t3 * vs_TEXCOORD3.y;
    t0 = t0 * t3;
    SV_Target0.w = t0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "FOG_OFF" "TINT_COLOR_ON" "VERTEX_COLOR_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump vec4 _MainTex_ST;
uniform lowp vec4 _TintColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD3;
varying highp float xlv_TEXCOORD4;
varying highp float xlv_TEXCOORD5;
void main ()
{
  mediump vec2 tmpvar_1;
  lowp vec4 tmpvar_2;
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_2.w = _TintColor.w;
  tmpvar_2.xyz = (_TintColor.xyz * 2.0);
  highp vec4 tmpvar_3;
  tmpvar_3.w = 0.0;
  tmpvar_3.xyz = _glesNormal;
  highp vec4 tmpvar_4;
  tmpvar_4 = (_Object2World * _glesVertex);
  highp vec3 tmpvar_5;
  tmpvar_5 = (_WorldSpaceCameraPos - tmpvar_4.xyz);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD3 = normalize((tmpvar_3 * _World2Object).xyz);
  xlv_TEXCOORD4 = sqrt(dot (tmpvar_5, tmpvar_5));
  xlv_TEXCOORD5 = (_WorldSpaceCameraPos.y - tmpvar_4.y);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform highp float _BoostAlpha;
uniform highp float _CamHeightOffset;
uniform highp float _FadeDistance;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD3;
varying highp float xlv_TEXCOORD4;
varying highp float xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  c_1 = (texture2D (_MainTex, xlv_TEXCOORD0) * xlv_TEXCOORD1);
  highp float tmpvar_2;
  tmpvar_2 = clamp ((_CamHeightOffset - xlv_TEXCOORD5), 0.0, 1.0);
  c_1.w = (c_1.w * tmpvar_2);
  c_1.w = (c_1.w * xlv_TEXCOORD3.y);
  highp float tmpvar_3;
  tmpvar_3 = sqrt((1.0 - clamp (
    ((xlv_TEXCOORD4 - 1.0) / (_FadeDistance - 1.0))
  , 0.0, 1.0)));
  c_1.w = (c_1.w * (tmpvar_3 * _BoostAlpha));
  gl_FragData[0] = c_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "FOG_OFF" "TINT_COLOR_ON" "VERTEX_COLOR_OFF" }
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	lowp vec4 _TintColor;
uniform 	float _BoostAlpha;
uniform 	float _CamHeightOffset;
uniform 	float _FadeDistance;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec2 vs_TEXCOORD0;
mediump  vec4 phase0_Output0_1;
out highp float vs_TEXCOORD4;
out highp float vs_TEXCOORD5;
out lowp vec4 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD3;
highp vec4 t0;
highp float t1;
highp float t3;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    phase0_Output0_1.xy = t0.xy;
    vs_TEXCOORD1 = _TintColor * vec4(2.0, 2.0, 2.0, 1.0);
    t0.x = dot(in_NORMAL0.xyz, _World2Object[0].xyz);
    t0.y = dot(in_NORMAL0.xyz, _World2Object[1].xyz);
    t0.z = dot(in_NORMAL0.xyz, _World2Object[2].xyz);
    t3 = dot(t0.xyz, t0.xyz);
    t3 = inversesqrt(t3);
    vs_TEXCOORD3.xyz = vec3(t3) * t0.xyz;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.yzw = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t1 = dot(t0.yzw, t0.yzw);
    t0.x = sqrt(t1);
    phase0_Output0_1 = t0.xz;
vs_TEXCOORD0 = phase0_Output0_1.xy;
vs_TEXCOORD4 = phase0_Output0_1.z;
vs_TEXCOORD5 = phase0_Output0_1.w;
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	lowp vec4 _TintColor;
uniform 	float _BoostAlpha;
uniform 	float _CamHeightOffset;
uniform 	float _FadeDistance;
uniform lowp sampler2D _MainTex;
in mediump vec2 vs_TEXCOORD0;
in highp float vs_TEXCOORD4;
in highp float vs_TEXCOORD5;
in lowp vec4 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
highp float t0;
lowp vec4 t10_1;
highp float t2;
void main()
{
    t0 = vs_TEXCOORD4 + -1.0;
    t2 = _FadeDistance + -1.0;
    t0 = t0 / t2;
    t0 = clamp(t0, 0.0, 1.0);
    t0 = (-t0) + 1.0;
    t0 = sqrt(t0);
    t0 = t0 * _BoostAlpha;
    t2 = vs_TEXCOORD5 + (-_CamHeightOffset);
    t2 = (-t2);
    t2 = clamp(t2, 0.0, 1.0);
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t10_1 = t10_1 * vs_TEXCOORD1;
    t2 = t2 * t10_1.w;
    SV_Target0.xyz = t10_1.xyz;
    t2 = t2 * vs_TEXCOORD3.y;
    t0 = t0 * t2;
    SV_Target0.w = t0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "TINT_COLOR_ON" "VERTEX_COLOR_OFF" "FOG_ON" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump vec4 _MainTex_ST;
uniform lowp vec4 _TintColor;
uniform mediump vec4 acid_Fog;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp float xlv_TEXCOORD4;
varying highp float xlv_TEXCOORD5;
void main ()
{
  mediump float fogpos_1;
  mediump vec2 tmpvar_2;
  lowp vec4 tmpvar_3;
  mediump vec2 tmpvar_4;
  tmpvar_2 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.w = _TintColor.w;
  tmpvar_3.xyz = (_TintColor.xyz * 2.0);
  highp float tmpvar_5;
  highp vec3 x_6;
  x_6 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_5 = sqrt(dot (x_6, x_6));
  fogpos_1 = tmpvar_5;
  tmpvar_4.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_4.x = (1.0 - tmpvar_4.y);
  highp vec4 tmpvar_7;
  tmpvar_7.w = 0.0;
  tmpvar_7.xyz = _glesNormal;
  highp vec4 tmpvar_8;
  tmpvar_8 = (_Object2World * _glesVertex);
  highp vec3 tmpvar_9;
  tmpvar_9 = (_WorldSpaceCameraPos - tmpvar_8.xyz);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_2;
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = normalize((tmpvar_7 * _World2Object).xyz);
  xlv_TEXCOORD4 = sqrt(dot (tmpvar_9, tmpvar_9));
  xlv_TEXCOORD5 = (_WorldSpaceCameraPos.y - tmpvar_8.y);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform highp float _BoostAlpha;
uniform highp float _CamHeightOffset;
uniform highp float _FadeDistance;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp float xlv_TEXCOORD4;
varying highp float xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  c_1 = (texture2D (_MainTex, xlv_TEXCOORD0) * xlv_TEXCOORD1);
  c_1.xyz = (c_1.xyz * xlv_TEXCOORD2.x);
  highp float tmpvar_2;
  tmpvar_2 = clamp ((_CamHeightOffset - xlv_TEXCOORD5), 0.0, 1.0);
  c_1.w = (c_1.w * tmpvar_2);
  c_1.w = (c_1.w * xlv_TEXCOORD3.y);
  highp float tmpvar_3;
  tmpvar_3 = sqrt((1.0 - clamp (
    ((xlv_TEXCOORD4 - 1.0) / (_FadeDistance - 1.0))
  , 0.0, 1.0)));
  c_1.w = (c_1.w * (tmpvar_3 * _BoostAlpha));
  gl_FragData[0] = c_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "TINT_COLOR_ON" "VERTEX_COLOR_OFF" "FOG_ON" }
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	lowp vec4 _TintColor;
uniform 	mediump vec4 acid_Fog;
uniform 	float _BoostAlpha;
uniform 	float _CamHeightOffset;
uniform 	float _FadeDistance;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD2;
out lowp vec4 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD3;
out highp float vs_TEXCOORD4;
out highp float vs_TEXCOORD5;
highp vec4 t0;
mediump float t16_1;
mediump float t16_3;
highp float t6;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_3 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_3;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_3 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_3;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD0.xy = t0.xy;
    vs_TEXCOORD1 = _TintColor * vec4(2.0, 2.0, 2.0, 1.0);
    t0.x = dot(in_NORMAL0.xyz, _World2Object[0].xyz);
    t0.y = dot(in_NORMAL0.xyz, _World2Object[1].xyz);
    t0.z = dot(in_NORMAL0.xyz, _World2Object[2].xyz);
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    vs_TEXCOORD3.xyz = vec3(t6) * t0.xyz;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    vs_TEXCOORD5 = t0.y;
    vs_TEXCOORD4 = sqrt(t0.x);
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	lowp vec4 _TintColor;
uniform 	mediump vec4 acid_Fog;
uniform 	float _BoostAlpha;
uniform 	float _CamHeightOffset;
uniform 	float _FadeDistance;
uniform lowp sampler2D _MainTex;
in mediump vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD2;
in lowp vec4 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD3;
in highp float vs_TEXCOORD4;
in highp float vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
highp float t0;
lowp vec4 t10_1;
mediump vec3 t16_2;
highp float t3;
void main()
{
    t0 = vs_TEXCOORD4 + -1.0;
    t3 = _FadeDistance + -1.0;
    t0 = t0 / t3;
    t0 = clamp(t0, 0.0, 1.0);
    t0 = (-t0) + 1.0;
    t0 = sqrt(t0);
    t0 = t0 * _BoostAlpha;
    t3 = vs_TEXCOORD5 + (-_CamHeightOffset);
    t3 = (-t3);
    t3 = clamp(t3, 0.0, 1.0);
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t10_1 = t10_1 * vs_TEXCOORD1;
    t3 = t3 * t10_1.w;
    t16_2.xyz = t10_1.xyz * vs_TEXCOORD2.xxx;
    SV_Target0.xyz = t16_2.xyz;
    t3 = t3 * vs_TEXCOORD3.y;
    t0 = t0 * t3;
    SV_Target0.w = t0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "FOG_OFF" "TINT_COLOR_ON" "VERTEX_COLOR_ON" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump vec4 _MainTex_ST;
uniform lowp vec4 _TintColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD3;
varying highp float xlv_TEXCOORD4;
varying highp float xlv_TEXCOORD5;
void main ()
{
  mediump vec2 tmpvar_1;
  lowp vec4 tmpvar_2;
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_2.w = _TintColor.w;
  tmpvar_2.xyz = (_TintColor.xyz * 2.0);
  tmpvar_2 = (tmpvar_2 * _glesColor);
  highp vec4 tmpvar_3;
  tmpvar_3.w = 0.0;
  tmpvar_3.xyz = _glesNormal;
  highp vec4 tmpvar_4;
  tmpvar_4 = (_Object2World * _glesVertex);
  highp vec3 tmpvar_5;
  tmpvar_5 = (_WorldSpaceCameraPos - tmpvar_4.xyz);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD3 = normalize((tmpvar_3 * _World2Object).xyz);
  xlv_TEXCOORD4 = sqrt(dot (tmpvar_5, tmpvar_5));
  xlv_TEXCOORD5 = (_WorldSpaceCameraPos.y - tmpvar_4.y);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform highp float _BoostAlpha;
uniform highp float _CamHeightOffset;
uniform highp float _FadeDistance;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD3;
varying highp float xlv_TEXCOORD4;
varying highp float xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  c_1 = (texture2D (_MainTex, xlv_TEXCOORD0) * xlv_TEXCOORD1);
  highp float tmpvar_2;
  tmpvar_2 = clamp ((_CamHeightOffset - xlv_TEXCOORD5), 0.0, 1.0);
  c_1.w = (c_1.w * tmpvar_2);
  c_1.w = (c_1.w * xlv_TEXCOORD3.y);
  highp float tmpvar_3;
  tmpvar_3 = sqrt((1.0 - clamp (
    ((xlv_TEXCOORD4 - 1.0) / (_FadeDistance - 1.0))
  , 0.0, 1.0)));
  c_1.w = (c_1.w * (tmpvar_3 * _BoostAlpha));
  gl_FragData[0] = c_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "FOG_OFF" "TINT_COLOR_ON" "VERTEX_COLOR_ON" }
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	lowp vec4 _TintColor;
uniform 	float _BoostAlpha;
uniform 	float _CamHeightOffset;
uniform 	float _FadeDistance;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in lowp vec4 in_COLOR0;
out mediump vec2 vs_TEXCOORD0;
mediump  vec4 phase0_Output0_1;
out highp float vs_TEXCOORD4;
out highp float vs_TEXCOORD5;
out lowp vec4 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD3;
highp vec4 t0;
lowp vec3 t10_1;
highp float t2;
highp float t6;
lowp float t10_7;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    phase0_Output0_1.xy = t0.xy;
    t10_1.xyz = in_COLOR0.xyz * _TintColor.xyz;
    t10_7 = 2.0;
    vs_TEXCOORD1.xyz = vec3(t10_7) * t10_1.xyz;
    t10_1.x = in_COLOR0.w;
    vs_TEXCOORD1.w = t10_1.x * _TintColor.w;
    t0.x = dot(in_NORMAL0.xyz, _World2Object[0].xyz);
    t0.y = dot(in_NORMAL0.xyz, _World2Object[1].xyz);
    t0.z = dot(in_NORMAL0.xyz, _World2Object[2].xyz);
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    vs_TEXCOORD3.xyz = vec3(t6) * t0.xyz;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.yzw = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t2 = dot(t0.yzw, t0.yzw);
    t0.x = sqrt(t2);
    phase0_Output0_1 = t0.xz;
vs_TEXCOORD0 = phase0_Output0_1.xy;
vs_TEXCOORD4 = phase0_Output0_1.z;
vs_TEXCOORD5 = phase0_Output0_1.w;
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	lowp vec4 _TintColor;
uniform 	float _BoostAlpha;
uniform 	float _CamHeightOffset;
uniform 	float _FadeDistance;
uniform lowp sampler2D _MainTex;
in mediump vec2 vs_TEXCOORD0;
in highp float vs_TEXCOORD4;
in highp float vs_TEXCOORD5;
in lowp vec4 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
highp float t0;
lowp vec4 t10_1;
highp float t2;
void main()
{
    t0 = vs_TEXCOORD4 + -1.0;
    t2 = _FadeDistance + -1.0;
    t0 = t0 / t2;
    t0 = clamp(t0, 0.0, 1.0);
    t0 = (-t0) + 1.0;
    t0 = sqrt(t0);
    t0 = t0 * _BoostAlpha;
    t2 = vs_TEXCOORD5 + (-_CamHeightOffset);
    t2 = (-t2);
    t2 = clamp(t2, 0.0, 1.0);
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t10_1 = t10_1 * vs_TEXCOORD1;
    t2 = t2 * t10_1.w;
    SV_Target0.xyz = t10_1.xyz;
    t2 = t2 * vs_TEXCOORD3.y;
    t0 = t0 * t2;
    SV_Target0.w = t0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "TINT_COLOR_ON" "VERTEX_COLOR_ON" "FOG_ON" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump vec4 _MainTex_ST;
uniform lowp vec4 _TintColor;
uniform mediump vec4 acid_Fog;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp float xlv_TEXCOORD4;
varying highp float xlv_TEXCOORD5;
void main ()
{
  mediump float fogpos_1;
  mediump vec2 tmpvar_2;
  lowp vec4 tmpvar_3;
  mediump vec2 tmpvar_4;
  tmpvar_2 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.w = _TintColor.w;
  tmpvar_3.xyz = (_TintColor.xyz * 2.0);
  tmpvar_3 = (tmpvar_3 * _glesColor);
  highp float tmpvar_5;
  highp vec3 x_6;
  x_6 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_5 = sqrt(dot (x_6, x_6));
  fogpos_1 = tmpvar_5;
  tmpvar_4.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_4.x = (1.0 - tmpvar_4.y);
  highp vec4 tmpvar_7;
  tmpvar_7.w = 0.0;
  tmpvar_7.xyz = _glesNormal;
  highp vec4 tmpvar_8;
  tmpvar_8 = (_Object2World * _glesVertex);
  highp vec3 tmpvar_9;
  tmpvar_9 = (_WorldSpaceCameraPos - tmpvar_8.xyz);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_2;
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = normalize((tmpvar_7 * _World2Object).xyz);
  xlv_TEXCOORD4 = sqrt(dot (tmpvar_9, tmpvar_9));
  xlv_TEXCOORD5 = (_WorldSpaceCameraPos.y - tmpvar_8.y);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform highp float _BoostAlpha;
uniform highp float _CamHeightOffset;
uniform highp float _FadeDistance;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp float xlv_TEXCOORD4;
varying highp float xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  c_1 = (texture2D (_MainTex, xlv_TEXCOORD0) * xlv_TEXCOORD1);
  c_1.xyz = (c_1.xyz * xlv_TEXCOORD2.x);
  highp float tmpvar_2;
  tmpvar_2 = clamp ((_CamHeightOffset - xlv_TEXCOORD5), 0.0, 1.0);
  c_1.w = (c_1.w * tmpvar_2);
  c_1.w = (c_1.w * xlv_TEXCOORD3.y);
  highp float tmpvar_3;
  tmpvar_3 = sqrt((1.0 - clamp (
    ((xlv_TEXCOORD4 - 1.0) / (_FadeDistance - 1.0))
  , 0.0, 1.0)));
  c_1.w = (c_1.w * (tmpvar_3 * _BoostAlpha));
  gl_FragData[0] = c_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "TINT_COLOR_ON" "VERTEX_COLOR_ON" "FOG_ON" }
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	lowp vec4 _TintColor;
uniform 	mediump vec4 acid_Fog;
uniform 	float _BoostAlpha;
uniform 	float _CamHeightOffset;
uniform 	float _FadeDistance;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in lowp vec4 in_COLOR0;
out mediump vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD2;
out lowp vec4 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD3;
out highp float vs_TEXCOORD4;
out highp float vs_TEXCOORD5;
highp vec4 t0;
mediump float t16_1;
lowp vec3 t10_2;
mediump float t16_4;
highp float t9;
lowp float t10_11;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_4 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_4;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_4 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_4;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD0.xy = t0.xy;
    t10_2.xyz = in_COLOR0.xyz * _TintColor.xyz;
    t10_11 = 2.0;
    vs_TEXCOORD1.xyz = vec3(t10_11) * t10_2.xyz;
    t10_2.x = in_COLOR0.w;
    vs_TEXCOORD1.w = t10_2.x * _TintColor.w;
    t0.x = dot(in_NORMAL0.xyz, _World2Object[0].xyz);
    t0.y = dot(in_NORMAL0.xyz, _World2Object[1].xyz);
    t0.z = dot(in_NORMAL0.xyz, _World2Object[2].xyz);
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    vs_TEXCOORD3.xyz = vec3(t9) * t0.xyz;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    vs_TEXCOORD5 = t0.y;
    vs_TEXCOORD4 = sqrt(t0.x);
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	lowp vec4 _TintColor;
uniform 	mediump vec4 acid_Fog;
uniform 	float _BoostAlpha;
uniform 	float _CamHeightOffset;
uniform 	float _FadeDistance;
uniform lowp sampler2D _MainTex;
in mediump vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD2;
in lowp vec4 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD3;
in highp float vs_TEXCOORD4;
in highp float vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
highp float t0;
lowp vec4 t10_1;
mediump vec3 t16_2;
highp float t3;
void main()
{
    t0 = vs_TEXCOORD4 + -1.0;
    t3 = _FadeDistance + -1.0;
    t0 = t0 / t3;
    t0 = clamp(t0, 0.0, 1.0);
    t0 = (-t0) + 1.0;
    t0 = sqrt(t0);
    t0 = t0 * _BoostAlpha;
    t3 = vs_TEXCOORD5 + (-_CamHeightOffset);
    t3 = (-t3);
    t3 = clamp(t3, 0.0, 1.0);
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t10_1 = t10_1 * vs_TEXCOORD1;
    t3 = t3 * t10_1.w;
    t16_2.xyz = t10_1.xyz * vs_TEXCOORD2.xxx;
    SV_Target0.xyz = t16_2.xyz;
    t3 = t3 * vs_TEXCOORD3.y;
    t0 = t0 * t3;
    SV_Target0.w = t0;
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "FOG_OFF" "TINT_COLOR_OFF" "VERTEX_COLOR_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "FOG_OFF" "TINT_COLOR_OFF" "VERTEX_COLOR_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "TINT_COLOR_OFF" "VERTEX_COLOR_OFF" "FOG_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "TINT_COLOR_OFF" "VERTEX_COLOR_OFF" "FOG_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "FOG_OFF" "VERTEX_COLOR_ON" "TINT_COLOR_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "FOG_OFF" "VERTEX_COLOR_ON" "TINT_COLOR_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "VERTEX_COLOR_ON" "TINT_COLOR_OFF" "FOG_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "VERTEX_COLOR_ON" "TINT_COLOR_OFF" "FOG_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "FOG_OFF" "TINT_COLOR_ON" "VERTEX_COLOR_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "FOG_OFF" "TINT_COLOR_ON" "VERTEX_COLOR_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "TINT_COLOR_ON" "VERTEX_COLOR_OFF" "FOG_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "TINT_COLOR_ON" "VERTEX_COLOR_OFF" "FOG_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "FOG_OFF" "TINT_COLOR_ON" "VERTEX_COLOR_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "FOG_OFF" "TINT_COLOR_ON" "VERTEX_COLOR_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "TINT_COLOR_ON" "VERTEX_COLOR_ON" "FOG_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "TINT_COLOR_ON" "VERTEX_COLOR_ON" "FOG_ON" }
"!!GLES3"
}
}
 }
}
}
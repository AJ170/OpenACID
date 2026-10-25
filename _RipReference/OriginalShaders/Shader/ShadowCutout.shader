Shader "Hidden/Shadow/Cutout" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _SpecColor ("Spec Color", Color) = (1,1,1,1)
 _Emission ("Emissive Color", Color) = (0,0,0,0)
 _Shininess ("Shininess", Range(0.01,1)) = 0.7
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}
SubShader { 
 Pass {
  Name "SHADOWCASTER"
  Tags { "LIGHTMODE"="SHADOWCASTER" "SHADOWSUPPORT"="true" }
  Cull Off
  Offset 1, 1
  GpuProgramID 2565
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_DEPTH" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 unity_LightShadowBias;
uniform highp mat4 glstate_matrix_mvp;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  highp vec4 tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = tmpvar_1.xy;
  highp vec4 tmpvar_4;
  tmpvar_4.w = 1.0;
  tmpvar_4.xyz = _glesVertex.xyz;
  tmpvar_2 = (glstate_matrix_mvp * tmpvar_4);
  highp vec4 clipPos_5;
  clipPos_5.xyw = tmpvar_2.xyw;
  clipPos_5.z = (tmpvar_2.z + clamp ((unity_LightShadowBias.x / tmpvar_2.w), 0.0, 1.0));
  clipPos_5.z = mix (clipPos_5.z, max (clipPos_5.z, -(tmpvar_2.w)), unity_LightShadowBias.y);
  tmpvar_2 = clipPos_5;
  gl_Position = clipPos_5;
  xlv_TEXCOORD1 = tmpvar_3;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD1);
  c_1 = tmpvar_2;
  if ((c_1.w < _Cutoff)) {
    discard;
  };
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
uniform 	mediump float _Cutoff;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
highp vec4 t0;
highp float t1;
highp float t4;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = t0 + glstate_matrix_mvp[3];
    t1 = unity_LightShadowBias.x / t0.w;
    t1 = clamp(t1, 0.0, 1.0);
    t4 = t0.z + t1;
    t1 = max((-t0.w), t4);
    gl_Position.xyw = t0.xyw;
    t0.x = (-t4) + t1;
    gl_Position.z = unity_LightShadowBias.y * t0.x + t4;
    vs_TEXCOORD1.xy = in_TEXCOORD0.xy;
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
uniform 	mediump float _Cutoff;
uniform lowp sampler2D _MainTex;
in mediump vec2 vs_TEXCOORD1;
layout(location = 0) out mediump vec4 SV_Target0;
lowp float t10_0;
bool tb0;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD1.xy).w;
    tb0 = t10_0<_Cutoff;
    if((int(tb0) * int(0xffffffffu))!=0){discard;}
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
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _LightPositionRange;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
varying highp vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump vec2 tmpvar_2;
  tmpvar_2 = tmpvar_1.xy;
  xlv_TEXCOORD0 = ((_Object2World * _glesVertex).xyz - _LightPositionRange.xyz);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _LightPositionRange;
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
varying highp vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 tmpvar_1;
  mediump vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD1);
  c_2 = tmpvar_3;
  if ((c_2.w < _Cutoff)) {
    discard;
  };
  highp vec4 enc_4;
  highp vec4 tmpvar_5;
  tmpvar_5 = fract((vec4(1.0, 255.0, 65025.0, 1.658138e+07) * min (
    (sqrt(dot (xlv_TEXCOORD0, xlv_TEXCOORD0)) * _LightPositionRange.w)
  , 0.999)));
  enc_4 = (tmpvar_5 - (tmpvar_5.yzww * 0.003921569));
  tmpvar_1 = enc_4;
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
uniform 	mediump float _Cutoff;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
out highp vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
highp vec4 t0;
void main()
{
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    vs_TEXCOORD0.xyz = t0.xyz + (-_LightPositionRange.xyz);
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD1.xy = in_TEXCOORD0.xy;
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
uniform 	mediump float _Cutoff;
uniform lowp sampler2D _MainTex;
in highp vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec4 t0;
lowp float t10_0;
bool tb0;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD1.xy).w;
    tb0 = t10_0<_Cutoff;
    if((int(tb0) * int(0xffffffffu))!=0){discard;}
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
  GpuProgramID 127835
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_NONATIVE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  highp vec4 tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = tmpvar_1.xy;
  highp vec4 tmpvar_4;
  tmpvar_4 = (_Object2World * _glesVertex);
  tmpvar_2.xyz = tmpvar_4.xyz;
  tmpvar_2.w = -((glstate_matrix_modelview0 * _glesVertex).z);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_4).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_4).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_4).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_4).xyz;
  xlv_TEXCOORD4 = tmpvar_2;
  xlv_TEXCOORD5 = tmpvar_3;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 zFar_4;
  highp vec4 zNear_5;
  mediump vec4 c_6;
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD5);
  c_6 = tmpvar_7;
  if ((c_6.w < _Cutoff)) {
    discard;
  };
  bvec4 tmpvar_8;
  tmpvar_8 = greaterThanEqual (xlv_TEXCOORD4.wwww, _LightSplitsNear);
  lowp vec4 tmpvar_9;
  tmpvar_9 = vec4(tmpvar_8);
  zNear_5 = tmpvar_9;
  bvec4 tmpvar_10;
  tmpvar_10 = lessThan (xlv_TEXCOORD4.wwww, _LightSplitsFar);
  lowp vec4 tmpvar_11;
  tmpvar_11 = vec4(tmpvar_10);
  zFar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (zNear_5 * zFar_4);
  highp float tmpvar_13;
  tmpvar_13 = clamp (((xlv_TEXCOORD4.w * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = (((
    (xlv_TEXCOORD0 * tmpvar_12.x)
   + 
    (xlv_TEXCOORD1 * tmpvar_12.y)
  ) + (xlv_TEXCOORD2 * tmpvar_12.z)) + (xlv_TEXCOORD3 * tmpvar_12.w));
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
Keywords { "SHADOWS_NATIVE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  highp vec4 tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = tmpvar_1.xy;
  highp vec4 tmpvar_4;
  tmpvar_4 = (_Object2World * _glesVertex);
  tmpvar_2.xyz = tmpvar_4.xyz;
  tmpvar_2.w = -((glstate_matrix_modelview0 * _glesVertex).z);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_4).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_4).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_4).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_4).xyz;
  xlv_TEXCOORD4 = tmpvar_2;
  xlv_TEXCOORD5 = tmpvar_3;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 zFar_4;
  highp vec4 zNear_5;
  mediump vec4 c_6;
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD5);
  c_6 = tmpvar_7;
  if ((c_6.w < _Cutoff)) {
    discard;
  };
  bvec4 tmpvar_8;
  tmpvar_8 = greaterThanEqual (xlv_TEXCOORD4.wwww, _LightSplitsNear);
  lowp vec4 tmpvar_9;
  tmpvar_9 = vec4(tmpvar_8);
  zNear_5 = tmpvar_9;
  bvec4 tmpvar_10;
  tmpvar_10 = lessThan (xlv_TEXCOORD4.wwww, _LightSplitsFar);
  lowp vec4 tmpvar_11;
  tmpvar_11 = vec4(tmpvar_10);
  zFar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (zNear_5 * zFar_4);
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (((
    (xlv_TEXCOORD0 * tmpvar_12.x)
   + 
    (xlv_TEXCOORD1 * tmpvar_12.y)
  ) + (xlv_TEXCOORD2 * tmpvar_12.z)) + (xlv_TEXCOORD3 * tmpvar_12.w));
  lowp float tmpvar_14;
  tmpvar_14 = shadow2DEXT (_ShadowMapTexture, tmpvar_13.xyz);
  mediump float tmpvar_15;
  tmpvar_15 = tmpvar_14;
  shadow_3 = (_LightShadowData.x + (tmpvar_15 * (1.0 - _LightShadowData.x)));
  res_2.x = clamp ((shadow_3 + clamp (
    ((xlv_TEXCOORD4.w * _LightShadowData.z) + _LightShadowData.w)
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
uniform 	mediump float _Cutoff;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
out highp vec3 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
out highp vec4 vs_TEXCOORD4;
out mediump vec2 vs_TEXCOORD5;
highp vec4 t0;
highp vec3 t1;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1.xyz = t0.yyy * unity_World2Shadow[0][1].xyz;
    t1.xyz = unity_World2Shadow[0][0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_World2Shadow[0][2].xyz * t0.zzz + t1.xyz;
    vs_TEXCOORD0.xyz = unity_World2Shadow[0][3].xyz * t0.www + t1.xyz;
    t1.xyz = t0.yyy * unity_World2Shadow[1][1].xyz;
    t1.xyz = unity_World2Shadow[1][0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_World2Shadow[1][2].xyz * t0.zzz + t1.xyz;
    vs_TEXCOORD1.xyz = unity_World2Shadow[1][3].xyz * t0.www + t1.xyz;
    t1.xyz = t0.yyy * unity_World2Shadow[2][1].xyz;
    t1.xyz = unity_World2Shadow[2][0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_World2Shadow[2][2].xyz * t0.zzz + t1.xyz;
    vs_TEXCOORD2.xyz = unity_World2Shadow[2][3].xyz * t0.www + t1.xyz;
    t1.xyz = t0.yyy * unity_World2Shadow[3][1].xyz;
    t1.xyz = unity_World2Shadow[3][0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_World2Shadow[3][2].xyz * t0.zzz + t1.xyz;
    vs_TEXCOORD3.xyz = unity_World2Shadow[3][3].xyz * t0.www + t1.xyz;
    vs_TEXCOORD4.xyz = t0.xyz;
    t0.x = in_POSITION0.y * glstate_matrix_modelview0[1].z;
    t0.x = glstate_matrix_modelview0[0].z * in_POSITION0.x + t0.x;
    t0.x = glstate_matrix_modelview0[2].z * in_POSITION0.z + t0.x;
    t0.x = glstate_matrix_modelview0[3].z * in_POSITION0.w + t0.x;
    vs_TEXCOORD4.w = (-t0.x);
    vs_TEXCOORD5.xy = in_TEXCOORD0.xy;
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
uniform 	mediump float _Cutoff;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec3 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
in highp vec4 vs_TEXCOORD4;
in mediump vec2 vs_TEXCOORD5;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec4 t0;
lowp float t10_0;
bvec4 tb0;
highp vec4 t1;
bvec4 tb1;
mediump float t16_2;
highp vec3 t4;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD5.xy).w;
    tb0.x = t10_0<_Cutoff;
    if((int(tb0.x) * int(0xffffffffu))!=0){discard;}
    tb0 = greaterThanEqual(vs_TEXCOORD4.wwww, _LightSplitsNear);
    t0 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tb0));
    tb1 = lessThan(vs_TEXCOORD4.wwww, _LightSplitsFar);
    t1 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tb1));
    t0 = t0 * t1;
    t1.x = vs_TEXCOORD4.w * _LightShadowData.z + _LightShadowData.w;
    t1.x = clamp(t1.x, 0.0, 1.0);
    t4.xyz = t0.yyy * vs_TEXCOORD1.xyz;
    t4.xyz = vs_TEXCOORD0.xyz * t0.xxx + t4.xyz;
    t0.xyz = vs_TEXCOORD2.xyz * t0.zzz + t4.xyz;
    t0.xyz = vs_TEXCOORD3.xyz * t0.www + t0.xyz;
    vec3 txVec14 = vec3(t0.xy,t0.z);
    t10_0 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec14, 0.0);
    t16_2 = (-_LightShadowData.x) + 1.0;
    t16_2 = t10_0 * t16_2 + _LightShadowData.x;
    t0.x = t1.x + t16_2;
    t0.x = clamp(t0.x, 0.0, 1.0);
    t1.x = (-vs_TEXCOORD4.w) * _ProjectionParams.w + 1.0;
    t1.xy = t1.xx * vec2(1.0, 255.0);
    t1.xy = fract(t1.xy);
    t0.z = (-t1.y) * 0.00392156886 + t1.x;
    t0.y = 1.0;
    t0.w = t1.y;
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
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  highp vec4 tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = tmpvar_1.xy;
  highp vec4 tmpvar_4;
  tmpvar_4 = (_Object2World * _glesVertex);
  tmpvar_2.xyz = tmpvar_4.xyz;
  tmpvar_2.w = -((glstate_matrix_modelview0 * _glesVertex).z);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_4).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_4).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_4).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_4).xyz;
  xlv_TEXCOORD4 = tmpvar_2;
  xlv_TEXCOORD5 = tmpvar_3;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 cascadeWeights_4;
  mediump vec4 c_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD5);
  c_5 = tmpvar_6;
  if ((c_5.w < _Cutoff)) {
    discard;
  };
  highp vec3 tmpvar_7;
  tmpvar_7 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[0].xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[1].xyz);
  highp vec3 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[2].xyz);
  highp vec3 tmpvar_10;
  tmpvar_10 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[3].xyz);
  highp vec4 tmpvar_11;
  tmpvar_11.x = dot (tmpvar_7, tmpvar_7);
  tmpvar_11.y = dot (tmpvar_8, tmpvar_8);
  tmpvar_11.z = dot (tmpvar_9, tmpvar_9);
  tmpvar_11.w = dot (tmpvar_10, tmpvar_10);
  bvec4 tmpvar_12;
  tmpvar_12 = lessThan (tmpvar_11, unity_ShadowSplitSqRadii);
  lowp vec4 tmpvar_13;
  tmpvar_13 = vec4(tmpvar_12);
  cascadeWeights_4 = tmpvar_13;
  cascadeWeights_4.yzw = clamp ((cascadeWeights_4.yzw - cascadeWeights_4.xyz), 0.0, 1.0);
  highp vec3 tmpvar_14;
  tmpvar_14 = (xlv_TEXCOORD4.xyz - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_15;
  tmpvar_15 = clamp (((
    sqrt(dot (tmpvar_14, tmpvar_14))
   * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = (((
    (xlv_TEXCOORD0 * cascadeWeights_4.x)
   + 
    (xlv_TEXCOORD1 * cascadeWeights_4.y)
  ) + (xlv_TEXCOORD2 * cascadeWeights_4.z)) + (xlv_TEXCOORD3 * cascadeWeights_4.w));
  highp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_ShadowMapTexture, tmpvar_16.xy);
  mediump float tmpvar_18;
  if ((tmpvar_17.x < tmpvar_16.z)) {
    tmpvar_18 = 0.0;
  } else {
    tmpvar_18 = 1.0;
  };
  shadow_3 = (_LightShadowData.x + (tmpvar_18 * (1.0 - _LightShadowData.x)));
  res_2.x = clamp ((shadow_3 + tmpvar_15), 0.0, 1.0);
  res_2.y = 1.0;
  highp vec2 enc_19;
  highp vec2 tmpvar_20;
  tmpvar_20 = fract((vec2(1.0, 255.0) * (1.0 - 
    (xlv_TEXCOORD4.w * _ProjectionParams.w)
  )));
  enc_19.y = tmpvar_20.y;
  enc_19.x = (tmpvar_20.x - (tmpvar_20.y * 0.003921569));
  res_2.zw = enc_19;
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
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  highp vec4 tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = tmpvar_1.xy;
  highp vec4 tmpvar_4;
  tmpvar_4 = (_Object2World * _glesVertex);
  tmpvar_2.xyz = tmpvar_4.xyz;
  tmpvar_2.w = -((glstate_matrix_modelview0 * _glesVertex).z);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_4).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_4).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_4).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_4).xyz;
  xlv_TEXCOORD4 = tmpvar_2;
  xlv_TEXCOORD5 = tmpvar_3;
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
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 cascadeWeights_4;
  mediump vec4 c_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD5);
  c_5 = tmpvar_6;
  if ((c_5.w < _Cutoff)) {
    discard;
  };
  highp vec3 tmpvar_7;
  tmpvar_7 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[0].xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[1].xyz);
  highp vec3 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[2].xyz);
  highp vec3 tmpvar_10;
  tmpvar_10 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[3].xyz);
  highp vec4 tmpvar_11;
  tmpvar_11.x = dot (tmpvar_7, tmpvar_7);
  tmpvar_11.y = dot (tmpvar_8, tmpvar_8);
  tmpvar_11.z = dot (tmpvar_9, tmpvar_9);
  tmpvar_11.w = dot (tmpvar_10, tmpvar_10);
  bvec4 tmpvar_12;
  tmpvar_12 = lessThan (tmpvar_11, unity_ShadowSplitSqRadii);
  lowp vec4 tmpvar_13;
  tmpvar_13 = vec4(tmpvar_12);
  cascadeWeights_4 = tmpvar_13;
  cascadeWeights_4.yzw = clamp ((cascadeWeights_4.yzw - cascadeWeights_4.xyz), 0.0, 1.0);
  highp vec3 tmpvar_14;
  tmpvar_14 = (xlv_TEXCOORD4.xyz - unity_ShadowFadeCenterAndType.xyz);
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = (((
    (xlv_TEXCOORD0 * cascadeWeights_4.x)
   + 
    (xlv_TEXCOORD1 * cascadeWeights_4.y)
  ) + (xlv_TEXCOORD2 * cascadeWeights_4.z)) + (xlv_TEXCOORD3 * cascadeWeights_4.w));
  lowp float tmpvar_16;
  tmpvar_16 = shadow2DEXT (_ShadowMapTexture, tmpvar_15.xyz);
  mediump float tmpvar_17;
  tmpvar_17 = tmpvar_16;
  shadow_3 = (_LightShadowData.x + (tmpvar_17 * (1.0 - _LightShadowData.x)));
  res_2.x = clamp ((shadow_3 + clamp (
    ((sqrt(dot (tmpvar_14, tmpvar_14)) * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  res_2.y = 1.0;
  highp vec2 enc_18;
  highp vec2 tmpvar_19;
  tmpvar_19 = fract((vec2(1.0, 255.0) * (1.0 - 
    (xlv_TEXCOORD4.w * _ProjectionParams.w)
  )));
  enc_18.y = tmpvar_19.y;
  enc_18.x = (tmpvar_19.x - (tmpvar_19.y * 0.003921569));
  res_2.zw = enc_18;
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
uniform 	mediump float _Cutoff;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
out highp vec3 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
out highp vec4 vs_TEXCOORD4;
out mediump vec2 vs_TEXCOORD5;
highp vec4 t0;
highp vec3 t1;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1.xyz = t0.yyy * unity_World2Shadow[0][1].xyz;
    t1.xyz = unity_World2Shadow[0][0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_World2Shadow[0][2].xyz * t0.zzz + t1.xyz;
    vs_TEXCOORD0.xyz = unity_World2Shadow[0][3].xyz * t0.www + t1.xyz;
    t1.xyz = t0.yyy * unity_World2Shadow[1][1].xyz;
    t1.xyz = unity_World2Shadow[1][0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_World2Shadow[1][2].xyz * t0.zzz + t1.xyz;
    vs_TEXCOORD1.xyz = unity_World2Shadow[1][3].xyz * t0.www + t1.xyz;
    t1.xyz = t0.yyy * unity_World2Shadow[2][1].xyz;
    t1.xyz = unity_World2Shadow[2][0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_World2Shadow[2][2].xyz * t0.zzz + t1.xyz;
    vs_TEXCOORD2.xyz = unity_World2Shadow[2][3].xyz * t0.www + t1.xyz;
    t1.xyz = t0.yyy * unity_World2Shadow[3][1].xyz;
    t1.xyz = unity_World2Shadow[3][0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_World2Shadow[3][2].xyz * t0.zzz + t1.xyz;
    vs_TEXCOORD3.xyz = unity_World2Shadow[3][3].xyz * t0.www + t1.xyz;
    vs_TEXCOORD4.xyz = t0.xyz;
    t0.x = in_POSITION0.y * glstate_matrix_modelview0[1].z;
    t0.x = glstate_matrix_modelview0[0].z * in_POSITION0.x + t0.x;
    t0.x = glstate_matrix_modelview0[2].z * in_POSITION0.z + t0.x;
    t0.x = glstate_matrix_modelview0[3].z * in_POSITION0.w + t0.x;
    vs_TEXCOORD4.w = (-t0.x);
    vs_TEXCOORD5.xy = in_TEXCOORD0.xy;
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
uniform 	mediump float _Cutoff;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec3 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
in highp vec4 vs_TEXCOORD4;
in mediump vec2 vs_TEXCOORD5;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec4 t0;
lowp float t10_0;
bvec4 tb0;
highp vec4 t1;
highp vec3 t2;
highp vec3 t3;
mediump float t16_4;
highp vec3 t6;
highp float t15;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD5.xy).w;
    tb0.x = t10_0<_Cutoff;
    if((int(tb0.x) * int(0xffffffffu))!=0){discard;}
    t0.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowSplitSpheres[0].xyz);
    t1.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowSplitSpheres[1].xyz);
    t2.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowSplitSpheres[2].xyz);
    t3.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowSplitSpheres[3].xyz);
    t0.x = dot(t0.xyz, t0.xyz);
    t0.y = dot(t1.xyz, t1.xyz);
    t0.z = dot(t2.xyz, t2.xyz);
    t0.w = dot(t3.xyz, t3.xyz);
    tb0 = lessThan(t0, unity_ShadowSplitSqRadii);
    t1 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tb0));
    t0.x = (tb0.x) ? float(-1.0) : float(-0.0);
    t0.y = (tb0.y) ? float(-1.0) : float(-0.0);
    t0.z = (tb0.z) ? float(-1.0) : float(-0.0);
    t0.xyz = vec3(t0.x + t1.y, t0.y + t1.z, t0.z + t1.w);
    t0.xyz = max(t0.xyz, vec3(0.0, 0.0, 0.0));
    t6.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowFadeCenterAndType.xyz);
    t15 = dot(t6.xyz, t6.xyz);
    t15 = sqrt(t15);
    t15 = t15 * _LightShadowData.z + _LightShadowData.w;
    t15 = clamp(t15, 0.0, 1.0);
    t6.xyz = t0.xxx * vs_TEXCOORD1.xyz;
    t1.xyz = vs_TEXCOORD0.xyz * t1.xxx + t6.xyz;
    t1.xyz = vs_TEXCOORD2.xyz * t0.yyy + t1.xyz;
    t0.xyz = vs_TEXCOORD3.xyz * t0.zzz + t1.xyz;
    vec3 txVec11 = vec3(t0.xy,t0.z);
    t10_0 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec11, 0.0);
    t16_4 = (-_LightShadowData.x) + 1.0;
    t16_4 = t10_0 * t16_4 + _LightShadowData.x;
    t0.x = t15 + t16_4;
    t0.x = clamp(t0.x, 0.0, 1.0);
    t1.x = (-vs_TEXCOORD4.w) * _ProjectionParams.w + 1.0;
    t1.xy = t1.xx * vec2(1.0, 255.0);
    t1.xy = fract(t1.xy);
    t0.z = (-t1.y) * 0.00392156886 + t1.x;
    t0.y = 1.0;
    t0.w = t1.y;
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
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  highp vec4 tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = tmpvar_1.xy;
  highp vec4 tmpvar_4;
  tmpvar_4 = (_Object2World * _glesVertex);
  tmpvar_2.xyz = tmpvar_4.xyz;
  tmpvar_2.w = -((glstate_matrix_modelview0 * _glesVertex).z);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_4).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_4).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_4).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_4).xyz;
  xlv_TEXCOORD4 = tmpvar_2;
  xlv_TEXCOORD5 = tmpvar_3;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 zFar_4;
  highp vec4 zNear_5;
  mediump vec4 c_6;
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD5);
  c_6 = tmpvar_7;
  if ((c_6.w < _Cutoff)) {
    discard;
  };
  bvec4 tmpvar_8;
  tmpvar_8 = greaterThanEqual (xlv_TEXCOORD4.wwww, _LightSplitsNear);
  lowp vec4 tmpvar_9;
  tmpvar_9 = vec4(tmpvar_8);
  zNear_5 = tmpvar_9;
  bvec4 tmpvar_10;
  tmpvar_10 = lessThan (xlv_TEXCOORD4.wwww, _LightSplitsFar);
  lowp vec4 tmpvar_11;
  tmpvar_11 = vec4(tmpvar_10);
  zFar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (zNear_5 * zFar_4);
  highp float tmpvar_13;
  tmpvar_13 = clamp (((xlv_TEXCOORD4.w * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = (((
    (xlv_TEXCOORD0 * tmpvar_12.x)
   + 
    (xlv_TEXCOORD1 * tmpvar_12.y)
  ) + (xlv_TEXCOORD2 * tmpvar_12.z)) + (xlv_TEXCOORD3 * tmpvar_12.w));
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
Keywords { "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  highp vec4 tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = tmpvar_1.xy;
  highp vec4 tmpvar_4;
  tmpvar_4 = (_Object2World * _glesVertex);
  tmpvar_2.xyz = tmpvar_4.xyz;
  tmpvar_2.w = -((glstate_matrix_modelview0 * _glesVertex).z);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_4).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_4).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_4).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_4).xyz;
  xlv_TEXCOORD4 = tmpvar_2;
  xlv_TEXCOORD5 = tmpvar_3;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 zFar_4;
  highp vec4 zNear_5;
  mediump vec4 c_6;
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD5);
  c_6 = tmpvar_7;
  if ((c_6.w < _Cutoff)) {
    discard;
  };
  bvec4 tmpvar_8;
  tmpvar_8 = greaterThanEqual (xlv_TEXCOORD4.wwww, _LightSplitsNear);
  lowp vec4 tmpvar_9;
  tmpvar_9 = vec4(tmpvar_8);
  zNear_5 = tmpvar_9;
  bvec4 tmpvar_10;
  tmpvar_10 = lessThan (xlv_TEXCOORD4.wwww, _LightSplitsFar);
  lowp vec4 tmpvar_11;
  tmpvar_11 = vec4(tmpvar_10);
  zFar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (zNear_5 * zFar_4);
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (((
    (xlv_TEXCOORD0 * tmpvar_12.x)
   + 
    (xlv_TEXCOORD1 * tmpvar_12.y)
  ) + (xlv_TEXCOORD2 * tmpvar_12.z)) + (xlv_TEXCOORD3 * tmpvar_12.w));
  lowp float tmpvar_14;
  tmpvar_14 = shadow2DEXT (_ShadowMapTexture, tmpvar_13.xyz);
  mediump float tmpvar_15;
  tmpvar_15 = tmpvar_14;
  shadow_3 = (_LightShadowData.x + (tmpvar_15 * (1.0 - _LightShadowData.x)));
  res_2.x = clamp ((shadow_3 + clamp (
    ((xlv_TEXCOORD4.w * _LightShadowData.z) + _LightShadowData.w)
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
uniform 	mediump float _Cutoff;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
out highp vec3 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
out highp vec4 vs_TEXCOORD4;
out mediump vec2 vs_TEXCOORD5;
highp vec4 t0;
highp vec3 t1;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1.xyz = t0.yyy * unity_World2Shadow[0][1].xyz;
    t1.xyz = unity_World2Shadow[0][0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_World2Shadow[0][2].xyz * t0.zzz + t1.xyz;
    vs_TEXCOORD0.xyz = unity_World2Shadow[0][3].xyz * t0.www + t1.xyz;
    t1.xyz = t0.yyy * unity_World2Shadow[1][1].xyz;
    t1.xyz = unity_World2Shadow[1][0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_World2Shadow[1][2].xyz * t0.zzz + t1.xyz;
    vs_TEXCOORD1.xyz = unity_World2Shadow[1][3].xyz * t0.www + t1.xyz;
    t1.xyz = t0.yyy * unity_World2Shadow[2][1].xyz;
    t1.xyz = unity_World2Shadow[2][0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_World2Shadow[2][2].xyz * t0.zzz + t1.xyz;
    vs_TEXCOORD2.xyz = unity_World2Shadow[2][3].xyz * t0.www + t1.xyz;
    t1.xyz = t0.yyy * unity_World2Shadow[3][1].xyz;
    t1.xyz = unity_World2Shadow[3][0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_World2Shadow[3][2].xyz * t0.zzz + t1.xyz;
    vs_TEXCOORD3.xyz = unity_World2Shadow[3][3].xyz * t0.www + t1.xyz;
    vs_TEXCOORD4.xyz = t0.xyz;
    t0.x = in_POSITION0.y * glstate_matrix_modelview0[1].z;
    t0.x = glstate_matrix_modelview0[0].z * in_POSITION0.x + t0.x;
    t0.x = glstate_matrix_modelview0[2].z * in_POSITION0.z + t0.x;
    t0.x = glstate_matrix_modelview0[3].z * in_POSITION0.w + t0.x;
    vs_TEXCOORD4.w = (-t0.x);
    vs_TEXCOORD5.xy = in_TEXCOORD0.xy;
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
uniform 	mediump float _Cutoff;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec3 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
in highp vec4 vs_TEXCOORD4;
in mediump vec2 vs_TEXCOORD5;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec4 t0;
lowp float t10_0;
bvec4 tb0;
highp vec4 t1;
bvec4 tb1;
mediump float t16_2;
highp vec3 t4;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD5.xy).w;
    tb0.x = t10_0<_Cutoff;
    if((int(tb0.x) * int(0xffffffffu))!=0){discard;}
    tb0 = greaterThanEqual(vs_TEXCOORD4.wwww, _LightSplitsNear);
    t0 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tb0));
    tb1 = lessThan(vs_TEXCOORD4.wwww, _LightSplitsFar);
    t1 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tb1));
    t0 = t0 * t1;
    t1.x = vs_TEXCOORD4.w * _LightShadowData.z + _LightShadowData.w;
    t1.x = clamp(t1.x, 0.0, 1.0);
    t4.xyz = t0.yyy * vs_TEXCOORD1.xyz;
    t4.xyz = vs_TEXCOORD0.xyz * t0.xxx + t4.xyz;
    t0.xyz = vs_TEXCOORD2.xyz * t0.zzz + t4.xyz;
    t0.xyz = vs_TEXCOORD3.xyz * t0.www + t0.xyz;
    vec3 txVec9 = vec3(t0.xy,t0.z);
    t10_0 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec9, 0.0);
    t16_2 = (-_LightShadowData.x) + 1.0;
    t16_2 = t10_0 * t16_2 + _LightShadowData.x;
    t0.x = t1.x + t16_2;
    t0.x = clamp(t0.x, 0.0, 1.0);
    t1.x = (-vs_TEXCOORD4.w) * _ProjectionParams.w + 1.0;
    t1.xy = t1.xx * vec2(1.0, 255.0);
    t1.xy = fract(t1.xy);
    t0.z = (-t1.y) * 0.00392156886 + t1.x;
    t0.y = 1.0;
    t0.w = t1.y;
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
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  highp vec4 tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = tmpvar_1.xy;
  highp vec4 tmpvar_4;
  tmpvar_4 = (_Object2World * _glesVertex);
  tmpvar_2.xyz = tmpvar_4.xyz;
  tmpvar_2.w = -((glstate_matrix_modelview0 * _glesVertex).z);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_4).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_4).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_4).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_4).xyz;
  xlv_TEXCOORD4 = tmpvar_2;
  xlv_TEXCOORD5 = tmpvar_3;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 cascadeWeights_4;
  mediump vec4 c_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD5);
  c_5 = tmpvar_6;
  if ((c_5.w < _Cutoff)) {
    discard;
  };
  highp vec3 tmpvar_7;
  tmpvar_7 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[0].xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[1].xyz);
  highp vec3 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[2].xyz);
  highp vec3 tmpvar_10;
  tmpvar_10 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[3].xyz);
  highp vec4 tmpvar_11;
  tmpvar_11.x = dot (tmpvar_7, tmpvar_7);
  tmpvar_11.y = dot (tmpvar_8, tmpvar_8);
  tmpvar_11.z = dot (tmpvar_9, tmpvar_9);
  tmpvar_11.w = dot (tmpvar_10, tmpvar_10);
  bvec4 tmpvar_12;
  tmpvar_12 = lessThan (tmpvar_11, unity_ShadowSplitSqRadii);
  lowp vec4 tmpvar_13;
  tmpvar_13 = vec4(tmpvar_12);
  cascadeWeights_4 = tmpvar_13;
  cascadeWeights_4.yzw = clamp ((cascadeWeights_4.yzw - cascadeWeights_4.xyz), 0.0, 1.0);
  highp vec3 tmpvar_14;
  tmpvar_14 = (xlv_TEXCOORD4.xyz - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_15;
  tmpvar_15 = clamp (((
    sqrt(dot (tmpvar_14, tmpvar_14))
   * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = (((
    (xlv_TEXCOORD0 * cascadeWeights_4.x)
   + 
    (xlv_TEXCOORD1 * cascadeWeights_4.y)
  ) + (xlv_TEXCOORD2 * cascadeWeights_4.z)) + (xlv_TEXCOORD3 * cascadeWeights_4.w));
  highp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_ShadowMapTexture, tmpvar_16.xy);
  mediump float tmpvar_18;
  if ((tmpvar_17.x < tmpvar_16.z)) {
    tmpvar_18 = 0.0;
  } else {
    tmpvar_18 = 1.0;
  };
  shadow_3 = (_LightShadowData.x + (tmpvar_18 * (1.0 - _LightShadowData.x)));
  res_2.x = clamp ((shadow_3 + tmpvar_15), 0.0, 1.0);
  res_2.y = 1.0;
  highp vec2 enc_19;
  highp vec2 tmpvar_20;
  tmpvar_20 = fract((vec2(1.0, 255.0) * (1.0 - 
    (xlv_TEXCOORD4.w * _ProjectionParams.w)
  )));
  enc_19.y = tmpvar_20.y;
  enc_19.x = (tmpvar_20.x - (tmpvar_20.y * 0.003921569));
  res_2.zw = enc_19;
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
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  highp vec4 tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = tmpvar_1.xy;
  highp vec4 tmpvar_4;
  tmpvar_4 = (_Object2World * _glesVertex);
  tmpvar_2.xyz = tmpvar_4.xyz;
  tmpvar_2.w = -((glstate_matrix_modelview0 * _glesVertex).z);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (unity_World2Shadow[0] * tmpvar_4).xyz;
  xlv_TEXCOORD1 = (unity_World2Shadow[1] * tmpvar_4).xyz;
  xlv_TEXCOORD2 = (unity_World2Shadow[2] * tmpvar_4).xyz;
  xlv_TEXCOORD3 = (unity_World2Shadow[3] * tmpvar_4).xyz;
  xlv_TEXCOORD4 = tmpvar_2;
  xlv_TEXCOORD5 = tmpvar_3;
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
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
varying mediump vec2 xlv_TEXCOORD5;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec4 res_2;
  mediump float shadow_3;
  highp vec4 cascadeWeights_4;
  mediump vec4 c_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD5);
  c_5 = tmpvar_6;
  if ((c_5.w < _Cutoff)) {
    discard;
  };
  highp vec3 tmpvar_7;
  tmpvar_7 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[0].xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[1].xyz);
  highp vec3 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[2].xyz);
  highp vec3 tmpvar_10;
  tmpvar_10 = (xlv_TEXCOORD4.xyz - unity_ShadowSplitSpheres[3].xyz);
  highp vec4 tmpvar_11;
  tmpvar_11.x = dot (tmpvar_7, tmpvar_7);
  tmpvar_11.y = dot (tmpvar_8, tmpvar_8);
  tmpvar_11.z = dot (tmpvar_9, tmpvar_9);
  tmpvar_11.w = dot (tmpvar_10, tmpvar_10);
  bvec4 tmpvar_12;
  tmpvar_12 = lessThan (tmpvar_11, unity_ShadowSplitSqRadii);
  lowp vec4 tmpvar_13;
  tmpvar_13 = vec4(tmpvar_12);
  cascadeWeights_4 = tmpvar_13;
  cascadeWeights_4.yzw = clamp ((cascadeWeights_4.yzw - cascadeWeights_4.xyz), 0.0, 1.0);
  highp vec3 tmpvar_14;
  tmpvar_14 = (xlv_TEXCOORD4.xyz - unity_ShadowFadeCenterAndType.xyz);
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = (((
    (xlv_TEXCOORD0 * cascadeWeights_4.x)
   + 
    (xlv_TEXCOORD1 * cascadeWeights_4.y)
  ) + (xlv_TEXCOORD2 * cascadeWeights_4.z)) + (xlv_TEXCOORD3 * cascadeWeights_4.w));
  lowp float tmpvar_16;
  tmpvar_16 = shadow2DEXT (_ShadowMapTexture, tmpvar_15.xyz);
  mediump float tmpvar_17;
  tmpvar_17 = tmpvar_16;
  shadow_3 = (_LightShadowData.x + (tmpvar_17 * (1.0 - _LightShadowData.x)));
  res_2.x = clamp ((shadow_3 + clamp (
    ((sqrt(dot (tmpvar_14, tmpvar_14)) * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  res_2.y = 1.0;
  highp vec2 enc_18;
  highp vec2 tmpvar_19;
  tmpvar_19 = fract((vec2(1.0, 255.0) * (1.0 - 
    (xlv_TEXCOORD4.w * _ProjectionParams.w)
  )));
  enc_18.y = tmpvar_19.y;
  enc_18.x = (tmpvar_19.x - (tmpvar_19.y * 0.003921569));
  res_2.zw = enc_18;
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
uniform 	mediump float _Cutoff;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
out highp vec3 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
out highp vec4 vs_TEXCOORD4;
out mediump vec2 vs_TEXCOORD5;
highp vec4 t0;
highp vec3 t1;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1.xyz = t0.yyy * unity_World2Shadow[0][1].xyz;
    t1.xyz = unity_World2Shadow[0][0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_World2Shadow[0][2].xyz * t0.zzz + t1.xyz;
    vs_TEXCOORD0.xyz = unity_World2Shadow[0][3].xyz * t0.www + t1.xyz;
    t1.xyz = t0.yyy * unity_World2Shadow[1][1].xyz;
    t1.xyz = unity_World2Shadow[1][0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_World2Shadow[1][2].xyz * t0.zzz + t1.xyz;
    vs_TEXCOORD1.xyz = unity_World2Shadow[1][3].xyz * t0.www + t1.xyz;
    t1.xyz = t0.yyy * unity_World2Shadow[2][1].xyz;
    t1.xyz = unity_World2Shadow[2][0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_World2Shadow[2][2].xyz * t0.zzz + t1.xyz;
    vs_TEXCOORD2.xyz = unity_World2Shadow[2][3].xyz * t0.www + t1.xyz;
    t1.xyz = t0.yyy * unity_World2Shadow[3][1].xyz;
    t1.xyz = unity_World2Shadow[3][0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_World2Shadow[3][2].xyz * t0.zzz + t1.xyz;
    vs_TEXCOORD3.xyz = unity_World2Shadow[3][3].xyz * t0.www + t1.xyz;
    vs_TEXCOORD4.xyz = t0.xyz;
    t0.x = in_POSITION0.y * glstate_matrix_modelview0[1].z;
    t0.x = glstate_matrix_modelview0[0].z * in_POSITION0.x + t0.x;
    t0.x = glstate_matrix_modelview0[2].z * in_POSITION0.z + t0.x;
    t0.x = glstate_matrix_modelview0[3].z * in_POSITION0.w + t0.x;
    vs_TEXCOORD4.w = (-t0.x);
    vs_TEXCOORD5.xy = in_TEXCOORD0.xy;
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
uniform 	mediump float _Cutoff;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec3 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
in highp vec4 vs_TEXCOORD4;
in mediump vec2 vs_TEXCOORD5;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec4 t0;
lowp float t10_0;
bvec4 tb0;
highp vec4 t1;
highp vec3 t2;
highp vec3 t3;
mediump float t16_4;
highp vec3 t6;
highp float t15;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD5.xy).w;
    tb0.x = t10_0<_Cutoff;
    if((int(tb0.x) * int(0xffffffffu))!=0){discard;}
    t0.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowSplitSpheres[0].xyz);
    t1.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowSplitSpheres[1].xyz);
    t2.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowSplitSpheres[2].xyz);
    t3.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowSplitSpheres[3].xyz);
    t0.x = dot(t0.xyz, t0.xyz);
    t0.y = dot(t1.xyz, t1.xyz);
    t0.z = dot(t2.xyz, t2.xyz);
    t0.w = dot(t3.xyz, t3.xyz);
    tb0 = lessThan(t0, unity_ShadowSplitSqRadii);
    t1 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tb0));
    t0.x = (tb0.x) ? float(-1.0) : float(-0.0);
    t0.y = (tb0.y) ? float(-1.0) : float(-0.0);
    t0.z = (tb0.z) ? float(-1.0) : float(-0.0);
    t0.xyz = vec3(t0.x + t1.y, t0.y + t1.z, t0.z + t1.w);
    t0.xyz = max(t0.xyz, vec3(0.0, 0.0, 0.0));
    t6.xyz = vs_TEXCOORD4.xyz + (-unity_ShadowFadeCenterAndType.xyz);
    t15 = dot(t6.xyz, t6.xyz);
    t15 = sqrt(t15);
    t15 = t15 * _LightShadowData.z + _LightShadowData.w;
    t15 = clamp(t15, 0.0, 1.0);
    t6.xyz = t0.xxx * vs_TEXCOORD1.xyz;
    t1.xyz = vs_TEXCOORD0.xyz * t1.xxx + t6.xyz;
    t1.xyz = vs_TEXCOORD2.xyz * t0.yyy + t1.xyz;
    t0.xyz = vs_TEXCOORD3.xyz * t0.zzz + t1.xyz;
    vec3 txVec12 = vec3(t0.xy,t0.z);
    t10_0 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec12, 0.0);
    t16_4 = (-_LightShadowData.x) + 1.0;
    t16_4 = t10_0 * t16_4 + _LightShadowData.x;
    t0.x = t15 + t16_4;
    t0.x = clamp(t0.x, 0.0, 1.0);
    t1.x = (-vs_TEXCOORD4.w) * _ProjectionParams.w + 1.0;
    t1.xy = t1.xx * vec2(1.0, 255.0);
    t1.xy = fract(t1.xy);
    t0.z = (-t1.y) * 0.00392156886 + t1.x;
    t0.y = 1.0;
    t0.w = t1.y;
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
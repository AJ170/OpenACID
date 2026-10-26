Shader "Animus/TerrainV2" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Billboard Texture", 2D) = "white" { }
}
SubShader { 
 Tags { "QUEUE"="Background" "IGNOREPROJECTOR"="true" }
 Pass {
  Tags { "QUEUE"="Background" "IGNOREPROJECTOR"="true" }
  ZWrite Off
  Cull Off
  Blend One One
  GpuProgramID 55616
Program "vp" {
SubProgram "gles " {
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp mat4 glstate_matrix_projection;
varying mediump vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD0;
void main ()
{
  mediump vec4 tmpvar_1;
  highp float waveInverse_2;
  highp float mDeg3_3;
  highp float mDeg_4;
  highp vec4 wPos_5;
  highp vec4 tmpvar_6;
  tmpvar_6 = (_Object2World * _glesVertex);
  wPos_5.zw = tmpvar_6.zw;
  highp float tmpvar_7;
  tmpvar_7 = ((tmpvar_6.z * tmpvar_6.x) * 0.0075);
  mDeg_4 = (tmpvar_7 + (_Time.z * 0.33));
  mDeg3_3 = (((tmpvar_6.z * tmpvar_6.x) * 0.75) + _Time.z);
  mediump float tmpvar_8;
  tmpvar_8 = (1.0 - _glesNormal.z);
  waveInverse_2 = tmpvar_8;
  wPos_5.x = (tmpvar_6.x + ((
    (cos((mDeg_4 * 0.5)) * _glesColor.w)
   * 2.0) * waveInverse_2));
  wPos_5.y = (tmpvar_6.y + ((
    (sin((mDeg_4 * 0.5)) * _glesColor.w)
   * 4.0) * waveInverse_2));
  wPos_5.x = (wPos_5.x + ((
    (cos((mDeg3_3 * 0.75)) * _glesNormal.z)
   * _glesColor.w) * 0.5));
  wPos_5.y = (wPos_5.y + ((
    (sin((mDeg3_3 * 0.75)) * _glesNormal.z)
   * _glesColor.w) * 0.5));
  wPos_5 = (_World2Object * wPos_5);
  wPos_5 = (glstate_matrix_modelview0 * wPos_5);
  highp float tmpvar_9;
  tmpvar_9 = sin(((tmpvar_7 + 
    (_Time.z * 0.2)
  ) * 3.5));
  tmpvar_1 = (_glesMultiTexCoord1 * (1.05 + (
    (tmpvar_9 * 0.35)
   * _glesColor.w)));
  highp float tmpvar_10;
  tmpvar_10 = sin((mDeg3_3 * 0.5));
  tmpvar_1 = (tmpvar_1 * (1.05 + (
    (tmpvar_10 * 0.55)
   * _glesNormal.z)));
  wPos_5 = (wPos_5 + tmpvar_1);
  gl_Position = (glstate_matrix_projection * wPos_5);
  xlv_COLOR = _glesColor;
  xlv_TEXCOORD0 = _glesMultiTexCoord0;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _Color;
uniform sampler2D _MainTex;
varying mediump vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  gl_FragData[0] = ((tmpvar_1 * xlv_COLOR) * _Color);
}


#endif
"
}
SubProgram "gles3 " {
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
uniform 	mediump vec4 _Color;
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in mediump vec4 in_NORMAL0;
in mediump vec4 in_TEXCOORD0;
in mediump vec4 in_TEXCOORD1;
out mediump vec4 vs_COLOR0;
out mediump vec4 vs_TEXCOORD0;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
highp float t3;
highp vec4 t4;
mediump float t16_5;
highp float t6;
highp float t7;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1.x = t0.x * t0.z;
    t7 = t1.x * 0.00749999983;
    t1.x = t1.x * 0.75 + _Time.z;
    t1.yw = _Time.zz * vec2(0.330000013, 0.200000003) + vec2(t7);
    t1 = t1.xyxw * vec4(0.75, 0.5, 0.5, 3.5);
    t2.x = sin(t1.y);
    t3 = cos(t1.y);
    t7 = sin(t1.w);
    t7 = t7 * 0.349999994;
    t7 = t7 * in_COLOR0.w + 1.04999995;
    t4 = vec4(t7) * in_TEXCOORD1;
    t7 = t2.x * in_COLOR0.w;
    t19 = dot(vec2(t3), in_COLOR0.ww);
    t7 = t7 * 4.0;
    t16_5 = (-in_NORMAL0.z) + 1.0;
    t6 = t7 * t16_5 + t0.y;
    t0.x = t19 * t16_5 + t0.x;
    t2.x = cos(t1.x);
    t1.x = sin(t1.x);
    t7 = sin(t1.z);
    t7 = t7 * 0.550000012;
    t7 = t7 * in_NORMAL0.z + 1.04999995;
    t1.x = t1.x * in_NORMAL0.z;
    t1.z = t2.x * in_NORMAL0.z;
    t1.xz = t1.xz * in_COLOR0.ww;
    t0.x = t1.z * 0.5 + t0.x;
    t6 = t1.x * 0.5 + t6;
    t2 = vec4(t6) * _World2Object[1];
    t2 = _World2Object[0] * t0.xxxx + t2;
    t2 = _World2Object[2] * t0.zzzz + t2;
    t0 = _World2Object[3] * t0.wwww + t2;
    t2 = t0.yyyy * glstate_matrix_modelview0[1];
    t2 = glstate_matrix_modelview0[0] * t0.xxxx + t2;
    t2 = glstate_matrix_modelview0[2] * t0.zzzz + t2;
    t0 = glstate_matrix_modelview0[3] * t0.wwww + t2;
    t0 = t4 * vec4(t7) + t0;
    t1 = t0.yyyy * glstate_matrix_projection[1];
    t1 = glstate_matrix_projection[0] * t0.xxxx + t1;
    t1 = glstate_matrix_projection[2] * t0.zzzz + t1;
    gl_Position = glstate_matrix_projection[3] * t0.wwww + t1;
    vs_COLOR0 = in_COLOR0;
    vs_TEXCOORD0 = in_TEXCOORD0;
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
uniform 	mediump vec4 _Color;
uniform lowp sampler2D _MainTex;
in mediump vec4 vs_COLOR0;
in mediump vec4 vs_TEXCOORD0;
layout(location = 0) out mediump vec4 SV_Target0;
mediump vec4 t16_0;
lowp vec4 t10_0;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_0 = t10_0 * vs_COLOR0;
    t16_0 = t16_0 * _Color;
    SV_Target0 = t16_0;
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
"!!GLES"
}
SubProgram "gles3 " {
"!!GLES3"
}
}
 }
}
}
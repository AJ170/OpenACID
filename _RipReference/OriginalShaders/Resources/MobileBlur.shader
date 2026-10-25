Shader "Hidden/FastBlur" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _Bloom ("Bloom (RGB)", 2D) = "black" { }
}
SubShader { 
 Pass {
  ZTest False
  ZWrite Off
  Cull Off
  GpuProgramID 63340
Program "vp" {
SubProgram "gles " {
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform mediump vec4 _MainTex_TexelSize;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD3;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (_glesMultiTexCoord0.xy + _MainTex_TexelSize.xy);
  xlv_TEXCOORD1 = (_glesMultiTexCoord0.xy + (_MainTex_TexelSize.xy * vec2(-0.5, -0.5)));
  xlv_TEXCOORD2 = (_glesMultiTexCoord0.xy + (_MainTex_TexelSize.xy * vec2(0.5, -0.5)));
  xlv_TEXCOORD3 = (_glesMultiTexCoord0.xy + (_MainTex_TexelSize.xy * vec2(-0.5, 0.5)));
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 color_1;
  color_1 = (texture2D (_MainTex, xlv_TEXCOORD0) + texture2D (_MainTex, xlv_TEXCOORD1));
  color_1 = (color_1 + texture2D (_MainTex, xlv_TEXCOORD2));
  color_1 = (color_1 + texture2D (_MainTex, xlv_TEXCOORD3));
  lowp vec4 tmpvar_2;
  tmpvar_2 = (color_1 / 4.0);
  gl_FragData[0] = tmpvar_2;
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
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	mediump vec4 _MainTex_TexelSize;
uniform 	mediump vec4 _Parameter;
in highp vec4 in_POSITION0;
in mediump vec2 in_TEXCOORD0;
out mediump vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec2 vs_TEXCOORD3;
highp vec4 t0;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy + _MainTex_TexelSize.xy;
    vs_TEXCOORD1.xy = _MainTex_TexelSize.xy * vec2(-0.5, -0.5) + in_TEXCOORD0.xy;
    vs_TEXCOORD2.xy = _MainTex_TexelSize.xy * vec2(0.5, -0.5) + in_TEXCOORD0.xy;
    vs_TEXCOORD3.xy = _MainTex_TexelSize.xy * vec2(-0.5, 0.5) + in_TEXCOORD0.xy;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
uniform lowp sampler2D _MainTex;
in mediump vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec2 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
lowp vec4 t10_0;
lowp vec4 t10_1;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
    t10_1 = texture(_MainTex, vs_TEXCOORD1.xy);
    t16_0 = t10_0 + t10_1;
    t10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
    t16_0 = t16_0 + t10_1;
    t10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
    t16_0 = t16_0 + t10_1;
    SV_Target0 = t16_0 * vec4(0.25, 0.25, 0.25, 0.25);
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
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  GpuProgramID 75181
Program "vp" {
SubProgram "gles " {
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform mediump vec4 _MainTex_TexelSize;
uniform mediump vec4 _Parameter;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 tmpvar_1;
  tmpvar_1.zw = vec2(1.0, 1.0);
  tmpvar_1.xy = _glesMultiTexCoord0.xy;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_MainTex_TexelSize.xy * vec2(0.0, 1.0)) * _Parameter.x);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 tap_1;
  mediump vec4 color_2;
  mediump vec2 coords_3;
  mediump vec2 tmpvar_4;
  tmpvar_4 = (xlv_TEXCOORD0.xy - (xlv_TEXCOORD1 * 3.0));
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, tmpvar_4);
  tap_1 = tmpvar_5;
  color_2 = (tap_1 * vec4(0.0205, 0.0205, 0.0205, 0.0));
  coords_3 = (tmpvar_4 + xlv_TEXCOORD1);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, coords_3);
  tap_1 = tmpvar_6;
  color_2 = (color_2 + (tap_1 * vec4(0.0855, 0.0855, 0.0855, 0.0)));
  coords_3 = (coords_3 + xlv_TEXCOORD1);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, coords_3);
  tap_1 = tmpvar_7;
  color_2 = (color_2 + (tap_1 * vec4(0.232, 0.232, 0.232, 0.0)));
  coords_3 = (coords_3 + xlv_TEXCOORD1);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, coords_3);
  tap_1 = tmpvar_8;
  color_2 = (color_2 + (tap_1 * vec4(0.324, 0.324, 0.324, 1.0)));
  coords_3 = (coords_3 + xlv_TEXCOORD1);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, coords_3);
  tap_1 = tmpvar_9;
  color_2 = (color_2 + (tap_1 * vec4(0.232, 0.232, 0.232, 0.0)));
  coords_3 = (coords_3 + xlv_TEXCOORD1);
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, coords_3);
  tap_1 = tmpvar_10;
  color_2 = (color_2 + (tap_1 * vec4(0.0855, 0.0855, 0.0855, 0.0)));
  coords_3 = (coords_3 + xlv_TEXCOORD1);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, coords_3);
  tap_1 = tmpvar_11;
  color_2 = (color_2 + (tap_1 * vec4(0.0205, 0.0205, 0.0205, 0.0)));
  coords_3 = (coords_3 + xlv_TEXCOORD1);
  gl_FragData[0] = color_2;
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
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	mediump vec4 _MainTex_TexelSize;
uniform 	mediump vec4 _Parameter;
in highp vec4 in_POSITION0;
in mediump vec2 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
highp vec4 t0;
mediump vec4 t16_0;
mediump vec2 t16_1;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    vs_TEXCOORD0.zw = vec2(1.0, 1.0);
    t16_0.xw = _Parameter.xx;
    t16_0.yz = vec2(1.0, 0.0);
    t16_1.xy = t16_0.xy * _MainTex_TexelSize.xy;
    vs_TEXCOORD1.xy = vec2(t16_0.z * t16_1.x, t16_0.w * t16_1.y);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
uniform lowp sampler2D _MainTex;
in mediump vec4 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
layout(location = 0) out mediump vec4 SV_Target0;
mediump vec2 t16_0;
lowp vec4 t10_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
mediump vec4 t16_2;
lowp vec4 t10_2;
mediump vec2 t16_6;
void main()
{
    t16_0.xy = (-vs_TEXCOORD1.xy) * vec2(3.0, 3.0) + vs_TEXCOORD0.xy;
    t16_6.xy = t16_0.xy + vs_TEXCOORD1.xy;
    t10_1 = texture(_MainTex, t16_0.xy);
    t10_2 = texture(_MainTex, t16_6.xy);
    t16_0.xy = t16_6.xy + vs_TEXCOORD1.xy;
    t16_2 = t10_2 * vec4(0.0855000019, 0.0855000019, 0.0855000019, 0.0);
    t16_1 = t10_1 * vec4(0.0205000006, 0.0205000006, 0.0205000006, 0.0) + t16_2;
    t10_2 = texture(_MainTex, t16_0.xy);
    t16_0.xy = t16_0.xy + vs_TEXCOORD1.xy;
    t16_1 = t10_2 * vec4(0.231999993, 0.231999993, 0.231999993, 0.0) + t16_1;
    t10_2 = texture(_MainTex, t16_0.xy);
    t16_0.xy = t16_0.xy + vs_TEXCOORD1.xy;
    t16_1 = t10_2 * vec4(0.324000001, 0.324000001, 0.324000001, 1.0) + t16_1;
    t10_2 = texture(_MainTex, t16_0.xy);
    t16_0.xy = t16_0.xy + vs_TEXCOORD1.xy;
    t16_1 = t10_2 * vec4(0.231999993, 0.231999993, 0.231999993, 0.0) + t16_1;
    t10_2 = texture(_MainTex, t16_0.xy);
    t16_0.xy = t16_0.xy + vs_TEXCOORD1.xy;
    t10_0 = texture(_MainTex, t16_0.xy);
    t16_1 = t10_2 * vec4(0.0855000019, 0.0855000019, 0.0855000019, 0.0) + t16_1;
    SV_Target0 = t10_0 * vec4(0.0205000006, 0.0205000006, 0.0205000006, 0.0) + t16_1;
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
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  GpuProgramID 172737
Program "vp" {
SubProgram "gles " {
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform mediump vec4 _MainTex_TexelSize;
uniform mediump vec4 _Parameter;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 tmpvar_1;
  tmpvar_1.zw = vec2(1.0, 1.0);
  tmpvar_1.xy = _glesMultiTexCoord0.xy;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_MainTex_TexelSize.xy * vec2(1.0, 0.0)) * _Parameter.x);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 tap_1;
  mediump vec4 color_2;
  mediump vec2 coords_3;
  mediump vec2 tmpvar_4;
  tmpvar_4 = (xlv_TEXCOORD0.xy - (xlv_TEXCOORD1 * 3.0));
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, tmpvar_4);
  tap_1 = tmpvar_5;
  color_2 = (tap_1 * vec4(0.0205, 0.0205, 0.0205, 0.0));
  coords_3 = (tmpvar_4 + xlv_TEXCOORD1);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, coords_3);
  tap_1 = tmpvar_6;
  color_2 = (color_2 + (tap_1 * vec4(0.0855, 0.0855, 0.0855, 0.0)));
  coords_3 = (coords_3 + xlv_TEXCOORD1);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, coords_3);
  tap_1 = tmpvar_7;
  color_2 = (color_2 + (tap_1 * vec4(0.232, 0.232, 0.232, 0.0)));
  coords_3 = (coords_3 + xlv_TEXCOORD1);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, coords_3);
  tap_1 = tmpvar_8;
  color_2 = (color_2 + (tap_1 * vec4(0.324, 0.324, 0.324, 1.0)));
  coords_3 = (coords_3 + xlv_TEXCOORD1);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, coords_3);
  tap_1 = tmpvar_9;
  color_2 = (color_2 + (tap_1 * vec4(0.232, 0.232, 0.232, 0.0)));
  coords_3 = (coords_3 + xlv_TEXCOORD1);
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, coords_3);
  tap_1 = tmpvar_10;
  color_2 = (color_2 + (tap_1 * vec4(0.0855, 0.0855, 0.0855, 0.0)));
  coords_3 = (coords_3 + xlv_TEXCOORD1);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, coords_3);
  tap_1 = tmpvar_11;
  color_2 = (color_2 + (tap_1 * vec4(0.0205, 0.0205, 0.0205, 0.0)));
  coords_3 = (coords_3 + xlv_TEXCOORD1);
  gl_FragData[0] = color_2;
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
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	mediump vec4 _MainTex_TexelSize;
uniform 	mediump vec4 _Parameter;
in highp vec4 in_POSITION0;
in mediump vec2 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
highp vec4 t0;
mediump vec4 t16_0;
mediump vec2 t16_1;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    vs_TEXCOORD0.zw = vec2(1.0, 1.0);
    t16_0.xw = vec2(1.0, 0.0);
    t16_0.yz = _Parameter.xx;
    t16_1.xy = t16_0.xy * _MainTex_TexelSize.xy;
    vs_TEXCOORD1.xy = vec2(t16_0.z * t16_1.x, t16_0.w * t16_1.y);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
uniform lowp sampler2D _MainTex;
in mediump vec4 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
layout(location = 0) out mediump vec4 SV_Target0;
mediump vec2 t16_0;
lowp vec4 t10_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
mediump vec4 t16_2;
lowp vec4 t10_2;
mediump vec2 t16_6;
void main()
{
    t16_0.xy = (-vs_TEXCOORD1.xy) * vec2(3.0, 3.0) + vs_TEXCOORD0.xy;
    t16_6.xy = t16_0.xy + vs_TEXCOORD1.xy;
    t10_1 = texture(_MainTex, t16_0.xy);
    t10_2 = texture(_MainTex, t16_6.xy);
    t16_0.xy = t16_6.xy + vs_TEXCOORD1.xy;
    t16_2 = t10_2 * vec4(0.0855000019, 0.0855000019, 0.0855000019, 0.0);
    t16_1 = t10_1 * vec4(0.0205000006, 0.0205000006, 0.0205000006, 0.0) + t16_2;
    t10_2 = texture(_MainTex, t16_0.xy);
    t16_0.xy = t16_0.xy + vs_TEXCOORD1.xy;
    t16_1 = t10_2 * vec4(0.231999993, 0.231999993, 0.231999993, 0.0) + t16_1;
    t10_2 = texture(_MainTex, t16_0.xy);
    t16_0.xy = t16_0.xy + vs_TEXCOORD1.xy;
    t16_1 = t10_2 * vec4(0.324000001, 0.324000001, 0.324000001, 1.0) + t16_1;
    t10_2 = texture(_MainTex, t16_0.xy);
    t16_0.xy = t16_0.xy + vs_TEXCOORD1.xy;
    t16_1 = t10_2 * vec4(0.231999993, 0.231999993, 0.231999993, 0.0) + t16_1;
    t10_2 = texture(_MainTex, t16_0.xy);
    t16_0.xy = t16_0.xy + vs_TEXCOORD1.xy;
    t10_0 = texture(_MainTex, t16_0.xy);
    t16_1 = t10_2 * vec4(0.0855000019, 0.0855000019, 0.0855000019, 0.0) + t16_1;
    SV_Target0 = t10_0 * vec4(0.0205000006, 0.0205000006, 0.0205000006, 0.0) + t16_1;
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
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  GpuProgramID 206922
Program "vp" {
SubProgram "gles " {
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform mediump vec4 _MainTex_TexelSize;
uniform mediump vec4 _Parameter;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD1_1;
varying mediump vec4 xlv_TEXCOORD1_2;
void main ()
{
  mediump vec4 coords_1;
  mediump vec4 tmpvar_2;
  mediump vec4 tmpvar_3;
  tmpvar_3.zw = vec2(1.0, 1.0);
  tmpvar_3.xy = _glesMultiTexCoord0.xy;
  mediump vec2 tmpvar_4;
  tmpvar_4 = ((_MainTex_TexelSize.xy * vec2(0.0, 1.0)) * _Parameter.x);
  mediump vec4 tmpvar_5;
  tmpvar_5 = (-(tmpvar_4.xyxy) * 3.0);
  coords_1 = (tmpvar_5 + tmpvar_4.xyxy);
  tmpvar_2 = (_glesMultiTexCoord0.xyxy + (coords_1 * vec4(1.0, 1.0, -1.0, -1.0)));
  coords_1 = (coords_1 + tmpvar_4.xyxy);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3.xy;
  xlv_TEXCOORD1 = (_glesMultiTexCoord0.xyxy + (tmpvar_5 * vec4(1.0, 1.0, -1.0, -1.0)));
  xlv_TEXCOORD1_1 = tmpvar_2;
  xlv_TEXCOORD1_2 = (_glesMultiTexCoord0.xyxy + (coords_1 * vec4(1.0, 1.0, -1.0, -1.0)));
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD1_1;
varying mediump vec4 xlv_TEXCOORD1_2;
void main ()
{
  mediump vec4 tapB_1;
  mediump vec4 tapA_2;
  mediump vec4 color_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1.xy);
  tapA_2 = tmpvar_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD1.zw);
  tapB_1 = tmpvar_6;
  color_3 = ((tmpvar_4 * vec4(0.324, 0.324, 0.324, 1.0)) + ((tapA_2 + tapB_1) * vec4(0.0205, 0.0205, 0.0205, 0.0)));
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD1_1.xy);
  tapA_2 = tmpvar_7;
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD1_1.zw);
  tapB_1 = tmpvar_8;
  color_3 = (color_3 + ((tapA_2 + tapB_1) * vec4(0.0855, 0.0855, 0.0855, 0.0)));
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD1_2.xy);
  tapA_2 = tmpvar_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD1_2.zw);
  tapB_1 = tmpvar_10;
  color_3 = (color_3 + ((tapA_2 + tapB_1) * vec4(0.232, 0.232, 0.232, 0.0)));
  gl_FragData[0] = color_3;
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
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	mediump vec4 _MainTex_TexelSize;
uniform 	mediump vec4 _Parameter;
in highp vec4 in_POSITION0;
in mediump vec2 in_TEXCOORD0;
out mediump vec2 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec4 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
highp vec4 t0;
mediump vec3 t16_1;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t16_1.x = _Parameter.x;
    t16_1.y = 1.0;
    t16_1.xy = t16_1.xy * _MainTex_TexelSize.xy;
    t16_1.z = t16_1.y * _Parameter.x;
    vs_TEXCOORD1 = t16_1.xzxz * vec4(-0.0, -3.0, 0.0, 3.0) + in_TEXCOORD0.xyxy;
    vs_TEXCOORD2 = t16_1.xzxz * vec4(0.0, -2.0, -0.0, 2.0) + in_TEXCOORD0.xyxy;
    vs_TEXCOORD3 = t16_1.xzxz * vec4(0.0, -1.0, -0.0, 1.0) + in_TEXCOORD0.xyxy;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
uniform lowp sampler2D _MainTex;
in mediump vec2 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec4 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
layout(location = 0) out mediump vec4 SV_Target0;
mediump vec4 t16_0;
lowp vec4 t10_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec4 t10_2;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD1.xy);
    t10_1 = texture(_MainTex, vs_TEXCOORD1.zw);
    t16_0 = t10_0 + t10_1;
    t16_0 = t16_0 * vec4(0.0205000006, 0.0205000006, 0.0205000006, 0.0);
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_0 = t10_1 * vec4(0.324000001, 0.324000001, 0.324000001, 1.0) + t16_0;
    t10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
    t10_2 = texture(_MainTex, vs_TEXCOORD2.zw);
    t16_1 = t10_1 + t10_2;
    t16_0 = t16_1 * vec4(0.0855000019, 0.0855000019, 0.0855000019, 0.0) + t16_0;
    t10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
    t10_2 = texture(_MainTex, vs_TEXCOORD3.zw);
    t16_1 = t10_1 + t10_2;
    SV_Target0 = t16_1 * vec4(0.231999993, 0.231999993, 0.231999993, 0.0) + t16_0;
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
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  GpuProgramID 310685
Program "vp" {
SubProgram "gles " {
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform mediump vec4 _MainTex_TexelSize;
uniform mediump vec4 _Parameter;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD1_1;
varying mediump vec4 xlv_TEXCOORD1_2;
void main ()
{
  mediump vec4 coords_1;
  mediump vec4 tmpvar_2;
  mediump vec2 tmpvar_3;
  tmpvar_3 = ((_MainTex_TexelSize.xy * vec2(1.0, 0.0)) * _Parameter.x);
  mediump vec4 tmpvar_4;
  tmpvar_4 = (-(tmpvar_3.xyxy) * 3.0);
  coords_1 = (tmpvar_4 + tmpvar_3.xyxy);
  tmpvar_2 = (_glesMultiTexCoord0.xyxy + (coords_1 * vec4(1.0, 1.0, -1.0, -1.0)));
  coords_1 = (coords_1 + tmpvar_3.xyxy);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = (_glesMultiTexCoord0.xyxy + (tmpvar_4 * vec4(1.0, 1.0, -1.0, -1.0)));
  xlv_TEXCOORD1_1 = tmpvar_2;
  xlv_TEXCOORD1_2 = (_glesMultiTexCoord0.xyxy + (coords_1 * vec4(1.0, 1.0, -1.0, -1.0)));
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD1_1;
varying mediump vec4 xlv_TEXCOORD1_2;
void main ()
{
  mediump vec4 tapB_1;
  mediump vec4 tapA_2;
  mediump vec4 color_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1.xy);
  tapA_2 = tmpvar_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD1.zw);
  tapB_1 = tmpvar_6;
  color_3 = ((tmpvar_4 * vec4(0.324, 0.324, 0.324, 1.0)) + ((tapA_2 + tapB_1) * vec4(0.0205, 0.0205, 0.0205, 0.0)));
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD1_1.xy);
  tapA_2 = tmpvar_7;
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD1_1.zw);
  tapB_1 = tmpvar_8;
  color_3 = (color_3 + ((tapA_2 + tapB_1) * vec4(0.0855, 0.0855, 0.0855, 0.0)));
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD1_2.xy);
  tapA_2 = tmpvar_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD1_2.zw);
  tapB_1 = tmpvar_10;
  color_3 = (color_3 + ((tapA_2 + tapB_1) * vec4(0.232, 0.232, 0.232, 0.0)));
  gl_FragData[0] = color_3;
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
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	mediump vec4 _MainTex_TexelSize;
uniform 	mediump vec4 _Parameter;
in highp vec4 in_POSITION0;
in mediump vec2 in_TEXCOORD0;
out mediump vec2 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec4 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
highp vec4 t0;
mediump vec3 t16_1;
mediump vec2 t16_3;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t16_3.x = 1.0;
    t16_3.y = _Parameter.x;
    t16_1.yz = t16_3.xy * _MainTex_TexelSize.xy;
    t16_1.x = t16_1.y * _Parameter.x;
    vs_TEXCOORD1 = t16_1.xzxz * vec4(-3.0, -0.0, 3.0, 0.0) + in_TEXCOORD0.xyxy;
    vs_TEXCOORD2 = t16_1.xzxz * vec4(-2.0, 0.0, 2.0, -0.0) + in_TEXCOORD0.xyxy;
    vs_TEXCOORD3 = t16_1.xzxz * vec4(-1.0, 0.0, 1.0, -0.0) + in_TEXCOORD0.xyxy;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
uniform lowp sampler2D _MainTex;
in mediump vec2 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec4 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
layout(location = 0) out mediump vec4 SV_Target0;
mediump vec4 t16_0;
lowp vec4 t10_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec4 t10_2;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD1.xy);
    t10_1 = texture(_MainTex, vs_TEXCOORD1.zw);
    t16_0 = t10_0 + t10_1;
    t16_0 = t16_0 * vec4(0.0205000006, 0.0205000006, 0.0205000006, 0.0);
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_0 = t10_1 * vec4(0.324000001, 0.324000001, 0.324000001, 1.0) + t16_0;
    t10_1 = texture(_MainTex, vs_TEXCOORD2.xy);
    t10_2 = texture(_MainTex, vs_TEXCOORD2.zw);
    t16_1 = t10_1 + t10_2;
    t16_0 = t16_1 * vec4(0.0855000019, 0.0855000019, 0.0855000019, 0.0) + t16_0;
    t10_1 = texture(_MainTex, vs_TEXCOORD3.xy);
    t10_2 = texture(_MainTex, vs_TEXCOORD3.zw);
    t16_1 = t10_1 + t10_2;
    SV_Target0 = t16_1 * vec4(0.231999993, 0.231999993, 0.231999993, 0.0) + t16_0;
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
Fallback Off
}
Shader "Animus/BillboardMeshCubemapHorizont" {
Properties {
 _MainTex ("Billboard Texture", 2D) = "white" { }
 _FadeOffset ("Fade Offset", Float) = 0.5
}
SubShader { 
 Tags { "QUEUE"="Background" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Background" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha One
  ColorMask RGB
  GpuProgramID 45660
Program "vp" {
SubProgram "gles " {
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp mat4 glstate_matrix_projection;
uniform highp float _FadeOffset;
varying mediump vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD0;
void main ()
{
  mediump vec4 tmpvar_1;
  tmpvar_1.xyz = _glesColor.xyz;
  highp vec4 vPos_2;
  highp vec4 wPos_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (_Object2World * _glesVertex);
  wPos_3.w = tmpvar_4.w;
  wPos_3.xyz = (tmpvar_4.xyz + _WorldSpaceCameraPos);
  wPos_3 = (_World2Object * wPos_3);
  highp vec4 tmpvar_5;
  tmpvar_5 = (glstate_matrix_modelview0 * wPos_3);
  vPos_2.zw = tmpvar_5.zw;
  highp float tmpvar_6;
  tmpvar_6 = (((_glesVertex.x * _glesVertex.y) * _glesVertex.z) + (_Time.z * 0.125));
  vPos_2.x = (tmpvar_5.x + (sin(
    (tmpvar_6 * 0.5)
  ) * 0.25));
  vPos_2.y = (tmpvar_5.y + (sin(tmpvar_6) * 0.1));
  tmpvar_1.w = (vPos_2.y + _FadeOffset);
  vPos_2 = (vPos_2 + (_glesMultiTexCoord1 * 1.2));
  gl_Position = (glstate_matrix_projection * vPos_2);
  xlv_COLOR = tmpvar_1;
  xlv_TEXCOORD0 = _glesMultiTexCoord0;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
varying mediump vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  gl_FragData[0] = (tmpvar_1 * xlv_COLOR);
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
uniform 	float _FadeOffset;
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in mediump vec4 in_TEXCOORD0;
in mediump vec4 in_TEXCOORD1;
out mediump vec4 vs_COLOR0;
out mediump vec4 vs_TEXCOORD0;
highp vec4 t0;
highp vec4 t1;
highp float t3;
void main()
{
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t0.xyz = t0.xyz + _WorldSpaceCameraPos.xyzx.xyz;
    t1 = t0.yyyy * _World2Object[1];
    t1 = _World2Object[0] * t0.xxxx + t1;
    t1 = _World2Object[2] * t0.zzzz + t1;
    t0 = _World2Object[3] * t0.wwww + t1;
    t1 = t0.yyyy * glstate_matrix_modelview0[1];
    t1 = glstate_matrix_modelview0[0] * t0.xxxx + t1;
    t1 = glstate_matrix_modelview0[2] * t0.zzzz + t1;
    t0 = glstate_matrix_modelview0[3] * t0.wwww + t1;
    t1.x = in_POSITION0.y * in_POSITION0.x;
    t3 = _Time.z * 0.125;
    t1.x = t1.x * in_POSITION0.z + t3;
    t3 = t1.x * 0.5;
    t1.x = sin(t1.x);
    t0.y = t1.x * 0.100000001 + t0.y;
    t1.x = sin(t3);
    t0.x = t1.x * 0.25 + t0.x;
    t1 = in_TEXCOORD1 * vec4(1.20000005, 1.20000005, 1.20000005, 1.20000005) + t0;
    t0.x = t0.y + _FadeOffset;
    vs_COLOR0.w = t0.x;
    t0 = t1.yyyy * glstate_matrix_projection[1];
    t0 = glstate_matrix_projection[0] * t1.xxxx + t0;
    t0 = glstate_matrix_projection[2] * t1.zzzz + t0;
    gl_Position = glstate_matrix_projection[3] * t1.wwww + t0;
    vs_COLOR0.xyz = in_COLOR0.xyz;
    vs_TEXCOORD0 = in_TEXCOORD0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
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
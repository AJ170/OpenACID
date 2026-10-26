Shader "Animus/AtmosphereClouds" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _Color ("Color", Color) = (1,1,1,0.5)
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  Lighting On
  ZWrite Off
  Cull Off
  Blend SrcAlpha One
  GpuProgramID 47168
Program "vp" {
SubProgram "gles " {
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _Color;
varying highp vec3 xlv_TEXCOORD;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_COLOR;
void main ()
{
  mediump float distance_1;
  highp vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = (_Object2World * _glesVertex);
  tmpvar_2 = (_WorldSpaceCameraPos - tmpvar_3.xyz);
  highp float tmpvar_4;
  tmpvar_4 = sqrt(dot (tmpvar_2, tmpvar_2));
  distance_1 = tmpvar_4;
  mediump float tmpvar_5;
  tmpvar_5 = clamp ((-2.0 + (distance_1 * 0.6)), 0.0, 1.0);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 0.0;
  tmpvar_6.xyz = _glesNormal;
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize((_Object2World * tmpvar_6).xyz);
  highp vec4 tmpvar_8;
  tmpvar_8.xyz = (_Color.xyz * vec3((1.5 * max (0.0, 
    dot (tmpvar_7, normalize(_WorldSpaceLightPos0.xyz))
  ))));
  tmpvar_8.w = tmpvar_5;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD = tmpvar_7;
  xlv_TEXCOORD1 = normalize((_WorldSpaceCameraPos - tmpvar_3.xyz));
  xlv_TEXCOORD2 = _glesMultiTexCoord0;
  xlv_COLOR = tmpvar_8;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_COLOR;
void main ()
{
  highp vec4 textureColor_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD2.xy);
  textureColor_1 = tmpvar_2;
  textureColor_1 = (textureColor_1 * xlv_COLOR);
  gl_FragData[0] = textureColor_1;
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
uniform 	vec4 _Color;
uniform 	vec4 _LightColor0;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out highp vec3 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec4 vs_TEXCOORD2;
out highp vec4 vs_COLOR0;
highp vec4 t0;
highp vec3 t1;
mediump float t16_2;
highp float t9;
highp float t10;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t0.xyz;
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    vs_TEXCOORD0.xyz = t0.xyz;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t10 = inversesqrt(t9);
    t9 = sqrt(t9);
    t16_2 = t9 * 0.600000024 + -2.0;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    vs_COLOR0.w = t16_2;
    vs_TEXCOORD1.xyz = vec3(t10) * t1.xyz;
    vs_TEXCOORD2 = in_TEXCOORD0;
    t9 = dot(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * _WorldSpaceLightPos0.xyz;
    t0.x = dot(t0.xyz, t1.xyz);
    t0.x = max(t0.x, 0.0);
    t0.x = t0.x * 1.5;
    vs_COLOR0.xyz = t0.xxx * _Color.xyz;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
uniform lowp sampler2D _MainTex;
in highp vec4 vs_TEXCOORD2;
in highp vec4 vs_COLOR0;
layout(location = 0) out highp vec4 SV_Target0;
lowp vec4 t10_0;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD2.xy);
    SV_Target0 = t10_0 * vs_COLOR0;
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
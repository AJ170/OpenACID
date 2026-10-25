Shader "Environment/EnvDiffuseFade" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _FadeColor ("Fade Color", Color) = (1,1,1,1)
}
SubShader { 
 LOD 99
 Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="AlphaTest" "RenderType"="Opaque" "EagleVision"="humanoid" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="AlphaTest" "RenderType"="Opaque" "EagleVision"="humanoid" }
  ZTest Less
  ColorMask 0
  GpuProgramID 28047
Program "vp" {
SubProgram "gles " {
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
uniform highp mat4 glstate_matrix_mvp;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
}


#endif
#ifdef FRAGMENT
void main ()
{
  gl_FragData[0] = vec4(1.0, 1.0, 1.0, 1.0);
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
in highp vec4 in_POSITION0;
highp vec4 t0;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
layout(location = 0) out lowp vec4 SV_Target0;
void main()
{
    SV_Target0 = vec4(1.0, 1.0, 1.0, 1.0);
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
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="AlphaTest" "RenderType"="Opaque" "EagleVision"="humanoid" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  GpuProgramID 96649
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_OFF" "SIMPLE_FADE" }
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
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 worldNormal_2;
  mediump vec3 worldViewDir_3;
  mediump vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  tmpvar_5.x = 0.0;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_6;
  tmpvar_6 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_3 = tmpvar_6;
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((tmpvar_7 * _glesNormal));
  worldNormal_2 = tmpvar_8;
  highp float tmpvar_9;
  highp vec3 x_10;
  x_10 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_9 = sqrt(dot (x_10, x_10));
  fogpos_1 = tmpvar_9;
  tmpvar_5.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_5.x = (1.0 - tmpvar_5.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = worldNormal_2;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD3 = worldViewDir_3;
  xlv_TEXCOORD4 = tmpvar_5;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16 = (c_3 * _FadeColor);
  lowp vec4 _color_17;
  _color_17.w = _color_16.w;
  _color_17.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_17.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_17.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_OFF" "SIMPLE_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD3;
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
    t0.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t0.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD0.xyz = t0.xyz;
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
    vs_TEXCOORD4.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_3;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t0.xy;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump float t16_6;
mediump float t16_15;
void main()
{
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_15 = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_15 = max(t16_15, 0.0);
    t16_1.x = t16_15 * _LightDir.w + t10_4;
    t16_15 = t16_15 * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_1.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_15) * t16_1.xyz + t16_0.xyz;
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_1.x = float(ti3);
    t16_6 = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_6 + t16_1.x;
    t16_1.xyz = vec3(t16_15) * _RimColor.xyz;
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz + t16_1.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_OFF" "FANCY_FADE" }
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
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 worldNormal_2;
  mediump vec3 worldViewDir_3;
  mediump vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  tmpvar_5.x = 0.0;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_6;
  tmpvar_6 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_3 = tmpvar_6;
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((tmpvar_7 * _glesNormal));
  worldNormal_2 = tmpvar_8;
  highp float tmpvar_9;
  highp vec3 x_10;
  x_10 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_9 = sqrt(dot (x_10, x_10));
  fogpos_1 = tmpvar_9;
  tmpvar_5.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_5.x = (1.0 - tmpvar_5.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = worldNormal_2;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD3 = worldViewDir_3;
  xlv_TEXCOORD4 = tmpvar_5;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16.w = c_3.w;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_NoiseTexture, xlv_TEXCOORD1);
  mediump float tmpvar_18;
  tmpvar_18 = clamp ((1.0 - (32.0 * 
    abs((_FadeColor.w - ((0.9375 * tmpvar_17.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_16.xyz = (_FadeColor.xyz * (tmpvar_18 + tmpvar_12));
  lowp vec4 _color_19;
  _color_19.w = _color_16.w;
  _color_19.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_19.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_19.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_OFF" "FANCY_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD3;
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
    t0.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t0.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD0.xyz = t0.xyz;
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
    vs_TEXCOORD4.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_3;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t0.xy;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _NoiseTexture;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump float t16_6;
mediump float t16_15;
void main()
{
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_15 = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_15 = max(t16_15, 0.0);
    t16_1.x = t16_15 * _LightDir.w + t10_4;
    t16_15 = t16_15 * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_1.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_15) * t16_1.xyz + t16_0.xyz;
    t10_3.x = texture(_NoiseTexture, vs_TEXCOORD1.xy).y;
    t16_15 = t10_3.x * 0.9375 + 0.03125;
    t16_15 = (-t16_15) + _FadeColor.w;
    t16_15 = -abs(t16_15) * 32.0 + 1.0;
    t16_15 = max(t16_15, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_15);
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_1.x = float(ti3);
    t16_6 = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_6 + t16_1.x;
    t16_1.xyz = vec3(t16_15) * _RimColor.xyz;
    t16_0.xyz = _FadeColor.xyz * t16_0.xyz + t16_1.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_OFF" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 worldNormal_2;
  mediump vec3 worldViewDir_3;
  highp vec4 tmpvar_4;
  mediump vec2 tmpvar_5;
  mediump vec2 tmpvar_6;
  mediump vec3 tmpvar_7;
  tmpvar_6.x = 0.0;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_8;
  highp vec4 tmpvar_9;
  tmpvar_9 = (tmpvar_4 * 0.5);
  highp vec2 tmpvar_10;
  tmpvar_10.x = tmpvar_9.x;
  tmpvar_10.y = (tmpvar_9.y * _ProjectionParams.x);
  o_8.xy = (tmpvar_10 + tmpvar_9.w);
  o_8.zw = tmpvar_4.zw;
  tmpvar_7 = o_8.xyw;
  tmpvar_5 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_3 = tmpvar_11;
  highp mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize((tmpvar_12 * _glesNormal));
  worldNormal_2 = tmpvar_13;
  highp float tmpvar_14;
  highp vec3 x_15;
  x_15 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_14 = sqrt(dot (x_15, x_15));
  fogpos_1 = tmpvar_14;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  gl_Position = tmpvar_4;
  xlv_TEXCOORD0 = worldNormal_2;
  xlv_TEXCOORD1 = tmpvar_5;
  xlv_TEXCOORD3 = worldViewDir_3;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16.w = c_3.w;
  mediump float rTest_17;
  mediump vec2 tmpvar_18;
  tmpvar_18 = (xlv_TEXCOORD6.xy / xlv_TEXCOORD6.z);
  mediump float tmpvar_19;
  tmpvar_19 = ((1.5 * _FadeColor.w) - tmpvar_18.y);
  if ((tmpvar_19 < 0.0)) {
    discard;
  };
  mediump float tmpvar_20;
  tmpvar_20 = clamp ((1.0 - (tmpvar_19 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_21;
  tmpvar_21.x = tmpvar_18.x;
  tmpvar_21.y = clamp ((tmpvar_20 - tmpvar_18.y), 0.0, 1.0);
  lowp float tmpvar_22;
  tmpvar_22 = texture2D (_RasterTex, tmpvar_21).z;
  rTest_17 = tmpvar_22;
  mediump float x_23;
  x_23 = (rTest_17 - 0.4);
  if ((x_23 < 0.0)) {
    discard;
  };
  _color_16.xyz = (tmpvar_12 + (_FadeColor.xyz * tmpvar_20));
  lowp vec4 _color_24;
  _color_24.w = _color_16.w;
  _color_24.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_24.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_24.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_OFF" "RASTER_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD3;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_2;
mediump float t16_5;
highp float t9;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t1.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * t1.xyz;
    vs_TEXCOORD0.xyz = t1.xyz;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = sqrt(t9);
    t16_2 = (-t9) + acid_Fog.y;
    t16_5 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_5;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_5 = t16_2 * acid_Fog.z;
    vs_TEXCOORD4.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_5;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t1.xy;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * t1.xyz;
    vs_TEXCOORD3.xyz = t1.xyz;
    t0.w = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t1.zz + t1.xw;
    vs_TEXCOORD6.xyz = t0.xyz;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _RasterTex;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
in mediump vec3 vs_TEXCOORD6;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp float t10_1;
bool tb1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump vec3 t16_7;
mediump float t16_15;
void main()
{
    t16_0.xy = vs_TEXCOORD6.xy / vs_TEXCOORD6.zz;
    t16_15 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_15<0.0;
    t16_15 = (-t16_15) * 2.5 + 1.0;
    t16_15 = clamp(t16_15, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_15;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1 = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1 + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_2.x = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_2.x = max(t16_2.x, 0.0);
    t16_7.x = t16_2.x * _LightDir.w + t10_4;
    t16_2.x = t16_2.x * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_7.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_7.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = t16_2.xxx * t16_7.xyz + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_15) + t16_0.xyz;
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_2.x = float(ti3);
    t16_7.x = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_7.x + t16_2.x;
    t16_0.xyz = vec3(t16_15) * _RimColor.xyz + t16_0.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "SIMPLE_FADE" }
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
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 worldNormal_2;
  mediump vec3 worldViewDir_3;
  mediump vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  tmpvar_5.x = 0.0;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_6;
  tmpvar_6 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_3 = tmpvar_6;
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((tmpvar_7 * _glesNormal));
  worldNormal_2 = tmpvar_8;
  highp float tmpvar_9;
  highp vec3 x_10;
  x_10 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_9 = sqrt(dot (x_10, x_10));
  fogpos_1 = tmpvar_9;
  tmpvar_5.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_5.x = (1.0 - tmpvar_5.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = worldNormal_2;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD3 = worldViewDir_3;
  xlv_TEXCOORD4 = tmpvar_5;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16 = (c_3 * _FadeColor);
  lowp vec4 _color_17;
  _color_17.w = _color_16.w;
  _color_17.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_17.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_17.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "SIMPLE_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD3;
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
    t0.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t0.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD0.xyz = t0.xyz;
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
    vs_TEXCOORD4.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_3;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t0.xy;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump float t16_6;
mediump float t16_15;
void main()
{
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_15 = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_15 = max(t16_15, 0.0);
    t16_1.x = t16_15 * _LightDir.w + t10_4;
    t16_15 = t16_15 * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_1.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_15) * t16_1.xyz + t16_0.xyz;
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_1.x = float(ti3);
    t16_6 = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_6 + t16_1.x;
    t16_1.xyz = vec3(t16_15) * _RimColor.xyz;
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz + t16_1.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "FANCY_FADE" }
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
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 worldNormal_2;
  mediump vec3 worldViewDir_3;
  mediump vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  tmpvar_5.x = 0.0;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_6;
  tmpvar_6 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_3 = tmpvar_6;
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((tmpvar_7 * _glesNormal));
  worldNormal_2 = tmpvar_8;
  highp float tmpvar_9;
  highp vec3 x_10;
  x_10 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_9 = sqrt(dot (x_10, x_10));
  fogpos_1 = tmpvar_9;
  tmpvar_5.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_5.x = (1.0 - tmpvar_5.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = worldNormal_2;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD3 = worldViewDir_3;
  xlv_TEXCOORD4 = tmpvar_5;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16.w = c_3.w;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_NoiseTexture, xlv_TEXCOORD1);
  mediump float tmpvar_18;
  tmpvar_18 = clamp ((1.0 - (32.0 * 
    abs((_FadeColor.w - ((0.9375 * tmpvar_17.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_16.xyz = (_FadeColor.xyz * (tmpvar_18 + tmpvar_12));
  lowp vec4 _color_19;
  _color_19.w = _color_16.w;
  _color_19.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_19.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_19.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "FANCY_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD3;
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
    t0.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t0.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD0.xyz = t0.xyz;
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
    vs_TEXCOORD4.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_3;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t0.xy;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _NoiseTexture;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump float t16_6;
mediump float t16_15;
void main()
{
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_15 = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_15 = max(t16_15, 0.0);
    t16_1.x = t16_15 * _LightDir.w + t10_4;
    t16_15 = t16_15 * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_1.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_15) * t16_1.xyz + t16_0.xyz;
    t10_3.x = texture(_NoiseTexture, vs_TEXCOORD1.xy).y;
    t16_15 = t10_3.x * 0.9375 + 0.03125;
    t16_15 = (-t16_15) + _FadeColor.w;
    t16_15 = -abs(t16_15) * 32.0 + 1.0;
    t16_15 = max(t16_15, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_15);
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_1.x = float(ti3);
    t16_6 = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_6 + t16_1.x;
    t16_1.xyz = vec3(t16_15) * _RimColor.xyz;
    t16_0.xyz = _FadeColor.xyz * t16_0.xyz + t16_1.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 worldNormal_2;
  mediump vec3 worldViewDir_3;
  highp vec4 tmpvar_4;
  mediump vec2 tmpvar_5;
  mediump vec2 tmpvar_6;
  mediump vec3 tmpvar_7;
  tmpvar_6.x = 0.0;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_8;
  highp vec4 tmpvar_9;
  tmpvar_9 = (tmpvar_4 * 0.5);
  highp vec2 tmpvar_10;
  tmpvar_10.x = tmpvar_9.x;
  tmpvar_10.y = (tmpvar_9.y * _ProjectionParams.x);
  o_8.xy = (tmpvar_10 + tmpvar_9.w);
  o_8.zw = tmpvar_4.zw;
  tmpvar_7 = o_8.xyw;
  tmpvar_5 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_3 = tmpvar_11;
  highp mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize((tmpvar_12 * _glesNormal));
  worldNormal_2 = tmpvar_13;
  highp float tmpvar_14;
  highp vec3 x_15;
  x_15 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_14 = sqrt(dot (x_15, x_15));
  fogpos_1 = tmpvar_14;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  gl_Position = tmpvar_4;
  xlv_TEXCOORD0 = worldNormal_2;
  xlv_TEXCOORD1 = tmpvar_5;
  xlv_TEXCOORD3 = worldViewDir_3;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16.w = c_3.w;
  mediump float rTest_17;
  mediump vec2 tmpvar_18;
  tmpvar_18 = (xlv_TEXCOORD6.xy / xlv_TEXCOORD6.z);
  mediump float tmpvar_19;
  tmpvar_19 = ((1.5 * _FadeColor.w) - tmpvar_18.y);
  if ((tmpvar_19 < 0.0)) {
    discard;
  };
  mediump float tmpvar_20;
  tmpvar_20 = clamp ((1.0 - (tmpvar_19 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_21;
  tmpvar_21.x = tmpvar_18.x;
  tmpvar_21.y = clamp ((tmpvar_20 - tmpvar_18.y), 0.0, 1.0);
  lowp float tmpvar_22;
  tmpvar_22 = texture2D (_RasterTex, tmpvar_21).z;
  rTest_17 = tmpvar_22;
  mediump float x_23;
  x_23 = (rTest_17 - 0.4);
  if ((x_23 < 0.0)) {
    discard;
  };
  _color_16.xyz = (tmpvar_12 + (_FadeColor.xyz * tmpvar_20));
  lowp vec4 _color_24;
  _color_24.w = _color_16.w;
  _color_24.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_24.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_24.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "RASTER_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD3;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_2;
mediump float t16_5;
highp float t9;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t1.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * t1.xyz;
    vs_TEXCOORD0.xyz = t1.xyz;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = sqrt(t9);
    t16_2 = (-t9) + acid_Fog.y;
    t16_5 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_5;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_5 = t16_2 * acid_Fog.z;
    vs_TEXCOORD4.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_5;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t1.xy;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * t1.xyz;
    vs_TEXCOORD3.xyz = t1.xyz;
    t0.w = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t1.zz + t1.xw;
    vs_TEXCOORD6.xyz = t0.xyz;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _RasterTex;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
in mediump vec3 vs_TEXCOORD6;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp float t10_1;
bool tb1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump vec3 t16_7;
mediump float t16_15;
void main()
{
    t16_0.xy = vs_TEXCOORD6.xy / vs_TEXCOORD6.zz;
    t16_15 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_15<0.0;
    t16_15 = (-t16_15) * 2.5 + 1.0;
    t16_15 = clamp(t16_15, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_15;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1 = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1 + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_2.x = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_2.x = max(t16_2.x, 0.0);
    t16_7.x = t16_2.x * _LightDir.w + t10_4;
    t16_2.x = t16_2.x * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_7.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_7.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = t16_2.xxx * t16_7.xyz + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_15) + t16_0.xyz;
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_2.x = float(ti3);
    t16_7.x = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_7.x + t16_2.x;
    t16_0.xyz = vec3(t16_15) * _RimColor.xyz + t16_0.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_OFF" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
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
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 worldNormal_2;
  mediump vec3 worldViewDir_3;
  mediump vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  mediump vec4 tmpvar_6;
  tmpvar_5.x = 0.0;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_7;
  highp vec4 tmpvar_8;
  tmpvar_8 = (_Object2World * _glesVertex);
  tmpvar_7 = normalize((_WorldSpaceCameraPos - tmpvar_8.xyz));
  worldViewDir_3 = tmpvar_7;
  highp mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * _glesNormal));
  worldNormal_2 = tmpvar_10;
  tmpvar_6 = (unity_World2Shadow[0] * tmpvar_8);
  highp float tmpvar_11;
  highp vec3 x_12;
  x_12 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_11 = sqrt(dot (x_12, x_12));
  fogpos_1 = tmpvar_11;
  tmpvar_5.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_5.x = (1.0 - tmpvar_5.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = worldNormal_2;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD3 = worldViewDir_3;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD8 = tmpvar_6;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16 = (c_3 * _FadeColor);
  lowp vec4 _color_17;
  _color_17.w = _color_16.w;
  _color_17.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_17.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_17.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_OFF" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
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
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 worldNormal_2;
  mediump vec3 worldViewDir_3;
  mediump vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  mediump vec4 tmpvar_6;
  tmpvar_5.x = 0.0;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_7;
  highp vec4 tmpvar_8;
  tmpvar_8 = (_Object2World * _glesVertex);
  tmpvar_7 = normalize((_WorldSpaceCameraPos - tmpvar_8.xyz));
  worldViewDir_3 = tmpvar_7;
  highp mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * _glesNormal));
  worldNormal_2 = tmpvar_10;
  tmpvar_6 = (unity_World2Shadow[0] * tmpvar_8);
  highp float tmpvar_11;
  highp vec3 x_12;
  x_12 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_11 = sqrt(dot (x_12, x_12));
  fogpos_1 = tmpvar_11;
  tmpvar_5.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_5.x = (1.0 - tmpvar_5.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = worldNormal_2;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD3 = worldViewDir_3;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD8 = tmpvar_6;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16.w = c_3.w;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_NoiseTexture, xlv_TEXCOORD1);
  mediump float tmpvar_18;
  tmpvar_18 = clamp ((1.0 - (32.0 * 
    abs((_FadeColor.w - ((0.9375 * tmpvar_17.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_16.xyz = (_FadeColor.xyz * (tmpvar_18 + tmpvar_12));
  lowp vec4 _color_19;
  _color_19.w = _color_16.w;
  _color_19.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_19.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_19.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_OFF" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 worldNormal_2;
  mediump vec3 worldViewDir_3;
  highp vec4 tmpvar_4;
  mediump vec2 tmpvar_5;
  mediump vec2 tmpvar_6;
  mediump vec3 tmpvar_7;
  mediump vec4 tmpvar_8;
  tmpvar_6.x = 0.0;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (tmpvar_4 * 0.5);
  highp vec2 tmpvar_11;
  tmpvar_11.x = tmpvar_10.x;
  tmpvar_11.y = (tmpvar_10.y * _ProjectionParams.x);
  o_9.xy = (tmpvar_11 + tmpvar_10.w);
  o_9.zw = tmpvar_4.zw;
  tmpvar_7 = o_9.xyw;
  tmpvar_5 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = (_Object2World * _glesVertex);
  tmpvar_12 = normalize((_WorldSpaceCameraPos - tmpvar_13.xyz));
  worldViewDir_3 = tmpvar_12;
  highp mat3 tmpvar_14;
  tmpvar_14[0] = _Object2World[0].xyz;
  tmpvar_14[1] = _Object2World[1].xyz;
  tmpvar_14[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_15;
  tmpvar_15 = normalize((tmpvar_14 * _glesNormal));
  worldNormal_2 = tmpvar_15;
  tmpvar_8 = (unity_World2Shadow[0] * tmpvar_13);
  highp float tmpvar_16;
  highp vec3 x_17;
  x_17 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_16 = sqrt(dot (x_17, x_17));
  fogpos_1 = tmpvar_16;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  gl_Position = tmpvar_4;
  xlv_TEXCOORD0 = worldNormal_2;
  xlv_TEXCOORD1 = tmpvar_5;
  xlv_TEXCOORD3 = worldViewDir_3;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
  xlv_TEXCOORD7 = tmpvar_8;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16.w = c_3.w;
  mediump float rTest_17;
  mediump vec2 tmpvar_18;
  tmpvar_18 = (xlv_TEXCOORD6.xy / xlv_TEXCOORD6.z);
  mediump float tmpvar_19;
  tmpvar_19 = ((1.5 * _FadeColor.w) - tmpvar_18.y);
  if ((tmpvar_19 < 0.0)) {
    discard;
  };
  mediump float tmpvar_20;
  tmpvar_20 = clamp ((1.0 - (tmpvar_19 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_21;
  tmpvar_21.x = tmpvar_18.x;
  tmpvar_21.y = clamp ((tmpvar_20 - tmpvar_18.y), 0.0, 1.0);
  lowp float tmpvar_22;
  tmpvar_22 = texture2D (_RasterTex, tmpvar_21).z;
  rTest_17 = tmpvar_22;
  mediump float x_23;
  x_23 = (rTest_17 - 0.4);
  if ((x_23 < 0.0)) {
    discard;
  };
  _color_16.xyz = (tmpvar_12 + (_FadeColor.xyz * tmpvar_20));
  lowp vec4 _color_24;
  _color_24.w = _color_16.w;
  _color_24.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_24.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_24.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
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
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 worldNormal_2;
  mediump vec3 worldViewDir_3;
  mediump vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  mediump vec4 tmpvar_6;
  tmpvar_5.x = 0.0;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_7;
  highp vec4 tmpvar_8;
  tmpvar_8 = (_Object2World * _glesVertex);
  tmpvar_7 = normalize((_WorldSpaceCameraPos - tmpvar_8.xyz));
  worldViewDir_3 = tmpvar_7;
  highp mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * _glesNormal));
  worldNormal_2 = tmpvar_10;
  tmpvar_6 = (unity_World2Shadow[0] * tmpvar_8);
  highp float tmpvar_11;
  highp vec3 x_12;
  x_12 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_11 = sqrt(dot (x_12, x_12));
  fogpos_1 = tmpvar_11;
  tmpvar_5.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_5.x = (1.0 - tmpvar_5.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = worldNormal_2;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD3 = worldViewDir_3;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD8 = tmpvar_6;
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
uniform lowp vec4 unity_FogColor;
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16 = (c_3 * _FadeColor);
  lowp vec4 _color_17;
  _color_17.w = _color_16.w;
  _color_17.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_17.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_17.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "SIMPLE_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
mediump float t16_3;
highp float t6;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t0.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD0.xyz = t0.xyz;
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
    vs_TEXCOORD4.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_3;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t0.xy;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump float t16_6;
mediump float t16_15;
void main()
{
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_15 = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_15 = max(t16_15, 0.0);
    t16_1.x = t16_15 * _LightDir.w + t10_4;
    t16_15 = t16_15 * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_1.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_15) * t16_1.xyz + t16_0.xyz;
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_1.x = float(ti3);
    t16_6 = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_6 + t16_1.x;
    t16_1.xyz = vec3(t16_15) * _RimColor.xyz;
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz + t16_1.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
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
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 worldNormal_2;
  mediump vec3 worldViewDir_3;
  mediump vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  mediump vec4 tmpvar_6;
  tmpvar_5.x = 0.0;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_7;
  highp vec4 tmpvar_8;
  tmpvar_8 = (_Object2World * _glesVertex);
  tmpvar_7 = normalize((_WorldSpaceCameraPos - tmpvar_8.xyz));
  worldViewDir_3 = tmpvar_7;
  highp mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * _glesNormal));
  worldNormal_2 = tmpvar_10;
  tmpvar_6 = (unity_World2Shadow[0] * tmpvar_8);
  highp float tmpvar_11;
  highp vec3 x_12;
  x_12 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_11 = sqrt(dot (x_12, x_12));
  fogpos_1 = tmpvar_11;
  tmpvar_5.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_5.x = (1.0 - tmpvar_5.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = worldNormal_2;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD3 = worldViewDir_3;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD8 = tmpvar_6;
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
uniform lowp vec4 unity_FogColor;
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16.w = c_3.w;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_NoiseTexture, xlv_TEXCOORD1);
  mediump float tmpvar_18;
  tmpvar_18 = clamp ((1.0 - (32.0 * 
    abs((_FadeColor.w - ((0.9375 * tmpvar_17.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_16.xyz = (_FadeColor.xyz * (tmpvar_18 + tmpvar_12));
  lowp vec4 _color_19;
  _color_19.w = _color_16.w;
  _color_19.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_19.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_19.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "FANCY_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
mediump float t16_3;
highp float t6;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t0.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD0.xyz = t0.xyz;
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
    vs_TEXCOORD4.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_3;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t0.xy;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _NoiseTexture;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump float t16_6;
mediump float t16_15;
void main()
{
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_15 = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_15 = max(t16_15, 0.0);
    t16_1.x = t16_15 * _LightDir.w + t10_4;
    t16_15 = t16_15 * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_1.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_15) * t16_1.xyz + t16_0.xyz;
    t10_3.x = texture(_NoiseTexture, vs_TEXCOORD1.xy).y;
    t16_15 = t10_3.x * 0.9375 + 0.03125;
    t16_15 = (-t16_15) + _FadeColor.w;
    t16_15 = -abs(t16_15) * 32.0 + 1.0;
    t16_15 = max(t16_15, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_15);
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_1.x = float(ti3);
    t16_6 = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_6 + t16_1.x;
    t16_1.xyz = vec3(t16_15) * _RimColor.xyz;
    t16_0.xyz = _FadeColor.xyz * t16_0.xyz + t16_1.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 worldNormal_2;
  mediump vec3 worldViewDir_3;
  highp vec4 tmpvar_4;
  mediump vec2 tmpvar_5;
  mediump vec2 tmpvar_6;
  mediump vec3 tmpvar_7;
  mediump vec4 tmpvar_8;
  tmpvar_6.x = 0.0;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (tmpvar_4 * 0.5);
  highp vec2 tmpvar_11;
  tmpvar_11.x = tmpvar_10.x;
  tmpvar_11.y = (tmpvar_10.y * _ProjectionParams.x);
  o_9.xy = (tmpvar_11 + tmpvar_10.w);
  o_9.zw = tmpvar_4.zw;
  tmpvar_7 = o_9.xyw;
  tmpvar_5 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = (_Object2World * _glesVertex);
  tmpvar_12 = normalize((_WorldSpaceCameraPos - tmpvar_13.xyz));
  worldViewDir_3 = tmpvar_12;
  highp mat3 tmpvar_14;
  tmpvar_14[0] = _Object2World[0].xyz;
  tmpvar_14[1] = _Object2World[1].xyz;
  tmpvar_14[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_15;
  tmpvar_15 = normalize((tmpvar_14 * _glesNormal));
  worldNormal_2 = tmpvar_15;
  tmpvar_8 = (unity_World2Shadow[0] * tmpvar_13);
  highp float tmpvar_16;
  highp vec3 x_17;
  x_17 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_16 = sqrt(dot (x_17, x_17));
  fogpos_1 = tmpvar_16;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  gl_Position = tmpvar_4;
  xlv_TEXCOORD0 = worldNormal_2;
  xlv_TEXCOORD1 = tmpvar_5;
  xlv_TEXCOORD3 = worldViewDir_3;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
  xlv_TEXCOORD7 = tmpvar_8;
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
uniform lowp vec4 unity_FogColor;
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16.w = c_3.w;
  mediump float rTest_17;
  mediump vec2 tmpvar_18;
  tmpvar_18 = (xlv_TEXCOORD6.xy / xlv_TEXCOORD6.z);
  mediump float tmpvar_19;
  tmpvar_19 = ((1.5 * _FadeColor.w) - tmpvar_18.y);
  if ((tmpvar_19 < 0.0)) {
    discard;
  };
  mediump float tmpvar_20;
  tmpvar_20 = clamp ((1.0 - (tmpvar_19 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_21;
  tmpvar_21.x = tmpvar_18.x;
  tmpvar_21.y = clamp ((tmpvar_20 - tmpvar_18.y), 0.0, 1.0);
  lowp float tmpvar_22;
  tmpvar_22 = texture2D (_RasterTex, tmpvar_21).z;
  rTest_17 = tmpvar_22;
  mediump float x_23;
  x_23 = (rTest_17 - 0.4);
  if ((x_23 < 0.0)) {
    discard;
  };
  _color_16.xyz = (tmpvar_12 + (_FadeColor.xyz * tmpvar_20));
  lowp vec4 _color_24;
  _color_24.w = _color_16.w;
  _color_24.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_24.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_24.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "RASTER_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD3;
out mediump vec3 vs_TEXCOORD6;
out mediump vec4 vs_TEXCOORD7;
highp vec4 t0;
highp vec4 t1;
mediump float t16_2;
mediump float t16_5;
highp float t9;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t1.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * t1.xyz;
    vs_TEXCOORD0.xyz = t1.xyz;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = sqrt(t9);
    t16_2 = (-t9) + acid_Fog.y;
    t16_5 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_5;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_5 = t16_2 * acid_Fog.z;
    vs_TEXCOORD4.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_5;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t1.xy;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * t1.xyz;
    vs_TEXCOORD3.xyz = t1.xyz;
    t0.w = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t1.zz + t1.xw;
    vs_TEXCOORD6.xyz = t0.xyz;
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1 = t0.yyyy * unity_World2Shadow[0][1];
    t1 = unity_World2Shadow[0][0] * t0.xxxx + t1;
    t1 = unity_World2Shadow[0][2] * t0.zzzz + t1;
    t0 = unity_World2Shadow[0][3] * t0.wwww + t1;
    vs_TEXCOORD7 = t0;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _RasterTex;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
in mediump vec3 vs_TEXCOORD6;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp float t10_1;
bool tb1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump vec3 t16_7;
mediump float t16_15;
void main()
{
    t16_0.xy = vs_TEXCOORD6.xy / vs_TEXCOORD6.zz;
    t16_15 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_15<0.0;
    t16_15 = (-t16_15) * 2.5 + 1.0;
    t16_15 = clamp(t16_15, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_15;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1 = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1 + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_2.x = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_2.x = max(t16_2.x, 0.0);
    t16_7.x = t16_2.x * _LightDir.w + t10_4;
    t16_2.x = t16_2.x * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_7.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_7.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = t16_2.xxx * t16_7.xyz + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_15) + t16_0.xyz;
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_2.x = float(ti3);
    t16_7.x = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_7.x + t16_2.x;
    t16_0.xyz = vec3(t16_15) * _RimColor.xyz + t16_0.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesVertex;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  highp vec4 tmpvar_5;
  mediump vec2 tmpvar_6;
  highp vec3 tmpvar_7;
  mediump vec2 tmpvar_8;
  tmpvar_7.z = 0.0;
  tmpvar_8.x = 0.0;
  tmpvar_5 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_6 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_4 = tmpvar_9;
  highp mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize((tmpvar_10 * _glesNormal));
  worldNormal_3 = tmpvar_11;
  highp float tmpvar_12;
  highp vec3 x_13;
  x_13 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_12 = sqrt(dot (x_13, x_13));
  fogpos_2 = tmpvar_12;
  tmpvar_8.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_8.x = (1.0 - tmpvar_8.y);
  highp vec3 tmpvar_14;
  tmpvar_14 = abs(_glesNormal);
  tmpvar_7.xy = tmpvar_1.xy;
  if ((tmpvar_14.x > tmpvar_14.y)) {
    if ((tmpvar_14.x > tmpvar_14.z)) {
      tmpvar_7.xy = tmpvar_1.yz;
    };
  } else {
    if ((tmpvar_14.y > tmpvar_14.z)) {
      tmpvar_7.xy = tmpvar_1.xz;
    };
  };
  tmpvar_7.xy = (tmpvar_7 * 2.0).xy;
  tmpvar_7.z = (_Object2World * _glesVertex).y;
  highp float tmpvar_15;
  tmpvar_15 = (((tmpvar_7.z * 2.0) + _Time.z) / 6.283);
  highp float tmpvar_16;
  tmpvar_16 = (fract(abs(tmpvar_15)) * 6.283);
  highp float tmpvar_17;
  if ((tmpvar_15 >= 0.0)) {
    tmpvar_17 = tmpvar_16;
  } else {
    tmpvar_17 = -(tmpvar_16);
  };
  tmpvar_7.z = sin(tmpvar_17);
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = worldNormal_3;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = worldViewDir_4;
  xlv_TEXCOORD4 = tmpvar_8;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16 = (c_3 * _FadeColor);
  lowp vec4 _color_17;
  _color_17.w = _color_16.w;
  _color_17.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_17.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_17.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out highp vec3 vs_TEXCOORD2;
out mediump vec3 vs_TEXCOORD3;
highp vec4 t0;
mediump float t16_1;
bvec3 tb2;
mediump float t16_4;
highp float t9;
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
    vs_TEXCOORD4.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_4;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t0.xy;
    t0.w = in_POSITION0.x;
    tb2.xyz = lessThan(abs(in_NORMAL0.yzzy), abs(in_NORMAL0.xxyx)).xyz;
    t0.x = (tb2.y) ? in_POSITION0.y : in_POSITION0.x;
    t0.y = (tb2.y) ? in_POSITION0.z : in_POSITION0.y;
    t0.z = (tb2.z) ? in_POSITION0.z : in_POSITION0.y;
    t0.xy = (tb2.x) ? t0.xy : t0.wz;
    vs_TEXCOORD2.xy = t0.xy + t0.xy;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t9 = t0.y * 2.0 + _Time.z;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t9 = t9 * 0.159159631;
    tb2.x = t9>=(-t9);
    t9 = fract(abs(t9));
    t9 = (tb2.x) ? t9 : (-t9);
    t9 = t9 * 6.28299999;
    vs_TEXCOORD2.z = sin(t9);
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump float t16_6;
mediump float t16_15;
void main()
{
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_15 = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_15 = max(t16_15, 0.0);
    t16_1.x = t16_15 * _LightDir.w + t10_4;
    t16_15 = t16_15 * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_1.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_15) * t16_1.xyz + t16_0.xyz;
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_1.x = float(ti3);
    t16_6 = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_6 + t16_1.x;
    t16_1.xyz = vec3(t16_15) * _RimColor.xyz;
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz + t16_1.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesVertex;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  highp vec4 tmpvar_5;
  mediump vec2 tmpvar_6;
  highp vec3 tmpvar_7;
  mediump vec2 tmpvar_8;
  tmpvar_7.z = 0.0;
  tmpvar_8.x = 0.0;
  tmpvar_5 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_6 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_4 = tmpvar_9;
  highp mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize((tmpvar_10 * _glesNormal));
  worldNormal_3 = tmpvar_11;
  highp float tmpvar_12;
  highp vec3 x_13;
  x_13 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_12 = sqrt(dot (x_13, x_13));
  fogpos_2 = tmpvar_12;
  tmpvar_8.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_8.x = (1.0 - tmpvar_8.y);
  highp vec3 tmpvar_14;
  tmpvar_14 = abs(_glesNormal);
  tmpvar_7.xy = tmpvar_1.xy;
  if ((tmpvar_14.x > tmpvar_14.y)) {
    if ((tmpvar_14.x > tmpvar_14.z)) {
      tmpvar_7.xy = tmpvar_1.yz;
    };
  } else {
    if ((tmpvar_14.y > tmpvar_14.z)) {
      tmpvar_7.xy = tmpvar_1.xz;
    };
  };
  tmpvar_7.xy = (tmpvar_7 * 2.0).xy;
  tmpvar_7.z = (_Object2World * _glesVertex).y;
  highp float tmpvar_15;
  tmpvar_15 = (((tmpvar_7.z * 2.0) + _Time.z) / 6.283);
  highp float tmpvar_16;
  tmpvar_16 = (fract(abs(tmpvar_15)) * 6.283);
  highp float tmpvar_17;
  if ((tmpvar_15 >= 0.0)) {
    tmpvar_17 = tmpvar_16;
  } else {
    tmpvar_17 = -(tmpvar_16);
  };
  tmpvar_7.z = sin(tmpvar_17);
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = worldNormal_3;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = worldViewDir_4;
  xlv_TEXCOORD4 = tmpvar_8;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16.w = c_3.w;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_NoiseTexture, xlv_TEXCOORD1);
  mediump float tmpvar_18;
  tmpvar_18 = clamp ((1.0 - (32.0 * 
    abs((_FadeColor.w - ((0.9375 * tmpvar_17.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_16.xyz = (_FadeColor.xyz * (tmpvar_18 + tmpvar_12));
  lowp vec4 _color_19;
  _color_19.w = _color_16.w;
  _color_19.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_19.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_19.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out highp vec3 vs_TEXCOORD2;
out mediump vec3 vs_TEXCOORD3;
highp vec4 t0;
mediump float t16_1;
bvec3 tb2;
mediump float t16_4;
highp float t9;
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
    vs_TEXCOORD4.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_4;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t0.xy;
    t0.w = in_POSITION0.x;
    tb2.xyz = lessThan(abs(in_NORMAL0.yzzy), abs(in_NORMAL0.xxyx)).xyz;
    t0.x = (tb2.y) ? in_POSITION0.y : in_POSITION0.x;
    t0.y = (tb2.y) ? in_POSITION0.z : in_POSITION0.y;
    t0.z = (tb2.z) ? in_POSITION0.z : in_POSITION0.y;
    t0.xy = (tb2.x) ? t0.xy : t0.wz;
    vs_TEXCOORD2.xy = t0.xy + t0.xy;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t9 = t0.y * 2.0 + _Time.z;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t9 = t9 * 0.159159631;
    tb2.x = t9>=(-t9);
    t9 = fract(abs(t9));
    t9 = (tb2.x) ? t9 : (-t9);
    t9 = t9 * 6.28299999;
    vs_TEXCOORD2.z = sin(t9);
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _NoiseTexture;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump float t16_6;
mediump float t16_15;
void main()
{
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_15 = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_15 = max(t16_15, 0.0);
    t16_1.x = t16_15 * _LightDir.w + t10_4;
    t16_15 = t16_15 * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_1.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_15) * t16_1.xyz + t16_0.xyz;
    t10_3.x = texture(_NoiseTexture, vs_TEXCOORD1.xy).y;
    t16_15 = t10_3.x * 0.9375 + 0.03125;
    t16_15 = (-t16_15) + _FadeColor.w;
    t16_15 = -abs(t16_15) * 32.0 + 1.0;
    t16_15 = max(t16_15, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_15);
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_1.x = float(ti3);
    t16_6 = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_6 + t16_1.x;
    t16_1.xyz = vec3(t16_15) * _RimColor.xyz;
    t16_0.xyz = _FadeColor.xyz * t16_0.xyz + t16_1.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesVertex;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  highp vec4 tmpvar_5;
  mediump vec2 tmpvar_6;
  highp vec3 tmpvar_7;
  mediump vec2 tmpvar_8;
  mediump vec3 tmpvar_9;
  tmpvar_7.z = 0.0;
  tmpvar_8.x = 0.0;
  tmpvar_5 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_5 * 0.5);
  highp vec2 tmpvar_12;
  tmpvar_12.x = tmpvar_11.x;
  tmpvar_12.y = (tmpvar_11.y * _ProjectionParams.x);
  o_10.xy = (tmpvar_12 + tmpvar_11.w);
  o_10.zw = tmpvar_5.zw;
  tmpvar_9 = o_10.xyw;
  tmpvar_6 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_4 = tmpvar_13;
  highp mat3 tmpvar_14;
  tmpvar_14[0] = _Object2World[0].xyz;
  tmpvar_14[1] = _Object2World[1].xyz;
  tmpvar_14[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_15;
  tmpvar_15 = normalize((tmpvar_14 * _glesNormal));
  worldNormal_3 = tmpvar_15;
  highp float tmpvar_16;
  highp vec3 x_17;
  x_17 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_16 = sqrt(dot (x_17, x_17));
  fogpos_2 = tmpvar_16;
  tmpvar_8.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_8.x = (1.0 - tmpvar_8.y);
  highp vec3 tmpvar_18;
  tmpvar_18 = abs(_glesNormal);
  tmpvar_7.xy = tmpvar_1.xy;
  if ((tmpvar_18.x > tmpvar_18.y)) {
    if ((tmpvar_18.x > tmpvar_18.z)) {
      tmpvar_7.xy = tmpvar_1.yz;
    };
  } else {
    if ((tmpvar_18.y > tmpvar_18.z)) {
      tmpvar_7.xy = tmpvar_1.xz;
    };
  };
  tmpvar_7.xy = (tmpvar_7 * 2.0).xy;
  tmpvar_7.z = (_Object2World * _glesVertex).y;
  highp float tmpvar_19;
  tmpvar_19 = (((tmpvar_7.z * 2.0) + _Time.z) / 6.283);
  highp float tmpvar_20;
  tmpvar_20 = (fract(abs(tmpvar_19)) * 6.283);
  highp float tmpvar_21;
  if ((tmpvar_19 >= 0.0)) {
    tmpvar_21 = tmpvar_20;
  } else {
    tmpvar_21 = -(tmpvar_20);
  };
  tmpvar_7.z = sin(tmpvar_21);
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = worldNormal_3;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = worldViewDir_4;
  xlv_TEXCOORD4 = tmpvar_8;
  xlv_TEXCOORD6 = tmpvar_9;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16.w = c_3.w;
  mediump float rTest_17;
  mediump vec2 tmpvar_18;
  tmpvar_18 = (xlv_TEXCOORD6.xy / xlv_TEXCOORD6.z);
  mediump float tmpvar_19;
  tmpvar_19 = ((1.5 * _FadeColor.w) - tmpvar_18.y);
  if ((tmpvar_19 < 0.0)) {
    discard;
  };
  mediump float tmpvar_20;
  tmpvar_20 = clamp ((1.0 - (tmpvar_19 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_21;
  tmpvar_21.x = tmpvar_18.x;
  tmpvar_21.y = clamp ((tmpvar_20 - tmpvar_18.y), 0.0, 1.0);
  lowp float tmpvar_22;
  tmpvar_22 = texture2D (_RasterTex, tmpvar_21).z;
  rTest_17 = tmpvar_22;
  mediump float x_23;
  x_23 = (rTest_17 - 0.4);
  if ((x_23 < 0.0)) {
    discard;
  };
  _color_16.xyz = (tmpvar_12 + (_FadeColor.xyz * tmpvar_20));
  lowp vec4 _color_24;
  _color_24.w = _color_16.w;
  _color_24.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_24.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_24.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out highp vec3 vs_TEXCOORD2;
out mediump vec3 vs_TEXCOORD3;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_2;
bvec3 tb3;
mediump float t16_6;
highp float t12;
bool tb13;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t1.xyz;
    t12 = dot(t1.xyz, t1.xyz);
    t12 = inversesqrt(t12);
    t1.xyz = vec3(t12) * t1.xyz;
    vs_TEXCOORD0.xyz = t1.xyz;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t12 = dot(t1.xyz, t1.xyz);
    t12 = sqrt(t12);
    t16_2 = (-t12) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_6;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_6 = t16_2 * acid_Fog.z;
    vs_TEXCOORD4.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_6;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t1.xy;
    t1.w = in_POSITION0.x;
    tb3.xyz = lessThan(abs(in_NORMAL0.yzzy), abs(in_NORMAL0.xxyx)).xyz;
    t1.x = (tb3.y) ? in_POSITION0.y : in_POSITION0.x;
    t1.y = (tb3.y) ? in_POSITION0.z : in_POSITION0.y;
    t1.z = (tb3.z) ? in_POSITION0.z : in_POSITION0.y;
    t1.xy = (tb3.x) ? t1.xy : t1.wz;
    vs_TEXCOORD2.xy = t1.xy + t1.xy;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t12 = t1.y * 2.0 + _Time.z;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t12 = t12 * 0.159159631;
    tb13 = t12>=(-t12);
    t12 = fract(abs(t12));
    t12 = (tb13) ? t12 : (-t12);
    t12 = t12 * 6.28299999;
    vs_TEXCOORD2.z = sin(t12);
    t12 = dot(t1.xyz, t1.xyz);
    t12 = inversesqrt(t12);
    t1.xyz = vec3(t12) * t1.xyz;
    vs_TEXCOORD3.xyz = t1.xyz;
    t0.w = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t1.zz + t1.xw;
    vs_TEXCOORD6.xyz = t0.xyz;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _RasterTex;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
in mediump vec3 vs_TEXCOORD6;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp float t10_1;
bool tb1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump vec3 t16_7;
mediump float t16_15;
void main()
{
    t16_0.xy = vs_TEXCOORD6.xy / vs_TEXCOORD6.zz;
    t16_15 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_15<0.0;
    t16_15 = (-t16_15) * 2.5 + 1.0;
    t16_15 = clamp(t16_15, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_15;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1 = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1 + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_2.x = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_2.x = max(t16_2.x, 0.0);
    t16_7.x = t16_2.x * _LightDir.w + t10_4;
    t16_2.x = t16_2.x * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_7.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_7.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = t16_2.xxx * t16_7.xyz + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_15) + t16_0.xyz;
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_2.x = float(ti3);
    t16_7.x = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_7.x + t16_2.x;
    t16_0.xyz = vec3(t16_15) * _RimColor.xyz + t16_0.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesVertex;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  highp vec4 tmpvar_5;
  mediump vec2 tmpvar_6;
  highp vec3 tmpvar_7;
  mediump vec2 tmpvar_8;
  tmpvar_7.z = 0.0;
  tmpvar_8.x = 0.0;
  tmpvar_5 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_6 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_4 = tmpvar_9;
  highp mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize((tmpvar_10 * _glesNormal));
  worldNormal_3 = tmpvar_11;
  highp float tmpvar_12;
  highp vec3 x_13;
  x_13 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_12 = sqrt(dot (x_13, x_13));
  fogpos_2 = tmpvar_12;
  tmpvar_8.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_8.x = (1.0 - tmpvar_8.y);
  highp vec3 tmpvar_14;
  tmpvar_14 = abs(_glesNormal);
  tmpvar_7.xy = tmpvar_1.xy;
  if ((tmpvar_14.x > tmpvar_14.y)) {
    if ((tmpvar_14.x > tmpvar_14.z)) {
      tmpvar_7.xy = tmpvar_1.yz;
    };
  } else {
    if ((tmpvar_14.y > tmpvar_14.z)) {
      tmpvar_7.xy = tmpvar_1.xz;
    };
  };
  tmpvar_7.xy = (tmpvar_7 * 2.0).xy;
  tmpvar_7.z = (_Object2World * _glesVertex).y;
  highp float tmpvar_15;
  tmpvar_15 = (((tmpvar_7.z * 2.0) + _Time.z) / 6.283);
  highp float tmpvar_16;
  tmpvar_16 = (fract(abs(tmpvar_15)) * 6.283);
  highp float tmpvar_17;
  if ((tmpvar_15 >= 0.0)) {
    tmpvar_17 = tmpvar_16;
  } else {
    tmpvar_17 = -(tmpvar_16);
  };
  tmpvar_7.z = sin(tmpvar_17);
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = worldNormal_3;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = worldViewDir_4;
  xlv_TEXCOORD4 = tmpvar_8;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16 = (c_3 * _FadeColor);
  lowp vec4 _color_17;
  _color_17.w = _color_16.w;
  _color_17.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_17.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_17.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out highp vec3 vs_TEXCOORD2;
out mediump vec3 vs_TEXCOORD3;
highp vec4 t0;
mediump float t16_1;
bvec3 tb2;
mediump float t16_4;
highp float t9;
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
    vs_TEXCOORD4.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_4;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t0.xy;
    t0.w = in_POSITION0.x;
    tb2.xyz = lessThan(abs(in_NORMAL0.yzzy), abs(in_NORMAL0.xxyx)).xyz;
    t0.x = (tb2.y) ? in_POSITION0.y : in_POSITION0.x;
    t0.y = (tb2.y) ? in_POSITION0.z : in_POSITION0.y;
    t0.z = (tb2.z) ? in_POSITION0.z : in_POSITION0.y;
    t0.xy = (tb2.x) ? t0.xy : t0.wz;
    vs_TEXCOORD2.xy = t0.xy + t0.xy;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t9 = t0.y * 2.0 + _Time.z;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t9 = t9 * 0.159159631;
    tb2.x = t9>=(-t9);
    t9 = fract(abs(t9));
    t9 = (tb2.x) ? t9 : (-t9);
    t9 = t9 * 6.28299999;
    vs_TEXCOORD2.z = sin(t9);
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump float t16_6;
mediump float t16_15;
void main()
{
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_15 = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_15 = max(t16_15, 0.0);
    t16_1.x = t16_15 * _LightDir.w + t10_4;
    t16_15 = t16_15 * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_1.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_15) * t16_1.xyz + t16_0.xyz;
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_1.x = float(ti3);
    t16_6 = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_6 + t16_1.x;
    t16_1.xyz = vec3(t16_15) * _RimColor.xyz;
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz + t16_1.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesVertex;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  highp vec4 tmpvar_5;
  mediump vec2 tmpvar_6;
  highp vec3 tmpvar_7;
  mediump vec2 tmpvar_8;
  tmpvar_7.z = 0.0;
  tmpvar_8.x = 0.0;
  tmpvar_5 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_6 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_4 = tmpvar_9;
  highp mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize((tmpvar_10 * _glesNormal));
  worldNormal_3 = tmpvar_11;
  highp float tmpvar_12;
  highp vec3 x_13;
  x_13 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_12 = sqrt(dot (x_13, x_13));
  fogpos_2 = tmpvar_12;
  tmpvar_8.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_8.x = (1.0 - tmpvar_8.y);
  highp vec3 tmpvar_14;
  tmpvar_14 = abs(_glesNormal);
  tmpvar_7.xy = tmpvar_1.xy;
  if ((tmpvar_14.x > tmpvar_14.y)) {
    if ((tmpvar_14.x > tmpvar_14.z)) {
      tmpvar_7.xy = tmpvar_1.yz;
    };
  } else {
    if ((tmpvar_14.y > tmpvar_14.z)) {
      tmpvar_7.xy = tmpvar_1.xz;
    };
  };
  tmpvar_7.xy = (tmpvar_7 * 2.0).xy;
  tmpvar_7.z = (_Object2World * _glesVertex).y;
  highp float tmpvar_15;
  tmpvar_15 = (((tmpvar_7.z * 2.0) + _Time.z) / 6.283);
  highp float tmpvar_16;
  tmpvar_16 = (fract(abs(tmpvar_15)) * 6.283);
  highp float tmpvar_17;
  if ((tmpvar_15 >= 0.0)) {
    tmpvar_17 = tmpvar_16;
  } else {
    tmpvar_17 = -(tmpvar_16);
  };
  tmpvar_7.z = sin(tmpvar_17);
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = worldNormal_3;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = worldViewDir_4;
  xlv_TEXCOORD4 = tmpvar_8;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16.w = c_3.w;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_NoiseTexture, xlv_TEXCOORD1);
  mediump float tmpvar_18;
  tmpvar_18 = clamp ((1.0 - (32.0 * 
    abs((_FadeColor.w - ((0.9375 * tmpvar_17.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_16.xyz = (_FadeColor.xyz * (tmpvar_18 + tmpvar_12));
  lowp vec4 _color_19;
  _color_19.w = _color_16.w;
  _color_19.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_19.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_19.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out highp vec3 vs_TEXCOORD2;
out mediump vec3 vs_TEXCOORD3;
highp vec4 t0;
mediump float t16_1;
bvec3 tb2;
mediump float t16_4;
highp float t9;
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
    vs_TEXCOORD4.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_4;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t0.xy;
    t0.w = in_POSITION0.x;
    tb2.xyz = lessThan(abs(in_NORMAL0.yzzy), abs(in_NORMAL0.xxyx)).xyz;
    t0.x = (tb2.y) ? in_POSITION0.y : in_POSITION0.x;
    t0.y = (tb2.y) ? in_POSITION0.z : in_POSITION0.y;
    t0.z = (tb2.z) ? in_POSITION0.z : in_POSITION0.y;
    t0.xy = (tb2.x) ? t0.xy : t0.wz;
    vs_TEXCOORD2.xy = t0.xy + t0.xy;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t9 = t0.y * 2.0 + _Time.z;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t9 = t9 * 0.159159631;
    tb2.x = t9>=(-t9);
    t9 = fract(abs(t9));
    t9 = (tb2.x) ? t9 : (-t9);
    t9 = t9 * 6.28299999;
    vs_TEXCOORD2.z = sin(t9);
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _NoiseTexture;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump float t16_6;
mediump float t16_15;
void main()
{
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_15 = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_15 = max(t16_15, 0.0);
    t16_1.x = t16_15 * _LightDir.w + t10_4;
    t16_15 = t16_15 * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_1.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_15) * t16_1.xyz + t16_0.xyz;
    t10_3.x = texture(_NoiseTexture, vs_TEXCOORD1.xy).y;
    t16_15 = t10_3.x * 0.9375 + 0.03125;
    t16_15 = (-t16_15) + _FadeColor.w;
    t16_15 = -abs(t16_15) * 32.0 + 1.0;
    t16_15 = max(t16_15, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_15);
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_1.x = float(ti3);
    t16_6 = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_6 + t16_1.x;
    t16_1.xyz = vec3(t16_15) * _RimColor.xyz;
    t16_0.xyz = _FadeColor.xyz * t16_0.xyz + t16_1.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesVertex;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  highp vec4 tmpvar_5;
  mediump vec2 tmpvar_6;
  highp vec3 tmpvar_7;
  mediump vec2 tmpvar_8;
  mediump vec3 tmpvar_9;
  tmpvar_7.z = 0.0;
  tmpvar_8.x = 0.0;
  tmpvar_5 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (tmpvar_5 * 0.5);
  highp vec2 tmpvar_12;
  tmpvar_12.x = tmpvar_11.x;
  tmpvar_12.y = (tmpvar_11.y * _ProjectionParams.x);
  o_10.xy = (tmpvar_12 + tmpvar_11.w);
  o_10.zw = tmpvar_5.zw;
  tmpvar_9 = o_10.xyw;
  tmpvar_6 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_4 = tmpvar_13;
  highp mat3 tmpvar_14;
  tmpvar_14[0] = _Object2World[0].xyz;
  tmpvar_14[1] = _Object2World[1].xyz;
  tmpvar_14[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_15;
  tmpvar_15 = normalize((tmpvar_14 * _glesNormal));
  worldNormal_3 = tmpvar_15;
  highp float tmpvar_16;
  highp vec3 x_17;
  x_17 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_16 = sqrt(dot (x_17, x_17));
  fogpos_2 = tmpvar_16;
  tmpvar_8.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_8.x = (1.0 - tmpvar_8.y);
  highp vec3 tmpvar_18;
  tmpvar_18 = abs(_glesNormal);
  tmpvar_7.xy = tmpvar_1.xy;
  if ((tmpvar_18.x > tmpvar_18.y)) {
    if ((tmpvar_18.x > tmpvar_18.z)) {
      tmpvar_7.xy = tmpvar_1.yz;
    };
  } else {
    if ((tmpvar_18.y > tmpvar_18.z)) {
      tmpvar_7.xy = tmpvar_1.xz;
    };
  };
  tmpvar_7.xy = (tmpvar_7 * 2.0).xy;
  tmpvar_7.z = (_Object2World * _glesVertex).y;
  highp float tmpvar_19;
  tmpvar_19 = (((tmpvar_7.z * 2.0) + _Time.z) / 6.283);
  highp float tmpvar_20;
  tmpvar_20 = (fract(abs(tmpvar_19)) * 6.283);
  highp float tmpvar_21;
  if ((tmpvar_19 >= 0.0)) {
    tmpvar_21 = tmpvar_20;
  } else {
    tmpvar_21 = -(tmpvar_20);
  };
  tmpvar_7.z = sin(tmpvar_21);
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = worldNormal_3;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = worldViewDir_4;
  xlv_TEXCOORD4 = tmpvar_8;
  xlv_TEXCOORD6 = tmpvar_9;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16.w = c_3.w;
  mediump float rTest_17;
  mediump vec2 tmpvar_18;
  tmpvar_18 = (xlv_TEXCOORD6.xy / xlv_TEXCOORD6.z);
  mediump float tmpvar_19;
  tmpvar_19 = ((1.5 * _FadeColor.w) - tmpvar_18.y);
  if ((tmpvar_19 < 0.0)) {
    discard;
  };
  mediump float tmpvar_20;
  tmpvar_20 = clamp ((1.0 - (tmpvar_19 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_21;
  tmpvar_21.x = tmpvar_18.x;
  tmpvar_21.y = clamp ((tmpvar_20 - tmpvar_18.y), 0.0, 1.0);
  lowp float tmpvar_22;
  tmpvar_22 = texture2D (_RasterTex, tmpvar_21).z;
  rTest_17 = tmpvar_22;
  mediump float x_23;
  x_23 = (rTest_17 - 0.4);
  if ((x_23 < 0.0)) {
    discard;
  };
  _color_16.xyz = (tmpvar_12 + (_FadeColor.xyz * tmpvar_20));
  lowp vec4 _color_24;
  _color_24.w = _color_16.w;
  _color_24.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_24.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_24.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out highp vec3 vs_TEXCOORD2;
out mediump vec3 vs_TEXCOORD3;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_2;
bvec3 tb3;
mediump float t16_6;
highp float t12;
bool tb13;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t1.xyz;
    t12 = dot(t1.xyz, t1.xyz);
    t12 = inversesqrt(t12);
    t1.xyz = vec3(t12) * t1.xyz;
    vs_TEXCOORD0.xyz = t1.xyz;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t12 = dot(t1.xyz, t1.xyz);
    t12 = sqrt(t12);
    t16_2 = (-t12) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_6;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_6 = t16_2 * acid_Fog.z;
    vs_TEXCOORD4.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_6;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t1.xy;
    t1.w = in_POSITION0.x;
    tb3.xyz = lessThan(abs(in_NORMAL0.yzzy), abs(in_NORMAL0.xxyx)).xyz;
    t1.x = (tb3.y) ? in_POSITION0.y : in_POSITION0.x;
    t1.y = (tb3.y) ? in_POSITION0.z : in_POSITION0.y;
    t1.z = (tb3.z) ? in_POSITION0.z : in_POSITION0.y;
    t1.xy = (tb3.x) ? t1.xy : t1.wz;
    vs_TEXCOORD2.xy = t1.xy + t1.xy;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t12 = t1.y * 2.0 + _Time.z;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t12 = t12 * 0.159159631;
    tb13 = t12>=(-t12);
    t12 = fract(abs(t12));
    t12 = (tb13) ? t12 : (-t12);
    t12 = t12 * 6.28299999;
    vs_TEXCOORD2.z = sin(t12);
    t12 = dot(t1.xyz, t1.xyz);
    t12 = inversesqrt(t12);
    t1.xyz = vec3(t12) * t1.xyz;
    vs_TEXCOORD3.xyz = t1.xyz;
    t0.w = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t1.zz + t1.xw;
    vs_TEXCOORD6.xyz = t0.xyz;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _RasterTex;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
in mediump vec3 vs_TEXCOORD6;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp float t10_1;
bool tb1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump vec3 t16_7;
mediump float t16_15;
void main()
{
    t16_0.xy = vs_TEXCOORD6.xy / vs_TEXCOORD6.zz;
    t16_15 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_15<0.0;
    t16_15 = (-t16_15) * 2.5 + 1.0;
    t16_15 = clamp(t16_15, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_15;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1 = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1 + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_2.x = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_2.x = max(t16_2.x, 0.0);
    t16_7.x = t16_2.x * _LightDir.w + t10_4;
    t16_2.x = t16_2.x * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_7.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_7.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = t16_2.xxx * t16_7.xyz + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_15) + t16_0.xyz;
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_2.x = float(ti3);
    t16_7.x = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_7.x + t16_2.x;
    t16_0.xyz = vec3(t16_15) * _RimColor.xyz + t16_0.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesVertex;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  highp vec4 tmpvar_5;
  mediump vec2 tmpvar_6;
  highp vec3 tmpvar_7;
  mediump vec2 tmpvar_8;
  mediump vec4 tmpvar_9;
  tmpvar_7.z = 0.0;
  tmpvar_8.x = 0.0;
  tmpvar_5 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_6 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Object2World * _glesVertex);
  tmpvar_10 = normalize((_WorldSpaceCameraPos - tmpvar_11.xyz));
  worldViewDir_4 = tmpvar_10;
  highp mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize((tmpvar_12 * _glesNormal));
  worldNormal_3 = tmpvar_13;
  tmpvar_9 = (unity_World2Shadow[0] * tmpvar_11);
  highp float tmpvar_14;
  highp vec3 x_15;
  x_15 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_14 = sqrt(dot (x_15, x_15));
  fogpos_2 = tmpvar_14;
  tmpvar_8.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_8.x = (1.0 - tmpvar_8.y);
  highp vec3 tmpvar_16;
  tmpvar_16 = abs(_glesNormal);
  tmpvar_7.xy = tmpvar_1.xy;
  if ((tmpvar_16.x > tmpvar_16.y)) {
    if ((tmpvar_16.x > tmpvar_16.z)) {
      tmpvar_7.xy = tmpvar_1.yz;
    };
  } else {
    if ((tmpvar_16.y > tmpvar_16.z)) {
      tmpvar_7.xy = tmpvar_1.xz;
    };
  };
  tmpvar_7.xy = (tmpvar_7 * 2.0).xy;
  tmpvar_7.z = (_Object2World * _glesVertex).y;
  highp float tmpvar_17;
  tmpvar_17 = (((tmpvar_7.z * 2.0) + _Time.z) / 6.283);
  highp float tmpvar_18;
  tmpvar_18 = (fract(abs(tmpvar_17)) * 6.283);
  highp float tmpvar_19;
  if ((tmpvar_17 >= 0.0)) {
    tmpvar_19 = tmpvar_18;
  } else {
    tmpvar_19 = -(tmpvar_18);
  };
  tmpvar_7.z = sin(tmpvar_19);
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = worldNormal_3;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = worldViewDir_4;
  xlv_TEXCOORD4 = tmpvar_8;
  xlv_TEXCOORD8 = tmpvar_9;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16 = (c_3 * _FadeColor);
  lowp vec4 _color_17;
  _color_17.w = _color_16.w;
  _color_17.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_17.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_17.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesVertex;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  highp vec4 tmpvar_5;
  mediump vec2 tmpvar_6;
  highp vec3 tmpvar_7;
  mediump vec2 tmpvar_8;
  mediump vec4 tmpvar_9;
  tmpvar_7.z = 0.0;
  tmpvar_8.x = 0.0;
  tmpvar_5 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_6 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Object2World * _glesVertex);
  tmpvar_10 = normalize((_WorldSpaceCameraPos - tmpvar_11.xyz));
  worldViewDir_4 = tmpvar_10;
  highp mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize((tmpvar_12 * _glesNormal));
  worldNormal_3 = tmpvar_13;
  tmpvar_9 = (unity_World2Shadow[0] * tmpvar_11);
  highp float tmpvar_14;
  highp vec3 x_15;
  x_15 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_14 = sqrt(dot (x_15, x_15));
  fogpos_2 = tmpvar_14;
  tmpvar_8.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_8.x = (1.0 - tmpvar_8.y);
  highp vec3 tmpvar_16;
  tmpvar_16 = abs(_glesNormal);
  tmpvar_7.xy = tmpvar_1.xy;
  if ((tmpvar_16.x > tmpvar_16.y)) {
    if ((tmpvar_16.x > tmpvar_16.z)) {
      tmpvar_7.xy = tmpvar_1.yz;
    };
  } else {
    if ((tmpvar_16.y > tmpvar_16.z)) {
      tmpvar_7.xy = tmpvar_1.xz;
    };
  };
  tmpvar_7.xy = (tmpvar_7 * 2.0).xy;
  tmpvar_7.z = (_Object2World * _glesVertex).y;
  highp float tmpvar_17;
  tmpvar_17 = (((tmpvar_7.z * 2.0) + _Time.z) / 6.283);
  highp float tmpvar_18;
  tmpvar_18 = (fract(abs(tmpvar_17)) * 6.283);
  highp float tmpvar_19;
  if ((tmpvar_17 >= 0.0)) {
    tmpvar_19 = tmpvar_18;
  } else {
    tmpvar_19 = -(tmpvar_18);
  };
  tmpvar_7.z = sin(tmpvar_19);
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = worldNormal_3;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = worldViewDir_4;
  xlv_TEXCOORD4 = tmpvar_8;
  xlv_TEXCOORD8 = tmpvar_9;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16.w = c_3.w;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_NoiseTexture, xlv_TEXCOORD1);
  mediump float tmpvar_18;
  tmpvar_18 = clamp ((1.0 - (32.0 * 
    abs((_FadeColor.w - ((0.9375 * tmpvar_17.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_16.xyz = (_FadeColor.xyz * (tmpvar_18 + tmpvar_12));
  lowp vec4 _color_19;
  _color_19.w = _color_16.w;
  _color_19.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_19.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_19.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesVertex;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  highp vec4 tmpvar_5;
  mediump vec2 tmpvar_6;
  highp vec3 tmpvar_7;
  mediump vec2 tmpvar_8;
  mediump vec3 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_7.z = 0.0;
  tmpvar_8.x = 0.0;
  tmpvar_5 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (tmpvar_5 * 0.5);
  highp vec2 tmpvar_13;
  tmpvar_13.x = tmpvar_12.x;
  tmpvar_13.y = (tmpvar_12.y * _ProjectionParams.x);
  o_11.xy = (tmpvar_13 + tmpvar_12.w);
  o_11.zw = tmpvar_5.zw;
  tmpvar_9 = o_11.xyw;
  tmpvar_6 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Object2World * _glesVertex);
  tmpvar_14 = normalize((_WorldSpaceCameraPos - tmpvar_15.xyz));
  worldViewDir_4 = tmpvar_14;
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _Object2World[0].xyz;
  tmpvar_16[1] = _Object2World[1].xyz;
  tmpvar_16[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_16 * _glesNormal));
  worldNormal_3 = tmpvar_17;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_15);
  highp float tmpvar_18;
  highp vec3 x_19;
  x_19 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_18 = sqrt(dot (x_19, x_19));
  fogpos_2 = tmpvar_18;
  tmpvar_8.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_8.x = (1.0 - tmpvar_8.y);
  highp vec3 tmpvar_20;
  tmpvar_20 = abs(_glesNormal);
  tmpvar_7.xy = tmpvar_1.xy;
  if ((tmpvar_20.x > tmpvar_20.y)) {
    if ((tmpvar_20.x > tmpvar_20.z)) {
      tmpvar_7.xy = tmpvar_1.yz;
    };
  } else {
    if ((tmpvar_20.y > tmpvar_20.z)) {
      tmpvar_7.xy = tmpvar_1.xz;
    };
  };
  tmpvar_7.xy = (tmpvar_7 * 2.0).xy;
  tmpvar_7.z = (_Object2World * _glesVertex).y;
  highp float tmpvar_21;
  tmpvar_21 = (((tmpvar_7.z * 2.0) + _Time.z) / 6.283);
  highp float tmpvar_22;
  tmpvar_22 = (fract(abs(tmpvar_21)) * 6.283);
  highp float tmpvar_23;
  if ((tmpvar_21 >= 0.0)) {
    tmpvar_23 = tmpvar_22;
  } else {
    tmpvar_23 = -(tmpvar_22);
  };
  tmpvar_7.z = sin(tmpvar_23);
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = worldNormal_3;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = worldViewDir_4;
  xlv_TEXCOORD4 = tmpvar_8;
  xlv_TEXCOORD6 = tmpvar_9;
  xlv_TEXCOORD7 = tmpvar_10;
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
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16.w = c_3.w;
  mediump float rTest_17;
  mediump vec2 tmpvar_18;
  tmpvar_18 = (xlv_TEXCOORD6.xy / xlv_TEXCOORD6.z);
  mediump float tmpvar_19;
  tmpvar_19 = ((1.5 * _FadeColor.w) - tmpvar_18.y);
  if ((tmpvar_19 < 0.0)) {
    discard;
  };
  mediump float tmpvar_20;
  tmpvar_20 = clamp ((1.0 - (tmpvar_19 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_21;
  tmpvar_21.x = tmpvar_18.x;
  tmpvar_21.y = clamp ((tmpvar_20 - tmpvar_18.y), 0.0, 1.0);
  lowp float tmpvar_22;
  tmpvar_22 = texture2D (_RasterTex, tmpvar_21).z;
  rTest_17 = tmpvar_22;
  mediump float x_23;
  x_23 = (rTest_17 - 0.4);
  if ((x_23 < 0.0)) {
    discard;
  };
  _color_16.xyz = (tmpvar_12 + (_FadeColor.xyz * tmpvar_20));
  lowp vec4 _color_24;
  _color_24.w = _color_16.w;
  _color_24.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_24.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_24.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesVertex;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  highp vec4 tmpvar_5;
  mediump vec2 tmpvar_6;
  highp vec3 tmpvar_7;
  mediump vec2 tmpvar_8;
  mediump vec4 tmpvar_9;
  tmpvar_7.z = 0.0;
  tmpvar_8.x = 0.0;
  tmpvar_5 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_6 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Object2World * _glesVertex);
  tmpvar_10 = normalize((_WorldSpaceCameraPos - tmpvar_11.xyz));
  worldViewDir_4 = tmpvar_10;
  highp mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize((tmpvar_12 * _glesNormal));
  worldNormal_3 = tmpvar_13;
  tmpvar_9 = (unity_World2Shadow[0] * tmpvar_11);
  highp float tmpvar_14;
  highp vec3 x_15;
  x_15 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_14 = sqrt(dot (x_15, x_15));
  fogpos_2 = tmpvar_14;
  tmpvar_8.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_8.x = (1.0 - tmpvar_8.y);
  highp vec3 tmpvar_16;
  tmpvar_16 = abs(_glesNormal);
  tmpvar_7.xy = tmpvar_1.xy;
  if ((tmpvar_16.x > tmpvar_16.y)) {
    if ((tmpvar_16.x > tmpvar_16.z)) {
      tmpvar_7.xy = tmpvar_1.yz;
    };
  } else {
    if ((tmpvar_16.y > tmpvar_16.z)) {
      tmpvar_7.xy = tmpvar_1.xz;
    };
  };
  tmpvar_7.xy = (tmpvar_7 * 2.0).xy;
  tmpvar_7.z = (_Object2World * _glesVertex).y;
  highp float tmpvar_17;
  tmpvar_17 = (((tmpvar_7.z * 2.0) + _Time.z) / 6.283);
  highp float tmpvar_18;
  tmpvar_18 = (fract(abs(tmpvar_17)) * 6.283);
  highp float tmpvar_19;
  if ((tmpvar_17 >= 0.0)) {
    tmpvar_19 = tmpvar_18;
  } else {
    tmpvar_19 = -(tmpvar_18);
  };
  tmpvar_7.z = sin(tmpvar_19);
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = worldNormal_3;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = worldViewDir_4;
  xlv_TEXCOORD4 = tmpvar_8;
  xlv_TEXCOORD8 = tmpvar_9;
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
uniform lowp vec4 unity_FogColor;
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16 = (c_3 * _FadeColor);
  lowp vec4 _color_17;
  _color_17.w = _color_16.w;
  _color_17.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_17.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_17.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out highp vec3 vs_TEXCOORD2;
out mediump vec3 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
bvec3 tb2;
mediump float t16_4;
highp float t9;
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
    vs_TEXCOORD4.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_4;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t0.xy;
    t0.w = in_POSITION0.x;
    tb2.xyz = lessThan(abs(in_NORMAL0.yzzy), abs(in_NORMAL0.xxyx)).xyz;
    t0.x = (tb2.y) ? in_POSITION0.y : in_POSITION0.x;
    t0.y = (tb2.y) ? in_POSITION0.z : in_POSITION0.y;
    t0.z = (tb2.z) ? in_POSITION0.z : in_POSITION0.y;
    t0.xy = (tb2.x) ? t0.xy : t0.wz;
    vs_TEXCOORD2.xy = t0.xy + t0.xy;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t9 = _Object2World[3].y * in_POSITION0.w + t0.y;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t9 = t9 * 2.0 + _Time.z;
    t9 = t9 * 0.159159631;
    tb2.x = t9>=(-t9);
    t9 = fract(abs(t9));
    t9 = (tb2.x) ? t9 : (-t9);
    t9 = t9 * 6.28299999;
    vs_TEXCOORD2.z = sin(t9);
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump float t16_6;
mediump float t16_15;
void main()
{
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_15 = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_15 = max(t16_15, 0.0);
    t16_1.x = t16_15 * _LightDir.w + t10_4;
    t16_15 = t16_15 * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_1.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_15) * t16_1.xyz + t16_0.xyz;
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_1.x = float(ti3);
    t16_6 = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_6 + t16_1.x;
    t16_1.xyz = vec3(t16_15) * _RimColor.xyz;
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz + t16_1.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesVertex;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  highp vec4 tmpvar_5;
  mediump vec2 tmpvar_6;
  highp vec3 tmpvar_7;
  mediump vec2 tmpvar_8;
  mediump vec4 tmpvar_9;
  tmpvar_7.z = 0.0;
  tmpvar_8.x = 0.0;
  tmpvar_5 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_6 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Object2World * _glesVertex);
  tmpvar_10 = normalize((_WorldSpaceCameraPos - tmpvar_11.xyz));
  worldViewDir_4 = tmpvar_10;
  highp mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize((tmpvar_12 * _glesNormal));
  worldNormal_3 = tmpvar_13;
  tmpvar_9 = (unity_World2Shadow[0] * tmpvar_11);
  highp float tmpvar_14;
  highp vec3 x_15;
  x_15 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_14 = sqrt(dot (x_15, x_15));
  fogpos_2 = tmpvar_14;
  tmpvar_8.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_8.x = (1.0 - tmpvar_8.y);
  highp vec3 tmpvar_16;
  tmpvar_16 = abs(_glesNormal);
  tmpvar_7.xy = tmpvar_1.xy;
  if ((tmpvar_16.x > tmpvar_16.y)) {
    if ((tmpvar_16.x > tmpvar_16.z)) {
      tmpvar_7.xy = tmpvar_1.yz;
    };
  } else {
    if ((tmpvar_16.y > tmpvar_16.z)) {
      tmpvar_7.xy = tmpvar_1.xz;
    };
  };
  tmpvar_7.xy = (tmpvar_7 * 2.0).xy;
  tmpvar_7.z = (_Object2World * _glesVertex).y;
  highp float tmpvar_17;
  tmpvar_17 = (((tmpvar_7.z * 2.0) + _Time.z) / 6.283);
  highp float tmpvar_18;
  tmpvar_18 = (fract(abs(tmpvar_17)) * 6.283);
  highp float tmpvar_19;
  if ((tmpvar_17 >= 0.0)) {
    tmpvar_19 = tmpvar_18;
  } else {
    tmpvar_19 = -(tmpvar_18);
  };
  tmpvar_7.z = sin(tmpvar_19);
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = worldNormal_3;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = worldViewDir_4;
  xlv_TEXCOORD4 = tmpvar_8;
  xlv_TEXCOORD8 = tmpvar_9;
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
uniform lowp vec4 unity_FogColor;
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16.w = c_3.w;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_NoiseTexture, xlv_TEXCOORD1);
  mediump float tmpvar_18;
  tmpvar_18 = clamp ((1.0 - (32.0 * 
    abs((_FadeColor.w - ((0.9375 * tmpvar_17.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_16.xyz = (_FadeColor.xyz * (tmpvar_18 + tmpvar_12));
  lowp vec4 _color_19;
  _color_19.w = _color_16.w;
  _color_19.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_19.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_19.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out highp vec3 vs_TEXCOORD2;
out mediump vec3 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
bvec3 tb2;
mediump float t16_4;
highp float t9;
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
    vs_TEXCOORD4.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_4;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t0.xy;
    t0.w = in_POSITION0.x;
    tb2.xyz = lessThan(abs(in_NORMAL0.yzzy), abs(in_NORMAL0.xxyx)).xyz;
    t0.x = (tb2.y) ? in_POSITION0.y : in_POSITION0.x;
    t0.y = (tb2.y) ? in_POSITION0.z : in_POSITION0.y;
    t0.z = (tb2.z) ? in_POSITION0.z : in_POSITION0.y;
    t0.xy = (tb2.x) ? t0.xy : t0.wz;
    vs_TEXCOORD2.xy = t0.xy + t0.xy;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t9 = _Object2World[3].y * in_POSITION0.w + t0.y;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t9 = t9 * 2.0 + _Time.z;
    t9 = t9 * 0.159159631;
    tb2.x = t9>=(-t9);
    t9 = fract(abs(t9));
    t9 = (tb2.x) ? t9 : (-t9);
    t9 = t9 * 6.28299999;
    vs_TEXCOORD2.z = sin(t9);
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _NoiseTexture;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump float t16_6;
mediump float t16_15;
void main()
{
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_15 = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_15 = max(t16_15, 0.0);
    t16_1.x = t16_15 * _LightDir.w + t10_4;
    t16_15 = t16_15 * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_1.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_15) * t16_1.xyz + t16_0.xyz;
    t10_3.x = texture(_NoiseTexture, vs_TEXCOORD1.xy).y;
    t16_15 = t10_3.x * 0.9375 + 0.03125;
    t16_15 = (-t16_15) + _FadeColor.w;
    t16_15 = -abs(t16_15) * 32.0 + 1.0;
    t16_15 = max(t16_15, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_15);
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_1.x = float(ti3);
    t16_6 = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_6 + t16_1.x;
    t16_1.xyz = vec3(t16_15) * _RimColor.xyz;
    t16_0.xyz = _FadeColor.xyz * t16_0.xyz + t16_1.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesVertex;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  highp vec4 tmpvar_5;
  mediump vec2 tmpvar_6;
  highp vec3 tmpvar_7;
  mediump vec2 tmpvar_8;
  mediump vec3 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_7.z = 0.0;
  tmpvar_8.x = 0.0;
  tmpvar_5 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (tmpvar_5 * 0.5);
  highp vec2 tmpvar_13;
  tmpvar_13.x = tmpvar_12.x;
  tmpvar_13.y = (tmpvar_12.y * _ProjectionParams.x);
  o_11.xy = (tmpvar_13 + tmpvar_12.w);
  o_11.zw = tmpvar_5.zw;
  tmpvar_9 = o_11.xyw;
  tmpvar_6 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  highp vec3 tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Object2World * _glesVertex);
  tmpvar_14 = normalize((_WorldSpaceCameraPos - tmpvar_15.xyz));
  worldViewDir_4 = tmpvar_14;
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _Object2World[0].xyz;
  tmpvar_16[1] = _Object2World[1].xyz;
  tmpvar_16[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_16 * _glesNormal));
  worldNormal_3 = tmpvar_17;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_15);
  highp float tmpvar_18;
  highp vec3 x_19;
  x_19 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_18 = sqrt(dot (x_19, x_19));
  fogpos_2 = tmpvar_18;
  tmpvar_8.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_8.x = (1.0 - tmpvar_8.y);
  highp vec3 tmpvar_20;
  tmpvar_20 = abs(_glesNormal);
  tmpvar_7.xy = tmpvar_1.xy;
  if ((tmpvar_20.x > tmpvar_20.y)) {
    if ((tmpvar_20.x > tmpvar_20.z)) {
      tmpvar_7.xy = tmpvar_1.yz;
    };
  } else {
    if ((tmpvar_20.y > tmpvar_20.z)) {
      tmpvar_7.xy = tmpvar_1.xz;
    };
  };
  tmpvar_7.xy = (tmpvar_7 * 2.0).xy;
  tmpvar_7.z = (_Object2World * _glesVertex).y;
  highp float tmpvar_21;
  tmpvar_21 = (((tmpvar_7.z * 2.0) + _Time.z) / 6.283);
  highp float tmpvar_22;
  tmpvar_22 = (fract(abs(tmpvar_21)) * 6.283);
  highp float tmpvar_23;
  if ((tmpvar_21 >= 0.0)) {
    tmpvar_23 = tmpvar_22;
  } else {
    tmpvar_23 = -(tmpvar_22);
  };
  tmpvar_7.z = sin(tmpvar_23);
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = worldNormal_3;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = worldViewDir_4;
  xlv_TEXCOORD4 = tmpvar_8;
  xlv_TEXCOORD6 = tmpvar_9;
  xlv_TEXCOORD7 = tmpvar_10;
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
uniform lowp vec4 unity_FogColor;
uniform highp int _ApplyRimIntensity;
uniform sampler2D _MainTex;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 c_3;
  lowp vec3 main_color_4;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = xlv_TEXCOORD0;
  mediump vec3 x2_7;
  mediump vec3 x1_8;
  x1_8.x = dot (unity_SHAr, tmpvar_6);
  x1_8.y = dot (unity_SHAg, tmpvar_6);
  x1_8.z = dot (unity_SHAb, tmpvar_6);
  mediump vec4 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD0.xyzz * xlv_TEXCOORD0.yzzx);
  x2_7.x = dot (unity_SHBr, tmpvar_9);
  x2_7.y = dot (unity_SHBg, tmpvar_9);
  x2_7.z = dot (unity_SHBb, tmpvar_9);
  mediump float tmpvar_10;
  tmpvar_10 = max (dot (xlv_TEXCOORD0, tmpvar_1), 0.0);
  tmpvar_2 = (1.0 - dot (xlv_TEXCOORD0, xlv_TEXCOORD3));
  mediump vec3 tmpvar_11;
  tmpvar_11 = ((max (
    ((1.055 * pow ((
      ((x2_7 + (unity_SHC.xyz * (
        (xlv_TEXCOORD0.x * xlv_TEXCOORD0.x)
       - 
        (xlv_TEXCOORD0.y * xlv_TEXCOORD0.y)
      ))) + x1_8)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_5.xyz) * ((tmpvar_10 * _LightDir.w) + (1.0 - _LightDir.w)));
  main_color_4 = tmpvar_11;
  lowp vec3 tmpvar_12;
  mediump float luminance_13;
  lowp float tmpvar_14;
  tmpvar_14 = (dot (main_color_4, vec3(1.0, 1.0, 1.0)) * 0.3333333);
  luminance_13 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (main_color_4, vec3(luminance_13), vec3((tmpvar_10 * _LightParameters.y)));
  tmpvar_12 = tmpvar_15;
  c_3.xyz = tmpvar_12;
  c_3.w = 1.0;
  lowp vec4 _color_16;
  _color_16.w = c_3.w;
  mediump float rTest_17;
  mediump vec2 tmpvar_18;
  tmpvar_18 = (xlv_TEXCOORD6.xy / xlv_TEXCOORD6.z);
  mediump float tmpvar_19;
  tmpvar_19 = ((1.5 * _FadeColor.w) - tmpvar_18.y);
  if ((tmpvar_19 < 0.0)) {
    discard;
  };
  mediump float tmpvar_20;
  tmpvar_20 = clamp ((1.0 - (tmpvar_19 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_21;
  tmpvar_21.x = tmpvar_18.x;
  tmpvar_21.y = clamp ((tmpvar_20 - tmpvar_18.y), 0.0, 1.0);
  lowp float tmpvar_22;
  tmpvar_22 = texture2D (_RasterTex, tmpvar_21).z;
  rTest_17 = tmpvar_22;
  mediump float x_23;
  x_23 = (rTest_17 - 0.4);
  if ((x_23 < 0.0)) {
    discard;
  };
  _color_16.xyz = (tmpvar_12 + (_FadeColor.xyz * tmpvar_20));
  lowp vec4 _color_24;
  _color_24.w = _color_16.w;
  _color_24.xyz = (_color_16.xyz + ((
    (tmpvar_2 * float(_ApplyRimIntensity))
   + 
    float((1 - _ApplyRimIntensity))
  ) * _RimColor.xyz));
  c_3.w = _color_24.w;
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (_color_24.xyz * xlv_TEXCOORD4.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec3 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD4;
out highp vec3 vs_TEXCOORD2;
out mediump vec3 vs_TEXCOORD3;
out mediump vec3 vs_TEXCOORD6;
out mediump vec4 vs_TEXCOORD7;
highp vec4 t0;
highp vec4 t1;
mediump float t16_2;
highp vec4 t3;
bvec3 tb3;
mediump float t16_6;
highp float t12;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t1.xyz;
    t12 = dot(t1.xyz, t1.xyz);
    t12 = inversesqrt(t12);
    t1.xyz = vec3(t12) * t1.xyz;
    vs_TEXCOORD0.xyz = t1.xyz;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t12 = dot(t1.xyz, t1.xyz);
    t12 = sqrt(t12);
    t16_2 = (-t12) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_6;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_6 = t16_2 * acid_Fog.z;
    vs_TEXCOORD4.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD4.y = t16_6;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    vs_TEXCOORD1.xy = t1.xy;
    t1.w = in_POSITION0.x;
    tb3.xyz = lessThan(abs(in_NORMAL0.yzzy), abs(in_NORMAL0.xxyx)).xyz;
    t1.x = (tb3.y) ? in_POSITION0.y : in_POSITION0.x;
    t1.y = (tb3.y) ? in_POSITION0.z : in_POSITION0.y;
    t1.z = (tb3.z) ? in_POSITION0.z : in_POSITION0.y;
    t1.xy = (tb3.x) ? t1.xy : t1.wz;
    vs_TEXCOORD2.xy = t1.xy + t1.xy;
    t1 = in_POSITION0.yyyy * _Object2World[1];
    t1 = _Object2World[0] * in_POSITION0.xxxx + t1;
    t1 = _Object2World[2] * in_POSITION0.zzzz + t1;
    t12 = _Object2World[3].y * in_POSITION0.w + t1.y;
    t1 = _Object2World[3] * in_POSITION0.wwww + t1;
    t12 = t12 * 2.0 + _Time.z;
    t12 = t12 * 0.159159631;
    tb3.x = t12>=(-t12);
    t12 = fract(abs(t12));
    t12 = (tb3.x) ? t12 : (-t12);
    t12 = t12 * 6.28299999;
    vs_TEXCOORD2.z = sin(t12);
    t3.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t3.xyz;
    t3.xyz = _Object2World[3].xyz * in_POSITION0.www + t3.xyz;
    t3.xyz = (-t3.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t12 = dot(t3.xyz, t3.xyz);
    t12 = inversesqrt(t12);
    t3.xyz = vec3(t12) * t3.xyz;
    vs_TEXCOORD3.xyz = t3.xyz;
    t0.w = t0.y * _ProjectionParams.x;
    t3.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t3.zz + t3.xw;
    vs_TEXCOORD6.xyz = t0.xyz;
    t0 = t1.yyyy * unity_World2Shadow[0][1];
    t0 = unity_World2Shadow[0][0] * t1.xxxx + t0;
    t0 = unity_World2Shadow[0][2] * t1.zzzz + t0;
    t0 = unity_World2Shadow[0][3] * t1.wwww + t0;
    vs_TEXCOORD7 = t0;
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
	lowp vec4 albedoTex;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	lowp vec3 glossColor;
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
uniform 	int _ApplyRimIntensity;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _LightDir;
uniform 	mediump float _Cutoff;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _RasterTex;
in mediump vec3 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD3;
in mediump vec3 vs_TEXCOORD6;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp float t10_1;
bool tb1;
mediump vec3 t16_2;
lowp vec3 t10_3;
highp int ti3;
lowp float t10_4;
mediump vec3 t16_7;
mediump float t16_15;
void main()
{
    t16_0.xy = vs_TEXCOORD6.xy / vs_TEXCOORD6.zz;
    t16_15 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_15<0.0;
    t16_15 = (-t16_15) * 2.5 + 1.0;
    t16_15 = clamp(t16_15, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_15;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1 = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1 + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD0.y * vs_TEXCOORD0.y;
    t16_0.x = vs_TEXCOORD0.x * vs_TEXCOORD0.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD0.yzzx * vs_TEXCOORD0.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD0.xyz;
    t16_1.w = 1.0;
    t16_2.x = dot(unity_SHAr, t16_1);
    t16_2.y = dot(unity_SHAg, t16_1);
    t16_2.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_2.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD1.xy).xyz;
    t16_0.xyz = t16_0.xyz * t10_3.xyz;
    t10_4 = (-_LightDir.w) + 1.0;
    t16_2.x = dot(vs_TEXCOORD0.xyz, (-_LightDir.xyz));
    t16_2.x = max(t16_2.x, 0.0);
    t16_7.x = t16_2.x * _LightDir.w + t10_4;
    t16_2.x = t16_2.x * _LightParameters.y;
    t16_0.xyz = t16_0.xyz * t16_7.xxx;
    t10_4 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_7.xyz = vec3(t10_4) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = t16_2.xxx * t16_7.xyz + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_15) + t16_0.xyz;
    t16_15 = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD3.xyz);
    t16_15 = (-t16_15) + 1.0;
    ti3 = (-_ApplyRimIntensity) + 0x1;
    t16_2.x = float(ti3);
    t16_7.x = float(_ApplyRimIntensity);
    t16_15 = t16_15 * t16_7.x + t16_2.x;
    t16_0.xyz = vec3(t16_15) * _RimColor.xyz + t16_0.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD4.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD4.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_OFF" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_OFF" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_OFF" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_OFF" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_OFF" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_OFF" "RASTER_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "RASTER_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_OFF" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_OFF" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_OFF" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "RASTER_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES3"
}
}
 }
}
Fallback "Environment/EnvDiffuse"
}
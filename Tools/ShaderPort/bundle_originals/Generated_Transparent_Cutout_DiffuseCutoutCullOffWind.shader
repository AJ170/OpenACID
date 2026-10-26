Shader "Generated/Transparent/Cutout/DiffuseCutoutCullOffWind" {
Properties {
 _MainTex ("Albedo (RGB) Alpha (A)", 2D) = "white" { }
 _WindFrequency ("Flutter Frequency (XYZ) Bend Frequency (W)", Vector) = (1.975,0.793,0.375,0.193)
 _WindAmplitude ("Flutter Amplitude (XYZ) Bend Amplitude (W)", Vector) = (1,1,1,1)
 _WindDirFromObjNormalOrGlobal ("Wind Direction from Global Windsystem or Objectspace Normal", Range(0,1)) = 0
 _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
 _ShadowBrightening ("ShadowColor Usage", Range(0,1)) = 0
 _ShadowColor ("Shadow Color", Color) = (0,0,0,0)
}
SubShader { 
 LOD 99
 Tags { "QUEUE"="AlphaTest" "RenderType"="TransparentCutout" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="AlphaTest" "RenderType"="TransparentCutout" }
  Cull Off
  ColorMask RGB
  GpuProgramID 20593
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "LIGHTMAP_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _World2Object;
uniform highp vec4 _WindParams;
uniform highp vec4 _WindDir;
uniform highp float _WindDirFromObjNormalOrGlobal;
uniform mediump vec4 _WindFrequency;
uniform mediump vec4 _WindAmplitude;
uniform mediump vec4 acid_Fog;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float fogpos_2;
  mediump vec2 tmpvar_3;
  tmpvar_3.x = 0.0;
  highp vec4 v_position_4;
  v_position_4.w = _glesVertex.w;
  lowp vec4 v_color_5;
  v_color_5 = tmpvar_1;
  highp vec4 frequency_6;
  frequency_6 = _WindFrequency;
  highp vec4 amplitude_7;
  amplitude_7 = _WindAmplitude;
  highp vec4 waves_8;
  highp vec4 wavesIn_9;
  highp float leafPhase_10;
  highp float objPhase_11;
  lowp float tmpvar_12;
  tmpvar_12 = v_color_5.x;
  objPhase_11 = tmpvar_12;
  lowp float tmpvar_13;
  tmpvar_13 = v_color_5.y;
  leafPhase_10 = tmpvar_13;
  wavesIn_9 = ((_WindParams.xxxy * frequency_6) + vec4(objPhase_11));
  wavesIn_9.xyz = (wavesIn_9.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_10) * vec3(0.5, 0.5, 0.5))));
  wavesIn_9.xyz = (wavesIn_9.xyz + vec3(leafPhase_10));
  highp vec4 tmpvar_14;
  tmpvar_14 = abs(((
    fract((((
      fract(wavesIn_9)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_8 = (((tmpvar_14 * tmpvar_14) * (3.0 - 
    (2.0 * tmpvar_14)
  )) * amplitude_7);
  v_position_4.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_8.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * v_color_5.w));
  highp vec4 tmpvar_15;
  tmpvar_15.w = 0.0;
  tmpvar_15.xyz = _WindDir.xyz;
  v_position_4.xyz = (v_position_4.xyz + (normalize(
    mix ((_World2Object * tmpvar_15).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * v_color_5.z) * waves_8.w)
   + 
    (v_color_5.z * _WindParams.z)
  )));
  highp float tmpvar_16;
  highp vec3 x_17;
  x_17 = (glstate_matrix_modelview0 * v_position_4).xyz;
  tmpvar_16 = sqrt(dot (x_17, x_17));
  fogpos_2 = tmpvar_16;
  tmpvar_3.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_3.x = (1.0 - tmpvar_3.y);
  gl_Position = (glstate_matrix_mvp * v_position_4);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD6 = tmpvar_3;
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD6;
void main ()
{
  mediump vec4 albedoTex_1;
  lowp vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  albedoTex_1 = tmpvar_3;
  c_2.xyz = albedoTex_1.xyz;
  c_2.w = albedoTex_1.w;
  mediump float x_4;
  x_4 = (c_2.w - _Cutoff);
  if ((x_4 < 0.0)) {
    discard;
  };
  c_2.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_2.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_2;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "LIGHTMAP_OFF" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_fd_Type {
	mediump vec3 albedo;
	mediump vec3 specular;
	mediump vec3 shadow;
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	mediump float _Cutoff;
uniform 	lowp float _ShadowBrightening;
uniform 	lowp vec4 _ShadowColor;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_3;
mediump float t16_7;
highp float t12;
void main()
{
    t0.xyz = _World2Object[1].xyz * _WindDir.yyy;
    t0.xyz = _World2Object[0].xyz * _WindDir.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WindDir.zzz + t0.xyz;
    t1.xyz = (-t0.xyz) + in_NORMAL0.xyz;
    t0.xyz = vec3(_WindDirFromObjNormalOrGlobal) * t1.xyz + t0.xyz;
    t12 = dot(t0.xyz, t0.xyz);
    t12 = inversesqrt(t12);
    t0.xyz = vec3(t12) * t0.xyz;
    t12 = in_COLOR0.y * 0.5 + 0.5;
    t1 = _WindParams.xxxy * _WindFrequency + in_COLOR0.xxxx;
    t1.xyz = t1.xyz * vec3(t12) + in_COLOR0.yyy;
    t2 = fract(t1);
    t1 = t2 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-0.5, -0.5, -0.5, -0.5);
    t1 = fract(t1);
    t1 = t1 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-1.0, -1.0, -1.0, -1.0);
    t2 = abs(t1) * abs(t1);
    t1 = -abs(t1) * vec4(2.0, 2.0, 2.0, 2.0) + vec4(3.0, 3.0, 3.0, 3.0);
    t1 = t1 * t2;
    t1 = t1 * _WindAmplitude;
    t12 = dot(t1.xyz, vec3(1.0, 1.0, 1.0));
    t12 = t12 * _WindParams.w;
    t1.xyz = vec3(t12) * in_NORMAL0.xyz;
    t1.xyz = t1.xyz * in_COLOR0.www + in_POSITION0.xyz;
    t12 = in_COLOR0.z * _WindDir.w;
    t2.x = in_COLOR0.z * _WindParams.z;
    t12 = t12 * t1.w + t2.x;
    t0.xyz = t0.xyz * vec3(t12) + t1.xyz;
    t1 = t0.yyyy * glstate_matrix_mvp[1];
    t1 = glstate_matrix_mvp[0] * t0.xxxx + t1;
    t1 = glstate_matrix_mvp[2] * t0.zzzz + t1;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t1;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t1.xyz = t0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyw = glstate_matrix_modelview0[0].xyz * t0.xxx + t1.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * t0.zzz + t0.xyw;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_3 = (-t0.x) + acid_Fog.y;
    t16_7 = (-acid_Fog.x) + acid_Fog.y;
    t16_3 = t16_3 / t16_7;
    t16_3 = clamp(t16_3, 0.0, 1.0);
    t16_3 = (-t16_3) + 1.0;
    vs_TEXCOORD6.x = (-t16_3) * acid_Fog.z + 1.0;
    t16_3 = t16_3 * acid_Fog.z;
    vs_TEXCOORD6.y = t16_3;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_fd_Type {
	mediump vec3 albedo;
	mediump vec3 specular;
	mediump vec3 shadow;
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	mediump float _Cutoff;
uniform 	lowp float _ShadowBrightening;
uniform 	lowp vec4 _ShadowColor;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
layout(location = 0) out lowp vec4 SV_Target0;
lowp vec4 t10_0;
mediump vec3 t16_1;
bool tb2;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_1.x = t10_0.w + (-_Cutoff);
    tb2 = t16_1.x<0.0;
    if((int(tb2) * int(0xffffffffu))!=0){discard;}
    t16_1.xyz = t10_0.xyz * vs_TEXCOORD6.xxx;
    SV_Target0.w = t10_0.w;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_1.xyz;
    SV_Target0.xyz = t16_1.xyz;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "LIGHTMAP_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _World2Object;
uniform highp vec4 _WindParams;
uniform highp vec4 _WindDir;
uniform highp float _WindDirFromObjNormalOrGlobal;
uniform mediump vec4 _WindFrequency;
uniform mediump vec4 _WindAmplitude;
uniform mediump vec4 acid_Fog;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float fogpos_2;
  mediump vec2 tmpvar_3;
  tmpvar_3.x = 0.0;
  highp vec4 v_position_4;
  v_position_4.w = _glesVertex.w;
  lowp vec4 v_color_5;
  v_color_5 = tmpvar_1;
  highp vec4 frequency_6;
  frequency_6 = _WindFrequency;
  highp vec4 amplitude_7;
  amplitude_7 = _WindAmplitude;
  highp vec4 waves_8;
  highp vec4 wavesIn_9;
  highp float leafPhase_10;
  highp float objPhase_11;
  lowp float tmpvar_12;
  tmpvar_12 = v_color_5.x;
  objPhase_11 = tmpvar_12;
  lowp float tmpvar_13;
  tmpvar_13 = v_color_5.y;
  leafPhase_10 = tmpvar_13;
  wavesIn_9 = ((_WindParams.xxxy * frequency_6) + vec4(objPhase_11));
  wavesIn_9.xyz = (wavesIn_9.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_10) * vec3(0.5, 0.5, 0.5))));
  wavesIn_9.xyz = (wavesIn_9.xyz + vec3(leafPhase_10));
  highp vec4 tmpvar_14;
  tmpvar_14 = abs(((
    fract((((
      fract(wavesIn_9)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_8 = (((tmpvar_14 * tmpvar_14) * (3.0 - 
    (2.0 * tmpvar_14)
  )) * amplitude_7);
  v_position_4.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_8.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * v_color_5.w));
  highp vec4 tmpvar_15;
  tmpvar_15.w = 0.0;
  tmpvar_15.xyz = _WindDir.xyz;
  v_position_4.xyz = (v_position_4.xyz + (normalize(
    mix ((_World2Object * tmpvar_15).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * v_color_5.z) * waves_8.w)
   + 
    (v_color_5.z * _WindParams.z)
  )));
  highp float tmpvar_16;
  highp vec3 x_17;
  x_17 = (glstate_matrix_modelview0 * v_position_4).xyz;
  tmpvar_16 = sqrt(dot (x_17, x_17));
  fogpos_2 = tmpvar_16;
  tmpvar_3.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_3.x = (1.0 - tmpvar_3.y);
  gl_Position = (glstate_matrix_mvp * v_position_4);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD6 = tmpvar_3;
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD6;
void main ()
{
  mediump vec4 albedoTex_1;
  lowp vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  albedoTex_1 = tmpvar_3;
  c_2.xyz = albedoTex_1.xyz;
  c_2.w = albedoTex_1.w;
  mediump float x_4;
  x_4 = (c_2.w - _Cutoff);
  if ((x_4 < 0.0)) {
    discard;
  };
  c_2.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_2.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_2;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "LIGHTMAP_OFF" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_fd_Type {
	mediump vec3 albedo;
	mediump vec3 specular;
	mediump vec3 shadow;
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	mediump float _Cutoff;
uniform 	lowp float _ShadowBrightening;
uniform 	lowp vec4 _ShadowColor;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_3;
mediump float t16_7;
highp float t12;
void main()
{
    t0.xyz = _World2Object[1].xyz * _WindDir.yyy;
    t0.xyz = _World2Object[0].xyz * _WindDir.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WindDir.zzz + t0.xyz;
    t1.xyz = (-t0.xyz) + in_NORMAL0.xyz;
    t0.xyz = vec3(_WindDirFromObjNormalOrGlobal) * t1.xyz + t0.xyz;
    t12 = dot(t0.xyz, t0.xyz);
    t12 = inversesqrt(t12);
    t0.xyz = vec3(t12) * t0.xyz;
    t12 = in_COLOR0.y * 0.5 + 0.5;
    t1 = _WindParams.xxxy * _WindFrequency + in_COLOR0.xxxx;
    t1.xyz = t1.xyz * vec3(t12) + in_COLOR0.yyy;
    t2 = fract(t1);
    t1 = t2 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-0.5, -0.5, -0.5, -0.5);
    t1 = fract(t1);
    t1 = t1 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-1.0, -1.0, -1.0, -1.0);
    t2 = abs(t1) * abs(t1);
    t1 = -abs(t1) * vec4(2.0, 2.0, 2.0, 2.0) + vec4(3.0, 3.0, 3.0, 3.0);
    t1 = t1 * t2;
    t1 = t1 * _WindAmplitude;
    t12 = dot(t1.xyz, vec3(1.0, 1.0, 1.0));
    t12 = t12 * _WindParams.w;
    t1.xyz = vec3(t12) * in_NORMAL0.xyz;
    t1.xyz = t1.xyz * in_COLOR0.www + in_POSITION0.xyz;
    t12 = in_COLOR0.z * _WindDir.w;
    t2.x = in_COLOR0.z * _WindParams.z;
    t12 = t12 * t1.w + t2.x;
    t0.xyz = t0.xyz * vec3(t12) + t1.xyz;
    t1 = t0.yyyy * glstate_matrix_mvp[1];
    t1 = glstate_matrix_mvp[0] * t0.xxxx + t1;
    t1 = glstate_matrix_mvp[2] * t0.zzzz + t1;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t1;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t1.xyz = t0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyw = glstate_matrix_modelview0[0].xyz * t0.xxx + t1.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * t0.zzz + t0.xyw;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_3 = (-t0.x) + acid_Fog.y;
    t16_7 = (-acid_Fog.x) + acid_Fog.y;
    t16_3 = t16_3 / t16_7;
    t16_3 = clamp(t16_3, 0.0, 1.0);
    t16_3 = (-t16_3) + 1.0;
    vs_TEXCOORD6.x = (-t16_3) * acid_Fog.z + 1.0;
    t16_3 = t16_3 * acid_Fog.z;
    vs_TEXCOORD6.y = t16_3;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_fd_Type {
	mediump vec3 albedo;
	mediump vec3 specular;
	mediump vec3 shadow;
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	mediump float _Cutoff;
uniform 	lowp float _ShadowBrightening;
uniform 	lowp vec4 _ShadowColor;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
layout(location = 0) out lowp vec4 SV_Target0;
lowp vec4 t10_0;
mediump vec3 t16_1;
bool tb2;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_1.x = t10_0.w + (-_Cutoff);
    tb2 = t16_1.x<0.0;
    if((int(tb2) * int(0xffffffffu))!=0){discard;}
    t16_1.xyz = t10_0.xyz * vs_TEXCOORD6.xxx;
    SV_Target0.w = t10_0.w;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_1.xyz;
    SV_Target0.xyz = t16_1.xyz;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "LIGHTMAP_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
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
uniform mediump vec4 acid_Fog;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float fogpos_2;
  mediump vec2 tmpvar_3;
  mediump vec4 tmpvar_4;
  tmpvar_3.x = 0.0;
  highp vec4 v_position_5;
  v_position_5.w = _glesVertex.w;
  lowp vec4 v_color_6;
  v_color_6 = tmpvar_1;
  highp vec4 frequency_7;
  frequency_7 = _WindFrequency;
  highp vec4 amplitude_8;
  amplitude_8 = _WindAmplitude;
  highp vec4 waves_9;
  highp vec4 wavesIn_10;
  highp float leafPhase_11;
  highp float objPhase_12;
  lowp float tmpvar_13;
  tmpvar_13 = v_color_6.x;
  objPhase_12 = tmpvar_13;
  lowp float tmpvar_14;
  tmpvar_14 = v_color_6.y;
  leafPhase_11 = tmpvar_14;
  wavesIn_10 = ((_WindParams.xxxy * frequency_7) + vec4(objPhase_12));
  wavesIn_10.xyz = (wavesIn_10.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_11) * vec3(0.5, 0.5, 0.5))));
  wavesIn_10.xyz = (wavesIn_10.xyz + vec3(leafPhase_11));
  highp vec4 tmpvar_15;
  tmpvar_15 = abs(((
    fract((((
      fract(wavesIn_10)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_9 = (((tmpvar_15 * tmpvar_15) * (3.0 - 
    (2.0 * tmpvar_15)
  )) * amplitude_8);
  v_position_5.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_9.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * v_color_6.w));
  highp vec4 tmpvar_16;
  tmpvar_16.w = 0.0;
  tmpvar_16.xyz = _WindDir.xyz;
  v_position_5.xyz = (v_position_5.xyz + (normalize(
    mix ((_World2Object * tmpvar_16).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * v_color_6.z) * waves_9.w)
   + 
    (v_color_6.z * _WindParams.z)
  )));
  highp float tmpvar_17;
  highp vec3 x_18;
  x_18 = (glstate_matrix_modelview0 * v_position_5).xyz;
  tmpvar_17 = sqrt(dot (x_18, x_18));
  fogpos_2 = tmpvar_17;
  tmpvar_3.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_3.x = (1.0 - tmpvar_3.y);
  tmpvar_4 = (unity_World2Shadow[0] * (_Object2World * v_position_5));
  gl_Position = (glstate_matrix_mvp * v_position_5);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD6 = tmpvar_3;
  xlv_TEXCOORD7 = tmpvar_4;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
uniform lowp vec4 _EnvAmbientColor;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  mediump vec4 albedoTex_2;
  lowp vec4 c_3;
  c_3.w = 1.0;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0);
  albedoTex_2 = tmpvar_4;
  c_3.xyz = albedoTex_2.xyz;
  lowp float tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD7).x > 
    (xlv_TEXCOORD7.z / xlv_TEXCOORD7.w)
  )), _LightShadowData.x);
  tmpvar_5 = tmpvar_6;
  tmpvar_1 = ((_EnvAmbientColor.xyz * (1.0 - tmpvar_5)) + tmpvar_5);
  c_3.xyz = (c_3.xyz * tmpvar_1);
  c_3.w = albedoTex_2.w;
  mediump float x_7;
  x_7 = (c_3.w - _Cutoff);
  if ((x_7 < 0.0)) {
    discard;
  };
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_3.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
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
uniform mediump vec4 acid_Fog;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float fogpos_2;
  mediump vec2 tmpvar_3;
  mediump vec4 tmpvar_4;
  tmpvar_3.x = 0.0;
  highp vec4 v_position_5;
  v_position_5.w = _glesVertex.w;
  lowp vec4 v_color_6;
  v_color_6 = tmpvar_1;
  highp vec4 frequency_7;
  frequency_7 = _WindFrequency;
  highp vec4 amplitude_8;
  amplitude_8 = _WindAmplitude;
  highp vec4 waves_9;
  highp vec4 wavesIn_10;
  highp float leafPhase_11;
  highp float objPhase_12;
  lowp float tmpvar_13;
  tmpvar_13 = v_color_6.x;
  objPhase_12 = tmpvar_13;
  lowp float tmpvar_14;
  tmpvar_14 = v_color_6.y;
  leafPhase_11 = tmpvar_14;
  wavesIn_10 = ((_WindParams.xxxy * frequency_7) + vec4(objPhase_12));
  wavesIn_10.xyz = (wavesIn_10.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_11) * vec3(0.5, 0.5, 0.5))));
  wavesIn_10.xyz = (wavesIn_10.xyz + vec3(leafPhase_11));
  highp vec4 tmpvar_15;
  tmpvar_15 = abs(((
    fract((((
      fract(wavesIn_10)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_9 = (((tmpvar_15 * tmpvar_15) * (3.0 - 
    (2.0 * tmpvar_15)
  )) * amplitude_8);
  v_position_5.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_9.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * v_color_6.w));
  highp vec4 tmpvar_16;
  tmpvar_16.w = 0.0;
  tmpvar_16.xyz = _WindDir.xyz;
  v_position_5.xyz = (v_position_5.xyz + (normalize(
    mix ((_World2Object * tmpvar_16).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * v_color_6.z) * waves_9.w)
   + 
    (v_color_6.z * _WindParams.z)
  )));
  highp float tmpvar_17;
  highp vec3 x_18;
  x_18 = (glstate_matrix_modelview0 * v_position_5).xyz;
  tmpvar_17 = sqrt(dot (x_18, x_18));
  fogpos_2 = tmpvar_17;
  tmpvar_3.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_3.x = (1.0 - tmpvar_3.y);
  tmpvar_4 = (unity_World2Shadow[0] * (_Object2World * v_position_5));
  gl_Position = (glstate_matrix_mvp * v_position_5);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD6 = tmpvar_3;
  xlv_TEXCOORD7 = tmpvar_4;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
uniform lowp vec4 _EnvAmbientColor;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  mediump vec4 albedoTex_2;
  lowp vec4 c_3;
  c_3.w = 1.0;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0);
  albedoTex_2 = tmpvar_4;
  c_3.xyz = albedoTex_2.xyz;
  lowp float shadow_5;
  shadow_5 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD7.xyz) * (1.0 - _LightShadowData.x)));
  tmpvar_1 = ((_EnvAmbientColor.xyz * (1.0 - shadow_5)) + shadow_5);
  c_3.xyz = (c_3.xyz * tmpvar_1);
  c_3.w = albedoTex_2.w;
  mediump float x_6;
  x_6 = (c_3.w - _Cutoff);
  if ((x_6 < 0.0)) {
    discard;
  };
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_3.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_3;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_fd_Type {
	mediump vec3 albedo;
	mediump vec3 specular;
	mediump vec3 shadow;
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	mediump float _Cutoff;
uniform 	lowp float _ShadowBrightening;
uniform 	lowp vec4 _ShadowColor;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
out mediump vec4 vs_TEXCOORD7;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_3;
mediump float t16_7;
highp float t12;
void main()
{
    t0.xyz = _World2Object[1].xyz * _WindDir.yyy;
    t0.xyz = _World2Object[0].xyz * _WindDir.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WindDir.zzz + t0.xyz;
    t1.xyz = (-t0.xyz) + in_NORMAL0.xyz;
    t0.xyz = vec3(_WindDirFromObjNormalOrGlobal) * t1.xyz + t0.xyz;
    t12 = dot(t0.xyz, t0.xyz);
    t12 = inversesqrt(t12);
    t0.xyz = vec3(t12) * t0.xyz;
    t12 = in_COLOR0.y * 0.5 + 0.5;
    t1 = _WindParams.xxxy * _WindFrequency + in_COLOR0.xxxx;
    t1.xyz = t1.xyz * vec3(t12) + in_COLOR0.yyy;
    t2 = fract(t1);
    t1 = t2 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-0.5, -0.5, -0.5, -0.5);
    t1 = fract(t1);
    t1 = t1 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-1.0, -1.0, -1.0, -1.0);
    t2 = abs(t1) * abs(t1);
    t1 = -abs(t1) * vec4(2.0, 2.0, 2.0, 2.0) + vec4(3.0, 3.0, 3.0, 3.0);
    t1 = t1 * t2;
    t1 = t1 * _WindAmplitude;
    t12 = dot(t1.xyz, vec3(1.0, 1.0, 1.0));
    t12 = t12 * _WindParams.w;
    t1.xyz = vec3(t12) * in_NORMAL0.xyz;
    t1.xyz = t1.xyz * in_COLOR0.www + in_POSITION0.xyz;
    t12 = in_COLOR0.z * _WindDir.w;
    t2.x = in_COLOR0.z * _WindParams.z;
    t12 = t12 * t1.w + t2.x;
    t0.xyz = t0.xyz * vec3(t12) + t1.xyz;
    t1 = t0.yyyy * glstate_matrix_mvp[1];
    t1 = glstate_matrix_mvp[0] * t0.xxxx + t1;
    t1 = glstate_matrix_mvp[2] * t0.zzzz + t1;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t1;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t1.xyz = t0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * t0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * t0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t12 = dot(t1.xyz, t1.xyz);
    t12 = sqrt(t12);
    t16_3 = (-t12) + acid_Fog.y;
    t16_7 = (-acid_Fog.x) + acid_Fog.y;
    t16_3 = t16_3 / t16_7;
    t16_3 = clamp(t16_3, 0.0, 1.0);
    t16_3 = (-t16_3) + 1.0;
    t16_7 = t16_3 * acid_Fog.z;
    vs_TEXCOORD6.x = (-t16_3) * acid_Fog.z + 1.0;
    vs_TEXCOORD6.y = t16_7;
    t1 = t0.yyyy * _Object2World[1];
    t1 = _Object2World[0] * t0.xxxx + t1;
    t0 = _Object2World[2] * t0.zzzz + t1;
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
struct g_fd_Type {
	mediump vec3 albedo;
	mediump vec3 specular;
	mediump vec3 shadow;
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	mediump float _Cutoff;
uniform 	lowp float _ShadowBrightening;
uniform 	lowp vec4 _ShadowColor;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in mediump vec4 vs_TEXCOORD7;
layout(location = 0) out lowp vec4 SV_Target0;
lowp vec4 t10_0;
mediump vec3 t16_1;
mediump vec3 t16_2;
bool tb2;
mediump float t16_4;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_1.x = t10_0.w + (-_Cutoff);
    tb2 = t16_1.x<0.0;
    if((int(tb2) * int(0xffffffffu))!=0){discard;}
    vec3 txVec9 = vec3(vs_TEXCOORD7.xy,vs_TEXCOORD7.z);
    t16_1.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec9, 0.0);
    t16_4 = (-_LightShadowData.x) + 1.0;
    t16_1.x = t16_1.x * t16_4 + _LightShadowData.x;
    t16_2.x = (-t16_1.x) + 1.0;
    t16_2.xyz = _EnvAmbientColor.xyz * t16_2.xxx + t16_1.xxx;
    t16_1.xyz = t10_0.xyz * t16_2.xyz;
    SV_Target0.w = t10_0.w;
    t16_1.xyz = t16_1.xyz * vs_TEXCOORD6.xxx;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_1.xyz;
    SV_Target0.xyz = t16_1.xyz;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "LIGHTMAP_ON" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _WindParams;
uniform highp vec4 _WindDir;
uniform highp float _WindDirFromObjNormalOrGlobal;
uniform mediump vec4 _WindFrequency;
uniform mediump vec4 _WindAmplitude;
uniform mediump vec4 acid_Fog;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD6;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float fogpos_2;
  mediump vec2 tmpvar_3;
  tmpvar_3.x = 0.0;
  highp vec4 v_position_4;
  v_position_4.w = _glesVertex.w;
  lowp vec4 v_color_5;
  v_color_5 = tmpvar_1;
  highp vec4 frequency_6;
  frequency_6 = _WindFrequency;
  highp vec4 amplitude_7;
  amplitude_7 = _WindAmplitude;
  highp vec4 waves_8;
  highp vec4 wavesIn_9;
  highp float leafPhase_10;
  highp float objPhase_11;
  lowp float tmpvar_12;
  tmpvar_12 = v_color_5.x;
  objPhase_11 = tmpvar_12;
  lowp float tmpvar_13;
  tmpvar_13 = v_color_5.y;
  leafPhase_10 = tmpvar_13;
  wavesIn_9 = ((_WindParams.xxxy * frequency_6) + vec4(objPhase_11));
  wavesIn_9.xyz = (wavesIn_9.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_10) * vec3(0.5, 0.5, 0.5))));
  wavesIn_9.xyz = (wavesIn_9.xyz + vec3(leafPhase_10));
  highp vec4 tmpvar_14;
  tmpvar_14 = abs(((
    fract((((
      fract(wavesIn_9)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_8 = (((tmpvar_14 * tmpvar_14) * (3.0 - 
    (2.0 * tmpvar_14)
  )) * amplitude_7);
  v_position_4.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_8.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * v_color_5.w));
  highp vec4 tmpvar_15;
  tmpvar_15.w = 0.0;
  tmpvar_15.xyz = _WindDir.xyz;
  v_position_4.xyz = (v_position_4.xyz + (normalize(
    mix ((_World2Object * tmpvar_15).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * v_color_5.z) * waves_8.w)
   + 
    (v_color_5.z * _WindParams.z)
  )));
  highp float tmpvar_16;
  highp vec3 x_17;
  x_17 = (glstate_matrix_modelview0 * v_position_4).xyz;
  tmpvar_16 = sqrt(dot (x_17, x_17));
  fogpos_2 = tmpvar_16;
  tmpvar_3.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_3.x = (1.0 - tmpvar_3.y);
  gl_Position = (glstate_matrix_mvp * v_position_4);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD6 = tmpvar_3;
  xlv_TEXCOORD8 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
uniform lowp float _ShadowBrightening;
uniform lowp vec4 _ShadowColor;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD6;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  lowp float brightness_2;
  mediump vec4 albedoTex_3;
  lowp vec4 c_4;
  c_4.w = 1.0;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0);
  albedoTex_3 = tmpvar_5;
  c_4.xyz = albedoTex_3.xyz;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_7;
  tmpvar_7 = (2.0 * tmpvar_6.xyz);
  mediump float tmpvar_8;
  tmpvar_8 = min (1.0, dot (tmpvar_7, vec3(0.3, 0.59, 0.11)));
  brightness_2 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = vec3(((1.0 - (
    (brightness_2 * brightness_2)
   * brightness_2)) * _ShadowBrightening));
  tmpvar_1 = mix (tmpvar_7, _ShadowColor.xyz, tmpvar_9);
  c_4.xyz = (c_4.xyz * tmpvar_1);
  c_4.w = albedoTex_3.w;
  mediump float x_10;
  x_10 = (c_4.w - _Cutoff);
  if ((x_10 < 0.0)) {
    discard;
  };
  c_4.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_4.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_4;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "LIGHTMAP_ON" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_fd_Type {
	mediump vec3 albedo;
	mediump vec3 specular;
	mediump vec3 shadow;
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	mediump float _Cutoff;
uniform 	lowp float _ShadowBrightening;
uniform 	lowp vec4 _ShadowColor;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD1;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
out highp vec2 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_3;
mediump float t16_7;
highp float t12;
void main()
{
    t0.xyz = _World2Object[1].xyz * _WindDir.yyy;
    t0.xyz = _World2Object[0].xyz * _WindDir.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WindDir.zzz + t0.xyz;
    t1.xyz = (-t0.xyz) + in_NORMAL0.xyz;
    t0.xyz = vec3(_WindDirFromObjNormalOrGlobal) * t1.xyz + t0.xyz;
    t12 = dot(t0.xyz, t0.xyz);
    t12 = inversesqrt(t12);
    t0.xyz = vec3(t12) * t0.xyz;
    t12 = in_COLOR0.y * 0.5 + 0.5;
    t1 = _WindParams.xxxy * _WindFrequency + in_COLOR0.xxxx;
    t1.xyz = t1.xyz * vec3(t12) + in_COLOR0.yyy;
    t2 = fract(t1);
    t1 = t2 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-0.5, -0.5, -0.5, -0.5);
    t1 = fract(t1);
    t1 = t1 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-1.0, -1.0, -1.0, -1.0);
    t2 = abs(t1) * abs(t1);
    t1 = -abs(t1) * vec4(2.0, 2.0, 2.0, 2.0) + vec4(3.0, 3.0, 3.0, 3.0);
    t1 = t1 * t2;
    t1 = t1 * _WindAmplitude;
    t12 = dot(t1.xyz, vec3(1.0, 1.0, 1.0));
    t12 = t12 * _WindParams.w;
    t1.xyz = vec3(t12) * in_NORMAL0.xyz;
    t1.xyz = t1.xyz * in_COLOR0.www + in_POSITION0.xyz;
    t12 = in_COLOR0.z * _WindDir.w;
    t2.x = in_COLOR0.z * _WindParams.z;
    t12 = t12 * t1.w + t2.x;
    t0.xyz = t0.xyz * vec3(t12) + t1.xyz;
    t1 = t0.yyyy * glstate_matrix_mvp[1];
    t1 = glstate_matrix_mvp[0] * t0.xxxx + t1;
    t1 = glstate_matrix_mvp[2] * t0.zzzz + t1;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t1;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t1.xyz = t0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyw = glstate_matrix_modelview0[0].xyz * t0.xxx + t1.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * t0.zzz + t0.xyw;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_3 = (-t0.x) + acid_Fog.y;
    t16_7 = (-acid_Fog.x) + acid_Fog.y;
    t16_3 = t16_3 / t16_7;
    t16_3 = clamp(t16_3, 0.0, 1.0);
    t16_3 = (-t16_3) + 1.0;
    vs_TEXCOORD6.x = (-t16_3) * acid_Fog.z + 1.0;
    t16_3 = t16_3 * acid_Fog.z;
    vs_TEXCOORD6.y = t16_3;
    vs_TEXCOORD8.xy = in_TEXCOORD1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_fd_Type {
	mediump vec3 albedo;
	mediump vec3 specular;
	mediump vec3 shadow;
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	mediump float _Cutoff;
uniform 	lowp float _ShadowBrightening;
uniform 	lowp vec4 _ShadowColor;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D unity_Lightmap;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in highp vec2 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
lowp vec4 t10_0;
mediump vec3 t16_1;
lowp vec3 t10_2;
bool tb2;
lowp vec3 t10_3;
mediump float t16_13;
lowp float t10_15;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_1.x = t10_0.w + (-_Cutoff);
    tb2 = t16_1.x<0.0;
    if((int(tb2) * int(0xffffffffu))!=0){discard;}
    t10_2.xyz = texture(unity_Lightmap, vs_TEXCOORD8.xy).xyz;
    t10_3.xyz = t10_2.xyz * vec3(2.0, 2.0, 2.0);
    t16_1.xyz = vec3(-2.0, -2.0, -2.0) * t10_2.xyz + _ShadowColor.xyz;
    t16_13 = dot(t10_3.xyz, vec3(0.300000012, 0.589999974, 0.109999999));
    t16_13 = min(t16_13, 1.0);
    t10_15 = t16_13 * t16_13;
    t10_15 = (-t10_15) * t16_13 + 1.0;
    t10_15 = t10_15 * _ShadowBrightening;
    t16_1.xyz = vec3(t10_15) * t16_1.xyz + t10_3.xyz;
    t16_1.xyz = t10_0.xyz * t16_1.xyz;
    SV_Target0.w = t10_0.w;
    t16_1.xyz = t16_1.xyz * vs_TEXCOORD6.xxx;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_1.xyz;
    SV_Target0.xyz = t16_1.xyz;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "LIGHTMAP_ON" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _WindParams;
uniform highp vec4 _WindDir;
uniform highp float _WindDirFromObjNormalOrGlobal;
uniform mediump vec4 _WindFrequency;
uniform mediump vec4 _WindAmplitude;
uniform mediump vec4 acid_Fog;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD6;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float fogpos_2;
  mediump vec2 tmpvar_3;
  tmpvar_3.x = 0.0;
  highp vec4 v_position_4;
  v_position_4.w = _glesVertex.w;
  lowp vec4 v_color_5;
  v_color_5 = tmpvar_1;
  highp vec4 frequency_6;
  frequency_6 = _WindFrequency;
  highp vec4 amplitude_7;
  amplitude_7 = _WindAmplitude;
  highp vec4 waves_8;
  highp vec4 wavesIn_9;
  highp float leafPhase_10;
  highp float objPhase_11;
  lowp float tmpvar_12;
  tmpvar_12 = v_color_5.x;
  objPhase_11 = tmpvar_12;
  lowp float tmpvar_13;
  tmpvar_13 = v_color_5.y;
  leafPhase_10 = tmpvar_13;
  wavesIn_9 = ((_WindParams.xxxy * frequency_6) + vec4(objPhase_11));
  wavesIn_9.xyz = (wavesIn_9.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_10) * vec3(0.5, 0.5, 0.5))));
  wavesIn_9.xyz = (wavesIn_9.xyz + vec3(leafPhase_10));
  highp vec4 tmpvar_14;
  tmpvar_14 = abs(((
    fract((((
      fract(wavesIn_9)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_8 = (((tmpvar_14 * tmpvar_14) * (3.0 - 
    (2.0 * tmpvar_14)
  )) * amplitude_7);
  v_position_4.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_8.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * v_color_5.w));
  highp vec4 tmpvar_15;
  tmpvar_15.w = 0.0;
  tmpvar_15.xyz = _WindDir.xyz;
  v_position_4.xyz = (v_position_4.xyz + (normalize(
    mix ((_World2Object * tmpvar_15).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * v_color_5.z) * waves_8.w)
   + 
    (v_color_5.z * _WindParams.z)
  )));
  highp float tmpvar_16;
  highp vec3 x_17;
  x_17 = (glstate_matrix_modelview0 * v_position_4).xyz;
  tmpvar_16 = sqrt(dot (x_17, x_17));
  fogpos_2 = tmpvar_16;
  tmpvar_3.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_3.x = (1.0 - tmpvar_3.y);
  gl_Position = (glstate_matrix_mvp * v_position_4);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD6 = tmpvar_3;
  xlv_TEXCOORD8 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
uniform lowp float _ShadowBrightening;
uniform lowp vec4 _ShadowColor;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD6;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  lowp float brightness_2;
  mediump vec4 albedoTex_3;
  lowp vec4 c_4;
  c_4.w = 1.0;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0);
  albedoTex_3 = tmpvar_5;
  c_4.xyz = albedoTex_3.xyz;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_7;
  tmpvar_7 = (2.0 * tmpvar_6.xyz);
  mediump float tmpvar_8;
  tmpvar_8 = min (1.0, dot (tmpvar_7, vec3(0.3, 0.59, 0.11)));
  brightness_2 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = vec3(((1.0 - (
    (brightness_2 * brightness_2)
   * brightness_2)) * _ShadowBrightening));
  tmpvar_1 = mix (tmpvar_7, _ShadowColor.xyz, tmpvar_9);
  c_4.xyz = (c_4.xyz * tmpvar_1);
  c_4.w = albedoTex_3.w;
  mediump float x_10;
  x_10 = (c_4.w - _Cutoff);
  if ((x_10 < 0.0)) {
    discard;
  };
  c_4.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_4.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_4;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "LIGHTMAP_ON" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_fd_Type {
	mediump vec3 albedo;
	mediump vec3 specular;
	mediump vec3 shadow;
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	mediump float _Cutoff;
uniform 	lowp float _ShadowBrightening;
uniform 	lowp vec4 _ShadowColor;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD1;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
out highp vec2 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_3;
mediump float t16_7;
highp float t12;
void main()
{
    t0.xyz = _World2Object[1].xyz * _WindDir.yyy;
    t0.xyz = _World2Object[0].xyz * _WindDir.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WindDir.zzz + t0.xyz;
    t1.xyz = (-t0.xyz) + in_NORMAL0.xyz;
    t0.xyz = vec3(_WindDirFromObjNormalOrGlobal) * t1.xyz + t0.xyz;
    t12 = dot(t0.xyz, t0.xyz);
    t12 = inversesqrt(t12);
    t0.xyz = vec3(t12) * t0.xyz;
    t12 = in_COLOR0.y * 0.5 + 0.5;
    t1 = _WindParams.xxxy * _WindFrequency + in_COLOR0.xxxx;
    t1.xyz = t1.xyz * vec3(t12) + in_COLOR0.yyy;
    t2 = fract(t1);
    t1 = t2 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-0.5, -0.5, -0.5, -0.5);
    t1 = fract(t1);
    t1 = t1 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-1.0, -1.0, -1.0, -1.0);
    t2 = abs(t1) * abs(t1);
    t1 = -abs(t1) * vec4(2.0, 2.0, 2.0, 2.0) + vec4(3.0, 3.0, 3.0, 3.0);
    t1 = t1 * t2;
    t1 = t1 * _WindAmplitude;
    t12 = dot(t1.xyz, vec3(1.0, 1.0, 1.0));
    t12 = t12 * _WindParams.w;
    t1.xyz = vec3(t12) * in_NORMAL0.xyz;
    t1.xyz = t1.xyz * in_COLOR0.www + in_POSITION0.xyz;
    t12 = in_COLOR0.z * _WindDir.w;
    t2.x = in_COLOR0.z * _WindParams.z;
    t12 = t12 * t1.w + t2.x;
    t0.xyz = t0.xyz * vec3(t12) + t1.xyz;
    t1 = t0.yyyy * glstate_matrix_mvp[1];
    t1 = glstate_matrix_mvp[0] * t0.xxxx + t1;
    t1 = glstate_matrix_mvp[2] * t0.zzzz + t1;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t1;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t1.xyz = t0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyw = glstate_matrix_modelview0[0].xyz * t0.xxx + t1.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * t0.zzz + t0.xyw;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_3 = (-t0.x) + acid_Fog.y;
    t16_7 = (-acid_Fog.x) + acid_Fog.y;
    t16_3 = t16_3 / t16_7;
    t16_3 = clamp(t16_3, 0.0, 1.0);
    t16_3 = (-t16_3) + 1.0;
    vs_TEXCOORD6.x = (-t16_3) * acid_Fog.z + 1.0;
    t16_3 = t16_3 * acid_Fog.z;
    vs_TEXCOORD6.y = t16_3;
    vs_TEXCOORD8.xy = in_TEXCOORD1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_fd_Type {
	mediump vec3 albedo;
	mediump vec3 specular;
	mediump vec3 shadow;
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	mediump float _Cutoff;
uniform 	lowp float _ShadowBrightening;
uniform 	lowp vec4 _ShadowColor;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D unity_Lightmap;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in highp vec2 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
lowp vec4 t10_0;
mediump vec3 t16_1;
lowp vec3 t10_2;
bool tb2;
lowp vec3 t10_3;
mediump float t16_13;
lowp float t10_15;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_1.x = t10_0.w + (-_Cutoff);
    tb2 = t16_1.x<0.0;
    if((int(tb2) * int(0xffffffffu))!=0){discard;}
    t10_2.xyz = texture(unity_Lightmap, vs_TEXCOORD8.xy).xyz;
    t10_3.xyz = t10_2.xyz * vec3(2.0, 2.0, 2.0);
    t16_1.xyz = vec3(-2.0, -2.0, -2.0) * t10_2.xyz + _ShadowColor.xyz;
    t16_13 = dot(t10_3.xyz, vec3(0.300000012, 0.589999974, 0.109999999));
    t16_13 = min(t16_13, 1.0);
    t10_15 = t16_13 * t16_13;
    t10_15 = (-t10_15) * t16_13 + 1.0;
    t10_15 = t10_15 * _ShadowBrightening;
    t16_1.xyz = vec3(t10_15) * t16_1.xyz + t10_3.xyz;
    t16_1.xyz = t10_0.xyz * t16_1.xyz;
    SV_Target0.w = t10_0.w;
    t16_1.xyz = t16_1.xyz * vs_TEXCOORD6.xxx;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_1.xyz;
    SV_Target0.xyz = t16_1.xyz;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "LIGHTMAP_ON" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _WindParams;
uniform highp vec4 _WindDir;
uniform highp float _WindDirFromObjNormalOrGlobal;
uniform mediump vec4 _WindFrequency;
uniform mediump vec4 _WindAmplitude;
uniform mediump vec4 acid_Fog;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float fogpos_2;
  mediump vec2 tmpvar_3;
  mediump vec4 tmpvar_4;
  tmpvar_3.x = 0.0;
  highp vec4 v_position_5;
  v_position_5.w = _glesVertex.w;
  lowp vec4 v_color_6;
  v_color_6 = tmpvar_1;
  highp vec4 frequency_7;
  frequency_7 = _WindFrequency;
  highp vec4 amplitude_8;
  amplitude_8 = _WindAmplitude;
  highp vec4 waves_9;
  highp vec4 wavesIn_10;
  highp float leafPhase_11;
  highp float objPhase_12;
  lowp float tmpvar_13;
  tmpvar_13 = v_color_6.x;
  objPhase_12 = tmpvar_13;
  lowp float tmpvar_14;
  tmpvar_14 = v_color_6.y;
  leafPhase_11 = tmpvar_14;
  wavesIn_10 = ((_WindParams.xxxy * frequency_7) + vec4(objPhase_12));
  wavesIn_10.xyz = (wavesIn_10.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_11) * vec3(0.5, 0.5, 0.5))));
  wavesIn_10.xyz = (wavesIn_10.xyz + vec3(leafPhase_11));
  highp vec4 tmpvar_15;
  tmpvar_15 = abs(((
    fract((((
      fract(wavesIn_10)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_9 = (((tmpvar_15 * tmpvar_15) * (3.0 - 
    (2.0 * tmpvar_15)
  )) * amplitude_8);
  v_position_5.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_9.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * v_color_6.w));
  highp vec4 tmpvar_16;
  tmpvar_16.w = 0.0;
  tmpvar_16.xyz = _WindDir.xyz;
  v_position_5.xyz = (v_position_5.xyz + (normalize(
    mix ((_World2Object * tmpvar_16).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * v_color_6.z) * waves_9.w)
   + 
    (v_color_6.z * _WindParams.z)
  )));
  highp float tmpvar_17;
  highp vec3 x_18;
  x_18 = (glstate_matrix_modelview0 * v_position_5).xyz;
  tmpvar_17 = sqrt(dot (x_18, x_18));
  fogpos_2 = tmpvar_17;
  tmpvar_3.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_3.x = (1.0 - tmpvar_3.y);
  tmpvar_4 = (unity_World2Shadow[0] * (_Object2World * v_position_5));
  gl_Position = (glstate_matrix_mvp * v_position_5);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD6 = tmpvar_3;
  xlv_TEXCOORD7 = tmpvar_4;
  xlv_TEXCOORD8 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform sampler2D unity_Lightmap;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
uniform lowp vec4 _EnvAmbientColor;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  lowp vec3 lm_2;
  mediump vec4 albedoTex_3;
  lowp vec4 c_4;
  c_4.w = 1.0;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0);
  albedoTex_3 = tmpvar_5;
  c_4.xyz = albedoTex_3.xyz;
  lowp float tmpvar_6;
  highp float tmpvar_7;
  tmpvar_7 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD7).x > 
    (xlv_TEXCOORD7.z / xlv_TEXCOORD7.w)
  )), _LightShadowData.x);
  tmpvar_6 = tmpvar_7;
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_9;
  tmpvar_9 = (2.0 * tmpvar_8.xyz);
  lm_2 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = min (lm_2, mix (_EnvAmbientColor.xyz, lm_2, vec3(tmpvar_6)));
  tmpvar_1 = tmpvar_10;
  c_4.xyz = (c_4.xyz * tmpvar_1);
  c_4.w = albedoTex_3.w;
  mediump float x_11;
  x_11 = (c_4.w - _Cutoff);
  if ((x_11 < 0.0)) {
    discard;
  };
  c_4.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_4.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_4;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _WindParams;
uniform highp vec4 _WindDir;
uniform highp float _WindDirFromObjNormalOrGlobal;
uniform mediump vec4 _WindFrequency;
uniform mediump vec4 _WindAmplitude;
uniform mediump vec4 acid_Fog;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float fogpos_2;
  mediump vec2 tmpvar_3;
  mediump vec4 tmpvar_4;
  tmpvar_3.x = 0.0;
  highp vec4 v_position_5;
  v_position_5.w = _glesVertex.w;
  lowp vec4 v_color_6;
  v_color_6 = tmpvar_1;
  highp vec4 frequency_7;
  frequency_7 = _WindFrequency;
  highp vec4 amplitude_8;
  amplitude_8 = _WindAmplitude;
  highp vec4 waves_9;
  highp vec4 wavesIn_10;
  highp float leafPhase_11;
  highp float objPhase_12;
  lowp float tmpvar_13;
  tmpvar_13 = v_color_6.x;
  objPhase_12 = tmpvar_13;
  lowp float tmpvar_14;
  tmpvar_14 = v_color_6.y;
  leafPhase_11 = tmpvar_14;
  wavesIn_10 = ((_WindParams.xxxy * frequency_7) + vec4(objPhase_12));
  wavesIn_10.xyz = (wavesIn_10.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_11) * vec3(0.5, 0.5, 0.5))));
  wavesIn_10.xyz = (wavesIn_10.xyz + vec3(leafPhase_11));
  highp vec4 tmpvar_15;
  tmpvar_15 = abs(((
    fract((((
      fract(wavesIn_10)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_9 = (((tmpvar_15 * tmpvar_15) * (3.0 - 
    (2.0 * tmpvar_15)
  )) * amplitude_8);
  v_position_5.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_9.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * v_color_6.w));
  highp vec4 tmpvar_16;
  tmpvar_16.w = 0.0;
  tmpvar_16.xyz = _WindDir.xyz;
  v_position_5.xyz = (v_position_5.xyz + (normalize(
    mix ((_World2Object * tmpvar_16).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * v_color_6.z) * waves_9.w)
   + 
    (v_color_6.z * _WindParams.z)
  )));
  highp float tmpvar_17;
  highp vec3 x_18;
  x_18 = (glstate_matrix_modelview0 * v_position_5).xyz;
  tmpvar_17 = sqrt(dot (x_18, x_18));
  fogpos_2 = tmpvar_17;
  tmpvar_3.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_3.x = (1.0 - tmpvar_3.y);
  tmpvar_4 = (unity_World2Shadow[0] * (_Object2World * v_position_5));
  gl_Position = (glstate_matrix_mvp * v_position_5);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD6 = tmpvar_3;
  xlv_TEXCOORD7 = tmpvar_4;
  xlv_TEXCOORD8 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform sampler2D unity_Lightmap;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform mediump float _Cutoff;
uniform lowp vec4 _EnvAmbientColor;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  lowp vec3 lm_2;
  mediump vec4 albedoTex_3;
  lowp vec4 c_4;
  c_4.w = 1.0;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0);
  albedoTex_3 = tmpvar_5;
  c_4.xyz = albedoTex_3.xyz;
  lowp float shadow_6;
  shadow_6 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD7.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_8;
  tmpvar_8 = (2.0 * tmpvar_7.xyz);
  lm_2 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = min (lm_2, mix (_EnvAmbientColor.xyz, lm_2, vec3(shadow_6)));
  tmpvar_1 = tmpvar_9;
  c_4.xyz = (c_4.xyz * tmpvar_1);
  c_4.w = albedoTex_3.w;
  mediump float x_10;
  x_10 = (c_4.w - _Cutoff);
  if ((x_10 < 0.0)) {
    discard;
  };
  c_4.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_4.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_4;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_fd_Type {
	mediump vec3 albedo;
	mediump vec3 specular;
	mediump vec3 shadow;
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	mediump float _Cutoff;
uniform 	lowp float _ShadowBrightening;
uniform 	lowp vec4 _ShadowColor;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD1;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
out mediump vec4 vs_TEXCOORD7;
out highp vec2 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_3;
mediump float t16_7;
highp float t12;
void main()
{
    t0.xyz = _World2Object[1].xyz * _WindDir.yyy;
    t0.xyz = _World2Object[0].xyz * _WindDir.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WindDir.zzz + t0.xyz;
    t1.xyz = (-t0.xyz) + in_NORMAL0.xyz;
    t0.xyz = vec3(_WindDirFromObjNormalOrGlobal) * t1.xyz + t0.xyz;
    t12 = dot(t0.xyz, t0.xyz);
    t12 = inversesqrt(t12);
    t0.xyz = vec3(t12) * t0.xyz;
    t12 = in_COLOR0.y * 0.5 + 0.5;
    t1 = _WindParams.xxxy * _WindFrequency + in_COLOR0.xxxx;
    t1.xyz = t1.xyz * vec3(t12) + in_COLOR0.yyy;
    t2 = fract(t1);
    t1 = t2 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-0.5, -0.5, -0.5, -0.5);
    t1 = fract(t1);
    t1 = t1 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-1.0, -1.0, -1.0, -1.0);
    t2 = abs(t1) * abs(t1);
    t1 = -abs(t1) * vec4(2.0, 2.0, 2.0, 2.0) + vec4(3.0, 3.0, 3.0, 3.0);
    t1 = t1 * t2;
    t1 = t1 * _WindAmplitude;
    t12 = dot(t1.xyz, vec3(1.0, 1.0, 1.0));
    t12 = t12 * _WindParams.w;
    t1.xyz = vec3(t12) * in_NORMAL0.xyz;
    t1.xyz = t1.xyz * in_COLOR0.www + in_POSITION0.xyz;
    t12 = in_COLOR0.z * _WindDir.w;
    t2.x = in_COLOR0.z * _WindParams.z;
    t12 = t12 * t1.w + t2.x;
    t0.xyz = t0.xyz * vec3(t12) + t1.xyz;
    t1 = t0.yyyy * glstate_matrix_mvp[1];
    t1 = glstate_matrix_mvp[0] * t0.xxxx + t1;
    t1 = glstate_matrix_mvp[2] * t0.zzzz + t1;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t1;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t1.xyz = t0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * t0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * t0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t12 = dot(t1.xyz, t1.xyz);
    t12 = sqrt(t12);
    t16_3 = (-t12) + acid_Fog.y;
    t16_7 = (-acid_Fog.x) + acid_Fog.y;
    t16_3 = t16_3 / t16_7;
    t16_3 = clamp(t16_3, 0.0, 1.0);
    t16_3 = (-t16_3) + 1.0;
    t16_7 = t16_3 * acid_Fog.z;
    vs_TEXCOORD6.x = (-t16_3) * acid_Fog.z + 1.0;
    vs_TEXCOORD6.y = t16_7;
    t1 = t0.yyyy * _Object2World[1];
    t1 = _Object2World[0] * t0.xxxx + t1;
    t0 = _Object2World[2] * t0.zzzz + t1;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1 = t0.yyyy * unity_World2Shadow[0][1];
    t1 = unity_World2Shadow[0][0] * t0.xxxx + t1;
    t1 = unity_World2Shadow[0][2] * t0.zzzz + t1;
    t0 = unity_World2Shadow[0][3] * t0.wwww + t1;
    vs_TEXCOORD7 = t0;
    vs_TEXCOORD8.xy = in_TEXCOORD1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_fd_Type {
	mediump vec3 albedo;
	mediump vec3 specular;
	mediump vec3 shadow;
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
uniform 	vec4 _WindParams;
uniform 	vec4 _WindDir;
uniform 	float _WindDirFromObjNormalOrGlobal;
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	mediump float _Cutoff;
uniform 	lowp float _ShadowBrightening;
uniform 	lowp vec4 _ShadowColor;
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D unity_Lightmap;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in mediump vec4 vs_TEXCOORD7;
in highp vec2 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
lowp vec4 t10_0;
mediump vec3 t16_1;
lowp vec3 t10_2;
bool tb2;
lowp vec3 t10_3;
lowp vec3 t10_4;
mediump float t16_6;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_1.x = t10_0.w + (-_Cutoff);
    tb2 = t16_1.x<0.0;
    if((int(tb2) * int(0xffffffffu))!=0){discard;}
    vec3 txVec1 = vec3(vs_TEXCOORD7.xy,vs_TEXCOORD7.z);
    t16_1.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec1, 0.0);
    t16_6 = (-_LightShadowData.x) + 1.0;
    t16_1.x = t16_1.x * t16_6 + _LightShadowData.x;
    t10_2.xyz = texture(unity_Lightmap, vs_TEXCOORD8.xy).xyz;
    t10_3.xyz = vec3(2.0, 2.0, 2.0) * t10_2.xyz + (-_EnvAmbientColor.xyz);
    t10_4.xyz = t10_2.xyz * vec3(2.0, 2.0, 2.0);
    t10_3.xyz = t16_1.xxx * t10_3.xyz + _EnvAmbientColor.xyz;
    t10_3.xyz = min(t10_3.xyz, t10_4.xyz);
    t16_1.xyz = t10_0.xyz * t10_3.xyz;
    SV_Target0.w = t10_0.w;
    t16_1.xyz = t16_1.xyz * vs_TEXCOORD6.xxx;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_1.xyz;
    SV_Target0.xyz = t16_1.xyz;
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "LIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "LIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "LIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "LIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "LIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "LIGHTMAP_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "LIGHTMAP_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "LIGHTMAP_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "LIGHTMAP_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "LIGHTMAP_ON" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" }
"!!GLES3"
}
}
 }
}
Fallback "Hidden/Shadow/WindCutout"
}
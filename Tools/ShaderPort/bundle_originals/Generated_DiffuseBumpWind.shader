Shader "Generated/DiffuseBumpWind" {
Properties {
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
 _BumpMap ("Normalmap", 2D) = "bump" { }
 _WindFrequency ("Flutter Frequency (XYZ) Bend Frequency (W)", Vector) = (1.975,0.793,0.375,0.193)
 _WindAmplitude ("Flutter Amplitude (XYZ) Bend Amplitude (W)", Vector) = (1,1,1,1)
 _WindDirFromObjNormalOrGlobal ("Wind Direction from Global Windsystem or Objectspace Normal", Range(0,1)) = 0
}
SubShader { 
 LOD 499
 Tags { "QUEUE"="Geometry" "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry" "RenderType"="Opaque" }
  GpuProgramID 35649
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "LIGHTMAP_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD6;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  highp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  mediump float fogpos_3;
  mediump vec3 binormal_4;
  mediump vec3 modelLightDir_5;
  mediump vec2 tmpvar_6;
  tmpvar_6.x = 0.0;
  highp vec4 v_position_7;
  v_position_7.w = _glesVertex.w;
  lowp vec4 v_color_8;
  v_color_8 = tmpvar_2;
  highp vec4 frequency_9;
  frequency_9 = _WindFrequency;
  highp vec4 amplitude_10;
  amplitude_10 = _WindAmplitude;
  highp vec4 waves_11;
  highp vec4 wavesIn_12;
  highp float leafPhase_13;
  highp float objPhase_14;
  lowp float tmpvar_15;
  tmpvar_15 = v_color_8.x;
  objPhase_14 = tmpvar_15;
  lowp float tmpvar_16;
  tmpvar_16 = v_color_8.y;
  leafPhase_13 = tmpvar_16;
  wavesIn_12 = ((_WindParams.xxxy * frequency_9) + vec4(objPhase_14));
  wavesIn_12.xyz = (wavesIn_12.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_13) * vec3(0.5, 0.5, 0.5))));
  wavesIn_12.xyz = (wavesIn_12.xyz + vec3(leafPhase_13));
  highp vec4 tmpvar_17;
  tmpvar_17 = abs(((
    fract((((
      fract(wavesIn_12)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_11 = (((tmpvar_17 * tmpvar_17) * (3.0 - 
    (2.0 * tmpvar_17)
  )) * amplitude_10);
  v_position_7.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_11.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * v_color_8.w));
  highp vec4 tmpvar_18;
  tmpvar_18.w = 0.0;
  tmpvar_18.xyz = _WindDir.xyz;
  v_position_7.xyz = (v_position_7.xyz + (normalize(
    mix ((_World2Object * tmpvar_18).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * v_color_8.z) * waves_11.w)
   + 
    (v_color_8.z * _WindParams.z)
  )));
  mediump vec4 tmpvar_19;
  tmpvar_19.w = 0.0;
  tmpvar_19.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_20;
  tmpvar_20 = (_World2Object * tmpvar_19).xyz;
  modelLightDir_5 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  binormal_4 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = _glesTANGENT.xyz;
  highp vec3 tmpvar_23;
  tmpvar_23 = binormal_4;
  mediump mat3 tmpvar_24;
  highp mat3 tmpvar_25;
  tmpvar_25[0].x = tmpvar_22.x;
  tmpvar_25[0].y = tmpvar_23.x;
  tmpvar_25[0].z = tmpvar_1.x;
  tmpvar_25[1].x = tmpvar_22.y;
  tmpvar_25[1].y = tmpvar_23.y;
  tmpvar_25[1].z = tmpvar_1.y;
  tmpvar_25[2].x = tmpvar_22.z;
  tmpvar_25[2].y = tmpvar_23.z;
  tmpvar_25[2].z = tmpvar_1.z;
  tmpvar_24 = tmpvar_25;
  highp float tmpvar_26;
  highp vec3 x_27;
  x_27 = (glstate_matrix_modelview0 * v_position_7).xyz;
  tmpvar_26 = sqrt(dot (x_27, x_27));
  fogpos_3 = tmpvar_26;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_3) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  gl_Position = (glstate_matrix_mvp * v_position_7);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = (tmpvar_24 * modelLightDir_5);
  xlv_TEXCOORD6 = tmpvar_6;
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD6;
void main ()
{
  mediump vec3 normal_1;
  mediump vec4 bumpTex_2;
  mediump vec4 albedoTex_3;
  lowp vec4 c_4;
  c_4.w = 1.0;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0);
  albedoTex_3 = tmpvar_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_BumpMap, xlv_TEXCOORD0);
  bumpTex_2 = tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec4 packednormal_8;
  packednormal_8 = bumpTex_2;
  tmpvar_7 = ((packednormal_8.xyz * 2.0) - 1.0);
  normal_1 = tmpvar_7;
  mediump vec3 tmpvar_9;
  tmpvar_9 = normalize(xlv_TEXCOORD2);
  mediump float tmpvar_10;
  tmpvar_10 = dot (tmpvar_9, normal_1);
  c_4.xyz = albedoTex_3.xyz;
  c_4.xyz = (c_4.xyz * (1.0 + (tmpvar_10 - tmpvar_9.z)));
  c_4.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_4.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_4;
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
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD2;
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
    t1.xyz = in_TANGENT0.yzx * in_NORMAL0.zxy;
    t1.xyz = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t1.xyz);
    t1.xyz = t1.xyz * in_TANGENT0.www;
    t2.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t2.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t2.xyz;
    t2.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t2.xyz;
    vs_TEXCOORD2.y = dot(t1.xyz, t2.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t2.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t2.xyz);
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
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in mediump vec3 vs_TEXCOORD2;
layout(location = 0) out lowp vec4 SV_Target0;
lowp vec3 t10_0;
lowp vec3 t10_1;
mediump vec3 t16_2;
mediump vec3 t16_5;
void main()
{
    t10_0.xyz = texture(_BumpMap, vs_TEXCOORD0.xy).xyz;
    t10_1.xyz = t10_0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_2.x = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_2.x = inversesqrt(t16_2.x);
    t16_5.xyz = t16_2.xxx * vs_TEXCOORD2.xyz;
    t16_5.x = dot(t16_5.xyz, t10_1.xyz);
    t16_2.x = (-vs_TEXCOORD2.z) * t16_2.x + t16_5.x;
    t16_2.x = t16_2.x + 1.0;
    t10_0.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_2.xyz = t16_2.xxx * t10_0.xyz;
    t16_2.xyz = t16_2.xyz * vs_TEXCOORD6.xxx;
    t16_2.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_2.xyz;
    SV_Target0.xyz = t16_2.xyz;
    SV_Target0.w = 1.0;
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
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD6;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  highp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  mediump float fogpos_3;
  mediump vec3 binormal_4;
  mediump vec3 modelLightDir_5;
  mediump vec2 tmpvar_6;
  tmpvar_6.x = 0.0;
  highp vec4 v_position_7;
  v_position_7.w = _glesVertex.w;
  lowp vec4 v_color_8;
  v_color_8 = tmpvar_2;
  highp vec4 frequency_9;
  frequency_9 = _WindFrequency;
  highp vec4 amplitude_10;
  amplitude_10 = _WindAmplitude;
  highp vec4 waves_11;
  highp vec4 wavesIn_12;
  highp float leafPhase_13;
  highp float objPhase_14;
  lowp float tmpvar_15;
  tmpvar_15 = v_color_8.x;
  objPhase_14 = tmpvar_15;
  lowp float tmpvar_16;
  tmpvar_16 = v_color_8.y;
  leafPhase_13 = tmpvar_16;
  wavesIn_12 = ((_WindParams.xxxy * frequency_9) + vec4(objPhase_14));
  wavesIn_12.xyz = (wavesIn_12.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_13) * vec3(0.5, 0.5, 0.5))));
  wavesIn_12.xyz = (wavesIn_12.xyz + vec3(leafPhase_13));
  highp vec4 tmpvar_17;
  tmpvar_17 = abs(((
    fract((((
      fract(wavesIn_12)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_11 = (((tmpvar_17 * tmpvar_17) * (3.0 - 
    (2.0 * tmpvar_17)
  )) * amplitude_10);
  v_position_7.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_11.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * v_color_8.w));
  highp vec4 tmpvar_18;
  tmpvar_18.w = 0.0;
  tmpvar_18.xyz = _WindDir.xyz;
  v_position_7.xyz = (v_position_7.xyz + (normalize(
    mix ((_World2Object * tmpvar_18).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * v_color_8.z) * waves_11.w)
   + 
    (v_color_8.z * _WindParams.z)
  )));
  mediump vec4 tmpvar_19;
  tmpvar_19.w = 0.0;
  tmpvar_19.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_20;
  tmpvar_20 = (_World2Object * tmpvar_19).xyz;
  modelLightDir_5 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  binormal_4 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = _glesTANGENT.xyz;
  highp vec3 tmpvar_23;
  tmpvar_23 = binormal_4;
  mediump mat3 tmpvar_24;
  highp mat3 tmpvar_25;
  tmpvar_25[0].x = tmpvar_22.x;
  tmpvar_25[0].y = tmpvar_23.x;
  tmpvar_25[0].z = tmpvar_1.x;
  tmpvar_25[1].x = tmpvar_22.y;
  tmpvar_25[1].y = tmpvar_23.y;
  tmpvar_25[1].z = tmpvar_1.y;
  tmpvar_25[2].x = tmpvar_22.z;
  tmpvar_25[2].y = tmpvar_23.z;
  tmpvar_25[2].z = tmpvar_1.z;
  tmpvar_24 = tmpvar_25;
  highp float tmpvar_26;
  highp vec3 x_27;
  x_27 = (glstate_matrix_modelview0 * v_position_7).xyz;
  tmpvar_26 = sqrt(dot (x_27, x_27));
  fogpos_3 = tmpvar_26;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_3) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  gl_Position = (glstate_matrix_mvp * v_position_7);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = (tmpvar_24 * modelLightDir_5);
  xlv_TEXCOORD6 = tmpvar_6;
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD6;
void main ()
{
  mediump vec3 normal_1;
  mediump vec4 bumpTex_2;
  mediump vec4 albedoTex_3;
  lowp vec4 c_4;
  c_4.w = 1.0;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0);
  albedoTex_3 = tmpvar_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_BumpMap, xlv_TEXCOORD0);
  bumpTex_2 = tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec4 packednormal_8;
  packednormal_8 = bumpTex_2;
  tmpvar_7 = ((packednormal_8.xyz * 2.0) - 1.0);
  normal_1 = tmpvar_7;
  mediump vec3 tmpvar_9;
  tmpvar_9 = normalize(xlv_TEXCOORD2);
  mediump float tmpvar_10;
  tmpvar_10 = dot (tmpvar_9, normal_1);
  c_4.xyz = albedoTex_3.xyz;
  c_4.xyz = (c_4.xyz * (1.0 + (tmpvar_10 - tmpvar_9.z)));
  c_4.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_4.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_4;
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
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD2;
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
    t1.xyz = in_TANGENT0.yzx * in_NORMAL0.zxy;
    t1.xyz = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t1.xyz);
    t1.xyz = t1.xyz * in_TANGENT0.www;
    t2.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t2.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t2.xyz;
    t2.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t2.xyz;
    vs_TEXCOORD2.y = dot(t1.xyz, t2.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t2.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t2.xyz);
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
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in mediump vec3 vs_TEXCOORD2;
layout(location = 0) out lowp vec4 SV_Target0;
lowp vec3 t10_0;
lowp vec3 t10_1;
mediump vec3 t16_2;
mediump vec3 t16_5;
void main()
{
    t10_0.xyz = texture(_BumpMap, vs_TEXCOORD0.xy).xyz;
    t10_1.xyz = t10_0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_2.x = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_2.x = inversesqrt(t16_2.x);
    t16_5.xyz = t16_2.xxx * vs_TEXCOORD2.xyz;
    t16_5.x = dot(t16_5.xyz, t10_1.xyz);
    t16_2.x = (-vs_TEXCOORD2.z) * t16_2.x + t16_5.x;
    t16_2.x = t16_2.x + 1.0;
    t10_0.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_2.xyz = t16_2.xxx * t10_0.xyz;
    t16_2.xyz = t16_2.xyz * vs_TEXCOORD6.xxx;
    t16_2.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_2.xyz;
    SV_Target0.xyz = t16_2.xyz;
    SV_Target0.w = 1.0;
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
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  highp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  mediump float fogpos_3;
  mediump vec3 binormal_4;
  mediump vec3 modelLightDir_5;
  mediump vec2 tmpvar_6;
  mediump vec4 tmpvar_7;
  tmpvar_6.x = 0.0;
  highp vec4 v_position_8;
  v_position_8.w = _glesVertex.w;
  lowp vec4 v_color_9;
  v_color_9 = tmpvar_2;
  highp vec4 frequency_10;
  frequency_10 = _WindFrequency;
  highp vec4 amplitude_11;
  amplitude_11 = _WindAmplitude;
  highp vec4 waves_12;
  highp vec4 wavesIn_13;
  highp float leafPhase_14;
  highp float objPhase_15;
  lowp float tmpvar_16;
  tmpvar_16 = v_color_9.x;
  objPhase_15 = tmpvar_16;
  lowp float tmpvar_17;
  tmpvar_17 = v_color_9.y;
  leafPhase_14 = tmpvar_17;
  wavesIn_13 = ((_WindParams.xxxy * frequency_10) + vec4(objPhase_15));
  wavesIn_13.xyz = (wavesIn_13.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_14) * vec3(0.5, 0.5, 0.5))));
  wavesIn_13.xyz = (wavesIn_13.xyz + vec3(leafPhase_14));
  highp vec4 tmpvar_18;
  tmpvar_18 = abs(((
    fract((((
      fract(wavesIn_13)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_12 = (((tmpvar_18 * tmpvar_18) * (3.0 - 
    (2.0 * tmpvar_18)
  )) * amplitude_11);
  v_position_8.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_12.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * v_color_9.w));
  highp vec4 tmpvar_19;
  tmpvar_19.w = 0.0;
  tmpvar_19.xyz = _WindDir.xyz;
  v_position_8.xyz = (v_position_8.xyz + (normalize(
    mix ((_World2Object * tmpvar_19).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * v_color_9.z) * waves_12.w)
   + 
    (v_color_9.z * _WindParams.z)
  )));
  mediump vec4 tmpvar_20;
  tmpvar_20.w = 0.0;
  tmpvar_20.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_21;
  tmpvar_21 = (_World2Object * tmpvar_20).xyz;
  modelLightDir_5 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  binormal_4 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = _glesTANGENT.xyz;
  highp vec3 tmpvar_24;
  tmpvar_24 = binormal_4;
  mediump mat3 tmpvar_25;
  highp mat3 tmpvar_26;
  tmpvar_26[0].x = tmpvar_23.x;
  tmpvar_26[0].y = tmpvar_24.x;
  tmpvar_26[0].z = tmpvar_1.x;
  tmpvar_26[1].x = tmpvar_23.y;
  tmpvar_26[1].y = tmpvar_24.y;
  tmpvar_26[1].z = tmpvar_1.y;
  tmpvar_26[2].x = tmpvar_23.z;
  tmpvar_26[2].y = tmpvar_24.z;
  tmpvar_26[2].z = tmpvar_1.z;
  tmpvar_25 = tmpvar_26;
  highp float tmpvar_27;
  highp vec3 x_28;
  x_28 = (glstate_matrix_modelview0 * v_position_8).xyz;
  tmpvar_27 = sqrt(dot (x_28, x_28));
  fogpos_3 = tmpvar_27;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_3) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  tmpvar_7 = (unity_World2Shadow[0] * (_Object2World * v_position_8));
  gl_Position = (glstate_matrix_mvp * v_position_8);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = (tmpvar_25 * modelLightDir_5);
  xlv_TEXCOORD6 = tmpvar_6;
  xlv_TEXCOORD7 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _EnvAmbientColor;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  mediump vec3 normal_2;
  mediump vec4 bumpTex_3;
  mediump vec4 albedoTex_4;
  lowp vec4 c_5;
  c_5.w = 1.0;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0);
  albedoTex_4 = tmpvar_6;
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_BumpMap, xlv_TEXCOORD0);
  bumpTex_3 = tmpvar_7;
  lowp vec3 tmpvar_8;
  lowp vec4 packednormal_9;
  packednormal_9 = bumpTex_3;
  tmpvar_8 = ((packednormal_9.xyz * 2.0) - 1.0);
  normal_2 = tmpvar_8;
  mediump vec3 tmpvar_10;
  tmpvar_10 = normalize(xlv_TEXCOORD2);
  mediump float tmpvar_11;
  tmpvar_11 = dot (tmpvar_10, normal_2);
  c_5.xyz = albedoTex_4.xyz;
  lowp float tmpvar_12;
  highp float tmpvar_13;
  tmpvar_13 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD7).x > 
    (xlv_TEXCOORD7.z / xlv_TEXCOORD7.w)
  )), _LightShadowData.x);
  tmpvar_12 = tmpvar_13;
  tmpvar_1 = ((_EnvAmbientColor.xyz * (1.0 - tmpvar_12)) + tmpvar_12);
  c_5.xyz = (c_5.xyz * tmpvar_1);
  c_5.xyz = (c_5.xyz * (1.0 + (tmpvar_11 - tmpvar_10.z)));
  c_5.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_5.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_5;
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
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  highp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  mediump float fogpos_3;
  mediump vec3 binormal_4;
  mediump vec3 modelLightDir_5;
  mediump vec2 tmpvar_6;
  mediump vec4 tmpvar_7;
  tmpvar_6.x = 0.0;
  highp vec4 v_position_8;
  v_position_8.w = _glesVertex.w;
  lowp vec4 v_color_9;
  v_color_9 = tmpvar_2;
  highp vec4 frequency_10;
  frequency_10 = _WindFrequency;
  highp vec4 amplitude_11;
  amplitude_11 = _WindAmplitude;
  highp vec4 waves_12;
  highp vec4 wavesIn_13;
  highp float leafPhase_14;
  highp float objPhase_15;
  lowp float tmpvar_16;
  tmpvar_16 = v_color_9.x;
  objPhase_15 = tmpvar_16;
  lowp float tmpvar_17;
  tmpvar_17 = v_color_9.y;
  leafPhase_14 = tmpvar_17;
  wavesIn_13 = ((_WindParams.xxxy * frequency_10) + vec4(objPhase_15));
  wavesIn_13.xyz = (wavesIn_13.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_14) * vec3(0.5, 0.5, 0.5))));
  wavesIn_13.xyz = (wavesIn_13.xyz + vec3(leafPhase_14));
  highp vec4 tmpvar_18;
  tmpvar_18 = abs(((
    fract((((
      fract(wavesIn_13)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_12 = (((tmpvar_18 * tmpvar_18) * (3.0 - 
    (2.0 * tmpvar_18)
  )) * amplitude_11);
  v_position_8.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_12.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * v_color_9.w));
  highp vec4 tmpvar_19;
  tmpvar_19.w = 0.0;
  tmpvar_19.xyz = _WindDir.xyz;
  v_position_8.xyz = (v_position_8.xyz + (normalize(
    mix ((_World2Object * tmpvar_19).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * v_color_9.z) * waves_12.w)
   + 
    (v_color_9.z * _WindParams.z)
  )));
  mediump vec4 tmpvar_20;
  tmpvar_20.w = 0.0;
  tmpvar_20.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_21;
  tmpvar_21 = (_World2Object * tmpvar_20).xyz;
  modelLightDir_5 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  binormal_4 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = _glesTANGENT.xyz;
  highp vec3 tmpvar_24;
  tmpvar_24 = binormal_4;
  mediump mat3 tmpvar_25;
  highp mat3 tmpvar_26;
  tmpvar_26[0].x = tmpvar_23.x;
  tmpvar_26[0].y = tmpvar_24.x;
  tmpvar_26[0].z = tmpvar_1.x;
  tmpvar_26[1].x = tmpvar_23.y;
  tmpvar_26[1].y = tmpvar_24.y;
  tmpvar_26[1].z = tmpvar_1.y;
  tmpvar_26[2].x = tmpvar_23.z;
  tmpvar_26[2].y = tmpvar_24.z;
  tmpvar_26[2].z = tmpvar_1.z;
  tmpvar_25 = tmpvar_26;
  highp float tmpvar_27;
  highp vec3 x_28;
  x_28 = (glstate_matrix_modelview0 * v_position_8).xyz;
  tmpvar_27 = sqrt(dot (x_28, x_28));
  fogpos_3 = tmpvar_27;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_3) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  tmpvar_7 = (unity_World2Shadow[0] * (_Object2World * v_position_8));
  gl_Position = (glstate_matrix_mvp * v_position_8);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = (tmpvar_25 * modelLightDir_5);
  xlv_TEXCOORD6 = tmpvar_6;
  xlv_TEXCOORD7 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _EnvAmbientColor;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  mediump vec3 normal_2;
  mediump vec4 bumpTex_3;
  mediump vec4 albedoTex_4;
  lowp vec4 c_5;
  c_5.w = 1.0;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0);
  albedoTex_4 = tmpvar_6;
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_BumpMap, xlv_TEXCOORD0);
  bumpTex_3 = tmpvar_7;
  lowp vec3 tmpvar_8;
  lowp vec4 packednormal_9;
  packednormal_9 = bumpTex_3;
  tmpvar_8 = ((packednormal_9.xyz * 2.0) - 1.0);
  normal_2 = tmpvar_8;
  mediump vec3 tmpvar_10;
  tmpvar_10 = normalize(xlv_TEXCOORD2);
  mediump float tmpvar_11;
  tmpvar_11 = dot (tmpvar_10, normal_2);
  c_5.xyz = albedoTex_4.xyz;
  lowp float shadow_12;
  shadow_12 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD7.xyz) * (1.0 - _LightShadowData.x)));
  tmpvar_1 = ((_EnvAmbientColor.xyz * (1.0 - shadow_12)) + shadow_12);
  c_5.xyz = (c_5.xyz * tmpvar_1);
  c_5.xyz = (c_5.xyz * (1.0 + (tmpvar_11 - tmpvar_10.z)));
  c_5.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_5.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_5;
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
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD2;
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
    t1.xyz = in_TANGENT0.yzx * in_NORMAL0.zxy;
    t1.xyz = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t1.xyz);
    t1.xyz = t1.xyz * in_TANGENT0.www;
    t2.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t2.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t2.xyz;
    t2.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t2.xyz;
    vs_TEXCOORD2.y = dot(t1.xyz, t2.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t2.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t2.xyz);
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
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in mediump vec3 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD7;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
lowp vec3 t10_0;
lowp vec3 t10_1;
mediump vec3 t16_2;
lowp vec3 t10_3;
mediump vec3 t16_6;
mediump float t16_10;
void main()
{
    t10_0.xyz = texture(_BumpMap, vs_TEXCOORD0.xy).xyz;
    t10_1.xyz = t10_0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_2.x = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_2.x = inversesqrt(t16_2.x);
    t16_6.xyz = t16_2.xxx * vs_TEXCOORD2.xyz;
    t16_6.x = dot(t16_6.xyz, t10_1.xyz);
    t16_2.x = (-vs_TEXCOORD2.z) * t16_2.x + t16_6.x;
    t16_2.x = t16_2.x + 1.0;
    vec3 txVec12 = vec3(vs_TEXCOORD7.xy,vs_TEXCOORD7.z);
    t16_6.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec12, 0.0);
    t16_10 = (-_LightShadowData.x) + 1.0;
    t16_6.x = t16_6.x * t16_10 + _LightShadowData.x;
    t16_0.x = (-t16_6.x) + 1.0;
    t16_0.xyz = _EnvAmbientColor.xyz * t16_0.xxx + t16_6.xxx;
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_6.xyz = t16_0.xyz * t10_3.xyz;
    t16_2.xyz = t16_2.xxx * t16_6.xyz;
    t16_2.xyz = t16_2.xyz * vs_TEXCOORD6.xxx;
    t16_2.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_2.xyz;
    SV_Target0.xyz = t16_2.xyz;
    SV_Target0.w = 1.0;
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
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD6;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  highp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  mediump float fogpos_3;
  mediump vec3 binormal_4;
  mediump vec3 modelLightDir_5;
  mediump vec2 tmpvar_6;
  tmpvar_6.x = 0.0;
  highp vec4 v_position_7;
  v_position_7.w = _glesVertex.w;
  lowp vec4 v_color_8;
  v_color_8 = tmpvar_2;
  highp vec4 frequency_9;
  frequency_9 = _WindFrequency;
  highp vec4 amplitude_10;
  amplitude_10 = _WindAmplitude;
  highp vec4 waves_11;
  highp vec4 wavesIn_12;
  highp float leafPhase_13;
  highp float objPhase_14;
  lowp float tmpvar_15;
  tmpvar_15 = v_color_8.x;
  objPhase_14 = tmpvar_15;
  lowp float tmpvar_16;
  tmpvar_16 = v_color_8.y;
  leafPhase_13 = tmpvar_16;
  wavesIn_12 = ((_WindParams.xxxy * frequency_9) + vec4(objPhase_14));
  wavesIn_12.xyz = (wavesIn_12.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_13) * vec3(0.5, 0.5, 0.5))));
  wavesIn_12.xyz = (wavesIn_12.xyz + vec3(leafPhase_13));
  highp vec4 tmpvar_17;
  tmpvar_17 = abs(((
    fract((((
      fract(wavesIn_12)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_11 = (((tmpvar_17 * tmpvar_17) * (3.0 - 
    (2.0 * tmpvar_17)
  )) * amplitude_10);
  v_position_7.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_11.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * v_color_8.w));
  highp vec4 tmpvar_18;
  tmpvar_18.w = 0.0;
  tmpvar_18.xyz = _WindDir.xyz;
  v_position_7.xyz = (v_position_7.xyz + (normalize(
    mix ((_World2Object * tmpvar_18).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * v_color_8.z) * waves_11.w)
   + 
    (v_color_8.z * _WindParams.z)
  )));
  mediump vec4 tmpvar_19;
  tmpvar_19.w = 0.0;
  tmpvar_19.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_20;
  tmpvar_20 = (_World2Object * tmpvar_19).xyz;
  modelLightDir_5 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  binormal_4 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = _glesTANGENT.xyz;
  highp vec3 tmpvar_23;
  tmpvar_23 = binormal_4;
  mediump mat3 tmpvar_24;
  highp mat3 tmpvar_25;
  tmpvar_25[0].x = tmpvar_22.x;
  tmpvar_25[0].y = tmpvar_23.x;
  tmpvar_25[0].z = tmpvar_1.x;
  tmpvar_25[1].x = tmpvar_22.y;
  tmpvar_25[1].y = tmpvar_23.y;
  tmpvar_25[1].z = tmpvar_1.y;
  tmpvar_25[2].x = tmpvar_22.z;
  tmpvar_25[2].y = tmpvar_23.z;
  tmpvar_25[2].z = tmpvar_1.z;
  tmpvar_24 = tmpvar_25;
  highp float tmpvar_26;
  highp vec3 x_27;
  x_27 = (glstate_matrix_modelview0 * v_position_7).xyz;
  tmpvar_26 = sqrt(dot (x_27, x_27));
  fogpos_3 = tmpvar_26;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_3) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  gl_Position = (glstate_matrix_mvp * v_position_7);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = (tmpvar_24 * modelLightDir_5);
  xlv_TEXCOORD6 = tmpvar_6;
  xlv_TEXCOORD8 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD6;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  mediump vec3 normal_1;
  mediump vec4 bumpTex_2;
  mediump vec4 albedoTex_3;
  lowp vec4 c_4;
  c_4.w = 1.0;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0);
  albedoTex_3 = tmpvar_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_BumpMap, xlv_TEXCOORD0);
  bumpTex_2 = tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec4 packednormal_8;
  packednormal_8 = bumpTex_2;
  tmpvar_7 = ((packednormal_8.xyz * 2.0) - 1.0);
  normal_1 = tmpvar_7;
  mediump vec3 tmpvar_9;
  tmpvar_9 = normalize(xlv_TEXCOORD2);
  mediump float tmpvar_10;
  tmpvar_10 = dot (tmpvar_9, normal_1);
  c_4.xyz = albedoTex_3.xyz;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_12;
  tmpvar_12 = (2.0 * tmpvar_11.xyz);
  c_4.xyz = (c_4.xyz * tmpvar_12);
  c_4.xyz = (c_4.xyz * (1.0 + (tmpvar_10 - tmpvar_9.z)));
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
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD1;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD2;
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
    t1.xyz = in_TANGENT0.yzx * in_NORMAL0.zxy;
    t1.xyz = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t1.xyz);
    t1.xyz = t1.xyz * in_TANGENT0.www;
    t2.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t2.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t2.xyz;
    t2.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t2.xyz;
    vs_TEXCOORD2.y = dot(t1.xyz, t2.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t2.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t2.xyz);
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
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D unity_Lightmap;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in mediump vec3 vs_TEXCOORD2;
in highp vec2 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
lowp vec3 t10_0;
lowp vec3 t10_1;
mediump vec3 t16_2;
mediump vec3 t16_5;
void main()
{
    t10_0.xyz = texture(_BumpMap, vs_TEXCOORD0.xy).xyz;
    t10_1.xyz = t10_0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_2.x = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_2.x = inversesqrt(t16_2.x);
    t16_5.xyz = t16_2.xxx * vs_TEXCOORD2.xyz;
    t16_5.x = dot(t16_5.xyz, t10_1.xyz);
    t16_2.x = (-vs_TEXCOORD2.z) * t16_2.x + t16_5.x;
    t16_2.x = t16_2.x + 1.0;
    t10_0.xyz = texture(unity_Lightmap, vs_TEXCOORD8.xy).xyz;
    t10_1.xyz = t10_0.xyz * vec3(2.0, 2.0, 2.0);
    t10_0.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_5.xyz = t10_1.xyz * t10_0.xyz;
    t16_2.xyz = t16_2.xxx * t16_5.xyz;
    t16_2.xyz = t16_2.xyz * vs_TEXCOORD6.xxx;
    t16_2.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_2.xyz;
    SV_Target0.xyz = t16_2.xyz;
    SV_Target0.w = 1.0;
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
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD6;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  highp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  mediump float fogpos_3;
  mediump vec3 binormal_4;
  mediump vec3 modelLightDir_5;
  mediump vec2 tmpvar_6;
  tmpvar_6.x = 0.0;
  highp vec4 v_position_7;
  v_position_7.w = _glesVertex.w;
  lowp vec4 v_color_8;
  v_color_8 = tmpvar_2;
  highp vec4 frequency_9;
  frequency_9 = _WindFrequency;
  highp vec4 amplitude_10;
  amplitude_10 = _WindAmplitude;
  highp vec4 waves_11;
  highp vec4 wavesIn_12;
  highp float leafPhase_13;
  highp float objPhase_14;
  lowp float tmpvar_15;
  tmpvar_15 = v_color_8.x;
  objPhase_14 = tmpvar_15;
  lowp float tmpvar_16;
  tmpvar_16 = v_color_8.y;
  leafPhase_13 = tmpvar_16;
  wavesIn_12 = ((_WindParams.xxxy * frequency_9) + vec4(objPhase_14));
  wavesIn_12.xyz = (wavesIn_12.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_13) * vec3(0.5, 0.5, 0.5))));
  wavesIn_12.xyz = (wavesIn_12.xyz + vec3(leafPhase_13));
  highp vec4 tmpvar_17;
  tmpvar_17 = abs(((
    fract((((
      fract(wavesIn_12)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_11 = (((tmpvar_17 * tmpvar_17) * (3.0 - 
    (2.0 * tmpvar_17)
  )) * amplitude_10);
  v_position_7.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_11.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * v_color_8.w));
  highp vec4 tmpvar_18;
  tmpvar_18.w = 0.0;
  tmpvar_18.xyz = _WindDir.xyz;
  v_position_7.xyz = (v_position_7.xyz + (normalize(
    mix ((_World2Object * tmpvar_18).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * v_color_8.z) * waves_11.w)
   + 
    (v_color_8.z * _WindParams.z)
  )));
  mediump vec4 tmpvar_19;
  tmpvar_19.w = 0.0;
  tmpvar_19.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_20;
  tmpvar_20 = (_World2Object * tmpvar_19).xyz;
  modelLightDir_5 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  binormal_4 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = _glesTANGENT.xyz;
  highp vec3 tmpvar_23;
  tmpvar_23 = binormal_4;
  mediump mat3 tmpvar_24;
  highp mat3 tmpvar_25;
  tmpvar_25[0].x = tmpvar_22.x;
  tmpvar_25[0].y = tmpvar_23.x;
  tmpvar_25[0].z = tmpvar_1.x;
  tmpvar_25[1].x = tmpvar_22.y;
  tmpvar_25[1].y = tmpvar_23.y;
  tmpvar_25[1].z = tmpvar_1.y;
  tmpvar_25[2].x = tmpvar_22.z;
  tmpvar_25[2].y = tmpvar_23.z;
  tmpvar_25[2].z = tmpvar_1.z;
  tmpvar_24 = tmpvar_25;
  highp float tmpvar_26;
  highp vec3 x_27;
  x_27 = (glstate_matrix_modelview0 * v_position_7).xyz;
  tmpvar_26 = sqrt(dot (x_27, x_27));
  fogpos_3 = tmpvar_26;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_3) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  gl_Position = (glstate_matrix_mvp * v_position_7);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = (tmpvar_24 * modelLightDir_5);
  xlv_TEXCOORD6 = tmpvar_6;
  xlv_TEXCOORD8 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD6;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  mediump vec3 normal_1;
  mediump vec4 bumpTex_2;
  mediump vec4 albedoTex_3;
  lowp vec4 c_4;
  c_4.w = 1.0;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0);
  albedoTex_3 = tmpvar_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_BumpMap, xlv_TEXCOORD0);
  bumpTex_2 = tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec4 packednormal_8;
  packednormal_8 = bumpTex_2;
  tmpvar_7 = ((packednormal_8.xyz * 2.0) - 1.0);
  normal_1 = tmpvar_7;
  mediump vec3 tmpvar_9;
  tmpvar_9 = normalize(xlv_TEXCOORD2);
  mediump float tmpvar_10;
  tmpvar_10 = dot (tmpvar_9, normal_1);
  c_4.xyz = albedoTex_3.xyz;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_12;
  tmpvar_12 = (2.0 * tmpvar_11.xyz);
  c_4.xyz = (c_4.xyz * tmpvar_12);
  c_4.xyz = (c_4.xyz * (1.0 + (tmpvar_10 - tmpvar_9.z)));
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
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD1;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD2;
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
    t1.xyz = in_TANGENT0.yzx * in_NORMAL0.zxy;
    t1.xyz = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t1.xyz);
    t1.xyz = t1.xyz * in_TANGENT0.www;
    t2.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t2.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t2.xyz;
    t2.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t2.xyz;
    vs_TEXCOORD2.y = dot(t1.xyz, t2.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t2.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t2.xyz);
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
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D unity_Lightmap;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in mediump vec3 vs_TEXCOORD2;
in highp vec2 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
lowp vec3 t10_0;
lowp vec3 t10_1;
mediump vec3 t16_2;
mediump vec3 t16_5;
void main()
{
    t10_0.xyz = texture(_BumpMap, vs_TEXCOORD0.xy).xyz;
    t10_1.xyz = t10_0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_2.x = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_2.x = inversesqrt(t16_2.x);
    t16_5.xyz = t16_2.xxx * vs_TEXCOORD2.xyz;
    t16_5.x = dot(t16_5.xyz, t10_1.xyz);
    t16_2.x = (-vs_TEXCOORD2.z) * t16_2.x + t16_5.x;
    t16_2.x = t16_2.x + 1.0;
    t10_0.xyz = texture(unity_Lightmap, vs_TEXCOORD8.xy).xyz;
    t10_1.xyz = t10_0.xyz * vec3(2.0, 2.0, 2.0);
    t10_0.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_5.xyz = t10_1.xyz * t10_0.xyz;
    t16_2.xyz = t16_2.xxx * t16_5.xyz;
    t16_2.xyz = t16_2.xyz * vs_TEXCOORD6.xxx;
    t16_2.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_2.xyz;
    SV_Target0.xyz = t16_2.xyz;
    SV_Target0.w = 1.0;
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
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  highp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  mediump float fogpos_3;
  mediump vec3 binormal_4;
  mediump vec3 modelLightDir_5;
  mediump vec2 tmpvar_6;
  mediump vec4 tmpvar_7;
  tmpvar_6.x = 0.0;
  highp vec4 v_position_8;
  v_position_8.w = _glesVertex.w;
  lowp vec4 v_color_9;
  v_color_9 = tmpvar_2;
  highp vec4 frequency_10;
  frequency_10 = _WindFrequency;
  highp vec4 amplitude_11;
  amplitude_11 = _WindAmplitude;
  highp vec4 waves_12;
  highp vec4 wavesIn_13;
  highp float leafPhase_14;
  highp float objPhase_15;
  lowp float tmpvar_16;
  tmpvar_16 = v_color_9.x;
  objPhase_15 = tmpvar_16;
  lowp float tmpvar_17;
  tmpvar_17 = v_color_9.y;
  leafPhase_14 = tmpvar_17;
  wavesIn_13 = ((_WindParams.xxxy * frequency_10) + vec4(objPhase_15));
  wavesIn_13.xyz = (wavesIn_13.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_14) * vec3(0.5, 0.5, 0.5))));
  wavesIn_13.xyz = (wavesIn_13.xyz + vec3(leafPhase_14));
  highp vec4 tmpvar_18;
  tmpvar_18 = abs(((
    fract((((
      fract(wavesIn_13)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_12 = (((tmpvar_18 * tmpvar_18) * (3.0 - 
    (2.0 * tmpvar_18)
  )) * amplitude_11);
  v_position_8.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_12.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * v_color_9.w));
  highp vec4 tmpvar_19;
  tmpvar_19.w = 0.0;
  tmpvar_19.xyz = _WindDir.xyz;
  v_position_8.xyz = (v_position_8.xyz + (normalize(
    mix ((_World2Object * tmpvar_19).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * v_color_9.z) * waves_12.w)
   + 
    (v_color_9.z * _WindParams.z)
  )));
  mediump vec4 tmpvar_20;
  tmpvar_20.w = 0.0;
  tmpvar_20.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_21;
  tmpvar_21 = (_World2Object * tmpvar_20).xyz;
  modelLightDir_5 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  binormal_4 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = _glesTANGENT.xyz;
  highp vec3 tmpvar_24;
  tmpvar_24 = binormal_4;
  mediump mat3 tmpvar_25;
  highp mat3 tmpvar_26;
  tmpvar_26[0].x = tmpvar_23.x;
  tmpvar_26[0].y = tmpvar_24.x;
  tmpvar_26[0].z = tmpvar_1.x;
  tmpvar_26[1].x = tmpvar_23.y;
  tmpvar_26[1].y = tmpvar_24.y;
  tmpvar_26[1].z = tmpvar_1.y;
  tmpvar_26[2].x = tmpvar_23.z;
  tmpvar_26[2].y = tmpvar_24.z;
  tmpvar_26[2].z = tmpvar_1.z;
  tmpvar_25 = tmpvar_26;
  highp float tmpvar_27;
  highp vec3 x_28;
  x_28 = (glstate_matrix_modelview0 * v_position_8).xyz;
  tmpvar_27 = sqrt(dot (x_28, x_28));
  fogpos_3 = tmpvar_27;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_3) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  tmpvar_7 = (unity_World2Shadow[0] * (_Object2World * v_position_8));
  gl_Position = (glstate_matrix_mvp * v_position_8);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = (tmpvar_25 * modelLightDir_5);
  xlv_TEXCOORD6 = tmpvar_6;
  xlv_TEXCOORD7 = tmpvar_7;
  xlv_TEXCOORD8 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform sampler2D unity_Lightmap;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _EnvAmbientColor;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  lowp vec3 lm_2;
  mediump vec3 normal_3;
  mediump vec4 bumpTex_4;
  mediump vec4 albedoTex_5;
  lowp vec4 c_6;
  c_6.w = 1.0;
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0);
  albedoTex_5 = tmpvar_7;
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_BumpMap, xlv_TEXCOORD0);
  bumpTex_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  lowp vec4 packednormal_10;
  packednormal_10 = bumpTex_4;
  tmpvar_9 = ((packednormal_10.xyz * 2.0) - 1.0);
  normal_3 = tmpvar_9;
  mediump vec3 tmpvar_11;
  tmpvar_11 = normalize(xlv_TEXCOORD2);
  mediump float tmpvar_12;
  tmpvar_12 = dot (tmpvar_11, normal_3);
  c_6.xyz = albedoTex_5.xyz;
  lowp float tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD7).x > 
    (xlv_TEXCOORD7.z / xlv_TEXCOORD7.w)
  )), _LightShadowData.x);
  tmpvar_13 = tmpvar_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_16;
  tmpvar_16 = (2.0 * tmpvar_15.xyz);
  lm_2 = tmpvar_16;
  lowp vec3 tmpvar_17;
  tmpvar_17 = min (lm_2, mix (_EnvAmbientColor.xyz, lm_2, vec3(tmpvar_13)));
  tmpvar_1 = tmpvar_17;
  c_6.xyz = (c_6.xyz * tmpvar_1);
  c_6.xyz = (c_6.xyz * (1.0 + (tmpvar_12 - tmpvar_11.z)));
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_6.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_6;
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
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  highp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  mediump float fogpos_3;
  mediump vec3 binormal_4;
  mediump vec3 modelLightDir_5;
  mediump vec2 tmpvar_6;
  mediump vec4 tmpvar_7;
  tmpvar_6.x = 0.0;
  highp vec4 v_position_8;
  v_position_8.w = _glesVertex.w;
  lowp vec4 v_color_9;
  v_color_9 = tmpvar_2;
  highp vec4 frequency_10;
  frequency_10 = _WindFrequency;
  highp vec4 amplitude_11;
  amplitude_11 = _WindAmplitude;
  highp vec4 waves_12;
  highp vec4 wavesIn_13;
  highp float leafPhase_14;
  highp float objPhase_15;
  lowp float tmpvar_16;
  tmpvar_16 = v_color_9.x;
  objPhase_15 = tmpvar_16;
  lowp float tmpvar_17;
  tmpvar_17 = v_color_9.y;
  leafPhase_14 = tmpvar_17;
  wavesIn_13 = ((_WindParams.xxxy * frequency_10) + vec4(objPhase_15));
  wavesIn_13.xyz = (wavesIn_13.xyz * (vec3(0.5, 0.5, 0.5) + (vec3(leafPhase_14) * vec3(0.5, 0.5, 0.5))));
  wavesIn_13.xyz = (wavesIn_13.xyz + vec3(leafPhase_14));
  highp vec4 tmpvar_18;
  tmpvar_18 = abs(((
    fract((((
      fract(wavesIn_13)
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  waves_12 = (((tmpvar_18 * tmpvar_18) * (3.0 - 
    (2.0 * tmpvar_18)
  )) * amplitude_11);
  v_position_8.xyz = (_glesVertex.xyz + ((_glesNormal * 
    (dot (waves_12.xyz, vec3(1.0, 1.0, 1.0)) * _WindParams.w)
  ) * v_color_9.w));
  highp vec4 tmpvar_19;
  tmpvar_19.w = 0.0;
  tmpvar_19.xyz = _WindDir.xyz;
  v_position_8.xyz = (v_position_8.xyz + (normalize(
    mix ((_World2Object * tmpvar_19).xyz, _glesNormal, vec3(_WindDirFromObjNormalOrGlobal))
  ) * (
    ((_WindDir.w * v_color_9.z) * waves_12.w)
   + 
    (v_color_9.z * _WindParams.z)
  )));
  mediump vec4 tmpvar_20;
  tmpvar_20.w = 0.0;
  tmpvar_20.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_21;
  tmpvar_21 = (_World2Object * tmpvar_20).xyz;
  modelLightDir_5 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  binormal_4 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = _glesTANGENT.xyz;
  highp vec3 tmpvar_24;
  tmpvar_24 = binormal_4;
  mediump mat3 tmpvar_25;
  highp mat3 tmpvar_26;
  tmpvar_26[0].x = tmpvar_23.x;
  tmpvar_26[0].y = tmpvar_24.x;
  tmpvar_26[0].z = tmpvar_1.x;
  tmpvar_26[1].x = tmpvar_23.y;
  tmpvar_26[1].y = tmpvar_24.y;
  tmpvar_26[1].z = tmpvar_1.y;
  tmpvar_26[2].x = tmpvar_23.z;
  tmpvar_26[2].y = tmpvar_24.z;
  tmpvar_26[2].z = tmpvar_1.z;
  tmpvar_25 = tmpvar_26;
  highp float tmpvar_27;
  highp vec3 x_28;
  x_28 = (glstate_matrix_modelview0 * v_position_8).xyz;
  tmpvar_27 = sqrt(dot (x_28, x_28));
  fogpos_3 = tmpvar_27;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_3) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  tmpvar_7 = (unity_World2Shadow[0] * (_Object2World * v_position_8));
  gl_Position = (glstate_matrix_mvp * v_position_8);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = (tmpvar_25 * modelLightDir_5);
  xlv_TEXCOORD6 = tmpvar_6;
  xlv_TEXCOORD7 = tmpvar_7;
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
uniform sampler2D _BumpMap;
uniform lowp vec4 _EnvAmbientColor;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  lowp vec3 lm_2;
  mediump vec3 normal_3;
  mediump vec4 bumpTex_4;
  mediump vec4 albedoTex_5;
  lowp vec4 c_6;
  c_6.w = 1.0;
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0);
  albedoTex_5 = tmpvar_7;
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_BumpMap, xlv_TEXCOORD0);
  bumpTex_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  lowp vec4 packednormal_10;
  packednormal_10 = bumpTex_4;
  tmpvar_9 = ((packednormal_10.xyz * 2.0) - 1.0);
  normal_3 = tmpvar_9;
  mediump vec3 tmpvar_11;
  tmpvar_11 = normalize(xlv_TEXCOORD2);
  mediump float tmpvar_12;
  tmpvar_12 = dot (tmpvar_11, normal_3);
  c_6.xyz = albedoTex_5.xyz;
  lowp float shadow_13;
  shadow_13 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD7.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_15;
  tmpvar_15 = (2.0 * tmpvar_14.xyz);
  lm_2 = tmpvar_15;
  lowp vec3 tmpvar_16;
  tmpvar_16 = min (lm_2, mix (_EnvAmbientColor.xyz, lm_2, vec3(shadow_13)));
  tmpvar_1 = tmpvar_16;
  c_6.xyz = (c_6.xyz * tmpvar_1);
  c_6.xyz = (c_6.xyz * (1.0 + (tmpvar_12 - tmpvar_11.z)));
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_6.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_6;
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
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD1;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD2;
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
    t1.xyz = in_TANGENT0.yzx * in_NORMAL0.zxy;
    t1.xyz = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t1.xyz);
    t1.xyz = t1.xyz * in_TANGENT0.www;
    t2.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t2.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t2.xyz;
    t2.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t2.xyz;
    vs_TEXCOORD2.y = dot(t1.xyz, t2.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t2.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t2.xyz);
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
uniform 	mediump vec4 _WindFrequency;
uniform 	mediump vec4 _WindAmplitude;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D unity_Lightmap;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in mediump vec3 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD7;
in highp vec2 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
lowp vec3 t10_1;
lowp vec3 t10_2;
lowp vec3 t10_3;
mediump vec3 t16_4;
mediump float t16_5;
mediump float t16_15;
void main()
{
    vec3 txVec5 = vec3(vs_TEXCOORD7.xy,vs_TEXCOORD7.z);
    t16_0.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec5, 0.0);
    t16_5 = (-_LightShadowData.x) + 1.0;
    t16_0.x = t16_0.x * t16_5 + _LightShadowData.x;
    t10_1.xyz = texture(unity_Lightmap, vs_TEXCOORD8.xy).xyz;
    t10_2.xyz = vec3(2.0, 2.0, 2.0) * t10_1.xyz + (-_EnvAmbientColor.xyz);
    t10_3.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0);
    t10_2.xyz = t16_0.xxx * t10_2.xyz + _EnvAmbientColor.xyz;
    t10_2.xyz = min(t10_2.xyz, t10_3.xyz);
    t10_1.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_0.xyz = t10_2.xyz * t10_1.xyz;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD0.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_15 = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_15 = inversesqrt(t16_15);
    t16_4.xyz = vec3(t16_15) * vs_TEXCOORD2.xyz;
    t16_4.x = dot(t16_4.xyz, t10_2.xyz);
    t16_15 = (-vs_TEXCOORD2.z) * t16_15 + t16_4.x;
    t16_15 = t16_15 + 1.0;
    t16_0.xyz = vec3(t16_15) * t16_0.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD6.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_0.xyz;
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
Fallback "Generated/DiffuseWind"
}
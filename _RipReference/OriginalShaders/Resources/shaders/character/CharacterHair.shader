Shader "Character/CharacterHair" {
Properties {
 _MainTex ("Albedo (RGB) Alpha (A)", 2D) = "white" { }
 _BumpMap ("Normalmap", 2D) = "bump" { }
}
SubShader { 
 LOD 99
 Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Transparent" "RenderType"="Transparent" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Transparent" "RenderType"="Transparent" }
  GpuProgramID 53219
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
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
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
mediump vec3 tmpvar_1;
mediump vec3 tmpvar_2;
mediump vec3 tmpvar_3;
mediump float tmpvar_4;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_5;
  mediump vec3 lit_color_6;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_2 = tmpvar_7.xyz;
  mediump vec3 tangentNormal_8;
  mediump vec3 tmpvar_9;
  tmpvar_9.x = xlv_TEXCOORD3.w;
  tmpvar_9.y = xlv_TEXCOORD4.w;
  tmpvar_9.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_10;
  mediump vec3 tmpvar_11;
  tmpvar_10 = xlv_TEXCOORD5.xyz;
  tmpvar_11 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_9.x;
  tmpvar_12[0].z = tmpvar_11.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_9.y;
  tmpvar_12[1].z = tmpvar_11.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_9.z;
  tmpvar_12[2].z = tmpvar_11.z;
  lowp vec3 tmpvar_13;
  tmpvar_13 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_8 = tmpvar_13;
  tmpvar_3 = (tangentNormal_8 * tmpvar_12);
  mediump vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_3;
  mediump vec3 x2_15;
  mediump vec3 x1_16;
  x1_16.x = dot (unity_SHAr, tmpvar_14);
  x1_16.y = dot (unity_SHAg, tmpvar_14);
  x1_16.z = dot (unity_SHAb, tmpvar_14);
  mediump vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_3.xyzz * tmpvar_3.yzzx);
  x2_15.x = dot (unity_SHBr, tmpvar_17);
  x2_15.y = dot (unity_SHBg, tmpvar_17);
  x2_15.z = dot (unity_SHBb, tmpvar_17);
  tmpvar_4 = ((max (
    dot (tmpvar_3, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_6 = ((max (
    ((1.055 * pow ((
      ((x2_15 + (unity_SHC.xyz * (
        (tmpvar_3.x * tmpvar_3.x)
       - 
        (tmpvar_3.y * tmpvar_3.y)
      ))) + x1_16)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_2) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_4), vec3(min (1.0, 
    (tmpvar_4 * 1.5)
  ))));
  mediump vec3 tmpvar_18;
  tmpvar_18 = mix (lit_color_6, vec3((dot (lit_color_6, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_4 * _LightParameters.y)));
  c_5.xyz = tmpvar_18;
  c_5.w = _RimColor.w;
  c_5.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (c_5.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_5;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_OFF" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
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
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec3 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp vec3 t10_5;
mediump vec3 t16_6;
mediump vec3 t16_9;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_6.xyz = t16_3.xyz + t16_4.xyz;
    t16_6.xyz = t16_6.xyz * _LightParameters.xxx;
    t16_6.xyz = log2(t16_6.xyz);
    t16_6.xyz = t16_6.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_6.xyz = exp2(t16_6.xyz);
    t16_0.yzw = t16_6.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0 = max(t16_0, vec4(0.0, 0.0, 0.0, 0.0));
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_6.xyz = vec3(t16_0.y * t10_5.x, t16_0.z * t10_5.y, t16_0.w * t10_5.z);
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_3.x = t16_0.x * 1.5;
    t16_3.x = min(t16_3.x, 1.0);
    t16_9.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_3.xyz = t16_3.xxx * t16_9.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_6.xyz * t16_3.xyz;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
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
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
mediump vec3 tmpvar_1;
mediump vec3 tmpvar_2;
mediump vec3 tmpvar_3;
mediump float tmpvar_4;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_5;
  mediump vec3 lit_color_6;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_2 = tmpvar_7.xyz;
  mediump vec3 tangentNormal_8;
  mediump vec3 tmpvar_9;
  tmpvar_9.x = xlv_TEXCOORD3.w;
  tmpvar_9.y = xlv_TEXCOORD4.w;
  tmpvar_9.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_10;
  mediump vec3 tmpvar_11;
  tmpvar_10 = xlv_TEXCOORD5.xyz;
  tmpvar_11 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_9.x;
  tmpvar_12[0].z = tmpvar_11.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_9.y;
  tmpvar_12[1].z = tmpvar_11.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_9.z;
  tmpvar_12[2].z = tmpvar_11.z;
  lowp vec3 tmpvar_13;
  tmpvar_13 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_8 = tmpvar_13;
  tmpvar_3 = (tangentNormal_8 * tmpvar_12);
  mediump vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_3;
  mediump vec3 x2_15;
  mediump vec3 x1_16;
  x1_16.x = dot (unity_SHAr, tmpvar_14);
  x1_16.y = dot (unity_SHAg, tmpvar_14);
  x1_16.z = dot (unity_SHAb, tmpvar_14);
  mediump vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_3.xyzz * tmpvar_3.yzzx);
  x2_15.x = dot (unity_SHBr, tmpvar_17);
  x2_15.y = dot (unity_SHBg, tmpvar_17);
  x2_15.z = dot (unity_SHBb, tmpvar_17);
  tmpvar_4 = ((max (
    dot (tmpvar_3, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_6 = ((max (
    ((1.055 * pow ((
      ((x2_15 + (unity_SHC.xyz * (
        (tmpvar_3.x * tmpvar_3.x)
       - 
        (tmpvar_3.y * tmpvar_3.y)
      ))) + x1_16)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_2) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_4), vec3(min (1.0, 
    (tmpvar_4 * 1.5)
  ))));
  mediump vec3 tmpvar_18;
  tmpvar_18 = mix (lit_color_6, vec3((dot (lit_color_6, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_4 * _LightParameters.y)));
  c_5.xyz = tmpvar_18;
  c_5.w = _RimColor.w;
  c_5.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (c_5.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_5;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
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
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec3 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp vec3 t10_5;
mediump vec3 t16_6;
mediump vec3 t16_9;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_6.xyz = t16_3.xyz + t16_4.xyz;
    t16_6.xyz = t16_6.xyz * _LightParameters.xxx;
    t16_6.xyz = log2(t16_6.xyz);
    t16_6.xyz = t16_6.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_6.xyz = exp2(t16_6.xyz);
    t16_0.yzw = t16_6.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0 = max(t16_0, vec4(0.0, 0.0, 0.0, 0.0));
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_6.xyz = vec3(t16_0.y * t10_5.x, t16_0.z * t10_5.y, t16_0.w * t10_5.z);
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_3.x = t16_0.x * 1.5;
    t16_3.x = min(t16_3.x, 1.0);
    t16_9.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_3.xyz = t16_3.xxx * t16_9.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_6.xyz * t16_3.xyz;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
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
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
mediump vec3 tmpvar_1;
mediump vec3 tmpvar_2;
mediump vec3 tmpvar_3;
mediump float tmpvar_4;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_5;
  mediump vec3 lit_color_6;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_2 = tmpvar_7.xyz;
  mediump vec3 tangentNormal_8;
  mediump vec3 tmpvar_9;
  tmpvar_9.x = xlv_TEXCOORD3.w;
  tmpvar_9.y = xlv_TEXCOORD4.w;
  tmpvar_9.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_10;
  mediump vec3 tmpvar_11;
  tmpvar_10 = xlv_TEXCOORD5.xyz;
  tmpvar_11 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_9.x;
  tmpvar_12[0].z = tmpvar_11.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_9.y;
  tmpvar_12[1].z = tmpvar_11.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_9.z;
  tmpvar_12[2].z = tmpvar_11.z;
  lowp vec3 tmpvar_13;
  tmpvar_13 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_8 = tmpvar_13;
  tmpvar_3 = (tangentNormal_8 * tmpvar_12);
  mediump vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_3;
  mediump vec3 x2_15;
  mediump vec3 x1_16;
  x1_16.x = dot (unity_SHAr, tmpvar_14);
  x1_16.y = dot (unity_SHAg, tmpvar_14);
  x1_16.z = dot (unity_SHAb, tmpvar_14);
  mediump vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_3.xyzz * tmpvar_3.yzzx);
  x2_15.x = dot (unity_SHBr, tmpvar_17);
  x2_15.y = dot (unity_SHBg, tmpvar_17);
  x2_15.z = dot (unity_SHBb, tmpvar_17);
  tmpvar_4 = ((max (
    dot (tmpvar_3, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_6 = ((max (
    ((1.055 * pow ((
      ((x2_15 + (unity_SHC.xyz * (
        (tmpvar_3.x * tmpvar_3.x)
       - 
        (tmpvar_3.y * tmpvar_3.y)
      ))) + x1_16)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_2) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_4), vec3(min (1.0, 
    (tmpvar_4 * 1.5)
  ))));
  mediump vec3 atten_18;
  lowp float tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_19 = tmpvar_20;
  lowp vec3 tmpvar_21;
  tmpvar_21 = vec3(tmpvar_19);
  atten_18 = tmpvar_21;
  lit_color_6 = (lit_color_6 * max ((_Shadows.x + 
    (atten_18 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_22;
  tmpvar_22 = mix (lit_color_6, vec3((dot (lit_color_6, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_4 * _LightParameters.y)));
  c_5.xyz = tmpvar_22;
  c_5.w = _RimColor.w;
  c_5.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (c_5.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_5;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
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
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
mediump vec3 tmpvar_1;
mediump vec3 tmpvar_2;
mediump vec3 tmpvar_3;
mediump float tmpvar_4;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_5;
  mediump vec3 lit_color_6;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_2 = tmpvar_7.xyz;
  mediump vec3 tangentNormal_8;
  mediump vec3 tmpvar_9;
  tmpvar_9.x = xlv_TEXCOORD3.w;
  tmpvar_9.y = xlv_TEXCOORD4.w;
  tmpvar_9.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_10;
  mediump vec3 tmpvar_11;
  tmpvar_10 = xlv_TEXCOORD5.xyz;
  tmpvar_11 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_9.x;
  tmpvar_12[0].z = tmpvar_11.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_9.y;
  tmpvar_12[1].z = tmpvar_11.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_9.z;
  tmpvar_12[2].z = tmpvar_11.z;
  lowp vec3 tmpvar_13;
  tmpvar_13 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_8 = tmpvar_13;
  tmpvar_3 = (tangentNormal_8 * tmpvar_12);
  mediump vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_3;
  mediump vec3 x2_15;
  mediump vec3 x1_16;
  x1_16.x = dot (unity_SHAr, tmpvar_14);
  x1_16.y = dot (unity_SHAg, tmpvar_14);
  x1_16.z = dot (unity_SHAb, tmpvar_14);
  mediump vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_3.xyzz * tmpvar_3.yzzx);
  x2_15.x = dot (unity_SHBr, tmpvar_17);
  x2_15.y = dot (unity_SHBg, tmpvar_17);
  x2_15.z = dot (unity_SHBb, tmpvar_17);
  tmpvar_4 = ((max (
    dot (tmpvar_3, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_6 = ((max (
    ((1.055 * pow ((
      ((x2_15 + (unity_SHC.xyz * (
        (tmpvar_3.x * tmpvar_3.x)
       - 
        (tmpvar_3.y * tmpvar_3.y)
      ))) + x1_16)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_2) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_4), vec3(min (1.0, 
    (tmpvar_4 * 1.5)
  ))));
  mediump vec3 atten_18;
  lowp float shadow_19;
  shadow_19 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_20;
  tmpvar_20 = vec3(shadow_19);
  atten_18 = tmpvar_20;
  lit_color_6 = (lit_color_6 * max ((_Shadows.x + 
    (atten_18 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_21;
  tmpvar_21 = mix (lit_color_6, vec3((dot (lit_color_6, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_4 * _LightParameters.y)));
  c_5.xyz = tmpvar_21;
  c_5.w = _RimColor.w;
  c_5.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (c_5.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_5;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
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
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec3 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp vec3 t10_5;
mediump vec3 t16_6;
mediump vec3 t16_9;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_6.xyz = t16_3.xyz + t16_4.xyz;
    t16_6.xyz = t16_6.xyz * _LightParameters.xxx;
    t16_6.xyz = log2(t16_6.xyz);
    t16_6.xyz = t16_6.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_6.xyz = exp2(t16_6.xyz);
    t16_0.yzw = t16_6.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0 = max(t16_0, vec4(0.0, 0.0, 0.0, 0.0));
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_6.xyz = vec3(t16_0.y * t10_5.x, t16_0.z * t10_5.y, t16_0.w * t10_5.z);
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_3.x = t16_0.x * 1.5;
    t16_3.x = min(t16_3.x, 1.0);
    t16_9.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_3.xyz = t16_3.xxx * t16_9.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_6.xyz * t16_3.xyz;
    vec3 txVec11 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_3.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec11, 0.0);
    t16_9.x = (-_LightShadowData.x) + 1.0;
    t16_3.x = t16_3.x * t16_9.x + _LightShadowData.x;
    t16_3.x = t16_3.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_3.x = max(t16_3.x, 0.0);
    t16_6.xyz = t16_6.xyz * t16_3.xxx;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
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
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump vec3 tmpvar_2;
mediump vec3 tmpvar_3;
mediump float tmpvar_4;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_5;
  mediump vec3 lit_color_6;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_2 = tmpvar_7.xyz;
  mediump vec3 tangentNormal_8;
  mediump vec3 tmpvar_9;
  tmpvar_9.x = xlv_TEXCOORD3.w;
  tmpvar_9.y = xlv_TEXCOORD4.w;
  tmpvar_9.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_10;
  mediump vec3 tmpvar_11;
  tmpvar_10 = xlv_TEXCOORD5.xyz;
  tmpvar_11 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_9.x;
  tmpvar_12[0].z = tmpvar_11.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_9.y;
  tmpvar_12[1].z = tmpvar_11.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_9.z;
  tmpvar_12[2].z = tmpvar_11.z;
  lowp vec3 tmpvar_13;
  tmpvar_13 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_8 = tmpvar_13;
  tmpvar_3 = (tangentNormal_8 * tmpvar_12);
  mediump vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_3;
  mediump vec3 x2_15;
  mediump vec3 x1_16;
  x1_16.x = dot (unity_SHAr, tmpvar_14);
  x1_16.y = dot (unity_SHAg, tmpvar_14);
  x1_16.z = dot (unity_SHAb, tmpvar_14);
  mediump vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_3.xyzz * tmpvar_3.yzzx);
  x2_15.x = dot (unity_SHBr, tmpvar_17);
  x2_15.y = dot (unity_SHBg, tmpvar_17);
  x2_15.z = dot (unity_SHBb, tmpvar_17);
  tmpvar_4 = ((max (
    dot (tmpvar_3, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_6 = ((max (
    ((1.055 * pow ((
      ((x2_15 + (unity_SHC.xyz * (
        (tmpvar_3.x * tmpvar_3.x)
       - 
        (tmpvar_3.y * tmpvar_3.y)
      ))) + x1_16)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_2) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_4), vec3(min (1.0, 
    (tmpvar_4 * 1.5)
  ))));
  mediump vec3 tmpvar_18;
  tmpvar_18 = mix (lit_color_6, vec3((dot (lit_color_6, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_4 * _LightParameters.y)));
  c_5.xyz = tmpvar_18;
  c_5.w = _RimColor.w;
  lowp vec4 _color_19;
  _color_19.w = c_5.w;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_21;
  tmpvar_21 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_20.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_19.xyz = (_NoiseGlow.xyz * (tmpvar_21 + c_5.xyz));
  c_5.w = _color_19.w;
  c_5.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_19.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_5;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
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
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _NoiseTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec3 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp vec3 t10_5;
mediump vec3 t16_6;
mediump vec3 t16_9;
mediump float t16_18;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_6.xyz = t16_3.xyz + t16_4.xyz;
    t16_6.xyz = t16_6.xyz * _LightParameters.xxx;
    t16_6.xyz = log2(t16_6.xyz);
    t16_6.xyz = t16_6.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_6.xyz = exp2(t16_6.xyz);
    t16_0.yzw = t16_6.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0 = max(t16_0, vec4(0.0, 0.0, 0.0, 0.0));
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_6.xyz = vec3(t16_0.y * t10_5.x, t16_0.z * t10_5.y, t16_0.w * t10_5.z);
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_3.x = t16_0.x * 1.5;
    t16_3.x = min(t16_3.x, 1.0);
    t16_9.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_3.xyz = t16_3.xxx * t16_9.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_6.xyz * t16_3.xyz;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t10_5.x = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_18 = t10_5.x * 0.9375 + 0.03125;
    t16_18 = (-t16_18) + _NoiseGlow.w;
    t16_18 = -abs(t16_18) * 32.0 + 1.0;
    t16_18 = max(t16_18, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_18);
    t16_0.xyz = t16_0.xyz * _NoiseGlow.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
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
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump vec3 tmpvar_2;
mediump vec3 tmpvar_3;
mediump float tmpvar_4;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_5;
  mediump vec3 lit_color_6;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_2 = tmpvar_7.xyz;
  mediump vec3 tangentNormal_8;
  mediump vec3 tmpvar_9;
  tmpvar_9.x = xlv_TEXCOORD3.w;
  tmpvar_9.y = xlv_TEXCOORD4.w;
  tmpvar_9.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_10;
  mediump vec3 tmpvar_11;
  tmpvar_10 = xlv_TEXCOORD5.xyz;
  tmpvar_11 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_9.x;
  tmpvar_12[0].z = tmpvar_11.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_9.y;
  tmpvar_12[1].z = tmpvar_11.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_9.z;
  tmpvar_12[2].z = tmpvar_11.z;
  lowp vec3 tmpvar_13;
  tmpvar_13 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_8 = tmpvar_13;
  tmpvar_3 = (tangentNormal_8 * tmpvar_12);
  mediump vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_3;
  mediump vec3 x2_15;
  mediump vec3 x1_16;
  x1_16.x = dot (unity_SHAr, tmpvar_14);
  x1_16.y = dot (unity_SHAg, tmpvar_14);
  x1_16.z = dot (unity_SHAb, tmpvar_14);
  mediump vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_3.xyzz * tmpvar_3.yzzx);
  x2_15.x = dot (unity_SHBr, tmpvar_17);
  x2_15.y = dot (unity_SHBg, tmpvar_17);
  x2_15.z = dot (unity_SHBb, tmpvar_17);
  tmpvar_4 = ((max (
    dot (tmpvar_3, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_6 = ((max (
    ((1.055 * pow ((
      ((x2_15 + (unity_SHC.xyz * (
        (tmpvar_3.x * tmpvar_3.x)
       - 
        (tmpvar_3.y * tmpvar_3.y)
      ))) + x1_16)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_2) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_4), vec3(min (1.0, 
    (tmpvar_4 * 1.5)
  ))));
  mediump vec3 tmpvar_18;
  tmpvar_18 = mix (lit_color_6, vec3((dot (lit_color_6, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_4 * _LightParameters.y)));
  c_5.xyz = tmpvar_18;
  c_5.w = _RimColor.w;
  lowp vec4 _color_19;
  _color_19.w = c_5.w;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_21;
  tmpvar_21 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_20.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_19.xyz = (_NoiseGlow.xyz * (tmpvar_21 + c_5.xyz));
  c_5.w = _color_19.w;
  c_5.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_19.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_5;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
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
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _NoiseTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec3 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp vec3 t10_5;
mediump vec3 t16_6;
mediump vec3 t16_9;
mediump float t16_18;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_6.xyz = t16_3.xyz + t16_4.xyz;
    t16_6.xyz = t16_6.xyz * _LightParameters.xxx;
    t16_6.xyz = log2(t16_6.xyz);
    t16_6.xyz = t16_6.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_6.xyz = exp2(t16_6.xyz);
    t16_0.yzw = t16_6.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0 = max(t16_0, vec4(0.0, 0.0, 0.0, 0.0));
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_6.xyz = vec3(t16_0.y * t10_5.x, t16_0.z * t10_5.y, t16_0.w * t10_5.z);
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_3.x = t16_0.x * 1.5;
    t16_3.x = min(t16_3.x, 1.0);
    t16_9.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_3.xyz = t16_3.xxx * t16_9.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_6.xyz * t16_3.xyz;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t10_5.x = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_18 = t10_5.x * 0.9375 + 0.03125;
    t16_18 = (-t16_18) + _NoiseGlow.w;
    t16_18 = -abs(t16_18) * 32.0 + 1.0;
    t16_18 = max(t16_18, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_18);
    t16_0.xyz = t16_0.xyz * _NoiseGlow.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_NOISEGLOW" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
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
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump vec3 tmpvar_2;
mediump vec3 tmpvar_3;
mediump float tmpvar_4;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_5;
  mediump vec3 lit_color_6;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_2 = tmpvar_7.xyz;
  mediump vec3 tangentNormal_8;
  mediump vec3 tmpvar_9;
  tmpvar_9.x = xlv_TEXCOORD3.w;
  tmpvar_9.y = xlv_TEXCOORD4.w;
  tmpvar_9.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_10;
  mediump vec3 tmpvar_11;
  tmpvar_10 = xlv_TEXCOORD5.xyz;
  tmpvar_11 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_9.x;
  tmpvar_12[0].z = tmpvar_11.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_9.y;
  tmpvar_12[1].z = tmpvar_11.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_9.z;
  tmpvar_12[2].z = tmpvar_11.z;
  lowp vec3 tmpvar_13;
  tmpvar_13 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_8 = tmpvar_13;
  tmpvar_3 = (tangentNormal_8 * tmpvar_12);
  mediump vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_3;
  mediump vec3 x2_15;
  mediump vec3 x1_16;
  x1_16.x = dot (unity_SHAr, tmpvar_14);
  x1_16.y = dot (unity_SHAg, tmpvar_14);
  x1_16.z = dot (unity_SHAb, tmpvar_14);
  mediump vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_3.xyzz * tmpvar_3.yzzx);
  x2_15.x = dot (unity_SHBr, tmpvar_17);
  x2_15.y = dot (unity_SHBg, tmpvar_17);
  x2_15.z = dot (unity_SHBb, tmpvar_17);
  tmpvar_4 = ((max (
    dot (tmpvar_3, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_6 = ((max (
    ((1.055 * pow ((
      ((x2_15 + (unity_SHC.xyz * (
        (tmpvar_3.x * tmpvar_3.x)
       - 
        (tmpvar_3.y * tmpvar_3.y)
      ))) + x1_16)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_2) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_4), vec3(min (1.0, 
    (tmpvar_4 * 1.5)
  ))));
  mediump vec3 atten_18;
  lowp float tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_19 = tmpvar_20;
  lowp vec3 tmpvar_21;
  tmpvar_21 = vec3(tmpvar_19);
  atten_18 = tmpvar_21;
  lit_color_6 = (lit_color_6 * max ((_Shadows.x + 
    (atten_18 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_22;
  tmpvar_22 = mix (lit_color_6, vec3((dot (lit_color_6, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_4 * _LightParameters.y)));
  c_5.xyz = tmpvar_22;
  c_5.w = _RimColor.w;
  lowp vec4 _color_23;
  _color_23.w = c_5.w;
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_24.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_23.xyz = (_NoiseGlow.xyz * (tmpvar_25 + c_5.xyz));
  c_5.w = _color_23.w;
  c_5.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_23.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_5;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
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
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump vec3 tmpvar_2;
mediump vec3 tmpvar_3;
mediump float tmpvar_4;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_5;
  mediump vec3 lit_color_6;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_2 = tmpvar_7.xyz;
  mediump vec3 tangentNormal_8;
  mediump vec3 tmpvar_9;
  tmpvar_9.x = xlv_TEXCOORD3.w;
  tmpvar_9.y = xlv_TEXCOORD4.w;
  tmpvar_9.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_10;
  mediump vec3 tmpvar_11;
  tmpvar_10 = xlv_TEXCOORD5.xyz;
  tmpvar_11 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_9.x;
  tmpvar_12[0].z = tmpvar_11.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_9.y;
  tmpvar_12[1].z = tmpvar_11.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_9.z;
  tmpvar_12[2].z = tmpvar_11.z;
  lowp vec3 tmpvar_13;
  tmpvar_13 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_8 = tmpvar_13;
  tmpvar_3 = (tangentNormal_8 * tmpvar_12);
  mediump vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_3;
  mediump vec3 x2_15;
  mediump vec3 x1_16;
  x1_16.x = dot (unity_SHAr, tmpvar_14);
  x1_16.y = dot (unity_SHAg, tmpvar_14);
  x1_16.z = dot (unity_SHAb, tmpvar_14);
  mediump vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_3.xyzz * tmpvar_3.yzzx);
  x2_15.x = dot (unity_SHBr, tmpvar_17);
  x2_15.y = dot (unity_SHBg, tmpvar_17);
  x2_15.z = dot (unity_SHBb, tmpvar_17);
  tmpvar_4 = ((max (
    dot (tmpvar_3, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_6 = ((max (
    ((1.055 * pow ((
      ((x2_15 + (unity_SHC.xyz * (
        (tmpvar_3.x * tmpvar_3.x)
       - 
        (tmpvar_3.y * tmpvar_3.y)
      ))) + x1_16)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_2) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_4), vec3(min (1.0, 
    (tmpvar_4 * 1.5)
  ))));
  mediump vec3 atten_18;
  lowp float shadow_19;
  shadow_19 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_20;
  tmpvar_20 = vec3(shadow_19);
  atten_18 = tmpvar_20;
  lit_color_6 = (lit_color_6 * max ((_Shadows.x + 
    (atten_18 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_21;
  tmpvar_21 = mix (lit_color_6, vec3((dot (lit_color_6, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_4 * _LightParameters.y)));
  c_5.xyz = tmpvar_21;
  c_5.w = _RimColor.w;
  lowp vec4 _color_22;
  _color_22.w = c_5.w;
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_24;
  tmpvar_24 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_23.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_22.xyz = (_NoiseGlow.xyz * (tmpvar_24 + c_5.xyz));
  c_5.w = _color_22.w;
  c_5.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_22.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_5;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
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
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _NoiseTexture;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec3 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp vec3 t10_5;
mediump vec3 t16_6;
mediump vec3 t16_9;
mediump float t16_18;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_6.xyz = t16_3.xyz + t16_4.xyz;
    t16_6.xyz = t16_6.xyz * _LightParameters.xxx;
    t16_6.xyz = log2(t16_6.xyz);
    t16_6.xyz = t16_6.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_6.xyz = exp2(t16_6.xyz);
    t16_0.yzw = t16_6.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0 = max(t16_0, vec4(0.0, 0.0, 0.0, 0.0));
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_6.xyz = vec3(t16_0.y * t10_5.x, t16_0.z * t10_5.y, t16_0.w * t10_5.z);
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_3.x = t16_0.x * 1.5;
    t16_3.x = min(t16_3.x, 1.0);
    t16_9.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_3.xyz = t16_3.xxx * t16_9.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_6.xyz * t16_3.xyz;
    vec3 txVec19 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_3.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec19, 0.0);
    t16_9.x = (-_LightShadowData.x) + 1.0;
    t16_3.x = t16_3.x * t16_9.x + _LightShadowData.x;
    t16_3.x = t16_3.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_3.x = max(t16_3.x, 0.0);
    t16_6.xyz = t16_6.xyz * t16_3.xxx;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t10_5.x = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_18 = t10_5.x * 0.9375 + 0.03125;
    t16_18 = (-t16_18) + _NoiseGlow.w;
    t16_18 = -abs(t16_18) * 32.0 + 1.0;
    t16_18 = max(t16_18, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_18);
    t16_0.xyz = t16_0.xyz * _NoiseGlow.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
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
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
mediump vec3 tmpvar_1;
mediump vec3 tmpvar_2;
mediump vec3 tmpvar_3;
mediump float tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_2 = tmpvar_8.xyz;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_3 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_3;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_3.xyzz * tmpvar_3.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  tmpvar_5 = (1.0 - dot (tmpvar_3, xlv_TEXCOORD3.xyz));
  tmpvar_4 = ((max (
    dot (tmpvar_3, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = ((max (
    ((1.055 * pow ((
      ((x2_16 + (unity_SHC.xyz * (
        (tmpvar_3.x * tmpvar_3.x)
       - 
        (tmpvar_3.y * tmpvar_3.y)
      ))) + x1_17)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_2) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_4), vec3(min (1.0, 
    (tmpvar_4 * 1.5)
  ))));
  mediump vec3 tmpvar_19;
  tmpvar_19 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_4 * _LightParameters.y)));
  c_6.xyz = tmpvar_19;
  c_6.w = _RimColor.w;
  lowp vec4 _color_20;
  _color_20.w = c_6.w;
  _color_20.xyz = (c_6.xyz + (tmpvar_5 * _RimColor.xyz));
  c_6.w = _color_20.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_20.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
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
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec3 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp vec3 t10_5;
mediump vec3 t16_6;
mediump float t16_12;
mediump float t16_18;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_3.xyz = t16_3.xyz * t10_5.xyz;
    t16_18 = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.x = dot(t16_0.xyz, vs_TEXCOORD3.xyz);
    t16_0.x = (-t16_0.x) + 1.0;
    t16_6.x = max(t16_18, 0.0);
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_6.x = t16_6.x * _LightDir.w + t10_2.x;
    t16_12 = t16_6.x * 1.5;
    t16_12 = min(t16_12, 1.0);
    t16_4.xyz = t16_6.xxx + (-_CharacterShadowColor.xyz);
    t16_6.x = t16_6.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_12) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_3.xyz = t16_3.xyz * t16_4.xyz;
    t16_12 = dot(t16_3.xyz, vec3(1.0, 1.0, 1.0));
    t16_4.xyz = vec3(t16_12) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_3.xyz);
    t16_6.xyz = t16_6.xxx * t16_4.xyz + t16_3.xyz;
    t16_0.xyz = t16_0.xxx * _RimColor.xyz + t16_6.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
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
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
mediump vec3 tmpvar_1;
mediump vec3 tmpvar_2;
mediump vec3 tmpvar_3;
mediump float tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_2 = tmpvar_8.xyz;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_3 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_3;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_3.xyzz * tmpvar_3.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  tmpvar_5 = (1.0 - dot (tmpvar_3, xlv_TEXCOORD3.xyz));
  tmpvar_4 = ((max (
    dot (tmpvar_3, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = ((max (
    ((1.055 * pow ((
      ((x2_16 + (unity_SHC.xyz * (
        (tmpvar_3.x * tmpvar_3.x)
       - 
        (tmpvar_3.y * tmpvar_3.y)
      ))) + x1_17)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_2) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_4), vec3(min (1.0, 
    (tmpvar_4 * 1.5)
  ))));
  mediump vec3 tmpvar_19;
  tmpvar_19 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_4 * _LightParameters.y)));
  c_6.xyz = tmpvar_19;
  c_6.w = _RimColor.w;
  lowp vec4 _color_20;
  _color_20.w = c_6.w;
  _color_20.xyz = (c_6.xyz + (tmpvar_5 * _RimColor.xyz));
  c_6.w = _color_20.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_20.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
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
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec3 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp vec3 t10_5;
mediump vec3 t16_6;
mediump float t16_12;
mediump float t16_18;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_3.xyz = t16_3.xyz * t10_5.xyz;
    t16_18 = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.x = dot(t16_0.xyz, vs_TEXCOORD3.xyz);
    t16_0.x = (-t16_0.x) + 1.0;
    t16_6.x = max(t16_18, 0.0);
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_6.x = t16_6.x * _LightDir.w + t10_2.x;
    t16_12 = t16_6.x * 1.5;
    t16_12 = min(t16_12, 1.0);
    t16_4.xyz = t16_6.xxx + (-_CharacterShadowColor.xyz);
    t16_6.x = t16_6.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_12) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_3.xyz = t16_3.xyz * t16_4.xyz;
    t16_12 = dot(t16_3.xyz, vec3(1.0, 1.0, 1.0));
    t16_4.xyz = vec3(t16_12) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_3.xyz);
    t16_6.xyz = t16_6.xxx * t16_4.xyz + t16_3.xyz;
    t16_0.xyz = t16_0.xxx * _RimColor.xyz + t16_6.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_HIGHLIGHT" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
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
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
mediump vec3 tmpvar_1;
mediump vec3 tmpvar_2;
mediump vec3 tmpvar_3;
mediump float tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_2 = tmpvar_8.xyz;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_3 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_3;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_3.xyzz * tmpvar_3.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  tmpvar_5 = (1.0 - dot (tmpvar_3, xlv_TEXCOORD3.xyz));
  tmpvar_4 = ((max (
    dot (tmpvar_3, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = ((max (
    ((1.055 * pow ((
      ((x2_16 + (unity_SHC.xyz * (
        (tmpvar_3.x * tmpvar_3.x)
       - 
        (tmpvar_3.y * tmpvar_3.y)
      ))) + x1_17)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_2) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_4), vec3(min (1.0, 
    (tmpvar_4 * 1.5)
  ))));
  mediump vec3 atten_19;
  lowp float tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_20 = tmpvar_21;
  lowp vec3 tmpvar_22;
  tmpvar_22 = vec3(tmpvar_20);
  atten_19 = tmpvar_22;
  lit_color_7 = (lit_color_7 * max ((_Shadows.x + 
    (atten_19 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_23;
  tmpvar_23 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_4 * _LightParameters.y)));
  c_6.xyz = tmpvar_23;
  c_6.w = _RimColor.w;
  lowp vec4 _color_24;
  _color_24.w = c_6.w;
  _color_24.xyz = (c_6.xyz + (tmpvar_5 * _RimColor.xyz));
  c_6.w = _color_24.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_24.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
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
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
mediump vec3 tmpvar_1;
mediump vec3 tmpvar_2;
mediump vec3 tmpvar_3;
mediump float tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_2 = tmpvar_8.xyz;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_3 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_3;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_3.xyzz * tmpvar_3.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  tmpvar_5 = (1.0 - dot (tmpvar_3, xlv_TEXCOORD3.xyz));
  tmpvar_4 = ((max (
    dot (tmpvar_3, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = ((max (
    ((1.055 * pow ((
      ((x2_16 + (unity_SHC.xyz * (
        (tmpvar_3.x * tmpvar_3.x)
       - 
        (tmpvar_3.y * tmpvar_3.y)
      ))) + x1_17)
     * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))) - 0.055)
  , vec3(0.0, 0.0, 0.0)) * tmpvar_2) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_4), vec3(min (1.0, 
    (tmpvar_4 * 1.5)
  ))));
  mediump vec3 atten_19;
  lowp float shadow_20;
  shadow_20 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_21;
  tmpvar_21 = vec3(shadow_20);
  atten_19 = tmpvar_21;
  lit_color_7 = (lit_color_7 * max ((_Shadows.x + 
    (atten_19 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_22;
  tmpvar_22 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_4 * _LightParameters.y)));
  c_6.xyz = tmpvar_22;
  c_6.w = _RimColor.w;
  lowp vec4 _color_23;
  _color_23.w = c_6.w;
  _color_23.xyz = (c_6.xyz + (tmpvar_5 * _RimColor.xyz));
  c_6.w = _color_23.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_23.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
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
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec3 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp vec3 t10_5;
mediump vec3 t16_6;
mediump float t16_12;
mediump float t16_18;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_3.xyz = t16_3.xyz * t10_5.xyz;
    t16_18 = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.x = dot(t16_0.xyz, vs_TEXCOORD3.xyz);
    t16_0.x = (-t16_0.x) + 1.0;
    t16_6.x = max(t16_18, 0.0);
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_6.x = t16_6.x * _LightDir.w + t10_2.x;
    t16_12 = t16_6.x * 1.5;
    t16_12 = min(t16_12, 1.0);
    t16_4.xyz = t16_6.xxx + (-_CharacterShadowColor.xyz);
    t16_6.x = t16_6.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_12) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_3.xyz = t16_3.xyz * t16_4.xyz;
    vec3 txVec17 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_12 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec17, 0.0);
    t16_18 = (-_LightShadowData.x) + 1.0;
    t16_12 = t16_12 * t16_18 + _LightShadowData.x;
    t16_12 = t16_12 * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_12 = max(t16_12, 0.0);
    t16_3.xyz = vec3(t16_12) * t16_3.xyz;
    t16_12 = dot(t16_3.xyz, vec3(1.0, 1.0, 1.0));
    t16_4.xyz = vec3(t16_12) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_3.xyz);
    t16_6.xyz = t16_6.xxx * t16_4.xyz + t16_3.xyz;
    t16_0.xyz = t16_0.xxx * _RimColor.xyz + t16_6.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_OFF" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_NOISEGLOW" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_HIGHLIGHT" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" }
"!!GLES3"
}
}
 }
}
Fallback "Hidden/Shadow/Opaque"
}
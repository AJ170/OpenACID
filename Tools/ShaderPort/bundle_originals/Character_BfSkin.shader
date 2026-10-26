Shader "Character/BfSkin" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _BumpTex ("BumpMap", 2D) = "white" { }
 _DetailNormalTex ("Detail normal", 2D) = "white" { }
 _MaskTex ("Mask", 2D) = "white" { }
 _SkinSSTex ("SubSurfaceScatter", 2D) = "white" { }
 _DetailScale ("Detail Normal Scale", Float) = 10
 _SkinColor ("Skin Color", Color) = (1,1,1,1)
 _SpecColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
}
SubShader { 
 LOD 200
 Tags { "RenderType"="Opaque" }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "SHADOWSUPPORT"="true" "RenderType"="Opaque" }
  GpuProgramID 48465
Program "vp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_WorldTransformParams;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
void main ()
{
  lowp float tangentSign_1;
  lowp vec3 worldTangent_2;
  lowp vec3 worldNormal_3;
  highp vec3 tmpvar_4;
  tmpvar_4 = (_Object2World * _glesVertex).xyz;
  highp vec4 v_5;
  v_5.x = _World2Object[0].x;
  v_5.y = _World2Object[1].x;
  v_5.z = _World2Object[2].x;
  v_5.w = _World2Object[3].x;
  highp vec4 v_6;
  v_6.x = _World2Object[0].y;
  v_6.y = _World2Object[1].y;
  v_6.z = _World2Object[2].y;
  v_6.w = _World2Object[3].y;
  highp vec4 v_7;
  v_7.x = _World2Object[0].z;
  v_7.y = _World2Object[1].z;
  v_7.z = _World2Object[2].z;
  v_7.w = _World2Object[3].z;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize(((
    (v_5.xyz * _glesNormal.x)
   + 
    (v_6.xyz * _glesNormal.y)
  ) + (v_7.xyz * _glesNormal.z)));
  worldNormal_3 = tmpvar_8;
  highp mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * _glesTANGENT.xyz));
  worldTangent_2 = tmpvar_10;
  highp float tmpvar_11;
  tmpvar_11 = (_glesTANGENT.w * unity_WorldTransformParams.w);
  tangentSign_1 = tmpvar_11;
  lowp vec3 tmpvar_12;
  tmpvar_12 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
  highp vec4 tmpvar_13;
  tmpvar_13.x = worldTangent_2.x;
  tmpvar_13.y = tmpvar_12.x;
  tmpvar_13.z = worldNormal_3.x;
  tmpvar_13.w = tmpvar_4.x;
  highp vec4 tmpvar_14;
  tmpvar_14.x = worldTangent_2.y;
  tmpvar_14.y = tmpvar_12.y;
  tmpvar_14.z = worldNormal_3.y;
  tmpvar_14.w = tmpvar_4.y;
  highp vec4 tmpvar_15;
  tmpvar_15.x = worldTangent_2.z;
  tmpvar_15.y = tmpvar_12.z;
  tmpvar_15.z = worldNormal_3.z;
  tmpvar_15.w = tmpvar_4.z;
  lowp vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = worldNormal_3;
  mediump vec4 normal_17;
  normal_17 = tmpvar_16;
  mediump vec3 x2_18;
  mediump vec3 x1_19;
  x1_19.x = dot (unity_SHAr, normal_17);
  x1_19.y = dot (unity_SHAg, normal_17);
  x1_19.z = dot (unity_SHAb, normal_17);
  mediump vec4 tmpvar_20;
  tmpvar_20 = (normal_17.xyzz * normal_17.yzzx);
  x2_18.x = dot (unity_SHBr, tmpvar_20);
  x2_18.y = dot (unity_SHBg, tmpvar_20);
  x2_18.z = dot (unity_SHBb, tmpvar_20);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_13;
  xlv_TEXCOORD2 = tmpvar_14;
  xlv_TEXCOORD3 = tmpvar_15;
  xlv_TEXCOORD4 = ((x2_18 + (unity_SHC.xyz * 
    ((normal_17.x * normal_17.x) - (normal_17.y * normal_17.y))
  )) + x1_19);
}


#endif
#ifdef FRAGMENT
uniform highp vec3 _WorldSpaceCameraPos;
uniform mediump vec4 _WorldSpaceLightPos0;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpTex;
uniform sampler2D _DetailNormalTex;
uniform sampler2D _MaskTex;
uniform highp float _DetailScale;
uniform highp vec4 _SkinColor;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
void main ()
{
  mediump vec3 tmpvar_1;
  mediump vec3 tmpvar_2;
  lowp vec3 worldN_3;
  lowp vec4 c_4;
  lowp vec3 worldViewDir_5;
  lowp vec3 lightDir_6;
  highp vec3 tmpvar_7;
  tmpvar_7.x = xlv_TEXCOORD1.w;
  tmpvar_7.y = xlv_TEXCOORD2.w;
  tmpvar_7.z = xlv_TEXCOORD3.w;
  mediump vec3 tmpvar_8;
  tmpvar_8 = _WorldSpaceLightPos0.xyz;
  lightDir_6 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize((_WorldSpaceCameraPos - tmpvar_7));
  worldViewDir_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  lowp vec3 tmpvar_11;
  lowp float tmpvar_12;
  lowp float tmpvar_13;
  mediump vec3 n_sample_14;
  mediump vec3 n_detail_15;
  mediump vec4 m_16;
  mediump vec4 c_17;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_17 = tmpvar_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_MaskTex, xlv_TEXCOORD0);
  m_16 = tmpvar_19;
  highp vec2 P_20;
  P_20 = (xlv_TEXCOORD0 * _DetailScale);
  lowp vec3 tmpvar_21;
  tmpvar_21 = ((texture2D (_DetailNormalTex, P_20).xyz * 2.0) - 1.0);
  n_detail_15 = tmpvar_21;
  lowp vec3 tmpvar_22;
  tmpvar_22 = ((texture2D (_BumpTex, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  n_sample_14 = tmpvar_22;
  tmpvar_10 = (c_17.xyz * _SkinColor.xyz);
  tmpvar_13 = c_17.w;
  tmpvar_12 = m_16.y;
  mediump vec3 tmpvar_23;
  tmpvar_23.z = 0.0;
  tmpvar_23.xy = n_detail_15.xy;
  mediump vec3 tmpvar_24;
  tmpvar_24 = normalize((n_sample_14 + (tmpvar_23 * m_16.x)));
  tmpvar_11 = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (xlv_TEXCOORD1.xyz, tmpvar_11);
  worldN_3.x = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (xlv_TEXCOORD2.xyz, tmpvar_11);
  worldN_3.y = tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = dot (xlv_TEXCOORD3.xyz, tmpvar_11);
  worldN_3.z = tmpvar_27;
  tmpvar_1 = _LightColor0.xyz;
  tmpvar_2 = lightDir_6;
  mediump vec3 viewDir_28;
  viewDir_28 = worldViewDir_5;
  lowp vec4 c_29;
  lowp vec4 c_30;
  highp float nh_31;
  lowp float diff_32;
  mediump float tmpvar_33;
  tmpvar_33 = max (0.0, dot (worldN_3, tmpvar_2));
  diff_32 = tmpvar_33;
  mediump float tmpvar_34;
  tmpvar_34 = max (0.0, dot (worldN_3, normalize(
    (tmpvar_2 + viewDir_28)
  )));
  nh_31 = tmpvar_34;
  highp float tmpvar_35;
  tmpvar_35 = (pow (nh_31, 128.0) * tmpvar_12);
  c_30.xyz = (((tmpvar_10 * tmpvar_1) * diff_32) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_35));
  c_30.w = tmpvar_13;
  c_29.w = c_30.w;
  c_29.xyz = (c_30.xyz + (tmpvar_10 * xlv_TEXCOORD4));
  c_4.xyz = c_29.xyz;
  c_4.w = 1.0;
  gl_FragData[0] = c_4;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
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
uniform 	mediump vec4 _WorldSpaceLightPos0;
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
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
out highp vec4 vs_TEXCOORD2;
out highp vec4 vs_TEXCOORD3;
out mediump vec3 vs_TEXCOORD4;
highp vec4 t0;
mediump vec4 t16_0;
highp vec3 t1;
highp vec4 t2;
highp vec3 t3;
lowp vec3 t10_4;
mediump vec3 t16_5;
mediump vec3 t16_6;
highp float t21;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    vs_TEXCOORD1.w = t0.x;
    t0.x = in_TANGENT0.w * unity_WorldTransformParams.w;
    t1.xyz = in_TANGENT0.yyy * _Object2World[1].yzx;
    t1.xyz = _Object2World[0].yzx * in_TANGENT0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].yzx * in_TANGENT0.zzz + t1.xyz;
    t21 = dot(t1.xyz, t1.xyz);
    t21 = inversesqrt(t21);
    t1.xyz = vec3(t21) * t1.xyz;
    t2.x = in_NORMAL0.x * _World2Object[0].x;
    t2.y = in_NORMAL0.x * _World2Object[1].x;
    t2.z = in_NORMAL0.x * _World2Object[2].x;
    t3.x = in_NORMAL0.y * _World2Object[0].y;
    t3.y = in_NORMAL0.y * _World2Object[1].y;
    t3.z = in_NORMAL0.y * _World2Object[2].y;
    t2.xyz = t2.xyz + t3.xyz;
    t3.x = in_NORMAL0.z * _World2Object[0].z;
    t3.y = in_NORMAL0.z * _World2Object[1].z;
    t3.z = in_NORMAL0.z * _World2Object[2].z;
    t2.xyz = t2.xyz + t3.xyz;
    t21 = dot(t2.xyz, t2.xyz);
    t21 = inversesqrt(t21);
    t2.xyz = vec3(t21) * t2.xyz;
    t10_4.xyz = t1.xyz * t2.zxy;
    t10_4.xyz = t2.yzx * t1.yzx + (-t10_4.xyz);
    t10_4.xyz = t0.xxx * t10_4.xyz;
    vs_TEXCOORD1.y = t10_4.x;
    vs_TEXCOORD1.x = t1.z;
    vs_TEXCOORD1.z = t2.x;
    vs_TEXCOORD2.x = t1.x;
    vs_TEXCOORD3.x = t1.y;
    vs_TEXCOORD2.w = t0.y;
    vs_TEXCOORD3.w = t0.z;
    vs_TEXCOORD2.y = t10_4.y;
    vs_TEXCOORD3.y = t10_4.z;
    vs_TEXCOORD2.z = t2.y;
    vs_TEXCOORD3.z = t2.z;
    t16_5.x = t2.y * t2.y;
    t16_5.x = t2.x * t2.x + (-t16_5.x);
    t16_0 = t2.yzzx * t2.xyzz;
    t16_6.x = dot(unity_SHBr, t16_0);
    t16_6.y = dot(unity_SHBg, t16_0);
    t16_6.z = dot(unity_SHBb, t16_0);
    t16_5.xyz = unity_SHC.xyz * t16_5.xxx + t16_6.xyz;
    t2.w = 1.0;
    t16_6.x = dot(unity_SHAr, t2);
    t16_6.y = dot(unity_SHAg, t2);
    t16_6.z = dot(unity_SHAb, t2);
    vs_TEXCOORD4.xyz = t16_5.xyz + t16_6.xyz;
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
uniform 	mediump vec4 _WorldSpaceLightPos0;
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
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _DetailNormalTex;
uniform lowp sampler2D _BumpTex;
in highp vec2 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
in highp vec4 vs_TEXCOORD2;
in highp vec4 vs_TEXCOORD3;
in mediump vec3 vs_TEXCOORD4;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec3 t0;
mediump vec3 t16_0;
lowp vec3 t10_0;
mediump vec4 t16_1;
lowp vec3 t10_2;
lowp vec2 t10_3;
mediump vec3 t16_4;
highp vec3 t5;
lowp vec3 t10_5;
highp float t18;
mediump float t16_19;
void main()
{
    t0.x = vs_TEXCOORD1.w;
    t0.y = vs_TEXCOORD2.w;
    t0.z = vs_TEXCOORD3.w;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t0.xyz, t0.xyz);
    t18 = inversesqrt(t18);
    t16_1.xyz = t0.xyz * vec3(t18) + _WorldSpaceLightPos0.xyz;
    t16_19 = dot(t16_1.xyz, t16_1.xyz);
    t16_19 = inversesqrt(t16_19);
    t16_1.xyz = vec3(t16_19) * t16_1.xyz;
    t10_0.xyz = texture(_BumpTex, vs_TEXCOORD0.xy).xyz;
    t10_2.xyz = t10_0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t0.xy = vs_TEXCOORD0.xy * vec2(_DetailScale);
    t10_0.xy = texture(_DetailNormalTex, t0.xy).xy;
    t10_3.xy = t10_0.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
    t10_0.xy = texture(_MaskTex, vs_TEXCOORD0.xy).xy;
    t16_4.xy = t10_0.xx * t10_3.xy;
    t16_4.z = 0.0;
    t16_4.xyz = t10_2.xyz + t16_4.xyz;
    t16_19 = dot(t16_4.xyz, t16_4.xyz);
    t16_19 = inversesqrt(t16_19);
    t16_4.xyz = vec3(t16_19) * t16_4.xyz;
    t5.x = dot(vs_TEXCOORD1.xyz, t16_4.xyz);
    t5.y = dot(vs_TEXCOORD2.xyz, t16_4.xyz);
    t5.z = dot(vs_TEXCOORD3.xyz, t16_4.xyz);
    t16_1.x = dot(t5.xyz, t16_1.xyz);
    t16_1.y = dot(t5.xyz, _WorldSpaceLightPos0.xyz);
    t16_1.xy = max(t16_1.xy, vec2(0.0, 0.0));
    t16_0.x = log2(t16_1.x);
    t16_0.x = t16_0.x * 128.0;
    t16_0.x = exp2(t16_0.x);
    t16_0.x = t10_0.y * t16_0.x;
    t16_1.xzw = _LightColor0.xyz * _SpecColor.xyz;
    t16_0.xyz = t16_0.xxx * t16_1.xzw;
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t5.xyz = t10_5.xyz * _SkinColor.xyz;
    t16_1.xzw = t5.xyz * _LightColor0.xyz;
    t16_0.xyz = t16_1.xzw * t16_1.yyy + t16_0.xyz;
    t16_1.xyz = t5.xyz * vs_TEXCOORD4.xyz + t16_0.xyz;
    SV_Target0.xyz = t16_1.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_WorldTransformParams;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp float tangentSign_1;
  lowp vec3 worldTangent_2;
  lowp vec3 worldNormal_3;
  mediump vec4 tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6 = (_Object2World * _glesVertex);
  tmpvar_5 = tmpvar_6.xyz;
  highp vec4 v_7;
  v_7.x = _World2Object[0].x;
  v_7.y = _World2Object[1].x;
  v_7.z = _World2Object[2].x;
  v_7.w = _World2Object[3].x;
  highp vec4 v_8;
  v_8.x = _World2Object[0].y;
  v_8.y = _World2Object[1].y;
  v_8.z = _World2Object[2].y;
  v_8.w = _World2Object[3].y;
  highp vec4 v_9;
  v_9.x = _World2Object[0].z;
  v_9.y = _World2Object[1].z;
  v_9.z = _World2Object[2].z;
  v_9.w = _World2Object[3].z;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize(((
    (v_7.xyz * _glesNormal.x)
   + 
    (v_8.xyz * _glesNormal.y)
  ) + (v_9.xyz * _glesNormal.z)));
  worldNormal_3 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesTANGENT.xyz));
  worldTangent_2 = tmpvar_12;
  highp float tmpvar_13;
  tmpvar_13 = (_glesTANGENT.w * unity_WorldTransformParams.w);
  tangentSign_1 = tmpvar_13;
  lowp vec3 tmpvar_14;
  tmpvar_14 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
  highp vec4 tmpvar_15;
  tmpvar_15.x = worldTangent_2.x;
  tmpvar_15.y = tmpvar_14.x;
  tmpvar_15.z = worldNormal_3.x;
  tmpvar_15.w = tmpvar_5.x;
  highp vec4 tmpvar_16;
  tmpvar_16.x = worldTangent_2.y;
  tmpvar_16.y = tmpvar_14.y;
  tmpvar_16.z = worldNormal_3.y;
  tmpvar_16.w = tmpvar_5.y;
  highp vec4 tmpvar_17;
  tmpvar_17.x = worldTangent_2.z;
  tmpvar_17.y = tmpvar_14.z;
  tmpvar_17.z = worldNormal_3.z;
  tmpvar_17.w = tmpvar_5.z;
  lowp vec4 tmpvar_18;
  tmpvar_18.w = 1.0;
  tmpvar_18.xyz = worldNormal_3;
  mediump vec4 normal_19;
  normal_19 = tmpvar_18;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  x1_21.x = dot (unity_SHAr, normal_19);
  x1_21.y = dot (unity_SHAg, normal_19);
  x1_21.z = dot (unity_SHAb, normal_19);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_19.xyzz * normal_19.yzzx);
  x2_20.x = dot (unity_SHBr, tmpvar_22);
  x2_20.y = dot (unity_SHBg, tmpvar_22);
  x2_20.z = dot (unity_SHBb, tmpvar_22);
  tmpvar_4 = (unity_World2Shadow[0] * tmpvar_6);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_15;
  xlv_TEXCOORD2 = tmpvar_16;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = ((x2_20 + (unity_SHC.xyz * 
    ((normal_19.x * normal_19.x) - (normal_19.y * normal_19.y))
  )) + x1_21);
  xlv_TEXCOORD5 = tmpvar_4;
}


#endif
#ifdef FRAGMENT
uniform highp vec3 _WorldSpaceCameraPos;
uniform mediump vec4 _WorldSpaceLightPos0;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpTex;
uniform sampler2D _DetailNormalTex;
uniform sampler2D _MaskTex;
uniform highp float _DetailScale;
uniform highp vec4 _SkinColor;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  mediump float tmpvar_1;
  mediump vec3 tmpvar_2;
  mediump vec3 tmpvar_3;
  lowp vec3 worldN_4;
  lowp vec4 c_5;
  lowp vec3 worldViewDir_6;
  lowp vec3 lightDir_7;
  highp vec3 tmpvar_8;
  tmpvar_8.x = xlv_TEXCOORD1.w;
  tmpvar_8.y = xlv_TEXCOORD2.w;
  tmpvar_8.z = xlv_TEXCOORD3.w;
  mediump vec3 tmpvar_9;
  tmpvar_9 = _WorldSpaceLightPos0.xyz;
  lightDir_7 = tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - tmpvar_8));
  worldViewDir_6 = tmpvar_10;
  lowp vec3 tmpvar_11;
  lowp vec3 tmpvar_12;
  lowp float tmpvar_13;
  lowp float tmpvar_14;
  mediump vec3 n_sample_15;
  mediump vec3 n_detail_16;
  mediump vec4 m_17;
  mediump vec4 c_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_18 = tmpvar_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_MaskTex, xlv_TEXCOORD0);
  m_17 = tmpvar_20;
  highp vec2 P_21;
  P_21 = (xlv_TEXCOORD0 * _DetailScale);
  lowp vec3 tmpvar_22;
  tmpvar_22 = ((texture2D (_DetailNormalTex, P_21).xyz * 2.0) - 1.0);
  n_detail_16 = tmpvar_22;
  lowp vec3 tmpvar_23;
  tmpvar_23 = ((texture2D (_BumpTex, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  n_sample_15 = tmpvar_23;
  tmpvar_11 = (c_18.xyz * _SkinColor.xyz);
  tmpvar_14 = c_18.w;
  tmpvar_13 = m_17.y;
  mediump vec3 tmpvar_24;
  tmpvar_24.z = 0.0;
  tmpvar_24.xy = n_detail_16.xy;
  mediump vec3 tmpvar_25;
  tmpvar_25 = normalize((n_sample_15 + (tmpvar_24 * m_17.x)));
  tmpvar_12 = tmpvar_25;
  lowp float tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD5).x > 
    (xlv_TEXCOORD5.z / xlv_TEXCOORD5.w)
  )), _LightShadowData.x);
  tmpvar_26 = tmpvar_27;
  highp float tmpvar_28;
  tmpvar_28 = dot (xlv_TEXCOORD1.xyz, tmpvar_12);
  worldN_4.x = tmpvar_28;
  highp float tmpvar_29;
  tmpvar_29 = dot (xlv_TEXCOORD2.xyz, tmpvar_12);
  worldN_4.y = tmpvar_29;
  highp float tmpvar_30;
  tmpvar_30 = dot (xlv_TEXCOORD3.xyz, tmpvar_12);
  worldN_4.z = tmpvar_30;
  tmpvar_2 = _LightColor0.xyz;
  tmpvar_3 = lightDir_7;
  tmpvar_1 = tmpvar_26;
  mediump vec3 tmpvar_31;
  tmpvar_31 = (tmpvar_2 * tmpvar_1);
  tmpvar_2 = tmpvar_31;
  mediump vec3 viewDir_32;
  viewDir_32 = worldViewDir_6;
  lowp vec4 c_33;
  lowp vec4 c_34;
  highp float nh_35;
  lowp float diff_36;
  mediump float tmpvar_37;
  tmpvar_37 = max (0.0, dot (worldN_4, tmpvar_3));
  diff_36 = tmpvar_37;
  mediump float tmpvar_38;
  tmpvar_38 = max (0.0, dot (worldN_4, normalize(
    (tmpvar_3 + viewDir_32)
  )));
  nh_35 = tmpvar_38;
  highp float tmpvar_39;
  tmpvar_39 = (pow (nh_35, 128.0) * tmpvar_13);
  c_34.xyz = (((tmpvar_11 * tmpvar_31) * diff_36) + ((tmpvar_31 * _SpecColor.xyz) * tmpvar_39));
  c_34.w = tmpvar_14;
  c_33.w = c_34.w;
  c_33.xyz = (c_34.xyz + (tmpvar_11 * xlv_TEXCOORD4));
  c_5.xyz = c_33.xyz;
  c_5.w = 1.0;
  gl_FragData[0] = c_5;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform mediump vec4 unity_4LightAtten0;
uniform mediump vec4 unity_LightColor[8];
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_WorldTransformParams;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
void main ()
{
  lowp float tangentSign_1;
  lowp vec3 worldTangent_2;
  lowp vec3 worldNormal_3;
  mediump vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (_Object2World * _glesVertex).xyz;
  highp vec4 v_6;
  v_6.x = _World2Object[0].x;
  v_6.y = _World2Object[1].x;
  v_6.z = _World2Object[2].x;
  v_6.w = _World2Object[3].x;
  highp vec4 v_7;
  v_7.x = _World2Object[0].y;
  v_7.y = _World2Object[1].y;
  v_7.z = _World2Object[2].y;
  v_7.w = _World2Object[3].y;
  highp vec4 v_8;
  v_8.x = _World2Object[0].z;
  v_8.y = _World2Object[1].z;
  v_8.z = _World2Object[2].z;
  v_8.w = _World2Object[3].z;
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize(((
    (v_6.xyz * _glesNormal.x)
   + 
    (v_7.xyz * _glesNormal.y)
  ) + (v_8.xyz * _glesNormal.z)));
  worldNormal_3 = tmpvar_9;
  highp mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize((tmpvar_10 * _glesTANGENT.xyz));
  worldTangent_2 = tmpvar_11;
  highp float tmpvar_12;
  tmpvar_12 = (_glesTANGENT.w * unity_WorldTransformParams.w);
  tangentSign_1 = tmpvar_12;
  lowp vec3 tmpvar_13;
  tmpvar_13 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
  highp vec4 tmpvar_14;
  tmpvar_14.x = worldTangent_2.x;
  tmpvar_14.y = tmpvar_13.x;
  tmpvar_14.z = worldNormal_3.x;
  tmpvar_14.w = tmpvar_5.x;
  highp vec4 tmpvar_15;
  tmpvar_15.x = worldTangent_2.y;
  tmpvar_15.y = tmpvar_13.y;
  tmpvar_15.z = worldNormal_3.y;
  tmpvar_15.w = tmpvar_5.y;
  highp vec4 tmpvar_16;
  tmpvar_16.x = worldTangent_2.z;
  tmpvar_16.y = tmpvar_13.z;
  tmpvar_16.z = worldNormal_3.z;
  tmpvar_16.w = tmpvar_5.z;
  lowp vec4 tmpvar_17;
  tmpvar_17.w = 1.0;
  tmpvar_17.xyz = worldNormal_3;
  mediump vec4 normal_18;
  normal_18 = tmpvar_17;
  mediump vec3 x2_19;
  mediump vec3 x1_20;
  x1_20.x = dot (unity_SHAr, normal_18);
  x1_20.y = dot (unity_SHAg, normal_18);
  x1_20.z = dot (unity_SHAb, normal_18);
  mediump vec4 tmpvar_21;
  tmpvar_21 = (normal_18.xyzz * normal_18.yzzx);
  x2_19.x = dot (unity_SHBr, tmpvar_21);
  x2_19.y = dot (unity_SHBg, tmpvar_21);
  x2_19.z = dot (unity_SHBb, tmpvar_21);
  highp vec3 lightColor0_22;
  lightColor0_22 = unity_LightColor[0].xyz;
  highp vec3 lightColor1_23;
  lightColor1_23 = unity_LightColor[1].xyz;
  highp vec3 lightColor2_24;
  lightColor2_24 = unity_LightColor[2].xyz;
  highp vec3 lightColor3_25;
  lightColor3_25 = unity_LightColor[3].xyz;
  highp vec4 lightAttenSq_26;
  lightAttenSq_26 = unity_4LightAtten0;
  highp vec3 normal_27;
  normal_27 = worldNormal_3;
  highp vec3 col_28;
  highp vec4 ndotl_29;
  highp vec4 lengthSq_30;
  highp vec4 tmpvar_31;
  tmpvar_31 = (unity_4LightPosX0 - tmpvar_5.x);
  highp vec4 tmpvar_32;
  tmpvar_32 = (unity_4LightPosY0 - tmpvar_5.y);
  highp vec4 tmpvar_33;
  tmpvar_33 = (unity_4LightPosZ0 - tmpvar_5.z);
  lengthSq_30 = (tmpvar_31 * tmpvar_31);
  lengthSq_30 = (lengthSq_30 + (tmpvar_32 * tmpvar_32));
  lengthSq_30 = (lengthSq_30 + (tmpvar_33 * tmpvar_33));
  ndotl_29 = (tmpvar_31 * normal_27.x);
  ndotl_29 = (ndotl_29 + (tmpvar_32 * normal_27.y));
  ndotl_29 = (ndotl_29 + (tmpvar_33 * normal_27.z));
  highp vec4 tmpvar_34;
  tmpvar_34 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_29 * inversesqrt(lengthSq_30)));
  ndotl_29 = tmpvar_34;
  highp vec4 tmpvar_35;
  tmpvar_35 = (tmpvar_34 * (1.0/((1.0 + 
    (lengthSq_30 * lightAttenSq_26)
  ))));
  col_28 = (lightColor0_22 * tmpvar_35.x);
  col_28 = (col_28 + (lightColor1_23 * tmpvar_35.y));
  col_28 = (col_28 + (lightColor2_24 * tmpvar_35.z));
  col_28 = (col_28 + (lightColor3_25 * tmpvar_35.w));
  tmpvar_4 = (((x2_19 + 
    (unity_SHC.xyz * ((normal_18.x * normal_18.x) - (normal_18.y * normal_18.y)))
  ) + x1_20) + col_28);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_14;
  xlv_TEXCOORD2 = tmpvar_15;
  xlv_TEXCOORD3 = tmpvar_16;
  xlv_TEXCOORD4 = tmpvar_4;
}


#endif
#ifdef FRAGMENT
uniform highp vec3 _WorldSpaceCameraPos;
uniform mediump vec4 _WorldSpaceLightPos0;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpTex;
uniform sampler2D _DetailNormalTex;
uniform sampler2D _MaskTex;
uniform highp float _DetailScale;
uniform highp vec4 _SkinColor;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
void main ()
{
  mediump vec3 tmpvar_1;
  mediump vec3 tmpvar_2;
  lowp vec3 worldN_3;
  lowp vec4 c_4;
  lowp vec3 worldViewDir_5;
  lowp vec3 lightDir_6;
  highp vec3 tmpvar_7;
  tmpvar_7.x = xlv_TEXCOORD1.w;
  tmpvar_7.y = xlv_TEXCOORD2.w;
  tmpvar_7.z = xlv_TEXCOORD3.w;
  mediump vec3 tmpvar_8;
  tmpvar_8 = _WorldSpaceLightPos0.xyz;
  lightDir_6 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize((_WorldSpaceCameraPos - tmpvar_7));
  worldViewDir_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  lowp vec3 tmpvar_11;
  lowp float tmpvar_12;
  lowp float tmpvar_13;
  mediump vec3 n_sample_14;
  mediump vec3 n_detail_15;
  mediump vec4 m_16;
  mediump vec4 c_17;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_17 = tmpvar_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_MaskTex, xlv_TEXCOORD0);
  m_16 = tmpvar_19;
  highp vec2 P_20;
  P_20 = (xlv_TEXCOORD0 * _DetailScale);
  lowp vec3 tmpvar_21;
  tmpvar_21 = ((texture2D (_DetailNormalTex, P_20).xyz * 2.0) - 1.0);
  n_detail_15 = tmpvar_21;
  lowp vec3 tmpvar_22;
  tmpvar_22 = ((texture2D (_BumpTex, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  n_sample_14 = tmpvar_22;
  tmpvar_10 = (c_17.xyz * _SkinColor.xyz);
  tmpvar_13 = c_17.w;
  tmpvar_12 = m_16.y;
  mediump vec3 tmpvar_23;
  tmpvar_23.z = 0.0;
  tmpvar_23.xy = n_detail_15.xy;
  mediump vec3 tmpvar_24;
  tmpvar_24 = normalize((n_sample_14 + (tmpvar_23 * m_16.x)));
  tmpvar_11 = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (xlv_TEXCOORD1.xyz, tmpvar_11);
  worldN_3.x = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (xlv_TEXCOORD2.xyz, tmpvar_11);
  worldN_3.y = tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = dot (xlv_TEXCOORD3.xyz, tmpvar_11);
  worldN_3.z = tmpvar_27;
  tmpvar_1 = _LightColor0.xyz;
  tmpvar_2 = lightDir_6;
  mediump vec3 viewDir_28;
  viewDir_28 = worldViewDir_5;
  lowp vec4 c_29;
  lowp vec4 c_30;
  highp float nh_31;
  lowp float diff_32;
  mediump float tmpvar_33;
  tmpvar_33 = max (0.0, dot (worldN_3, tmpvar_2));
  diff_32 = tmpvar_33;
  mediump float tmpvar_34;
  tmpvar_34 = max (0.0, dot (worldN_3, normalize(
    (tmpvar_2 + viewDir_28)
  )));
  nh_31 = tmpvar_34;
  highp float tmpvar_35;
  tmpvar_35 = (pow (nh_31, 128.0) * tmpvar_12);
  c_30.xyz = (((tmpvar_10 * tmpvar_1) * diff_32) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_35));
  c_30.w = tmpvar_13;
  c_29.w = c_30.w;
  c_29.xyz = (c_30.xyz + (tmpvar_10 * xlv_TEXCOORD4));
  c_4.xyz = c_29.xyz;
  c_4.w = 1.0;
  gl_FragData[0] = c_4;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
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
uniform 	mediump vec4 _WorldSpaceLightPos0;
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
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
out highp vec4 vs_TEXCOORD2;
out highp vec4 vs_TEXCOORD3;
out mediump vec3 vs_TEXCOORD4;
highp vec4 t0;
mediump vec4 t16_0;
highp vec4 t1;
highp vec4 t2;
highp vec4 t3;
lowp vec3 t10_3;
mediump vec3 t16_4;
highp vec4 t5;
mediump vec3 t16_5;
highp float t18;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.xyz = in_TANGENT0.yyy * _Object2World[1].yzx;
    t0.xyz = _Object2World[0].yzx * in_TANGENT0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].yzx * in_TANGENT0.zzz + t0.xyz;
    t18 = dot(t0.xyz, t0.xyz);
    t18 = inversesqrt(t18);
    t0.xyz = vec3(t18) * t0.xyz;
    vs_TEXCOORD1.x = t0.z;
    t18 = in_TANGENT0.w * unity_WorldTransformParams.w;
    t1.x = in_NORMAL0.x * _World2Object[0].x;
    t1.y = in_NORMAL0.x * _World2Object[1].x;
    t1.z = in_NORMAL0.x * _World2Object[2].x;
    t2.x = in_NORMAL0.y * _World2Object[0].y;
    t2.y = in_NORMAL0.y * _World2Object[1].y;
    t2.z = in_NORMAL0.y * _World2Object[2].y;
    t1.xyz = t1.xyz + t2.xyz;
    t2.x = in_NORMAL0.z * _World2Object[0].z;
    t2.y = in_NORMAL0.z * _World2Object[1].z;
    t2.z = in_NORMAL0.z * _World2Object[2].z;
    t1.xyz = t1.xyz + t2.xyz;
    t19 = dot(t1.xyz, t1.xyz);
    t19 = inversesqrt(t19);
    t1.xyz = vec3(t19) * t1.xyz;
    t10_3.xyz = t0.xyz * t1.zxy;
    t10_3.xyz = t1.yzx * t0.yzx + (-t10_3.xyz);
    t10_3.xyz = vec3(t18) * t10_3.xyz;
    vs_TEXCOORD1.y = t10_3.x;
    vs_TEXCOORD1.z = t1.x;
    t2.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t2.xyz;
    t2.xyz = _Object2World[3].xyz * in_POSITION0.www + t2.xyz;
    vs_TEXCOORD1.w = t2.x;
    vs_TEXCOORD2.x = t0.x;
    vs_TEXCOORD3.x = t0.y;
    vs_TEXCOORD2.y = t10_3.y;
    vs_TEXCOORD3.y = t10_3.z;
    vs_TEXCOORD2.z = t1.y;
    vs_TEXCOORD2.w = t2.y;
    vs_TEXCOORD3.w = t2.z;
    vs_TEXCOORD3.z = t1.z;
    t16_4.x = t1.y * t1.y;
    t16_4.x = t1.x * t1.x + (-t16_4.x);
    t16_0 = t1.yzzx * t1.xyzz;
    t16_5.x = dot(unity_SHBr, t16_0);
    t16_5.y = dot(unity_SHBg, t16_0);
    t16_5.z = dot(unity_SHBb, t16_0);
    t16_4.xyz = unity_SHC.xyz * t16_4.xxx + t16_5.xyz;
    t1.w = 1.0;
    t16_5.x = dot(unity_SHAr, t1);
    t16_5.y = dot(unity_SHAg, t1);
    t16_5.z = dot(unity_SHAb, t1);
    t16_4.xyz = t16_4.xyz + t16_5.xyz;
    t0 = (-t2.yyyy) + unity_4LightPosY0;
    t3 = t1.yyyy * t0;
    t0 = t0 * t0;
    t5 = (-t2.xxxx) + unity_4LightPosX0;
    t2 = (-t2.zzzz) + unity_4LightPosZ0;
    t3 = t5 * t1.xxxx + t3;
    t1 = t2 * t1.zzzz + t3;
    t0 = t5 * t5 + t0;
    t0 = t2 * t2 + t0;
    t2 = inversesqrt(t0);
    t0 = t0 * unity_4LightAtten0 + vec4(1.0, 1.0, 1.0, 1.0);
    t0 = vec4(1.0, 1.0, 1.0, 1.0) / t0;
    t1 = t1 * t2;
    t1 = max(t1, vec4(0.0, 0.0, 0.0, 0.0));
    t0 = t0 * t1;
    t1.xyz = t0.yyy * unity_LightColor[1].xyz;
    t1.xyz = unity_LightColor[0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_LightColor[2].xyz * t0.zzz + t1.xyz;
    t1.xyz = unity_LightColor[3].xyz * t0.www + t1.xyz;
    t1.xyz = t1.xyz + t16_4.xyz;
    vs_TEXCOORD4.xyz = t1.xyz;
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
uniform 	mediump vec4 _WorldSpaceLightPos0;
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
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _DetailNormalTex;
uniform lowp sampler2D _BumpTex;
in highp vec2 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
in highp vec4 vs_TEXCOORD2;
in highp vec4 vs_TEXCOORD3;
in mediump vec3 vs_TEXCOORD4;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec3 t0;
mediump vec3 t16_0;
lowp vec3 t10_0;
mediump vec4 t16_1;
lowp vec3 t10_2;
lowp vec2 t10_3;
mediump vec3 t16_4;
highp vec3 t5;
lowp vec3 t10_5;
highp float t18;
mediump float t16_19;
void main()
{
    t0.x = vs_TEXCOORD1.w;
    t0.y = vs_TEXCOORD2.w;
    t0.z = vs_TEXCOORD3.w;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t0.xyz, t0.xyz);
    t18 = inversesqrt(t18);
    t16_1.xyz = t0.xyz * vec3(t18) + _WorldSpaceLightPos0.xyz;
    t16_19 = dot(t16_1.xyz, t16_1.xyz);
    t16_19 = inversesqrt(t16_19);
    t16_1.xyz = vec3(t16_19) * t16_1.xyz;
    t10_0.xyz = texture(_BumpTex, vs_TEXCOORD0.xy).xyz;
    t10_2.xyz = t10_0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t0.xy = vs_TEXCOORD0.xy * vec2(_DetailScale);
    t10_0.xy = texture(_DetailNormalTex, t0.xy).xy;
    t10_3.xy = t10_0.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
    t10_0.xy = texture(_MaskTex, vs_TEXCOORD0.xy).xy;
    t16_4.xy = t10_0.xx * t10_3.xy;
    t16_4.z = 0.0;
    t16_4.xyz = t10_2.xyz + t16_4.xyz;
    t16_19 = dot(t16_4.xyz, t16_4.xyz);
    t16_19 = inversesqrt(t16_19);
    t16_4.xyz = vec3(t16_19) * t16_4.xyz;
    t5.x = dot(vs_TEXCOORD1.xyz, t16_4.xyz);
    t5.y = dot(vs_TEXCOORD2.xyz, t16_4.xyz);
    t5.z = dot(vs_TEXCOORD3.xyz, t16_4.xyz);
    t16_1.x = dot(t5.xyz, t16_1.xyz);
    t16_1.y = dot(t5.xyz, _WorldSpaceLightPos0.xyz);
    t16_1.xy = max(t16_1.xy, vec2(0.0, 0.0));
    t16_0.x = log2(t16_1.x);
    t16_0.x = t16_0.x * 128.0;
    t16_0.x = exp2(t16_0.x);
    t16_0.x = t10_0.y * t16_0.x;
    t16_1.xzw = _LightColor0.xyz * _SpecColor.xyz;
    t16_0.xyz = t16_0.xxx * t16_1.xzw;
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t5.xyz = t10_5.xyz * _SkinColor.xyz;
    t16_1.xzw = t5.xyz * _LightColor0.xyz;
    t16_0.xyz = t16_1.xzw * t16_1.yyy + t16_0.xyz;
    t16_1.xyz = t5.xyz * vs_TEXCOORD4.xyz + t16_0.xyz;
    SV_Target0.xyz = t16_1.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform mediump vec4 unity_4LightAtten0;
uniform mediump vec4 unity_LightColor[8];
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_WorldTransformParams;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp float tangentSign_1;
  lowp vec3 worldTangent_2;
  lowp vec3 worldNormal_3;
  mediump vec3 tmpvar_4;
  mediump vec4 tmpvar_5;
  highp vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7 = (_Object2World * _glesVertex);
  tmpvar_6 = tmpvar_7.xyz;
  highp vec4 v_8;
  v_8.x = _World2Object[0].x;
  v_8.y = _World2Object[1].x;
  v_8.z = _World2Object[2].x;
  v_8.w = _World2Object[3].x;
  highp vec4 v_9;
  v_9.x = _World2Object[0].y;
  v_9.y = _World2Object[1].y;
  v_9.z = _World2Object[2].y;
  v_9.w = _World2Object[3].y;
  highp vec4 v_10;
  v_10.x = _World2Object[0].z;
  v_10.y = _World2Object[1].z;
  v_10.z = _World2Object[2].z;
  v_10.w = _World2Object[3].z;
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize(((
    (v_8.xyz * _glesNormal.x)
   + 
    (v_9.xyz * _glesNormal.y)
  ) + (v_10.xyz * _glesNormal.z)));
  worldNormal_3 = tmpvar_11;
  highp mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize((tmpvar_12 * _glesTANGENT.xyz));
  worldTangent_2 = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = (_glesTANGENT.w * unity_WorldTransformParams.w);
  tangentSign_1 = tmpvar_14;
  lowp vec3 tmpvar_15;
  tmpvar_15 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
  highp vec4 tmpvar_16;
  tmpvar_16.x = worldTangent_2.x;
  tmpvar_16.y = tmpvar_15.x;
  tmpvar_16.z = worldNormal_3.x;
  tmpvar_16.w = tmpvar_6.x;
  highp vec4 tmpvar_17;
  tmpvar_17.x = worldTangent_2.y;
  tmpvar_17.y = tmpvar_15.y;
  tmpvar_17.z = worldNormal_3.y;
  tmpvar_17.w = tmpvar_6.y;
  highp vec4 tmpvar_18;
  tmpvar_18.x = worldTangent_2.z;
  tmpvar_18.y = tmpvar_15.z;
  tmpvar_18.z = worldNormal_3.z;
  tmpvar_18.w = tmpvar_6.z;
  lowp vec4 tmpvar_19;
  tmpvar_19.w = 1.0;
  tmpvar_19.xyz = worldNormal_3;
  mediump vec4 normal_20;
  normal_20 = tmpvar_19;
  mediump vec3 x2_21;
  mediump vec3 x1_22;
  x1_22.x = dot (unity_SHAr, normal_20);
  x1_22.y = dot (unity_SHAg, normal_20);
  x1_22.z = dot (unity_SHAb, normal_20);
  mediump vec4 tmpvar_23;
  tmpvar_23 = (normal_20.xyzz * normal_20.yzzx);
  x2_21.x = dot (unity_SHBr, tmpvar_23);
  x2_21.y = dot (unity_SHBg, tmpvar_23);
  x2_21.z = dot (unity_SHBb, tmpvar_23);
  highp vec3 lightColor0_24;
  lightColor0_24 = unity_LightColor[0].xyz;
  highp vec3 lightColor1_25;
  lightColor1_25 = unity_LightColor[1].xyz;
  highp vec3 lightColor2_26;
  lightColor2_26 = unity_LightColor[2].xyz;
  highp vec3 lightColor3_27;
  lightColor3_27 = unity_LightColor[3].xyz;
  highp vec4 lightAttenSq_28;
  lightAttenSq_28 = unity_4LightAtten0;
  highp vec3 normal_29;
  normal_29 = worldNormal_3;
  highp vec3 col_30;
  highp vec4 ndotl_31;
  highp vec4 lengthSq_32;
  highp vec4 tmpvar_33;
  tmpvar_33 = (unity_4LightPosX0 - tmpvar_7.x);
  highp vec4 tmpvar_34;
  tmpvar_34 = (unity_4LightPosY0 - tmpvar_7.y);
  highp vec4 tmpvar_35;
  tmpvar_35 = (unity_4LightPosZ0 - tmpvar_7.z);
  lengthSq_32 = (tmpvar_33 * tmpvar_33);
  lengthSq_32 = (lengthSq_32 + (tmpvar_34 * tmpvar_34));
  lengthSq_32 = (lengthSq_32 + (tmpvar_35 * tmpvar_35));
  ndotl_31 = (tmpvar_33 * normal_29.x);
  ndotl_31 = (ndotl_31 + (tmpvar_34 * normal_29.y));
  ndotl_31 = (ndotl_31 + (tmpvar_35 * normal_29.z));
  highp vec4 tmpvar_36;
  tmpvar_36 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_31 * inversesqrt(lengthSq_32)));
  ndotl_31 = tmpvar_36;
  highp vec4 tmpvar_37;
  tmpvar_37 = (tmpvar_36 * (1.0/((1.0 + 
    (lengthSq_32 * lightAttenSq_28)
  ))));
  col_30 = (lightColor0_24 * tmpvar_37.x);
  col_30 = (col_30 + (lightColor1_25 * tmpvar_37.y));
  col_30 = (col_30 + (lightColor2_26 * tmpvar_37.z));
  col_30 = (col_30 + (lightColor3_27 * tmpvar_37.w));
  tmpvar_4 = (((x2_21 + 
    (unity_SHC.xyz * ((normal_20.x * normal_20.x) - (normal_20.y * normal_20.y)))
  ) + x1_22) + col_30);
  tmpvar_5 = (unity_World2Shadow[0] * tmpvar_7);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_16;
  xlv_TEXCOORD2 = tmpvar_17;
  xlv_TEXCOORD3 = tmpvar_18;
  xlv_TEXCOORD4 = tmpvar_4;
  xlv_TEXCOORD5 = tmpvar_5;
}


#endif
#ifdef FRAGMENT
uniform highp vec3 _WorldSpaceCameraPos;
uniform mediump vec4 _WorldSpaceLightPos0;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpTex;
uniform sampler2D _DetailNormalTex;
uniform sampler2D _MaskTex;
uniform highp float _DetailScale;
uniform highp vec4 _SkinColor;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  mediump float tmpvar_1;
  mediump vec3 tmpvar_2;
  mediump vec3 tmpvar_3;
  lowp vec3 worldN_4;
  lowp vec4 c_5;
  lowp vec3 worldViewDir_6;
  lowp vec3 lightDir_7;
  highp vec3 tmpvar_8;
  tmpvar_8.x = xlv_TEXCOORD1.w;
  tmpvar_8.y = xlv_TEXCOORD2.w;
  tmpvar_8.z = xlv_TEXCOORD3.w;
  mediump vec3 tmpvar_9;
  tmpvar_9 = _WorldSpaceLightPos0.xyz;
  lightDir_7 = tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - tmpvar_8));
  worldViewDir_6 = tmpvar_10;
  lowp vec3 tmpvar_11;
  lowp vec3 tmpvar_12;
  lowp float tmpvar_13;
  lowp float tmpvar_14;
  mediump vec3 n_sample_15;
  mediump vec3 n_detail_16;
  mediump vec4 m_17;
  mediump vec4 c_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_18 = tmpvar_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_MaskTex, xlv_TEXCOORD0);
  m_17 = tmpvar_20;
  highp vec2 P_21;
  P_21 = (xlv_TEXCOORD0 * _DetailScale);
  lowp vec3 tmpvar_22;
  tmpvar_22 = ((texture2D (_DetailNormalTex, P_21).xyz * 2.0) - 1.0);
  n_detail_16 = tmpvar_22;
  lowp vec3 tmpvar_23;
  tmpvar_23 = ((texture2D (_BumpTex, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  n_sample_15 = tmpvar_23;
  tmpvar_11 = (c_18.xyz * _SkinColor.xyz);
  tmpvar_14 = c_18.w;
  tmpvar_13 = m_17.y;
  mediump vec3 tmpvar_24;
  tmpvar_24.z = 0.0;
  tmpvar_24.xy = n_detail_16.xy;
  mediump vec3 tmpvar_25;
  tmpvar_25 = normalize((n_sample_15 + (tmpvar_24 * m_17.x)));
  tmpvar_12 = tmpvar_25;
  lowp float tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD5).x > 
    (xlv_TEXCOORD5.z / xlv_TEXCOORD5.w)
  )), _LightShadowData.x);
  tmpvar_26 = tmpvar_27;
  highp float tmpvar_28;
  tmpvar_28 = dot (xlv_TEXCOORD1.xyz, tmpvar_12);
  worldN_4.x = tmpvar_28;
  highp float tmpvar_29;
  tmpvar_29 = dot (xlv_TEXCOORD2.xyz, tmpvar_12);
  worldN_4.y = tmpvar_29;
  highp float tmpvar_30;
  tmpvar_30 = dot (xlv_TEXCOORD3.xyz, tmpvar_12);
  worldN_4.z = tmpvar_30;
  tmpvar_2 = _LightColor0.xyz;
  tmpvar_3 = lightDir_7;
  tmpvar_1 = tmpvar_26;
  mediump vec3 tmpvar_31;
  tmpvar_31 = (tmpvar_2 * tmpvar_1);
  tmpvar_2 = tmpvar_31;
  mediump vec3 viewDir_32;
  viewDir_32 = worldViewDir_6;
  lowp vec4 c_33;
  lowp vec4 c_34;
  highp float nh_35;
  lowp float diff_36;
  mediump float tmpvar_37;
  tmpvar_37 = max (0.0, dot (worldN_4, tmpvar_3));
  diff_36 = tmpvar_37;
  mediump float tmpvar_38;
  tmpvar_38 = max (0.0, dot (worldN_4, normalize(
    (tmpvar_3 + viewDir_32)
  )));
  nh_35 = tmpvar_38;
  highp float tmpvar_39;
  tmpvar_39 = (pow (nh_35, 128.0) * tmpvar_13);
  c_34.xyz = (((tmpvar_11 * tmpvar_31) * diff_36) + ((tmpvar_31 * _SpecColor.xyz) * tmpvar_39));
  c_34.w = tmpvar_14;
  c_33.w = c_34.w;
  c_33.xyz = (c_34.xyz + (tmpvar_11 * xlv_TEXCOORD4));
  c_5.xyz = c_33.xyz;
  c_5.w = 1.0;
  gl_FragData[0] = c_5;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_WorldTransformParams;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp float tangentSign_1;
  lowp vec3 worldTangent_2;
  lowp vec3 worldNormal_3;
  mediump vec4 tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6 = (_Object2World * _glesVertex);
  tmpvar_5 = tmpvar_6.xyz;
  highp vec4 v_7;
  v_7.x = _World2Object[0].x;
  v_7.y = _World2Object[1].x;
  v_7.z = _World2Object[2].x;
  v_7.w = _World2Object[3].x;
  highp vec4 v_8;
  v_8.x = _World2Object[0].y;
  v_8.y = _World2Object[1].y;
  v_8.z = _World2Object[2].y;
  v_8.w = _World2Object[3].y;
  highp vec4 v_9;
  v_9.x = _World2Object[0].z;
  v_9.y = _World2Object[1].z;
  v_9.z = _World2Object[2].z;
  v_9.w = _World2Object[3].z;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize(((
    (v_7.xyz * _glesNormal.x)
   + 
    (v_8.xyz * _glesNormal.y)
  ) + (v_9.xyz * _glesNormal.z)));
  worldNormal_3 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesTANGENT.xyz));
  worldTangent_2 = tmpvar_12;
  highp float tmpvar_13;
  tmpvar_13 = (_glesTANGENT.w * unity_WorldTransformParams.w);
  tangentSign_1 = tmpvar_13;
  lowp vec3 tmpvar_14;
  tmpvar_14 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
  highp vec4 tmpvar_15;
  tmpvar_15.x = worldTangent_2.x;
  tmpvar_15.y = tmpvar_14.x;
  tmpvar_15.z = worldNormal_3.x;
  tmpvar_15.w = tmpvar_5.x;
  highp vec4 tmpvar_16;
  tmpvar_16.x = worldTangent_2.y;
  tmpvar_16.y = tmpvar_14.y;
  tmpvar_16.z = worldNormal_3.y;
  tmpvar_16.w = tmpvar_5.y;
  highp vec4 tmpvar_17;
  tmpvar_17.x = worldTangent_2.z;
  tmpvar_17.y = tmpvar_14.z;
  tmpvar_17.z = worldNormal_3.z;
  tmpvar_17.w = tmpvar_5.z;
  lowp vec4 tmpvar_18;
  tmpvar_18.w = 1.0;
  tmpvar_18.xyz = worldNormal_3;
  mediump vec4 normal_19;
  normal_19 = tmpvar_18;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  x1_21.x = dot (unity_SHAr, normal_19);
  x1_21.y = dot (unity_SHAg, normal_19);
  x1_21.z = dot (unity_SHAb, normal_19);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_19.xyzz * normal_19.yzzx);
  x2_20.x = dot (unity_SHBr, tmpvar_22);
  x2_20.y = dot (unity_SHBg, tmpvar_22);
  x2_20.z = dot (unity_SHBb, tmpvar_22);
  tmpvar_4 = (unity_World2Shadow[0] * tmpvar_6);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_15;
  xlv_TEXCOORD2 = tmpvar_16;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = ((x2_20 + (unity_SHC.xyz * 
    ((normal_19.x * normal_19.x) - (normal_19.y * normal_19.y))
  )) + x1_21);
  xlv_TEXCOORD5 = tmpvar_4;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform highp vec3 _WorldSpaceCameraPos;
uniform mediump vec4 _WorldSpaceLightPos0;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpTex;
uniform sampler2D _DetailNormalTex;
uniform sampler2D _MaskTex;
uniform highp float _DetailScale;
uniform highp vec4 _SkinColor;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  mediump float tmpvar_1;
  mediump vec3 tmpvar_2;
  mediump vec3 tmpvar_3;
  lowp vec3 worldN_4;
  lowp vec4 c_5;
  lowp vec3 worldViewDir_6;
  lowp vec3 lightDir_7;
  highp vec3 tmpvar_8;
  tmpvar_8.x = xlv_TEXCOORD1.w;
  tmpvar_8.y = xlv_TEXCOORD2.w;
  tmpvar_8.z = xlv_TEXCOORD3.w;
  mediump vec3 tmpvar_9;
  tmpvar_9 = _WorldSpaceLightPos0.xyz;
  lightDir_7 = tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - tmpvar_8));
  worldViewDir_6 = tmpvar_10;
  lowp vec3 tmpvar_11;
  lowp vec3 tmpvar_12;
  lowp float tmpvar_13;
  lowp float tmpvar_14;
  mediump vec3 n_sample_15;
  mediump vec3 n_detail_16;
  mediump vec4 m_17;
  mediump vec4 c_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_18 = tmpvar_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_MaskTex, xlv_TEXCOORD0);
  m_17 = tmpvar_20;
  highp vec2 P_21;
  P_21 = (xlv_TEXCOORD0 * _DetailScale);
  lowp vec3 tmpvar_22;
  tmpvar_22 = ((texture2D (_DetailNormalTex, P_21).xyz * 2.0) - 1.0);
  n_detail_16 = tmpvar_22;
  lowp vec3 tmpvar_23;
  tmpvar_23 = ((texture2D (_BumpTex, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  n_sample_15 = tmpvar_23;
  tmpvar_11 = (c_18.xyz * _SkinColor.xyz);
  tmpvar_14 = c_18.w;
  tmpvar_13 = m_17.y;
  mediump vec3 tmpvar_24;
  tmpvar_24.z = 0.0;
  tmpvar_24.xy = n_detail_16.xy;
  mediump vec3 tmpvar_25;
  tmpvar_25 = normalize((n_sample_15 + (tmpvar_24 * m_17.x)));
  tmpvar_12 = tmpvar_25;
  lowp float shadow_26;
  shadow_26 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD5.xyz) * (1.0 - _LightShadowData.x)));
  highp float tmpvar_27;
  tmpvar_27 = dot (xlv_TEXCOORD1.xyz, tmpvar_12);
  worldN_4.x = tmpvar_27;
  highp float tmpvar_28;
  tmpvar_28 = dot (xlv_TEXCOORD2.xyz, tmpvar_12);
  worldN_4.y = tmpvar_28;
  highp float tmpvar_29;
  tmpvar_29 = dot (xlv_TEXCOORD3.xyz, tmpvar_12);
  worldN_4.z = tmpvar_29;
  tmpvar_2 = _LightColor0.xyz;
  tmpvar_3 = lightDir_7;
  tmpvar_1 = shadow_26;
  mediump vec3 tmpvar_30;
  tmpvar_30 = (tmpvar_2 * tmpvar_1);
  tmpvar_2 = tmpvar_30;
  mediump vec3 viewDir_31;
  viewDir_31 = worldViewDir_6;
  lowp vec4 c_32;
  lowp vec4 c_33;
  highp float nh_34;
  lowp float diff_35;
  mediump float tmpvar_36;
  tmpvar_36 = max (0.0, dot (worldN_4, tmpvar_3));
  diff_35 = tmpvar_36;
  mediump float tmpvar_37;
  tmpvar_37 = max (0.0, dot (worldN_4, normalize(
    (tmpvar_3 + viewDir_31)
  )));
  nh_34 = tmpvar_37;
  highp float tmpvar_38;
  tmpvar_38 = (pow (nh_34, 128.0) * tmpvar_13);
  c_33.xyz = (((tmpvar_11 * tmpvar_30) * diff_35) + ((tmpvar_30 * _SpecColor.xyz) * tmpvar_38));
  c_33.w = tmpvar_14;
  c_32.w = c_33.w;
  c_32.xyz = (c_33.xyz + (tmpvar_11 * xlv_TEXCOORD4));
  c_5.xyz = c_32.xyz;
  c_5.w = 1.0;
  gl_FragData[0] = c_5;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
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
uniform 	mediump vec4 _WorldSpaceLightPos0;
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
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
out highp vec4 vs_TEXCOORD2;
out highp vec4 vs_TEXCOORD3;
out mediump vec3 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
highp vec4 t0;
mediump vec4 t16_0;
highp vec4 t1;
highp vec4 t2;
highp vec3 t3;
lowp vec3 t10_4;
mediump vec3 t16_5;
mediump vec3 t16_6;
highp float t21;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    vs_TEXCOORD1.w = t0.x;
    t0.x = in_TANGENT0.w * unity_WorldTransformParams.w;
    t1.xyz = in_TANGENT0.yyy * _Object2World[1].yzx;
    t1.xyz = _Object2World[0].yzx * in_TANGENT0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].yzx * in_TANGENT0.zzz + t1.xyz;
    t21 = dot(t1.xyz, t1.xyz);
    t21 = inversesqrt(t21);
    t1.xyz = vec3(t21) * t1.xyz;
    t2.x = in_NORMAL0.x * _World2Object[0].x;
    t2.y = in_NORMAL0.x * _World2Object[1].x;
    t2.z = in_NORMAL0.x * _World2Object[2].x;
    t3.x = in_NORMAL0.y * _World2Object[0].y;
    t3.y = in_NORMAL0.y * _World2Object[1].y;
    t3.z = in_NORMAL0.y * _World2Object[2].y;
    t2.xyz = t2.xyz + t3.xyz;
    t3.x = in_NORMAL0.z * _World2Object[0].z;
    t3.y = in_NORMAL0.z * _World2Object[1].z;
    t3.z = in_NORMAL0.z * _World2Object[2].z;
    t2.xyz = t2.xyz + t3.xyz;
    t21 = dot(t2.xyz, t2.xyz);
    t21 = inversesqrt(t21);
    t2.xyz = vec3(t21) * t2.xyz;
    t10_4.xyz = t1.xyz * t2.zxy;
    t10_4.xyz = t2.yzx * t1.yzx + (-t10_4.xyz);
    t10_4.xyz = t0.xxx * t10_4.xyz;
    vs_TEXCOORD1.y = t10_4.x;
    vs_TEXCOORD1.x = t1.z;
    vs_TEXCOORD1.z = t2.x;
    vs_TEXCOORD2.x = t1.x;
    vs_TEXCOORD3.x = t1.y;
    vs_TEXCOORD2.w = t0.y;
    vs_TEXCOORD3.w = t0.z;
    vs_TEXCOORD2.y = t10_4.y;
    vs_TEXCOORD3.y = t10_4.z;
    vs_TEXCOORD2.z = t2.y;
    vs_TEXCOORD3.z = t2.z;
    t16_5.x = t2.y * t2.y;
    t16_5.x = t2.x * t2.x + (-t16_5.x);
    t16_0 = t2.yzzx * t2.xyzz;
    t16_6.x = dot(unity_SHBr, t16_0);
    t16_6.y = dot(unity_SHBg, t16_0);
    t16_6.z = dot(unity_SHBb, t16_0);
    t16_5.xyz = unity_SHC.xyz * t16_5.xxx + t16_6.xyz;
    t2.w = 1.0;
    t16_6.x = dot(unity_SHAr, t2);
    t16_6.y = dot(unity_SHAg, t2);
    t16_6.z = dot(unity_SHAb, t2);
    vs_TEXCOORD4.xyz = t16_5.xyz + t16_6.xyz;
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1 = t0.yyyy * unity_World2Shadow[0][1];
    t1 = unity_World2Shadow[0][0] * t0.xxxx + t1;
    t1 = unity_World2Shadow[0][2] * t0.zzzz + t1;
    t0 = unity_World2Shadow[0][3] * t0.wwww + t1;
    vs_TEXCOORD5 = t0;
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
uniform 	mediump vec4 _WorldSpaceLightPos0;
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
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _DetailNormalTex;
uniform lowp sampler2D _BumpTex;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
in highp vec4 vs_TEXCOORD2;
in highp vec4 vs_TEXCOORD3;
in mediump vec3 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec3 t0;
mediump vec3 t16_0;
lowp vec3 t10_0;
mediump vec4 t16_1;
lowp vec3 t10_2;
lowp vec2 t10_3;
mediump vec3 t16_4;
highp vec3 t5;
lowp vec3 t10_5;
mediump float t16_13;
highp float t18;
mediump float t16_19;
void main()
{
    t0.x = vs_TEXCOORD1.w;
    t0.y = vs_TEXCOORD2.w;
    t0.z = vs_TEXCOORD3.w;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t0.xyz, t0.xyz);
    t18 = inversesqrt(t18);
    t16_1.xyz = t0.xyz * vec3(t18) + _WorldSpaceLightPos0.xyz;
    t16_19 = dot(t16_1.xyz, t16_1.xyz);
    t16_19 = inversesqrt(t16_19);
    t16_1.xyz = vec3(t16_19) * t16_1.xyz;
    t10_0.xyz = texture(_BumpTex, vs_TEXCOORD0.xy).xyz;
    t10_2.xyz = t10_0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t0.xy = vs_TEXCOORD0.xy * vec2(_DetailScale);
    t10_0.xy = texture(_DetailNormalTex, t0.xy).xy;
    t10_3.xy = t10_0.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
    t10_0.xy = texture(_MaskTex, vs_TEXCOORD0.xy).xy;
    t16_4.xy = t10_0.xx * t10_3.xy;
    t16_4.z = 0.0;
    t16_4.xyz = t10_2.xyz + t16_4.xyz;
    t16_19 = dot(t16_4.xyz, t16_4.xyz);
    t16_19 = inversesqrt(t16_19);
    t16_4.xyz = vec3(t16_19) * t16_4.xyz;
    t5.x = dot(vs_TEXCOORD1.xyz, t16_4.xyz);
    t5.y = dot(vs_TEXCOORD2.xyz, t16_4.xyz);
    t5.z = dot(vs_TEXCOORD3.xyz, t16_4.xyz);
    t16_1.x = dot(t5.xyz, t16_1.xyz);
    t16_1.y = dot(t5.xyz, _WorldSpaceLightPos0.xyz);
    t16_1.xy = max(t16_1.xy, vec2(0.0, 0.0));
    t16_0.x = log2(t16_1.x);
    t16_0.x = t16_0.x * 128.0;
    t16_0.x = exp2(t16_0.x);
    t16_0.x = t10_0.y * t16_0.x;
    vec3 txVec0 = vec3(vs_TEXCOORD5.xy,vs_TEXCOORD5.z);
    t16_1.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec0, 0.0);
    t16_13 = (-_LightShadowData.x) + 1.0;
    t16_1.x = t16_1.x * t16_13 + _LightShadowData.x;
    t16_1.xzw = t16_1.xxx * _LightColor0.xyz;
    t16_4.xyz = t16_1.xzw * _SpecColor.xyz;
    t16_0.xyz = t16_0.xxx * t16_4.xyz;
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t5.xyz = t10_5.xyz * _SkinColor.xyz;
    t16_1.xzw = t16_1.xzw * t5.xyz;
    t16_0.xyz = t16_1.xzw * t16_1.yyy + t16_0.xyz;
    t16_1.xyz = t5.xyz * vs_TEXCOORD4.xyz + t16_0.xyz;
    SV_Target0.xyz = t16_1.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform mediump vec4 unity_4LightAtten0;
uniform mediump vec4 unity_LightColor[8];
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_WorldTransformParams;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp float tangentSign_1;
  lowp vec3 worldTangent_2;
  lowp vec3 worldNormal_3;
  mediump vec3 tmpvar_4;
  mediump vec4 tmpvar_5;
  highp vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7 = (_Object2World * _glesVertex);
  tmpvar_6 = tmpvar_7.xyz;
  highp vec4 v_8;
  v_8.x = _World2Object[0].x;
  v_8.y = _World2Object[1].x;
  v_8.z = _World2Object[2].x;
  v_8.w = _World2Object[3].x;
  highp vec4 v_9;
  v_9.x = _World2Object[0].y;
  v_9.y = _World2Object[1].y;
  v_9.z = _World2Object[2].y;
  v_9.w = _World2Object[3].y;
  highp vec4 v_10;
  v_10.x = _World2Object[0].z;
  v_10.y = _World2Object[1].z;
  v_10.z = _World2Object[2].z;
  v_10.w = _World2Object[3].z;
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize(((
    (v_8.xyz * _glesNormal.x)
   + 
    (v_9.xyz * _glesNormal.y)
  ) + (v_10.xyz * _glesNormal.z)));
  worldNormal_3 = tmpvar_11;
  highp mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize((tmpvar_12 * _glesTANGENT.xyz));
  worldTangent_2 = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = (_glesTANGENT.w * unity_WorldTransformParams.w);
  tangentSign_1 = tmpvar_14;
  lowp vec3 tmpvar_15;
  tmpvar_15 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
  highp vec4 tmpvar_16;
  tmpvar_16.x = worldTangent_2.x;
  tmpvar_16.y = tmpvar_15.x;
  tmpvar_16.z = worldNormal_3.x;
  tmpvar_16.w = tmpvar_6.x;
  highp vec4 tmpvar_17;
  tmpvar_17.x = worldTangent_2.y;
  tmpvar_17.y = tmpvar_15.y;
  tmpvar_17.z = worldNormal_3.y;
  tmpvar_17.w = tmpvar_6.y;
  highp vec4 tmpvar_18;
  tmpvar_18.x = worldTangent_2.z;
  tmpvar_18.y = tmpvar_15.z;
  tmpvar_18.z = worldNormal_3.z;
  tmpvar_18.w = tmpvar_6.z;
  lowp vec4 tmpvar_19;
  tmpvar_19.w = 1.0;
  tmpvar_19.xyz = worldNormal_3;
  mediump vec4 normal_20;
  normal_20 = tmpvar_19;
  mediump vec3 x2_21;
  mediump vec3 x1_22;
  x1_22.x = dot (unity_SHAr, normal_20);
  x1_22.y = dot (unity_SHAg, normal_20);
  x1_22.z = dot (unity_SHAb, normal_20);
  mediump vec4 tmpvar_23;
  tmpvar_23 = (normal_20.xyzz * normal_20.yzzx);
  x2_21.x = dot (unity_SHBr, tmpvar_23);
  x2_21.y = dot (unity_SHBg, tmpvar_23);
  x2_21.z = dot (unity_SHBb, tmpvar_23);
  highp vec3 lightColor0_24;
  lightColor0_24 = unity_LightColor[0].xyz;
  highp vec3 lightColor1_25;
  lightColor1_25 = unity_LightColor[1].xyz;
  highp vec3 lightColor2_26;
  lightColor2_26 = unity_LightColor[2].xyz;
  highp vec3 lightColor3_27;
  lightColor3_27 = unity_LightColor[3].xyz;
  highp vec4 lightAttenSq_28;
  lightAttenSq_28 = unity_4LightAtten0;
  highp vec3 normal_29;
  normal_29 = worldNormal_3;
  highp vec3 col_30;
  highp vec4 ndotl_31;
  highp vec4 lengthSq_32;
  highp vec4 tmpvar_33;
  tmpvar_33 = (unity_4LightPosX0 - tmpvar_7.x);
  highp vec4 tmpvar_34;
  tmpvar_34 = (unity_4LightPosY0 - tmpvar_7.y);
  highp vec4 tmpvar_35;
  tmpvar_35 = (unity_4LightPosZ0 - tmpvar_7.z);
  lengthSq_32 = (tmpvar_33 * tmpvar_33);
  lengthSq_32 = (lengthSq_32 + (tmpvar_34 * tmpvar_34));
  lengthSq_32 = (lengthSq_32 + (tmpvar_35 * tmpvar_35));
  ndotl_31 = (tmpvar_33 * normal_29.x);
  ndotl_31 = (ndotl_31 + (tmpvar_34 * normal_29.y));
  ndotl_31 = (ndotl_31 + (tmpvar_35 * normal_29.z));
  highp vec4 tmpvar_36;
  tmpvar_36 = max (vec4(0.0, 0.0, 0.0, 0.0), (ndotl_31 * inversesqrt(lengthSq_32)));
  ndotl_31 = tmpvar_36;
  highp vec4 tmpvar_37;
  tmpvar_37 = (tmpvar_36 * (1.0/((1.0 + 
    (lengthSq_32 * lightAttenSq_28)
  ))));
  col_30 = (lightColor0_24 * tmpvar_37.x);
  col_30 = (col_30 + (lightColor1_25 * tmpvar_37.y));
  col_30 = (col_30 + (lightColor2_26 * tmpvar_37.z));
  col_30 = (col_30 + (lightColor3_27 * tmpvar_37.w));
  tmpvar_4 = (((x2_21 + 
    (unity_SHC.xyz * ((normal_20.x * normal_20.x) - (normal_20.y * normal_20.y)))
  ) + x1_22) + col_30);
  tmpvar_5 = (unity_World2Shadow[0] * tmpvar_7);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_16;
  xlv_TEXCOORD2 = tmpvar_17;
  xlv_TEXCOORD3 = tmpvar_18;
  xlv_TEXCOORD4 = tmpvar_4;
  xlv_TEXCOORD5 = tmpvar_5;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform highp vec3 _WorldSpaceCameraPos;
uniform mediump vec4 _WorldSpaceLightPos0;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpTex;
uniform sampler2D _DetailNormalTex;
uniform sampler2D _MaskTex;
uniform highp float _DetailScale;
uniform highp vec4 _SkinColor;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  mediump float tmpvar_1;
  mediump vec3 tmpvar_2;
  mediump vec3 tmpvar_3;
  lowp vec3 worldN_4;
  lowp vec4 c_5;
  lowp vec3 worldViewDir_6;
  lowp vec3 lightDir_7;
  highp vec3 tmpvar_8;
  tmpvar_8.x = xlv_TEXCOORD1.w;
  tmpvar_8.y = xlv_TEXCOORD2.w;
  tmpvar_8.z = xlv_TEXCOORD3.w;
  mediump vec3 tmpvar_9;
  tmpvar_9 = _WorldSpaceLightPos0.xyz;
  lightDir_7 = tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - tmpvar_8));
  worldViewDir_6 = tmpvar_10;
  lowp vec3 tmpvar_11;
  lowp vec3 tmpvar_12;
  lowp float tmpvar_13;
  lowp float tmpvar_14;
  mediump vec3 n_sample_15;
  mediump vec3 n_detail_16;
  mediump vec4 m_17;
  mediump vec4 c_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_18 = tmpvar_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_MaskTex, xlv_TEXCOORD0);
  m_17 = tmpvar_20;
  highp vec2 P_21;
  P_21 = (xlv_TEXCOORD0 * _DetailScale);
  lowp vec3 tmpvar_22;
  tmpvar_22 = ((texture2D (_DetailNormalTex, P_21).xyz * 2.0) - 1.0);
  n_detail_16 = tmpvar_22;
  lowp vec3 tmpvar_23;
  tmpvar_23 = ((texture2D (_BumpTex, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  n_sample_15 = tmpvar_23;
  tmpvar_11 = (c_18.xyz * _SkinColor.xyz);
  tmpvar_14 = c_18.w;
  tmpvar_13 = m_17.y;
  mediump vec3 tmpvar_24;
  tmpvar_24.z = 0.0;
  tmpvar_24.xy = n_detail_16.xy;
  mediump vec3 tmpvar_25;
  tmpvar_25 = normalize((n_sample_15 + (tmpvar_24 * m_17.x)));
  tmpvar_12 = tmpvar_25;
  lowp float shadow_26;
  shadow_26 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD5.xyz) * (1.0 - _LightShadowData.x)));
  highp float tmpvar_27;
  tmpvar_27 = dot (xlv_TEXCOORD1.xyz, tmpvar_12);
  worldN_4.x = tmpvar_27;
  highp float tmpvar_28;
  tmpvar_28 = dot (xlv_TEXCOORD2.xyz, tmpvar_12);
  worldN_4.y = tmpvar_28;
  highp float tmpvar_29;
  tmpvar_29 = dot (xlv_TEXCOORD3.xyz, tmpvar_12);
  worldN_4.z = tmpvar_29;
  tmpvar_2 = _LightColor0.xyz;
  tmpvar_3 = lightDir_7;
  tmpvar_1 = shadow_26;
  mediump vec3 tmpvar_30;
  tmpvar_30 = (tmpvar_2 * tmpvar_1);
  tmpvar_2 = tmpvar_30;
  mediump vec3 viewDir_31;
  viewDir_31 = worldViewDir_6;
  lowp vec4 c_32;
  lowp vec4 c_33;
  highp float nh_34;
  lowp float diff_35;
  mediump float tmpvar_36;
  tmpvar_36 = max (0.0, dot (worldN_4, tmpvar_3));
  diff_35 = tmpvar_36;
  mediump float tmpvar_37;
  tmpvar_37 = max (0.0, dot (worldN_4, normalize(
    (tmpvar_3 + viewDir_31)
  )));
  nh_34 = tmpvar_37;
  highp float tmpvar_38;
  tmpvar_38 = (pow (nh_34, 128.0) * tmpvar_13);
  c_33.xyz = (((tmpvar_11 * tmpvar_30) * diff_35) + ((tmpvar_30 * _SpecColor.xyz) * tmpvar_38));
  c_33.w = tmpvar_14;
  c_32.w = c_33.w;
  c_32.xyz = (c_33.xyz + (tmpvar_11 * xlv_TEXCOORD4));
  c_5.xyz = c_32.xyz;
  c_5.w = 1.0;
  gl_FragData[0] = c_5;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
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
uniform 	mediump vec4 _WorldSpaceLightPos0;
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
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
out highp vec4 vs_TEXCOORD2;
out highp vec4 vs_TEXCOORD3;
out mediump vec3 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
highp vec4 t0;
mediump vec4 t16_0;
highp vec4 t1;
highp vec4 t2;
highp vec4 t3;
lowp vec3 t10_3;
mediump vec3 t16_4;
highp vec4 t5;
mediump vec3 t16_5;
highp float t18;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.xyz = in_TANGENT0.yyy * _Object2World[1].yzx;
    t0.xyz = _Object2World[0].yzx * in_TANGENT0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].yzx * in_TANGENT0.zzz + t0.xyz;
    t18 = dot(t0.xyz, t0.xyz);
    t18 = inversesqrt(t18);
    t0.xyz = vec3(t18) * t0.xyz;
    vs_TEXCOORD1.x = t0.z;
    t18 = in_TANGENT0.w * unity_WorldTransformParams.w;
    t1.x = in_NORMAL0.x * _World2Object[0].x;
    t1.y = in_NORMAL0.x * _World2Object[1].x;
    t1.z = in_NORMAL0.x * _World2Object[2].x;
    t2.x = in_NORMAL0.y * _World2Object[0].y;
    t2.y = in_NORMAL0.y * _World2Object[1].y;
    t2.z = in_NORMAL0.y * _World2Object[2].y;
    t1.xyz = t1.xyz + t2.xyz;
    t2.x = in_NORMAL0.z * _World2Object[0].z;
    t2.y = in_NORMAL0.z * _World2Object[1].z;
    t2.z = in_NORMAL0.z * _World2Object[2].z;
    t1.xyz = t1.xyz + t2.xyz;
    t19 = dot(t1.xyz, t1.xyz);
    t19 = inversesqrt(t19);
    t1.xyz = vec3(t19) * t1.xyz;
    t10_3.xyz = t0.xyz * t1.zxy;
    t10_3.xyz = t1.yzx * t0.yzx + (-t10_3.xyz);
    t10_3.xyz = vec3(t18) * t10_3.xyz;
    vs_TEXCOORD1.y = t10_3.x;
    vs_TEXCOORD1.z = t1.x;
    t2.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t2.xyz;
    t2.xyz = _Object2World[3].xyz * in_POSITION0.www + t2.xyz;
    vs_TEXCOORD1.w = t2.x;
    vs_TEXCOORD2.x = t0.x;
    vs_TEXCOORD3.x = t0.y;
    vs_TEXCOORD2.y = t10_3.y;
    vs_TEXCOORD3.y = t10_3.z;
    vs_TEXCOORD2.z = t1.y;
    vs_TEXCOORD2.w = t2.y;
    vs_TEXCOORD3.w = t2.z;
    vs_TEXCOORD3.z = t1.z;
    t16_4.x = t1.y * t1.y;
    t16_4.x = t1.x * t1.x + (-t16_4.x);
    t16_0 = t1.yzzx * t1.xyzz;
    t16_5.x = dot(unity_SHBr, t16_0);
    t16_5.y = dot(unity_SHBg, t16_0);
    t16_5.z = dot(unity_SHBb, t16_0);
    t16_4.xyz = unity_SHC.xyz * t16_4.xxx + t16_5.xyz;
    t1.w = 1.0;
    t16_5.x = dot(unity_SHAr, t1);
    t16_5.y = dot(unity_SHAg, t1);
    t16_5.z = dot(unity_SHAb, t1);
    t16_4.xyz = t16_4.xyz + t16_5.xyz;
    t0 = (-t2.yyyy) + unity_4LightPosY0;
    t3 = t1.yyyy * t0;
    t0 = t0 * t0;
    t5 = (-t2.xxxx) + unity_4LightPosX0;
    t2 = (-t2.zzzz) + unity_4LightPosZ0;
    t3 = t5 * t1.xxxx + t3;
    t1 = t2 * t1.zzzz + t3;
    t0 = t5 * t5 + t0;
    t0 = t2 * t2 + t0;
    t2 = inversesqrt(t0);
    t0 = t0 * unity_4LightAtten0 + vec4(1.0, 1.0, 1.0, 1.0);
    t0 = vec4(1.0, 1.0, 1.0, 1.0) / t0;
    t1 = t1 * t2;
    t1 = max(t1, vec4(0.0, 0.0, 0.0, 0.0));
    t0 = t0 * t1;
    t1.xyz = t0.yyy * unity_LightColor[1].xyz;
    t1.xyz = unity_LightColor[0].xyz * t0.xxx + t1.xyz;
    t1.xyz = unity_LightColor[2].xyz * t0.zzz + t1.xyz;
    t1.xyz = unity_LightColor[3].xyz * t0.www + t1.xyz;
    t1.xyz = t1.xyz + t16_4.xyz;
    vs_TEXCOORD4.xyz = t1.xyz;
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1 = t0.yyyy * unity_World2Shadow[0][1];
    t1 = unity_World2Shadow[0][0] * t0.xxxx + t1;
    t1 = unity_World2Shadow[0][2] * t0.zzzz + t1;
    t0 = unity_World2Shadow[0][3] * t0.wwww + t1;
    vs_TEXCOORD5 = t0;
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
uniform 	mediump vec4 _WorldSpaceLightPos0;
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
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _DetailNormalTex;
uniform lowp sampler2D _BumpTex;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
in highp vec4 vs_TEXCOORD2;
in highp vec4 vs_TEXCOORD3;
in mediump vec3 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec3 t0;
mediump vec3 t16_0;
lowp vec3 t10_0;
mediump vec4 t16_1;
lowp vec3 t10_2;
lowp vec2 t10_3;
mediump vec3 t16_4;
highp vec3 t5;
lowp vec3 t10_5;
mediump float t16_13;
highp float t18;
mediump float t16_19;
void main()
{
    t0.x = vs_TEXCOORD1.w;
    t0.y = vs_TEXCOORD2.w;
    t0.z = vs_TEXCOORD3.w;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t0.xyz, t0.xyz);
    t18 = inversesqrt(t18);
    t16_1.xyz = t0.xyz * vec3(t18) + _WorldSpaceLightPos0.xyz;
    t16_19 = dot(t16_1.xyz, t16_1.xyz);
    t16_19 = inversesqrt(t16_19);
    t16_1.xyz = vec3(t16_19) * t16_1.xyz;
    t10_0.xyz = texture(_BumpTex, vs_TEXCOORD0.xy).xyz;
    t10_2.xyz = t10_0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t0.xy = vs_TEXCOORD0.xy * vec2(_DetailScale);
    t10_0.xy = texture(_DetailNormalTex, t0.xy).xy;
    t10_3.xy = t10_0.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
    t10_0.xy = texture(_MaskTex, vs_TEXCOORD0.xy).xy;
    t16_4.xy = t10_0.xx * t10_3.xy;
    t16_4.z = 0.0;
    t16_4.xyz = t10_2.xyz + t16_4.xyz;
    t16_19 = dot(t16_4.xyz, t16_4.xyz);
    t16_19 = inversesqrt(t16_19);
    t16_4.xyz = vec3(t16_19) * t16_4.xyz;
    t5.x = dot(vs_TEXCOORD1.xyz, t16_4.xyz);
    t5.y = dot(vs_TEXCOORD2.xyz, t16_4.xyz);
    t5.z = dot(vs_TEXCOORD3.xyz, t16_4.xyz);
    t16_1.x = dot(t5.xyz, t16_1.xyz);
    t16_1.y = dot(t5.xyz, _WorldSpaceLightPos0.xyz);
    t16_1.xy = max(t16_1.xy, vec2(0.0, 0.0));
    t16_0.x = log2(t16_1.x);
    t16_0.x = t16_0.x * 128.0;
    t16_0.x = exp2(t16_0.x);
    t16_0.x = t10_0.y * t16_0.x;
    vec3 txVec0 = vec3(vs_TEXCOORD5.xy,vs_TEXCOORD5.z);
    t16_1.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec0, 0.0);
    t16_13 = (-_LightShadowData.x) + 1.0;
    t16_1.x = t16_1.x * t16_13 + _LightShadowData.x;
    t16_1.xzw = t16_1.xxx * _LightColor0.xyz;
    t16_4.xyz = t16_1.xzw * _SpecColor.xyz;
    t16_0.xyz = t16_0.xxx * t16_4.xyz;
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t5.xyz = t10_5.xyz * _SkinColor.xyz;
    t16_1.xzw = t16_1.xzw * t5.xyz;
    t16_0.xyz = t16_1.xzw * t16_1.yyy + t16_0.xyz;
    t16_1.xyz = t5.xyz * vs_TEXCOORD4.xyz + t16_0.xyz;
    SV_Target0.xyz = t16_1.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
"!!GLES3"
}
}
 }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardAdd" "RenderType"="Opaque" }
  ZWrite Off
  Blend One One
  GpuProgramID 92894
Program "vp" {
SubProgram "gles " {
Keywords { "POINT" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_WorldTransformParams;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp float tangentSign_1;
  lowp vec3 worldTangent_2;
  lowp vec3 worldNormal_3;
  highp vec4 v_4;
  v_4.x = _World2Object[0].x;
  v_4.y = _World2Object[1].x;
  v_4.z = _World2Object[2].x;
  v_4.w = _World2Object[3].x;
  highp vec4 v_5;
  v_5.x = _World2Object[0].y;
  v_5.y = _World2Object[1].y;
  v_5.z = _World2Object[2].y;
  v_5.w = _World2Object[3].y;
  highp vec4 v_6;
  v_6.x = _World2Object[0].z;
  v_6.y = _World2Object[1].z;
  v_6.z = _World2Object[2].z;
  v_6.w = _World2Object[3].z;
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize(((
    (v_4.xyz * _glesNormal.x)
   + 
    (v_5.xyz * _glesNormal.y)
  ) + (v_6.xyz * _glesNormal.z)));
  worldNormal_3 = tmpvar_7;
  highp mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize((tmpvar_8 * _glesTANGENT.xyz));
  worldTangent_2 = tmpvar_9;
  highp float tmpvar_10;
  tmpvar_10 = (_glesTANGENT.w * unity_WorldTransformParams.w);
  tangentSign_1 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
  lowp vec3 tmpvar_12;
  tmpvar_12.x = worldTangent_2.x;
  tmpvar_12.y = tmpvar_11.x;
  tmpvar_12.z = worldNormal_3.x;
  lowp vec3 tmpvar_13;
  tmpvar_13.x = worldTangent_2.y;
  tmpvar_13.y = tmpvar_11.y;
  tmpvar_13.z = worldNormal_3.y;
  lowp vec3 tmpvar_14;
  tmpvar_14.x = worldTangent_2.z;
  tmpvar_14.y = tmpvar_11.z;
  tmpvar_14.z = worldNormal_3.z;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_12;
  xlv_TEXCOORD2 = tmpvar_13;
  xlv_TEXCOORD3 = tmpvar_14;
  xlv_TEXCOORD4 = (_Object2World * _glesVertex).xyz;
}


#endif
#ifdef FRAGMENT
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _WorldSpaceLightPos0;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _LightTexture0;
uniform mediump mat4 _LightMatrix0;
uniform sampler2D _MainTex;
uniform sampler2D _BumpTex;
uniform sampler2D _DetailNormalTex;
uniform sampler2D _MaskTex;
uniform highp float _DetailScale;
uniform highp vec4 _SkinColor;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  mediump vec3 tmpvar_1;
  mediump vec3 tmpvar_2;
  lowp vec3 worldN_3;
  lowp vec4 c_4;
  lowp vec3 worldViewDir_5;
  lowp vec3 lightDir_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD4));
  lightDir_6 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD4));
  worldViewDir_5 = tmpvar_8;
  lowp vec3 tmpvar_9;
  lowp vec3 tmpvar_10;
  lowp float tmpvar_11;
  lowp float tmpvar_12;
  mediump vec3 n_sample_13;
  mediump vec3 n_detail_14;
  mediump vec4 m_15;
  mediump vec4 c_16;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_16 = tmpvar_17;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_MaskTex, xlv_TEXCOORD0);
  m_15 = tmpvar_18;
  highp vec2 P_19;
  P_19 = (xlv_TEXCOORD0 * _DetailScale);
  lowp vec3 tmpvar_20;
  tmpvar_20 = ((texture2D (_DetailNormalTex, P_19).xyz * 2.0) - 1.0);
  n_detail_14 = tmpvar_20;
  lowp vec3 tmpvar_21;
  tmpvar_21 = ((texture2D (_BumpTex, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  n_sample_13 = tmpvar_21;
  tmpvar_9 = (c_16.xyz * _SkinColor.xyz);
  tmpvar_12 = c_16.w;
  tmpvar_11 = m_15.y;
  mediump vec3 tmpvar_22;
  tmpvar_22.z = 0.0;
  tmpvar_22.xy = n_detail_14.xy;
  mediump vec3 tmpvar_23;
  tmpvar_23 = normalize((n_sample_13 + (tmpvar_22 * m_15.x)));
  tmpvar_10 = tmpvar_23;
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = xlv_TEXCOORD4;
  highp vec3 tmpvar_25;
  tmpvar_25 = (_LightMatrix0 * tmpvar_24).xyz;
  highp float tmpvar_26;
  tmpvar_26 = dot (tmpvar_25, tmpvar_25);
  lowp float tmpvar_27;
  tmpvar_27 = texture2D (_LightTexture0, vec2(tmpvar_26)).w;
  worldN_3.x = dot (xlv_TEXCOORD1, tmpvar_10);
  worldN_3.y = dot (xlv_TEXCOORD2, tmpvar_10);
  worldN_3.z = dot (xlv_TEXCOORD3, tmpvar_10);
  tmpvar_1 = _LightColor0.xyz;
  tmpvar_2 = lightDir_6;
  tmpvar_1 = (tmpvar_1 * tmpvar_27);
  mediump vec3 viewDir_28;
  viewDir_28 = worldViewDir_5;
  lowp vec4 c_29;
  lowp vec4 c_30;
  highp float nh_31;
  lowp float diff_32;
  mediump float tmpvar_33;
  tmpvar_33 = max (0.0, dot (worldN_3, tmpvar_2));
  diff_32 = tmpvar_33;
  mediump float tmpvar_34;
  tmpvar_34 = max (0.0, dot (worldN_3, normalize(
    (tmpvar_2 + viewDir_28)
  )));
  nh_31 = tmpvar_34;
  highp float tmpvar_35;
  tmpvar_35 = (pow (nh_31, 128.0) * tmpvar_11);
  c_30.xyz = (((tmpvar_9 * tmpvar_1) * diff_32) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_35));
  c_30.w = tmpvar_12;
  c_29.w = c_30.w;
  c_29.xyz = c_30.xyz;
  c_4.xyz = c_29.xyz;
  c_4.w = 1.0;
  gl_FragData[0] = c_4;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "POINT" }
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	mediump mat4 _LightMatrix0;
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
out lowp vec3 vs_TEXCOORD1;
out lowp vec3 vs_TEXCOORD2;
out lowp vec3 vs_TEXCOORD3;
out highp vec3 vs_TEXCOORD4;
highp vec4 t0;
highp vec3 t1;
lowp vec3 t10_2;
highp float t3;
highp float t9;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.y = in_NORMAL0.x * _World2Object[0].x;
    t0.z = in_NORMAL0.x * _World2Object[1].x;
    t0.x = in_NORMAL0.x * _World2Object[2].x;
    t1.y = in_NORMAL0.y * _World2Object[0].y;
    t1.z = in_NORMAL0.y * _World2Object[1].y;
    t1.x = in_NORMAL0.y * _World2Object[2].y;
    t0.xyz = t0.xyz + t1.xyz;
    t1.y = in_NORMAL0.z * _World2Object[0].z;
    t1.z = in_NORMAL0.z * _World2Object[1].z;
    t1.x = in_NORMAL0.z * _World2Object[2].z;
    t0.xyz = t0.xyz + t1.xyz;
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    vs_TEXCOORD1.z = t0.y;
    t1.xyz = in_TANGENT0.yyy * _Object2World[1].yzx;
    t1.xyz = _Object2World[0].yzx * in_TANGENT0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].yzx * in_TANGENT0.zzz + t1.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * t1.xyz;
    t10_2.xyz = t0.xyz * t1.xyz;
    t10_2.xyz = t0.zxy * t1.yzx + (-t10_2.xyz);
    t3 = in_TANGENT0.w * unity_WorldTransformParams.w;
    t10_2.xyz = vec3(t3) * t10_2.xyz;
    vs_TEXCOORD1.y = t10_2.x;
    vs_TEXCOORD1.x = t1.z;
    vs_TEXCOORD2.z = t0.z;
    vs_TEXCOORD3.z = t0.x;
    vs_TEXCOORD2.x = t1.x;
    vs_TEXCOORD3.x = t1.y;
    vs_TEXCOORD2.y = t10_2.y;
    vs_TEXCOORD3.y = t10_2.z;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    vs_TEXCOORD4.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	mediump mat4 _LightMatrix0;
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _DetailNormalTex;
uniform lowp sampler2D _BumpTex;
uniform lowp sampler2D _LightTexture0;
in highp vec2 vs_TEXCOORD0;
in lowp vec3 vs_TEXCOORD1;
in lowp vec3 vs_TEXCOORD2;
in lowp vec3 vs_TEXCOORD3;
in highp vec3 vs_TEXCOORD4;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec3 t0;
mediump vec3 t16_0;
highp vec3 t1;
lowp vec3 t10_1;
mediump vec4 t16_2;
lowp vec3 t10_3;
lowp vec2 t10_4;
mediump vec3 t16_5;
mediump float t16_6;
lowp float t10_6;
highp float t18;
mediump float t16_20;
void main()
{
    t0.xyz = (-vs_TEXCOORD4.xyz) + _WorldSpaceLightPos0.xyz;
    t18 = dot(t0.xyz, t0.xyz);
    t18 = inversesqrt(t18);
    t0.xyz = vec3(t18) * t0.xyz;
    t1.xyz = (-vs_TEXCOORD4.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = inversesqrt(t18);
    t16_2.xyz = t1.xyz * vec3(t18) + t0.xyz;
    t16_20 = dot(t16_2.xyz, t16_2.xyz);
    t16_20 = inversesqrt(t16_20);
    t16_2.xyz = vec3(t16_20) * t16_2.xyz;
    t10_1.xyz = texture(_BumpTex, vs_TEXCOORD0.xy).xyz;
    t10_3.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t1.xy = vs_TEXCOORD0.xy * vec2(_DetailScale);
    t10_1.xy = texture(_DetailNormalTex, t1.xy).xy;
    t10_4.xy = t10_1.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
    t10_1.xy = texture(_MaskTex, vs_TEXCOORD0.xy).xy;
    t16_5.xy = t10_1.xx * t10_4.xy;
    t16_5.z = 0.0;
    t16_5.xyz = t10_3.xyz + t16_5.xyz;
    t16_20 = dot(t16_5.xyz, t16_5.xyz);
    t16_20 = inversesqrt(t16_20);
    t16_5.xyz = vec3(t16_20) * t16_5.xyz;
    t10_3.x = dot(vs_TEXCOORD1.xyz, t16_5.xyz);
    t10_3.y = dot(vs_TEXCOORD2.xyz, t16_5.xyz);
    t10_3.z = dot(vs_TEXCOORD3.xyz, t16_5.xyz);
    t16_2.x = dot(t10_3.xyz, t16_2.xyz);
    t16_2.y = dot(t10_3.xyz, t0.xyz);
    t16_2.xy = max(t16_2.xy, vec2(0.0, 0.0));
    t16_0.x = log2(t16_2.x);
    t16_0.x = t16_0.x * 128.0;
    t16_0.x = exp2(t16_0.x);
    t16_0.x = t10_1.y * t16_0.x;
    t16_2.xzw = vs_TEXCOORD4.yyy * _LightMatrix0[1].xyz;
    t16_2.xzw = _LightMatrix0[0].xyz * vs_TEXCOORD4.xxx + t16_2.xzw;
    t16_2.xzw = _LightMatrix0[2].xyz * vs_TEXCOORD4.zzz + t16_2.xzw;
    t16_2.xzw = t16_2.xzw + _LightMatrix0[3].xyz;
    t16_6 = dot(t16_2.xzw, t16_2.xzw);
    t10_6 = texture(_LightTexture0, vec2(t16_6)).w;
    t16_2.xzw = vec3(t10_6) * _LightColor0.xyz;
    t16_5.xyz = t16_2.xzw * _SpecColor.xyz;
    t16_0.xyz = t16_0.xxx * t16_5.xyz;
    t10_1.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t1.xyz = t10_1.xyz * _SkinColor.xyz;
    t16_2.xzw = t16_2.xzw * t1.xyz;
    t16_0.xyz = t16_2.xzw * t16_2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_WorldTransformParams;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp float tangentSign_1;
  lowp vec3 worldTangent_2;
  lowp vec3 worldNormal_3;
  highp vec4 v_4;
  v_4.x = _World2Object[0].x;
  v_4.y = _World2Object[1].x;
  v_4.z = _World2Object[2].x;
  v_4.w = _World2Object[3].x;
  highp vec4 v_5;
  v_5.x = _World2Object[0].y;
  v_5.y = _World2Object[1].y;
  v_5.z = _World2Object[2].y;
  v_5.w = _World2Object[3].y;
  highp vec4 v_6;
  v_6.x = _World2Object[0].z;
  v_6.y = _World2Object[1].z;
  v_6.z = _World2Object[2].z;
  v_6.w = _World2Object[3].z;
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize(((
    (v_4.xyz * _glesNormal.x)
   + 
    (v_5.xyz * _glesNormal.y)
  ) + (v_6.xyz * _glesNormal.z)));
  worldNormal_3 = tmpvar_7;
  highp mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize((tmpvar_8 * _glesTANGENT.xyz));
  worldTangent_2 = tmpvar_9;
  highp float tmpvar_10;
  tmpvar_10 = (_glesTANGENT.w * unity_WorldTransformParams.w);
  tangentSign_1 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
  lowp vec3 tmpvar_12;
  tmpvar_12.x = worldTangent_2.x;
  tmpvar_12.y = tmpvar_11.x;
  tmpvar_12.z = worldNormal_3.x;
  lowp vec3 tmpvar_13;
  tmpvar_13.x = worldTangent_2.y;
  tmpvar_13.y = tmpvar_11.y;
  tmpvar_13.z = worldNormal_3.y;
  lowp vec3 tmpvar_14;
  tmpvar_14.x = worldTangent_2.z;
  tmpvar_14.y = tmpvar_11.z;
  tmpvar_14.z = worldNormal_3.z;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_12;
  xlv_TEXCOORD2 = tmpvar_13;
  xlv_TEXCOORD3 = tmpvar_14;
  xlv_TEXCOORD4 = (_Object2World * _glesVertex).xyz;
}


#endif
#ifdef FRAGMENT
uniform highp vec3 _WorldSpaceCameraPos;
uniform mediump vec4 _WorldSpaceLightPos0;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpTex;
uniform sampler2D _DetailNormalTex;
uniform sampler2D _MaskTex;
uniform highp float _DetailScale;
uniform highp vec4 _SkinColor;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  mediump vec3 tmpvar_1;
  mediump vec3 tmpvar_2;
  lowp vec3 worldN_3;
  lowp vec4 c_4;
  lowp vec3 worldViewDir_5;
  lowp vec3 lightDir_6;
  mediump vec3 tmpvar_7;
  tmpvar_7 = _WorldSpaceLightPos0.xyz;
  lightDir_6 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD4));
  worldViewDir_5 = tmpvar_8;
  lowp vec3 tmpvar_9;
  lowp vec3 tmpvar_10;
  lowp float tmpvar_11;
  lowp float tmpvar_12;
  mediump vec3 n_sample_13;
  mediump vec3 n_detail_14;
  mediump vec4 m_15;
  mediump vec4 c_16;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_16 = tmpvar_17;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_MaskTex, xlv_TEXCOORD0);
  m_15 = tmpvar_18;
  highp vec2 P_19;
  P_19 = (xlv_TEXCOORD0 * _DetailScale);
  lowp vec3 tmpvar_20;
  tmpvar_20 = ((texture2D (_DetailNormalTex, P_19).xyz * 2.0) - 1.0);
  n_detail_14 = tmpvar_20;
  lowp vec3 tmpvar_21;
  tmpvar_21 = ((texture2D (_BumpTex, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  n_sample_13 = tmpvar_21;
  tmpvar_9 = (c_16.xyz * _SkinColor.xyz);
  tmpvar_12 = c_16.w;
  tmpvar_11 = m_15.y;
  mediump vec3 tmpvar_22;
  tmpvar_22.z = 0.0;
  tmpvar_22.xy = n_detail_14.xy;
  mediump vec3 tmpvar_23;
  tmpvar_23 = normalize((n_sample_13 + (tmpvar_22 * m_15.x)));
  tmpvar_10 = tmpvar_23;
  worldN_3.x = dot (xlv_TEXCOORD1, tmpvar_10);
  worldN_3.y = dot (xlv_TEXCOORD2, tmpvar_10);
  worldN_3.z = dot (xlv_TEXCOORD3, tmpvar_10);
  tmpvar_1 = _LightColor0.xyz;
  tmpvar_2 = lightDir_6;
  mediump vec3 viewDir_24;
  viewDir_24 = worldViewDir_5;
  lowp vec4 c_25;
  lowp vec4 c_26;
  highp float nh_27;
  lowp float diff_28;
  mediump float tmpvar_29;
  tmpvar_29 = max (0.0, dot (worldN_3, tmpvar_2));
  diff_28 = tmpvar_29;
  mediump float tmpvar_30;
  tmpvar_30 = max (0.0, dot (worldN_3, normalize(
    (tmpvar_2 + viewDir_24)
  )));
  nh_27 = tmpvar_30;
  highp float tmpvar_31;
  tmpvar_31 = (pow (nh_27, 128.0) * tmpvar_11);
  c_26.xyz = (((tmpvar_9 * tmpvar_1) * diff_28) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_31));
  c_26.w = tmpvar_12;
  c_25.w = c_26.w;
  c_25.xyz = c_26.xyz;
  c_4.xyz = c_25.xyz;
  c_4.w = 1.0;
  gl_FragData[0] = c_4;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" }
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
uniform 	mediump vec4 _WorldSpaceLightPos0;
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
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
out lowp vec3 vs_TEXCOORD1;
out lowp vec3 vs_TEXCOORD2;
out lowp vec3 vs_TEXCOORD3;
out highp vec3 vs_TEXCOORD4;
highp vec4 t0;
highp vec3 t1;
lowp vec3 t10_2;
highp float t3;
highp float t9;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.y = in_NORMAL0.x * _World2Object[0].x;
    t0.z = in_NORMAL0.x * _World2Object[1].x;
    t0.x = in_NORMAL0.x * _World2Object[2].x;
    t1.y = in_NORMAL0.y * _World2Object[0].y;
    t1.z = in_NORMAL0.y * _World2Object[1].y;
    t1.x = in_NORMAL0.y * _World2Object[2].y;
    t0.xyz = t0.xyz + t1.xyz;
    t1.y = in_NORMAL0.z * _World2Object[0].z;
    t1.z = in_NORMAL0.z * _World2Object[1].z;
    t1.x = in_NORMAL0.z * _World2Object[2].z;
    t0.xyz = t0.xyz + t1.xyz;
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    vs_TEXCOORD1.z = t0.y;
    t1.xyz = in_TANGENT0.yyy * _Object2World[1].yzx;
    t1.xyz = _Object2World[0].yzx * in_TANGENT0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].yzx * in_TANGENT0.zzz + t1.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * t1.xyz;
    t10_2.xyz = t0.xyz * t1.xyz;
    t10_2.xyz = t0.zxy * t1.yzx + (-t10_2.xyz);
    t3 = in_TANGENT0.w * unity_WorldTransformParams.w;
    t10_2.xyz = vec3(t3) * t10_2.xyz;
    vs_TEXCOORD1.y = t10_2.x;
    vs_TEXCOORD1.x = t1.z;
    vs_TEXCOORD2.z = t0.z;
    vs_TEXCOORD3.z = t0.x;
    vs_TEXCOORD2.x = t1.x;
    vs_TEXCOORD3.x = t1.y;
    vs_TEXCOORD2.y = t10_2.y;
    vs_TEXCOORD3.y = t10_2.z;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    vs_TEXCOORD4.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
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
uniform 	mediump vec4 _WorldSpaceLightPos0;
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
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _DetailNormalTex;
uniform lowp sampler2D _BumpTex;
in highp vec2 vs_TEXCOORD0;
in lowp vec3 vs_TEXCOORD1;
in lowp vec3 vs_TEXCOORD2;
in lowp vec3 vs_TEXCOORD3;
in highp vec3 vs_TEXCOORD4;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec3 t0;
mediump vec3 t16_0;
lowp vec3 t10_0;
mediump vec4 t16_1;
lowp vec3 t10_2;
lowp vec2 t10_3;
mediump vec3 t16_4;
highp vec3 t5;
lowp vec3 t10_5;
highp float t18;
mediump float t16_19;
void main()
{
    t0.xyz = (-vs_TEXCOORD4.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t0.xyz, t0.xyz);
    t18 = inversesqrt(t18);
    t16_1.xyz = t0.xyz * vec3(t18) + _WorldSpaceLightPos0.xyz;
    t16_19 = dot(t16_1.xyz, t16_1.xyz);
    t16_19 = inversesqrt(t16_19);
    t16_1.xyz = vec3(t16_19) * t16_1.xyz;
    t10_0.xyz = texture(_BumpTex, vs_TEXCOORD0.xy).xyz;
    t10_2.xyz = t10_0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t0.xy = vs_TEXCOORD0.xy * vec2(_DetailScale);
    t10_0.xy = texture(_DetailNormalTex, t0.xy).xy;
    t10_3.xy = t10_0.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
    t10_0.xy = texture(_MaskTex, vs_TEXCOORD0.xy).xy;
    t16_4.xy = t10_0.xx * t10_3.xy;
    t16_4.z = 0.0;
    t16_4.xyz = t10_2.xyz + t16_4.xyz;
    t16_19 = dot(t16_4.xyz, t16_4.xyz);
    t16_19 = inversesqrt(t16_19);
    t16_4.xyz = vec3(t16_19) * t16_4.xyz;
    t10_2.x = dot(vs_TEXCOORD1.xyz, t16_4.xyz);
    t10_2.y = dot(vs_TEXCOORD2.xyz, t16_4.xyz);
    t10_2.z = dot(vs_TEXCOORD3.xyz, t16_4.xyz);
    t16_1.x = dot(t10_2.xyz, t16_1.xyz);
    t16_1.y = dot(t10_2.xyz, _WorldSpaceLightPos0.xyz);
    t16_1.xy = max(t16_1.xy, vec2(0.0, 0.0));
    t16_0.x = log2(t16_1.x);
    t16_0.x = t16_0.x * 128.0;
    t16_0.x = exp2(t16_0.x);
    t16_0.x = t10_0.y * t16_0.x;
    t16_1.xzw = _LightColor0.xyz * _SpecColor.xyz;
    t16_0.xyz = t16_0.xxx * t16_1.xzw;
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t5.xyz = t10_5.xyz * _SkinColor.xyz;
    t16_1.xzw = t5.xyz * _LightColor0.xyz;
    t16_0.xyz = t16_1.xzw * t16_1.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_WorldTransformParams;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp float tangentSign_1;
  lowp vec3 worldTangent_2;
  lowp vec3 worldNormal_3;
  highp vec4 v_4;
  v_4.x = _World2Object[0].x;
  v_4.y = _World2Object[1].x;
  v_4.z = _World2Object[2].x;
  v_4.w = _World2Object[3].x;
  highp vec4 v_5;
  v_5.x = _World2Object[0].y;
  v_5.y = _World2Object[1].y;
  v_5.z = _World2Object[2].y;
  v_5.w = _World2Object[3].y;
  highp vec4 v_6;
  v_6.x = _World2Object[0].z;
  v_6.y = _World2Object[1].z;
  v_6.z = _World2Object[2].z;
  v_6.w = _World2Object[3].z;
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize(((
    (v_4.xyz * _glesNormal.x)
   + 
    (v_5.xyz * _glesNormal.y)
  ) + (v_6.xyz * _glesNormal.z)));
  worldNormal_3 = tmpvar_7;
  highp mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize((tmpvar_8 * _glesTANGENT.xyz));
  worldTangent_2 = tmpvar_9;
  highp float tmpvar_10;
  tmpvar_10 = (_glesTANGENT.w * unity_WorldTransformParams.w);
  tangentSign_1 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
  lowp vec3 tmpvar_12;
  tmpvar_12.x = worldTangent_2.x;
  tmpvar_12.y = tmpvar_11.x;
  tmpvar_12.z = worldNormal_3.x;
  lowp vec3 tmpvar_13;
  tmpvar_13.x = worldTangent_2.y;
  tmpvar_13.y = tmpvar_11.y;
  tmpvar_13.z = worldNormal_3.y;
  lowp vec3 tmpvar_14;
  tmpvar_14.x = worldTangent_2.z;
  tmpvar_14.y = tmpvar_11.z;
  tmpvar_14.z = worldNormal_3.z;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_12;
  xlv_TEXCOORD2 = tmpvar_13;
  xlv_TEXCOORD3 = tmpvar_14;
  xlv_TEXCOORD4 = (_Object2World * _glesVertex).xyz;
}


#endif
#ifdef FRAGMENT
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _WorldSpaceLightPos0;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _LightTexture0;
uniform mediump mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _MainTex;
uniform sampler2D _BumpTex;
uniform sampler2D _DetailNormalTex;
uniform sampler2D _MaskTex;
uniform highp float _DetailScale;
uniform highp vec4 _SkinColor;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  mediump vec3 tmpvar_1;
  mediump vec3 tmpvar_2;
  lowp vec3 worldN_3;
  lowp vec4 c_4;
  lowp float atten_5;
  mediump vec4 lightCoord_6;
  lowp vec3 worldViewDir_7;
  lowp vec3 lightDir_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD4));
  lightDir_8 = tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD4));
  worldViewDir_7 = tmpvar_10;
  lowp vec3 tmpvar_11;
  lowp vec3 tmpvar_12;
  lowp float tmpvar_13;
  lowp float tmpvar_14;
  mediump vec3 n_sample_15;
  mediump vec3 n_detail_16;
  mediump vec4 m_17;
  mediump vec4 c_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_18 = tmpvar_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_MaskTex, xlv_TEXCOORD0);
  m_17 = tmpvar_20;
  highp vec2 P_21;
  P_21 = (xlv_TEXCOORD0 * _DetailScale);
  lowp vec3 tmpvar_22;
  tmpvar_22 = ((texture2D (_DetailNormalTex, P_21).xyz * 2.0) - 1.0);
  n_detail_16 = tmpvar_22;
  lowp vec3 tmpvar_23;
  tmpvar_23 = ((texture2D (_BumpTex, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  n_sample_15 = tmpvar_23;
  tmpvar_11 = (c_18.xyz * _SkinColor.xyz);
  tmpvar_14 = c_18.w;
  tmpvar_13 = m_17.y;
  mediump vec3 tmpvar_24;
  tmpvar_24.z = 0.0;
  tmpvar_24.xy = n_detail_16.xy;
  mediump vec3 tmpvar_25;
  tmpvar_25 = normalize((n_sample_15 + (tmpvar_24 * m_17.x)));
  tmpvar_12 = tmpvar_25;
  highp vec4 tmpvar_26;
  tmpvar_26.w = 1.0;
  tmpvar_26.xyz = xlv_TEXCOORD4;
  highp vec4 tmpvar_27;
  tmpvar_27 = (_LightMatrix0 * tmpvar_26);
  lightCoord_6 = tmpvar_27;
  lowp vec4 tmpvar_28;
  mediump vec2 P_29;
  P_29 = ((lightCoord_6.xy / lightCoord_6.w) + 0.5);
  tmpvar_28 = texture2D (_LightTexture0, P_29);
  highp vec3 LightCoord_30;
  LightCoord_30 = lightCoord_6.xyz;
  highp float tmpvar_31;
  tmpvar_31 = dot (LightCoord_30, LightCoord_30);
  lowp vec4 tmpvar_32;
  tmpvar_32 = texture2D (_LightTextureB0, vec2(tmpvar_31));
  mediump float tmpvar_33;
  tmpvar_33 = ((float(
    (lightCoord_6.z > 0.0)
  ) * tmpvar_28.w) * tmpvar_32.w);
  atten_5 = tmpvar_33;
  worldN_3.x = dot (xlv_TEXCOORD1, tmpvar_12);
  worldN_3.y = dot (xlv_TEXCOORD2, tmpvar_12);
  worldN_3.z = dot (xlv_TEXCOORD3, tmpvar_12);
  tmpvar_1 = _LightColor0.xyz;
  tmpvar_2 = lightDir_8;
  tmpvar_1 = (tmpvar_1 * atten_5);
  mediump vec3 viewDir_34;
  viewDir_34 = worldViewDir_7;
  lowp vec4 c_35;
  lowp vec4 c_36;
  highp float nh_37;
  lowp float diff_38;
  mediump float tmpvar_39;
  tmpvar_39 = max (0.0, dot (worldN_3, tmpvar_2));
  diff_38 = tmpvar_39;
  mediump float tmpvar_40;
  tmpvar_40 = max (0.0, dot (worldN_3, normalize(
    (tmpvar_2 + viewDir_34)
  )));
  nh_37 = tmpvar_40;
  highp float tmpvar_41;
  tmpvar_41 = (pow (nh_37, 128.0) * tmpvar_13);
  c_36.xyz = (((tmpvar_11 * tmpvar_1) * diff_38) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_41));
  c_36.w = tmpvar_14;
  c_35.w = c_36.w;
  c_35.xyz = c_36.xyz;
  c_4.xyz = c_35.xyz;
  c_4.w = 1.0;
  gl_FragData[0] = c_4;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SPOT" }
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	mediump mat4 _LightMatrix0;
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
out lowp vec3 vs_TEXCOORD1;
out lowp vec3 vs_TEXCOORD2;
out lowp vec3 vs_TEXCOORD3;
out highp vec3 vs_TEXCOORD4;
highp vec4 t0;
highp vec3 t1;
lowp vec3 t10_2;
highp float t3;
highp float t9;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.y = in_NORMAL0.x * _World2Object[0].x;
    t0.z = in_NORMAL0.x * _World2Object[1].x;
    t0.x = in_NORMAL0.x * _World2Object[2].x;
    t1.y = in_NORMAL0.y * _World2Object[0].y;
    t1.z = in_NORMAL0.y * _World2Object[1].y;
    t1.x = in_NORMAL0.y * _World2Object[2].y;
    t0.xyz = t0.xyz + t1.xyz;
    t1.y = in_NORMAL0.z * _World2Object[0].z;
    t1.z = in_NORMAL0.z * _World2Object[1].z;
    t1.x = in_NORMAL0.z * _World2Object[2].z;
    t0.xyz = t0.xyz + t1.xyz;
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    vs_TEXCOORD1.z = t0.y;
    t1.xyz = in_TANGENT0.yyy * _Object2World[1].yzx;
    t1.xyz = _Object2World[0].yzx * in_TANGENT0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].yzx * in_TANGENT0.zzz + t1.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * t1.xyz;
    t10_2.xyz = t0.xyz * t1.xyz;
    t10_2.xyz = t0.zxy * t1.yzx + (-t10_2.xyz);
    t3 = in_TANGENT0.w * unity_WorldTransformParams.w;
    t10_2.xyz = vec3(t3) * t10_2.xyz;
    vs_TEXCOORD1.y = t10_2.x;
    vs_TEXCOORD1.x = t1.z;
    vs_TEXCOORD2.z = t0.z;
    vs_TEXCOORD3.z = t0.x;
    vs_TEXCOORD2.x = t1.x;
    vs_TEXCOORD3.x = t1.y;
    vs_TEXCOORD2.y = t10_2.y;
    vs_TEXCOORD3.y = t10_2.z;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    vs_TEXCOORD4.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	mediump mat4 _LightMatrix0;
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _DetailNormalTex;
uniform lowp sampler2D _BumpTex;
uniform lowp sampler2D _LightTexture0;
uniform lowp sampler2D _LightTextureB0;
in highp vec2 vs_TEXCOORD0;
in lowp vec3 vs_TEXCOORD1;
in lowp vec3 vs_TEXCOORD2;
in lowp vec3 vs_TEXCOORD3;
in highp vec3 vs_TEXCOORD4;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec3 t16_1;
highp vec3 t2;
mediump vec3 t16_2;
lowp vec3 t10_2;
lowp vec3 t10_3;
highp vec3 t4;
lowp vec3 t10_4;
mediump vec3 t16_5;
lowp vec2 t10_6;
mediump vec3 t16_7;
bool tb10;
mediump float t16_18;
lowp float t10_18;
mediump float t16_24;
mediump float t16_25;
highp float t26;
void main()
{
    t16_0 = vs_TEXCOORD4.yyyy * _LightMatrix0[1];
    t16_0 = _LightMatrix0[0] * vs_TEXCOORD4.xxxx + t16_0;
    t16_0 = _LightMatrix0[2] * vs_TEXCOORD4.zzzz + t16_0;
    t16_0 = t16_0 + _LightMatrix0[3];
    t16_1.xy = t16_0.xy / t16_0.ww;
    t16_1.xy = t16_1.xy + vec2(0.5, 0.5);
    t10_2.x = texture(_LightTexture0, t16_1.xy).w;
    tb10 = 0.0<t16_0.z;
    t16_18 = dot(t16_0.xyz, t16_0.xyz);
    t10_18 = texture(_LightTextureB0, vec2(t16_18)).w;
    t10_3.x = (tb10) ? 1.0 : 0.0;
    t10_3.x = t10_2.x * t10_3.x;
    t10_3.x = t10_18 * t10_3.x;
    t16_0.xyz = t10_3.xxx * _LightColor0.xyz;
    t10_2.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t2.xyz = t10_2.xyz * _SkinColor.xyz;
    t16_1.xyz = t16_0.xyz * t2.xyz;
    t16_0.xyz = t16_0.xyz * _SpecColor.xyz;
    t2.xyz = (-vs_TEXCOORD4.xyz) + _WorldSpaceLightPos0.xyz;
    t26 = dot(t2.xyz, t2.xyz);
    t26 = inversesqrt(t26);
    t2.xyz = vec3(t26) * t2.xyz;
    t4.xyz = (-vs_TEXCOORD4.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t26 = dot(t4.xyz, t4.xyz);
    t26 = inversesqrt(t26);
    t16_5.xyz = t4.xyz * vec3(t26) + t2.xyz;
    t16_24 = dot(t16_5.xyz, t16_5.xyz);
    t16_24 = inversesqrt(t16_24);
    t16_5.xyz = vec3(t16_24) * t16_5.xyz;
    t10_4.xyz = texture(_BumpTex, vs_TEXCOORD0.xy).xyz;
    t10_3.xyz = t10_4.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t4.xy = vs_TEXCOORD0.xy * vec2(_DetailScale);
    t10_4.xy = texture(_DetailNormalTex, t4.xy).xy;
    t10_6.xy = t10_4.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
    t10_4.xy = texture(_MaskTex, vs_TEXCOORD0.xy).xy;
    t16_7.xy = t10_4.xx * t10_6.xy;
    t16_7.z = 0.0;
    t16_7.xyz = t10_3.xyz + t16_7.xyz;
    t16_24 = dot(t16_7.xyz, t16_7.xyz);
    t16_24 = inversesqrt(t16_24);
    t16_7.xyz = vec3(t16_24) * t16_7.xyz;
    t10_3.x = dot(vs_TEXCOORD1.xyz, t16_7.xyz);
    t10_3.y = dot(vs_TEXCOORD2.xyz, t16_7.xyz);
    t10_3.z = dot(vs_TEXCOORD3.xyz, t16_7.xyz);
    t16_24 = dot(t10_3.xyz, t16_5.xyz);
    t16_25 = dot(t10_3.xyz, t2.xyz);
    t16_25 = max(t16_25, 0.0);
    t16_24 = max(t16_24, 0.0);
    t16_2.x = log2(t16_24);
    t16_2.x = t16_2.x * 128.0;
    t16_2.x = exp2(t16_2.x);
    t16_2.x = t10_4.y * t16_2.x;
    t16_2.xyz = t16_0.xyz * t16_2.xxx;
    t16_2.xyz = t16_1.xyz * vec3(t16_25) + t16_2.xyz;
    SV_Target0.xyz = t16_2.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_WorldTransformParams;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp float tangentSign_1;
  lowp vec3 worldTangent_2;
  lowp vec3 worldNormal_3;
  highp vec4 v_4;
  v_4.x = _World2Object[0].x;
  v_4.y = _World2Object[1].x;
  v_4.z = _World2Object[2].x;
  v_4.w = _World2Object[3].x;
  highp vec4 v_5;
  v_5.x = _World2Object[0].y;
  v_5.y = _World2Object[1].y;
  v_5.z = _World2Object[2].y;
  v_5.w = _World2Object[3].y;
  highp vec4 v_6;
  v_6.x = _World2Object[0].z;
  v_6.y = _World2Object[1].z;
  v_6.z = _World2Object[2].z;
  v_6.w = _World2Object[3].z;
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize(((
    (v_4.xyz * _glesNormal.x)
   + 
    (v_5.xyz * _glesNormal.y)
  ) + (v_6.xyz * _glesNormal.z)));
  worldNormal_3 = tmpvar_7;
  highp mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize((tmpvar_8 * _glesTANGENT.xyz));
  worldTangent_2 = tmpvar_9;
  highp float tmpvar_10;
  tmpvar_10 = (_glesTANGENT.w * unity_WorldTransformParams.w);
  tangentSign_1 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
  lowp vec3 tmpvar_12;
  tmpvar_12.x = worldTangent_2.x;
  tmpvar_12.y = tmpvar_11.x;
  tmpvar_12.z = worldNormal_3.x;
  lowp vec3 tmpvar_13;
  tmpvar_13.x = worldTangent_2.y;
  tmpvar_13.y = tmpvar_11.y;
  tmpvar_13.z = worldNormal_3.y;
  lowp vec3 tmpvar_14;
  tmpvar_14.x = worldTangent_2.z;
  tmpvar_14.y = tmpvar_11.z;
  tmpvar_14.z = worldNormal_3.z;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_12;
  xlv_TEXCOORD2 = tmpvar_13;
  xlv_TEXCOORD3 = tmpvar_14;
  xlv_TEXCOORD4 = (_Object2World * _glesVertex).xyz;
}


#endif
#ifdef FRAGMENT
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _WorldSpaceLightPos0;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform lowp samplerCube _LightTexture0;
uniform mediump mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _MainTex;
uniform sampler2D _BumpTex;
uniform sampler2D _DetailNormalTex;
uniform sampler2D _MaskTex;
uniform highp float _DetailScale;
uniform highp vec4 _SkinColor;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  mediump vec3 tmpvar_1;
  mediump vec3 tmpvar_2;
  lowp vec3 worldN_3;
  lowp vec4 c_4;
  lowp vec3 worldViewDir_5;
  lowp vec3 lightDir_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize((_WorldSpaceLightPos0.xyz - xlv_TEXCOORD4));
  lightDir_6 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD4));
  worldViewDir_5 = tmpvar_8;
  lowp vec3 tmpvar_9;
  lowp vec3 tmpvar_10;
  lowp float tmpvar_11;
  lowp float tmpvar_12;
  mediump vec3 n_sample_13;
  mediump vec3 n_detail_14;
  mediump vec4 m_15;
  mediump vec4 c_16;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_16 = tmpvar_17;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_MaskTex, xlv_TEXCOORD0);
  m_15 = tmpvar_18;
  highp vec2 P_19;
  P_19 = (xlv_TEXCOORD0 * _DetailScale);
  lowp vec3 tmpvar_20;
  tmpvar_20 = ((texture2D (_DetailNormalTex, P_19).xyz * 2.0) - 1.0);
  n_detail_14 = tmpvar_20;
  lowp vec3 tmpvar_21;
  tmpvar_21 = ((texture2D (_BumpTex, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  n_sample_13 = tmpvar_21;
  tmpvar_9 = (c_16.xyz * _SkinColor.xyz);
  tmpvar_12 = c_16.w;
  tmpvar_11 = m_15.y;
  mediump vec3 tmpvar_22;
  tmpvar_22.z = 0.0;
  tmpvar_22.xy = n_detail_14.xy;
  mediump vec3 tmpvar_23;
  tmpvar_23 = normalize((n_sample_13 + (tmpvar_22 * m_15.x)));
  tmpvar_10 = tmpvar_23;
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = xlv_TEXCOORD4;
  highp vec3 tmpvar_25;
  tmpvar_25 = (_LightMatrix0 * tmpvar_24).xyz;
  highp float tmpvar_26;
  tmpvar_26 = dot (tmpvar_25, tmpvar_25);
  lowp float tmpvar_27;
  tmpvar_27 = (texture2D (_LightTextureB0, vec2(tmpvar_26)).w * textureCube (_LightTexture0, tmpvar_25).w);
  worldN_3.x = dot (xlv_TEXCOORD1, tmpvar_10);
  worldN_3.y = dot (xlv_TEXCOORD2, tmpvar_10);
  worldN_3.z = dot (xlv_TEXCOORD3, tmpvar_10);
  tmpvar_1 = _LightColor0.xyz;
  tmpvar_2 = lightDir_6;
  tmpvar_1 = (tmpvar_1 * tmpvar_27);
  mediump vec3 viewDir_28;
  viewDir_28 = worldViewDir_5;
  lowp vec4 c_29;
  lowp vec4 c_30;
  highp float nh_31;
  lowp float diff_32;
  mediump float tmpvar_33;
  tmpvar_33 = max (0.0, dot (worldN_3, tmpvar_2));
  diff_32 = tmpvar_33;
  mediump float tmpvar_34;
  tmpvar_34 = max (0.0, dot (worldN_3, normalize(
    (tmpvar_2 + viewDir_28)
  )));
  nh_31 = tmpvar_34;
  highp float tmpvar_35;
  tmpvar_35 = (pow (nh_31, 128.0) * tmpvar_11);
  c_30.xyz = (((tmpvar_9 * tmpvar_1) * diff_32) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_35));
  c_30.w = tmpvar_12;
  c_29.w = c_30.w;
  c_29.xyz = c_30.xyz;
  c_4.xyz = c_29.xyz;
  c_4.w = 1.0;
  gl_FragData[0] = c_4;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "POINT_COOKIE" }
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	mediump mat4 _LightMatrix0;
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
out lowp vec3 vs_TEXCOORD1;
out lowp vec3 vs_TEXCOORD2;
out lowp vec3 vs_TEXCOORD3;
out highp vec3 vs_TEXCOORD4;
highp vec4 t0;
highp vec3 t1;
lowp vec3 t10_2;
highp float t3;
highp float t9;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.y = in_NORMAL0.x * _World2Object[0].x;
    t0.z = in_NORMAL0.x * _World2Object[1].x;
    t0.x = in_NORMAL0.x * _World2Object[2].x;
    t1.y = in_NORMAL0.y * _World2Object[0].y;
    t1.z = in_NORMAL0.y * _World2Object[1].y;
    t1.x = in_NORMAL0.y * _World2Object[2].y;
    t0.xyz = t0.xyz + t1.xyz;
    t1.y = in_NORMAL0.z * _World2Object[0].z;
    t1.z = in_NORMAL0.z * _World2Object[1].z;
    t1.x = in_NORMAL0.z * _World2Object[2].z;
    t0.xyz = t0.xyz + t1.xyz;
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    vs_TEXCOORD1.z = t0.y;
    t1.xyz = in_TANGENT0.yyy * _Object2World[1].yzx;
    t1.xyz = _Object2World[0].yzx * in_TANGENT0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].yzx * in_TANGENT0.zzz + t1.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * t1.xyz;
    t10_2.xyz = t0.xyz * t1.xyz;
    t10_2.xyz = t0.zxy * t1.yzx + (-t10_2.xyz);
    t3 = in_TANGENT0.w * unity_WorldTransformParams.w;
    t10_2.xyz = vec3(t3) * t10_2.xyz;
    vs_TEXCOORD1.y = t10_2.x;
    vs_TEXCOORD1.x = t1.z;
    vs_TEXCOORD2.z = t0.z;
    vs_TEXCOORD3.z = t0.x;
    vs_TEXCOORD2.x = t1.x;
    vs_TEXCOORD3.x = t1.y;
    vs_TEXCOORD2.y = t10_2.y;
    vs_TEXCOORD3.y = t10_2.z;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    vs_TEXCOORD4.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	mediump mat4 _LightMatrix0;
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _DetailNormalTex;
uniform lowp sampler2D _BumpTex;
uniform lowp sampler2D _LightTextureB0;
uniform lowp samplerCube _LightTexture0;
in highp vec2 vs_TEXCOORD0;
in lowp vec3 vs_TEXCOORD1;
in lowp vec3 vs_TEXCOORD2;
in lowp vec3 vs_TEXCOORD3;
in highp vec3 vs_TEXCOORD4;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec3 t0;
mediump vec3 t16_0;
highp vec3 t1;
lowp vec3 t10_1;
mediump vec4 t16_2;
lowp vec3 t10_3;
lowp vec2 t10_4;
mediump vec3 t16_5;
mediump float t16_6;
lowp float t10_6;
lowp float t10_12;
highp float t18;
mediump float t16_20;
void main()
{
    t0.xyz = (-vs_TEXCOORD4.xyz) + _WorldSpaceLightPos0.xyz;
    t18 = dot(t0.xyz, t0.xyz);
    t18 = inversesqrt(t18);
    t0.xyz = vec3(t18) * t0.xyz;
    t1.xyz = (-vs_TEXCOORD4.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = inversesqrt(t18);
    t16_2.xyz = t1.xyz * vec3(t18) + t0.xyz;
    t16_20 = dot(t16_2.xyz, t16_2.xyz);
    t16_20 = inversesqrt(t16_20);
    t16_2.xyz = vec3(t16_20) * t16_2.xyz;
    t10_1.xyz = texture(_BumpTex, vs_TEXCOORD0.xy).xyz;
    t10_3.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t1.xy = vs_TEXCOORD0.xy * vec2(_DetailScale);
    t10_1.xy = texture(_DetailNormalTex, t1.xy).xy;
    t10_4.xy = t10_1.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
    t10_1.xy = texture(_MaskTex, vs_TEXCOORD0.xy).xy;
    t16_5.xy = t10_1.xx * t10_4.xy;
    t16_5.z = 0.0;
    t16_5.xyz = t10_3.xyz + t16_5.xyz;
    t16_20 = dot(t16_5.xyz, t16_5.xyz);
    t16_20 = inversesqrt(t16_20);
    t16_5.xyz = vec3(t16_20) * t16_5.xyz;
    t10_3.x = dot(vs_TEXCOORD1.xyz, t16_5.xyz);
    t10_3.y = dot(vs_TEXCOORD2.xyz, t16_5.xyz);
    t10_3.z = dot(vs_TEXCOORD3.xyz, t16_5.xyz);
    t16_2.x = dot(t10_3.xyz, t16_2.xyz);
    t16_2.y = dot(t10_3.xyz, t0.xyz);
    t16_2.xy = max(t16_2.xy, vec2(0.0, 0.0));
    t16_0.x = log2(t16_2.x);
    t16_0.x = t16_0.x * 128.0;
    t16_0.x = exp2(t16_0.x);
    t16_0.x = t10_1.y * t16_0.x;
    t16_2.xzw = vs_TEXCOORD4.yyy * _LightMatrix0[1].xyz;
    t16_2.xzw = _LightMatrix0[0].xyz * vs_TEXCOORD4.xxx + t16_2.xzw;
    t16_2.xzw = _LightMatrix0[2].xyz * vs_TEXCOORD4.zzz + t16_2.xzw;
    t16_2.xzw = t16_2.xzw + _LightMatrix0[3].xyz;
    t16_6 = dot(t16_2.xzw, t16_2.xzw);
    t10_12 = texture(_LightTexture0, t16_2.xzw).w;
    t10_6 = texture(_LightTextureB0, vec2(t16_6)).w;
    t16_6 = t10_12 * t10_6;
    t16_2.xzw = vec3(t16_6) * _LightColor0.xyz;
    t16_5.xyz = t16_2.xzw * _SpecColor.xyz;
    t16_0.xyz = t16_0.xxx * t16_5.xyz;
    t10_1.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t1.xyz = t10_1.xyz * _SkinColor.xyz;
    t16_2.xzw = t16_2.xzw * t1.xyz;
    t16_0.xyz = t16_2.xzw * t16_2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_WorldTransformParams;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp float tangentSign_1;
  lowp vec3 worldTangent_2;
  lowp vec3 worldNormal_3;
  highp vec4 v_4;
  v_4.x = _World2Object[0].x;
  v_4.y = _World2Object[1].x;
  v_4.z = _World2Object[2].x;
  v_4.w = _World2Object[3].x;
  highp vec4 v_5;
  v_5.x = _World2Object[0].y;
  v_5.y = _World2Object[1].y;
  v_5.z = _World2Object[2].y;
  v_5.w = _World2Object[3].y;
  highp vec4 v_6;
  v_6.x = _World2Object[0].z;
  v_6.y = _World2Object[1].z;
  v_6.z = _World2Object[2].z;
  v_6.w = _World2Object[3].z;
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize(((
    (v_4.xyz * _glesNormal.x)
   + 
    (v_5.xyz * _glesNormal.y)
  ) + (v_6.xyz * _glesNormal.z)));
  worldNormal_3 = tmpvar_7;
  highp mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize((tmpvar_8 * _glesTANGENT.xyz));
  worldTangent_2 = tmpvar_9;
  highp float tmpvar_10;
  tmpvar_10 = (_glesTANGENT.w * unity_WorldTransformParams.w);
  tangentSign_1 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
  lowp vec3 tmpvar_12;
  tmpvar_12.x = worldTangent_2.x;
  tmpvar_12.y = tmpvar_11.x;
  tmpvar_12.z = worldNormal_3.x;
  lowp vec3 tmpvar_13;
  tmpvar_13.x = worldTangent_2.y;
  tmpvar_13.y = tmpvar_11.y;
  tmpvar_13.z = worldNormal_3.y;
  lowp vec3 tmpvar_14;
  tmpvar_14.x = worldTangent_2.z;
  tmpvar_14.y = tmpvar_11.z;
  tmpvar_14.z = worldNormal_3.z;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_12;
  xlv_TEXCOORD2 = tmpvar_13;
  xlv_TEXCOORD3 = tmpvar_14;
  xlv_TEXCOORD4 = (_Object2World * _glesVertex).xyz;
}


#endif
#ifdef FRAGMENT
uniform highp vec3 _WorldSpaceCameraPos;
uniform mediump vec4 _WorldSpaceLightPos0;
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
uniform sampler2D _LightTexture0;
uniform mediump mat4 _LightMatrix0;
uniform sampler2D _MainTex;
uniform sampler2D _BumpTex;
uniform sampler2D _DetailNormalTex;
uniform sampler2D _MaskTex;
uniform highp float _DetailScale;
uniform highp vec4 _SkinColor;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  mediump vec3 tmpvar_1;
  mediump vec3 tmpvar_2;
  lowp vec3 worldN_3;
  lowp vec4 c_4;
  lowp vec3 worldViewDir_5;
  lowp vec3 lightDir_6;
  mediump vec3 tmpvar_7;
  tmpvar_7 = _WorldSpaceLightPos0.xyz;
  lightDir_6 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((_WorldSpaceCameraPos - xlv_TEXCOORD4));
  worldViewDir_5 = tmpvar_8;
  lowp vec3 tmpvar_9;
  lowp vec3 tmpvar_10;
  lowp float tmpvar_11;
  lowp float tmpvar_12;
  mediump vec3 n_sample_13;
  mediump vec3 n_detail_14;
  mediump vec4 m_15;
  mediump vec4 c_16;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_16 = tmpvar_17;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_MaskTex, xlv_TEXCOORD0);
  m_15 = tmpvar_18;
  highp vec2 P_19;
  P_19 = (xlv_TEXCOORD0 * _DetailScale);
  lowp vec3 tmpvar_20;
  tmpvar_20 = ((texture2D (_DetailNormalTex, P_19).xyz * 2.0) - 1.0);
  n_detail_14 = tmpvar_20;
  lowp vec3 tmpvar_21;
  tmpvar_21 = ((texture2D (_BumpTex, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  n_sample_13 = tmpvar_21;
  tmpvar_9 = (c_16.xyz * _SkinColor.xyz);
  tmpvar_12 = c_16.w;
  tmpvar_11 = m_15.y;
  mediump vec3 tmpvar_22;
  tmpvar_22.z = 0.0;
  tmpvar_22.xy = n_detail_14.xy;
  mediump vec3 tmpvar_23;
  tmpvar_23 = normalize((n_sample_13 + (tmpvar_22 * m_15.x)));
  tmpvar_10 = tmpvar_23;
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = xlv_TEXCOORD4;
  highp vec2 tmpvar_25;
  tmpvar_25 = (_LightMatrix0 * tmpvar_24).xy;
  lowp float tmpvar_26;
  tmpvar_26 = texture2D (_LightTexture0, tmpvar_25).w;
  worldN_3.x = dot (xlv_TEXCOORD1, tmpvar_10);
  worldN_3.y = dot (xlv_TEXCOORD2, tmpvar_10);
  worldN_3.z = dot (xlv_TEXCOORD3, tmpvar_10);
  tmpvar_1 = _LightColor0.xyz;
  tmpvar_2 = lightDir_6;
  tmpvar_1 = (tmpvar_1 * tmpvar_26);
  mediump vec3 viewDir_27;
  viewDir_27 = worldViewDir_5;
  lowp vec4 c_28;
  lowp vec4 c_29;
  highp float nh_30;
  lowp float diff_31;
  mediump float tmpvar_32;
  tmpvar_32 = max (0.0, dot (worldN_3, tmpvar_2));
  diff_31 = tmpvar_32;
  mediump float tmpvar_33;
  tmpvar_33 = max (0.0, dot (worldN_3, normalize(
    (tmpvar_2 + viewDir_27)
  )));
  nh_30 = tmpvar_33;
  highp float tmpvar_34;
  tmpvar_34 = (pow (nh_30, 128.0) * tmpvar_11);
  c_29.xyz = (((tmpvar_9 * tmpvar_1) * diff_31) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_34));
  c_29.w = tmpvar_12;
  c_28.w = c_29.w;
  c_28.xyz = c_29.xyz;
  c_4.xyz = c_28.xyz;
  c_4.w = 1.0;
  gl_FragData[0] = c_4;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL_COOKIE" }
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
uniform 	mediump vec4 _WorldSpaceLightPos0;
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
uniform 	mediump mat4 _LightMatrix0;
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
out lowp vec3 vs_TEXCOORD1;
out lowp vec3 vs_TEXCOORD2;
out lowp vec3 vs_TEXCOORD3;
out highp vec3 vs_TEXCOORD4;
highp vec4 t0;
highp vec3 t1;
lowp vec3 t10_2;
highp float t3;
highp float t9;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.y = in_NORMAL0.x * _World2Object[0].x;
    t0.z = in_NORMAL0.x * _World2Object[1].x;
    t0.x = in_NORMAL0.x * _World2Object[2].x;
    t1.y = in_NORMAL0.y * _World2Object[0].y;
    t1.z = in_NORMAL0.y * _World2Object[1].y;
    t1.x = in_NORMAL0.y * _World2Object[2].y;
    t0.xyz = t0.xyz + t1.xyz;
    t1.y = in_NORMAL0.z * _World2Object[0].z;
    t1.z = in_NORMAL0.z * _World2Object[1].z;
    t1.x = in_NORMAL0.z * _World2Object[2].z;
    t0.xyz = t0.xyz + t1.xyz;
    t9 = dot(t0.xyz, t0.xyz);
    t9 = inversesqrt(t9);
    t0.xyz = vec3(t9) * t0.xyz;
    vs_TEXCOORD1.z = t0.y;
    t1.xyz = in_TANGENT0.yyy * _Object2World[1].yzx;
    t1.xyz = _Object2World[0].yzx * in_TANGENT0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].yzx * in_TANGENT0.zzz + t1.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * t1.xyz;
    t10_2.xyz = t0.xyz * t1.xyz;
    t10_2.xyz = t0.zxy * t1.yzx + (-t10_2.xyz);
    t3 = in_TANGENT0.w * unity_WorldTransformParams.w;
    t10_2.xyz = vec3(t3) * t10_2.xyz;
    vs_TEXCOORD1.y = t10_2.x;
    vs_TEXCOORD1.x = t1.z;
    vs_TEXCOORD2.z = t0.z;
    vs_TEXCOORD3.z = t0.x;
    vs_TEXCOORD2.x = t1.x;
    vs_TEXCOORD3.x = t1.y;
    vs_TEXCOORD2.y = t10_2.y;
    vs_TEXCOORD3.y = t10_2.z;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    vs_TEXCOORD4.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
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
uniform 	mediump vec4 _WorldSpaceLightPos0;
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
uniform 	mediump mat4 _LightMatrix0;
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _DetailNormalTex;
uniform lowp sampler2D _BumpTex;
uniform lowp sampler2D _LightTexture0;
in highp vec2 vs_TEXCOORD0;
in lowp vec3 vs_TEXCOORD1;
in lowp vec3 vs_TEXCOORD2;
in lowp vec3 vs_TEXCOORD3;
in highp vec3 vs_TEXCOORD4;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec3 t0;
mediump vec3 t16_0;
lowp vec3 t10_0;
mediump vec4 t16_1;
lowp vec3 t10_2;
lowp vec2 t10_3;
mediump vec3 t16_4;
highp vec3 t5;
lowp vec3 t10_5;
lowp float t10_6;
highp float t18;
mediump float t16_19;
void main()
{
    t0.xyz = (-vs_TEXCOORD4.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t0.xyz, t0.xyz);
    t18 = inversesqrt(t18);
    t16_1.xyz = t0.xyz * vec3(t18) + _WorldSpaceLightPos0.xyz;
    t16_19 = dot(t16_1.xyz, t16_1.xyz);
    t16_19 = inversesqrt(t16_19);
    t16_1.xyz = vec3(t16_19) * t16_1.xyz;
    t10_0.xyz = texture(_BumpTex, vs_TEXCOORD0.xy).xyz;
    t10_2.xyz = t10_0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t0.xy = vs_TEXCOORD0.xy * vec2(_DetailScale);
    t10_0.xy = texture(_DetailNormalTex, t0.xy).xy;
    t10_3.xy = t10_0.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
    t10_0.xy = texture(_MaskTex, vs_TEXCOORD0.xy).xy;
    t16_4.xy = t10_0.xx * t10_3.xy;
    t16_4.z = 0.0;
    t16_4.xyz = t10_2.xyz + t16_4.xyz;
    t16_19 = dot(t16_4.xyz, t16_4.xyz);
    t16_19 = inversesqrt(t16_19);
    t16_4.xyz = vec3(t16_19) * t16_4.xyz;
    t10_2.x = dot(vs_TEXCOORD1.xyz, t16_4.xyz);
    t10_2.y = dot(vs_TEXCOORD2.xyz, t16_4.xyz);
    t10_2.z = dot(vs_TEXCOORD3.xyz, t16_4.xyz);
    t16_1.x = dot(t10_2.xyz, t16_1.xyz);
    t16_1.y = dot(t10_2.xyz, _WorldSpaceLightPos0.xyz);
    t16_1.xy = max(t16_1.xy, vec2(0.0, 0.0));
    t16_0.x = log2(t16_1.x);
    t16_0.x = t16_0.x * 128.0;
    t16_0.x = exp2(t16_0.x);
    t16_0.x = t10_0.y * t16_0.x;
    t16_1.xz = vs_TEXCOORD4.yy * _LightMatrix0[1].xy;
    t16_1.xz = _LightMatrix0[0].xy * vs_TEXCOORD4.xx + t16_1.xz;
    t16_1.xz = _LightMatrix0[2].xy * vs_TEXCOORD4.zz + t16_1.xz;
    t16_1.xz = t16_1.xz + _LightMatrix0[3].xy;
    t10_6 = texture(_LightTexture0, t16_1.xz).w;
    t16_1.xzw = vec3(t10_6) * _LightColor0.xyz;
    t16_4.xyz = t16_1.xzw * _SpecColor.xyz;
    t16_0.xyz = t16_0.xxx * t16_4.xyz;
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t5.xyz = t10_5.xyz * _SkinColor.xyz;
    t16_1.xzw = t16_1.xzw * t5.xyz;
    t16_0.xyz = t16_1.xzw * t16_1.yyy + t16_0.xyz;
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
Keywords { "POINT" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "POINT" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SPOT" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SPOT" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "POINT_COOKIE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES3"
}
}
 }
 Pass {
  Name "PREPASS"
  Tags { "LIGHTMODE"="PrePassBase" "RenderType"="Opaque" }
  GpuProgramID 152486
Program "vp" {
SubProgram "gles " {
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_WorldTransformParams;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp float tangentSign_1;
  lowp vec3 worldTangent_2;
  lowp vec3 worldNormal_3;
  highp vec3 tmpvar_4;
  tmpvar_4 = (_Object2World * _glesVertex).xyz;
  highp vec4 v_5;
  v_5.x = _World2Object[0].x;
  v_5.y = _World2Object[1].x;
  v_5.z = _World2Object[2].x;
  v_5.w = _World2Object[3].x;
  highp vec4 v_6;
  v_6.x = _World2Object[0].y;
  v_6.y = _World2Object[1].y;
  v_6.z = _World2Object[2].y;
  v_6.w = _World2Object[3].y;
  highp vec4 v_7;
  v_7.x = _World2Object[0].z;
  v_7.y = _World2Object[1].z;
  v_7.z = _World2Object[2].z;
  v_7.w = _World2Object[3].z;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize(((
    (v_5.xyz * _glesNormal.x)
   + 
    (v_6.xyz * _glesNormal.y)
  ) + (v_7.xyz * _glesNormal.z)));
  worldNormal_3 = tmpvar_8;
  highp mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * _glesTANGENT.xyz));
  worldTangent_2 = tmpvar_10;
  highp float tmpvar_11;
  tmpvar_11 = (_glesTANGENT.w * unity_WorldTransformParams.w);
  tangentSign_1 = tmpvar_11;
  lowp vec3 tmpvar_12;
  tmpvar_12 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
  highp vec4 tmpvar_13;
  tmpvar_13.x = worldTangent_2.x;
  tmpvar_13.y = tmpvar_12.x;
  tmpvar_13.z = worldNormal_3.x;
  tmpvar_13.w = tmpvar_4.x;
  highp vec4 tmpvar_14;
  tmpvar_14.x = worldTangent_2.y;
  tmpvar_14.y = tmpvar_12.y;
  tmpvar_14.z = worldNormal_3.y;
  tmpvar_14.w = tmpvar_4.y;
  highp vec4 tmpvar_15;
  tmpvar_15.x = worldTangent_2.z;
  tmpvar_15.y = tmpvar_12.z;
  tmpvar_15.z = worldNormal_3.z;
  tmpvar_15.w = tmpvar_4.z;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_13;
  xlv_TEXCOORD2 = tmpvar_14;
  xlv_TEXCOORD3 = tmpvar_15;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _BumpTex;
uniform sampler2D _DetailNormalTex;
uniform sampler2D _MaskTex;
uniform highp float _DetailScale;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 res_1;
  lowp vec3 worldN_2;
  lowp vec3 tmpvar_3;
  mediump vec3 n_sample_4;
  mediump vec3 n_detail_5;
  mediump vec4 m_6;
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MaskTex, xlv_TEXCOORD0);
  m_6 = tmpvar_7;
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD0 * _DetailScale);
  lowp vec3 tmpvar_9;
  tmpvar_9 = ((texture2D (_DetailNormalTex, P_8).xyz * 2.0) - 1.0);
  n_detail_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = ((texture2D (_BumpTex, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  n_sample_4 = tmpvar_10;
  mediump vec3 tmpvar_11;
  tmpvar_11.z = 0.0;
  tmpvar_11.xy = n_detail_5.xy;
  mediump vec3 tmpvar_12;
  tmpvar_12 = normalize((n_sample_4 + (tmpvar_11 * m_6.x)));
  tmpvar_3 = tmpvar_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (xlv_TEXCOORD1.xyz, tmpvar_3);
  worldN_2.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (xlv_TEXCOORD2.xyz, tmpvar_3);
  worldN_2.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (xlv_TEXCOORD3.xyz, tmpvar_3);
  worldN_2.z = tmpvar_15;
  res_1.xyz = ((worldN_2 * 0.5) + 0.5);
  res_1.w = 1.0;
  gl_FragData[0] = res_1;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
out highp vec4 vs_TEXCOORD2;
out highp vec4 vs_TEXCOORD3;
highp vec4 t0;
highp vec3 t1;
lowp vec3 t10_2;
highp vec3 t3;
highp float t4;
highp float t12;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.y = in_NORMAL0.x * _World2Object[0].x;
    t0.z = in_NORMAL0.x * _World2Object[1].x;
    t0.x = in_NORMAL0.x * _World2Object[2].x;
    t1.y = in_NORMAL0.y * _World2Object[0].y;
    t1.z = in_NORMAL0.y * _World2Object[1].y;
    t1.x = in_NORMAL0.y * _World2Object[2].y;
    t0.xyz = t0.xyz + t1.xyz;
    t1.y = in_NORMAL0.z * _World2Object[0].z;
    t1.z = in_NORMAL0.z * _World2Object[1].z;
    t1.x = in_NORMAL0.z * _World2Object[2].z;
    t0.xyz = t0.xyz + t1.xyz;
    t12 = dot(t0.xyz, t0.xyz);
    t12 = inversesqrt(t12);
    t0.xyz = vec3(t12) * t0.xyz;
    vs_TEXCOORD1.z = t0.y;
    t1.xyz = in_TANGENT0.yyy * _Object2World[1].yzx;
    t1.xyz = _Object2World[0].yzx * in_TANGENT0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].yzx * in_TANGENT0.zzz + t1.xyz;
    t12 = dot(t1.xyz, t1.xyz);
    t12 = inversesqrt(t12);
    t1.xyz = vec3(t12) * t1.xyz;
    t10_2.xyz = t0.xyz * t1.xyz;
    t10_2.xyz = t0.zxy * t1.yzx + (-t10_2.xyz);
    t4 = in_TANGENT0.w * unity_WorldTransformParams.w;
    t10_2.xyz = vec3(t4) * t10_2.xyz;
    vs_TEXCOORD1.y = t10_2.x;
    t3.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t3.xyz;
    t3.xyz = _Object2World[3].xyz * in_POSITION0.www + t3.xyz;
    vs_TEXCOORD1.w = t3.x;
    vs_TEXCOORD1.x = t1.z;
    vs_TEXCOORD2.z = t0.z;
    vs_TEXCOORD3.z = t0.x;
    vs_TEXCOORD2.x = t1.x;
    vs_TEXCOORD3.x = t1.y;
    vs_TEXCOORD2.w = t3.y;
    vs_TEXCOORD3.w = t3.z;
    vs_TEXCOORD2.y = t10_2.y;
    vs_TEXCOORD3.y = t10_2.z;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _DetailNormalTex;
uniform lowp sampler2D _BumpTex;
in highp vec2 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
in highp vec4 vs_TEXCOORD2;
in highp vec4 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec3 t0;
lowp vec3 t10_0;
lowp vec3 t10_1;
mediump vec3 t16_2;
mediump float t16_11;
void main()
{
    t0.xy = vs_TEXCOORD0.xy * vec2(_DetailScale);
    t10_0.xy = texture(_DetailNormalTex, t0.xy).xy;
    t10_1.xy = t10_0.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
    t10_0.x = texture(_MaskTex, vs_TEXCOORD0.xy).x;
    t16_2.xy = t10_0.xx * t10_1.xy;
    t10_0.xyz = texture(_BumpTex, vs_TEXCOORD0.xy).xyz;
    t10_1.xyz = t10_0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_2.z = 0.0;
    t16_2.xyz = t10_1.xyz + t16_2.xyz;
    t16_11 = dot(t16_2.xyz, t16_2.xyz);
    t16_11 = inversesqrt(t16_11);
    t16_2.xyz = vec3(t16_11) * t16_2.xyz;
    t0.x = dot(vs_TEXCOORD1.xyz, t16_2.xyz);
    t0.y = dot(vs_TEXCOORD2.xyz, t16_2.xyz);
    t0.z = dot(vs_TEXCOORD3.xyz, t16_2.xyz);
    SV_Target0.xyz = t0.xyz * vec3(0.5, 0.5, 0.5) + vec3(0.5, 0.5, 0.5);
    SV_Target0.w = 1.0;
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
  Name "PREPASS"
  Tags { "LIGHTMODE"="PrePassFinal" "RenderType"="Opaque" }
  ZWrite Off
  GpuProgramID 227120
Program "vp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  highp vec3 tmpvar_3;
  tmpvar_1 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_4;
  highp vec4 tmpvar_5;
  tmpvar_5 = (tmpvar_1 * 0.5);
  highp vec2 tmpvar_6;
  tmpvar_6.x = tmpvar_5.x;
  tmpvar_6.y = (tmpvar_5.y * _ProjectionParams.x);
  o_4.xy = (tmpvar_6 + tmpvar_5.w);
  o_4.zw = tmpvar_1.zw;
  tmpvar_2.zw = vec2(0.0, 0.0);
  tmpvar_2.xy = vec2(0.0, 0.0);
  highp vec4 v_7;
  v_7.x = _World2Object[0].x;
  v_7.y = _World2Object[1].x;
  v_7.z = _World2Object[2].x;
  v_7.w = _World2Object[3].x;
  highp vec4 v_8;
  v_8.x = _World2Object[0].y;
  v_8.y = _World2Object[1].y;
  v_8.z = _World2Object[2].y;
  v_8.w = _World2Object[3].y;
  highp vec4 v_9;
  v_9.x = _World2Object[0].z;
  v_9.y = _World2Object[1].z;
  v_9.z = _World2Object[2].z;
  v_9.w = _World2Object[3].z;
  highp vec4 tmpvar_10;
  tmpvar_10.w = 1.0;
  tmpvar_10.xyz = normalize(((
    (v_7.xyz * _glesNormal.x)
   + 
    (v_8.xyz * _glesNormal.y)
  ) + (v_9.xyz * _glesNormal.z)));
  mediump vec3 tmpvar_11;
  mediump vec4 normal_12;
  normal_12 = tmpvar_10;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  x1_14.x = dot (unity_SHAr, normal_12);
  x1_14.y = dot (unity_SHAg, normal_12);
  x1_14.z = dot (unity_SHAb, normal_12);
  mediump vec4 tmpvar_15;
  tmpvar_15 = (normal_12.xyzz * normal_12.yzzx);
  x2_13.x = dot (unity_SHBr, tmpvar_15);
  x2_13.y = dot (unity_SHBg, tmpvar_15);
  x2_13.z = dot (unity_SHBb, tmpvar_15);
  tmpvar_11 = ((x2_13 + (unity_SHC.xyz * 
    ((normal_12.x * normal_12.x) - (normal_12.y * normal_12.y))
  )) + x1_14);
  tmpvar_3 = tmpvar_11;
  gl_Position = tmpvar_1;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = (_Object2World * _glesVertex).xyz;
  xlv_TEXCOORD2 = o_4;
  xlv_TEXCOORD3 = tmpvar_2;
  xlv_TEXCOORD4 = tmpvar_3;
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform highp vec4 _SkinColor;
uniform sampler2D _LightBuffer;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec4 light_3;
  lowp vec3 tmpvar_4;
  lowp float tmpvar_5;
  lowp float tmpvar_6;
  mediump vec4 m_7;
  mediump vec4 c_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_8 = tmpvar_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MaskTex, xlv_TEXCOORD0);
  m_7 = tmpvar_10;
  tmpvar_4 = (c_8.xyz * _SkinColor.xyz);
  tmpvar_6 = c_8.w;
  tmpvar_5 = m_7.y;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
  light_3 = tmpvar_11;
  light_3 = -(log2(max (light_3, vec4(0.001, 0.001, 0.001, 0.001))));
  light_3.xyz = (light_3.xyz + xlv_TEXCOORD4);
  lowp vec4 c_12;
  lowp float spec_13;
  mediump float tmpvar_14;
  tmpvar_14 = (light_3.w * tmpvar_5);
  spec_13 = tmpvar_14;
  c_12.xyz = ((tmpvar_4 * light_3.xyz) + ((light_3.xyz * _SpecColor.xyz) * spec_13));
  c_12.w = tmpvar_6;
  c_2.xyz = c_12.xyz;
  c_2.w = 1.0;
  tmpvar_1 = c_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
uniform 	lowp vec4 unity_Ambient;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec4 vs_TEXCOORD2;
out highp vec4 vs_TEXCOORD3;
out highp vec3 vs_TEXCOORD4;
highp vec4 t0;
highp vec4 t1;
mediump vec4 t16_1;
mediump vec3 t16_2;
mediump vec3 t16_3;
highp float t12;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    gl_Position = t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    vs_TEXCOORD1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t0.y = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xwy * vec3(0.5, 0.5, 0.5);
    vs_TEXCOORD2.zw = t0.zw;
    vs_TEXCOORD2.xy = t1.zz + t1.xw;
    vs_TEXCOORD3 = vec4(0.0, 0.0, 0.0, 0.0);
    t0.x = in_NORMAL0.x * _World2Object[0].x;
    t0.y = in_NORMAL0.x * _World2Object[1].x;
    t0.z = in_NORMAL0.x * _World2Object[2].x;
    t1.x = in_NORMAL0.y * _World2Object[0].y;
    t1.y = in_NORMAL0.y * _World2Object[1].y;
    t1.z = in_NORMAL0.y * _World2Object[2].y;
    t0.xyz = t0.xyz + t1.xyz;
    t1.x = in_NORMAL0.z * _World2Object[0].z;
    t1.y = in_NORMAL0.z * _World2Object[1].z;
    t1.z = in_NORMAL0.z * _World2Object[2].z;
    t0.xyz = t0.xyz + t1.xyz;
    t12 = dot(t0.xyz, t0.xyz);
    t12 = inversesqrt(t12);
    t0.xyz = vec3(t12) * t0.xyz;
    t16_2.x = t0.y * t0.y;
    t16_2.x = t0.x * t0.x + (-t16_2.x);
    t16_1 = t0.yzzx * t0.xyzz;
    t16_3.x = dot(unity_SHBr, t16_1);
    t16_3.y = dot(unity_SHBg, t16_1);
    t16_3.z = dot(unity_SHBb, t16_1);
    t16_2.xyz = unity_SHC.xyz * t16_2.xxx + t16_3.xyz;
    t0.w = 1.0;
    t16_3.x = dot(unity_SHAr, t0);
    t16_3.y = dot(unity_SHAg, t0);
    t16_3.z = dot(unity_SHAb, t0);
    t16_2.xyz = t16_2.xyz + t16_3.xyz;
    vs_TEXCOORD4.xyz = t16_2.xyz;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
uniform 	lowp vec4 unity_Ambient;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _LightBuffer;
in highp vec2 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD4;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec2 t0;
mediump vec4 t16_0;
lowp vec4 t10_0;
highp vec3 t1;
lowp float t10_1;
mediump vec3 t16_2;
highp vec3 t3;
lowp vec3 t10_3;
mediump vec3 t16_6;
void main()
{
    t0.xy = vs_TEXCOORD2.xy / vs_TEXCOORD2.ww;
    t10_0 = texture(_LightBuffer, t0.xy);
    t16_0 = max(t10_0, vec4(0.00100000005, 0.00100000005, 0.00100000005, 0.00100000005));
    t16_0 = log2(t16_0);
    t10_1 = texture(_MaskTex, vs_TEXCOORD0.xy).y;
    t16_2.x = (-t16_0.w) * t10_1;
    t1.xyz = (-t16_0.xyz) + vs_TEXCOORD4.xyz;
    t16_6.xyz = t1.xyz * _SpecColor.xyz;
    t16_2.xyz = t16_2.xxx * t16_6.xyz;
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t3.xyz = t10_3.xyz * _SkinColor.xyz;
    t16_2.xyz = t3.xyz * t1.xyz + t16_2.xyz;
    SV_Target0.xyz = t16_2.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "UNITY_HDR_ON" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  highp vec3 tmpvar_3;
  tmpvar_1 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_4;
  highp vec4 tmpvar_5;
  tmpvar_5 = (tmpvar_1 * 0.5);
  highp vec2 tmpvar_6;
  tmpvar_6.x = tmpvar_5.x;
  tmpvar_6.y = (tmpvar_5.y * _ProjectionParams.x);
  o_4.xy = (tmpvar_6 + tmpvar_5.w);
  o_4.zw = tmpvar_1.zw;
  tmpvar_2.zw = vec2(0.0, 0.0);
  tmpvar_2.xy = vec2(0.0, 0.0);
  highp vec4 v_7;
  v_7.x = _World2Object[0].x;
  v_7.y = _World2Object[1].x;
  v_7.z = _World2Object[2].x;
  v_7.w = _World2Object[3].x;
  highp vec4 v_8;
  v_8.x = _World2Object[0].y;
  v_8.y = _World2Object[1].y;
  v_8.z = _World2Object[2].y;
  v_8.w = _World2Object[3].y;
  highp vec4 v_9;
  v_9.x = _World2Object[0].z;
  v_9.y = _World2Object[1].z;
  v_9.z = _World2Object[2].z;
  v_9.w = _World2Object[3].z;
  highp vec4 tmpvar_10;
  tmpvar_10.w = 1.0;
  tmpvar_10.xyz = normalize(((
    (v_7.xyz * _glesNormal.x)
   + 
    (v_8.xyz * _glesNormal.y)
  ) + (v_9.xyz * _glesNormal.z)));
  mediump vec3 tmpvar_11;
  mediump vec4 normal_12;
  normal_12 = tmpvar_10;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  x1_14.x = dot (unity_SHAr, normal_12);
  x1_14.y = dot (unity_SHAg, normal_12);
  x1_14.z = dot (unity_SHAb, normal_12);
  mediump vec4 tmpvar_15;
  tmpvar_15 = (normal_12.xyzz * normal_12.yzzx);
  x2_13.x = dot (unity_SHBr, tmpvar_15);
  x2_13.y = dot (unity_SHBg, tmpvar_15);
  x2_13.z = dot (unity_SHBb, tmpvar_15);
  tmpvar_11 = ((x2_13 + (unity_SHC.xyz * 
    ((normal_12.x * normal_12.x) - (normal_12.y * normal_12.y))
  )) + x1_14);
  tmpvar_3 = tmpvar_11;
  gl_Position = tmpvar_1;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = (_Object2World * _glesVertex).xyz;
  xlv_TEXCOORD2 = o_4;
  xlv_TEXCOORD3 = tmpvar_2;
  xlv_TEXCOORD4 = tmpvar_3;
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 _SpecColor;
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform highp vec4 _SkinColor;
uniform sampler2D _LightBuffer;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec4 light_3;
  lowp vec3 tmpvar_4;
  lowp float tmpvar_5;
  lowp float tmpvar_6;
  mediump vec4 m_7;
  mediump vec4 c_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_8 = tmpvar_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MaskTex, xlv_TEXCOORD0);
  m_7 = tmpvar_10;
  tmpvar_4 = (c_8.xyz * _SkinColor.xyz);
  tmpvar_6 = c_8.w;
  tmpvar_5 = m_7.y;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
  light_3 = tmpvar_11;
  mediump vec4 tmpvar_12;
  tmpvar_12 = max (light_3, vec4(0.001, 0.001, 0.001, 0.001));
  light_3.w = tmpvar_12.w;
  light_3.xyz = (tmpvar_12.xyz + xlv_TEXCOORD4);
  lowp vec4 c_13;
  lowp float spec_14;
  mediump float tmpvar_15;
  tmpvar_15 = (tmpvar_12.w * tmpvar_5);
  spec_14 = tmpvar_15;
  c_13.xyz = ((tmpvar_4 * light_3.xyz) + ((light_3.xyz * _SpecColor.xyz) * spec_14));
  c_13.w = tmpvar_6;
  c_2.xyz = c_13.xyz;
  c_2.w = 1.0;
  tmpvar_1 = c_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "UNITY_HDR_ON" }
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
uniform 	lowp vec4 unity_Ambient;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec4 vs_TEXCOORD2;
out highp vec4 vs_TEXCOORD3;
out highp vec3 vs_TEXCOORD4;
highp vec4 t0;
highp vec4 t1;
mediump vec4 t16_1;
mediump vec3 t16_2;
mediump vec3 t16_3;
highp float t12;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    gl_Position = t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    vs_TEXCOORD1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t0.y = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xwy * vec3(0.5, 0.5, 0.5);
    vs_TEXCOORD2.zw = t0.zw;
    vs_TEXCOORD2.xy = t1.zz + t1.xw;
    vs_TEXCOORD3 = vec4(0.0, 0.0, 0.0, 0.0);
    t0.x = in_NORMAL0.x * _World2Object[0].x;
    t0.y = in_NORMAL0.x * _World2Object[1].x;
    t0.z = in_NORMAL0.x * _World2Object[2].x;
    t1.x = in_NORMAL0.y * _World2Object[0].y;
    t1.y = in_NORMAL0.y * _World2Object[1].y;
    t1.z = in_NORMAL0.y * _World2Object[2].y;
    t0.xyz = t0.xyz + t1.xyz;
    t1.x = in_NORMAL0.z * _World2Object[0].z;
    t1.y = in_NORMAL0.z * _World2Object[1].z;
    t1.z = in_NORMAL0.z * _World2Object[2].z;
    t0.xyz = t0.xyz + t1.xyz;
    t12 = dot(t0.xyz, t0.xyz);
    t12 = inversesqrt(t12);
    t0.xyz = vec3(t12) * t0.xyz;
    t16_2.x = t0.y * t0.y;
    t16_2.x = t0.x * t0.x + (-t16_2.x);
    t16_1 = t0.yzzx * t0.xyzz;
    t16_3.x = dot(unity_SHBr, t16_1);
    t16_3.y = dot(unity_SHBg, t16_1);
    t16_3.z = dot(unity_SHBb, t16_1);
    t16_2.xyz = unity_SHC.xyz * t16_2.xxx + t16_3.xyz;
    t0.w = 1.0;
    t16_3.x = dot(unity_SHAr, t0);
    t16_3.y = dot(unity_SHAg, t0);
    t16_3.z = dot(unity_SHAb, t0);
    t16_2.xyz = t16_2.xyz + t16_3.xyz;
    vs_TEXCOORD4.xyz = t16_2.xyz;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	float _DetailScale;
uniform 	vec4 _SkinColor;
uniform 	vec4 _MainTex_ST;
uniform 	lowp vec4 unity_Ambient;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _LightBuffer;
in highp vec2 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD4;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec3 t0;
lowp float t10_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
mediump vec3 t16_2;
highp vec3 t3;
lowp vec3 t10_3;
highp vec2 t4;
mediump vec3 t16_6;
void main()
{
    t10_0 = texture(_MaskTex, vs_TEXCOORD0.xy).y;
    t4.xy = vs_TEXCOORD2.xy / vs_TEXCOORD2.ww;
    t10_1 = texture(_LightBuffer, t4.xy);
    t16_1 = max(t10_1, vec4(0.00100000005, 0.00100000005, 0.00100000005, 0.00100000005));
    t16_2.x = t10_0 * t16_1.w;
    t0.xyz = t16_1.xyz + vs_TEXCOORD4.xyz;
    t16_6.xyz = t0.xyz * _SpecColor.xyz;
    t16_2.xyz = t16_2.xxx * t16_6.xyz;
    t10_3.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t3.xyz = t10_3.xyz * _SkinColor.xyz;
    t16_2.xyz = t3.xyz * t0.xyz + t16_2.xyz;
    SV_Target0.xyz = t16_2.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "UNITY_HDR_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "UNITY_HDR_ON" }
"!!GLES3"
}
}
 }
}
Fallback "Diffuse"
}
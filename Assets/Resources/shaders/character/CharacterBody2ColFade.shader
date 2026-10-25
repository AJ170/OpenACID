š¨Shader "Character/CharacterBody2ColFade" {
Properties {
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
 _BumpMap ("Normalmap", 2D) = "bump" { }
 _MaskTex ("Colorize (RG) Specular Mask (B)", 2D) = "white" { }
 _Color0 ("Color 0", Color) = (1,1,1,1)
 _Color1 ("Color 1", Color) = (1,1,1,1)
 _NoiseGlow ("Noise glow color and layer", Color) = (1,1,1,0)
 _FadeColor ("Fade Color", Color) = (1,1,1,1)
 _MatGlossiness ("Glossiness", Float) = 1
 _RimColor ("RimColor, will be set from code", Color) = (1,0,1,1)
}
SubShader { 
 LOD 99
 Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="AlphaTest+2" "RenderType"="Opaque" "EagleVision"="humanoid" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="AlphaTest+2" "RenderType"="Opaque" "EagleVision"="humanoid" }
  ZTest Less
  ColorMask 0
  GpuProgramID 48741
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
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="AlphaTest+2" "RenderType"="Opaque" "EagleVision"="humanoid" }
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  GpuProgramID 70494
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SIMPLE_FADE" }
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
uniform sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump float tmpvar_3;
mediump float tmpvar_4;
mediump float tmpvar_5;
mediump vec3 tmpvar_6;
mediump vec3 tmpvar_7;
mediump float tmpvar_8;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_9;
  mediump vec3 lit_color_10;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_11.xyz;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_12.x;
  tmpvar_3 = tmpvar_12.y;
  tmpvar_4 = tmpvar_12.z;
  tmpvar_5 = tmpvar_12.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec3 tangentNormal_13;
  mediump vec3 tmpvar_14;
  tmpvar_14.x = xlv_TEXCOORD3.w;
  tmpvar_14.y = xlv_TEXCOORD4.w;
  tmpvar_14.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_15;
  mediump vec3 tmpvar_16;
  tmpvar_15 = xlv_TEXCOORD5.xyz;
  tmpvar_16 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_17;
  tmpvar_17[0].x = tmpvar_15.x;
  tmpvar_17[0].y = tmpvar_14.x;
  tmpvar_17[0].z = tmpvar_16.x;
  tmpvar_17[1].x = tmpvar_15.y;
  tmpvar_17[1].y = tmpvar_14.y;
  tmpvar_17[1].z = tmpvar_16.y;
  tmpvar_17[2].x = tmpvar_15.z;
  tmpvar_17[2].y = tmpvar_14.z;
  tmpvar_17[2].z = tmpvar_16.z;
  lowp vec3 tmpvar_18;
  tmpvar_18 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_13 = tmpvar_18;
  tmpvar_7 = (tangentNormal_13 * tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19.w = 1.0;
  tmpvar_19.xyz = tmpvar_7;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  x1_21.x = dot (unity_SHAr, tmpvar_19);
  x1_21.y = dot (unity_SHAg, tmpvar_19);
  x1_21.z = dot (unity_SHAb, tmpvar_19);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (tmpvar_7.xyzz * tmpvar_7.yzzx);
  x2_20.x = dot (unity_SHBr, tmpvar_22);
  x2_20.y = dot (unity_SHBg, tmpvar_22);
  x2_20.z = dot (unity_SHBb, tmpvar_22);
  mediump vec3 tmpvar_23;
  tmpvar_23 = max (((1.055 * 
    pow ((((x2_20 + 
      (unity_SHC.xyz * ((tmpvar_7.x * tmpvar_7.x) - (tmpvar_7.y * tmpvar_7.y)))
    ) + x1_21) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  mediump vec3 cube_24;
  mediump vec3 tmpvar_25;
  mediump vec3 I_26;
  I_26 = -(xlv_TEXCOORD3.xyz);
  tmpvar_25 = (I_26 - (2.0 * (
    dot (tmpvar_7, I_26)
   * tmpvar_7)));
  lowp vec4 tmpvar_27;
  tmpvar_27 = textureCube (_HiSpecCube, tmpvar_25);
  cube_24 = tmpvar_27.xyz;
  tmpvar_8 = ((max (
    dot (tmpvar_7, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_10 = (((
    ((tmpvar_23 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_8), vec3(
    min (1.0, (tmpvar_8 * 1.5))
  ))) + ((
    ((((
      (max (dot ((xlv_TEXCOORD3.xyz - 
        (2.0 * (dot (tmpvar_7, xlv_TEXCOORD3.xyz) * tmpvar_7))
      ), -(tmpvar_7)), 0.25) * _Glossiness.w)
     * 
      (10.0 * _MatGlossiness)
    ) * _Glossiness.xyz) * tmpvar_23) * cube_24)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 tmpvar_28;
  tmpvar_28 = mix (lit_color_10, vec3((dot (lit_color_10, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_8 * _LightParameters.y)));
  c_9.xyz = tmpvar_28;
  c_9.w = _RimColor.w;
  lowp vec4 _color_29;
  _color_29 = (c_9 * _FadeColor);
  c_9.w = _color_29.w;
  c_9.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_29.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_9;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SIMPLE_FADE" }
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec4 t16_4;
mediump vec3 t16_5;
lowp vec3 t10_6;
mediump vec3 t16_7;
mediump float t16_21;
mediump float t16_24;
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
    t16_21 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_21 = t16_21 + t16_21;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_21)) + vs_TEXCOORD3.xyz;
    t16_21 = dot(t16_4.xyz, (-t16_0.xyz));
    t16_21 = max(t16_21, 0.25);
    t16_21 = t16_21 * _Glossiness.w;
    t16_21 = t16_21 * _MatGlossiness;
    t16_21 = t16_21 * 10.0;
    t16_4.xyz = vec3(t16_21) * _Glossiness.xyz;
    t16_4.xyz = t16_3.xyz * t16_4.xyz;
    t16_21 = dot((-vs_TEXCOORD3.xyz), t16_0.xyz);
    t16_21 = t16_21 + t16_21;
    t16_5.xyz = t16_0.xyz * (-vec3(t16_21)) + (-vs_TEXCOORD3.xyz);
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.x = max(t16_0.x, 0.0);
    t10_6.xyz = texture(_HiSpecCube, t16_5.xyz).xyz;
    t16_7.xyz = t16_4.xyz * t10_6.xyz;
    t10_1 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_4.xy = vec2((-t10_1.w) + float(1.0), (-t10_1.z) + float(1.0));
    t16_24 = (-t16_4.x) * _LightParameters.z + 1.0;
    t16_4.x = t10_1.z * t16_24;
    t16_4.x = t16_4.x * 5.0;
    t16_7.xyz = t16_7.xyz * t16_4.xxx;
    t16_4.xzw = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_4.xzw = t10_1.xxx * t16_4.xzw + vec3(1.0, 1.0, 1.0);
    t10_6.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_4.xzw = t16_4.xzw * t10_6.xyz;
    t16_5.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_5.xyz = t10_1.yyy * t16_5.xyz + vec3(1.0, 1.0, 1.0);
    t16_4.xzw = t16_4.xzw * t16_5.xyz;
    t16_7.xyz = t16_7.xyz * t16_4.xzw;
    t16_3.xyz = t16_3.xyz * t16_4.xzw;
    t16_3.xyz = vec3(t16_24) * t16_3.xyz;
    t16_3.xyz = t16_4.yyy * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_24 = t16_0.x * 1.5;
    t16_24 = min(t16_24, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_24) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_7.xyz = t16_3.xyz * t16_4.xyz + t16_7.xyz;
    t16_3.x = dot(t16_7.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_7.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_7.xyz;
    t10_2.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t10_2.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w * _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FANCY_FADE" }
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
uniform sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump float tmpvar_3;
mediump float tmpvar_4;
mediump float tmpvar_5;
mediump vec3 tmpvar_6;
mediump vec3 tmpvar_7;
mediump float tmpvar_8;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_9;
  mediump vec3 lit_color_10;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_11.xyz;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_12.x;
  tmpvar_3 = tmpvar_12.y;
  tmpvar_4 = tmpvar_12.z;
  tmpvar_5 = tmpvar_12.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec3 tangentNormal_13;
  mediump vec3 tmpvar_14;
  tmpvar_14.x = xlv_TEXCOORD3.w;
  tmpvar_14.y = xlv_TEXCOORD4.w;
  tmpvar_14.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_15;
  mediump vec3 tmpvar_16;
  tmpvar_15 = xlv_TEXCOORD5.xyz;
  tmpvar_16 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_17;
  tmpvar_17[0].x = tmpvar_15.x;
  tmpvar_17[0].y = tmpvar_14.x;
  tmpvar_17[0].z = tmpvar_16.x;
  tmpvar_17[1].x = tmpvar_15.y;
  tmpvar_17[1].y = tmpvar_14.y;
  tmpvar_17[1].z = tmpvar_16.y;
  tmpvar_17[2].x = tmpvar_15.z;
  tmpvar_17[2].y = tmpvar_14.z;
  tmpvar_17[2].z = tmpvar_16.z;
  lowp vec3 tmpvar_18;
  tmpvar_18 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_13 = tmpvar_18;
  tmpvar_7 = (tangentNormal_13 * tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19.w = 1.0;
  tmpvar_19.xyz = tmpvar_7;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  x1_21.x = dot (unity_SHAr, tmpvar_19);
  x1_21.y = dot (unity_SHAg, tmpvar_19);
  x1_21.z = dot (unity_SHAb, tmpvar_19);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (tmpvar_7.xyzz * tmpvar_7.yzzx);
  x2_20.x = dot (unity_SHBr, tmpvar_22);
  x2_20.y = dot (unity_SHBg, tmpvar_22);
  x2_20.z = dot (unity_SHBb, tmpvar_22);
  mediump vec3 tmpvar_23;
  tmpvar_23 = max (((1.055 * 
    pow ((((x2_20 + 
      (unity_SHC.xyz * ((tmpvar_7.x * tmpvar_7.x) - (tmpvar_7.y * tmpvar_7.y)))
    ) + x1_21) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  mediump vec3 cube_24;
  mediump vec3 tmpvar_25;
  mediump vec3 I_26;
  I_26 = -(xlv_TEXCOORD3.xyz);
  tmpvar_25 = (I_26 - (2.0 * (
    dot (tmpvar_7, I_26)
   * tmpvar_7)));
  lowp vec4 tmpvar_27;
  tmpvar_27 = textureCube (_HiSpecCube, tmpvar_25);
  cube_24 = tmpvar_27.xyz;
  tmpvar_8 = ((max (
    dot (tmpvar_7, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_10 = (((
    ((tmpvar_23 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_8), vec3(
    min (1.0, (tmpvar_8 * 1.5))
  ))) + ((
    ((((
      (max (dot ((xlv_TEXCOORD3.xyz - 
        (2.0 * (dot (tmpvar_7, xlv_TEXCOORD3.xyz) * tmpvar_7))
      ), -(tmpvar_7)), 0.25) * _Glossiness.w)
     * 
      (10.0 * _MatGlossiness)
    ) * _Glossiness.xyz) * tmpvar_23) * cube_24)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 tmpvar_28;
  tmpvar_28 = mix (lit_color_10, vec3((dot (lit_color_10, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_8 * _LightParameters.y)));
  c_9.xyz = tmpvar_28;
  c_9.w = _RimColor.w;
  lowp vec4 _color_29;
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_31;
  tmpvar_31 = (_FadeColor.w - ((0.9375 * tmpvar_30.y) + 0.03125));
  mediump float tmpvar_32;
  tmpvar_32 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_31)
  )), 0.0, 1.0);
  _color_29.xyz = (_FadeColor.xyz * (tmpvar_32 + c_9.xyz));
  _color_29.w = ((32.0 * tmpvar_31) + 0.9375);
  c_9.w = _color_29.w;
  c_9.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_29.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_9;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FANCY_FADE" }
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
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
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec4 t16_4;
mediump vec3 t16_5;
lowp vec3 t10_6;
mediump vec3 t16_7;
mediump float t16_21;
mediump float t16_24;
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
    t16_21 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_21 = t16_21 + t16_21;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_21)) + vs_TEXCOORD3.xyz;
    t16_21 = dot(t16_4.xyz, (-t16_0.xyz));
    t16_21 = max(t16_21, 0.25);
    t16_21 = t16_21 * _Glossiness.w;
    t16_21 = t16_21 * _MatGlossiness;
    t16_21 = t16_21 * 10.0;
    t16_4.xyz = vec3(t16_21) * _Glossiness.xyz;
    t16_4.xyz = t16_3.xyz * t16_4.xyz;
    t16_21 = dot((-vs_TEXCOORD3.xyz), t16_0.xyz);
    t16_21 = t16_21 + t16_21;
    t16_5.xyz = t16_0.xyz * (-vec3(t16_21)) + (-vs_TEXCOORD3.xyz);
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.x = max(t16_0.x, 0.0);
    t10_6.xyz = texture(_HiSpecCube, t16_5.xyz).xyz;
    t16_7.xyz = t16_4.xyz * t10_6.xyz;
    t10_1 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_4.xy = vec2((-t10_1.w) + float(1.0), (-t10_1.z) + float(1.0));
    t16_24 = (-t16_4.x) * _LightParameters.z + 1.0;
    t16_4.x = t10_1.z * t16_24;
    t16_4.x = t16_4.x * 5.0;
    t16_7.xyz = t16_7.xyz * t16_4.xxx;
    t16_4.xzw = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_4.xzw = t10_1.xxx * t16_4.xzw + vec3(1.0, 1.0, 1.0);
    t10_6.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_4.xzw = t16_4.xzw * t10_6.xyz;
    t16_5.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_5.xyz = t10_1.yyy * t16_5.xyz + vec3(1.0, 1.0, 1.0);
    t16_4.xzw = t16_4.xzw * t16_5.xyz;
    t16_7.xyz = t16_7.xyz * t16_4.xzw;
    t16_3.xyz = t16_3.xyz * t16_4.xzw;
    t16_3.xyz = vec3(t16_24) * t16_3.xyz;
    t16_3.xyz = t16_4.yyy * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_24 = t16_0.x * 1.5;
    t16_24 = min(t16_24, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_24) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_7.xyz = t16_3.xyz * t16_4.xyz + t16_7.xyz;
    t16_3.x = dot(t16_7.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_7.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_7.xyz;
    t10_6.x = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_21 = t10_6.x * 0.9375 + 0.03125;
    t16_21 = (-t16_21) + _FadeColor.w;
    t16_3.x = -abs(t16_21) * 32.0 + 1.0;
    t16_1.w = t16_21 * 32.0 + 0.9375;
    t16_21 = max(t16_3.x, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_21);
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0 = t16_1;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  highp vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  mediump vec2 tmpvar_10;
  mediump vec3 tmpvar_11;
  tmpvar_10.x = 0.0;
  tmpvar_7 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = (tmpvar_7 * 0.5);
  highp vec2 tmpvar_14;
  tmpvar_14.x = tmpvar_13.x;
  tmpvar_14.y = (tmpvar_13.y * _ProjectionParams.x);
  o_12.xy = (tmpvar_14 + tmpvar_13.w);
  o_12.zw = tmpvar_7.zw;
  tmpvar_11 = o_12.xyw;
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_8.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_9.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_9.zw = tmpvar_1.xy;
  highp vec3 tmpvar_15;
  tmpvar_15 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_15;
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _Object2World[0].xyz;
  tmpvar_16[1] = _Object2World[1].xyz;
  tmpvar_16[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_16 * _glesNormal));
  worldNormal_5 = tmpvar_17;
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_18 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_19;
  mediump vec3 tmpvar_20;
  tmpvar_20 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22.xyz = worldViewDir_6;
  tmpvar_22.w = worldBinormal_3.x;
  mediump vec4 tmpvar_23;
  tmpvar_23.xyz = worldNormal_5;
  tmpvar_23.w = worldBinormal_3.y;
  mediump vec4 tmpvar_24;
  tmpvar_24.xyz = worldTangent_4;
  tmpvar_24.w = worldBinormal_3.z;
  highp float tmpvar_25;
  highp vec3 x_26;
  x_26 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_25 = sqrt(dot (x_26, x_26));
  fogpos_2 = tmpvar_25;
  tmpvar_10.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_10.x = (1.0 - tmpvar_10.y);
  gl_Position = tmpvar_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = tmpvar_10;
  xlv_TEXCOORD3 = tmpvar_22;
  xlv_TEXCOORD4 = tmpvar_23;
  xlv_TEXCOORD5 = tmpvar_24;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_11;
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
uniform sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump float tmpvar_3;
mediump float tmpvar_4;
mediump float tmpvar_5;
mediump vec3 tmpvar_6;
mediump vec3 tmpvar_7;
mediump float tmpvar_8;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  lowp vec4 c_9;
  mediump vec3 lit_color_10;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_11.xyz;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_12.x;
  tmpvar_3 = tmpvar_12.y;
  tmpvar_4 = tmpvar_12.z;
  tmpvar_5 = tmpvar_12.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec3 tangentNormal_13;
  mediump vec3 tmpvar_14;
  tmpvar_14.x = xlv_TEXCOORD3.w;
  tmpvar_14.y = xlv_TEXCOORD4.w;
  tmpvar_14.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_15;
  mediump vec3 tmpvar_16;
  tmpvar_15 = xlv_TEXCOORD5.xyz;
  tmpvar_16 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_17;
  tmpvar_17[0].x = tmpvar_15.x;
  tmpvar_17[0].y = tmpvar_14.x;
  tmpvar_17[0].z = tmpvar_16.x;
  tmpvar_17[1].x = tmpvar_15.y;
  tmpvar_17[1].y = tmpvar_14.y;
  tmpvar_17[1].z = tmpvar_16.y;
  tmpvar_17[2].x = tmpvar_15.z;
  tmpvar_17[2].y = tmpvar_14.z;
  tmpvar_17[2].z = tmpvar_16.z;
  lowp vec3 tmpvar_18;
  tmpvar_18 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_13 = tmpvar_18;
  tmpvar_7 = (tangentNormal_13 * tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19.w = 1.0;
  tmpvar_19.xyz = tmpvar_7;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  x1_21.x = dot (unity_SHAr, tmpvar_19);
  x1_21.y = dot (unity_SHAg, tmpvar_19);
  x1_21.z = dot (unity_SHAb, tmpvar_19);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (tmpvar_7.xyzz * tmpvar_7.yzzx);
  x2_20.x = dot (unity_SHBr, tmpvar_22);
  x2_20.y = dot (unity_SHBg, tmpvar_22);
  x2_20.z = dot (unity_SHBb, tmpvar_22);
  mediump vec3 tmpvar_23;
  tmpvar_23 = max (((1.055 * 
    pow ((((x2_20 + 
      (unity_SHC.xyz * ((tmpvar_7.x * tmpvar_7.x) - (tmpvar_7.y * tmpvar_7.y)))
    ) + x1_21) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  mediump vec3 cube_24;
  mediump vec3 tmpvar_25;
  mediump vec3 I_26;
  I_26 = -(xlv_TEXCOORD3.xyz);
  tmpvar_25 = (I_26 - (2.0 * (
    dot (tmpvar_7, I_26)
   * tmpvar_7)));
  lowp vec4 tmpvar_27;
  tmpvar_27 = textureCube (_HiSpecCube, tmpvar_25);
  cube_24 = tmpvar_27.xyz;
  tmpvar_8 = ((max (
    dot (tmpvar_7, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_10 = (((
    ((tmpvar_23 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_8), vec3(
    min (1.0, (tmpvar_8 * 1.5))
  ))) + ((
    ((((
      (max (dot ((xlv_TEXCOORD3.xyz - 
        (2.0 * (dot (tmpvar_7, xlv_TEXCOORD3.xyz) * tmpvar_7))
      ), -(tmpvar_7)), 0.25) * _Glossiness.w)
     * 
      (10.0 * _MatGlossiness)
    ) * _Glossiness.xyz) * tmpvar_23) * cube_24)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 tmpvar_28;
  tmpvar_28 = mix (lit_color_10, vec3((dot (lit_color_10, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_8 * _LightParameters.y)));
  c_9.xyz = tmpvar_28;
  c_9.w = _RimColor.w;
  lowp vec4 _color_29;
  _color_29.w = c_9.w;
  mediump float rTest_30;
  mediump vec2 tmpvar_31;
  tmpvar_31 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_32;
  tmpvar_32 = ((1.5 * _FadeColor.w) - tmpvar_31.y);
  if ((tmpvar_32 < 0.0)) {
    discard;
  };
  mediump float tmpvar_33;
  tmpvar_33 = clamp ((1.0 - (tmpvar_32 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_34;
  tmpvar_34.x = tmpvar_31.x;
  tmpvar_34.y = clamp ((tmpvar_33 - tmpvar_31.y), 0.0, 1.0);
  lowp float tmpvar_35;
  tmpvar_35 = texture2D (_RasterTex, tmpvar_34).z;
  rTest_30 = tmpvar_35;
  mediump float x_36;
  x_36 = (rTest_30 - 0.4);
  if ((x_36 < 0.0)) {
    discard;
  };
  _color_29.xyz = (c_9.xyz + (_FadeColor.xyz * tmpvar_33));
  c_9.w = _color_29.w;
  c_9.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_29.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_9;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "RASTER_FADE" }
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
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
out mediump vec3 vs_TEXCOORD7;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_2;
highp vec4 t3;
mediump vec3 t16_4;
highp vec3 t5;
mediump float t16_8;
highp float t18;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t1.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t1;
    t1.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t1.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t1;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = sqrt(t18);
    t16_2 = (-t18) + acid_Fog.y;
    t16_8 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_8;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_8 = t16_2 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_8;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = inversesqrt(t18);
    t1.xyz = vec3(t18) * t1.xyz;
    t3.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t2.xyz = vec3(t18) * t3.xyz;
    t3.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t3.xyz = vec3(t18) * t3.xyz;
    t16_4.xyz = t2.zxy * t3.yzx;
    t16_4.xyz = t2.yzx * t3.zxy + (-t16_4.xyz);
    t5.xyz = t16_4.xyz * in_TANGENT0.www;
    t18 = dot(t5.xyz, t5.xyz);
    t18 = inversesqrt(t18);
    t5.xyz = vec3(t18) * t5.xyz;
    t1.w = t5.x;
    vs_TEXCOORD3 = t1;
    t2.w = t5.y;
    t3.w = t5.z;
    vs_TEXCOORD5 = t3;
    vs_TEXCOORD4 = t2;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0.w = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t1.zz + t1.xw;
    vs_TEXCOORD7.xyz = t0.xyz;
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
uniform lowp sampler2D _RasterTex;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec3 vs_TEXCOORD7;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
bool tb1;
mediump vec4 t16_2;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp vec3 t10_5;
mediump vec3 t16_6;
mediump vec3 t16_7;
lowp float t10_8;
mediump vec3 t16_13;
mediump float t16_22;
mediump float t16_27;
mediump float t16_30;
void main()
{
    t16_0.xy = vs_TEXCOORD7.xy / vs_TEXCOORD7.zz;
    t16_27 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_27<0.0;
    t16_27 = (-t16_27) * 2.5 + 1.0;
    t16_27 = clamp(t16_27, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_27;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1.x = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1.x + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = vs_TEXCOORD5.y;
    t16_0.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = t16_1.y * t16_1.y;
    t16_0.x = t16_1.x * t16_1.x + (-t16_0.x);
    t16_3.xy = vs_TEXCOORD5.zw;
    t16_3.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_2.xyz, t16_3.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_3.x = dot(unity_SHBr, t16_2);
    t16_3.y = dot(unity_SHBg, t16_2);
    t16_3.z = dot(unity_SHBb, t16_2);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_3.xyz;
    t16_1.w = 1.0;
    t16_3.x = dot(unity_SHAr, t16_1);
    t16_3.y = dot(unity_SHAg, t16_1);
    t16_3.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_3.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t16_3.x = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_3.x = t16_3.x + t16_3.x;
    t16_3.xyz = t16_1.xyz * (-t16_3.xxx) + vs_TEXCOORD3.xyz;
    t16_3.x = dot(t16_3.xyz, (-t16_1.xyz));
    t16_3.x = max(t16_3.x, 0.25);
    t16_3.x = t16_3.x * _Glossiness.w;
    t16_3.x = t16_3.x * _MatGlossiness;
    t16_3.x = t16_3.x * 10.0;
    t16_3.xyz = t16_3.xxx * _Glossiness.xyz;
    t16_3.xyz = t16_0.xyz * t16_3.xyz;
    t16_30 = dot((-vs_TEXCOORD3.xyz), t16_1.xyz);
    t16_30 = t16_30 + t16_30;
    t16_4.xyz = t16_1.xyz * (-vec3(t16_30)) + (-vs_TEXCOORD3.xyz);
    t16_30 = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_30 = max(t16_30, 0.0);
    t10_5.xyz = texture(_HiSpecCube, t16_4.xyz).xyz;
    t16_3.xyz = t16_3.xyz * t10_5.xyz;
    t10_1 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_4.xy = vec2((-t10_1.w) + float(1.0), (-t10_1.z) + float(1.0));
    t16_4.x = (-t16_4.x) * _LightParameters.z + 1.0;
    t16_22 = t10_1.z * t16_4.x;
    t16_22 = t16_22 * 5.0;
    t16_3.xyz = t16_3.xyz * vec3(t16_22);
    t16_6.xyz = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_6.xyz = t10_1.xxx * t16_6.xyz + vec3(1.0, 1.0, 1.0);
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_6.xyz = t16_6.xyz * t10_5.xyz;
    t16_7.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_7.xyz = t10_1.yyy * t16_7.xyz + vec3(1.0, 1.0, 1.0);
    t16_6.xyz = t16_6.xyz * t16_7.xyz;
    t16_3.xyz = t16_3.xyz * t16_6.xyz;
    t16_0.xyz = t16_0.xyz * t16_6.xyz;
    t16_0.xyz = t16_4.xxx * t16_0.xyz;
    t16_0.xyz = t16_4.yyy * t16_0.xyz;
    t10_8 = (-_LightDir.w) + 1.0;
    t16_30 = t16_30 * _LightDir.w + t10_8;
    t16_4.x = t16_30 * 1.5;
    t16_4.x = min(t16_4.x, 1.0);
    t16_13.xyz = vec3(t16_30) + (-_CharacterShadowColor.xyz);
    t16_30 = t16_30 * _LightParameters.y;
    t16_4.xyz = t16_4.xxx * t16_13.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_4.xyz + t16_3.xyz;
    t16_3.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_30) * t16_3.xyz + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_27) + t16_0.xyz;
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
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "SIMPLE_FADE" }
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
uniform sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump float tmpvar_3;
mediump float tmpvar_4;
mediump float tmpvar_5;
mediump vec3 tmpvar_6;
mediump vec3 tmpvar_7;
mediump float tmpvar_8;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_9;
  mediump vec3 lit_color_10;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_11.xyz;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_12.x;
  tmpvar_3 = tmpvar_12.y;
  tmpvar_4 = tmpvar_12.z;
  tmpvar_5 = tmpvar_12.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec3 tangentNormal_13;
  mediump vec3 tmpvar_14;
  tmpvar_14.x = xlv_TEXCOORD3.w;
  tmpvar_14.y = xlv_TEXCOORD4.w;
  tmpvar_14.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_15;
  mediump vec3 tmpvar_16;
  tmpvar_15 = xlv_TEXCOORD5.xyz;
  tmpvar_16 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_17;
  tmpvar_17[0].x = tmpvar_15.x;
  tmpvar_17[0].y = tmpvar_14.x;
  tmpvar_17[0].z = tmpvar_16.x;
  tmpvar_17[1].x = tmpvar_15.y;
  tmpvar_17[1].y = tmpvar_14.y;
  tmpvar_17[1].z = tmpvar_16.y;
  tmpvar_17[2].x = tmpvar_15.z;
  tmpvar_17[2].y = tmpvar_14.z;
  tmpvar_17[2].z = tmpvar_16.z;
  lowp vec3 tmpvar_18;
  tmpvar_18 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_13 = tmpvar_18;
  tmpvar_7 = (tangentNormal_13 * tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19.w = 1.0;
  tmpvar_19.xyz = tmpvar_7;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  x1_21.x = dot (unity_SHAr, tmpvar_19);
  x1_21.y = dot (unity_SHAg, tmpvar_19);
  x1_21.z = dot (unity_SHAb, tmpvar_19);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (tmpvar_7.xyzz * tmpvar_7.yzzx);
  x2_20.x = dot (unity_SHBr, tmpvar_22);
  x2_20.y = dot (unity_SHBg, tmpvar_22);
  x2_20.z = dot (unity_SHBb, tmpvar_22);
  mediump vec3 tmpvar_23;
  tmpvar_23 = max (((1.055 * 
    pow ((((x2_20 + 
      (unity_SHC.xyz * ((tmpvar_7.x * tmpvar_7.x) - (tmpvar_7.y * tmpvar_7.y)))
    ) + x1_21) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  mediump vec3 cube_24;
  mediump vec3 tmpvar_25;
  mediump vec3 I_26;
  I_26 = -(xlv_TEXCOORD3.xyz);
  tmpvar_25 = (I_26 - (2.0 * (
    dot (tmpvar_7, I_26)
   * tmpvar_7)));
  lowp vec4 tmpvar_27;
  tmpvar_27 = textureCube (_HiSpecCube, tmpvar_25);
  cube_24 = tmpvar_27.xyz;
  tmpvar_8 = ((max (
    dot (tmpvar_7, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_10 = (((
    ((tmpvar_23 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_8), vec3(
    min (1.0, (tmpvar_8 * 1.5))
  ))) + ((
    ((((
      (max (dot ((xlv_TEXCOORD3.xyz - 
        (2.0 * (dot (tmpvar_7, xlv_TEXCOORD3.xyz) * tmpvar_7))
      ), -(tmpvar_7)), 0.25) * _Glossiness.w)
     * 
      (10.0 * _MatGlossiness)
    ) * _Glossiness.xyz) * tmpvar_23) * cube_24)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 tmpvar_28;
  tmpvar_28 = mix (lit_color_10, vec3((dot (lit_color_10, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_8 * _LightParameters.y)));
  c_9.xyz = tmpvar_28;
  c_9.w = _RimColor.w;
  lowp vec4 _color_29;
  _color_29 = (c_9 * _FadeColor);
  c_9.w = _color_29.w;
  c_9.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_29.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_9;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "SIMPLE_FADE" }
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec4 t16_4;
mediump vec3 t16_5;
lowp vec3 t10_6;
mediump vec3 t16_7;
mediump float t16_21;
mediump float t16_24;
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
    t16_21 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_21 = t16_21 + t16_21;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_21)) + vs_TEXCOORD3.xyz;
    t16_21 = dot(t16_4.xyz, (-t16_0.xyz));
    t16_21 = max(t16_21, 0.25);
    t16_21 = t16_21 * _Glossiness.w;
    t16_21 = t16_21 * _MatGlossiness;
    t16_21 = t16_21 * 10.0;
    t16_4.xyz = vec3(t16_21) * _Glossiness.xyz;
    t16_4.xyz = t16_3.xyz * t16_4.xyz;
    t16_21 = dot((-vs_TEXCOORD3.xyz), t16_0.xyz);
    t16_21 = t16_21 + t16_21;
    t16_5.xyz = t16_0.xyz * (-vec3(t16_21)) + (-vs_TEXCOORD3.xyz);
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.x = max(t16_0.x, 0.0);
    t10_6.xyz = texture(_HiSpecCube, t16_5.xyz).xyz;
    t16_7.xyz = t16_4.xyz * t10_6.xyz;
    t10_1 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_4.xy = vec2((-t10_1.w) + float(1.0), (-t10_1.z) + float(1.0));
    t16_24 = (-t16_4.x) * _LightParameters.z + 1.0;
    t16_4.x = t10_1.z * t16_24;
    t16_4.x = t16_4.x * 5.0;
    t16_7.xyz = t16_7.xyz * t16_4.xxx;
    t16_4.xzw = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_4.xzw = t10_1.xxx * t16_4.xzw + vec3(1.0, 1.0, 1.0);
    t10_6.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_4.xzw = t16_4.xzw * t10_6.xyz;
    t16_5.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_5.xyz = t10_1.yyy * t16_5.xyz + vec3(1.0, 1.0, 1.0);
    t16_4.xzw = t16_4.xzw * t16_5.xyz;
    t16_7.xyz = t16_7.xyz * t16_4.xzw;
    t16_3.xyz = t16_3.xyz * t16_4.xzw;
    t16_3.xyz = vec3(t16_24) * t16_3.xyz;
    t16_3.xyz = t16_4.yyy * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_24 = t16_0.x * 1.5;
    t16_24 = min(t16_24, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_24) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_7.xyz = t16_3.xyz * t16_4.xyz + t16_7.xyz;
    t16_3.x = dot(t16_7.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_7.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_7.xyz;
    t10_2.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t10_2.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w * _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FANCY_FADE" }
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
uniform sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump float tmpvar_3;
mediump float tmpvar_4;
mediump float tmpvar_5;
mediump vec3 tmpvar_6;
mediump vec3 tmpvar_7;
mediump float tmpvar_8;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_9;
  mediump vec3 lit_color_10;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_11.xyz;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_12.x;
  tmpvar_3 = tmpvar_12.y;
  tmpvar_4 = tmpvar_12.z;
  tmpvar_5 = tmpvar_12.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec3 tangentNormal_13;
  mediump vec3 tmpvar_14;
  tmpvar_14.x = xlv_TEXCOORD3.w;
  tmpvar_14.y = xlv_TEXCOORD4.w;
  tmpvar_14.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_15;
  mediump vec3 tmpvar_16;
  tmpvar_15 = xlv_TEXCOORD5.xyz;
  tmpvar_16 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_17;
  tmpvar_17[0].x = tmpvar_15.x;
  tmpvar_17[0].y = tmpvar_14.x;
  tmpvar_17[0].z = tmpvar_16.x;
  tmpvar_17[1].x = tmpvar_15.y;
  tmpvar_17[1].y = tmpvar_14.y;
  tmpvar_17[1].z = tmpvar_16.y;
  tmpvar_17[2].x = tmpvar_15.z;
  tmpvar_17[2].y = tmpvar_14.z;
  tmpvar_17[2].z = tmpvar_16.z;
  lowp vec3 tmpvar_18;
  tmpvar_18 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_13 = tmpvar_18;
  tmpvar_7 = (tangentNormal_13 * tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19.w = 1.0;
  tmpvar_19.xyz = tmpvar_7;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  x1_21.x = dot (unity_SHAr, tmpvar_19);
  x1_21.y = dot (unity_SHAg, tmpvar_19);
  x1_21.z = dot (unity_SHAb, tmpvar_19);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (tmpvar_7.xyzz * tmpvar_7.yzzx);
  x2_20.x = dot (unity_SHBr, tmpvar_22);
  x2_20.y = dot (unity_SHBg, tmpvar_22);
  x2_20.z = dot (unity_SHBb, tmpvar_22);
  mediump vec3 tmpvar_23;
  tmpvar_23 = max (((1.055 * 
    pow ((((x2_20 + 
      (unity_SHC.xyz * ((tmpvar_7.x * tmpvar_7.x) - (tmpvar_7.y * tmpvar_7.y)))
    ) + x1_21) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  mediump vec3 cube_24;
  mediump vec3 tmpvar_25;
  mediump vec3 I_26;
  I_26 = -(xlv_TEXCOORD3.xyz);
  tmpvar_25 = (I_26 - (2.0 * (
    dot (tmpvar_7, I_26)
   * tmpvar_7)));
  lowp vec4 tmpvar_27;
  tmpvar_27 = textureCube (_HiSpecCube, tmpvar_25);
  cube_24 = tmpvar_27.xyz;
  tmpvar_8 = ((max (
    dot (tmpvar_7, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_10 = (((
    ((tmpvar_23 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_8), vec3(
    min (1.0, (tmpvar_8 * 1.5))
  ))) + ((
    ((((
      (max (dot ((xlv_TEXCOORD3.xyz - 
        (2.0 * (dot (tmpvar_7, xlv_TEXCOORD3.xyz) * tmpvar_7))
      ), -(tmpvar_7)), 0.25) * _Glossiness.w)
     * 
      (10.0 * _MatGlossiness)
    ) * _Glossiness.xyz) * tmpvar_23) * cube_24)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 tmpvar_28;
  tmpvar_28 = mix (lit_color_10, vec3((dot (lit_color_10, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_8 * _LightParameters.y)));
  c_9.xyz = tmpvar_28;
  c_9.w = _RimColor.w;
  lowp vec4 _color_29;
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_31;
  tmpvar_31 = (_FadeColor.w - ((0.9375 * tmpvar_30.y) + 0.03125));
  mediump float tmpvar_32;
  tmpvar_32 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_31)
  )), 0.0, 1.0);
  _color_29.xyz = (_FadeColor.xyz * (tmpvar_32 + c_9.xyz));
  _color_29.w = ((32.0 * tmpvar_31) + 0.9375);
  c_9.w = _color_29.w;
  c_9.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_29.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_9;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FANCY_FADE" }
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
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
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec4 t16_4;
mediump vec3 t16_5;
lowp vec3 t10_6;
mediump vec3 t16_7;
mediump float t16_21;
mediump float t16_24;
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
    t16_21 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_21 = t16_21 + t16_21;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_21)) + vs_TEXCOORD3.xyz;
    t16_21 = dot(t16_4.xyz, (-t16_0.xyz));
    t16_21 = max(t16_21, 0.25);
    t16_21 = t16_21 * _Glossiness.w;
    t16_21 = t16_21 * _MatGlossiness;
    t16_21 = t16_21 * 10.0;
    t16_4.xyz = vec3(t16_21) * _Glossiness.xyz;
    t16_4.xyz = t16_3.xyz * t16_4.xyz;
    t16_21 = dot((-vs_TEXCOORD3.xyz), t16_0.xyz);
    t16_21 = t16_21 + t16_21;
    t16_5.xyz = t16_0.xyz * (-vec3(t16_21)) + (-vs_TEXCOORD3.xyz);
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.x = max(t16_0.x, 0.0);
    t10_6.xyz = texture(_HiSpecCube, t16_5.xyz).xyz;
    t16_7.xyz = t16_4.xyz * t10_6.xyz;
    t10_1 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_4.xy = vec2((-t10_1.w) + float(1.0), (-t10_1.z) + float(1.0));
    t16_24 = (-t16_4.x) * _LightParameters.z + 1.0;
    t16_4.x = t10_1.z * t16_24;
    t16_4.x = t16_4.x * 5.0;
    t16_7.xyz = t16_7.xyz * t16_4.xxx;
    t16_4.xzw = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_4.xzw = t10_1.xxx * t16_4.xzw + vec3(1.0, 1.0, 1.0);
    t10_6.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_4.xzw = t16_4.xzw * t10_6.xyz;
    t16_5.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_5.xyz = t10_1.yyy * t16_5.xyz + vec3(1.0, 1.0, 1.0);
    t16_4.xzw = t16_4.xzw * t16_5.xyz;
    t16_7.xyz = t16_7.xyz * t16_4.xzw;
    t16_3.xyz = t16_3.xyz * t16_4.xzw;
    t16_3.xyz = vec3(t16_24) * t16_3.xyz;
    t16_3.xyz = t16_4.yyy * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_24 = t16_0.x * 1.5;
    t16_24 = min(t16_24, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_24) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_7.xyz = t16_3.xyz * t16_4.xyz + t16_7.xyz;
    t16_3.x = dot(t16_7.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_7.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_7.xyz;
    t10_6.x = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_21 = t10_6.x * 0.9375 + 0.03125;
    t16_21 = (-t16_21) + _FadeColor.w;
    t16_3.x = -abs(t16_21) * 32.0 + 1.0;
    t16_1.w = t16_21 * 32.0 + 0.9375;
    t16_21 = max(t16_3.x, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_21);
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0 = t16_1;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  highp vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  mediump vec2 tmpvar_10;
  mediump vec3 tmpvar_11;
  tmpvar_10.x = 0.0;
  tmpvar_7 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = (tmpvar_7 * 0.5);
  highp vec2 tmpvar_14;
  tmpvar_14.x = tmpvar_13.x;
  tmpvar_14.y = (tmpvar_13.y * _ProjectionParams.x);
  o_12.xy = (tmpvar_14 + tmpvar_13.w);
  o_12.zw = tmpvar_7.zw;
  tmpvar_11 = o_12.xyw;
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_8.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_9.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_9.zw = tmpvar_1.xy;
  highp vec3 tmpvar_15;
  tmpvar_15 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_15;
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _Object2World[0].xyz;
  tmpvar_16[1] = _Object2World[1].xyz;
  tmpvar_16[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_16 * _glesNormal));
  worldNormal_5 = tmpvar_17;
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_18 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_19;
  mediump vec3 tmpvar_20;
  tmpvar_20 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22.xyz = worldViewDir_6;
  tmpvar_22.w = worldBinormal_3.x;
  mediump vec4 tmpvar_23;
  tmpvar_23.xyz = worldNormal_5;
  tmpvar_23.w = worldBinormal_3.y;
  mediump vec4 tmpvar_24;
  tmpvar_24.xyz = worldTangent_4;
  tmpvar_24.w = worldBinormal_3.z;
  highp float tmpvar_25;
  highp vec3 x_26;
  x_26 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_25 = sqrt(dot (x_26, x_26));
  fogpos_2 = tmpvar_25;
  tmpvar_10.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_10.x = (1.0 - tmpvar_10.y);
  gl_Position = tmpvar_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = tmpvar_10;
  xlv_TEXCOORD3 = tmpvar_22;
  xlv_TEXCOORD4 = tmpvar_23;
  xlv_TEXCOORD5 = tmpvar_24;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_11;
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
uniform sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump float tmpvar_3;
mediump float tmpvar_4;
mediump float tmpvar_5;
mediump vec3 tmpvar_6;
mediump vec3 tmpvar_7;
mediump float tmpvar_8;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  lowp vec4 c_9;
  mediump vec3 lit_color_10;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_11.xyz;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_12.x;
  tmpvar_3 = tmpvar_12.y;
  tmpvar_4 = tmpvar_12.z;
  tmpvar_5 = tmpvar_12.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec3 tangentNormal_13;
  mediump vec3 tmpvar_14;
  tmpvar_14.x = xlv_TEXCOORD3.w;
  tmpvar_14.y = xlv_TEXCOORD4.w;
  tmpvar_14.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_15;
  mediump vec3 tmpvar_16;
  tmpvar_15 = xlv_TEXCOORD5.xyz;
  tmpvar_16 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_17;
  tmpvar_17[0].x = tmpvar_15.x;
  tmpvar_17[0].y = tmpvar_14.x;
  tmpvar_17[0].z = tmpvar_16.x;
  tmpvar_17[1].x = tmpvar_15.y;
  tmpvar_17[1].y = tmpvar_14.y;
  tmpvar_17[1].z = tmpvar_16.y;
  tmpvar_17[2].x = tmpvar_15.z;
  tmpvar_17[2].y = tmpvar_14.z;
  tmpvar_17[2].z = tmpvar_16.z;
  lowp vec3 tmpvar_18;
  tmpvar_18 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_13 = tmpvar_18;
  tmpvar_7 = (tangentNormal_13 * tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19.w = 1.0;
  tmpvar_19.xyz = tmpvar_7;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  x1_21.x = dot (unity_SHAr, tmpvar_19);
  x1_21.y = dot (unity_SHAg, tmpvar_19);
  x1_21.z = dot (unity_SHAb, tmpvar_19);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (tmpvar_7.xyzz * tmpvar_7.yzzx);
  x2_20.x = dot (unity_SHBr, tmpvar_22);
  x2_20.y = dot (unity_SHBg, tmpvar_22);
  x2_20.z = dot (unity_SHBb, tmpvar_22);
  mediump vec3 tmpvar_23;
  tmpvar_23 = max (((1.055 * 
    pow ((((x2_20 + 
      (unity_SHC.xyz * ((tmpvar_7.x * tmpvar_7.x) - (tmpvar_7.y * tmpvar_7.y)))
    ) + x1_21) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  mediump vec3 cube_24;
  mediump vec3 tmpvar_25;
  mediump vec3 I_26;
  I_26 = -(xlv_TEXCOORD3.xyz);
  tmpvar_25 = (I_26 - (2.0 * (
    dot (tmpvar_7, I_26)
   * tmpvar_7)));
  lowp vec4 tmpvar_27;
  tmpvar_27 = textureCube (_HiSpecCube, tmpvar_25);
  cube_24 = tmpvar_27.xyz;
  tmpvar_8 = ((max (
    dot (tmpvar_7, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_10 = (((
    ((tmpvar_23 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_8), vec3(
    min (1.0, (tmpvar_8 * 1.5))
  ))) + ((
    ((((
      (max (dot ((xlv_TEXCOORD3.xyz - 
        (2.0 * (dot (tmpvar_7, xlv_TEXCOORD3.xyz) * tmpvar_7))
      ), -(tmpvar_7)), 0.25) * _Glossiness.w)
     * 
      (10.0 * _MatGlossiness)
    ) * _Glossiness.xyz) * tmpvar_23) * cube_24)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 tmpvar_28;
  tmpvar_28 = mix (lit_color_10, vec3((dot (lit_color_10, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_8 * _LightParameters.y)));
  c_9.xyz = tmpvar_28;
  c_9.w = _RimColor.w;
  lowp vec4 _color_29;
  _color_29.w = c_9.w;
  mediump float rTest_30;
  mediump vec2 tmpvar_31;
  tmpvar_31 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_32;
  tmpvar_32 = ((1.5 * _FadeColor.w) - tmpvar_31.y);
  if ((tmpvar_32 < 0.0)) {
    discard;
  };
  mediump float tmpvar_33;
  tmpvar_33 = clamp ((1.0 - (tmpvar_32 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_34;
  tmpvar_34.x = tmpvar_31.x;
  tmpvar_34.y = clamp ((tmpvar_33 - tmpvar_31.y), 0.0, 1.0);
  lowp float tmpvar_35;
  tmpvar_35 = texture2D (_RasterTex, tmpvar_34).z;
  rTest_30 = tmpvar_35;
  mediump float x_36;
  x_36 = (rTest_30 - 0.4);
  if ((x_36 < 0.0)) {
    discard;
  };
  _color_29.xyz = (c_9.xyz + (_FadeColor.xyz * tmpvar_33));
  c_9.w = _color_29.w;
  c_9.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_29.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_9;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "RASTER_FADE" }
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
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
out mediump vec3 vs_TEXCOORD7;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_2;
highp vec4 t3;
mediump vec3 t16_4;
highp vec3 t5;
mediump float t16_8;
highp float t18;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t1.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t1;
    t1.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t1.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t1;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = sqrt(t18);
    t16_2 = (-t18) + acid_Fog.y;
    t16_8 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_8;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_8 = t16_2 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_8;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = inversesqrt(t18);
    t1.xyz = vec3(t18) * t1.xyz;
    t3.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t2.xyz = vec3(t18) * t3.xyz;
    t3.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t3.xyz = vec3(t18) * t3.xyz;
    t16_4.xyz = t2.zxy * t3.yzx;
    t16_4.xyz = t2.yzx * t3.zxy + (-t16_4.xyz);
    t5.xyz = t16_4.xyz * in_TANGENT0.www;
    t18 = dot(t5.xyz, t5.xyz);
    t18 = inversesqrt(t18);
    t5.xyz = vec3(t18) * t5.xyz;
    t1.w = t5.x;
    vs_TEXCOORD3 = t1;
    t2.w = t5.y;
    t3.w = t5.z;
    vs_TEXCOORD5 = t3;
    vs_TEXCOORD4 = t2;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0.w = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t1.zz + t1.xw;
    vs_TEXCOORD7.xyz = t0.xyz;
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
uniform lowp sampler2D _RasterTex;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec3 vs_TEXCOORD7;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
bool tb1;
mediump vec4 t16_2;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp vec3 t10_5;
mediump vec3 t16_6;
mediump vec3 t16_7;
lowp float t10_8;
mediump vec3 t16_13;
mediump float t16_22;
mediump float t16_27;
mediump float t16_30;
void main()
{
    t16_0.xy = vs_TEXCOORD7.xy / vs_TEXCOORD7.zz;
    t16_27 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_27<0.0;
    t16_27 = (-t16_27) * 2.5 + 1.0;
    t16_27 = clamp(t16_27, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_27;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1.x = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1.x + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = vs_TEXCOORD5.y;
    t16_0.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = t16_1.y * t16_1.y;
    t16_0.x = t16_1.x * t16_1.x + (-t16_0.x);
    t16_3.xy = vs_TEXCOORD5.zw;
    t16_3.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_2.xyz, t16_3.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_3.x = dot(unity_SHBr, t16_2);
    t16_3.y = dot(unity_SHBg, t16_2);
    t16_3.z = dot(unity_SHBb, t16_2);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_3.xyz;
    t16_1.w = 1.0;
    t16_3.x = dot(unity_SHAr, t16_1);
    t16_3.y = dot(unity_SHAg, t16_1);
    t16_3.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_3.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t16_3.x = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_3.x = t16_3.x + t16_3.x;
    t16_3.xyz = t16_1.xyz * (-t16_3.xxx) + vs_TEXCOORD3.xyz;
    t16_3.x = dot(t16_3.xyz, (-t16_1.xyz));
    t16_3.x = max(t16_3.x, 0.25);
    t16_3.x = t16_3.x * _Glossiness.w;
    t16_3.x = t16_3.x * _MatGlossiness;
    t16_3.x = t16_3.x * 10.0;
    t16_3.xyz = t16_3.xxx * _Glossiness.xyz;
    t16_3.xyz = t16_0.xyz * t16_3.xyz;
    t16_30 = dot((-vs_TEXCOORD3.xyz), t16_1.xyz);
    t16_30 = t16_30 + t16_30;
    t16_4.xyz = t16_1.xyz * (-vec3(t16_30)) + (-vs_TEXCOORD3.xyz);
    t16_30 = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_30 = max(t16_30, 0.0);
    t10_5.xyz = texture(_HiSpecCube, t16_4.xyz).xyz;
    t16_3.xyz = t16_3.xyz * t10_5.xyz;
    t10_1 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_4.xy = vec2((-t10_1.w) + float(1.0), (-t10_1.z) + float(1.0));
    t16_4.x = (-t16_4.x) * _LightParameters.z + 1.0;
    t16_22 = t10_1.z * t16_4.x;
    t16_22 = t16_22 * 5.0;
    t16_3.xyz = t16_3.xyz * vec3(t16_22);
    t16_6.xyz = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_6.xyz = t10_1.xxx * t16_6.xyz + vec3(1.0, 1.0, 1.0);
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_6.xyz = t16_6.xyz * t10_5.xyz;
    t16_7.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_7.xyz = t10_1.yyy * t16_7.xyz + vec3(1.0, 1.0, 1.0);
    t16_6.xyz = t16_6.xyz * t16_7.xyz;
    t16_3.xyz = t16_3.xyz * t16_6.xyz;
    t16_0.xyz = t16_0.xyz * t16_6.xyz;
    t16_0.xyz = t16_4.xxx * t16_0.xyz;
    t16_0.xyz = t16_4.yyy * t16_0.xyz;
    t10_8 = (-_LightDir.w) + 1.0;
    t16_30 = t16_30 * _LightDir.w + t10_8;
    t16_4.x = t16_30 * 1.5;
    t16_4.x = min(t16_4.x, 1.0);
    t16_13.xyz = vec3(t16_30) + (-_CharacterShadowColor.xyz);
    t16_30 = t16_30 * _LightParameters.y;
    t16_4.xyz = t16_4.xxx * t16_13.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_4.xyz + t16_3.xyz;
    t16_3.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_30) * t16_3.xyz + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_27) + t16_0.xyz;
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
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "SIMPLE_FADE" }
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
uniform sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump float tmpvar_3;
mediump float tmpvar_4;
mediump float tmpvar_5;
mediump vec3 tmpvar_6;
mediump vec3 tmpvar_7;
mediump float tmpvar_8;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_9;
  mediump vec3 lit_color_10;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_11.xyz;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_12.x;
  tmpvar_3 = tmpvar_12.y;
  tmpvar_4 = tmpvar_12.z;
  tmpvar_5 = tmpvar_12.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec3 tangentNormal_13;
  mediump vec3 tmpvar_14;
  tmpvar_14.x = xlv_TEXCOORD3.w;
  tmpvar_14.y = xlv_TEXCOORD4.w;
  tmpvar_14.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_15;
  mediump vec3 tmpvar_16;
  tmpvar_15 = xlv_TEXCOORD5.xyz;
  tmpvar_16 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_17;
  tmpvar_17[0].x = tmpvar_15.x;
  tmpvar_17[0].y = tmpvar_14.x;
  tmpvar_17[0].z = tmpvar_16.x;
  tmpvar_17[1].x = tmpvar_15.y;
  tmpvar_17[1].y = tmpvar_14.y;
  tmpvar_17[1].z = tmpvar_16.y;
  tmpvar_17[2].x = tmpvar_15.z;
  tmpvar_17[2].y = tmpvar_14.z;
  tmpvar_17[2].z = tmpvar_16.z;
  lowp vec3 tmpvar_18;
  tmpvar_18 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_13 = tmpvar_18;
  tmpvar_7 = (tangentNormal_13 * tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19.w = 1.0;
  tmpvar_19.xyz = tmpvar_7;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  x1_21.x = dot (unity_SHAr, tmpvar_19);
  x1_21.y = dot (unity_SHAg, tmpvar_19);
  x1_21.z = dot (unity_SHAb, tmpvar_19);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (tmpvar_7.xyzz * tmpvar_7.yzzx);
  x2_20.x = dot (unity_SHBr, tmpvar_22);
  x2_20.y = dot (unity_SHBg, tmpvar_22);
  x2_20.z = dot (unity_SHBb, tmpvar_22);
  mediump vec3 tmpvar_23;
  tmpvar_23 = max (((1.055 * 
    pow ((((x2_20 + 
      (unity_SHC.xyz * ((tmpvar_7.x * tmpvar_7.x) - (tmpvar_7.y * tmpvar_7.y)))
    ) + x1_21) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  mediump vec3 cube_24;
  mediump vec3 tmpvar_25;
  mediump vec3 I_26;
  I_26 = -(xlv_TEXCOORD3.xyz);
  tmpvar_25 = (I_26 - (2.0 * (
    dot (tmpvar_7, I_26)
   * tmpvar_7)));
  lowp vec4 tmpvar_27;
  tmpvar_27 = textureCube (_HiSpecCube, tmpvar_25);
  cube_24 = tmpvar_27.xyz;
  tmpvar_8 = ((max (
    dot (tmpvar_7, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_10 = (((
    ((tmpvar_23 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_8), vec3(
    min (1.0, (tmpvar_8 * 1.5))
  ))) + ((
    ((((
      (max (dot ((xlv_TEXCOORD3.xyz - 
        (2.0 * (dot (tmpvar_7, xlv_TEXCOORD3.xyz) * tmpvar_7))
      ), -(tmpvar_7)), 0.25) * _Glossiness.w)
     * 
      (10.0 * _MatGlossiness)
    ) * _Glossiness.xyz) * tmpvar_23) * cube_24)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 atten_28;
  lowp float tmpvar_29;
  highp float tmpvar_30;
  tmpvar_30 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_29 = tmpvar_30;
  lowp vec3 tmpvar_31;
  tmpvar_31 = vec3(tmpvar_29);
  atten_28 = tmpvar_31;
  lit_color_10 = (lit_color_10 * max ((_Shadows.x + 
    (atten_28 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_32;
  tmpvar_32 = mix (lit_color_10, vec3((dot (lit_color_10, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_8 * _LightParameters.y)));
  c_9.xyz = tmpvar_32;
  c_9.w = _RimColor.w;
  lowp vec4 _color_33;
  _color_33 = (c_9 * _FadeColor);
  c_9.w = _color_33.w;
  c_9.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_33.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_9;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FANCY_FADE" }
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
uniform sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump float tmpvar_3;
mediump float tmpvar_4;
mediump float tmpvar_5;
mediump vec3 tmpvar_6;
mediump vec3 tmpvar_7;
mediump float tmpvar_8;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_9;
  mediump vec3 lit_color_10;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_11.xyz;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_12.x;
  tmpvar_3 = tmpvar_12.y;
  tmpvar_4 = tmpvar_12.z;
  tmpvar_5 = tmpvar_12.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec3 tangentNormal_13;
  mediump vec3 tmpvar_14;
  tmpvar_14.x = xlv_TEXCOORD3.w;
  tmpvar_14.y = xlv_TEXCOORD4.w;
  tmpvar_14.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_15;
  mediump vec3 tmpvar_16;
  tmpvar_15 = xlv_TEXCOORD5.xyz;
  tmpvar_16 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_17;
  tmpvar_17[0].x = tmpvar_15.x;
  tmpvar_17[0].y = tmpvar_14.x;
  tmpvar_17[0].z = tmpvar_16.x;
  tmpvar_17[1].x = tmpvar_15.y;
  tmpvar_17[1].y = tmpvar_14.y;
  tmpvar_17[1].z = tmpvar_16.y;
  tmpvar_17[2].x = tmpvar_15.z;
  tmpvar_17[2].y = tmpvar_14.z;
  tmpvar_17[2].z = tmpvar_16.z;
  lowp vec3 tmpvar_18;
  tmpvar_18 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_13 = tmpvar_18;
  tmpvar_7 = (tangentNormal_13 * tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19.w = 1.0;
  tmpvar_19.xyz = tmpvar_7;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  x1_21.x = dot (unity_SHAr, tmpvar_19);
  x1_21.y = dot (unity_SHAg, tmpvar_19);
  x1_21.z = dot (unity_SHAb, tmpvar_19);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (tmpvar_7.xyzz * tmpvar_7.yzzx);
  x2_20.x = dot (unity_SHBr, tmpvar_22);
  x2_20.y = dot (unity_SHBg, tmpvar_22);
  x2_20.z = dot (unity_SHBb, tmpvar_22);
  mediump vec3 tmpvar_23;
  tmpvar_23 = max (((1.055 * 
    pow ((((x2_20 + 
      (unity_SHC.xyz * ((tmpvar_7.x * tmpvar_7.x) - (tmpvar_7.y * tmpvar_7.y)))
    ) + x1_21) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  mediump vec3 cube_24;
  mediump vec3 tmpvar_25;
  mediump vec3 I_26;
  I_26 = -(xlv_TEXCOORD3.xyz);
  tmpvar_25 = (I_26 - (2.0 * (
    dot (tmpvar_7, I_26)
   * tmpvar_7)));
  lowp vec4 tmpvar_27;
  tmpvar_27 = textureCube (_HiSpecCube, tmpvar_25);
  cube_24 = tmpvar_27.xyz;
  tmpvar_8 = ((max (
    dot (tmpvar_7, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_10 = (((
    ((tmpvar_23 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_8), vec3(
    min (1.0, (tmpvar_8 * 1.5))
  ))) + ((
    ((((
      (max (dot ((xlv_TEXCOORD3.xyz - 
        (2.0 * (dot (tmpvar_7, xlv_TEXCOORD3.xyz) * tmpvar_7))
      ), -(tmpvar_7)), 0.25) * _Glossiness.w)
     * 
      (10.0 * _MatGlossiness)
    ) * _Glossiness.xyz) * tmpvar_23) * cube_24)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 atten_28;
  lowp float tmpvar_29;
  highp float tmpvar_30;
  tmpvar_30 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_29 = tmpvar_30;
  lowp vec3 tmpvar_31;
  tmpvar_31 = vec3(tmpvar_29);
  atten_28 = tmpvar_31;
  lit_color_10 = (lit_color_10 * max ((_Shadows.x + 
    (atten_28 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_32;
  tmpvar_32 = mix (lit_color_10, vec3((dot (lit_color_10, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_8 * _LightParameters.y)));
  c_9.xyz = tmpvar_32;
  c_9.w = _RimColor.w;
  lowp vec4 _color_33;
  lowp vec4 tmpvar_34;
  tmpvar_34 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_35;
  tmpvar_35 = (_FadeColor.w - ((0.9375 * tmpvar_34.y) + 0.03125));
  mediump float tmpvar_36;
  tmpvar_36 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_35)
  )), 0.0, 1.0);
  _color_33.xyz = (_FadeColor.xyz * (tmpvar_36 + c_9.xyz));
  _color_33.w = ((32.0 * tmpvar_35) + 0.9375);
  c_9.w = _color_33.w;
  c_9.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_33.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_9;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
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
  highp vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  mediump vec2 tmpvar_10;
  mediump vec3 tmpvar_11;
  mediump vec4 tmpvar_12;
  tmpvar_10.x = 0.0;
  tmpvar_7 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_7 * 0.5);
  highp vec2 tmpvar_15;
  tmpvar_15.x = tmpvar_14.x;
  tmpvar_15.y = (tmpvar_14.y * _ProjectionParams.x);
  o_13.xy = (tmpvar_15 + tmpvar_14.w);
  o_13.zw = tmpvar_7.zw;
  tmpvar_11 = o_13.xyw;
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_8.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_9.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_9.zw = tmpvar_1.xy;
  highp vec3 tmpvar_16;
  highp vec4 tmpvar_17;
  tmpvar_17 = (_Object2World * _glesVertex);
  tmpvar_16 = normalize((_WorldSpaceCameraPos - tmpvar_17.xyz));
  worldViewDir_6 = tmpvar_16;
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_18 * _glesNormal));
  worldNormal_5 = tmpvar_19;
  highp mat3 tmpvar_20;
  tmpvar_20[0] = _Object2World[0].xyz;
  tmpvar_20[1] = _Object2World[1].xyz;
  tmpvar_20[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_21;
  mediump vec3 tmpvar_22;
  tmpvar_22 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_23;
  tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_23;
  mediump vec4 tmpvar_24;
  tmpvar_24.xyz = worldViewDir_6;
  tmpvar_24.w = worldBinormal_3.x;
  mediump vec4 tmpvar_25;
  tmpvar_25.xyz = worldNormal_5;
  tmpvar_25.w = worldBinormal_3.y;
  mediump vec4 tmpvar_26;
  tmpvar_26.xyz = worldTangent_4;
  tmpvar_26.w = worldBinormal_3.z;
  tmpvar_12 = (unity_World2Shadow[0] * tmpvar_17);
  highp float tmpvar_27;
  highp vec3 x_28;
  x_28 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_27 = sqrt(dot (x_28, x_28));
  fogpos_2 = tmpvar_27;
  tmpvar_10.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_10.x = (1.0 - tmpvar_10.y);
  gl_Position = tmpvar_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = tmpvar_10;
  xlv_TEXCOORD3 = tmpvar_24;
  xlv_TEXCOORD4 = tmpvar_25;
  xlv_TEXCOORD5 = tmpvar_26;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_11;
  xlv_TEXCOORD8 = tmpvar_12;
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
uniform sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump float tmpvar_3;
mediump float tmpvar_4;
mediump float tmpvar_5;
mediump vec3 tmpvar_6;
mediump vec3 tmpvar_7;
mediump float tmpvar_8;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_9;
  mediump vec3 lit_color_10;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_11.xyz;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_12.x;
  tmpvar_3 = tmpvar_12.y;
  tmpvar_4 = tmpvar_12.z;
  tmpvar_5 = tmpvar_12.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec3 tangentNormal_13;
  mediump vec3 tmpvar_14;
  tmpvar_14.x = xlv_TEXCOORD3.w;
  tmpvar_14.y = xlv_TEXCOORD4.w;
  tmpvar_14.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_15;
  mediump vec3 tmpvar_16;
  tmpvar_15 = xlv_TEXCOORD5.xyz;
  tmpvar_16 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_17;
  tmpvar_17[0].x = tmpvar_15.x;
  tmpvar_17[0].y = tmpvar_14.x;
  tmpvar_17[0].z = tmpvar_16.x;
  tmpvar_17[1].x = tmpvar_15.y;
  tmpvar_17[1].y = tmpvar_14.y;
  tmpvar_17[1].z = tmpvar_16.y;
  tmpvar_17[2].x = tmpvar_15.z;
  tmpvar_17[2].y = tmpvar_14.z;
  tmpvar_17[2].z = tmpvar_16.z;
  lowp vec3 tmpvar_18;
  tmpvar_18 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_13 = tmpvar_18;
  tmpvar_7 = (tangentNormal_13 * tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19.w = 1.0;
  tmpvar_19.xyz = tmpvar_7;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  x1_21.x = dot (unity_SHAr, tmpvar_19);
  x1_21.y = dot (unity_SHAg, tmpvar_19);
  x1_21.z = dot (unity_SHAb, tmpvar_19);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (tmpvar_7.xyzz * tmpvar_7.yzzx);
  x2_20.x = dot (unity_SHBr, tmpvar_22);
  x2_20.y = dot (unity_SHBg, tmpvar_22);
  x2_20.z = dot (unity_SHBb, tmpvar_22);
  mediump vec3 tmpvar_23;
  tmpvar_23 = max (((1.055 * 
    pow ((((x2_20 + 
      (unity_SHC.xyz * ((tmpvar_7.x * tmpvar_7.x) - (tmpvar_7.y * tmpvar_7.y)))
    ) + x1_21) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  mediump vec3 cube_24;
  mediump vec3 tmpvar_25;
  mediump vec3 I_26;
  I_26 = -(xlv_TEXCOORD3.xyz);
  tmpvar_25 = (I_26 - (2.0 * (
    dot (tmpvar_7, I_26)
   * tmpvar_7)));
  lowp vec4 tmpvar_27;
  tmpvar_27 = textureCube (_HiSpecCube, tmpvar_25);
  cube_24 = tmpvar_27.xyz;
  tmpvar_8 = ((max (
    dot (tmpvar_7, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_10 = (((
    ((tmpvar_23 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_8), vec3(
    min (1.0, (tmpvar_8 * 1.5))
  ))) + ((
    ((((
      (max (dot ((xlv_TEXCOORD3.xyz - 
        (2.0 * (dot (tmpvar_7, xlv_TEXCOORD3.xyz) * tmpvar_7))
      ), -(tmpvar_7)), 0.25) * _Glossiness.w)
     * 
      (10.0 * _MatGlossiness)
    ) * _Glossiness.xyz) * tmpvar_23) * cube_24)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 atten_28;
  lowp float tmpvar_29;
  highp float tmpvar_30;
  tmpvar_30 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_29 = tmpvar_30;
  lowp vec3 tmpvar_31;
  tmpvar_31 = vec3(tmpvar_29);
  atten_28 = tmpvar_31;
  lit_color_10 = (lit_color_10 * max ((_Shadows.x + 
    (atten_28 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_32;
  tmpvar_32 = mix (lit_color_10, vec3((dot (lit_color_10, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_8 * _LightParameters.y)));
  c_9.xyz = tmpvar_32;
  c_9.w = _RimColor.w;
  lowp vec4 _color_33;
  _color_33.w = c_9.w;
  mediump float rTest_34;
  mediump vec2 tmpvar_35;
  tmpvar_35 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_36;
  tmpvar_36 = ((1.5 * _FadeColor.w) - tmpvar_35.y);
  if ((tmpvar_36 < 0.0)) {
    discard;
  };
  mediump float tmpvar_37;
  tmpvar_37 = clamp ((1.0 - (tmpvar_36 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_38;
  tmpvar_38.x = tmpvar_35.x;
  tmpvar_38.y = clamp ((tmpvar_37 - tmpvar_35.y), 0.0, 1.0);
  lowp float tmpvar_39;
  tmpvar_39 = texture2D (_RasterTex, tmpvar_38).z;
  rTest_34 = tmpvar_39;
  mediump float x_40;
  x_40 = (rTest_34 - 0.4);
  if ((x_40 < 0.0)) {
    discard;
  };
  _color_33.xyz = (c_9.xyz + (_FadeColor.xyz * tmpvar_37));
  c_9.w = _color_33.w;
  c_9.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_33.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_9;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "SIMPLE_FADE" }
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
uniform sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump float tmpvar_3;
mediump float tmpvar_4;
mediump float tmpvar_5;
mediump vec3 tmpvar_6;
mediump vec3 tmpvar_7;
mediump float tmpvar_8;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_9;
  mediump vec3 lit_color_10;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_11.xyz;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_12.x;
  tmpvar_3 = tmpvar_12.y;
  tmpvar_4 = tmpvar_12.z;
  tmpvar_5 = tmpvar_12.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec3 tangentNormal_13;
  mediump vec3 tmpvar_14;
  tmpvar_14.x = xlv_TEXCOORD3.w;
  tmpvar_14.y = xlv_TEXCOORD4.w;
  tmpvar_14.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_15;
  mediump vec3 tmpvar_16;
  tmpvar_15 = xlv_TEXCOORD5.xyz;
  tmpvar_16 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_17;
  tmpvar_17[0].x = tmpvar_15.x;
  tmpvar_17[0].y = tmpvar_14.x;
  tmpvar_17[0].z = tmpvar_16.x;
  tmpvar_17[1].x = tmpvar_15.y;
  tmpvar_17[1].y = tmpvar_14.y;
  tmpvar_17[1].z = tmpvar_16.y;
  tmpvar_17[2].x = tmpvar_15.z;
  tmpvar_17[2].y = tmpvar_14.z;
  tmpvar_17[2].z = tmpvar_16.z;
  lowp vec3 tmpvar_18;
  tmpvar_18 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_13 = tmpvar_18;
  tmpvar_7 = (tangentNormal_13 * tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19.w = 1.0;
  tmpvar_19.xyz = tmpvar_7;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  x1_21.x = dot (unity_SHAr, tmpvar_19);
  x1_21.y = dot (unity_SHAg, tmpvar_19);
  x1_21.z = dot (unity_SHAb, tmpvar_19);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (tmpvar_7.xyzz * tmpvar_7.yzzx);
  x2_20.x = dot (unity_SHBr, tmpvar_22);
  x2_20.y = dot (unity_SHBg, tmpvar_22);
  x2_20.z = dot (unity_SHBb, tmpvar_22);
  mediump vec3 tmpvar_23;
  tmpvar_23 = max (((1.055 * 
    pow ((((x2_20 + 
      (unity_SHC.xyz * ((tmpvar_7.x * tmpvar_7.x) - (tmpvar_7.y * tmpvar_7.y)))
    ) + x1_21) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  mediump vec3 cube_24;
  mediump vec3 tmpvar_25;
  mediump vec3 I_26;
  I_26 = -(xlv_TEXCOORD3.xyz);
  tmpvar_25 = (I_26 - (2.0 * (
    dot (tmpvar_7, I_26)
   * tmpvar_7)));
  lowp vec4 tmpvar_27;
  tmpvar_27 = textureCube (_HiSpecCube, tmpvar_25);
  cube_24 = tmpvar_27.xyz;
  tmpvar_8 = ((max (
    dot (tmpvar_7, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_10 = (((
    ((tmpvar_23 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_8), vec3(
    min (1.0, (tmpvar_8 * 1.5))
  ))) + ((
    ((((
      (max (dot ((xlv_TEXCOORD3.xyz - 
        (2.0 * (dot (tmpvar_7, xlv_TEXCOORD3.xyz) * tmpvar_7))
      ), -(tmpvar_7)), 0.25) * _Glossiness.w)
     * 
      (10.0 * _MatGlossiness)
    ) * _Glossiness.xyz) * tmpvar_23) * cube_24)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 atten_28;
  lowp float shadow_29;
  shadow_29 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_30;
  tmpvar_30 = vec3(shadow_29);
  atten_28 = tmpvar_30;
  lit_color_10 = (lit_color_10 * max ((_Shadows.x + 
    (atten_28 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_31;
  tmpvar_31 = mix (lit_color_10, vec3((dot (lit_color_10, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_8 * _LightParameters.y)));
  c_9.xyz = tmpvar_31;
  c_9.w = _RimColor.w;
  lowp vec4 _color_32;
  _color_32 = (c_9 * _FadeColor);
  c_9.w = _color_32.w;
  c_9.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_32.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_9;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "SIMPLE_FADE" }
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
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
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec4 t16_4;
mediump vec3 t16_5;
lowp vec3 t10_6;
mediump vec3 t16_7;
mediump float t16_10;
mediump float t16_21;
mediump float t16_24;
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
    t16_21 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_21 = t16_21 + t16_21;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_21)) + vs_TEXCOORD3.xyz;
    t16_21 = dot(t16_4.xyz, (-t16_0.xyz));
    t16_21 = max(t16_21, 0.25);
    t16_21 = t16_21 * _Glossiness.w;
    t16_21 = t16_21 * _MatGlossiness;
    t16_21 = t16_21 * 10.0;
    t16_4.xyz = vec3(t16_21) * _Glossiness.xyz;
    t16_4.xyz = t16_3.xyz * t16_4.xyz;
    t16_21 = dot((-vs_TEXCOORD3.xyz), t16_0.xyz);
    t16_21 = t16_21 + t16_21;
    t16_5.xyz = t16_0.xyz * (-vec3(t16_21)) + (-vs_TEXCOORD3.xyz);
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.x = max(t16_0.x, 0.0);
    t10_6.xyz = texture(_HiSpecCube, t16_5.xyz).xyz;
    t16_7.xyz = t16_4.xyz * t10_6.xyz;
    t10_1 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_4.xy = vec2((-t10_1.w) + float(1.0), (-t10_1.z) + float(1.0));
    t16_24 = (-t16_4.x) * _LightParameters.z + 1.0;
    t16_4.x = t10_1.z * t16_24;
    t16_4.x = t16_4.x * 5.0;
    t16_7.xyz = t16_7.xyz * t16_4.xxx;
    t16_4.xzw = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_4.xzw = t10_1.xxx * t16_4.xzw + vec3(1.0, 1.0, 1.0);
    t10_6.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_4.xzw = t16_4.xzw * t10_6.xyz;
    t16_5.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_5.xyz = t10_1.yyy * t16_5.xyz + vec3(1.0, 1.0, 1.0);
    t16_4.xzw = t16_4.xzw * t16_5.xyz;
    t16_7.xyz = t16_7.xyz * t16_4.xzw;
    t16_3.xyz = t16_3.xyz * t16_4.xzw;
    t16_3.xyz = vec3(t16_24) * t16_3.xyz;
    t16_3.xyz = t16_4.yyy * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_24 = t16_0.x * 1.5;
    t16_24 = min(t16_24, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_24) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_7.xyz = t16_3.xyz * t16_4.xyz + t16_7.xyz;
    vec3 txVec20 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_3.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec20, 0.0);
    t16_10 = (-_LightShadowData.x) + 1.0;
    t16_3.x = t16_3.x * t16_10 + _LightShadowData.x;
    t16_3.x = t16_3.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_3.x = max(t16_3.x, 0.0);
    t16_7.xyz = t16_7.xyz * t16_3.xxx;
    t16_3.x = dot(t16_7.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_7.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_7.xyz;
    t10_2.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t10_2.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w * _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FANCY_FADE" }
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
uniform sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump float tmpvar_3;
mediump float tmpvar_4;
mediump float tmpvar_5;
mediump vec3 tmpvar_6;
mediump vec3 tmpvar_7;
mediump float tmpvar_8;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_9;
  mediump vec3 lit_color_10;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_11.xyz;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_12.x;
  tmpvar_3 = tmpvar_12.y;
  tmpvar_4 = tmpvar_12.z;
  tmpvar_5 = tmpvar_12.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec3 tangentNormal_13;
  mediump vec3 tmpvar_14;
  tmpvar_14.x = xlv_TEXCOORD3.w;
  tmpvar_14.y = xlv_TEXCOORD4.w;
  tmpvar_14.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_15;
  mediump vec3 tmpvar_16;
  tmpvar_15 = xlv_TEXCOORD5.xyz;
  tmpvar_16 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_17;
  tmpvar_17[0].x = tmpvar_15.x;
  tmpvar_17[0].y = tmpvar_14.x;
  tmpvar_17[0].z = tmpvar_16.x;
  tmpvar_17[1].x = tmpvar_15.y;
  tmpvar_17[1].y = tmpvar_14.y;
  tmpvar_17[1].z = tmpvar_16.y;
  tmpvar_17[2].x = tmpvar_15.z;
  tmpvar_17[2].y = tmpvar_14.z;
  tmpvar_17[2].z = tmpvar_16.z;
  lowp vec3 tmpvar_18;
  tmpvar_18 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_13 = tmpvar_18;
  tmpvar_7 = (tangentNormal_13 * tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19.w = 1.0;
  tmpvar_19.xyz = tmpvar_7;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  x1_21.x = dot (unity_SHAr, tmpvar_19);
  x1_21.y = dot (unity_SHAg, tmpvar_19);
  x1_21.z = dot (unity_SHAb, tmpvar_19);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (tmpvar_7.xyzz * tmpvar_7.yzzx);
  x2_20.x = dot (unity_SHBr, tmpvar_22);
  x2_20.y = dot (unity_SHBg, tmpvar_22);
  x2_20.z = dot (unity_SHBb, tmpvar_22);
  mediump vec3 tmpvar_23;
  tmpvar_23 = max (((1.055 * 
    pow ((((x2_20 + 
      (unity_SHC.xyz * ((tmpvar_7.x * tmpvar_7.x) - (tmpvar_7.y * tmpvar_7.y)))
    ) + x1_21) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  mediump vec3 cube_24;
  mediump vec3 tmpvar_25;
  mediump vec3 I_26;
  I_26 = -(xlv_TEXCOORD3.xyz);
  tmpvar_25 = (I_26 - (2.0 * (
    dot (tmpvar_7, I_26)
   * tmpvar_7)));
  lowp vec4 tmpvar_27;
  tmpvar_27 = textureCube (_HiSpecCube, tmpvar_25);
  cube_24 = tmpvar_27.xyz;
  tmpvar_8 = ((max (
    dot (tmpvar_7, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_10 = (((
    ((tmpvar_23 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_8), vec3(
    min (1.0, (tmpvar_8 * 1.5))
  ))) + ((
    ((((
      (max (dot ((xlv_TEXCOORD3.xyz - 
        (2.0 * (dot (tmpvar_7, xlv_TEXCOORD3.xyz) * tmpvar_7))
      ), -(tmpvar_7)), 0.25) * _Glossiness.w)
     * 
      (10.0 * _MatGlossiness)
    ) * _Glossiness.xyz) * tmpvar_23) * cube_24)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 atten_28;
  lowp float shadow_29;
  shadow_29 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_30;
  tmpvar_30 = vec3(shadow_29);
  atten_28 = tmpvar_30;
  lit_color_10 = (lit_color_10 * max ((_Shadows.x + 
    (atten_28 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_31;
  tmpvar_31 = mix (lit_color_10, vec3((dot (lit_color_10, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_8 * _LightParameters.y)));
  c_9.xyz = tmpvar_31;
  c_9.w = _RimColor.w;
  lowp vec4 _color_32;
  lowp vec4 tmpvar_33;
  tmpvar_33 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_34;
  tmpvar_34 = (_FadeColor.w - ((0.9375 * tmpvar_33.y) + 0.03125));
  mediump float tmpvar_35;
  tmpvar_35 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_34)
  )), 0.0, 1.0);
  _color_32.xyz = (_FadeColor.xyz * (tmpvar_35 + c_9.xyz));
  _color_32.w = ((32.0 * tmpvar_34) + 0.9375);
  c_9.w = _color_32.w;
  c_9.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_32.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_9;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FANCY_FADE" }
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
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
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec4 t16_4;
mediump vec3 t16_5;
lowp vec3 t10_6;
mediump vec3 t16_7;
mediump float t16_10;
mediump float t16_21;
mediump float t16_24;
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
    t16_21 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_21 = t16_21 + t16_21;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_21)) + vs_TEXCOORD3.xyz;
    t16_21 = dot(t16_4.xyz, (-t16_0.xyz));
    t16_21 = max(t16_21, 0.25);
    t16_21 = t16_21 * _Glossiness.w;
    t16_21 = t16_21 * _MatGlossiness;
    t16_21 = t16_21 * 10.0;
    t16_4.xyz = vec3(t16_21) * _Glossiness.xyz;
    t16_4.xyz = t16_3.xyz * t16_4.xyz;
    t16_21 = dot((-vs_TEXCOORD3.xyz), t16_0.xyz);
    t16_21 = t16_21 + t16_21;
    t16_5.xyz = t16_0.xyz * (-vec3(t16_21)) + (-vs_TEXCOORD3.xyz);
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.x = max(t16_0.x, 0.0);
    t10_6.xyz = texture(_HiSpecCube, t16_5.xyz).xyz;
    t16_7.xyz = t16_4.xyz * t10_6.xyz;
    t10_1 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_4.xy = vec2((-t10_1.w) + float(1.0), (-t10_1.z) + float(1.0));
    t16_24 = (-t16_4.x) * _LightParameters.z + 1.0;
    t16_4.x = t10_1.z * t16_24;
    t16_4.x = t16_4.x * 5.0;
    t16_7.xyz = t16_7.xyz * t16_4.xxx;
    t16_4.xzw = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_4.xzw = t10_1.xxx * t16_4.xzw + vec3(1.0, 1.0, 1.0);
    t10_6.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_4.xzw = t16_4.xzw * t10_6.xyz;
    t16_5.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_5.xyz = t10_1.yyy * t16_5.xyz + vec3(1.0, 1.0, 1.0);
    t16_4.xzw = t16_4.xzw * t16_5.xyz;
    t16_7.xyz = t16_7.xyz * t16_4.xzw;
    t16_3.xyz = t16_3.xyz * t16_4.xzw;
    t16_3.xyz = vec3(t16_24) * t16_3.xyz;
    t16_3.xyz = t16_4.yyy * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_24 = t16_0.x * 1.5;
    t16_24 = min(t16_24, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_24) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_7.xyz = t16_3.xyz * t16_4.xyz + t16_7.xyz;
    vec3 txVec12 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_3.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec12, 0.0);
    t16_10 = (-_LightShadowData.x) + 1.0;
    t16_3.x = t16_3.x * t16_10 + _LightShadowData.x;
    t16_3.x = t16_3.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_3.x = max(t16_3.x, 0.0);
    t16_7.xyz = t16_7.xyz * t16_3.xxx;
    t16_3.x = dot(t16_7.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_7.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_7.xyz;
    t10_6.x = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_21 = t10_6.x * 0.9375 + 0.03125;
    t16_21 = (-t16_21) + _FadeColor.w;
    t16_3.x = -abs(t16_21) * 32.0 + 1.0;
    t16_1.w = t16_21 * 32.0 + 0.9375;
    t16_21 = max(t16_3.x, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_21);
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0 = t16_1;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
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
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
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
  highp vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  mediump vec2 tmpvar_10;
  mediump vec3 tmpvar_11;
  mediump vec4 tmpvar_12;
  tmpvar_10.x = 0.0;
  tmpvar_7 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_7 * 0.5);
  highp vec2 tmpvar_15;
  tmpvar_15.x = tmpvar_14.x;
  tmpvar_15.y = (tmpvar_14.y * _ProjectionParams.x);
  o_13.xy = (tmpvar_15 + tmpvar_14.w);
  o_13.zw = tmpvar_7.zw;
  tmpvar_11 = o_13.xyw;
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_8.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_9.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_9.zw = tmpvar_1.xy;
  highp vec3 tmpvar_16;
  highp vec4 tmpvar_17;
  tmpvar_17 = (_Object2World * _glesVertex);
  tmpvar_16 = normalize((_WorldSpaceCameraPos - tmpvar_17.xyz));
  worldViewDir_6 = tmpvar_16;
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_18 * _glesNormal));
  worldNormal_5 = tmpvar_19;
  highp mat3 tmpvar_20;
  tmpvar_20[0] = _Object2World[0].xyz;
  tmpvar_20[1] = _Object2World[1].xyz;
  tmpvar_20[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_21;
  mediump vec3 tmpvar_22;
  tmpvar_22 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_23;
  tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_23;
  mediump vec4 tmpvar_24;
  tmpvar_24.xyz = worldViewDir_6;
  tmpvar_24.w = worldBinormal_3.x;
  mediump vec4 tmpvar_25;
  tmpvar_25.xyz = worldNormal_5;
  tmpvar_25.w = worldBinormal_3.y;
  mediump vec4 tmpvar_26;
  tmpvar_26.xyz = worldTangent_4;
  tmpvar_26.w = worldBinormal_3.z;
  tmpvar_12 = (unity_World2Shadow[0] * tmpvar_17);
  highp float tmpvar_27;
  highp vec3 x_28;
  x_28 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_27 = sqrt(dot (x_28, x_28));
  fogpos_2 = tmpvar_27;
  tmpvar_10.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_10.x = (1.0 - tmpvar_10.y);
  gl_Position = tmpvar_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = tmpvar_10;
  xlv_TEXCOORD3 = tmpvar_24;
  xlv_TEXCOORD4 = tmpvar_25;
  xlv_TEXCOORD5 = tmpvar_26;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_11;
  xlv_TEXCOORD8 = tmpvar_12;
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
uniform sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _Glossiness;
uniform mediump float _MatGlossiness;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump float tmpvar_3;
mediump float tmpvar_4;
mediump float tmpvar_5;
mediump vec3 tmpvar_6;
mediump vec3 tmpvar_7;
mediump float tmpvar_8;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_9;
  mediump vec3 lit_color_10;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_11.xyz;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_12.x;
  tmpvar_3 = tmpvar_12.y;
  tmpvar_4 = tmpvar_12.z;
  tmpvar_5 = tmpvar_12.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec3 tangentNormal_13;
  mediump vec3 tmpvar_14;
  tmpvar_14.x = xlv_TEXCOORD3.w;
  tmpvar_14.y = xlv_TEXCOORD4.w;
  tmpvar_14.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_15;
  mediump vec3 tmpvar_16;
  tmpvar_15 = xlv_TEXCOORD5.xyz;
  tmpvar_16 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_17;
  tmpvar_17[0].x = tmpvar_15.x;
  tmpvar_17[0].y = tmpvar_14.x;
  tmpvar_17[0].z = tmpvar_16.x;
  tmpvar_17[1].x = tmpvar_15.y;
  tmpvar_17[1].y = tmpvar_14.y;
  tmpvar_17[1].z = tmpvar_16.y;
  tmpvar_17[2].x = tmpvar_15.z;
  tmpvar_17[2].y = tmpvar_14.z;
  tmpvar_17[2].z = tmpvar_16.z;
  lowp vec3 tmpvar_18;
  tmpvar_18 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_13 = tmpvar_18;
  tmpvar_7 = (tangentNormal_13 * tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19.w = 1.0;
  tmpvar_19.xyz = tmpvar_7;
  mediump vec3 x2_20;
  mediump vec3 x1_21;
  x1_21.x = dot (unity_SHAr, tmpvar_19);
  x1_21.y = dot (unity_SHAg, tmpvar_19);
  x1_21.z = dot (unity_SHAb, tmpvar_19);
  mediump vec4 tmpvar_22;
  tmpvar_22 = (tmpvar_7.xyzz * tmpvar_7.yzzx);
  x2_20.x = dot (unity_SHBr, tmpvar_22);
  x2_20.y = dot (unity_SHBg, tmpvar_22);
  x2_20.z = dot (unity_SHBb, tmpvar_22);
  mediump vec3 tmpvar_23;
  tmpvar_23 = max (((1.055 * 
    pow ((((x2_20 + 
      (unity_SHC.xyz * ((tmpvar_7.x * tmpvar_7.x) - (tmpvar_7.y * tmpvar_7.y)))
    ) + x1_21) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  mediump vec3 cube_24;
  mediump vec3 tmpvar_25;
  mediump vec3 I_26;
  I_26 = -(xlv_TEXCOORD3.xyz);
  tmpvar_25 = (I_26 - (2.0 * (
    dot (tmpvar_7, I_26)
   * tmpvar_7)));
  lowp vec4 tmpvar_27;
  tmpvar_27 = textureCube (_HiSpecCube, tmpvar_25);
  cube_24 = tmpvar_27.xyz;
  tmpvar_8 = ((max (
    dot (tmpvar_7, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_10 = (((
    ((tmpvar_23 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_8), vec3(
    min (1.0, (tmpvar_8 * 1.5))
  ))) + ((
    ((((
      (max (dot ((xlv_TEXCOORD3.xyz - 
        (2.0 * (dot (tmpvar_7, xlv_TEXCOORD3.xyz) * tmpvar_7))
      ), -(tmpvar_7)), 0.25) * _Glossiness.w)
     * 
      (10.0 * _MatGlossiness)
    ) * _Glossiness.xyz) * tmpvar_23) * cube_24)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 atten_28;
  lowp float shadow_29;
  shadow_29 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_30;
  tmpvar_30 = vec3(shadow_29);
  atten_28 = tmpvar_30;
  lit_color_10 = (lit_color_10 * max ((_Shadows.x + 
    (atten_28 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_31;
  tmpvar_31 = mix (lit_color_10, vec3((dot (lit_color_10, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_8 * _LightParameters.y)));
  c_9.xyz = tmpvar_31;
  c_9.w = _RimColor.w;
  lowp vec4 _color_32;
  _color_32.w = c_9.w;
  mediump float rTest_33;
  mediump vec2 tmpvar_34;
  tmpvar_34 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_35;
  tmpvar_35 = ((1.5 * _FadeColor.w) - tmpvar_34.y);
  if ((tmpvar_35 < 0.0)) {
    discard;
  };
  mediump float tmpvar_36;
  tmpvar_36 = clamp ((1.0 - (tmpvar_35 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_37;
  tmpvar_37.x = tmpvar_34.x;
  tmpvar_37.y = clamp ((tmpvar_36 - tmpvar_34.y), 0.0, 1.0);
  lowp float tmpvar_38;
  tmpvar_38 = texture2D (_RasterTex, tmpvar_37).z;
  rTest_33 = tmpvar_38;
  mediump float x_39;
  x_39 = (rTest_33 - 0.4);
  if ((x_39 < 0.0)) {
    discard;
  };
  _color_32.xyz = (c_9.xyz + (_FadeColor.xyz * tmpvar_36));
  c_9.w = _color_32.w;
  c_9.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_32.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_9;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "RASTER_FADE" }
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
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
out mediump vec3 vs_TEXCOORD7;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_2;
highp vec4 t3;
mediump vec3 t16_4;
highp vec3 t5;
mediump float t16_8;
highp float t18;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t1.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t1;
    t1.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t1.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t1;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = sqrt(t18);
    t16_2 = (-t18) + acid_Fog.y;
    t16_8 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_8;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_8 = t16_2 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_8;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = inversesqrt(t18);
    t1.xyz = vec3(t18) * t1.xyz;
    t3.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t2.xyz = vec3(t18) * t3.xyz;
    t3.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t3.xyz = vec3(t18) * t3.xyz;
    t16_4.xyz = t2.zxy * t3.yzx;
    t16_4.xyz = t2.yzx * t3.zxy + (-t16_4.xyz);
    t5.xyz = t16_4.xyz * in_TANGENT0.www;
    t18 = dot(t5.xyz, t5.xyz);
    t18 = inversesqrt(t18);
    t5.xyz = vec3(t18) * t5.xyz;
    t1.w = t5.x;
    vs_TEXCOORD3 = t1;
    t2.w = t5.y;
    t3.w = t5.z;
    vs_TEXCOORD5 = t3;
    vs_TEXCOORD4 = t2;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0.w = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t1.zz + t1.xw;
    vs_TEXCOORD7.xyz = t0.xyz;
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
uniform 	mediump vec4 _Glossiness;
uniform 	mediump float _MatGlossiness;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _MaskTex;
uniform lowp samplerCube _HiSpecCube;
uniform lowp sampler2D _RasterTex;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec3 vs_TEXCOORD7;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
bool tb1;
mediump vec4 t16_2;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp vec3 t10_5;
mediump vec3 t16_6;
mediump vec3 t16_7;
lowp float t10_8;
mediump float t16_12;
mediump vec3 t16_13;
mediump float t16_22;
mediump float t16_27;
mediump float t16_30;
void main()
{
    t16_0.xy = vs_TEXCOORD7.xy / vs_TEXCOORD7.zz;
    t16_27 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_27<0.0;
    t16_27 = (-t16_27) * 2.5 + 1.0;
    t16_27 = clamp(t16_27, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_27;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1.x = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1.x + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = vs_TEXCOORD5.y;
    t16_0.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = t16_1.y * t16_1.y;
    t16_0.x = t16_1.x * t16_1.x + (-t16_0.x);
    t16_3.xy = vs_TEXCOORD5.zw;
    t16_3.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_2.xyz, t16_3.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_3.x = dot(unity_SHBr, t16_2);
    t16_3.y = dot(unity_SHBg, t16_2);
    t16_3.z = dot(unity_SHBb, t16_2);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_3.xyz;
    t16_1.w = 1.0;
    t16_3.x = dot(unity_SHAr, t16_1);
    t16_3.y = dot(unity_SHAg, t16_1);
    t16_3.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_3.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t16_3.x = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_3.x = t16_3.x + t16_3.x;
    t16_3.xyz = t16_1.xyz * (-t16_3.xxx) + vs_TEXCOORD3.xyz;
    t16_3.x = dot(t16_3.xyz, (-t16_1.xyz));
    t16_3.x = max(t16_3.x, 0.25);
    t16_3.x = t16_3.x * _Glossiness.w;
    t16_3.x = t16_3.x * _MatGlossiness;
    t16_3.x = t16_3.x * 10.0;
    t16_3.xyz = t16_3.xxx * _Glossiness.xyz;
    t16_3.xyz = t16_0.xyz * t16_3.xyz;
    t16_30 = dot((-vs_TEXCOORD3.xyz), t16_1.xyz);
    t16_30 = t16_30 + t16_30;
    t16_4.xyz = t16_1.xyz * (-vec3(t16_30)) + (-vs_TEXCOORD3.xyz);
    t16_30 = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_30 = max(t16_30, 0.0);
    t10_5.xyz = texture(_HiSpecCube, t16_4.xyz).xyz;
    t16_3.xyz = t16_3.xyz * t10_5.xyz;
    t10_1 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_4.xy = vec2((-t10_1.w) + float(1.0), (-t10_1.z) + float(1.0));
    t16_4.x = (-t16_4.x) * _LightParameters.z + 1.0;
    t16_22 = t10_1.z * t16_4.x;
    t16_22 = t16_22 * 5.0;
    t16_3.xyz = t16_3.xyz * vec3(t16_22);
    t16_6.xyz = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_6.xyz = t10_1.xxx * t16_6.xyz + vec3(1.0, 1.0, 1.0);
    t10_5.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_6.xyz = t16_6.xyz * t10_5.xyz;
    t16_7.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_7.xyz = t10_1.yyy * t16_7.xyz + vec3(1.0, 1.0, 1.0);
    t16_6.xyz = t16_6.xyz * t16_7.xyz;
    t16_3.xyz = t16_3.xyz * t16_6.xyz;
    t16_0.xyz = t16_0.xyz * t16_6.xyz;
    t16_0.xyz = t16_4.xxx * t16_0.xyz;
    t16_0.xyz = t16_4.yyy * t16_0.xyz;
    t10_8 = (-_LightDir.w) + 1.0;
    t16_30 = t16_30 * _LightDir.w + t10_8;
    t16_4.x = t16_30 * 1.5;
    t16_4.x = min(t16_4.x, 1.0);
    t16_13.xyz = vec3(t16_30) + (-_CharacterShadowColor.xyz);
    t16_30 = t16_30 * _LightParameters.y;
    t16_4.xyz = t16_4.xxx * t16_13.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_4.xyz + t16_3.xyz;
    vec3 txVec21 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_3.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec21, 0.0);
    t16_12 = (-_LightShadowData.x) + 1.0;
    t16_3.x = t16_3.x * t16_12 + _LightShadowData.x;
    t16_3.x = t16_3.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_3.x = max(t16_3.x, 0.0);
    t16_0.xyz = t16_0.xyz * t16_3.xxx;
    t16_3.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_30) * t16_3.xyz + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_27) + t16_0.xyz;
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
Keywords { "SHADOWS_OFF" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "RASTER_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "RASTER_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "RASTER_FADE" }
"!!GLES3"
}
}
 }
}
Fallback "Hidden/Shadow/Opaque"
}
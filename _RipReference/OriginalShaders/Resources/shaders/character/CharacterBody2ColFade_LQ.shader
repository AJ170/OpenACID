Shader "Character/CharacterBody2ColFade_LQ" {
Properties {
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
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
  GpuProgramID 15209
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
  GpuProgramID 125454
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SIMPLE_FADE" }
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
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  mediump vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  mediump vec2 tmpvar_7;
  tmpvar_6.xy = vec2(0.0, 0.0);
  tmpvar_7.x = 0.0;
  tmpvar_5.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_5.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_6.zw = tmpvar_1.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_4 = tmpvar_8;
  highp mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * _glesNormal));
  worldNormal_3 = tmpvar_10;
  mediump vec4 tmpvar_11;
  tmpvar_11.xyz = worldViewDir_4;
  tmpvar_11.w = 0.0;
  mediump vec4 tmpvar_12;
  tmpvar_12.xyz = worldNormal_3;
  tmpvar_12.w = 0.0;
  highp float tmpvar_13;
  highp vec3 x_14;
  x_14 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_13 = sqrt(dot (x_14, x_14));
  fogpos_2 = tmpvar_13;
  tmpvar_7.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_7.x = (1.0 - tmpvar_7.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_5;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = tmpvar_11;
  xlv_TEXCOORD4 = tmpvar_12;
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
uniform sampler2D _MaskTex;
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
mediump float tmpvar_7;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_8;
  mediump vec3 lit_color_9;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_10.xyz;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_11.x;
  tmpvar_3 = tmpvar_11.y;
  tmpvar_4 = tmpvar_11.z;
  tmpvar_5 = tmpvar_11.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = xlv_TEXCOORD4.xyz;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  x1_14.x = dot (unity_SHAr, tmpvar_12);
  x1_14.y = dot (unity_SHAg, tmpvar_12);
  x1_14.z = dot (unity_SHAb, tmpvar_12);
  mediump vec4 tmpvar_15;
  tmpvar_15 = (xlv_TEXCOORD4.xyzz * xlv_TEXCOORD4.yzzx);
  x2_13.x = dot (unity_SHBr, tmpvar_15);
  x2_13.y = dot (unity_SHBg, tmpvar_15);
  x2_13.z = dot (unity_SHBb, tmpvar_15);
  mediump vec3 tmpvar_16;
  tmpvar_16 = max (((1.055 * 
    pow ((((x2_13 + 
      (unity_SHC.xyz * ((xlv_TEXCOORD4.x * xlv_TEXCOORD4.x) - (xlv_TEXCOORD4.y * xlv_TEXCOORD4.y)))
    ) + x1_14) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_7 = ((max (
    dot (xlv_TEXCOORD4.xyz, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_9 = (((
    ((tmpvar_16 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_7), vec3(
    min (1.0, (tmpvar_7 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (xlv_TEXCOORD4.xyz, xlv_TEXCOORD3.xyz) * xlv_TEXCOORD4.xyz)
      )), -(xlv_TEXCOORD4.xyz)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_16)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 tmpvar_17;
  tmpvar_17 = mix (lit_color_9, vec3((dot (lit_color_9, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_7 * _LightParameters.y)));
  c_8.xyz = tmpvar_17;
  c_8.w = _RimColor.w;
  lowp vec4 _color_18;
  _color_18 = (c_8 * _FadeColor);
  c_8.w = _color_18.w;
  c_8.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_18.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_8;
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
	lowp vec4 albedoTex;
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
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD6;
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
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    vs_TEXCOORD1.xy = vec2(0.0, 0.0);
    vs_TEXCOORD1.zw = in_TEXCOORD0.xy;
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
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_3;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
    vs_TEXCOORD3.w = 0.0;
    t0.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t0.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD4.xyz = t0.xyz;
    vs_TEXCOORD4.w = 0.0;
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
	lowp vec4 albedoTex;
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
uniform lowp sampler2D _MaskTex;
in mediump vec4 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
mediump vec3 t16_2;
lowp vec4 t10_2;
mediump vec4 t16_3;
lowp vec3 t10_4;
mediump vec3 t16_5;
lowp vec3 t10_6;
mediump float t16_21;
mediump float t16_22;
void main()
{
    t16_0.x = vs_TEXCOORD4.y * vs_TEXCOORD4.y;
    t16_0.x = vs_TEXCOORD4.x * vs_TEXCOORD4.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD4.yzzx * vs_TEXCOORD4.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD4.xyz;
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
    t16_21 = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD4.xyz);
    t16_21 = t16_21 + t16_21;
    t16_1.xyz = vs_TEXCOORD4.xyz * (-vec3(t16_21)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_1.xyz, (-vs_TEXCOORD4.xyz));
    t16_0 = max(t16_0, vec4(0.0, 0.0, 0.0, 0.25));
    t16_21 = t16_0.w * _Glossiness.w;
    t16_21 = t16_21 * _MatGlossiness;
    t16_21 = t16_21 * 10.0;
    t16_1.xyz = vec3(t16_21) * _Glossiness.xyz;
    t16_1.xyz = t16_0.xyz * t16_1.xyz;
    t10_2 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_3.xy = vec2((-t10_2.w) + float(1.0), (-t10_2.z) + float(1.0));
    t16_21 = (-t16_3.x) * _LightParameters.z + 1.0;
    t16_22 = t16_21 * t10_2.z;
    t16_22 = t16_22 * 5.0;
    t16_1.xyz = vec3(t16_22) * t16_1.xyz;
    t16_3.xzw = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_3.xzw = t10_2.xxx * t16_3.xzw + vec3(1.0, 1.0, 1.0);
    t10_4.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_3.xzw = t16_3.xzw * t10_4.xyz;
    t16_5.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_5.xyz = t10_2.yyy * t16_5.xyz + vec3(1.0, 1.0, 1.0);
    t16_3.xzw = t16_3.xzw * t16_5.xyz;
    t16_1.xyz = t16_1.xyz * t16_3.xzw;
    t16_0.xyz = t16_0.xyz * t16_3.xzw;
    t16_0.xyz = vec3(t16_21) * t16_0.xyz;
    t16_0.xyz = t16_3.yyy * t16_0.xyz;
    t16_21 = dot(vs_TEXCOORD4.xyz, (-_LightDir.xyz));
    t16_21 = max(t16_21, 0.0);
    t10_6.x = (-_LightDir.w) + 1.0;
    t16_21 = t16_21 * _LightDir.w + t10_6.x;
    t16_22 = t16_21 * 1.5;
    t16_22 = min(t16_22, 1.0);
    t16_3.xyz = vec3(t16_21) + (-_CharacterShadowColor.xyz);
    t16_21 = t16_21 * _LightParameters.y;
    t16_3.xyz = vec3(t16_22) * t16_3.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_3.xyz + t16_1.xyz;
    t16_1.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = t16_1.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_21) * t16_1.xyz + t16_0.xyz;
    t10_6.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t10_6.xyz * vs_TEXCOORD2.xxx;
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
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  mediump vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  mediump vec2 tmpvar_7;
  tmpvar_6.xy = vec2(0.0, 0.0);
  tmpvar_7.x = 0.0;
  tmpvar_5.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_5.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_6.zw = tmpvar_1.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_4 = tmpvar_8;
  highp mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * _glesNormal));
  worldNormal_3 = tmpvar_10;
  mediump vec4 tmpvar_11;
  tmpvar_11.xyz = worldViewDir_4;
  tmpvar_11.w = 0.0;
  mediump vec4 tmpvar_12;
  tmpvar_12.xyz = worldNormal_3;
  tmpvar_12.w = 0.0;
  highp float tmpvar_13;
  highp vec3 x_14;
  x_14 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_13 = sqrt(dot (x_14, x_14));
  fogpos_2 = tmpvar_13;
  tmpvar_7.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_7.x = (1.0 - tmpvar_7.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_5;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = tmpvar_11;
  xlv_TEXCOORD4 = tmpvar_12;
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
uniform sampler2D _MaskTex;
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
mediump float tmpvar_7;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_8;
  mediump vec3 lit_color_9;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_10.xyz;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_11.x;
  tmpvar_3 = tmpvar_11.y;
  tmpvar_4 = tmpvar_11.z;
  tmpvar_5 = tmpvar_11.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = xlv_TEXCOORD4.xyz;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  x1_14.x = dot (unity_SHAr, tmpvar_12);
  x1_14.y = dot (unity_SHAg, tmpvar_12);
  x1_14.z = dot (unity_SHAb, tmpvar_12);
  mediump vec4 tmpvar_15;
  tmpvar_15 = (xlv_TEXCOORD4.xyzz * xlv_TEXCOORD4.yzzx);
  x2_13.x = dot (unity_SHBr, tmpvar_15);
  x2_13.y = dot (unity_SHBg, tmpvar_15);
  x2_13.z = dot (unity_SHBb, tmpvar_15);
  mediump vec3 tmpvar_16;
  tmpvar_16 = max (((1.055 * 
    pow ((((x2_13 + 
      (unity_SHC.xyz * ((xlv_TEXCOORD4.x * xlv_TEXCOORD4.x) - (xlv_TEXCOORD4.y * xlv_TEXCOORD4.y)))
    ) + x1_14) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_7 = ((max (
    dot (xlv_TEXCOORD4.xyz, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_9 = (((
    ((tmpvar_16 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_7), vec3(
    min (1.0, (tmpvar_7 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (xlv_TEXCOORD4.xyz, xlv_TEXCOORD3.xyz) * xlv_TEXCOORD4.xyz)
      )), -(xlv_TEXCOORD4.xyz)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_16)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 tmpvar_17;
  tmpvar_17 = mix (lit_color_9, vec3((dot (lit_color_9, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_7 * _LightParameters.y)));
  c_8.xyz = tmpvar_17;
  c_8.w = _RimColor.w;
  lowp vec4 _color_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_20;
  tmpvar_20 = (_FadeColor.w - ((0.9375 * tmpvar_19.y) + 0.03125));
  mediump float tmpvar_21;
  tmpvar_21 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_20)
  )), 0.0, 1.0);
  _color_18.xyz = (_FadeColor.xyz * (tmpvar_21 + c_8.xyz));
  _color_18.w = ((32.0 * tmpvar_20) + 0.9375);
  c_8.w = _color_18.w;
  c_8.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_18.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_8;
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
	lowp vec4 albedoTex;
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
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD6;
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
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    vs_TEXCOORD1.xy = vec2(0.0, 0.0);
    vs_TEXCOORD1.zw = in_TEXCOORD0.xy;
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
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_3;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
    vs_TEXCOORD3.w = 0.0;
    t0.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t0.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD4.xyz = t0.xyz;
    vs_TEXCOORD4.w = 0.0;
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
	lowp vec4 albedoTex;
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
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _NoiseTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
mediump vec4 t16_2;
lowp vec4 t10_2;
mediump vec4 t16_3;
lowp vec3 t10_4;
mediump vec3 t16_5;
lowp float t10_6;
mediump float t16_21;
mediump float t16_22;
void main()
{
    t16_0.x = vs_TEXCOORD4.y * vs_TEXCOORD4.y;
    t16_0.x = vs_TEXCOORD4.x * vs_TEXCOORD4.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD4.yzzx * vs_TEXCOORD4.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD4.xyz;
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
    t16_21 = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD4.xyz);
    t16_21 = t16_21 + t16_21;
    t16_1.xyz = vs_TEXCOORD4.xyz * (-vec3(t16_21)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_1.xyz, (-vs_TEXCOORD4.xyz));
    t16_0 = max(t16_0, vec4(0.0, 0.0, 0.0, 0.25));
    t16_21 = t16_0.w * _Glossiness.w;
    t16_21 = t16_21 * _MatGlossiness;
    t16_21 = t16_21 * 10.0;
    t16_1.xyz = vec3(t16_21) * _Glossiness.xyz;
    t16_1.xyz = t16_0.xyz * t16_1.xyz;
    t10_2 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_3.xy = vec2((-t10_2.w) + float(1.0), (-t10_2.z) + float(1.0));
    t16_21 = (-t16_3.x) * _LightParameters.z + 1.0;
    t16_22 = t16_21 * t10_2.z;
    t16_22 = t16_22 * 5.0;
    t16_1.xyz = vec3(t16_22) * t16_1.xyz;
    t16_3.xzw = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_3.xzw = t10_2.xxx * t16_3.xzw + vec3(1.0, 1.0, 1.0);
    t10_4.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_3.xzw = t16_3.xzw * t10_4.xyz;
    t16_5.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_5.xyz = t10_2.yyy * t16_5.xyz + vec3(1.0, 1.0, 1.0);
    t16_3.xzw = t16_3.xzw * t16_5.xyz;
    t16_1.xyz = t16_1.xyz * t16_3.xzw;
    t16_0.xyz = t16_0.xyz * t16_3.xzw;
    t16_0.xyz = vec3(t16_21) * t16_0.xyz;
    t16_0.xyz = t16_3.yyy * t16_0.xyz;
    t16_21 = dot(vs_TEXCOORD4.xyz, (-_LightDir.xyz));
    t16_21 = max(t16_21, 0.0);
    t10_6 = (-_LightDir.w) + 1.0;
    t16_21 = t16_21 * _LightDir.w + t10_6;
    t16_22 = t16_21 * 1.5;
    t16_22 = min(t16_22, 1.0);
    t16_3.xyz = vec3(t16_21) + (-_CharacterShadowColor.xyz);
    t16_21 = t16_21 * _LightParameters.y;
    t16_3.xyz = vec3(t16_22) * t16_3.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_3.xyz + t16_1.xyz;
    t16_1.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = t16_1.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_21) * t16_1.xyz + t16_0.xyz;
    t10_4.x = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_21 = t10_4.x * 0.9375 + 0.03125;
    t16_21 = (-t16_21) + _FadeColor.w;
    t16_1.x = -abs(t16_21) * 32.0 + 1.0;
    t16_2.w = t16_21 * 32.0 + 0.9375;
    t16_21 = max(t16_1.x, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_21);
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_2.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0 = t16_2;
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
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  highp vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  mediump vec4 tmpvar_7;
  mediump vec2 tmpvar_8;
  mediump vec3 tmpvar_9;
  tmpvar_7.xy = vec2(0.0, 0.0);
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
  tmpvar_6.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_6.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_7.zw = tmpvar_1.xy;
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
  mediump vec4 tmpvar_16;
  tmpvar_16.xyz = worldViewDir_4;
  tmpvar_16.w = 0.0;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldNormal_3;
  tmpvar_17.w = 0.0;
  highp float tmpvar_18;
  highp vec3 x_19;
  x_19 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_18 = sqrt(dot (x_19, x_19));
  fogpos_2 = tmpvar_18;
  tmpvar_8.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_8.x = (1.0 - tmpvar_8.y);
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = tmpvar_6;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = tmpvar_8;
  xlv_TEXCOORD3 = tmpvar_16;
  xlv_TEXCOORD4 = tmpvar_17;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_9;
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
uniform sampler2D _MaskTex;
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
mediump float tmpvar_7;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  lowp vec4 c_8;
  mediump vec3 lit_color_9;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_10.xyz;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_11.x;
  tmpvar_3 = tmpvar_11.y;
  tmpvar_4 = tmpvar_11.z;
  tmpvar_5 = tmpvar_11.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = xlv_TEXCOORD4.xyz;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  x1_14.x = dot (unity_SHAr, tmpvar_12);
  x1_14.y = dot (unity_SHAg, tmpvar_12);
  x1_14.z = dot (unity_SHAb, tmpvar_12);
  mediump vec4 tmpvar_15;
  tmpvar_15 = (xlv_TEXCOORD4.xyzz * xlv_TEXCOORD4.yzzx);
  x2_13.x = dot (unity_SHBr, tmpvar_15);
  x2_13.y = dot (unity_SHBg, tmpvar_15);
  x2_13.z = dot (unity_SHBb, tmpvar_15);
  mediump vec3 tmpvar_16;
  tmpvar_16 = max (((1.055 * 
    pow ((((x2_13 + 
      (unity_SHC.xyz * ((xlv_TEXCOORD4.x * xlv_TEXCOORD4.x) - (xlv_TEXCOORD4.y * xlv_TEXCOORD4.y)))
    ) + x1_14) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_7 = ((max (
    dot (xlv_TEXCOORD4.xyz, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_9 = (((
    ((tmpvar_16 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_7), vec3(
    min (1.0, (tmpvar_7 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (xlv_TEXCOORD4.xyz, xlv_TEXCOORD3.xyz) * xlv_TEXCOORD4.xyz)
      )), -(xlv_TEXCOORD4.xyz)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_16)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 tmpvar_17;
  tmpvar_17 = mix (lit_color_9, vec3((dot (lit_color_9, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_7 * _LightParameters.y)));
  c_8.xyz = tmpvar_17;
  c_8.w = _RimColor.w;
  lowp vec4 _color_18;
  _color_18.w = c_8.w;
  mediump float rTest_19;
  mediump vec2 tmpvar_20;
  tmpvar_20 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_21;
  tmpvar_21 = ((1.5 * _FadeColor.w) - tmpvar_20.y);
  if ((tmpvar_21 < 0.0)) {
    discard;
  };
  mediump float tmpvar_22;
  tmpvar_22 = clamp ((1.0 - (tmpvar_21 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_23;
  tmpvar_23.x = tmpvar_20.x;
  tmpvar_23.y = clamp ((tmpvar_22 - tmpvar_20.y), 0.0, 1.0);
  lowp float tmpvar_24;
  tmpvar_24 = texture2D (_RasterTex, tmpvar_23).z;
  rTest_19 = tmpvar_24;
  mediump float x_25;
  x_25 = (rTest_19 - 0.4);
  if ((x_25 < 0.0)) {
    discard;
  };
  _color_18.xyz = (c_8.xyz + (_FadeColor.xyz * tmpvar_22));
  c_8.w = _color_18.w;
  c_8.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_18.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_8;
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
	lowp vec4 albedoTex;
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
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD7;
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
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t1.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t1;
    vs_TEXCOORD1.xy = vec2(0.0, 0.0);
    vs_TEXCOORD1.zw = in_TEXCOORD0.xy;
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
    vs_TEXCOORD2.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_5;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * t1.xyz;
    vs_TEXCOORD3.xyz = t1.xyz;
    vs_TEXCOORD3.w = 0.0;
    t1.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t1.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * t1.xyz;
    vs_TEXCOORD4.xyz = t1.xyz;
    vs_TEXCOORD4.w = 0.0;
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
	lowp vec4 albedoTex;
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
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _RasterTex;
in mediump vec4 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD7;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
bool tb1;
mediump vec3 t16_2;
mediump vec4 t16_3;
lowp vec3 t10_4;
mediump vec3 t16_5;
lowp float t10_6;
mediump vec3 t16_10;
mediump float t16_21;
mediump float t16_23;
void main()
{
    t16_0.xy = vs_TEXCOORD7.xy / vs_TEXCOORD7.zz;
    t16_21 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_21<0.0;
    t16_21 = (-t16_21) * 2.5 + 1.0;
    t16_21 = clamp(t16_21, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_21;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1.x = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1.x + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD4.y * vs_TEXCOORD4.y;
    t16_0.x = vs_TEXCOORD4.x * vs_TEXCOORD4.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD4.yzzx * vs_TEXCOORD4.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD4.xyz;
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
    t16_2.x = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD4.xyz);
    t16_2.x = t16_2.x + t16_2.x;
    t16_2.xyz = vs_TEXCOORD4.xyz * (-t16_2.xxx) + vs_TEXCOORD3.xyz;
    t16_2.x = dot(t16_2.xyz, (-vs_TEXCOORD4.xyz));
    t16_2.x = max(t16_2.x, 0.25);
    t16_2.x = t16_2.x * _Glossiness.w;
    t16_2.x = t16_2.x * _MatGlossiness;
    t16_2.x = t16_2.x * 10.0;
    t16_2.xyz = t16_2.xxx * _Glossiness.xyz;
    t16_2.xyz = t16_0.xyz * t16_2.xyz;
    t10_1 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_3.xy = vec2((-t10_1.w) + float(1.0), (-t10_1.z) + float(1.0));
    t16_23 = (-t16_3.x) * _LightParameters.z + 1.0;
    t16_3.x = t10_1.z * t16_23;
    t16_3.x = t16_3.x * 5.0;
    t16_2.xyz = t16_2.xyz * t16_3.xxx;
    t16_3.xzw = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_3.xzw = t10_1.xxx * t16_3.xzw + vec3(1.0, 1.0, 1.0);
    t10_4.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_3.xzw = t16_3.xzw * t10_4.xyz;
    t16_5.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_5.xyz = t10_1.yyy * t16_5.xyz + vec3(1.0, 1.0, 1.0);
    t16_3.xzw = t16_3.xzw * t16_5.xyz;
    t16_2.xyz = t16_2.xyz * t16_3.xzw;
    t16_0.xyz = t16_0.xyz * t16_3.xzw;
    t16_0.xyz = vec3(t16_23) * t16_0.xyz;
    t16_0.xyz = t16_3.yyy * t16_0.xyz;
    t16_23 = dot(vs_TEXCOORD4.xyz, (-_LightDir.xyz));
    t16_23 = max(t16_23, 0.0);
    t10_6 = (-_LightDir.w) + 1.0;
    t16_23 = t16_23 * _LightDir.w + t10_6;
    t16_3.x = t16_23 * 1.5;
    t16_3.x = min(t16_3.x, 1.0);
    t16_10.xyz = vec3(t16_23) + (-_CharacterShadowColor.xyz);
    t16_23 = t16_23 * _LightParameters.y;
    t16_3.xyz = t16_3.xxx * t16_10.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_3.xyz + t16_2.xyz;
    t16_2.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_2.xyz = t16_2.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_23) * t16_2.xyz + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_21) + t16_0.xyz;
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
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  mediump vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  mediump vec2 tmpvar_7;
  tmpvar_6.xy = vec2(0.0, 0.0);
  tmpvar_7.x = 0.0;
  tmpvar_5.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_5.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_6.zw = tmpvar_1.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_4 = tmpvar_8;
  highp mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * _glesNormal));
  worldNormal_3 = tmpvar_10;
  mediump vec4 tmpvar_11;
  tmpvar_11.xyz = worldViewDir_4;
  tmpvar_11.w = 0.0;
  mediump vec4 tmpvar_12;
  tmpvar_12.xyz = worldNormal_3;
  tmpvar_12.w = 0.0;
  highp float tmpvar_13;
  highp vec3 x_14;
  x_14 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_13 = sqrt(dot (x_14, x_14));
  fogpos_2 = tmpvar_13;
  tmpvar_7.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_7.x = (1.0 - tmpvar_7.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_5;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = tmpvar_11;
  xlv_TEXCOORD4 = tmpvar_12;
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
uniform sampler2D _MaskTex;
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
mediump float tmpvar_7;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_8;
  mediump vec3 lit_color_9;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_10.xyz;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_11.x;
  tmpvar_3 = tmpvar_11.y;
  tmpvar_4 = tmpvar_11.z;
  tmpvar_5 = tmpvar_11.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = xlv_TEXCOORD4.xyz;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  x1_14.x = dot (unity_SHAr, tmpvar_12);
  x1_14.y = dot (unity_SHAg, tmpvar_12);
  x1_14.z = dot (unity_SHAb, tmpvar_12);
  mediump vec4 tmpvar_15;
  tmpvar_15 = (xlv_TEXCOORD4.xyzz * xlv_TEXCOORD4.yzzx);
  x2_13.x = dot (unity_SHBr, tmpvar_15);
  x2_13.y = dot (unity_SHBg, tmpvar_15);
  x2_13.z = dot (unity_SHBb, tmpvar_15);
  mediump vec3 tmpvar_16;
  tmpvar_16 = max (((1.055 * 
    pow ((((x2_13 + 
      (unity_SHC.xyz * ((xlv_TEXCOORD4.x * xlv_TEXCOORD4.x) - (xlv_TEXCOORD4.y * xlv_TEXCOORD4.y)))
    ) + x1_14) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_7 = ((max (
    dot (xlv_TEXCOORD4.xyz, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_9 = (((
    ((tmpvar_16 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_7), vec3(
    min (1.0, (tmpvar_7 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (xlv_TEXCOORD4.xyz, xlv_TEXCOORD3.xyz) * xlv_TEXCOORD4.xyz)
      )), -(xlv_TEXCOORD4.xyz)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_16)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 tmpvar_17;
  tmpvar_17 = mix (lit_color_9, vec3((dot (lit_color_9, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_7 * _LightParameters.y)));
  c_8.xyz = tmpvar_17;
  c_8.w = _RimColor.w;
  lowp vec4 _color_18;
  _color_18 = (c_8 * _FadeColor);
  c_8.w = _color_18.w;
  c_8.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_18.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_8;
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
	lowp vec4 albedoTex;
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
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD6;
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
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    vs_TEXCOORD1.xy = vec2(0.0, 0.0);
    vs_TEXCOORD1.zw = in_TEXCOORD0.xy;
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
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_3;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
    vs_TEXCOORD3.w = 0.0;
    t0.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t0.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD4.xyz = t0.xyz;
    vs_TEXCOORD4.w = 0.0;
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
	lowp vec4 albedoTex;
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
uniform lowp sampler2D _MaskTex;
in mediump vec4 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
mediump vec3 t16_2;
lowp vec4 t10_2;
mediump vec4 t16_3;
lowp vec3 t10_4;
mediump vec3 t16_5;
lowp vec3 t10_6;
mediump float t16_21;
mediump float t16_22;
void main()
{
    t16_0.x = vs_TEXCOORD4.y * vs_TEXCOORD4.y;
    t16_0.x = vs_TEXCOORD4.x * vs_TEXCOORD4.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD4.yzzx * vs_TEXCOORD4.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD4.xyz;
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
    t16_21 = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD4.xyz);
    t16_21 = t16_21 + t16_21;
    t16_1.xyz = vs_TEXCOORD4.xyz * (-vec3(t16_21)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_1.xyz, (-vs_TEXCOORD4.xyz));
    t16_0 = max(t16_0, vec4(0.0, 0.0, 0.0, 0.25));
    t16_21 = t16_0.w * _Glossiness.w;
    t16_21 = t16_21 * _MatGlossiness;
    t16_21 = t16_21 * 10.0;
    t16_1.xyz = vec3(t16_21) * _Glossiness.xyz;
    t16_1.xyz = t16_0.xyz * t16_1.xyz;
    t10_2 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_3.xy = vec2((-t10_2.w) + float(1.0), (-t10_2.z) + float(1.0));
    t16_21 = (-t16_3.x) * _LightParameters.z + 1.0;
    t16_22 = t16_21 * t10_2.z;
    t16_22 = t16_22 * 5.0;
    t16_1.xyz = vec3(t16_22) * t16_1.xyz;
    t16_3.xzw = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_3.xzw = t10_2.xxx * t16_3.xzw + vec3(1.0, 1.0, 1.0);
    t10_4.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_3.xzw = t16_3.xzw * t10_4.xyz;
    t16_5.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_5.xyz = t10_2.yyy * t16_5.xyz + vec3(1.0, 1.0, 1.0);
    t16_3.xzw = t16_3.xzw * t16_5.xyz;
    t16_1.xyz = t16_1.xyz * t16_3.xzw;
    t16_0.xyz = t16_0.xyz * t16_3.xzw;
    t16_0.xyz = vec3(t16_21) * t16_0.xyz;
    t16_0.xyz = t16_3.yyy * t16_0.xyz;
    t16_21 = dot(vs_TEXCOORD4.xyz, (-_LightDir.xyz));
    t16_21 = max(t16_21, 0.0);
    t10_6.x = (-_LightDir.w) + 1.0;
    t16_21 = t16_21 * _LightDir.w + t10_6.x;
    t16_22 = t16_21 * 1.5;
    t16_22 = min(t16_22, 1.0);
    t16_3.xyz = vec3(t16_21) + (-_CharacterShadowColor.xyz);
    t16_21 = t16_21 * _LightParameters.y;
    t16_3.xyz = vec3(t16_22) * t16_3.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_3.xyz + t16_1.xyz;
    t16_1.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = t16_1.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_21) * t16_1.xyz + t16_0.xyz;
    t10_6.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t10_6.xyz * vs_TEXCOORD2.xxx;
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
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  mediump vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  mediump vec2 tmpvar_7;
  tmpvar_6.xy = vec2(0.0, 0.0);
  tmpvar_7.x = 0.0;
  tmpvar_5.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_5.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_6.zw = tmpvar_1.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_4 = tmpvar_8;
  highp mat3 tmpvar_9;
  tmpvar_9[0] = _Object2World[0].xyz;
  tmpvar_9[1] = _Object2World[1].xyz;
  tmpvar_9[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((tmpvar_9 * _glesNormal));
  worldNormal_3 = tmpvar_10;
  mediump vec4 tmpvar_11;
  tmpvar_11.xyz = worldViewDir_4;
  tmpvar_11.w = 0.0;
  mediump vec4 tmpvar_12;
  tmpvar_12.xyz = worldNormal_3;
  tmpvar_12.w = 0.0;
  highp float tmpvar_13;
  highp vec3 x_14;
  x_14 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_13 = sqrt(dot (x_14, x_14));
  fogpos_2 = tmpvar_13;
  tmpvar_7.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_7.x = (1.0 - tmpvar_7.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_5;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = tmpvar_11;
  xlv_TEXCOORD4 = tmpvar_12;
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
uniform sampler2D _MaskTex;
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
mediump float tmpvar_7;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_8;
  mediump vec3 lit_color_9;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_10.xyz;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_11.x;
  tmpvar_3 = tmpvar_11.y;
  tmpvar_4 = tmpvar_11.z;
  tmpvar_5 = tmpvar_11.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = xlv_TEXCOORD4.xyz;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  x1_14.x = dot (unity_SHAr, tmpvar_12);
  x1_14.y = dot (unity_SHAg, tmpvar_12);
  x1_14.z = dot (unity_SHAb, tmpvar_12);
  mediump vec4 tmpvar_15;
  tmpvar_15 = (xlv_TEXCOORD4.xyzz * xlv_TEXCOORD4.yzzx);
  x2_13.x = dot (unity_SHBr, tmpvar_15);
  x2_13.y = dot (unity_SHBg, tmpvar_15);
  x2_13.z = dot (unity_SHBb, tmpvar_15);
  mediump vec3 tmpvar_16;
  tmpvar_16 = max (((1.055 * 
    pow ((((x2_13 + 
      (unity_SHC.xyz * ((xlv_TEXCOORD4.x * xlv_TEXCOORD4.x) - (xlv_TEXCOORD4.y * xlv_TEXCOORD4.y)))
    ) + x1_14) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_7 = ((max (
    dot (xlv_TEXCOORD4.xyz, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_9 = (((
    ((tmpvar_16 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_7), vec3(
    min (1.0, (tmpvar_7 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (xlv_TEXCOORD4.xyz, xlv_TEXCOORD3.xyz) * xlv_TEXCOORD4.xyz)
      )), -(xlv_TEXCOORD4.xyz)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_16)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 tmpvar_17;
  tmpvar_17 = mix (lit_color_9, vec3((dot (lit_color_9, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_7 * _LightParameters.y)));
  c_8.xyz = tmpvar_17;
  c_8.w = _RimColor.w;
  lowp vec4 _color_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_20;
  tmpvar_20 = (_FadeColor.w - ((0.9375 * tmpvar_19.y) + 0.03125));
  mediump float tmpvar_21;
  tmpvar_21 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_20)
  )), 0.0, 1.0);
  _color_18.xyz = (_FadeColor.xyz * (tmpvar_21 + c_8.xyz));
  _color_18.w = ((32.0 * tmpvar_20) + 0.9375);
  c_8.w = _color_18.w;
  c_8.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_18.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_8;
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
	lowp vec4 albedoTex;
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
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD6;
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
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    vs_TEXCOORD1.xy = vec2(0.0, 0.0);
    vs_TEXCOORD1.zw = in_TEXCOORD0.xy;
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
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_3;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
    vs_TEXCOORD3.w = 0.0;
    t0.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t0.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD4.xyz = t0.xyz;
    vs_TEXCOORD4.w = 0.0;
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
	lowp vec4 albedoTex;
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
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _NoiseTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
mediump vec4 t16_2;
lowp vec4 t10_2;
mediump vec4 t16_3;
lowp vec3 t10_4;
mediump vec3 t16_5;
lowp float t10_6;
mediump float t16_21;
mediump float t16_22;
void main()
{
    t16_0.x = vs_TEXCOORD4.y * vs_TEXCOORD4.y;
    t16_0.x = vs_TEXCOORD4.x * vs_TEXCOORD4.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD4.yzzx * vs_TEXCOORD4.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD4.xyz;
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
    t16_21 = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD4.xyz);
    t16_21 = t16_21 + t16_21;
    t16_1.xyz = vs_TEXCOORD4.xyz * (-vec3(t16_21)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_1.xyz, (-vs_TEXCOORD4.xyz));
    t16_0 = max(t16_0, vec4(0.0, 0.0, 0.0, 0.25));
    t16_21 = t16_0.w * _Glossiness.w;
    t16_21 = t16_21 * _MatGlossiness;
    t16_21 = t16_21 * 10.0;
    t16_1.xyz = vec3(t16_21) * _Glossiness.xyz;
    t16_1.xyz = t16_0.xyz * t16_1.xyz;
    t10_2 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_3.xy = vec2((-t10_2.w) + float(1.0), (-t10_2.z) + float(1.0));
    t16_21 = (-t16_3.x) * _LightParameters.z + 1.0;
    t16_22 = t16_21 * t10_2.z;
    t16_22 = t16_22 * 5.0;
    t16_1.xyz = vec3(t16_22) * t16_1.xyz;
    t16_3.xzw = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_3.xzw = t10_2.xxx * t16_3.xzw + vec3(1.0, 1.0, 1.0);
    t10_4.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_3.xzw = t16_3.xzw * t10_4.xyz;
    t16_5.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_5.xyz = t10_2.yyy * t16_5.xyz + vec3(1.0, 1.0, 1.0);
    t16_3.xzw = t16_3.xzw * t16_5.xyz;
    t16_1.xyz = t16_1.xyz * t16_3.xzw;
    t16_0.xyz = t16_0.xyz * t16_3.xzw;
    t16_0.xyz = vec3(t16_21) * t16_0.xyz;
    t16_0.xyz = t16_3.yyy * t16_0.xyz;
    t16_21 = dot(vs_TEXCOORD4.xyz, (-_LightDir.xyz));
    t16_21 = max(t16_21, 0.0);
    t10_6 = (-_LightDir.w) + 1.0;
    t16_21 = t16_21 * _LightDir.w + t10_6;
    t16_22 = t16_21 * 1.5;
    t16_22 = min(t16_22, 1.0);
    t16_3.xyz = vec3(t16_21) + (-_CharacterShadowColor.xyz);
    t16_21 = t16_21 * _LightParameters.y;
    t16_3.xyz = vec3(t16_22) * t16_3.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_3.xyz + t16_1.xyz;
    t16_1.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = t16_1.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_21) * t16_1.xyz + t16_0.xyz;
    t10_4.x = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_21 = t10_4.x * 0.9375 + 0.03125;
    t16_21 = (-t16_21) + _FadeColor.w;
    t16_1.x = -abs(t16_21) * 32.0 + 1.0;
    t16_2.w = t16_21 * 32.0 + 0.9375;
    t16_21 = max(t16_1.x, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_21);
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_2.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0 = t16_2;
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
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  highp vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  mediump vec4 tmpvar_7;
  mediump vec2 tmpvar_8;
  mediump vec3 tmpvar_9;
  tmpvar_7.xy = vec2(0.0, 0.0);
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
  tmpvar_6.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_6.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_7.zw = tmpvar_1.xy;
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
  mediump vec4 tmpvar_16;
  tmpvar_16.xyz = worldViewDir_4;
  tmpvar_16.w = 0.0;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldNormal_3;
  tmpvar_17.w = 0.0;
  highp float tmpvar_18;
  highp vec3 x_19;
  x_19 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_18 = sqrt(dot (x_19, x_19));
  fogpos_2 = tmpvar_18;
  tmpvar_8.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_8.x = (1.0 - tmpvar_8.y);
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = tmpvar_6;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = tmpvar_8;
  xlv_TEXCOORD3 = tmpvar_16;
  xlv_TEXCOORD4 = tmpvar_17;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_9;
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
uniform sampler2D _MaskTex;
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
mediump float tmpvar_7;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  lowp vec4 c_8;
  mediump vec3 lit_color_9;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_10.xyz;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_11.x;
  tmpvar_3 = tmpvar_11.y;
  tmpvar_4 = tmpvar_11.z;
  tmpvar_5 = tmpvar_11.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = xlv_TEXCOORD4.xyz;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  x1_14.x = dot (unity_SHAr, tmpvar_12);
  x1_14.y = dot (unity_SHAg, tmpvar_12);
  x1_14.z = dot (unity_SHAb, tmpvar_12);
  mediump vec4 tmpvar_15;
  tmpvar_15 = (xlv_TEXCOORD4.xyzz * xlv_TEXCOORD4.yzzx);
  x2_13.x = dot (unity_SHBr, tmpvar_15);
  x2_13.y = dot (unity_SHBg, tmpvar_15);
  x2_13.z = dot (unity_SHBb, tmpvar_15);
  mediump vec3 tmpvar_16;
  tmpvar_16 = max (((1.055 * 
    pow ((((x2_13 + 
      (unity_SHC.xyz * ((xlv_TEXCOORD4.x * xlv_TEXCOORD4.x) - (xlv_TEXCOORD4.y * xlv_TEXCOORD4.y)))
    ) + x1_14) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_7 = ((max (
    dot (xlv_TEXCOORD4.xyz, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_9 = (((
    ((tmpvar_16 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_7), vec3(
    min (1.0, (tmpvar_7 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (xlv_TEXCOORD4.xyz, xlv_TEXCOORD3.xyz) * xlv_TEXCOORD4.xyz)
      )), -(xlv_TEXCOORD4.xyz)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_16)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 tmpvar_17;
  tmpvar_17 = mix (lit_color_9, vec3((dot (lit_color_9, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_7 * _LightParameters.y)));
  c_8.xyz = tmpvar_17;
  c_8.w = _RimColor.w;
  lowp vec4 _color_18;
  _color_18.w = c_8.w;
  mediump float rTest_19;
  mediump vec2 tmpvar_20;
  tmpvar_20 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_21;
  tmpvar_21 = ((1.5 * _FadeColor.w) - tmpvar_20.y);
  if ((tmpvar_21 < 0.0)) {
    discard;
  };
  mediump float tmpvar_22;
  tmpvar_22 = clamp ((1.0 - (tmpvar_21 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_23;
  tmpvar_23.x = tmpvar_20.x;
  tmpvar_23.y = clamp ((tmpvar_22 - tmpvar_20.y), 0.0, 1.0);
  lowp float tmpvar_24;
  tmpvar_24 = texture2D (_RasterTex, tmpvar_23).z;
  rTest_19 = tmpvar_24;
  mediump float x_25;
  x_25 = (rTest_19 - 0.4);
  if ((x_25 < 0.0)) {
    discard;
  };
  _color_18.xyz = (c_8.xyz + (_FadeColor.xyz * tmpvar_22));
  c_8.w = _color_18.w;
  c_8.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_18.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_8;
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
	lowp vec4 albedoTex;
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
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD7;
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
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t1.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t1;
    vs_TEXCOORD1.xy = vec2(0.0, 0.0);
    vs_TEXCOORD1.zw = in_TEXCOORD0.xy;
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
    vs_TEXCOORD2.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_5;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * t1.xyz;
    vs_TEXCOORD3.xyz = t1.xyz;
    vs_TEXCOORD3.w = 0.0;
    t1.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t1.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * t1.xyz;
    vs_TEXCOORD4.xyz = t1.xyz;
    vs_TEXCOORD4.w = 0.0;
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
	lowp vec4 albedoTex;
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
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _RasterTex;
in mediump vec4 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD7;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
bool tb1;
mediump vec3 t16_2;
mediump vec4 t16_3;
lowp vec3 t10_4;
mediump vec3 t16_5;
lowp float t10_6;
mediump vec3 t16_10;
mediump float t16_21;
mediump float t16_23;
void main()
{
    t16_0.xy = vs_TEXCOORD7.xy / vs_TEXCOORD7.zz;
    t16_21 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_21<0.0;
    t16_21 = (-t16_21) * 2.5 + 1.0;
    t16_21 = clamp(t16_21, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_21;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1.x = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1.x + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD4.y * vs_TEXCOORD4.y;
    t16_0.x = vs_TEXCOORD4.x * vs_TEXCOORD4.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD4.yzzx * vs_TEXCOORD4.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD4.xyz;
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
    t16_2.x = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD4.xyz);
    t16_2.x = t16_2.x + t16_2.x;
    t16_2.xyz = vs_TEXCOORD4.xyz * (-t16_2.xxx) + vs_TEXCOORD3.xyz;
    t16_2.x = dot(t16_2.xyz, (-vs_TEXCOORD4.xyz));
    t16_2.x = max(t16_2.x, 0.25);
    t16_2.x = t16_2.x * _Glossiness.w;
    t16_2.x = t16_2.x * _MatGlossiness;
    t16_2.x = t16_2.x * 10.0;
    t16_2.xyz = t16_2.xxx * _Glossiness.xyz;
    t16_2.xyz = t16_0.xyz * t16_2.xyz;
    t10_1 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_3.xy = vec2((-t10_1.w) + float(1.0), (-t10_1.z) + float(1.0));
    t16_23 = (-t16_3.x) * _LightParameters.z + 1.0;
    t16_3.x = t10_1.z * t16_23;
    t16_3.x = t16_3.x * 5.0;
    t16_2.xyz = t16_2.xyz * t16_3.xxx;
    t16_3.xzw = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_3.xzw = t10_1.xxx * t16_3.xzw + vec3(1.0, 1.0, 1.0);
    t10_4.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_3.xzw = t16_3.xzw * t10_4.xyz;
    t16_5.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_5.xyz = t10_1.yyy * t16_5.xyz + vec3(1.0, 1.0, 1.0);
    t16_3.xzw = t16_3.xzw * t16_5.xyz;
    t16_2.xyz = t16_2.xyz * t16_3.xzw;
    t16_0.xyz = t16_0.xyz * t16_3.xzw;
    t16_0.xyz = vec3(t16_23) * t16_0.xyz;
    t16_0.xyz = t16_3.yyy * t16_0.xyz;
    t16_23 = dot(vs_TEXCOORD4.xyz, (-_LightDir.xyz));
    t16_23 = max(t16_23, 0.0);
    t10_6 = (-_LightDir.w) + 1.0;
    t16_23 = t16_23 * _LightDir.w + t10_6;
    t16_3.x = t16_23 * 1.5;
    t16_3.x = min(t16_3.x, 1.0);
    t16_10.xyz = vec3(t16_23) + (-_CharacterShadowColor.xyz);
    t16_23 = t16_23 * _LightParameters.y;
    t16_3.xyz = t16_3.xxx * t16_10.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_3.xyz + t16_2.xyz;
    t16_2.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_2.xyz = t16_2.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_23) * t16_2.xyz + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_21) + t16_0.xyz;
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
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  mediump vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  mediump vec2 tmpvar_7;
  mediump vec4 tmpvar_8;
  tmpvar_6.xy = vec2(0.0, 0.0);
  tmpvar_7.x = 0.0;
  tmpvar_5.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_5.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_6.zw = tmpvar_1.xy;
  highp vec3 tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Object2World * _glesVertex);
  tmpvar_9 = normalize((_WorldSpaceCameraPos - tmpvar_10.xyz));
  worldViewDir_4 = tmpvar_9;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_3 = tmpvar_12;
  mediump vec4 tmpvar_13;
  tmpvar_13.xyz = worldViewDir_4;
  tmpvar_13.w = 0.0;
  mediump vec4 tmpvar_14;
  tmpvar_14.xyz = worldNormal_3;
  tmpvar_14.w = 0.0;
  tmpvar_8 = (unity_World2Shadow[0] * tmpvar_10);
  highp float tmpvar_15;
  highp vec3 x_16;
  x_16 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_15 = sqrt(dot (x_16, x_16));
  fogpos_2 = tmpvar_15;
  tmpvar_7.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_7.x = (1.0 - tmpvar_7.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_5;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = tmpvar_13;
  xlv_TEXCOORD4 = tmpvar_14;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_8;
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
uniform sampler2D _MaskTex;
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
mediump float tmpvar_7;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_8;
  mediump vec3 lit_color_9;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_10.xyz;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_11.x;
  tmpvar_3 = tmpvar_11.y;
  tmpvar_4 = tmpvar_11.z;
  tmpvar_5 = tmpvar_11.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = xlv_TEXCOORD4.xyz;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  x1_14.x = dot (unity_SHAr, tmpvar_12);
  x1_14.y = dot (unity_SHAg, tmpvar_12);
  x1_14.z = dot (unity_SHAb, tmpvar_12);
  mediump vec4 tmpvar_15;
  tmpvar_15 = (xlv_TEXCOORD4.xyzz * xlv_TEXCOORD4.yzzx);
  x2_13.x = dot (unity_SHBr, tmpvar_15);
  x2_13.y = dot (unity_SHBg, tmpvar_15);
  x2_13.z = dot (unity_SHBb, tmpvar_15);
  mediump vec3 tmpvar_16;
  tmpvar_16 = max (((1.055 * 
    pow ((((x2_13 + 
      (unity_SHC.xyz * ((xlv_TEXCOORD4.x * xlv_TEXCOORD4.x) - (xlv_TEXCOORD4.y * xlv_TEXCOORD4.y)))
    ) + x1_14) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_7 = ((max (
    dot (xlv_TEXCOORD4.xyz, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_9 = (((
    ((tmpvar_16 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_7), vec3(
    min (1.0, (tmpvar_7 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (xlv_TEXCOORD4.xyz, xlv_TEXCOORD3.xyz) * xlv_TEXCOORD4.xyz)
      )), -(xlv_TEXCOORD4.xyz)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_16)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 atten_17;
  lowp float tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_18 = tmpvar_19;
  lowp vec3 tmpvar_20;
  tmpvar_20 = vec3(tmpvar_18);
  atten_17 = tmpvar_20;
  lit_color_9 = (lit_color_9 * max ((_Shadows.x + 
    (atten_17 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_21;
  tmpvar_21 = mix (lit_color_9, vec3((dot (lit_color_9, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_7 * _LightParameters.y)));
  c_8.xyz = tmpvar_21;
  c_8.w = _RimColor.w;
  lowp vec4 _color_22;
  _color_22 = (c_8 * _FadeColor);
  c_8.w = _color_22.w;
  c_8.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_22.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_8;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FANCY_FADE" }
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
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  mediump vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  mediump vec2 tmpvar_7;
  mediump vec4 tmpvar_8;
  tmpvar_6.xy = vec2(0.0, 0.0);
  tmpvar_7.x = 0.0;
  tmpvar_5.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_5.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_6.zw = tmpvar_1.xy;
  highp vec3 tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Object2World * _glesVertex);
  tmpvar_9 = normalize((_WorldSpaceCameraPos - tmpvar_10.xyz));
  worldViewDir_4 = tmpvar_9;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_3 = tmpvar_12;
  mediump vec4 tmpvar_13;
  tmpvar_13.xyz = worldViewDir_4;
  tmpvar_13.w = 0.0;
  mediump vec4 tmpvar_14;
  tmpvar_14.xyz = worldNormal_3;
  tmpvar_14.w = 0.0;
  tmpvar_8 = (unity_World2Shadow[0] * tmpvar_10);
  highp float tmpvar_15;
  highp vec3 x_16;
  x_16 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_15 = sqrt(dot (x_16, x_16));
  fogpos_2 = tmpvar_15;
  tmpvar_7.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_7.x = (1.0 - tmpvar_7.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_5;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = tmpvar_13;
  xlv_TEXCOORD4 = tmpvar_14;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_8;
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
uniform sampler2D _MaskTex;
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
mediump float tmpvar_7;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_8;
  mediump vec3 lit_color_9;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_10.xyz;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_11.x;
  tmpvar_3 = tmpvar_11.y;
  tmpvar_4 = tmpvar_11.z;
  tmpvar_5 = tmpvar_11.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = xlv_TEXCOORD4.xyz;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  x1_14.x = dot (unity_SHAr, tmpvar_12);
  x1_14.y = dot (unity_SHAg, tmpvar_12);
  x1_14.z = dot (unity_SHAb, tmpvar_12);
  mediump vec4 tmpvar_15;
  tmpvar_15 = (xlv_TEXCOORD4.xyzz * xlv_TEXCOORD4.yzzx);
  x2_13.x = dot (unity_SHBr, tmpvar_15);
  x2_13.y = dot (unity_SHBg, tmpvar_15);
  x2_13.z = dot (unity_SHBb, tmpvar_15);
  mediump vec3 tmpvar_16;
  tmpvar_16 = max (((1.055 * 
    pow ((((x2_13 + 
      (unity_SHC.xyz * ((xlv_TEXCOORD4.x * xlv_TEXCOORD4.x) - (xlv_TEXCOORD4.y * xlv_TEXCOORD4.y)))
    ) + x1_14) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_7 = ((max (
    dot (xlv_TEXCOORD4.xyz, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_9 = (((
    ((tmpvar_16 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_7), vec3(
    min (1.0, (tmpvar_7 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (xlv_TEXCOORD4.xyz, xlv_TEXCOORD3.xyz) * xlv_TEXCOORD4.xyz)
      )), -(xlv_TEXCOORD4.xyz)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_16)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 atten_17;
  lowp float tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_18 = tmpvar_19;
  lowp vec3 tmpvar_20;
  tmpvar_20 = vec3(tmpvar_18);
  atten_17 = tmpvar_20;
  lit_color_9 = (lit_color_9 * max ((_Shadows.x + 
    (atten_17 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_21;
  tmpvar_21 = mix (lit_color_9, vec3((dot (lit_color_9, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_7 * _LightParameters.y)));
  c_8.xyz = tmpvar_21;
  c_8.w = _RimColor.w;
  lowp vec4 _color_22;
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_24;
  tmpvar_24 = (_FadeColor.w - ((0.9375 * tmpvar_23.y) + 0.03125));
  mediump float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_24)
  )), 0.0, 1.0);
  _color_22.xyz = (_FadeColor.xyz * (tmpvar_25 + c_8.xyz));
  _color_22.w = ((32.0 * tmpvar_24) + 0.9375);
  c_8.w = _color_22.w;
  c_8.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_22.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_8;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "RASTER_FADE" }
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
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  highp vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  mediump vec4 tmpvar_7;
  mediump vec2 tmpvar_8;
  mediump vec3 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_7.xy = vec2(0.0, 0.0);
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
  tmpvar_6.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_6.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_7.zw = tmpvar_1.xy;
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
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldViewDir_4;
  tmpvar_18.w = 0.0;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldNormal_3;
  tmpvar_19.w = 0.0;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_15);
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_8.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_8.x = (1.0 - tmpvar_8.y);
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = tmpvar_6;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = tmpvar_8;
  xlv_TEXCOORD3 = tmpvar_18;
  xlv_TEXCOORD4 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_9;
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
uniform sampler2D _MaskTex;
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
mediump float tmpvar_7;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_8;
  mediump vec3 lit_color_9;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_10.xyz;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_11.x;
  tmpvar_3 = tmpvar_11.y;
  tmpvar_4 = tmpvar_11.z;
  tmpvar_5 = tmpvar_11.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = xlv_TEXCOORD4.xyz;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  x1_14.x = dot (unity_SHAr, tmpvar_12);
  x1_14.y = dot (unity_SHAg, tmpvar_12);
  x1_14.z = dot (unity_SHAb, tmpvar_12);
  mediump vec4 tmpvar_15;
  tmpvar_15 = (xlv_TEXCOORD4.xyzz * xlv_TEXCOORD4.yzzx);
  x2_13.x = dot (unity_SHBr, tmpvar_15);
  x2_13.y = dot (unity_SHBg, tmpvar_15);
  x2_13.z = dot (unity_SHBb, tmpvar_15);
  mediump vec3 tmpvar_16;
  tmpvar_16 = max (((1.055 * 
    pow ((((x2_13 + 
      (unity_SHC.xyz * ((xlv_TEXCOORD4.x * xlv_TEXCOORD4.x) - (xlv_TEXCOORD4.y * xlv_TEXCOORD4.y)))
    ) + x1_14) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_7 = ((max (
    dot (xlv_TEXCOORD4.xyz, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_9 = (((
    ((tmpvar_16 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_7), vec3(
    min (1.0, (tmpvar_7 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (xlv_TEXCOORD4.xyz, xlv_TEXCOORD3.xyz) * xlv_TEXCOORD4.xyz)
      )), -(xlv_TEXCOORD4.xyz)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_16)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 atten_17;
  lowp float tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_18 = tmpvar_19;
  lowp vec3 tmpvar_20;
  tmpvar_20 = vec3(tmpvar_18);
  atten_17 = tmpvar_20;
  lit_color_9 = (lit_color_9 * max ((_Shadows.x + 
    (atten_17 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_21;
  tmpvar_21 = mix (lit_color_9, vec3((dot (lit_color_9, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_7 * _LightParameters.y)));
  c_8.xyz = tmpvar_21;
  c_8.w = _RimColor.w;
  lowp vec4 _color_22;
  _color_22.w = c_8.w;
  mediump float rTest_23;
  mediump vec2 tmpvar_24;
  tmpvar_24 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_25;
  tmpvar_25 = ((1.5 * _FadeColor.w) - tmpvar_24.y);
  if ((tmpvar_25 < 0.0)) {
    discard;
  };
  mediump float tmpvar_26;
  tmpvar_26 = clamp ((1.0 - (tmpvar_25 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_27;
  tmpvar_27.x = tmpvar_24.x;
  tmpvar_27.y = clamp ((tmpvar_26 - tmpvar_24.y), 0.0, 1.0);
  lowp float tmpvar_28;
  tmpvar_28 = texture2D (_RasterTex, tmpvar_27).z;
  rTest_23 = tmpvar_28;
  mediump float x_29;
  x_29 = (rTest_23 - 0.4);
  if ((x_29 < 0.0)) {
    discard;
  };
  _color_22.xyz = (c_8.xyz + (_FadeColor.xyz * tmpvar_26));
  c_8.w = _color_22.w;
  c_8.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_22.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_8;
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
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  mediump vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  mediump vec2 tmpvar_7;
  mediump vec4 tmpvar_8;
  tmpvar_6.xy = vec2(0.0, 0.0);
  tmpvar_7.x = 0.0;
  tmpvar_5.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_5.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_6.zw = tmpvar_1.xy;
  highp vec3 tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Object2World * _glesVertex);
  tmpvar_9 = normalize((_WorldSpaceCameraPos - tmpvar_10.xyz));
  worldViewDir_4 = tmpvar_9;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_3 = tmpvar_12;
  mediump vec4 tmpvar_13;
  tmpvar_13.xyz = worldViewDir_4;
  tmpvar_13.w = 0.0;
  mediump vec4 tmpvar_14;
  tmpvar_14.xyz = worldNormal_3;
  tmpvar_14.w = 0.0;
  tmpvar_8 = (unity_World2Shadow[0] * tmpvar_10);
  highp float tmpvar_15;
  highp vec3 x_16;
  x_16 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_15 = sqrt(dot (x_16, x_16));
  fogpos_2 = tmpvar_15;
  tmpvar_7.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_7.x = (1.0 - tmpvar_7.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_5;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = tmpvar_13;
  xlv_TEXCOORD4 = tmpvar_14;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_8;
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
uniform sampler2D _MaskTex;
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
mediump float tmpvar_7;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_8;
  mediump vec3 lit_color_9;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_10.xyz;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_11.x;
  tmpvar_3 = tmpvar_11.y;
  tmpvar_4 = tmpvar_11.z;
  tmpvar_5 = tmpvar_11.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = xlv_TEXCOORD4.xyz;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  x1_14.x = dot (unity_SHAr, tmpvar_12);
  x1_14.y = dot (unity_SHAg, tmpvar_12);
  x1_14.z = dot (unity_SHAb, tmpvar_12);
  mediump vec4 tmpvar_15;
  tmpvar_15 = (xlv_TEXCOORD4.xyzz * xlv_TEXCOORD4.yzzx);
  x2_13.x = dot (unity_SHBr, tmpvar_15);
  x2_13.y = dot (unity_SHBg, tmpvar_15);
  x2_13.z = dot (unity_SHBb, tmpvar_15);
  mediump vec3 tmpvar_16;
  tmpvar_16 = max (((1.055 * 
    pow ((((x2_13 + 
      (unity_SHC.xyz * ((xlv_TEXCOORD4.x * xlv_TEXCOORD4.x) - (xlv_TEXCOORD4.y * xlv_TEXCOORD4.y)))
    ) + x1_14) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_7 = ((max (
    dot (xlv_TEXCOORD4.xyz, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_9 = (((
    ((tmpvar_16 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_7), vec3(
    min (1.0, (tmpvar_7 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (xlv_TEXCOORD4.xyz, xlv_TEXCOORD3.xyz) * xlv_TEXCOORD4.xyz)
      )), -(xlv_TEXCOORD4.xyz)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_16)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 atten_17;
  lowp float shadow_18;
  shadow_18 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_19;
  tmpvar_19 = vec3(shadow_18);
  atten_17 = tmpvar_19;
  lit_color_9 = (lit_color_9 * max ((_Shadows.x + 
    (atten_17 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_20;
  tmpvar_20 = mix (lit_color_9, vec3((dot (lit_color_9, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_7 * _LightParameters.y)));
  c_8.xyz = tmpvar_20;
  c_8.w = _RimColor.w;
  lowp vec4 _color_21;
  _color_21 = (c_8 * _FadeColor);
  c_8.w = _color_21.w;
  c_8.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_21.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_8;
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
	lowp vec4 albedoTex;
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
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD6;
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
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    vs_TEXCOORD1.xy = vec2(0.0, 0.0);
    vs_TEXCOORD1.zw = in_TEXCOORD0.xy;
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
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_3;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
    vs_TEXCOORD3.w = 0.0;
    t0.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t0.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD4.xyz = t0.xyz;
    vs_TEXCOORD4.w = 0.0;
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
	lowp vec4 albedoTex;
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
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
mediump vec3 t16_2;
lowp vec4 t10_2;
mediump vec4 t16_3;
lowp vec3 t10_4;
mediump vec3 t16_5;
lowp vec3 t10_6;
mediump float t16_8;
mediump float t16_21;
mediump float t16_22;
void main()
{
    t16_0.x = vs_TEXCOORD4.y * vs_TEXCOORD4.y;
    t16_0.x = vs_TEXCOORD4.x * vs_TEXCOORD4.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD4.yzzx * vs_TEXCOORD4.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD4.xyz;
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
    t16_21 = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD4.xyz);
    t16_21 = t16_21 + t16_21;
    t16_1.xyz = vs_TEXCOORD4.xyz * (-vec3(t16_21)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_1.xyz, (-vs_TEXCOORD4.xyz));
    t16_0 = max(t16_0, vec4(0.0, 0.0, 0.0, 0.25));
    t16_21 = t16_0.w * _Glossiness.w;
    t16_21 = t16_21 * _MatGlossiness;
    t16_21 = t16_21 * 10.0;
    t16_1.xyz = vec3(t16_21) * _Glossiness.xyz;
    t16_1.xyz = t16_0.xyz * t16_1.xyz;
    t10_2 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_3.xy = vec2((-t10_2.w) + float(1.0), (-t10_2.z) + float(1.0));
    t16_21 = (-t16_3.x) * _LightParameters.z + 1.0;
    t16_22 = t16_21 * t10_2.z;
    t16_22 = t16_22 * 5.0;
    t16_1.xyz = vec3(t16_22) * t16_1.xyz;
    t16_3.xzw = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_3.xzw = t10_2.xxx * t16_3.xzw + vec3(1.0, 1.0, 1.0);
    t10_4.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_3.xzw = t16_3.xzw * t10_4.xyz;
    t16_5.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_5.xyz = t10_2.yyy * t16_5.xyz + vec3(1.0, 1.0, 1.0);
    t16_3.xzw = t16_3.xzw * t16_5.xyz;
    t16_1.xyz = t16_1.xyz * t16_3.xzw;
    t16_0.xyz = t16_0.xyz * t16_3.xzw;
    t16_0.xyz = vec3(t16_21) * t16_0.xyz;
    t16_0.xyz = t16_3.yyy * t16_0.xyz;
    t16_21 = dot(vs_TEXCOORD4.xyz, (-_LightDir.xyz));
    t16_21 = max(t16_21, 0.0);
    t10_6.x = (-_LightDir.w) + 1.0;
    t16_21 = t16_21 * _LightDir.w + t10_6.x;
    t16_22 = t16_21 * 1.5;
    t16_22 = min(t16_22, 1.0);
    t16_3.xyz = vec3(t16_21) + (-_CharacterShadowColor.xyz);
    t16_21 = t16_21 * _LightParameters.y;
    t16_3.xyz = vec3(t16_22) * t16_3.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_3.xyz + t16_1.xyz;
    vec3 txVec24 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_1.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec24, 0.0);
    t16_8 = (-_LightShadowData.x) + 1.0;
    t16_1.x = t16_1.x * t16_8 + _LightShadowData.x;
    t16_1.x = t16_1.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_1.x = max(t16_1.x, 0.0);
    t16_0.xyz = t16_0.xyz * t16_1.xxx;
    t16_1.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = t16_1.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_21) * t16_1.xyz + t16_0.xyz;
    t10_6.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t10_6.xyz * vs_TEXCOORD2.xxx;
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
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  mediump vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  mediump vec2 tmpvar_7;
  mediump vec4 tmpvar_8;
  tmpvar_6.xy = vec2(0.0, 0.0);
  tmpvar_7.x = 0.0;
  tmpvar_5.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_5.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_6.zw = tmpvar_1.xy;
  highp vec3 tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Object2World * _glesVertex);
  tmpvar_9 = normalize((_WorldSpaceCameraPos - tmpvar_10.xyz));
  worldViewDir_4 = tmpvar_9;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_3 = tmpvar_12;
  mediump vec4 tmpvar_13;
  tmpvar_13.xyz = worldViewDir_4;
  tmpvar_13.w = 0.0;
  mediump vec4 tmpvar_14;
  tmpvar_14.xyz = worldNormal_3;
  tmpvar_14.w = 0.0;
  tmpvar_8 = (unity_World2Shadow[0] * tmpvar_10);
  highp float tmpvar_15;
  highp vec3 x_16;
  x_16 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_15 = sqrt(dot (x_16, x_16));
  fogpos_2 = tmpvar_15;
  tmpvar_7.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_7.x = (1.0 - tmpvar_7.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_5;
  xlv_TEXCOORD1 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = tmpvar_13;
  xlv_TEXCOORD4 = tmpvar_14;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_8;
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
uniform sampler2D _MaskTex;
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
mediump float tmpvar_7;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_8;
  mediump vec3 lit_color_9;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_10.xyz;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_11.x;
  tmpvar_3 = tmpvar_11.y;
  tmpvar_4 = tmpvar_11.z;
  tmpvar_5 = tmpvar_11.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = xlv_TEXCOORD4.xyz;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  x1_14.x = dot (unity_SHAr, tmpvar_12);
  x1_14.y = dot (unity_SHAg, tmpvar_12);
  x1_14.z = dot (unity_SHAb, tmpvar_12);
  mediump vec4 tmpvar_15;
  tmpvar_15 = (xlv_TEXCOORD4.xyzz * xlv_TEXCOORD4.yzzx);
  x2_13.x = dot (unity_SHBr, tmpvar_15);
  x2_13.y = dot (unity_SHBg, tmpvar_15);
  x2_13.z = dot (unity_SHBb, tmpvar_15);
  mediump vec3 tmpvar_16;
  tmpvar_16 = max (((1.055 * 
    pow ((((x2_13 + 
      (unity_SHC.xyz * ((xlv_TEXCOORD4.x * xlv_TEXCOORD4.x) - (xlv_TEXCOORD4.y * xlv_TEXCOORD4.y)))
    ) + x1_14) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_7 = ((max (
    dot (xlv_TEXCOORD4.xyz, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_9 = (((
    ((tmpvar_16 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_7), vec3(
    min (1.0, (tmpvar_7 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (xlv_TEXCOORD4.xyz, xlv_TEXCOORD3.xyz) * xlv_TEXCOORD4.xyz)
      )), -(xlv_TEXCOORD4.xyz)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_16)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 atten_17;
  lowp float shadow_18;
  shadow_18 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_19;
  tmpvar_19 = vec3(shadow_18);
  atten_17 = tmpvar_19;
  lit_color_9 = (lit_color_9 * max ((_Shadows.x + 
    (atten_17 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_20;
  tmpvar_20 = mix (lit_color_9, vec3((dot (lit_color_9, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_7 * _LightParameters.y)));
  c_8.xyz = tmpvar_20;
  c_8.w = _RimColor.w;
  lowp vec4 _color_21;
  lowp vec4 tmpvar_22;
  tmpvar_22 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_23;
  tmpvar_23 = (_FadeColor.w - ((0.9375 * tmpvar_22.y) + 0.03125));
  mediump float tmpvar_24;
  tmpvar_24 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_23)
  )), 0.0, 1.0);
  _color_21.xyz = (_FadeColor.xyz * (tmpvar_24 + c_8.xyz));
  _color_21.w = ((32.0 * tmpvar_23) + 0.9375);
  c_8.w = _color_21.w;
  c_8.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_21.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_8;
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
	lowp vec4 albedoTex;
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
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD6;
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
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    vs_TEXCOORD1.xy = vec2(0.0, 0.0);
    vs_TEXCOORD1.zw = in_TEXCOORD0.xy;
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
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_3;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
    vs_TEXCOORD3.w = 0.0;
    t0.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t0.xyz;
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    vs_TEXCOORD4.xyz = t0.xyz;
    vs_TEXCOORD4.w = 0.0;
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
	lowp vec4 albedoTex;
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
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _NoiseTexture;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
mediump vec4 t16_2;
lowp vec4 t10_2;
mediump vec4 t16_3;
lowp vec3 t10_4;
mediump vec3 t16_5;
lowp float t10_6;
mediump float t16_8;
mediump float t16_21;
mediump float t16_22;
void main()
{
    t16_0.x = vs_TEXCOORD4.y * vs_TEXCOORD4.y;
    t16_0.x = vs_TEXCOORD4.x * vs_TEXCOORD4.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD4.yzzx * vs_TEXCOORD4.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD4.xyz;
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
    t16_21 = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD4.xyz);
    t16_21 = t16_21 + t16_21;
    t16_1.xyz = vs_TEXCOORD4.xyz * (-vec3(t16_21)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_1.xyz, (-vs_TEXCOORD4.xyz));
    t16_0 = max(t16_0, vec4(0.0, 0.0, 0.0, 0.25));
    t16_21 = t16_0.w * _Glossiness.w;
    t16_21 = t16_21 * _MatGlossiness;
    t16_21 = t16_21 * 10.0;
    t16_1.xyz = vec3(t16_21) * _Glossiness.xyz;
    t16_1.xyz = t16_0.xyz * t16_1.xyz;
    t10_2 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_3.xy = vec2((-t10_2.w) + float(1.0), (-t10_2.z) + float(1.0));
    t16_21 = (-t16_3.x) * _LightParameters.z + 1.0;
    t16_22 = t16_21 * t10_2.z;
    t16_22 = t16_22 * 5.0;
    t16_1.xyz = vec3(t16_22) * t16_1.xyz;
    t16_3.xzw = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_3.xzw = t10_2.xxx * t16_3.xzw + vec3(1.0, 1.0, 1.0);
    t10_4.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_3.xzw = t16_3.xzw * t10_4.xyz;
    t16_5.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_5.xyz = t10_2.yyy * t16_5.xyz + vec3(1.0, 1.0, 1.0);
    t16_3.xzw = t16_3.xzw * t16_5.xyz;
    t16_1.xyz = t16_1.xyz * t16_3.xzw;
    t16_0.xyz = t16_0.xyz * t16_3.xzw;
    t16_0.xyz = vec3(t16_21) * t16_0.xyz;
    t16_0.xyz = t16_3.yyy * t16_0.xyz;
    t16_21 = dot(vs_TEXCOORD4.xyz, (-_LightDir.xyz));
    t16_21 = max(t16_21, 0.0);
    t10_6 = (-_LightDir.w) + 1.0;
    t16_21 = t16_21 * _LightDir.w + t10_6;
    t16_22 = t16_21 * 1.5;
    t16_22 = min(t16_22, 1.0);
    t16_3.xyz = vec3(t16_21) + (-_CharacterShadowColor.xyz);
    t16_21 = t16_21 * _LightParameters.y;
    t16_3.xyz = vec3(t16_22) * t16_3.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_3.xyz + t16_1.xyz;
    vec3 txVec16 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_1.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec16, 0.0);
    t16_8 = (-_LightShadowData.x) + 1.0;
    t16_1.x = t16_1.x * t16_8 + _LightShadowData.x;
    t16_1.x = t16_1.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_1.x = max(t16_1.x, 0.0);
    t16_0.xyz = t16_0.xyz * t16_1.xxx;
    t16_1.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_1.xyz = t16_1.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_21) * t16_1.xyz + t16_0.xyz;
    t10_4.x = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_21 = t10_4.x * 0.9375 + 0.03125;
    t16_21 = (-t16_21) + _FadeColor.w;
    t16_1.x = -abs(t16_21) * 32.0 + 1.0;
    t16_2.w = t16_21 * 32.0 + 0.9375;
    t16_21 = max(t16_1.x, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_21);
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_2.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0 = t16_2;
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
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldNormal_3;
  mediump vec3 worldViewDir_4;
  highp vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  mediump vec4 tmpvar_7;
  mediump vec2 tmpvar_8;
  mediump vec3 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_7.xy = vec2(0.0, 0.0);
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
  tmpvar_6.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_6.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_7.zw = tmpvar_1.xy;
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
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldViewDir_4;
  tmpvar_18.w = 0.0;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldNormal_3;
  tmpvar_19.w = 0.0;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_15);
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_8.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_8.x = (1.0 - tmpvar_8.y);
  gl_Position = tmpvar_5;
  xlv_TEXCOORD0 = tmpvar_6;
  xlv_TEXCOORD1 = tmpvar_7;
  xlv_TEXCOORD2 = tmpvar_8;
  xlv_TEXCOORD3 = tmpvar_18;
  xlv_TEXCOORD4 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_9;
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
uniform sampler2D _MaskTex;
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
mediump float tmpvar_7;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_8;
  mediump vec3 lit_color_9;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_6 = tmpvar_10.xyz;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MaskTex, xlv_TEXCOORD0.zw);
  tmpvar_2 = tmpvar_11.x;
  tmpvar_3 = tmpvar_11.y;
  tmpvar_4 = tmpvar_11.z;
  tmpvar_5 = tmpvar_11.w;
  tmpvar_5 = (1.0 - ((1.0 - tmpvar_5) * _LightParameters.z));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, vec3(tmpvar_2)));
  tmpvar_6 = (tmpvar_6 * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, vec3(tmpvar_3)));
  mediump vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = xlv_TEXCOORD4.xyz;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  x1_14.x = dot (unity_SHAr, tmpvar_12);
  x1_14.y = dot (unity_SHAg, tmpvar_12);
  x1_14.z = dot (unity_SHAb, tmpvar_12);
  mediump vec4 tmpvar_15;
  tmpvar_15 = (xlv_TEXCOORD4.xyzz * xlv_TEXCOORD4.yzzx);
  x2_13.x = dot (unity_SHBr, tmpvar_15);
  x2_13.y = dot (unity_SHBg, tmpvar_15);
  x2_13.z = dot (unity_SHBb, tmpvar_15);
  mediump vec3 tmpvar_16;
  tmpvar_16 = max (((1.055 * 
    pow ((((x2_13 + 
      (unity_SHC.xyz * ((xlv_TEXCOORD4.x * xlv_TEXCOORD4.x) - (xlv_TEXCOORD4.y * xlv_TEXCOORD4.y)))
    ) + x1_14) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_7 = ((max (
    dot (xlv_TEXCOORD4.xyz, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_9 = (((
    ((tmpvar_16 * tmpvar_6) * tmpvar_5)
   * 
    (1.0 - tmpvar_4)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_7), vec3(
    min (1.0, (tmpvar_7 * 1.5))
  ))) + ((
    ((((
      max (dot ((xlv_TEXCOORD3.xyz - (2.0 * 
        (dot (xlv_TEXCOORD4.xyz, xlv_TEXCOORD3.xyz) * xlv_TEXCOORD4.xyz)
      )), -(xlv_TEXCOORD4.xyz)), 0.25)
     * _Glossiness.w) * (10.0 * _MatGlossiness)) * _Glossiness.xyz) * tmpvar_16)
   * 
    ((tmpvar_4 * tmpvar_5) * 5.0)
  ) * tmpvar_6));
  mediump vec3 atten_17;
  lowp float shadow_18;
  shadow_18 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_19;
  tmpvar_19 = vec3(shadow_18);
  atten_17 = tmpvar_19;
  lit_color_9 = (lit_color_9 * max ((_Shadows.x + 
    (atten_17 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_20;
  tmpvar_20 = mix (lit_color_9, vec3((dot (lit_color_9, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_7 * _LightParameters.y)));
  c_8.xyz = tmpvar_20;
  c_8.w = _RimColor.w;
  lowp vec4 _color_21;
  _color_21.w = c_8.w;
  mediump float rTest_22;
  mediump vec2 tmpvar_23;
  tmpvar_23 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_24;
  tmpvar_24 = ((1.5 * _FadeColor.w) - tmpvar_23.y);
  if ((tmpvar_24 < 0.0)) {
    discard;
  };
  mediump float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (tmpvar_24 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_26;
  tmpvar_26.x = tmpvar_23.x;
  tmpvar_26.y = clamp ((tmpvar_25 - tmpvar_23.y), 0.0, 1.0);
  lowp float tmpvar_27;
  tmpvar_27 = texture2D (_RasterTex, tmpvar_26).z;
  rTest_22 = tmpvar_27;
  mediump float x_28;
  x_28 = (rTest_22 - 0.4);
  if ((x_28 < 0.0)) {
    discard;
  };
  _color_21.xyz = (c_8.xyz + (_FadeColor.xyz * tmpvar_25));
  c_8.w = _color_21.w;
  c_8.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_21.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_8;
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
	lowp vec4 albedoTex;
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
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD7;
out mediump vec4 vs_TEXCOORD8;
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
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t1.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t1;
    vs_TEXCOORD1.xy = vec2(0.0, 0.0);
    vs_TEXCOORD1.zw = in_TEXCOORD0.xy;
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
    vs_TEXCOORD2.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_5;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * t1.xyz;
    vs_TEXCOORD3.xyz = t1.xyz;
    vs_TEXCOORD3.w = 0.0;
    t1.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t1.xyz;
    t9 = dot(t1.xyz, t1.xyz);
    t9 = inversesqrt(t9);
    t1.xyz = vec3(t9) * t1.xyz;
    vs_TEXCOORD4.xyz = t1.xyz;
    vs_TEXCOORD4.w = 0.0;
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
	lowp vec4 albedoTex;
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
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _RasterTex;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD7;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
bool tb1;
mediump vec3 t16_2;
mediump vec4 t16_3;
lowp vec3 t10_4;
mediump vec3 t16_5;
lowp float t10_6;
mediump float t16_9;
mediump vec3 t16_10;
mediump float t16_21;
mediump float t16_23;
void main()
{
    t16_0.xy = vs_TEXCOORD7.xy / vs_TEXCOORD7.zz;
    t16_21 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_21<0.0;
    t16_21 = (-t16_21) * 2.5 + 1.0;
    t16_21 = clamp(t16_21, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_21;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1.x = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1.x + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD4.y * vs_TEXCOORD4.y;
    t16_0.x = vs_TEXCOORD4.x * vs_TEXCOORD4.x + (-t16_0.x);
    t16_1 = vs_TEXCOORD4.yzzx * vs_TEXCOORD4.xyzz;
    t16_2.x = dot(unity_SHBr, t16_1);
    t16_2.y = dot(unity_SHBg, t16_1);
    t16_2.z = dot(unity_SHBb, t16_1);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_2.xyz;
    t16_1.xyz = vs_TEXCOORD4.xyz;
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
    t16_2.x = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD4.xyz);
    t16_2.x = t16_2.x + t16_2.x;
    t16_2.xyz = vs_TEXCOORD4.xyz * (-t16_2.xxx) + vs_TEXCOORD3.xyz;
    t16_2.x = dot(t16_2.xyz, (-vs_TEXCOORD4.xyz));
    t16_2.x = max(t16_2.x, 0.25);
    t16_2.x = t16_2.x * _Glossiness.w;
    t16_2.x = t16_2.x * _MatGlossiness;
    t16_2.x = t16_2.x * 10.0;
    t16_2.xyz = t16_2.xxx * _Glossiness.xyz;
    t16_2.xyz = t16_0.xyz * t16_2.xyz;
    t10_1 = texture(_MaskTex, vs_TEXCOORD0.zw);
    t16_3.xy = vec2((-t10_1.w) + float(1.0), (-t10_1.z) + float(1.0));
    t16_23 = (-t16_3.x) * _LightParameters.z + 1.0;
    t16_3.x = t10_1.z * t16_23;
    t16_3.x = t16_3.x * 5.0;
    t16_2.xyz = t16_2.xyz * t16_3.xxx;
    t16_3.xzw = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t16_3.xzw = t10_1.xxx * t16_3.xzw + vec3(1.0, 1.0, 1.0);
    t10_4.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t16_3.xzw = t16_3.xzw * t10_4.xyz;
    t16_5.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t16_5.xyz = t10_1.yyy * t16_5.xyz + vec3(1.0, 1.0, 1.0);
    t16_3.xzw = t16_3.xzw * t16_5.xyz;
    t16_2.xyz = t16_2.xyz * t16_3.xzw;
    t16_0.xyz = t16_0.xyz * t16_3.xzw;
    t16_0.xyz = vec3(t16_23) * t16_0.xyz;
    t16_0.xyz = t16_3.yyy * t16_0.xyz;
    t16_23 = dot(vs_TEXCOORD4.xyz, (-_LightDir.xyz));
    t16_23 = max(t16_23, 0.0);
    t10_6 = (-_LightDir.w) + 1.0;
    t16_23 = t16_23 * _LightDir.w + t10_6;
    t16_3.x = t16_23 * 1.5;
    t16_3.x = min(t16_3.x, 1.0);
    t16_10.xyz = vec3(t16_23) + (-_CharacterShadowColor.xyz);
    t16_23 = t16_23 * _LightParameters.y;
    t16_3.xyz = t16_3.xxx * t16_10.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_3.xyz + t16_2.xyz;
    vec3 txVec28 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_2.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec28, 0.0);
    t16_9 = (-_LightShadowData.x) + 1.0;
    t16_2.x = t16_2.x * t16_9 + _LightShadowData.x;
    t16_2.x = t16_2.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_2.x = max(t16_2.x, 0.0);
    t16_0.xyz = t16_0.xyz * t16_2.xxx;
    t16_2.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_2.xyz = t16_2.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_23) * t16_2.xyz + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_21) + t16_0.xyz;
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
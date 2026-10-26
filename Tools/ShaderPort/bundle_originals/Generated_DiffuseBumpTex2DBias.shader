Shader "Generated/DiffuseBumpTex2DBias" {
Properties {
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
 _BumpMap ("Normalmap", 2D) = "bump" { }
}
SubShader { 
 LOD 299
 Tags { "QUEUE"="Geometry" "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry" "RenderType"="Opaque" }
  GpuProgramID 3057
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
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 binormal_2;
  mediump vec3 modelLightDir_3;
  highp vec4 tmpvar_4;
  mediump vec2 tmpvar_5;
  tmpvar_5.x = 0.0;
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 0.0;
  tmpvar_6.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_7;
  tmpvar_7 = (_World2Object * tmpvar_6).xyz;
  modelLightDir_3 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  binormal_2 = tmpvar_8;
  highp vec3 tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_9 = _glesTANGENT.xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = binormal_2;
  tmpvar_10 = _glesNormal;
  mediump mat3 tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_9.x;
  tmpvar_13[0].y = tmpvar_11.x;
  tmpvar_13[0].z = tmpvar_10.x;
  tmpvar_13[1].x = tmpvar_9.y;
  tmpvar_13[1].y = tmpvar_11.y;
  tmpvar_13[1].z = tmpvar_10.y;
  tmpvar_13[2].x = tmpvar_9.z;
  tmpvar_13[2].y = tmpvar_11.z;
  tmpvar_13[2].z = tmpvar_10.z;
  tmpvar_12 = tmpvar_13;
  tmpvar_4.xy = _glesColor.xy;
  tmpvar_4.zw = (_glesColor.xy + _glesColor.zw);
  tmpvar_4 = (tmpvar_4 * 0.9960938);
  tmpvar_4.zw = (tmpvar_4.zw + 0.00390625);
  highp float tmpvar_14;
  highp vec3 x_15;
  x_15 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_14 = sqrt(dot (x_15, x_15));
  fogpos_1 = tmpvar_14;
  tmpvar_5.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_5.x = (1.0 - tmpvar_5.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = (tmpvar_12 * modelLightDir_3);
  xlv_TEXCOORD5 = tmpvar_4;
  xlv_TEXCOORD6 = tmpvar_5;
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
void main ()
{
  mediump vec3 normal_1;
  highp vec4 biasuv_2;
  lowp vec4 c_3;
  c_3.w = 1.0;
  highp vec2 tmpvar_4;
  tmpvar_4 = fract(xlv_TEXCOORD0);
  biasuv_2.xy = tmpvar_4;
  biasuv_2.zw = (vec2(1.0, 1.0) - tmpvar_4);
  highp vec4 tmpvar_5;
  tmpvar_5.z = 0.0;
  tmpvar_5.xy = mix (xlv_TEXCOORD5.xy, xlv_TEXCOORD5.zw, tmpvar_4);
  tmpvar_5.w = (min (dot (
    max (vec4(0.0, 0.0, 0.0, 0.0), ((biasuv_2 - vec4(0.93, 0.93, 0.93, 0.93)) * 14.2857))
  , vec4(1.0, 1.0, 1.0, 1.0)), 1.0) * -20.0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, tmpvar_5.xy, tmpvar_5.w);
  mediump vec4 tmpvar_7;
  tmpvar_7 = tmpvar_6;
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_BumpMap, tmpvar_5.xy, tmpvar_5.w);
  mediump vec4 tmpvar_9;
  tmpvar_9 = tmpvar_8;
  lowp vec3 tmpvar_10;
  lowp vec4 packednormal_11;
  packednormal_11 = tmpvar_9;
  tmpvar_10 = ((packednormal_11.xyz * 2.0) - 1.0);
  normal_1 = tmpvar_10;
  mediump vec3 tmpvar_12;
  tmpvar_12 = normalize(xlv_TEXCOORD2);
  mediump float tmpvar_13;
  tmpvar_13 = dot (tmpvar_12, normal_1);
  c_3.xyz = tmpvar_7.xyz;
  c_3.xyz = (c_3.xyz * (1.0 + (tmpvar_13 - tmpvar_12.z)));
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_3.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_3;
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
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
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
out highp vec4 vs_TEXCOORD5;
highp vec4 t0;
highp vec3 t1;
mediump float t16_2;
mediump float t16_5;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t0.xyz = in_TANGENT0.yzx * in_NORMAL0.zxy;
    t0.xyz = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t0.xyz);
    t0.xyz = t0.xyz * in_TANGENT0.www;
    t1.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t1.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t1.xyz;
    t1.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t1.xyz;
    vs_TEXCOORD2.y = dot(t0.xyz, t1.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t1.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t1.xyz);
    t0.zw = vec2(in_COLOR0.z + in_COLOR0.x, in_COLOR0.w + in_COLOR0.y);
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_2 = (-t0.x) + acid_Fog.y;
    t16_5 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_5;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_5 = t16_2 * acid_Fog.z;
    vs_TEXCOORD6.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD6.y = t16_5;
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
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in mediump vec3 vs_TEXCOORD2;
in highp vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec4 t0;
lowp vec3 t10_0;
highp vec4 t1;
lowp vec3 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_7;
highp float t8;
void main()
{
    t0.xy = fract(vs_TEXCOORD0.xy);
    t0.zw = (-t0.xy);
    t1 = t0 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t1 = t1 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t1 = max(t1, vec4(0.0, 0.0, 0.0, 0.0));
    t8 = dot(t1, vec4(1.0, 1.0, 1.0, 1.0));
    t8 = min(t8, 1.0);
    t8 = t8 * -20.0;
    t1.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t0.xy = t0.xy * t1.xy + vs_TEXCOORD5.xy;
    t10_1.xyz = texture(_BumpMap, t0.xy, t8).xyz;
    t10_0.xyz = texture(_MainTex, t0.xy, t8).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_3.x = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_3.x = inversesqrt(t16_3.x);
    t16_7.xyz = t16_3.xxx * vs_TEXCOORD2.xyz;
    t16_7.x = dot(t16_7.xyz, t10_2.xyz);
    t16_3.x = (-vs_TEXCOORD2.z) * t16_3.x + t16_7.x;
    t16_3.x = t16_3.x + 1.0;
    t16_3.xyz = t10_0.xyz * t16_3.xxx;
    t16_3.xyz = t16_3.xyz * vs_TEXCOORD6.xxx;
    t16_3.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_3.xyz;
    SV_Target0.xyz = t16_3.xyz;
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
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 binormal_2;
  mediump vec3 modelLightDir_3;
  highp vec4 tmpvar_4;
  mediump vec2 tmpvar_5;
  tmpvar_5.x = 0.0;
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 0.0;
  tmpvar_6.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_7;
  tmpvar_7 = (_World2Object * tmpvar_6).xyz;
  modelLightDir_3 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  binormal_2 = tmpvar_8;
  highp vec3 tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_9 = _glesTANGENT.xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = binormal_2;
  tmpvar_10 = _glesNormal;
  mediump mat3 tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_9.x;
  tmpvar_13[0].y = tmpvar_11.x;
  tmpvar_13[0].z = tmpvar_10.x;
  tmpvar_13[1].x = tmpvar_9.y;
  tmpvar_13[1].y = tmpvar_11.y;
  tmpvar_13[1].z = tmpvar_10.y;
  tmpvar_13[2].x = tmpvar_9.z;
  tmpvar_13[2].y = tmpvar_11.z;
  tmpvar_13[2].z = tmpvar_10.z;
  tmpvar_12 = tmpvar_13;
  tmpvar_4.xy = _glesColor.xy;
  tmpvar_4.zw = (_glesColor.xy + _glesColor.zw);
  tmpvar_4 = (tmpvar_4 * 0.9960938);
  tmpvar_4.zw = (tmpvar_4.zw + 0.00390625);
  highp float tmpvar_14;
  highp vec3 x_15;
  x_15 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_14 = sqrt(dot (x_15, x_15));
  fogpos_1 = tmpvar_14;
  tmpvar_5.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_5.x = (1.0 - tmpvar_5.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = (tmpvar_12 * modelLightDir_3);
  xlv_TEXCOORD5 = tmpvar_4;
  xlv_TEXCOORD6 = tmpvar_5;
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
void main ()
{
  mediump vec3 normal_1;
  highp vec4 biasuv_2;
  lowp vec4 c_3;
  c_3.w = 1.0;
  highp vec2 tmpvar_4;
  tmpvar_4 = fract(xlv_TEXCOORD0);
  biasuv_2.xy = tmpvar_4;
  biasuv_2.zw = (vec2(1.0, 1.0) - tmpvar_4);
  highp vec4 tmpvar_5;
  tmpvar_5.z = 0.0;
  tmpvar_5.xy = mix (xlv_TEXCOORD5.xy, xlv_TEXCOORD5.zw, tmpvar_4);
  tmpvar_5.w = (min (dot (
    max (vec4(0.0, 0.0, 0.0, 0.0), ((biasuv_2 - vec4(0.93, 0.93, 0.93, 0.93)) * 14.2857))
  , vec4(1.0, 1.0, 1.0, 1.0)), 1.0) * -20.0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, tmpvar_5.xy, tmpvar_5.w);
  mediump vec4 tmpvar_7;
  tmpvar_7 = tmpvar_6;
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_BumpMap, tmpvar_5.xy, tmpvar_5.w);
  mediump vec4 tmpvar_9;
  tmpvar_9 = tmpvar_8;
  lowp vec3 tmpvar_10;
  lowp vec4 packednormal_11;
  packednormal_11 = tmpvar_9;
  tmpvar_10 = ((packednormal_11.xyz * 2.0) - 1.0);
  normal_1 = tmpvar_10;
  mediump vec3 tmpvar_12;
  tmpvar_12 = normalize(xlv_TEXCOORD2);
  mediump float tmpvar_13;
  tmpvar_13 = dot (tmpvar_12, normal_1);
  c_3.xyz = tmpvar_7.xyz;
  c_3.xyz = (c_3.xyz * (1.0 + (tmpvar_13 - tmpvar_12.z)));
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_3.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_3;
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
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
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
out highp vec4 vs_TEXCOORD5;
highp vec4 t0;
highp vec3 t1;
mediump float t16_2;
mediump float t16_5;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t0.xyz = in_TANGENT0.yzx * in_NORMAL0.zxy;
    t0.xyz = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t0.xyz);
    t0.xyz = t0.xyz * in_TANGENT0.www;
    t1.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t1.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t1.xyz;
    t1.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t1.xyz;
    vs_TEXCOORD2.y = dot(t0.xyz, t1.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t1.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t1.xyz);
    t0.zw = vec2(in_COLOR0.z + in_COLOR0.x, in_COLOR0.w + in_COLOR0.y);
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_2 = (-t0.x) + acid_Fog.y;
    t16_5 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_5;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_5 = t16_2 * acid_Fog.z;
    vs_TEXCOORD6.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD6.y = t16_5;
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
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in mediump vec3 vs_TEXCOORD2;
in highp vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec4 t0;
lowp vec3 t10_0;
highp vec4 t1;
lowp vec3 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_7;
highp float t8;
void main()
{
    t0.xy = fract(vs_TEXCOORD0.xy);
    t0.zw = (-t0.xy);
    t1 = t0 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t1 = t1 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t1 = max(t1, vec4(0.0, 0.0, 0.0, 0.0));
    t8 = dot(t1, vec4(1.0, 1.0, 1.0, 1.0));
    t8 = min(t8, 1.0);
    t8 = t8 * -20.0;
    t1.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t0.xy = t0.xy * t1.xy + vs_TEXCOORD5.xy;
    t10_1.xyz = texture(_BumpMap, t0.xy, t8).xyz;
    t10_0.xyz = texture(_MainTex, t0.xy, t8).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_3.x = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_3.x = inversesqrt(t16_3.x);
    t16_7.xyz = t16_3.xxx * vs_TEXCOORD2.xyz;
    t16_7.x = dot(t16_7.xyz, t10_2.xyz);
    t16_3.x = (-vs_TEXCOORD2.z) * t16_3.x + t16_7.x;
    t16_3.x = t16_3.x + 1.0;
    t16_3.xyz = t10_0.xyz * t16_3.xxx;
    t16_3.xyz = t16_3.xyz * vs_TEXCOORD6.xxx;
    t16_3.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_3.xyz;
    SV_Target0.xyz = t16_3.xyz;
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
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 binormal_2;
  mediump vec3 modelLightDir_3;
  highp vec4 tmpvar_4;
  mediump vec2 tmpvar_5;
  mediump vec4 tmpvar_6;
  tmpvar_5.x = 0.0;
  mediump vec4 tmpvar_7;
  tmpvar_7.w = 0.0;
  tmpvar_7.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (_World2Object * tmpvar_7).xyz;
  modelLightDir_3 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  binormal_2 = tmpvar_9;
  highp vec3 tmpvar_10;
  highp vec3 tmpvar_11;
  tmpvar_10 = _glesTANGENT.xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = binormal_2;
  tmpvar_11 = _glesNormal;
  mediump mat3 tmpvar_13;
  highp mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_10.x;
  tmpvar_14[0].y = tmpvar_12.x;
  tmpvar_14[0].z = tmpvar_11.x;
  tmpvar_14[1].x = tmpvar_10.y;
  tmpvar_14[1].y = tmpvar_12.y;
  tmpvar_14[1].z = tmpvar_11.y;
  tmpvar_14[2].x = tmpvar_10.z;
  tmpvar_14[2].y = tmpvar_12.z;
  tmpvar_14[2].z = tmpvar_11.z;
  tmpvar_13 = tmpvar_14;
  tmpvar_4.xy = _glesColor.xy;
  tmpvar_4.zw = (_glesColor.xy + _glesColor.zw);
  tmpvar_4 = (tmpvar_4 * 0.9960938);
  tmpvar_4.zw = (tmpvar_4.zw + 0.00390625);
  highp float tmpvar_15;
  highp vec3 x_16;
  x_16 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_15 = sqrt(dot (x_16, x_16));
  fogpos_1 = tmpvar_15;
  tmpvar_5.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_5.x = (1.0 - tmpvar_5.y);
  tmpvar_6 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = (tmpvar_13 * modelLightDir_3);
  xlv_TEXCOORD5 = tmpvar_4;
  xlv_TEXCOORD6 = tmpvar_5;
  xlv_TEXCOORD7 = tmpvar_6;
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
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  mediump vec3 normal_2;
  highp vec4 biasuv_3;
  lowp vec4 c_4;
  c_4.w = 1.0;
  highp vec2 tmpvar_5;
  tmpvar_5 = fract(xlv_TEXCOORD0);
  biasuv_3.xy = tmpvar_5;
  biasuv_3.zw = (vec2(1.0, 1.0) - tmpvar_5);
  highp vec4 tmpvar_6;
  tmpvar_6.z = 0.0;
  tmpvar_6.xy = mix (xlv_TEXCOORD5.xy, xlv_TEXCOORD5.zw, tmpvar_5);
  tmpvar_6.w = (min (dot (
    max (vec4(0.0, 0.0, 0.0, 0.0), ((biasuv_3 - vec4(0.93, 0.93, 0.93, 0.93)) * 14.2857))
  , vec4(1.0, 1.0, 1.0, 1.0)), 1.0) * -20.0);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, tmpvar_6.xy, tmpvar_6.w);
  mediump vec4 tmpvar_8;
  tmpvar_8 = tmpvar_7;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_BumpMap, tmpvar_6.xy, tmpvar_6.w);
  mediump vec4 tmpvar_10;
  tmpvar_10 = tmpvar_9;
  lowp vec3 tmpvar_11;
  lowp vec4 packednormal_12;
  packednormal_12 = tmpvar_10;
  tmpvar_11 = ((packednormal_12.xyz * 2.0) - 1.0);
  normal_2 = tmpvar_11;
  mediump vec3 tmpvar_13;
  tmpvar_13 = normalize(xlv_TEXCOORD2);
  mediump float tmpvar_14;
  tmpvar_14 = dot (tmpvar_13, normal_2);
  c_4.xyz = tmpvar_8.xyz;
  lowp float tmpvar_15;
  highp float tmpvar_16;
  tmpvar_16 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD7).x > 
    (xlv_TEXCOORD7.z / xlv_TEXCOORD7.w)
  )), _LightShadowData.x);
  tmpvar_15 = tmpvar_16;
  tmpvar_1 = ((_EnvAmbientColor.xyz * (1.0 - tmpvar_15)) + tmpvar_15);
  c_4.xyz = (c_4.xyz * tmpvar_1);
  c_4.xyz = (c_4.xyz * (1.0 + (tmpvar_14 - tmpvar_13.z)));
  c_4.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_4.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_4;
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
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 binormal_2;
  mediump vec3 modelLightDir_3;
  highp vec4 tmpvar_4;
  mediump vec2 tmpvar_5;
  mediump vec4 tmpvar_6;
  tmpvar_5.x = 0.0;
  mediump vec4 tmpvar_7;
  tmpvar_7.w = 0.0;
  tmpvar_7.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (_World2Object * tmpvar_7).xyz;
  modelLightDir_3 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  binormal_2 = tmpvar_9;
  highp vec3 tmpvar_10;
  highp vec3 tmpvar_11;
  tmpvar_10 = _glesTANGENT.xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = binormal_2;
  tmpvar_11 = _glesNormal;
  mediump mat3 tmpvar_13;
  highp mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_10.x;
  tmpvar_14[0].y = tmpvar_12.x;
  tmpvar_14[0].z = tmpvar_11.x;
  tmpvar_14[1].x = tmpvar_10.y;
  tmpvar_14[1].y = tmpvar_12.y;
  tmpvar_14[1].z = tmpvar_11.y;
  tmpvar_14[2].x = tmpvar_10.z;
  tmpvar_14[2].y = tmpvar_12.z;
  tmpvar_14[2].z = tmpvar_11.z;
  tmpvar_13 = tmpvar_14;
  tmpvar_4.xy = _glesColor.xy;
  tmpvar_4.zw = (_glesColor.xy + _glesColor.zw);
  tmpvar_4 = (tmpvar_4 * 0.9960938);
  tmpvar_4.zw = (tmpvar_4.zw + 0.00390625);
  highp float tmpvar_15;
  highp vec3 x_16;
  x_16 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_15 = sqrt(dot (x_16, x_16));
  fogpos_1 = tmpvar_15;
  tmpvar_5.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_5.x = (1.0 - tmpvar_5.y);
  tmpvar_6 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = (tmpvar_13 * modelLightDir_3);
  xlv_TEXCOORD5 = tmpvar_4;
  xlv_TEXCOORD6 = tmpvar_5;
  xlv_TEXCOORD7 = tmpvar_6;
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
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  mediump vec3 normal_2;
  highp vec4 biasuv_3;
  lowp vec4 c_4;
  c_4.w = 1.0;
  highp vec2 tmpvar_5;
  tmpvar_5 = fract(xlv_TEXCOORD0);
  biasuv_3.xy = tmpvar_5;
  biasuv_3.zw = (vec2(1.0, 1.0) - tmpvar_5);
  highp vec4 tmpvar_6;
  tmpvar_6.z = 0.0;
  tmpvar_6.xy = mix (xlv_TEXCOORD5.xy, xlv_TEXCOORD5.zw, tmpvar_5);
  tmpvar_6.w = (min (dot (
    max (vec4(0.0, 0.0, 0.0, 0.0), ((biasuv_3 - vec4(0.93, 0.93, 0.93, 0.93)) * 14.2857))
  , vec4(1.0, 1.0, 1.0, 1.0)), 1.0) * -20.0);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_MainTex, tmpvar_6.xy, tmpvar_6.w);
  mediump vec4 tmpvar_8;
  tmpvar_8 = tmpvar_7;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_BumpMap, tmpvar_6.xy, tmpvar_6.w);
  mediump vec4 tmpvar_10;
  tmpvar_10 = tmpvar_9;
  lowp vec3 tmpvar_11;
  lowp vec4 packednormal_12;
  packednormal_12 = tmpvar_10;
  tmpvar_11 = ((packednormal_12.xyz * 2.0) - 1.0);
  normal_2 = tmpvar_11;
  mediump vec3 tmpvar_13;
  tmpvar_13 = normalize(xlv_TEXCOORD2);
  mediump float tmpvar_14;
  tmpvar_14 = dot (tmpvar_13, normal_2);
  c_4.xyz = tmpvar_8.xyz;
  lowp float shadow_15;
  shadow_15 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD7.xyz) * (1.0 - _LightShadowData.x)));
  tmpvar_1 = ((_EnvAmbientColor.xyz * (1.0 - shadow_15)) + shadow_15);
  c_4.xyz = (c_4.xyz * tmpvar_1);
  c_4.xyz = (c_4.xyz * (1.0 + (tmpvar_14 - tmpvar_13.z)));
  c_4.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_4.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_4;
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
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
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
out highp vec4 vs_TEXCOORD5;
out mediump vec4 vs_TEXCOORD7;
highp vec4 t0;
highp vec4 t1;
mediump float t16_2;
mediump float t16_5;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t0.xyz = in_TANGENT0.yzx * in_NORMAL0.zxy;
    t0.xyz = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t0.xyz);
    t0.xyz = t0.xyz * in_TANGENT0.www;
    t1.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t1.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t1.xyz;
    t1.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t1.xyz;
    vs_TEXCOORD2.y = dot(t0.xyz, t1.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t1.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t1.xyz);
    t0.zw = vec2(in_COLOR0.z + in_COLOR0.x, in_COLOR0.w + in_COLOR0.y);
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_2 = (-t0.x) + acid_Fog.y;
    t16_5 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_5;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_5 = t16_2 * acid_Fog.z;
    vs_TEXCOORD6.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD6.y = t16_5;
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
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
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
in highp vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD7;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec4 t0;
lowp vec3 t10_0;
highp vec4 t1;
mediump vec3 t16_1;
lowp vec3 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_7;
highp float t8;
mediump float t16_11;
mediump float t16_12;
void main()
{
    t0.xy = fract(vs_TEXCOORD0.xy);
    t0.zw = (-t0.xy);
    t1 = t0 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t1 = t1 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t1 = max(t1, vec4(0.0, 0.0, 0.0, 0.0));
    t8 = dot(t1, vec4(1.0, 1.0, 1.0, 1.0));
    t8 = min(t8, 1.0);
    t8 = t8 * -20.0;
    t1.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t0.xy = t0.xy * t1.xy + vs_TEXCOORD5.xy;
    t10_1.xyz = texture(_BumpMap, t0.xy, t8).xyz;
    t10_0.xyz = texture(_MainTex, t0.xy, t8).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_3.x = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_3.x = inversesqrt(t16_3.x);
    t16_7.xyz = t16_3.xxx * vs_TEXCOORD2.xyz;
    t16_7.x = dot(t16_7.xyz, t10_2.xyz);
    t16_3.x = (-vs_TEXCOORD2.z) * t16_3.x + t16_7.x;
    t16_3.x = t16_3.x + 1.0;
    vec3 txVec10 = vec3(vs_TEXCOORD7.xy,vs_TEXCOORD7.z);
    t16_7.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec10, 0.0);
    t16_11 = (-_LightShadowData.x) + 1.0;
    t16_7.x = t16_7.x * t16_11 + _LightShadowData.x;
    t16_12 = (-t16_7.x) + 1.0;
    t16_1.xyz = _EnvAmbientColor.xyz * vec3(t16_12) + t16_7.xxx;
    t16_7.xyz = t10_0.xyz * t16_1.xyz;
    t16_3.xyz = t16_3.xxx * t16_7.xyz;
    t16_3.xyz = t16_3.xyz * vs_TEXCOORD6.xxx;
    t16_3.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_3.xyz;
    SV_Target0.xyz = t16_3.xyz;
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
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 binormal_2;
  mediump vec3 modelLightDir_3;
  highp vec4 tmpvar_4;
  mediump vec2 tmpvar_5;
  tmpvar_5.x = 0.0;
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 0.0;
  tmpvar_6.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_7;
  tmpvar_7 = (_World2Object * tmpvar_6).xyz;
  modelLightDir_3 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  binormal_2 = tmpvar_8;
  highp vec3 tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_9 = _glesTANGENT.xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = binormal_2;
  tmpvar_10 = _glesNormal;
  mediump mat3 tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_9.x;
  tmpvar_13[0].y = tmpvar_11.x;
  tmpvar_13[0].z = tmpvar_10.x;
  tmpvar_13[1].x = tmpvar_9.y;
  tmpvar_13[1].y = tmpvar_11.y;
  tmpvar_13[1].z = tmpvar_10.y;
  tmpvar_13[2].x = tmpvar_9.z;
  tmpvar_13[2].y = tmpvar_11.z;
  tmpvar_13[2].z = tmpvar_10.z;
  tmpvar_12 = tmpvar_13;
  tmpvar_4.xy = _glesColor.xy;
  tmpvar_4.zw = (_glesColor.xy + _glesColor.zw);
  tmpvar_4 = (tmpvar_4 * 0.9960938);
  tmpvar_4.zw = (tmpvar_4.zw + 0.00390625);
  highp float tmpvar_14;
  highp vec3 x_15;
  x_15 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_14 = sqrt(dot (x_15, x_15));
  fogpos_1 = tmpvar_14;
  tmpvar_5.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_5.x = (1.0 - tmpvar_5.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = (tmpvar_12 * modelLightDir_3);
  xlv_TEXCOORD5 = tmpvar_4;
  xlv_TEXCOORD6 = tmpvar_5;
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
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  mediump vec3 normal_1;
  highp vec4 biasuv_2;
  lowp vec4 c_3;
  c_3.w = 1.0;
  highp vec2 tmpvar_4;
  tmpvar_4 = fract(xlv_TEXCOORD0);
  biasuv_2.xy = tmpvar_4;
  biasuv_2.zw = (vec2(1.0, 1.0) - tmpvar_4);
  highp vec4 tmpvar_5;
  tmpvar_5.z = 0.0;
  tmpvar_5.xy = mix (xlv_TEXCOORD5.xy, xlv_TEXCOORD5.zw, tmpvar_4);
  tmpvar_5.w = (min (dot (
    max (vec4(0.0, 0.0, 0.0, 0.0), ((biasuv_2 - vec4(0.93, 0.93, 0.93, 0.93)) * 14.2857))
  , vec4(1.0, 1.0, 1.0, 1.0)), 1.0) * -20.0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, tmpvar_5.xy, tmpvar_5.w);
  mediump vec4 tmpvar_7;
  tmpvar_7 = tmpvar_6;
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_BumpMap, tmpvar_5.xy, tmpvar_5.w);
  mediump vec4 tmpvar_9;
  tmpvar_9 = tmpvar_8;
  lowp vec3 tmpvar_10;
  lowp vec4 packednormal_11;
  packednormal_11 = tmpvar_9;
  tmpvar_10 = ((packednormal_11.xyz * 2.0) - 1.0);
  normal_1 = tmpvar_10;
  mediump vec3 tmpvar_12;
  tmpvar_12 = normalize(xlv_TEXCOORD2);
  mediump float tmpvar_13;
  tmpvar_13 = dot (tmpvar_12, normal_1);
  c_3.xyz = tmpvar_7.xyz;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_15;
  tmpvar_15 = (2.0 * tmpvar_14.xyz);
  c_3.xyz = (c_3.xyz * tmpvar_15);
  c_3.xyz = (c_3.xyz * (1.0 + (tmpvar_13 - tmpvar_12.z)));
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_3.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_3;
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
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
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
out highp vec4 vs_TEXCOORD5;
out highp vec2 vs_TEXCOORD8;
highp vec4 t0;
highp vec3 t1;
mediump float t16_2;
mediump float t16_5;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t0.xyz = in_TANGENT0.yzx * in_NORMAL0.zxy;
    t0.xyz = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t0.xyz);
    t0.xyz = t0.xyz * in_TANGENT0.www;
    t1.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t1.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t1.xyz;
    t1.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t1.xyz;
    vs_TEXCOORD2.y = dot(t0.xyz, t1.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t1.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t1.xyz);
    t0.zw = vec2(in_COLOR0.z + in_COLOR0.x, in_COLOR0.w + in_COLOR0.y);
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_2 = (-t0.x) + acid_Fog.y;
    t16_5 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_5;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_5 = t16_2 * acid_Fog.z;
    vs_TEXCOORD6.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD6.y = t16_5;
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
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
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
in highp vec4 vs_TEXCOORD5;
in highp vec2 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec4 t0;
lowp vec3 t10_0;
highp vec4 t1;
lowp vec3 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_7;
highp float t8;
void main()
{
    t0.xy = fract(vs_TEXCOORD0.xy);
    t0.zw = (-t0.xy);
    t1 = t0 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t1 = t1 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t1 = max(t1, vec4(0.0, 0.0, 0.0, 0.0));
    t8 = dot(t1, vec4(1.0, 1.0, 1.0, 1.0));
    t8 = min(t8, 1.0);
    t8 = t8 * -20.0;
    t1.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t0.xy = t0.xy * t1.xy + vs_TEXCOORD5.xy;
    t10_1.xyz = texture(_BumpMap, t0.xy, t8).xyz;
    t10_0.xyz = texture(_MainTex, t0.xy, t8).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_3.x = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_3.x = inversesqrt(t16_3.x);
    t16_7.xyz = t16_3.xxx * vs_TEXCOORD2.xyz;
    t16_7.x = dot(t16_7.xyz, t10_2.xyz);
    t16_3.x = (-vs_TEXCOORD2.z) * t16_3.x + t16_7.x;
    t16_3.x = t16_3.x + 1.0;
    t10_1.xyz = texture(unity_Lightmap, vs_TEXCOORD8.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0);
    t16_7.xyz = t10_0.xyz * t10_2.xyz;
    t16_3.xyz = t16_3.xxx * t16_7.xyz;
    t16_3.xyz = t16_3.xyz * vs_TEXCOORD6.xxx;
    t16_3.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_3.xyz;
    SV_Target0.xyz = t16_3.xyz;
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
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 binormal_2;
  mediump vec3 modelLightDir_3;
  highp vec4 tmpvar_4;
  mediump vec2 tmpvar_5;
  tmpvar_5.x = 0.0;
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 0.0;
  tmpvar_6.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_7;
  tmpvar_7 = (_World2Object * tmpvar_6).xyz;
  modelLightDir_3 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  binormal_2 = tmpvar_8;
  highp vec3 tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_9 = _glesTANGENT.xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = binormal_2;
  tmpvar_10 = _glesNormal;
  mediump mat3 tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_9.x;
  tmpvar_13[0].y = tmpvar_11.x;
  tmpvar_13[0].z = tmpvar_10.x;
  tmpvar_13[1].x = tmpvar_9.y;
  tmpvar_13[1].y = tmpvar_11.y;
  tmpvar_13[1].z = tmpvar_10.y;
  tmpvar_13[2].x = tmpvar_9.z;
  tmpvar_13[2].y = tmpvar_11.z;
  tmpvar_13[2].z = tmpvar_10.z;
  tmpvar_12 = tmpvar_13;
  tmpvar_4.xy = _glesColor.xy;
  tmpvar_4.zw = (_glesColor.xy + _glesColor.zw);
  tmpvar_4 = (tmpvar_4 * 0.9960938);
  tmpvar_4.zw = (tmpvar_4.zw + 0.00390625);
  highp float tmpvar_14;
  highp vec3 x_15;
  x_15 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_14 = sqrt(dot (x_15, x_15));
  fogpos_1 = tmpvar_14;
  tmpvar_5.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_5.x = (1.0 - tmpvar_5.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = (tmpvar_12 * modelLightDir_3);
  xlv_TEXCOORD5 = tmpvar_4;
  xlv_TEXCOORD6 = tmpvar_5;
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
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  mediump vec3 normal_1;
  highp vec4 biasuv_2;
  lowp vec4 c_3;
  c_3.w = 1.0;
  highp vec2 tmpvar_4;
  tmpvar_4 = fract(xlv_TEXCOORD0);
  biasuv_2.xy = tmpvar_4;
  biasuv_2.zw = (vec2(1.0, 1.0) - tmpvar_4);
  highp vec4 tmpvar_5;
  tmpvar_5.z = 0.0;
  tmpvar_5.xy = mix (xlv_TEXCOORD5.xy, xlv_TEXCOORD5.zw, tmpvar_4);
  tmpvar_5.w = (min (dot (
    max (vec4(0.0, 0.0, 0.0, 0.0), ((biasuv_2 - vec4(0.93, 0.93, 0.93, 0.93)) * 14.2857))
  , vec4(1.0, 1.0, 1.0, 1.0)), 1.0) * -20.0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, tmpvar_5.xy, tmpvar_5.w);
  mediump vec4 tmpvar_7;
  tmpvar_7 = tmpvar_6;
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_BumpMap, tmpvar_5.xy, tmpvar_5.w);
  mediump vec4 tmpvar_9;
  tmpvar_9 = tmpvar_8;
  lowp vec3 tmpvar_10;
  lowp vec4 packednormal_11;
  packednormal_11 = tmpvar_9;
  tmpvar_10 = ((packednormal_11.xyz * 2.0) - 1.0);
  normal_1 = tmpvar_10;
  mediump vec3 tmpvar_12;
  tmpvar_12 = normalize(xlv_TEXCOORD2);
  mediump float tmpvar_13;
  tmpvar_13 = dot (tmpvar_12, normal_1);
  c_3.xyz = tmpvar_7.xyz;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_15;
  tmpvar_15 = (2.0 * tmpvar_14.xyz);
  c_3.xyz = (c_3.xyz * tmpvar_15);
  c_3.xyz = (c_3.xyz * (1.0 + (tmpvar_13 - tmpvar_12.z)));
  c_3.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_3.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_3;
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
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
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
out highp vec4 vs_TEXCOORD5;
out highp vec2 vs_TEXCOORD8;
highp vec4 t0;
highp vec3 t1;
mediump float t16_2;
mediump float t16_5;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t0.xyz = in_TANGENT0.yzx * in_NORMAL0.zxy;
    t0.xyz = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t0.xyz);
    t0.xyz = t0.xyz * in_TANGENT0.www;
    t1.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t1.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t1.xyz;
    t1.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t1.xyz;
    vs_TEXCOORD2.y = dot(t0.xyz, t1.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t1.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t1.xyz);
    t0.zw = vec2(in_COLOR0.z + in_COLOR0.x, in_COLOR0.w + in_COLOR0.y);
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_2 = (-t0.x) + acid_Fog.y;
    t16_5 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_5;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_5 = t16_2 * acid_Fog.z;
    vs_TEXCOORD6.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD6.y = t16_5;
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
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
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
in highp vec4 vs_TEXCOORD5;
in highp vec2 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec4 t0;
lowp vec3 t10_0;
highp vec4 t1;
lowp vec3 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_7;
highp float t8;
void main()
{
    t0.xy = fract(vs_TEXCOORD0.xy);
    t0.zw = (-t0.xy);
    t1 = t0 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t1 = t1 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t1 = max(t1, vec4(0.0, 0.0, 0.0, 0.0));
    t8 = dot(t1, vec4(1.0, 1.0, 1.0, 1.0));
    t8 = min(t8, 1.0);
    t8 = t8 * -20.0;
    t1.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t0.xy = t0.xy * t1.xy + vs_TEXCOORD5.xy;
    t10_1.xyz = texture(_BumpMap, t0.xy, t8).xyz;
    t10_0.xyz = texture(_MainTex, t0.xy, t8).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_3.x = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_3.x = inversesqrt(t16_3.x);
    t16_7.xyz = t16_3.xxx * vs_TEXCOORD2.xyz;
    t16_7.x = dot(t16_7.xyz, t10_2.xyz);
    t16_3.x = (-vs_TEXCOORD2.z) * t16_3.x + t16_7.x;
    t16_3.x = t16_3.x + 1.0;
    t10_1.xyz = texture(unity_Lightmap, vs_TEXCOORD8.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0);
    t16_7.xyz = t10_0.xyz * t10_2.xyz;
    t16_3.xyz = t16_3.xxx * t16_7.xyz;
    t16_3.xyz = t16_3.xyz * vs_TEXCOORD6.xxx;
    t16_3.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_3.xyz;
    SV_Target0.xyz = t16_3.xyz;
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
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 binormal_2;
  mediump vec3 modelLightDir_3;
  highp vec4 tmpvar_4;
  mediump vec2 tmpvar_5;
  mediump vec4 tmpvar_6;
  tmpvar_5.x = 0.0;
  mediump vec4 tmpvar_7;
  tmpvar_7.w = 0.0;
  tmpvar_7.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (_World2Object * tmpvar_7).xyz;
  modelLightDir_3 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  binormal_2 = tmpvar_9;
  highp vec3 tmpvar_10;
  highp vec3 tmpvar_11;
  tmpvar_10 = _glesTANGENT.xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = binormal_2;
  tmpvar_11 = _glesNormal;
  mediump mat3 tmpvar_13;
  highp mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_10.x;
  tmpvar_14[0].y = tmpvar_12.x;
  tmpvar_14[0].z = tmpvar_11.x;
  tmpvar_14[1].x = tmpvar_10.y;
  tmpvar_14[1].y = tmpvar_12.y;
  tmpvar_14[1].z = tmpvar_11.y;
  tmpvar_14[2].x = tmpvar_10.z;
  tmpvar_14[2].y = tmpvar_12.z;
  tmpvar_14[2].z = tmpvar_11.z;
  tmpvar_13 = tmpvar_14;
  tmpvar_4.xy = _glesColor.xy;
  tmpvar_4.zw = (_glesColor.xy + _glesColor.zw);
  tmpvar_4 = (tmpvar_4 * 0.9960938);
  tmpvar_4.zw = (tmpvar_4.zw + 0.00390625);
  highp float tmpvar_15;
  highp vec3 x_16;
  x_16 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_15 = sqrt(dot (x_16, x_16));
  fogpos_1 = tmpvar_15;
  tmpvar_5.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_5.x = (1.0 - tmpvar_5.y);
  tmpvar_6 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = (tmpvar_13 * modelLightDir_3);
  xlv_TEXCOORD5 = tmpvar_4;
  xlv_TEXCOORD6 = tmpvar_5;
  xlv_TEXCOORD7 = tmpvar_6;
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
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  lowp vec3 lm_2;
  mediump vec3 normal_3;
  highp vec4 biasuv_4;
  lowp vec4 c_5;
  c_5.w = 1.0;
  highp vec2 tmpvar_6;
  tmpvar_6 = fract(xlv_TEXCOORD0);
  biasuv_4.xy = tmpvar_6;
  biasuv_4.zw = (vec2(1.0, 1.0) - tmpvar_6);
  highp vec4 tmpvar_7;
  tmpvar_7.z = 0.0;
  tmpvar_7.xy = mix (xlv_TEXCOORD5.xy, xlv_TEXCOORD5.zw, tmpvar_6);
  tmpvar_7.w = (min (dot (
    max (vec4(0.0, 0.0, 0.0, 0.0), ((biasuv_4 - vec4(0.93, 0.93, 0.93, 0.93)) * 14.2857))
  , vec4(1.0, 1.0, 1.0, 1.0)), 1.0) * -20.0);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, tmpvar_7.xy, tmpvar_7.w);
  mediump vec4 tmpvar_9;
  tmpvar_9 = tmpvar_8;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_BumpMap, tmpvar_7.xy, tmpvar_7.w);
  mediump vec4 tmpvar_11;
  tmpvar_11 = tmpvar_10;
  lowp vec3 tmpvar_12;
  lowp vec4 packednormal_13;
  packednormal_13 = tmpvar_11;
  tmpvar_12 = ((packednormal_13.xyz * 2.0) - 1.0);
  normal_3 = tmpvar_12;
  mediump vec3 tmpvar_14;
  tmpvar_14 = normalize(xlv_TEXCOORD2);
  mediump float tmpvar_15;
  tmpvar_15 = dot (tmpvar_14, normal_3);
  c_5.xyz = tmpvar_9.xyz;
  lowp float tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD7).x > 
    (xlv_TEXCOORD7.z / xlv_TEXCOORD7.w)
  )), _LightShadowData.x);
  tmpvar_16 = tmpvar_17;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_19;
  tmpvar_19 = (2.0 * tmpvar_18.xyz);
  lm_2 = tmpvar_19;
  lowp vec3 tmpvar_20;
  tmpvar_20 = min (lm_2, mix (_EnvAmbientColor.xyz, lm_2, vec3(tmpvar_16)));
  tmpvar_1 = tmpvar_20;
  c_5.xyz = (c_5.xyz * tmpvar_1);
  c_5.xyz = (c_5.xyz * (1.0 + (tmpvar_15 - tmpvar_14.z)));
  c_5.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_5.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_5;
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
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  mediump float fogpos_1;
  mediump vec3 binormal_2;
  mediump vec3 modelLightDir_3;
  highp vec4 tmpvar_4;
  mediump vec2 tmpvar_5;
  mediump vec4 tmpvar_6;
  tmpvar_5.x = 0.0;
  mediump vec4 tmpvar_7;
  tmpvar_7.w = 0.0;
  tmpvar_7.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (_World2Object * tmpvar_7).xyz;
  modelLightDir_3 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  binormal_2 = tmpvar_9;
  highp vec3 tmpvar_10;
  highp vec3 tmpvar_11;
  tmpvar_10 = _glesTANGENT.xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = binormal_2;
  tmpvar_11 = _glesNormal;
  mediump mat3 tmpvar_13;
  highp mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_10.x;
  tmpvar_14[0].y = tmpvar_12.x;
  tmpvar_14[0].z = tmpvar_11.x;
  tmpvar_14[1].x = tmpvar_10.y;
  tmpvar_14[1].y = tmpvar_12.y;
  tmpvar_14[1].z = tmpvar_11.y;
  tmpvar_14[2].x = tmpvar_10.z;
  tmpvar_14[2].y = tmpvar_12.z;
  tmpvar_14[2].z = tmpvar_11.z;
  tmpvar_13 = tmpvar_14;
  tmpvar_4.xy = _glesColor.xy;
  tmpvar_4.zw = (_glesColor.xy + _glesColor.zw);
  tmpvar_4 = (tmpvar_4 * 0.9960938);
  tmpvar_4.zw = (tmpvar_4.zw + 0.00390625);
  highp float tmpvar_15;
  highp vec3 x_16;
  x_16 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_15 = sqrt(dot (x_16, x_16));
  fogpos_1 = tmpvar_15;
  tmpvar_5.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_5.x = (1.0 - tmpvar_5.y);
  tmpvar_6 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = (tmpvar_13 * modelLightDir_3);
  xlv_TEXCOORD5 = tmpvar_4;
  xlv_TEXCOORD6 = tmpvar_5;
  xlv_TEXCOORD7 = tmpvar_6;
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
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  lowp vec3 lm_2;
  mediump vec3 normal_3;
  highp vec4 biasuv_4;
  lowp vec4 c_5;
  c_5.w = 1.0;
  highp vec2 tmpvar_6;
  tmpvar_6 = fract(xlv_TEXCOORD0);
  biasuv_4.xy = tmpvar_6;
  biasuv_4.zw = (vec2(1.0, 1.0) - tmpvar_6);
  highp vec4 tmpvar_7;
  tmpvar_7.z = 0.0;
  tmpvar_7.xy = mix (xlv_TEXCOORD5.xy, xlv_TEXCOORD5.zw, tmpvar_6);
  tmpvar_7.w = (min (dot (
    max (vec4(0.0, 0.0, 0.0, 0.0), ((biasuv_4 - vec4(0.93, 0.93, 0.93, 0.93)) * 14.2857))
  , vec4(1.0, 1.0, 1.0, 1.0)), 1.0) * -20.0);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, tmpvar_7.xy, tmpvar_7.w);
  mediump vec4 tmpvar_9;
  tmpvar_9 = tmpvar_8;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_BumpMap, tmpvar_7.xy, tmpvar_7.w);
  mediump vec4 tmpvar_11;
  tmpvar_11 = tmpvar_10;
  lowp vec3 tmpvar_12;
  lowp vec4 packednormal_13;
  packednormal_13 = tmpvar_11;
  tmpvar_12 = ((packednormal_13.xyz * 2.0) - 1.0);
  normal_3 = tmpvar_12;
  mediump vec3 tmpvar_14;
  tmpvar_14 = normalize(xlv_TEXCOORD2);
  mediump float tmpvar_15;
  tmpvar_15 = dot (tmpvar_14, normal_3);
  c_5.xyz = tmpvar_9.xyz;
  lowp float shadow_16;
  shadow_16 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD7.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_18;
  tmpvar_18 = (2.0 * tmpvar_17.xyz);
  lm_2 = tmpvar_18;
  lowp vec3 tmpvar_19;
  tmpvar_19 = min (lm_2, mix (_EnvAmbientColor.xyz, lm_2, vec3(shadow_16)));
  tmpvar_1 = tmpvar_19;
  c_5.xyz = (c_5.xyz * tmpvar_1);
  c_5.xyz = (c_5.xyz * (1.0 + (tmpvar_15 - tmpvar_14.z)));
  c_5.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_5.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_5;
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
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
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
out highp vec4 vs_TEXCOORD5;
out mediump vec4 vs_TEXCOORD7;
out highp vec2 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_2;
mediump float t16_5;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t0.xyz = in_TANGENT0.yzx * in_NORMAL0.zxy;
    t0.xyz = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t0.xyz);
    t0.xyz = t0.xyz * in_TANGENT0.www;
    t1.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t1.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t1.xyz;
    t1.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t1.xyz;
    vs_TEXCOORD2.y = dot(t0.xyz, t1.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t1.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t1.xyz);
    t0.zw = vec2(in_COLOR0.z + in_COLOR0.x, in_COLOR0.w + in_COLOR0.y);
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_2 = (-t0.x) + acid_Fog.y;
    t16_5 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_5;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_5 = t16_2 * acid_Fog.z;
    vs_TEXCOORD6.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD6.y = t16_5;
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
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
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
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
in highp vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD7;
in highp vec2 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec4 t0;
mediump float t16_0;
highp vec4 t1;
lowp vec3 t10_1;
lowp vec3 t10_2;
lowp vec3 t10_3;
lowp vec3 t10_4;
mediump vec3 t16_5;
mediump vec3 t16_6;
mediump float t16_7;
highp vec2 t8;
mediump float t16_26;
void main()
{
    vec3 txVec2 = vec3(vs_TEXCOORD7.xy,vs_TEXCOORD7.z);
    t16_0 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec2, 0.0);
    t16_7 = (-_LightShadowData.x) + 1.0;
    t16_0 = t16_0 * t16_7 + _LightShadowData.x;
    t10_1.xyz = texture(unity_Lightmap, vs_TEXCOORD8.xy).xyz;
    t10_2.xyz = vec3(2.0, 2.0, 2.0) * t10_1.xyz + (-_EnvAmbientColor.xyz);
    t10_3.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0);
    t10_2.xyz = vec3(t16_0) * t10_2.xyz + _EnvAmbientColor.xyz;
    t10_2.xyz = min(t10_2.xyz, t10_3.xyz);
    t0.xy = fract(vs_TEXCOORD0.xy);
    t0.zw = (-t0.xy);
    t1 = t0 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t1 = t1 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t1 = max(t1, vec4(0.0, 0.0, 0.0, 0.0));
    t1.x = dot(t1, vec4(1.0, 1.0, 1.0, 1.0));
    t1.x = min(t1.x, 1.0);
    t1.x = t1.x * -20.0;
    t8.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t8.xy = t0.xy * t8.xy + vs_TEXCOORD5.xy;
    t10_4.xyz = texture(_MainTex, t8.xy, t1.x).xyz;
    t10_1.xyz = texture(_BumpMap, t8.xy, t1.x).xyz;
    t10_3.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_5.xyz = t10_2.xyz * t10_4.xyz;
    t16_26 = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_26 = inversesqrt(t16_26);
    t16_6.xyz = vec3(t16_26) * vs_TEXCOORD2.xyz;
    t16_6.x = dot(t16_6.xyz, t10_3.xyz);
    t16_26 = (-vs_TEXCOORD2.z) * t16_26 + t16_6.x;
    t16_26 = t16_26 + 1.0;
    t16_5.xyz = vec3(t16_26) * t16_5.xyz;
    t16_5.xyz = t16_5.xyz * vs_TEXCOORD6.xxx;
    t16_5.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_5.xyz;
    SV_Target0.xyz = t16_5.xyz;
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
Fallback "Generated/DiffuseTex2DLOD"
}
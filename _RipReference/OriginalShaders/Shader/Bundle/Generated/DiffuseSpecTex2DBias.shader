Shader "Generated/DiffuseSpecTex2DBias" {
Properties {
 _MainTex ("Albedo (RGB)", 2D) = "white" { }
 _SpecTex ("Gloss (RGB)", 2D) = "white" { }
 _Shininess ("Shininess", Range(1,128)) = 10
}
SubShader { 
 LOD 199
 Tags { "QUEUE"="Geometry" "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry" "RenderType"="Opaque" }
  GpuProgramID 8585
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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _World2Object;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  mediump float fogpos_2;
  mediump vec3 modelViewDir_3;
  mediump vec3 modelLightDir_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  mediump vec2 tmpvar_7;
  tmpvar_7.x = 0.0;
  tmpvar_5 = tmpvar_1;
  mediump vec4 tmpvar_8;
  tmpvar_8.w = 0.0;
  tmpvar_8.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_9;
  tmpvar_9 = (_World2Object * tmpvar_8).xyz;
  modelLightDir_4 = tmpvar_9;
  highp vec3 tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11.w = 1.0;
  tmpvar_11.xyz = _WorldSpaceCameraPos;
  tmpvar_10 = ((_World2Object * tmpvar_11).xyz - _glesVertex.xyz);
  modelViewDir_3 = tmpvar_10;
  tmpvar_6.xy = _glesColor.xy;
  tmpvar_6.zw = (_glesColor.xy + _glesColor.zw);
  tmpvar_6 = (tmpvar_6 * 0.9960938);
  tmpvar_6.zw = (tmpvar_6.zw + 0.00390625);
  highp float tmpvar_12;
  highp vec3 x_13;
  x_13 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_12 = sqrt(dot (x_13, x_13));
  fogpos_2 = tmpvar_12;
  tmpvar_7.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_7.x = (1.0 - tmpvar_7.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = modelLightDir_4;
  xlv_TEXCOORD3 = modelViewDir_3;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _SpecTex;
uniform mediump float _Shininess;
uniform lowp vec4 _LightColor0;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
void main ()
{
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
  tmpvar_8 = texture2D (_SpecTex, tmpvar_5.xy, tmpvar_5.w);
  mediump vec4 tmpvar_9;
  tmpvar_9 = tmpvar_8;
  c_3.xyz = tmpvar_7.xyz;
  tmpvar_1 = ((tmpvar_9.xyz * _LightColor0.xyz) * pow (max (0.0, 
    dot (normalize(xlv_TEXCOORD4), normalize((normalize(xlv_TEXCOORD3) + normalize(xlv_TEXCOORD2))))
  ), _Shininess));
  c_3.xyz = (c_3.xyz + tmpvar_1);
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
uniform 	mediump float _Shininess;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	lowp vec4 _LightColor0;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD2;
out mediump vec3 vs_TEXCOORD3;
out mediump vec3 vs_TEXCOORD4;
out highp vec4 vs_TEXCOORD5;
highp vec4 t0;
mediump float t16_1;
mediump float t16_3;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t0.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t0.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t0.xyz;
    t0.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t0.xyz;
    vs_TEXCOORD2.xyz = t0.xyz;
    t0.xyz = _WorldSpaceCameraPos.xyzx.yyy * _World2Object[1].xyz;
    t0.xyz = _World2Object[0].xyz * _WorldSpaceCameraPos.xyzx.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WorldSpaceCameraPos.xyzx.zzz + t0.xyz;
    t0.xyz = t0.xyz + _World2Object[3].xyz;
    t0.xyz = t0.xyz + (-in_POSITION0.xyz);
    vs_TEXCOORD3.xyz = t0.xyz;
    vs_TEXCOORD4.xyz = in_NORMAL0.xyz;
    t0.zw = vec2(in_COLOR0.z + in_COLOR0.x, in_COLOR0.w + in_COLOR0.y);
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
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
    vs_TEXCOORD6.x = (-t16_1) * acid_Fog.z + 1.0;
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
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	mediump float _Shininess;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	lowp vec4 _LightColor0;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _SpecTex;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in mediump vec3 vs_TEXCOORD2;
in mediump vec3 vs_TEXCOORD3;
in mediump vec3 vs_TEXCOORD4;
in highp vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
highp vec4 t1;
mediump vec3 t16_1;
highp vec4 t2;
lowp vec3 t10_2;
lowp vec3 t10_3;
mediump vec3 t16_4;
highp vec2 t6;
mediump float t16_12;
void main()
{
    t16_0.x = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_0.x = inversesqrt(t16_0.x);
    t16_0.xyz = t16_0.xxx * vs_TEXCOORD2.xyz;
    t16_12 = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD3.xyz);
    t16_12 = inversesqrt(t16_12);
    t16_0.xyz = vs_TEXCOORD3.xyz * vec3(t16_12) + t16_0.xyz;
    t16_12 = dot(t16_0.xyz, t16_0.xyz);
    t16_12 = inversesqrt(t16_12);
    t16_0.xyz = vec3(t16_12) * t16_0.xyz;
    t16_12 = dot(vs_TEXCOORD4.xyz, vs_TEXCOORD4.xyz);
    t16_12 = inversesqrt(t16_12);
    t16_1.xyz = vec3(t16_12) * vs_TEXCOORD4.xyz;
    t16_0.x = dot(t16_1.xyz, t16_0.xyz);
    t16_0.x = max(t16_0.x, 0.0);
    t16_0.x = log2(t16_0.x);
    t16_0.x = t16_0.x * _Shininess;
    t16_0.x = exp2(t16_0.x);
    t1.xy = fract(vs_TEXCOORD0.xy);
    t1.zw = (-t1.xy);
    t2 = t1 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t2 = t2 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t2 = max(t2, vec4(0.0, 0.0, 0.0, 0.0));
    t2.x = dot(t2, vec4(1.0, 1.0, 1.0, 1.0));
    t2.x = min(t2.x, 1.0);
    t2.x = t2.x * -20.0;
    t6.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t6.xy = t1.xy * t6.xy + vs_TEXCOORD5.xy;
    t10_3.xyz = texture(_SpecTex, t6.xy, t2.x).xyz;
    t10_2.xyz = texture(_MainTex, t6.xy, t2.x).xyz;
    t16_4.xyz = t10_3.xyz * _LightColor0.xyz;
    t16_0.xyz = t16_4.xyz * t16_0.xxx + t10_2.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD6.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
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
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _World2Object;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  mediump float fogpos_2;
  mediump vec3 modelViewDir_3;
  mediump vec3 modelLightDir_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  mediump vec2 tmpvar_7;
  tmpvar_7.x = 0.0;
  tmpvar_5 = tmpvar_1;
  mediump vec4 tmpvar_8;
  tmpvar_8.w = 0.0;
  tmpvar_8.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_9;
  tmpvar_9 = (_World2Object * tmpvar_8).xyz;
  modelLightDir_4 = tmpvar_9;
  highp vec3 tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11.w = 1.0;
  tmpvar_11.xyz = _WorldSpaceCameraPos;
  tmpvar_10 = ((_World2Object * tmpvar_11).xyz - _glesVertex.xyz);
  modelViewDir_3 = tmpvar_10;
  tmpvar_6.xy = _glesColor.xy;
  tmpvar_6.zw = (_glesColor.xy + _glesColor.zw);
  tmpvar_6 = (tmpvar_6 * 0.9960938);
  tmpvar_6.zw = (tmpvar_6.zw + 0.00390625);
  highp float tmpvar_12;
  highp vec3 x_13;
  x_13 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_12 = sqrt(dot (x_13, x_13));
  fogpos_2 = tmpvar_12;
  tmpvar_7.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_7.x = (1.0 - tmpvar_7.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = modelLightDir_4;
  xlv_TEXCOORD3 = modelViewDir_3;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _SpecTex;
uniform mediump float _Shininess;
uniform lowp vec4 _LightColor0;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
void main ()
{
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
  tmpvar_8 = texture2D (_SpecTex, tmpvar_5.xy, tmpvar_5.w);
  mediump vec4 tmpvar_9;
  tmpvar_9 = tmpvar_8;
  c_3.xyz = tmpvar_7.xyz;
  tmpvar_1 = ((tmpvar_9.xyz * _LightColor0.xyz) * pow (max (0.0, 
    dot (normalize(xlv_TEXCOORD4), normalize((normalize(xlv_TEXCOORD3) + normalize(xlv_TEXCOORD2))))
  ), _Shininess));
  c_3.xyz = (c_3.xyz + tmpvar_1);
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
uniform 	mediump float _Shininess;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	lowp vec4 _LightColor0;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD2;
out mediump vec3 vs_TEXCOORD3;
out mediump vec3 vs_TEXCOORD4;
out highp vec4 vs_TEXCOORD5;
highp vec4 t0;
mediump float t16_1;
mediump float t16_3;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t0.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t0.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t0.xyz;
    t0.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t0.xyz;
    vs_TEXCOORD2.xyz = t0.xyz;
    t0.xyz = _WorldSpaceCameraPos.xyzx.yyy * _World2Object[1].xyz;
    t0.xyz = _World2Object[0].xyz * _WorldSpaceCameraPos.xyzx.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WorldSpaceCameraPos.xyzx.zzz + t0.xyz;
    t0.xyz = t0.xyz + _World2Object[3].xyz;
    t0.xyz = t0.xyz + (-in_POSITION0.xyz);
    vs_TEXCOORD3.xyz = t0.xyz;
    vs_TEXCOORD4.xyz = in_NORMAL0.xyz;
    t0.zw = vec2(in_COLOR0.z + in_COLOR0.x, in_COLOR0.w + in_COLOR0.y);
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
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
    vs_TEXCOORD6.x = (-t16_1) * acid_Fog.z + 1.0;
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
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	mediump float _Shininess;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	lowp vec4 _LightColor0;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _SpecTex;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in mediump vec3 vs_TEXCOORD2;
in mediump vec3 vs_TEXCOORD3;
in mediump vec3 vs_TEXCOORD4;
in highp vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
highp vec4 t1;
mediump vec3 t16_1;
highp vec4 t2;
lowp vec3 t10_2;
lowp vec3 t10_3;
mediump vec3 t16_4;
highp vec2 t6;
mediump float t16_12;
void main()
{
    t16_0.x = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_0.x = inversesqrt(t16_0.x);
    t16_0.xyz = t16_0.xxx * vs_TEXCOORD2.xyz;
    t16_12 = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD3.xyz);
    t16_12 = inversesqrt(t16_12);
    t16_0.xyz = vs_TEXCOORD3.xyz * vec3(t16_12) + t16_0.xyz;
    t16_12 = dot(t16_0.xyz, t16_0.xyz);
    t16_12 = inversesqrt(t16_12);
    t16_0.xyz = vec3(t16_12) * t16_0.xyz;
    t16_12 = dot(vs_TEXCOORD4.xyz, vs_TEXCOORD4.xyz);
    t16_12 = inversesqrt(t16_12);
    t16_1.xyz = vec3(t16_12) * vs_TEXCOORD4.xyz;
    t16_0.x = dot(t16_1.xyz, t16_0.xyz);
    t16_0.x = max(t16_0.x, 0.0);
    t16_0.x = log2(t16_0.x);
    t16_0.x = t16_0.x * _Shininess;
    t16_0.x = exp2(t16_0.x);
    t1.xy = fract(vs_TEXCOORD0.xy);
    t1.zw = (-t1.xy);
    t2 = t1 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t2 = t2 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t2 = max(t2, vec4(0.0, 0.0, 0.0, 0.0));
    t2.x = dot(t2, vec4(1.0, 1.0, 1.0, 1.0));
    t2.x = min(t2.x, 1.0);
    t2.x = t2.x * -20.0;
    t6.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t6.xy = t1.xy * t6.xy + vs_TEXCOORD5.xy;
    t10_3.xyz = texture(_SpecTex, t6.xy, t2.x).xyz;
    t10_2.xyz = texture(_MainTex, t6.xy, t2.x).xyz;
    t16_4.xyz = t10_3.xyz * _LightColor0.xyz;
    t16_0.xyz = t16_4.xyz * t16_0.xxx + t10_2.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD6.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
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
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  mediump float fogpos_2;
  mediump vec3 modelViewDir_3;
  mediump vec3 modelLightDir_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  mediump vec2 tmpvar_7;
  mediump vec4 tmpvar_8;
  tmpvar_7.x = 0.0;
  tmpvar_5 = tmpvar_1;
  mediump vec4 tmpvar_9;
  tmpvar_9.w = 0.0;
  tmpvar_9.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_10;
  tmpvar_10 = (_World2Object * tmpvar_9).xyz;
  modelLightDir_4 = tmpvar_10;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = _WorldSpaceCameraPos;
  tmpvar_11 = ((_World2Object * tmpvar_12).xyz - _glesVertex.xyz);
  modelViewDir_3 = tmpvar_11;
  tmpvar_6.xy = _glesColor.xy;
  tmpvar_6.zw = (_glesColor.xy + _glesColor.zw);
  tmpvar_6 = (tmpvar_6 * 0.9960938);
  tmpvar_6.zw = (tmpvar_6.zw + 0.00390625);
  highp float tmpvar_13;
  highp vec3 x_14;
  x_14 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_13 = sqrt(dot (x_14, x_14));
  fogpos_2 = tmpvar_13;
  tmpvar_7.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_7.x = (1.0 - tmpvar_7.y);
  tmpvar_8 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = modelLightDir_4;
  xlv_TEXCOORD3 = modelViewDir_3;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
  xlv_TEXCOORD7 = tmpvar_8;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _SpecTex;
uniform mediump float _Shininess;
uniform lowp vec4 _EnvAmbientColor;
uniform lowp vec4 _LightColor0;
mediump vec3 tmpvar_1;
mediump vec3 tmpvar_2;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
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
  tmpvar_9 = texture2D (_SpecTex, tmpvar_6.xy, tmpvar_6.w);
  mediump vec4 tmpvar_10;
  tmpvar_10 = tmpvar_9;
  c_4.xyz = tmpvar_8.xyz;
  tmpvar_1 = ((tmpvar_10.xyz * _LightColor0.xyz) * pow (max (0.0, 
    dot (normalize(xlv_TEXCOORD4), normalize((normalize(xlv_TEXCOORD3) + normalize(xlv_TEXCOORD2))))
  ), _Shininess));
  c_4.xyz = (c_4.xyz + tmpvar_1);
  lowp float tmpvar_11;
  highp float tmpvar_12;
  tmpvar_12 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD7).x > 
    (xlv_TEXCOORD7.z / xlv_TEXCOORD7.w)
  )), _LightShadowData.x);
  tmpvar_11 = tmpvar_12;
  tmpvar_2 = ((_EnvAmbientColor.xyz * (1.0 - tmpvar_11)) + tmpvar_11);
  c_4.xyz = (c_4.xyz * tmpvar_2);
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
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  mediump float fogpos_2;
  mediump vec3 modelViewDir_3;
  mediump vec3 modelLightDir_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  mediump vec2 tmpvar_7;
  mediump vec4 tmpvar_8;
  tmpvar_7.x = 0.0;
  tmpvar_5 = tmpvar_1;
  mediump vec4 tmpvar_9;
  tmpvar_9.w = 0.0;
  tmpvar_9.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_10;
  tmpvar_10 = (_World2Object * tmpvar_9).xyz;
  modelLightDir_4 = tmpvar_10;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = _WorldSpaceCameraPos;
  tmpvar_11 = ((_World2Object * tmpvar_12).xyz - _glesVertex.xyz);
  modelViewDir_3 = tmpvar_11;
  tmpvar_6.xy = _glesColor.xy;
  tmpvar_6.zw = (_glesColor.xy + _glesColor.zw);
  tmpvar_6 = (tmpvar_6 * 0.9960938);
  tmpvar_6.zw = (tmpvar_6.zw + 0.00390625);
  highp float tmpvar_13;
  highp vec3 x_14;
  x_14 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_13 = sqrt(dot (x_14, x_14));
  fogpos_2 = tmpvar_13;
  tmpvar_7.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_7.x = (1.0 - tmpvar_7.y);
  tmpvar_8 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = modelLightDir_4;
  xlv_TEXCOORD3 = modelViewDir_3;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
  xlv_TEXCOORD7 = tmpvar_8;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _SpecTex;
uniform mediump float _Shininess;
uniform lowp vec4 _EnvAmbientColor;
uniform lowp vec4 _LightColor0;
mediump vec3 tmpvar_1;
mediump vec3 tmpvar_2;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
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
  tmpvar_9 = texture2D (_SpecTex, tmpvar_6.xy, tmpvar_6.w);
  mediump vec4 tmpvar_10;
  tmpvar_10 = tmpvar_9;
  c_4.xyz = tmpvar_8.xyz;
  tmpvar_1 = ((tmpvar_10.xyz * _LightColor0.xyz) * pow (max (0.0, 
    dot (normalize(xlv_TEXCOORD4), normalize((normalize(xlv_TEXCOORD3) + normalize(xlv_TEXCOORD2))))
  ), _Shininess));
  c_4.xyz = (c_4.xyz + tmpvar_1);
  lowp float shadow_11;
  shadow_11 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD7.xyz) * (1.0 - _LightShadowData.x)));
  tmpvar_2 = ((_EnvAmbientColor.xyz * (1.0 - shadow_11)) + shadow_11);
  c_4.xyz = (c_4.xyz * tmpvar_2);
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
uniform 	mediump float _Shininess;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	lowp vec4 _LightColor0;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD2;
out mediump vec3 vs_TEXCOORD3;
out mediump vec3 vs_TEXCOORD4;
out highp vec4 vs_TEXCOORD5;
out mediump vec4 vs_TEXCOORD7;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
mediump float t16_3;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t0.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t0.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t0.xyz;
    t0.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t0.xyz;
    vs_TEXCOORD2.xyz = t0.xyz;
    t0.xyz = _WorldSpaceCameraPos.xyzx.yyy * _World2Object[1].xyz;
    t0.xyz = _World2Object[0].xyz * _WorldSpaceCameraPos.xyzx.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WorldSpaceCameraPos.xyzx.zzz + t0.xyz;
    t0.xyz = t0.xyz + _World2Object[3].xyz;
    t0.xyz = t0.xyz + (-in_POSITION0.xyz);
    vs_TEXCOORD3.xyz = t0.xyz;
    vs_TEXCOORD4.xyz = in_NORMAL0.xyz;
    t0.zw = vec2(in_COLOR0.z + in_COLOR0.x, in_COLOR0.w + in_COLOR0.y);
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
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
    vs_TEXCOORD6.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD6.y = t16_3;
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
uniform 	mediump float _Shininess;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	lowp vec4 _LightColor0;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _SpecTex;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in mediump vec3 vs_TEXCOORD2;
in mediump vec3 vs_TEXCOORD3;
in mediump vec3 vs_TEXCOORD4;
in highp vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD7;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
highp vec4 t1;
mediump vec3 t16_1;
highp vec4 t2;
mediump vec3 t16_2;
lowp vec3 t10_2;
lowp vec3 t10_3;
mediump float t16_4;
mediump vec3 t16_5;
highp vec2 t7;
mediump float t16_15;
void main()
{
    t16_0.x = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_0.x = inversesqrt(t16_0.x);
    t16_0.xyz = t16_0.xxx * vs_TEXCOORD2.xyz;
    t16_15 = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD3.xyz);
    t16_15 = inversesqrt(t16_15);
    t16_0.xyz = vs_TEXCOORD3.xyz * vec3(t16_15) + t16_0.xyz;
    t16_15 = dot(t16_0.xyz, t16_0.xyz);
    t16_15 = inversesqrt(t16_15);
    t16_0.xyz = vec3(t16_15) * t16_0.xyz;
    t16_15 = dot(vs_TEXCOORD4.xyz, vs_TEXCOORD4.xyz);
    t16_15 = inversesqrt(t16_15);
    t16_1.xyz = vec3(t16_15) * vs_TEXCOORD4.xyz;
    t16_0.x = dot(t16_1.xyz, t16_0.xyz);
    t16_0.x = max(t16_0.x, 0.0);
    t16_0.x = log2(t16_0.x);
    t16_0.x = t16_0.x * _Shininess;
    t16_0.x = exp2(t16_0.x);
    t1.xy = fract(vs_TEXCOORD0.xy);
    t1.zw = (-t1.xy);
    t2 = t1 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t2 = t2 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t2 = max(t2, vec4(0.0, 0.0, 0.0, 0.0));
    t2.x = dot(t2, vec4(1.0, 1.0, 1.0, 1.0));
    t2.x = min(t2.x, 1.0);
    t2.x = t2.x * -20.0;
    t7.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t7.xy = t1.xy * t7.xy + vs_TEXCOORD5.xy;
    t10_3.xyz = texture(_SpecTex, t7.xy, t2.x).xyz;
    t10_2.xyz = texture(_MainTex, t7.xy, t2.x).xyz;
    t16_5.xyz = t10_3.xyz * _LightColor0.xyz;
    t16_0.xyz = t16_5.xyz * t16_0.xxx + t10_2.xyz;
    vec3 txVec17 = vec3(vs_TEXCOORD7.xy,vs_TEXCOORD7.z);
    t16_15 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec17, 0.0);
    t16_4 = (-_LightShadowData.x) + 1.0;
    t16_15 = t16_15 * t16_4 + _LightShadowData.x;
    t16_2.x = (-t16_15) + 1.0;
    t16_2.xyz = _EnvAmbientColor.xyz * t16_2.xxx + vec3(t16_15);
    t16_0.xyz = t16_0.xyz * t16_2.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD6.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
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
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_LightmapST;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  mediump float fogpos_2;
  mediump vec3 modelViewDir_3;
  mediump vec3 modelLightDir_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  mediump vec2 tmpvar_7;
  tmpvar_7.x = 0.0;
  tmpvar_5 = tmpvar_1;
  mediump vec4 tmpvar_8;
  tmpvar_8.w = 0.0;
  tmpvar_8.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_9;
  tmpvar_9 = (_World2Object * tmpvar_8).xyz;
  modelLightDir_4 = tmpvar_9;
  highp vec3 tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11.w = 1.0;
  tmpvar_11.xyz = _WorldSpaceCameraPos;
  tmpvar_10 = ((_World2Object * tmpvar_11).xyz - _glesVertex.xyz);
  modelViewDir_3 = tmpvar_10;
  tmpvar_6.xy = _glesColor.xy;
  tmpvar_6.zw = (_glesColor.xy + _glesColor.zw);
  tmpvar_6 = (tmpvar_6 * 0.9960938);
  tmpvar_6.zw = (tmpvar_6.zw + 0.00390625);
  highp float tmpvar_12;
  highp vec3 x_13;
  x_13 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_12 = sqrt(dot (x_13, x_13));
  fogpos_2 = tmpvar_12;
  tmpvar_7.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_7.x = (1.0 - tmpvar_7.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = modelLightDir_4;
  xlv_TEXCOORD3 = modelViewDir_3;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
  xlv_TEXCOORD8 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
uniform sampler2D _SpecTex;
uniform mediump float _Shininess;
uniform lowp vec4 _LightColor0;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
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
  tmpvar_8 = texture2D (_SpecTex, tmpvar_5.xy, tmpvar_5.w);
  mediump vec4 tmpvar_9;
  tmpvar_9 = tmpvar_8;
  c_3.xyz = tmpvar_7.xyz;
  tmpvar_1 = ((tmpvar_9.xyz * _LightColor0.xyz) * pow (max (0.0, 
    dot (normalize(xlv_TEXCOORD4), normalize((normalize(xlv_TEXCOORD3) + normalize(xlv_TEXCOORD2))))
  ), _Shininess));
  c_3.xyz = (c_3.xyz + tmpvar_1);
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_11;
  tmpvar_11 = (2.0 * tmpvar_10.xyz);
  c_3.xyz = (c_3.xyz * tmpvar_11);
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
uniform 	mediump float _Shininess;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	lowp vec4 _LightColor0;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD1;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD2;
out mediump vec3 vs_TEXCOORD3;
out mediump vec3 vs_TEXCOORD4;
out highp vec4 vs_TEXCOORD5;
out highp vec2 vs_TEXCOORD8;
highp vec4 t0;
mediump float t16_1;
mediump float t16_3;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t0.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t0.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t0.xyz;
    t0.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t0.xyz;
    vs_TEXCOORD2.xyz = t0.xyz;
    t0.xyz = _WorldSpaceCameraPos.xyzx.yyy * _World2Object[1].xyz;
    t0.xyz = _World2Object[0].xyz * _WorldSpaceCameraPos.xyzx.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WorldSpaceCameraPos.xyzx.zzz + t0.xyz;
    t0.xyz = t0.xyz + _World2Object[3].xyz;
    t0.xyz = t0.xyz + (-in_POSITION0.xyz);
    vs_TEXCOORD3.xyz = t0.xyz;
    vs_TEXCOORD4.xyz = in_NORMAL0.xyz;
    t0.zw = vec2(in_COLOR0.z + in_COLOR0.x, in_COLOR0.w + in_COLOR0.y);
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
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
    vs_TEXCOORD6.x = (-t16_1) * acid_Fog.z + 1.0;
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
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	mediump float _Shininess;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	lowp vec4 _LightColor0;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _SpecTex;
uniform lowp sampler2D unity_Lightmap;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in mediump vec3 vs_TEXCOORD2;
in mediump vec3 vs_TEXCOORD3;
in mediump vec3 vs_TEXCOORD4;
in highp vec4 vs_TEXCOORD5;
in highp vec2 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
highp vec4 t1;
mediump vec3 t16_1;
highp vec4 t2;
lowp vec3 t10_2;
lowp vec3 t10_3;
lowp vec3 t10_4;
mediump vec3 t16_5;
highp vec2 t7;
mediump float t16_15;
void main()
{
    t16_0.x = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_0.x = inversesqrt(t16_0.x);
    t16_0.xyz = t16_0.xxx * vs_TEXCOORD2.xyz;
    t16_15 = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD3.xyz);
    t16_15 = inversesqrt(t16_15);
    t16_0.xyz = vs_TEXCOORD3.xyz * vec3(t16_15) + t16_0.xyz;
    t16_15 = dot(t16_0.xyz, t16_0.xyz);
    t16_15 = inversesqrt(t16_15);
    t16_0.xyz = vec3(t16_15) * t16_0.xyz;
    t16_15 = dot(vs_TEXCOORD4.xyz, vs_TEXCOORD4.xyz);
    t16_15 = inversesqrt(t16_15);
    t16_1.xyz = vec3(t16_15) * vs_TEXCOORD4.xyz;
    t16_0.x = dot(t16_1.xyz, t16_0.xyz);
    t16_0.x = max(t16_0.x, 0.0);
    t16_0.x = log2(t16_0.x);
    t16_0.x = t16_0.x * _Shininess;
    t16_0.x = exp2(t16_0.x);
    t1.xy = fract(vs_TEXCOORD0.xy);
    t1.zw = (-t1.xy);
    t2 = t1 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t2 = t2 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t2 = max(t2, vec4(0.0, 0.0, 0.0, 0.0));
    t2.x = dot(t2, vec4(1.0, 1.0, 1.0, 1.0));
    t2.x = min(t2.x, 1.0);
    t2.x = t2.x * -20.0;
    t7.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t7.xy = t1.xy * t7.xy + vs_TEXCOORD5.xy;
    t10_3.xyz = texture(_SpecTex, t7.xy, t2.x).xyz;
    t10_2.xyz = texture(_MainTex, t7.xy, t2.x).xyz;
    t16_5.xyz = t10_3.xyz * _LightColor0.xyz;
    t16_0.xyz = t16_5.xyz * t16_0.xxx + t10_2.xyz;
    t10_2.xyz = texture(unity_Lightmap, vs_TEXCOORD8.xy).xyz;
    t10_4.xyz = t10_2.xyz * vec3(2.0, 2.0, 2.0);
    t16_0.xyz = t16_0.xyz * t10_4.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD6.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
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
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_LightmapST;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _LightDir;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  mediump float fogpos_2;
  mediump vec3 modelViewDir_3;
  mediump vec3 modelLightDir_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  mediump vec2 tmpvar_7;
  tmpvar_7.x = 0.0;
  tmpvar_5 = tmpvar_1;
  mediump vec4 tmpvar_8;
  tmpvar_8.w = 0.0;
  tmpvar_8.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_9;
  tmpvar_9 = (_World2Object * tmpvar_8).xyz;
  modelLightDir_4 = tmpvar_9;
  highp vec3 tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11.w = 1.0;
  tmpvar_11.xyz = _WorldSpaceCameraPos;
  tmpvar_10 = ((_World2Object * tmpvar_11).xyz - _glesVertex.xyz);
  modelViewDir_3 = tmpvar_10;
  tmpvar_6.xy = _glesColor.xy;
  tmpvar_6.zw = (_glesColor.xy + _glesColor.zw);
  tmpvar_6 = (tmpvar_6 * 0.9960938);
  tmpvar_6.zw = (tmpvar_6.zw + 0.00390625);
  highp float tmpvar_12;
  highp vec3 x_13;
  x_13 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_12 = sqrt(dot (x_13, x_13));
  fogpos_2 = tmpvar_12;
  tmpvar_7.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_7.x = (1.0 - tmpvar_7.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = modelLightDir_4;
  xlv_TEXCOORD3 = modelViewDir_3;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
  xlv_TEXCOORD8 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
uniform sampler2D _SpecTex;
uniform mediump float _Shininess;
uniform lowp vec4 _LightColor0;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
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
  tmpvar_8 = texture2D (_SpecTex, tmpvar_5.xy, tmpvar_5.w);
  mediump vec4 tmpvar_9;
  tmpvar_9 = tmpvar_8;
  c_3.xyz = tmpvar_7.xyz;
  tmpvar_1 = ((tmpvar_9.xyz * _LightColor0.xyz) * pow (max (0.0, 
    dot (normalize(xlv_TEXCOORD4), normalize((normalize(xlv_TEXCOORD3) + normalize(xlv_TEXCOORD2))))
  ), _Shininess));
  c_3.xyz = (c_3.xyz + tmpvar_1);
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_11;
  tmpvar_11 = (2.0 * tmpvar_10.xyz);
  c_3.xyz = (c_3.xyz * tmpvar_11);
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
uniform 	mediump float _Shininess;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	lowp vec4 _LightColor0;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD1;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD2;
out mediump vec3 vs_TEXCOORD3;
out mediump vec3 vs_TEXCOORD4;
out highp vec4 vs_TEXCOORD5;
out highp vec2 vs_TEXCOORD8;
highp vec4 t0;
mediump float t16_1;
mediump float t16_3;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t0.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t0.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t0.xyz;
    t0.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t0.xyz;
    vs_TEXCOORD2.xyz = t0.xyz;
    t0.xyz = _WorldSpaceCameraPos.xyzx.yyy * _World2Object[1].xyz;
    t0.xyz = _World2Object[0].xyz * _WorldSpaceCameraPos.xyzx.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WorldSpaceCameraPos.xyzx.zzz + t0.xyz;
    t0.xyz = t0.xyz + _World2Object[3].xyz;
    t0.xyz = t0.xyz + (-in_POSITION0.xyz);
    vs_TEXCOORD3.xyz = t0.xyz;
    vs_TEXCOORD4.xyz = in_NORMAL0.xyz;
    t0.zw = vec2(in_COLOR0.z + in_COLOR0.x, in_COLOR0.w + in_COLOR0.y);
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
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
    vs_TEXCOORD6.x = (-t16_1) * acid_Fog.z + 1.0;
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
uniform 	mediump vec3 acier_LightmapDecodeMul;
uniform 	mediump vec3 acier_LightmapDecodeAdd;
uniform 	mediump float _Shininess;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	lowp vec4 _LightColor0;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _SpecTex;
uniform lowp sampler2D unity_Lightmap;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in mediump vec3 vs_TEXCOORD2;
in mediump vec3 vs_TEXCOORD3;
in mediump vec3 vs_TEXCOORD4;
in highp vec4 vs_TEXCOORD5;
in highp vec2 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
highp vec4 t1;
mediump vec3 t16_1;
highp vec4 t2;
lowp vec3 t10_2;
lowp vec3 t10_3;
lowp vec3 t10_4;
mediump vec3 t16_5;
highp vec2 t7;
mediump float t16_15;
void main()
{
    t16_0.x = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_0.x = inversesqrt(t16_0.x);
    t16_0.xyz = t16_0.xxx * vs_TEXCOORD2.xyz;
    t16_15 = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD3.xyz);
    t16_15 = inversesqrt(t16_15);
    t16_0.xyz = vs_TEXCOORD3.xyz * vec3(t16_15) + t16_0.xyz;
    t16_15 = dot(t16_0.xyz, t16_0.xyz);
    t16_15 = inversesqrt(t16_15);
    t16_0.xyz = vec3(t16_15) * t16_0.xyz;
    t16_15 = dot(vs_TEXCOORD4.xyz, vs_TEXCOORD4.xyz);
    t16_15 = inversesqrt(t16_15);
    t16_1.xyz = vec3(t16_15) * vs_TEXCOORD4.xyz;
    t16_0.x = dot(t16_1.xyz, t16_0.xyz);
    t16_0.x = max(t16_0.x, 0.0);
    t16_0.x = log2(t16_0.x);
    t16_0.x = t16_0.x * _Shininess;
    t16_0.x = exp2(t16_0.x);
    t1.xy = fract(vs_TEXCOORD0.xy);
    t1.zw = (-t1.xy);
    t2 = t1 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t2 = t2 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t2 = max(t2, vec4(0.0, 0.0, 0.0, 0.0));
    t2.x = dot(t2, vec4(1.0, 1.0, 1.0, 1.0));
    t2.x = min(t2.x, 1.0);
    t2.x = t2.x * -20.0;
    t7.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t7.xy = t1.xy * t7.xy + vs_TEXCOORD5.xy;
    t10_3.xyz = texture(_SpecTex, t7.xy, t2.x).xyz;
    t10_2.xyz = texture(_MainTex, t7.xy, t2.x).xyz;
    t16_5.xyz = t10_3.xyz * _LightColor0.xyz;
    t16_0.xyz = t16_5.xyz * t16_0.xxx + t10_2.xyz;
    t10_2.xyz = texture(unity_Lightmap, vs_TEXCOORD8.xy).xyz;
    t10_4.xyz = t10_2.xyz * vec3(2.0, 2.0, 2.0);
    t16_0.xyz = t16_0.xyz * t10_4.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD6.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
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
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
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
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  mediump float fogpos_2;
  mediump vec3 modelViewDir_3;
  mediump vec3 modelLightDir_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  mediump vec2 tmpvar_7;
  mediump vec4 tmpvar_8;
  tmpvar_7.x = 0.0;
  tmpvar_5 = tmpvar_1;
  mediump vec4 tmpvar_9;
  tmpvar_9.w = 0.0;
  tmpvar_9.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_10;
  tmpvar_10 = (_World2Object * tmpvar_9).xyz;
  modelLightDir_4 = tmpvar_10;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = _WorldSpaceCameraPos;
  tmpvar_11 = ((_World2Object * tmpvar_12).xyz - _glesVertex.xyz);
  modelViewDir_3 = tmpvar_11;
  tmpvar_6.xy = _glesColor.xy;
  tmpvar_6.zw = (_glesColor.xy + _glesColor.zw);
  tmpvar_6 = (tmpvar_6 * 0.9960938);
  tmpvar_6.zw = (tmpvar_6.zw + 0.00390625);
  highp float tmpvar_13;
  highp vec3 x_14;
  x_14 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_13 = sqrt(dot (x_14, x_14));
  fogpos_2 = tmpvar_13;
  tmpvar_7.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_7.x = (1.0 - tmpvar_7.y);
  tmpvar_8 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = modelLightDir_4;
  xlv_TEXCOORD3 = modelViewDir_3;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
  xlv_TEXCOORD7 = tmpvar_8;
  xlv_TEXCOORD8 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform sampler2D unity_Lightmap;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _SpecTex;
uniform mediump float _Shininess;
uniform lowp vec4 _EnvAmbientColor;
uniform lowp vec4 _LightColor0;
mediump vec3 tmpvar_1;
mediump vec3 tmpvar_2;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  lowp vec3 lm_3;
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
  tmpvar_10 = texture2D (_SpecTex, tmpvar_7.xy, tmpvar_7.w);
  mediump vec4 tmpvar_11;
  tmpvar_11 = tmpvar_10;
  c_5.xyz = tmpvar_9.xyz;
  tmpvar_1 = ((tmpvar_11.xyz * _LightColor0.xyz) * pow (max (0.0, 
    dot (normalize(xlv_TEXCOORD4), normalize((normalize(xlv_TEXCOORD3) + normalize(xlv_TEXCOORD2))))
  ), _Shininess));
  c_5.xyz = (c_5.xyz + tmpvar_1);
  lowp float tmpvar_12;
  highp float tmpvar_13;
  tmpvar_13 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD7).x > 
    (xlv_TEXCOORD7.z / xlv_TEXCOORD7.w)
  )), _LightShadowData.x);
  tmpvar_12 = tmpvar_13;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_15;
  tmpvar_15 = (2.0 * tmpvar_14.xyz);
  lm_3 = tmpvar_15;
  lowp vec3 tmpvar_16;
  tmpvar_16 = min (lm_3, mix (_EnvAmbientColor.xyz, lm_3, vec3(tmpvar_12)));
  tmpvar_2 = tmpvar_16;
  c_5.xyz = (c_5.xyz * tmpvar_2);
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
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
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
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  mediump float fogpos_2;
  mediump vec3 modelViewDir_3;
  mediump vec3 modelLightDir_4;
  mediump vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  mediump vec2 tmpvar_7;
  mediump vec4 tmpvar_8;
  tmpvar_7.x = 0.0;
  tmpvar_5 = tmpvar_1;
  mediump vec4 tmpvar_9;
  tmpvar_9.w = 0.0;
  tmpvar_9.xyz = -(_LightDir.xyz);
  highp vec3 tmpvar_10;
  tmpvar_10 = (_World2Object * tmpvar_9).xyz;
  modelLightDir_4 = tmpvar_10;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = _WorldSpaceCameraPos;
  tmpvar_11 = ((_World2Object * tmpvar_12).xyz - _glesVertex.xyz);
  modelViewDir_3 = tmpvar_11;
  tmpvar_6.xy = _glesColor.xy;
  tmpvar_6.zw = (_glesColor.xy + _glesColor.zw);
  tmpvar_6 = (tmpvar_6 * 0.9960938);
  tmpvar_6.zw = (tmpvar_6.zw + 0.00390625);
  highp float tmpvar_13;
  highp vec3 x_14;
  x_14 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_13 = sqrt(dot (x_14, x_14));
  fogpos_2 = tmpvar_13;
  tmpvar_7.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_7.x = (1.0 - tmpvar_7.y);
  tmpvar_8 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD2 = modelLightDir_4;
  xlv_TEXCOORD3 = modelViewDir_3;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
  xlv_TEXCOORD7 = tmpvar_8;
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
uniform sampler2D _SpecTex;
uniform mediump float _Shininess;
uniform lowp vec4 _EnvAmbientColor;
uniform lowp vec4 _LightColor0;
mediump vec3 tmpvar_1;
mediump vec3 tmpvar_2;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec2 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  lowp vec3 lm_3;
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
  tmpvar_10 = texture2D (_SpecTex, tmpvar_7.xy, tmpvar_7.w);
  mediump vec4 tmpvar_11;
  tmpvar_11 = tmpvar_10;
  c_5.xyz = tmpvar_9.xyz;
  tmpvar_1 = ((tmpvar_11.xyz * _LightColor0.xyz) * pow (max (0.0, 
    dot (normalize(xlv_TEXCOORD4), normalize((normalize(xlv_TEXCOORD3) + normalize(xlv_TEXCOORD2))))
  ), _Shininess));
  c_5.xyz = (c_5.xyz + tmpvar_1);
  lowp float shadow_12;
  shadow_12 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD7.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_14;
  tmpvar_14 = (2.0 * tmpvar_13.xyz);
  lm_3 = tmpvar_14;
  lowp vec3 tmpvar_15;
  tmpvar_15 = min (lm_3, mix (_EnvAmbientColor.xyz, lm_3, vec3(shadow_12)));
  tmpvar_2 = tmpvar_15;
  c_5.xyz = (c_5.xyz * tmpvar_2);
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
uniform 	mediump float _Shininess;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	lowp vec4 _LightColor0;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD1;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD2;
out mediump vec3 vs_TEXCOORD3;
out mediump vec3 vs_TEXCOORD4;
out highp vec4 vs_TEXCOORD5;
out mediump vec4 vs_TEXCOORD7;
out highp vec2 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
mediump float t16_3;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t0.xyz = _World2Object[1].xyz * (-_LightDir.yyy);
    t0.xyz = _World2Object[0].xyz * (-_LightDir.xxx) + t0.xyz;
    t0.xyz = _World2Object[2].xyz * (-_LightDir.zzz) + t0.xyz;
    vs_TEXCOORD2.xyz = t0.xyz;
    t0.xyz = _WorldSpaceCameraPos.xyzx.yyy * _World2Object[1].xyz;
    t0.xyz = _World2Object[0].xyz * _WorldSpaceCameraPos.xyzx.xxx + t0.xyz;
    t0.xyz = _World2Object[2].xyz * _WorldSpaceCameraPos.xyzx.zzz + t0.xyz;
    t0.xyz = t0.xyz + _World2Object[3].xyz;
    t0.xyz = t0.xyz + (-in_POSITION0.xyz);
    vs_TEXCOORD3.xyz = t0.xyz;
    vs_TEXCOORD4.xyz = in_NORMAL0.xyz;
    t0.zw = vec2(in_COLOR0.z + in_COLOR0.x, in_COLOR0.w + in_COLOR0.y);
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
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
    vs_TEXCOORD6.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD6.y = t16_3;
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
uniform 	mediump float _Shininess;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _LightDir;
uniform 	lowp vec4 _LightColor0;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _SpecTex;
uniform lowp sampler2D unity_Lightmap;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD6;
in mediump vec3 vs_TEXCOORD2;
in mediump vec3 vs_TEXCOORD3;
in mediump vec3 vs_TEXCOORD4;
in highp vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD7;
in highp vec2 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
highp vec4 t1;
mediump vec3 t16_1;
highp vec4 t2;
lowp vec3 t10_2;
lowp vec3 t10_3;
mediump float t16_4;
lowp vec3 t10_5;
lowp vec3 t10_6;
mediump vec3 t16_7;
highp vec2 t9;
mediump float t16_21;
void main()
{
    t16_0.x = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
    t16_0.x = inversesqrt(t16_0.x);
    t16_0.xyz = t16_0.xxx * vs_TEXCOORD2.xyz;
    t16_21 = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD3.xyz);
    t16_21 = inversesqrt(t16_21);
    t16_0.xyz = vs_TEXCOORD3.xyz * vec3(t16_21) + t16_0.xyz;
    t16_21 = dot(t16_0.xyz, t16_0.xyz);
    t16_21 = inversesqrt(t16_21);
    t16_0.xyz = vec3(t16_21) * t16_0.xyz;
    t16_21 = dot(vs_TEXCOORD4.xyz, vs_TEXCOORD4.xyz);
    t16_21 = inversesqrt(t16_21);
    t16_1.xyz = vec3(t16_21) * vs_TEXCOORD4.xyz;
    t16_0.x = dot(t16_1.xyz, t16_0.xyz);
    t16_0.x = max(t16_0.x, 0.0);
    t16_0.x = log2(t16_0.x);
    t16_0.x = t16_0.x * _Shininess;
    t16_0.x = exp2(t16_0.x);
    t1.xy = fract(vs_TEXCOORD0.xy);
    t1.zw = (-t1.xy);
    t2 = t1 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t2 = t2 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t2 = max(t2, vec4(0.0, 0.0, 0.0, 0.0));
    t2.x = dot(t2, vec4(1.0, 1.0, 1.0, 1.0));
    t2.x = min(t2.x, 1.0);
    t2.x = t2.x * -20.0;
    t9.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t9.xy = t1.xy * t9.xy + vs_TEXCOORD5.xy;
    t10_3.xyz = texture(_SpecTex, t9.xy, t2.x).xyz;
    t10_2.xyz = texture(_MainTex, t9.xy, t2.x).xyz;
    t16_7.xyz = t10_3.xyz * _LightColor0.xyz;
    t16_0.xyz = t16_7.xyz * t16_0.xxx + t10_2.xyz;
    vec3 txVec1 = vec3(vs_TEXCOORD7.xy,vs_TEXCOORD7.z);
    t16_21 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec1, 0.0);
    t16_4 = (-_LightShadowData.x) + 1.0;
    t16_21 = t16_21 * t16_4 + _LightShadowData.x;
    t10_2.xyz = texture(unity_Lightmap, vs_TEXCOORD8.xy).xyz;
    t10_5.xyz = vec3(2.0, 2.0, 2.0) * t10_2.xyz + (-_EnvAmbientColor.xyz);
    t10_6.xyz = t10_2.xyz * vec3(2.0, 2.0, 2.0);
    t10_5.xyz = vec3(t16_21) * t10_5.xyz + _EnvAmbientColor.xyz;
    t10_5.xyz = min(t10_5.xyz, t10_6.xyz);
    t16_0.xyz = t16_0.xyz * t10_5.xyz;
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
Fallback "Generated/DiffuseTex2DLOD"
}
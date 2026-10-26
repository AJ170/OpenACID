Shader "Barrier/Wall" {
Properties {
 _MainTex ("Billboard Texture", 2D) = "white" { }
 _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,1)
}
SubShader { 
 Tags { "QUEUE"="Transparent+1" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent+1" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend One One
  ColorMask RGB
  GpuProgramID 39399
Program "vp" {
SubProgram "gles " {
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 _TintColor;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD3;
varying mediump vec3 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD5;
void main ()
{
  mediump float fade_1;
  highp vec3 modelRight_2;
  mediump vec3 delta_3;
  mediump vec2 tmpvar_4;
  mediump vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  mediump vec3 tmpvar_7;
  mediump vec3 tmpvar_8;
  mediump vec3 tmpvar_9;
  tmpvar_6.w = 0.0;
  highp vec2 tmpvar_10;
  tmpvar_10 = _glesMultiTexCoord0.xy;
  tmpvar_4 = tmpvar_10;
  tmpvar_5 = ((2.0 * _TintColor) * _glesColor.z);
  highp vec3 tmpvar_11;
  tmpvar_11 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 0.0;
  tmpvar_12.xyz = _glesNormal;
  highp vec3 tmpvar_13;
  tmpvar_13 = (_Object2World * tmpvar_12).xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_11 - _WorldSpaceCameraPos));
  tmpvar_7 = tmpvar_14;
  tmpvar_8 = tmpvar_13;
  tmpvar_9 = tmpvar_11;
  highp vec3 tmpvar_15;
  tmpvar_15 = (tmpvar_11 - _WorldSpaceCameraPos);
  delta_3 = tmpvar_15;
  highp vec4 tmpvar_16;
  tmpvar_16.w = 0.0;
  tmpvar_16.xyz = _glesTANGENT.xyz;
  modelRight_2.xz = (_Object2World * tmpvar_16).xz;
  modelRight_2.y = 0.0;
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize(modelRight_2);
  modelRight_2 = tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_18 = abs(((
    (tmpvar_17 * delta_3.z)
   + 
    (vec3(0.0, 1.0, 0.0) * delta_3.y)
  ) + (tmpvar_13 * delta_3.x)));
  highp vec3 tmpvar_19;
  tmpvar_19 = ((min (1.0, 
    (2.0 / tmpvar_18.x)
  ) * _glesColor.z) * _TintColor).xyz;
  tmpvar_6.xyz = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = pow (min (1.0, (17.5 / tmpvar_18.x)), 4.0);
  fade_1 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = clamp (((tmpvar_18.z - _glesTANGENT.w) * 0.05), 0.0, 1.0);
  mediump float tmpvar_22;
  tmpvar_22 = max (0.0, (fade_1 - tmpvar_21));
  fade_1 = tmpvar_22;
  tmpvar_5.xyz = (tmpvar_5.xyz * tmpvar_22);
  tmpvar_6.xyz = (tmpvar_6.xyz * tmpvar_22);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = tmpvar_5;
  xlv_TEXCOORD2 = tmpvar_6;
  xlv_TEXCOORD3 = tmpvar_7;
  xlv_TEXCOORD4 = tmpvar_8;
  xlv_TEXCOORD5 = tmpvar_9;
}


#endif
#ifdef FRAGMENT
uniform highp vec3 _WorldSpaceCameraPos;
uniform sampler2D _MainTex;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
varying mediump vec3 xlv_TEXCOORD4;
varying mediump vec3 xlv_TEXCOORD5;
void main ()
{
  mediump vec4 tmpvar_1;
  highp float f_2;
  highp vec4 tex_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0);
  tex_3 = tmpvar_4;
  highp float tmpvar_5;
  tmpvar_5 = dot (xlv_TEXCOORD4, normalize((xlv_TEXCOORD5 - _WorldSpaceCameraPos)));
  f_2 = (tmpvar_5 * tmpvar_5);
  f_2 = (f_2 * f_2);
  tmpvar_1 = (((f_2 * tex_3) * xlv_TEXCOORD1) + (xlv_TEXCOORD2 * tex_3));
  gl_FragData[0] = tmpvar_1;
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
uniform 	vec4 _TintColor;
uniform 	vec4 _Size;
uniform 	vec4 _Speed;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in lowp vec4 in_COLOR0;
out vec4 undefined;
out mediump vec2 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec4 vs_TEXCOORD2;
out mediump vec3 vs_TEXCOORD3;
out mediump vec3 vs_TEXCOORD4;
out mediump vec3 vs_TEXCOORD5;
highp vec4 t0;
highp vec3 t1;
highp vec4 t2;
highp vec3 t3;
highp float t4;
highp vec2 t8;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    gl_Position = t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t0.xy = in_TANGENT0.yy * _Object2World[1].xz;
    t0.xy = _Object2World[0].xz * in_TANGENT0.xx + t0.xy;
    t0.xy = _Object2World[2].xz * in_TANGENT0.zz + t0.xy;
    t8.x = dot(t0.xy, t0.xy);
    t8.x = inversesqrt(t8.x);
    t0.xy = t8.xx * t0.xy;
    t1.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t1.xyz;
    t2.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t2.xyz;
    t2.xyz = _Object2World[3].xyz * in_POSITION0.www + t2.xyz;
    t3.xyz = t2.xyz + (-_WorldSpaceCameraPos.xyzx.xyz);
    vs_TEXCOORD5.xyz = t2.xyz;
    t8.xy = t1.xz * t3.xx;
    vs_TEXCOORD4.xyz = t1.xyz;
    t0.xy = t0.xy * t3.zz + t8.xy;
    t4 = abs(t0.y) + (-in_TANGENT0.w);
    t0.xz = vec2(2.0, 17.5) / abs(t0.xx);
    t0.xz = min(t0.xz, vec2(1.0, 1.0));
    t4 = t4 * 0.0500000007;
    t4 = clamp(t4, 0.0, 1.0);
    t8.x = t0.z * t0.z;
    t0.x = t0.x * in_COLOR0.z;
    t1.xyz = t0.xxx * _TintColor.xyz;
    t0.x = t8.x * t8.x + (-t4);
    t0.x = max(t0.x, 0.0);
    t2 = _TintColor + _TintColor;
    t2 = t2 * in_COLOR0.zzzz;
    vs_TEXCOORD1.xyz = t0.xxx * t2.xyz;
    vs_TEXCOORD2.xyz = t0.xxx * t1.xyz;
    vs_TEXCOORD1.w = t2.w;
    vs_TEXCOORD2.w = 0.0;
    t0.x = dot(t3.xyz, t3.xyz);
    t0.x = inversesqrt(t0.x);
    t0.xyz = t0.xxx * t3.xyz;
    vs_TEXCOORD3.xyz = t0.xyz;
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
uniform 	vec4 _TintColor;
uniform 	vec4 _Size;
uniform 	vec4 _Speed;
uniform lowp sampler2D _MainTex;
in mediump vec2 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec4 vs_TEXCOORD2;
in mediump vec3 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD5;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec4 t0;
mediump vec4 t16_1;
lowp vec4 t10_1;
highp float t6;
void main()
{
    t0.xyz = vs_TEXCOORD5.xyz + (-_WorldSpaceCameraPos.xyzx.xyz);
    t6 = dot(t0.xyz, t0.xyz);
    t6 = inversesqrt(t6);
    t0.xyz = vec3(t6) * t0.xyz;
    t0.x = dot(vs_TEXCOORD4.xyz, t0.xyz);
    t0.x = t0.x * t0.x;
    t0.x = t0.x * t0.x;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t0 = t0.xxxx * t10_1;
    t16_1 = t10_1 * vs_TEXCOORD2;
    t0 = t0 * vs_TEXCOORD1 + t16_1;
    SV_Target0 = t0;
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
Shader "Animus/PrimitiveFloatingFloor" {
Properties {
 _Color ("Main Color", Color) = (0.5,0.5,0.5,1)
 _ShadowColor ("Shadow Color", Color) = (0,0,0,0.5)
 _BumpMap ("Bumpmap (RGB)", 2D) = "bump" { }
 _MatCap ("MatCap (RGB)", 2D) = "white" { }
 _EffectCube ("Reflection Map", CUBE) = "" { }
 _FadeDistance ("Fade Distance", Float) = 10
 _Falloff ("Distance Falloff", Float) = 0.5
 _Amplitude ("Distance Amplitude", Float) = 1.5
 _MeshWrapSizeX ("Mesh Wrap Size X", Float) = 10
 _MeshWrapSizeZ ("Mesh Wrap Size Z", Float) = 10
 _ReflIntensity ("Reflection Intensity", Float) = 0.25
 _OffsetY ("Floor Y-Offset", Float) = -0.05
}
SubShader { 
 Tags { "QUEUE"="Background+1" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Background+1" }
  Lighting On
  GpuProgramID 6332
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp mat4 unity_MatrixVP;
uniform mediump float _FadeDistance;
uniform mediump float _Falloff;
uniform mediump float _MeshWrapSizeX;
uniform mediump float _MeshWrapSizeZ;
uniform mediump float _Amplitude;
uniform mediump float _OffsetY;
varying mediump vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  mediump vec4 tmpvar_2;
  tmpvar_2.xyw = _glesColor.xyw;
  mediump float mDeg_3;
  mediump float distance_4;
  highp vec4 wPos_5;
  highp vec2 meshWrapSize_6;
  highp vec3 tmpvar_7;
  highp vec3 tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_8 = _glesTANGENT.xyz;
  tmpvar_9 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_10;
  tmpvar_10[0].x = tmpvar_8.x;
  tmpvar_10[0].y = tmpvar_9.x;
  tmpvar_10[0].z = tmpvar_1.x;
  tmpvar_10[1].x = tmpvar_8.y;
  tmpvar_10[1].y = tmpvar_9.y;
  tmpvar_10[1].z = tmpvar_1.y;
  tmpvar_10[2].x = tmpvar_8.z;
  tmpvar_10[2].y = tmpvar_9.z;
  tmpvar_10[2].z = tmpvar_1.z;
  highp vec2 tmpvar_11;
  tmpvar_11.x = _Object2World[3].x;
  tmpvar_11.y = _Object2World[3].z;
  mediump vec2 tmpvar_12;
  tmpvar_12.x = _MeshWrapSizeX;
  tmpvar_12.y = _MeshWrapSizeZ;
  meshWrapSize_6 = tmpvar_12;
  highp vec2 tmpvar_13;
  tmpvar_13 = ((vec2(
    ivec2(((tmpvar_11 / meshWrapSize_6) + (sign(tmpvar_11) * vec2(0.5, 0.5))))
  ) * meshWrapSize_6) + _glesVertex.xz);
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11 - tmpvar_13);
  wPos_5.xz = (tmpvar_13 + ((
    vec2(greaterThanEqual (abs(tmpvar_14), (meshWrapSize_6 * 0.5)))
   * 
    sign(tmpvar_14)
  ) * meshWrapSize_6));
  wPos_5.y = _OffsetY;
  wPos_5.w = 1.0;
  highp float tmpvar_15;
  highp vec2 x_16;
  x_16 = (wPos_5.xz - tmpvar_11);
  tmpvar_15 = sqrt(dot (x_16, x_16));
  distance_4 = tmpvar_15;
  mediump float tmpvar_17;
  tmpvar_17 = clamp ((_FadeDistance + (distance_4 * _Falloff)), 0.0, 1.0);
  highp float tmpvar_18;
  tmpvar_18 = ((wPos_5.z * wPos_5.x) * 0.0075);
  mDeg_3 = tmpvar_18;
  mDeg_3 = (mDeg_3 + (_Time.z * 0.5));
  highp vec4 v_19;
  v_19.x = _World2Object[0].z;
  v_19.y = _World2Object[1].z;
  v_19.z = _World2Object[2].z;
  v_19.w = _World2Object[3].z;
  wPos_5.xyz = (wPos_5.xyz - v_19.xyz);
  wPos_5.xz = (wPos_5.xz + _glesMultiTexCoord1.xy);
  wPos_5.y = (wPos_5.y - (_glesColor.z + (tmpvar_17 * _Amplitude)));
  mediump float tmpvar_20;
  tmpvar_20 = sin((mDeg_3 + (_glesColor.x * 10.0)));
  wPos_5.y = (wPos_5.y + ((tmpvar_20 * tmpvar_17) * 0.5));
  tmpvar_7 = ((wPos_5.xyz + v_19.xyz) - _WorldSpaceCameraPos);
  tmpvar_2.z = tmpvar_17;
  mediump float tmpvar_21;
  tmpvar_21 = sin((mDeg_3 + (_glesColor.x * 12.0)));
  highp vec4 tmpvar_22;
  tmpvar_22.w = 1.0;
  tmpvar_22.xyz = (wPos_5.xyz + v_19.xyz);
  wPos_5 = tmpvar_22;
  highp vec4 v_23;
  v_23.x = glstate_matrix_modelview0[0].x;
  v_23.y = glstate_matrix_modelview0[1].x;
  v_23.z = glstate_matrix_modelview0[2].x;
  v_23.w = glstate_matrix_modelview0[3].x;
  highp vec4 v_24;
  v_24.x = glstate_matrix_modelview0[0].y;
  v_24.y = glstate_matrix_modelview0[1].y;
  v_24.z = glstate_matrix_modelview0[2].y;
  v_24.w = glstate_matrix_modelview0[3].y;
  gl_Position = (unity_MatrixVP * tmpvar_22);
  xlv_COLOR = tmpvar_2;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = (tmpvar_10 * v_23.xyz);
  xlv_TEXCOORD2 = (tmpvar_10 * v_24.xyz);
  xlv_TEXCOORD3 = (_glesNormal + ((tmpvar_21 * 0.0125) * _glesColor.y));
  xlv_TEXCOORD4 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _MatCap_ST;
uniform mediump vec4 _Color;
uniform sampler2D _BumpMap;
uniform sampler2D _MatCap;
uniform lowp samplerCube _EffectCube;
uniform mediump float _ReflIntensity;
varying mediump vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  mediump vec4 finalColor_1;
  mediump vec3 reflectedDir_2;
  mediump vec2 vn_3;
  highp vec3 normal_4;
  lowp vec3 tmpvar_5;
  tmpvar_5 = ((texture2D (_BumpMap, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  normal_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = dot (xlv_TEXCOORD1, normal_4);
  vn_3.x = tmpvar_6;
  highp float tmpvar_7;
  tmpvar_7 = dot (xlv_TEXCOORD2, normal_4);
  vn_3.y = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize(xlv_TEXCOORD3);
  highp vec3 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD4 - (2.0 * (
    dot (tmpvar_8, xlv_TEXCOORD4)
   * tmpvar_8)));
  reflectedDir_2 = tmpvar_9;
  lowp vec4 tmpvar_10;
  mediump vec2 P_11;
  P_11 = (((vn_3 * 0.5) + 0.5) + _MatCap_ST.zw);
  tmpvar_10 = texture2D (_MatCap, P_11);
  mediump float tmpvar_12;
  tmpvar_12 = (1.0 - xlv_COLOR.z);
  finalColor_1 = ((_Color * xlv_COLOR.z) + ((tmpvar_10 * _Color) * (2.0 * tmpvar_12)));
  lowp vec4 tmpvar_13;
  tmpvar_13 = textureCube (_EffectCube, reflectedDir_2);
  finalColor_1.xyz = (finalColor_1 + ((tmpvar_13 * _ReflIntensity) * tmpvar_12)).xyz;
  finalColor_1.w = 1.0;
  gl_FragData[0] = finalColor_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" }
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
uniform 	mediump vec4 _MatCap_ST;
uniform 	mediump vec4 _Color;
uniform 	mediump vec4 _ShadowColor;
uniform 	mediump float _FadeDistance;
uniform 	mediump float _Falloff;
uniform 	mediump float _MeshWrapSizeX;
uniform 	mediump float _MeshWrapSizeZ;
uniform 	mediump float _Amplitude;
uniform 	mediump float _ReflIntensity;
uniform 	mediump float _OffsetY;
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in mediump vec4 in_TEXCOORD0;
in mediump vec4 in_TEXCOORD1;
in highp vec3 in_NORMAL0;
in highp vec4 in_TANGENT0;
out mediump vec4 vs_COLOR0;
out mediump vec2 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
out highp vec3 vs_TEXCOORD4;
highp vec3 t0;
highp ivec2 ti0;
highp vec4 t1;
highp ivec2 ti1;
mediump vec2 t16_2;
highp vec3 t3;
mediump float t16_6;
highp vec2 t8;
highp ivec2 ti8;
bvec2 tb8;
highp vec2 t9;
highp ivec2 ti9;
mediump float t16_10;
highp float t12;
void main()
{
    ti0.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), _Object2World[3].xzxx).xy) * 0xFFFFFFFFu);
    ti8.xy = ivec2(uvec2(lessThan(_Object2World[3].xzxz, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti0.xy = (-ti0.xy) + ti8.xy;
    t0.xy = vec2(ti0.xy);
    t8.xy = vec2(_Object2World[3].x / float(_MeshWrapSizeX), _Object2World[3].z / float(_MeshWrapSizeZ));
    t0.xy = t0.xy * vec2(0.5, 0.5) + t8.xy;
    t0.xy = trunc(t0.xy);
    t0.xy = t0.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + in_POSITION0.xz;
    t8.xy = (-t0.xy) + _Object2World[3].xz;
    ti1.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), t8.xyxx).xy) * 0xFFFFFFFFu);
    ti9.xy = ivec2(uvec2(lessThan(t8.xyxy, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti1.xy = (-ti1.xy) + ti9.xy;
    t1.xy = vec2(ti1.xy);
    t9.xy = vec2(float(_MeshWrapSizeX) * float(0.5), float(_MeshWrapSizeZ) * float(0.5));
    tb8.xy = greaterThanEqual(abs(t8.xyxy), t9.xyxy).xy;
    t8.xy = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb8.xy));
    t8.xy = t1.xy * t8.xy;
    t0.xy = t8.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + t0.xy;
    t8.x = t0.x * t0.y;
    t12 = _Time.z * 0.5;
    t8.x = t8.x * 0.00749999983 + t12;
    t16_2.xy = in_COLOR0.xx * vec2(10.0, 12.0) + t8.xx;
    t16_2.xy = sin(t16_2.xy);
    t8.xy = t0.xy + (-_Object2World[3].xz);
    t8.x = dot(t8.xy, t8.xy);
    t8.x = sqrt(t8.x);
    t16_10 = t8.x * _Falloff + _FadeDistance;
    t16_10 = clamp(t16_10, 0.0, 1.0);
    t16_2.x = t16_10 * t16_2.x;
    t16_6 = t16_2.y * in_COLOR0.y;
    vs_TEXCOORD3.xyz = vec3(t16_6) * vec3(0.0125000002, 0.0125000002, 0.0125000002) + in_NORMAL0.xyz;
    t16_6 = t16_10 * _Amplitude + in_COLOR0.z;
    vs_COLOR0.z = t16_10;
    t1.y = _World2Object[1].z;
    t8.x = (-t1.y) + _OffsetY;
    t8.x = (-t16_6) + t8.x;
    t3.y = t16_2.x * 0.5 + t8.x;
    t1.x = _World2Object[0].z;
    t1.z = _World2Object[2].z;
    t0.xy = t0.xy + (-t1.xz);
    t3.xz = t0.xy + in_TEXCOORD1.xy;
    t0.xyz = t1.xyz + t3.xyz;
    t1 = t0.yyyy * unity_MatrixVP[1];
    t1 = unity_MatrixVP[0] * t0.xxxx + t1;
    t1 = unity_MatrixVP[2] * t0.zzzz + t1;
    vs_TEXCOORD4.xyz = t0.xyz + (-_WorldSpaceCameraPos.xyzx.xyz);
    gl_Position = t1 + unity_MatrixVP[3];
    vs_COLOR0.xyw = in_COLOR0.xyw;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t0.xyz = in_NORMAL0.zxy * in_TANGENT0.yzx;
    t0.xyz = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t0.xyz);
    t0.xyz = t0.xyz * in_TANGENT0.www;
    t1.x = glstate_matrix_modelview0[0].x;
    t1.y = glstate_matrix_modelview0[1].x;
    t1.z = glstate_matrix_modelview0[2].x;
    vs_TEXCOORD1.y = dot(t0.xyz, t1.xyz);
    vs_TEXCOORD1.x = dot(in_TANGENT0.xyz, t1.xyz);
    vs_TEXCOORD1.z = dot(in_NORMAL0.xyz, t1.xyz);
    t1.x = glstate_matrix_modelview0[0].y;
    t1.y = glstate_matrix_modelview0[1].y;
    t1.z = glstate_matrix_modelview0[2].y;
    vs_TEXCOORD2.y = dot(t0.xyz, t1.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t1.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t1.xyz);
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
uniform 	mediump vec4 _MatCap_ST;
uniform 	mediump vec4 _Color;
uniform 	mediump vec4 _ShadowColor;
uniform 	mediump float _FadeDistance;
uniform 	mediump float _Falloff;
uniform 	mediump float _MeshWrapSizeX;
uniform 	mediump float _MeshWrapSizeZ;
uniform 	mediump float _Amplitude;
uniform 	mediump float _ReflIntensity;
uniform 	mediump float _OffsetY;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _MatCap;
uniform lowp samplerCube _EffectCube;
in mediump vec4 vs_COLOR0;
in mediump vec2 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
in highp vec3 vs_TEXCOORD4;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec3 t0;
mediump vec3 t16_0;
lowp vec3 t10_0;
highp vec2 t1;
mediump vec3 t16_1;
lowp vec3 t10_1;
lowp vec3 t10_2;
mediump vec2 t16_3;
mediump vec3 t16_7;
highp float t12;
void main()
{
    t0.x = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD3.xyz);
    t0.x = inversesqrt(t0.x);
    t0.xyz = t0.xxx * vs_TEXCOORD3.xyz;
    t12 = dot(vs_TEXCOORD4.xyz, t0.xyz);
    t12 = t12 + t12;
    t0.xyz = t0.xyz * (-vec3(t12)) + vs_TEXCOORD4.xyz;
    t10_0.xyz = texture(_EffectCube, t0.xyz).xyz;
    t16_0.xyz = t10_0.xyz * vec3(vec3(_ReflIntensity, _ReflIntensity, _ReflIntensity));
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD0.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t1.x = dot(vs_TEXCOORD1.xyz, t10_2.xyz);
    t1.y = dot(vs_TEXCOORD2.xyz, t10_2.xyz);
    t16_3.xy = t1.xy * vec2(0.5, 0.5) + _MatCap_ST.zw;
    t16_3.xy = t16_3.xy + vec2(0.5, 0.5);
    t10_1.xyz = texture(_MatCap, t16_3.xy).xyz;
    t16_1.xyz = t10_1.xyz * _Color.xyz;
    t16_1.xyz = t16_1.xyz + t16_1.xyz;
    t16_3.x = (-vs_COLOR0.z) + 1.0;
    t16_7.xyz = t16_1.xyz * t16_3.xxx;
    t16_7.xyz = _Color.xyz * vs_COLOR0.zzz + t16_7.xyz;
    t16_0.xyz = t16_0.xyz * t16_3.xxx + t16_7.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp mat4 unity_MatrixVP;
uniform mediump float _FadeDistance;
uniform mediump float _Falloff;
uniform mediump float _MeshWrapSizeX;
uniform mediump float _MeshWrapSizeZ;
uniform mediump float _Amplitude;
uniform mediump float _OffsetY;
varying mediump vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  mediump vec4 tmpvar_2;
  tmpvar_2.xyw = _glesColor.xyw;
  mediump float mDeg_3;
  mediump float distance_4;
  highp vec4 wPos_5;
  highp vec2 meshWrapSize_6;
  highp vec3 tmpvar_7;
  highp vec3 tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_8 = _glesTANGENT.xyz;
  tmpvar_9 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_10;
  tmpvar_10[0].x = tmpvar_8.x;
  tmpvar_10[0].y = tmpvar_9.x;
  tmpvar_10[0].z = tmpvar_1.x;
  tmpvar_10[1].x = tmpvar_8.y;
  tmpvar_10[1].y = tmpvar_9.y;
  tmpvar_10[1].z = tmpvar_1.y;
  tmpvar_10[2].x = tmpvar_8.z;
  tmpvar_10[2].y = tmpvar_9.z;
  tmpvar_10[2].z = tmpvar_1.z;
  highp vec2 tmpvar_11;
  tmpvar_11.x = _Object2World[3].x;
  tmpvar_11.y = _Object2World[3].z;
  mediump vec2 tmpvar_12;
  tmpvar_12.x = _MeshWrapSizeX;
  tmpvar_12.y = _MeshWrapSizeZ;
  meshWrapSize_6 = tmpvar_12;
  highp vec2 tmpvar_13;
  tmpvar_13 = ((vec2(
    ivec2(((tmpvar_11 / meshWrapSize_6) + (sign(tmpvar_11) * vec2(0.5, 0.5))))
  ) * meshWrapSize_6) + _glesVertex.xz);
  highp vec2 tmpvar_14;
  tmpvar_14 = (tmpvar_11 - tmpvar_13);
  wPos_5.xz = (tmpvar_13 + ((
    vec2(greaterThanEqual (abs(tmpvar_14), (meshWrapSize_6 * 0.5)))
   * 
    sign(tmpvar_14)
  ) * meshWrapSize_6));
  wPos_5.y = _OffsetY;
  wPos_5.w = 1.0;
  highp float tmpvar_15;
  highp vec2 x_16;
  x_16 = (wPos_5.xz - tmpvar_11);
  tmpvar_15 = sqrt(dot (x_16, x_16));
  distance_4 = tmpvar_15;
  mediump float tmpvar_17;
  tmpvar_17 = clamp ((_FadeDistance + (distance_4 * _Falloff)), 0.0, 1.0);
  highp float tmpvar_18;
  tmpvar_18 = ((wPos_5.z * wPos_5.x) * 0.0075);
  mDeg_3 = tmpvar_18;
  mDeg_3 = (mDeg_3 + (_Time.z * 0.5));
  highp vec4 v_19;
  v_19.x = _World2Object[0].z;
  v_19.y = _World2Object[1].z;
  v_19.z = _World2Object[2].z;
  v_19.w = _World2Object[3].z;
  wPos_5.xyz = (wPos_5.xyz - v_19.xyz);
  wPos_5.xz = (wPos_5.xz + _glesMultiTexCoord1.xy);
  wPos_5.y = (wPos_5.y - (_glesColor.z + (tmpvar_17 * _Amplitude)));
  mediump float tmpvar_20;
  tmpvar_20 = sin((mDeg_3 + (_glesColor.x * 10.0)));
  wPos_5.y = (wPos_5.y + ((tmpvar_20 * tmpvar_17) * 0.5));
  tmpvar_7 = ((wPos_5.xyz + v_19.xyz) - _WorldSpaceCameraPos);
  tmpvar_2.z = tmpvar_17;
  mediump float tmpvar_21;
  tmpvar_21 = sin((mDeg_3 + (_glesColor.x * 12.0)));
  highp vec4 tmpvar_22;
  tmpvar_22.w = 1.0;
  tmpvar_22.xyz = (wPos_5.xyz + v_19.xyz);
  wPos_5 = tmpvar_22;
  highp vec4 v_23;
  v_23.x = glstate_matrix_modelview0[0].x;
  v_23.y = glstate_matrix_modelview0[1].x;
  v_23.z = glstate_matrix_modelview0[2].x;
  v_23.w = glstate_matrix_modelview0[3].x;
  highp vec4 v_24;
  v_24.x = glstate_matrix_modelview0[0].y;
  v_24.y = glstate_matrix_modelview0[1].y;
  v_24.z = glstate_matrix_modelview0[2].y;
  v_24.w = glstate_matrix_modelview0[3].y;
  gl_Position = (unity_MatrixVP * tmpvar_22);
  xlv_COLOR = tmpvar_2;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = (tmpvar_10 * v_23.xyz);
  xlv_TEXCOORD2 = (tmpvar_10 * v_24.xyz);
  xlv_TEXCOORD3 = (_glesNormal + ((tmpvar_21 * 0.0125) * _glesColor.y));
  xlv_TEXCOORD4 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _MatCap_ST;
uniform mediump vec4 _Color;
uniform sampler2D _BumpMap;
uniform sampler2D _MatCap;
uniform lowp samplerCube _EffectCube;
uniform mediump float _ReflIntensity;
varying mediump vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  mediump vec4 finalColor_1;
  mediump vec3 reflectedDir_2;
  mediump vec2 vn_3;
  highp vec3 normal_4;
  lowp vec3 tmpvar_5;
  tmpvar_5 = ((texture2D (_BumpMap, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  normal_4 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = dot (xlv_TEXCOORD1, normal_4);
  vn_3.x = tmpvar_6;
  highp float tmpvar_7;
  tmpvar_7 = dot (xlv_TEXCOORD2, normal_4);
  vn_3.y = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize(xlv_TEXCOORD3);
  highp vec3 tmpvar_9;
  tmpvar_9 = (xlv_TEXCOORD4 - (2.0 * (
    dot (tmpvar_8, xlv_TEXCOORD4)
   * tmpvar_8)));
  reflectedDir_2 = tmpvar_9;
  lowp vec4 tmpvar_10;
  mediump vec2 P_11;
  P_11 = (((vn_3 * 0.5) + 0.5) + _MatCap_ST.zw);
  tmpvar_10 = texture2D (_MatCap, P_11);
  mediump float tmpvar_12;
  tmpvar_12 = (1.0 - xlv_COLOR.z);
  finalColor_1 = ((_Color * xlv_COLOR.z) + ((tmpvar_10 * _Color) * (2.0 * tmpvar_12)));
  lowp vec4 tmpvar_13;
  tmpvar_13 = textureCube (_EffectCube, reflectedDir_2);
  finalColor_1.xyz = (finalColor_1 + ((tmpvar_13 * _ReflIntensity) * tmpvar_12)).xyz;
  finalColor_1.w = 1.0;
  gl_FragData[0] = finalColor_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" }
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
uniform 	mediump vec4 _MatCap_ST;
uniform 	mediump vec4 _Color;
uniform 	mediump vec4 _ShadowColor;
uniform 	mediump float _FadeDistance;
uniform 	mediump float _Falloff;
uniform 	mediump float _MeshWrapSizeX;
uniform 	mediump float _MeshWrapSizeZ;
uniform 	mediump float _Amplitude;
uniform 	mediump float _ReflIntensity;
uniform 	mediump float _OffsetY;
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in mediump vec4 in_TEXCOORD0;
in mediump vec4 in_TEXCOORD1;
in highp vec3 in_NORMAL0;
in highp vec4 in_TANGENT0;
out mediump vec4 vs_COLOR0;
out mediump vec2 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
out highp vec3 vs_TEXCOORD4;
highp vec3 t0;
highp ivec2 ti0;
highp vec4 t1;
highp ivec2 ti1;
mediump vec2 t16_2;
highp vec3 t3;
mediump float t16_6;
highp vec2 t8;
highp ivec2 ti8;
bvec2 tb8;
highp vec2 t9;
highp ivec2 ti9;
mediump float t16_10;
highp float t12;
void main()
{
    ti0.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), _Object2World[3].xzxx).xy) * 0xFFFFFFFFu);
    ti8.xy = ivec2(uvec2(lessThan(_Object2World[3].xzxz, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti0.xy = (-ti0.xy) + ti8.xy;
    t0.xy = vec2(ti0.xy);
    t8.xy = vec2(_Object2World[3].x / float(_MeshWrapSizeX), _Object2World[3].z / float(_MeshWrapSizeZ));
    t0.xy = t0.xy * vec2(0.5, 0.5) + t8.xy;
    t0.xy = trunc(t0.xy);
    t0.xy = t0.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + in_POSITION0.xz;
    t8.xy = (-t0.xy) + _Object2World[3].xz;
    ti1.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), t8.xyxx).xy) * 0xFFFFFFFFu);
    ti9.xy = ivec2(uvec2(lessThan(t8.xyxy, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti1.xy = (-ti1.xy) + ti9.xy;
    t1.xy = vec2(ti1.xy);
    t9.xy = vec2(float(_MeshWrapSizeX) * float(0.5), float(_MeshWrapSizeZ) * float(0.5));
    tb8.xy = greaterThanEqual(abs(t8.xyxy), t9.xyxy).xy;
    t8.xy = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb8.xy));
    t8.xy = t1.xy * t8.xy;
    t0.xy = t8.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + t0.xy;
    t8.x = t0.x * t0.y;
    t12 = _Time.z * 0.5;
    t8.x = t8.x * 0.00749999983 + t12;
    t16_2.xy = in_COLOR0.xx * vec2(10.0, 12.0) + t8.xx;
    t16_2.xy = sin(t16_2.xy);
    t8.xy = t0.xy + (-_Object2World[3].xz);
    t8.x = dot(t8.xy, t8.xy);
    t8.x = sqrt(t8.x);
    t16_10 = t8.x * _Falloff + _FadeDistance;
    t16_10 = clamp(t16_10, 0.0, 1.0);
    t16_2.x = t16_10 * t16_2.x;
    t16_6 = t16_2.y * in_COLOR0.y;
    vs_TEXCOORD3.xyz = vec3(t16_6) * vec3(0.0125000002, 0.0125000002, 0.0125000002) + in_NORMAL0.xyz;
    t16_6 = t16_10 * _Amplitude + in_COLOR0.z;
    vs_COLOR0.z = t16_10;
    t1.y = _World2Object[1].z;
    t8.x = (-t1.y) + _OffsetY;
    t8.x = (-t16_6) + t8.x;
    t3.y = t16_2.x * 0.5 + t8.x;
    t1.x = _World2Object[0].z;
    t1.z = _World2Object[2].z;
    t0.xy = t0.xy + (-t1.xz);
    t3.xz = t0.xy + in_TEXCOORD1.xy;
    t0.xyz = t1.xyz + t3.xyz;
    t1 = t0.yyyy * unity_MatrixVP[1];
    t1 = unity_MatrixVP[0] * t0.xxxx + t1;
    t1 = unity_MatrixVP[2] * t0.zzzz + t1;
    vs_TEXCOORD4.xyz = t0.xyz + (-_WorldSpaceCameraPos.xyzx.xyz);
    gl_Position = t1 + unity_MatrixVP[3];
    vs_COLOR0.xyw = in_COLOR0.xyw;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t0.xyz = in_NORMAL0.zxy * in_TANGENT0.yzx;
    t0.xyz = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t0.xyz);
    t0.xyz = t0.xyz * in_TANGENT0.www;
    t1.x = glstate_matrix_modelview0[0].x;
    t1.y = glstate_matrix_modelview0[1].x;
    t1.z = glstate_matrix_modelview0[2].x;
    vs_TEXCOORD1.y = dot(t0.xyz, t1.xyz);
    vs_TEXCOORD1.x = dot(in_TANGENT0.xyz, t1.xyz);
    vs_TEXCOORD1.z = dot(in_NORMAL0.xyz, t1.xyz);
    t1.x = glstate_matrix_modelview0[0].y;
    t1.y = glstate_matrix_modelview0[1].y;
    t1.z = glstate_matrix_modelview0[2].y;
    vs_TEXCOORD2.y = dot(t0.xyz, t1.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t1.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t1.xyz);
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
uniform 	mediump vec4 _MatCap_ST;
uniform 	mediump vec4 _Color;
uniform 	mediump vec4 _ShadowColor;
uniform 	mediump float _FadeDistance;
uniform 	mediump float _Falloff;
uniform 	mediump float _MeshWrapSizeX;
uniform 	mediump float _MeshWrapSizeZ;
uniform 	mediump float _Amplitude;
uniform 	mediump float _ReflIntensity;
uniform 	mediump float _OffsetY;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _MatCap;
uniform lowp samplerCube _EffectCube;
in mediump vec4 vs_COLOR0;
in mediump vec2 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
in highp vec3 vs_TEXCOORD4;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec3 t0;
mediump vec3 t16_0;
lowp vec3 t10_0;
highp vec2 t1;
mediump vec3 t16_1;
lowp vec3 t10_1;
lowp vec3 t10_2;
mediump vec2 t16_3;
mediump vec3 t16_7;
highp float t12;
void main()
{
    t0.x = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD3.xyz);
    t0.x = inversesqrt(t0.x);
    t0.xyz = t0.xxx * vs_TEXCOORD3.xyz;
    t12 = dot(vs_TEXCOORD4.xyz, t0.xyz);
    t12 = t12 + t12;
    t0.xyz = t0.xyz * (-vec3(t12)) + vs_TEXCOORD4.xyz;
    t10_0.xyz = texture(_EffectCube, t0.xyz).xyz;
    t16_0.xyz = t10_0.xyz * vec3(vec3(_ReflIntensity, _ReflIntensity, _ReflIntensity));
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD0.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t1.x = dot(vs_TEXCOORD1.xyz, t10_2.xyz);
    t1.y = dot(vs_TEXCOORD2.xyz, t10_2.xyz);
    t16_3.xy = t1.xy * vec2(0.5, 0.5) + _MatCap_ST.zw;
    t16_3.xy = t16_3.xy + vec2(0.5, 0.5);
    t10_1.xyz = texture(_MatCap, t16_3.xy).xyz;
    t16_1.xyz = t10_1.xyz * _Color.xyz;
    t16_1.xyz = t16_1.xyz + t16_1.xyz;
    t16_3.x = (-vs_COLOR0.z) + 1.0;
    t16_7.xyz = t16_1.xyz * t16_3.xxx;
    t16_7.xyz = _Color.xyz * vs_COLOR0.zzz + t16_7.xyz;
    t16_0.xyz = t16_0.xyz * t16_3.xxx + t16_7.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp mat4 unity_MatrixVP;
uniform mediump float _FadeDistance;
uniform mediump float _Falloff;
uniform mediump float _MeshWrapSizeX;
uniform mediump float _MeshWrapSizeZ;
uniform mediump float _Amplitude;
uniform mediump float _OffsetY;
varying mediump vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  mediump vec4 tmpvar_2;
  tmpvar_2.xyw = _glesColor.xyw;
  mediump float mDeg_3;
  mediump float distance_4;
  highp vec4 wPos_5;
  highp vec2 meshWrapSize_6;
  highp vec3 tmpvar_7;
  mediump vec4 tmpvar_8;
  highp vec3 tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_9 = _glesTANGENT.xyz;
  tmpvar_10 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_11;
  tmpvar_11[0].x = tmpvar_9.x;
  tmpvar_11[0].y = tmpvar_10.x;
  tmpvar_11[0].z = tmpvar_1.x;
  tmpvar_11[1].x = tmpvar_9.y;
  tmpvar_11[1].y = tmpvar_10.y;
  tmpvar_11[1].z = tmpvar_1.y;
  tmpvar_11[2].x = tmpvar_9.z;
  tmpvar_11[2].y = tmpvar_10.z;
  tmpvar_11[2].z = tmpvar_1.z;
  highp vec2 tmpvar_12;
  tmpvar_12.x = _Object2World[3].x;
  tmpvar_12.y = _Object2World[3].z;
  mediump vec2 tmpvar_13;
  tmpvar_13.x = _MeshWrapSizeX;
  tmpvar_13.y = _MeshWrapSizeZ;
  meshWrapSize_6 = tmpvar_13;
  highp vec2 tmpvar_14;
  tmpvar_14 = ((vec2(
    ivec2(((tmpvar_12 / meshWrapSize_6) + (sign(tmpvar_12) * vec2(0.5, 0.5))))
  ) * meshWrapSize_6) + _glesVertex.xz);
  highp vec2 tmpvar_15;
  tmpvar_15 = (tmpvar_12 - tmpvar_14);
  wPos_5.xz = (tmpvar_14 + ((
    vec2(greaterThanEqual (abs(tmpvar_15), (meshWrapSize_6 * 0.5)))
   * 
    sign(tmpvar_15)
  ) * meshWrapSize_6));
  wPos_5.y = _OffsetY;
  wPos_5.w = 1.0;
  highp float tmpvar_16;
  highp vec2 x_17;
  x_17 = (wPos_5.xz - tmpvar_12);
  tmpvar_16 = sqrt(dot (x_17, x_17));
  distance_4 = tmpvar_16;
  mediump float tmpvar_18;
  tmpvar_18 = clamp ((_FadeDistance + (distance_4 * _Falloff)), 0.0, 1.0);
  highp float tmpvar_19;
  tmpvar_19 = ((wPos_5.z * wPos_5.x) * 0.0075);
  mDeg_3 = tmpvar_19;
  mDeg_3 = (mDeg_3 + (_Time.z * 0.5));
  highp vec4 v_20;
  v_20.x = _World2Object[0].z;
  v_20.y = _World2Object[1].z;
  v_20.z = _World2Object[2].z;
  v_20.w = _World2Object[3].z;
  wPos_5.xyz = (wPos_5.xyz - v_20.xyz);
  wPos_5.xz = (wPos_5.xz + _glesMultiTexCoord1.xy);
  wPos_5.y = (wPos_5.y - (_glesColor.z + (tmpvar_18 * _Amplitude)));
  mediump float tmpvar_21;
  tmpvar_21 = sin((mDeg_3 + (_glesColor.x * 10.0)));
  wPos_5.y = (wPos_5.y + ((tmpvar_21 * tmpvar_18) * 0.5));
  tmpvar_7 = ((wPos_5.xyz + v_20.xyz) - _WorldSpaceCameraPos);
  tmpvar_2.z = tmpvar_18;
  mediump float tmpvar_22;
  tmpvar_22 = sin((mDeg_3 + (_glesColor.x * 12.0)));
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = (wPos_5.xyz + v_20.xyz);
  wPos_5 = tmpvar_23;
  highp vec4 v_24;
  v_24.x = glstate_matrix_modelview0[0].x;
  v_24.y = glstate_matrix_modelview0[1].x;
  v_24.z = glstate_matrix_modelview0[2].x;
  v_24.w = glstate_matrix_modelview0[3].x;
  highp vec4 v_25;
  v_25.x = glstate_matrix_modelview0[0].y;
  v_25.y = glstate_matrix_modelview0[1].y;
  v_25.z = glstate_matrix_modelview0[2].y;
  v_25.w = glstate_matrix_modelview0[3].y;
  tmpvar_8 = (unity_World2Shadow[0] * tmpvar_23);
  gl_Position = (unity_MatrixVP * tmpvar_23);
  xlv_COLOR = tmpvar_2;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = (tmpvar_11 * v_24.xyz);
  xlv_TEXCOORD2 = (tmpvar_11 * v_25.xyz);
  xlv_TEXCOORD3 = (_glesNormal + ((tmpvar_22 * 0.0125) * _glesColor.y));
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
uniform mediump vec4 _MatCap_ST;
uniform mediump vec4 _Color;
uniform mediump vec4 _ShadowColor;
uniform sampler2D _BumpMap;
uniform sampler2D _MatCap;
uniform lowp samplerCube _EffectCube;
uniform mediump float _ReflIntensity;
varying mediump vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp float atten_1;
  mediump vec4 finalColor_2;
  mediump vec3 reflectedDir_3;
  mediump vec2 vn_4;
  highp vec3 normal_5;
  lowp vec3 tmpvar_6;
  tmpvar_6 = ((texture2D (_BumpMap, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  normal_5 = tmpvar_6;
  highp float tmpvar_7;
  tmpvar_7 = dot (xlv_TEXCOORD1, normal_5);
  vn_4.x = tmpvar_7;
  highp float tmpvar_8;
  tmpvar_8 = dot (xlv_TEXCOORD2, normal_5);
  vn_4.y = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize(xlv_TEXCOORD3);
  highp vec3 tmpvar_10;
  tmpvar_10 = (xlv_TEXCOORD4 - (2.0 * (
    dot (tmpvar_9, xlv_TEXCOORD4)
   * tmpvar_9)));
  reflectedDir_3 = tmpvar_10;
  lowp vec4 tmpvar_11;
  mediump vec2 P_12;
  P_12 = (((vn_4 * 0.5) + 0.5) + _MatCap_ST.zw);
  tmpvar_11 = texture2D (_MatCap, P_12);
  mediump float tmpvar_13;
  tmpvar_13 = (1.0 - xlv_COLOR.z);
  finalColor_2 = ((_Color * xlv_COLOR.z) + ((tmpvar_11 * _Color) * (2.0 * tmpvar_13)));
  lowp vec4 tmpvar_14;
  tmpvar_14 = textureCube (_EffectCube, reflectedDir_3);
  finalColor_2 = (finalColor_2 + ((tmpvar_14 * _ReflIntensity) * tmpvar_13));
  lowp float tmpvar_15;
  highp float tmpvar_16;
  tmpvar_16 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD5).x > 
    (xlv_TEXCOORD5.z / xlv_TEXCOORD5.w)
  )), _LightShadowData.x);
  tmpvar_15 = tmpvar_16;
  mediump float tmpvar_17;
  tmpvar_17 = ((1.0 - tmpvar_15) * tmpvar_13);
  atten_1 = tmpvar_17;
  finalColor_2.xyz = mix (finalColor_2.xyz, _ShadowColor.xyz, vec3((atten_1 * _ShadowColor.w)));
  finalColor_2.w = 1.0;
  gl_FragData[0] = finalColor_2;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
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
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp mat4 unity_MatrixVP;
uniform mediump float _FadeDistance;
uniform mediump float _Falloff;
uniform mediump float _MeshWrapSizeX;
uniform mediump float _MeshWrapSizeZ;
uniform mediump float _Amplitude;
uniform mediump float _OffsetY;
varying mediump vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  mediump vec4 tmpvar_2;
  tmpvar_2.xyw = _glesColor.xyw;
  mediump float mDeg_3;
  mediump float distance_4;
  highp vec4 wPos_5;
  highp vec2 meshWrapSize_6;
  highp vec3 tmpvar_7;
  mediump vec4 tmpvar_8;
  highp vec3 tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_9 = _glesTANGENT.xyz;
  tmpvar_10 = (((_glesNormal.yzx * _glesTANGENT.zxy) - (_glesNormal.zxy * _glesTANGENT.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_11;
  tmpvar_11[0].x = tmpvar_9.x;
  tmpvar_11[0].y = tmpvar_10.x;
  tmpvar_11[0].z = tmpvar_1.x;
  tmpvar_11[1].x = tmpvar_9.y;
  tmpvar_11[1].y = tmpvar_10.y;
  tmpvar_11[1].z = tmpvar_1.y;
  tmpvar_11[2].x = tmpvar_9.z;
  tmpvar_11[2].y = tmpvar_10.z;
  tmpvar_11[2].z = tmpvar_1.z;
  highp vec2 tmpvar_12;
  tmpvar_12.x = _Object2World[3].x;
  tmpvar_12.y = _Object2World[3].z;
  mediump vec2 tmpvar_13;
  tmpvar_13.x = _MeshWrapSizeX;
  tmpvar_13.y = _MeshWrapSizeZ;
  meshWrapSize_6 = tmpvar_13;
  highp vec2 tmpvar_14;
  tmpvar_14 = ((vec2(
    ivec2(((tmpvar_12 / meshWrapSize_6) + (sign(tmpvar_12) * vec2(0.5, 0.5))))
  ) * meshWrapSize_6) + _glesVertex.xz);
  highp vec2 tmpvar_15;
  tmpvar_15 = (tmpvar_12 - tmpvar_14);
  wPos_5.xz = (tmpvar_14 + ((
    vec2(greaterThanEqual (abs(tmpvar_15), (meshWrapSize_6 * 0.5)))
   * 
    sign(tmpvar_15)
  ) * meshWrapSize_6));
  wPos_5.y = _OffsetY;
  wPos_5.w = 1.0;
  highp float tmpvar_16;
  highp vec2 x_17;
  x_17 = (wPos_5.xz - tmpvar_12);
  tmpvar_16 = sqrt(dot (x_17, x_17));
  distance_4 = tmpvar_16;
  mediump float tmpvar_18;
  tmpvar_18 = clamp ((_FadeDistance + (distance_4 * _Falloff)), 0.0, 1.0);
  highp float tmpvar_19;
  tmpvar_19 = ((wPos_5.z * wPos_5.x) * 0.0075);
  mDeg_3 = tmpvar_19;
  mDeg_3 = (mDeg_3 + (_Time.z * 0.5));
  highp vec4 v_20;
  v_20.x = _World2Object[0].z;
  v_20.y = _World2Object[1].z;
  v_20.z = _World2Object[2].z;
  v_20.w = _World2Object[3].z;
  wPos_5.xyz = (wPos_5.xyz - v_20.xyz);
  wPos_5.xz = (wPos_5.xz + _glesMultiTexCoord1.xy);
  wPos_5.y = (wPos_5.y - (_glesColor.z + (tmpvar_18 * _Amplitude)));
  mediump float tmpvar_21;
  tmpvar_21 = sin((mDeg_3 + (_glesColor.x * 10.0)));
  wPos_5.y = (wPos_5.y + ((tmpvar_21 * tmpvar_18) * 0.5));
  tmpvar_7 = ((wPos_5.xyz + v_20.xyz) - _WorldSpaceCameraPos);
  tmpvar_2.z = tmpvar_18;
  mediump float tmpvar_22;
  tmpvar_22 = sin((mDeg_3 + (_glesColor.x * 12.0)));
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = (wPos_5.xyz + v_20.xyz);
  wPos_5 = tmpvar_23;
  highp vec4 v_24;
  v_24.x = glstate_matrix_modelview0[0].x;
  v_24.y = glstate_matrix_modelview0[1].x;
  v_24.z = glstate_matrix_modelview0[2].x;
  v_24.w = glstate_matrix_modelview0[3].x;
  highp vec4 v_25;
  v_25.x = glstate_matrix_modelview0[0].y;
  v_25.y = glstate_matrix_modelview0[1].y;
  v_25.z = glstate_matrix_modelview0[2].y;
  v_25.w = glstate_matrix_modelview0[3].y;
  tmpvar_8 = (unity_World2Shadow[0] * tmpvar_23);
  gl_Position = (unity_MatrixVP * tmpvar_23);
  xlv_COLOR = tmpvar_2;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = (tmpvar_11 * v_24.xyz);
  xlv_TEXCOORD2 = (tmpvar_11 * v_25.xyz);
  xlv_TEXCOORD3 = (_glesNormal + ((tmpvar_22 * 0.0125) * _glesColor.y));
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform mediump vec4 _MatCap_ST;
uniform mediump vec4 _Color;
uniform mediump vec4 _ShadowColor;
uniform sampler2D _BumpMap;
uniform sampler2D _MatCap;
uniform lowp samplerCube _EffectCube;
uniform mediump float _ReflIntensity;
varying mediump vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp float atten_1;
  mediump vec4 finalColor_2;
  mediump vec3 reflectedDir_3;
  mediump vec2 vn_4;
  highp vec3 normal_5;
  lowp vec3 tmpvar_6;
  tmpvar_6 = ((texture2D (_BumpMap, xlv_TEXCOORD0).xyz * 2.0) - 1.0);
  normal_5 = tmpvar_6;
  highp float tmpvar_7;
  tmpvar_7 = dot (xlv_TEXCOORD1, normal_5);
  vn_4.x = tmpvar_7;
  highp float tmpvar_8;
  tmpvar_8 = dot (xlv_TEXCOORD2, normal_5);
  vn_4.y = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize(xlv_TEXCOORD3);
  highp vec3 tmpvar_10;
  tmpvar_10 = (xlv_TEXCOORD4 - (2.0 * (
    dot (tmpvar_9, xlv_TEXCOORD4)
   * tmpvar_9)));
  reflectedDir_3 = tmpvar_10;
  lowp vec4 tmpvar_11;
  mediump vec2 P_12;
  P_12 = (((vn_4 * 0.5) + 0.5) + _MatCap_ST.zw);
  tmpvar_11 = texture2D (_MatCap, P_12);
  mediump float tmpvar_13;
  tmpvar_13 = (1.0 - xlv_COLOR.z);
  finalColor_2 = ((_Color * xlv_COLOR.z) + ((tmpvar_11 * _Color) * (2.0 * tmpvar_13)));
  lowp vec4 tmpvar_14;
  tmpvar_14 = textureCube (_EffectCube, reflectedDir_3);
  finalColor_2 = (finalColor_2 + ((tmpvar_14 * _ReflIntensity) * tmpvar_13));
  lowp float shadow_15;
  shadow_15 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD5.xyz) * (1.0 - _LightShadowData.x)));
  mediump float tmpvar_16;
  tmpvar_16 = ((1.0 - shadow_15) * tmpvar_13);
  atten_1 = tmpvar_16;
  finalColor_2.xyz = mix (finalColor_2.xyz, _ShadowColor.xyz, vec3((atten_1 * _ShadowColor.w)));
  finalColor_2.w = 1.0;
  gl_FragData[0] = finalColor_2;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
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
uniform 	mediump vec4 _MatCap_ST;
uniform 	mediump vec4 _Color;
uniform 	mediump vec4 _ShadowColor;
uniform 	mediump float _FadeDistance;
uniform 	mediump float _Falloff;
uniform 	mediump float _MeshWrapSizeX;
uniform 	mediump float _MeshWrapSizeZ;
uniform 	mediump float _Amplitude;
uniform 	mediump float _ReflIntensity;
uniform 	mediump float _OffsetY;
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in mediump vec4 in_TEXCOORD0;
in mediump vec4 in_TEXCOORD1;
in highp vec3 in_NORMAL0;
in highp vec4 in_TANGENT0;
out mediump vec4 vs_COLOR0;
out mediump vec2 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
out highp vec3 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
highp vec4 t0;
highp ivec2 ti0;
highp vec4 t1;
highp ivec2 ti1;
mediump vec2 t16_2;
highp vec3 t3;
mediump float t16_6;
highp vec2 t8;
highp ivec2 ti8;
bvec2 tb8;
highp vec2 t9;
highp ivec2 ti9;
mediump float t16_10;
highp float t12;
void main()
{
    ti0.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), _Object2World[3].xzxx).xy) * 0xFFFFFFFFu);
    ti8.xy = ivec2(uvec2(lessThan(_Object2World[3].xzxz, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti0.xy = (-ti0.xy) + ti8.xy;
    t0.xy = vec2(ti0.xy);
    t8.xy = vec2(_Object2World[3].x / float(_MeshWrapSizeX), _Object2World[3].z / float(_MeshWrapSizeZ));
    t0.xy = t0.xy * vec2(0.5, 0.5) + t8.xy;
    t0.xy = trunc(t0.xy);
    t0.xy = t0.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + in_POSITION0.xz;
    t8.xy = (-t0.xy) + _Object2World[3].xz;
    ti1.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), t8.xyxx).xy) * 0xFFFFFFFFu);
    ti9.xy = ivec2(uvec2(lessThan(t8.xyxy, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti1.xy = (-ti1.xy) + ti9.xy;
    t1.xy = vec2(ti1.xy);
    t9.xy = vec2(float(_MeshWrapSizeX) * float(0.5), float(_MeshWrapSizeZ) * float(0.5));
    tb8.xy = greaterThanEqual(abs(t8.xyxy), t9.xyxy).xy;
    t8.xy = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb8.xy));
    t8.xy = t1.xy * t8.xy;
    t0.xy = t8.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + t0.xy;
    t8.x = t0.x * t0.y;
    t12 = _Time.z * 0.5;
    t8.x = t8.x * 0.00749999983 + t12;
    t16_2.xy = in_COLOR0.xx * vec2(10.0, 12.0) + t8.xx;
    t16_2.xy = sin(t16_2.xy);
    t8.xy = t0.xy + (-_Object2World[3].xz);
    t8.x = dot(t8.xy, t8.xy);
    t8.x = sqrt(t8.x);
    t16_10 = t8.x * _Falloff + _FadeDistance;
    t16_10 = clamp(t16_10, 0.0, 1.0);
    t16_2.x = t16_10 * t16_2.x;
    t16_6 = t16_2.y * in_COLOR0.y;
    vs_TEXCOORD3.xyz = vec3(t16_6) * vec3(0.0125000002, 0.0125000002, 0.0125000002) + in_NORMAL0.xyz;
    t16_6 = t16_10 * _Amplitude + in_COLOR0.z;
    vs_COLOR0.z = t16_10;
    t1.y = _World2Object[1].z;
    t8.x = (-t1.y) + _OffsetY;
    t8.x = (-t16_6) + t8.x;
    t3.y = t16_2.x * 0.5 + t8.x;
    t1.x = _World2Object[0].z;
    t1.z = _World2Object[2].z;
    t0.xy = t0.xy + (-t1.xz);
    t3.xz = t0.xy + in_TEXCOORD1.xy;
    t0.xyz = t1.xyz + t3.xyz;
    t1 = t0.yyyy * unity_MatrixVP[1];
    t1 = unity_MatrixVP[0] * t0.xxxx + t1;
    t1 = unity_MatrixVP[2] * t0.zzzz + t1;
    gl_Position = t1 + unity_MatrixVP[3];
    vs_COLOR0.xyw = in_COLOR0.xyw;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    t1.xyz = in_NORMAL0.zxy * in_TANGENT0.yzx;
    t1.xyz = in_NORMAL0.yzx * in_TANGENT0.zxy + (-t1.xyz);
    t1.xyz = t1.xyz * in_TANGENT0.www;
    t3.x = glstate_matrix_modelview0[0].x;
    t3.y = glstate_matrix_modelview0[1].x;
    t3.z = glstate_matrix_modelview0[2].x;
    vs_TEXCOORD1.y = dot(t1.xyz, t3.xyz);
    vs_TEXCOORD1.x = dot(in_TANGENT0.xyz, t3.xyz);
    vs_TEXCOORD1.z = dot(in_NORMAL0.xyz, t3.xyz);
    t3.x = glstate_matrix_modelview0[0].y;
    t3.y = glstate_matrix_modelview0[1].y;
    t3.z = glstate_matrix_modelview0[2].y;
    vs_TEXCOORD2.y = dot(t1.xyz, t3.xyz);
    vs_TEXCOORD2.x = dot(in_TANGENT0.xyz, t3.xyz);
    vs_TEXCOORD2.z = dot(in_NORMAL0.xyz, t3.xyz);
    vs_TEXCOORD4.xyz = t0.xyz + (-_WorldSpaceCameraPos.xyzx.xyz);
    t1 = t0.yyyy * unity_World2Shadow[0][1];
    t1 = unity_World2Shadow[0][0] * t0.xxxx + t1;
    t0 = unity_World2Shadow[0][2] * t0.zzzz + t1;
    t0 = t0 + unity_World2Shadow[0][3];
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
uniform 	mediump vec4 _MatCap_ST;
uniform 	mediump vec4 _Color;
uniform 	mediump vec4 _ShadowColor;
uniform 	mediump float _FadeDistance;
uniform 	mediump float _Falloff;
uniform 	mediump float _MeshWrapSizeX;
uniform 	mediump float _MeshWrapSizeZ;
uniform 	mediump float _Amplitude;
uniform 	mediump float _ReflIntensity;
uniform 	mediump float _OffsetY;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _MatCap;
uniform lowp samplerCube _EffectCube;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_COLOR0;
in mediump vec2 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
in highp vec3 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec3 t0;
mediump vec3 t16_0;
lowp vec3 t10_0;
highp vec2 t1;
mediump vec3 t16_1;
lowp vec3 t10_1;
lowp vec3 t10_2;
mediump vec2 t16_3;
mediump float t16_4;
mediump vec3 t16_8;
mediump float t16_9;
highp float t15;
void main()
{
    t0.x = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD3.xyz);
    t0.x = inversesqrt(t0.x);
    t0.xyz = t0.xxx * vs_TEXCOORD3.xyz;
    t15 = dot(vs_TEXCOORD4.xyz, t0.xyz);
    t15 = t15 + t15;
    t0.xyz = t0.xyz * (-vec3(t15)) + vs_TEXCOORD4.xyz;
    t10_0.xyz = texture(_EffectCube, t0.xyz).xyz;
    t16_0.xyz = t10_0.xyz * vec3(vec3(_ReflIntensity, _ReflIntensity, _ReflIntensity));
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD0.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t1.x = dot(vs_TEXCOORD1.xyz, t10_2.xyz);
    t1.y = dot(vs_TEXCOORD2.xyz, t10_2.xyz);
    t16_3.xy = t1.xy * vec2(0.5, 0.5) + _MatCap_ST.zw;
    t16_3.xy = t16_3.xy + vec2(0.5, 0.5);
    t10_1.xyz = texture(_MatCap, t16_3.xy).xyz;
    t16_1.xyz = t10_1.xyz * _Color.xyz;
    t16_1.xyz = t16_1.xyz + t16_1.xyz;
    t16_3.x = (-vs_COLOR0.z) + 1.0;
    t16_8.xyz = t16_1.xyz * t16_3.xxx;
    t16_8.xyz = _Color.xyz * vs_COLOR0.zzz + t16_8.xyz;
    t16_0.xyz = t16_0.xyz * t16_3.xxx + t16_8.xyz;
    t16_8.xyz = (-t16_0.xyz) + _ShadowColor.xyz;
    vec3 txVec20 = vec3(vs_TEXCOORD5.xy,vs_TEXCOORD5.z);
    t16_4 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec20, 0.0);
    t16_9 = (-_LightShadowData.x) + 1.0;
    t16_4 = t16_4 * t16_9 + _LightShadowData.x;
    t10_2.x = (-t16_4) + 1.0;
    t16_3.x = t16_3.x * t10_2.x;
    t16_3.x = t16_3.x * _ShadowColor.w;
    SV_Target0.xyz = t16_3.xxx * t16_8.xyz + t16_0.xyz;
    SV_Target0.w = 1.0;
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES3"
}
}
 }
 Pass {
  Name "SHADOWCASTER"
  Tags { "LIGHTMODE"="SHADOWCASTER" "QUEUE"="Background+1" "SHADOWSUPPORT"="true" }
  Cull Off
  GpuProgramID 83613
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_DEPTH" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec4 unity_LightShadowBias;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp mat4 unity_MatrixVP;
uniform mediump float _FadeDistance;
uniform mediump float _Falloff;
uniform mediump float _MeshWrapSizeX;
uniform mediump float _MeshWrapSizeZ;
uniform mediump float _Amplitude;
uniform mediump float _OffsetY;
void main ()
{
  mediump float mDeg_1;
  mediump float distance_2;
  highp vec4 wPos_3;
  highp vec2 meshWrapSize_4;
  highp vec4 tmpvar_5;
  highp vec2 tmpvar_6;
  tmpvar_6.x = _Object2World[3].x;
  tmpvar_6.y = _Object2World[3].z;
  mediump vec2 tmpvar_7;
  tmpvar_7.x = _MeshWrapSizeX;
  tmpvar_7.y = _MeshWrapSizeZ;
  meshWrapSize_4 = tmpvar_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = ((vec2(
    ivec2(((tmpvar_6 / meshWrapSize_4) + (sign(tmpvar_6) * vec2(0.5, 0.5))))
  ) * meshWrapSize_4) + _glesVertex.xz);
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_6 - tmpvar_8);
  wPos_3.xz = (tmpvar_8 + ((
    vec2(greaterThanEqual (abs(tmpvar_9), (meshWrapSize_4 * 0.5)))
   * 
    sign(tmpvar_9)
  ) * meshWrapSize_4));
  wPos_3.y = _OffsetY;
  wPos_3.w = 1.0;
  highp float tmpvar_10;
  highp vec2 x_11;
  x_11 = (wPos_3.xz - tmpvar_6);
  tmpvar_10 = sqrt(dot (x_11, x_11));
  distance_2 = tmpvar_10;
  mediump float tmpvar_12;
  tmpvar_12 = clamp ((_FadeDistance + (distance_2 * _Falloff)), 0.0, 1.0);
  highp float tmpvar_13;
  tmpvar_13 = ((wPos_3.z * wPos_3.x) * 0.0075);
  mDeg_1 = tmpvar_13;
  mDeg_1 = (mDeg_1 + (_Time.z * 0.5));
  highp vec4 v_14;
  v_14.x = _World2Object[0].z;
  v_14.y = _World2Object[1].z;
  v_14.z = _World2Object[2].z;
  v_14.w = _World2Object[3].z;
  wPos_3.xyz = (wPos_3.xyz - v_14.xyz);
  wPos_3.xz = (wPos_3.xz + _glesMultiTexCoord1.xy);
  wPos_3.y = (wPos_3.y - (_glesColor.z + (tmpvar_12 * _Amplitude)));
  mediump float tmpvar_15;
  tmpvar_15 = sin((mDeg_1 + (_glesColor.x * 10.0)));
  wPos_3.y = (wPos_3.y + ((tmpvar_15 * tmpvar_12) * 0.5));
  wPos_3.xyz = (wPos_3.xyz + v_14.xyz);
  highp vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = wPos_3.xyz;
  tmpvar_5 = (unity_MatrixVP * tmpvar_16);
  highp vec4 clipPos_17;
  clipPos_17.xyw = tmpvar_5.xyw;
  clipPos_17.z = (tmpvar_5.z + clamp ((unity_LightShadowBias.x / tmpvar_5.w), 0.0, 1.0));
  clipPos_17.z = mix (clipPos_17.z, max (clipPos_17.z, -(tmpvar_5.w)), unity_LightShadowBias.y);
  tmpvar_5 = clipPos_17;
  gl_Position = clipPos_17;
}


#endif
#ifdef FRAGMENT
void main ()
{
  gl_FragData[0] = vec4(0.0, 0.0, 0.0, 0.0);
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_DEPTH" }
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
uniform 	mediump float _FadeDistance;
uniform 	mediump float _Falloff;
uniform 	mediump float _MeshWrapSizeX;
uniform 	mediump float _MeshWrapSizeZ;
uniform 	mediump float _Amplitude;
uniform 	mediump float _OffsetY;
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in mediump vec4 in_TEXCOORD1;
highp vec4 t0;
highp ivec2 ti0;
highp vec4 t1;
highp ivec2 ti1;
mediump float t16_2;
highp vec3 t3;
mediump float t16_6;
highp vec2 t8;
highp ivec2 ti8;
bvec2 tb8;
highp vec2 t9;
highp ivec2 ti9;
highp float t12;
void main()
{
    ti0.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), _Object2World[3].xzxx).xy) * 0xFFFFFFFFu);
    ti8.xy = ivec2(uvec2(lessThan(_Object2World[3].xzxz, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti0.xy = (-ti0.xy) + ti8.xy;
    t0.xy = vec2(ti0.xy);
    t8.xy = vec2(_Object2World[3].x / float(_MeshWrapSizeX), _Object2World[3].z / float(_MeshWrapSizeZ));
    t0.xy = t0.xy * vec2(0.5, 0.5) + t8.xy;
    t0.xy = trunc(t0.xy);
    t0.xy = t0.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + in_POSITION0.xz;
    t8.xy = (-t0.xy) + _Object2World[3].xz;
    ti1.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), t8.xyxx).xy) * 0xFFFFFFFFu);
    ti9.xy = ivec2(uvec2(lessThan(t8.xyxy, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti1.xy = (-ti1.xy) + ti9.xy;
    t1.xy = vec2(ti1.xy);
    t9.xy = vec2(float(_MeshWrapSizeX) * float(0.5), float(_MeshWrapSizeZ) * float(0.5));
    tb8.xy = greaterThanEqual(abs(t8.xyxy), t9.xyxy).xy;
    t8.xy = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb8.xy));
    t8.xy = t1.xy * t8.xy;
    t0.xy = t8.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + t0.xy;
    t8.x = t0.x * t0.y;
    t12 = _Time.z * 0.5;
    t8.x = t8.x * 0.00749999983 + t12;
    t16_2 = in_COLOR0.x * 10.0 + t8.x;
    t16_2 = sin(t16_2);
    t8.xy = t0.xy + (-_Object2World[3].xz);
    t8.x = dot(t8.xy, t8.xy);
    t8.x = sqrt(t8.x);
    t16_6 = t8.x * _Falloff + _FadeDistance;
    t16_6 = clamp(t16_6, 0.0, 1.0);
    t16_2 = t16_6 * t16_2;
    t16_6 = t16_6 * _Amplitude + in_COLOR0.z;
    t1.y = _World2Object[1].z;
    t8.x = (-t1.y) + _OffsetY;
    t8.x = (-t16_6) + t8.x;
    t3.y = t16_2 * 0.5 + t8.x;
    t1.x = _World2Object[0].z;
    t1.z = _World2Object[2].z;
    t0.xy = t0.xy + (-t1.xz);
    t3.xz = t0.xy + in_TEXCOORD1.xy;
    t0.xyz = t1.xyz + t3.xyz;
    t1 = t0.yyyy * unity_MatrixVP[1];
    t1 = unity_MatrixVP[0] * t0.xxxx + t1;
    t0 = unity_MatrixVP[2] * t0.zzzz + t1;
    t0 = t0 + unity_MatrixVP[3];
    t1.x = unity_LightShadowBias.x / t0.w;
    t1.x = clamp(t1.x, 0.0, 1.0);
    t8.x = t0.z + t1.x;
    t1.x = max((-t0.w), t8.x);
    gl_Position.xyw = t0.xyw;
    t0.x = (-t8.x) + t1.x;
    gl_Position.z = unity_LightShadowBias.y * t0.x + t8.x;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
layout(location = 0) out highp vec4 SV_Target0;
void main()
{
    SV_Target0 = vec4(0.0, 0.0, 0.0, 0.0);
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_CUBE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec4 _LightPositionRange;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp mat4 unity_MatrixVP;
uniform mediump float _FadeDistance;
uniform mediump float _Falloff;
uniform mediump float _MeshWrapSizeX;
uniform mediump float _MeshWrapSizeZ;
uniform mediump float _Amplitude;
uniform mediump float _OffsetY;
varying highp vec3 xlv_TEXCOORD0;
void main ()
{
  mediump float mDeg_1;
  mediump float distance_2;
  highp vec4 wPos_3;
  highp vec2 meshWrapSize_4;
  highp vec2 tmpvar_5;
  tmpvar_5.x = _Object2World[3].x;
  tmpvar_5.y = _Object2World[3].z;
  mediump vec2 tmpvar_6;
  tmpvar_6.x = _MeshWrapSizeX;
  tmpvar_6.y = _MeshWrapSizeZ;
  meshWrapSize_4 = tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((vec2(
    ivec2(((tmpvar_5 / meshWrapSize_4) + (sign(tmpvar_5) * vec2(0.5, 0.5))))
  ) * meshWrapSize_4) + _glesVertex.xz);
  highp vec2 tmpvar_8;
  tmpvar_8 = (tmpvar_5 - tmpvar_7);
  wPos_3.xz = (tmpvar_7 + ((
    vec2(greaterThanEqual (abs(tmpvar_8), (meshWrapSize_4 * 0.5)))
   * 
    sign(tmpvar_8)
  ) * meshWrapSize_4));
  wPos_3.y = _OffsetY;
  wPos_3.w = 1.0;
  highp float tmpvar_9;
  highp vec2 x_10;
  x_10 = (wPos_3.xz - tmpvar_5);
  tmpvar_9 = sqrt(dot (x_10, x_10));
  distance_2 = tmpvar_9;
  mediump float tmpvar_11;
  tmpvar_11 = clamp ((_FadeDistance + (distance_2 * _Falloff)), 0.0, 1.0);
  highp float tmpvar_12;
  tmpvar_12 = ((wPos_3.z * wPos_3.x) * 0.0075);
  mDeg_1 = tmpvar_12;
  mDeg_1 = (mDeg_1 + (_Time.z * 0.5));
  highp vec4 v_13;
  v_13.x = _World2Object[0].z;
  v_13.y = _World2Object[1].z;
  v_13.z = _World2Object[2].z;
  v_13.w = _World2Object[3].z;
  wPos_3.xyz = (wPos_3.xyz - v_13.xyz);
  wPos_3.xz = (wPos_3.xz + _glesMultiTexCoord1.xy);
  wPos_3.y = (wPos_3.y - (_glesColor.z + (tmpvar_11 * _Amplitude)));
  mediump float tmpvar_14;
  tmpvar_14 = sin((mDeg_1 + (_glesColor.x * 10.0)));
  wPos_3.y = (wPos_3.y + ((tmpvar_14 * tmpvar_11) * 0.5));
  wPos_3.xyz = (wPos_3.xyz + v_13.xyz);
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = wPos_3.xyz;
  xlv_TEXCOORD0 = (wPos_3.xyz - _LightPositionRange.xyz);
  gl_Position = (unity_MatrixVP * tmpvar_15);
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _LightPositionRange;
varying highp vec3 xlv_TEXCOORD0;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = fract((vec4(1.0, 255.0, 65025.0, 1.658138e+07) * min (
    (sqrt(dot (xlv_TEXCOORD0, xlv_TEXCOORD0)) * _LightPositionRange.w)
  , 0.999)));
  highp vec4 tmpvar_2;
  tmpvar_2 = (tmpvar_1 - (tmpvar_1.yzww * 0.003921569));
  gl_FragData[0] = tmpvar_2;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_CUBE" }
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
uniform 	mediump float _FadeDistance;
uniform 	mediump float _Falloff;
uniform 	mediump float _MeshWrapSizeX;
uniform 	mediump float _MeshWrapSizeZ;
uniform 	mediump float _Amplitude;
uniform 	mediump float _OffsetY;
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in mediump vec4 in_TEXCOORD1;
out highp vec3 vs_TEXCOORD0;
highp vec4 t0;
highp ivec2 ti0;
highp vec4 t1;
highp ivec2 ti1;
mediump float t16_2;
highp vec3 t3;
mediump float t16_6;
highp vec2 t8;
highp ivec2 ti8;
bvec2 tb8;
highp vec2 t9;
highp ivec2 ti9;
highp float t12;
void main()
{
    ti0.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), _Object2World[3].xzxx).xy) * 0xFFFFFFFFu);
    ti8.xy = ivec2(uvec2(lessThan(_Object2World[3].xzxz, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti0.xy = (-ti0.xy) + ti8.xy;
    t0.xy = vec2(ti0.xy);
    t8.xy = vec2(_Object2World[3].x / float(_MeshWrapSizeX), _Object2World[3].z / float(_MeshWrapSizeZ));
    t0.xy = t0.xy * vec2(0.5, 0.5) + t8.xy;
    t0.xy = trunc(t0.xy);
    t0.xy = t0.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + in_POSITION0.xz;
    t8.xy = (-t0.xy) + _Object2World[3].xz;
    ti1.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), t8.xyxx).xy) * 0xFFFFFFFFu);
    ti9.xy = ivec2(uvec2(lessThan(t8.xyxy, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti1.xy = (-ti1.xy) + ti9.xy;
    t1.xy = vec2(ti1.xy);
    t9.xy = vec2(float(_MeshWrapSizeX) * float(0.5), float(_MeshWrapSizeZ) * float(0.5));
    tb8.xy = greaterThanEqual(abs(t8.xyxy), t9.xyxy).xy;
    t8.xy = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb8.xy));
    t8.xy = t1.xy * t8.xy;
    t0.xy = t8.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + t0.xy;
    t8.x = t0.x * t0.y;
    t12 = _Time.z * 0.5;
    t8.x = t8.x * 0.00749999983 + t12;
    t16_2 = in_COLOR0.x * 10.0 + t8.x;
    t16_2 = sin(t16_2);
    t8.xy = t0.xy + (-_Object2World[3].xz);
    t8.x = dot(t8.xy, t8.xy);
    t8.x = sqrt(t8.x);
    t16_6 = t8.x * _Falloff + _FadeDistance;
    t16_6 = clamp(t16_6, 0.0, 1.0);
    t16_2 = t16_6 * t16_2;
    t16_6 = t16_6 * _Amplitude + in_COLOR0.z;
    t1.y = _World2Object[1].z;
    t8.x = (-t1.y) + _OffsetY;
    t8.x = (-t16_6) + t8.x;
    t3.y = t16_2 * 0.5 + t8.x;
    t1.x = _World2Object[0].z;
    t1.z = _World2Object[2].z;
    t0.xy = t0.xy + (-t1.xz);
    t3.xz = t0.xy + in_TEXCOORD1.xy;
    t0.xyz = t1.xyz + t3.xyz;
    vs_TEXCOORD0.xyz = t0.xyz + (-_LightPositionRange.xyz);
    t1 = t0.yyyy * unity_MatrixVP[1];
    t1 = unity_MatrixVP[0] * t0.xxxx + t1;
    t0 = unity_MatrixVP[2] * t0.zzzz + t1;
    gl_Position = t0 + unity_MatrixVP[3];
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
uniform 	mediump float _FadeDistance;
uniform 	mediump float _Falloff;
uniform 	mediump float _MeshWrapSizeX;
uniform 	mediump float _MeshWrapSizeZ;
uniform 	mediump float _Amplitude;
uniform 	mediump float _OffsetY;
in highp vec3 vs_TEXCOORD0;
layout(location = 0) out highp vec4 SV_Target0;
highp vec4 t0;
void main()
{
    t0.x = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD0.xyz);
    t0.x = sqrt(t0.x);
    t0.x = t0.x * _LightPositionRange.w;
    t0.x = min(t0.x, 0.999000013);
    t0 = t0.xxxx * vec4(1.0, 255.0, 65025.0, 16581375.0);
    t0 = fract(t0);
    SV_Target0 = (-t0.yzww) * vec4(0.00392156886, 0.00392156886, 0.00392156886, 0.00392156886) + t0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_DEPTH" "SHADOWS_NATIVE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec4 unity_LightShadowBias;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp mat4 unity_MatrixVP;
uniform mediump float _FadeDistance;
uniform mediump float _Falloff;
uniform mediump float _MeshWrapSizeX;
uniform mediump float _MeshWrapSizeZ;
uniform mediump float _Amplitude;
uniform mediump float _OffsetY;
void main ()
{
  mediump float mDeg_1;
  mediump float distance_2;
  highp vec4 wPos_3;
  highp vec2 meshWrapSize_4;
  highp vec4 tmpvar_5;
  highp vec2 tmpvar_6;
  tmpvar_6.x = _Object2World[3].x;
  tmpvar_6.y = _Object2World[3].z;
  mediump vec2 tmpvar_7;
  tmpvar_7.x = _MeshWrapSizeX;
  tmpvar_7.y = _MeshWrapSizeZ;
  meshWrapSize_4 = tmpvar_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = ((vec2(
    ivec2(((tmpvar_6 / meshWrapSize_4) + (sign(tmpvar_6) * vec2(0.5, 0.5))))
  ) * meshWrapSize_4) + _glesVertex.xz);
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_6 - tmpvar_8);
  wPos_3.xz = (tmpvar_8 + ((
    vec2(greaterThanEqual (abs(tmpvar_9), (meshWrapSize_4 * 0.5)))
   * 
    sign(tmpvar_9)
  ) * meshWrapSize_4));
  wPos_3.y = _OffsetY;
  wPos_3.w = 1.0;
  highp float tmpvar_10;
  highp vec2 x_11;
  x_11 = (wPos_3.xz - tmpvar_6);
  tmpvar_10 = sqrt(dot (x_11, x_11));
  distance_2 = tmpvar_10;
  mediump float tmpvar_12;
  tmpvar_12 = clamp ((_FadeDistance + (distance_2 * _Falloff)), 0.0, 1.0);
  highp float tmpvar_13;
  tmpvar_13 = ((wPos_3.z * wPos_3.x) * 0.0075);
  mDeg_1 = tmpvar_13;
  mDeg_1 = (mDeg_1 + (_Time.z * 0.5));
  highp vec4 v_14;
  v_14.x = _World2Object[0].z;
  v_14.y = _World2Object[1].z;
  v_14.z = _World2Object[2].z;
  v_14.w = _World2Object[3].z;
  wPos_3.xyz = (wPos_3.xyz - v_14.xyz);
  wPos_3.xz = (wPos_3.xz + _glesMultiTexCoord1.xy);
  wPos_3.y = (wPos_3.y - (_glesColor.z + (tmpvar_12 * _Amplitude)));
  mediump float tmpvar_15;
  tmpvar_15 = sin((mDeg_1 + (_glesColor.x * 10.0)));
  wPos_3.y = (wPos_3.y + ((tmpvar_15 * tmpvar_12) * 0.5));
  wPos_3.xyz = (wPos_3.xyz + v_14.xyz);
  highp vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = wPos_3.xyz;
  tmpvar_5 = (unity_MatrixVP * tmpvar_16);
  highp vec4 clipPos_17;
  clipPos_17.xyw = tmpvar_5.xyw;
  clipPos_17.z = (tmpvar_5.z + clamp ((unity_LightShadowBias.x / tmpvar_5.w), 0.0, 1.0));
  clipPos_17.z = mix (clipPos_17.z, max (clipPos_17.z, -(tmpvar_5.w)), unity_LightShadowBias.y);
  tmpvar_5 = clipPos_17;
  gl_Position = clipPos_17;
}


#endif
#ifdef FRAGMENT
void main ()
{
  gl_FragData[0] = vec4(0.0, 0.0, 0.0, 0.0);
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_DEPTH" "SHADOWS_NATIVE" }
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
uniform 	mediump float _FadeDistance;
uniform 	mediump float _Falloff;
uniform 	mediump float _MeshWrapSizeX;
uniform 	mediump float _MeshWrapSizeZ;
uniform 	mediump float _Amplitude;
uniform 	mediump float _OffsetY;
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in mediump vec4 in_TEXCOORD1;
highp vec4 t0;
highp ivec2 ti0;
highp vec4 t1;
highp ivec2 ti1;
mediump float t16_2;
highp vec3 t3;
mediump float t16_6;
highp vec2 t8;
highp ivec2 ti8;
bvec2 tb8;
highp vec2 t9;
highp ivec2 ti9;
highp float t12;
void main()
{
    ti0.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), _Object2World[3].xzxx).xy) * 0xFFFFFFFFu);
    ti8.xy = ivec2(uvec2(lessThan(_Object2World[3].xzxz, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti0.xy = (-ti0.xy) + ti8.xy;
    t0.xy = vec2(ti0.xy);
    t8.xy = vec2(_Object2World[3].x / float(_MeshWrapSizeX), _Object2World[3].z / float(_MeshWrapSizeZ));
    t0.xy = t0.xy * vec2(0.5, 0.5) + t8.xy;
    t0.xy = trunc(t0.xy);
    t0.xy = t0.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + in_POSITION0.xz;
    t8.xy = (-t0.xy) + _Object2World[3].xz;
    ti1.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), t8.xyxx).xy) * 0xFFFFFFFFu);
    ti9.xy = ivec2(uvec2(lessThan(t8.xyxy, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti1.xy = (-ti1.xy) + ti9.xy;
    t1.xy = vec2(ti1.xy);
    t9.xy = vec2(float(_MeshWrapSizeX) * float(0.5), float(_MeshWrapSizeZ) * float(0.5));
    tb8.xy = greaterThanEqual(abs(t8.xyxy), t9.xyxy).xy;
    t8.xy = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb8.xy));
    t8.xy = t1.xy * t8.xy;
    t0.xy = t8.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + t0.xy;
    t8.x = t0.x * t0.y;
    t12 = _Time.z * 0.5;
    t8.x = t8.x * 0.00749999983 + t12;
    t16_2 = in_COLOR0.x * 10.0 + t8.x;
    t16_2 = sin(t16_2);
    t8.xy = t0.xy + (-_Object2World[3].xz);
    t8.x = dot(t8.xy, t8.xy);
    t8.x = sqrt(t8.x);
    t16_6 = t8.x * _Falloff + _FadeDistance;
    t16_6 = clamp(t16_6, 0.0, 1.0);
    t16_2 = t16_6 * t16_2;
    t16_6 = t16_6 * _Amplitude + in_COLOR0.z;
    t1.y = _World2Object[1].z;
    t8.x = (-t1.y) + _OffsetY;
    t8.x = (-t16_6) + t8.x;
    t3.y = t16_2 * 0.5 + t8.x;
    t1.x = _World2Object[0].z;
    t1.z = _World2Object[2].z;
    t0.xy = t0.xy + (-t1.xz);
    t3.xz = t0.xy + in_TEXCOORD1.xy;
    t0.xyz = t1.xyz + t3.xyz;
    t1 = t0.yyyy * unity_MatrixVP[1];
    t1 = unity_MatrixVP[0] * t0.xxxx + t1;
    t0 = unity_MatrixVP[2] * t0.zzzz + t1;
    t0 = t0 + unity_MatrixVP[3];
    t1.x = unity_LightShadowBias.x / t0.w;
    t1.x = clamp(t1.x, 0.0, 1.0);
    t8.x = t0.z + t1.x;
    t1.x = max((-t0.w), t8.x);
    gl_Position.xyw = t0.xyw;
    t0.x = (-t8.x) + t1.x;
    gl_Position.z = unity_LightShadowBias.y * t0.x + t8.x;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
layout(location = 0) out highp vec4 SV_Target0;
void main()
{
    SV_Target0 = vec4(0.0, 0.0, 0.0, 0.0);
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_CUBE" "SHADOWS_NATIVE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec4 _LightPositionRange;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp mat4 unity_MatrixVP;
uniform mediump float _FadeDistance;
uniform mediump float _Falloff;
uniform mediump float _MeshWrapSizeX;
uniform mediump float _MeshWrapSizeZ;
uniform mediump float _Amplitude;
uniform mediump float _OffsetY;
varying highp vec3 xlv_TEXCOORD0;
void main ()
{
  mediump float mDeg_1;
  mediump float distance_2;
  highp vec4 wPos_3;
  highp vec2 meshWrapSize_4;
  highp vec2 tmpvar_5;
  tmpvar_5.x = _Object2World[3].x;
  tmpvar_5.y = _Object2World[3].z;
  mediump vec2 tmpvar_6;
  tmpvar_6.x = _MeshWrapSizeX;
  tmpvar_6.y = _MeshWrapSizeZ;
  meshWrapSize_4 = tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((vec2(
    ivec2(((tmpvar_5 / meshWrapSize_4) + (sign(tmpvar_5) * vec2(0.5, 0.5))))
  ) * meshWrapSize_4) + _glesVertex.xz);
  highp vec2 tmpvar_8;
  tmpvar_8 = (tmpvar_5 - tmpvar_7);
  wPos_3.xz = (tmpvar_7 + ((
    vec2(greaterThanEqual (abs(tmpvar_8), (meshWrapSize_4 * 0.5)))
   * 
    sign(tmpvar_8)
  ) * meshWrapSize_4));
  wPos_3.y = _OffsetY;
  wPos_3.w = 1.0;
  highp float tmpvar_9;
  highp vec2 x_10;
  x_10 = (wPos_3.xz - tmpvar_5);
  tmpvar_9 = sqrt(dot (x_10, x_10));
  distance_2 = tmpvar_9;
  mediump float tmpvar_11;
  tmpvar_11 = clamp ((_FadeDistance + (distance_2 * _Falloff)), 0.0, 1.0);
  highp float tmpvar_12;
  tmpvar_12 = ((wPos_3.z * wPos_3.x) * 0.0075);
  mDeg_1 = tmpvar_12;
  mDeg_1 = (mDeg_1 + (_Time.z * 0.5));
  highp vec4 v_13;
  v_13.x = _World2Object[0].z;
  v_13.y = _World2Object[1].z;
  v_13.z = _World2Object[2].z;
  v_13.w = _World2Object[3].z;
  wPos_3.xyz = (wPos_3.xyz - v_13.xyz);
  wPos_3.xz = (wPos_3.xz + _glesMultiTexCoord1.xy);
  wPos_3.y = (wPos_3.y - (_glesColor.z + (tmpvar_11 * _Amplitude)));
  mediump float tmpvar_14;
  tmpvar_14 = sin((mDeg_1 + (_glesColor.x * 10.0)));
  wPos_3.y = (wPos_3.y + ((tmpvar_14 * tmpvar_11) * 0.5));
  wPos_3.xyz = (wPos_3.xyz + v_13.xyz);
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = wPos_3.xyz;
  xlv_TEXCOORD0 = (wPos_3.xyz - _LightPositionRange.xyz);
  gl_Position = (unity_MatrixVP * tmpvar_15);
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _LightPositionRange;
varying highp vec3 xlv_TEXCOORD0;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = fract((vec4(1.0, 255.0, 65025.0, 1.658138e+07) * min (
    (sqrt(dot (xlv_TEXCOORD0, xlv_TEXCOORD0)) * _LightPositionRange.w)
  , 0.999)));
  highp vec4 tmpvar_2;
  tmpvar_2 = (tmpvar_1 - (tmpvar_1.yzww * 0.003921569));
  gl_FragData[0] = tmpvar_2;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_CUBE" "SHADOWS_NATIVE" }
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
uniform 	mediump float _FadeDistance;
uniform 	mediump float _Falloff;
uniform 	mediump float _MeshWrapSizeX;
uniform 	mediump float _MeshWrapSizeZ;
uniform 	mediump float _Amplitude;
uniform 	mediump float _OffsetY;
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in mediump vec4 in_TEXCOORD1;
out highp vec3 vs_TEXCOORD0;
highp vec4 t0;
highp ivec2 ti0;
highp vec4 t1;
highp ivec2 ti1;
mediump float t16_2;
highp vec3 t3;
mediump float t16_6;
highp vec2 t8;
highp ivec2 ti8;
bvec2 tb8;
highp vec2 t9;
highp ivec2 ti9;
highp float t12;
void main()
{
    ti0.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), _Object2World[3].xzxx).xy) * 0xFFFFFFFFu);
    ti8.xy = ivec2(uvec2(lessThan(_Object2World[3].xzxz, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti0.xy = (-ti0.xy) + ti8.xy;
    t0.xy = vec2(ti0.xy);
    t8.xy = vec2(_Object2World[3].x / float(_MeshWrapSizeX), _Object2World[3].z / float(_MeshWrapSizeZ));
    t0.xy = t0.xy * vec2(0.5, 0.5) + t8.xy;
    t0.xy = trunc(t0.xy);
    t0.xy = t0.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + in_POSITION0.xz;
    t8.xy = (-t0.xy) + _Object2World[3].xz;
    ti1.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), t8.xyxx).xy) * 0xFFFFFFFFu);
    ti9.xy = ivec2(uvec2(lessThan(t8.xyxy, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti1.xy = (-ti1.xy) + ti9.xy;
    t1.xy = vec2(ti1.xy);
    t9.xy = vec2(float(_MeshWrapSizeX) * float(0.5), float(_MeshWrapSizeZ) * float(0.5));
    tb8.xy = greaterThanEqual(abs(t8.xyxy), t9.xyxy).xy;
    t8.xy = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb8.xy));
    t8.xy = t1.xy * t8.xy;
    t0.xy = t8.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + t0.xy;
    t8.x = t0.x * t0.y;
    t12 = _Time.z * 0.5;
    t8.x = t8.x * 0.00749999983 + t12;
    t16_2 = in_COLOR0.x * 10.0 + t8.x;
    t16_2 = sin(t16_2);
    t8.xy = t0.xy + (-_Object2World[3].xz);
    t8.x = dot(t8.xy, t8.xy);
    t8.x = sqrt(t8.x);
    t16_6 = t8.x * _Falloff + _FadeDistance;
    t16_6 = clamp(t16_6, 0.0, 1.0);
    t16_2 = t16_6 * t16_2;
    t16_6 = t16_6 * _Amplitude + in_COLOR0.z;
    t1.y = _World2Object[1].z;
    t8.x = (-t1.y) + _OffsetY;
    t8.x = (-t16_6) + t8.x;
    t3.y = t16_2 * 0.5 + t8.x;
    t1.x = _World2Object[0].z;
    t1.z = _World2Object[2].z;
    t0.xy = t0.xy + (-t1.xz);
    t3.xz = t0.xy + in_TEXCOORD1.xy;
    t0.xyz = t1.xyz + t3.xyz;
    vs_TEXCOORD0.xyz = t0.xyz + (-_LightPositionRange.xyz);
    t1 = t0.yyyy * unity_MatrixVP[1];
    t1 = unity_MatrixVP[0] * t0.xxxx + t1;
    t0 = unity_MatrixVP[2] * t0.zzzz + t1;
    gl_Position = t0 + unity_MatrixVP[3];
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
uniform 	mediump float _FadeDistance;
uniform 	mediump float _Falloff;
uniform 	mediump float _MeshWrapSizeX;
uniform 	mediump float _MeshWrapSizeZ;
uniform 	mediump float _Amplitude;
uniform 	mediump float _OffsetY;
in highp vec3 vs_TEXCOORD0;
layout(location = 0) out highp vec4 SV_Target0;
highp vec4 t0;
void main()
{
    t0.x = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD0.xyz);
    t0.x = sqrt(t0.x);
    t0.x = t0.x * _LightPositionRange.w;
    t0.x = min(t0.x, 0.999000013);
    t0 = t0.xxxx * vec4(1.0, 255.0, 65025.0, 16581375.0);
    t0 = fract(t0);
    SV_Target0 = (-t0.yzww) * vec4(0.00392156886, 0.00392156886, 0.00392156886, 0.00392156886) + t0;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_DEPTH" "SHADOWS_SCREEN" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec4 unity_LightShadowBias;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp mat4 unity_MatrixVP;
uniform mediump float _FadeDistance;
uniform mediump float _Falloff;
uniform mediump float _MeshWrapSizeX;
uniform mediump float _MeshWrapSizeZ;
uniform mediump float _Amplitude;
uniform mediump float _OffsetY;
void main ()
{
  mediump float mDeg_1;
  mediump float distance_2;
  highp vec4 wPos_3;
  highp vec2 meshWrapSize_4;
  highp vec4 tmpvar_5;
  highp vec2 tmpvar_6;
  tmpvar_6.x = _Object2World[3].x;
  tmpvar_6.y = _Object2World[3].z;
  mediump vec2 tmpvar_7;
  tmpvar_7.x = _MeshWrapSizeX;
  tmpvar_7.y = _MeshWrapSizeZ;
  meshWrapSize_4 = tmpvar_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = ((vec2(
    ivec2(((tmpvar_6 / meshWrapSize_4) + (sign(tmpvar_6) * vec2(0.5, 0.5))))
  ) * meshWrapSize_4) + _glesVertex.xz);
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_6 - tmpvar_8);
  wPos_3.xz = (tmpvar_8 + ((
    vec2(greaterThanEqual (abs(tmpvar_9), (meshWrapSize_4 * 0.5)))
   * 
    sign(tmpvar_9)
  ) * meshWrapSize_4));
  wPos_3.y = _OffsetY;
  wPos_3.w = 1.0;
  highp float tmpvar_10;
  highp vec2 x_11;
  x_11 = (wPos_3.xz - tmpvar_6);
  tmpvar_10 = sqrt(dot (x_11, x_11));
  distance_2 = tmpvar_10;
  mediump float tmpvar_12;
  tmpvar_12 = clamp ((_FadeDistance + (distance_2 * _Falloff)), 0.0, 1.0);
  highp float tmpvar_13;
  tmpvar_13 = ((wPos_3.z * wPos_3.x) * 0.0075);
  mDeg_1 = tmpvar_13;
  mDeg_1 = (mDeg_1 + (_Time.z * 0.5));
  highp vec4 v_14;
  v_14.x = _World2Object[0].z;
  v_14.y = _World2Object[1].z;
  v_14.z = _World2Object[2].z;
  v_14.w = _World2Object[3].z;
  wPos_3.xyz = (wPos_3.xyz - v_14.xyz);
  wPos_3.xz = (wPos_3.xz + _glesMultiTexCoord1.xy);
  wPos_3.y = (wPos_3.y - (_glesColor.z + (tmpvar_12 * _Amplitude)));
  mediump float tmpvar_15;
  tmpvar_15 = sin((mDeg_1 + (_glesColor.x * 10.0)));
  wPos_3.y = (wPos_3.y + ((tmpvar_15 * tmpvar_12) * 0.5));
  wPos_3.xyz = (wPos_3.xyz + v_14.xyz);
  highp vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = wPos_3.xyz;
  tmpvar_5 = (unity_MatrixVP * tmpvar_16);
  highp vec4 clipPos_17;
  clipPos_17.xyw = tmpvar_5.xyw;
  clipPos_17.z = (tmpvar_5.z + clamp ((unity_LightShadowBias.x / tmpvar_5.w), 0.0, 1.0));
  clipPos_17.z = mix (clipPos_17.z, max (clipPos_17.z, -(tmpvar_5.w)), unity_LightShadowBias.y);
  tmpvar_5 = clipPos_17;
  gl_Position = clipPos_17;
}


#endif
#ifdef FRAGMENT
void main ()
{
  gl_FragData[0] = vec4(0.0, 0.0, 0.0, 0.0);
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "SHADOWS_CUBE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec4 _LightPositionRange;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp mat4 unity_MatrixVP;
uniform mediump float _FadeDistance;
uniform mediump float _Falloff;
uniform mediump float _MeshWrapSizeX;
uniform mediump float _MeshWrapSizeZ;
uniform mediump float _Amplitude;
uniform mediump float _OffsetY;
varying highp vec3 xlv_TEXCOORD0;
void main ()
{
  mediump float mDeg_1;
  mediump float distance_2;
  highp vec4 wPos_3;
  highp vec2 meshWrapSize_4;
  highp vec2 tmpvar_5;
  tmpvar_5.x = _Object2World[3].x;
  tmpvar_5.y = _Object2World[3].z;
  mediump vec2 tmpvar_6;
  tmpvar_6.x = _MeshWrapSizeX;
  tmpvar_6.y = _MeshWrapSizeZ;
  meshWrapSize_4 = tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((vec2(
    ivec2(((tmpvar_5 / meshWrapSize_4) + (sign(tmpvar_5) * vec2(0.5, 0.5))))
  ) * meshWrapSize_4) + _glesVertex.xz);
  highp vec2 tmpvar_8;
  tmpvar_8 = (tmpvar_5 - tmpvar_7);
  wPos_3.xz = (tmpvar_7 + ((
    vec2(greaterThanEqual (abs(tmpvar_8), (meshWrapSize_4 * 0.5)))
   * 
    sign(tmpvar_8)
  ) * meshWrapSize_4));
  wPos_3.y = _OffsetY;
  wPos_3.w = 1.0;
  highp float tmpvar_9;
  highp vec2 x_10;
  x_10 = (wPos_3.xz - tmpvar_5);
  tmpvar_9 = sqrt(dot (x_10, x_10));
  distance_2 = tmpvar_9;
  mediump float tmpvar_11;
  tmpvar_11 = clamp ((_FadeDistance + (distance_2 * _Falloff)), 0.0, 1.0);
  highp float tmpvar_12;
  tmpvar_12 = ((wPos_3.z * wPos_3.x) * 0.0075);
  mDeg_1 = tmpvar_12;
  mDeg_1 = (mDeg_1 + (_Time.z * 0.5));
  highp vec4 v_13;
  v_13.x = _World2Object[0].z;
  v_13.y = _World2Object[1].z;
  v_13.z = _World2Object[2].z;
  v_13.w = _World2Object[3].z;
  wPos_3.xyz = (wPos_3.xyz - v_13.xyz);
  wPos_3.xz = (wPos_3.xz + _glesMultiTexCoord1.xy);
  wPos_3.y = (wPos_3.y - (_glesColor.z + (tmpvar_11 * _Amplitude)));
  mediump float tmpvar_14;
  tmpvar_14 = sin((mDeg_1 + (_glesColor.x * 10.0)));
  wPos_3.y = (wPos_3.y + ((tmpvar_14 * tmpvar_11) * 0.5));
  wPos_3.xyz = (wPos_3.xyz + v_13.xyz);
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = wPos_3.xyz;
  xlv_TEXCOORD0 = (wPos_3.xyz - _LightPositionRange.xyz);
  gl_Position = (unity_MatrixVP * tmpvar_15);
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _LightPositionRange;
varying highp vec3 xlv_TEXCOORD0;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = fract((vec4(1.0, 255.0, 65025.0, 1.658138e+07) * min (
    (sqrt(dot (xlv_TEXCOORD0, xlv_TEXCOORD0)) * _LightPositionRange.w)
  , 0.999)));
  highp vec4 tmpvar_2;
  tmpvar_2 = (tmpvar_1 - (tmpvar_1.yzww * 0.003921569));
  gl_FragData[0] = tmpvar_2;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_DEPTH" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec4 unity_LightShadowBias;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp mat4 unity_MatrixVP;
uniform mediump float _FadeDistance;
uniform mediump float _Falloff;
uniform mediump float _MeshWrapSizeX;
uniform mediump float _MeshWrapSizeZ;
uniform mediump float _Amplitude;
uniform mediump float _OffsetY;
void main ()
{
  mediump float mDeg_1;
  mediump float distance_2;
  highp vec4 wPos_3;
  highp vec2 meshWrapSize_4;
  highp vec4 tmpvar_5;
  highp vec2 tmpvar_6;
  tmpvar_6.x = _Object2World[3].x;
  tmpvar_6.y = _Object2World[3].z;
  mediump vec2 tmpvar_7;
  tmpvar_7.x = _MeshWrapSizeX;
  tmpvar_7.y = _MeshWrapSizeZ;
  meshWrapSize_4 = tmpvar_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = ((vec2(
    ivec2(((tmpvar_6 / meshWrapSize_4) + (sign(tmpvar_6) * vec2(0.5, 0.5))))
  ) * meshWrapSize_4) + _glesVertex.xz);
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_6 - tmpvar_8);
  wPos_3.xz = (tmpvar_8 + ((
    vec2(greaterThanEqual (abs(tmpvar_9), (meshWrapSize_4 * 0.5)))
   * 
    sign(tmpvar_9)
  ) * meshWrapSize_4));
  wPos_3.y = _OffsetY;
  wPos_3.w = 1.0;
  highp float tmpvar_10;
  highp vec2 x_11;
  x_11 = (wPos_3.xz - tmpvar_6);
  tmpvar_10 = sqrt(dot (x_11, x_11));
  distance_2 = tmpvar_10;
  mediump float tmpvar_12;
  tmpvar_12 = clamp ((_FadeDistance + (distance_2 * _Falloff)), 0.0, 1.0);
  highp float tmpvar_13;
  tmpvar_13 = ((wPos_3.z * wPos_3.x) * 0.0075);
  mDeg_1 = tmpvar_13;
  mDeg_1 = (mDeg_1 + (_Time.z * 0.5));
  highp vec4 v_14;
  v_14.x = _World2Object[0].z;
  v_14.y = _World2Object[1].z;
  v_14.z = _World2Object[2].z;
  v_14.w = _World2Object[3].z;
  wPos_3.xyz = (wPos_3.xyz - v_14.xyz);
  wPos_3.xz = (wPos_3.xz + _glesMultiTexCoord1.xy);
  wPos_3.y = (wPos_3.y - (_glesColor.z + (tmpvar_12 * _Amplitude)));
  mediump float tmpvar_15;
  tmpvar_15 = sin((mDeg_1 + (_glesColor.x * 10.0)));
  wPos_3.y = (wPos_3.y + ((tmpvar_15 * tmpvar_12) * 0.5));
  wPos_3.xyz = (wPos_3.xyz + v_14.xyz);
  highp vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = wPos_3.xyz;
  tmpvar_5 = (unity_MatrixVP * tmpvar_16);
  highp vec4 clipPos_17;
  clipPos_17.xyw = tmpvar_5.xyw;
  clipPos_17.z = (tmpvar_5.z + clamp ((unity_LightShadowBias.x / tmpvar_5.w), 0.0, 1.0));
  clipPos_17.z = mix (clipPos_17.z, max (clipPos_17.z, -(tmpvar_5.w)), unity_LightShadowBias.y);
  tmpvar_5 = clipPos_17;
  gl_Position = clipPos_17;
}


#endif
#ifdef FRAGMENT
void main ()
{
  gl_FragData[0] = vec4(0.0, 0.0, 0.0, 0.0);
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_DEPTH" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
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
uniform 	mediump float _FadeDistance;
uniform 	mediump float _Falloff;
uniform 	mediump float _MeshWrapSizeX;
uniform 	mediump float _MeshWrapSizeZ;
uniform 	mediump float _Amplitude;
uniform 	mediump float _OffsetY;
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in mediump vec4 in_TEXCOORD1;
highp vec4 t0;
highp ivec2 ti0;
highp vec4 t1;
highp ivec2 ti1;
mediump float t16_2;
highp vec3 t3;
mediump float t16_6;
highp vec2 t8;
highp ivec2 ti8;
bvec2 tb8;
highp vec2 t9;
highp ivec2 ti9;
highp float t12;
void main()
{
    ti0.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), _Object2World[3].xzxx).xy) * 0xFFFFFFFFu);
    ti8.xy = ivec2(uvec2(lessThan(_Object2World[3].xzxz, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti0.xy = (-ti0.xy) + ti8.xy;
    t0.xy = vec2(ti0.xy);
    t8.xy = vec2(_Object2World[3].x / float(_MeshWrapSizeX), _Object2World[3].z / float(_MeshWrapSizeZ));
    t0.xy = t0.xy * vec2(0.5, 0.5) + t8.xy;
    t0.xy = trunc(t0.xy);
    t0.xy = t0.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + in_POSITION0.xz;
    t8.xy = (-t0.xy) + _Object2World[3].xz;
    ti1.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), t8.xyxx).xy) * 0xFFFFFFFFu);
    ti9.xy = ivec2(uvec2(lessThan(t8.xyxy, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti1.xy = (-ti1.xy) + ti9.xy;
    t1.xy = vec2(ti1.xy);
    t9.xy = vec2(float(_MeshWrapSizeX) * float(0.5), float(_MeshWrapSizeZ) * float(0.5));
    tb8.xy = greaterThanEqual(abs(t8.xyxy), t9.xyxy).xy;
    t8.xy = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb8.xy));
    t8.xy = t1.xy * t8.xy;
    t0.xy = t8.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + t0.xy;
    t8.x = t0.x * t0.y;
    t12 = _Time.z * 0.5;
    t8.x = t8.x * 0.00749999983 + t12;
    t16_2 = in_COLOR0.x * 10.0 + t8.x;
    t16_2 = sin(t16_2);
    t8.xy = t0.xy + (-_Object2World[3].xz);
    t8.x = dot(t8.xy, t8.xy);
    t8.x = sqrt(t8.x);
    t16_6 = t8.x * _Falloff + _FadeDistance;
    t16_6 = clamp(t16_6, 0.0, 1.0);
    t16_2 = t16_6 * t16_2;
    t16_6 = t16_6 * _Amplitude + in_COLOR0.z;
    t1.y = _World2Object[1].z;
    t8.x = (-t1.y) + _OffsetY;
    t8.x = (-t16_6) + t8.x;
    t3.y = t16_2 * 0.5 + t8.x;
    t1.x = _World2Object[0].z;
    t1.z = _World2Object[2].z;
    t0.xy = t0.xy + (-t1.xz);
    t3.xz = t0.xy + in_TEXCOORD1.xy;
    t0.xyz = t1.xyz + t3.xyz;
    t1 = t0.yyyy * unity_MatrixVP[1];
    t1 = unity_MatrixVP[0] * t0.xxxx + t1;
    t0 = unity_MatrixVP[2] * t0.zzzz + t1;
    t0 = t0 + unity_MatrixVP[3];
    t1.x = unity_LightShadowBias.x / t0.w;
    t1.x = clamp(t1.x, 0.0, 1.0);
    t8.x = t0.z + t1.x;
    t1.x = max((-t0.w), t8.x);
    gl_Position.xyw = t0.xyw;
    t0.x = (-t8.x) + t1.x;
    gl_Position.z = unity_LightShadowBias.y * t0.x + t8.x;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
layout(location = 0) out highp vec4 SV_Target0;
void main()
{
    SV_Target0 = vec4(0.0, 0.0, 0.0, 0.0);
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_CUBE" "SHADOWS_NATIVE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec4 _LightPositionRange;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp mat4 unity_MatrixVP;
uniform mediump float _FadeDistance;
uniform mediump float _Falloff;
uniform mediump float _MeshWrapSizeX;
uniform mediump float _MeshWrapSizeZ;
uniform mediump float _Amplitude;
uniform mediump float _OffsetY;
varying highp vec3 xlv_TEXCOORD0;
void main ()
{
  mediump float mDeg_1;
  mediump float distance_2;
  highp vec4 wPos_3;
  highp vec2 meshWrapSize_4;
  highp vec2 tmpvar_5;
  tmpvar_5.x = _Object2World[3].x;
  tmpvar_5.y = _Object2World[3].z;
  mediump vec2 tmpvar_6;
  tmpvar_6.x = _MeshWrapSizeX;
  tmpvar_6.y = _MeshWrapSizeZ;
  meshWrapSize_4 = tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((vec2(
    ivec2(((tmpvar_5 / meshWrapSize_4) + (sign(tmpvar_5) * vec2(0.5, 0.5))))
  ) * meshWrapSize_4) + _glesVertex.xz);
  highp vec2 tmpvar_8;
  tmpvar_8 = (tmpvar_5 - tmpvar_7);
  wPos_3.xz = (tmpvar_7 + ((
    vec2(greaterThanEqual (abs(tmpvar_8), (meshWrapSize_4 * 0.5)))
   * 
    sign(tmpvar_8)
  ) * meshWrapSize_4));
  wPos_3.y = _OffsetY;
  wPos_3.w = 1.0;
  highp float tmpvar_9;
  highp vec2 x_10;
  x_10 = (wPos_3.xz - tmpvar_5);
  tmpvar_9 = sqrt(dot (x_10, x_10));
  distance_2 = tmpvar_9;
  mediump float tmpvar_11;
  tmpvar_11 = clamp ((_FadeDistance + (distance_2 * _Falloff)), 0.0, 1.0);
  highp float tmpvar_12;
  tmpvar_12 = ((wPos_3.z * wPos_3.x) * 0.0075);
  mDeg_1 = tmpvar_12;
  mDeg_1 = (mDeg_1 + (_Time.z * 0.5));
  highp vec4 v_13;
  v_13.x = _World2Object[0].z;
  v_13.y = _World2Object[1].z;
  v_13.z = _World2Object[2].z;
  v_13.w = _World2Object[3].z;
  wPos_3.xyz = (wPos_3.xyz - v_13.xyz);
  wPos_3.xz = (wPos_3.xz + _glesMultiTexCoord1.xy);
  wPos_3.y = (wPos_3.y - (_glesColor.z + (tmpvar_11 * _Amplitude)));
  mediump float tmpvar_14;
  tmpvar_14 = sin((mDeg_1 + (_glesColor.x * 10.0)));
  wPos_3.y = (wPos_3.y + ((tmpvar_14 * tmpvar_11) * 0.5));
  wPos_3.xyz = (wPos_3.xyz + v_13.xyz);
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = wPos_3.xyz;
  xlv_TEXCOORD0 = (wPos_3.xyz - _LightPositionRange.xyz);
  gl_Position = (unity_MatrixVP * tmpvar_15);
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _LightPositionRange;
varying highp vec3 xlv_TEXCOORD0;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = fract((vec4(1.0, 255.0, 65025.0, 1.658138e+07) * min (
    (sqrt(dot (xlv_TEXCOORD0, xlv_TEXCOORD0)) * _LightPositionRange.w)
  , 0.999)));
  highp vec4 tmpvar_2;
  tmpvar_2 = (tmpvar_1 - (tmpvar_1.yzww * 0.003921569));
  gl_FragData[0] = tmpvar_2;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_CUBE" "SHADOWS_NATIVE" }
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
uniform 	mediump float _FadeDistance;
uniform 	mediump float _Falloff;
uniform 	mediump float _MeshWrapSizeX;
uniform 	mediump float _MeshWrapSizeZ;
uniform 	mediump float _Amplitude;
uniform 	mediump float _OffsetY;
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in mediump vec4 in_TEXCOORD1;
out highp vec3 vs_TEXCOORD0;
highp vec4 t0;
highp ivec2 ti0;
highp vec4 t1;
highp ivec2 ti1;
mediump float t16_2;
highp vec3 t3;
mediump float t16_6;
highp vec2 t8;
highp ivec2 ti8;
bvec2 tb8;
highp vec2 t9;
highp ivec2 ti9;
highp float t12;
void main()
{
    ti0.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), _Object2World[3].xzxx).xy) * 0xFFFFFFFFu);
    ti8.xy = ivec2(uvec2(lessThan(_Object2World[3].xzxz, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti0.xy = (-ti0.xy) + ti8.xy;
    t0.xy = vec2(ti0.xy);
    t8.xy = vec2(_Object2World[3].x / float(_MeshWrapSizeX), _Object2World[3].z / float(_MeshWrapSizeZ));
    t0.xy = t0.xy * vec2(0.5, 0.5) + t8.xy;
    t0.xy = trunc(t0.xy);
    t0.xy = t0.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + in_POSITION0.xz;
    t8.xy = (-t0.xy) + _Object2World[3].xz;
    ti1.xy = ivec2(uvec2(lessThan(vec4(0.0, 0.0, 0.0, 0.0), t8.xyxx).xy) * 0xFFFFFFFFu);
    ti9.xy = ivec2(uvec2(lessThan(t8.xyxy, vec4(0.0, 0.0, 0.0, 0.0)).xy) * 0xFFFFFFFFu);
    ti1.xy = (-ti1.xy) + ti9.xy;
    t1.xy = vec2(ti1.xy);
    t9.xy = vec2(float(_MeshWrapSizeX) * float(0.5), float(_MeshWrapSizeZ) * float(0.5));
    tb8.xy = greaterThanEqual(abs(t8.xyxy), t9.xyxy).xy;
    t8.xy = mix(vec2(0.0, 0.0), vec2(1.0, 1.0), vec2(tb8.xy));
    t8.xy = t1.xy * t8.xy;
    t0.xy = t8.xy * vec2(_MeshWrapSizeX, _MeshWrapSizeZ) + t0.xy;
    t8.x = t0.x * t0.y;
    t12 = _Time.z * 0.5;
    t8.x = t8.x * 0.00749999983 + t12;
    t16_2 = in_COLOR0.x * 10.0 + t8.x;
    t16_2 = sin(t16_2);
    t8.xy = t0.xy + (-_Object2World[3].xz);
    t8.x = dot(t8.xy, t8.xy);
    t8.x = sqrt(t8.x);
    t16_6 = t8.x * _Falloff + _FadeDistance;
    t16_6 = clamp(t16_6, 0.0, 1.0);
    t16_2 = t16_6 * t16_2;
    t16_6 = t16_6 * _Amplitude + in_COLOR0.z;
    t1.y = _World2Object[1].z;
    t8.x = (-t1.y) + _OffsetY;
    t8.x = (-t16_6) + t8.x;
    t3.y = t16_2 * 0.5 + t8.x;
    t1.x = _World2Object[0].z;
    t1.z = _World2Object[2].z;
    t0.xy = t0.xy + (-t1.xz);
    t3.xz = t0.xy + in_TEXCOORD1.xy;
    t0.xyz = t1.xyz + t3.xyz;
    vs_TEXCOORD0.xyz = t0.xyz + (-_LightPositionRange.xyz);
    t1 = t0.yyyy * unity_MatrixVP[1];
    t1 = unity_MatrixVP[0] * t0.xxxx + t1;
    t0 = unity_MatrixVP[2] * t0.zzzz + t1;
    gl_Position = t0 + unity_MatrixVP[3];
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
uniform 	mediump float _FadeDistance;
uniform 	mediump float _Falloff;
uniform 	mediump float _MeshWrapSizeX;
uniform 	mediump float _MeshWrapSizeZ;
uniform 	mediump float _Amplitude;
uniform 	mediump float _OffsetY;
in highp vec3 vs_TEXCOORD0;
layout(location = 0) out highp vec4 SV_Target0;
highp vec4 t0;
void main()
{
    t0.x = dot(vs_TEXCOORD0.xyz, vs_TEXCOORD0.xyz);
    t0.x = sqrt(t0.x);
    t0.x = t0.x * _LightPositionRange.w;
    t0.x = min(t0.x, 0.999000013);
    t0 = t0.xxxx * vec4(1.0, 255.0, 65025.0, 16581375.0);
    t0 = fract(t0);
    SV_Target0 = (-t0.yzww) * vec4(0.00392156886, 0.00392156886, 0.00392156886, 0.00392156886) + t0;
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_DEPTH" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_DEPTH" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_CUBE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_CUBE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_DEPTH" "SHADOWS_NATIVE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_DEPTH" "SHADOWS_NATIVE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_CUBE" "SHADOWS_NATIVE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_CUBE" "SHADOWS_NATIVE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_DEPTH" "SHADOWS_SCREEN" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "SHADOWS_CUBE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_DEPTH" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_DEPTH" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_CUBE" "SHADOWS_NATIVE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_CUBE" "SHADOWS_NATIVE" }
"!!GLES3"
}
}
 }
}
}
®€Shader "Barrier/IntroBeam" {
Properties {
 _MainTex ("Billboard Texture", 2D) = "white" { }
 _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,1)
 _Thickness ("Thickness", Float) = 1
 _Frequency ("Frequency", Vector) = (0.3,0.35,0.25,0.25)
 _Amplitude ("Amplitude", Vector) = (1,1,1,1)
 _FadeDistance ("Fade Distance", Float) = 25
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Blend One One
  ColorMask RGB
  GpuProgramID 58757
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
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_projection;
uniform highp mat4 unity_MatrixV;
uniform highp vec4 _TintColor;
uniform highp vec4 _Frequency;
uniform highp vec4 _Amplitude;
uniform highp float _Thickness;
uniform highp float _FadeDistance;
uniform highp vec4 _CustomTime;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesTANGENT;
  highp float va_center_2;
  highp vec2 size_3;
  highp float t_4;
  highp vec3 modelRight_5;
  highp float hv_6;
  highp vec4 pos_7;
  mediump vec2 tmpvar_8;
  mediump vec3 tmpvar_9;
  mediump vec4 tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Object2World * _glesVertex);
  pos_7.w = tmpvar_11.w;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 0.0;
  tmpvar_12.xyz = _glesNormal;
  highp vec3 tmpvar_13;
  tmpvar_13 = (_Object2World * tmpvar_12).xyz;
  lowp float tmpvar_14;
  tmpvar_14 = _glesColor.y;
  hv_6 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15 = (sin((
    (tmpvar_13.y * 6.283185)
   + 
    ((_Frequency * _CustomTime.zzzz) * (0.5 + (tmpvar_13.x * 0.5)))
  )) * _Amplitude);
  highp vec3 tmpvar_16;
  tmpvar_16.z = 0.0;
  tmpvar_16.xy = tmpvar_15.xy;
  highp vec3 tmpvar_17;
  tmpvar_17.z = 0.0;
  tmpvar_17.xy = tmpvar_15.zw;
  highp vec3 tmpvar_18;
  tmpvar_18.y = 0.0;
  tmpvar_18.x = tmpvar_15.x;
  tmpvar_18.z = tmpvar_15.y;
  highp vec3 tmpvar_19;
  tmpvar_19.y = 0.0;
  tmpvar_19.x = tmpvar_15.z;
  tmpvar_19.z = tmpvar_15.w;
  highp vec3 tmpvar_20;
  tmpvar_20 = mix ((tmpvar_16 + tmpvar_17), (tmpvar_18 + tmpvar_19), vec3(hv_6));
  highp vec4 tmpvar_21;
  tmpvar_21.w = 0.0;
  tmpvar_21.xyz = tmpvar_1.xyz;
  modelRight_5.xz = (_Object2World * tmpvar_21).xz;
  modelRight_5.y = 0.0;
  highp vec3 tmpvar_22;
  tmpvar_22 = normalize(modelRight_5);
  modelRight_5 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = ((tmpvar_22.yzx * vec3(0.0, 0.0, 1.0)) - (tmpvar_22.zxy * vec3(1.0, 0.0, 0.0)));
  pos_7.xyz = (tmpvar_11.xyz + ((tmpvar_20.x * tmpvar_23) + (tmpvar_20.y * vec3(0.0, 1.0, 0.0))));
  highp vec3 tmpvar_24;
  tmpvar_24 = (_WorldSpaceCameraPos - pos_7.xyz);
  highp vec3 tmpvar_25;
  tmpvar_25 = (((tmpvar_22 * tmpvar_24.z) + (vec3(0.0, 1.0, 0.0) * tmpvar_24.y)) + (tmpvar_23 * tmpvar_24.x));
  highp vec2 tmpvar_26;
  tmpvar_26 = mix (tmpvar_25.xy, tmpvar_25.xz, _glesColor.yy);
  t_4 = (_Thickness * min (1.0, (
    sqrt(dot (tmpvar_26, tmpvar_26))
   * 0.1)));
  highp vec2 tmpvar_27;
  tmpvar_27.x = tmpvar_1.w;
  tmpvar_27.y = t_4;
  highp vec2 tmpvar_28;
  tmpvar_28.x = t_4;
  tmpvar_28.y = tmpvar_1.w;
  size_3 = (mix (tmpvar_27, tmpvar_28, vec2(hv_6)) * ((_glesMultiTexCoord0.xy * 2.0) - 1.0));
  highp vec3 tmpvar_29;
  tmpvar_29 = normalize(tmpvar_24);
  lowp float tmpvar_30;
  tmpvar_30 = (_glesColor.w - 0.4980392);
  va_center_2 = tmpvar_30;
  highp float tmpvar_31;
  tmpvar_31 = (1.0 - (abs(va_center_2) * 2.0));
  pos_7.xyz = (pos_7.xyz + mix ((
    ((tmpvar_22 * size_3.x) + (normalize((
      (tmpvar_22.yzx * tmpvar_29.zxy)
     - 
      (tmpvar_22.zxy * tmpvar_29.yzx)
    )) * size_3.y))
   + 
    ((tmpvar_22 * va_center_2) * _FadeDistance)
  ), (
    ((normalize((
      (tmpvar_29.yzx * vec3(0.0, 0.0, 1.0))
     - 
      (tmpvar_29.zxy * vec3(1.0, 0.0, 0.0))
    )) * size_3.x) + (vec3(0.0, 1.0, 0.0) * size_3.y))
   + 
    ((vec3(0.0, 1.0, 0.0) * va_center_2) * _FadeDistance)
  ), vec3(hv_6)));
  highp vec3 tmpvar_32;
  tmpvar_32 = pos_7.xyz;
  tmpvar_9 = tmpvar_32;
  pos_7 = (unity_MatrixV * pos_7);
  pos_7 = (glstate_matrix_projection * pos_7);
  highp vec2 tmpvar_33;
  tmpvar_33 = mix (_glesMultiTexCoord1, _glesMultiTexCoord0, vec4(hv_6)).xy;
  tmpvar_8 = tmpvar_33;
  highp float tmpvar_34;
  tmpvar_34 = sin(((tmpvar_13.x * 6.283185) + (_Time.z * 
    (0.5 + (tmpvar_13.y * 0.5))
  )));
  tmpvar_10 = (((2.0 * _TintColor) * (1.0 + tmpvar_34)) * tmpvar_31);
  tmpvar_10 = (tmpvar_10 * 2.0);
  gl_Position = pos_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
uniform highp vec3 _WorldSpaceCameraPos;
uniform sampler2D _MainTex;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
void main ()
{
  mediump vec4 tmpvar_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  highp float tmpvar_3;
  highp vec3 tmpvar_4;
  tmpvar_4 = (xlv_TEXCOORD1 - _WorldSpaceCameraPos);
  tmpvar_3 = sqrt(dot (tmpvar_4, tmpvar_4));
  tmpvar_1 = ((tmpvar_2 * xlv_TEXCOORD2) * (1.0/(tmpvar_3)));
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
uniform 	vec4 _Frequency;
uniform 	vec4 _Amplitude;
uniform 	float _Thickness;
uniform 	float _FadeDistance;
uniform 	vec4 _CustomTime;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD1;
in lowp vec4 in_COLOR0;
out vec4 undefined;
out mediump vec2 vs_TEXCOORD0;
out mediump vec3 vs_TEXCOORD1;
out mediump vec4 vs_TEXCOORD2;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
highp vec4 t3;
highp vec3 t4;
lowp float t10_5;
highp vec3 t6;
highp vec3 t7;
highp vec2 t12;
highp float t18;
highp float t19;
void main()
{
    t0 = _Frequency * _CustomTime.zzzz;
    t1 = in_NORMAL0.yyyy * _Object2World[1].xyyx;
    t1 = _Object2World[0].xyyx * in_NORMAL0.xxxx + t1;
    t1 = _Object2World[2].xyyx * in_NORMAL0.zzzz + t1;
    t7.xz = vec2(t1.y * float(6.28318548), t1.w * float(6.28318548));
    t1.xz = t1.xz * vec2(0.5, 0.5) + vec2(0.5, 0.5);
    t0 = t0 * t1.xxxx + t7.xxxx;
    t1.x = _Time.z * t1.z + t7.z;
    t1.x = sin(t1.x);
    t1.x = t1.x + 1.0;
    t0 = sin(t0);
    t0 = t0 * _Amplitude;
    t0.xyz = vec3(t0.z + t0.x, t0.w + t0.y, t0.z + t0.x);
    t0.w = 0.0;
    t12.xy = vec2((-t0.x) + t0.z, (-t0.y) + t0.w);
    t0.xy = in_COLOR0.yy * t12.xy + t0.xy;
    t6.xyz = t0.yyy * vec3(0.0, 1.0, 0.0);
    t7.xy = in_TANGENT0.yy * _Object2World[1].xz;
    t7.xy = _Object2World[0].xz * in_TANGENT0.xx + t7.xy;
    t7.xy = _Object2World[2].xz * in_TANGENT0.zz + t7.xy;
    t19 = dot(t7.xy, t7.xy);
    t2.x = inversesqrt(t19);
    t2.yw = t7.xy * t2.xx;
    t7.xyz = t2.wyx * vec3(1.0, 0.0, 0.0);
    t7.xyz = t2.xwy * vec3(0.0, 0.0, 1.0) + (-t7.xyz);
    t0.xyz = t0.xxx * t7.xyz + t6.xyz;
    t3 = in_POSITION0.yyyy * _Object2World[1];
    t3 = _Object2World[0] * in_POSITION0.xxxx + t3;
    t3 = _Object2World[2] * in_POSITION0.zzzz + t3;
    t3 = _Object2World[3] * in_POSITION0.wwww + t3;
    t0.xyz = t0.xyz + t3.xyz;
    t3.xyz = (-t0.yzx) + _WorldSpaceCameraPos.xyzx.yzx;
    t4.xyz = t3.xxx * vec3(0.0, 1.0, 0.0);
    t2.z = 0.0;
    t4.xyz = t2.yzw * t3.yyy + t4.xyz;
    t7.xyz = t7.xyz * t3.zzz + t4.xyz;
    t4.xy = (-t7.xy) + t7.xz;
    t7.xy = in_COLOR0.yy * t4.xy + t7.xy;
    t18 = dot(t7.xy, t7.xy);
    t18 = sqrt(t18);
    t18 = t18 * 0.100000001;
    t18 = min(t18, 1.0);
    t4.y = t18 * _Thickness;
    t4.x = in_TANGENT0.w;
    t7.xy = (-t4.xy) + t4.yx;
    t7.xy = in_COLOR0.yy * t7.xy + t4.xy;
    t4.xy = in_TEXCOORD0.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
    t7.xy = t7.xy * t4.xy;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t3.xyz = vec3(t18) * t3.xyz;
    t4.xyz = vec3(t2.w * t3.x, t2.y * t3.y, t2.z * t3.z);
    t4.xyz = t2.zwy * t3.yzx + (-t4.xyz);
    t18 = dot(t4.xyz, t4.xyz);
    t18 = inversesqrt(t18);
    t4.xyz = vec3(t18) * t4.xyz;
    t4.xyz = t7.yyy * t4.xyz;
    t4.xyz = t2.yzw * t7.xxx + t4.xyz;
    t10_5 = in_COLOR0.w + -0.498039216;
    t2.xyz = vec3(t2.y * float(t10_5), t2.z * float(t10_5), t2.w * float(t10_5));
    t2.xyz = t2.xyz * vec3(vec3(_FadeDistance, _FadeDistance, _FadeDistance)) + t4.xyz;
    t4.xyz = t3.yzx * vec3(1.0, 0.0, 0.0);
    t3.xyz = t3.xyz * vec3(0.0, 0.0, 1.0) + (-t4.xyz);
    t18 = dot(t3.xz, t3.xz);
    t18 = inversesqrt(t18);
    t3.xyz = vec3(t18) * t3.xyz;
    t4.xyz = t7.yyy * vec3(0.0, 1.0, 0.0);
    t7.xyz = t3.xyz * t7.xxx + t4.xyz;
    t3.xyz = vec3(t10_5) * vec3(0.0, 1.0, 0.0);
    t18 = -abs(t10_5) * 2.0 + 1.0;
    t7.xyz = t3.xyz * vec3(vec3(_FadeDistance, _FadeDistance, _FadeDistance)) + t7.xyz;
    t7.xyz = (-t2.xyz) + t7.xyz;
    t7.xyz = in_COLOR0.yyy * t7.xyz + t2.xyz;
    t0.xyz = t0.xyz + t7.xyz;
    t2 = t0.yyyy * unity_MatrixV[1];
    t2 = unity_MatrixV[0] * t0.xxxx + t2;
    t2 = unity_MatrixV[2] * t0.zzzz + t2;
    vs_TEXCOORD1.xyz = t0.xyz;
    t2 = unity_MatrixV[3] * t3.wwww + t2;
    t3 = t2.yyyy * glstate_matrix_projection[1];
    t3 = glstate_matrix_projection[0] * t2.xxxx + t3;
    t3 = glstate_matrix_projection[2] * t2.zzzz + t3;
    t2 = glstate_matrix_projection[3] * t2.wwww + t3;
    gl_Position = t2;
    t0.xy = in_TEXCOORD0.xy + (-in_TEXCOORD1.xy);
    t0.xy = in_COLOR0.yy * t0.xy + in_TEXCOORD1.xy;
    vs_TEXCOORD0.xy = t0.xy;
    t2 = _TintColor + _TintColor;
    t1 = t1.xxxx * t2;
    t0 = vec4(t18) * t1;
    vs_TEXCOORD2 = t0 + t0;
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
uniform 	vec4 _Frequency;
uniform 	vec4 _Amplitude;
uniform 	float _Thickness;
uniform 	float _FadeDistance;
uniform 	vec4 _CustomTime;
uniform lowp sampler2D _MainTex;
in mediump vec2 vs_TEXCOORD0;
in mediump vec3 vs_TEXCOORD1;
in mediump vec4 vs_TEXCOORD2;
layout(location = 0) out mediump vec4 SV_Target0;
highp vec4 t0;
mediump vec4 t16_1;
lowp vec4 t10_1;
void main()
{
    t0.xyz = vs_TEXCOORD1.xyz + (-_WorldSpaceCameraPos.xyzx.xyz);
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t0.x = float(1.0) / t0.x;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_1 = t10_1 * vs_TEXCOORD2;
    t0 = t0.xxxx * t16_1;
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
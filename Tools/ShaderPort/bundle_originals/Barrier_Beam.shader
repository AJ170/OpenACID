Shader "Barrier/Beam" {
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
  GpuProgramID 29169
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
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesTANGENT;
  mediump float fade_h_2;
  mediump float fade_v_3;
  highp float va_center_4;
  highp vec2 size_5;
  highp float t_6;
  highp vec3 modelRight_7;
  highp float hv_8;
  highp vec4 pos_9;
  mediump vec2 tmpvar_10;
  mediump vec3 tmpvar_11;
  mediump vec4 tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = (_Object2World * _glesVertex);
  pos_9.w = tmpvar_13.w;
  highp vec4 tmpvar_14;
  tmpvar_14.w = 0.0;
  tmpvar_14.xyz = _glesNormal;
  highp vec3 tmpvar_15;
  tmpvar_15 = (_Object2World * tmpvar_14).xyz;
  lowp float tmpvar_16;
  tmpvar_16 = _glesColor.y;
  hv_8 = tmpvar_16;
  highp vec4 tmpvar_17;
  tmpvar_17 = (sin((
    (tmpvar_15.y * 6.283185)
   + 
    ((_Frequency * _Time.zzzz) * (0.5 + (tmpvar_15.x * 0.5)))
  )) * _Amplitude);
  highp vec3 tmpvar_18;
  tmpvar_18.z = 0.0;
  tmpvar_18.xy = tmpvar_17.xy;
  highp vec3 tmpvar_19;
  tmpvar_19.z = 0.0;
  tmpvar_19.xy = tmpvar_17.zw;
  highp vec3 tmpvar_20;
  tmpvar_20.y = 0.0;
  tmpvar_20.x = tmpvar_17.x;
  tmpvar_20.z = tmpvar_17.y;
  highp vec3 tmpvar_21;
  tmpvar_21.y = 0.0;
  tmpvar_21.x = tmpvar_17.z;
  tmpvar_21.z = tmpvar_17.w;
  highp vec3 tmpvar_22;
  tmpvar_22 = mix ((tmpvar_18 + tmpvar_19), (tmpvar_20 + tmpvar_21), vec3(hv_8));
  highp vec4 tmpvar_23;
  tmpvar_23.w = 0.0;
  tmpvar_23.xyz = tmpvar_1.xyz;
  modelRight_7.xz = (_Object2World * tmpvar_23).xz;
  modelRight_7.y = 0.0;
  highp vec3 tmpvar_24;
  tmpvar_24 = normalize(modelRight_7);
  modelRight_7 = tmpvar_24;
  highp vec3 tmpvar_25;
  tmpvar_25 = ((tmpvar_24.yzx * vec3(0.0, 0.0, 1.0)) - (tmpvar_24.zxy * vec3(1.0, 0.0, 0.0)));
  pos_9.xyz = (tmpvar_13.xyz + ((tmpvar_22.x * tmpvar_25) + (tmpvar_22.y * vec3(0.0, 1.0, 0.0))));
  highp vec3 tmpvar_26;
  tmpvar_26 = (_WorldSpaceCameraPos - pos_9.xyz);
  highp vec3 tmpvar_27;
  tmpvar_27 = (((tmpvar_24 * tmpvar_26.z) + (vec3(0.0, 1.0, 0.0) * tmpvar_26.y)) + (tmpvar_25 * tmpvar_26.x));
  highp vec2 tmpvar_28;
  tmpvar_28 = mix (tmpvar_27.xy, tmpvar_27.xz, _glesColor.yy);
  highp float tmpvar_29;
  tmpvar_29 = sqrt(dot (tmpvar_28, tmpvar_28));
  t_6 = (_Thickness * min (1.0, (tmpvar_29 * 0.1)));
  highp vec2 tmpvar_30;
  tmpvar_30.x = tmpvar_1.w;
  tmpvar_30.y = t_6;
  highp vec2 tmpvar_31;
  tmpvar_31.x = t_6;
  tmpvar_31.y = tmpvar_1.w;
  size_5 = (mix (tmpvar_30, tmpvar_31, vec2(hv_8)) * ((_glesMultiTexCoord0.xy * 2.0) - 1.0));
  highp vec3 tmpvar_32;
  tmpvar_32 = normalize(tmpvar_26);
  lowp float tmpvar_33;
  tmpvar_33 = (_glesColor.w - 0.4980392);
  va_center_4 = tmpvar_33;
  highp float tmpvar_34;
  tmpvar_34 = (1.0 - (abs(va_center_4) * 2.0));
  pos_9.xyz = (pos_9.xyz + mix ((
    ((tmpvar_24 * size_5.x) + (normalize((
      (tmpvar_24.yzx * tmpvar_32.zxy)
     - 
      (tmpvar_24.zxy * tmpvar_32.yzx)
    )) * size_5.y))
   + 
    ((tmpvar_24 * va_center_4) * _FadeDistance)
  ), (
    ((normalize((
      (tmpvar_32.yzx * vec3(0.0, 0.0, 1.0))
     - 
      (tmpvar_32.zxy * vec3(1.0, 0.0, 0.0))
    )) * size_5.x) + (vec3(0.0, 1.0, 0.0) * size_5.y))
   + 
    ((vec3(0.0, 1.0, 0.0) * va_center_4) * _FadeDistance)
  ), vec3(hv_8)));
  highp vec3 tmpvar_35;
  tmpvar_35 = pos_9.xyz;
  tmpvar_11 = tmpvar_35;
  pos_9 = (unity_MatrixV * pos_9);
  pos_9 = (glstate_matrix_projection * pos_9);
  highp vec2 tmpvar_36;
  tmpvar_36 = mix (_glesMultiTexCoord1, _glesMultiTexCoord0, vec4(hv_8)).xy;
  tmpvar_10 = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = sin(((tmpvar_15.x * 6.283185) + (_Time.z * 
    (0.5 + (tmpvar_15.y * 0.5))
  )));
  tmpvar_12 = (((2.0 * _TintColor) * (1.0 + tmpvar_37)) * tmpvar_34);
  highp vec3 tmpvar_38;
  tmpvar_38 = abs(tmpvar_27);
  highp float tmpvar_39;
  tmpvar_39 = max ((min (1.0, 
    pow ((12.5 / tmpvar_38.x), 4.0)
  ) - clamp (
    ((tmpvar_38.z - _glesTANGENT.w) * 0.05)
  , 0.0, 1.0)), 0.0);
  fade_v_3 = tmpvar_39;
  highp float tmpvar_40;
  tmpvar_40 = min (1.0, pow ((17.5 / tmpvar_29), 4.0));
  fade_h_2 = tmpvar_40;
  lowp float tmpvar_41;
  tmpvar_41 = clamp (_glesColor.y, 0.0, 1.0);
  tmpvar_12.xyz = (tmpvar_12.xyz * mix (fade_v_3, fade_h_2, tmpvar_41));
  gl_Position = pos_9;
  xlv_TEXCOORD0 = tmpvar_10;
  xlv_TEXCOORD1 = tmpvar_11;
  xlv_TEXCOORD2 = tmpvar_12;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD0);
  gl_FragData[0] = (tmpvar_1 * xlv_TEXCOORD2);
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
highp vec3 t5;
highp vec3 t6;
lowp float t10_7;
mediump float t16_8;
highp vec3 t9;
highp vec3 t10;
highp vec2 t12;
highp vec2 t18;
highp vec2 t19;
highp float t27;
highp float t28;
highp float t29;
void main()
{
    t0 = _Time.zzzz * _Frequency;
    t1 = in_NORMAL0.yyyy * _Object2World[1].xyyx;
    t1 = _Object2World[0].xyyx * in_NORMAL0.xxxx + t1;
    t1 = _Object2World[2].xyyx * in_NORMAL0.zzzz + t1;
    t10.xz = vec2(t1.y * float(6.28318548), t1.w * float(6.28318548));
    t1.xz = t1.xz * vec2(0.5, 0.5) + vec2(0.5, 0.5);
    t0 = t0 * t1.xxxx + t10.xxxx;
    t1.x = _Time.z * t1.z + t10.z;
    t1.x = sin(t1.x);
    t1.x = t1.x + 1.0;
    t0 = sin(t0);
    t0 = t0 * _Amplitude;
    t0.xyz = vec3(t0.z + t0.x, t0.w + t0.y, t0.z + t0.x);
    t0.w = 0.0;
    t18.xy = vec2((-t0.x) + t0.z, (-t0.y) + t0.w);
    t0.xy = in_COLOR0.yy * t18.xy + t0.xy;
    t9.xyz = t0.yyy * vec3(0.0, 1.0, 0.0);
    t10.xy = in_TANGENT0.yy * _Object2World[1].xz;
    t10.xy = _Object2World[0].xz * in_TANGENT0.xx + t10.xy;
    t10.xy = _Object2World[2].xz * in_TANGENT0.zz + t10.xy;
    t28 = dot(t10.xy, t10.xy);
    t2.x = inversesqrt(t28);
    t2.yw = t10.xy * t2.xx;
    t10.xyz = t2.wyx * vec3(1.0, 0.0, 0.0);
    t10.xyz = t2.xwy * vec3(0.0, 0.0, 1.0) + (-t10.xyz);
    t0.xyz = t0.xxx * t10.xyz + t9.xyz;
    t3 = in_POSITION0.yyyy * _Object2World[1];
    t3 = _Object2World[0] * in_POSITION0.xxxx + t3;
    t3 = _Object2World[2] * in_POSITION0.zzzz + t3;
    t3 = _Object2World[3] * in_POSITION0.wwww + t3;
    t0.xyz = t0.xyz + t3.xyz;
    t3.xyz = (-t0.yzx) + _WorldSpaceCameraPos.xyzx.yzx;
    t27 = dot(t3.xyz, t3.xyz);
    t27 = inversesqrt(t27);
    t4.xyz = vec3(t27) * t3.xyz;
    t2.z = 0.0;
    t5.xyz = vec3(t2.w * t4.x, t2.y * t4.y, t2.z * t4.z);
    t5.xyz = t2.zwy * t4.yzx + (-t5.xyz);
    t27 = dot(t5.xyz, t5.xyz);
    t27 = inversesqrt(t27);
    t5.xyz = vec3(t27) * t5.xyz;
    t6.xyz = t3.xxx * vec3(0.0, 1.0, 0.0);
    t6.xyz = t2.yzw * t3.yyy + t6.xyz;
    t10.xyz = t10.xyz * t3.zzz + t6.xyz;
    t3.xy = (-t10.xy) + t10.xz;
    t3.xy = in_COLOR0.yy * t3.xy + t10.xy;
    t27 = dot(t3.xy, t3.xy);
    t27 = sqrt(t27);
    t19.x = t27 * 0.100000001;
    t27 = 17.5 / t27;
    t27 = t27 * t27;
    t27 = t27 * t27;
    t27 = min(t27, 1.0);
    t19.x = min(t19.x, 1.0);
    t3.y = t19.x * _Thickness;
    t3.x = in_TANGENT0.w;
    t6.xy = (-t3.xy) + t3.yx;
    t12.xy = in_COLOR0.yy * t6.xy + t3.xy;
    t19.x = abs(t10.z) + (-t3.x);
    t10.x = 12.5 / abs(t10.x);
    t10.x = t10.x * t10.x;
    t10.x = t10.x * t10.x;
    t10.x = min(t10.x, 1.0);
    t19.x = t19.x * 0.0500000007;
    t19.x = clamp(t19.x, 0.0, 1.0);
    t10.x = (-t19.x) + t10.x;
    t10.x = max(t10.x, 0.0);
    t19.xy = in_TEXCOORD0.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
    t19.xy = t19.xy * t12.xy;
    t3.xyz = t19.yyy * t5.xyz;
    t3.xyz = t2.yzw * t19.xxx + t3.xyz;
    t10_7 = in_COLOR0.w + -0.498039216;
    t2.xyz = vec3(t2.y * float(t10_7), t2.z * float(t10_7), t2.w * float(t10_7));
    t2.xyz = t2.xyz * vec3(vec3(_FadeDistance, _FadeDistance, _FadeDistance)) + t3.xyz;
    t3.xyz = t4.yzx * vec3(1.0, 0.0, 0.0);
    t3.xyz = t4.xyz * vec3(0.0, 0.0, 1.0) + (-t3.xyz);
    t29 = dot(t3.xz, t3.xz);
    t29 = inversesqrt(t29);
    t3.xyz = vec3(t29) * t3.xyz;
    t4.xyz = t19.yyy * vec3(0.0, 1.0, 0.0);
    t3.xyz = t3.xyz * t19.xxx + t4.xyz;
    t4.xyz = vec3(t10_7) * vec3(0.0, 1.0, 0.0);
    t19.x = -abs(t10_7) * 2.0 + 1.0;
    t3.xyz = t4.xyz * vec3(vec3(_FadeDistance, _FadeDistance, _FadeDistance)) + t3.xyz;
    t3.xyz = (-t2.xyz) + t3.xyz;
    t2.xyz = in_COLOR0.yyy * t3.xyz + t2.xyz;
    t0.xyz = t0.xyz + t2.xyz;
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
    t16_8 = t27 + (-t10.x);
    t10_7 = in_COLOR0.y;
    t10_7 = clamp(t10_7, 0.0, 1.0);
    t16_8 = t10_7 * t16_8 + t10.x;
    t0 = _TintColor + _TintColor;
    t0 = t1.xxxx * t0;
    t0 = t19.xxxx * t0;
    vs_TEXCOORD2.xyz = vec3(t16_8) * t0.xyz;
    vs_TEXCOORD2.w = t0.w;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
uniform lowp sampler2D _MainTex;
in mediump vec2 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD2;
layout(location = 0) out mediump vec4 SV_Target0;
mediump vec4 t16_0;
lowp vec4 t10_0;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_0 = t10_0 * vs_TEXCOORD2;
    SV_Target0 = t16_0;
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
Shader "Animus/BillboardMeshCubemap" {
Properties {
 _MainTex ("Billboard Texture", 2D) = "white" { }
 _FadeOffset ("Fade Offset", Float) = 0.25
}
SubShader { 
 Tags { "QUEUE"="Background" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Background" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha One
  ColorMask RGB
  GpuProgramID 22627
Program "vp" {
SubProgram "gles " {
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp mat4 glstate_matrix_projection;
uniform highp float _FadeOffset;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  highp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec4 vPos_3;
  highp vec4 wPos_4;
  highp vec4 tmpvar_5;
  tmpvar_5 = (_Object2World * _glesVertex);
  wPos_4.w = tmpvar_5.w;
  wPos_4.xyz = (tmpvar_5.xyz + _WorldSpaceCameraPos);
  wPos_4 = (_World2Object * wPos_4);
  highp vec4 tmpvar_6;
  tmpvar_6 = (glstate_matrix_modelview0 * wPos_4);
  vPos_3.zw = tmpvar_6.zw;
  highp float tmpvar_7;
  tmpvar_7 = (((_glesVertex.x * _glesVertex.y) * _glesVertex.z) + (_Time.z * 0.25));
  vPos_3.x = (tmpvar_6.x + (sin(
    (tmpvar_7 * 0.5)
  ) * 0.25));
  vPos_3.y = (tmpvar_6.y + (-0.25 - (
    sin(tmpvar_7)
   * 0.1)));
  highp float tmpvar_8;
  tmpvar_8 = (sin((tmpvar_7 * 0.05)) * 10.0);
  highp float tmpvar_9;
  highp float tmpvar_10;
  highp float tmpvar_11;
  tmpvar_11 = (_glesMultiTexCoord1.x / _glesMultiTexCoord1.y);
  tmpvar_10 = (min (abs(tmpvar_11), 1.0) / max (abs(tmpvar_11), 1.0));
  highp float tmpvar_12;
  tmpvar_12 = (tmpvar_10 * tmpvar_10);
  tmpvar_12 = (((
    ((((
      ((((-0.01213232 * tmpvar_12) + 0.05368138) * tmpvar_12) - 0.1173503)
     * tmpvar_12) + 0.1938925) * tmpvar_12) - 0.3326756)
   * tmpvar_12) + 0.9999793) * tmpvar_10);
  tmpvar_12 = (tmpvar_12 + (float(
    (abs(tmpvar_11) > 1.0)
  ) * (
    (tmpvar_12 * -2.0)
   + 1.570796)));
  tmpvar_9 = (tmpvar_12 * sign(tmpvar_11));
  if ((abs(_glesMultiTexCoord1.y) > (1e-08 * abs(_glesMultiTexCoord1.x)))) {
    if ((_glesMultiTexCoord1.y < 0.0)) {
      if ((_glesMultiTexCoord1.x >= 0.0)) {
        tmpvar_9 += 3.141593;
      } else {
        tmpvar_9 = (tmpvar_9 - 3.141593);
      };
    };
  } else {
    tmpvar_9 = (sign(_glesMultiTexCoord1.x) * 1.570796);
  };
  vPos_3.x = (vPos_3.x + (sin(
    (tmpvar_8 + tmpvar_9)
  ) * (1.0 + 
    (sin(tmpvar_7) * 0.25)
  )));
  highp float tmpvar_13;
  highp float tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = (_glesMultiTexCoord1.x / _glesMultiTexCoord1.y);
  tmpvar_14 = (min (abs(tmpvar_15), 1.0) / max (abs(tmpvar_15), 1.0));
  highp float tmpvar_16;
  tmpvar_16 = (tmpvar_14 * tmpvar_14);
  tmpvar_16 = (((
    ((((
      ((((-0.01213232 * tmpvar_16) + 0.05368138) * tmpvar_16) - 0.1173503)
     * tmpvar_16) + 0.1938925) * tmpvar_16) - 0.3326756)
   * tmpvar_16) + 0.9999793) * tmpvar_14);
  tmpvar_16 = (tmpvar_16 + (float(
    (abs(tmpvar_15) > 1.0)
  ) * (
    (tmpvar_16 * -2.0)
   + 1.570796)));
  tmpvar_13 = (tmpvar_16 * sign(tmpvar_15));
  if ((abs(_glesMultiTexCoord1.y) > (1e-08 * abs(_glesMultiTexCoord1.x)))) {
    if ((_glesMultiTexCoord1.y < 0.0)) {
      if ((_glesMultiTexCoord1.x >= 0.0)) {
        tmpvar_13 += 3.141593;
      } else {
        tmpvar_13 = (tmpvar_13 - 3.141593);
      };
    };
  } else {
    tmpvar_13 = (sign(_glesMultiTexCoord1.x) * 1.570796);
  };
  vPos_3.y = (vPos_3.y + (cos(
    (tmpvar_8 + tmpvar_13)
  ) * (1.0 + 
    (sin(tmpvar_7) * 0.25)
  )));
  tmpvar_2.w = (vPos_3.y - _FadeOffset);
  gl_Position = (glstate_matrix_projection * vPos_3);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  highp vec4 tmpvar_2;
  tmpvar_2 = (tmpvar_1 * xlv_TEXCOORD1);
  gl_FragData[0] = tmpvar_2;
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
uniform 	float _FadeOffset;
in highp vec4 in_POSITION0;
in highp vec4 in_COLOR0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD1;
out highp vec4 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
highp vec4 t3;
highp vec3 t4;
highp int ti4;
bool tb4;
highp vec3 t5;
highp float t8;
bool tb8;
highp float t12;
bool tb12;
void main()
{
    t0.x = max(abs(in_TEXCOORD1.y), abs(in_TEXCOORD1.x));
    t0.x = float(1.0) / t0.x;
    t4.x = min(abs(in_TEXCOORD1.y), abs(in_TEXCOORD1.x));
    t0.x = t0.x * t4.x;
    t4.x = t0.x * t0.x;
    t8 = t4.x * 0.0208350997 + -0.0851330012;
    t8 = t4.x * t8 + 0.180141002;
    t8 = t4.x * t8 + -0.330299497;
    t4.x = t4.x * t8 + 0.999866009;
    t8 = t4.x * t0.x;
    t8 = t8 * -2.0 + 1.57079637;
    tb12 = abs(in_TEXCOORD1.y)<abs(in_TEXCOORD1.x);
    t8 = tb12 ? t8 : float(0.0);
    t0.x = t0.x * t4.x + t8;
    tb4 = in_TEXCOORD1.y<(-in_TEXCOORD1.y);
    t4.x = tb4 ? -3.14159274 : float(0.0);
    t0.x = t4.x + t0.x;
    t4.x = min(in_TEXCOORD1.y, in_TEXCOORD1.x);
    tb4 = t4.x<(-t4.x);
    t8 = max(in_TEXCOORD1.y, in_TEXCOORD1.x);
    tb8 = t8>=(-t8);
    ti4 = int(uint(tb8) * 0xffffffffu & uint(tb4) * 0xffffffffu);
    t0.x = (ti4 != 0) ? (-t0.x) : t0.x;
    t4.x = in_POSITION0.y * in_POSITION0.x;
    t8 = _Time.z * 0.25;
    t4.x = t4.x * in_POSITION0.z + t8;
    t4.yz = t4.xx * vec2(0.5, 0.0500000007);
    t4.xyz = sin(t4.xyz);
    t0.x = t4.z * 10.0 + t0.x;
    t1.x = cos(t0.x);
    t0.x = sin(t0.x);
    t2 = in_POSITION0.yyyy * _Object2World[1];
    t2 = _Object2World[0] * in_POSITION0.xxxx + t2;
    t2 = _Object2World[2] * in_POSITION0.zzzz + t2;
    t2 = _Object2World[3] * in_POSITION0.wwww + t2;
    t5.xyz = t2.xyz + _WorldSpaceCameraPos.xyzx.xyz;
    t3 = t5.yyyy * _World2Object[1];
    t3 = _World2Object[0] * t5.xxxx + t3;
    t3 = _World2Object[2] * t5.zzzz + t3;
    t2 = _World2Object[3] * t2.wwww + t3;
    t3 = t2.yyyy * glstate_matrix_modelview0[1];
    t3 = glstate_matrix_modelview0[0] * t2.xxxx + t3;
    t3 = glstate_matrix_modelview0[2] * t2.zzzz + t3;
    t2 = glstate_matrix_modelview0[3] * t2.wwww + t3;
    t8 = t4.y * 0.25 + t2.x;
    t12 = t4.x * 0.25 + 1.0;
    t4.x = (-t4.x) * 0.100000001 + t2.y;
    t4.x = t4.x + -0.25;
    t4.x = t1.x * t12 + t4.x;
    t0.x = t0.x * t12 + t8;
    t1 = t4.xxxx * glstate_matrix_projection[1];
    vs_TEXCOORD1.w = t4.x + (-_FadeOffset);
    t0 = glstate_matrix_projection[0] * t0.xxxx + t1;
    t0 = glstate_matrix_projection[2] * t2.zzzz + t0;
    gl_Position = glstate_matrix_projection[3] * t2.wwww + t0;
    vs_TEXCOORD0 = in_TEXCOORD0;
    vs_TEXCOORD1.xyz = in_COLOR0.xyz;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
uniform lowp sampler2D _MainTex;
in highp vec4 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
layout(location = 0) out highp vec4 SV_Target0;
lowp vec4 t10_0;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
    SV_Target0 = t10_0 * vs_TEXCOORD1;
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
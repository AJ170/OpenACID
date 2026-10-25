Shader "Hidden/LqBlurIteration" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
}
SubShader { 
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  GpuProgramID 14992
Program "vp" {
SubProgram "gles " {
Keywords { "SGX_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform mediump vec4 _MainTex_TexelSize;
uniform mediump float _Parameter;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 tmpvar_1;
  tmpvar_1.zw = vec2(1.0, 1.0);
  tmpvar_1.xy = _glesMultiTexCoord0.xy;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_MainTex_TexelSize.xy * vec2(0.0, 1.0)) * _Parameter);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 color_1;
  mediump vec2 coords_2;
  coords_2 = (xlv_TEXCOORD0.xy - (xlv_TEXCOORD1 * 3.0));
  mediump vec4 tap_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, coords_2);
  tap_3 = tmpvar_4;
  color_1 = (tap_3 * vec4(0.0205, 0.0205, 0.0205, 0.0));
  coords_2 = (coords_2 + xlv_TEXCOORD1);
  mediump vec4 tap_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, coords_2);
  tap_5 = tmpvar_6;
  color_1 = (color_1 + (tap_5 * vec4(0.0855, 0.0855, 0.0855, 0.0)));
  coords_2 = (coords_2 + xlv_TEXCOORD1);
  mediump vec4 tap_7;
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, coords_2);
  tap_7 = tmpvar_8;
  color_1 = (color_1 + (tap_7 * vec4(0.232, 0.232, 0.232, 0.0)));
  coords_2 = (coords_2 + xlv_TEXCOORD1);
  mediump vec4 tap_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, coords_2);
  tap_9 = tmpvar_10;
  color_1 = (color_1 + (tap_9 * vec4(0.324, 0.324, 0.324, 1.0)));
  coords_2 = (coords_2 + xlv_TEXCOORD1);
  mediump vec4 tap_11;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_MainTex, coords_2);
  tap_11 = tmpvar_12;
  color_1 = (color_1 + (tap_11 * vec4(0.232, 0.232, 0.232, 0.0)));
  coords_2 = (coords_2 + xlv_TEXCOORD1);
  mediump vec4 tap_13;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_MainTex, coords_2);
  tap_13 = tmpvar_14;
  color_1 = (color_1 + (tap_13 * vec4(0.0855, 0.0855, 0.0855, 0.0)));
  coords_2 = (coords_2 + xlv_TEXCOORD1);
  mediump vec4 tap_15;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_MainTex, coords_2);
  tap_15 = tmpvar_16;
  color_1 = (color_1 + (tap_15 * vec4(0.0205, 0.0205, 0.0205, 0.0)));
  coords_2 = (coords_2 + xlv_TEXCOORD1);
  gl_FragData[0] = color_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SGX_OFF" }
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
uniform 	mediump vec4 _MainTex_TexelSize;
uniform 	mediump float _Parameter;
in highp vec4 in_POSITION0;
in mediump vec2 in_TEXCOORD0;
out vec4 undefined;
out mediump vec4 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
highp vec4 t0;
mediump vec4 t16_0;
mediump vec2 t16_1;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    gl_Position = t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    vs_TEXCOORD0.zw = vec2(1.0, 1.0);
    t16_0.xw = vec2(_Parameter);
    t16_0.yz = vec2(1.0, 0.0);
    t16_1.xy = t16_0.xy * _MainTex_TexelSize.xy;
    vs_TEXCOORD1.xy = vec2(t16_0.z * t16_1.x, t16_0.w * t16_1.y);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
// XXX ADRENOCONSTARRAYBUG
#define immediateConstBufferF (vec4[7] (\
	vec4(0.0205000006, 0, 0, 0), \
	vec4(0.0855000019, 0, 0, 0), \
	vec4(0.231999993, 0, 0, 0), \
	vec4(0.324000001, 0, 0, 1), \
	vec4(0.231999993, 0, 0, 0), \
	vec4(0.0855000019, 0, 0, 0), \
	vec4(0.0205000006, 0, 0, 0)))
uniform lowp sampler2D _MainTex;
in mediump vec4 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
layout(location = 0) out mediump vec4 SV_Target0;
mediump vec2 t16_0;
mediump vec4 t16_1;
highp int ti2;
lowp vec4 t10_3;
bool tb6;
mediump vec2 t16_8;
void main()
{
    t16_0.xy = (-vs_TEXCOORD1.xy) * vec2(3.0, 3.0) + vs_TEXCOORD0.xy;
    t16_1 = vec4(0.0, 0.0, 0.0, 0.0);
    t16_8.xy = t16_0.xy;
    ti2 = 0x0;
    while(true){
        tb6 = ti2>=0x7;
        if(tb6){break;}
        t10_3 = texture(_MainTex, t16_8.xy);
        t16_1 = t10_3 * immediateConstBufferF[ti2].xxxw + t16_1;
        t16_8.xy = t16_8.xy + vs_TEXCOORD1.xy;
        ti2 = ti2 + 0x1;
    }
    SV_Target0 = t16_1;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SGX_ON" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform mediump vec4 _MainTex_TexelSize;
uniform mediump float _Parameter;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD1_1;
varying mediump vec4 xlv_TEXCOORD1_2;
void main ()
{
  mediump vec4 coords_1;
  mediump vec4 tmpvar_2;
  mediump vec4 tmpvar_3;
  tmpvar_3.zw = vec2(1.0, 1.0);
  tmpvar_3.xy = _glesMultiTexCoord0.xy;
  mediump vec2 tmpvar_4;
  tmpvar_4 = ((_MainTex_TexelSize.xy * vec2(0.0, 1.0)) * _Parameter);
  mediump vec4 tmpvar_5;
  tmpvar_5 = (-(tmpvar_4.xyxy) * 3.0);
  coords_1 = (tmpvar_5 + tmpvar_4.xyxy);
  tmpvar_2 = (_glesMultiTexCoord0.xyxy + (coords_1 * vec4(1.0, 1.0, -1.0, -1.0)));
  coords_1 = (coords_1 + tmpvar_4.xyxy);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = (_glesMultiTexCoord0.xyxy + (tmpvar_5 * vec4(1.0, 1.0, -1.0, -1.0)));
  xlv_TEXCOORD1_1 = tmpvar_2;
  xlv_TEXCOORD1_2 = (_glesMultiTexCoord0.xyxy + (coords_1 * vec4(1.0, 1.0, -1.0, -1.0)));
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD1_1;
varying mediump vec4 xlv_TEXCOORD1_2;
void main ()
{
  mediump vec4 color_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  color_1 = (tmpvar_2 * vec4(0.324, 0.324, 0.324, 1.0));
  mediump vec4 tapB_3;
  mediump vec4 tapA_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1.xy);
  tapA_4 = tmpvar_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD1.zw);
  tapB_3 = tmpvar_6;
  color_1 = (color_1 + ((tapA_4 + tapB_3) * vec4(0.0205, 0.0205, 0.0205, 0.0)));
  mediump vec4 tapB_7;
  mediump vec4 tapA_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD1_1.xy);
  tapA_8 = tmpvar_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD1_1.zw);
  tapB_7 = tmpvar_10;
  color_1 = (color_1 + ((tapA_8 + tapB_7) * vec4(0.0855, 0.0855, 0.0855, 0.0)));
  mediump vec4 tapB_11;
  mediump vec4 tapA_12;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_MainTex, xlv_TEXCOORD1_2.xy);
  tapA_12 = tmpvar_13;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_MainTex, xlv_TEXCOORD1_2.zw);
  tapB_11 = tmpvar_14;
  color_1 = (color_1 + ((tapA_12 + tapB_11) * vec4(0.232, 0.232, 0.232, 0.0)));
  gl_FragData[0] = color_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SGX_ON" }
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
uniform 	mediump vec4 _MainTex_TexelSize;
uniform 	mediump float _Parameter;
in highp vec4 in_POSITION0;
in mediump vec2 in_TEXCOORD0;
out vec4 undefined;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec4 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
highp vec4 t0;
mediump vec3 t16_1;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    gl_Position = t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    vs_TEXCOORD0.zw = vec2(1.0, 1.0);
    t16_1.x = _Parameter;
    t16_1.y = 1.0;
    t16_1.xy = t16_1.xy * _MainTex_TexelSize.xy;
    t16_1.z = t16_1.y * _Parameter;
    vs_TEXCOORD1 = t16_1.xzxz * vec4(-0.0, -3.0, 0.0, 3.0) + in_TEXCOORD0.xyxy;
    vs_TEXCOORD2 = t16_1.xzxz * vec4(0.0, -2.0, -0.0, 2.0) + in_TEXCOORD0.xyxy;
    vs_TEXCOORD3 = t16_1.xzxz * vec4(0.0, -1.0, -0.0, 1.0) + in_TEXCOORD0.xyxy;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
// XXX ADRENOCONSTARRAYBUG
#define immediateConstBufferF (vec4[7] (\
	vec4(0.0205000006, 0, 0, 0), \
	vec4(0.0855000019, 0, 0, 0), \
	vec4(0.231999993, 0, 0, 0), \
	vec4(0.324000001, 0, 0, 1), \
	vec4(0.231999993, 0, 0, 0), \
	vec4(0.0855000019, 0, 0, 0), \
	vec4(0.0205000006, 0, 0, 0)))
uniform lowp sampler2D _MainTex;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec4 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
layout(location = 0) out mediump vec4 SV_Target0;
mediump vec4 t16_0;
lowp vec4 t10_0;
mediump vec4 t16_1;
highp int ti2;
mediump vec4 t16_3;
lowp vec4 t10_3;
lowp vec4 t10_4;
bool tb7;
mediump vec4 phase0_Input2[3];
void main()
{
phase0_Input2[0] = vs_TEXCOORD1;
phase0_Input2[1] = vs_TEXCOORD2;
phase0_Input2[2] = vs_TEXCOORD3;
    t10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_0 = t10_0 * vec4(0.324000001, 0.324000001, 0.324000001, 1.0);
    t16_1 = t16_0;
    ti2 = 0x0;
    while(true){
        tb7 = ti2>=0x3;
        if(tb7){break;}
        t10_3 = texture(_MainTex, phase0_Input2[ti2].xy);
        t10_4 = texture(_MainTex, phase0_Input2[ti2].zw);
        t16_3 = t10_3 + t10_4;
        t16_1 = t16_3 * immediateConstBufferF[ti2].xxxw + t16_1;
        ti2 = ti2 + 0x1;
    }
    SV_Target0 = t16_1;
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SGX_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SGX_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SGX_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SGX_ON" }
"!!GLES3"
}
}
 }
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  GpuProgramID 69326
Program "vp" {
SubProgram "gles " {
Keywords { "SGX_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform mediump vec4 _MainTex_TexelSize;
uniform mediump float _Parameter;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 tmpvar_1;
  tmpvar_1.zw = vec2(1.0, 1.0);
  tmpvar_1.xy = _glesMultiTexCoord0.xy;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_MainTex_TexelSize.xy * vec2(1.0, 0.0)) * _Parameter);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 color_1;
  mediump vec2 coords_2;
  coords_2 = (xlv_TEXCOORD0.xy - (xlv_TEXCOORD1 * 3.0));
  mediump vec4 tap_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, coords_2);
  tap_3 = tmpvar_4;
  color_1 = (tap_3 * vec4(0.0205, 0.0205, 0.0205, 0.0));
  coords_2 = (coords_2 + xlv_TEXCOORD1);
  mediump vec4 tap_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, coords_2);
  tap_5 = tmpvar_6;
  color_1 = (color_1 + (tap_5 * vec4(0.0855, 0.0855, 0.0855, 0.0)));
  coords_2 = (coords_2 + xlv_TEXCOORD1);
  mediump vec4 tap_7;
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, coords_2);
  tap_7 = tmpvar_8;
  color_1 = (color_1 + (tap_7 * vec4(0.232, 0.232, 0.232, 0.0)));
  coords_2 = (coords_2 + xlv_TEXCOORD1);
  mediump vec4 tap_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, coords_2);
  tap_9 = tmpvar_10;
  color_1 = (color_1 + (tap_9 * vec4(0.324, 0.324, 0.324, 1.0)));
  coords_2 = (coords_2 + xlv_TEXCOORD1);
  mediump vec4 tap_11;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_MainTex, coords_2);
  tap_11 = tmpvar_12;
  color_1 = (color_1 + (tap_11 * vec4(0.232, 0.232, 0.232, 0.0)));
  coords_2 = (coords_2 + xlv_TEXCOORD1);
  mediump vec4 tap_13;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_MainTex, coords_2);
  tap_13 = tmpvar_14;
  color_1 = (color_1 + (tap_13 * vec4(0.0855, 0.0855, 0.0855, 0.0)));
  coords_2 = (coords_2 + xlv_TEXCOORD1);
  mediump vec4 tap_15;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_MainTex, coords_2);
  tap_15 = tmpvar_16;
  color_1 = (color_1 + (tap_15 * vec4(0.0205, 0.0205, 0.0205, 0.0)));
  coords_2 = (coords_2 + xlv_TEXCOORD1);
  gl_FragData[0] = color_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SGX_OFF" }
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
uniform 	mediump vec4 _MainTex_TexelSize;
uniform 	mediump float _Parameter;
in highp vec4 in_POSITION0;
in mediump vec2 in_TEXCOORD0;
out vec4 undefined;
out mediump vec4 vs_TEXCOORD0;
out mediump vec2 vs_TEXCOORD1;
highp vec4 t0;
mediump vec4 t16_0;
mediump vec2 t16_1;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    gl_Position = t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    vs_TEXCOORD0.zw = vec2(1.0, 1.0);
    t16_0.xw = vec2(1.0, 0.0);
    t16_0.yz = vec2(_Parameter);
    t16_1.xy = t16_0.xy * _MainTex_TexelSize.xy;
    vs_TEXCOORD1.xy = vec2(t16_0.z * t16_1.x, t16_0.w * t16_1.y);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
// XXX ADRENOCONSTARRAYBUG
#define immediateConstBufferF (vec4[7] (\
	vec4(0.0205000006, 0, 0, 0), \
	vec4(0.0855000019, 0, 0, 0), \
	vec4(0.231999993, 0, 0, 0), \
	vec4(0.324000001, 0, 0, 1), \
	vec4(0.231999993, 0, 0, 0), \
	vec4(0.0855000019, 0, 0, 0), \
	vec4(0.0205000006, 0, 0, 0)))
uniform lowp sampler2D _MainTex;
in mediump vec4 vs_TEXCOORD0;
in mediump vec2 vs_TEXCOORD1;
layout(location = 0) out mediump vec4 SV_Target0;
mediump vec2 t16_0;
mediump vec4 t16_1;
highp int ti2;
lowp vec4 t10_3;
bool tb6;
mediump vec2 t16_8;
void main()
{
    t16_0.xy = (-vs_TEXCOORD1.xy) * vec2(3.0, 3.0) + vs_TEXCOORD0.xy;
    t16_1 = vec4(0.0, 0.0, 0.0, 0.0);
    t16_8.xy = t16_0.xy;
    ti2 = 0x0;
    while(true){
        tb6 = ti2>=0x7;
        if(tb6){break;}
        t10_3 = texture(_MainTex, t16_8.xy);
        t16_1 = t10_3 * immediateConstBufferF[ti2].xxxw + t16_1;
        t16_8.xy = t16_8.xy + vs_TEXCOORD1.xy;
        ti2 = ti2 + 0x1;
    }
    SV_Target0 = t16_1;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SGX_ON" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform mediump vec4 _MainTex_TexelSize;
uniform mediump float _Parameter;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD1_1;
varying mediump vec4 xlv_TEXCOORD1_2;
void main ()
{
  mediump vec4 coords_1;
  mediump vec4 tmpvar_2;
  mediump vec4 tmpvar_3;
  tmpvar_3.zw = vec2(1.0, 1.0);
  tmpvar_3.xy = _glesMultiTexCoord0.xy;
  mediump vec2 tmpvar_4;
  tmpvar_4 = ((_MainTex_TexelSize.xy * vec2(1.0, 0.0)) * _Parameter);
  mediump vec4 tmpvar_5;
  tmpvar_5 = (-(tmpvar_4.xyxy) * 3.0);
  coords_1 = (tmpvar_5 + tmpvar_4.xyxy);
  tmpvar_2 = (_glesMultiTexCoord0.xyxy + (coords_1 * vec4(1.0, 1.0, -1.0, -1.0)));
  coords_1 = (coords_1 + tmpvar_4.xyxy);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = (_glesMultiTexCoord0.xyxy + (tmpvar_5 * vec4(1.0, 1.0, -1.0, -1.0)));
  xlv_TEXCOORD1_1 = tmpvar_2;
  xlv_TEXCOORD1_2 = (_glesMultiTexCoord0.xyxy + (coords_1 * vec4(1.0, 1.0, -1.0, -1.0)));
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec4 xlv_TEXCOORD1_1;
varying mediump vec4 xlv_TEXCOORD1_2;
void main ()
{
  mediump vec4 color_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  color_1 = (tmpvar_2 * vec4(0.324, 0.324, 0.324, 1.0));
  mediump vec4 tapB_3;
  mediump vec4 tapA_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD1.xy);
  tapA_4 = tmpvar_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD1.zw);
  tapB_3 = tmpvar_6;
  color_1 = (color_1 + ((tapA_4 + tapB_3) * vec4(0.0205, 0.0205, 0.0205, 0.0)));
  mediump vec4 tapB_7;
  mediump vec4 tapA_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD1_1.xy);
  tapA_8 = tmpvar_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, xlv_TEXCOORD1_1.zw);
  tapB_7 = tmpvar_10;
  color_1 = (color_1 + ((tapA_8 + tapB_7) * vec4(0.0855, 0.0855, 0.0855, 0.0)));
  mediump vec4 tapB_11;
  mediump vec4 tapA_12;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_MainTex, xlv_TEXCOORD1_2.xy);
  tapA_12 = tmpvar_13;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_MainTex, xlv_TEXCOORD1_2.zw);
  tapB_11 = tmpvar_14;
  color_1 = (color_1 + ((tapA_12 + tapB_11) * vec4(0.232, 0.232, 0.232, 0.0)));
  gl_FragData[0] = color_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SGX_ON" }
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
uniform 	mediump vec4 _MainTex_TexelSize;
uniform 	mediump float _Parameter;
in highp vec4 in_POSITION0;
in mediump vec2 in_TEXCOORD0;
out vec4 undefined;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec4 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
highp vec4 t0;
mediump vec3 t16_1;
mediump vec2 t16_3;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    gl_Position = t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    vs_TEXCOORD0.zw = vec2(1.0, 1.0);
    t16_3.x = 1.0;
    t16_3.y = _Parameter;
    t16_1.yz = t16_3.xy * _MainTex_TexelSize.xy;
    t16_1.x = t16_1.y * _Parameter;
    vs_TEXCOORD1 = t16_1.xzxz * vec4(-3.0, -0.0, 3.0, 0.0) + in_TEXCOORD0.xyxy;
    vs_TEXCOORD2 = t16_1.xzxz * vec4(-2.0, 0.0, 2.0, -0.0) + in_TEXCOORD0.xyxy;
    vs_TEXCOORD3 = t16_1.xzxz * vec4(-1.0, 0.0, 1.0, -0.0) + in_TEXCOORD0.xyxy;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
// XXX ADRENOCONSTARRAYBUG
#define immediateConstBufferF (vec4[7] (\
	vec4(0.0205000006, 0, 0, 0), \
	vec4(0.0855000019, 0, 0, 0), \
	vec4(0.231999993, 0, 0, 0), \
	vec4(0.324000001, 0, 0, 1), \
	vec4(0.231999993, 0, 0, 0), \
	vec4(0.0855000019, 0, 0, 0), \
	vec4(0.0205000006, 0, 0, 0)))
uniform lowp sampler2D _MainTex;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec4 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
layout(location = 0) out mediump vec4 SV_Target0;
mediump vec4 t16_0;
lowp vec4 t10_0;
mediump vec4 t16_1;
highp int ti2;
mediump vec4 t16_3;
lowp vec4 t10_3;
lowp vec4 t10_4;
bool tb7;
mediump vec4 phase0_Input2[3];
void main()
{
phase0_Input2[0] = vs_TEXCOORD1;
phase0_Input2[1] = vs_TEXCOORD2;
phase0_Input2[2] = vs_TEXCOORD3;
    t10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_0 = t10_0 * vec4(0.324000001, 0.324000001, 0.324000001, 1.0);
    t16_1 = t16_0;
    ti2 = 0x0;
    while(true){
        tb7 = ti2>=0x3;
        if(tb7){break;}
        t10_3 = texture(_MainTex, phase0_Input2[ti2].xy);
        t10_4 = texture(_MainTex, phase0_Input2[ti2].zw);
        t16_3 = t10_3 + t10_4;
        t16_1 = t16_3 * immediateConstBufferF[ti2].xxxw + t16_1;
        ti2 = ti2 + 0x1;
    }
    SV_Target0 = t16_1;
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SGX_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SGX_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SGX_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SGX_ON" }
"!!GLES3"
}
}
 }
}
Fallback "Diffuse"
}
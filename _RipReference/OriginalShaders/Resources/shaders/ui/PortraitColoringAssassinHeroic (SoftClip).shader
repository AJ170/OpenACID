Shader "UI/PortraitColoringAssassinHeroic (SoftClip)" {
Properties {
 _MainTex ("Background Texture", 2D) = "white" { }
 _MaskTex ("Masking Texture", 2D) = "white" { }
 _Foreground ("Foreground Texture", 2D) = "white" { }
 _HeroicFrame ("Heroic Frame Texture", 2D) = "white" { }
 _HeroicStencil ("HF Stencil Texture", 2D) = "white" { }
 _ShowInsigniaItem ("Show Insignia Item", Float) = 0
 _InsigniaItem ("Insignia Item Texture", 2D) = "white" { }
 _Color0 ("Color1", Color) = (1,1,1,1)
 _Color1 ("Color2", Color) = (1,1,1,1)
}
SubShader { 
 Pass {
  Blend SrcAlpha OneMinusSrcAlpha
  GpuProgramID 51489
Program "vp" {
SubProgram "gles " {
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _MaskTex_ST;
uniform mediump vec4 _HeroicFrame_ST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _HeroicFrame_ST.xy) + _HeroicFrame_ST.zw);
  xlv_TEXCOORD2 = ((_glesVertex.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _Foreground;
uniform sampler2D _HeroicFrame;
uniform sampler2D _HeroicStencil;
uniform sampler2D _InsigniaItem;
uniform lowp vec4 _Color0;
uniform lowp vec4 _Color1;
uniform highp int _ShowInsigniaItem;
uniform mediump vec2 _ClipSharpness;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
void main ()
{
  mediump vec2 factor_1;
  lowp vec4 col_2;
  lowp vec4 foreground_3;
  lowp vec3 background_4;
  mediump vec2 uv_HeroicStencil_5;
  mediump vec2 uv_HeroicFrame_6;
  mediump vec2 uv_InsigniaItem_7;
  mediump vec2 uv_Foreground_8;
  mediump vec2 uv_MaskTex_9;
  mediump vec2 uv_background_10;
  uv_background_10 = xlv_TEXCOORD0;
  uv_MaskTex_9 = xlv_TEXCOORD0;
  uv_Foreground_8 = xlv_TEXCOORD0;
  uv_InsigniaItem_7 = xlv_TEXCOORD0;
  uv_HeroicFrame_6 = xlv_TEXCOORD1;
  uv_HeroicStencil_5 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, uv_background_10);
  lowp vec3 tmpvar_12;
  tmpvar_12 = texture2D (_MaskTex, uv_MaskTex_9).xyz;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_Foreground, uv_Foreground_8);
  foreground_3.w = tmpvar_13.w;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_InsigniaItem, uv_InsigniaItem_7);
  foreground_3.xyz = (tmpvar_13.xyz * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, tmpvar_12.xxx));
  foreground_3.xyz = (foreground_3.xyz * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, tmpvar_12.yyy));
  highp vec3 tmpvar_15;
  tmpvar_15 = mix (tmpvar_11.xyz, tmpvar_14.xyz, vec3((tmpvar_14.w * float(_ShowInsigniaItem))));
  background_4 = tmpvar_15;
  lowp vec4 tmpvar_16;
  tmpvar_16.xyz = mix (background_4, foreground_3.xyz, tmpvar_12.zzz);
  tmpvar_16.w = (_Color0.w * tmpvar_13.w);
  lowp vec4 tmpvar_17;
  tmpvar_17 = mix (texture2D (_HeroicFrame, uv_HeroicFrame_6), tmpvar_16, vec4((tmpvar_16.w * texture2D (_HeroicStencil, uv_HeroicStencil_5).x)));
  col_2.xyz = tmpvar_17.xyz;
  highp vec2 tmpvar_18;
  tmpvar_18 = ((vec2(1.0, 1.0) - abs(xlv_TEXCOORD2)) * _ClipSharpness);
  factor_1 = tmpvar_18;
  mediump float tmpvar_19;
  tmpvar_19 = clamp (min (factor_1.x, factor_1.y), 0.0, 1.0);
  col_2.w = (tmpvar_17.w * tmpvar_19);
  gl_FragData[0] = col_2;
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Foreground_ST;
uniform 	mediump vec4 _HeroicFrame_ST;
uniform 	mediump vec4 _HeroicStencil_ST;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	int _ShowInsigniaItem;
uniform 	mediump vec2 _ClipSharpness;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
out highp vec2 vs_TEXCOORD1;
out highp vec2 vs_TEXCOORD2;
highp vec4 t0;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD1.xy = in_TEXCOORD0.xy * _HeroicFrame_ST.xy + _HeroicFrame_ST.zw;
    vs_TEXCOORD2.xy = in_POSITION0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
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
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _Foreground_ST;
uniform 	mediump vec4 _HeroicFrame_ST;
uniform 	mediump vec4 _HeroicStencil_ST;
uniform 	lowp vec4 _Color0;
uniform 	lowp vec4 _Color1;
uniform 	int _ShowInsigniaItem;
uniform 	mediump vec2 _ClipSharpness;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _Foreground;
uniform lowp sampler2D _InsigniaItem;
uniform lowp sampler2D _HeroicFrame;
uniform lowp sampler2D _HeroicStencil;
in highp vec2 vs_TEXCOORD0;
in highp vec2 vs_TEXCOORD1;
in highp vec2 vs_TEXCOORD2;
layout(location = 0) out lowp vec4 SV_Target0;
lowp vec4 t10_0;
lowp vec4 t10_1;
lowp vec4 t10_2;
lowp vec3 t10_3;
lowp vec3 t10_4;
highp vec2 t5;
lowp float t10_5;
mediump float t16_6;
lowp vec3 t10_7;
void main()
{
    t10_0.x = float(_ShowInsigniaItem);
    t10_1 = texture(_InsigniaItem, vs_TEXCOORD0.xy);
    t10_0.x = t10_0.x * t10_1.w;
    t10_2.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    t10_7.xyz = t10_1.xyz + (-t10_2.xyz);
    t10_0.xyz = t10_0.xxx * t10_7.xyz + t10_2.xyz;
    t10_3.xyz = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t10_1.xyz = texture(_MaskTex, vs_TEXCOORD0.xy).xyz;
    t10_3.xyz = t10_1.xxx * t10_3.xyz + vec3(1.0, 1.0, 1.0);
    t10_2 = texture(_Foreground, vs_TEXCOORD0.xy);
    t10_3.xyz = t10_3.xyz * t10_2.xyz;
    t10_2.w = t10_2.w * _Color0.w;
    t10_4.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t10_4.xyz = t10_1.yyy * t10_4.xyz + vec3(1.0, 1.0, 1.0);
    t10_3.xyz = t10_3.xyz * t10_4.xyz + (-t10_0.xyz);
    t10_2.xyz = t10_1.zzz * t10_3.xyz + t10_0.xyz;
    t10_0 = texture(_HeroicFrame, vs_TEXCOORD1.xy);
    t10_1 = (-t10_0) + t10_2;
    t10_5 = texture(_HeroicStencil, vs_TEXCOORD1.xy).x;
    t10_3.x = t10_2.w * t10_5;
    t10_0 = t10_3.xxxx * t10_1 + t10_0;
    t5.xy = -abs(vs_TEXCOORD2.xy) + vec2(1.0, 1.0);
    t5.xy = t5.xy * _ClipSharpness.xyxx.xy;
    t16_6 = min(t5.y, t5.x);
    t16_6 = clamp(t16_6, 0.0, 1.0);
    t16_6 = t10_0.w * t16_6;
    SV_Target0.xyz = t10_0.xyz;
    SV_Target0.w = t16_6;
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
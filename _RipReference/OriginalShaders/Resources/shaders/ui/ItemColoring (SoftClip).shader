Shader "UI/ItemColoring (SoftClip)" {
Properties {
 _MainTex ("Background Texture", 2D) = "white" { }
 _MaskTex ("Masking Texture", 2D) = "white" { }
 _Foreground ("Foreground Texture", 2D) = "white" { }
 _Color0 ("Color1", Color) = (1,1,1,1)
 _Color1 ("Color2", Color) = (1,1,1,1)
 _Rarity ("Rarity color", Color) = (1,1,1,1)
 _Desaturation ("Desaturation Amount", Range(0,1)) = 0
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  GpuProgramID 4857
Program "vp" {
SubProgram "gles " {
Keywords { "BACKGROUND_ON" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0;
  xlv_TEXCOORD1 = ((_glesVertex.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _Foreground;
uniform highp vec4 _Color0;
uniform highp vec4 _Color1;
uniform highp vec4 _Rarity;
uniform highp vec2 _ClipSharpness;
uniform highp float _Desaturation;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 col_1;
  highp vec4 foreground_2;
  highp vec4 mask_3;
  highp vec4 background_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  background_4 = tmpvar_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MaskTex, xlv_TEXCOORD0.xy);
  mask_3 = tmpvar_6;
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_Foreground, xlv_TEXCOORD0.xy);
  foreground_2 = tmpvar_7;
  foreground_2.xyz = (foreground_2.xyz * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, mask_3.xxx));
  foreground_2.xyz = (foreground_2.xyz * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, mask_3.yyy));
  background_4.xyz = mix (mix (background_4.xyz, (background_4.xyz * _Rarity.xyz), background_4.www), _Rarity.xyz, mask_3.zzz);
  highp vec4 tmpvar_8;
  tmpvar_8.xyz = mix (background_4, foreground_2, foreground_2.wwww).xyz;
  tmpvar_8.w = _Color0.w;
  col_1 = tmpvar_8;
  highp vec2 tmpvar_9;
  tmpvar_9 = ((vec2(1.0, 1.0) - abs(xlv_TEXCOORD1)) * _ClipSharpness);
  highp float tmpvar_10;
  tmpvar_10 = clamp (min (tmpvar_9.x, tmpvar_9.y), 0.0, 1.0);
  col_1.w = (col_1.w * tmpvar_10);
  lowp vec4 _color_11;
  _color_11 = col_1;
  lowp float _amount_12;
  _amount_12 = _Desaturation;
  lowp vec4 tmpvar_13;
  tmpvar_13.xyz = mix (_color_11.xyz, vec3(dot (vec3(0.222, 0.707, 0.071), _color_11.xyz)), vec3(_amount_12));
  tmpvar_13.w = _color_11.w;
  gl_FragData[0] = tmpvar_13;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "BACKGROUND_ON" }
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
uniform 	vec4 _MainTex_ST;
uniform 	vec4 _Color0;
uniform 	vec4 _Color1;
uniform 	vec4 _Rarity;
uniform 	vec2 _ClipSharpness;
uniform 	float _Desaturation;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
out highp vec4 vs_TEXCOORD0;
out highp vec2 vs_TEXCOORD1;
highp vec4 t0;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0 = in_TEXCOORD0;
    vs_TEXCOORD1.xy = in_POSITION0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
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
uniform 	vec4 _MainTex_ST;
uniform 	vec4 _Color0;
uniform 	vec4 _Color1;
uniform 	vec4 _Rarity;
uniform 	vec2 _ClipSharpness;
uniform 	float _Desaturation;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _Foreground;
in highp vec4 vs_TEXCOORD0;
in highp vec2 vs_TEXCOORD1;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec3 t0;
lowp vec4 t10_0;
highp vec3 t1;
highp vec4 t2;
lowp vec3 t10_2;
lowp vec4 t10_3;
lowp vec3 t10_4;
void main()
{
    t10_0 = texture(_MainTex, vs_TEXCOORD0.xy);
    t1.xyz = t10_0.xyz * _Rarity.xyz + (-t10_0.xyz);
    t0.xyz = t10_0.www * t1.xyz + t10_0.xyz;
    t1.xyz = (-t0.xyz) + _Rarity.xyz;
    t10_2.xyz = texture(_MaskTex, vs_TEXCOORD0.xy).xyz;
    t0.xyz = t10_2.zzz * t1.xyz + t0.xyz;
    t1.xyz = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t1.xyz = t10_2.xxx * t1.xyz + vec3(1.0, 1.0, 1.0);
    t10_3 = texture(_Foreground, vs_TEXCOORD0.xy);
    t1.xyz = t1.xyz * t10_3.xyz;
    t2.xzw = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t2.xyz = t10_2.yyy * t2.xzw + vec3(1.0, 1.0, 1.0);
    t1.xyz = t1.xyz * t2.xyz + (-t0.xyz);
    t0.xyz = t10_3.www * t1.xyz + t0.xyz;
    t10_4.x = dot(vec3(0.222000003, 0.707000017, 0.0710000023), t0.xyz);
    t10_4.xyz = (-t0.xyz) + t10_4.xxx;
    SV_Target0.xyz = vec3(vec3(_Desaturation, _Desaturation, _Desaturation)) * t10_4.xyz + t0.xyz;
    t0.xy = -abs(vs_TEXCOORD1.xy) + vec2(1.0, 1.0);
    t0.xy = t0.xy * _ClipSharpness.xyxx.xy;
    t0.x = min(t0.y, t0.x);
    t0.x = clamp(t0.x, 0.0, 1.0);
    t0.x = t0.x * _Color0.w;
    SV_Target0.w = t0.x;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "BACKGROUND_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0;
  xlv_TEXCOORD1 = ((_glesVertex.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform sampler2D _MaskTex;
uniform sampler2D _Foreground;
uniform highp vec4 _Color0;
uniform highp vec4 _Color1;
uniform highp vec4 _Rarity;
uniform highp vec2 _ClipSharpness;
uniform highp float _Desaturation;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 col_1;
  highp vec4 foreground_2;
  highp vec4 mask_3;
  highp vec4 background_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  background_4.w = tmpvar_5.w;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MaskTex, xlv_TEXCOORD0.xy);
  mask_3 = tmpvar_6;
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_Foreground, xlv_TEXCOORD0.xy);
  foreground_2 = tmpvar_7;
  foreground_2.xyz = (foreground_2.xyz * mix (vec3(1.0, 1.0, 1.0), _Color0.xyz, mask_3.xxx));
  foreground_2.xyz = (foreground_2.xyz * mix (vec3(1.0, 1.0, 1.0), _Color1.xyz, mask_3.yyy));
  background_4.xyz = _Rarity.xyz;
  highp vec4 tmpvar_8;
  tmpvar_8.xyz = mix (background_4, foreground_2, foreground_2.wwww).xyz;
  tmpvar_8.w = (max (mask_3.z, foreground_2.w) * _Color0.w);
  col_1 = tmpvar_8;
  highp vec2 tmpvar_9;
  tmpvar_9 = ((vec2(1.0, 1.0) - abs(xlv_TEXCOORD1)) * _ClipSharpness);
  highp float tmpvar_10;
  tmpvar_10 = clamp (min (tmpvar_9.x, tmpvar_9.y), 0.0, 1.0);
  col_1.w = (col_1.w * tmpvar_10);
  lowp vec4 _color_11;
  _color_11 = col_1;
  lowp float _amount_12;
  _amount_12 = _Desaturation;
  lowp vec4 tmpvar_13;
  tmpvar_13.xyz = mix (_color_11.xyz, vec3(dot (vec3(0.222, 0.707, 0.071), _color_11.xyz)), vec3(_amount_12));
  tmpvar_13.w = _color_11.w;
  gl_FragData[0] = tmpvar_13;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "BACKGROUND_OFF" }
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
uniform 	vec4 _MainTex_ST;
uniform 	vec4 _Color0;
uniform 	vec4 _Color1;
uniform 	vec4 _Rarity;
uniform 	vec2 _ClipSharpness;
uniform 	float _Desaturation;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
out highp vec4 vs_TEXCOORD0;
out highp vec2 vs_TEXCOORD1;
highp vec4 t0;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD0 = in_TEXCOORD0;
    vs_TEXCOORD1.xy = in_POSITION0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
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
uniform 	vec4 _MainTex_ST;
uniform 	vec4 _Color0;
uniform 	vec4 _Color1;
uniform 	vec4 _Rarity;
uniform 	vec2 _ClipSharpness;
uniform 	float _Desaturation;
uniform lowp sampler2D _MaskTex;
uniform lowp sampler2D _Foreground;
in highp vec4 vs_TEXCOORD0;
in highp vec2 vs_TEXCOORD1;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec3 t0;
highp vec4 t1;
lowp vec3 t10_1;
highp vec3 t2;
lowp vec4 t10_2;
lowp vec3 t10_3;
highp float t12;
mediump float t16_12;
void main()
{
    t0.xyz = _Color1.xyz + vec3(-1.0, -1.0, -1.0);
    t10_1.xyz = texture(_MaskTex, vs_TEXCOORD0.xy).xyz;
    t0.xyz = t10_1.yyy * t0.xyz + vec3(1.0, 1.0, 1.0);
    t2.xyz = _Color0.xyz + vec3(-1.0, -1.0, -1.0);
    t1.xyw = t10_1.xxx * t2.xyz + vec3(1.0, 1.0, 1.0);
    t10_2 = texture(_Foreground, vs_TEXCOORD0.xy);
    t1.xyw = t1.xyw * t10_2.xyz;
    t0.xyz = t1.xyw * t0.xyz + (-_Rarity.xyz);
    t0.xyz = t10_2.www * t0.xyz + _Rarity.xyz;
    t16_12 = max(t10_1.z, t10_2.w);
    t12 = t16_12 * _Color0.w;
    t10_3.x = dot(vec3(0.222000003, 0.707000017, 0.0710000023), t0.xyz);
    t10_3.xyz = (-t0.xyz) + t10_3.xxx;
    SV_Target0.xyz = vec3(vec3(_Desaturation, _Desaturation, _Desaturation)) * t10_3.xyz + t0.xyz;
    t0.xy = -abs(vs_TEXCOORD1.xy) + vec2(1.0, 1.0);
    t0.xy = t0.xy * _ClipSharpness.xyxx.xy;
    t0.x = min(t0.y, t0.x);
    t0.x = clamp(t0.x, 0.0, 1.0);
    t0.x = t0.x * t12;
    SV_Target0.w = t0.x;
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "BACKGROUND_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "BACKGROUND_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "BACKGROUND_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "BACKGROUND_OFF" }
"!!GLES3"
}
}
 }
}
}
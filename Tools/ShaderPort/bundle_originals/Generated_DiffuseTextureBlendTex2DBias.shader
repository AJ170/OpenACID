Shader "Generated/DiffuseTextureBlendTex2DBias" {
Properties {
 _MainTex ("Albedo (RGBA)", 2D) = "white" { }
 _BlendTex ("Albedo (RGBA)", 2D) = "white" { }
 _PixelBlenderBias ("Pixel Blend Bias", Float) = 0
 _PixelBlenderScale ("Pixel Blend Scale", Float) = 0.5
 _Layer0UvScale ("Tiling MainTex", Float) = 1
 _Layer1UvScale ("Tiling BlendTex", Float) = 1
 _Layer0Color ("MainTex Color", Color) = (1,1,1,1)
 _Layer1Color ("BlendTex Color", Color) = (1,1,1,1)
}
SubShader { 
 LOD 199
 Tags { "QUEUE"="Geometry" "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry" "RenderType"="Opaque" }
  GpuProgramID 57476
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "LIGHTMAP_OFF" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord2;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform mediump float _Layer0UvScale;
uniform mediump float _Layer1UvScale;
uniform lowp vec4 _Layer0Color;
uniform lowp vec4 _Layer1Color;
uniform mediump vec4 acid_Fog;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD3;
void main ()
{
  mediump float fogpos_1;
  mediump float val_2;
  highp vec2 tmpvar_3;
  highp vec4 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  lowp vec4 tmpvar_7;
  lowp vec4 tmpvar_8;
  tmpvar_6.x = 0.0;
  tmpvar_5.xy = _glesColor.xy;
  tmpvar_5.zw = (_glesColor.xy + _glesColor.zz);
  tmpvar_5 = (tmpvar_5 * 0.9960938);
  tmpvar_5.zw = (tmpvar_5.zw + 0.00390625);
  tmpvar_3 = _glesMultiTexCoord0.xy;
  tmpvar_4.xy = tmpvar_3;
  tmpvar_4.zw = _glesMultiTexCoord2.xy;
  highp float tmpvar_9;
  tmpvar_9 = ((_glesColor.w * 2.0) - 1.0);
  val_2 = tmpvar_9;
  tmpvar_6.z = abs(val_2);
  mediump float tmpvar_10;
  tmpvar_10 = clamp ((val_2 * 512.0), 0.0, 1.0);
  tmpvar_6.w = ((tmpvar_10 * 2.0) - 1.0);
  mediump float tmpvar_11;
  tmpvar_11 = mix (_Layer1UvScale, _Layer0UvScale, tmpvar_10);
  tmpvar_3 = (_glesMultiTexCoord0.xy * tmpvar_11);
  mediump float tmpvar_12;
  tmpvar_12 = mix (_Layer0UvScale, _Layer1UvScale, tmpvar_10);
  tmpvar_4.xy = (tmpvar_4.xy * tmpvar_12);
  mediump vec4 tmpvar_13;
  tmpvar_13 = mix (_Layer1Color, _Layer0Color, vec4(tmpvar_10));
  tmpvar_7 = tmpvar_13;
  mediump vec4 tmpvar_14;
  tmpvar_14 = mix (_Layer0Color, _Layer1Color, vec4(tmpvar_10));
  tmpvar_8 = tmpvar_14;
  highp float tmpvar_15;
  highp vec3 x_16;
  x_16 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_15 = sqrt(dot (x_16, x_16));
  fogpos_1 = tmpvar_15;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD5 = tmpvar_5;
  xlv_TEXCOORD6 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = tmpvar_8;
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BlendTex;
uniform mediump float _PixelBlenderBias;
uniform mediump float _PixelBlenderScale;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 secondColor_1;
  lowp vec4 firstColor_2;
  highp vec4 biasuv_3;
  lowp vec4 c_4;
  c_4.w = 1.0;
  highp vec2 tmpvar_5;
  tmpvar_5 = fract(xlv_TEXCOORD0);
  biasuv_3.xy = tmpvar_5;
  biasuv_3.zw = (vec2(1.0, 1.0) - tmpvar_5);
  highp float tmpvar_6;
  tmpvar_6 = (min (dot (
    max (vec4(0.0, 0.0, 0.0, 0.0), ((biasuv_3 - vec4(0.93, 0.93, 0.93, 0.93)) * 14.2857))
  , vec4(1.0, 1.0, 1.0, 1.0)), 1.0) * -20.0);
  highp vec4 tmpvar_7;
  tmpvar_7.z = 0.0;
  tmpvar_7.xy = mix (xlv_TEXCOORD5.xy, xlv_TEXCOORD5.zw, tmpvar_5);
  tmpvar_7.w = tmpvar_6;
  highp vec4 tmpvar_8;
  tmpvar_8.z = 0.0;
  tmpvar_8.xy = mix ((xlv_TEXCOORD5.xy + xlv_TEXCOORD1.zw), (xlv_TEXCOORD5.zw + xlv_TEXCOORD1.zw), fract(xlv_TEXCOORD1).xy);
  tmpvar_8.w = tmpvar_6;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, tmpvar_7.xy, tmpvar_6);
  mediump vec4 tmpvar_10;
  tmpvar_10 = tmpvar_9;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_BlendTex, tmpvar_8.xy, tmpvar_6);
  mediump vec4 tmpvar_12;
  tmpvar_12 = tmpvar_11;
  mediump vec4 tmpvar_13;
  tmpvar_13 = (tmpvar_10 * xlv_TEXCOORD2);
  firstColor_2 = tmpvar_13;
  mediump vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * xlv_TEXCOORD3);
  secondColor_1 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (firstColor_2.xyz, secondColor_1.xyz, vec3(clamp ((xlv_TEXCOORD6.z + 
    (_PixelBlenderScale * (((xlv_TEXCOORD6.w * _PixelBlenderBias) + (
      ((secondColor_1.w - firstColor_2.w) * 0.5)
     + xlv_TEXCOORD6.z)) - 0.5))
  ), 0.0, 1.0)));
  c_4.xyz = tmpvar_15;
  c_4.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_4.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_4;
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
uniform 	mediump float _PixelBlenderBias;
uniform 	mediump float _PixelBlenderScale;
uniform 	mediump float _Layer0UvScale;
uniform 	mediump float _Layer1UvScale;
uniform 	lowp vec4 _Layer0Color;
uniform 	lowp vec4 _Layer1Color;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD2;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
out highp vec4 vs_TEXCOORD5;
out mediump vec4 vs_TEXCOORD6;
out lowp vec4 vs_TEXCOORD2;
out lowp vec4 vs_TEXCOORD3;
highp vec4 t0;
mediump vec4 t16_0;
mediump vec2 t16_1;
mediump float t16_3;
mediump float t16_5;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t16_1.xy = vec2((-float(_Layer1UvScale)) + float(_Layer0UvScale), (-float(_Layer0UvScale)) + float(_Layer1UvScale));
    t0.x = in_COLOR0.w * 2.0 + -1.0;
    t16_5 = t0.x * 512.0;
    t16_5 = clamp(t16_5, 0.0, 1.0);
    vs_TEXCOORD6.z = abs(t0.x);
    t16_1.xy = vec2(t16_5) * t16_1.xy + vec2(_Layer1UvScale, _Layer0UvScale);
    vs_TEXCOORD0.xy = t16_1.xx * in_TEXCOORD0.xy;
    vs_TEXCOORD1.xy = t16_1.yy * in_TEXCOORD0.xy;
    vs_TEXCOORD1.zw = in_TEXCOORD2.xy;
    t0.zw = in_COLOR0.zz + in_COLOR0.xy;
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1.x = (-t0.x) + acid_Fog.y;
    t16_3 = (-acid_Fog.x) + acid_Fog.y;
    t16_1.x = t16_1.x / t16_3;
    t16_1.x = clamp(t16_1.x, 0.0, 1.0);
    t16_1.x = (-t16_1.x) + 1.0;
    t16_3 = t16_1.x * acid_Fog.z;
    vs_TEXCOORD6.x = (-t16_1.x) * acid_Fog.z + 1.0;
    vs_TEXCOORD6.y = t16_3;
    vs_TEXCOORD6.w = t16_5 * 2.0 + -1.0;
    t16_0 = _Layer0Color + (-_Layer1Color);
    t16_0 = vec4(t16_5) * t16_0 + _Layer1Color;
    vs_TEXCOORD2 = t16_0;
    t16_0 = (-_Layer0Color) + _Layer1Color;
    t16_0 = vec4(t16_5) * t16_0 + _Layer0Color;
    vs_TEXCOORD3 = t16_0;
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
uniform 	mediump float _PixelBlenderBias;
uniform 	mediump float _PixelBlenderScale;
uniform 	mediump float _Layer0UvScale;
uniform 	mediump float _Layer1UvScale;
uniform 	lowp vec4 _Layer0Color;
uniform 	lowp vec4 _Layer1Color;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BlendTex;
in highp vec2 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
in highp vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD6;
in lowp vec4 vs_TEXCOORD2;
in lowp vec4 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec2 t0;
mediump vec4 t16_0;
lowp vec4 t10_0;
highp vec4 t1;
highp vec4 t2;
lowp vec4 t10_2;
lowp float t10_3;
mediump vec3 t16_4;
highp vec2 t10;
mediump float t16_19;
void main()
{
    t0.xy = fract(vs_TEXCOORD1.xy);
    t1 = vec4(vs_TEXCOORD1.z + vs_TEXCOORD5.x, vs_TEXCOORD1.w + vs_TEXCOORD5.y, vs_TEXCOORD1.z + vs_TEXCOORD5.z, vs_TEXCOORD1.w + vs_TEXCOORD5.w);
    t10.xy = vec2((-t1.x) + t1.z, (-t1.y) + t1.w);
    t0.xy = t0.xy * t10.xy + t1.xy;
    t1.xy = fract(vs_TEXCOORD0.xy);
    t1.zw = (-t1.xy);
    t2 = t1 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t2 = t2 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t2 = max(t2, vec4(0.0, 0.0, 0.0, 0.0));
    t10.x = dot(t2, vec4(1.0, 1.0, 1.0, 1.0));
    t10.x = min(t10.x, 1.0);
    t10.x = t10.x * -20.0;
    t10_2 = texture(_BlendTex, t0.xy, t10.x);
    t0.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t0.xy = t1.xy * t0.xy + vs_TEXCOORD5.xy;
    t10_0 = texture(_MainTex, t0.xy, t10.x);
    t16_0 = t10_0 * vs_TEXCOORD2;
    t10_3 = t10_2.w * vs_TEXCOORD3.w + (-t16_0.w);
    t16_4.xyz = t10_2.xyz * vs_TEXCOORD3.xyz + (-t16_0.xyz);
    t16_19 = t10_3 * 0.5 + vs_TEXCOORD6.z;
    t16_19 = vs_TEXCOORD6.w * _PixelBlenderBias + t16_19;
    t16_19 = t16_19 + -0.5;
    t16_19 = _PixelBlenderScale * t16_19 + vs_TEXCOORD6.z;
    t16_19 = clamp(t16_19, 0.0, 1.0);
    t16_4.xyz = vec3(t16_19) * t16_4.xyz + t16_0.xyz;
    t16_4.xyz = t16_4.xyz * vs_TEXCOORD6.xxx;
    t16_4.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_4.xyz;
    SV_Target0.xyz = t16_4.xyz;
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
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord2;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform mediump float _Layer0UvScale;
uniform mediump float _Layer1UvScale;
uniform lowp vec4 _Layer0Color;
uniform lowp vec4 _Layer1Color;
uniform mediump vec4 acid_Fog;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD3;
void main ()
{
  mediump float fogpos_1;
  mediump float val_2;
  highp vec2 tmpvar_3;
  highp vec4 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  lowp vec4 tmpvar_7;
  lowp vec4 tmpvar_8;
  tmpvar_6.x = 0.0;
  tmpvar_5.xy = _glesColor.xy;
  tmpvar_5.zw = (_glesColor.xy + _glesColor.zz);
  tmpvar_5 = (tmpvar_5 * 0.9960938);
  tmpvar_5.zw = (tmpvar_5.zw + 0.00390625);
  tmpvar_3 = _glesMultiTexCoord0.xy;
  tmpvar_4.xy = tmpvar_3;
  tmpvar_4.zw = _glesMultiTexCoord2.xy;
  highp float tmpvar_9;
  tmpvar_9 = ((_glesColor.w * 2.0) - 1.0);
  val_2 = tmpvar_9;
  tmpvar_6.z = abs(val_2);
  mediump float tmpvar_10;
  tmpvar_10 = clamp ((val_2 * 512.0), 0.0, 1.0);
  tmpvar_6.w = ((tmpvar_10 * 2.0) - 1.0);
  mediump float tmpvar_11;
  tmpvar_11 = mix (_Layer1UvScale, _Layer0UvScale, tmpvar_10);
  tmpvar_3 = (_glesMultiTexCoord0.xy * tmpvar_11);
  mediump float tmpvar_12;
  tmpvar_12 = mix (_Layer0UvScale, _Layer1UvScale, tmpvar_10);
  tmpvar_4.xy = (tmpvar_4.xy * tmpvar_12);
  mediump vec4 tmpvar_13;
  tmpvar_13 = mix (_Layer1Color, _Layer0Color, vec4(tmpvar_10));
  tmpvar_7 = tmpvar_13;
  mediump vec4 tmpvar_14;
  tmpvar_14 = mix (_Layer0Color, _Layer1Color, vec4(tmpvar_10));
  tmpvar_8 = tmpvar_14;
  highp float tmpvar_15;
  highp vec3 x_16;
  x_16 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_15 = sqrt(dot (x_16, x_16));
  fogpos_1 = tmpvar_15;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD5 = tmpvar_5;
  xlv_TEXCOORD6 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = tmpvar_8;
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BlendTex;
uniform mediump float _PixelBlenderBias;
uniform mediump float _PixelBlenderScale;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 secondColor_1;
  lowp vec4 firstColor_2;
  highp vec4 biasuv_3;
  lowp vec4 c_4;
  c_4.w = 1.0;
  highp vec2 tmpvar_5;
  tmpvar_5 = fract(xlv_TEXCOORD0);
  biasuv_3.xy = tmpvar_5;
  biasuv_3.zw = (vec2(1.0, 1.0) - tmpvar_5);
  highp float tmpvar_6;
  tmpvar_6 = (min (dot (
    max (vec4(0.0, 0.0, 0.0, 0.0), ((biasuv_3 - vec4(0.93, 0.93, 0.93, 0.93)) * 14.2857))
  , vec4(1.0, 1.0, 1.0, 1.0)), 1.0) * -20.0);
  highp vec4 tmpvar_7;
  tmpvar_7.z = 0.0;
  tmpvar_7.xy = mix (xlv_TEXCOORD5.xy, xlv_TEXCOORD5.zw, tmpvar_5);
  tmpvar_7.w = tmpvar_6;
  highp vec4 tmpvar_8;
  tmpvar_8.z = 0.0;
  tmpvar_8.xy = mix ((xlv_TEXCOORD5.xy + xlv_TEXCOORD1.zw), (xlv_TEXCOORD5.zw + xlv_TEXCOORD1.zw), fract(xlv_TEXCOORD1).xy);
  tmpvar_8.w = tmpvar_6;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, tmpvar_7.xy, tmpvar_6);
  mediump vec4 tmpvar_10;
  tmpvar_10 = tmpvar_9;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_BlendTex, tmpvar_8.xy, tmpvar_6);
  mediump vec4 tmpvar_12;
  tmpvar_12 = tmpvar_11;
  mediump vec4 tmpvar_13;
  tmpvar_13 = (tmpvar_10 * xlv_TEXCOORD2);
  firstColor_2 = tmpvar_13;
  mediump vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * xlv_TEXCOORD3);
  secondColor_1 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (firstColor_2.xyz, secondColor_1.xyz, vec3(clamp ((xlv_TEXCOORD6.z + 
    (_PixelBlenderScale * (((xlv_TEXCOORD6.w * _PixelBlenderBias) + (
      ((secondColor_1.w - firstColor_2.w) * 0.5)
     + xlv_TEXCOORD6.z)) - 0.5))
  ), 0.0, 1.0)));
  c_4.xyz = tmpvar_15;
  c_4.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_4.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_4;
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
uniform 	mediump float _PixelBlenderBias;
uniform 	mediump float _PixelBlenderScale;
uniform 	mediump float _Layer0UvScale;
uniform 	mediump float _Layer1UvScale;
uniform 	lowp vec4 _Layer0Color;
uniform 	lowp vec4 _Layer1Color;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD2;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
out highp vec4 vs_TEXCOORD5;
out mediump vec4 vs_TEXCOORD6;
out lowp vec4 vs_TEXCOORD2;
out lowp vec4 vs_TEXCOORD3;
highp vec4 t0;
mediump vec4 t16_0;
mediump vec2 t16_1;
mediump float t16_3;
mediump float t16_5;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t16_1.xy = vec2((-float(_Layer1UvScale)) + float(_Layer0UvScale), (-float(_Layer0UvScale)) + float(_Layer1UvScale));
    t0.x = in_COLOR0.w * 2.0 + -1.0;
    t16_5 = t0.x * 512.0;
    t16_5 = clamp(t16_5, 0.0, 1.0);
    vs_TEXCOORD6.z = abs(t0.x);
    t16_1.xy = vec2(t16_5) * t16_1.xy + vec2(_Layer1UvScale, _Layer0UvScale);
    vs_TEXCOORD0.xy = t16_1.xx * in_TEXCOORD0.xy;
    vs_TEXCOORD1.xy = t16_1.yy * in_TEXCOORD0.xy;
    vs_TEXCOORD1.zw = in_TEXCOORD2.xy;
    t0.zw = in_COLOR0.zz + in_COLOR0.xy;
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1.x = (-t0.x) + acid_Fog.y;
    t16_3 = (-acid_Fog.x) + acid_Fog.y;
    t16_1.x = t16_1.x / t16_3;
    t16_1.x = clamp(t16_1.x, 0.0, 1.0);
    t16_1.x = (-t16_1.x) + 1.0;
    t16_3 = t16_1.x * acid_Fog.z;
    vs_TEXCOORD6.x = (-t16_1.x) * acid_Fog.z + 1.0;
    vs_TEXCOORD6.y = t16_3;
    vs_TEXCOORD6.w = t16_5 * 2.0 + -1.0;
    t16_0 = _Layer0Color + (-_Layer1Color);
    t16_0 = vec4(t16_5) * t16_0 + _Layer1Color;
    vs_TEXCOORD2 = t16_0;
    t16_0 = (-_Layer0Color) + _Layer1Color;
    t16_0 = vec4(t16_5) * t16_0 + _Layer0Color;
    vs_TEXCOORD3 = t16_0;
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
uniform 	mediump float _PixelBlenderBias;
uniform 	mediump float _PixelBlenderScale;
uniform 	mediump float _Layer0UvScale;
uniform 	mediump float _Layer1UvScale;
uniform 	lowp vec4 _Layer0Color;
uniform 	lowp vec4 _Layer1Color;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BlendTex;
in highp vec2 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
in highp vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD6;
in lowp vec4 vs_TEXCOORD2;
in lowp vec4 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec2 t0;
mediump vec4 t16_0;
lowp vec4 t10_0;
highp vec4 t1;
highp vec4 t2;
lowp vec4 t10_2;
lowp float t10_3;
mediump vec3 t16_4;
highp vec2 t10;
mediump float t16_19;
void main()
{
    t0.xy = fract(vs_TEXCOORD1.xy);
    t1 = vec4(vs_TEXCOORD1.z + vs_TEXCOORD5.x, vs_TEXCOORD1.w + vs_TEXCOORD5.y, vs_TEXCOORD1.z + vs_TEXCOORD5.z, vs_TEXCOORD1.w + vs_TEXCOORD5.w);
    t10.xy = vec2((-t1.x) + t1.z, (-t1.y) + t1.w);
    t0.xy = t0.xy * t10.xy + t1.xy;
    t1.xy = fract(vs_TEXCOORD0.xy);
    t1.zw = (-t1.xy);
    t2 = t1 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t2 = t2 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t2 = max(t2, vec4(0.0, 0.0, 0.0, 0.0));
    t10.x = dot(t2, vec4(1.0, 1.0, 1.0, 1.0));
    t10.x = min(t10.x, 1.0);
    t10.x = t10.x * -20.0;
    t10_2 = texture(_BlendTex, t0.xy, t10.x);
    t0.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t0.xy = t1.xy * t0.xy + vs_TEXCOORD5.xy;
    t10_0 = texture(_MainTex, t0.xy, t10.x);
    t16_0 = t10_0 * vs_TEXCOORD2;
    t10_3 = t10_2.w * vs_TEXCOORD3.w + (-t16_0.w);
    t16_4.xyz = t10_2.xyz * vs_TEXCOORD3.xyz + (-t16_0.xyz);
    t16_19 = t10_3 * 0.5 + vs_TEXCOORD6.z;
    t16_19 = vs_TEXCOORD6.w * _PixelBlenderBias + t16_19;
    t16_19 = t16_19 + -0.5;
    t16_19 = _PixelBlenderScale * t16_19 + vs_TEXCOORD6.z;
    t16_19 = clamp(t16_19, 0.0, 1.0);
    t16_4.xyz = vec3(t16_19) * t16_4.xyz + t16_0.xyz;
    t16_4.xyz = t16_4.xyz * vs_TEXCOORD6.xxx;
    t16_4.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_4.xyz;
    SV_Target0.xyz = t16_4.xyz;
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
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord2;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump float _Layer0UvScale;
uniform mediump float _Layer1UvScale;
uniform lowp vec4 _Layer0Color;
uniform lowp vec4 _Layer1Color;
uniform mediump vec4 acid_Fog;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  mediump float fogpos_1;
  mediump float val_2;
  highp vec2 tmpvar_3;
  highp vec4 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  lowp vec4 tmpvar_7;
  lowp vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  tmpvar_6.x = 0.0;
  tmpvar_5.xy = _glesColor.xy;
  tmpvar_5.zw = (_glesColor.xy + _glesColor.zz);
  tmpvar_5 = (tmpvar_5 * 0.9960938);
  tmpvar_5.zw = (tmpvar_5.zw + 0.00390625);
  tmpvar_3 = _glesMultiTexCoord0.xy;
  tmpvar_4.xy = tmpvar_3;
  tmpvar_4.zw = _glesMultiTexCoord2.xy;
  highp float tmpvar_10;
  tmpvar_10 = ((_glesColor.w * 2.0) - 1.0);
  val_2 = tmpvar_10;
  tmpvar_6.z = abs(val_2);
  mediump float tmpvar_11;
  tmpvar_11 = clamp ((val_2 * 512.0), 0.0, 1.0);
  tmpvar_6.w = ((tmpvar_11 * 2.0) - 1.0);
  mediump float tmpvar_12;
  tmpvar_12 = mix (_Layer1UvScale, _Layer0UvScale, tmpvar_11);
  tmpvar_3 = (_glesMultiTexCoord0.xy * tmpvar_12);
  mediump float tmpvar_13;
  tmpvar_13 = mix (_Layer0UvScale, _Layer1UvScale, tmpvar_11);
  tmpvar_4.xy = (tmpvar_4.xy * tmpvar_13);
  mediump vec4 tmpvar_14;
  tmpvar_14 = mix (_Layer1Color, _Layer0Color, vec4(tmpvar_11));
  tmpvar_7 = tmpvar_14;
  mediump vec4 tmpvar_15;
  tmpvar_15 = mix (_Layer0Color, _Layer1Color, vec4(tmpvar_11));
  tmpvar_8 = tmpvar_15;
  highp float tmpvar_16;
  highp vec3 x_17;
  x_17 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_16 = sqrt(dot (x_17, x_17));
  fogpos_1 = tmpvar_16;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  tmpvar_9 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD5 = tmpvar_5;
  xlv_TEXCOORD6 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = tmpvar_8;
  xlv_TEXCOORD7 = tmpvar_9;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BlendTex;
uniform mediump float _PixelBlenderBias;
uniform mediump float _PixelBlenderScale;
uniform lowp vec4 _EnvAmbientColor;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  lowp vec4 secondColor_2;
  lowp vec4 firstColor_3;
  highp vec4 biasuv_4;
  lowp vec4 c_5;
  c_5.w = 1.0;
  highp vec2 tmpvar_6;
  tmpvar_6 = fract(xlv_TEXCOORD0);
  biasuv_4.xy = tmpvar_6;
  biasuv_4.zw = (vec2(1.0, 1.0) - tmpvar_6);
  highp float tmpvar_7;
  tmpvar_7 = (min (dot (
    max (vec4(0.0, 0.0, 0.0, 0.0), ((biasuv_4 - vec4(0.93, 0.93, 0.93, 0.93)) * 14.2857))
  , vec4(1.0, 1.0, 1.0, 1.0)), 1.0) * -20.0);
  highp vec4 tmpvar_8;
  tmpvar_8.z = 0.0;
  tmpvar_8.xy = mix (xlv_TEXCOORD5.xy, xlv_TEXCOORD5.zw, tmpvar_6);
  tmpvar_8.w = tmpvar_7;
  highp vec4 tmpvar_9;
  tmpvar_9.z = 0.0;
  tmpvar_9.xy = mix ((xlv_TEXCOORD5.xy + xlv_TEXCOORD1.zw), (xlv_TEXCOORD5.zw + xlv_TEXCOORD1.zw), fract(xlv_TEXCOORD1).xy);
  tmpvar_9.w = tmpvar_7;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, tmpvar_8.xy, tmpvar_7);
  mediump vec4 tmpvar_11;
  tmpvar_11 = tmpvar_10;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_BlendTex, tmpvar_9.xy, tmpvar_7);
  mediump vec4 tmpvar_13;
  tmpvar_13 = tmpvar_12;
  mediump vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_11 * xlv_TEXCOORD2);
  firstColor_3 = tmpvar_14;
  mediump vec4 tmpvar_15;
  tmpvar_15 = (tmpvar_13 * xlv_TEXCOORD3);
  secondColor_2 = tmpvar_15;
  mediump vec3 tmpvar_16;
  tmpvar_16 = mix (firstColor_3.xyz, secondColor_2.xyz, vec3(clamp ((xlv_TEXCOORD6.z + 
    (_PixelBlenderScale * (((xlv_TEXCOORD6.w * _PixelBlenderBias) + (
      ((secondColor_2.w - firstColor_3.w) * 0.5)
     + xlv_TEXCOORD6.z)) - 0.5))
  ), 0.0, 1.0)));
  c_5.xyz = tmpvar_16;
  lowp float tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD7).x > 
    (xlv_TEXCOORD7.z / xlv_TEXCOORD7.w)
  )), _LightShadowData.x);
  tmpvar_17 = tmpvar_18;
  tmpvar_1 = ((_EnvAmbientColor.xyz * (1.0 - tmpvar_17)) + tmpvar_17);
  c_5.xyz = (c_5.xyz * tmpvar_1);
  c_5.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_5.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_5;
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
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord2;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump float _Layer0UvScale;
uniform mediump float _Layer1UvScale;
uniform lowp vec4 _Layer0Color;
uniform lowp vec4 _Layer1Color;
uniform mediump vec4 acid_Fog;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  mediump float fogpos_1;
  mediump float val_2;
  highp vec2 tmpvar_3;
  highp vec4 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  lowp vec4 tmpvar_7;
  lowp vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  tmpvar_6.x = 0.0;
  tmpvar_5.xy = _glesColor.xy;
  tmpvar_5.zw = (_glesColor.xy + _glesColor.zz);
  tmpvar_5 = (tmpvar_5 * 0.9960938);
  tmpvar_5.zw = (tmpvar_5.zw + 0.00390625);
  tmpvar_3 = _glesMultiTexCoord0.xy;
  tmpvar_4.xy = tmpvar_3;
  tmpvar_4.zw = _glesMultiTexCoord2.xy;
  highp float tmpvar_10;
  tmpvar_10 = ((_glesColor.w * 2.0) - 1.0);
  val_2 = tmpvar_10;
  tmpvar_6.z = abs(val_2);
  mediump float tmpvar_11;
  tmpvar_11 = clamp ((val_2 * 512.0), 0.0, 1.0);
  tmpvar_6.w = ((tmpvar_11 * 2.0) - 1.0);
  mediump float tmpvar_12;
  tmpvar_12 = mix (_Layer1UvScale, _Layer0UvScale, tmpvar_11);
  tmpvar_3 = (_glesMultiTexCoord0.xy * tmpvar_12);
  mediump float tmpvar_13;
  tmpvar_13 = mix (_Layer0UvScale, _Layer1UvScale, tmpvar_11);
  tmpvar_4.xy = (tmpvar_4.xy * tmpvar_13);
  mediump vec4 tmpvar_14;
  tmpvar_14 = mix (_Layer1Color, _Layer0Color, vec4(tmpvar_11));
  tmpvar_7 = tmpvar_14;
  mediump vec4 tmpvar_15;
  tmpvar_15 = mix (_Layer0Color, _Layer1Color, vec4(tmpvar_11));
  tmpvar_8 = tmpvar_15;
  highp float tmpvar_16;
  highp vec3 x_17;
  x_17 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_16 = sqrt(dot (x_17, x_17));
  fogpos_1 = tmpvar_16;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  tmpvar_9 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD5 = tmpvar_5;
  xlv_TEXCOORD6 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = tmpvar_8;
  xlv_TEXCOORD7 = tmpvar_9;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BlendTex;
uniform mediump float _PixelBlenderBias;
uniform mediump float _PixelBlenderScale;
uniform lowp vec4 _EnvAmbientColor;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD7;
void main ()
{
  lowp vec4 secondColor_2;
  lowp vec4 firstColor_3;
  highp vec4 biasuv_4;
  lowp vec4 c_5;
  c_5.w = 1.0;
  highp vec2 tmpvar_6;
  tmpvar_6 = fract(xlv_TEXCOORD0);
  biasuv_4.xy = tmpvar_6;
  biasuv_4.zw = (vec2(1.0, 1.0) - tmpvar_6);
  highp float tmpvar_7;
  tmpvar_7 = (min (dot (
    max (vec4(0.0, 0.0, 0.0, 0.0), ((biasuv_4 - vec4(0.93, 0.93, 0.93, 0.93)) * 14.2857))
  , vec4(1.0, 1.0, 1.0, 1.0)), 1.0) * -20.0);
  highp vec4 tmpvar_8;
  tmpvar_8.z = 0.0;
  tmpvar_8.xy = mix (xlv_TEXCOORD5.xy, xlv_TEXCOORD5.zw, tmpvar_6);
  tmpvar_8.w = tmpvar_7;
  highp vec4 tmpvar_9;
  tmpvar_9.z = 0.0;
  tmpvar_9.xy = mix ((xlv_TEXCOORD5.xy + xlv_TEXCOORD1.zw), (xlv_TEXCOORD5.zw + xlv_TEXCOORD1.zw), fract(xlv_TEXCOORD1).xy);
  tmpvar_9.w = tmpvar_7;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, tmpvar_8.xy, tmpvar_7);
  mediump vec4 tmpvar_11;
  tmpvar_11 = tmpvar_10;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_BlendTex, tmpvar_9.xy, tmpvar_7);
  mediump vec4 tmpvar_13;
  tmpvar_13 = tmpvar_12;
  mediump vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_11 * xlv_TEXCOORD2);
  firstColor_3 = tmpvar_14;
  mediump vec4 tmpvar_15;
  tmpvar_15 = (tmpvar_13 * xlv_TEXCOORD3);
  secondColor_2 = tmpvar_15;
  mediump vec3 tmpvar_16;
  tmpvar_16 = mix (firstColor_3.xyz, secondColor_2.xyz, vec3(clamp ((xlv_TEXCOORD6.z + 
    (_PixelBlenderScale * (((xlv_TEXCOORD6.w * _PixelBlenderBias) + (
      ((secondColor_2.w - firstColor_3.w) * 0.5)
     + xlv_TEXCOORD6.z)) - 0.5))
  ), 0.0, 1.0)));
  c_5.xyz = tmpvar_16;
  lowp float shadow_17;
  shadow_17 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD7.xyz) * (1.0 - _LightShadowData.x)));
  tmpvar_1 = ((_EnvAmbientColor.xyz * (1.0 - shadow_17)) + shadow_17);
  c_5.xyz = (c_5.xyz * tmpvar_1);
  c_5.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_5.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_5;
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
uniform 	mediump float _PixelBlenderBias;
uniform 	mediump float _PixelBlenderScale;
uniform 	mediump float _Layer0UvScale;
uniform 	mediump float _Layer1UvScale;
uniform 	lowp vec4 _Layer0Color;
uniform 	lowp vec4 _Layer1Color;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD2;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
out highp vec4 vs_TEXCOORD5;
out mediump vec4 vs_TEXCOORD6;
out lowp vec4 vs_TEXCOORD2;
out lowp vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD7;
highp vec4 t0;
mediump vec4 t16_0;
highp vec4 t1;
mediump vec2 t16_1;
mediump float t16_3;
mediump float t16_5;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t16_1.xy = vec2((-float(_Layer1UvScale)) + float(_Layer0UvScale), (-float(_Layer0UvScale)) + float(_Layer1UvScale));
    t0.x = in_COLOR0.w * 2.0 + -1.0;
    t16_5 = t0.x * 512.0;
    t16_5 = clamp(t16_5, 0.0, 1.0);
    vs_TEXCOORD6.z = abs(t0.x);
    t16_1.xy = vec2(t16_5) * t16_1.xy + vec2(_Layer1UvScale, _Layer0UvScale);
    vs_TEXCOORD0.xy = t16_1.xx * in_TEXCOORD0.xy;
    vs_TEXCOORD1.xy = t16_1.yy * in_TEXCOORD0.xy;
    vs_TEXCOORD1.zw = in_TEXCOORD2.xy;
    t0.zw = in_COLOR0.zz + in_COLOR0.xy;
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1.x = (-t0.x) + acid_Fog.y;
    t16_3 = (-acid_Fog.x) + acid_Fog.y;
    t16_1.x = t16_1.x / t16_3;
    t16_1.x = clamp(t16_1.x, 0.0, 1.0);
    t16_1.x = (-t16_1.x) + 1.0;
    t16_3 = t16_1.x * acid_Fog.z;
    vs_TEXCOORD6.x = (-t16_1.x) * acid_Fog.z + 1.0;
    vs_TEXCOORD6.y = t16_3;
    vs_TEXCOORD6.w = t16_5 * 2.0 + -1.0;
    t16_0 = _Layer0Color + (-_Layer1Color);
    t16_0 = vec4(t16_5) * t16_0 + _Layer1Color;
    vs_TEXCOORD2 = t16_0;
    t16_0 = (-_Layer0Color) + _Layer1Color;
    t16_0 = vec4(t16_5) * t16_0 + _Layer0Color;
    vs_TEXCOORD3 = t16_0;
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
uniform 	mediump float _PixelBlenderBias;
uniform 	mediump float _PixelBlenderScale;
uniform 	mediump float _Layer0UvScale;
uniform 	mediump float _Layer1UvScale;
uniform 	lowp vec4 _Layer0Color;
uniform 	lowp vec4 _Layer1Color;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BlendTex;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
in highp vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD6;
in lowp vec4 vs_TEXCOORD2;
in lowp vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD7;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec2 t0;
mediump vec4 t16_0;
lowp vec4 t10_0;
highp vec4 t1;
mediump vec3 t16_1;
highp vec4 t2;
lowp vec4 t10_2;
lowp float t10_3;
mediump vec3 t16_4;
mediump float t16_5;
highp vec2 t12;
mediump float t16_22;
void main()
{
    t0.xy = fract(vs_TEXCOORD1.xy);
    t1 = vec4(vs_TEXCOORD1.z + vs_TEXCOORD5.x, vs_TEXCOORD1.w + vs_TEXCOORD5.y, vs_TEXCOORD1.z + vs_TEXCOORD5.z, vs_TEXCOORD1.w + vs_TEXCOORD5.w);
    t12.xy = vec2((-t1.x) + t1.z, (-t1.y) + t1.w);
    t0.xy = t0.xy * t12.xy + t1.xy;
    t1.xy = fract(vs_TEXCOORD0.xy);
    t1.zw = (-t1.xy);
    t2 = t1 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t2 = t2 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t2 = max(t2, vec4(0.0, 0.0, 0.0, 0.0));
    t12.x = dot(t2, vec4(1.0, 1.0, 1.0, 1.0));
    t12.x = min(t12.x, 1.0);
    t12.x = t12.x * -20.0;
    t10_2 = texture(_BlendTex, t0.xy, t12.x);
    t0.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t0.xy = t1.xy * t0.xy + vs_TEXCOORD5.xy;
    t10_0 = texture(_MainTex, t0.xy, t12.x);
    t16_0 = t10_0 * vs_TEXCOORD2;
    t10_3 = t10_2.w * vs_TEXCOORD3.w + (-t16_0.w);
    t16_4.xyz = t10_2.xyz * vs_TEXCOORD3.xyz + (-t16_0.xyz);
    t16_22 = t10_3 * 0.5 + vs_TEXCOORD6.z;
    t16_22 = vs_TEXCOORD6.w * _PixelBlenderBias + t16_22;
    t16_22 = t16_22 + -0.5;
    t16_22 = _PixelBlenderScale * t16_22 + vs_TEXCOORD6.z;
    t16_22 = clamp(t16_22, 0.0, 1.0);
    t16_4.xyz = vec3(t16_22) * t16_4.xyz + t16_0.xyz;
    vec3 txVec12 = vec3(vs_TEXCOORD7.xy,vs_TEXCOORD7.z);
    t16_22 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec12, 0.0);
    t16_5 = (-_LightShadowData.x) + 1.0;
    t16_22 = t16_22 * t16_5 + _LightShadowData.x;
    t16_1.x = (-t16_22) + 1.0;
    t16_1.xyz = _EnvAmbientColor.xyz * t16_1.xxx + vec3(t16_22);
    t16_4.xyz = t16_1.xyz * t16_4.xyz;
    t16_4.xyz = t16_4.xyz * vs_TEXCOORD6.xxx;
    t16_4.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_4.xyz;
    SV_Target0.xyz = t16_4.xyz;
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
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
attribute vec4 _glesMultiTexCoord2;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp vec4 unity_LightmapST;
uniform mediump float _Layer0UvScale;
uniform mediump float _Layer1UvScale;
uniform lowp vec4 _Layer0Color;
uniform lowp vec4 _Layer1Color;
uniform mediump vec4 acid_Fog;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD3;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  mediump float fogpos_1;
  mediump float val_2;
  highp vec2 tmpvar_3;
  highp vec4 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  lowp vec4 tmpvar_7;
  lowp vec4 tmpvar_8;
  tmpvar_6.x = 0.0;
  tmpvar_5.xy = _glesColor.xy;
  tmpvar_5.zw = (_glesColor.xy + _glesColor.zz);
  tmpvar_5 = (tmpvar_5 * 0.9960938);
  tmpvar_5.zw = (tmpvar_5.zw + 0.00390625);
  tmpvar_3 = _glesMultiTexCoord0.xy;
  tmpvar_4.xy = tmpvar_3;
  tmpvar_4.zw = _glesMultiTexCoord2.xy;
  highp float tmpvar_9;
  tmpvar_9 = ((_glesColor.w * 2.0) - 1.0);
  val_2 = tmpvar_9;
  tmpvar_6.z = abs(val_2);
  mediump float tmpvar_10;
  tmpvar_10 = clamp ((val_2 * 512.0), 0.0, 1.0);
  tmpvar_6.w = ((tmpvar_10 * 2.0) - 1.0);
  mediump float tmpvar_11;
  tmpvar_11 = mix (_Layer1UvScale, _Layer0UvScale, tmpvar_10);
  tmpvar_3 = (_glesMultiTexCoord0.xy * tmpvar_11);
  mediump float tmpvar_12;
  tmpvar_12 = mix (_Layer0UvScale, _Layer1UvScale, tmpvar_10);
  tmpvar_4.xy = (tmpvar_4.xy * tmpvar_12);
  mediump vec4 tmpvar_13;
  tmpvar_13 = mix (_Layer1Color, _Layer0Color, vec4(tmpvar_10));
  tmpvar_7 = tmpvar_13;
  mediump vec4 tmpvar_14;
  tmpvar_14 = mix (_Layer0Color, _Layer1Color, vec4(tmpvar_10));
  tmpvar_8 = tmpvar_14;
  highp float tmpvar_15;
  highp vec3 x_16;
  x_16 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_15 = sqrt(dot (x_16, x_16));
  fogpos_1 = tmpvar_15;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD5 = tmpvar_5;
  xlv_TEXCOORD6 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = tmpvar_8;
  xlv_TEXCOORD8 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
uniform sampler2D _BlendTex;
uniform mediump float _PixelBlenderBias;
uniform mediump float _PixelBlenderScale;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD3;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 secondColor_1;
  lowp vec4 firstColor_2;
  highp vec4 biasuv_3;
  lowp vec4 c_4;
  c_4.w = 1.0;
  highp vec2 tmpvar_5;
  tmpvar_5 = fract(xlv_TEXCOORD0);
  biasuv_3.xy = tmpvar_5;
  biasuv_3.zw = (vec2(1.0, 1.0) - tmpvar_5);
  highp float tmpvar_6;
  tmpvar_6 = (min (dot (
    max (vec4(0.0, 0.0, 0.0, 0.0), ((biasuv_3 - vec4(0.93, 0.93, 0.93, 0.93)) * 14.2857))
  , vec4(1.0, 1.0, 1.0, 1.0)), 1.0) * -20.0);
  highp vec4 tmpvar_7;
  tmpvar_7.z = 0.0;
  tmpvar_7.xy = mix (xlv_TEXCOORD5.xy, xlv_TEXCOORD5.zw, tmpvar_5);
  tmpvar_7.w = tmpvar_6;
  highp vec4 tmpvar_8;
  tmpvar_8.z = 0.0;
  tmpvar_8.xy = mix ((xlv_TEXCOORD5.xy + xlv_TEXCOORD1.zw), (xlv_TEXCOORD5.zw + xlv_TEXCOORD1.zw), fract(xlv_TEXCOORD1).xy);
  tmpvar_8.w = tmpvar_6;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, tmpvar_7.xy, tmpvar_6);
  mediump vec4 tmpvar_10;
  tmpvar_10 = tmpvar_9;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_BlendTex, tmpvar_8.xy, tmpvar_6);
  mediump vec4 tmpvar_12;
  tmpvar_12 = tmpvar_11;
  mediump vec4 tmpvar_13;
  tmpvar_13 = (tmpvar_10 * xlv_TEXCOORD2);
  firstColor_2 = tmpvar_13;
  mediump vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * xlv_TEXCOORD3);
  secondColor_1 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (firstColor_2.xyz, secondColor_1.xyz, vec3(clamp ((xlv_TEXCOORD6.z + 
    (_PixelBlenderScale * (((xlv_TEXCOORD6.w * _PixelBlenderBias) + (
      ((secondColor_1.w - firstColor_2.w) * 0.5)
     + xlv_TEXCOORD6.z)) - 0.5))
  ), 0.0, 1.0)));
  c_4.xyz = tmpvar_15;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_17;
  tmpvar_17 = (2.0 * tmpvar_16.xyz);
  c_4.xyz = (c_4.xyz * tmpvar_17);
  c_4.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_4.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_4;
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
uniform 	mediump float _PixelBlenderBias;
uniform 	mediump float _PixelBlenderScale;
uniform 	mediump float _Layer0UvScale;
uniform 	mediump float _Layer1UvScale;
uniform 	lowp vec4 _Layer0Color;
uniform 	lowp vec4 _Layer1Color;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD1;
in highp vec4 in_TEXCOORD2;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out highp vec2 vs_TEXCOORD8;
out highp vec4 vs_TEXCOORD1;
out highp vec4 vs_TEXCOORD5;
out mediump vec4 vs_TEXCOORD6;
out lowp vec4 vs_TEXCOORD2;
out lowp vec4 vs_TEXCOORD3;
highp vec4 t0;
mediump vec4 t16_0;
mediump vec2 t16_1;
mediump float t16_3;
mediump float t16_5;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD8.xy = in_TEXCOORD1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    t16_1.xy = vec2((-float(_Layer1UvScale)) + float(_Layer0UvScale), (-float(_Layer0UvScale)) + float(_Layer1UvScale));
    t0.x = in_COLOR0.w * 2.0 + -1.0;
    t16_5 = t0.x * 512.0;
    t16_5 = clamp(t16_5, 0.0, 1.0);
    vs_TEXCOORD6.z = abs(t0.x);
    t16_1.xy = vec2(t16_5) * t16_1.xy + vec2(_Layer1UvScale, _Layer0UvScale);
    vs_TEXCOORD0.xy = t16_1.xx * in_TEXCOORD0.xy;
    vs_TEXCOORD1.xy = t16_1.yy * in_TEXCOORD0.xy;
    vs_TEXCOORD1.zw = in_TEXCOORD2.xy;
    t0.zw = in_COLOR0.zz + in_COLOR0.xy;
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1.x = (-t0.x) + acid_Fog.y;
    t16_3 = (-acid_Fog.x) + acid_Fog.y;
    t16_1.x = t16_1.x / t16_3;
    t16_1.x = clamp(t16_1.x, 0.0, 1.0);
    t16_1.x = (-t16_1.x) + 1.0;
    t16_3 = t16_1.x * acid_Fog.z;
    vs_TEXCOORD6.x = (-t16_1.x) * acid_Fog.z + 1.0;
    vs_TEXCOORD6.y = t16_3;
    vs_TEXCOORD6.w = t16_5 * 2.0 + -1.0;
    t16_0 = _Layer0Color + (-_Layer1Color);
    t16_0 = vec4(t16_5) * t16_0 + _Layer1Color;
    vs_TEXCOORD2 = t16_0;
    t16_0 = (-_Layer0Color) + _Layer1Color;
    t16_0 = vec4(t16_5) * t16_0 + _Layer0Color;
    vs_TEXCOORD3 = t16_0;
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
uniform 	mediump float _PixelBlenderBias;
uniform 	mediump float _PixelBlenderScale;
uniform 	mediump float _Layer0UvScale;
uniform 	mediump float _Layer1UvScale;
uniform 	lowp vec4 _Layer0Color;
uniform 	lowp vec4 _Layer1Color;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BlendTex;
uniform lowp sampler2D unity_Lightmap;
in highp vec2 vs_TEXCOORD0;
in highp vec2 vs_TEXCOORD8;
in highp vec4 vs_TEXCOORD1;
in highp vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD6;
in lowp vec4 vs_TEXCOORD2;
in lowp vec4 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec2 t0;
mediump vec4 t16_0;
lowp vec4 t10_0;
highp vec4 t1;
lowp vec3 t10_1;
highp vec4 t2;
lowp vec4 t10_2;
lowp vec3 t10_3;
mediump vec3 t16_4;
highp vec2 t10;
mediump float t16_19;
void main()
{
    t0.xy = fract(vs_TEXCOORD1.xy);
    t1 = vec4(vs_TEXCOORD1.z + vs_TEXCOORD5.x, vs_TEXCOORD1.w + vs_TEXCOORD5.y, vs_TEXCOORD1.z + vs_TEXCOORD5.z, vs_TEXCOORD1.w + vs_TEXCOORD5.w);
    t10.xy = vec2((-t1.x) + t1.z, (-t1.y) + t1.w);
    t0.xy = t0.xy * t10.xy + t1.xy;
    t1.xy = fract(vs_TEXCOORD0.xy);
    t1.zw = (-t1.xy);
    t2 = t1 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t2 = t2 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t2 = max(t2, vec4(0.0, 0.0, 0.0, 0.0));
    t10.x = dot(t2, vec4(1.0, 1.0, 1.0, 1.0));
    t10.x = min(t10.x, 1.0);
    t10.x = t10.x * -20.0;
    t10_2 = texture(_BlendTex, t0.xy, t10.x);
    t0.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t0.xy = t1.xy * t0.xy + vs_TEXCOORD5.xy;
    t10_0 = texture(_MainTex, t0.xy, t10.x);
    t16_0 = t10_0 * vs_TEXCOORD2;
    t10_3.x = t10_2.w * vs_TEXCOORD3.w + (-t16_0.w);
    t16_4.xyz = t10_2.xyz * vs_TEXCOORD3.xyz + (-t16_0.xyz);
    t16_19 = t10_3.x * 0.5 + vs_TEXCOORD6.z;
    t16_19 = vs_TEXCOORD6.w * _PixelBlenderBias + t16_19;
    t16_19 = t16_19 + -0.5;
    t16_19 = _PixelBlenderScale * t16_19 + vs_TEXCOORD6.z;
    t16_19 = clamp(t16_19, 0.0, 1.0);
    t16_4.xyz = vec3(t16_19) * t16_4.xyz + t16_0.xyz;
    t10_1.xyz = texture(unity_Lightmap, vs_TEXCOORD8.xy).xyz;
    t10_3.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0);
    t16_4.xyz = t10_3.xyz * t16_4.xyz;
    t16_4.xyz = t16_4.xyz * vs_TEXCOORD6.xxx;
    t16_4.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_4.xyz;
    SV_Target0.xyz = t16_4.xyz;
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
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
attribute vec4 _glesMultiTexCoord2;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp vec4 unity_LightmapST;
uniform mediump float _Layer0UvScale;
uniform mediump float _Layer1UvScale;
uniform lowp vec4 _Layer0Color;
uniform lowp vec4 _Layer1Color;
uniform mediump vec4 acid_Fog;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD3;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  mediump float fogpos_1;
  mediump float val_2;
  highp vec2 tmpvar_3;
  highp vec4 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  lowp vec4 tmpvar_7;
  lowp vec4 tmpvar_8;
  tmpvar_6.x = 0.0;
  tmpvar_5.xy = _glesColor.xy;
  tmpvar_5.zw = (_glesColor.xy + _glesColor.zz);
  tmpvar_5 = (tmpvar_5 * 0.9960938);
  tmpvar_5.zw = (tmpvar_5.zw + 0.00390625);
  tmpvar_3 = _glesMultiTexCoord0.xy;
  tmpvar_4.xy = tmpvar_3;
  tmpvar_4.zw = _glesMultiTexCoord2.xy;
  highp float tmpvar_9;
  tmpvar_9 = ((_glesColor.w * 2.0) - 1.0);
  val_2 = tmpvar_9;
  tmpvar_6.z = abs(val_2);
  mediump float tmpvar_10;
  tmpvar_10 = clamp ((val_2 * 512.0), 0.0, 1.0);
  tmpvar_6.w = ((tmpvar_10 * 2.0) - 1.0);
  mediump float tmpvar_11;
  tmpvar_11 = mix (_Layer1UvScale, _Layer0UvScale, tmpvar_10);
  tmpvar_3 = (_glesMultiTexCoord0.xy * tmpvar_11);
  mediump float tmpvar_12;
  tmpvar_12 = mix (_Layer0UvScale, _Layer1UvScale, tmpvar_10);
  tmpvar_4.xy = (tmpvar_4.xy * tmpvar_12);
  mediump vec4 tmpvar_13;
  tmpvar_13 = mix (_Layer1Color, _Layer0Color, vec4(tmpvar_10));
  tmpvar_7 = tmpvar_13;
  mediump vec4 tmpvar_14;
  tmpvar_14 = mix (_Layer0Color, _Layer1Color, vec4(tmpvar_10));
  tmpvar_8 = tmpvar_14;
  highp float tmpvar_15;
  highp vec3 x_16;
  x_16 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_15 = sqrt(dot (x_16, x_16));
  fogpos_1 = tmpvar_15;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD5 = tmpvar_5;
  xlv_TEXCOORD6 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = tmpvar_8;
  xlv_TEXCOORD8 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
uniform sampler2D _BlendTex;
uniform mediump float _PixelBlenderBias;
uniform mediump float _PixelBlenderScale;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD3;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 secondColor_1;
  lowp vec4 firstColor_2;
  highp vec4 biasuv_3;
  lowp vec4 c_4;
  c_4.w = 1.0;
  highp vec2 tmpvar_5;
  tmpvar_5 = fract(xlv_TEXCOORD0);
  biasuv_3.xy = tmpvar_5;
  biasuv_3.zw = (vec2(1.0, 1.0) - tmpvar_5);
  highp float tmpvar_6;
  tmpvar_6 = (min (dot (
    max (vec4(0.0, 0.0, 0.0, 0.0), ((biasuv_3 - vec4(0.93, 0.93, 0.93, 0.93)) * 14.2857))
  , vec4(1.0, 1.0, 1.0, 1.0)), 1.0) * -20.0);
  highp vec4 tmpvar_7;
  tmpvar_7.z = 0.0;
  tmpvar_7.xy = mix (xlv_TEXCOORD5.xy, xlv_TEXCOORD5.zw, tmpvar_5);
  tmpvar_7.w = tmpvar_6;
  highp vec4 tmpvar_8;
  tmpvar_8.z = 0.0;
  tmpvar_8.xy = mix ((xlv_TEXCOORD5.xy + xlv_TEXCOORD1.zw), (xlv_TEXCOORD5.zw + xlv_TEXCOORD1.zw), fract(xlv_TEXCOORD1).xy);
  tmpvar_8.w = tmpvar_6;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, tmpvar_7.xy, tmpvar_6);
  mediump vec4 tmpvar_10;
  tmpvar_10 = tmpvar_9;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_BlendTex, tmpvar_8.xy, tmpvar_6);
  mediump vec4 tmpvar_12;
  tmpvar_12 = tmpvar_11;
  mediump vec4 tmpvar_13;
  tmpvar_13 = (tmpvar_10 * xlv_TEXCOORD2);
  firstColor_2 = tmpvar_13;
  mediump vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_12 * xlv_TEXCOORD3);
  secondColor_1 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = mix (firstColor_2.xyz, secondColor_1.xyz, vec3(clamp ((xlv_TEXCOORD6.z + 
    (_PixelBlenderScale * (((xlv_TEXCOORD6.w * _PixelBlenderBias) + (
      ((secondColor_1.w - firstColor_2.w) * 0.5)
     + xlv_TEXCOORD6.z)) - 0.5))
  ), 0.0, 1.0)));
  c_4.xyz = tmpvar_15;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_17;
  tmpvar_17 = (2.0 * tmpvar_16.xyz);
  c_4.xyz = (c_4.xyz * tmpvar_17);
  c_4.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_4.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_4;
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
uniform 	mediump float _PixelBlenderBias;
uniform 	mediump float _PixelBlenderScale;
uniform 	mediump float _Layer0UvScale;
uniform 	mediump float _Layer1UvScale;
uniform 	lowp vec4 _Layer0Color;
uniform 	lowp vec4 _Layer1Color;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD1;
in highp vec4 in_TEXCOORD2;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out highp vec2 vs_TEXCOORD8;
out highp vec4 vs_TEXCOORD1;
out highp vec4 vs_TEXCOORD5;
out mediump vec4 vs_TEXCOORD6;
out lowp vec4 vs_TEXCOORD2;
out lowp vec4 vs_TEXCOORD3;
highp vec4 t0;
mediump vec4 t16_0;
mediump vec2 t16_1;
mediump float t16_3;
mediump float t16_5;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD8.xy = in_TEXCOORD1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    t16_1.xy = vec2((-float(_Layer1UvScale)) + float(_Layer0UvScale), (-float(_Layer0UvScale)) + float(_Layer1UvScale));
    t0.x = in_COLOR0.w * 2.0 + -1.0;
    t16_5 = t0.x * 512.0;
    t16_5 = clamp(t16_5, 0.0, 1.0);
    vs_TEXCOORD6.z = abs(t0.x);
    t16_1.xy = vec2(t16_5) * t16_1.xy + vec2(_Layer1UvScale, _Layer0UvScale);
    vs_TEXCOORD0.xy = t16_1.xx * in_TEXCOORD0.xy;
    vs_TEXCOORD1.xy = t16_1.yy * in_TEXCOORD0.xy;
    vs_TEXCOORD1.zw = in_TEXCOORD2.xy;
    t0.zw = in_COLOR0.zz + in_COLOR0.xy;
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1.x = (-t0.x) + acid_Fog.y;
    t16_3 = (-acid_Fog.x) + acid_Fog.y;
    t16_1.x = t16_1.x / t16_3;
    t16_1.x = clamp(t16_1.x, 0.0, 1.0);
    t16_1.x = (-t16_1.x) + 1.0;
    t16_3 = t16_1.x * acid_Fog.z;
    vs_TEXCOORD6.x = (-t16_1.x) * acid_Fog.z + 1.0;
    vs_TEXCOORD6.y = t16_3;
    vs_TEXCOORD6.w = t16_5 * 2.0 + -1.0;
    t16_0 = _Layer0Color + (-_Layer1Color);
    t16_0 = vec4(t16_5) * t16_0 + _Layer1Color;
    vs_TEXCOORD2 = t16_0;
    t16_0 = (-_Layer0Color) + _Layer1Color;
    t16_0 = vec4(t16_5) * t16_0 + _Layer0Color;
    vs_TEXCOORD3 = t16_0;
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
uniform 	mediump float _PixelBlenderBias;
uniform 	mediump float _PixelBlenderScale;
uniform 	mediump float _Layer0UvScale;
uniform 	mediump float _Layer1UvScale;
uniform 	lowp vec4 _Layer0Color;
uniform 	lowp vec4 _Layer1Color;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BlendTex;
uniform lowp sampler2D unity_Lightmap;
in highp vec2 vs_TEXCOORD0;
in highp vec2 vs_TEXCOORD8;
in highp vec4 vs_TEXCOORD1;
in highp vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD6;
in lowp vec4 vs_TEXCOORD2;
in lowp vec4 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec2 t0;
mediump vec4 t16_0;
lowp vec4 t10_0;
highp vec4 t1;
lowp vec3 t10_1;
highp vec4 t2;
lowp vec4 t10_2;
lowp vec3 t10_3;
mediump vec3 t16_4;
highp vec2 t10;
mediump float t16_19;
void main()
{
    t0.xy = fract(vs_TEXCOORD1.xy);
    t1 = vec4(vs_TEXCOORD1.z + vs_TEXCOORD5.x, vs_TEXCOORD1.w + vs_TEXCOORD5.y, vs_TEXCOORD1.z + vs_TEXCOORD5.z, vs_TEXCOORD1.w + vs_TEXCOORD5.w);
    t10.xy = vec2((-t1.x) + t1.z, (-t1.y) + t1.w);
    t0.xy = t0.xy * t10.xy + t1.xy;
    t1.xy = fract(vs_TEXCOORD0.xy);
    t1.zw = (-t1.xy);
    t2 = t1 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t2 = t2 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t2 = max(t2, vec4(0.0, 0.0, 0.0, 0.0));
    t10.x = dot(t2, vec4(1.0, 1.0, 1.0, 1.0));
    t10.x = min(t10.x, 1.0);
    t10.x = t10.x * -20.0;
    t10_2 = texture(_BlendTex, t0.xy, t10.x);
    t0.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t0.xy = t1.xy * t0.xy + vs_TEXCOORD5.xy;
    t10_0 = texture(_MainTex, t0.xy, t10.x);
    t16_0 = t10_0 * vs_TEXCOORD2;
    t10_3.x = t10_2.w * vs_TEXCOORD3.w + (-t16_0.w);
    t16_4.xyz = t10_2.xyz * vs_TEXCOORD3.xyz + (-t16_0.xyz);
    t16_19 = t10_3.x * 0.5 + vs_TEXCOORD6.z;
    t16_19 = vs_TEXCOORD6.w * _PixelBlenderBias + t16_19;
    t16_19 = t16_19 + -0.5;
    t16_19 = _PixelBlenderScale * t16_19 + vs_TEXCOORD6.z;
    t16_19 = clamp(t16_19, 0.0, 1.0);
    t16_4.xyz = vec3(t16_19) * t16_4.xyz + t16_0.xyz;
    t10_1.xyz = texture(unity_Lightmap, vs_TEXCOORD8.xy).xyz;
    t10_3.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0);
    t16_4.xyz = t10_3.xyz * t16_4.xyz;
    t16_4.xyz = t16_4.xyz * vs_TEXCOORD6.xxx;
    t16_4.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_4.xyz;
    SV_Target0.xyz = t16_4.xyz;
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
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
attribute vec4 _glesMultiTexCoord2;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform mediump float _Layer0UvScale;
uniform mediump float _Layer1UvScale;
uniform lowp vec4 _Layer0Color;
uniform lowp vec4 _Layer1Color;
uniform mediump vec4 acid_Fog;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  mediump float fogpos_1;
  mediump float val_2;
  highp vec2 tmpvar_3;
  highp vec4 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  lowp vec4 tmpvar_7;
  lowp vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  tmpvar_6.x = 0.0;
  tmpvar_5.xy = _glesColor.xy;
  tmpvar_5.zw = (_glesColor.xy + _glesColor.zz);
  tmpvar_5 = (tmpvar_5 * 0.9960938);
  tmpvar_5.zw = (tmpvar_5.zw + 0.00390625);
  tmpvar_3 = _glesMultiTexCoord0.xy;
  tmpvar_4.xy = tmpvar_3;
  tmpvar_4.zw = _glesMultiTexCoord2.xy;
  highp float tmpvar_10;
  tmpvar_10 = ((_glesColor.w * 2.0) - 1.0);
  val_2 = tmpvar_10;
  tmpvar_6.z = abs(val_2);
  mediump float tmpvar_11;
  tmpvar_11 = clamp ((val_2 * 512.0), 0.0, 1.0);
  tmpvar_6.w = ((tmpvar_11 * 2.0) - 1.0);
  mediump float tmpvar_12;
  tmpvar_12 = mix (_Layer1UvScale, _Layer0UvScale, tmpvar_11);
  tmpvar_3 = (_glesMultiTexCoord0.xy * tmpvar_12);
  mediump float tmpvar_13;
  tmpvar_13 = mix (_Layer0UvScale, _Layer1UvScale, tmpvar_11);
  tmpvar_4.xy = (tmpvar_4.xy * tmpvar_13);
  mediump vec4 tmpvar_14;
  tmpvar_14 = mix (_Layer1Color, _Layer0Color, vec4(tmpvar_11));
  tmpvar_7 = tmpvar_14;
  mediump vec4 tmpvar_15;
  tmpvar_15 = mix (_Layer0Color, _Layer1Color, vec4(tmpvar_11));
  tmpvar_8 = tmpvar_15;
  highp float tmpvar_16;
  highp vec3 x_17;
  x_17 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_16 = sqrt(dot (x_17, x_17));
  fogpos_1 = tmpvar_16;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  tmpvar_9 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD5 = tmpvar_5;
  xlv_TEXCOORD6 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = tmpvar_8;
  xlv_TEXCOORD7 = tmpvar_9;
  xlv_TEXCOORD8 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform sampler2D unity_Lightmap;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BlendTex;
uniform mediump float _PixelBlenderBias;
uniform mediump float _PixelBlenderScale;
uniform lowp vec4 _EnvAmbientColor;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  lowp vec3 lm_2;
  lowp vec4 secondColor_3;
  lowp vec4 firstColor_4;
  highp vec4 biasuv_5;
  lowp vec4 c_6;
  c_6.w = 1.0;
  highp vec2 tmpvar_7;
  tmpvar_7 = fract(xlv_TEXCOORD0);
  biasuv_5.xy = tmpvar_7;
  biasuv_5.zw = (vec2(1.0, 1.0) - tmpvar_7);
  highp float tmpvar_8;
  tmpvar_8 = (min (dot (
    max (vec4(0.0, 0.0, 0.0, 0.0), ((biasuv_5 - vec4(0.93, 0.93, 0.93, 0.93)) * 14.2857))
  , vec4(1.0, 1.0, 1.0, 1.0)), 1.0) * -20.0);
  highp vec4 tmpvar_9;
  tmpvar_9.z = 0.0;
  tmpvar_9.xy = mix (xlv_TEXCOORD5.xy, xlv_TEXCOORD5.zw, tmpvar_7);
  tmpvar_9.w = tmpvar_8;
  highp vec4 tmpvar_10;
  tmpvar_10.z = 0.0;
  tmpvar_10.xy = mix ((xlv_TEXCOORD5.xy + xlv_TEXCOORD1.zw), (xlv_TEXCOORD5.zw + xlv_TEXCOORD1.zw), fract(xlv_TEXCOORD1).xy);
  tmpvar_10.w = tmpvar_8;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, tmpvar_9.xy, tmpvar_8);
  mediump vec4 tmpvar_12;
  tmpvar_12 = tmpvar_11;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_BlendTex, tmpvar_10.xy, tmpvar_8);
  mediump vec4 tmpvar_14;
  tmpvar_14 = tmpvar_13;
  mediump vec4 tmpvar_15;
  tmpvar_15 = (tmpvar_12 * xlv_TEXCOORD2);
  firstColor_4 = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (tmpvar_14 * xlv_TEXCOORD3);
  secondColor_3 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = mix (firstColor_4.xyz, secondColor_3.xyz, vec3(clamp ((xlv_TEXCOORD6.z + 
    (_PixelBlenderScale * (((xlv_TEXCOORD6.w * _PixelBlenderBias) + (
      ((secondColor_3.w - firstColor_4.w) * 0.5)
     + xlv_TEXCOORD6.z)) - 0.5))
  ), 0.0, 1.0)));
  c_6.xyz = tmpvar_17;
  lowp float tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD7).x > 
    (xlv_TEXCOORD7.z / xlv_TEXCOORD7.w)
  )), _LightShadowData.x);
  tmpvar_18 = tmpvar_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_21;
  tmpvar_21 = (2.0 * tmpvar_20.xyz);
  lm_2 = tmpvar_21;
  lowp vec3 tmpvar_22;
  tmpvar_22 = min (lm_2, mix (_EnvAmbientColor.xyz, lm_2, vec3(tmpvar_18)));
  tmpvar_1 = tmpvar_22;
  c_6.xyz = (c_6.xyz * tmpvar_1);
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_6.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_6;
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
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
attribute vec4 _glesMultiTexCoord2;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform mediump float _Layer0UvScale;
uniform mediump float _Layer1UvScale;
uniform lowp vec4 _Layer0Color;
uniform lowp vec4 _Layer1Color;
uniform mediump vec4 acid_Fog;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  mediump float fogpos_1;
  mediump float val_2;
  highp vec2 tmpvar_3;
  highp vec4 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump vec4 tmpvar_6;
  lowp vec4 tmpvar_7;
  lowp vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  tmpvar_6.x = 0.0;
  tmpvar_5.xy = _glesColor.xy;
  tmpvar_5.zw = (_glesColor.xy + _glesColor.zz);
  tmpvar_5 = (tmpvar_5 * 0.9960938);
  tmpvar_5.zw = (tmpvar_5.zw + 0.00390625);
  tmpvar_3 = _glesMultiTexCoord0.xy;
  tmpvar_4.xy = tmpvar_3;
  tmpvar_4.zw = _glesMultiTexCoord2.xy;
  highp float tmpvar_10;
  tmpvar_10 = ((_glesColor.w * 2.0) - 1.0);
  val_2 = tmpvar_10;
  tmpvar_6.z = abs(val_2);
  mediump float tmpvar_11;
  tmpvar_11 = clamp ((val_2 * 512.0), 0.0, 1.0);
  tmpvar_6.w = ((tmpvar_11 * 2.0) - 1.0);
  mediump float tmpvar_12;
  tmpvar_12 = mix (_Layer1UvScale, _Layer0UvScale, tmpvar_11);
  tmpvar_3 = (_glesMultiTexCoord0.xy * tmpvar_12);
  mediump float tmpvar_13;
  tmpvar_13 = mix (_Layer0UvScale, _Layer1UvScale, tmpvar_11);
  tmpvar_4.xy = (tmpvar_4.xy * tmpvar_13);
  mediump vec4 tmpvar_14;
  tmpvar_14 = mix (_Layer1Color, _Layer0Color, vec4(tmpvar_11));
  tmpvar_7 = tmpvar_14;
  mediump vec4 tmpvar_15;
  tmpvar_15 = mix (_Layer0Color, _Layer1Color, vec4(tmpvar_11));
  tmpvar_8 = tmpvar_15;
  highp float tmpvar_16;
  highp vec3 x_17;
  x_17 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_16 = sqrt(dot (x_17, x_17));
  fogpos_1 = tmpvar_16;
  tmpvar_6.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_1) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_6.x = (1.0 - tmpvar_6.y);
  tmpvar_9 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD5 = tmpvar_5;
  xlv_TEXCOORD6 = tmpvar_6;
  xlv_TEXCOORD2 = tmpvar_7;
  xlv_TEXCOORD3 = tmpvar_8;
  xlv_TEXCOORD7 = tmpvar_9;
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
uniform sampler2D _BlendTex;
uniform mediump float _PixelBlenderBias;
uniform mediump float _PixelBlenderScale;
uniform lowp vec4 _EnvAmbientColor;
mediump vec3 tmpvar_1;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD7;
varying highp vec2 xlv_TEXCOORD8;
void main ()
{
  lowp vec3 lm_2;
  lowp vec4 secondColor_3;
  lowp vec4 firstColor_4;
  highp vec4 biasuv_5;
  lowp vec4 c_6;
  c_6.w = 1.0;
  highp vec2 tmpvar_7;
  tmpvar_7 = fract(xlv_TEXCOORD0);
  biasuv_5.xy = tmpvar_7;
  biasuv_5.zw = (vec2(1.0, 1.0) - tmpvar_7);
  highp float tmpvar_8;
  tmpvar_8 = (min (dot (
    max (vec4(0.0, 0.0, 0.0, 0.0), ((biasuv_5 - vec4(0.93, 0.93, 0.93, 0.93)) * 14.2857))
  , vec4(1.0, 1.0, 1.0, 1.0)), 1.0) * -20.0);
  highp vec4 tmpvar_9;
  tmpvar_9.z = 0.0;
  tmpvar_9.xy = mix (xlv_TEXCOORD5.xy, xlv_TEXCOORD5.zw, tmpvar_7);
  tmpvar_9.w = tmpvar_8;
  highp vec4 tmpvar_10;
  tmpvar_10.z = 0.0;
  tmpvar_10.xy = mix ((xlv_TEXCOORD5.xy + xlv_TEXCOORD1.zw), (xlv_TEXCOORD5.zw + xlv_TEXCOORD1.zw), fract(xlv_TEXCOORD1).xy);
  tmpvar_10.w = tmpvar_8;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, tmpvar_9.xy, tmpvar_8);
  mediump vec4 tmpvar_12;
  tmpvar_12 = tmpvar_11;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_BlendTex, tmpvar_10.xy, tmpvar_8);
  mediump vec4 tmpvar_14;
  tmpvar_14 = tmpvar_13;
  mediump vec4 tmpvar_15;
  tmpvar_15 = (tmpvar_12 * xlv_TEXCOORD2);
  firstColor_4 = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (tmpvar_14 * xlv_TEXCOORD3);
  secondColor_3 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = mix (firstColor_4.xyz, secondColor_3.xyz, vec3(clamp ((xlv_TEXCOORD6.z + 
    (_PixelBlenderScale * (((xlv_TEXCOORD6.w * _PixelBlenderBias) + (
      ((secondColor_3.w - firstColor_4.w) * 0.5)
     + xlv_TEXCOORD6.z)) - 0.5))
  ), 0.0, 1.0)));
  c_6.xyz = tmpvar_17;
  lowp float shadow_18;
  shadow_18 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD7.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (unity_Lightmap, xlv_TEXCOORD8);
  mediump vec3 tmpvar_20;
  tmpvar_20 = (2.0 * tmpvar_19.xyz);
  lm_2 = tmpvar_20;
  lowp vec3 tmpvar_21;
  tmpvar_21 = min (lm_2, mix (_EnvAmbientColor.xyz, lm_2, vec3(shadow_18)));
  tmpvar_1 = tmpvar_21;
  c_6.xyz = (c_6.xyz * tmpvar_1);
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD6.y) + (c_6.xyz * xlv_TEXCOORD6.x));
  gl_FragData[0] = c_6;
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
uniform 	mediump float _PixelBlenderBias;
uniform 	mediump float _PixelBlenderScale;
uniform 	mediump float _Layer0UvScale;
uniform 	mediump float _Layer1UvScale;
uniform 	lowp vec4 _Layer0Color;
uniform 	lowp vec4 _Layer1Color;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD1;
in highp vec4 in_TEXCOORD2;
in highp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out highp vec2 vs_TEXCOORD8;
out highp vec4 vs_TEXCOORD1;
out highp vec4 vs_TEXCOORD5;
out mediump vec4 vs_TEXCOORD6;
out lowp vec4 vs_TEXCOORD2;
out lowp vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD7;
highp vec4 t0;
mediump vec4 t16_0;
highp vec4 t1;
mediump vec2 t16_1;
mediump float t16_3;
mediump float t16_5;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    vs_TEXCOORD8.xy = in_TEXCOORD1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    t16_1.xy = vec2((-float(_Layer1UvScale)) + float(_Layer0UvScale), (-float(_Layer0UvScale)) + float(_Layer1UvScale));
    t0.x = in_COLOR0.w * 2.0 + -1.0;
    t16_5 = t0.x * 512.0;
    t16_5 = clamp(t16_5, 0.0, 1.0);
    vs_TEXCOORD6.z = abs(t0.x);
    t16_1.xy = vec2(t16_5) * t16_1.xy + vec2(_Layer1UvScale, _Layer0UvScale);
    vs_TEXCOORD0.xy = t16_1.xx * in_TEXCOORD0.xy;
    vs_TEXCOORD1.xy = t16_1.yy * in_TEXCOORD0.xy;
    vs_TEXCOORD1.zw = in_TEXCOORD2.xy;
    t0.zw = in_COLOR0.zz + in_COLOR0.xy;
    t0.xy = in_COLOR0.xy;
    vs_TEXCOORD5 = t0 * vec4(0.99609375, 0.99609375, 0.99609375, 0.99609375) + vec4(0.0, 0.0, 0.00390625, 0.00390625);
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1.x = (-t0.x) + acid_Fog.y;
    t16_3 = (-acid_Fog.x) + acid_Fog.y;
    t16_1.x = t16_1.x / t16_3;
    t16_1.x = clamp(t16_1.x, 0.0, 1.0);
    t16_1.x = (-t16_1.x) + 1.0;
    t16_3 = t16_1.x * acid_Fog.z;
    vs_TEXCOORD6.x = (-t16_1.x) * acid_Fog.z + 1.0;
    vs_TEXCOORD6.y = t16_3;
    vs_TEXCOORD6.w = t16_5 * 2.0 + -1.0;
    t16_0 = _Layer0Color + (-_Layer1Color);
    t16_0 = vec4(t16_5) * t16_0 + _Layer1Color;
    vs_TEXCOORD2 = t16_0;
    t16_0 = (-_Layer0Color) + _Layer1Color;
    t16_0 = vec4(t16_5) * t16_0 + _Layer0Color;
    vs_TEXCOORD3 = t16_0;
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
uniform 	mediump float _PixelBlenderBias;
uniform 	mediump float _PixelBlenderScale;
uniform 	mediump float _Layer0UvScale;
uniform 	mediump float _Layer1UvScale;
uniform 	lowp vec4 _Layer0Color;
uniform 	lowp vec4 _Layer1Color;
uniform 	lowp vec4 _EnvAmbientColor;
uniform 	mediump vec4 acid_Fog;
uniform 	g_fd_Type g_fd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BlendTex;
uniform lowp sampler2D unity_Lightmap;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in highp vec2 vs_TEXCOORD8;
in highp vec4 vs_TEXCOORD1;
in highp vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD6;
in lowp vec4 vs_TEXCOORD2;
in lowp vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD7;
layout(location = 0) out lowp vec4 SV_Target0;
highp vec2 t0;
mediump vec4 t16_0;
lowp vec4 t10_0;
highp vec4 t1;
lowp vec3 t10_1;
highp vec4 t2;
lowp vec4 t10_2;
lowp vec3 t10_3;
mediump vec3 t16_4;
mediump float t16_5;
lowp vec3 t10_6;
highp vec2 t14;
mediump float t16_25;
void main()
{
    t0.xy = fract(vs_TEXCOORD1.xy);
    t1 = vec4(vs_TEXCOORD1.z + vs_TEXCOORD5.x, vs_TEXCOORD1.w + vs_TEXCOORD5.y, vs_TEXCOORD1.z + vs_TEXCOORD5.z, vs_TEXCOORD1.w + vs_TEXCOORD5.w);
    t14.xy = vec2((-t1.x) + t1.z, (-t1.y) + t1.w);
    t0.xy = t0.xy * t14.xy + t1.xy;
    t1.xy = fract(vs_TEXCOORD0.xy);
    t1.zw = (-t1.xy);
    t2 = t1 + vec4(-0.930000007, -0.930000007, 0.0699999928, 0.0699999928);
    t2 = t2 * vec4(14.2856998, 14.2856998, 14.2856998, 14.2856998);
    t2 = max(t2, vec4(0.0, 0.0, 0.0, 0.0));
    t14.x = dot(t2, vec4(1.0, 1.0, 1.0, 1.0));
    t14.x = min(t14.x, 1.0);
    t14.x = t14.x * -20.0;
    t10_2 = texture(_BlendTex, t0.xy, t14.x);
    t0.xy = vec2((-vs_TEXCOORD5.x) + vs_TEXCOORD5.z, (-vs_TEXCOORD5.y) + vs_TEXCOORD5.w);
    t0.xy = t1.xy * t0.xy + vs_TEXCOORD5.xy;
    t10_0 = texture(_MainTex, t0.xy, t14.x);
    t16_0 = t10_0 * vs_TEXCOORD2;
    t10_3.x = t10_2.w * vs_TEXCOORD3.w + (-t16_0.w);
    t16_4.xyz = t10_2.xyz * vs_TEXCOORD3.xyz + (-t16_0.xyz);
    t16_25 = t10_3.x * 0.5 + vs_TEXCOORD6.z;
    t16_25 = vs_TEXCOORD6.w * _PixelBlenderBias + t16_25;
    t16_25 = t16_25 + -0.5;
    t16_25 = _PixelBlenderScale * t16_25 + vs_TEXCOORD6.z;
    t16_25 = clamp(t16_25, 0.0, 1.0);
    t16_4.xyz = vec3(t16_25) * t16_4.xyz + t16_0.xyz;
    vec3 txVec0 = vec3(vs_TEXCOORD7.xy,vs_TEXCOORD7.z);
    t16_25 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec0, 0.0);
    t16_5 = (-_LightShadowData.x) + 1.0;
    t16_25 = t16_25 * t16_5 + _LightShadowData.x;
    t10_1.xyz = texture(unity_Lightmap, vs_TEXCOORD8.xy).xyz;
    t10_3.xyz = vec3(2.0, 2.0, 2.0) * t10_1.xyz + (-_EnvAmbientColor.xyz);
    t10_6.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0);
    t10_3.xyz = vec3(t16_25) * t10_3.xyz + _EnvAmbientColor.xyz;
    t10_3.xyz = min(t10_3.xyz, t10_6.xyz);
    t16_4.xyz = t10_3.xyz * t16_4.xyz;
    t16_4.xyz = t16_4.xyz * vs_TEXCOORD6.xxx;
    t16_4.xyz = unity_FogColor.xyz * vs_TEXCOORD6.yyy + t16_4.xyz;
    SV_Target0.xyz = t16_4.xyz;
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
Fallback "Hidden/Shadow/Opaque"
}
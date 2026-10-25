οκ)Shader "Character/CharacterPropFade" {
Properties {
 _MainTex ("Albedo (RGB) Specular Mask (A)", 2D) = "white" { }
 _BumpMap ("Normalmap", 2D) = "bump" { }
 _RampTex ("Ramp Texture", 2D) = "bump" { }
 _FadeColor ("Fade Color", Color) = (1,1,1,1)
}
SubShader { 
 LOD 99
 Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry" "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry" "RenderType"="Opaque" }
  GpuProgramID 41773
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_OFF" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_20;
  tmpvar_20 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_20;
  c_6.w = _RimColor.w;
  lowp vec4 _color_21;
  _color_21 = (c_6 * _FadeColor);
  c_6.w = _color_21.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_21.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_OFF" "SIMPLE_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
mediump vec3 t16_5;
mediump float t16_15;
mediump float t16_18;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_15 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_15 = t16_15 + t16_15;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_15)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_4.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_5.x = t16_0.y * _PropGlossiness.w;
    t16_5.x = t16_5.x * 10.0;
    t16_5.xyz = t16_5.xxx * _PropGlossiness.xyz;
    t16_5.xyz = t16_3.xyz * t16_5.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_5.xyz = t16_5.xyz * t10_1.www;
    t16_5.xyz = t10_1.xyz * t16_5.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_18 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_18) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_18 = t16_0.x * 1.5;
    t16_18 = min(t16_18, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_18) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_5.xyz = t16_3.xyz * t16_4.xyz + t16_5.xyz;
    t16_3.x = dot(t16_5.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_5.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_5.xyz;
    t10_2.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t10_2.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w * _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_OFF" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_20;
  tmpvar_20 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_20;
  c_6.w = _RimColor.w;
  lowp vec4 _color_21;
  lowp vec4 tmpvar_22;
  tmpvar_22 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_23;
  tmpvar_23 = (_FadeColor.w - ((0.9375 * tmpvar_22.y) + 0.03125));
  mediump float tmpvar_24;
  tmpvar_24 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_23)
  )), 0.0, 1.0);
  _color_21.xyz = (_FadeColor.xyz * (tmpvar_24 + c_6.xyz));
  _color_21.w = ((32.0 * tmpvar_23) + 0.9375);
  c_6.w = _color_21.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_21.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_OFF" "FANCY_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _NoiseTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
mediump vec3 t16_6;
mediump float t16_18;
mediump float t16_21;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_18 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_18 = t16_18 + t16_18;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_18)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_4.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_6.x = t16_0.y * _PropGlossiness.w;
    t16_6.x = t16_6.x * 10.0;
    t16_6.xyz = t16_6.xxx * _PropGlossiness.xyz;
    t16_6.xyz = t16_3.xyz * t16_6.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_6.xyz = t16_6.xyz * t10_1.www;
    t16_6.xyz = t10_1.xyz * t16_6.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_21 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_21) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_21 = t16_0.x * 1.5;
    t16_21 = min(t16_21, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_21) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_3.xyz * t16_4.xyz + t16_6.xyz;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t10_5 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_18 = t10_5 * 0.9375 + 0.03125;
    t16_18 = (-t16_18) + _FadeColor.w;
    t16_3.x = -abs(t16_18) * 32.0 + 1.0;
    t16_1.w = t16_18 * 32.0 + 0.9375;
    t16_18 = max(t16_3.x, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_18);
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0 = t16_1;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_OFF" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  highp vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  mediump vec2 tmpvar_10;
  mediump vec3 tmpvar_11;
  tmpvar_10.x = 0.0;
  tmpvar_7 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = (tmpvar_7 * 0.5);
  highp vec2 tmpvar_14;
  tmpvar_14.x = tmpvar_13.x;
  tmpvar_14.y = (tmpvar_13.y * _ProjectionParams.x);
  o_12.xy = (tmpvar_14 + tmpvar_13.w);
  o_12.zw = tmpvar_7.zw;
  tmpvar_11 = o_12.xyw;
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_8.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_9.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_9.zw = tmpvar_1.xy;
  highp vec3 tmpvar_15;
  tmpvar_15 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_15;
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _Object2World[0].xyz;
  tmpvar_16[1] = _Object2World[1].xyz;
  tmpvar_16[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_16 * _glesNormal));
  worldNormal_5 = tmpvar_17;
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_18 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_19;
  mediump vec3 tmpvar_20;
  tmpvar_20 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22.xyz = worldViewDir_6;
  tmpvar_22.w = worldBinormal_3.x;
  mediump vec4 tmpvar_23;
  tmpvar_23.xyz = worldNormal_5;
  tmpvar_23.w = worldBinormal_3.y;
  mediump vec4 tmpvar_24;
  tmpvar_24.xyz = worldTangent_4;
  tmpvar_24.w = worldBinormal_3.z;
  highp float tmpvar_25;
  highp vec3 x_26;
  x_26 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_25 = sqrt(dot (x_26, x_26));
  fogpos_2 = tmpvar_25;
  tmpvar_10.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_10.x = (1.0 - tmpvar_10.y);
  gl_Position = tmpvar_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = tmpvar_10;
  xlv_TEXCOORD3 = tmpvar_22;
  xlv_TEXCOORD4 = tmpvar_23;
  xlv_TEXCOORD5 = tmpvar_24;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_11;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_20;
  tmpvar_20 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_20;
  c_6.w = _RimColor.w;
  lowp vec4 _color_21;
  _color_21.w = c_6.w;
  mediump float rTest_22;
  mediump vec2 tmpvar_23;
  tmpvar_23 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_24;
  tmpvar_24 = ((1.5 * _FadeColor.w) - tmpvar_23.y);
  if ((tmpvar_24 < 0.0)) {
    discard;
  };
  mediump float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (tmpvar_24 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_26;
  tmpvar_26.x = tmpvar_23.x;
  tmpvar_26.y = clamp ((tmpvar_25 - tmpvar_23.y), 0.0, 1.0);
  lowp float tmpvar_27;
  tmpvar_27 = texture2D (_RasterTex, tmpvar_26).z;
  rTest_22 = tmpvar_27;
  mediump float x_28;
  x_28 = (rTest_22 - 0.4);
  if ((x_28 < 0.0)) {
    discard;
  };
  _color_21.xyz = (c_6.xyz + (_FadeColor.xyz * tmpvar_25));
  c_6.w = _color_21.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_21.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_OFF" "RASTER_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD7;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_2;
highp vec4 t3;
mediump vec3 t16_4;
highp vec3 t5;
mediump float t16_8;
highp float t18;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t1.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t1;
    t1.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t1.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t1;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = sqrt(t18);
    t16_2 = (-t18) + acid_Fog.y;
    t16_8 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_8;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_8 = t16_2 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_8;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = inversesqrt(t18);
    t1.xyz = vec3(t18) * t1.xyz;
    t3.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t2.xyz = vec3(t18) * t3.xyz;
    t3.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t3.xyz = vec3(t18) * t3.xyz;
    t16_4.xyz = t2.zxy * t3.yzx;
    t16_4.xyz = t2.yzx * t3.zxy + (-t16_4.xyz);
    t5.xyz = t16_4.xyz * in_TANGENT0.www;
    t18 = dot(t5.xyz, t5.xyz);
    t18 = inversesqrt(t18);
    t5.xyz = vec3(t18) * t5.xyz;
    t1.w = t5.x;
    vs_TEXCOORD3 = t1;
    t2.w = t5.y;
    t3.w = t5.z;
    vs_TEXCOORD5 = t3;
    vs_TEXCOORD4 = t2;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0.w = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t1.zz + t1.xw;
    vs_TEXCOORD7.xyz = t0.xyz;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _RasterTex;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec3 vs_TEXCOORD7;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
bool tb1;
mediump vec4 t16_2;
lowp vec3 t10_2;
mediump vec4 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
mediump float t16_9;
mediump vec3 t16_10;
mediump float t16_18;
void main()
{
    t16_0.xy = vs_TEXCOORD7.xy / vs_TEXCOORD7.zz;
    t16_18 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_18<0.0;
    t16_18 = (-t16_18) * 2.5 + 1.0;
    t16_18 = clamp(t16_18, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_18;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1.x = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1.x + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = vs_TEXCOORD5.y;
    t16_0.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = t16_1.y * t16_1.y;
    t16_0.x = t16_1.x * t16_1.x + (-t16_0.x);
    t16_3.xy = vs_TEXCOORD5.zw;
    t16_3.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_2.xyz, t16_3.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_3.x = dot(unity_SHBr, t16_2);
    t16_3.y = dot(unity_SHBg, t16_2);
    t16_3.z = dot(unity_SHBb, t16_2);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_3.xyz;
    t16_1.w = 1.0;
    t16_3.x = dot(unity_SHAr, t16_1);
    t16_3.y = dot(unity_SHAg, t16_1);
    t16_3.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_3.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t16_3.x = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_3.x = t16_3.x + t16_3.x;
    t16_3.xyz = t16_1.xyz * (-t16_3.xxx) + vs_TEXCOORD3.xyz;
    t16_3.x = dot(t16_3.xyz, (-t16_1.xyz));
    t16_3.y = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_3.xy = max(t16_3.xy, vec2(0.25, 0.0));
    t16_3.x = t16_3.x * _PropGlossiness.w;
    t16_3.x = t16_3.x * 10.0;
    t16_3.xzw = t16_3.xxx * _PropGlossiness.xyz;
    t16_3.xzw = t16_0.xyz * t16_3.xzw;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_3.xzw = t10_1.www * t16_3.xzw;
    t16_3.xzw = t10_1.xyz * t16_3.xzw;
    t16_0.xyz = t16_0.xyz * t10_1.xyz;
    t16_4.x = (-t10_1.w) + 1.0;
    t16_0.xyz = t16_0.xyz * t16_4.xxx;
    t10_5 = (-_LightDir.w) + 1.0;
    t16_9 = t16_3.y * _LightDir.w + t10_5;
    t16_4.x = t16_9 * 1.5;
    t16_4.x = min(t16_4.x, 1.0);
    t16_10.xyz = vec3(t16_9) + (-_CharacterShadowColor.xyz);
    t16_9 = t16_9 * _LightParameters.y;
    t16_4.xyz = t16_4.xxx * t16_10.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_4.xyz + t16_3.xzw;
    t16_3.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xzw = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_9) * t16_3.xzw + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_18) + t16_0.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_20;
  tmpvar_20 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_20;
  c_6.w = _RimColor.w;
  lowp vec4 _color_21;
  _color_21 = (c_6 * _FadeColor);
  c_6.w = _color_21.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_21.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "SIMPLE_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
mediump vec3 t16_5;
mediump float t16_15;
mediump float t16_18;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_15 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_15 = t16_15 + t16_15;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_15)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_4.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_5.x = t16_0.y * _PropGlossiness.w;
    t16_5.x = t16_5.x * 10.0;
    t16_5.xyz = t16_5.xxx * _PropGlossiness.xyz;
    t16_5.xyz = t16_3.xyz * t16_5.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_5.xyz = t16_5.xyz * t10_1.www;
    t16_5.xyz = t10_1.xyz * t16_5.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_18 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_18) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_18 = t16_0.x * 1.5;
    t16_18 = min(t16_18, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_18) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_5.xyz = t16_3.xyz * t16_4.xyz + t16_5.xyz;
    t16_3.x = dot(t16_5.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_5.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_5.xyz;
    t10_2.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t10_2.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w * _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_20;
  tmpvar_20 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_20;
  c_6.w = _RimColor.w;
  lowp vec4 _color_21;
  lowp vec4 tmpvar_22;
  tmpvar_22 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_23;
  tmpvar_23 = (_FadeColor.w - ((0.9375 * tmpvar_22.y) + 0.03125));
  mediump float tmpvar_24;
  tmpvar_24 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_23)
  )), 0.0, 1.0);
  _color_21.xyz = (_FadeColor.xyz * (tmpvar_24 + c_6.xyz));
  _color_21.w = ((32.0 * tmpvar_23) + 0.9375);
  c_6.w = _color_21.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_21.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "FANCY_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _NoiseTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
mediump vec3 t16_6;
mediump float t16_18;
mediump float t16_21;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_18 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_18 = t16_18 + t16_18;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_18)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_4.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_6.x = t16_0.y * _PropGlossiness.w;
    t16_6.x = t16_6.x * 10.0;
    t16_6.xyz = t16_6.xxx * _PropGlossiness.xyz;
    t16_6.xyz = t16_3.xyz * t16_6.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_6.xyz = t16_6.xyz * t10_1.www;
    t16_6.xyz = t10_1.xyz * t16_6.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_21 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_21) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_21 = t16_0.x * 1.5;
    t16_21 = min(t16_21, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_21) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_3.xyz * t16_4.xyz + t16_6.xyz;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t10_5 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_18 = t10_5 * 0.9375 + 0.03125;
    t16_18 = (-t16_18) + _FadeColor.w;
    t16_3.x = -abs(t16_18) * 32.0 + 1.0;
    t16_1.w = t16_18 * 32.0 + 0.9375;
    t16_18 = max(t16_3.x, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_18);
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0 = t16_1;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  highp vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  mediump vec2 tmpvar_10;
  mediump vec3 tmpvar_11;
  tmpvar_10.x = 0.0;
  tmpvar_7 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = (tmpvar_7 * 0.5);
  highp vec2 tmpvar_14;
  tmpvar_14.x = tmpvar_13.x;
  tmpvar_14.y = (tmpvar_13.y * _ProjectionParams.x);
  o_12.xy = (tmpvar_14 + tmpvar_13.w);
  o_12.zw = tmpvar_7.zw;
  tmpvar_11 = o_12.xyw;
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_8.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_9.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_9.zw = tmpvar_1.xy;
  highp vec3 tmpvar_15;
  tmpvar_15 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_15;
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _Object2World[0].xyz;
  tmpvar_16[1] = _Object2World[1].xyz;
  tmpvar_16[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_16 * _glesNormal));
  worldNormal_5 = tmpvar_17;
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_18 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_19;
  mediump vec3 tmpvar_20;
  tmpvar_20 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22.xyz = worldViewDir_6;
  tmpvar_22.w = worldBinormal_3.x;
  mediump vec4 tmpvar_23;
  tmpvar_23.xyz = worldNormal_5;
  tmpvar_23.w = worldBinormal_3.y;
  mediump vec4 tmpvar_24;
  tmpvar_24.xyz = worldTangent_4;
  tmpvar_24.w = worldBinormal_3.z;
  highp float tmpvar_25;
  highp vec3 x_26;
  x_26 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_25 = sqrt(dot (x_26, x_26));
  fogpos_2 = tmpvar_25;
  tmpvar_10.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_10.x = (1.0 - tmpvar_10.y);
  gl_Position = tmpvar_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = tmpvar_10;
  xlv_TEXCOORD3 = tmpvar_22;
  xlv_TEXCOORD4 = tmpvar_23;
  xlv_TEXCOORD5 = tmpvar_24;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_11;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_20;
  tmpvar_20 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_20;
  c_6.w = _RimColor.w;
  lowp vec4 _color_21;
  _color_21.w = c_6.w;
  mediump float rTest_22;
  mediump vec2 tmpvar_23;
  tmpvar_23 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_24;
  tmpvar_24 = ((1.5 * _FadeColor.w) - tmpvar_23.y);
  if ((tmpvar_24 < 0.0)) {
    discard;
  };
  mediump float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (tmpvar_24 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_26;
  tmpvar_26.x = tmpvar_23.x;
  tmpvar_26.y = clamp ((tmpvar_25 - tmpvar_23.y), 0.0, 1.0);
  lowp float tmpvar_27;
  tmpvar_27 = texture2D (_RasterTex, tmpvar_26).z;
  rTest_22 = tmpvar_27;
  mediump float x_28;
  x_28 = (rTest_22 - 0.4);
  if ((x_28 < 0.0)) {
    discard;
  };
  _color_21.xyz = (c_6.xyz + (_FadeColor.xyz * tmpvar_25));
  c_6.w = _color_21.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_21.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "RASTER_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD7;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_2;
highp vec4 t3;
mediump vec3 t16_4;
highp vec3 t5;
mediump float t16_8;
highp float t18;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t1.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t1;
    t1.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t1.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t1;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = sqrt(t18);
    t16_2 = (-t18) + acid_Fog.y;
    t16_8 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_8;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_8 = t16_2 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_8;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = inversesqrt(t18);
    t1.xyz = vec3(t18) * t1.xyz;
    t3.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t2.xyz = vec3(t18) * t3.xyz;
    t3.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t3.xyz = vec3(t18) * t3.xyz;
    t16_4.xyz = t2.zxy * t3.yzx;
    t16_4.xyz = t2.yzx * t3.zxy + (-t16_4.xyz);
    t5.xyz = t16_4.xyz * in_TANGENT0.www;
    t18 = dot(t5.xyz, t5.xyz);
    t18 = inversesqrt(t18);
    t5.xyz = vec3(t18) * t5.xyz;
    t1.w = t5.x;
    vs_TEXCOORD3 = t1;
    t2.w = t5.y;
    t3.w = t5.z;
    vs_TEXCOORD5 = t3;
    vs_TEXCOORD4 = t2;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0.w = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t1.zz + t1.xw;
    vs_TEXCOORD7.xyz = t0.xyz;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _RasterTex;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec3 vs_TEXCOORD7;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
bool tb1;
mediump vec4 t16_2;
lowp vec3 t10_2;
mediump vec4 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
mediump float t16_9;
mediump vec3 t16_10;
mediump float t16_18;
void main()
{
    t16_0.xy = vs_TEXCOORD7.xy / vs_TEXCOORD7.zz;
    t16_18 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_18<0.0;
    t16_18 = (-t16_18) * 2.5 + 1.0;
    t16_18 = clamp(t16_18, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_18;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1.x = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1.x + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = vs_TEXCOORD5.y;
    t16_0.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = t16_1.y * t16_1.y;
    t16_0.x = t16_1.x * t16_1.x + (-t16_0.x);
    t16_3.xy = vs_TEXCOORD5.zw;
    t16_3.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_2.xyz, t16_3.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_3.x = dot(unity_SHBr, t16_2);
    t16_3.y = dot(unity_SHBg, t16_2);
    t16_3.z = dot(unity_SHBb, t16_2);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_3.xyz;
    t16_1.w = 1.0;
    t16_3.x = dot(unity_SHAr, t16_1);
    t16_3.y = dot(unity_SHAg, t16_1);
    t16_3.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_3.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t16_3.x = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_3.x = t16_3.x + t16_3.x;
    t16_3.xyz = t16_1.xyz * (-t16_3.xxx) + vs_TEXCOORD3.xyz;
    t16_3.x = dot(t16_3.xyz, (-t16_1.xyz));
    t16_3.y = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_3.xy = max(t16_3.xy, vec2(0.25, 0.0));
    t16_3.x = t16_3.x * _PropGlossiness.w;
    t16_3.x = t16_3.x * 10.0;
    t16_3.xzw = t16_3.xxx * _PropGlossiness.xyz;
    t16_3.xzw = t16_0.xyz * t16_3.xzw;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_3.xzw = t10_1.www * t16_3.xzw;
    t16_3.xzw = t10_1.xyz * t16_3.xzw;
    t16_0.xyz = t16_0.xyz * t10_1.xyz;
    t16_4.x = (-t10_1.w) + 1.0;
    t16_0.xyz = t16_0.xyz * t16_4.xxx;
    t10_5 = (-_LightDir.w) + 1.0;
    t16_9 = t16_3.y * _LightDir.w + t10_5;
    t16_4.x = t16_9 * 1.5;
    t16_4.x = min(t16_4.x, 1.0);
    t16_10.xyz = vec3(t16_9) + (-_CharacterShadowColor.xyz);
    t16_9 = t16_9 * _LightParameters.y;
    t16_4.xyz = t16_4.xxx * t16_10.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_4.xyz + t16_3.xzw;
    t16_3.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xzw = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_9) * t16_3.xzw + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_18) + t16_0.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_OFF" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_20;
  lowp float tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_21 = tmpvar_22;
  lowp vec3 tmpvar_23;
  tmpvar_23 = vec3(tmpvar_21);
  atten_20 = tmpvar_23;
  lit_color_7 = (lit_color_7 * max ((_Shadows.x + 
    (atten_20 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_24;
  tmpvar_24 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_24;
  c_6.w = _RimColor.w;
  lowp vec4 _color_25;
  _color_25 = (c_6 * _FadeColor);
  c_6.w = _color_25.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_25.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_OFF" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_20;
  lowp float tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_21 = tmpvar_22;
  lowp vec3 tmpvar_23;
  tmpvar_23 = vec3(tmpvar_21);
  atten_20 = tmpvar_23;
  lit_color_7 = (lit_color_7 * max ((_Shadows.x + 
    (atten_20 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_24;
  tmpvar_24 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_24;
  c_6.w = _RimColor.w;
  lowp vec4 _color_25;
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_27;
  tmpvar_27 = (_FadeColor.w - ((0.9375 * tmpvar_26.y) + 0.03125));
  mediump float tmpvar_28;
  tmpvar_28 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_27)
  )), 0.0, 1.0);
  _color_25.xyz = (_FadeColor.xyz * (tmpvar_28 + c_6.xyz));
  _color_25.w = ((32.0 * tmpvar_27) + 0.9375);
  c_6.w = _color_25.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_25.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_OFF" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  highp vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  mediump vec2 tmpvar_10;
  mediump vec3 tmpvar_11;
  mediump vec4 tmpvar_12;
  tmpvar_10.x = 0.0;
  tmpvar_7 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_7 * 0.5);
  highp vec2 tmpvar_15;
  tmpvar_15.x = tmpvar_14.x;
  tmpvar_15.y = (tmpvar_14.y * _ProjectionParams.x);
  o_13.xy = (tmpvar_15 + tmpvar_14.w);
  o_13.zw = tmpvar_7.zw;
  tmpvar_11 = o_13.xyw;
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_8.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_9.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_9.zw = tmpvar_1.xy;
  highp vec3 tmpvar_16;
  highp vec4 tmpvar_17;
  tmpvar_17 = (_Object2World * _glesVertex);
  tmpvar_16 = normalize((_WorldSpaceCameraPos - tmpvar_17.xyz));
  worldViewDir_6 = tmpvar_16;
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_18 * _glesNormal));
  worldNormal_5 = tmpvar_19;
  highp mat3 tmpvar_20;
  tmpvar_20[0] = _Object2World[0].xyz;
  tmpvar_20[1] = _Object2World[1].xyz;
  tmpvar_20[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_21;
  mediump vec3 tmpvar_22;
  tmpvar_22 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_23;
  tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_23;
  mediump vec4 tmpvar_24;
  tmpvar_24.xyz = worldViewDir_6;
  tmpvar_24.w = worldBinormal_3.x;
  mediump vec4 tmpvar_25;
  tmpvar_25.xyz = worldNormal_5;
  tmpvar_25.w = worldBinormal_3.y;
  mediump vec4 tmpvar_26;
  tmpvar_26.xyz = worldTangent_4;
  tmpvar_26.w = worldBinormal_3.z;
  tmpvar_12 = (unity_World2Shadow[0] * tmpvar_17);
  highp float tmpvar_27;
  highp vec3 x_28;
  x_28 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_27 = sqrt(dot (x_28, x_28));
  fogpos_2 = tmpvar_27;
  tmpvar_10.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_10.x = (1.0 - tmpvar_10.y);
  gl_Position = tmpvar_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = tmpvar_10;
  xlv_TEXCOORD3 = tmpvar_24;
  xlv_TEXCOORD4 = tmpvar_25;
  xlv_TEXCOORD5 = tmpvar_26;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_11;
  xlv_TEXCOORD8 = tmpvar_12;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_20;
  lowp float tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_21 = tmpvar_22;
  lowp vec3 tmpvar_23;
  tmpvar_23 = vec3(tmpvar_21);
  atten_20 = tmpvar_23;
  lit_color_7 = (lit_color_7 * max ((_Shadows.x + 
    (atten_20 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_24;
  tmpvar_24 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_24;
  c_6.w = _RimColor.w;
  lowp vec4 _color_25;
  _color_25.w = c_6.w;
  mediump float rTest_26;
  mediump vec2 tmpvar_27;
  tmpvar_27 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_28;
  tmpvar_28 = ((1.5 * _FadeColor.w) - tmpvar_27.y);
  if ((tmpvar_28 < 0.0)) {
    discard;
  };
  mediump float tmpvar_29;
  tmpvar_29 = clamp ((1.0 - (tmpvar_28 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_30;
  tmpvar_30.x = tmpvar_27.x;
  tmpvar_30.y = clamp ((tmpvar_29 - tmpvar_27.y), 0.0, 1.0);
  lowp float tmpvar_31;
  tmpvar_31 = texture2D (_RasterTex, tmpvar_30).z;
  rTest_26 = tmpvar_31;
  mediump float x_32;
  x_32 = (rTest_26 - 0.4);
  if ((x_32 < 0.0)) {
    discard;
  };
  _color_25.xyz = (c_6.xyz + (_FadeColor.xyz * tmpvar_29));
  c_6.w = _color_25.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_25.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_20;
  lowp float shadow_21;
  shadow_21 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_22;
  tmpvar_22 = vec3(shadow_21);
  atten_20 = tmpvar_22;
  lit_color_7 = (lit_color_7 * max ((_Shadows.x + 
    (atten_20 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_23;
  tmpvar_23 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_23;
  c_6.w = _RimColor.w;
  lowp vec4 _color_24;
  _color_24 = (c_6 * _FadeColor);
  c_6.w = _color_24.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_24.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "SIMPLE_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1 = t0.yyyy * unity_World2Shadow[0][1];
    t1 = unity_World2Shadow[0][0] * t0.xxxx + t1;
    t1 = unity_World2Shadow[0][2] * t0.zzzz + t1;
    t0 = unity_World2Shadow[0][3] * t0.wwww + t1;
    vs_TEXCOORD8 = t0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
mediump vec3 t16_5;
mediump float t16_8;
mediump float t16_15;
mediump float t16_18;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_15 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_15 = t16_15 + t16_15;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_15)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_4.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_5.x = t16_0.y * _PropGlossiness.w;
    t16_5.x = t16_5.x * 10.0;
    t16_5.xyz = t16_5.xxx * _PropGlossiness.xyz;
    t16_5.xyz = t16_3.xyz * t16_5.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_5.xyz = t16_5.xyz * t10_1.www;
    t16_5.xyz = t10_1.xyz * t16_5.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_18 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_18) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_18 = t16_0.x * 1.5;
    t16_18 = min(t16_18, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_18) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_5.xyz = t16_3.xyz * t16_4.xyz + t16_5.xyz;
    vec3 txVec24 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_3.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec24, 0.0);
    t16_8 = (-_LightShadowData.x) + 1.0;
    t16_3.x = t16_3.x * t16_8 + _LightShadowData.x;
    t16_3.x = t16_3.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_3.x = max(t16_3.x, 0.0);
    t16_5.xyz = t16_5.xyz * t16_3.xxx;
    t16_3.x = dot(t16_5.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_5.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_5.xyz;
    t10_2.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t10_2.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w * _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_20;
  lowp float shadow_21;
  shadow_21 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_22;
  tmpvar_22 = vec3(shadow_21);
  atten_20 = tmpvar_22;
  lit_color_7 = (lit_color_7 * max ((_Shadows.x + 
    (atten_20 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_23;
  tmpvar_23 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_23;
  c_6.w = _RimColor.w;
  lowp vec4 _color_24;
  lowp vec4 tmpvar_25;
  tmpvar_25 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_26;
  tmpvar_26 = (_FadeColor.w - ((0.9375 * tmpvar_25.y) + 0.03125));
  mediump float tmpvar_27;
  tmpvar_27 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_26)
  )), 0.0, 1.0);
  _color_24.xyz = (_FadeColor.xyz * (tmpvar_27 + c_6.xyz));
  _color_24.w = ((32.0 * tmpvar_26) + 0.9375);
  c_6.w = _color_24.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_24.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "FANCY_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1 = t0.yyyy * unity_World2Shadow[0][1];
    t1 = unity_World2Shadow[0][0] * t0.xxxx + t1;
    t1 = unity_World2Shadow[0][2] * t0.zzzz + t1;
    t0 = unity_World2Shadow[0][3] * t0.wwww + t1;
    vs_TEXCOORD8 = t0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _NoiseTexture;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
mediump vec3 t16_6;
mediump float t16_9;
mediump float t16_18;
mediump float t16_21;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_18 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_18 = t16_18 + t16_18;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_18)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_4.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_6.x = t16_0.y * _PropGlossiness.w;
    t16_6.x = t16_6.x * 10.0;
    t16_6.xyz = t16_6.xxx * _PropGlossiness.xyz;
    t16_6.xyz = t16_3.xyz * t16_6.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_6.xyz = t16_6.xyz * t10_1.www;
    t16_6.xyz = t10_1.xyz * t16_6.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_21 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_21) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_21 = t16_0.x * 1.5;
    t16_21 = min(t16_21, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_21) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_3.xyz * t16_4.xyz + t16_6.xyz;
    vec3 txVec28 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_3.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec28, 0.0);
    t16_9 = (-_LightShadowData.x) + 1.0;
    t16_3.x = t16_3.x * t16_9 + _LightShadowData.x;
    t16_3.x = t16_3.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_3.x = max(t16_3.x, 0.0);
    t16_6.xyz = t16_6.xyz * t16_3.xxx;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t10_5 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_18 = t10_5 * 0.9375 + 0.03125;
    t16_18 = (-t16_18) + _FadeColor.w;
    t16_3.x = -abs(t16_18) * 32.0 + 1.0;
    t16_1.w = t16_18 * 32.0 + 0.9375;
    t16_18 = max(t16_3.x, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_18);
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0 = t16_1;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  highp vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  mediump vec2 tmpvar_10;
  mediump vec3 tmpvar_11;
  mediump vec4 tmpvar_12;
  tmpvar_10.x = 0.0;
  tmpvar_7 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_7 * 0.5);
  highp vec2 tmpvar_15;
  tmpvar_15.x = tmpvar_14.x;
  tmpvar_15.y = (tmpvar_14.y * _ProjectionParams.x);
  o_13.xy = (tmpvar_15 + tmpvar_14.w);
  o_13.zw = tmpvar_7.zw;
  tmpvar_11 = o_13.xyw;
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_8.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_9.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_9.zw = tmpvar_1.xy;
  highp vec3 tmpvar_16;
  highp vec4 tmpvar_17;
  tmpvar_17 = (_Object2World * _glesVertex);
  tmpvar_16 = normalize((_WorldSpaceCameraPos - tmpvar_17.xyz));
  worldViewDir_6 = tmpvar_16;
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_18 * _glesNormal));
  worldNormal_5 = tmpvar_19;
  highp mat3 tmpvar_20;
  tmpvar_20[0] = _Object2World[0].xyz;
  tmpvar_20[1] = _Object2World[1].xyz;
  tmpvar_20[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_21;
  mediump vec3 tmpvar_22;
  tmpvar_22 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_23;
  tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_23;
  mediump vec4 tmpvar_24;
  tmpvar_24.xyz = worldViewDir_6;
  tmpvar_24.w = worldBinormal_3.x;
  mediump vec4 tmpvar_25;
  tmpvar_25.xyz = worldNormal_5;
  tmpvar_25.w = worldBinormal_3.y;
  mediump vec4 tmpvar_26;
  tmpvar_26.xyz = worldTangent_4;
  tmpvar_26.w = worldBinormal_3.z;
  tmpvar_12 = (unity_World2Shadow[0] * tmpvar_17);
  highp float tmpvar_27;
  highp vec3 x_28;
  x_28 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_27 = sqrt(dot (x_28, x_28));
  fogpos_2 = tmpvar_27;
  tmpvar_10.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_10.x = (1.0 - tmpvar_10.y);
  gl_Position = tmpvar_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = tmpvar_10;
  xlv_TEXCOORD3 = tmpvar_24;
  xlv_TEXCOORD4 = tmpvar_25;
  xlv_TEXCOORD5 = tmpvar_26;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_11;
  xlv_TEXCOORD8 = tmpvar_12;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_20;
  lowp float shadow_21;
  shadow_21 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_22;
  tmpvar_22 = vec3(shadow_21);
  atten_20 = tmpvar_22;
  lit_color_7 = (lit_color_7 * max ((_Shadows.x + 
    (atten_20 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_23;
  tmpvar_23 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_23;
  c_6.w = _RimColor.w;
  lowp vec4 _color_24;
  _color_24.w = c_6.w;
  mediump float rTest_25;
  mediump vec2 tmpvar_26;
  tmpvar_26 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_27;
  tmpvar_27 = ((1.5 * _FadeColor.w) - tmpvar_26.y);
  if ((tmpvar_27 < 0.0)) {
    discard;
  };
  mediump float tmpvar_28;
  tmpvar_28 = clamp ((1.0 - (tmpvar_27 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_29;
  tmpvar_29.x = tmpvar_26.x;
  tmpvar_29.y = clamp ((tmpvar_28 - tmpvar_26.y), 0.0, 1.0);
  lowp float tmpvar_30;
  tmpvar_30 = texture2D (_RasterTex, tmpvar_29).z;
  rTest_25 = tmpvar_30;
  mediump float x_31;
  x_31 = (rTest_25 - 0.4);
  if ((x_31 < 0.0)) {
    discard;
  };
  _color_24.xyz = (c_6.xyz + (_FadeColor.xyz * tmpvar_28));
  c_6.w = _color_24.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_24.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "RASTER_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD7;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_2;
highp vec4 t3;
mediump vec3 t16_4;
highp vec3 t5;
mediump float t16_8;
highp float t18;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t1.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t1;
    t1.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t1.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t1;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = sqrt(t18);
    t16_2 = (-t18) + acid_Fog.y;
    t16_8 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_8;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_8 = t16_2 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_8;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = inversesqrt(t18);
    t1.xyz = vec3(t18) * t1.xyz;
    t3.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t2.xyz = vec3(t18) * t3.xyz;
    t3.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t3.xyz = vec3(t18) * t3.xyz;
    t16_4.xyz = t2.zxy * t3.yzx;
    t16_4.xyz = t2.yzx * t3.zxy + (-t16_4.xyz);
    t5.xyz = t16_4.xyz * in_TANGENT0.www;
    t18 = dot(t5.xyz, t5.xyz);
    t18 = inversesqrt(t18);
    t5.xyz = vec3(t18) * t5.xyz;
    t1.w = t5.x;
    vs_TEXCOORD3 = t1;
    t2.w = t5.y;
    t3.w = t5.z;
    vs_TEXCOORD5 = t3;
    vs_TEXCOORD4 = t2;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0.w = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t1.zz + t1.xw;
    vs_TEXCOORD7.xyz = t0.xyz;
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1 = t0.yyyy * unity_World2Shadow[0][1];
    t1 = unity_World2Shadow[0][0] * t0.xxxx + t1;
    t1 = unity_World2Shadow[0][2] * t0.zzzz + t1;
    t0 = unity_World2Shadow[0][3] * t0.wwww + t1;
    vs_TEXCOORD8 = t0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _RasterTex;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec3 vs_TEXCOORD7;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
bool tb1;
mediump vec4 t16_2;
lowp vec3 t10_2;
mediump vec4 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
mediump float t16_9;
mediump vec3 t16_10;
mediump float t16_15;
mediump float t16_18;
void main()
{
    t16_0.xy = vs_TEXCOORD7.xy / vs_TEXCOORD7.zz;
    t16_18 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_18<0.0;
    t16_18 = (-t16_18) * 2.5 + 1.0;
    t16_18 = clamp(t16_18, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_18;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1.x = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1.x + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = vs_TEXCOORD5.y;
    t16_0.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = t16_1.y * t16_1.y;
    t16_0.x = t16_1.x * t16_1.x + (-t16_0.x);
    t16_3.xy = vs_TEXCOORD5.zw;
    t16_3.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_2.xyz, t16_3.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_3.x = dot(unity_SHBr, t16_2);
    t16_3.y = dot(unity_SHBg, t16_2);
    t16_3.z = dot(unity_SHBb, t16_2);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_3.xyz;
    t16_1.w = 1.0;
    t16_3.x = dot(unity_SHAr, t16_1);
    t16_3.y = dot(unity_SHAg, t16_1);
    t16_3.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_3.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t16_3.x = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_3.x = t16_3.x + t16_3.x;
    t16_3.xyz = t16_1.xyz * (-t16_3.xxx) + vs_TEXCOORD3.xyz;
    t16_3.x = dot(t16_3.xyz, (-t16_1.xyz));
    t16_3.y = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_3.xy = max(t16_3.xy, vec2(0.25, 0.0));
    t16_3.x = t16_3.x * _PropGlossiness.w;
    t16_3.x = t16_3.x * 10.0;
    t16_3.xzw = t16_3.xxx * _PropGlossiness.xyz;
    t16_3.xzw = t16_0.xyz * t16_3.xzw;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_3.xzw = t10_1.www * t16_3.xzw;
    t16_3.xzw = t10_1.xyz * t16_3.xzw;
    t16_0.xyz = t16_0.xyz * t10_1.xyz;
    t16_4.x = (-t10_1.w) + 1.0;
    t16_0.xyz = t16_0.xyz * t16_4.xxx;
    t10_5 = (-_LightDir.w) + 1.0;
    t16_9 = t16_3.y * _LightDir.w + t10_5;
    t16_4.x = t16_9 * 1.5;
    t16_4.x = min(t16_4.x, 1.0);
    t16_10.xyz = vec3(t16_9) + (-_CharacterShadowColor.xyz);
    t16_9 = t16_9 * _LightParameters.y;
    t16_4.xyz = t16_4.xxx * t16_10.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_4.xyz + t16_3.xzw;
    vec3 txVec25 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_3.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec25, 0.0);
    t16_15 = (-_LightShadowData.x) + 1.0;
    t16_3.x = t16_3.x * t16_15 + _LightShadowData.x;
    t16_3.x = t16_3.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_3.x = max(t16_3.x, 0.0);
    t16_0.xyz = t16_0.xyz * t16_3.xxx;
    t16_3.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xzw = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_9) * t16_3.xzw + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_18) + t16_0.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_20;
  tmpvar_20 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_20;
  c_6.w = _RimColor.w;
  lowp vec4 _color_21;
  _color_21 = (c_6 * _FadeColor);
  lowp vec4 _color_22;
  _color_22.w = _color_21.w;
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_24;
  tmpvar_24 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_23.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_22.xyz = (_NoiseGlow.xyz * (tmpvar_24 + _color_21.xyz));
  c_6.w = _color_22.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_22.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" "SIMPLE_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _NoiseTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
mediump vec3 t16_6;
mediump float t16_18;
mediump float t16_21;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_18 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_18 = t16_18 + t16_18;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_18)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_4.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_6.x = t16_0.y * _PropGlossiness.w;
    t16_6.x = t16_6.x * 10.0;
    t16_6.xyz = t16_6.xxx * _PropGlossiness.xyz;
    t16_6.xyz = t16_3.xyz * t16_6.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_6.xyz = t16_6.xyz * t10_1.www;
    t16_6.xyz = t10_1.xyz * t16_6.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_21 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_21) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_21 = t16_0.x * 1.5;
    t16_21 = min(t16_21, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_21) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_3.xyz * t16_4.xyz + t16_6.xyz;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t10_5 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_18 = t10_5 * 0.9375 + 0.03125;
    t16_18 = (-t16_18) + _NoiseGlow.w;
    t16_18 = -abs(t16_18) * 32.0 + 1.0;
    t16_18 = max(t16_18, 0.0);
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz + vec3(t16_18);
    t16_0.xyz = t16_0.xyz * _NoiseGlow.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w * _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_20;
  tmpvar_20 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_20;
  c_6.w = _RimColor.w;
  lowp vec4 _color_21;
  lowp vec4 tmpvar_22;
  tmpvar_22 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_23;
  tmpvar_23 = (_FadeColor.w - ((0.9375 * tmpvar_22.y) + 0.03125));
  mediump float tmpvar_24;
  tmpvar_24 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_23)
  )), 0.0, 1.0);
  _color_21.xyz = (_FadeColor.xyz * (tmpvar_24 + c_6.xyz));
  _color_21.w = ((32.0 * tmpvar_23) + 0.9375);
  lowp vec4 _color_25;
  _color_25.w = _color_21.w;
  mediump float tmpvar_26;
  tmpvar_26 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_22.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_25.xyz = (_NoiseGlow.xyz * (tmpvar_26 + _color_21.xyz));
  c_6.w = _color_25.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_25.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" "FANCY_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _NoiseTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
mediump vec3 t16_6;
mediump float t16_9;
mediump float t16_18;
mediump float t16_21;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_18 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_18 = t16_18 + t16_18;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_18)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_4.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_6.x = t16_0.y * _PropGlossiness.w;
    t16_6.x = t16_6.x * 10.0;
    t16_6.xyz = t16_6.xxx * _PropGlossiness.xyz;
    t16_6.xyz = t16_3.xyz * t16_6.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_6.xyz = t16_6.xyz * t10_1.www;
    t16_6.xyz = t10_1.xyz * t16_6.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_21 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_21) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_21 = t16_0.x * 1.5;
    t16_21 = min(t16_21, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_21) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_3.xyz * t16_4.xyz + t16_6.xyz;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t10_5 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_18 = t10_5 * 0.9375 + 0.03125;
    t16_3.x = (-t16_18) + _FadeColor.w;
    t16_18 = (-t16_18) + _NoiseGlow.w;
    t16_18 = -abs(t16_18) * 32.0 + 1.0;
    t16_18 = max(t16_18, 0.0);
    t16_9 = -abs(t16_3.x) * 32.0 + 1.0;
    t16_1.w = t16_3.x * 32.0 + 0.9375;
    t16_3.x = max(t16_9, 0.0);
    t16_0.xyz = t16_0.xyz + t16_3.xxx;
    t16_0.xyz = _FadeColor.xyz * t16_0.xyz + vec3(t16_18);
    t16_0.xyz = t16_0.xyz * _NoiseGlow.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0 = t16_1;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  highp vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  mediump vec2 tmpvar_10;
  mediump vec3 tmpvar_11;
  tmpvar_10.x = 0.0;
  tmpvar_7 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = (tmpvar_7 * 0.5);
  highp vec2 tmpvar_14;
  tmpvar_14.x = tmpvar_13.x;
  tmpvar_14.y = (tmpvar_13.y * _ProjectionParams.x);
  o_12.xy = (tmpvar_14 + tmpvar_13.w);
  o_12.zw = tmpvar_7.zw;
  tmpvar_11 = o_12.xyw;
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_8.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_9.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_9.zw = tmpvar_1.xy;
  highp vec3 tmpvar_15;
  tmpvar_15 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_15;
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _Object2World[0].xyz;
  tmpvar_16[1] = _Object2World[1].xyz;
  tmpvar_16[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_16 * _glesNormal));
  worldNormal_5 = tmpvar_17;
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_18 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_19;
  mediump vec3 tmpvar_20;
  tmpvar_20 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22.xyz = worldViewDir_6;
  tmpvar_22.w = worldBinormal_3.x;
  mediump vec4 tmpvar_23;
  tmpvar_23.xyz = worldNormal_5;
  tmpvar_23.w = worldBinormal_3.y;
  mediump vec4 tmpvar_24;
  tmpvar_24.xyz = worldTangent_4;
  tmpvar_24.w = worldBinormal_3.z;
  highp float tmpvar_25;
  highp vec3 x_26;
  x_26 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_25 = sqrt(dot (x_26, x_26));
  fogpos_2 = tmpvar_25;
  tmpvar_10.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_10.x = (1.0 - tmpvar_10.y);
  gl_Position = tmpvar_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = tmpvar_10;
  xlv_TEXCOORD3 = tmpvar_22;
  xlv_TEXCOORD4 = tmpvar_23;
  xlv_TEXCOORD5 = tmpvar_24;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_11;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_20;
  tmpvar_20 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_20;
  c_6.w = _RimColor.w;
  lowp vec4 _color_21;
  _color_21.w = c_6.w;
  mediump float rTest_22;
  mediump vec2 tmpvar_23;
  tmpvar_23 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_24;
  tmpvar_24 = ((1.5 * _FadeColor.w) - tmpvar_23.y);
  if ((tmpvar_24 < 0.0)) {
    discard;
  };
  mediump float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (tmpvar_24 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_26;
  tmpvar_26.x = tmpvar_23.x;
  tmpvar_26.y = clamp ((tmpvar_25 - tmpvar_23.y), 0.0, 1.0);
  lowp float tmpvar_27;
  tmpvar_27 = texture2D (_RasterTex, tmpvar_26).z;
  rTest_22 = tmpvar_27;
  mediump float x_28;
  x_28 = (rTest_22 - 0.4);
  if ((x_28 < 0.0)) {
    discard;
  };
  _color_21.xyz = (c_6.xyz + (_FadeColor.xyz * tmpvar_25));
  lowp vec4 _color_29;
  _color_29.w = _color_21.w;
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_31;
  tmpvar_31 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_30.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_29.xyz = (_NoiseGlow.xyz * (tmpvar_31 + _color_21.xyz));
  c_6.w = _color_29.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_29.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" "RASTER_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD7;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_2;
highp vec4 t3;
mediump vec3 t16_4;
highp vec3 t5;
mediump float t16_8;
highp float t18;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t1.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t1;
    t1.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t1.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t1;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = sqrt(t18);
    t16_2 = (-t18) + acid_Fog.y;
    t16_8 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_8;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_8 = t16_2 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_8;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = inversesqrt(t18);
    t1.xyz = vec3(t18) * t1.xyz;
    t3.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t2.xyz = vec3(t18) * t3.xyz;
    t3.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t3.xyz = vec3(t18) * t3.xyz;
    t16_4.xyz = t2.zxy * t3.yzx;
    t16_4.xyz = t2.yzx * t3.zxy + (-t16_4.xyz);
    t5.xyz = t16_4.xyz * in_TANGENT0.www;
    t18 = dot(t5.xyz, t5.xyz);
    t18 = inversesqrt(t18);
    t5.xyz = vec3(t18) * t5.xyz;
    t1.w = t5.x;
    vs_TEXCOORD3 = t1;
    t2.w = t5.y;
    t3.w = t5.z;
    vs_TEXCOORD5 = t3;
    vs_TEXCOORD4 = t2;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0.w = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t1.zz + t1.xw;
    vs_TEXCOORD7.xyz = t0.xyz;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _RasterTex;
uniform lowp sampler2D _NoiseTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec3 vs_TEXCOORD7;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
bool tb1;
mediump vec4 t16_2;
lowp vec3 t10_2;
mediump vec4 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
lowp float t10_6;
mediump float t16_10;
mediump vec3 t16_11;
mediump float t16_21;
void main()
{
    t16_0.xy = vs_TEXCOORD7.xy / vs_TEXCOORD7.zz;
    t16_21 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_21<0.0;
    t16_21 = (-t16_21) * 2.5 + 1.0;
    t16_21 = clamp(t16_21, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_21;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1.x = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1.x + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = vs_TEXCOORD5.y;
    t16_0.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = t16_1.y * t16_1.y;
    t16_0.x = t16_1.x * t16_1.x + (-t16_0.x);
    t16_3.xy = vs_TEXCOORD5.zw;
    t16_3.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_2.xyz, t16_3.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_3.x = dot(unity_SHBr, t16_2);
    t16_3.y = dot(unity_SHBg, t16_2);
    t16_3.z = dot(unity_SHBb, t16_2);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_3.xyz;
    t16_1.w = 1.0;
    t16_3.x = dot(unity_SHAr, t16_1);
    t16_3.y = dot(unity_SHAg, t16_1);
    t16_3.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_3.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t16_3.x = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_3.x = t16_3.x + t16_3.x;
    t16_3.xyz = t16_1.xyz * (-t16_3.xxx) + vs_TEXCOORD3.xyz;
    t16_3.x = dot(t16_3.xyz, (-t16_1.xyz));
    t16_3.y = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_3.xy = max(t16_3.xy, vec2(0.25, 0.0));
    t16_3.x = t16_3.x * _PropGlossiness.w;
    t16_3.x = t16_3.x * 10.0;
    t16_3.xzw = t16_3.xxx * _PropGlossiness.xyz;
    t16_3.xzw = t16_0.xyz * t16_3.xzw;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_3.xzw = t10_1.www * t16_3.xzw;
    t16_3.xzw = t10_1.xyz * t16_3.xzw;
    t16_0.xyz = t16_0.xyz * t10_1.xyz;
    t16_4.x = (-t10_1.w) + 1.0;
    t16_0.xyz = t16_0.xyz * t16_4.xxx;
    t10_5 = (-_LightDir.w) + 1.0;
    t16_10 = t16_3.y * _LightDir.w + t10_5;
    t16_4.x = t16_10 * 1.5;
    t16_4.x = min(t16_4.x, 1.0);
    t16_11.xyz = vec3(t16_10) + (-_CharacterShadowColor.xyz);
    t16_10 = t16_10 * _LightParameters.y;
    t16_4.xyz = t16_4.xxx * t16_11.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_4.xyz + t16_3.xzw;
    t16_3.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xzw = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_10) * t16_3.xzw + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_21) + t16_0.xyz;
    t10_6 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_21 = t10_6 * 0.9375 + 0.03125;
    t16_21 = (-t16_21) + _NoiseGlow.w;
    t16_21 = -abs(t16_21) * 32.0 + 1.0;
    t16_21 = max(t16_21, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_21);
    t16_0.xyz = t16_0.xyz * _NoiseGlow.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_20;
  tmpvar_20 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_20;
  c_6.w = _RimColor.w;
  lowp vec4 _color_21;
  _color_21 = (c_6 * _FadeColor);
  lowp vec4 _color_22;
  _color_22.w = _color_21.w;
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_24;
  tmpvar_24 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_23.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_22.xyz = (_NoiseGlow.xyz * (tmpvar_24 + _color_21.xyz));
  c_6.w = _color_22.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_22.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" "SIMPLE_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _NoiseTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
mediump vec3 t16_6;
mediump float t16_18;
mediump float t16_21;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_18 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_18 = t16_18 + t16_18;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_18)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_4.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_6.x = t16_0.y * _PropGlossiness.w;
    t16_6.x = t16_6.x * 10.0;
    t16_6.xyz = t16_6.xxx * _PropGlossiness.xyz;
    t16_6.xyz = t16_3.xyz * t16_6.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_6.xyz = t16_6.xyz * t10_1.www;
    t16_6.xyz = t10_1.xyz * t16_6.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_21 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_21) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_21 = t16_0.x * 1.5;
    t16_21 = min(t16_21, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_21) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_3.xyz * t16_4.xyz + t16_6.xyz;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t10_5 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_18 = t10_5 * 0.9375 + 0.03125;
    t16_18 = (-t16_18) + _NoiseGlow.w;
    t16_18 = -abs(t16_18) * 32.0 + 1.0;
    t16_18 = max(t16_18, 0.0);
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz + vec3(t16_18);
    t16_0.xyz = t16_0.xyz * _NoiseGlow.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w * _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_20;
  tmpvar_20 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_20;
  c_6.w = _RimColor.w;
  lowp vec4 _color_21;
  lowp vec4 tmpvar_22;
  tmpvar_22 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_23;
  tmpvar_23 = (_FadeColor.w - ((0.9375 * tmpvar_22.y) + 0.03125));
  mediump float tmpvar_24;
  tmpvar_24 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_23)
  )), 0.0, 1.0);
  _color_21.xyz = (_FadeColor.xyz * (tmpvar_24 + c_6.xyz));
  _color_21.w = ((32.0 * tmpvar_23) + 0.9375);
  lowp vec4 _color_25;
  _color_25.w = _color_21.w;
  mediump float tmpvar_26;
  tmpvar_26 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_22.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_25.xyz = (_NoiseGlow.xyz * (tmpvar_26 + _color_21.xyz));
  c_6.w = _color_25.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_25.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" "FANCY_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _NoiseTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
mediump vec3 t16_6;
mediump float t16_9;
mediump float t16_18;
mediump float t16_21;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_18 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_18 = t16_18 + t16_18;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_18)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_4.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_6.x = t16_0.y * _PropGlossiness.w;
    t16_6.x = t16_6.x * 10.0;
    t16_6.xyz = t16_6.xxx * _PropGlossiness.xyz;
    t16_6.xyz = t16_3.xyz * t16_6.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_6.xyz = t16_6.xyz * t10_1.www;
    t16_6.xyz = t10_1.xyz * t16_6.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_21 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_21) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_21 = t16_0.x * 1.5;
    t16_21 = min(t16_21, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_21) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_3.xyz * t16_4.xyz + t16_6.xyz;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t10_5 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_18 = t10_5 * 0.9375 + 0.03125;
    t16_3.x = (-t16_18) + _FadeColor.w;
    t16_18 = (-t16_18) + _NoiseGlow.w;
    t16_18 = -abs(t16_18) * 32.0 + 1.0;
    t16_18 = max(t16_18, 0.0);
    t16_9 = -abs(t16_3.x) * 32.0 + 1.0;
    t16_1.w = t16_3.x * 32.0 + 0.9375;
    t16_3.x = max(t16_9, 0.0);
    t16_0.xyz = t16_0.xyz + t16_3.xxx;
    t16_0.xyz = _FadeColor.xyz * t16_0.xyz + vec3(t16_18);
    t16_0.xyz = t16_0.xyz * _NoiseGlow.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0 = t16_1;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  highp vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  mediump vec2 tmpvar_10;
  mediump vec3 tmpvar_11;
  tmpvar_10.x = 0.0;
  tmpvar_7 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = (tmpvar_7 * 0.5);
  highp vec2 tmpvar_14;
  tmpvar_14.x = tmpvar_13.x;
  tmpvar_14.y = (tmpvar_13.y * _ProjectionParams.x);
  o_12.xy = (tmpvar_14 + tmpvar_13.w);
  o_12.zw = tmpvar_7.zw;
  tmpvar_11 = o_12.xyw;
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_8.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_9.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_9.zw = tmpvar_1.xy;
  highp vec3 tmpvar_15;
  tmpvar_15 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_15;
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _Object2World[0].xyz;
  tmpvar_16[1] = _Object2World[1].xyz;
  tmpvar_16[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_16 * _glesNormal));
  worldNormal_5 = tmpvar_17;
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_18 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_19;
  mediump vec3 tmpvar_20;
  tmpvar_20 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22.xyz = worldViewDir_6;
  tmpvar_22.w = worldBinormal_3.x;
  mediump vec4 tmpvar_23;
  tmpvar_23.xyz = worldNormal_5;
  tmpvar_23.w = worldBinormal_3.y;
  mediump vec4 tmpvar_24;
  tmpvar_24.xyz = worldTangent_4;
  tmpvar_24.w = worldBinormal_3.z;
  highp float tmpvar_25;
  highp vec3 x_26;
  x_26 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_25 = sqrt(dot (x_26, x_26));
  fogpos_2 = tmpvar_25;
  tmpvar_10.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_10.x = (1.0 - tmpvar_10.y);
  gl_Position = tmpvar_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = tmpvar_10;
  xlv_TEXCOORD3 = tmpvar_22;
  xlv_TEXCOORD4 = tmpvar_23;
  xlv_TEXCOORD5 = tmpvar_24;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_11;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_20;
  tmpvar_20 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_20;
  c_6.w = _RimColor.w;
  lowp vec4 _color_21;
  _color_21.w = c_6.w;
  mediump float rTest_22;
  mediump vec2 tmpvar_23;
  tmpvar_23 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_24;
  tmpvar_24 = ((1.5 * _FadeColor.w) - tmpvar_23.y);
  if ((tmpvar_24 < 0.0)) {
    discard;
  };
  mediump float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (tmpvar_24 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_26;
  tmpvar_26.x = tmpvar_23.x;
  tmpvar_26.y = clamp ((tmpvar_25 - tmpvar_23.y), 0.0, 1.0);
  lowp float tmpvar_27;
  tmpvar_27 = texture2D (_RasterTex, tmpvar_26).z;
  rTest_22 = tmpvar_27;
  mediump float x_28;
  x_28 = (rTest_22 - 0.4);
  if ((x_28 < 0.0)) {
    discard;
  };
  _color_21.xyz = (c_6.xyz + (_FadeColor.xyz * tmpvar_25));
  lowp vec4 _color_29;
  _color_29.w = _color_21.w;
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_31;
  tmpvar_31 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_30.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_29.xyz = (_NoiseGlow.xyz * (tmpvar_31 + _color_21.xyz));
  c_6.w = _color_29.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_29.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" "RASTER_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD7;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_2;
highp vec4 t3;
mediump vec3 t16_4;
highp vec3 t5;
mediump float t16_8;
highp float t18;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t1.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t1;
    t1.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t1.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t1;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = sqrt(t18);
    t16_2 = (-t18) + acid_Fog.y;
    t16_8 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_8;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_8 = t16_2 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_8;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = inversesqrt(t18);
    t1.xyz = vec3(t18) * t1.xyz;
    t3.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t2.xyz = vec3(t18) * t3.xyz;
    t3.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t3.xyz = vec3(t18) * t3.xyz;
    t16_4.xyz = t2.zxy * t3.yzx;
    t16_4.xyz = t2.yzx * t3.zxy + (-t16_4.xyz);
    t5.xyz = t16_4.xyz * in_TANGENT0.www;
    t18 = dot(t5.xyz, t5.xyz);
    t18 = inversesqrt(t18);
    t5.xyz = vec3(t18) * t5.xyz;
    t1.w = t5.x;
    vs_TEXCOORD3 = t1;
    t2.w = t5.y;
    t3.w = t5.z;
    vs_TEXCOORD5 = t3;
    vs_TEXCOORD4 = t2;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0.w = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t1.zz + t1.xw;
    vs_TEXCOORD7.xyz = t0.xyz;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _RasterTex;
uniform lowp sampler2D _NoiseTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec3 vs_TEXCOORD7;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
bool tb1;
mediump vec4 t16_2;
lowp vec3 t10_2;
mediump vec4 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
lowp float t10_6;
mediump float t16_10;
mediump vec3 t16_11;
mediump float t16_21;
void main()
{
    t16_0.xy = vs_TEXCOORD7.xy / vs_TEXCOORD7.zz;
    t16_21 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_21<0.0;
    t16_21 = (-t16_21) * 2.5 + 1.0;
    t16_21 = clamp(t16_21, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_21;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1.x = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1.x + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = vs_TEXCOORD5.y;
    t16_0.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = t16_1.y * t16_1.y;
    t16_0.x = t16_1.x * t16_1.x + (-t16_0.x);
    t16_3.xy = vs_TEXCOORD5.zw;
    t16_3.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_2.xyz, t16_3.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_3.x = dot(unity_SHBr, t16_2);
    t16_3.y = dot(unity_SHBg, t16_2);
    t16_3.z = dot(unity_SHBb, t16_2);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_3.xyz;
    t16_1.w = 1.0;
    t16_3.x = dot(unity_SHAr, t16_1);
    t16_3.y = dot(unity_SHAg, t16_1);
    t16_3.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_3.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t16_3.x = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_3.x = t16_3.x + t16_3.x;
    t16_3.xyz = t16_1.xyz * (-t16_3.xxx) + vs_TEXCOORD3.xyz;
    t16_3.x = dot(t16_3.xyz, (-t16_1.xyz));
    t16_3.y = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_3.xy = max(t16_3.xy, vec2(0.25, 0.0));
    t16_3.x = t16_3.x * _PropGlossiness.w;
    t16_3.x = t16_3.x * 10.0;
    t16_3.xzw = t16_3.xxx * _PropGlossiness.xyz;
    t16_3.xzw = t16_0.xyz * t16_3.xzw;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_3.xzw = t10_1.www * t16_3.xzw;
    t16_3.xzw = t10_1.xyz * t16_3.xzw;
    t16_0.xyz = t16_0.xyz * t10_1.xyz;
    t16_4.x = (-t10_1.w) + 1.0;
    t16_0.xyz = t16_0.xyz * t16_4.xxx;
    t10_5 = (-_LightDir.w) + 1.0;
    t16_10 = t16_3.y * _LightDir.w + t10_5;
    t16_4.x = t16_10 * 1.5;
    t16_4.x = min(t16_4.x, 1.0);
    t16_11.xyz = vec3(t16_10) + (-_CharacterShadowColor.xyz);
    t16_10 = t16_10 * _LightParameters.y;
    t16_4.xyz = t16_4.xxx * t16_11.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_4.xyz + t16_3.xzw;
    t16_3.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xzw = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_10) * t16_3.xzw + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_21) + t16_0.xyz;
    t10_6 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_21 = t10_6 * 0.9375 + 0.03125;
    t16_21 = (-t16_21) + _NoiseGlow.w;
    t16_21 = -abs(t16_21) * 32.0 + 1.0;
    t16_21 = max(t16_21, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_21);
    t16_0.xyz = t16_0.xyz * _NoiseGlow.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_NOISEGLOW" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_20;
  lowp float tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_21 = tmpvar_22;
  lowp vec3 tmpvar_23;
  tmpvar_23 = vec3(tmpvar_21);
  atten_20 = tmpvar_23;
  lit_color_7 = (lit_color_7 * max ((_Shadows.x + 
    (atten_20 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_24;
  tmpvar_24 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_24;
  c_6.w = _RimColor.w;
  lowp vec4 _color_25;
  _color_25 = (c_6 * _FadeColor);
  lowp vec4 _color_26;
  _color_26.w = _color_25.w;
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_28;
  tmpvar_28 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_27.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_26.xyz = (_NoiseGlow.xyz * (tmpvar_28 + _color_25.xyz));
  c_6.w = _color_26.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_26.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_NOISEGLOW" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_20;
  lowp float tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_21 = tmpvar_22;
  lowp vec3 tmpvar_23;
  tmpvar_23 = vec3(tmpvar_21);
  atten_20 = tmpvar_23;
  lit_color_7 = (lit_color_7 * max ((_Shadows.x + 
    (atten_20 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_24;
  tmpvar_24 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_24;
  c_6.w = _RimColor.w;
  lowp vec4 _color_25;
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_27;
  tmpvar_27 = (_FadeColor.w - ((0.9375 * tmpvar_26.y) + 0.03125));
  mediump float tmpvar_28;
  tmpvar_28 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_27)
  )), 0.0, 1.0);
  _color_25.xyz = (_FadeColor.xyz * (tmpvar_28 + c_6.xyz));
  _color_25.w = ((32.0 * tmpvar_27) + 0.9375);
  lowp vec4 _color_29;
  _color_29.w = _color_25.w;
  mediump float tmpvar_30;
  tmpvar_30 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_26.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_29.xyz = (_NoiseGlow.xyz * (tmpvar_30 + _color_25.xyz));
  c_6.w = _color_29.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_29.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_NOISEGLOW" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  highp vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  mediump vec2 tmpvar_10;
  mediump vec3 tmpvar_11;
  mediump vec4 tmpvar_12;
  tmpvar_10.x = 0.0;
  tmpvar_7 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_7 * 0.5);
  highp vec2 tmpvar_15;
  tmpvar_15.x = tmpvar_14.x;
  tmpvar_15.y = (tmpvar_14.y * _ProjectionParams.x);
  o_13.xy = (tmpvar_15 + tmpvar_14.w);
  o_13.zw = tmpvar_7.zw;
  tmpvar_11 = o_13.xyw;
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_8.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_9.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_9.zw = tmpvar_1.xy;
  highp vec3 tmpvar_16;
  highp vec4 tmpvar_17;
  tmpvar_17 = (_Object2World * _glesVertex);
  tmpvar_16 = normalize((_WorldSpaceCameraPos - tmpvar_17.xyz));
  worldViewDir_6 = tmpvar_16;
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_18 * _glesNormal));
  worldNormal_5 = tmpvar_19;
  highp mat3 tmpvar_20;
  tmpvar_20[0] = _Object2World[0].xyz;
  tmpvar_20[1] = _Object2World[1].xyz;
  tmpvar_20[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_21;
  mediump vec3 tmpvar_22;
  tmpvar_22 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_23;
  tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_23;
  mediump vec4 tmpvar_24;
  tmpvar_24.xyz = worldViewDir_6;
  tmpvar_24.w = worldBinormal_3.x;
  mediump vec4 tmpvar_25;
  tmpvar_25.xyz = worldNormal_5;
  tmpvar_25.w = worldBinormal_3.y;
  mediump vec4 tmpvar_26;
  tmpvar_26.xyz = worldTangent_4;
  tmpvar_26.w = worldBinormal_3.z;
  tmpvar_12 = (unity_World2Shadow[0] * tmpvar_17);
  highp float tmpvar_27;
  highp vec3 x_28;
  x_28 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_27 = sqrt(dot (x_28, x_28));
  fogpos_2 = tmpvar_27;
  tmpvar_10.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_10.x = (1.0 - tmpvar_10.y);
  gl_Position = tmpvar_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = tmpvar_10;
  xlv_TEXCOORD3 = tmpvar_24;
  xlv_TEXCOORD4 = tmpvar_25;
  xlv_TEXCOORD5 = tmpvar_26;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_11;
  xlv_TEXCOORD8 = tmpvar_12;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_20;
  lowp float tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_21 = tmpvar_22;
  lowp vec3 tmpvar_23;
  tmpvar_23 = vec3(tmpvar_21);
  atten_20 = tmpvar_23;
  lit_color_7 = (lit_color_7 * max ((_Shadows.x + 
    (atten_20 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_24;
  tmpvar_24 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_24;
  c_6.w = _RimColor.w;
  lowp vec4 _color_25;
  _color_25.w = c_6.w;
  mediump float rTest_26;
  mediump vec2 tmpvar_27;
  tmpvar_27 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_28;
  tmpvar_28 = ((1.5 * _FadeColor.w) - tmpvar_27.y);
  if ((tmpvar_28 < 0.0)) {
    discard;
  };
  mediump float tmpvar_29;
  tmpvar_29 = clamp ((1.0 - (tmpvar_28 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_30;
  tmpvar_30.x = tmpvar_27.x;
  tmpvar_30.y = clamp ((tmpvar_29 - tmpvar_27.y), 0.0, 1.0);
  lowp float tmpvar_31;
  tmpvar_31 = texture2D (_RasterTex, tmpvar_30).z;
  rTest_26 = tmpvar_31;
  mediump float x_32;
  x_32 = (rTest_26 - 0.4);
  if ((x_32 < 0.0)) {
    discard;
  };
  _color_25.xyz = (c_6.xyz + (_FadeColor.xyz * tmpvar_29));
  lowp vec4 _color_33;
  _color_33.w = _color_25.w;
  lowp vec4 tmpvar_34;
  tmpvar_34 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_35;
  tmpvar_35 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_34.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_33.xyz = (_NoiseGlow.xyz * (tmpvar_35 + _color_25.xyz));
  c_6.w = _color_33.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_33.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_20;
  lowp float shadow_21;
  shadow_21 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_22;
  tmpvar_22 = vec3(shadow_21);
  atten_20 = tmpvar_22;
  lit_color_7 = (lit_color_7 * max ((_Shadows.x + 
    (atten_20 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_23;
  tmpvar_23 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_23;
  c_6.w = _RimColor.w;
  lowp vec4 _color_24;
  _color_24 = (c_6 * _FadeColor);
  lowp vec4 _color_25;
  _color_25.w = _color_24.w;
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_27;
  tmpvar_27 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_26.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_25.xyz = (_NoiseGlow.xyz * (tmpvar_27 + _color_24.xyz));
  c_6.w = _color_25.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_25.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" "SIMPLE_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1 = t0.yyyy * unity_World2Shadow[0][1];
    t1 = unity_World2Shadow[0][0] * t0.xxxx + t1;
    t1 = unity_World2Shadow[0][2] * t0.zzzz + t1;
    t0 = unity_World2Shadow[0][3] * t0.wwww + t1;
    vs_TEXCOORD8 = t0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _NoiseTexture;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
mediump vec3 t16_6;
mediump float t16_9;
mediump float t16_18;
mediump float t16_21;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_18 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_18 = t16_18 + t16_18;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_18)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_4.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_6.x = t16_0.y * _PropGlossiness.w;
    t16_6.x = t16_6.x * 10.0;
    t16_6.xyz = t16_6.xxx * _PropGlossiness.xyz;
    t16_6.xyz = t16_3.xyz * t16_6.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_6.xyz = t16_6.xyz * t10_1.www;
    t16_6.xyz = t10_1.xyz * t16_6.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_21 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_21) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_21 = t16_0.x * 1.5;
    t16_21 = min(t16_21, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_21) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_3.xyz * t16_4.xyz + t16_6.xyz;
    vec3 txVec36 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_3.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec36, 0.0);
    t16_9 = (-_LightShadowData.x) + 1.0;
    t16_3.x = t16_3.x * t16_9 + _LightShadowData.x;
    t16_3.x = t16_3.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_3.x = max(t16_3.x, 0.0);
    t16_6.xyz = t16_6.xyz * t16_3.xxx;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t10_5 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_18 = t10_5 * 0.9375 + 0.03125;
    t16_18 = (-t16_18) + _NoiseGlow.w;
    t16_18 = -abs(t16_18) * 32.0 + 1.0;
    t16_18 = max(t16_18, 0.0);
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz + vec3(t16_18);
    t16_0.xyz = t16_0.xyz * _NoiseGlow.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w * _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_20;
  lowp float shadow_21;
  shadow_21 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_22;
  tmpvar_22 = vec3(shadow_21);
  atten_20 = tmpvar_22;
  lit_color_7 = (lit_color_7 * max ((_Shadows.x + 
    (atten_20 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_23;
  tmpvar_23 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_23;
  c_6.w = _RimColor.w;
  lowp vec4 _color_24;
  lowp vec4 tmpvar_25;
  tmpvar_25 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_26;
  tmpvar_26 = (_FadeColor.w - ((0.9375 * tmpvar_25.y) + 0.03125));
  mediump float tmpvar_27;
  tmpvar_27 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_26)
  )), 0.0, 1.0);
  _color_24.xyz = (_FadeColor.xyz * (tmpvar_27 + c_6.xyz));
  _color_24.w = ((32.0 * tmpvar_26) + 0.9375);
  lowp vec4 _color_28;
  _color_28.w = _color_24.w;
  mediump float tmpvar_29;
  tmpvar_29 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_25.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_28.xyz = (_NoiseGlow.xyz * (tmpvar_29 + _color_24.xyz));
  c_6.w = _color_28.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_28.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" "FANCY_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1 = t0.yyyy * unity_World2Shadow[0][1];
    t1 = unity_World2Shadow[0][0] * t0.xxxx + t1;
    t1 = unity_World2Shadow[0][2] * t0.zzzz + t1;
    t0 = unity_World2Shadow[0][3] * t0.wwww + t1;
    vs_TEXCOORD8 = t0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _NoiseTexture;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
mediump vec3 t16_6;
mediump float t16_9;
mediump float t16_18;
mediump float t16_21;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_18 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_18 = t16_18 + t16_18;
    t16_4.xyz = t16_0.xyz * (-vec3(t16_18)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_4.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_6.x = t16_0.y * _PropGlossiness.w;
    t16_6.x = t16_6.x * 10.0;
    t16_6.xyz = t16_6.xxx * _PropGlossiness.xyz;
    t16_6.xyz = t16_3.xyz * t16_6.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_6.xyz = t16_6.xyz * t10_1.www;
    t16_6.xyz = t10_1.xyz * t16_6.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_21 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_21) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_21 = t16_0.x * 1.5;
    t16_21 = min(t16_21, 1.0);
    t16_4.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_4.xyz = vec3(t16_21) * t16_4.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_3.xyz * t16_4.xyz + t16_6.xyz;
    vec3 txVec29 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_3.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec29, 0.0);
    t16_9 = (-_LightShadowData.x) + 1.0;
    t16_3.x = t16_3.x * t16_9 + _LightShadowData.x;
    t16_3.x = t16_3.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_3.x = max(t16_3.x, 0.0);
    t16_6.xyz = t16_6.xyz * t16_3.xxx;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t10_5 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_18 = t10_5 * 0.9375 + 0.03125;
    t16_3.x = (-t16_18) + _FadeColor.w;
    t16_18 = (-t16_18) + _NoiseGlow.w;
    t16_18 = -abs(t16_18) * 32.0 + 1.0;
    t16_18 = max(t16_18, 0.0);
    t16_9 = -abs(t16_3.x) * 32.0 + 1.0;
    t16_1.w = t16_3.x * 32.0 + 0.9375;
    t16_3.x = max(t16_9, 0.0);
    t16_0.xyz = t16_0.xyz + t16_3.xxx;
    t16_0.xyz = _FadeColor.xyz * t16_0.xyz + vec3(t16_18);
    t16_0.xyz = t16_0.xyz * _NoiseGlow.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0 = t16_1;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  highp vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  mediump vec2 tmpvar_10;
  mediump vec3 tmpvar_11;
  mediump vec4 tmpvar_12;
  tmpvar_10.x = 0.0;
  tmpvar_7 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_7 * 0.5);
  highp vec2 tmpvar_15;
  tmpvar_15.x = tmpvar_14.x;
  tmpvar_15.y = (tmpvar_14.y * _ProjectionParams.x);
  o_13.xy = (tmpvar_15 + tmpvar_14.w);
  o_13.zw = tmpvar_7.zw;
  tmpvar_11 = o_13.xyw;
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_8.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_9.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_9.zw = tmpvar_1.xy;
  highp vec3 tmpvar_16;
  highp vec4 tmpvar_17;
  tmpvar_17 = (_Object2World * _glesVertex);
  tmpvar_16 = normalize((_WorldSpaceCameraPos - tmpvar_17.xyz));
  worldViewDir_6 = tmpvar_16;
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_18 * _glesNormal));
  worldNormal_5 = tmpvar_19;
  highp mat3 tmpvar_20;
  tmpvar_20[0] = _Object2World[0].xyz;
  tmpvar_20[1] = _Object2World[1].xyz;
  tmpvar_20[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_21;
  mediump vec3 tmpvar_22;
  tmpvar_22 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_23;
  tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_23;
  mediump vec4 tmpvar_24;
  tmpvar_24.xyz = worldViewDir_6;
  tmpvar_24.w = worldBinormal_3.x;
  mediump vec4 tmpvar_25;
  tmpvar_25.xyz = worldNormal_5;
  tmpvar_25.w = worldBinormal_3.y;
  mediump vec4 tmpvar_26;
  tmpvar_26.xyz = worldTangent_4;
  tmpvar_26.w = worldBinormal_3.z;
  tmpvar_12 = (unity_World2Shadow[0] * tmpvar_17);
  highp float tmpvar_27;
  highp vec3 x_28;
  x_28 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_27 = sqrt(dot (x_28, x_28));
  fogpos_2 = tmpvar_27;
  tmpvar_10.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_10.x = (1.0 - tmpvar_10.y);
  gl_Position = tmpvar_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = tmpvar_10;
  xlv_TEXCOORD3 = tmpvar_24;
  xlv_TEXCOORD4 = tmpvar_25;
  xlv_TEXCOORD5 = tmpvar_26;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_11;
  xlv_TEXCOORD8 = tmpvar_12;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
uniform mediump vec4 _NoiseGlow;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_6;
  mediump vec3 lit_color_7;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_8.xyz;
  tmpvar_2 = tmpvar_8.w;
  mediump vec3 tangentNormal_9;
  mediump vec3 tmpvar_10;
  tmpvar_10.x = xlv_TEXCOORD3.w;
  tmpvar_10.y = xlv_TEXCOORD4.w;
  tmpvar_10.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_11 = xlv_TEXCOORD5.xyz;
  tmpvar_12 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_13;
  tmpvar_13[0].x = tmpvar_11.x;
  tmpvar_13[0].y = tmpvar_10.x;
  tmpvar_13[0].z = tmpvar_12.x;
  tmpvar_13[1].x = tmpvar_11.y;
  tmpvar_13[1].y = tmpvar_10.y;
  tmpvar_13[1].z = tmpvar_12.y;
  tmpvar_13[2].x = tmpvar_11.z;
  tmpvar_13[2].y = tmpvar_10.z;
  tmpvar_13[2].z = tmpvar_12.z;
  lowp vec3 tmpvar_14;
  tmpvar_14 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_9 = tmpvar_14;
  tmpvar_4 = (tangentNormal_9 * tmpvar_13);
  mediump vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_4;
  mediump vec3 x2_16;
  mediump vec3 x1_17;
  x1_17.x = dot (unity_SHAr, tmpvar_15);
  x1_17.y = dot (unity_SHAg, tmpvar_15);
  x1_17.z = dot (unity_SHAb, tmpvar_15);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_16.x = dot (unity_SHBr, tmpvar_18);
  x2_16.y = dot (unity_SHBg, tmpvar_18);
  x2_16.z = dot (unity_SHBb, tmpvar_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = max (((1.055 * 
    pow ((((x2_16 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_17) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_7 = (((
    (tmpvar_19 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_19)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_20;
  lowp float shadow_21;
  shadow_21 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_22;
  tmpvar_22 = vec3(shadow_21);
  atten_20 = tmpvar_22;
  lit_color_7 = (lit_color_7 * max ((_Shadows.x + 
    (atten_20 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_23;
  tmpvar_23 = mix (lit_color_7, vec3((dot (lit_color_7, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_6.xyz = tmpvar_23;
  c_6.w = _RimColor.w;
  lowp vec4 _color_24;
  _color_24.w = c_6.w;
  mediump float rTest_25;
  mediump vec2 tmpvar_26;
  tmpvar_26 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_27;
  tmpvar_27 = ((1.5 * _FadeColor.w) - tmpvar_26.y);
  if ((tmpvar_27 < 0.0)) {
    discard;
  };
  mediump float tmpvar_28;
  tmpvar_28 = clamp ((1.0 - (tmpvar_27 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_29;
  tmpvar_29.x = tmpvar_26.x;
  tmpvar_29.y = clamp ((tmpvar_28 - tmpvar_26.y), 0.0, 1.0);
  lowp float tmpvar_30;
  tmpvar_30 = texture2D (_RasterTex, tmpvar_29).z;
  rTest_25 = tmpvar_30;
  mediump float x_31;
  x_31 = (rTest_25 - 0.4);
  if ((x_31 < 0.0)) {
    discard;
  };
  _color_24.xyz = (c_6.xyz + (_FadeColor.xyz * tmpvar_28));
  lowp vec4 _color_32;
  _color_32.w = _color_24.w;
  lowp vec4 tmpvar_33;
  tmpvar_33 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_34;
  tmpvar_34 = clamp ((1.0 - (32.0 * 
    abs((_NoiseGlow.w - ((0.9375 * tmpvar_33.y) + 0.03125)))
  )), 0.0, 1.0);
  _color_32.xyz = (_NoiseGlow.xyz * (tmpvar_34 + _color_24.xyz));
  c_6.w = _color_32.w;
  c_6.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_32.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_6;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" "RASTER_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD7;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_2;
highp vec4 t3;
mediump vec3 t16_4;
highp vec3 t5;
mediump float t16_8;
highp float t18;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t1.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t1;
    t1.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t1.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t1;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = sqrt(t18);
    t16_2 = (-t18) + acid_Fog.y;
    t16_8 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_8;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_8 = t16_2 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_8;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = inversesqrt(t18);
    t1.xyz = vec3(t18) * t1.xyz;
    t3.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t2.xyz = vec3(t18) * t3.xyz;
    t3.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t3.xyz = vec3(t18) * t3.xyz;
    t16_4.xyz = t2.zxy * t3.yzx;
    t16_4.xyz = t2.yzx * t3.zxy + (-t16_4.xyz);
    t5.xyz = t16_4.xyz * in_TANGENT0.www;
    t18 = dot(t5.xyz, t5.xyz);
    t18 = inversesqrt(t18);
    t5.xyz = vec3(t18) * t5.xyz;
    t1.w = t5.x;
    vs_TEXCOORD3 = t1;
    t2.w = t5.y;
    t3.w = t5.z;
    vs_TEXCOORD5 = t3;
    vs_TEXCOORD4 = t2;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0.w = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t1.zz + t1.xw;
    vs_TEXCOORD7.xyz = t0.xyz;
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1 = t0.yyyy * unity_World2Shadow[0][1];
    t1 = unity_World2Shadow[0][0] * t0.xxxx + t1;
    t1 = unity_World2Shadow[0][2] * t0.zzzz + t1;
    t0 = unity_World2Shadow[0][3] * t0.wwww + t1;
    vs_TEXCOORD8 = t0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	mediump vec4 _NoiseGlow;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _RasterTex;
uniform lowp sampler2D _NoiseTexture;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec3 vs_TEXCOORD7;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
bool tb1;
mediump vec4 t16_2;
lowp vec3 t10_2;
mediump vec4 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
lowp float t10_6;
mediump float t16_10;
mediump vec3 t16_11;
mediump float t16_17;
mediump float t16_21;
void main()
{
    t16_0.xy = vs_TEXCOORD7.xy / vs_TEXCOORD7.zz;
    t16_21 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_21<0.0;
    t16_21 = (-t16_21) * 2.5 + 1.0;
    t16_21 = clamp(t16_21, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_21;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1.x = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1.x + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = vs_TEXCOORD5.y;
    t16_0.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = t16_1.y * t16_1.y;
    t16_0.x = t16_1.x * t16_1.x + (-t16_0.x);
    t16_3.xy = vs_TEXCOORD5.zw;
    t16_3.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_2.xyz, t16_3.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_3.x = dot(unity_SHBr, t16_2);
    t16_3.y = dot(unity_SHBg, t16_2);
    t16_3.z = dot(unity_SHBb, t16_2);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_3.xyz;
    t16_1.w = 1.0;
    t16_3.x = dot(unity_SHAr, t16_1);
    t16_3.y = dot(unity_SHAg, t16_1);
    t16_3.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_3.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t16_3.x = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_3.x = t16_3.x + t16_3.x;
    t16_3.xyz = t16_1.xyz * (-t16_3.xxx) + vs_TEXCOORD3.xyz;
    t16_3.x = dot(t16_3.xyz, (-t16_1.xyz));
    t16_3.y = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_3.xy = max(t16_3.xy, vec2(0.25, 0.0));
    t16_3.x = t16_3.x * _PropGlossiness.w;
    t16_3.x = t16_3.x * 10.0;
    t16_3.xzw = t16_3.xxx * _PropGlossiness.xyz;
    t16_3.xzw = t16_0.xyz * t16_3.xzw;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_3.xzw = t10_1.www * t16_3.xzw;
    t16_3.xzw = t10_1.xyz * t16_3.xzw;
    t16_0.xyz = t16_0.xyz * t10_1.xyz;
    t16_4.x = (-t10_1.w) + 1.0;
    t16_0.xyz = t16_0.xyz * t16_4.xxx;
    t10_5 = (-_LightDir.w) + 1.0;
    t16_10 = t16_3.y * _LightDir.w + t10_5;
    t16_4.x = t16_10 * 1.5;
    t16_4.x = min(t16_4.x, 1.0);
    t16_11.xyz = vec3(t16_10) + (-_CharacterShadowColor.xyz);
    t16_10 = t16_10 * _LightParameters.y;
    t16_4.xyz = t16_4.xxx * t16_11.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_4.xyz + t16_3.xzw;
    vec3 txVec37 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_3.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec37, 0.0);
    t16_17 = (-_LightShadowData.x) + 1.0;
    t16_3.x = t16_3.x * t16_17 + _LightShadowData.x;
    t16_3.x = t16_3.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_3.x = max(t16_3.x, 0.0);
    t16_0.xyz = t16_0.xyz * t16_3.xxx;
    t16_3.x = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xzw = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = vec3(t16_10) * t16_3.xzw + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_21) + t16_0.xyz;
    t10_6 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_21 = t10_6 * 0.9375 + 0.03125;
    t16_21 = (-t16_21) + _NoiseGlow.w;
    t16_21 = -abs(t16_21) * 32.0 + 1.0;
    t16_21 = max(t16_21, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_21);
    t16_0.xyz = t16_0.xyz * _NoiseGlow.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_9.xyz;
  tmpvar_2 = tmpvar_9.w;
  mediump vec3 tangentNormal_10;
  mediump vec3 tmpvar_11;
  tmpvar_11.x = xlv_TEXCOORD3.w;
  tmpvar_11.y = xlv_TEXCOORD4.w;
  tmpvar_11.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_12;
  mediump vec3 tmpvar_13;
  tmpvar_12 = xlv_TEXCOORD5.xyz;
  tmpvar_13 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_11.x;
  tmpvar_14[0].z = tmpvar_13.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_11.y;
  tmpvar_14[1].z = tmpvar_13.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_11.z;
  tmpvar_14[2].z = tmpvar_13.z;
  lowp vec3 tmpvar_15;
  tmpvar_15 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_10 = tmpvar_15;
  tmpvar_4 = (tangentNormal_10 * tmpvar_14);
  mediump vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = tmpvar_4;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  x1_18.x = dot (unity_SHAr, tmpvar_16);
  x1_18.y = dot (unity_SHAg, tmpvar_16);
  x1_18.z = dot (unity_SHAb, tmpvar_16);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_17.x = dot (unity_SHBr, tmpvar_19);
  x2_17.y = dot (unity_SHBg, tmpvar_19);
  x2_17.z = dot (unity_SHBb, tmpvar_19);
  mediump vec3 tmpvar_20;
  tmpvar_20 = max (((1.055 * 
    pow ((((x2_17 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_18) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = (1.0 - dot (tmpvar_4, xlv_TEXCOORD3.xyz));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_20 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_20)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_21;
  tmpvar_21 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_7.xyz = tmpvar_21;
  c_7.w = _RimColor.w;
  lowp vec4 _color_22;
  _color_22 = (c_7 * _FadeColor);
  lowp vec4 _color_23;
  _color_23.w = _color_22.w;
  _color_23.xyz = (_color_22.xyz + (tmpvar_6 * _RimColor.xyz));
  c_7.w = _color_23.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_23.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
mediump vec3 t16_5;
mediump vec3 t16_6;
mediump float t16_18;
mediump float t16_21;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_18 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_21 = t16_18 + t16_18;
    t16_18 = (-t16_18) + 1.0;
    t16_4.xyz = vec3(t16_18) * _RimColor.xyz;
    t16_5.xyz = t16_0.xyz * (-vec3(t16_21)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_5.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_6.x = t16_0.y * _PropGlossiness.w;
    t16_6.x = t16_6.x * 10.0;
    t16_6.xyz = t16_6.xxx * _PropGlossiness.xyz;
    t16_6.xyz = t16_3.xyz * t16_6.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_6.xyz = t16_6.xyz * t10_1.www;
    t16_6.xyz = t10_1.xyz * t16_6.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_21 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_21) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_21 = t16_0.x * 1.5;
    t16_21 = min(t16_21, 1.0);
    t16_5.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_5.xyz = vec3(t16_21) * t16_5.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_3.xyz * t16_5.xyz + t16_6.xyz;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz + t16_4.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w * _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_9.xyz;
  tmpvar_2 = tmpvar_9.w;
  mediump vec3 tangentNormal_10;
  mediump vec3 tmpvar_11;
  tmpvar_11.x = xlv_TEXCOORD3.w;
  tmpvar_11.y = xlv_TEXCOORD4.w;
  tmpvar_11.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_12;
  mediump vec3 tmpvar_13;
  tmpvar_12 = xlv_TEXCOORD5.xyz;
  tmpvar_13 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_11.x;
  tmpvar_14[0].z = tmpvar_13.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_11.y;
  tmpvar_14[1].z = tmpvar_13.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_11.z;
  tmpvar_14[2].z = tmpvar_13.z;
  lowp vec3 tmpvar_15;
  tmpvar_15 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_10 = tmpvar_15;
  tmpvar_4 = (tangentNormal_10 * tmpvar_14);
  mediump vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = tmpvar_4;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  x1_18.x = dot (unity_SHAr, tmpvar_16);
  x1_18.y = dot (unity_SHAg, tmpvar_16);
  x1_18.z = dot (unity_SHAb, tmpvar_16);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_17.x = dot (unity_SHBr, tmpvar_19);
  x2_17.y = dot (unity_SHBg, tmpvar_19);
  x2_17.z = dot (unity_SHBb, tmpvar_19);
  mediump vec3 tmpvar_20;
  tmpvar_20 = max (((1.055 * 
    pow ((((x2_17 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_18) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = (1.0 - dot (tmpvar_4, xlv_TEXCOORD3.xyz));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_20 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_20)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_21;
  tmpvar_21 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_7.xyz = tmpvar_21;
  c_7.w = _RimColor.w;
  lowp vec4 _color_22;
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_24;
  tmpvar_24 = (_FadeColor.w - ((0.9375 * tmpvar_23.y) + 0.03125));
  mediump float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_24)
  )), 0.0, 1.0);
  _color_22.xyz = (_FadeColor.xyz * (tmpvar_25 + c_7.xyz));
  _color_22.w = ((32.0 * tmpvar_24) + 0.9375);
  lowp vec4 _color_26;
  _color_26.w = _color_22.w;
  _color_26.xyz = (_color_22.xyz + (tmpvar_6 * _RimColor.xyz));
  c_7.w = _color_26.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_26.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _NoiseTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
mediump vec3 t16_5;
lowp float t10_6;
mediump vec3 t16_7;
mediump float t16_21;
mediump float t16_24;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_21 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_24 = t16_21 + t16_21;
    t16_21 = (-t16_21) + 1.0;
    t16_4.xyz = vec3(t16_21) * _RimColor.xyz;
    t16_5.xyz = t16_0.xyz * (-vec3(t16_24)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_5.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_7.x = t16_0.y * _PropGlossiness.w;
    t16_7.x = t16_7.x * 10.0;
    t16_7.xyz = t16_7.xxx * _PropGlossiness.xyz;
    t16_7.xyz = t16_3.xyz * t16_7.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_7.xyz = t16_7.xyz * t10_1.www;
    t16_7.xyz = t10_1.xyz * t16_7.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_24 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_24) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_24 = t16_0.x * 1.5;
    t16_24 = min(t16_24, 1.0);
    t16_5.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_5.xyz = vec3(t16_24) * t16_5.xyz + _CharacterShadowColor.xyz;
    t16_7.xyz = t16_3.xyz * t16_5.xyz + t16_7.xyz;
    t16_3.x = dot(t16_7.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_7.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_7.xyz;
    t10_6 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_21 = t10_6 * 0.9375 + 0.03125;
    t16_21 = (-t16_21) + _FadeColor.w;
    t16_3.x = -abs(t16_21) * 32.0 + 1.0;
    t16_1.w = t16_21 * 32.0 + 0.9375;
    t16_21 = max(t16_3.x, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_21);
    t16_0.xyz = _FadeColor.xyz * t16_0.xyz + t16_4.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0 = t16_1;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  highp vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  mediump vec2 tmpvar_10;
  mediump vec3 tmpvar_11;
  tmpvar_10.x = 0.0;
  tmpvar_7 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = (tmpvar_7 * 0.5);
  highp vec2 tmpvar_14;
  tmpvar_14.x = tmpvar_13.x;
  tmpvar_14.y = (tmpvar_13.y * _ProjectionParams.x);
  o_12.xy = (tmpvar_14 + tmpvar_13.w);
  o_12.zw = tmpvar_7.zw;
  tmpvar_11 = o_12.xyw;
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_8.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_9.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_9.zw = tmpvar_1.xy;
  highp vec3 tmpvar_15;
  tmpvar_15 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_15;
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _Object2World[0].xyz;
  tmpvar_16[1] = _Object2World[1].xyz;
  tmpvar_16[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_16 * _glesNormal));
  worldNormal_5 = tmpvar_17;
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_18 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_19;
  mediump vec3 tmpvar_20;
  tmpvar_20 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22.xyz = worldViewDir_6;
  tmpvar_22.w = worldBinormal_3.x;
  mediump vec4 tmpvar_23;
  tmpvar_23.xyz = worldNormal_5;
  tmpvar_23.w = worldBinormal_3.y;
  mediump vec4 tmpvar_24;
  tmpvar_24.xyz = worldTangent_4;
  tmpvar_24.w = worldBinormal_3.z;
  highp float tmpvar_25;
  highp vec3 x_26;
  x_26 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_25 = sqrt(dot (x_26, x_26));
  fogpos_2 = tmpvar_25;
  tmpvar_10.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_10.x = (1.0 - tmpvar_10.y);
  gl_Position = tmpvar_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = tmpvar_10;
  xlv_TEXCOORD3 = tmpvar_22;
  xlv_TEXCOORD4 = tmpvar_23;
  xlv_TEXCOORD5 = tmpvar_24;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_11;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_9.xyz;
  tmpvar_2 = tmpvar_9.w;
  mediump vec3 tangentNormal_10;
  mediump vec3 tmpvar_11;
  tmpvar_11.x = xlv_TEXCOORD3.w;
  tmpvar_11.y = xlv_TEXCOORD4.w;
  tmpvar_11.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_12;
  mediump vec3 tmpvar_13;
  tmpvar_12 = xlv_TEXCOORD5.xyz;
  tmpvar_13 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_11.x;
  tmpvar_14[0].z = tmpvar_13.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_11.y;
  tmpvar_14[1].z = tmpvar_13.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_11.z;
  tmpvar_14[2].z = tmpvar_13.z;
  lowp vec3 tmpvar_15;
  tmpvar_15 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_10 = tmpvar_15;
  tmpvar_4 = (tangentNormal_10 * tmpvar_14);
  mediump vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = tmpvar_4;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  x1_18.x = dot (unity_SHAr, tmpvar_16);
  x1_18.y = dot (unity_SHAg, tmpvar_16);
  x1_18.z = dot (unity_SHAb, tmpvar_16);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_17.x = dot (unity_SHBr, tmpvar_19);
  x2_17.y = dot (unity_SHBg, tmpvar_19);
  x2_17.z = dot (unity_SHBb, tmpvar_19);
  mediump vec3 tmpvar_20;
  tmpvar_20 = max (((1.055 * 
    pow ((((x2_17 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_18) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = (1.0 - dot (tmpvar_4, xlv_TEXCOORD3.xyz));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_20 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_20)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_21;
  tmpvar_21 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_7.xyz = tmpvar_21;
  c_7.w = _RimColor.w;
  lowp vec4 _color_22;
  _color_22.w = c_7.w;
  mediump float rTest_23;
  mediump vec2 tmpvar_24;
  tmpvar_24 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_25;
  tmpvar_25 = ((1.5 * _FadeColor.w) - tmpvar_24.y);
  if ((tmpvar_25 < 0.0)) {
    discard;
  };
  mediump float tmpvar_26;
  tmpvar_26 = clamp ((1.0 - (tmpvar_25 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_27;
  tmpvar_27.x = tmpvar_24.x;
  tmpvar_27.y = clamp ((tmpvar_26 - tmpvar_24.y), 0.0, 1.0);
  lowp float tmpvar_28;
  tmpvar_28 = texture2D (_RasterTex, tmpvar_27).z;
  rTest_23 = tmpvar_28;
  mediump float x_29;
  x_29 = (rTest_23 - 0.4);
  if ((x_29 < 0.0)) {
    discard;
  };
  _color_22.xyz = (c_7.xyz + (_FadeColor.xyz * tmpvar_26));
  lowp vec4 _color_30;
  _color_30.w = _color_22.w;
  _color_30.xyz = (_color_22.xyz + (tmpvar_6 * _RimColor.xyz));
  c_7.w = _color_30.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_30.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD7;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_2;
highp vec4 t3;
mediump vec3 t16_4;
highp vec3 t5;
mediump float t16_8;
highp float t18;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t1.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t1;
    t1.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t1.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t1;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = sqrt(t18);
    t16_2 = (-t18) + acid_Fog.y;
    t16_8 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_8;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_8 = t16_2 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_8;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = inversesqrt(t18);
    t1.xyz = vec3(t18) * t1.xyz;
    t3.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t2.xyz = vec3(t18) * t3.xyz;
    t3.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t3.xyz = vec3(t18) * t3.xyz;
    t16_4.xyz = t2.zxy * t3.yzx;
    t16_4.xyz = t2.yzx * t3.zxy + (-t16_4.xyz);
    t5.xyz = t16_4.xyz * in_TANGENT0.www;
    t18 = dot(t5.xyz, t5.xyz);
    t18 = inversesqrt(t18);
    t5.xyz = vec3(t18) * t5.xyz;
    t1.w = t5.x;
    vs_TEXCOORD3 = t1;
    t2.w = t5.y;
    t3.w = t5.z;
    vs_TEXCOORD5 = t3;
    vs_TEXCOORD4 = t2;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0.w = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t1.zz + t1.xw;
    vs_TEXCOORD7.xyz = t0.xyz;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _RasterTex;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec3 vs_TEXCOORD7;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
bool tb1;
mediump vec4 t16_2;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
mediump vec3 t16_6;
mediump vec3 t16_10;
mediump float t16_17;
mediump float t16_21;
void main()
{
    t16_0.xy = vs_TEXCOORD7.xy / vs_TEXCOORD7.zz;
    t16_21 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_21<0.0;
    t16_21 = (-t16_21) * 2.5 + 1.0;
    t16_21 = clamp(t16_21, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_21;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1.x = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1.x + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = vs_TEXCOORD5.y;
    t16_0.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = t16_1.y * t16_1.y;
    t16_0.x = t16_1.x * t16_1.x + (-t16_0.x);
    t16_3.xy = vs_TEXCOORD5.zw;
    t16_3.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_2.xyz, t16_3.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_3.x = dot(unity_SHBr, t16_2);
    t16_3.y = dot(unity_SHBg, t16_2);
    t16_3.z = dot(unity_SHBb, t16_2);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_3.xyz;
    t16_1.w = 1.0;
    t16_3.x = dot(unity_SHAr, t16_1);
    t16_3.y = dot(unity_SHAg, t16_1);
    t16_3.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_3.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t16_3.x = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_10.x = t16_3.x + t16_3.x;
    t16_3.x = (-t16_3.x) + 1.0;
    t16_10.xyz = t16_1.xyz * (-t16_10.xxx) + vs_TEXCOORD3.xyz;
    t16_10.x = dot(t16_10.xyz, (-t16_1.xyz));
    t16_10.y = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_10.xy = max(t16_10.xy, vec2(0.25, 0.0));
    t16_10.x = t16_10.x * _PropGlossiness.w;
    t16_10.x = t16_10.x * 10.0;
    t16_4.xyz = t16_10.xxx * _PropGlossiness.xyz;
    t16_4.xyz = t16_0.xyz * t16_4.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_4.xyz = t10_1.www * t16_4.xyz;
    t16_4.xyz = t10_1.xyz * t16_4.xyz;
    t16_0.xyz = t16_0.xyz * t10_1.xyz;
    t16_10.x = (-t10_1.w) + 1.0;
    t16_0.xyz = t16_0.xyz * t16_10.xxx;
    t10_5 = (-_LightDir.w) + 1.0;
    t16_10.x = t16_10.y * _LightDir.w + t10_5;
    t16_17 = t16_10.x * 1.5;
    t16_17 = min(t16_17, 1.0);
    t16_6.xyz = t16_10.xxx + (-_CharacterShadowColor.xyz);
    t16_10.x = t16_10.x * _LightParameters.y;
    t16_6.xyz = vec3(t16_17) * t16_6.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_6.xyz + t16_4.xyz;
    t16_17 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_4.xyz = vec3(t16_17) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = t16_10.xxx * t16_4.xyz + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_21) + t16_0.xyz;
    t16_0.xyz = t16_3.xxx * _RimColor.xyz + t16_0.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_9.xyz;
  tmpvar_2 = tmpvar_9.w;
  mediump vec3 tangentNormal_10;
  mediump vec3 tmpvar_11;
  tmpvar_11.x = xlv_TEXCOORD3.w;
  tmpvar_11.y = xlv_TEXCOORD4.w;
  tmpvar_11.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_12;
  mediump vec3 tmpvar_13;
  tmpvar_12 = xlv_TEXCOORD5.xyz;
  tmpvar_13 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_11.x;
  tmpvar_14[0].z = tmpvar_13.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_11.y;
  tmpvar_14[1].z = tmpvar_13.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_11.z;
  tmpvar_14[2].z = tmpvar_13.z;
  lowp vec3 tmpvar_15;
  tmpvar_15 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_10 = tmpvar_15;
  tmpvar_4 = (tangentNormal_10 * tmpvar_14);
  mediump vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = tmpvar_4;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  x1_18.x = dot (unity_SHAr, tmpvar_16);
  x1_18.y = dot (unity_SHAg, tmpvar_16);
  x1_18.z = dot (unity_SHAb, tmpvar_16);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_17.x = dot (unity_SHBr, tmpvar_19);
  x2_17.y = dot (unity_SHBg, tmpvar_19);
  x2_17.z = dot (unity_SHBb, tmpvar_19);
  mediump vec3 tmpvar_20;
  tmpvar_20 = max (((1.055 * 
    pow ((((x2_17 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_18) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = (1.0 - dot (tmpvar_4, xlv_TEXCOORD3.xyz));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_20 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_20)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_21;
  tmpvar_21 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_7.xyz = tmpvar_21;
  c_7.w = _RimColor.w;
  lowp vec4 _color_22;
  _color_22 = (c_7 * _FadeColor);
  lowp vec4 _color_23;
  _color_23.w = _color_22.w;
  _color_23.xyz = (_color_22.xyz + (tmpvar_6 * _RimColor.xyz));
  c_7.w = _color_23.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_23.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
mediump vec3 t16_5;
mediump vec3 t16_6;
mediump float t16_18;
mediump float t16_21;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_18 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_21 = t16_18 + t16_18;
    t16_18 = (-t16_18) + 1.0;
    t16_4.xyz = vec3(t16_18) * _RimColor.xyz;
    t16_5.xyz = t16_0.xyz * (-vec3(t16_21)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_5.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_6.x = t16_0.y * _PropGlossiness.w;
    t16_6.x = t16_6.x * 10.0;
    t16_6.xyz = t16_6.xxx * _PropGlossiness.xyz;
    t16_6.xyz = t16_3.xyz * t16_6.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_6.xyz = t16_6.xyz * t10_1.www;
    t16_6.xyz = t10_1.xyz * t16_6.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_21 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_21) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_21 = t16_0.x * 1.5;
    t16_21 = min(t16_21, 1.0);
    t16_5.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_5.xyz = vec3(t16_21) * t16_5.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_3.xyz * t16_5.xyz + t16_6.xyz;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz + t16_4.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w * _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_10;
  tmpvar_10 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_10;
  highp mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize((tmpvar_11 * _glesNormal));
  worldNormal_5 = tmpvar_12;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_16;
  mediump vec4 tmpvar_17;
  tmpvar_17.xyz = worldViewDir_6;
  tmpvar_17.w = worldBinormal_3.x;
  mediump vec4 tmpvar_18;
  tmpvar_18.xyz = worldNormal_5;
  tmpvar_18.w = worldBinormal_3.y;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldTangent_4;
  tmpvar_19.w = worldBinormal_3.z;
  highp float tmpvar_20;
  highp vec3 x_21;
  x_21 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_20 = sqrt(dot (x_21, x_21));
  fogpos_2 = tmpvar_20;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_17;
  xlv_TEXCOORD4 = tmpvar_18;
  xlv_TEXCOORD5 = tmpvar_19;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_9.xyz;
  tmpvar_2 = tmpvar_9.w;
  mediump vec3 tangentNormal_10;
  mediump vec3 tmpvar_11;
  tmpvar_11.x = xlv_TEXCOORD3.w;
  tmpvar_11.y = xlv_TEXCOORD4.w;
  tmpvar_11.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_12;
  mediump vec3 tmpvar_13;
  tmpvar_12 = xlv_TEXCOORD5.xyz;
  tmpvar_13 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_11.x;
  tmpvar_14[0].z = tmpvar_13.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_11.y;
  tmpvar_14[1].z = tmpvar_13.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_11.z;
  tmpvar_14[2].z = tmpvar_13.z;
  lowp vec3 tmpvar_15;
  tmpvar_15 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_10 = tmpvar_15;
  tmpvar_4 = (tangentNormal_10 * tmpvar_14);
  mediump vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = tmpvar_4;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  x1_18.x = dot (unity_SHAr, tmpvar_16);
  x1_18.y = dot (unity_SHAg, tmpvar_16);
  x1_18.z = dot (unity_SHAb, tmpvar_16);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_17.x = dot (unity_SHBr, tmpvar_19);
  x2_17.y = dot (unity_SHBg, tmpvar_19);
  x2_17.z = dot (unity_SHBb, tmpvar_19);
  mediump vec3 tmpvar_20;
  tmpvar_20 = max (((1.055 * 
    pow ((((x2_17 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_18) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = (1.0 - dot (tmpvar_4, xlv_TEXCOORD3.xyz));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_20 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_20)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_21;
  tmpvar_21 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_7.xyz = tmpvar_21;
  c_7.w = _RimColor.w;
  lowp vec4 _color_22;
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_24;
  tmpvar_24 = (_FadeColor.w - ((0.9375 * tmpvar_23.y) + 0.03125));
  mediump float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_24)
  )), 0.0, 1.0);
  _color_22.xyz = (_FadeColor.xyz * (tmpvar_25 + c_7.xyz));
  _color_22.w = ((32.0 * tmpvar_24) + 0.9375);
  lowp vec4 _color_26;
  _color_26.w = _color_22.w;
  _color_26.xyz = (_color_22.xyz + (tmpvar_6 * _RimColor.xyz));
  c_7.w = _color_26.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_26.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _NoiseTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
mediump vec3 t16_5;
lowp float t10_6;
mediump vec3 t16_7;
mediump float t16_21;
mediump float t16_24;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_21 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_24 = t16_21 + t16_21;
    t16_21 = (-t16_21) + 1.0;
    t16_4.xyz = vec3(t16_21) * _RimColor.xyz;
    t16_5.xyz = t16_0.xyz * (-vec3(t16_24)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_5.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_7.x = t16_0.y * _PropGlossiness.w;
    t16_7.x = t16_7.x * 10.0;
    t16_7.xyz = t16_7.xxx * _PropGlossiness.xyz;
    t16_7.xyz = t16_3.xyz * t16_7.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_7.xyz = t16_7.xyz * t10_1.www;
    t16_7.xyz = t10_1.xyz * t16_7.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_24 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_24) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_24 = t16_0.x * 1.5;
    t16_24 = min(t16_24, 1.0);
    t16_5.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_5.xyz = vec3(t16_24) * t16_5.xyz + _CharacterShadowColor.xyz;
    t16_7.xyz = t16_3.xyz * t16_5.xyz + t16_7.xyz;
    t16_3.x = dot(t16_7.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_7.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_7.xyz;
    t10_6 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_21 = t10_6 * 0.9375 + 0.03125;
    t16_21 = (-t16_21) + _FadeColor.w;
    t16_3.x = -abs(t16_21) * 32.0 + 1.0;
    t16_1.w = t16_21 * 32.0 + 0.9375;
    t16_21 = max(t16_3.x, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_21);
    t16_0.xyz = _FadeColor.xyz * t16_0.xyz + t16_4.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0 = t16_1;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  highp vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  mediump vec2 tmpvar_10;
  mediump vec3 tmpvar_11;
  tmpvar_10.x = 0.0;
  tmpvar_7 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_12;
  highp vec4 tmpvar_13;
  tmpvar_13 = (tmpvar_7 * 0.5);
  highp vec2 tmpvar_14;
  tmpvar_14.x = tmpvar_13.x;
  tmpvar_14.y = (tmpvar_13.y * _ProjectionParams.x);
  o_12.xy = (tmpvar_14 + tmpvar_13.w);
  o_12.zw = tmpvar_7.zw;
  tmpvar_11 = o_12.xyw;
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_8.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_9.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_9.zw = tmpvar_1.xy;
  highp vec3 tmpvar_15;
  tmpvar_15 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  worldViewDir_6 = tmpvar_15;
  highp mat3 tmpvar_16;
  tmpvar_16[0] = _Object2World[0].xyz;
  tmpvar_16[1] = _Object2World[1].xyz;
  tmpvar_16[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_17;
  tmpvar_17 = normalize((tmpvar_16 * _glesNormal));
  worldNormal_5 = tmpvar_17;
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_18 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_19;
  mediump vec3 tmpvar_20;
  tmpvar_20 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22.xyz = worldViewDir_6;
  tmpvar_22.w = worldBinormal_3.x;
  mediump vec4 tmpvar_23;
  tmpvar_23.xyz = worldNormal_5;
  tmpvar_23.w = worldBinormal_3.y;
  mediump vec4 tmpvar_24;
  tmpvar_24.xyz = worldTangent_4;
  tmpvar_24.w = worldBinormal_3.z;
  highp float tmpvar_25;
  highp vec3 x_26;
  x_26 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_25 = sqrt(dot (x_26, x_26));
  fogpos_2 = tmpvar_25;
  tmpvar_10.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_10.x = (1.0 - tmpvar_10.y);
  gl_Position = tmpvar_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = tmpvar_10;
  xlv_TEXCOORD3 = tmpvar_22;
  xlv_TEXCOORD4 = tmpvar_23;
  xlv_TEXCOORD5 = tmpvar_24;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_11;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform lowp vec4 unity_FogColor;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD7;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_9.xyz;
  tmpvar_2 = tmpvar_9.w;
  mediump vec3 tangentNormal_10;
  mediump vec3 tmpvar_11;
  tmpvar_11.x = xlv_TEXCOORD3.w;
  tmpvar_11.y = xlv_TEXCOORD4.w;
  tmpvar_11.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_12;
  mediump vec3 tmpvar_13;
  tmpvar_12 = xlv_TEXCOORD5.xyz;
  tmpvar_13 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_11.x;
  tmpvar_14[0].z = tmpvar_13.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_11.y;
  tmpvar_14[1].z = tmpvar_13.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_11.z;
  tmpvar_14[2].z = tmpvar_13.z;
  lowp vec3 tmpvar_15;
  tmpvar_15 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_10 = tmpvar_15;
  tmpvar_4 = (tangentNormal_10 * tmpvar_14);
  mediump vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = tmpvar_4;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  x1_18.x = dot (unity_SHAr, tmpvar_16);
  x1_18.y = dot (unity_SHAg, tmpvar_16);
  x1_18.z = dot (unity_SHAb, tmpvar_16);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_17.x = dot (unity_SHBr, tmpvar_19);
  x2_17.y = dot (unity_SHBg, tmpvar_19);
  x2_17.z = dot (unity_SHBb, tmpvar_19);
  mediump vec3 tmpvar_20;
  tmpvar_20 = max (((1.055 * 
    pow ((((x2_17 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_18) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = (1.0 - dot (tmpvar_4, xlv_TEXCOORD3.xyz));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_20 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_20)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 tmpvar_21;
  tmpvar_21 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_7.xyz = tmpvar_21;
  c_7.w = _RimColor.w;
  lowp vec4 _color_22;
  _color_22.w = c_7.w;
  mediump float rTest_23;
  mediump vec2 tmpvar_24;
  tmpvar_24 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_25;
  tmpvar_25 = ((1.5 * _FadeColor.w) - tmpvar_24.y);
  if ((tmpvar_25 < 0.0)) {
    discard;
  };
  mediump float tmpvar_26;
  tmpvar_26 = clamp ((1.0 - (tmpvar_25 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_27;
  tmpvar_27.x = tmpvar_24.x;
  tmpvar_27.y = clamp ((tmpvar_26 - tmpvar_24.y), 0.0, 1.0);
  lowp float tmpvar_28;
  tmpvar_28 = texture2D (_RasterTex, tmpvar_27).z;
  rTest_23 = tmpvar_28;
  mediump float x_29;
  x_29 = (rTest_23 - 0.4);
  if ((x_29 < 0.0)) {
    discard;
  };
  _color_22.xyz = (c_7.xyz + (_FadeColor.xyz * tmpvar_26));
  lowp vec4 _color_30;
  _color_30.w = _color_22.w;
  _color_30.xyz = (_color_22.xyz + (tmpvar_6 * _RimColor.xyz));
  c_7.w = _color_30.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_30.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD7;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_2;
highp vec4 t3;
mediump vec3 t16_4;
highp vec3 t5;
mediump float t16_8;
highp float t18;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t1.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t1;
    t1.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t1.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t1;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = sqrt(t18);
    t16_2 = (-t18) + acid_Fog.y;
    t16_8 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_8;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_8 = t16_2 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_8;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = inversesqrt(t18);
    t1.xyz = vec3(t18) * t1.xyz;
    t3.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t2.xyz = vec3(t18) * t3.xyz;
    t3.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t3.xyz = vec3(t18) * t3.xyz;
    t16_4.xyz = t2.zxy * t3.yzx;
    t16_4.xyz = t2.yzx * t3.zxy + (-t16_4.xyz);
    t5.xyz = t16_4.xyz * in_TANGENT0.www;
    t18 = dot(t5.xyz, t5.xyz);
    t18 = inversesqrt(t18);
    t5.xyz = vec3(t18) * t5.xyz;
    t1.w = t5.x;
    vs_TEXCOORD3 = t1;
    t2.w = t5.y;
    t3.w = t5.z;
    vs_TEXCOORD5 = t3;
    vs_TEXCOORD4 = t2;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0.w = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t1.zz + t1.xw;
    vs_TEXCOORD7.xyz = t0.xyz;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _RasterTex;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec3 vs_TEXCOORD7;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
bool tb1;
mediump vec4 t16_2;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
mediump vec3 t16_6;
mediump vec3 t16_10;
mediump float t16_17;
mediump float t16_21;
void main()
{
    t16_0.xy = vs_TEXCOORD7.xy / vs_TEXCOORD7.zz;
    t16_21 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_21<0.0;
    t16_21 = (-t16_21) * 2.5 + 1.0;
    t16_21 = clamp(t16_21, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_21;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1.x = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1.x + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = vs_TEXCOORD5.y;
    t16_0.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = t16_1.y * t16_1.y;
    t16_0.x = t16_1.x * t16_1.x + (-t16_0.x);
    t16_3.xy = vs_TEXCOORD5.zw;
    t16_3.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_2.xyz, t16_3.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_3.x = dot(unity_SHBr, t16_2);
    t16_3.y = dot(unity_SHBg, t16_2);
    t16_3.z = dot(unity_SHBb, t16_2);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_3.xyz;
    t16_1.w = 1.0;
    t16_3.x = dot(unity_SHAr, t16_1);
    t16_3.y = dot(unity_SHAg, t16_1);
    t16_3.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_3.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t16_3.x = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_10.x = t16_3.x + t16_3.x;
    t16_3.x = (-t16_3.x) + 1.0;
    t16_10.xyz = t16_1.xyz * (-t16_10.xxx) + vs_TEXCOORD3.xyz;
    t16_10.x = dot(t16_10.xyz, (-t16_1.xyz));
    t16_10.y = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_10.xy = max(t16_10.xy, vec2(0.25, 0.0));
    t16_10.x = t16_10.x * _PropGlossiness.w;
    t16_10.x = t16_10.x * 10.0;
    t16_4.xyz = t16_10.xxx * _PropGlossiness.xyz;
    t16_4.xyz = t16_0.xyz * t16_4.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_4.xyz = t10_1.www * t16_4.xyz;
    t16_4.xyz = t10_1.xyz * t16_4.xyz;
    t16_0.xyz = t16_0.xyz * t10_1.xyz;
    t16_10.x = (-t10_1.w) + 1.0;
    t16_0.xyz = t16_0.xyz * t16_10.xxx;
    t10_5 = (-_LightDir.w) + 1.0;
    t16_10.x = t16_10.y * _LightDir.w + t10_5;
    t16_17 = t16_10.x * 1.5;
    t16_17 = min(t16_17, 1.0);
    t16_6.xyz = t16_10.xxx + (-_CharacterShadowColor.xyz);
    t16_10.x = t16_10.x * _LightParameters.y;
    t16_6.xyz = vec3(t16_17) * t16_6.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_6.xyz + t16_4.xyz;
    t16_17 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_4.xyz = vec3(t16_17) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = t16_10.xxx * t16_4.xyz + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_21) + t16_0.xyz;
    t16_0.xyz = t16_3.xxx * _RimColor.xyz + t16_0.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_9.xyz;
  tmpvar_2 = tmpvar_9.w;
  mediump vec3 tangentNormal_10;
  mediump vec3 tmpvar_11;
  tmpvar_11.x = xlv_TEXCOORD3.w;
  tmpvar_11.y = xlv_TEXCOORD4.w;
  tmpvar_11.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_12;
  mediump vec3 tmpvar_13;
  tmpvar_12 = xlv_TEXCOORD5.xyz;
  tmpvar_13 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_11.x;
  tmpvar_14[0].z = tmpvar_13.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_11.y;
  tmpvar_14[1].z = tmpvar_13.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_11.z;
  tmpvar_14[2].z = tmpvar_13.z;
  lowp vec3 tmpvar_15;
  tmpvar_15 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_10 = tmpvar_15;
  tmpvar_4 = (tangentNormal_10 * tmpvar_14);
  mediump vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = tmpvar_4;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  x1_18.x = dot (unity_SHAr, tmpvar_16);
  x1_18.y = dot (unity_SHAg, tmpvar_16);
  x1_18.z = dot (unity_SHAb, tmpvar_16);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_17.x = dot (unity_SHBr, tmpvar_19);
  x2_17.y = dot (unity_SHBg, tmpvar_19);
  x2_17.z = dot (unity_SHBb, tmpvar_19);
  mediump vec3 tmpvar_20;
  tmpvar_20 = max (((1.055 * 
    pow ((((x2_17 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_18) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = (1.0 - dot (tmpvar_4, xlv_TEXCOORD3.xyz));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_20 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_20)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_21;
  lowp float tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_22 = tmpvar_23;
  lowp vec3 tmpvar_24;
  tmpvar_24 = vec3(tmpvar_22);
  atten_21 = tmpvar_24;
  lit_color_8 = (lit_color_8 * max ((_Shadows.x + 
    (atten_21 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_25;
  tmpvar_25 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_7.xyz = tmpvar_25;
  c_7.w = _RimColor.w;
  lowp vec4 _color_26;
  _color_26 = (c_7 * _FadeColor);
  lowp vec4 _color_27;
  _color_27.w = _color_26.w;
  _color_27.xyz = (_color_26.xyz + (tmpvar_6 * _RimColor.xyz));
  c_7.w = _color_27.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_27.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_9.xyz;
  tmpvar_2 = tmpvar_9.w;
  mediump vec3 tangentNormal_10;
  mediump vec3 tmpvar_11;
  tmpvar_11.x = xlv_TEXCOORD3.w;
  tmpvar_11.y = xlv_TEXCOORD4.w;
  tmpvar_11.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_12;
  mediump vec3 tmpvar_13;
  tmpvar_12 = xlv_TEXCOORD5.xyz;
  tmpvar_13 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_11.x;
  tmpvar_14[0].z = tmpvar_13.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_11.y;
  tmpvar_14[1].z = tmpvar_13.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_11.z;
  tmpvar_14[2].z = tmpvar_13.z;
  lowp vec3 tmpvar_15;
  tmpvar_15 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_10 = tmpvar_15;
  tmpvar_4 = (tangentNormal_10 * tmpvar_14);
  mediump vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = tmpvar_4;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  x1_18.x = dot (unity_SHAr, tmpvar_16);
  x1_18.y = dot (unity_SHAg, tmpvar_16);
  x1_18.z = dot (unity_SHAb, tmpvar_16);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_17.x = dot (unity_SHBr, tmpvar_19);
  x2_17.y = dot (unity_SHBg, tmpvar_19);
  x2_17.z = dot (unity_SHBb, tmpvar_19);
  mediump vec3 tmpvar_20;
  tmpvar_20 = max (((1.055 * 
    pow ((((x2_17 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_18) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = (1.0 - dot (tmpvar_4, xlv_TEXCOORD3.xyz));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_20 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_20)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_21;
  lowp float tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_22 = tmpvar_23;
  lowp vec3 tmpvar_24;
  tmpvar_24 = vec3(tmpvar_22);
  atten_21 = tmpvar_24;
  lit_color_8 = (lit_color_8 * max ((_Shadows.x + 
    (atten_21 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_25;
  tmpvar_25 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_7.xyz = tmpvar_25;
  c_7.w = _RimColor.w;
  lowp vec4 _color_26;
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_28;
  tmpvar_28 = (_FadeColor.w - ((0.9375 * tmpvar_27.y) + 0.03125));
  mediump float tmpvar_29;
  tmpvar_29 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_28)
  )), 0.0, 1.0);
  _color_26.xyz = (_FadeColor.xyz * (tmpvar_29 + c_7.xyz));
  _color_26.w = ((32.0 * tmpvar_28) + 0.9375);
  lowp vec4 _color_30;
  _color_30.w = _color_26.w;
  _color_30.xyz = (_color_26.xyz + (tmpvar_6 * _RimColor.xyz));
  c_7.w = _color_30.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_30.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  highp vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  mediump vec2 tmpvar_10;
  mediump vec3 tmpvar_11;
  mediump vec4 tmpvar_12;
  tmpvar_10.x = 0.0;
  tmpvar_7 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_7 * 0.5);
  highp vec2 tmpvar_15;
  tmpvar_15.x = tmpvar_14.x;
  tmpvar_15.y = (tmpvar_14.y * _ProjectionParams.x);
  o_13.xy = (tmpvar_15 + tmpvar_14.w);
  o_13.zw = tmpvar_7.zw;
  tmpvar_11 = o_13.xyw;
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_8.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_9.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_9.zw = tmpvar_1.xy;
  highp vec3 tmpvar_16;
  highp vec4 tmpvar_17;
  tmpvar_17 = (_Object2World * _glesVertex);
  tmpvar_16 = normalize((_WorldSpaceCameraPos - tmpvar_17.xyz));
  worldViewDir_6 = tmpvar_16;
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_18 * _glesNormal));
  worldNormal_5 = tmpvar_19;
  highp mat3 tmpvar_20;
  tmpvar_20[0] = _Object2World[0].xyz;
  tmpvar_20[1] = _Object2World[1].xyz;
  tmpvar_20[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_21;
  mediump vec3 tmpvar_22;
  tmpvar_22 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_23;
  tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_23;
  mediump vec4 tmpvar_24;
  tmpvar_24.xyz = worldViewDir_6;
  tmpvar_24.w = worldBinormal_3.x;
  mediump vec4 tmpvar_25;
  tmpvar_25.xyz = worldNormal_5;
  tmpvar_25.w = worldBinormal_3.y;
  mediump vec4 tmpvar_26;
  tmpvar_26.xyz = worldTangent_4;
  tmpvar_26.w = worldBinormal_3.z;
  tmpvar_12 = (unity_World2Shadow[0] * tmpvar_17);
  highp float tmpvar_27;
  highp vec3 x_28;
  x_28 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_27 = sqrt(dot (x_28, x_28));
  fogpos_2 = tmpvar_27;
  tmpvar_10.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_10.x = (1.0 - tmpvar_10.y);
  gl_Position = tmpvar_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = tmpvar_10;
  xlv_TEXCOORD3 = tmpvar_24;
  xlv_TEXCOORD4 = tmpvar_25;
  xlv_TEXCOORD5 = tmpvar_26;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_11;
  xlv_TEXCOORD8 = tmpvar_12;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_9.xyz;
  tmpvar_2 = tmpvar_9.w;
  mediump vec3 tangentNormal_10;
  mediump vec3 tmpvar_11;
  tmpvar_11.x = xlv_TEXCOORD3.w;
  tmpvar_11.y = xlv_TEXCOORD4.w;
  tmpvar_11.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_12;
  mediump vec3 tmpvar_13;
  tmpvar_12 = xlv_TEXCOORD5.xyz;
  tmpvar_13 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_11.x;
  tmpvar_14[0].z = tmpvar_13.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_11.y;
  tmpvar_14[1].z = tmpvar_13.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_11.z;
  tmpvar_14[2].z = tmpvar_13.z;
  lowp vec3 tmpvar_15;
  tmpvar_15 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_10 = tmpvar_15;
  tmpvar_4 = (tangentNormal_10 * tmpvar_14);
  mediump vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = tmpvar_4;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  x1_18.x = dot (unity_SHAr, tmpvar_16);
  x1_18.y = dot (unity_SHAg, tmpvar_16);
  x1_18.z = dot (unity_SHAb, tmpvar_16);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_17.x = dot (unity_SHBr, tmpvar_19);
  x2_17.y = dot (unity_SHBg, tmpvar_19);
  x2_17.z = dot (unity_SHBb, tmpvar_19);
  mediump vec3 tmpvar_20;
  tmpvar_20 = max (((1.055 * 
    pow ((((x2_17 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_18) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = (1.0 - dot (tmpvar_4, xlv_TEXCOORD3.xyz));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_20 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_20)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_21;
  lowp float tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD8).x > 
    (xlv_TEXCOORD8.z / xlv_TEXCOORD8.w)
  )), _LightShadowData.x);
  tmpvar_22 = tmpvar_23;
  lowp vec3 tmpvar_24;
  tmpvar_24 = vec3(tmpvar_22);
  atten_21 = tmpvar_24;
  lit_color_8 = (lit_color_8 * max ((_Shadows.x + 
    (atten_21 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_25;
  tmpvar_25 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_7.xyz = tmpvar_25;
  c_7.w = _RimColor.w;
  lowp vec4 _color_26;
  _color_26.w = c_7.w;
  mediump float rTest_27;
  mediump vec2 tmpvar_28;
  tmpvar_28 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_29;
  tmpvar_29 = ((1.5 * _FadeColor.w) - tmpvar_28.y);
  if ((tmpvar_29 < 0.0)) {
    discard;
  };
  mediump float tmpvar_30;
  tmpvar_30 = clamp ((1.0 - (tmpvar_29 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_31;
  tmpvar_31.x = tmpvar_28.x;
  tmpvar_31.y = clamp ((tmpvar_30 - tmpvar_28.y), 0.0, 1.0);
  lowp float tmpvar_32;
  tmpvar_32 = texture2D (_RasterTex, tmpvar_31).z;
  rTest_27 = tmpvar_32;
  mediump float x_33;
  x_33 = (rTest_27 - 0.4);
  if ((x_33 < 0.0)) {
    discard;
  };
  _color_26.xyz = (c_7.xyz + (_FadeColor.xyz * tmpvar_30));
  lowp vec4 _color_34;
  _color_34.w = _color_26.w;
  _color_34.xyz = (_color_26.xyz + (tmpvar_6 * _RimColor.xyz));
  c_7.w = _color_34.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_34.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_9.xyz;
  tmpvar_2 = tmpvar_9.w;
  mediump vec3 tangentNormal_10;
  mediump vec3 tmpvar_11;
  tmpvar_11.x = xlv_TEXCOORD3.w;
  tmpvar_11.y = xlv_TEXCOORD4.w;
  tmpvar_11.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_12;
  mediump vec3 tmpvar_13;
  tmpvar_12 = xlv_TEXCOORD5.xyz;
  tmpvar_13 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_11.x;
  tmpvar_14[0].z = tmpvar_13.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_11.y;
  tmpvar_14[1].z = tmpvar_13.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_11.z;
  tmpvar_14[2].z = tmpvar_13.z;
  lowp vec3 tmpvar_15;
  tmpvar_15 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_10 = tmpvar_15;
  tmpvar_4 = (tangentNormal_10 * tmpvar_14);
  mediump vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = tmpvar_4;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  x1_18.x = dot (unity_SHAr, tmpvar_16);
  x1_18.y = dot (unity_SHAg, tmpvar_16);
  x1_18.z = dot (unity_SHAb, tmpvar_16);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_17.x = dot (unity_SHBr, tmpvar_19);
  x2_17.y = dot (unity_SHBg, tmpvar_19);
  x2_17.z = dot (unity_SHBb, tmpvar_19);
  mediump vec3 tmpvar_20;
  tmpvar_20 = max (((1.055 * 
    pow ((((x2_17 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_18) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = (1.0 - dot (tmpvar_4, xlv_TEXCOORD3.xyz));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_20 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_20)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_21;
  lowp float shadow_22;
  shadow_22 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_23;
  tmpvar_23 = vec3(shadow_22);
  atten_21 = tmpvar_23;
  lit_color_8 = (lit_color_8 * max ((_Shadows.x + 
    (atten_21 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_24;
  tmpvar_24 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_7.xyz = tmpvar_24;
  c_7.w = _RimColor.w;
  lowp vec4 _color_25;
  _color_25 = (c_7 * _FadeColor);
  lowp vec4 _color_26;
  _color_26.w = _color_25.w;
  _color_26.xyz = (_color_25.xyz + (tmpvar_6 * _RimColor.xyz));
  c_7.w = _color_26.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_26.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1 = t0.yyyy * unity_World2Shadow[0][1];
    t1 = unity_World2Shadow[0][0] * t0.xxxx + t1;
    t1 = unity_World2Shadow[0][2] * t0.zzzz + t1;
    t0 = unity_World2Shadow[0][3] * t0.wwww + t1;
    vs_TEXCOORD8 = t0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
mediump vec3 t16_5;
mediump vec3 t16_6;
mediump float t16_9;
mediump float t16_18;
mediump float t16_21;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_18 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_21 = t16_18 + t16_18;
    t16_18 = (-t16_18) + 1.0;
    t16_4.xyz = vec3(t16_18) * _RimColor.xyz;
    t16_5.xyz = t16_0.xyz * (-vec3(t16_21)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_5.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_6.x = t16_0.y * _PropGlossiness.w;
    t16_6.x = t16_6.x * 10.0;
    t16_6.xyz = t16_6.xxx * _PropGlossiness.xyz;
    t16_6.xyz = t16_3.xyz * t16_6.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_6.xyz = t16_6.xyz * t10_1.www;
    t16_6.xyz = t10_1.xyz * t16_6.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_21 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_21) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_21 = t16_0.x * 1.5;
    t16_21 = min(t16_21, 1.0);
    t16_5.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_5.xyz = vec3(t16_21) * t16_5.xyz + _CharacterShadowColor.xyz;
    t16_6.xyz = t16_3.xyz * t16_5.xyz + t16_6.xyz;
    vec3 txVec24 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_3.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec24, 0.0);
    t16_9 = (-_LightShadowData.x) + 1.0;
    t16_3.x = t16_3.x * t16_9 + _LightShadowData.x;
    t16_3.x = t16_3.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_3.x = max(t16_3.x, 0.0);
    t16_6.xyz = t16_6.xyz * t16_3.xxx;
    t16_3.x = dot(t16_6.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_6.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_6.xyz;
    t16_0.xyz = t16_0.xyz * _FadeColor.xyz + t16_4.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w * _FadeColor.w;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  mediump vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec2 tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_9.x = 0.0;
  tmpvar_7.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_7.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_8.zw = tmpvar_1.xy;
  highp vec3 tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Object2World * _glesVertex);
  tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_12.xyz));
  worldViewDir_6 = tmpvar_11;
  highp mat3 tmpvar_13;
  tmpvar_13[0] = _Object2World[0].xyz;
  tmpvar_13[1] = _Object2World[1].xyz;
  tmpvar_13[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((tmpvar_13 * _glesNormal));
  worldNormal_5 = tmpvar_14;
  highp mat3 tmpvar_15;
  tmpvar_15[0] = _Object2World[0].xyz;
  tmpvar_15[1] = _Object2World[1].xyz;
  tmpvar_15[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_16;
  tmpvar_16 = normalize((tmpvar_15 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_17 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19.xyz = worldViewDir_6;
  tmpvar_19.w = worldBinormal_3.x;
  mediump vec4 tmpvar_20;
  tmpvar_20.xyz = worldNormal_5;
  tmpvar_20.w = worldBinormal_3.y;
  mediump vec4 tmpvar_21;
  tmpvar_21.xyz = worldTangent_4;
  tmpvar_21.w = worldBinormal_3.z;
  tmpvar_10 = (unity_World2Shadow[0] * tmpvar_12);
  highp float tmpvar_22;
  highp vec3 x_23;
  x_23 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_22 = sqrt(dot (x_23, x_23));
  fogpos_2 = tmpvar_22;
  tmpvar_9.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_9.x = (1.0 - tmpvar_9.y);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = tmpvar_8;
  xlv_TEXCOORD2 = tmpvar_9;
  xlv_TEXCOORD3 = tmpvar_19;
  xlv_TEXCOORD4 = tmpvar_20;
  xlv_TEXCOORD5 = tmpvar_21;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD8 = tmpvar_10;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _NoiseTexture;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_9.xyz;
  tmpvar_2 = tmpvar_9.w;
  mediump vec3 tangentNormal_10;
  mediump vec3 tmpvar_11;
  tmpvar_11.x = xlv_TEXCOORD3.w;
  tmpvar_11.y = xlv_TEXCOORD4.w;
  tmpvar_11.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_12;
  mediump vec3 tmpvar_13;
  tmpvar_12 = xlv_TEXCOORD5.xyz;
  tmpvar_13 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_11.x;
  tmpvar_14[0].z = tmpvar_13.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_11.y;
  tmpvar_14[1].z = tmpvar_13.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_11.z;
  tmpvar_14[2].z = tmpvar_13.z;
  lowp vec3 tmpvar_15;
  tmpvar_15 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_10 = tmpvar_15;
  tmpvar_4 = (tangentNormal_10 * tmpvar_14);
  mediump vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = tmpvar_4;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  x1_18.x = dot (unity_SHAr, tmpvar_16);
  x1_18.y = dot (unity_SHAg, tmpvar_16);
  x1_18.z = dot (unity_SHAb, tmpvar_16);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_17.x = dot (unity_SHBr, tmpvar_19);
  x2_17.y = dot (unity_SHBg, tmpvar_19);
  x2_17.z = dot (unity_SHBb, tmpvar_19);
  mediump vec3 tmpvar_20;
  tmpvar_20 = max (((1.055 * 
    pow ((((x2_17 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_18) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = (1.0 - dot (tmpvar_4, xlv_TEXCOORD3.xyz));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_20 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_20)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_21;
  lowp float shadow_22;
  shadow_22 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_23;
  tmpvar_23 = vec3(shadow_22);
  atten_21 = tmpvar_23;
  lit_color_8 = (lit_color_8 * max ((_Shadows.x + 
    (atten_21 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_24;
  tmpvar_24 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_7.xyz = tmpvar_24;
  c_7.w = _RimColor.w;
  lowp vec4 _color_25;
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture2D (_NoiseTexture, xlv_TEXCOORD1.zw);
  mediump float tmpvar_27;
  tmpvar_27 = (_FadeColor.w - ((0.9375 * tmpvar_26.y) + 0.03125));
  mediump float tmpvar_28;
  tmpvar_28 = clamp ((1.0 - (32.0 * 
    abs(tmpvar_27)
  )), 0.0, 1.0);
  _color_25.xyz = (_FadeColor.xyz * (tmpvar_28 + c_7.xyz));
  _color_25.w = ((32.0 * tmpvar_27) + 0.9375);
  lowp vec4 _color_29;
  _color_29.w = _color_25.w;
  _color_29.xyz = (_color_25.xyz + (tmpvar_6 * _RimColor.xyz));
  c_7.w = _color_29.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_29.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
mediump float t16_1;
highp vec4 t2;
mediump vec3 t16_3;
highp vec3 t4;
mediump float t16_6;
highp float t15;
highp float t17;
highp float t19;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    t0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t0.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t0;
    t0.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t0.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t0;
    t0.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t0.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t0.xyz;
    t0.x = dot(t0.xyz, t0.xyz);
    t0.x = sqrt(t0.x);
    t16_1 = (-t0.x) + acid_Fog.y;
    t16_6 = (-acid_Fog.x) + acid_Fog.y;
    t16_1 = t16_1 / t16_6;
    t16_1 = clamp(t16_1, 0.0, 1.0);
    t16_1 = (-t16_1) + 1.0;
    t16_6 = t16_1 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_1) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_6;
    t0.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t0.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t0.xyz;
    t0.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t0.xyz;
    t0.xyz = _Object2World[3].xyz * in_POSITION0.www + t0.xyz;
    t0.xyz = (-t0.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t15 = dot(t0.xyz, t0.xyz);
    t15 = inversesqrt(t15);
    t0.xyz = vec3(t15) * t0.xyz;
    t2.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t1.xyz = vec3(t17) * t2.xyz;
    t2.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t2.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t2.xyz;
    t2.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t2.xyz;
    t17 = dot(t2.xyz, t2.xyz);
    t17 = inversesqrt(t17);
    t2.xyz = vec3(t17) * t2.xyz;
    t16_3.xyz = t1.zxy * t2.yzx;
    t16_3.xyz = t1.yzx * t2.zxy + (-t16_3.xyz);
    t4.xyz = t16_3.xyz * in_TANGENT0.www;
    t19 = dot(t4.xyz, t4.xyz);
    t19 = inversesqrt(t19);
    t4.xyz = vec3(t19) * t4.xyz;
    t0.w = t4.x;
    vs_TEXCOORD3 = t0;
    t1.w = t4.y;
    t2.w = t4.z;
    vs_TEXCOORD5 = t2;
    vs_TEXCOORD4 = t1;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1 = t0.yyyy * unity_World2Shadow[0][1];
    t1 = unity_World2Shadow[0][0] * t0.xxxx + t1;
    t1 = unity_World2Shadow[0][2] * t0.zzzz + t1;
    t0 = unity_World2Shadow[0][3] * t0.wwww + t1;
    vs_TEXCOORD8 = t0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _NoiseTexture;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec4 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
mediump vec3 t16_5;
lowp float t10_6;
mediump vec3 t16_7;
mediump float t16_10;
mediump float t16_21;
mediump float t16_24;
void main()
{
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_0.x = dot(t10_2.xyz, t16_0.xyz);
    t16_3.x = vs_TEXCOORD5.y;
    t16_3.yz = vs_TEXCOORD4.wy;
    t16_0.y = dot(t10_2.xyz, t16_3.xyz);
    t16_3.x = t16_0.y * t16_0.y;
    t16_3.x = t16_0.x * t16_0.x + (-t16_3.x);
    t16_4.xy = vs_TEXCOORD5.zw;
    t16_4.z = vs_TEXCOORD4.z;
    t16_0.z = dot(t10_2.xyz, t16_4.xyz);
    t16_1 = t16_0.yzzx * t16_0.xyzz;
    t16_4.x = dot(unity_SHBr, t16_1);
    t16_4.y = dot(unity_SHBg, t16_1);
    t16_4.z = dot(unity_SHBb, t16_1);
    t16_3.xyz = unity_SHC.xyz * t16_3.xxx + t16_4.xyz;
    t16_0.w = 1.0;
    t16_4.x = dot(unity_SHAr, t16_0);
    t16_4.y = dot(unity_SHAg, t16_0);
    t16_4.z = dot(unity_SHAb, t16_0);
    t16_3.xyz = t16_3.xyz + t16_4.xyz;
    t16_3.xyz = t16_3.xyz * _LightParameters.xxx;
    t16_3.xyz = log2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_3.xyz = exp2(t16_3.xyz);
    t16_3.xyz = t16_3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_3.xyz = max(t16_3.xyz, vec3(0.0, 0.0, 0.0));
    t16_21 = dot(vs_TEXCOORD3.xyz, t16_0.xyz);
    t16_24 = t16_21 + t16_21;
    t16_21 = (-t16_21) + 1.0;
    t16_4.xyz = vec3(t16_21) * _RimColor.xyz;
    t16_5.xyz = t16_0.xyz * (-vec3(t16_24)) + vs_TEXCOORD3.xyz;
    t16_0.w = dot(t16_5.xyz, (-t16_0.xyz));
    t16_0.x = dot(t16_0.xyz, (-_LightDir.xyz));
    t16_0.xy = max(t16_0.xw, vec2(0.0, 0.25));
    t16_7.x = t16_0.y * _PropGlossiness.w;
    t16_7.x = t16_7.x * 10.0;
    t16_7.xyz = t16_7.xxx * _PropGlossiness.xyz;
    t16_7.xyz = t16_3.xyz * t16_7.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_7.xyz = t16_7.xyz * t10_1.www;
    t16_7.xyz = t10_1.xyz * t16_7.xyz;
    t16_3.xyz = t10_1.xyz * t16_3.xyz;
    t16_24 = (-t10_1.w) + 1.0;
    t16_3.xyz = vec3(t16_24) * t16_3.xyz;
    t10_2.x = (-_LightDir.w) + 1.0;
    t16_0.x = t16_0.x * _LightDir.w + t10_2.x;
    t16_24 = t16_0.x * 1.5;
    t16_24 = min(t16_24, 1.0);
    t16_5.xyz = t16_0.xxx + (-_CharacterShadowColor.xyz);
    t16_0.x = t16_0.x * _LightParameters.y;
    t16_5.xyz = vec3(t16_24) * t16_5.xyz + _CharacterShadowColor.xyz;
    t16_7.xyz = t16_3.xyz * t16_5.xyz + t16_7.xyz;
    vec3 txVec26 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_3.x = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec26, 0.0);
    t16_10 = (-_LightShadowData.x) + 1.0;
    t16_3.x = t16_3.x * t16_10 + _LightShadowData.x;
    t16_3.x = t16_3.x * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_3.x = max(t16_3.x, 0.0);
    t16_7.xyz = t16_7.xyz * t16_3.xxx;
    t16_3.x = dot(t16_7.xyz, vec3(1.0, 1.0, 1.0));
    t16_3.xyz = t16_3.xxx * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_7.xyz);
    t16_0.xyz = t16_0.xxx * t16_3.xyz + t16_7.xyz;
    t10_6 = texture(_NoiseTexture, vs_TEXCOORD1.zw).y;
    t16_21 = t10_6 * 0.9375 + 0.03125;
    t16_21 = (-t16_21) + _FadeColor.w;
    t16_3.x = -abs(t16_21) * 32.0 + 1.0;
    t16_1.w = t16_21 * 32.0 + 0.9375;
    t16_21 = max(t16_3.x, 0.0);
    t16_0.xyz = t16_0.xyz + vec3(t16_21);
    t16_0.xyz = _FadeColor.xyz * t16_0.xyz + t16_4.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_1.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0 = t16_1;
    return;
}

#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES
#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform mediump vec4 acid_Fog;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _BumpMap_ST;
uniform mediump vec4 _MaskTex_ST;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  mediump float fogpos_2;
  mediump vec3 worldBinormal_3;
  mediump vec3 worldTangent_4;
  mediump vec3 worldNormal_5;
  mediump vec3 worldViewDir_6;
  highp vec4 tmpvar_7;
  mediump vec4 tmpvar_8;
  mediump vec4 tmpvar_9;
  mediump vec2 tmpvar_10;
  mediump vec3 tmpvar_11;
  mediump vec4 tmpvar_12;
  tmpvar_10.x = 0.0;
  tmpvar_7 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_7 * 0.5);
  highp vec2 tmpvar_15;
  tmpvar_15.x = tmpvar_14.x;
  tmpvar_15.y = (tmpvar_14.y * _ProjectionParams.x);
  o_13.xy = (tmpvar_15 + tmpvar_14.w);
  o_13.zw = tmpvar_7.zw;
  tmpvar_11 = o_13.xyw;
  tmpvar_8.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_8.zw = ((_glesMultiTexCoord0.xy * _MaskTex_ST.xy) + _MaskTex_ST.zw);
  tmpvar_9.xy = ((_glesMultiTexCoord0.xy * _BumpMap_ST.xy) + _BumpMap_ST.zw);
  tmpvar_9.zw = tmpvar_1.xy;
  highp vec3 tmpvar_16;
  highp vec4 tmpvar_17;
  tmpvar_17 = (_Object2World * _glesVertex);
  tmpvar_16 = normalize((_WorldSpaceCameraPos - tmpvar_17.xyz));
  worldViewDir_6 = tmpvar_16;
  highp mat3 tmpvar_18;
  tmpvar_18[0] = _Object2World[0].xyz;
  tmpvar_18[1] = _Object2World[1].xyz;
  tmpvar_18[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_18 * _glesNormal));
  worldNormal_5 = tmpvar_19;
  highp mat3 tmpvar_20;
  tmpvar_20[0] = _Object2World[0].xyz;
  tmpvar_20[1] = _Object2World[1].xyz;
  tmpvar_20[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_20 * _glesTANGENT.xyz));
  worldTangent_4 = tmpvar_21;
  mediump vec3 tmpvar_22;
  tmpvar_22 = ((worldNormal_5.yzx * worldTangent_4.zxy) - (worldNormal_5.zxy * worldTangent_4.yzx));
  highp vec3 tmpvar_23;
  tmpvar_23 = normalize((tmpvar_22 * _glesTANGENT.w));
  worldBinormal_3 = tmpvar_23;
  mediump vec4 tmpvar_24;
  tmpvar_24.xyz = worldViewDir_6;
  tmpvar_24.w = worldBinormal_3.x;
  mediump vec4 tmpvar_25;
  tmpvar_25.xyz = worldNormal_5;
  tmpvar_25.w = worldBinormal_3.y;
  mediump vec4 tmpvar_26;
  tmpvar_26.xyz = worldTangent_4;
  tmpvar_26.w = worldBinormal_3.z;
  tmpvar_12 = (unity_World2Shadow[0] * tmpvar_17);
  highp float tmpvar_27;
  highp vec3 x_28;
  x_28 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  tmpvar_27 = sqrt(dot (x_28, x_28));
  fogpos_2 = tmpvar_27;
  tmpvar_10.y = ((1.0 - clamp (
    ((acid_Fog.y - fogpos_2) / (acid_Fog.y - acid_Fog.x))
  , 0.0, 1.0)) * acid_Fog.z);
  tmpvar_10.x = (1.0 - tmpvar_10.y);
  gl_Position = tmpvar_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = tmpvar_10;
  xlv_TEXCOORD3 = tmpvar_24;
  xlv_TEXCOORD4 = tmpvar_25;
  xlv_TEXCOORD5 = tmpvar_26;
  xlv_TEXCOORD6 = vec3(0.0, 0.0, 0.0);
  xlv_TEXCOORD7 = tmpvar_11;
  xlv_TEXCOORD8 = tmpvar_12;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 unity_SHAr;
uniform mediump vec4 unity_SHAg;
uniform mediump vec4 unity_SHAb;
uniform mediump vec4 unity_SHBr;
uniform mediump vec4 unity_SHBg;
uniform mediump vec4 unity_SHBb;
uniform mediump vec4 unity_SHC;
uniform mediump vec4 _LightShadowData;
uniform lowp vec4 unity_FogColor;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform sampler2D _RasterTex;
uniform lowp vec4 _RimColor;
uniform mediump vec2 _Shadows;
uniform lowp vec4 _LightDir;
uniform lowp vec4 _LightParameters;
uniform lowp vec4 _CharacterShadowColor;
uniform mediump vec4 _PropGlossiness;
uniform lowp vec4 _FadeColor;
mediump vec3 tmpvar_1;
mediump float tmpvar_2;
mediump vec3 tmpvar_3;
mediump vec3 tmpvar_4;
mediump float tmpvar_5;
mediump float tmpvar_6;
varying mediump vec4 xlv_TEXCOORD0;
varying mediump vec4 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec4 xlv_TEXCOORD3;
varying mediump vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD7;
varying mediump vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 c_7;
  mediump vec3 lit_color_8;
  tmpvar_1 = -(_LightDir.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  tmpvar_3 = tmpvar_9.xyz;
  tmpvar_2 = tmpvar_9.w;
  mediump vec3 tangentNormal_10;
  mediump vec3 tmpvar_11;
  tmpvar_11.x = xlv_TEXCOORD3.w;
  tmpvar_11.y = xlv_TEXCOORD4.w;
  tmpvar_11.z = xlv_TEXCOORD5.w;
  mediump vec3 tmpvar_12;
  mediump vec3 tmpvar_13;
  tmpvar_12 = xlv_TEXCOORD5.xyz;
  tmpvar_13 = xlv_TEXCOORD4.xyz;
  mediump mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_11.x;
  tmpvar_14[0].z = tmpvar_13.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_11.y;
  tmpvar_14[1].z = tmpvar_13.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_11.z;
  tmpvar_14[2].z = tmpvar_13.z;
  lowp vec3 tmpvar_15;
  tmpvar_15 = ((texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0) - 1.0);
  tangentNormal_10 = tmpvar_15;
  tmpvar_4 = (tangentNormal_10 * tmpvar_14);
  mediump vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = tmpvar_4;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  x1_18.x = dot (unity_SHAr, tmpvar_16);
  x1_18.y = dot (unity_SHAg, tmpvar_16);
  x1_18.z = dot (unity_SHAb, tmpvar_16);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_4.xyzz * tmpvar_4.yzzx);
  x2_17.x = dot (unity_SHBr, tmpvar_19);
  x2_17.y = dot (unity_SHBg, tmpvar_19);
  x2_17.z = dot (unity_SHBb, tmpvar_19);
  mediump vec3 tmpvar_20;
  tmpvar_20 = max (((1.055 * 
    pow ((((x2_17 + 
      (unity_SHC.xyz * ((tmpvar_4.x * tmpvar_4.x) - (tmpvar_4.y * tmpvar_4.y)))
    ) + x1_18) * _LightParameters.x), vec3(0.4166667, 0.4166667, 0.4166667))
  ) - 0.055), vec3(0.0, 0.0, 0.0));
  tmpvar_6 = (1.0 - dot (tmpvar_4, xlv_TEXCOORD3.xyz));
  tmpvar_5 = ((max (
    dot (tmpvar_4, tmpvar_1)
  , 0.0) * _LightDir.w) + (1.0 - _LightDir.w));
  lit_color_8 = (((
    (tmpvar_20 * tmpvar_3)
   * 
    (1.0 - tmpvar_2)
  ) * mix (_CharacterShadowColor.xyz, vec3(tmpvar_5), vec3(
    min (1.0, (tmpvar_5 * 1.5))
  ))) + ((
    (((max (
      dot ((xlv_TEXCOORD3.xyz - (2.0 * (
        dot (tmpvar_4, xlv_TEXCOORD3.xyz)
       * tmpvar_4))), -(tmpvar_4))
    , 0.25) * (_PropGlossiness.w * 10.0)) * _PropGlossiness.xyz) * tmpvar_20)
   * tmpvar_2) * tmpvar_3));
  mediump vec3 atten_21;
  lowp float shadow_22;
  shadow_22 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD8.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec3 tmpvar_23;
  tmpvar_23 = vec3(shadow_22);
  atten_21 = tmpvar_23;
  lit_color_8 = (lit_color_8 * max ((_Shadows.x + 
    (atten_21 * _Shadows.y)
  ), vec3(0.0, 0.0, 0.0)));
  mediump vec3 tmpvar_24;
  tmpvar_24 = mix (lit_color_8, vec3((dot (lit_color_8, vec3(1.0, 1.0, 1.0)) * 0.3333333)), vec3((tmpvar_5 * _LightParameters.y)));
  c_7.xyz = tmpvar_24;
  c_7.w = _RimColor.w;
  lowp vec4 _color_25;
  _color_25.w = c_7.w;
  mediump float rTest_26;
  mediump vec2 tmpvar_27;
  tmpvar_27 = (xlv_TEXCOORD7.xy / xlv_TEXCOORD7.z);
  mediump float tmpvar_28;
  tmpvar_28 = ((1.5 * _FadeColor.w) - tmpvar_27.y);
  if ((tmpvar_28 < 0.0)) {
    discard;
  };
  mediump float tmpvar_29;
  tmpvar_29 = clamp ((1.0 - (tmpvar_28 * 2.5)), 0.0, 1.0);
  mediump vec2 tmpvar_30;
  tmpvar_30.x = tmpvar_27.x;
  tmpvar_30.y = clamp ((tmpvar_29 - tmpvar_27.y), 0.0, 1.0);
  lowp float tmpvar_31;
  tmpvar_31 = texture2D (_RasterTex, tmpvar_30).z;
  rTest_26 = tmpvar_31;
  mediump float x_32;
  x_32 = (rTest_26 - 0.4);
  if ((x_32 < 0.0)) {
    discard;
  };
  _color_25.xyz = (c_7.xyz + (_FadeColor.xyz * tmpvar_29));
  lowp vec4 _color_33;
  _color_33.w = _color_25.w;
  _color_33.xyz = (_color_25.xyz + (tmpvar_6 * _RimColor.xyz));
  c_7.w = _color_33.w;
  c_7.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD2.y) + (_color_33.xyz * xlv_TEXCOORD2.x));
  gl_FragData[0] = c_7;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out mediump vec4 vs_TEXCOORD0;
out mediump vec4 vs_TEXCOORD1;
out mediump vec2 vs_TEXCOORD2;
out mediump vec4 vs_TEXCOORD3;
out mediump vec4 vs_TEXCOORD4;
out mediump vec4 vs_TEXCOORD5;
out mediump vec3 vs_TEXCOORD6;
out mediump vec3 vs_TEXCOORD7;
out mediump vec4 vs_TEXCOORD8;
highp vec4 t0;
highp vec4 t1;
highp vec4 t2;
mediump float t16_2;
highp vec4 t3;
mediump vec3 t16_4;
highp vec3 t5;
mediump float t16_8;
highp float t18;
void main()
{
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    t0 = glstate_matrix_mvp[3].xywz * in_POSITION0.wwww + t0.xywz;
    gl_Position = t0.xywz;
    t1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    t1.zw = in_TEXCOORD0.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
    vs_TEXCOORD0 = t1;
    t1.xy = in_TEXCOORD0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
    t1.zw = in_TEXCOORD0.xy;
    vs_TEXCOORD1 = t1;
    t1.xyz = in_POSITION0.yyy * glstate_matrix_modelview0[1].xyz;
    t1.xyz = glstate_matrix_modelview0[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = glstate_matrix_modelview0[3].xyz * in_POSITION0.www + t1.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = sqrt(t18);
    t16_2 = (-t18) + acid_Fog.y;
    t16_8 = (-acid_Fog.x) + acid_Fog.y;
    t16_2 = t16_2 / t16_8;
    t16_2 = clamp(t16_2, 0.0, 1.0);
    t16_2 = (-t16_2) + 1.0;
    t16_8 = t16_2 * acid_Fog.z;
    vs_TEXCOORD2.x = (-t16_2) * acid_Fog.z + 1.0;
    vs_TEXCOORD2.y = t16_8;
    t1.xyz = in_POSITION0.yyy * _Object2World[1].xyz;
    t1.xyz = _Object2World[0].xyz * in_POSITION0.xxx + t1.xyz;
    t1.xyz = _Object2World[2].xyz * in_POSITION0.zzz + t1.xyz;
    t1.xyz = _Object2World[3].xyz * in_POSITION0.www + t1.xyz;
    t1.xyz = (-t1.xyz) + _WorldSpaceCameraPos.xyzx.xyz;
    t18 = dot(t1.xyz, t1.xyz);
    t18 = inversesqrt(t18);
    t1.xyz = vec3(t18) * t1.xyz;
    t3.xyz = in_NORMAL0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_NORMAL0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_NORMAL0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t2.xyz = vec3(t18) * t3.xyz;
    t3.xyz = in_TANGENT0.yyy * _Object2World[1].xyz;
    t3.xyz = _Object2World[0].xyz * in_TANGENT0.xxx + t3.xyz;
    t3.xyz = _Object2World[2].xyz * in_TANGENT0.zzz + t3.xyz;
    t18 = dot(t3.xyz, t3.xyz);
    t18 = inversesqrt(t18);
    t3.xyz = vec3(t18) * t3.xyz;
    t16_4.xyz = t2.zxy * t3.yzx;
    t16_4.xyz = t2.yzx * t3.zxy + (-t16_4.xyz);
    t5.xyz = t16_4.xyz * in_TANGENT0.www;
    t18 = dot(t5.xyz, t5.xyz);
    t18 = inversesqrt(t18);
    t5.xyz = vec3(t18) * t5.xyz;
    t1.w = t5.x;
    vs_TEXCOORD3 = t1;
    t2.w = t5.y;
    t3.w = t5.z;
    vs_TEXCOORD5 = t3;
    vs_TEXCOORD4 = t2;
    vs_TEXCOORD6.xyz = vec3(0.0, 0.0, 0.0);
    t0.w = t0.y * _ProjectionParams.x;
    t1.xzw = t0.xzw * vec3(0.5, 0.5, 0.5);
    t0.xy = t1.zz + t1.xw;
    vs_TEXCOORD7.xyz = t0.xyz;
    t0 = in_POSITION0.yyyy * _Object2World[1];
    t0 = _Object2World[0] * in_POSITION0.xxxx + t0;
    t0 = _Object2World[2] * in_POSITION0.zzzz + t0;
    t0 = _Object2World[3] * in_POSITION0.wwww + t0;
    t1 = t0.yyyy * unity_World2Shadow[0][1];
    t1 = unity_World2Shadow[0][0] * t0.xxxx + t1;
    t1 = unity_World2Shadow[0][2] * t0.zzzz + t1;
    t0 = unity_World2Shadow[0][3] * t0.wwww + t1;
    vs_TEXCOORD8 = t0;
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
struct g_pd_Type {
	mediump vec3 worldLightDir;
	mediump vec3 worldViewDir;
	mediump vec3 sh9;
	mediump vec2 uvMain;
	mediump vec2 uvBump;
	lowp vec4 albedoTex;
	lowp vec4 bumpTex;
	mediump float color0Scale;
	mediump float color1Scale;
	mediump float specularMask;
	mediump float ambientOcclusion;
	mediump vec3 albedo;
	mediump vec3 worldNormal;
	mediump float lightIntensity;
	mediump float glossIntensity;
	mediump float rimIntensity;
	mediump vec3 glossColor;
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
uniform 	lowp vec4 _LightColor0;
uniform 	lowp vec4 _SpecColor;
uniform 	mediump vec4 unity_LightGammaCorrectionConsts;
uniform 	lowp vec4 _RimColor;
uniform 	mediump vec2 _Shadows;
uniform 	lowp vec4 _LightDir;
uniform 	lowp vec4 _LightParameters;
uniform 	lowp vec4 _CharacterShadowColor;
uniform 	mediump vec4 acid_Fog;
uniform 	mediump vec4 _MainTex_ST;
uniform 	mediump vec4 _BumpMap_ST;
uniform 	mediump vec4 _MaskTex_ST;
uniform 	mediump vec4 _PropGlossiness;
uniform 	lowp vec4 _FadeColor;
uniform 	g_pd_Type g_pd;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _BumpMap;
uniform lowp sampler2D _RasterTex;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in mediump vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec2 vs_TEXCOORD2;
in mediump vec4 vs_TEXCOORD3;
in mediump vec4 vs_TEXCOORD4;
in mediump vec4 vs_TEXCOORD5;
in mediump vec3 vs_TEXCOORD7;
in mediump vec4 vs_TEXCOORD8;
layout(location = 0) out lowp vec4 SV_Target0;
mediump vec3 t16_0;
mediump vec4 t16_1;
lowp vec4 t10_1;
bool tb1;
mediump vec4 t16_2;
lowp vec3 t10_2;
mediump vec3 t16_3;
mediump vec3 t16_4;
lowp float t10_5;
mediump vec3 t16_6;
mediump vec3 t16_10;
mediump float t16_17;
mediump float t16_21;
mediump float t16_24;
void main()
{
    t16_0.xy = vs_TEXCOORD7.xy / vs_TEXCOORD7.zz;
    t16_21 = _FadeColor.w * 1.5 + (-t16_0.y);
    tb1 = t16_21<0.0;
    t16_21 = (-t16_21) * 2.5 + 1.0;
    t16_21 = clamp(t16_21, 0.0, 1.0);
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.z = (-t16_0.y) + t16_21;
    t16_0.z = clamp(t16_0.z, 0.0, 1.0);
    t10_1.x = texture(_RasterTex, t16_0.xz).z;
    t16_0.x = t10_1.x + -0.400000006;
    tb1 = t16_0.x<0.0;
    if((int(tb1) * int(0xffffffffu))!=0){discard;}
    t16_0.x = vs_TEXCOORD5.x;
    t16_0.y = vs_TEXCOORD3.w;
    t16_0.z = vs_TEXCOORD4.x;
    t10_1.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
    t10_2.xyz = t10_1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
    t16_1.x = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = vs_TEXCOORD5.y;
    t16_0.yz = vs_TEXCOORD4.wy;
    t16_1.y = dot(t10_2.xyz, t16_0.xyz);
    t16_0.x = t16_1.y * t16_1.y;
    t16_0.x = t16_1.x * t16_1.x + (-t16_0.x);
    t16_3.xy = vs_TEXCOORD5.zw;
    t16_3.z = vs_TEXCOORD4.z;
    t16_1.z = dot(t10_2.xyz, t16_3.xyz);
    t16_2 = t16_1.yzzx * t16_1.xyzz;
    t16_3.x = dot(unity_SHBr, t16_2);
    t16_3.y = dot(unity_SHBg, t16_2);
    t16_3.z = dot(unity_SHBb, t16_2);
    t16_0.xyz = unity_SHC.xyz * t16_0.xxx + t16_3.xyz;
    t16_1.w = 1.0;
    t16_3.x = dot(unity_SHAr, t16_1);
    t16_3.y = dot(unity_SHAg, t16_1);
    t16_3.z = dot(unity_SHAb, t16_1);
    t16_0.xyz = t16_0.xyz + t16_3.xyz;
    t16_0.xyz = t16_0.xyz * _LightParameters.xxx;
    t16_0.xyz = log2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
    t16_0.xyz = exp2(t16_0.xyz);
    t16_0.xyz = t16_0.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
    t16_0.xyz = max(t16_0.xyz, vec3(0.0, 0.0, 0.0));
    t16_3.x = dot(vs_TEXCOORD3.xyz, t16_1.xyz);
    t16_10.x = t16_3.x + t16_3.x;
    t16_3.x = (-t16_3.x) + 1.0;
    t16_10.xyz = t16_1.xyz * (-t16_10.xxx) + vs_TEXCOORD3.xyz;
    t16_10.x = dot(t16_10.xyz, (-t16_1.xyz));
    t16_10.y = dot(t16_1.xyz, (-_LightDir.xyz));
    t16_10.xy = max(t16_10.xy, vec2(0.25, 0.0));
    t16_10.x = t16_10.x * _PropGlossiness.w;
    t16_10.x = t16_10.x * 10.0;
    t16_4.xyz = t16_10.xxx * _PropGlossiness.xyz;
    t16_4.xyz = t16_0.xyz * t16_4.xyz;
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    t16_4.xyz = t10_1.www * t16_4.xyz;
    t16_4.xyz = t10_1.xyz * t16_4.xyz;
    t16_0.xyz = t16_0.xyz * t10_1.xyz;
    t16_10.x = (-t10_1.w) + 1.0;
    t16_0.xyz = t16_0.xyz * t16_10.xxx;
    t10_5 = (-_LightDir.w) + 1.0;
    t16_10.x = t16_10.y * _LightDir.w + t10_5;
    t16_17 = t16_10.x * 1.5;
    t16_17 = min(t16_17, 1.0);
    t16_6.xyz = t16_10.xxx + (-_CharacterShadowColor.xyz);
    t16_10.x = t16_10.x * _LightParameters.y;
    t16_6.xyz = vec3(t16_17) * t16_6.xyz + _CharacterShadowColor.xyz;
    t16_0.xyz = t16_0.xyz * t16_6.xyz + t16_4.xyz;
    vec3 txVec25 = vec3(vs_TEXCOORD8.xy,vs_TEXCOORD8.z);
    t16_17 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec25, 0.0);
    t16_24 = (-_LightShadowData.x) + 1.0;
    t16_17 = t16_17 * t16_24 + _LightShadowData.x;
    t16_17 = t16_17 * _Shadows.xyxx.y + _Shadows.xyxx.x;
    t16_17 = max(t16_17, 0.0);
    t16_0.xyz = t16_0.xyz * vec3(t16_17);
    t16_17 = dot(t16_0.xyz, vec3(1.0, 1.0, 1.0));
    t16_4.xyz = vec3(t16_17) * vec3(0.333333343, 0.333333343, 0.333333343) + (-t16_0.xyz);
    t16_0.xyz = t16_10.xxx * t16_4.xyz + t16_0.xyz;
    t16_0.xyz = _FadeColor.xyz * vec3(t16_21) + t16_0.xyz;
    t16_0.xyz = t16_3.xxx * _RimColor.xyz + t16_0.xyz;
    t16_0.xyz = t16_0.xyz * vs_TEXCOORD2.xxx;
    t16_0.xyz = unity_FogColor.xyz * vs_TEXCOORD2.yyy + t16_0.xyz;
    SV_Target0.xyz = t16_0.xyz;
    SV_Target0.w = _RimColor.w;
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_OFF" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_OFF" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_OFF" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_OFF" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_OFF" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_OFF" "RASTER_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_OFF" "RASTER_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_OFF" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_OFF" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_OFF" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_OFF" "RASTER_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_NOISEGLOW" "RASTER_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_NOISEGLOW" "RASTER_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_NOISEGLOW" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_NOISEGLOW" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_NOISEGLOW" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_NOISEGLOW" "RASTER_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_OFF" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_OFF" "SHADOWS_SCREEN" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "SIMPLE_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "FANCY_FADE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SCREEN" "SHADOWS_NATIVE" "FX_HIGHLIGHT" "RASTER_FADE" }
"!!GLES3"
}
}
 }
}
Fallback "Hidden/Shadow/Opaque"
}
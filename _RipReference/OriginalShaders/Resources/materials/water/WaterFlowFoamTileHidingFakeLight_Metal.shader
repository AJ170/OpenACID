Shader "Water/FlowFoamTileHidingFakeLight_Metal" {
Properties {
 _DistFoamTex ("DistortionFoam", 2D) = "white" { }
 _GradientTex ("Gradient", 2D) = "white" { }
 _DistortParams ("Distortions (World Normal, Reflection, Specular, Unused)", Vector) = (1,1,2,1.15)
 _BumpTiling ("Bump Tiling (Tiling x, Tiling z, Blend factor x, Blend factor z", Vector) = (1,1,0.12,0.12)
 _Shininess ("Shininess", Range(0,1000)) = 400
 _CrestFoamParameters ("Foam Parameters (Crest foam intensity, Shore foam intensity, Unused, Unused)", Vector) = (2,1,1,0)
 _OverlayFoamIntensity ("Overlay-Foam Intensity", Range(0,1)) = 1
 _GerstnerIntensity ("Per vertex displacement", Float) = 1
 _GAmplitude ("Wave Amplitude", Vector) = (0.3,0.35,0.25,0.25)
 _GFrequency ("Wave Frequency", Vector) = (1.3,1.35,1.25,1.25)
 _GSteepness ("Wave Steepness", Vector) = (1,1,1,1)
 _GSpeed ("Wave Speed", Vector) = (1.2,1.375,1.1,1.5)
 _GDirectionAB ("Wave Direction", Vector) = (0.3,0.85,0.85,0.25)
 _GDirectionCD ("Wave Direction", Vector) = (0.1,0.9,0.5,0.5)
 _DebugReflectionPower ("Reflection Power (DEBUG)", Range(0,1)) = 0
 _DebugReflectionDamping ("Reflection Vector Damping", Range(0,1)) = 0
 _DebugReflectionIntensity ("Reflection Intensity", Float) = 1
}
SubShader { 
 Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry+2" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry+2" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  GpuProgramID 60251
Program "vp" {
SubProgram "gles " {
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform mediump float _GerstnerIntensity;
uniform mediump vec4 _BumpTiling;
uniform mediump vec4 _GAmplitude;
uniform mediump vec4 _GFrequency;
uniform mediump vec4 _GSteepness;
uniform mediump vec4 _GSpeed;
uniform mediump vec4 _GDirectionAB;
uniform mediump vec4 _GDirectionCD;
uniform mediump vec4 _CrestFoamParameters;
uniform highp vec4 _flowPhasesLerp;
uniform mediump vec2 _flowSpeed;
uniform lowp vec4 acid_Fog;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
varying lowp vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  highp vec4 tmpvar_2;
  tmpvar_2.w = _glesVertex.w;
  mediump float diff_3;
  mediump vec2 uv_4;
  mediump float foamShoreAmount_5;
  highp float viewDirLen_6;
  highp vec4 tmpvar_7;
  highp vec2 tmpvar_8;
  highp vec2 tmpvar_9;
  lowp vec4 tmpvar_10;
  mediump vec4 tmpvar_11;
  mediump vec3 tmpvar_12;
  mediump vec3 tmpvar_13;
  lowp vec4 tmpvar_14;
  mediump float waterBaseHeight_15;
  highp vec3 worldSpaceVertex_16;
  highp vec3 tmpvar_17;
  tmpvar_17 = (_Object2World * _glesVertex).xyz;
  highp float tmpvar_18;
  tmpvar_18 = tmpvar_17.y;
  waterBaseHeight_15 = tmpvar_18;
  mediump vec4 dotABCD_19;
  highp vec3 offsets_20;
  mediump vec4 tmpvar_21;
  tmpvar_21 = ((_GSteepness.xxyy * _GAmplitude.xxyy) * _GDirectionAB);
  mediump vec4 tmpvar_22;
  tmpvar_22 = ((_GSteepness.zzww * _GAmplitude.zzww) * _GDirectionCD);
  highp vec4 tmpvar_23;
  tmpvar_23.x = dot (_GDirectionAB.xy, tmpvar_17.xz);
  tmpvar_23.y = dot (_GDirectionAB.zw, tmpvar_17.xz);
  tmpvar_23.z = dot (_GDirectionCD.xy, tmpvar_17.xz);
  tmpvar_23.w = dot (_GDirectionCD.zw, tmpvar_17.xz);
  highp vec4 tmpvar_24;
  tmpvar_24 = (_GFrequency * tmpvar_23);
  dotABCD_19 = tmpvar_24;
  highp vec4 tmpvar_25;
  tmpvar_25 = (_Time.yyyy * _GSpeed);
  highp vec4 tmpvar_26;
  tmpvar_26 = cos((dotABCD_19 + tmpvar_25));
  mediump vec4 tmpvar_27;
  tmpvar_27.xy = tmpvar_21.xz;
  tmpvar_27.zw = tmpvar_22.xz;
  offsets_20.x = dot (tmpvar_26, tmpvar_27);
  mediump vec4 tmpvar_28;
  tmpvar_28.xy = tmpvar_21.yw;
  tmpvar_28.zw = tmpvar_22.yw;
  offsets_20.z = dot (tmpvar_26, tmpvar_28);
  offsets_20.y = dot (sin((dotABCD_19 + tmpvar_25)), _GAmplitude);
  highp vec2 xzVtx_29;
  xzVtx_29 = (tmpvar_17.xz + offsets_20.xz);
  mediump vec4 dotABCD_30;
  highp vec3 nrml_31;
  nrml_31.y = 2.0;
  mediump vec4 tmpvar_32;
  tmpvar_32 = ((_GFrequency.xxyy * _GAmplitude.xxyy) * _GDirectionAB);
  mediump vec4 tmpvar_33;
  tmpvar_33 = ((_GFrequency.zzww * _GAmplitude.zzww) * _GDirectionCD);
  highp vec4 tmpvar_34;
  tmpvar_34.x = dot (_GDirectionAB.xy, xzVtx_29);
  tmpvar_34.y = dot (_GDirectionAB.zw, xzVtx_29);
  tmpvar_34.z = dot (_GDirectionCD.xy, xzVtx_29);
  tmpvar_34.w = dot (_GDirectionCD.zw, xzVtx_29);
  highp vec4 tmpvar_35;
  tmpvar_35 = (_GFrequency * tmpvar_34);
  dotABCD_30 = tmpvar_35;
  highp vec4 tmpvar_36;
  tmpvar_36 = cos((dotABCD_30 + tmpvar_25));
  mediump vec4 tmpvar_37;
  tmpvar_37.xy = tmpvar_32.xz;
  tmpvar_37.zw = tmpvar_33.xz;
  nrml_31.x = -(dot (tmpvar_36, tmpvar_37));
  mediump vec4 tmpvar_38;
  tmpvar_38.xy = tmpvar_32.yw;
  tmpvar_38.zw = tmpvar_33.yw;
  nrml_31.z = -(dot (tmpvar_36, tmpvar_38));
  nrml_31.xz = (nrml_31.xz * _GerstnerIntensity);
  highp vec3 tmpvar_39;
  tmpvar_39 = normalize(nrml_31);
  nrml_31 = tmpvar_39;
  tmpvar_2.xyz = (_glesVertex.xyz + offsets_20);
  worldSpaceVertex_16 = (_Object2World * tmpvar_2).xyz;
  tmpvar_12 = (_WorldSpaceCameraPos - worldSpaceVertex_16);
  mediump float tmpvar_40;
  tmpvar_40 = sqrt(dot (tmpvar_12, tmpvar_12));
  viewDirLen_6 = tmpvar_40;
  tmpvar_12 = (tmpvar_12 / viewDirLen_6);
  tmpvar_7 = (glstate_matrix_mvp * tmpvar_2);
  highp vec4 o_41;
  highp vec4 tmpvar_42;
  tmpvar_42 = (tmpvar_7 * 0.5);
  highp vec2 tmpvar_43;
  tmpvar_43.x = tmpvar_42.x;
  tmpvar_43.y = (tmpvar_42.y * _ProjectionParams.x);
  o_41.xy = (tmpvar_43 + tmpvar_42.w);
  o_41.zw = tmpvar_7.zw;
  tmpvar_13 = o_41.xyw;
  tmpvar_11.xyz = tmpvar_39;
  tmpvar_11.w = tmpvar_1.w;
  tmpvar_14.w = tmpvar_1.y;
  mediump float tmpvar_44;
  highp float tmpvar_45;
  tmpvar_45 = clamp (((worldSpaceVertex_16.y - waterBaseHeight_15) / (_CrestFoamParameters.x - waterBaseHeight_15)), 0.0, 1.0);
  tmpvar_44 = tmpvar_45;
  lowp float tmpvar_46;
  tmpvar_46 = (1.0 - (_glesColor.w * _glesColor.w));
  foamShoreAmount_5 = tmpvar_46;
  mediump float tmpvar_47;
  tmpvar_47 = max ((tmpvar_44 * _CrestFoamParameters.y), (foamShoreAmount_5 * _CrestFoamParameters.z));
  tmpvar_10.z = tmpvar_47;
  highp vec2 tmpvar_48;
  tmpvar_48 = (worldSpaceVertex_16.xz * _BumpTiling.xy);
  uv_4 = tmpvar_48;
  mediump vec2 tmpvar_49;
  tmpvar_49 = (((_glesColor.xz * 2.0) - 1.0) * _flowSpeed);
  tmpvar_8 = (uv_4 + (tmpvar_49 * _flowPhasesLerp.x));
  tmpvar_9 = (uv_4 + (tmpvar_49 * _flowPhasesLerp.y));
  lowp float tmpvar_50;
  tmpvar_50 = (acid_Fog.y - acid_Fog.x);
  diff_3 = tmpvar_50;
  highp float tmpvar_51;
  tmpvar_51 = clamp (((acid_Fog.y - viewDirLen_6) / diff_3), 0.0, 1.0);
  tmpvar_10.y = ((1.0 - tmpvar_51) * acid_Fog.z);
  tmpvar_10.x = (1.0 - tmpvar_10.y);
  gl_Position = tmpvar_7;
  xlv_TEXCOORD0 = tmpvar_8;
  xlv_TEXCOORD1 = tmpvar_9;
  xlv_TEXCOORD2 = (tmpvar_8 * _BumpTiling.zw);
  xlv_TEXCOORD3 = (tmpvar_9 * _BumpTiling.zw);
  xlv_TEXCOORD4 = tmpvar_10;
  xlv_TEXCOORD5 = tmpvar_11;
  xlv_TEXCOORD6 = tmpvar_12;
  xlv_TEXCOORD7 = tmpvar_13;
  xlv_TEXCOORD8 = tmpvar_14;
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 unity_FogColor;
uniform sampler2D _DistFoamTex;
uniform sampler2D _GradientTex;
uniform sampler2D _ReflectionTex;
uniform lowp vec4 _ReflColor;
uniform mediump vec4 _DistortParams;
uniform mediump float _Shininess;
uniform lowp vec4 _SpecularColor;
uniform mediump float _DebugReflectionPower;
uniform mediump float _DebugReflectionDamping;
uniform mediump float _DebugReflectionIntensity;
uniform lowp float _OverlayFoamIntensity;
uniform mediump vec4 _fakeLightDir;
uniform highp vec4 _flowPhasesLerp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD4;
varying mediump vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD6;
varying mediump vec3 xlv_TEXCOORD7;
varying lowp vec4 xlv_TEXCOORD8;
void main ()
{
  lowp vec4 tmpvar_1;
  lowp float foamAdd_2;
  lowp float foamColor_3;
  lowp float spec_4;
  lowp vec3 rtReflections_5;
  lowp vec4 c_6;
  mediump vec3 specNormal_7;
  mediump vec3 combinedBumpFoam_8;
  mediump vec3 bumpFoamVal1_9;
  mediump vec3 bumpFoamVal0_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = (texture2D (_DistFoamTex, xlv_TEXCOORD0).xyz + texture2D (_DistFoamTex, xlv_TEXCOORD2).xyz);
  bumpFoamVal0_10 = tmpvar_11;
  lowp vec3 tmpvar_12;
  tmpvar_12 = (texture2D (_DistFoamTex, xlv_TEXCOORD1).xyz + texture2D (_DistFoamTex, xlv_TEXCOORD3).xyz);
  bumpFoamVal1_9 = tmpvar_12;
  highp vec3 tmpvar_13;
  tmpvar_13 = mix (bumpFoamVal0_10, bumpFoamVal1_9, _flowPhasesLerp.zzz);
  combinedBumpFoam_8 = tmpvar_13;
  combinedBumpFoam_8.xy = ((combinedBumpFoam_8.xy - 1.0) * _DistortParams.z);
  specNormal_7.y = xlv_TEXCOORD5.y;
  specNormal_7.xz = (xlv_TEXCOORD5.xz + combinedBumpFoam_8.xy);
  mediump vec3 tmpvar_14;
  tmpvar_14 = normalize(specNormal_7);
  specNormal_7 = tmpvar_14;
  mediump vec2 tmpvar_15;
  tmpvar_15.x = xlv_TEXCOORD8.w;
  tmpvar_15.y = dot (tmpvar_14, xlv_TEXCOORD6);
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_GradientTex, tmpvar_15);
  c_6.w = tmpvar_16.w;
  lowp vec4 tmpvar_17;
  mediump vec2 P_18;
  P_18 = ((xlv_TEXCOORD7.xy + (tmpvar_14.xz * _DebugReflectionDamping)) / xlv_TEXCOORD7.z);
  tmpvar_17 = texture2D (_ReflectionTex, P_18);
  mediump vec3 tmpvar_19;
  tmpvar_19 = (_DebugReflectionIntensity * tmpvar_17.xyz);
  rtReflections_5 = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = pow (abs(dot (tmpvar_14, 
    normalize((xlv_TEXCOORD6 + _fakeLightDir.xyz))
  )), _Shininess);
  spec_4 = tmpvar_20;
  mediump float tmpvar_21;
  tmpvar_21 = combinedBumpFoam_8.z;
  foamColor_3 = tmpvar_21;
  foamAdd_2 = (pow (foamColor_3, 5.0) * _OverlayFoamIntensity);
  foamAdd_2 = (foamAdd_2 + (foamColor_3 * xlv_TEXCOORD4.z));
  c_6.xyz = (tmpvar_16.xyz + (foamAdd_2 + (
    (1.0 - foamAdd_2)
   * 
    (((rtReflections_5 * _ReflColor.xyz) * tmpvar_16.w) + (spec_4 * _SpecularColor.xyz))
  )));
  mediump vec3 tmpvar_22;
  lowp vec3 y_23;
  y_23 = (rtReflections_5 * _ReflColor.xyz);
  tmpvar_22 = mix (c_6.xyz, y_23, vec3(_DebugReflectionPower));
  c_6.xyz = tmpvar_22;
  mediump vec4 tmpvar_24;
  tmpvar_24.xyz = ((unity_FogColor.xyz * xlv_TEXCOORD4.y) + (c_6.xyz * xlv_TEXCOORD4.x));
  tmpvar_24.w = xlv_TEXCOORD5.w;
  tmpvar_1 = tmpvar_24;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
}
Program "fp" {
SubProgram "gles " {
"!!GLES"
}
}
 }
}
Fallback "Mobile/VertexLit"
}
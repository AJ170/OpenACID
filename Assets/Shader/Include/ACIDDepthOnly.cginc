// Depth pre-pass used by the *Fade shaders (ColorMask 0 in the pass state).
#ifndef ACID_DEPTHONLY_INCLUDED
#define ACID_DEPTHONLY_INCLUDED

#include "UnityCG.cginc"

float4 vertDepth(float4 vertex : POSITION) : SV_POSITION
{
    return UnityObjectToClipPos(vertex);
}

fixed4 fragDepth() : SV_Target
{
    return fixed4(1.0, 1.0, 1.0, 1.0);
}

#endif // ACID_DEPTHONLY_INCLUDED

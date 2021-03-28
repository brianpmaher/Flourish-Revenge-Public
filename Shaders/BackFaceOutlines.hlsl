#ifndef BACKFACEOUTLINES_INCLUDED
#define BACKFACEOUTLINES_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

// Mesh data
struct Attributes
{
    float4 positionOS : POSITION; // position in object space
    float3 normalOS   : NORMAL;   // normal vector in object space
};

// Output from vertex function; input into fragment function
struct VertexOutput
{
    float4 positionCS : SV_POSITION; // position in clip space
};

// Properties
float _Thickness;
float4 _Color;

VertexOutput Vertex(Attributes input)
{
    VertexOutput output = (VertexOutput)0;
    
    float3 normalOS = input.normalOS;

    // Extrude the object space position along a normal vector
    float3 posOS = input.positionOS.xyz + normalOS * _Thickness;

    // Convert position to world and clip space
    output.positionCS = GetVertexPositionInputs(posOS).positionCS;

    return output;
}

float4 Fragment(VertexOutput input) : SV_TARGET
{
    return _Color;
}

#endif

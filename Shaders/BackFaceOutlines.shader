Shader "Outlines/BackFaceOutlines"
{
    Properties
    {
        _Thickness("Thickness", Float) = 1
        _Color("Color", Color) = (1, 1, 1, 1)
    }
    
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        
        Pass
        {
            Name "Outlines"
            
            Cull Front
            
            HLSLPROGRAM

            // Standard URP Requirements
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex Vertex
            #pragma fragment Fragment

            #include "BackFaceOutlines.hlsl"
            
            ENDHLSL
        }
    }
}

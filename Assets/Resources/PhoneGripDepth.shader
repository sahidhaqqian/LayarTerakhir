// Rendering support only: writes no color and creates no visual artwork.
Shader "LayarTerakhir/PhoneGripDepth"
{
    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "Queue"="Geometry-10" }
        Pass
        {
            Tags { "LightMode"="UniversalForward" }
            ZWrite On
            ZTest LEqual
            Cull Off
            ColorMask 0
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            struct Input { float4 positionOS : POSITION; };
            struct Output { float4 positionCS : SV_POSITION; };
            Output Vert(Input input) { Output output;output.positionCS=TransformObjectToHClip(input.positionOS.xyz);return output; }
            half4 Frag(Output input) : SV_Target { return 0; }
            ENDHLSL
        }
    }
}

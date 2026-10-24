Shader "LayarTerakhir/Phase3 UV Mark"
{
    Properties { _MainTex("Licensed ink drawing",2D)="white"{} _Color("UV fluorescence",Color)=(.7,.4,1,1) }
    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Cull Off ZWrite Off ZTest LEqual Blend SrcAlpha OneMinusSrcAlpha
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);
            CBUFFER_START(UnityPerMaterial)
            half4 _Color;
            CBUFFER_END
            struct A {float4 positionOS:POSITION;float2 uv:TEXCOORD0;};
            struct V {float4 positionCS:SV_POSITION;float2 uv:TEXCOORD0;};
            V Vert(A i){V o;o.positionCS=TransformObjectToHClip(i.positionOS.xyz);o.uv=i.uv;return o;}
            half4 Frag(V i):SV_Target
            {
                half3 ink=SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv).rgb;
                half alpha=(1-dot(ink,half3(.3333,.3333,.3333)))*_Color.a;
                clip(alpha-.015h);return half4(_Color.rgb,alpha);
            }
            ENDHLSL
        }
    }
}

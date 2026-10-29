Shader "LayarTerakhir/Wall Scratch Decal"
{
    Properties
    {
        _BaseMap("NPS photograph of claw gouges on a wall",2D)="black"{}
        _BaseColor("Dried blood",Color)=(.27,.014,.008,1)
    }
    SubShader
    {
        Tags{"RenderPipeline"="UniversalPipeline" "Queue"="Transparent" "RenderType"="Transparent"}
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            TEXTURE2D(_BaseMap);SAMPLER(sampler_BaseMap);
            CBUFFER_START(UnityPerMaterial)
            float4 _BaseColor;
            CBUFFER_END
            struct Attributes{float4 positionOS:POSITION;float2 uv:TEXCOORD0;};
            struct Varyings{float4 positionCS:SV_POSITION;float2 uv:TEXCOORD0;};
            Varyings vert(Attributes v){Varyings o;o.positionCS=TransformObjectToHClip(v.positionOS.xyz);o.uv=v.uv;return o;}
            half4 frag(Varyings i):SV_Target
            {
                // Crop the original photo in the material; preserve its irregular
                // real gouges and discard the darker, undamaged wood background.
                float2 photoUV=i.uv*float2(.17,.12)+float2(.50,.54);
                half3 photo=SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap,photoUV).rgb;
                half light=min(photo.r,min(photo.g,photo.b));
                half edge=saturate(min(min(i.uv.x,1-i.uv.x),min(i.uv.y,1-i.uv.y))*20);
                half coverage=saturate((light-.28)*6)*edge;
                return half4(_BaseColor.rgb*(.5+light),coverage*_BaseColor.a);

            }
            ENDHLSL
        }
    }
}

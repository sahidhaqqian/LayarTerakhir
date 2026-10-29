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
            float GougeMask(float2 uv,float2 a,float2 b,float width)
            {
                float t=saturate(dot(uv-a,b-a)/dot(b-a,b-a));
                float radius=lerp(width,width*.22,t);
                return 1-smoothstep(radius*.65,radius,length(uv-lerp(a,b,t)));
            }
            half4 frag(Varyings i):SV_Target
            {
                // Crop the original photo in the material; preserve its irregular
                // real gouges and discard the darker, undamaged wood background.
                // The source photograph's four parallel gouges sit below and
                // to the right of its center. Crop the actual mark cluster,
                // leaving most of the unbroken timber outside the decal.
                // Tight crop around the photographed gouges in the middle of
                // the source plank; avoid the pale wood grain elsewhere.
                float2 photoUV=i.uv*float2(.08,.10)+float2(.59,.56);
                half3 photo=SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap,photoUV).rgb;
                half3 surrounding=.5*(SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap,photoUV+float2(.007,0)).rgb+SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap,photoUV-float2(.007,0)).rgb);
                half light=min(photo.r,min(photo.g,photo.b));
                half edge=saturate(min(min(i.uv.x,1-i.uv.x),min(i.uv.y,1-i.uv.y))*20);
                // The real gouges expose pale fibers. Restrict coverage to
                // those highlights so the timber photograph reads as scratches
                // instead of scattered red dots over the wall.
                // Trace the three photographed cuts, retaining their dark
                // cavities and splintered fibers as well as their highlights.
                // A brightness threshold erased the cavities into isolated dots.
                half coverage=max(GougeMask(photoUV,float2(.602,.650),float2(.621,.570),.006),
                    max(GougeMask(photoUV,float2(.622,.650),float2(.644,.575),.006),
                        GougeMask(photoUV,float2(.640,.650),float2(.662,.570),.006)))*edge;
                coverage*=smoothstep(.025,.10,length(photo-surrounding));
                return half4(lerp(photo,_BaseColor.rgb,.12),coverage*_BaseColor.a);

            }
            ENDHLSL
        }
    }
}

Shader "LayarTerakhir/AtmajaPhoneFeed"
{
    Properties { [PerRendererData] _MainTex("Feed",2D)="white" {} _ARGain("AR gain",Float)=1 _ReducedGlitch("Reduced glitch",Float)=0 }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" }
        Cull Off ZWrite Off ZTest Always Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; float4 color:COLOR; };
            struct v2f { float4 vertex:SV_POSITION; float2 uv:TEXCOORD0; float4 color:COLOR; };
            sampler2D _MainTex; float _ARGain,_ReducedGlitch;
            v2f vert(appdata v){v2f o;o.vertex=UnityObjectToClipPos(v.vertex);o.uv=v.uv;o.color=v.color;return o;}
            fixed4 frag(v2f i):SV_Target
            {
                fixed4 c=tex2D(_MainTex,i.uv);c.rgb=saturate(c.rgb*_ARGain);
                if(_ARGain>1.5)
                {
                    float l=dot(c.rgb,float3(.25,.60,.15));
                    float other=max(c.g,c.b);
                    float red=smoothstep(.025,.08,c.r-other)*step(other*1.8+.01,c.r);fixed3 original=c.rgb;
                    float scan=.97+.03*(1-_ReducedGlitch)*sin(i.uv.y*1256+_Time.y*8);
                    float grain=frac(sin(dot(i.uv+_Time.yy,float2(12.9898,78.233)))*43758.5453)-.5;
                    c.rgb=(l*float3(.48,1,.63)+float3(.003,.01,.005))*scan+grain*.012*(1-_ReducedGlitch);c.rgb=lerp(c.rgb,original,red);
                }
                return fixed4(c.rgb,1)*i.color;
            }
            ENDCG
        }
    }
}

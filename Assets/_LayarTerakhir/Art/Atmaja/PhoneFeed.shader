Shader "LayarTerakhir/AtmajaPhoneFeed"
{
    Properties { [PerRendererData] _MainTex("Feed",2D)="white" {} _ARGain("AR gain",Float)=1 }
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
            sampler2D _MainTex; float _ARGain;
            v2f vert(appdata v){v2f o;o.vertex=UnityObjectToClipPos(v.vertex);o.uv=v.uv;o.color=v.color;return o;}
            fixed4 frag(v2f i):SV_Target
            {
                fixed4 c=tex2D(_MainTex,i.uv);c.rgb=saturate(c.rgb*_ARGain);
                if(_ARGain>1.5)
                {
                    float l=dot(c.rgb,float3(.25,.60,.15));
                    float scan=.94+.06*sin(i.uv.y*1256+_Time.y*8);
                    float grain=frac(sin(dot(i.uv+_Time.yy,float2(12.9898,78.233)))*43758.5453)-.5;
                    c.rgb=(l*float3(.48,1,.63)+float3(.008,.025,.012))*scan+grain*.025;
                }
                return fixed4(c.rgb,1)*i.color;
            }
            ENDCG
        }
    }
}

Shader "LayarTerakhir/NightSky"
{
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off ZWrite Off
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            struct appdata { float4 vertex : POSITION; };
            struct v2f { float4 position : SV_POSITION; float3 direction : TEXCOORD0; };
            v2f vert(appdata v) { v2f o; o.position=UnityObjectToClipPos(v.vertex);o.direction=v.vertex.xyz;return o; }
            float hash(float3 p)
            {
                p=frac(p*.1031);p+=dot(p,p.yzx+33.33);return frac((p.x+p.y)*p.z);
            }
            float noise(float3 p)
            {
                float3 i=floor(p),f=frac(p);f=f*f*(3-2*f);
                return lerp(lerp(lerp(hash(i),hash(i+float3(1,0,0)),f.x),lerp(hash(i+float3(0,1,0)),hash(i+float3(1,1,0)),f.x),f.y),
                    lerp(lerp(hash(i+float3(0,0,1)),hash(i+float3(1,0,1)),f.x),lerp(hash(i+float3(0,1,1)),hash(i+1),f.x),f.y),f.z);
            }
            float cloud(float3 p)
            {
                float value=0,weight=.55;
                [unroll] for(int octave=0;octave<4;octave++){value+=noise(p)*weight;p=p*2.03+float3(7.1,13.7,3.3);weight*=.48;}
                return value;
            }
            float4 frag(v2f input) : SV_Target
            {
                float3 d=normalize(input.direction);
                float horizon=pow(saturate(d.y),.55);
                float3 sky=lerp(float3(.065,.078,.088),float3(.010,.017,.026),horizon);
                // Smooth multi-scale cloud density replaces periodic sine stripes.
                float density=cloud(d*7.5+float3(.014*_Time.y,0,.006*_Time.y));
                sky*=lerp(.60,1.05,smoothstep(.20,.75,density));
                sky+=float3(.009,.010,.011)*smoothstep(.48,.8,density)*(1-horizon*.65);
                float3 moon=normalize(float3(.30,.48,-.82));float dist=length(d-moon);
                float disk=1-smoothstep(.008,.011,dist);
                float veil=lerp(.25,.7,smoothstep(.3,.72,density));
                sky+=float3(.27,.30,.32)*(disk*.6+exp(-dist*19)*.045)*veil;
                return float4(sky*.35,1);
            }
            ENDHLSL
        }
    }
}

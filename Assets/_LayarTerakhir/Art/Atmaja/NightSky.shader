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
            float hash(float3 p) { return frac(sin(dot(p,float3(127.1,311.7,74.7)))*43758.5453); }
            half4 frag(v2f input) : SV_Target
            {
                float3 d=normalize(input.direction);
                float horizon=saturate(d.y*1.8);
                float3 sky=lerp(float3(.045,.060,.085),float3(.006,.011,.030),horizon);
                float3 grid=d*440;
                float star=step(.997,hash(floor(grid)))*pow(saturate(1-length(frac(grid)-.5)*2),9);
                sky+=star*float3(.62,.72,.90)*saturate(d.y*4);
                float3 moon=normalize(float3(.30,.48,-.82));float dist=length(d-moon);
                float disk=1-smoothstep(.030,.033,dist);
                float shadow=1-smoothstep(.028,.031,length(d-normalize(moon+float3(.016,.006,0))));
                sky+=float3(.68,.72,.77)*disk*(1-shadow*.86);
                sky+=float3(.022,.028,.04)*exp(-dist*23);
                float cloud=(sin(d.x*27+d.z*14)*sin(d.z*19-d.y*11)+1)*.5;
                sky=lerp(sky,sky*.5,cloud*.24);
                return half4(sky,1);
            }
            ENDHLSL
        }
    }
}

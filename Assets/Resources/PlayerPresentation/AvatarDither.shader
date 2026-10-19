// URP's installed Lit includes supply PBR lighting, shadows and normals.
// Only camera-facing coverage is dithered; the complete body still casts shadows.
Shader "LayarTerakhir/Avatar Dither Lit"
{
 Properties
 {
  _BaseMap("Albedo",2D)="white"{}
  _BaseColor("Colour",Color)=(1,1,1,1)
  _BumpMap("Normal",2D)="bump"{}
  _BumpScale("Normal strength",Float)=1
  _MetallicGlossMap("Metallic",2D)="white"{}
  _Metallic("Metallic",Range(0,1))=0
  _Smoothness("Smoothness",Range(0,1))=.25
  _OcclusionMap("Occlusion",2D)="white"{}
  _OcclusionStrength("Occlusion strength",Range(0,1))=1
  _EmissionMap("Emission",2D)="white"{}
  _EmissionColor("Emission colour",Color)=(0,0,0,0)
  _Cutoff("Cutoff",Range(0,1))=.5
  _DitherAlpha("Camera coverage",Range(0,1))=1
  [HideInInspector] _Surface("Surface",Float)=0
  [HideInInspector] _Cull("Cull",Float)=2
  [HideInInspector] _SpecularHighlights("Specular",Float)=1
  [HideInInspector] _EnvironmentReflections("Reflections",Float)=1
 }
 SubShader
 {
  Tags {"RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry"}
  HLSLINCLUDE
  #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
  float _DitherAlpha;
  void CameraCoverage(float4 positionCS)
  {
   // Stable 4x4 ordered coverage, including a truly empty alpha=0.
   const float threshold[16]={0,8,2,10,12,4,14,6,3,11,1,9,15,7,13,5};
   uint2 pixel=(uint2)positionCS.xy & 3;
   clip(_DitherAlpha-(threshold[pixel.y*4+pixel.x]+.5)/16);
  }
  ENDHLSL
  Pass
  {
   Name "ForwardLit"
   Tags {"LightMode"="UniversalForwardOnly"}
   ZWrite On Cull [_Cull]
   HLSLPROGRAM
   #pragma target 3.5
   #pragma vertex LitPassVertex
   #pragma fragment DitherFragment
   #pragma shader_feature_local _NORMALMAP
   #pragma shader_feature_local_fragment _EMISSION
   #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
   #pragma shader_feature_local_fragment _OCCLUSIONMAP
   #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
   #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
   #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
   #pragma multi_compile_fragment _ _SHADOWS_SOFT _SHADOWS_SOFT_LOW _SHADOWS_SOFT_MEDIUM _SHADOWS_SOFT_HIGH
   #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
   #pragma multi_compile_fragment _ _LIGHT_COOKIES
   #pragma multi_compile _ _LIGHT_LAYERS
   #pragma multi_compile _ _CLUSTER_LIGHT_LOOP
   #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
   #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Fog.hlsl"
   #pragma multi_compile_instancing
   #pragma instancing_options renderinglayer
   #include "Packages/com.unity.render-pipelines.universal/Shaders/LitForwardPass.hlsl"
   void DitherFragment(Varyings input,out half4 colour:SV_Target0
   #ifdef _WRITE_RENDERING_LAYERS
    ,out uint layers:SV_Target1
   #endif
   )
   {
    CameraCoverage(input.positionCS);
    LitPassFragment(input,colour
    #ifdef _WRITE_RENDERING_LAYERS
     ,layers
    #endif
    );
   }
   ENDHLSL
  }
  // Camera fading must not remove Raka's shadow from the environment.
  UsePass "Universal Render Pipeline/Lit/SHADOWCASTER"
  Pass
  {
   Name "DepthOnly"
   Tags {"LightMode"="DepthOnly"}
   ZWrite On ColorMask R Cull [_Cull]
   HLSLPROGRAM
   #pragma target 3.5
   #pragma vertex DepthOnlyVertex
   #pragma fragment DitherDepth
   #pragma multi_compile_instancing
   #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
   half DitherDepth(Varyings input):SV_TARGET {CameraCoverage(input.positionCS);return DepthOnlyFragment(input);}
   ENDHLSL
  }
  Pass
  {
   Name "DepthNormals"
   Tags {"LightMode"="DepthNormalsOnly"}
   ZWrite On Cull [_Cull]
   HLSLPROGRAM
   #pragma target 3.5
   #pragma vertex DepthNormalsVertex
   #pragma fragment DitherNormals
   #pragma multi_compile_instancing
   #pragma instancing_options renderinglayer
   #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
   #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
   #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthNormalsPass.hlsl"
   void DitherNormals(Varyings input,out half4 normal:SV_Target0
   #ifdef _WRITE_RENDERING_LAYERS
    ,out uint layers:SV_Target1
   #endif
   )
   {
    CameraCoverage(input.positionCS);
    DepthNormalsFragment(input,normal
    #ifdef _WRITE_RENDERING_LAYERS
     ,layers
    #endif
    );
   }
   ENDHLSL
  }
 }
 Fallback Off
}

Shader "Beffio/Standard Dithered - Soft Lights" 
{
	Properties 
	{
		// Standard Properties
		_Color ("Base Albedo", Color) = (1,1,1,1)
		_MainTex ("Base Albedo Texture", 2D) = "white" {}
		_Glossiness ("Base Smoothness", Range(0,1)) = 0.5
		[Gamma] _Metallic ("Base Metallic", Range(0,1)) = 0.0
		_MetallicGlossMap ("Base Metallic Texture", 2D) = "white" {}
		_BumpScale("Scale", Float) = 1.0
		[Normal] _BumpMap("Normal Map", 2D) = "bump" {}
		_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		_OcclusionMap("Occlusion", 2D) = "white" {}
		_EmissionColor("Color", Color) = (0,0,0)
		_EmissionMap("Emission", 2D) = "white" {}

		// Dithering
		_PaletteColorCount ("Mixed Color Count", float) = 4
		_PaletteHeight ("Palette Texture Height", float) = 128
		_PaletteTex ("Palette Texture", 2D) = "black" {}
		_PatternSize ("Pattern Texture Size", float) = 8
		_PatternTex ("Pattern Texture", 2D) = "black" {}
		_PatternScale("Pattern Scale", float) = 1

		// Reference hack
		[HideInInspector]__paletteRef ("Palette Asset", Vector) = (0,0,0,0)
		[HideInInspector]__patternRef ("Pattern Asset", Vector) = (0,0,0,0)
		[HideInInspector]__hasPatternTex ("Has Pattern Texture", float) = 0
	}

	SubShader 
	{
		Tags 
		{
			"Queue"="Geometry"
			"RenderType"="Opaque"
			"IgnoreProjector"="false"
		}
		LOD 200
		
		CGPROGRAM

			#pragma surface StandardSurface Standard fullforwardshadows vertex:DitheringVertex finalcolor:StandardDither
			#pragma target 3.0

			// Shader features
			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _METALLICGLOSSMAP
			#pragma shader_feature _OCCLUSIONMAP
			#pragma shader_feature _EMISSIVEMAP
			#pragma shader_feature _SCREENSPACEDITHER

			#include "Dithering Material Base.cginc"
			#include "Standard Base Include.cginc"

			void StandardDither(Input i, SurfaceOutputStandard o, inout fixed4 color) 
			{
				#ifndef UNITY_PASS_FORWARDADD
					color.rgb = GetDitherColor(color.rgb, _PatternTex, _PaletteTex, _PaletteHeight, i.ditherPos, _PaletteColorCount, _PatternScale);
				#endif
			}
		ENDCG
	}
	FallBack "Diffuse"
    CustomEditor "Beffio.Dithering.StandardDitheredEditor"
}

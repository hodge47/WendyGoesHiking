// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Beffio/Image Effects/Dithering Image Effect" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_PaletteColorCount ("Mixed Color Count", float) = 4
		_PaletteHeight ("Palette Height", float) = 128
		_PaletteTex ("Palette", 2D) = "black" {}
		_PatternSize ("Palette Size", float) = 8
		_PatternTex ("Palette Texture", 2D) = "black" {}
		_PatternScale("Pattern Scale", float) = 1
	}

	SubShader 
	{
		Tags 
		{ 
			"IgnoreProjector"="True" 
			"RenderType"="Opaque" 
		}
		LOD 200

		Lighting Off
		ZTest Always 
		Cull Off 
		ZWrite Off 
		Fog { Mode Off }

		Pass 
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				sampler2D _MainTex;
				sampler2D _PaletteTex;
				sampler2D _PatternTex;

				float _PaletteColorCount;
				float _PaletteHeight;
				float _PatternSize;
				float _PatternScale;

				struct VertexInput 
				{
					float4 position : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct Input 
				{
					float4 position : SV_POSITION;
					float2 uv : TEXCOORD0;
					float4 ditherPos : TEXCOORD1;
				};

				#include "Dithering Base.cginc"

				Input vert(VertexInput i) 
				{
					Input o;
					o.position = UnityObjectToClipPos(i.position);
					o.uv = i.uv;
					o.ditherPos = GetDitherPos(i.position, _PatternSize);
					return o;
				}

				fixed4 frag(Input i) : COLOR 
				{
					fixed4 c = tex2D(_MainTex, i.uv);
					return fixed4(GetDitherColor(c.rgb, _PatternTex, _PaletteTex, _PaletteHeight, i.ditherPos, _PaletteColorCount, _PatternScale), c.a);
				}
			ENDCG
		}
	}

	Fallback "Unlit/Texture"
}
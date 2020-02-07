#ifndef DITHERING_MATERIAL_INCLUDED
#define DITHERING_MATERIAL_INCLUDED

#include "UnityCG.cginc"
#include "Dithering Base.cginc"

struct Input 
{
	float2 uv_MainTex;
	float2 uv_EmissionMap;
	float4 ditherPos;
};

sampler2D _PaletteTex;
sampler2D _PatternTex;

float _PaletteColorCount;
float _PaletteHeight;
float _PatternSize;
float _PatternScale;

void DitheringVertex(inout appdata_full v, out Input o) 
{
	UNITY_INITIALIZE_OUTPUT(Input, o);
	#ifdef _SCREENSPACEDITHER
		// Screen space based pattern
		o.ditherPos = GetDitherPos(v.vertex, _PatternSize);
	#else
		// UV based pattern
		o.ditherPos = float4(v.texcoord.xy, 0, 1);
	#endif
}

#endif //DITHERING_MATERIAL_INCLUDED
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#ifndef DITHERING_INCLUDED
#define DITHERING_INCLUDED

#include "UnityCG.cginc"

inline float4 GetDitherPos(float4 vertex, float ditherSize) 
{
	// Get the dither pixel position from the screen coordinates.
	float4 screenPos = ComputeScreenPos(UnityObjectToClipPos(vertex));
	return float4(screenPos.xy * _ScreenParams.xy / ditherSize, 0, screenPos.w);
}

inline fixed3 GetDitherColor(fixed3 color, sampler2D ditherTex, sampler2D paletteTex,
							 float paletteHeight, float4 ditherPos, float colorCount, float patternScale) 
{
	// To find the palette color to use for this pixel:
	//	The row offset decides which row of color squares to use.
	//	The red component decides which column of color squares to use.
	//	The green and blue components points to the color in the 16x16 pixel square.
	float ditherValue = tex2D(ditherTex, (ditherPos.xy / ditherPos.w) * patternScale).r;
	ditherValue = min(ditherValue, 0.99);

	float u = min(floor(color.r * 16), 15) / 16 + clamp(color.b * 16, 0.5, 15.5) / 256;
	float v = (clamp(color.g * 16, 0.5, 15.5) + floor(ditherValue * colorCount) * 16) / paletteHeight;
	// Return the new color from the palette texture
	return tex2D(paletteTex, float2(u, v)).rgb;
}

#endif // DITHERING_INCLUDED
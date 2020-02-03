// Base Material
fixed4 _Color;
sampler2D _MainTex;
sampler2D _MetallicGlossMap;
half _Glossiness;
half _Metallic;
sampler2D _BumpMap;
half _BumpScale;
sampler2D _OcclusionMap;
half _OcclusionStrength;
sampler2D _EmissionMap;
half3 _EmissionColor;


void StandardSurface (Input IN, inout SurfaceOutputStandard o) 
{
	// NORMALS
	#ifdef _NORMALMAP
		half3 outputNormal = UnpackScaleNormal(tex2D (_BumpMap, IN.uv_MainTex), _BumpScale);
	#else
		half3 outputNormal = half3(0,0,1);
	#endif

	o.Normal = outputNormal;

	// ALBEDO
	fixed4 textureColorValue = tex2D (_MainTex, IN.uv_MainTex);
	fixed3 finalColor = _Color.xyz * textureColorValue.xyz;

	o.Albedo = finalColor;


	// METALLIC & SMOOTHNESS
	#ifdef _METALLICGLOSSMAP
		// Metallic (R) - Smoothness (A)
		half4 textureMetallicGlossValue = tex2D (_MetallicGlossMap, IN.uv_MainTex);
		o.Metallic = textureMetallicGlossValue.r;
		o.Smoothness = textureMetallicGlossValue.a;
	#else
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;
	#endif

	// OCCLUSION
	#ifdef _OCCLUSIONMAP
		// Occlusion (G)
		o.Occlusion = lerp(1.0, tex2D(_OcclusionMap, IN.uv_MainTex).g, _OcclusionStrength); 
		// Use uv_MainTex to fix occlusion.
		// See: http://forum.unity3d.com/threads/standard-surface-shader-with-ambient-occlusion-based-on-2nd-uv-set.382094/
	#endif

	// EMISSIVE
	#ifdef _EMISSIVEMAP
		// Emissive (RGB)
		half4 emissiveValue = tex2D (_EmissionMap, IN.uv_EmissionMap);
		o.Emission = emissiveValue.rgb * _EmissionColor.rgb;
	#else
		o.Emission = _EmissionColor.rgb;
	#endif


	// ALPHA - Custom alpha based on the pattern
	o.Alpha = tex2D(_PatternTex, (IN.ditherPos.xy / IN.ditherPos.w) * _PatternScale).r;
	//o.Alpha = _Color.a * textureColorValue.a;
}
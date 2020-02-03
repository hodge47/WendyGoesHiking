// Base Material
fixed4 _Color;
sampler2D _MainTex;
half _BumpScale;
sampler2D _BumpMap;


void LambertSurface (Input IN, inout SurfaceOutput o) 
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

	// ALPHA - Custom alpha based on the pattern
	o.Alpha = tex2D(_PatternTex, (IN.ditherPos.xy / IN.ditherPos.w) * _PatternScale).r;
	//o.Alpha = _Color.a * textureColorValue.a;
}
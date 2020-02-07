using UnityEngine;
//using UnityEngine.PostProcessing;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Beffio.Dithering
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Other/Stylizer")]
	[System.Serializable]
	[DisallowMultipleComponent]
	public class Stylizer : StylizerBase
	{
	
		/* ENABLING VARIABLES */
		#region Enabling variables

		[SerializeField]
		public bool Dither;
	
		[SerializeField]
		public bool Pixelate;

		[SerializeField]
		public bool Grain;

		[SerializeField]
		public bool Grain_Old;

		[SerializeField]
		public bool Grain_New;

		public GrainMod grain = new GrainMod();

		 [Tooltip("Enable the use of colored grain.")]
		public bool colored;

		[Range(0f, 1f), Tooltip("Grain strength. Higher means more visible grain.")]
		public float intensity = 0.2f;

		[Range(0.3f, 3f), Tooltip("Grain particle size.")]
		public float size = 0.4f;

		[Range(0f, 1f), Tooltip("Controls the noisiness response curve based on scene luminance. Lower values mean less noise in dark areas.")]
		public float luminanceContribution;

		[Tooltip("Is the grain static or animated.")]
		public bool animated;
	
	
		#endregion

		/* DITHERING VARIABLES */
		#region Dithering variables
		[SerializeField] private Palette _palette;
		public Palette Palette { get { return _palette; } set { _palette = value; } }
		[SerializeField] private Pattern _pattern;
		public Pattern Pattern { get { return _pattern; } set { _pattern = value; } }
		[SerializeField] private Texture2D _patternTexture;
		public Texture2D PatternTexture { get { return _patternTexture; } set { _patternTexture = value; } }

		[SerializeField] private Shader _shader;
		public Shader Shader
		{
			get
			{
				if (_shader == null)
				{
					_shader = Shader.Find("Beffio/Image Effects/Dithering Image Effect");
				}
				return _shader;
			}
		}

		private Material _material;
		public Material Material
		{
			get
			{
				if (_material == null)
				{
					_material = new Material(Shader);
					_material.hideFlags = HideFlags.HideAndDontSave;
				}
				return _material;
			}
		}
		#endregion

		/* PIXELATION VARIABLES */
		#region Pixelation variables

		[SerializeField] [Range(0.01f,1f)] [Tooltip("The level of image pixelization.")]private float _pixelScale = 0.5f;
		public float PixelScale {
			get { 
				return _pixelScale; 
			} 

			set { 
				_pixelScale = value; 
			} 
		}

		Camera cam = null;
		Camera ortoCamera = null;
		RenderTexture renTex =  null;
		GameObject pixelator = null;
		GameObject quad = null;

		Renderer quadRenderer = null;

		int previousWidth = -1;
		int previousHeight = -1;
		float previousPixelation = -0.01f;

		#endregion

		/* GRAIN VARIABLES */

		#region Grain variables

		public float intensityMultiplier = 0.25f;

        public float generalIntensity = 0.5f;
        public float blackIntensity = 1.0f;
        public float whiteIntensity = 1.0f;
        public float midGrey = 0.2f;

        public bool  dx11Grain = false;
        public float softness = 0.0001f;
        public bool  monochrome = true;

        public Vector3 intensities = new Vector3(1.0f, 1.0f, 1.0f);
        public Vector3 tiling = new Vector3(64.0f, 64.0f, 64.0f);
        public float monochromeTiling = 64.0f;

        public FilterMode filterMode = FilterMode.Bilinear;

        public Texture2D noiseTexture;

		

        public Shader noiseShader;
        private Material noiseMaterial = null;

        public Shader dx11NoiseShader;
        private Material dx11NoiseMaterial = null;

        private static float TILE_AMOUNT = 64.0f;

		Material mat; 

		//NEW GRAIN

		[SerializeField][HideInInspector]
		public PostProf profile =  null;
		

		[SerializeField][HideInInspector]
		public Material ub =  null;
		
		[SerializeField][HideInInspector]
		public Material gm =  null;
		

		GrainComp m_Grain;
		PostCont m_Context = new PostCont();
		RTF m_RenderTextureFactory = new RTF();
		MF m_MaterialFactory = new MF();

		List<PostComp> m_Components = new List<PostComp>();

		#endregion

		/* SETUP */
		#region Setup
		private void OnEnable()
		{	
			m_Grain = AddComponent(new GrainComp());
			cam = GetComponent<Camera>();

			mat =new Material(Shader.Find("Unlit/Texture"));


#if UNITY_EDITOR

		string _noiseTexturePath = "Assets/Stylizer/Resources/Shaders/NoiseAndGrain.png";
		string _defaultPalettePath = "Assets/Stylizer/Styles/Palette/Palette_1.asset";
		string _defaultNoisePath = "Assets/Stylizer/Styles/Pattern/Pattern_1.asset";

		string _profilePath = "Assets/Stylizer/Resources/Scripts/UnityGrain/Grain.asset";
		string _ubPath = "Assets/Stylizer/Resources/Scripts/UnityGrain/Shaders/Build materials/ub.mat";
		string _gmPath = "Assets/Stylizer/Resources/Scripts/UnityGrain/Shaders/Build materials/gm.mat";


			if(profile==null){
				profile = AssetDatabase.LoadAssetAtPath<PostProf>(_profilePath);
					if (profile == null)
					{
						Debug.LogWarningFormat("No grain profile found.", _profilePath);
					}
			}

			if(ub==null){
				ub = AssetDatabase.LoadAssetAtPath<Material>(_ubPath);
					if (ub == null)
					{
						Debug.LogWarningFormat("No ub material found.", _ubPath);
					}
			}

			if(gm==null){
				gm = AssetDatabase.LoadAssetAtPath<Material>(_gmPath);
					if (gm == null)
					{
						Debug.LogWarningFormat("No ub material found.", _gmPath);
					}
			}

			if(_palette==null){
				_palette = AssetDatabase.LoadAssetAtPath<Palette>(_defaultPalettePath);
					if (_palette == null)
					{
						Debug.LogWarningFormat("No default palette found.", _defaultPalettePath);
					}
			}

			if(_pattern==null){
				_pattern = AssetDatabase.LoadAssetAtPath<Pattern>(_defaultNoisePath);
					if (_pattern == null)
					{
						Debug.LogWarningFormat("No default pattern found.", _defaultNoisePath);
					}
			}

			if(noiseTexture == null){
					noiseTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(_noiseTexturePath);
					if (noiseTexture == null)
					{
						Debug.LogWarningFormat("No branding logo texture found at {0}, please make sure a texture is available at that path, or change the path in the variable _brandingLogoPath in this script.", _noiseTexturePath);

					}
			}

#endif

			checkSupport();
		}
		
		void Awake(){
			EnablePixel();
		}
			
		private void OnValidate(){
			if(previousPixelation != _pixelScale) {
				previousPixelation = _pixelScale;
				UpdateTex();
			}
			grain.settings = new Beffio.Dithering.GrainMod.Settings(colored,intensity,size,luminanceContribution,animated);
			if(profile!=null){
				profile.grain = grain;
			}
		}

		private void checkSupport()
		{
			//	Notify if we don't support image effects
			if (!SystemInfo.supportsImageEffects)
			{
				Debug.LogError("Image effects not supported.");
				return;
			}

			// Shader not found
			if (!Shader)
			{
				Debug.LogError("Shader not found.");
				return;
			}

			// Notify if the shader has errors
			// and can't run on the users graphics card
			if(!Shader.isSupported)
			{
				Debug.LogErrorFormat("Shader not supported on this graphics card, please report any error seen in the inspector when selecting the file from the shader: {0}.", Shader.name);
			}
		}

		private void OnDisable()
		{
			if (_material)
			{
				DestroyImmediate(_material);
			}
			_material = null;

			DisablePixel();
		}
			
		void OnDestroy(){
			DisablePixel();
		}

		 public override bool CheckResources ()
		{
            CheckSupport (false);

			noiseShader = Shader.Find("Hidden/NoiseAndGrain_Grain");

            noiseMaterial = CheckShaderAndCreateMaterial (noiseShader, noiseMaterial);

            if (dx11Grain && supportDX11)
			{
#if UNITY_EDITOR
                dx11NoiseShader = Shader.Find("Hidden/NoiseAndGrainDX11");
#endif
                dx11NoiseMaterial = CheckShaderAndCreateMaterial (dx11NoiseShader, dx11NoiseMaterial);
            }

            if (!isSupported)
                ReportAutoDisable ();
            return isSupported;
        }


		#endregion

		/* RENDERING */
		#region Rendering
		 [ImageEffectTransformsToLDR]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		
			if((Dither && !Grain_Old && !Grain_New)||(Dither && !Grain)) {
				DitherRender(source,destination);			
			}

			if(Grain_Old && !Dither && !Grain_New && Grain){
				OldGrainRender(source,destination);
			}

			if(Grain_New && !Dither && !Grain_Old && Grain){
				NewGrainRender(source,destination);
				#if UNITY_EDITOR
					if(!Application.isPlaying){
						NewGrainRender(source, destination);
					}
				#endif
			}

			if(Grain_Old && Dither && !Grain_New && Grain){
				Combine(source,destination);			
			}

			if(Grain_New && Dither && !Grain_Old && Grain){
				NewCombine(source,destination);	
				#if UNITY_EDITOR
					if(!Application.isPlaying){
						NewCombine(source, destination);
					}
				#endif	
			}

			

			if((!Grain_Old && !Dither && !Grain_New) || (!Dither && !Grain)) {
				Graphics.Blit(source, destination, mat);	
			}

			if(Grain_Old && Grain_New && Grain && !Dither){
				CombineGrain(source,destination);
				#if UNITY_EDITOR
					if(!Application.isPlaying){
						CombineGrain(source, destination);
					}
				#endif
			}

			if(Grain_Old && Grain_New && Dither && Grain){
				CombineEverything(source,destination);
			}


			
		}

		private void DitherRender(RenderTexture source, RenderTexture destination){


				if(_palette == null || (_pattern == null && _patternTexture == null)) {
					return;
				}

				if(!_palette.HasTexture || (_patternTexture == null && !_pattern.HasTexture)) {
					return;
				}

				Texture2D patTex = (_pattern == null ? _patternTexture : _pattern.Texture);

				Material.SetFloat("_PaletteColorCount", _palette.MixedColorCount);
				Material.SetFloat("_PaletteHeight", _palette.Texture.height);
				Material.SetTexture("_PaletteTex", _palette.Texture);
				Material.SetFloat("_PatternSize", patTex.width);
				Material.SetTexture("_PatternTex", patTex);

				Graphics.Blit(source, destination, Material);

		}

		private void OldGrainRender(RenderTexture source, RenderTexture destination){

			if (CheckResources()==false || (null==noiseTexture))
				{
					Graphics.Blit (source, destination);
					if (null==noiseTexture) {
						Debug.LogWarning("Noise & Grain effect failing as noise texture is not assigned. please assign.", transform);
					}
					return;
				}

				softness = Mathf.Clamp(softness, 0.0001f, 0.99f);

				if (dx11Grain && supportDX11)
				{
					// We have a fancy, procedural noise pattern in this version, so no texture needed

					dx11NoiseMaterial.SetFloat("_DX11NoiseTime", Time.frameCount);
					dx11NoiseMaterial.SetTexture ("_NoiseTex", noiseTexture);
					dx11NoiseMaterial.SetVector ("_NoisePerChannel", monochrome ? Vector3.one : intensities);
					dx11NoiseMaterial.SetVector ("_MidGrey", new Vector3(midGrey, 1.0f/(1.0f-midGrey), -1.0f/midGrey));
					dx11NoiseMaterial.SetVector ("_NoiseAmount", new Vector3(generalIntensity, blackIntensity, whiteIntensity) * intensityMultiplier);

					if (softness > Mathf.Epsilon)
					{
						RenderTexture rt = RenderTexture.GetTemporary((int) (source.width * (1.0f-softness)), (int) (source.height * (1.0f-softness)));
						DrawNoiseQuadGrid (source, rt, dx11NoiseMaterial, noiseTexture, monochrome ? 3 : 2);
						dx11NoiseMaterial.SetTexture("_NoiseTex", rt);
					
						Graphics.Blit(source, destination, dx11NoiseMaterial, 4);

						RenderTexture.ReleaseTemporary(rt);
					}
					else{
						DrawNoiseQuadGrid (source, destination, dx11NoiseMaterial, noiseTexture, (monochrome ? 1 : 0));
					}
				}
				else
				{
					// normal noise (DX9 style)

					if (noiseTexture) {
						noiseTexture.wrapMode = TextureWrapMode.Repeat;
						noiseTexture.filterMode = filterMode;
					}

					noiseMaterial.SetTexture ("_NoiseTex", noiseTexture);
					noiseMaterial.SetVector ("_NoisePerChannel", monochrome ? Vector3.one : intensities);
					noiseMaterial.SetVector ("_NoiseTilingPerChannel", monochrome ? Vector3.one * monochromeTiling : tiling);
					noiseMaterial.SetVector ("_MidGrey", new Vector3(midGrey, 1.0f/(1.0f-midGrey), -1.0f/midGrey));
					noiseMaterial.SetVector ("_NoiseAmount", new Vector3(generalIntensity, blackIntensity, whiteIntensity) * intensityMultiplier);

					if (softness > Mathf.Epsilon)
					{
						RenderTexture rt2 = RenderTexture.GetTemporary((int) (source.width * (1.0f-softness)), (int) (source.height * (1.0f-softness)));
						DrawNoiseQuadGrid (source, rt2, noiseMaterial, noiseTexture, 2);
						noiseMaterial.SetTexture("_NoiseTex", rt2);
						
						Graphics.Blit(source, destination, noiseMaterial, 1);
						
						RenderTexture.ReleaseTemporary(rt2);
					}
					else{	
						DrawNoiseQuadGrid (source, destination, noiseMaterial, noiseTexture, 0);
					}
				}

		}

		private void Combine(RenderTexture source, RenderTexture destination){
			
			RenderTexture transport = RenderTexture.GetTemporary(cam.pixelWidth,cam.pixelHeight);
			
			if (CheckResources()==false || (null==noiseTexture))
				{
					Graphics.Blit (source, destination);
					if (null==noiseTexture) {
						Debug.LogWarning("Noise & Grain effect failing as noise texture is not assigned. please assign.", transform);
					}
					return;
				}

				softness = Mathf.Clamp(softness, 0.0001f, 0.99f);

				if (dx11Grain && supportDX11)
				{
					// We have a fancy, procedural noise pattern in this version, so no texture needed

					dx11NoiseMaterial.SetFloat("_DX11NoiseTime", Time.frameCount);
					dx11NoiseMaterial.SetTexture ("_NoiseTex", noiseTexture);
					dx11NoiseMaterial.SetVector ("_NoisePerChannel", monochrome ? Vector3.one : intensities);
					dx11NoiseMaterial.SetVector ("_MidGrey", new Vector3(midGrey, 1.0f/(1.0f-midGrey), -1.0f/midGrey));
					dx11NoiseMaterial.SetVector ("_NoiseAmount", new Vector3(generalIntensity, blackIntensity, whiteIntensity) * intensityMultiplier);

					if (softness > Mathf.Epsilon)
					{
						RenderTexture rt = RenderTexture.GetTemporary((int) (source.width * (1.0f-softness)), (int) (source.height * (1.0f-softness)));
						DrawNoiseQuadGrid (source, rt, dx11NoiseMaterial, noiseTexture, monochrome ? 3 : 2);
						dx11NoiseMaterial.SetTexture("_NoiseTex", rt);
						
						/* THE BLIT */
						Graphics.Blit(source, transport, dx11NoiseMaterial, 4);

						RenderTexture.ReleaseTemporary(rt);
					}
					else{
						DrawNoiseQuadGrid (source, destination, dx11NoiseMaterial, noiseTexture, (monochrome ? 1 : 0));
					}
				}
				else
				{
					// normal noise (DX9 style)

					if (noiseTexture) {
						noiseTexture.wrapMode = TextureWrapMode.Repeat;
						noiseTexture.filterMode = filterMode;
					}

					noiseMaterial.SetTexture ("_NoiseTex", noiseTexture);
					noiseMaterial.SetVector ("_NoisePerChannel", monochrome ? Vector3.one : intensities);
					noiseMaterial.SetVector ("_NoiseTilingPerChannel", monochrome ? Vector3.one * monochromeTiling : tiling);
					noiseMaterial.SetVector ("_MidGrey", new Vector3(midGrey, 1.0f/(1.0f-midGrey), -1.0f/midGrey));
					noiseMaterial.SetVector ("_NoiseAmount", new Vector3(generalIntensity, blackIntensity, whiteIntensity) * intensityMultiplier);

					if (softness > Mathf.Epsilon)
					{
						RenderTexture rt2 = RenderTexture.GetTemporary((int) (source.width * (1.0f-softness)), (int) (source.height * (1.0f-softness)));
						DrawNoiseQuadGrid (source, rt2, noiseMaterial, noiseTexture, 2);
						noiseMaterial.SetTexture("_NoiseTex", rt2);
						
						/* THE BLIT */
						Graphics.Blit(source, transport, noiseMaterial, 1);
						
						RenderTexture.ReleaseTemporary(rt2);
					}
					else{
						DrawNoiseQuadGrid (source, destination, noiseMaterial, noiseTexture, 0);
					}
				}

				if(_palette == null || (_pattern == null && _patternTexture == null)) {
					return;
				}

				if(!_palette.HasTexture || (_patternTexture == null && !_pattern.HasTexture)) {
					return;
				}

				Texture2D patTex = (_pattern == null ? _patternTexture : _pattern.Texture);

				Material.SetFloat("_PaletteColorCount", _palette.MixedColorCount);
				Material.SetFloat("_PaletteHeight", _palette.Texture.height);
				Material.SetTexture("_PaletteTex", _palette.Texture);
				Material.SetFloat("_PatternSize", patTex.width);
				Material.SetTexture("_PatternTex", patTex);

				Graphics.Blit(transport, destination, Material);

				RenderTexture.ReleaseTemporary(transport);
		}

		private void NewGrainRender(RenderTexture source, RenderTexture destination){
			if(profile.grain.enabled==false){
				profile.grain.enabled=true;
			}
			
			var context = m_Context.Reset();
            context.profile = profile;
            context.renderTextureFactory = m_RenderTextureFactory;
            context.materialFactory = m_MaterialFactory;
            context.camera = cam;
#if UNITY_EDITOR
			var uberMaterial = m_MaterialFactory.Get("Hidden/Post FX/Uber Shader_Grain");
#else
			var uberMaterial = ub;
#endif
            uberMaterial.shaderKeywords = null;

			Texture autoExposure = GU.whiteTexture;
			uberMaterial.SetTexture("_AutoExposure", autoExposure);


			m_Grain.Init(context,profile.grain);

			TryPrepareUberImageEffect(m_Grain, uberMaterial);

			Graphics.Blit(source, destination, uberMaterial, 0);

		 	m_RenderTextureFactory.ReleaseAll();

		
		}

		private void NewCombine(RenderTexture source, RenderTexture destination){
			
			RenderTexture transport = RenderTexture.GetTemporary(cam.pixelWidth,cam.pixelHeight);
			
			if(profile.grain.enabled==false){
				profile.grain.enabled=true;
			}
			
			var context = m_Context.Reset();
            context.profile = profile;
            context.renderTextureFactory = m_RenderTextureFactory;
            context.materialFactory = m_MaterialFactory;
            context.camera = cam;
#if UNITY_EDITOR
			var uberMaterial = m_MaterialFactory.Get("Hidden/Post FX/Uber Shader_Grain");
#else
			var uberMaterial = ub;
#endif
            uberMaterial.shaderKeywords = null;

			Texture autoExposure = GU.whiteTexture;
			uberMaterial.SetTexture("_AutoExposure", autoExposure);

			m_Grain.Init(context,profile.grain);

			TryPrepareUberImageEffect(m_Grain, uberMaterial);

			Graphics.Blit(source, transport, uberMaterial, 0);

			if(_palette == null || (_pattern == null && _patternTexture == null)) {
				return;
			}

			if(!_palette.HasTexture || (_patternTexture == null && !_pattern.HasTexture)) {
				return;
			}

			Texture2D patTex = (_pattern == null ? _patternTexture : _pattern.Texture);

			Material.SetFloat("_PaletteColorCount", _palette.MixedColorCount);
			Material.SetFloat("_PaletteHeight", _palette.Texture.height);
			Material.SetTexture("_PaletteTex", _palette.Texture);
			Material.SetFloat("_PatternSize", patTex.width);
			Material.SetTexture("_PatternTex", patTex);

			Graphics.Blit(transport, destination, Material);

			RenderTexture.ReleaseTemporary(transport);

			m_RenderTextureFactory.ReleaseAll();
		}

		private void CombineGrain(RenderTexture source, RenderTexture destination){
			RenderTexture transport = RenderTexture.GetTemporary(cam.pixelWidth,cam.pixelHeight);

			if (CheckResources()==false || (null==noiseTexture))
			{
				Graphics.Blit (source, destination);
				if (null==noiseTexture) {
					Debug.LogWarning("Noise & Grain effect failing as noise texture is not assigned. please assign.", transform);
				}
				return;
			}

			softness = Mathf.Clamp(softness, 0.0001f, 0.99f);

			if (dx11Grain && supportDX11)
			{
				// We have a fancy, procedural noise pattern in this version, so no texture needed

				dx11NoiseMaterial.SetFloat("_DX11NoiseTime", Time.frameCount);
				dx11NoiseMaterial.SetTexture ("_NoiseTex", noiseTexture);
				dx11NoiseMaterial.SetVector ("_NoisePerChannel", monochrome ? Vector3.one : intensities);
				dx11NoiseMaterial.SetVector ("_MidGrey", new Vector3(midGrey, 1.0f/(1.0f-midGrey), -1.0f/midGrey));
				dx11NoiseMaterial.SetVector ("_NoiseAmount", new Vector3(generalIntensity, blackIntensity, whiteIntensity) * intensityMultiplier);

				if (softness > Mathf.Epsilon)
				{
					RenderTexture rt = RenderTexture.GetTemporary((int) (source.width * (1.0f-softness)), (int) (source.height * (1.0f-softness)));
					DrawNoiseQuadGrid (source, rt, dx11NoiseMaterial, noiseTexture, monochrome ? 3 : 2);
					dx11NoiseMaterial.SetTexture("_NoiseTex", rt);
					
					/* THE BLIT */
					Graphics.Blit(source, transport, dx11NoiseMaterial, 4);

					RenderTexture.ReleaseTemporary(rt);
				}
				else{
					DrawNoiseQuadGrid (source, destination, dx11NoiseMaterial, noiseTexture, (monochrome ? 1 : 0));
				}
			}
			else
			{
				// normal noise (DX9 style)

				if (noiseTexture) {
					noiseTexture.wrapMode = TextureWrapMode.Repeat;
					noiseTexture.filterMode = filterMode;
				}

				noiseMaterial.SetTexture ("_NoiseTex", noiseTexture);
				noiseMaterial.SetVector ("_NoisePerChannel", monochrome ? Vector3.one : intensities);
				noiseMaterial.SetVector ("_NoiseTilingPerChannel", monochrome ? Vector3.one * monochromeTiling : tiling);
				noiseMaterial.SetVector ("_MidGrey", new Vector3(midGrey, 1.0f/(1.0f-midGrey), -1.0f/midGrey));
				noiseMaterial.SetVector ("_NoiseAmount", new Vector3(generalIntensity, blackIntensity, whiteIntensity) * intensityMultiplier);

				if (softness > Mathf.Epsilon)
				{
					RenderTexture rt2 = RenderTexture.GetTemporary((int) (source.width * (1.0f-softness)), (int) (source.height * (1.0f-softness)));
					DrawNoiseQuadGrid (source, rt2, noiseMaterial, noiseTexture, 2);
					noiseMaterial.SetTexture("_NoiseTex", rt2);
					
					/* THE BLIT */
					Graphics.Blit(source, transport, noiseMaterial, 1);
					
					RenderTexture.ReleaseTemporary(rt2);
				}
				else{
					DrawNoiseQuadGrid (source, destination, noiseMaterial, noiseTexture, 0);
				}
			}

			if(profile.grain.enabled==false){
				profile.grain.enabled=true;
			}
			
			var context = m_Context.Reset();
            context.profile = profile;
            context.renderTextureFactory = m_RenderTextureFactory;
            context.materialFactory = m_MaterialFactory;
            context.camera = cam;
#if UNITY_EDITOR
			var uberMaterial = m_MaterialFactory.Get("Hidden/Post FX/Uber Shader_Grain");
#else
			var uberMaterial = ub;
#endif
            uberMaterial.shaderKeywords = null;

			Texture autoExposure = GU.whiteTexture;
			uberMaterial.SetTexture("_AutoExposure", autoExposure);

			m_Grain.Init(context,profile.grain);

			TryPrepareUberImageEffect(m_Grain, uberMaterial);

			Graphics.Blit(transport, destination, uberMaterial, 0);

			RenderTexture.ReleaseTemporary(transport);

			m_RenderTextureFactory.ReleaseAll();
		}

		private void CombineEverything(RenderTexture source, RenderTexture destination){
			RenderTexture transport = RenderTexture.GetTemporary(cam.pixelWidth,cam.pixelHeight);
			
			if (CheckResources()==false || (null==noiseTexture))
			{
				Graphics.Blit (source, destination);
				if (null==noiseTexture) {
					Debug.LogWarning("Noise & Grain effect failing as noise texture is not assigned. please assign.", transform);
				}
				return;
			}

			softness = Mathf.Clamp(softness, 0.0001f, 0.99f);

			if (dx11Grain && supportDX11)
			{
				// We have a fancy, procedural noise pattern in this version, so no texture needed

				dx11NoiseMaterial.SetFloat("_DX11NoiseTime", Time.frameCount);
				dx11NoiseMaterial.SetTexture ("_NoiseTex", noiseTexture);
				dx11NoiseMaterial.SetVector ("_NoisePerChannel", monochrome ? Vector3.one : intensities);
				dx11NoiseMaterial.SetVector ("_MidGrey", new Vector3(midGrey, 1.0f/(1.0f-midGrey), -1.0f/midGrey));
				dx11NoiseMaterial.SetVector ("_NoiseAmount", new Vector3(generalIntensity, blackIntensity, whiteIntensity) * intensityMultiplier);

				if (softness > Mathf.Epsilon)
				{
					RenderTexture rt = RenderTexture.GetTemporary((int) (source.width * (1.0f-softness)), (int) (source.height * (1.0f-softness)));
					DrawNoiseQuadGrid (source, rt, dx11NoiseMaterial, noiseTexture, monochrome ? 3 : 2);
					dx11NoiseMaterial.SetTexture("_NoiseTex", rt);
					
					/* THE BLIT */
					Graphics.Blit(source, transport, dx11NoiseMaterial, 4);

					RenderTexture.ReleaseTemporary(rt);
				}
				else{
					DrawNoiseQuadGrid (source, destination, dx11NoiseMaterial, noiseTexture, (monochrome ? 1 : 0));
				}
			}
			else
			{
				// normal noise (DX9 style)

				if (noiseTexture) {
					noiseTexture.wrapMode = TextureWrapMode.Repeat;
					noiseTexture.filterMode = filterMode;
				}

				noiseMaterial.SetTexture ("_NoiseTex", noiseTexture);
				noiseMaterial.SetVector ("_NoisePerChannel", monochrome ? Vector3.one : intensities);
				noiseMaterial.SetVector ("_NoiseTilingPerChannel", monochrome ? Vector3.one * monochromeTiling : tiling);
				noiseMaterial.SetVector ("_MidGrey", new Vector3(midGrey, 1.0f/(1.0f-midGrey), -1.0f/midGrey));
				noiseMaterial.SetVector ("_NoiseAmount", new Vector3(generalIntensity, blackIntensity, whiteIntensity) * intensityMultiplier);

				if (softness > Mathf.Epsilon)
				{
					RenderTexture rt2 = RenderTexture.GetTemporary((int) (source.width * (1.0f-softness)), (int) (source.height * (1.0f-softness)));
					DrawNoiseQuadGrid (source, rt2, noiseMaterial, noiseTexture, 2);
					noiseMaterial.SetTexture("_NoiseTex", rt2);
					
					/* THE BLIT */
					Graphics.Blit(source, transport, noiseMaterial, 1);
					
					RenderTexture.ReleaseTemporary(rt2);
				}
				else{
					DrawNoiseQuadGrid (source, destination, noiseMaterial, noiseTexture, 0);
				}
			}

			if(profile.grain.enabled==false){
				profile.grain.enabled=true;
			}
			
			var context = m_Context.Reset();
            context.profile = profile;
            context.renderTextureFactory = m_RenderTextureFactory;
            context.materialFactory = m_MaterialFactory;
            context.camera = cam;
#if UNITY_EDITOR
			var uberMaterial = m_MaterialFactory.Get("Hidden/Post FX/Uber Shader_Grain");
#else
			var uberMaterial = ub;
#endif
            uberMaterial.shaderKeywords = null;

			Texture autoExposure = GU.whiteTexture;
			uberMaterial.SetTexture("_AutoExposure", autoExposure);


			m_Grain.Init(context,profile.grain);

			TryPrepareUberImageEffect(m_Grain, uberMaterial);

			//Graphics.Blit(source, destination, uberMaterial, 0);

			RenderTexture transport2 = RenderTexture.GetTemporary(cam.pixelWidth,cam.pixelHeight);

			Graphics.Blit(transport, transport2, uberMaterial, 0);

			RenderTexture.ReleaseTemporary(transport);

			if(_palette == null || (_pattern == null && _patternTexture == null)) {
				return;
			}

			if(!_palette.HasTexture || (_patternTexture == null && !_pattern.HasTexture)) {
				return;
			}

			Texture2D patTex = (_pattern == null ? _patternTexture : _pattern.Texture);

			Material.SetFloat("_PaletteColorCount", _palette.MixedColorCount);
			Material.SetFloat("_PaletteHeight", _palette.Texture.height);
			Material.SetTexture("_PaletteTex", _palette.Texture);
			Material.SetFloat("_PatternSize", patTex.width);
			Material.SetTexture("_PatternTex", patTex);

			Graphics.Blit(transport2, destination, Material);

			RenderTexture.ReleaseTemporary(transport2);
			m_RenderTextureFactory.ReleaseAll();
		}

		static void DrawNoiseQuadGrid (RenderTexture source, RenderTexture dest, Material fxMaterial, Texture2D noise, int passNr)
		{

            RenderTexture.active = dest;

            float noiseSize = (noise.width * 1.0f);
            float subDs = (1.0f * source.width) / TILE_AMOUNT;

            fxMaterial.SetTexture ("_MainTex", source);

            GL.PushMatrix ();
            GL.LoadOrtho ();

            float aspectCorrection = (1.0f * source.width) / (1.0f * source.height);
            float stepSizeX = 1.0f / subDs;
            float stepSizeY = stepSizeX * aspectCorrection;
            float texTile = noiseSize / (noise.width * 1.0f);

            fxMaterial.SetPass (passNr);

            GL.Begin (GL.QUADS);

            for (float x1 = 0.0f; x1 < 1.0f; x1 += stepSizeX)
			{
                for (float y1 = 0.0f; y1 < 1.0f; y1 += stepSizeY)
				{
                    float tcXStart = UnityEngine.Random.Range (0.0f, 1.0f);
                    float tcYStart = UnityEngine.Random.Range (0.0f, 1.0f);

                    tcXStart = Mathf.Floor(tcXStart*noiseSize) / noiseSize;
                    tcYStart = Mathf.Floor(tcYStart*noiseSize) / noiseSize;

                    float texTileMod = 1.0f / noiseSize;

                    GL.MultiTexCoord2 (0, tcXStart, tcYStart);
                    GL.MultiTexCoord2 (1, 0.0f, 0.0f);
                    //GL.Color( c );
                    GL.Vertex3 (x1, y1, 0.1f);
                    GL.MultiTexCoord2 (0, tcXStart + texTile * texTileMod, tcYStart);
                    GL.MultiTexCoord2 (1, 1.0f, 0.0f);
                    //GL.Color( c );
                    GL.Vertex3 (x1 + stepSizeX, y1, 0.1f);
                    GL.MultiTexCoord2 (0, tcXStart + texTile * texTileMod, tcYStart + texTile * texTileMod);
                    GL.MultiTexCoord2 (1, 1.0f, 1.0f);
                    //GL.Color( c );
                    GL.Vertex3 (x1 + stepSizeX, y1 + stepSizeY, 0.1f);
                    GL.MultiTexCoord2 (0, tcXStart, tcYStart + texTile * texTileMod);
                    GL.MultiTexCoord2 (1, 0.0f, 1.0f);
                    //GL.Color( c );
                    GL.Vertex3 (x1, y1 + stepSizeY, 0.1f);
                }
            }

            GL.End ();
            GL.PopMatrix ();
        }

		#endregion

		/* PIXELATION FUNCTIONALITY */
		#region Pixelation

		public void EnablePixel(){

			if(pixelator == null || pixelator.GetComponent<Camera>() == null || quad == null || ortoCamera == null) {
				if(enabled){
				SetUpPixelator();
				}
			}
		}

		public void DisablePixel(){

			DestroyImmediate(pixelator);
			if(cam != null) {
				cam.targetTexture = null;
			}
		}

		void Update(){

			if(pixelator == null || cam == null || quad == null || ortoCamera == null) {
				SetUpPixelator();
			}


			if(renTex == null && Pixelate) {

				renTex = new RenderTexture((int)(ortoCamera.pixelWidth*_pixelScale), (int)(ortoCamera.pixelHeight*_pixelScale), 24, RenderTextureFormat.Default);
				renTex.filterMode = FilterMode.Point;
				renTex.name = "Pixel render texture";
				renTex.Create();
				cam.targetTexture = renTex;

			}

			if(ortoCamera.pixelWidth != previousWidth || ortoCamera.pixelHeight != previousHeight) {
				ResizeQuad();
				previousWidth = ortoCamera.pixelWidth;
				previousHeight = ortoCamera.pixelHeight;
			}

			if(quadRenderer.sharedMaterial.mainTexture != renTex) {
				quadRenderer.sharedMaterial.mainTexture = renTex;
			}

			if(previousPixelation != _pixelScale) {
				previousPixelation = _pixelScale;
				UpdateTex();
			}

			if(!Pixelate) {
				ortoCamera.enabled = false;
				cam.targetTexture = null;
				DestroyImmediate(pixelator);
			} else {
				ortoCamera.enabled = true;
				cam.targetTexture = renTex;
			}

		}
		//update the render texture
		void UpdateTex(){
			if(ortoCamera != null && Pixelate) {
				if(renTex!=null){
					renTex.Release();
				}
				renTex = new RenderTexture((int)(ortoCamera.pixelWidth*_pixelScale), (int)(ortoCamera.pixelHeight*_pixelScale), 24, RenderTextureFormat.Default);
				renTex.filterMode = FilterMode.Point;
				renTex.name = "Pixel render texture";
				renTex.Create();
				cam.targetTexture = renTex;
				quad.GetComponent<Renderer>().sharedMaterial.mainTexture = renTex;
			}
		}
		//set up the pixelator object
		void SetUpPixelator(){
	
			if(GameObject.Find("Pixelator")) {
				DestroyImmediate(GameObject.Find("Pixelator"));
			}

			if(GetComponent<Camera>() != null) {
				cam = GetComponent<Camera>();
			}

			if(pixelator == null) {
				pixelator = new GameObject("Pixelator");
			}
			pixelator.transform.position = new Vector3(0, 0, -9999);
			if(pixelator.GetComponent<Camera>() == null) {
				pixelator.AddComponent<Camera>();
			}
				
			ortoCamera = pixelator.GetComponent<Camera>();

			ortoCamera.orthographic = true;
			ortoCamera.orthographicSize = 0.5f;
			ortoCamera.farClipPlane = 2.0f;
			ortoCamera.depth = cam.depth;
			ortoCamera.rect = cam.rect;
			ortoCamera.clearFlags = CameraClearFlags.Nothing;
			ortoCamera.useOcclusionCulling = false;

			if(quad == null) {
				quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
				quad.name = "Quad";
				DestroyImmediate(quad.GetComponent<Collider>());
				quad.transform.parent = pixelator.transform;
				quad.transform.localPosition = 1.0f * pixelator.transform.forward;

				ResizeQuad();

				Material mat = new Material(Shader.Find("Unlit/Texture"));
				mat.mainTexture = renTex;

				quadRenderer = quad.GetComponent<Renderer>();

				quadRenderer.sharedMaterial = mat;
			}

			ResizeQuad();
		}
		//resize the quad if the camera viewport changes
		void ResizeQuad(){
			ortoCamera.rect = cam.rect;
			if(ortoCamera.pixelHeight > 0) {
				quad.transform.localScale = new Vector3(
					(float)ortoCamera.pixelWidth / ortoCamera.pixelHeight,
					1f, 1f);
			} else {
				quad.transform.localScale = Vector3.one;
			}
			UpdateTex();

		}

		#endregion

		#region New grain functions
		T AddComponent<T>(T component)
            where T : PostComp
        {
            m_Components.Add(component);
            return component;
        }

		bool TryPrepareUberImageEffect<T>(PostProcessingComponentRenderTexture<T> component, Material material)
            where T : PostMod
        {
            if (!component.active)
                return false;

            component.Prepare(material, gm);
            return true;
        }

		#endregion

	}
}
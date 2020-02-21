using UnityEngine;
using System.Collections;
using UnityEditor;


namespace Beffio.Dithering
{
	[CustomEditor(typeof(Pattern))]
	public class PatternEditor : Editor 
	{
		/* UI TEXT */
		private static class ContentText
		{
			public static GUIContent windowTitle = new GUIContent("Pattern Generation");

			public static GUIContent[] types = new GUIContent[]
				{
					new GUIContent("Noise", "A random noise texture"),
					new GUIContent("Checkerboard", "A checkerboard pattern"),
					new GUIContent("Lines", "A line pattern")
				};

			public static GUIContent textureSizeProperty = new GUIContent("Texture Size", "Size of the texture width and height");
			public static GUIContent noiseRangeProperty = new GUIContent("Noise Value Range", "Minimum and maximum values of the pattern color");
			public static GUIContent dotsRangeProperty = new GUIContent("Dots Value Range", "Minimum and maximum values of the pattern color");
			public static GUIContent varianceProperty = new GUIContent("Value Variance", "Per pixel variance of the pattern color");
			public static GUIContent dotsSizeProperty = new GUIContent("Dot Size", "Size of the checker pattern dots");
			public static GUIContent lineRangeProperty = new GUIContent("Lines Value Range", "Minimum and maximum values of the pattern color");
			public static GUIContent lineWidthProperty = new GUIContent("Line Width", "Width of the lines");
			public static GUIContent lineDirectiomProperty = new GUIContent("Line Direction", "Direction to render the lines");

			public static GUIContent generateNoiseButton = new GUIContent("Generate", "Generate a texture with a noise pattern");
			public static GUIContent generateDotsButton = new GUIContent("Generate", "Generate a texture with a dotted pattern");
			public static GUIContent generateLinesButton = new GUIContent("Generate", "Generate a texture with a line pattern");

			public static GUIContent previewHeader = new GUIContent("Palette Texture", "");

			public static string notGeneratedHelpBox = "Your asset has been edited, but the texture is not generated yet.";

			public static string inspectorUndo = "Inspector";

			public static string uiMissingPatternTypeWarning = "No UI drawing specified for the pattern type: {0}";
			public static string buttonMissingPatternTypeWarning = "No button text specified for the pattern type: {0}";
			public static string generationMissingPatternTypeWarning = "No generation algorithm specified for the pattern type: {0}";
			public static string linesMissingDirectionWarning = "The line direction {0} is not supported yet.";
		}

		/* AUTO GENERATE */
		private bool _autoGenerate = true;

		/* VARIABLES */
		#region Variables
		private Pattern _pattern;
		public Pattern CurrentPattern
		{
			get
			{
				if (_pattern == null)
				{
					_pattern = target as Pattern;
				}
				return _pattern;
			}
		}
		#endregion

		/* SETUP */
		#region Setup
		private void OnEnable()
		{
			Undo.undoRedoPerformed += OnUndoRedo;
		}

		private void OnDisable()
		{
			Undo.undoRedoPerformed -= OnUndoRedo;
		}

		private void OnUndoRedo()
		{
			if (_autoGenerate)
			{
				Generate(CurrentPattern);
				CurrentPattern.IsDirty = false;
			}
		}
		#endregion

		/* EDITOR DRAWING */
		#region Editor Drawing
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			// Branding
			BeffioDitherEditorUtilities.DrawInspectorBranding();
			GUILayout.Space(10);

			// Cache dirty state to prevent layouting error
			bool isDirty = CurrentPattern.IsDirty;

			// Settings
			EditorGUI.BeginChangeCheck();
			{
				// Texture Settings
				DrawTextureSettingGUI();

				// Type Selection
				DrawPatternTypeSelectionGUI();

				// Pattern Settings
				DrawPatternSettingsGUI();

				// Apply changes to serializedObject
				serializedObject.ApplyModifiedProperties();

				if(EditorGUI.EndChangeCheck())
				{
					if (_autoGenerate)
					{
						Generate(CurrentPattern);
						BeffioDitherEditorUtilities.UpdateMaterialsForTexture(_pattern.Texture);
						CurrentPattern.IsDirty = false;
					}
					else
					{
						CurrentPattern.IsDirty = true;
					}
				}
			}

			// Buttons
			DrawButtonGUI();

			// Help Message
			if (isDirty)
			{
				EditorGUILayout.HelpBox(ContentText.notGeneratedHelpBox, MessageType.Warning);
			}
		}

		private void DrawTextureSettingGUI()
		{
			// Texture Size
			EditorGUI.BeginChangeCheck();
			int texturePower = EditorGUILayout.IntSlider(ContentText.textureSizeProperty, CurrentPattern.TextureSize, 4, 64, GUILayout.ExpandWidth(true));
			if(EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(CurrentPattern, ContentText.inspectorUndo);
				CurrentPattern.TextureSize = Mathf.ClosestPowerOfTwo(texturePower);
				EditorUtility.SetDirty(CurrentPattern);
			}
		}

		private void DrawPatternTypeSelectionGUI()
		{
			// Toolbar
			EditorGUI.BeginChangeCheck();
			{
				Beffio.Dithering.PatternType type = (Beffio.Dithering.PatternType)GUILayout.Toolbar((int)CurrentPattern.Type, ContentText.types);
				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(CurrentPattern, ContentText.inspectorUndo);
					CurrentPattern.Type = type;
					EditorUtility.SetDirty(CurrentPattern);
				}
			}
		}

		private void DrawPatternSettingsGUI()
		{
			switch (CurrentPattern.Type)
			{
				case PatternType.Noise:
					DrawNoiseSettingsGUI();
					break;
				case PatternType.Dots:
					DrawDotPatternSettingsGUI();
					break;
				case PatternType.Lines:
					DrawLinePatternSettingsGUI();
					break;
				default:
					Debug.LogWarningFormat(ContentText.uiMissingPatternTypeWarning, CurrentPattern.Type);
					break;
			}
		}

		private void DrawNoiseSettingsGUI()
		{
			// Minimum and Maximum Color Values
			EditorGUI.BeginChangeCheck();
			float min = CurrentPattern.MinimumValue;
			float max = CurrentPattern.MaximumValue;
			EditorGUILayout.MinMaxSlider(ContentText.noiseRangeProperty, ref min, ref max, 0.0f, 1.0f);
			if(EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(CurrentPattern, ContentText.inspectorUndo);
				CurrentPattern.MinimumValue = min;
				CurrentPattern.MaximumValue = max;
				EditorUtility.SetDirty(CurrentPattern);
			}
		}

		private void DrawDotPatternSettingsGUI()
		{
			// Minimum and Maximum Color Values
			EditorGUI.BeginChangeCheck();
			{
				float min = CurrentPattern.MinimumValue;
				float max = CurrentPattern.MaximumValue;
				EditorGUILayout.MinMaxSlider(ContentText.dotsRangeProperty, ref min, ref max, 0.0f, 1.0f);
				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(CurrentPattern, ContentText.inspectorUndo);
					CurrentPattern.MinimumValue = min;
					CurrentPattern.MaximumValue = max;
					EditorUtility.SetDirty(CurrentPattern);
				}
			}

			// Color Variance
			SerializedProperty colorVarianceProperty = serializedObject.FindProperty("ColorVariance");
			EditorGUILayout.Slider(colorVarianceProperty, 0.0f, 0.5f, ContentText.varianceProperty);

			// Element Size
			EditorGUI.BeginChangeCheck();
			int elementSize = EditorGUILayout.IntSlider(ContentText.dotsSizeProperty, (int)CurrentPattern.ElementSize, 1, 8);
			if(EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(CurrentPattern, ContentText.inspectorUndo);
				CurrentPattern.ElementSize = (float)Mathf.ClosestPowerOfTwo(elementSize);
				EditorUtility.SetDirty(CurrentPattern);
			}
		}

		private void DrawLinePatternSettingsGUI()
		{
			// Minimum and Maximum Color Values
			EditorGUI.BeginChangeCheck();
			float min = CurrentPattern.MinimumValue;
			float max = CurrentPattern.MaximumValue;
			EditorGUILayout.MinMaxSlider(ContentText.lineRangeProperty, ref min, ref max, 0.0f, 1.0f);
			if(EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(CurrentPattern, ContentText.inspectorUndo);
				CurrentPattern.MinimumValue = min;
				CurrentPattern.MaximumValue = max;
				EditorUtility.SetDirty(CurrentPattern);
			}

			// Color Variance
			SerializedProperty colorVarianceProperty = serializedObject.FindProperty("ColorVariance");
			EditorGUILayout.Slider(colorVarianceProperty, 0.0f, 0.5f, ContentText.varianceProperty);

			// Element Size
			EditorGUI.BeginChangeCheck();
			int elementSize = EditorGUILayout.IntSlider(ContentText.lineWidthProperty, (int)CurrentPattern.ElementSize, 1, 8);
			if(EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(CurrentPattern, ContentText.inspectorUndo);
				CurrentPattern.ElementSize = (float)Mathf.ClosestPowerOfTwo(elementSize);
				EditorUtility.SetDirty(CurrentPattern);
			}

			// Line Direction
			EditorGUI.BeginChangeCheck();
			LineDirection direction = (LineDirection)EditorGUILayout.EnumPopup(ContentText.lineDirectiomProperty, CurrentPattern.Direction);
			if(EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(CurrentPattern, ContentText.inspectorUndo);
				CurrentPattern.Direction = direction;
				EditorUtility.SetDirty(CurrentPattern);
			}
		}

		private void DrawButtonGUI()
		{
			// Generate button
			GUILayout.Space(10);
			GUIContent generateButton = ContentText.generateNoiseButton;
			switch (CurrentPattern.Type)
			{
				case PatternType.Noise:
					generateButton = ContentText.generateNoiseButton;
					break;
				case PatternType.Dots:
					generateButton = ContentText.generateDotsButton;
					break;
				case PatternType.Lines:
					generateButton = ContentText.generateLinesButton;
					break;
				default:
					Debug.LogWarningFormat(ContentText.buttonMissingPatternTypeWarning, CurrentPattern.Type);
					break;
			}
			if (GUILayout.Button(generateButton))
			{
				Generate(CurrentPattern);
				BeffioDitherEditorUtilities.UpdateMaterialsForTexture(_pattern.Texture);
				CurrentPattern.IsDirty = false;
			}
		}

		public override GUIContent GetPreviewTitle()
		{
			return ContentText.previewHeader;
		}

		public override bool HasPreviewGUI()
		{
			return true;
		}

		public override void OnPreviewGUI(Rect r, GUIStyle background)
		{
			Texture2D texture = CurrentPattern.Texture;

			if (texture != null)
			{
				// Calculate width and height
				float aspectRatio = texture.width / texture.height;

				float height = r.height;
				float width = height * aspectRatio;

				if (r.width < width)
				{
					width = r.width;
					height = width / aspectRatio;
				}

				// Calculate center position
				float x = r.width / 2;
				x -= width / 2;
				x += r.x;
				float y = r.height / 2;
				y -= height / 2;
				y += r.y;

				// Draw
				GUI.DrawTexture(new Rect(x, y, width, height), CurrentPattern.Texture);
			}
		}
		#endregion

		/* PATTERN GENERATION */
		#region Pattern generation

		public void Generate(Pattern pattern)
		{
			int size = pattern.TextureSize;

			if (pattern.Texture == null)
			{
				// Create pattern if it doesn't exist yet
				pattern.Texture = new Texture2D(size, size, TextureFormat.RGB24, false);
				pattern.Texture.name = "Pattern Texture";
				pattern.Texture.filterMode = FilterMode.Point;
				pattern.Texture.wrapMode = TextureWrapMode.Repeat;

				AssetDatabase.AddObjectToAsset(pattern.Texture, pattern);
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(pattern.Texture));
			}
			else if (pattern.Texture.width != size)
			{
				// Resize pattern if necessary
				pattern.Texture.Resize(size, size);
			}

			// Generate Pattern
			switch (pattern.Type)
			{
				case PatternType.Noise:
					GenerateRandomNoise(pattern);
					break;
				case PatternType.Dots:
					GenerateDotPattern(pattern);
					break;
				case PatternType.Lines:
					GenerateLinePattern(pattern);
					break;
				default:
					Debug.LogWarningFormat(ContentText.generationMissingPatternTypeWarning, pattern.Type);
					break;
			}

			// Apply Pattern
			pattern.Texture.Apply();
			pattern.HasTexture = true;
		}

		/// <summary>
		/// Generates a pattern with random noise.
		/// </summary>
		/// <param name="pattern">The pattern settings and texture to use.</param>
		private void GenerateRandomNoise(Pattern pattern)
		{
			int x = 0, y = 0;
			Color color = new Color(1,0,0,1);
			int width = pattern.Texture.width;
			int height = pattern.Texture.height;
			float maxValue = pattern.MaximumValue;

			float value;
			for (int i = 0; i < width * height; ++i)
			{
				x = i / width;
				y = i % width;

				// Generate random value
				value = UnityEngine.Random.Range(pattern.MinimumValue, maxValue);
				color.r = value;
				color.g = value;
				color.b = value;
				pattern.Texture.SetPixel(x, y, color);
			}
		}
	
		/// <summary>
		/// Generates a pattern with checkerboard.
		/// </summary>
		/// <param name="pattern">The pattern settings and texture to use.</param>
		private void GenerateDotPattern(Pattern pattern)
		{
			int x = 0, y = 0;
			Color color = new Color(1,0,0,1);
			bool isDot = false;
			float doubleSize = pattern.ElementSize * 2;
			float halfSize = pattern.ElementSize;
			int width = pattern.Texture.width;
			int height = pattern.Texture.height;
			float maxValue = pattern.MaximumValue;
			
			float value;
			for (int i = 0; i < width * height; ++i)
			{
				x = i / width;
				y = i % width;

				// Check if is one or the other value
				isDot = (x % doubleSize < halfSize && y % doubleSize < halfSize)
					|| (x % doubleSize >= halfSize && y % doubleSize >= halfSize);

				if (isDot)
				{
					value = UnityEngine.Random.Range(maxValue, maxValue - pattern.ColorVariance);
				}
				else
				{
					value = UnityEngine.Random.Range(pattern.MinimumValue, pattern.MinimumValue + pattern.ColorVariance);
				}
				color.r = value;
				color.g = value;
				color.b = value;

				pattern.Texture.SetPixel(x, y, color);
			}
		}

		/// <summary>
		/// Generates a pattern with lines.
		/// </summary>
		/// <param name="pattern">The pattern settings and texture to use.</param>
		private void GenerateLinePattern(Pattern pattern)
		{
			// Setup angle
			float angle = 0;
			switch (pattern.Direction)
			{
				case LineDirection.Horizontal:
					angle = Mathf.PI/2;
					break;
				case LineDirection.Vertical:
					angle = 0;
					break;
				case LineDirection.Slope45:
					angle = Mathf.PI / 4;
					break;
				case LineDirection.Slope135:
					angle = 3 * Mathf.PI / 4;
					break;
				default:
					Debug.LogWarningFormat(ContentText.linesMissingDirectionWarning, pattern.Direction);
					break;
			}

			// Generate with angle
			GenerateLinePatternWithAngle(pattern, angle);
		}

		/// <summary>
		/// Internal method for generating lines on a texture over a specified angle.
		/// </summary>
		/// <param name="pattern">The pattern settings and texture to use.</param>
		private void GenerateLinePatternWithAngle(Pattern pattern, float angle)
		{
			int x = 0, y = 0;
			float rx = 0.0f;
			Color color = new Color(1,0,0,1);

			float maxValue = pattern.MaximumValue;

			float varianceMinLimit = Mathf.Lerp(pattern.MinimumValue, maxValue, 0.333f);
			float varianceMaxLimit = Mathf.Lerp(pattern.MinimumValue, maxValue, 0.666f);

			int width = pattern.Texture.width;
			int height = pattern.Texture.height;
			float elementSize = pattern.ElementSize;

			float shortAngle = angle % (Mathf.PI / 2);
			if (shortAngle > Mathf.PI / 4)
			{
				shortAngle = (Mathf.PI / 2) - shortAngle;
			}
			float multiplier = (Mathf.Sqrt(width * width *2) / width); // 1 ... 1.414213;
			multiplier = ((multiplier - 1) * Mathf.InverseLerp(0, Mathf.PI / 4, shortAngle)) + 1;
				
			elementSize *=  multiplier;

			float value;
			for (int i = 0; i < width * height; ++i)
			{
				x = i / width;
				y = i % width;

				rx = x * Mathf.Cos(angle) - y * Mathf.Sin(angle);

				// Lines based on the cosine
				value = (Mathf.Cos(rx * Mathf.PI / elementSize) + 1)/ 2;

				// Scale value between min and max limit
				value = Mathf.Lerp(pattern.MinimumValue, maxValue, value);

				// Apply random variance
				if (CurrentPattern.ColorVariance > .001f)
				{
					if (value < varianceMinLimit)
					{
						value += UnityEngine.Random.Range(0, pattern.ColorVariance);
					}
					else if (value > varianceMaxLimit)
					{
						value -= UnityEngine.Random.Range(0, pattern.ColorVariance);
					}
					else
					{
						value += UnityEngine.Random.Range(- pattern.ColorVariance, pattern.ColorVariance);
					}
				}
				color.r = value;
				color.g = value;
				color.b = value;

				pattern.Texture.SetPixel(x, y, color);
			}
		}

		#endregion
	}
}

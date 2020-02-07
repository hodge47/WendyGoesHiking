using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Beffio.Dithering
{
	[CustomEditor(typeof(Palette))]
	public class PaletteEditor : Editor 
	{
		// UI Text
		private static class ContentText
		{
			public static GUIContent windowTitle = new GUIContent("Palette Generation");

			public static GUIContent mixedColorCountProperty = new GUIContent("Mixed Color Count", "The amount of different colors the pattern can choose from");

			public static GUIContent colorListHeader = new GUIContent("Palette Color List", "List with colors included in the palette");

			public static GUIContent previewHeader = new GUIContent("Palette Texture", "");

			public static GUIContent autoGenerate = new GUIContent("Auto Generate", "Will automatically generate the texture when editing a property. (Might result in slow performance)");
			public static GUIContent imageFromListButton = new GUIContent("List From Image", "Generate a palette list from the colors found in an image");
			public static GUIContent generateFromListButton = new GUIContent("Generate Texture", "Generate a palette texture from the selected colors in the list");

			public static GUIContent randomizeColors = new GUIContent("Randomize colors", "Randomize all existing colors");
			public static GUIContent randomizeColorsBW = new GUIContent("Randomize colors only in B&W", "Randomize all existing colors in black and white");
			public static GUIContent selectiveRandomization = new GUIContent("Selective randomization", "Randomize only selected RGB values");

			public static string noTextureHelpBox = "No Texture has been generated yet.";
			public static string notGeneratedHelpBox = "Your asset has been edited, but the texture is not generated yet.";

			public static string tooManyColorsDialogTitle = "Error";
			public static string tooManyColorsDialogText = "Source image contains more than {0} colors. Continuing may lock up Unity for a long time";
			public static string tooManyColorsDialogContinueButton = "Continue";
			public static string tooManyColorsDialogStopButton = "Stop";
			public static string selectImageDialogText = "Select your .PNG color image";

			public static string emptyColorListError = "List of colors is empty.";
		}


		/* VARIABLES */
		#region Variables
		private ReorderableList _colorList;
		public ReorderableList ColorList
		{
			get
			{
				if (_colorList == null)
				{
					LoadColorList();
				}
				return _colorList;
			}
		}

		private Palette _palette;
		public Palette CurrentPalette
		{
			get
			{
				if (_palette == null)
				{
					_palette = target as Palette;
					LoadColorList();
				}
				return _palette;
			}
		}

		[SerializeField] private string _loadPath = "";

		// Settings
		private const int _colorSquares = 16;
		private const int _maxColors = 256;

		private static bool _autoGenerate = false;
		private static bool _randomBW = false;

		private static bool _selectiveRandomization = false;
		private static bool _r = false;
		private static bool _g = false;
		private static bool _b = false;

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

		private void LoadColorList()
		{
			_colorList = new ReorderableList(serializedObject, serializedObject.FindProperty("Colors"), true, false, true, true);
			_colorList.drawElementCallback = DrawColorElement;
			_colorList.onAddCallback = AddColorElement;
			_colorList.drawHeaderCallback = DrawColorListHeader;
		}

		private void DrawColorElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			rect.y += 2;
			SerializedProperty element = ColorList.serializedProperty.GetArrayElementAtIndex(index); 
			EditorGUI.PropertyField(  
				new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
				element, GUIContent.none);
		}

		private void AddColorElement(ReorderableList list)
		{
			CurrentPalette.Colors.Add(Color.magenta);
		}

		private void DrawColorListHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, ContentText.colorListHeader);
		}

		private void OnUndoRedo()
		{
			if (_autoGenerate)
			{
				GeneratePaletteTexture(CurrentPalette);
				CurrentPalette.IsDirty = false;
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

			// Cache dirty and texture states to prevent layouting error
			bool hasTexture = CurrentPalette.HasTexture;
			bool isDirty = CurrentPalette.IsDirty;

			EditorGUI.BeginChangeCheck();
			{
				// Settings
				DrawSettingsGUI();

				// Color List
				DrawColorListGUI();

				// Apply Changes to serializedObject
				serializedObject.ApplyModifiedProperties();

				if (EditorGUI.EndChangeCheck())
				{
					if (_autoGenerate)
					{
						GeneratePaletteTexture(CurrentPalette);
						BeffioDitherEditorUtilities.UpdateMaterialsForTexture(_palette.Texture);
						CurrentPalette.IsDirty = false;
					}
					else
					{
						CurrentPalette.IsDirty = true;
					}
				}
			}

			GUILayout.Space(10);

			// Auto generate checkbox
			_autoGenerate = EditorGUILayout.Toggle(ContentText.autoGenerate, _autoGenerate);

			// Buttons
			DrawButtonsGUI();

			// BW randomization checkbox
			_randomBW = EditorGUILayout.Toggle(ContentText.randomizeColorsBW, _randomBW);
			if(!_randomBW){
				_selectiveRandomization = EditorGUILayout.Toggle(ContentText.selectiveRandomization, _selectiveRandomization);
				if(_selectiveRandomization){
					EditorGUI.indentLevel = 1;
					_r = EditorGUILayout.Toggle("R", _r);
					_g = EditorGUILayout.Toggle("G", _g);
					_b = EditorGUILayout.Toggle("B", _b);
					EditorGUI.indentLevel = 0;
				}
			}

			// Help Messages
			DrawHelpBoxGUI(hasTexture, isDirty);
		}

		private void DrawSettingsGUI()
		{
			// Mixed Color Count
			SerializedProperty mixedColorProperty = serializedObject.FindProperty("MixedColorCount");
			EditorGUILayout.IntSlider(mixedColorProperty, 1, 16, ContentText.mixedColorCountProperty);
		}

		private void DrawColorListGUI()
		{
			// Color List
			ColorList.DoLayoutList();

			// Populate List From Image
			if (GUILayout.Button(ContentText.imageFromListButton, GUILayout.Width(100)))
			{
				GenerateListFromImage();
			}
		}

		float v;
		private void DrawButtonsGUI()
		{
			// Generate Button
			if (GUILayout.Button(ContentText.generateFromListButton))
			{
				GeneratePaletteTexture(CurrentPalette);
				BeffioDitherEditorUtilities.UpdateMaterialsForTexture(_palette.Texture);
				CurrentPalette.IsDirty = false;
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Random color generation",EditorStyles.boldLabel);
			//EditorGUILayout.LabelField("_______________________________________________________________________________________________________________");

			// Randomize colors button
			if (GUILayout.Button(ContentText.randomizeColors))
			{
				for(int i = 0; i < CurrentPalette.Colors.Count; i++){
					if(!_randomBW && !_selectiveRandomization){
						CurrentPalette.Colors[i] = new Color(UnityEngine.Random.Range(0f,1f),UnityEngine.Random.Range(0f,1f),UnityEngine.Random.Range(0f,1f));
					}else if(_randomBW){
						v = UnityEngine.Random.Range(0f,1f);
						CurrentPalette.Colors[i] = new Color(v,v,v);
					}else if(!_randomBW && _selectiveRandomization){
						if(_r && _g && _b){
							CurrentPalette.Colors[i] = new Color(UnityEngine.Random.Range(0f,1f),UnityEngine.Random.Range(0f,1f),UnityEngine.Random.Range(0f,1f));
						}else if(!_r && _g && _b){
							CurrentPalette.Colors[i] = new Color(CurrentPalette.Colors[i].r,UnityEngine.Random.Range(0f,1f),UnityEngine.Random.Range(0f,1f));
						}else if(_r && !_g && _b){
							CurrentPalette.Colors[i] = new Color(UnityEngine.Random.Range(0f,1f),CurrentPalette.Colors[i].g,UnityEngine.Random.Range(0f,1f));
						}else if(_r && _g && !_b){
							CurrentPalette.Colors[i] = new Color(UnityEngine.Random.Range(0f,1f),UnityEngine.Random.Range(0f,1f),CurrentPalette.Colors[i].b);
						}else if(!_r && _g && !_b){
							CurrentPalette.Colors[i] = new Color(CurrentPalette.Colors[i].r,UnityEngine.Random.Range(0f,1f),CurrentPalette.Colors[i].b);
						}else if(_r && !_g && !_b){
							CurrentPalette.Colors[i] = new Color(UnityEngine.Random.Range(0f,1f),CurrentPalette.Colors[i].g,CurrentPalette.Colors[i].b);
						}else if(!_r && !_g && _b){
							CurrentPalette.Colors[i] = new Color(CurrentPalette.Colors[i].r,CurrentPalette.Colors[i].g,UnityEngine.Random.Range(0f,1f));
						}else if(!_r && !_g && !_b){
							CurrentPalette.Colors[i] = new Color(CurrentPalette.Colors[i].r,CurrentPalette.Colors[i].g,CurrentPalette.Colors[i].b);
						}
					}
				}

				GeneratePaletteTexture(CurrentPalette);
				BeffioDitherEditorUtilities.UpdateMaterialsForTexture(_palette.Texture);
				CurrentPalette.IsDirty = false;
			}
		}

		private void DrawHelpBoxGUI(bool hasTexture, bool isDirty)
		{
			// No Texture
			if (!hasTexture)
			{
				EditorGUILayout.HelpBox(ContentText.noTextureHelpBox, MessageType.Warning);
			}
			// Texture not generated
			if (isDirty)
			{
				EditorGUILayout.HelpBox(ContentText.notGeneratedHelpBox, MessageType.Warning);
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
			Texture2D texture = CurrentPalette.Texture;
			
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
				GUI.DrawTexture(new Rect(x, y, width, height), CurrentPalette.Texture);
			}
		}
		#endregion

		/* LIST GENERATION */
		#region List Generation

		/// <summary>
		/// Populate the color list from the colors found in an image.
		/// - Will check for each pixel in the image if the color is already present in the list,
		///   and add the color to the list if not present yet.
		/// </summary>
		private void GenerateListFromImage() 
		{
			Texture2D colorTexture = SelectAndLoadTexture();

			if (colorTexture == null) 
				return;

			// Remove all previous colors
			CurrentPalette.Colors.Clear();
			
			// Create a list of all the unique colors in the color image
			bool proceed = false;
			Color color;
			for (int x = 0; x < colorTexture.width; x++)
			{
				for (int y = 0; y < colorTexture.height; y++)
				{
					color = colorTexture.GetPixel(x, y);
					if (!CurrentPalette.Colors.Contains(color)) // Check if color isn't already in the list
					{
						CurrentPalette.Colors.Add(color);
						if (!proceed && CurrentPalette.Colors.Count > _maxColors) // Check if list size doesn't exceed maximum amount
						{
							proceed = EditorUtility.DisplayDialog(
								ContentText.tooManyColorsDialogTitle, 
								string.Format(ContentText.tooManyColorsDialogText, _maxColors),
								ContentText.tooManyColorsDialogContinueButton,
								ContentText.tooManyColorsDialogStopButton);
							
							if (!proceed)
							{
								return;
							}
						}
					}
				}
			}

			EditorUtility.SetDirty(CurrentPalette);
		}
		#endregion

		/* PALETTE GENERATION */
		#region Palette generation

		/// <summary>
		///  Creates and saves a palette image from the unique colors in another image.
		///  
		///  The palette image structure:
		/// 
		///  |  |  |  |  |  |  |  |    row of 16 16x16 squares with the third mix color
		///  |__|__|__|__|__|__|__|__ 
		///  |  |  |  |  |  |  |  |    row of 16 16x16 squares with the second mix color
		///  |__|__|__|__|__|__|__|__ 
		///  |  |  |  |  |  |  |  |    row of 16 16x16 squares with the first mix color
		///  |__|__|__|__|__|__|__|__ 
		/// 
		///  Each horizontal square has a fixed Red component: 0/15 to 15/15.
		///  The blue component increases from 0 to 1 horizontally over a square.
		///  The green component increases from 0 to 1 vertically over a square.
		/// 
		///  The palette image is used in the shaders to convert a truecolor to several dithered colors.
		///  The truecolor RGB components points to N colors in the palette, one color per row of squares. 
		///  The N colors are mixed together in a dithering pattern to produce a close approximation of the original truecolor.
		///  
		///  The steps to create the palette image:
		/// 
		///	 1. Load the color image to a Texture2D
		///	 2. Create a list of all the unique colors in the color image
		///	 3. Create the palette image
		///	 4. a) Loop through each pixel in the palette image and determine the truecolor for that pixel
		///		b) Device a mixing plan to achieve the truecolor
		///		c) Save the N colors in the square column
		///  5. Save the palette image
		/// </summary>
		private void GeneratePaletteTexture(Palette palette)
		{
			if (palette.Colors.Count == 0)
			{
				Debug.LogError(ContentText.emptyColorListError);
				return;
			}

			uint[] paletteColors = ConvertColorListToPaletteList(palette.Colors);
			MixingPlanner mixingPlanner = new MixingPlanner(paletteColors);

			// Create the palette image
			int height = (int)Math.Pow(2, Math.Ceiling(Math.Log(_colorSquares * CurrentPalette.MixedColorCount - 1) / Math.Log(2)));

			if (palette.Texture == null)
			{
				palette.Texture = new Texture2D(_colorSquares * _colorSquares, height, TextureFormat.RGB24, false);
				palette.Texture.name = "Palette Texture";
				palette.Texture.filterMode = FilterMode.Point;
				palette.Texture.wrapMode = TextureWrapMode.Repeat;

				AssetDatabase.AddObjectToAsset(palette.Texture, palette);
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(palette.Texture));

			}
			else if (palette.Texture.height != height)
			{
				palette.Texture.Resize(_colorSquares * _colorSquares, height);
			}

			// Fill empty rows with magenta
			int x = 0, y = 0;
			for (int i = 0; i < palette.Texture.height * palette.Texture.width; i++)
			{
				x = i / palette.Texture.height;
				y = i % palette.Texture.width;
				palette.Texture.SetPixel(x, y, Color.magenta);
			}

			// Loop through each pixel in the palette image and determine the target color for that pixel
			for (x = 0; x < _colorSquares * _colorSquares; x++)
			{
				for (y = 0; y < _colorSquares; y++)
				{
					byte r = (byte)((float)(x / _colorSquares) / (_colorSquares - 1) * 255);
					byte g = (byte)((float)y / (_colorSquares - 1) * 255);
					byte b = (byte)(((float)x % _colorSquares) / (_colorSquares - 1) * 255);
					uint targetColor = (uint)((r << 16) + (g << 8) + b);

					// Device a mixing plan to achieve the truecolor
					uint[] mixingPlan = mixingPlanner.DeviseBestMixingPlan(targetColor, (uint)CurrentPalette.MixedColorCount);

					// Save the N colors in the square column
					for (int c = 0; c < CurrentPalette.MixedColorCount; c++)
					{
						palette.Texture.SetPixel(x, y + (CurrentPalette.MixedColorCount - c - 1) * _colorSquares, palette.Colors[(int)mixingPlan[c]]);
					}
				}
			}

			palette.Texture.Apply();
			palette.HasTexture = true;

			// Save the palette image
			//		SaveTexture(texture);
		}

		private uint[] ConvertColorListToPaletteList(List<Color> colors)
		{
			uint[] palette = new uint[colors.Count];
			for (int i = 0; i < colors.Count; i++)
			{
				palette[i] = ColorToInt(colors[i]);
			}

			return palette;
		}

		private static uint ColorToInt(Color color)
		{
			return ((uint)(color.r * 255) << 16) + ((uint)(color.g * 255) << 8) + (uint)(color.b * 255);
		}

		/// <summary>
		///  The mixing planner is based on algorithms and code by Joel Yliluoma.
		///  http://bisqwit.iki.fi/story/howto/dither/jy/
		/// </summary>
		private class MixingPlanner 
		{

			private const double Gamma = 2.2;

			private uint[] palette;
			private uint[] luminance;
			private double[,] gammaCorrect;

			private double GammaCorrect(double v) 
			{
				return Math.Pow(v, Gamma);
			}
			private double GammaUncorrect(double v) 
			{
				return Math.Pow(v, 1.0 / Gamma);
			}

			public MixingPlanner(uint[] palette) 
			{
				this.palette = palette;
				luminance = new uint[palette.Length];
				gammaCorrect = new double[palette.Length, 3];

				for (int i = 0; i < palette.Length; i++) 
				{
					byte r = (byte)((palette[i] >> 16) & 0xff);
					byte g = (byte)((palette[i] >> 8) & 0xff);
					byte b = (byte)(palette[i] & 0xff);

					luminance[i] = (uint)(r * 299 + g * 587 + b * 114);

					gammaCorrect[i, 0] = GammaCorrect(r / 255.0);
					gammaCorrect[i, 1] = GammaCorrect(g / 255.0);
					gammaCorrect[i, 2] = GammaCorrect(b / 255.0);
				}
			}

			private double ColorCompare(byte r1, byte g1, byte b1, byte r2, byte g2, byte b2) 
			{
				double luma1 = (r1 * 299 + g1 * 587 + b1 * 114) / (255.0 * 1000);
				double luma2 = (r2 * 299 + g2 * 587 + b2 * 114) / (255.0 * 1000);
				double lumadiff = luma1 - luma2;
				double diffR = (r1 - r2) / 255.0, diffG = (g1 - g2) / 255.0, diffB = (b1 - b2) / 255.0;
				return (diffR * diffR * 0.299 + diffG * diffG * 0.587 + diffB * diffB * 0.114) * 0.75 + lumadiff * lumadiff;
			}

			public uint[] DeviseBestMixingPlan(uint targetColor, uint colorCount) 
			{
				byte[] inputRgb = new byte[] 
					{ 
						(byte)((targetColor >> 16) & 0xff), 
						(byte)((targetColor >> 8) & 0xff), 
						(byte)(targetColor & 0xff) 
					};

				uint[] mixingPlan = new uint[colorCount];

				if (palette.Length == 2) 
				{
					// Use an alternative planning algorithm if the palette only has 2 colors
					uint[] soFar = new uint[] { 0, 0, 0 };

					uint proportionTotal = 0;
					while (proportionTotal < colorCount) 
					{
						uint chosenAmount = 1;
						uint chosen = 0;

						uint maxTestCount = Math.Max(1, proportionTotal);

						double leastPenalty = -1;
						for (uint i = 0; i < palette.Length; ++i) {
							uint color = palette[i];
							uint[] sum = new uint[] { soFar[0], soFar[1], soFar[2] };
							uint[] add = new uint[] { color >> 16, (color >> 8) & 0xff, color & 0xff };
							for (uint p = 1; p <= maxTestCount; p *= 2)
							{
								for (uint c = 0; c < 3; ++c) sum[c] += add[c];
								for (uint c = 0; c < 3; ++c) add[c] += add[c];
								uint t = proportionTotal + p;
								uint[] test = new uint[] { sum[0] / t, sum[1] / t, sum[2] / t };
								double penalty = ColorCompare((byte)inputRgb[0], (byte)inputRgb[1], (byte)inputRgb[2],
									(byte)test[0], (byte)test[1], (byte)test[2]);
								if (penalty < leastPenalty || leastPenalty < 0) 
								{
									leastPenalty = penalty;
									chosen = i;
									chosenAmount = p;
								}
							}
						}
						for (uint p = 0; p < chosenAmount; ++p)
						{
							if (proportionTotal >= colorCount) break;
							mixingPlan[proportionTotal++] = chosen;
						}

						uint newColor = palette[chosen];
						uint[] palcolor = new uint[] { newColor >> 16, (newColor >> 8) & 0xff, newColor & 0xff };

						for (uint c = 0; c < 3; ++c)
						{
							soFar[c] += palcolor[c] * chosenAmount;
						}
					}
				}
				else
				{
					// Use the gamma corrected planning algorithm if the palette has more than 2 colors
					Dictionary<uint, uint> solution = new Dictionary<uint, uint>();

					double currentPenalty = -1;

					uint chosenIndex = 0;
					for (uint i = 0; i < palette.Length; ++i)
					{
						byte r = (byte)((palette[i] >> 16) & 0xff);
						byte g = (byte)((palette[i] >> 8) & 0xff);
						byte b = (byte)(palette[i] & 0xff);

						double penalty = ColorCompare(inputRgb[0], inputRgb[1], inputRgb[2], r, g, b);
						if (penalty < currentPenalty || currentPenalty < 0)
						{
							currentPenalty = penalty;
							chosenIndex = i;
						}
					}
					solution[chosenIndex] = colorCount;

					double dblLimit = 1.0 / colorCount;
					while (currentPenalty != 0.0)
					{
						double bestPenalty = currentPenalty;
						uint bestSplitFrom = 0;
						uint[] bestSplitTo = new uint[] { 0, 0 };

						foreach (KeyValuePair<uint, uint> i in solution)
						{
							uint splitColor = i.Key;
							uint splitCount = i.Value;

							double[] sum = new double[] { 0, 0, 0 };
							foreach (KeyValuePair<uint, uint> j in solution)
							{
								if (j.Key == splitColor)
									continue;
								sum[0] += gammaCorrect[j.Key, 0] * j.Value * dblLimit;
								sum[1] += gammaCorrect[j.Key, 1] * j.Value * dblLimit;
								sum[2] += gammaCorrect[j.Key, 2] * j.Value * dblLimit;
							}

							double portion1 = (splitCount / 2) * dblLimit;
							double portion2 = (splitCount - splitCount / 2) * dblLimit;
							for (uint a = 0; a < palette.Length; ++a)
							{
								uint firstb = 0;
								if (portion1 == portion2)
								{
									firstb = a + 1;
								}

								for (uint b = firstb; b < palette.Length; ++b)
								{
									if (a == b)
										continue;
									int lumadiff = (int)(luminance[a]) - (int)(luminance[b]);
									if (lumadiff < 0)
										lumadiff = -lumadiff;
									if (lumadiff > 80000)
										continue;

									double[] test = new double[]
										{ 
											GammaUncorrect(sum[0] + gammaCorrect[a, 0] * portion1 + gammaCorrect[b, 0] * portion2),
											GammaUncorrect(sum[1] + gammaCorrect[a, 1] * portion1 + gammaCorrect[b, 1] * portion2),
											GammaUncorrect(sum[2] + gammaCorrect[a, 2] * portion1 + gammaCorrect[b, 2] * portion2) 
										};

									double penalty = ColorCompare(inputRgb[0], inputRgb[1], inputRgb[2],
										(byte)(test[0] * 255), (byte)(test[1] * 255), (byte)(test[2] * 255));

									if (penalty < bestPenalty)
									{
										bestPenalty = penalty;
										bestSplitFrom = splitColor;
										bestSplitTo[0] = a;
										bestSplitTo[1] = b;
									}

									if (portion2 == 0)
									{
										break;
									}
								}
							}
						}

						if (bestPenalty == currentPenalty)
						{
							break;
						}

						uint splitC = solution[bestSplitFrom];
						uint split1 = splitC / 2;
						uint split2 = splitC - split1;
						solution.Remove(bestSplitFrom);
						if (split1 > 0)
						{
							solution[bestSplitTo[0]] = (solution.ContainsKey(bestSplitTo[0]) ? solution[bestSplitTo[0]] : 0) + split1;
						}
						if (split2 > 0)
						{
							solution[bestSplitTo[1]] = (solution.ContainsKey(bestSplitTo[1]) ? solution[bestSplitTo[1]] : 0) + split2;
						}
						currentPenalty = bestPenalty;
					}

					uint n = 0;
					foreach (KeyValuePair<uint, uint> i in solution)
					{
						for (uint c = 0; c < i.Value; c++)
						{
							mixingPlan[n++] = i.Key;
						}
					}
				}

				// Sort the colors by luminance and return the mixing plan
				Array.Sort(mixingPlan, delegate(uint index1, uint index2) 
					{
						return luminance[index1].CompareTo(luminance[index2]);
					});
				return mixingPlan;
			}

		}
		#endregion

		/* LOADING AND SAVING */
		#region Loading and Saving

		/// <summary>
		///  Opens a file dialog and loads a .png image to a Texture2D.
		/// </summary>
		private Texture2D SelectAndLoadTexture() 
		{
			string path = EditorUtility.OpenFilePanel(ContentText.selectImageDialogText, _loadPath, "png");
			if (path == "")
			{
				return null;
			}

			_loadPath = Path.GetDirectoryName(path);

			return BeffioDitherEditorUtilities.LoadTexture(path);
		}
		#endregion

	}
}

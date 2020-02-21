using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Beffio.Dithering
{
	public class StandardDitheredEditor : StandardShaderBaseEditor 
	{
		// UI Text
		private static class ContentText
		{
			public static GUIContent ditheringHeader = new GUIContent("Dithering", "");

			public static GUIContent paletteProperty = new GUIContent("Palette Asset", "Asset - The colors which can appear in the dithering effect, and how many colors can be mixed");
			public static GUIContent patternProperty = new GUIContent("Pattern Asset", "Asset - The pattern deciding which pixels are sampled from which palette square");
			public static GUIContent patternTextureProperty = new GUIContent("Pattern Texture", "Texture (R) - The pattern deciding which pixels are sampled from which palette square");
			public static GUIContent patternScaleProperty = new GUIContent("Pattern Scale", "Scale of the pattern");
			public static GUIContent screenSpaceProperty = new GUIContent("Screen Space Pattern", "Enabled: Apply pattern over screen - Disabled: Apply pattern over regular UV coordinates");
			public static GUIContent cutoutProperty = new GUIContent("Alpha Cutout", "The value of the texture to cut out");

			public static string noTextureWarning = "{0}The selected {1} '{2}' did not generate the texture yet. Press the 'Generate Texture' button on the asset.\n";
			public static string oldTextureWarning = "{0}The selected {1} '{2}' has been adjusted, but the texture is not updated yet. Press the 'Generate Texture' button on the asset.\n";

			public static string inspectorUndo = "Inspector";
			public static string screenSpaceUndo = "Screen Space";
		}
		
		/* VARIABLES */
		#region Variables
		// Dithering Properties
		private MaterialProperty _colorCountProperty 			= null;
		private MaterialProperty _paletteHeightProperty 		= null;
		private MaterialProperty _paletteTexProperty 			= null;
		private MaterialProperty _patternSizeProperty 			= null;
		private MaterialProperty _patternTexProperty 			= null;
		private MaterialProperty _patternScaleProperty 			= null;

		private MaterialProperty _cutoutProperty 				= null;

		private MaterialProperty _paletteRefProperty 			= null;
		private MaterialProperty _patternRefProperty 			= null;
		private MaterialProperty _hasPatternTextureProperty 	= null;


		// Dithering
		[SerializeField] private Palette _palette;
		[SerializeField] private Pattern _pattern;
		[SerializeField] private Texture2D _patternTexture;

		#endregion

		/* SETUP */
		#region Setup

		/// <summary>
		/// Find the shader properties.
		/// </summary>
		private void FindProperties(MaterialProperty[] properties)
		{
			_colorCountProperty 		= FindProperty("_PaletteColorCount", 	properties);
			_paletteHeightProperty		= FindProperty("_PaletteHeight",		properties);
			_paletteTexProperty 		= FindProperty("_PaletteTex", 			properties);
			_patternSizeProperty		= FindProperty("_PatternSize",			properties);
			_patternTexProperty			= FindProperty("_PatternTex",			properties);
			_patternScaleProperty 		= FindProperty("_PatternScale", 		properties);

			if (_targetMat.HasProperty("_Cutout"))
			{
				_cutoutProperty 		= FindProperty("_Cutout", 				properties);
			}

			_paletteRefProperty			= FindProperty("__paletteRef",			properties);
			_patternRefProperty			= FindProperty("__patternRef",			properties);
			_hasPatternTextureProperty	= FindProperty("__hasPatternTex",		properties);
		}
		#endregion

		/* INSPECTOR DRAWING */
		#region Inspector Drawing
		protected override void DrawGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			// Branding
			BeffioDitherEditorUtilities.DrawInspectorBranding();
			EditorGUILayout.Space();

			// Base Information
			FindProperties(properties);

			// Draw Base Paint Settings
			DrawStandardPropertiesGUI(materialEditor, properties);

			EditorGUILayout.Space();
			EditorGUILayout.LabelField(ContentText.ditheringHeader, EditorStyles.boldLabel);

			DrawDitheringSettingsGUI(materialEditor);
		}

		private static string _defaultPalettePath = "Assets/Stylizer/Styles/Palette/Palette_1.asset";
		private static string _defaultPatternPath = "Assets/Stylizer/Styles/Pattern/Pattern_1.asset";
		private void DrawDitheringSettingsGUI(MaterialEditor materialEditor)
		{
			// Hack: 	Get Palette and Pattern objects.
			// 			Because it's not possible to save references to other objects within a shader or material object,
			//			This hack save the instance id's in shader properties, by converting the exact bit setup to vectors.
			_palette = 	BeffioDitherEditorUtilities.GetAssetFromVector(_paletteRefProperty.vectorValue) as Palette;
			_pattern = 	BeffioDitherEditorUtilities.GetAssetFromVector(_patternRefProperty.vectorValue) as Pattern;

			if(_palette==null){
				_palette = AssetDatabase.LoadAssetAtPath<Palette>(_defaultPalettePath);
			}
			
			if(_pattern==null){
				_pattern = AssetDatabase.LoadAssetAtPath<Pattern>(_defaultPatternPath);
			}

			bool hasPatternTexture = (_hasPatternTextureProperty.floatValue > 0.5f);

			EditorGUI.indentLevel = 2;

			// Palette
			_palette = EditorGUILayout.ObjectField(ContentText.paletteProperty, _palette, typeof(Palette), false) as Palette;
			if (_palette != null)
			{
				_colorCountProperty.floatValue = _palette.MixedColorCount;
				_paletteHeightProperty.floatValue = _palette.Texture.height;
				_paletteTexProperty.textureValue = _palette.Texture;
				_paletteRefProperty.vectorValue = BeffioDitherEditorUtilities.AssetToVector4GUID(_palette);

			}
			else
			{
				_colorCountProperty.floatValue = 0;
				_paletteHeightProperty.floatValue = Texture2D.whiteTexture.height;
				_paletteTexProperty.textureValue = Texture2D.whiteTexture;
				_paletteRefProperty.vectorValue = Vector4.zero;
			}

			// Pattern Asset
			if (!hasPatternTexture)
			{
				EditorGUI.BeginChangeCheck();
				{
					_pattern = EditorGUILayout.ObjectField(ContentText.patternProperty, _pattern, typeof(Pattern), false) as Pattern;
					if (EditorGUI.EndChangeCheck())
					{
						if (_pattern != null)
						{
							_patternSizeProperty.floatValue = _pattern.Texture.height;
							_patternTexProperty.textureValue = _pattern.Texture;
							_patternRefProperty.vectorValue = BeffioDitherEditorUtilities.AssetToVector4GUID(_pattern);
						}
						else
						{
							_patternSizeProperty.floatValue = 0;
							_patternTexProperty.textureValue = null;
							_patternRefProperty.vectorValue = Vector4.zero;
						}
						_hasPatternTextureProperty.floatValue = 0;
					}
				}
			}


			// Pattern Texture
			if (_pattern == null)
			{
				EditorGUI.BeginChangeCheck();
				{
					EditorGUI.indentLevel = 0;
					materialEditor.TexturePropertySingleLine(ContentText.patternTextureProperty, _patternTexProperty);
					if (EditorGUI.EndChangeCheck())
					{
						if (_patternTexProperty.textureValue)
						{
							_patternSizeProperty.floatValue = _patternTexProperty.textureValue.height;
						}
						else
						{
							_patternSizeProperty.floatValue = 0;
						}
						_hasPatternTextureProperty.floatValue = (_patternTexProperty.textureValue == null ? 0 : 1);
					}
				}
			}

			EditorGUI.indentLevel = 2;

			// Pattern Scale
			materialEditor.FloatProperty(_patternScaleProperty, ContentText.patternScaleProperty.text);

			// Screen Space Dithering
			EditorGUI.BeginChangeCheck();
			{
				bool enabled = EditorGUILayout.Toggle(ContentText.screenSpaceProperty, _targetMat.IsKeywordEnabled("_SCREENSPACEDITHER"));
				if (EditorGUI.EndChangeCheck())
				{
					EnableKeyword("_SCREENSPACEDITHER", enabled, ContentText.screenSpaceUndo);
				}
			}

			// Alpha Cutout
			if (_targetMat.HasProperty("_Cutout"))
			{
				materialEditor.ShaderProperty(_cutoutProperty, ContentText.cutoutProperty.text);
			}
		}
		#endregion
	}
}

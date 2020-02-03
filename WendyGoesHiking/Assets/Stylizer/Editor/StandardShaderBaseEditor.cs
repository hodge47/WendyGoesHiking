using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class StandardShaderBaseEditor : ShaderGUI 
{
	// UI Text
	private static class BaseContentText
	{
		public static GUIContent baseAlbedoProperty = new GUIContent("Base Albedo", "Albedo (RGB) and Transparency (A)");
		public static GUIContent baseMetallicMapProperty = new GUIContent("Base Metallic", "Metallic (R) and Smoothness (A)");
		public static GUIContent baseSmoothnessProperty = new GUIContent("Smoothness");
		public static GUIContent normalMapProperty = new GUIContent("Normal Map", "Normal Map");
		public static GUIContent occlusionMapProperty = new GUIContent("Occlusion", "Occlusion (G)");
		public static GUIContent emissionMapProperty = new GUIContent("Emission", "Emission (RGB)");
	}

	/* VARIABLES */
	#region Variables
	// Material
	protected Material _targetMat = null;
	protected List<Material> _targetMats = new List<Material>();

	// Material Properties
	protected MaterialProperty _albedoMapProperty 				= null;
	protected MaterialProperty _albedoColorProperty 			= null;
	protected MaterialProperty _metallicMapProperty 			= null;
	protected MaterialProperty _metallicProperty 				= null;
	protected MaterialProperty _smoothnessProperty 				= null;
	protected MaterialProperty _normalMapProperty 				= null;
	protected MaterialProperty _normalScaleProperty 			= null;
	protected MaterialProperty _occlusionMapProperty 			= null;
	protected MaterialProperty _occlusionStrengthProperty 		= null;
	protected MaterialProperty _emissionMapProperty 			= null;
	protected MaterialProperty _emissionColorProperty 			= null;

	// Emission
	private ColorPickerHDRConfig _colorPickerHDRConfig = new ColorPickerHDRConfig(0f, 99f, 1/99f, 3f);
	#endregion

	/* SETUP */
	#region Setup

	/// <summary>
	/// Find the shader properties.
	/// </summary>
	private void FindBaseProperties(MaterialProperty[] properties)
	{
		_albedoColorProperty 		= FindProperty("_Color", 				properties);
		_albedoMapProperty			= FindProperty("_MainTex",				properties);
		_metallicProperty 			= FindProperty("_Metallic", 			properties);
		_smoothnessProperty 		= FindProperty("_Glossiness",			properties);
		_metallicMapProperty 		= FindProperty("_MetallicGlossMap",		properties);
		_normalMapProperty 			= FindProperty("_BumpMap",				properties);
		_normalScaleProperty 		= FindProperty("_BumpScale",			properties);
		_occlusionMapProperty 		= FindProperty("_OcclusionMap",			properties);
		_occlusionStrengthProperty 	= FindProperty("_OcclusionStrength",	properties);
		_emissionMapProperty 		= FindProperty("_EmissionMap",			properties);
		_emissionColorProperty 		= FindProperty("_EmissionColor",		properties);
	}

	/// <summary>
	/// Find the shader material.
	/// </summary>
	private void GetMaterial(MaterialEditor materialEditor)
	{
		_targetMat = materialEditor.target as Material;
		_targetMats.Clear();
		_targetMats = materialEditor.targets.Cast<Material>().ToList();
	}
	#endregion

	/* INSPECTOR DRAWING */
	#region Inspector Drawing
	public sealed override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
	{
		// Don't show if material inspector is folded in
		if (!materialEditor.isVisible)
		{
			return;
		}

		GetMaterial(materialEditor);

		DrawGUI(materialEditor, properties);
	}

	protected virtual void DrawGUI(MaterialEditor materialEditor, MaterialProperty[] properties) {}

	protected void DrawStandardPropertiesGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
	{
		// Get base information
		FindBaseProperties(properties);

		// Albedo map
		materialEditor.TexturePropertySingleLine(BaseContentText.baseAlbedoProperty, _albedoMapProperty, _albedoColorProperty);

		// Metallic and smoothness
		EditorGUI.BeginChangeCheck();
		{
			if (_metallicMapProperty.textureValue == null)
			{
				// Show sliders if no texture is specified
				materialEditor.TexturePropertyTwoLines(
					BaseContentText.baseMetallicMapProperty, 
					_metallicMapProperty, 
					_metallicProperty, 
					BaseContentText.baseSmoothnessProperty, 
					_smoothnessProperty);
			}
			else
			{
				// Only show texture if texture is specified
				materialEditor.TexturePropertySingleLine(BaseContentText.baseMetallicMapProperty, _metallicMapProperty);
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			// Setup keyword
			EnableKeyword("_METALLICGLOSSMAP", _metallicMapProperty.textureValue != null);
		}

		// Normals
		EditorGUI.BeginChangeCheck();
		{
			materialEditor.TexturePropertySingleLine(BaseContentText.normalMapProperty, _normalMapProperty, _normalMapProperty.textureValue != null ? _normalScaleProperty : null);
		}
		if (EditorGUI.EndChangeCheck())
		{
			// Setup keyword
			EnableKeyword("_NORMALMAP", _normalMapProperty.textureValue != null);
		}

		// Occlusion
		EditorGUI.BeginChangeCheck();
		{
			materialEditor.TexturePropertySingleLine(BaseContentText.occlusionMapProperty, _occlusionMapProperty, _occlusionMapProperty.textureValue != null ? _occlusionStrengthProperty : null);
		}
		if (EditorGUI.EndChangeCheck())
		{
			// Setup keyword
			EnableKeyword("_OCCLUSIONMAP", _occlusionMapProperty.textureValue != null);
		}

		// Emission
		EditorGUI.BeginChangeCheck();
		{
			float brightness = _emissionColorProperty.colorValue.maxColorComponent;
			bool showEmissionColorAndGIControls = brightness > 0.0f;
			bool hadEmissionTexture = (_emissionMapProperty.textureValue != null);

			// Texture and HDR color controls
			materialEditor.TexturePropertyWithHDRColor(BaseContentText.emissionMapProperty, _emissionMapProperty, _emissionColorProperty, _colorPickerHDRConfig, false);

			// If texture was assigned and color was black set color to white
			if (_emissionMapProperty.textureValue != null && !hadEmissionTexture && brightness <= 0f)
			{
				_emissionColorProperty.colorValue = Color.white;
			}

			// Dynamic Lightmapping mode
			if (showEmissionColorAndGIControls)
			{
				bool shouldEmissionBeEnabled = ShouldEmissionBeEnabled(_emissionColorProperty.colorValue);
				EditorGUI.BeginDisabledGroup(!shouldEmissionBeEnabled);

				materialEditor.LightmapEmissionProperty(MaterialEditor.kMiniTextureFieldLabelIndentLevel + 1);

				EditorGUI.EndDisabledGroup();
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			// Setup keyword
			EnableKeyword("_EMISSIVEMAP", _emissionMapProperty.textureValue != null);
		}

		// TILING
		EditorGUI.indentLevel = 1;
		EditorGUI.BeginChangeCheck();
		materialEditor.TextureScaleOffsetProperty(_albedoMapProperty);
		if (EditorGUI.EndChangeCheck())
		{
			_emissionMapProperty.textureScaleAndOffset = _albedoMapProperty.textureScaleAndOffset; // Apply the main texture scale and offset to the emission texture as well, for Enlighten's sake
		}
		EditorGUI.indentLevel = 0;

	}
	#endregion

	/* UTILITY */
	#region Utility

	private static bool ShouldEmissionBeEnabled (Color color)
	{
		return color.maxColorComponent > (0.1f / 255.0f);
	}

	/// <summary>
	/// Enables/Disable a shader keyword.
	/// </summary>
	/// <param name="keyword">Keyword.</param>
	/// <param name="enabled">If set to <c>true</c> enabled.</param>
	/// <param name="undoString">Optional string to show in the undo redo menu items.</param>
	protected void EnableKeyword(string keyword, bool enabled, string undoString = "")
	{
		string undoName = _targetMat.name;
		if (_targetMats.Count > 1)
		{
			undoName = "Multiple Materials";
		}
		if (undoString.Length == 0) 
		{
			undoString = keyword;
		}

		Undo.RecordObjects(_targetMats.ToArray(), string.Format("{0} of {1}", undoString, undoName));
		if (enabled)
		{
			_targetMats.ForEach(m => m.EnableKeyword(keyword));
		}
		else
		{
			_targetMats.ForEach(m => m.DisableKeyword(keyword));
		}
	}
	#endregion
}

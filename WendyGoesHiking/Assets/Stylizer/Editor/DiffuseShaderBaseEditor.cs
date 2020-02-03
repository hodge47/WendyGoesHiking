using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class DiffuseShaderBaseEditor : ShaderGUI 
{
	// UI Text
	private static class BaseContentText
	{
		public static GUIContent baseAlbedoProperty = new GUIContent("Base Color", "Color (RGB) and Transparency (A)");
		public static GUIContent normalMapProperty = new GUIContent("Normal Map", "Normal Map");
	}

	/* VARIABLES */
	#region Variables
	// Material
	protected Material _targetMat = null;
	protected List<Material> _targetMats = new List<Material>();

	// Material Properties
	protected MaterialProperty _colorMapProperty 				= null;
	protected MaterialProperty _colorProperty 					= null;
	protected MaterialProperty _normalMapProperty 				= null;
	protected MaterialProperty _normalScaleProperty 			= null;
	#endregion

	/* SETUP */
	#region Setup

	/// <summary>
	/// Find the shader properties.
	/// </summary>
	private void FindBaseProperties(MaterialProperty[] properties)
	{
		_colorProperty 				= FindProperty("_Color", 				properties);
		_colorMapProperty			= FindProperty("_MainTex",				properties);
		_normalMapProperty 			= FindProperty("_BumpMap",				properties);
		_normalScaleProperty 		= FindProperty("_BumpScale",			properties);
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

		// Color
		materialEditor.TexturePropertySingleLine(BaseContentText.baseAlbedoProperty, _colorMapProperty, _colorProperty);

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

		// TILING
		EditorGUI.indentLevel = 1;
		EditorGUI.BeginChangeCheck();
		materialEditor.TextureScaleOffsetProperty(_colorMapProperty);
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

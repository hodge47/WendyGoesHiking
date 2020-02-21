using UnityEditor;
using UnityEngine;

namespace Beffio.Dithering
{
	[CustomEditor(typeof(Stylizer))]
	[System.Serializable]
	public class StylizerEditor : Editor 
	{
		// UI Text
		private static class ContentText
		{
			public static GUIContent shaderProperty = new GUIContent("Shader", "Shader - Beffio/Dithering Image Effect");
			public static GUIContent paletteProperty = new GUIContent("Palette Asset", "Asset - The colors which can appear in the dithering effect, and how many colors can be mixed");
			public static GUIContent patternProperty = new GUIContent("Pattern Asset", "Asset - The pattern deciding which pixels are sampled from which palette square");
			public static GUIContent patternTextureProperty = new GUIContent("Pattern Texture", "Texture (R) - The pattern deciding which pixels are sampled from which palette square");
		
			public static string noTextureWarning = "{0}The selected {1} '{2}' did not generate the texture yet. Press the 'Generate Texture' button on the asset.\n";
			public static string oldTextureWarning = "{0}The selected {1} '{2}' has been adjusted, but the texture is not updated yet. Press the 'Generate Texture' button on the asset.\n";
		}

		/* VARIABLES */
		#region Variables
		private Stylizer _effect;
		public Stylizer Effect
		{
			get
			{
				if (_effect == null)
				{
					_effect = target as Stylizer;
				}
				return _effect;
			}
		}

		SerializedProperty _paletteProperty;
		SerializedProperty Pixelate;
		SerializedProperty Dither;
		SerializedProperty Grain;

		SerializedProperty Grain_Old;
		SerializedProperty Grain_New;
		SerializedProperty _patternProperty;
		SerializedProperty _patternTextureProperty;
		SerializedProperty _shaderProperty;
	
		//pixelization properties
		SerializedProperty _pixelationProperty;

		//grain properties

		SerializedObject serObj;

        SerializedProperty intensityMultiplier;
        SerializedProperty generalIntensity;
        SerializedProperty blackIntensity;
        SerializedProperty whiteIntensity;
        SerializedProperty midGrey;
        SerializedProperty softness;

		//new grain properties
		SerializedProperty g_Colored;
        SerializedProperty g_Intensity;
        SerializedProperty g_Size;
        SerializedProperty g_LuminanceContribution;
        SerializedProperty g_animated;


		#endregion

		/* EDITOR DRAWING */
		#region Editor Drawing

		void OnEnable(){

			serObj = new SerializedObject (target);
			Dither = serObj.FindProperty("Dither");
			Pixelate = serObj.FindProperty("Pixelate");
			Grain = serObj.FindProperty("Grain");
			Grain_Old = serObj.FindProperty("Grain_Old");
			Grain_New = serObj.FindProperty("Grain_New");
			g_Colored = serObj.FindProperty("colored");
			g_Intensity = serObj.FindProperty("intensity");
			g_Size = serObj.FindProperty("size");
			g_LuminanceContribution = serObj.FindProperty("luminanceContribution");
			g_animated = serObj.FindProperty("animated");

			_paletteProperty = serObj.FindProperty("_palette");
			_patternProperty = serObj.FindProperty("_pattern");
			_patternTextureProperty = serObj.FindProperty("_patternTexture");
			_shaderProperty = serObj.FindProperty("_shader");
			_pixelationProperty = serObj.FindProperty("_pixelScale");


			intensityMultiplier = serObj.FindProperty("intensityMultiplier");
            generalIntensity = serObj.FindProperty("generalIntensity");
            blackIntensity = serObj.FindProperty("blackIntensity");
            whiteIntensity = serObj.FindProperty("whiteIntensity");
            midGrey = serObj.FindProperty("midGrey");
			
            softness = serObj.FindProperty("softness");
  
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			
			// Branding
			BeffioDitherEditorUtilities.DrawInspectorBranding();
			GUILayout.Space(10);

			// Warning
			DrawGUIWarning();
			GUILayout.Space(10);

			// Fields
			DrawGUIFields();

			serializedObject.ApplyModifiedProperties();

			if (GUI.changed) EditorUtility.SetDirty(target);
		}

		private void DrawGUIFields()
		{

			EditorGUI.indentLevel = 0;
			EditorGUILayout.PropertyField(Dither, new GUIContent("Dithering"));
			EditorGUILayout.LabelField("Adds a dithering effect to the camera", EditorStyles.miniLabel);
			if(Dither.boolValue) {
				EditorGUI.indentLevel = 1;
				
				_effect.Dither = Dither.boolValue;
				EditorGUILayout.PropertyField(_shaderProperty, ContentText.shaderProperty);
				EditorGUILayout.PropertyField(_paletteProperty, ContentText.paletteProperty);
				if(Effect.PatternTexture == null) {
					EditorGUILayout.PropertyField(_patternProperty, ContentText.patternProperty);
				}
				if(Effect.Pattern == null) {
					EditorGUILayout.PropertyField(_patternTextureProperty, ContentText.patternTextureProperty);
				}
			}else{
				_effect.Dither = Dither.boolValue;
			}

			EditorGUI.indentLevel = 0;
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField("————————————————————————————————————————————————————————————————————————————————————————————————————————————————————",EditorStyles.miniLabel);
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			
			EditorGUILayout.PropertyField(Pixelate, new GUIContent("Pixelation"));
			EditorGUILayout.LabelField("Pixelates the image", EditorStyles.miniLabel);
			if(Pixelate.boolValue) {
				if(_effect.enabled) {
					_effect.EnablePixel();
					_effect.Pixelate = Pixelate.boolValue;
				}
				EditorGUI.indentLevel = 1;
				
				EditorGUILayout.Slider(_pixelationProperty, 0.01f, 1f);
			} else {
				_effect.Pixelate = Pixelate.boolValue;
				_effect.DisablePixel();
			}

			EditorGUI.indentLevel = 0;
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField("————————————————————————————————————————————————————————————————————————————————————————————————————————————————————",EditorStyles.miniLabel);
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			
			EditorGUILayout.PropertyField(Grain, new GUIContent("Grain"));
			EditorGUILayout.LabelField("Overlays noise patterns", EditorStyles.miniLabel);
			if(Grain.boolValue) {
				EditorGUI.indentLevel = 1;
				_effect.Grain = Grain.boolValue;
				
				EditorGUILayout.PropertyField(Grain_New, new GUIContent("Grain method #1"));
				if(Grain_New.boolValue){
					EditorGUI.indentLevel = 2;
					_effect.Grain_New = Grain_New.boolValue;

					//EditorGUILayout.PropertyField(profile);

					EditorGUILayout.PropertyField(g_Intensity);
           			EditorGUILayout.PropertyField(g_LuminanceContribution);
            		EditorGUILayout.PropertyField(g_Size);
            		EditorGUILayout.PropertyField(g_Colored);
           			EditorGUILayout.PropertyField(g_animated);

					_effect.intensity = g_Intensity.floatValue;
					_effect.luminanceContribution = g_LuminanceContribution.floatValue;
					_effect.size = g_Size.floatValue;
					_effect.colored = g_Colored.boolValue;
					_effect.animated = g_animated.boolValue;

				}else{
					_effect.Grain_New = Grain_New.boolValue;
				}
				EditorGUI.indentLevel = 1;

				EditorGUILayout.PropertyField(Grain_Old, new GUIContent("Grain method #2"));
				if(Grain_Old.boolValue){
					EditorGUI.indentLevel = 2;
					_effect.Grain_Old = Grain_Old.boolValue;

					EditorGUILayout.PropertyField(intensityMultiplier, new GUIContent("Intensity Multiplier"));
					EditorGUILayout.PropertyField(generalIntensity, new GUIContent(" General"));
					EditorGUILayout.PropertyField(blackIntensity, new GUIContent(" Black Boost"));
					EditorGUILayout.PropertyField(whiteIntensity, new GUIContent(" White Boost"));
					midGrey.floatValue = EditorGUILayout.Slider( new GUIContent(" Mid Grey (for Boost)"), midGrey.floatValue, 0.0f, 1.0f);

					EditorGUILayout.LabelField("Noise Shape");

					softness.floatValue = EditorGUILayout.Slider( new GUIContent(" Softness"),softness.floatValue, 0.0001f, 0.99f);

				}else{
					_effect.Grain_Old = Grain_Old.boolValue;
				}
				EditorGUI.indentLevel = 1;

				//EditorGUILayout.Separator();
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				

			}else{
				_effect.Grain = Grain.boolValue;
			}

			serObj.ApplyModifiedProperties();
				
		}

		private void DrawGUIWarning()
		{
			string warningMessage = "";
			if (Effect.Palette != null)
			{
				if (!Effect.Palette.HasTexture)
				{
					warningMessage = string.Format(ContentText.noTextureWarning, warningMessage, typeof(Palette).Name, Effect.Palette.name);
				}
				if (Effect.Palette.IsDirty)
				{
					warningMessage = string.Format(ContentText.oldTextureWarning, warningMessage, typeof(Palette).Name, Effect.Palette.name);
				}
			}

			if (Effect.Pattern != null)
			{
				if (!Effect.Pattern.HasTexture)
				{
					warningMessage = string.Format(ContentText.noTextureWarning, warningMessage, typeof(Pattern).Name, Effect.Pattern.name);
				}
				if (Effect.Pattern.IsDirty)
				{
					warningMessage = string.Format(ContentText.oldTextureWarning, warningMessage, typeof(Pattern).Name, Effect.Pattern.name);
				}
			}

			if (warningMessage != "")
			{
				warningMessage = warningMessage.TrimEnd('\n');
				EditorGUILayout.HelpBox(warningMessage, MessageType.Warning);
			}
		}
		#endregion

	}
}
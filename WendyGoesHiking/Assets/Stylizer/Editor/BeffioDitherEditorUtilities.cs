using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Beffio.Dithering
{
	public static class BeffioDitherEditorUtilities
	{
		/* BRANDING */
		#region Branding
		private static string _brandingLogoPath = "Assets/Stylizer/Editor/StylizerLogo.png";

		/// <summary>
		/// Draws inspector branding centered within the panel.
		/// </summary>
		public static void DrawInspectorBranding()
		{
			// Get texture and its aspect ratio
			Texture tex = AssetDatabase.LoadAssetAtPath<Texture>(_brandingLogoPath);
			if (tex == null)
			{
				Debug.LogWarningFormat("No branding logo texture found at {0}, please make sure a texture is available at that path, or change the path in the variable _brandingLogoPath in this script.", _brandingLogoPath);
				return;
			}

			float imageAspect = tex.height/tex.width;

			// Make GUIContent from texture
			GUIContent content = new GUIContent(tex);

			// Calculate width and height clamped to the width of the inspector
			float scrollBarWidth = 40;
			float width = EditorGUIUtility.currentViewWidth - scrollBarWidth; // See for scrollbar: http://forum.unity3d.com/threads/editorguilayout-get-width-of-inspector-window-area.82068/
			float height = width * imageAspect;

			// Save background color
			Color oldCol = GUI.backgroundColor;
			// Set transparent background color
			GUI.backgroundColor = new Color(0,0,0,0);

			// Draw Header branding
			GUILayout.Box(content, GUILayout.Width(width), GUILayout.Height(height));

			// Reset old color
			GUI.backgroundColor = oldCol;
		}
		#endregion

		/* LOADING AND SAVING */
		#region Loading and Saving

		/// <summary>
		///  Opens a file dialog and loads a .png image to a Texture2D.
		/// </summary>
		public static Texture2D LoadTexture(string path) 
		{
			if (path.Length == 0)
			{
				return null;
			}

			Texture2D texture = new Texture2D(4, 4, TextureFormat.ARGB32, false);
			texture.name = Path.GetFileNameWithoutExtension(path);

			new WWW("file://" + path).LoadImageIntoTexture(texture);

			return texture;
		}

		public static void SaveTexture(Texture2D texture, string absoluteFilePath) 
		{
			if (absoluteFilePath.Length == 0)
			{
				return;
			}

			byte[] bytes = texture.EncodeToPNG();
			File.WriteAllBytes(absoluteFilePath, bytes);

			string fileName = Path.GetFileName(absoluteFilePath);
			string relativeFilePath = string.Format("Assets{0}", absoluteFilePath.Replace(Application.dataPath, ""));
			string relativeFolderPath = relativeFilePath.Replace(string.Format("/{0}", fileName), "");

			if (AssetDatabase.IsValidFolder(relativeFolderPath))
			{
				ImportAndReloadTexture(relativeFilePath);
			}
		}

		public static void ImportAndReloadTexture(string assetPath)
		{
			// Set correct import settings
			TextureImporter textureImport = TextureImporter.GetAtPath(assetPath) as TextureImporter;
			textureImport.mipmapEnabled = false;
			textureImport.wrapMode = TextureWrapMode.Repeat;
			textureImport.filterMode = FilterMode.Point;
			#if UNITY_5_5_OR_NEWER
			textureImport.textureType = TextureImporterType.Default;
			textureImport.textureCompression = TextureImporterCompression.Uncompressed;
			#else
			textureImport.textureType = TextureImporterType.Advanced;
			textureImport.textureFormat = TextureImporterFormat.RGB24;
			#endif

			// Import asset
			AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
		}
		#endregion

		/* MATERIAL UPDATING */
		#region Material Updating
		/// <summary>
		/// Updates the Palette and Pattern on materials which reference a given texture.
		/// </summary>
		/// <param name="texture">Texture to look for.</param>
		public static void UpdateMaterialsForTexture(Texture2D texture)
		{
			int textureId = texture.GetInstanceID();

			Material[] materials = Resources.FindObjectsOfTypeAll<Material>();
			foreach (Material mat in materials)
			{
				UnityEngine.Object[] m = new UnityEngine.Object[1];
				m[0] = mat as UnityEngine.Object;
				UnityEngine.Object[] dependencies = EditorUtility.CollectDependencies(m);
				foreach (UnityEngine.Object o in dependencies)
				{
					if (o.GetInstanceID() == textureId)
					{
						UpdateMaterialAssets(mat);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Updates the Palette and Pattern on a certain material.
		/// </summary>
		/// <param name="mat">Material to update.</param>
		public static void UpdateMaterialAssets(Material mat)
		{
			if (!mat.HasProperty("__paletteRef") || !mat.HasProperty("__patternRef"))
			{
				return;
			}

			Palette palette = GetAssetFromVector(mat.GetVector("__paletteRef")) as Palette;
			Pattern pattern = GetAssetFromVector(mat.GetVector("__patternRef")) as Pattern;

			if (palette != null && palette.Texture != null)
			{
				mat.SetFloat("_PaletteColorCount", palette.MixedColorCount);
				mat.SetFloat("_PaletteHeight", palette.Texture.height);
				mat.SetTexture("_PaletteTex", palette.Texture);
			}
			else
			{
				mat.SetFloat("_PaletteColorCount", 1);
				mat.SetFloat("_PaletteHeight", Texture2D.whiteTexture.height);
				mat.SetTexture("_PaletteTex", Texture2D.whiteTexture);
			}

			if (pattern != null && pattern.Texture != null)
			{
				mat.SetFloat("_PatternSize", pattern.Texture.height);
				mat.SetTexture("_PatternTex", pattern.Texture);
			}
			else
			{
				mat.SetFloat("_PatternSize", Texture2D.whiteTexture.height);
				mat.SetTexture("_PatternTex", Texture2D.whiteTexture);
			}
		}

		private static int FloatBitsToInt(float f)
		{
			return System.BitConverter.ToInt32(System.BitConverter.GetBytes(f), 0);
		}
		private static float IntBitsToFloat(int i)
		{
			return System.BitConverter.ToSingle(System.BitConverter.GetBytes(i), 0);
		}

		/// <summary>
		/// Gets the asset from a GUID stored in a vector.
		/// </summary>
		/// <returns>The asset.</returns>
		/// <param name="guidVector">Vector in which a GUID is stored.</param>
		public static UnityEngine.Object GetAssetFromVector(Vector4 guidVector)
		{
			List<float> guidFloats = new List<float>(4);
			guidFloats.Add(guidVector.x);
			guidFloats.Add(guidVector.y);
			guidFloats.Add(guidVector.z);
			guidFloats.Add(guidVector.w);

			List<string> guidStrings = guidFloats.ConvertAll(f => FloatBitsToInt(f).ToString("x8"));

			string guid = string.Format("{0}{1}{2}{3}", guidStrings[0], guidStrings[1], guidStrings[2], guidStrings[3]);

			if (guid != "")
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);
				return AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
			}

			return null;
		}

		/// <summary>
		/// Store an asset GUID into a vector.
		/// </summary>
		/// <returns>A GUID stored in a vector.</returns>
		/// <param name="obj">Object to store.</param>
		public static Vector4 AssetToVector4GUID(UnityEngine.Object obj)
		{
			string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj));
			int chunkSize = 8;
			List<float> guidInts = Enumerable.Range(0, guid.Length / chunkSize)
				.Select(s => IntBitsToFloat(System.Int32.Parse(guid.Substring(s * chunkSize, chunkSize), System.Globalization.NumberStyles.HexNumber))).ToList();
			Vector4 output = Vector4.zero;
			output.x = guidInts[0];
			output.y = guidInts[1];
			output.z = guidInts[2];
			output.w = guidInts[3];
			return output;
		}
		#endregion
	}
}

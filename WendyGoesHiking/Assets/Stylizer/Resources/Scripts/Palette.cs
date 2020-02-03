using UnityEngine;
using System;
using System.Collections.Generic;

namespace Beffio.Dithering
{
	[Serializable]
	[CreateAssetMenu(menuName = "Beffio/Dithering Palette")]
	public class Palette : ScriptableObject
	{
		private static Color[] _defaultColors = { new Color(0f,0f,0f), new Color(0.25f,0.25f,0.25f), new Color(0.5f,0.5f,0.5f), new Color(0.75f,0.75f,0.75f), new Color(1f,1f,1f)  };

		[Header("Palette Settings")]
		public int MixedColorCount = 2;
		public List<Color> Colors = new List<Color>(_defaultColors);

		[Header("Texture Settings")]
		public Texture2D Texture;
		public bool HasTexture = false;

		public bool IsDirty = false;
	}
}
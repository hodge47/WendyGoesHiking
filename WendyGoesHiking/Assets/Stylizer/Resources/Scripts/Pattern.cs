using UnityEngine;
using System.Collections;
using System;

namespace Beffio.Dithering
{
	public enum PatternType
	{
		Noise,
		Dots,
		Lines
	}

	public enum LineDirection
	{
		Vertical,
		Horizontal,
		Slope45,
		Slope135
	}

	[Serializable]
	[CreateAssetMenu(menuName = "Beffio/Dithering Pattern")]
	public class Pattern : ScriptableObject 
	{
		[Header("Pattern Settings")]
		public PatternType Type = PatternType.Noise;
		public float MinimumValue = 0.0f;
		public float MaximumValue = 1.0f;

		public float ColorVariance = 0.1f;
		public float ElementSize = 1.0f;

		public LineDirection Direction = LineDirection.Horizontal;

		[Header("Texture Settings")]
		public int TextureSize = 8;
		public Texture2D Texture;
		public bool HasTexture = false;

		public bool IsDirty = false;
	}
}

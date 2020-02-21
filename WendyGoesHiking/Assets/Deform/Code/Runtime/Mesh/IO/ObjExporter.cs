﻿/*
 * Credit to KeliHlodversson for writing the original code and posting it to the unify community.
 * http://wiki.unity3d.com/index.php/ObjExporter
 * I added support for skinned mesh renderers and I reformatted/styled the code to my personal preference.
 */

using System.IO;
using System.Text;
using UnityEngine;

namespace Deform
{
	/// <summary>
	/// Handles saving meshes to obj files.
	/// </summary>
	public class ObjExporter
	{
		/// <summary>
		/// Saves mesh as obj.
		/// </summary>
		public static void SaveMesh (Mesh mesh, Renderer renderer, string fullFolderPath, string name)
		{
			var path = $"{Path.Combine (Application.dataPath, fullFolderPath)}{name}.obj";
			MeshToFile (mesh, renderer, path, name);
		}

		/// <summary>
		/// Writes mesh file to disk.
		/// </summary>
		private static void MeshToFile (Mesh mesh, Renderer renderer, string path, string name)
		{
			using (StreamWriter sw = new StreamWriter (path))
				sw.Write (MeshToString (mesh, renderer, name));
		}

		/// <summary>
		/// Converts mesh to obj string.
		/// </summary>
		private static string MeshToString (Mesh mesh, Renderer renderer, string name)
		{
			if (renderer == null)
				throw new System.NullReferenceException("Renderer cannot be null to convert mesh to string.");

			var materials = renderer.sharedMaterials;

			var stringBuilder = new StringBuilder ();
			var culture = System.Globalization.CultureInfo.InvariantCulture;

			stringBuilder.Append ("g ").Append (name).Append ("\n");
			foreach (var vertice in mesh.vertices)
				stringBuilder.AppendFormat (culture, "v {0} {1} {2}\n", -vertice.x, vertice.y, vertice.z);

			stringBuilder.Append ("\n");
			foreach (var normal in mesh.normals)
				stringBuilder.AppendFormat (culture, "vn {0} {1} {2}\n", -normal.x, normal.y, normal.z);

			stringBuilder.Append ("\n");
			foreach (var uv in mesh.uv)
				stringBuilder.AppendFormat (culture, "vt {0} {1}\n", uv.x, uv.y);

			for (int material = 0; material < mesh.subMeshCount; material++)
			{
				stringBuilder.Append ("\n");
				var materialName = (materials != null && material < materials.Length) ? materials[material].name : "Material";
				stringBuilder.Append ("usemtl ").Append (materialName).Append ("\n");
				stringBuilder.Append ("usemap ").Append (materialName).Append ("\n");

				var triangles = mesh.GetTriangles (material);
				for (int i = 0; i < triangles.Length; i += 3)
				{
					stringBuilder.AppendFormat
					(
						culture,
						"f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
						triangles[i + 2] + 1,
						triangles[i + 1] + 1,
						triangles[i] + 1
					);
				}
			}

			return stringBuilder.ToString ();
		}
	}
}
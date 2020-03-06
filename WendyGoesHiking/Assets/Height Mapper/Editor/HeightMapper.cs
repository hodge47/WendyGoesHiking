using UnityEngine;
using UnityEditor;

namespace GameSurgeon.HeightMapper
{
    [ExecuteInEditMode]
    public class HeightMapper : EditorWindow
    {
        private Rect dropRect;

        private Terrain terrain;
        private Texture2D heightMap;

        [MenuItem("Terrain/Height Mapper")]
        public static void ShowWindow()
        {
            GetWindow(typeof(HeightMapper));
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical("box");

            terrain = (Terrain)EditorGUILayout.ObjectField("Terrain Object", terrain, typeof(Terrain), true);
            heightMap = (Texture2D)EditorGUILayout.ObjectField("Height Map Texture", heightMap, typeof(Texture2D), false);

            if (terrain == null || heightMap == null)
                EditorGUILayout.HelpBox("You need both a terrain object and height map texutre set before you can map the two.", MessageType.Warning);
            else if (GUILayout.Button("Map"))
                HeightMapFromTexture.ApplyHeightmap(heightMap, terrain.terrainData);

            EditorGUILayout.EndVertical();
        }
    }
}

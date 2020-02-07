using System;
using System.Collections.Generic;
using UnityEngine;

namespace Beffio.Dithering
{
    using UnityObject = System.Object;

    public sealed class MF : IDisposable
    {
        Dictionary<string, Material> m_Materials;

        public MF()
        {
            m_Materials = new Dictionary<string, Material>();
        }

        public Material Get(string shaderName)
        {
            Material material;

            if (!m_Materials.TryGetValue(shaderName, out material))
            {
                var shader = Shader.Find(shaderName);

                if (shader == null)
                    throw new ArgumentException(string.Format("Shader not found ({0})", shaderName));

                material = new Material(shader)
                {
                    name = string.Format("PostFX - {0}", shaderName.Substring(shaderName.LastIndexOf("/") + 1)),
                    hideFlags = HideFlags.DontSave
                };

                m_Materials.Add(shaderName, material);
            }

            return material;
        }

        public void Dispose()
        {
            var enumerator = m_Materials.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var material = enumerator.Current.Value;
                GU.Destroy(material);
            }

            m_Materials.Clear();
        }
    }
}

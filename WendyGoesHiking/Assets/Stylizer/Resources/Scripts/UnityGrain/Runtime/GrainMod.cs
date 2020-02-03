using System;
using UnityEngine;
namespace Beffio.Dithering
{
    [Serializable]
    public class GrainMod : PostMod
    {
        [Serializable]
        public struct Settings
        {
            [Tooltip("Enable the use of colored grain.")]
            public bool colored;

            [Range(0f, 1f), Tooltip("Grain strength. Higher means more visible grain.")]
            public float intensity;

            [Range(0.3f, 3f), Tooltip("Grain particle size.")]
            public float size;

            [Range(0f, 1f), Tooltip("Controls the noisiness response curve based on scene luminance. Lower values mean less noise in dark areas.")]
            public float luminanceContribution;

             [Tooltip("Is the grain static or animated.")]
            public bool animated;

            public static Settings defaultSettings
            {
                get
                {
                    return new Settings
                    {
                        colored = true,
                        intensity = 0.5f,
                        size = 1f,
                        luminanceContribution = 0.8f,
                        animated = true
                    };
                }
            }

            //CONSTRUCTOR
            public Settings(bool colored, float intensity, float size, float luminanceContribution, bool animated) 
            {
                this.colored = colored;
                this.intensity = intensity;
                this.size = size;
                this.luminanceContribution = luminanceContribution;
                this.animated = animated;
            }
        }

        [SerializeField]
        Settings m_Settings = Settings.defaultSettings;
        public Settings settings
        {
            get { return m_Settings; }
            set { m_Settings = value; }
        }

        public override void Reset()
        {
            m_Settings = Settings.defaultSettings;
        }
    }
}

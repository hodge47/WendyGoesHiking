using System;
using UnityEngine;
namespace Beffio.Dithering
{
    [Serializable]
    public abstract class PostMod
    {
        [SerializeField]
        bool m_Enabled;
        public bool enabled
        {
            get { return m_Enabled; }
            set
            {
                m_Enabled = value;

                if (value)
                    OnValidate();
            }
        }

        public abstract void Reset();

        public virtual void OnValidate()
        {}
    }
}

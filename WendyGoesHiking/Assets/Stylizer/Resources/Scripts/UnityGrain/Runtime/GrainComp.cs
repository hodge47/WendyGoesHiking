using UnityEngine;
namespace Beffio.Dithering
{
    public sealed class GrainComp : PostProcessingComponentRenderTexture<GrainMod>
    {
        static class Uniforms
        {
            internal static readonly int _Grain_Params1 = Shader.PropertyToID("_Grain_Params1");
            internal static readonly int _Grain_Params2 = Shader.PropertyToID("_Grain_Params2");
            internal static readonly int _GrainTex      = Shader.PropertyToID("_GrainTex");
            internal static readonly int _Phase         = Shader.PropertyToID("_Phase");
        }

        public override bool active
        {
            get
            {
                return model.enabled
                       && model.settings.intensity > 0f
                       && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf)
                       && !context.interrupted;
            }
        }

        RenderTexture m_GrainLookupRT;

        public override void OnDisable()
        {
            GU.Destroy(m_GrainLookupRT);
            m_GrainLookupRT = null;
        }

        public override void Prepare(Material uberMaterial, Material grainMaterial)
        {
            var settings = model.settings;

            uberMaterial.EnableKeyword("GRAIN");

            float rndOffsetX;
            float rndOffsetY;
            float time;

            if(!model.settings.animated){
                time = 4f;
                rndOffsetX = 0f;
                rndOffsetY = 0f;
            }else{
                time = Random.Range(0f,1f);
                rndOffsetX = Random.Range(0f,1f);
                rndOffsetY = Random.Range(0f,1f);
            }


            // Generate the grain lut for the current frame first
            if (m_GrainLookupRT == null || !m_GrainLookupRT.IsCreated())
            {
                GU.Destroy(m_GrainLookupRT);

                m_GrainLookupRT = new RenderTexture(192, 192, 0, RenderTextureFormat.ARGBHalf)
                {
                    filterMode = FilterMode.Bilinear,
                    wrapMode = TextureWrapMode.Repeat,
                    anisoLevel = 0,
                    name = "Grain Lookup Texture"
                };

                m_GrainLookupRT.Create();
            }

            //var grainMaterial = context.materialFactory.Get("Hidden/Post FX/Grain Generator");
            grainMaterial.SetFloat(Uniforms._Phase, time / 20f);

            Graphics.Blit((Texture)null, m_GrainLookupRT, grainMaterial, settings.colored ? 1 : 0);

            // Send everything to the uber shader
            uberMaterial.SetTexture(Uniforms._GrainTex, m_GrainLookupRT);
            uberMaterial.SetVector(Uniforms._Grain_Params1, new Vector2(settings.luminanceContribution, settings.intensity * 20f));
            uberMaterial.SetVector(Uniforms._Grain_Params2, new Vector4((float)context.width / (float)m_GrainLookupRT.width / settings.size, (float)context.height / (float)m_GrainLookupRT.height / settings.size, rndOffsetX, rndOffsetY));
        }
    }
}

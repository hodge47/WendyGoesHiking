using UnityEngine.Rendering;
using UnityEngine;

namespace Beffio.Dithering
{
    public abstract class PostComp
    {
        public PostCont context;

        public virtual DepthTextureMode GetCameraFlags()
        {
            return DepthTextureMode.None;
        }

        public abstract bool active { get; }

        public virtual void OnEnable()
        {}

        public virtual void OnDisable()
        {}

        public abstract PostMod GetModel();
    }

    public abstract class PostProcessingComponent<T> : PostComp
        where T : PostMod
    {
        public T model { get; internal set; }

        public virtual void Init(PostCont pcontext, T pmodel)
        {
            context = pcontext;
            model = pmodel;
        }

        public override PostMod GetModel()
        {
            return model;
        }
    }

    public abstract class PostProcessingComponentCommandBuffer<T> : PostProcessingComponent<T>
        where T : PostMod
    {
        public abstract CameraEvent GetCameraEvent();

        public abstract string GetName();

        public abstract void PopulateCommandBuffer(CommandBuffer cb);
    }

    public abstract class PostProcessingComponentRenderTexture<T> : PostProcessingComponent<T>
        where T : PostMod
    {
        public virtual void Prepare(Material material, Material grainMaterial)
        {}
    }
}

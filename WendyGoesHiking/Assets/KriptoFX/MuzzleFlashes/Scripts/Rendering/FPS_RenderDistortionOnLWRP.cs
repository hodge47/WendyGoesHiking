using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class FPS_RenderDistortionOnLWRP: MonoBehaviour
{
    void OnEnable()
    {
        LWRP_Rendering();
    }

    void Update()
    {
        LWRP_Rendering();
    }

    void LWRP_Rendering()
    {
#if KRIPTO_FX_LWRP_RENDERING
        var cam = Camera.main;
        var mobileLwrpDistortion = cam.GetComponent<FPS_LWRP_RenderDistortion>();
        if (mobileLwrpDistortion == null) mobileLwrpDistortion = cam.gameObject.AddComponent<FPS_LWRP_RenderDistortion>();
        mobileLwrpDistortion.IsActive = true;
#endif
    }

    void OnDisable()
    {
        var cam = Camera.main;
        if (cam == null) return;

#if KRIPTO_FX_LWRP_RENDERING
        var mobileLwrpDistortion = cam.GetComponent<FPS_LWRP_RenderDistortion>();
        if (mobileLwrpDistortion != null) mobileLwrpDistortion.IsActive = false;
#endif
    }
    
}

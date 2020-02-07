using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Pixelation : MonoBehaviour {

	[Range(1, 550)]
	public int pixelScale = 150;
	int pixelWidth;

	Camera cam = null;
	Camera ortoCamera = null;
	RenderTexture renTex =  null;
	GameObject pixelator = null;
	GameObject quad = null;

	int previousWidth = -1;
	int previousHeight = -1;
	int previousPixelation = -1;

	void Start(){

		Debug.Log("Yup");

		if(GameObject.Find("Pixelator")) {
			DestroyImmediate(GameObject.Find("Pixelator"));
		}

		if(GetComponent<Camera>() != null) {
			cam = GetComponent<Camera>();
		}
		if(pixelator == null || pixelator.GetComponent<Camera>()==null || quad==null || ortoCamera==null) {
			SetUpPixelator();
		}
			
	}

	void OnValidate(){
		if(previousPixelation != pixelScale) {
			previousPixelation = pixelScale;
			UpdateTex();
		}
	}

	void OnDisable(){
		DestroyImmediate(pixelator);
		if (cam != null) {
			cam.targetTexture = null;
		}
	}

	void OnDestroy(){
		DestroyImmediate(pixelator);
		if (cam != null) {
			cam.targetTexture = null;
		}
	}

	void Update(){
 
		if(pixelator == null || pixelator.GetComponent<Camera>()==null || quad==null || ortoCamera==null) {
			SetUpPixelator();
		}
			
		if(cam == null) {
			if(!GetComponent<Camera>()) {
				Debug.LogWarning("No camera detected.");
				return;
			} else {
				cam = GetComponent<Camera>();
			}
		}
			
		if(renTex == null) {

			float actualAspect = ortoCamera.aspect * ortoCamera.rect.height / ortoCamera.rect.width;
			pixelWidth = Mathf.RoundToInt(pixelScale * actualAspect);

			renTex = new RenderTexture(pixelWidth, pixelScale, 24, RenderTextureFormat.Default);
			renTex.filterMode = FilterMode.Point;
			renTex.name = "Pixel render texture";
			renTex.Create();
			cam.targetTexture = renTex;
		}
			
		if(ortoCamera.pixelWidth != previousWidth || ortoCamera.pixelHeight != previousHeight) {
			ResizeQuad();
			previousWidth = cam.pixelWidth;
			previousHeight = cam.pixelHeight;
		}

		if(quad.GetComponent<Renderer>().sharedMaterial.mainTexture != renTex) {
			quad.GetComponent<Renderer>().sharedMaterial.mainTexture = renTex;
		}

		if(previousPixelation != pixelScale) {
			previousPixelation = pixelScale;
			UpdateTex();
		}

	}

	void UpdateTex(){
		if(ortoCamera != null) {
			float actualAspect = ortoCamera.aspect * ortoCamera.rect.height / ortoCamera.rect.width;
			pixelWidth = Mathf.RoundToInt(pixelScale * actualAspect);

			renTex = new RenderTexture(pixelWidth, pixelScale, 24, RenderTextureFormat.Default);
			renTex.filterMode = FilterMode.Point;
			renTex.name = "Pixel render texture";
			renTex.Create();
			cam.targetTexture = renTex;
		}
	}

	void SetUpPixelator(){
		if(pixelator == null) {
			pixelator = new GameObject("Pixelator");
		}
		pixelator.transform.position = new Vector3(0, 0, -9999);
		if(pixelator.GetComponent<Camera>() == null) {
			pixelator.AddComponent<Camera>();
		}

		ortoCamera = pixelator.GetComponent<Camera>();

		ortoCamera.orthographic = true;
		ortoCamera.orthographicSize = 0.5f;
		ortoCamera.farClipPlane = 2.0f;
		ortoCamera.depth = cam.depth;
		ortoCamera.rect = cam.rect;
		ortoCamera.clearFlags = CameraClearFlags.Nothing;
		ortoCamera.useOcclusionCulling = false;

		if(quad == null) {
			quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
			quad.name = "Quad";
			DestroyImmediate(quad.GetComponent<Collider>());
			quad.transform.parent = pixelator.transform;
			quad.transform.localPosition = 1.0f * pixelator.transform.forward;

			ResizeQuad();

			Material mat = new Material(Shader.Find("Unlit/Texture"));
			mat.mainTexture = renTex;

			quad.GetComponent<Renderer>().sharedMaterial = mat;
		}

	}

	void ResizeQuad(){
		ortoCamera.rect = cam.rect;
		if(ortoCamera.pixelHeight > 0) {
			quad.transform.localScale = new Vector3(
				(float)ortoCamera.pixelWidth / ortoCamera.pixelHeight,
				1f, 1f);
		} else {
			quad.transform.localScale = Vector3.one;
		}

		UpdateTex();

	}

}

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

[InitializeOnLoad]
public class NL_PhysicsPlacer : MonoBehaviour {
	private Camera Cam;
	private Transform SpawnPoint;
	private RaycastHit hit;
	private float xRot;
	private float yRot;
	private float boost = 1;
	private Vector3 pos;
	private Vector3 rot;
	private float spawnOffcetY = 1;
	private bool isSpawnPoint = false;
	private Transform pointerArrow;
	private GameObject createdObj;
	private List<GameObject> Objects = new List<GameObject>();
	private List<string> droppedObjectsPaths = new List<string> ();
	private List<GameObject> _ObjectsList = new List<GameObject>();
	private List<SavedObject> _objects = new List<SavedObject>(); 
	private List<GameObject> SelectedObjects = new List<GameObject>();
	public static Transform parentObj;
	private Canvas canvas;
	private Text headerText;
	private Text hintText;
	private Text showCursorText;
	private Vector3 lastPos;
	private Texture2D cursorEmpty;
	private Texture2D cursorDrag;
	private Light pointLight;

	void Start () {
		

		parentObj = null;

		if (EditorPrefs.HasKey ("PhysicsEditMode")) {
			if (EditorPrefs.GetBool ("PhysicsEditMode") != true) {
				this.enabled = false;
			} else {

				cursorEmpty = (Texture2D)AssetDatabase.LoadAssetAtPath (EditorPrefs.GetString("RelativePath") + "CursorEmpty.png", typeof(Texture2D));
				cursorDrag = (Texture2D)AssetDatabase.LoadAssetAtPath (EditorPrefs.GetString("RelativePath") + "CursorDrag.png", typeof(Texture2D));
				Physics.queriesHitTriggers = false;

				Undo.undoRedoPerformed += UndoCallback;

				if(EditorPrefs.HasKey("initCamPosX")){
					transform.position = new Vector3 (EditorPrefs.GetFloat("initCamPosX"), EditorPrefs.GetFloat("initCamPosY"), EditorPrefs.GetFloat("initCamPosZ"));
					transform.eulerAngles = new Vector3 (EditorPrefs.GetFloat("initCamRotX"), EditorPrefs.GetFloat("initCamRotY"), EditorPrefs.GetFloat("initCamRotZ"));
					yRot = transform.eulerAngles.y;
					xRot = transform.eulerAngles.x;
				}

				float timestep = Time.fixedDeltaTime;

				if(EditorPrefs.HasKey("MorePreciseSimulation")){
					if (EditorPrefs.GetBool ("MorePreciseSimulation") == true) {
						Time.fixedDeltaTime = 0.001f;
					} else {
						Time.fixedDeltaTime = timestep;
					}
				}

				//create new Canvas
				canvas = new GameObject ("PhysicsPlacerInterface").AddComponent<Canvas> ();
				canvas.gameObject.AddComponent<CanvasScaler> ();
				canvas.renderMode = RenderMode.ScreenSpaceOverlay;
				canvas.sortingOrder = 100;
				canvas.transform.SetAsFirstSibling ();

				Font arial = (Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");

			

				//create Header Text
				GameObject ShowCursorTextObject = new GameObject ("ShowCursorText");
				ShowCursorTextObject.transform.SetParent (canvas.transform);
				showCursorText = ShowCursorTextObject.AddComponent<Text> ();
				showCursorText.gameObject.AddComponent<Outline> ().effectColor = new Color (0,0,0,1);
				showCursorText.font = arial;
				showCursorText.alignment = TextAnchor.MiddleCenter;
				showCursorText.fontSize = 24;
				showCursorText.fontStyle = FontStyle.Bold;
				showCursorText.color = Color.white;

				showCursorText.rectTransform.anchorMax = new Vector2 (0.5f, 0.5f);
				showCursorText.rectTransform.anchorMin = new Vector2 (0.5f, 0.5f);

				showCursorText.rectTransform.sizeDelta = new Vector3 (700, 64);
				showCursorText.rectTransform.anchoredPosition = new Vector2 (0, 0);
				showCursorText.text = "Some of your scripts hide cursor. \nPress ESC to show it.";
				showCursorText.gameObject.SetActive (false);

				//create Header Text
				GameObject HeaderTextObj = new GameObject ("HeaderText");
				HeaderTextObj.transform.SetParent (canvas.transform);
				headerText = HeaderTextObj.AddComponent<Text> ();
				headerText.gameObject.AddComponent<Outline> ().effectColor = new Color (0,0,0,1);
				headerText.font = arial;
				headerText.alignment = TextAnchor.MiddleCenter;
				headerText.fontSize = 24;
				headerText.fontStyle = FontStyle.Bold;
				headerText.color = Color.white;

				headerText.rectTransform.anchorMax = new Vector2 (0.5f, 1);
				headerText.rectTransform.anchorMin = new Vector2 (0.5f, 1);

				headerText.rectTransform.sizeDelta = new Vector3 (700, 64);
				headerText.rectTransform.anchoredPosition = new Vector2 (0, -32);
				headerText.text = "You are in Physics Placement Mode";

				//create Hint Text
				GameObject HintTextObj = new GameObject("HintText");
				HintTextObj.transform.SetParent (canvas.transform);
				hintText = HintTextObj.AddComponent<Text> ();
				hintText.gameObject.AddComponent<Outline> ().effectColor = new Color (0,0,0,1);
				hintText.font = arial;
				hintText.alignment = TextAnchor.MiddleLeft;
				hintText.fontSize = 18;
				hintText.color = Color.white;

				hintText.rectTransform.anchorMax = new Vector2 (0, 0);
				hintText.rectTransform.anchorMin = new Vector2 (0, 0);

				hintText.rectTransform.sizeDelta = new Vector3 (700, 234);
				hintText.rectTransform.anchoredPosition = new Vector2 (390, 146);
				hintText.text = "SPAWN OBJECT: CTRL + Right Mouse Button \n————————————————————— \nMOVE OBJECT: Left Alt + Right Mouse Button \n————————————————————— \nCHANGE SPAWN HEIGHT: Mouse Wheel Up/Down \n———————————————————————— \nMOVE VIEW: W A S D keys (+Left Shift to accelerate) \n———————————————————————— \nROTATE VIEW: Mouse + Right Mouse Button \n———————————————————— \nPAN VIEW: Mouse + Middle Mouse Button";


				//create a new camera component
				Cam = gameObject.AddComponent<Camera> ();
				Cam.allowMSAA = false;
				Cam.depth = 100;
				Cam.nearClipPlane = 0.1f;
				Nl_DragObject.Cam = Cam;

				//create light
				pointLight = gameObject.AddComponent<Light>();
				pointLight.type = LightType.Point;
				pointLight.range = 20;


				//create a pointer object
				if (AssetDatabase.LoadAssetAtPath(EditorPrefs.GetString("RelativePath") + "OPT_PointerAdd_prefab.prefab", typeof(GameObject)) != null) {
					GameObject SpawnPointObj = Instantiate(AssetDatabase.LoadAssetAtPath(EditorPrefs.GetString("RelativePath") + "OPT_PointerAdd_prefab.prefab", typeof(GameObject))) as GameObject;
					SpawnPoint = SpawnPointObj.transform;
					pointerArrow = GameObject.Find ("OPT_PointerArrow").transform;
					pointerArrow.localPosition = new Vector3 (0, spawnOffcetY, 0);
					isSpawnPoint = true;
				} else {
					//fallback to simple pointer if prefab not found
					isSpawnPoint = false;
					SpawnPoint = GameObject.CreatePrimitive (PrimitiveType.Sphere).transform;
					SpawnPoint.GetComponent<SphereCollider> ().enabled = false;
					SpawnPoint.localScale = new Vector3 (0.5f, 0.01f, 0.5f);
					MeshRenderer SpawnPointMesh = SpawnPoint.GetComponent<MeshRenderer> ();
					SpawnPointMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
					SpawnPointMesh.material = new Material (Shader.Find ("Standard"));
					SpawnPointMesh.material.color = new Color (0.137f, 0.713f, 1, 0.5f);
				}
				
				SpawnPoint.transform.SetAsFirstSibling ();
				SpawnPoint.gameObject.hideFlags = HideFlags.NotEditable;
				SpawnPoint.name = "SpawnPoint";
				SpawnPoint.gameObject.SetActive (false);

				Objects.Clear ();

				Stream stream = File.Open (EditorPrefs.GetString("RelativePath") + "Cache/NL_PhysicsPlacerObjectsCache.data", FileMode.Open);
				BinaryFormatter bformater = new BinaryFormatter ();
				droppedObjectsPaths = bformater.Deserialize (stream) as List<string>;
				stream.Close ();

				foreach(string _path in droppedObjectsPaths){
					GameObject _droppedObj = (GameObject) AssetDatabase.LoadAssetAtPath (_path, typeof(GameObject));
					Objects.Add (_droppedObj);
				}

			}
		} else {
			this.enabled = false;
		}
	}
		

	void SpawnObjectRuntime(){
		
		int rndIndex = Random.Range (0, Objects.Count); //select a random object to spawn from list

		createdObj = (GameObject)PrefabUtility.InstantiatePrefab(Objects[rndIndex]); //spawn selected object

		_ObjectsList.Add (createdObj); //add spawned object to list

		createdObj.transform.position = new Vector3 (hit.point.x, hit.point.y + spawnOffcetY, hit.point.z); // set a spawned object position to the click point

		if (EditorPrefs.HasKey ("RandomizeRotation")) {
			if (EditorPrefs.GetBool ("RandomizeRotation") == true) {
			
				Vector3 AnglesRandomLimits = new Vector3 (EditorPrefs.GetFloat ("RandomLimitX"), EditorPrefs.GetFloat ("RandomLimitY"), EditorPrefs.GetFloat ("RandomLimitZ"));

				createdObj.transform.eulerAngles = new Vector3 (EditorPrefs.GetFloat ("RotationX") + Random.Range (-AnglesRandomLimits.x / 2, AnglesRandomLimits.x / 2), EditorPrefs.GetFloat ("RotationY") + createdObj.transform.rotation.y + Random.Range (-AnglesRandomLimits.y / 2, AnglesRandomLimits.y / 2), EditorPrefs.GetFloat ("RotationZ") + Random.Range (-AnglesRandomLimits.z / 2, AnglesRandomLimits.z / 2));
			} else if (EditorPrefs.HasKey ("RotationX")) {
				createdObj.transform.eulerAngles = new Vector3 (EditorPrefs.GetFloat ("RotationX"), EditorPrefs.GetFloat ("RotationY"), EditorPrefs.GetFloat ("RotationZ"));
			} else {
				createdObj.transform.eulerAngles = Vector3.zero;
			}
		} else {
			createdObj.transform.eulerAngles = Vector3.zero;
		}

		if (EditorPrefs.HasKey ("RandomizeScale")) {
			if (EditorPrefs.GetBool ("RandomizeScale") == true) {
				float scaleMin = EditorPrefs.GetFloat ("ScaleMin");
				float scaleMax = EditorPrefs.GetFloat ("ScaleMax");
				float randomScale = Random.Range (scaleMin, scaleMax);
				createdObj.transform.localScale = new Vector3 (randomScale, randomScale, randomScale);
			
			} else if (EditorPrefs.HasKey ("Scale")) {
				float scale = EditorPrefs.GetFloat ("Scale");
				createdObj.transform.localScale = new Vector3 (scale, scale, scale);
			} else {
				createdObj.transform.localScale = new Vector3 (1, 1, 1);
			}
		} else {
			createdObj.transform.localScale = new Vector3 (1, 1, 1);
		}

		//add rigidbody component to the spawned object and set/change collision to MeshCollider for more precise simulation
		Rigidbody createdRB;
		if (createdObj.GetComponent<Rigidbody> () == null) {
			createdRB = createdObj.AddComponent<Rigidbody> (); 
		} else {
			createdRB = createdObj.GetComponent<Rigidbody> ();
		}

		createdObj.AddComponent<Nl_DragObject>();


		if(createdObj.GetComponentsInChildren<Collider>().Length == 0){
			foreach(Transform _transform in createdObj.GetComponentsInChildren<Transform>()){
				if(_transform.GetComponent<MeshFilter>() != null){
					_transform.gameObject.AddComponent<MeshCollider> ();
				}
			}
		}


		foreach(Collider _coll in createdObj.GetComponentsInChildren<Collider>()){

			if (_coll.gameObject.GetComponent<MeshFilter> () != null) {

				_coll.enabled = false;
				MeshCollider _meshColl = _coll.gameObject.AddComponent<MeshCollider> ();

				_meshColl.sharedMesh = _coll.gameObject.GetComponent<MeshFilter> ().mesh;
				_meshColl.convex = true;
				createdRB.mass = 40 * (_meshColl.bounds.size.x * _meshColl.bounds.size.y * _meshColl.bounds.size.z);
			} else {
				createdRB.mass = 40 * (_coll.bounds.size.x * _coll.bounds.size.y * _coll.bounds.size.z);
			}
		}
		Undo.RegisterCreatedObjectUndo (createdObj, "Spawn object " + createdObj.name);
	}
		
	public void SpawnObjectEditor(){
		
		SelectedObjects.Clear ();

		Stream stream = File.Open (EditorPrefs.GetString("RelativePath") + "Cache/NL_PhysicsPlacerSpawnCache.data", FileMode.Open);
		BinaryFormatter bformater = new BinaryFormatter ();
		_objects = bformater.Deserialize (stream) as List<SavedObject>;
		stream.Close ();

		foreach(SavedObject so in _objects){
			createdObj = (GameObject) AssetDatabase.LoadAssetAtPath (so.path, typeof(GameObject));
			createdObj = (GameObject)PrefabUtility.InstantiatePrefab (createdObj);
			createdObj.transform.position = new Vector3 (so.posX, so.posY, so.posZ);
			createdObj.transform.eulerAngles = new Vector3 (so.rotX, so.rotY, so.rotZ);
			createdObj.transform.localScale = new Vector3 (so.scale, so.scale, so.scale);
			Undo.RegisterCreatedObjectUndo (createdObj, "Create object " + createdObj.name);
			SelectedObjects.Add (createdObj);
			if(parentObj != null){
				createdObj.transform.parent = parentObj.transform;
			}
		}
		Selection.objects = SelectedObjects.ToArray();
	}
	public void SaveObjects(){
		_objects.Clear ();
		EditorPrefs.SetBool ("PhysicsEditMode", false);

		if(_ObjectsList.Count > 0){
			foreach(GameObject _obj in _ObjectsList){

				SavedObject so = new SavedObject ();

				so.path = AssetDatabase.GetAssetPath (PrefabUtility.GetCorrespondingObjectFromSource(_obj));

				so.posX = _obj.transform.position.x;
				so.posY = _obj.transform.position.y;
				so.posZ = _obj.transform.position.z;

				so.rotX = _obj.transform.eulerAngles.x;
				so.rotY = _obj.transform.eulerAngles.y;
				so.rotZ = _obj.transform.eulerAngles.z;

				so.scale = _obj.transform.localScale.x;

				_objects.Add (so);
			}
		}


		EditorPrefs.SetInt ("CreatedObjectsCount", _objects.Count);

		Stream stream = File.Open (EditorPrefs.GetString("RelativePath") + "Cache/NL_PhysicsPlacerSpawnCache.data", FileMode.Create);
		BinaryFormatter bformater = new BinaryFormatter ();
		bformater.Serialize (stream, _objects);
		stream.Close ();
	}

	void OnApplicationQuit(){
		SaveObjects ();
	}

	[System.Serializable]
	public struct SavedObject {

		public string path;
		
		public float posX;
		public float posY;
		public float posZ;

		public float rotX;
		public float rotY;
		public float rotZ;

		public float scale;
	}

	void UndoCallback(){
		if(_ObjectsList.Count > 0){
			_ObjectsList.RemoveAt (_ObjectsList.Count - 1);
		}

	}
		
	void Update () {

		if(Application.isEditor){

			if(Input.GetKeyDown(KeyCode.LeftAlt)){
				Cursor.SetCursor (cursorDrag, new Vector2 (12, 11), CursorMode.Auto);
			}
			if(Input.GetKeyUp(KeyCode.LeftAlt)){
				Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
			}

			if(Input.GetKeyDown(KeyCode.Escape)){
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}

			if (!Cursor.visible || Cursor.lockState == CursorLockMode.Locked) {
				showCursorText.gameObject.SetActive (true);
			} else {
				showCursorText.gameObject.SetActive (false);
			}

			if(Input.GetAxis("Mouse ScrollWheel") != 0){
				if (SpawnPoint.gameObject.activeSelf) {
					spawnOffcetY += Input.GetAxis ("Mouse ScrollWheel");
					spawnOffcetY = Mathf.Clamp (spawnOffcetY, 0.1f, 20);
					if (isSpawnPoint) {
						pointerArrow.localPosition = new Vector3 (0, spawnOffcetY, 0);
					}
				} else {
					if (Input.GetKey (KeyCode.LeftShift)) {
						transform.Translate (0, 0, Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime * 600);
					} else {
						transform.Translate (0, 0, Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime * 150);
					}
				}
			}



			if(Input.GetMouseButtonDown(2)){
				lastPos = Input.mousePosition;
			}

			if(Input.GetMouseButton(2)){

				Vector3 delta = Input.mousePosition - lastPos;
				transform.Translate (-delta.x * 0.3f * Time.deltaTime, -delta.y * 0.3f * Time.deltaTime, 0);
				lastPos = Input.mousePosition;
			}

			if(Input.GetMouseButton(1) && !Input.GetKey(KeyCode.LeftAlt)){
				
				yRot += Input.GetAxis ("Mouse X") * 3;
				xRot -= Input.GetAxis("Mouse Y") * 3;
				transform.localEulerAngles = new Vector3 (xRot, yRot, 0);
			}

			if (Input.GetAxis("Vertical") != 0 && !Input.GetKey(KeyCode.LeftAlt)){
				transform.Translate(Vector3.forward * 3 * boost * Input.GetAxis("Vertical") * Time.deltaTime);
			}

			if (Input.GetAxis("Horizontal") != 0 && !Input.GetKey(KeyCode.LeftAlt)){
				transform.Translate(Vector3.right * 3 * boost * Input.GetAxis("Horizontal") * Time.deltaTime);
			}

			if (Input.GetKey (KeyCode.LeftShift)) {
				boost += 3 * 1.5f * Time.deltaTime;
			} else {
				boost = 1;
			}
				
			
			if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.LeftCommand)) {

				Ray ray = Cam.ScreenPointToRay (Input.mousePosition);
				LayerMask layermask = ~0;
				if (Physics.Raycast (ray, out hit, 1000, layermask, QueryTriggerInteraction.Ignore)) {
					SpawnPoint.gameObject.SetActive (true);

                    float pointScale = Mathf.Clamp(Vector3.Distance(SpawnPoint.position, Cam.transform.position) * 0.3f, 0.1f, 1f); 
                    SpawnPoint.localScale = new Vector3(pointScale, pointScale, pointScale);

					Cursor.SetCursor (cursorEmpty, new Vector2 (0, 0), CursorMode.Auto);
					SpawnPoint.position = hit.point;
				}

				if (Input.GetMouseButtonDown (1) && SpawnPoint.gameObject.activeSelf) {
					SpawnObjectRuntime ();

				}
			} else if(Input.GetKeyUp (KeyCode.LeftControl) || Input.GetKeyUp (KeyCode.LeftCommand)) {
				SpawnPoint.gameObject.SetActive (false);
				Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
			}
	}
}
}
#endif

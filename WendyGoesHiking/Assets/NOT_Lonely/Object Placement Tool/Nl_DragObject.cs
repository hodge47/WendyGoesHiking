using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nl_DragObject : MonoBehaviour {
	public static Camera Cam;
	private Rigidbody rigidboy;
	private float distanceZ;
	private bool isTaken = false;
	private Vector3 offset;
	private Vector3 dir;
	// Use this for initialization
	void Start () {
		rigidboy = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(isTaken){
			if (Input.GetMouseButton (1)) {
				Vector3 mousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distanceZ);
				Vector3 objPos = Cam.ScreenToWorldPoint (mousePos); 
				rigidboy.MovePosition (objPos + offset);
			} else {
				rigidboy.useGravity = true;
				rigidboy.constraints = RigidbodyConstraints.None;
				isTaken = false;
			}
			if(Input.GetAxis("Horizontal") != 0 && Input.GetKey(KeyCode.LeftAlt)){
				transform.Rotate (Vector3.up * 100 * Time.deltaTime * Input.GetAxis ("Horizontal"));
			}
			if(Input.GetAxis("Vertical") != 0 && Input.GetKey(KeyCode.LeftAlt)){
				transform.Rotate (Vector3.right * 100 * Time.deltaTime * Input.GetAxis ("Vertical"));
			}
		}
	}

	void OnMouseOver(){
		if(Input.GetKey(KeyCode.LeftAlt)){
			if(Input.GetMouseButtonDown(1)){
				isTaken = true;
				distanceZ = Vector3.Distance (Cam.transform.position, gameObject.transform.position);

				Vector3 mousePosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distanceZ);
				Vector3 objPosition = Cam.ScreenToWorldPoint (mousePosition);

				offset = rigidboy.position - objPosition;

				rigidboy.velocity = Vector3.zero;
				rigidboy.useGravity = false;
				rigidboy.constraints = RigidbodyConstraints.FreezeRotation;
			}
		}
	}
}

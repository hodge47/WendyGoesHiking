using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    ///jpost Audio
    ///a class to check the gameobject tag of objects directly below the player
    ///

    //public string footStepType;
    public SphereCollider sphereCollider;
    public List<GameObject> footStepObjects;
    public string currentFootstepType;

    private void Start()
    {
        sphereCollider = GetComponentInParent<SphereCollider>();
        footStepObjects = new List<GameObject>();
        currentFootstepType = "none";
    }

    //a way of detecting if a nearbyGameObject is tagged "Grass"
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Grass":
                currentFootstepType = "Grass";
                break;
            case "Dirt":
                currentFootstepType = "Dirt";
                break;
            default:
                currentFootstepType = "none";
                break;
        }
        if (other.gameObject.tag == "Grass")
        {
            footStepObjects.Add(other.gameObject);
            currentFootstepType = other.gameObject.tag;
        }
    }

    //a way of detecting if a nearbyGameObject is tagged "Grass" and the player has moved away from it
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Grass")
        {
            footStepObjects.Remove(other.gameObject);
        }
    }


    ///not working currently
    //private void Start()
    //{
    //    //footStepType = "Dirt";
    //}
    //private void Update()
    //{        
    //    RaycastHit hit;
    //    //checks to see if a ray of length 1.25f pointing down from the center of the player intersects with an object
    //    if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.25f))
    //    {             
    //        //if the object intersecting is tagged as grass
    //        if (hit.collider.CompareTag("Grass"))
    //        {
    //            print("object tag: " + hit.collider.tag);
    //            footStepType = hit.collider.tag;
    //            Debug.DrawLine(transform.position, hit.point, Color.green);

    //        }
    //        //if the object intersecting is tagged as dirt
    //        if (hit.collider.CompareTag("Dirt"))
    //        {
    //            print("object tag: " + hit.collider.tag);
    //            footStepType = hit.collider.tag.ToString(); ;
    //            Debug.DrawLine(transform.position, hit.point, Color.green);
    //        }            
    //    }

    //    //footStepType = hit.collider.tag.ToString();
    //    //print("object tag: " + hit.collider.tag);
    //}

    //public string GetObjectTagUnderPlayer()
    //{
    //    string objectUnderPlayer = "None";
    //    RaycastHit hit;
    //    //checks to see if a ray of length 1.25f pointing down from the center of the player intersects with an object
    //    if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.25f))
    //    {
    //        //if the object intersecting is tagged as grass
    //        if (hit.collider.CompareTag("Grass"))
    //        {
    //            //print("object tag: " + hit.collider.tag);
    //            footStepType = hit.collider.tag;
    //            Debug.DrawLine(transform.position, hit.point, Color.green);

    //        }
    //        //if the object intersecting is tagged as dirt
    //        if (hit.collider.CompareTag("Dirt"))
    //        {
    //            //print("object tag: " + hit.collider.tag);
    //            footStepType = hit.collider.tag.ToString(); ;
    //            Debug.DrawLine(transform.position, hit.point, Color.green);
    //        }
    //    }

    //    //footStepType = hit.collider.tag.ToString();
    //    //print("object tag: " + hit.collider.tag);
    //    return objectUnderPlayer;
    //}
}

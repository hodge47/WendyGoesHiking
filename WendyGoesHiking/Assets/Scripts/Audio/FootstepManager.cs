using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    ///jpost Audio
    ///a class to check the gameobject tag of objects directly below the player and tell the GlideController.cs what footstep sfx to play
    ///
    
    private SphereCollider sphereCollider;    
    public string currentFootstepType;

    private void Start()
    {
        sphereCollider = GetComponentInParent<SphereCollider>();        
        currentFootstepType = "none";       
    }

    //a way of detecting if a nearby GameObject is tagged "Grass"
    private void OnTriggerStay(Collider other)
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
                currentFootstepType = "Default";
                break;
        }        
    }

    ///deprecated and currently broken
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFMODOneShot : MonoBehaviour
{
    ///jpost Audio
    ///a class dedicated to playing an FMOD event OnTriggerEnter
    ///

    //serialized fields
    //decide whether or not the event should play at the location of the main camera
    [SerializeField] bool playEventAtMainCamera = true;
    //decide whether or not the event should be triggered by the player colliding with the trigger's boxcollider
    [SerializeField] bool playerTrigger = true;
    //decide whether or not this event can be played *every* time the triggerObject enters this objects box collider
    [SerializeField] bool playEventOnce = true;

    //box collider
    private BoxCollider boxCollider;
    //the tag (string) of the collider to trigger this event
    public string triggerObject;
    //the name (string) of the FMOD event to play
    public string eventPath;
    //the gameobject that the FMOD event should play at
    public GameObject playBackObject;
    //a boolean gate to allow for the FMOD event to only be played the *first* time the triggerObject enters 
    private bool canPlayOneShot = true;

    

    private void Start()
    {
        //initialize boxCollider to be the gameObject's box collider
        boxCollider = gameObject.GetComponent<BoxCollider>();
        
        //initialize play back object to default player camera
        if (playEventAtMainCamera)
        {
            playBackObject = GameObject.FindGameObjectWithTag("MainCamera");
        }
        //initialze triggerObject to be the player if playerTrigger is true
        if (playerTrigger)
        {
            triggerObject = "Player";
        }        
    }
    
    //method for detecting trigger
    private void OnTriggerEnter(Collider other)
    {
        //if the gameobject collides with the triggerObject
        if (other.CompareTag(triggerObject))
        {
            if (canPlayOneShot)
            {
                PlayFMODOneShot();
            }          

            //if playEventOnce is true, set canPlayOneShot to false
            if (playEventOnce)
            {
                canPlayOneShot = false;
            }
        }
    }

    //a method to play the desired FMOD event at the location set by playBackObject
    public void PlayFMODOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventPath, playBackObject.transform.position);
    }
}

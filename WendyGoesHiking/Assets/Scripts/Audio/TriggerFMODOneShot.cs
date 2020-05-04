using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFMODOneShot : MonoBehaviour
{
    ///jpost Audio
    ///a class dedicated to playing an FMOD event OnTriggerEnter
    ///

    //serialized fields
    //decide whether or not this event can be played *every* time the triggerTag enters this objects box collider
    [SerializeField] bool playEventOnce = true;
    //decide whether or not the event should play at the location of the main camera
    [SerializeField] bool playEventAtMainCamera = true;
    //decide whether or not the event should be triggered by the player colliding with the trigger's boxcollider
    [SerializeField] bool playerTrigger = true;
    

    //box collider
    private BoxCollider boxCollider;
    //the tag (string) of the collider to trigger this event
    public string triggerTag;
    //the name (string) of the FMOD event to play
    public string eventPath;
    //the gameobject that the FMOD event should play at
    public GameObject playBackObject;
    //a boolean gate to allow for the FMOD event to only be played the *first* time the triggerTag enters 
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
        //initialze triggerTag to be the player if playerTrigger is true
        if (playerTrigger)
        {
            triggerTag = "Player";
        }        
    }
    
    //method for detecting trigger
    private void OnTriggerEnter(Collider other)
    {
        //if the gameobject collides with the triggerTag
        if (other.CompareTag(triggerTag))
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

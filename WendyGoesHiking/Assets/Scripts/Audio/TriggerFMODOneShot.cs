using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFMODOneShot : MonoBehaviour
{
    ///jpost Audio
    ///a class dedicated to playing an FMOD event OnTriggerEnter
    ///

    //box collider
    private BoxCollider boxCollider;
    //the tag (string) of the collider to trigger this event
    public string triggerObject;
    //the name (string) of the FMOD event to play
    public string eventPath;
    //the gameobject that the FMOD event should play at
    public GameObject playBackObject;

    //serialized fields
    [SerializeField] bool playEventAtMainCamera = true;
    [SerializeField] bool playerTrigger = true;

    private void Start()
    {
        //initialize boxCollider to be the gameObject's box collider
        boxCollider = gameObject.GetComponent<BoxCollider>();
        //initialize event name to default empty string
        eventPath = "";
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
            PlayFMODOneShot();
        }
    }

    //a method to play the desired FMOD event at the location set by 
    public void PlayFMODOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventPath, playBackObject.transform.position);
    }
}

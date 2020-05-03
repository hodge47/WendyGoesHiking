using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class AmbienceEmitterHandler : MonoBehaviour
{
    //jpost Audio
    /// a script to control which type of ambience should be played based on if the player has progressed to nighttime or not
    /// 

    //has a list of daytime emitters
    public List<GameObject> ambienceEmitters = new List<GameObject>();
       
    //method to change the FMOD event that the emitter plays
    public void ChangeFMODEvent()
    {
        //debug testing
        //Debug.Log("FMOD ambience event should be changing");

        foreach(GameObject g in ambienceEmitters)
        {
            g.GetComponent<StudioEventEmitter>().SendMessage("Stop");
            g.GetComponent<StudioEventEmitter>().Event = "event:/Ambience/sx_wgh_game_amb_tree_static_night";
            //g.GetComponent<StudioEventEmitter>().SendMessage("Play");
        }
    }
}

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

    public bool isNightTime;

    //method to change the FMOD event that the emitter plays
    public void ChangeFMODEvent()
    {
        foreach(GameObject g in ambienceEmitters)
        {
            StudioEventEmitter emitterComponent = GetComponent<StudioEventEmitter>();
            emitterComponent.Event = "event:/Ambience/sx_wgh_game_amb_tree_static_night";
        }
    }
}

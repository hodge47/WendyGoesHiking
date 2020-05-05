using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCutsceneAudio : MonoBehaviour
{
    public FMOD.Studio.EventInstance cutsceneAudioEvent;

    // Start is called before the first frame update
    void Start()
    {
        cutsceneAudioEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Interactibles/sx_wgh_game_int_cutscene_fall_2d");

    }

    //a method to determine the playback state of an FMOD EVENT instance
    //taken from https://alessandrofama.com/tutorials/fmod-unity/playback-states/
    FMOD.Studio.PLAYBACK_STATE PlaybackState(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE pS;
        instance.getPlaybackState(out pS);
        return pS;
    }

    //jpost Audio
    public void PlayCutsceneAudio()
    {
        Debug.Log("Play Cutscene audio!");
        if (PlaybackState(cutsceneAudioEvent) != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            cutsceneAudioEvent.start();

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

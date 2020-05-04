using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class IntroMusicProgress : MonoBehaviour
{
    //jpost Audio

    ///A script to manage all FMOD music states as well as some mixing scripting
    ///
    public FMOD.Studio.PLAYBACK_STATE pState;
    public float introProgress;
    //define strings for FMOD events
    public string introMusic = "event:/Music/Intro/mx_wgh_intro";
    public string nightMusic = "event:/Music/Night Time/mx_wgh_nighttime";
    public string safeMusic = "event:/Music/Safe Zone/mx_wgh_safezone";
    public string interiorSnapshot = "snapshot:/Interior Space"; 

    //create FMOD events
    public FMOD.Studio.EventInstance introMusicEvent;
    public FMOD.Studio.EventInstance nightMusicEvent;
    public FMOD.Studio.EventInstance safeMusicEvent;
    public FMOD.Studio.EventInstance interiorSnapshotEvent;
    //declare FMOD parameter link
    public FMOD.Studio.PARAMETER_ID introProgressParameterId;

    [SerializeField] bool playIntroMusic;

    private void Start()
    {
        //initialize introProgress
        introProgress = 0;
        //assign FMOD events
        introMusicEvent = FMODUnity.RuntimeManager.CreateInstance(introMusic);
        nightMusicEvent = FMODUnity.RuntimeManager.CreateInstance(nightMusic);
        safeMusicEvent = FMODUnity.RuntimeManager.CreateInstance(safeMusic);
        interiorSnapshotEvent = FMODUnity.RuntimeManager.CreateInstance(interiorSnapshot);

        introMusicEvent.getParameterByName("introProgress", out introProgress);
        
        //allow user toggling of enabling music in the editor
        if (playIntroMusic)
        {
            introMusicEvent.start();
        }
        
    }
    
    //progresses introProgress appropriately
    private void OnTriggerEnter(Collider other)
    {
        switch (other.name)
        {
            case "Intro Music Progress Zone 1":
                introProgress = 0;                
                break;
            case "Intro Music Progress Zone 2":
                introProgress = 10;
                break;
            case "Intro Music Progress Zone 3":
                introProgress = 20;
                break;
            case "Intro Music Progress Zone 4":
                introProgress = 30;
                break;
            case "Intro Music Progress Zone 5":
                introProgress = 0;
                introMusicEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                
                //ChangeToNightMusic();
                break;
        }

        introMusicEvent.setParameterByName("introProgress", introProgress);
    }

    //a method to determine the playback state of an FMOD EVENT instance
    //taken from https://alessandrofama.com/tutorials/fmod-unity/playback-states/
    FMOD.Studio.PLAYBACK_STATE PlaybackState(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE pS;
        instance.getPlaybackState(out pS);
        return pS;
    }

    public void ChangeToNightMusic()
    {
        if(PlaybackState(introMusicEvent) == FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            introMusicEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        if (PlaybackState(safeMusicEvent) == FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            safeMusicEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        if (PlaybackState(nightMusicEvent) != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            nightMusicEvent.start();
        }
        else
        {
            Debug.Log("Event instance already playing");
        }
    }

    public void ChangeToSafeZoneMusic()
    {
        if (PlaybackState(nightMusicEvent) == FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            nightMusicEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            safeMusicEvent.start();
        }
    }

    public void ChangeToInteriorSpaceSnapshot()
    {
        interiorSnapshotEvent.start();
    }

    public void ChangeToDefaultSnapshot()
    {
        interiorSnapshotEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}

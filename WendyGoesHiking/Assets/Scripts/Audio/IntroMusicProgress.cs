using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class IntroMusicProgress : MonoBehaviour
{
    //jpost Audio

    ///A script to pass a parameter value from Unity to FMOD that controls what sections of a song to play based on player progression
    ///

    public float introProgress;
    
    public string introMusic = "event:/Music/Intro/mx_wgh_intro";
    public FMOD.Studio.EventInstance introMusicEvent;
    public FMOD.Studio.PARAMETER_ID introProgressParameterId;

    [SerializeField] bool playIntroMusic;

    private void Start()
    {
        introProgress = 0;

        introMusicEvent = FMODUnity.RuntimeManager.CreateInstance(introMusic);
        introMusicEvent.getParameterByName("introProgress", out introProgress);
        if (playIntroMusic)
        {
            introMusicEvent.start();
        }
        
    }

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
                break;
        }
        introMusicEvent.setParameterByName("introProgress", introProgress);
    }
}

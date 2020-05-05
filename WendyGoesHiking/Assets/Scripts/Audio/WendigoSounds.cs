using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class WendigoSounds : MonoBehaviour
{
    //jpost audio
    /// a class to handle playback of all of the Wendigo SFX
    /// 

    //event strings
    //attack
    public string wendigoAttack = "event:/NPC/Wendigo/Vocalizations/sx_wgh_game_npc_wendigo_atk";
    //take damage
    public string wendigoTakeDamage = "event:/NPC/Wendigo/Vocalizations/sx_wgh_game_npc_wendigo_takedamage";
    //breath
    public string wendigoBreath = "event:/NPC/Wendigo/Vocalizations/sx_wgh_game_npc_wendigo_breath";
    //walk/run
    public string wendigoRun = "event:/NPC/Wendigo/Movement/sx_wgh_game_npc_wendigo_run";
    //tree jump
    public string wendigoJumpTree = "event:/NPC/Wendigo/Movement/sx_wgh_game_npc_wendigo_jump_tree";
    //tree land
    public string wendigoLandTree = "event:/NPC/Wendigo/Movement/sx_wgh_game_npc_wendigo_land_tree";
    //die
    public string wendigoDie = "event:/NPC/Wendigo/Vocalizations/sx_wgh_game_npc_wendigo_die";

    //event instances
    public FMOD.Studio.EventInstance wendigoAttackEvent;
    public FMOD.Studio.EventInstance wendigoTakeDamageEvent;
    public FMOD.Studio.EventInstance wendigoBreathEvent;
    public FMOD.Studio.EventInstance wendigoJumpTreeEvent;
    public FMOD.Studio.EventInstance wendigoLandTreeEvent;
    public FMOD.Studio.EventInstance wendigoRunEvent;
    public FMOD.Studio.EventInstance wendigoDieEvent;

    private void Start()
    {
        //initialize events with strings
        wendigoAttackEvent = FMODUnity.RuntimeManager.CreateInstance(wendigoAttack);
        wendigoTakeDamageEvent = FMODUnity.RuntimeManager.CreateInstance(wendigoTakeDamage);
        wendigoBreathEvent = FMODUnity.RuntimeManager.CreateInstance(wendigoBreath);
        wendigoJumpTreeEvent = FMODUnity.RuntimeManager.CreateInstance(wendigoJumpTree);
        wendigoLandTreeEvent = FMODUnity.RuntimeManager.CreateInstance(wendigoLandTree);
        wendigoRunEvent = FMODUnity.RuntimeManager.CreateInstance(wendigoRun);
        wendigoDieEvent = FMODUnity.RuntimeManager.CreateInstance(wendigoDie);
    }

    //methods

    //a method to determine the playback state of an FMOD EVENT instance
    //taken from https://alessandrofama.com/tutorials/fmod-unity/playback-states/
    FMOD.Studio.PLAYBACK_STATE PlaybackState(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE pS;
        instance.getPlaybackState(out pS);
        return pS;
    }

    //play Wendigo Attack sfx
    public void PlayWendigoAttackEvent()
    {
        if(PlaybackState(wendigoAttackEvent) != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            wendigoAttackEvent.start();
        }        
    }
    //play Wendigo Take Damage sfx
    public void PlayWendigoTakeDamageEvent()
    {
        if (PlaybackState(wendigoAttackEvent) != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            wendigoTakeDamageEvent.start();
        }
    }
    //play Wendigo Breath sfx
    public void PlayWendigoBreathEvent()
    {
        wendigoBreathEvent.start();
        if (PlaybackState(wendigoBreathEvent) != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            Debug.Log("try to play breath");
            
        }
    }
    //play Wendigo Jump Tree sfx
    public void PlayWendigoJumpTreeEvent()
    {
        if (PlaybackState(wendigoJumpTreeEvent) != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            wendigoJumpTreeEvent.start();
        }
    }
    //play Wendigo Land Tree sfx
    public void PlayWendigoLandTreeEvent()
    {
        if (PlaybackState(wendigoLandTreeEvent) != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            wendigoLandTreeEvent.start();
        }
    }
    //play Wendigo Run sfx
    public void PlayWendigoRunEvent()
    {
        if (PlaybackState(wendigoRunEvent) != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            wendigoRunEvent.start();
        }
    }
    //play Wendigo Die sfx
    public void PlayWendigoDieEvent()
    {
        if (PlaybackState(wendigoDieEvent) != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            wendigoDieEvent.start();
        }
    }

}

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
    //thud
    public string wendigoThud = "event:/NPC/Wendigo/Movement/sx_wgh_game_npc_wendigo_body_thud";

    ////event instances
    //public FMOD.Studio.EventInstance wendigoAttackEvent;
    //public FMOD.Studio.EventInstance wendigoTakeDamageEvent;
    //public FMOD.Studio.EventInstance wendigoBreathEvent;
    //public FMOD.Studio.EventInstance wendigoJumpTreeEvent;
    //public FMOD.Studio.EventInstance wendigoLandTreeEvent;
    //public FMOD.Studio.EventInstance wendigoRunEvent;
    //public FMOD.Studio.EventInstance wendigoDieEvent;

    private void Start()
    {
        ////initialize events with strings
        //wendigoAttackEvent = FMODUnity.RuntimeManager.CreateInstance(wendigoAttack);
        //wendigoTakeDamageEvent = FMODUnity.RuntimeManager.CreateInstance(wendigoTakeDamage);
        //wendigoBreathEvent = FMODUnity.RuntimeManager.CreateInstance(wendigoBreath);
        //wendigoJumpTreeEvent = FMODUnity.RuntimeManager.CreateInstance(wendigoJumpTree);
        //wendigoLandTreeEvent = FMODUnity.RuntimeManager.CreateInstance(wendigoLandTree);
        //wendigoRunEvent = FMODUnity.RuntimeManager.CreateInstance(wendigoRun);
        //wendigoDieEvent = FMODUnity.RuntimeManager.CreateInstance(wendigoDie);
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
        FMODUnity.RuntimeManager.PlayOneShot(wendigoAttack, gameObject.transform.position);                      
    }
    //play Wendigo Take Damage sfx
    public void PlayWendigoTakeDamageEvent()
    {
        FMODUnity.RuntimeManager.PlayOneShot(wendigoTakeDamage, gameObject.transform.position);
    }
    //play Wendigo Breath sfx
    public void PlayWendigoBreathEvent()
    {
        FMODUnity.RuntimeManager.PlayOneShot(wendigoBreath, gameObject.transform.position);
    }
    //play Wendigo Jump Tree sfx
    public void PlayWendigoJumpTreeEvent()
    {
        FMODUnity.RuntimeManager.PlayOneShot(wendigoJumpTree, gameObject.transform.position);
    }
    //play Wendigo Land Tree sfx
    public void PlayWendigoLandTreeEvent()
    {
        FMODUnity.RuntimeManager.PlayOneShot(wendigoLandTree, gameObject.transform.position);
    }
    //play Wendigo Run sfx
    public void PlayWendigoRunEvent()
    {
        FMODUnity.RuntimeManager.PlayOneShot(wendigoRun, gameObject.transform.position);
    }
    //play Wendigo Die sfx
    public void PlayWendigoDieEvent()
    {
        FMODUnity.RuntimeManager.PlayOneShot(wendigoDie, gameObject.transform.position);
    }
    //play Wendigo Body Thud sfx
    public void PlayWendigoThudEvent()
    {
        FMODUnity.RuntimeManager.PlayOneShot(wendigoThud, gameObject.transform.position);
    }

}

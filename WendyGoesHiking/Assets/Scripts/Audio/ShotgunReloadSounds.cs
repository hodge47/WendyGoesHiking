using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunReloadSounds : MonoBehaviour
{
    //jpost Audio
    ///a script to control the triggering of shotgun sounds based off of keyframes in the animation
    ///

    //fmod audio event methods
    public void PlayOpenBreach()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Interactibles/sx_wgh_game_int_weapon_shotgun_breach_open", gameObject.transform.position);
    }

    public void PlayDumpShells()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Interactibles/sx_wgh_game_int_weapon_shotgun_dump_shells", gameObject.transform.position);
    }

    public void PlayLoadShells()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Interactibles/sx_wgh_game_int_weapon_shotgun_load_shells", gameObject.transform.position);
    }

    public void PlayCloseBreach()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Interactibles/sx_wgh_game_int_weapon_shotgun_breach_close", gameObject.transform.position);
    }

}

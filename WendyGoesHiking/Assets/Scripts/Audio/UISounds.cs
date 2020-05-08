using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    //jpost Audio
    /// <summary>
    /// a class to handle plaback of UI sounds in game
    /// </summary>


    //method to play UI click through sfx
    public void PlayUIClickThrough()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/sx_wgh_game_ui_click_through", gameObject.transform.position);
    }
    //method to play UI click back sfx
    public void PlayUIClickBack()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/sx_wgh_game_ui_click_back", gameObject.transform.position);
    }
    //method to play UI click start sfx
    public void PlayUIClickStartGame()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/sx_wgh_game_ui_click_start_game", gameObject.transform.position);
    }
    //method to play UI click exit sfx
    public void PlayUIClickExitGame()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/sx_wgh_game_ui_click_exit_game", gameObject.transform.position);
    }
    //method to play UI hover sfx
    public void PlayUIHover()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/sx_wgh_game_ui_hover", gameObject.transform.position);
    }
}

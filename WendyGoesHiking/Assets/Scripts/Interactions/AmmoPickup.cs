using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour, IInteractive
{
    [Tooltip("Name of the interactive object the UI will display when looked at in the world.")]
    [SerializeField]
    protected string displayText = nameof(InteractiveObject);
    public virtual string DisplayText => displayText;



    /// <summary>
    /// This is where audio should probably go.
    /// </summary>
    /// 
    //jpost Audio
    private void PlayAmmoPickup()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Interactibles/sx_wgh_game_int_ammobox_pickup", gameObject.transform.position);
    }

    public virtual void InteractWith()
    {
        //Put audio play in here (probably)
        //I didn't implement it
        //Audio people should do it
        //jpost Audio
        PlayAmmoPickup();

        Debug.Log($"Player just interacted with: {gameObject.name}");
        Destroy(gameObject);
    }



}

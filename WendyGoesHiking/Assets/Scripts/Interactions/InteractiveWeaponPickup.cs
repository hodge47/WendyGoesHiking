using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveWeaponPickup : MonoBehaviour, IInteractive
{
    [Tooltip("Name of the interactive object the UI will display when looked at in the world.")]
    [SerializeField]
    protected string displayText = nameof(InteractiveObject);
    [SerializeField]
    GameObject interactiveGameObject;
    public virtual string DisplayText => displayText;

    public GameObject InteractiveGameObject => interactiveGameObject;

    [SerializeField] Gun gun;


    public virtual void InteractWith()
    {
        //Put audio play in here (probably)
        //I didn't implement it
        //Audio people should do it
        
        //jpost Audio
        PlayGunPickup();

        WeaponManager.current.loadout.Add(gun);
        WeaponManager.current.loadout[WeaponManager.current.loadout.Count - 1].Initialize();
        WeaponManager.current.Equip(WeaponManager.current.loadout.Count - 1);


        Debug.Log($"Player just interacted with: {gameObject.name}");


        Destroy(gameObject);



    }

    //jpost Audio
    private void PlayGunPickup()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Interactibles/sx_wgh_game_int_shotgun_pickup", gameObject.transform.position);
    }


}

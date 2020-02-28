using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveAmmoPickup : InventoryObject, IInteractive
{
    [Tooltip("Name of the interactive object the UI will display when looked at in the world.")]
    [SerializeField]
    protected string displayText = nameof(InteractiveObject);
    public virtual string DisplayText => displayText;

    [SerializeField]
    Gun ammoPickup;

    public override void InteractWith()
    {
        WeaponManager.current.loadout.Add(ammoPickup);

        WeaponManager.current.loadout[WeaponManager.current.loadout.Count - 1].Initialize();
        WeaponManager.current.Equip(WeaponManager.current.loadout.Count - 1);

        Debug.Log($"Player just interacted with: {gameObject.name}");
        PlayerInventory.InventoryObjects.Add(this);

        Destroy(gameObject);
    }
}
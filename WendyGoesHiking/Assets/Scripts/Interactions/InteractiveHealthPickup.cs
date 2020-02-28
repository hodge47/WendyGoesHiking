using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveHealthPickup : InventoryObject, IInteractive
{
    [SerializeField]
    Gun healthPickup;

    public int amountOfHealthToHeal;

    public virtual void InteractWith()
    {
        WeaponManager.current.loadout.Add(healthPickup);

        WeaponManager.current.loadout[WeaponManager.current.loadout.Count - 1].Initialize();
        WeaponManager.current.Equip(WeaponManager.current.loadout.Count - 1);

        Debug.Log($"Player just interacted with: {gameObject.name}");
        PlayerInventory.InventoryObjects.Add(this);

        Destroy(gameObject);
    }
}

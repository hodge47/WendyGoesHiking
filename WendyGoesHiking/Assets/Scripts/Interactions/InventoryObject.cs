using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObject : InteractiveObject
{
    //TODO: add long description field for object
    //TODO: add field for icon

    [Tooltip("The name of the object as it will appear in the inventory.")]
    [SerializeField]
    private string objectName = nameof(InventoryObject);
    public string ObjectName => objectName;


    private new Renderer renderer;
    private new Collider collider;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();
    }

    public InventoryObject()
    {
        displayText = $"Take {objectName}.";
    }




    /// <summary>
    /// When player interacts with InventoryObject do 2 things:
    /// 1: add the inventory object to the PlayerInventory
    /// 2: Remove object from game world / scene
    ///     - Disable renderer and collider
    /// </summary>
    public override void InteractWith()
    {
        base.InteractWith();
        PlayerInventory.InventoryObjects.Add(this);
        Debug.Log($"The {ObjectName} was added to the inventory.");
        renderer.enabled = false;
        collider.enabled = false;
        Debug.Log($"Inventory menu game object name: {InventoryMenu.Instance.name}");
    }
}

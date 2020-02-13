﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : InteractiveObject
{
    [Tooltip("Assigning a key will lock the door. Can be opened if the player has the key in their inventory.")]
    [SerializeField]
    private InventoryObject key;

    [Tooltip("If this is checked the key will be removed from the inventory when the key is used.")]
    [SerializeField]
    private bool consumesKey;

    [Tooltip("Text that is displayed to player when door is locked.")]
    [SerializeField]
    private string lockedDisplayText = "Locked.";

    public override string DisplayText
    {
        get
        {
            string toReturn;
            if (isLocked)
            {
                toReturn = HasKey ? $"Use {key.ObjectName}." : lockedDisplayText;
            }
            else
                toReturn = base.displayText;

            return toReturn;
        }
    }
    //=> isLocked ? lockedDisplayText : base.DisplayText;
    private bool HasKey => PlayerInventory.InventoryObjects.Contains(key);


    private bool isLocked;
    private Animator animator;
    private bool isOpen = false;
    private int shouldOpenAnimParameter = Animator.StringToHash(nameof(shouldOpenAnimParameter));

    /// <summary>
    /// Using a constructor to initial the displayText in the editor
    /// </summary>
    public Door()
    {
        displayText = nameof(Door);
    }


    void Awake()
    {
        animator = GetComponent<Animator>();
        InitializeIsLocked();
    }

    private void InitializeIsLocked()
    {
        if (key != null)
        {
            isLocked = true;
        }
    }

    public override void InteractWith()
    {
        if(!isOpen)
        {
            if(isLocked && !HasKey)
            {
                //play locked door sound effect here
            }
            else //if not locked or player has the key
            {
                animator.SetBool(shouldOpenAnimParameter, true);
                displayText = string.Empty;
                isOpen = true;
                UnlockDoor();
            }
            base.InteractWith(); //runs the logic from the base class         
        }
    }

    private void UnlockDoor()
    {
        isLocked = false;

        if (key != null && consumesKey)
        {
            PlayerInventory.InventoryObjects.Remove(key);
        }
    }
}
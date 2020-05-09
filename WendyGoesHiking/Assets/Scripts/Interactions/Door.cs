using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : InteractiveObject
{
    public bool isOpen = false;

    [Tooltip("Assigning a Key here will lock the door.")]
    [SerializeField]
    private InventoryObject key;

    [Tooltip("If this is checked, assigned key will be removed from inventory once used")]
    [SerializeField]
    private bool consumesKey;

    [Tooltip("Check this box to lock the door.")]
    [SerializeField]
    private bool isLocked;

    [Tooltip("Display text for locked doors.")]
    [SerializeField]
    private string lockedDisplayText = "Locked";

    private bool hasKey => PlayerInventory.InventoryObjects.Contains(key);
    private Animator animator;
    private int shouldOpenAnimParamater = Animator.StringToHash("shouldOpen");
    private int shouldCloseAnimParamater = Animator.StringToHash("shouldClose");
    private int shouldLockedAnimParamater = Animator.StringToHash("shouldLocked");

    public override string DisplayText
    {
        get
        {
            string toReturn;
            if (isLocked)
            {
                toReturn = hasKey ? $"Use {key.ObjectName}." : lockedDisplayText;
            }
            else
                toReturn = base.displayText;

            return toReturn;
        }
    }

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
        if (!isOpen)
        {
            if (!isLocked)  //not locked
            {
             //   audioSource.clip = openSound;
                animator.SetBool(shouldOpenAnimParamater, true);
                animator.SetBool(shouldCloseAnimParamater, false);
                isOpen = true;
            }
            else if (isLocked && !hasKey)   //locked no key
            {
                PlayTryLockedDoor();
                animator.Play("Door_Locked_Try");
            }
            else if (isLocked && hasKey)    //locked has key
            {
                animator.SetBool(shouldOpenAnimParamater, true);
                animator.SetBool(shouldCloseAnimParamater, false);
                isOpen = true;
                UnlockDoor();
            }
            base.InteractWith();    //plays a sound effect
        }
        else if (isOpen)
        {
           // audioSource.clip = closeSound;
            base.InteractWith(); //plays a sound effect
            animator.SetBool(shouldOpenAnimParamater, false);
            animator.SetBool(shouldCloseAnimParamater, true);
            isOpen = false;
        }

    }

    /*
    public override void InteractWith()
    {
        if(!isOpen)
        {
            if(isLocked && !HasKey)
            {
                //play locked door sound effect here
                //jpost Audio
                PlayTryLockedDoor();
            }
            else //if not locked or player has the key
            {
                animator.SetBool(shouldOpenAnimParameter, true);
                displayText = string.Empty;
                isOpen = true;
                UnlockDoor();
                //jpost Audio
                PlayOpenDoor();
            }
            base.InteractWith(); //runs the logic from the base class         
        }
    }
    */
    private void UnlockDoor()
    {
        isLocked = false;

        if (key != null && consumesKey)
        {
            PlayerInventory.InventoryObjects.Remove(key);
            Debug.Log($"The {key} has been removed from the inventory.");
            //jpost Audio
            PlayUseKey();
        }
    }

    //jpost Audio
    private void PlayOpenDoor()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Interactibles/sx_wgh_game_int_door_open", gameObject.transform.position);
    }

    private void PlayUseKey()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Interactibles/sx_wgh_game_int_door_usekey", gameObject.transform.position);
    }

    private void PlayCloseDoor()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Interactibles/sx_wgh_game_int_door_close", gameObject.transform.position);
    }

    private void PlayTryLockedDoor()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Interactibles/sx_wgh_game_int_door_try_locked", gameObject.transform.position);
    }
}
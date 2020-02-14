using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSetActive : InteractiveObject
{
    [Tooltip("The object that gets toggled on/off.")]
    [SerializeField]
    private GameObject objectToToggle;

    [Tooltip("Can the player interact with this more than once?")]
    [SerializeField]
    private bool isReusable = true;

    private bool hasBeenUsed = false;

    /// <summary>
    /// Toggles the activeself value for the object to toggle when the player interacts with the object.
    /// </summary>
    public override void InteractWith()
    {
        if(isReusable || !hasBeenUsed)
        {
            base.InteractWith();
            objectToToggle.SetActive(!objectToToggle.activeSelf);
            hasBeenUsed = true;
            if (!isReusable)
                displayText = string.Empty;
        }
        
        
    }
}

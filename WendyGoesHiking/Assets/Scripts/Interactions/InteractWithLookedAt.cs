using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects when the player presses the interact button while looking at an IInteractive,
/// then calls the InteractWith method.
/// </summary>
public class InteractWithLookedAt : MonoBehaviour
{
    private IInteractive lookedAtInteractive;


    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.E) && lookedAtInteractive != null) || (Input.GetButtonDown("Interact") && lookedAtInteractive != null))
        {
            Debug.Log("Player pressed the interact button");
            lookedAtInteractive.InteractWith();
        }
    }

    /// <summary>
    /// Event handler for DetectLookedAtInteractive.LookedAtInteractiveChanged
    /// </summary>
    /// <param name="newLookedAtInteractive">Reference to the new IInteractive the player is looking at.</param>
    private void OnLookedAtInteractiveChanged(IInteractive newLookedAtInteractive)
    {
        lookedAtInteractive = newLookedAtInteractive;
    }

    #region Event Subscription / Unsubscription
    private void OnEnable()
    {
        DetectLookedAtInteractive.LookedAtInteractiveChanged += OnLookedAtInteractiveChanged;
    }

    private void OnDisable()
    {
        DetectLookedAtInteractive.LookedAtInteractiveChanged -= OnLookedAtInteractiveChanged;
    }
    #endregion

}

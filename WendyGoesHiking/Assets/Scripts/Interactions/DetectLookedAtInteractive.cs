using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script determines the interactive elements the player is looking at.
/// </summary>
public class DetectLookedAtInteractive : MonoBehaviour
{
    [Tooltip("This is where the raycast begins.")]
    [SerializeField]
    private Transform raycastOrigin;

    [Tooltip("How long the raycast is.")]
    [SerializeField]
    private float maxRange = 3.0f;

    /// <summary>
    /// An Event raised when a player looks at a different IInteractive
    /// </summary>
    public static event Action<IInteractive> LookedAtInteractiveChanged;

    private IInteractive lookedAtInteractive;
    public IInteractive LookedAtInteractive 
    { 
        get => lookedAtInteractive; 
        private set
        {
            bool isInteractiveChanged = value != lookedAtInteractive;
            if(isInteractiveChanged)
            {
                lookedAtInteractive = value;
                LookedAtInteractiveChanged?.Invoke(lookedAtInteractive);
            }
        }
    }

    private void FixedUpdate()
    {
        LookedAtInteractive = GetLookedAtInteractive();
    }

    /// <summary>
    /// Raycasts forward from the camera to look for IInteractives
    /// </summary>
    /// <returns>The first IInteractive detected; or null if no IInteractive detected.</returns>
    private IInteractive GetLookedAtInteractive()
    {
        Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * maxRange, Color.red);
        RaycastHit hitInfo;
        bool objectWasDetected = Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hitInfo, maxRange);

        IInteractive interactive = null;
        LookedAtInteractive = interactive;

        if (objectWasDetected)
        {
            interactive = hitInfo.collider.gameObject.GetComponent<IInteractive>();
        }
        return interactive;
    }
}

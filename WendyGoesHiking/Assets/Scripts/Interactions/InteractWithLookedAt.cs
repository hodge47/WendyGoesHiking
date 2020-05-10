using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectOutline;
/// <summary>
/// Detects when the player presses the interact button while looking at an IInteractive,
/// then calls the InteractWith method.
/// </summary>
public class InteractWithLookedAt : MonoBehaviour
{
    private IInteractive lookedAtInteractive;

    // Input
    PlayerControlActions playerControlActions;
    private ObjectOutline.Outline interactiveObjectOutline;

    private void Start()
    {
        // Initialize input
        playerControlActions = PlayerControlActions.CreateWithDefaultBindings();
        // Load the Player's bindings
        if (PlayerPrefs.HasKey("InputBindings"))
        {
            playerControlActions.Load(PlayerPrefs.GetString("InputBindings"));
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (lookedAtInteractive != null && lookedAtInteractive.InteractiveGameObject != null && lookedAtInteractive.InteractiveGameObject.GetComponent<Outline>() != null)
        {
            interactiveObjectOutline = lookedAtInteractive.InteractiveGameObject.GetComponent<Outline>();
            interactiveObjectOutline.OutlineWidth = 3f;


            if (playerControlActions.Use.IsPressed)
            {

                //Debug.Log("Player Pressed Interact Button");
                lookedAtInteractive.InteractWith();

            }
        }
        else if (interactiveObjectOutline != null)
        {
            interactiveObjectOutline.OutlineWidth = 0f;
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

    private void OnDestroy()
    {
        // Destroy the player action set
        playerControlActions.Destroy();
    }

}

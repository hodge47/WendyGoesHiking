using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This UI text displays info about the currently looked at interactive object,
/// the text should be hidden if the player is not looking at an interactive object.
/// </summary>
public class LookedAtInteractiveDisplayText : MonoBehaviour
{
    private IInteractive lookedAtInteractive;
    [SerializeField]
    private TextMeshProUGUI displayText;

    private void Awake()
    {
        
        UpdateDisplayText();
    }

    private void UpdateDisplayText()
    {
        if (lookedAtInteractive != null)
        {
            displayText.SetText(lookedAtInteractive.DisplayText);
        }
        else
        {
            displayText.SetText(string.Empty);
        }

    }

    /// <summary>
    /// Event handler for DetectLookedAtInteractive.LookedAtInteractiveChanged
    /// </summary>
    /// <param name="newLookedAtInteractive">Reference to the new IInteractive the player is looking at.</param>
    private void OnLookedAtInteractiveChanged(IInteractive newLookedAtInteractive)
    {
        lookedAtInteractive = newLookedAtInteractive;
        UpdateDisplayText();
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

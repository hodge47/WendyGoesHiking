using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour, IInteractive
{
    [Tooltip("Name of the interactive object the UI will display.")]
    [SerializeField]
    protected string displayText = nameof(InteractiveObject);
    public string DisplayText => displayText;

    public virtual void InteractWith()
    {
        Debug.Log($"Player just interacted with: {gameObject.name}");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for elements the player can interact with by pressing the interact button
/// </summary>
public interface IInteractive
{
    GameObject InteractiveGameObject
    {
        get;
    }
    string DisplayText { get; }
    void InteractWith();
}

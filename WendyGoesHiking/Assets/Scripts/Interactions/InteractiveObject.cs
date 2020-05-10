using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour, IInteractive
{
    [Tooltip("Name of the interactive object the UI will display when looked at in the world.")]
    [SerializeField]
    protected string displayText = nameof(InteractiveObject);
    [SerializeField]
    GameObject interactiveGameObject;
    public virtual string DisplayText => displayText;
    public GameObject InteractiveGameObject => interactiveGameObject;

    /// <summary>
    /// This is where audio should probably go.
    /// </summary>
    public virtual void InteractWith()
    {
        //Put audio play in here (probably)
        //I didn't implement it
        //Audio people should do it


        Debug.Log($"Player just interacted with: {gameObject.name}");
    }

}

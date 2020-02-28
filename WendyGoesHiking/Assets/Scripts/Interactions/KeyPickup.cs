using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour, IInteractive
{
    [Tooltip("Name of the interactive object the UI will display when looked at in the world.")]
    [SerializeField]
    protected string displayText = nameof(InteractiveObject);
    public virtual string DisplayText => displayText;

    [SerializeField] GameObject KeyUI;


    private void Start()
    {
        KeyUI.SetActive(false);
    }

    /// <summary>
    /// This is where audio should probably go.
    /// </summary>
    public virtual void InteractWith()
    {
        //Put audio play in here (probably)
        //I didn't implement it
        //Audio people should do it

        KeyUI.SetActive(true);

        Debug.Log($"Player just interacted with: {gameObject.name}");
        Destroy(gameObject);
    }



}

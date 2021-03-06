﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidPickup : MonoBehaviour, IInteractive
{
    [Tooltip("Name of the interactive object the UI will display when looked at in the world.")]
    [SerializeField]
    protected string displayText = nameof(InteractiveObject);
    [SerializeField]
    GameObject interactiveGameObject;
    public GameObject InteractiveGameObject => interactiveGameObject;
    public virtual string DisplayText => displayText;

    [SerializeField] GameObject FirstAidUI;


    private void Start()
    {
        FirstAidUI.SetActive(false);
    }

    /// <summary>
    /// This is where audio should probably go.
    /// </summary>
    public virtual void InteractWith()
    {
        //Put audio play in here (probably)
        //I didn't implement it
        //Audio people should do it
        //jpost Audio
        PlayHealthPackPickup();

        FirstAidUI.SetActive(true);

        Debug.Log($"Player just interacted with: {gameObject.name}");
        Destroy(gameObject);
    }

    //jpost Audio
    private void PlayHealthPackPickup()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Interactibles/sx_wgh_game_int_healthpack_pickup", gameObject.transform.position);
    }

}

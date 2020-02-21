using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;

/// <summary>
/// Increases and Decreases stamina of the player based on whether they are sprinting or not.
/// </summary>
public class StaminaSystem : MonoBehaviour
{
    /// <summary>
    /// The maximum amount of stamina the player can have at any given time.
    /// Should not be changed in code.
    /// </summary>
    public float maxStamina = 100;
    /// <summary>
    /// The minimum amount of stamina required for sprinting
    /// </summary>
    [Tooltip("The minimum amount of stamina the player should have before being able to sprint.")]
    public float minStaminaBeforeSprintingEnabled;
    /// <summary>
    /// The player's current stamina, it decreases by a set amount while the player is sprinting.
    /// It is set to maxStamina at the start of the game.
    /// </summary>
    [HideInInspector]
    public float currentStamina;
    /// <summary>
    /// The amount of stamina removed every second
    /// </summary>
    [Tooltip("How much stamina is recovered per second. Smaller the number the more is recovered.")]
    public float staminaRecoveryTime;
    /// <summary>
    /// The amount of stamina recovered every second
    /// </summary>
    [Tooltip("How much stamina is removed per second. Smaller the number the more is removed.")]
    public float staminaDecreaseTime;
    /// <summary>
    /// Bool used to tell whether the player has stamina above the minStaminaBeforeSprintingEnabled amount
    /// </summary>
    [HideInInspector]
    public bool hasEnoughStamina = true;


    private void Start()
    {
        currentStamina = maxStamina;
    }

    /// <summary>
    /// Fixed Update works better than Update()
    /// The numbers decrease more consistently.
    /// </summary>
    private void FixedUpdate()
    {
        PreventStaminaFromGoingOutofBounds();

        CheckIfPlayerHasEnoughStamina();

        IncreaseStamina();
        DecreaseStamina();

        Debug.Log($"Current stamina: {currentStamina}");
    }
    /// <summary>
    /// This makes sure the stamina doesn't go above max stamina or below zero
    /// </summary>
    private void PreventStaminaFromGoingOutofBounds()
    {
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        if (currentStamina > maxStamina)
            currentStamina = maxStamina;
        if (currentStamina < 0)
            currentStamina = 0;
    }

    /// <summary>
    /// Checks to see if the player's currentStamina is equal to or greater than <param>minStaminaBeforeSprintingEnabled</param>
    /// if it is then the player is allowed to run, if not then they are not.
    /// The variable <param>hasEnoughStamina</param> is used by the GlideController to determine whether or not the player is allowed to sprint
    /// </summary>
    private void CheckIfPlayerHasEnoughStamina()
    {
        if (currentStamina < minStaminaBeforeSprintingEnabled)
            hasEnoughStamina = false;
        if (currentStamina >= minStaminaBeforeSprintingEnabled)
            hasEnoughStamina = true;
    }

    /// <summary>
    /// Decreases the currentStamina if the sprintButton is held down
    /// currentStamina is decreased everysecond by the amount of staminaDecreaseTime * 10
    /// </summary>
    private void DecreaseStamina()
    {
        if (Input.GetKey(GlideController.current.sprintButton) && currentStamina > 0)
            currentStamina -= ((1/staminaDecreaseTime) * Time.deltaTime);
    }
    /// <summary>
    /// Increases the currentStamina if the sprintButton is not held down
    /// currentStamina is increased everysecond by the amount of staminaIncreaseTime * 10
    /// </summary>
    private void IncreaseStamina()
    {
        if (!Input.GetKey(GlideController.current.sprintButton) && currentStamina != maxStamina)
            currentStamina += ((1 / staminaDecreaseTime) * Time.deltaTime);
    }
}

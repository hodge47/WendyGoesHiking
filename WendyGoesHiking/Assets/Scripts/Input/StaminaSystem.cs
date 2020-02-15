using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks how long the player has been running for
/// </summary>
public class StaminaSystem : MonoBehaviour
{
    /// <summary>
    /// The maximum amount of stamina the player can have at any given time.
    /// Can not be changed.
    /// </summary>
    public const float maxStamina = 100;
    /// <summary>
    /// The minimum amount of stamina required for sprinting
    /// </summary>
    public float minStaminaBeforeSprintingEnabled = 10;
    /// <summary>
    /// The player's current stamina, it decreases by a set amount while the player is sprinting.
    /// </summary>
    public float currentStamina;
    /// <summary>
    /// The amount of stamina removed every second
    /// </summary>
    public float staminaReductionAmount = 1;
    /// <summary>
    /// The amount of stamina recovered every second
    /// </summary>
    public float staminaRecoveryAmount = 1;
    /// <summary>
    /// This is the amount of time that passes before the player's stamina starts to recover
    /// </summary>
    public float timeToWaitBeforeStaminaRecovers;


    public bool hasEnoughStamina = true;
    public bool IsSprinting
    {
        get;
        set;
    }


    private void Start()
    {
        currentStamina = maxStamina;
    }


    private void Update()
    {
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        CheckIfPlayerHasEnoughStamina();
        CheckIfPlayerIsSprinting();

        if (IsSprinting)
            StartCoroutine(DecreaseStamina());
        if (IsSprinting == false && currentStamina != maxStamina)
            StartCoroutine(IncreaseStamina());


        if (currentStamina == maxStamina)
            StopCoroutine(IncreaseStamina());
        if (currentStamina == 0 || IsSprinting == false)
            StopCoroutine(DecreaseStamina());

        if (currentStamina > maxStamina)
            currentStamina = maxStamina;

        Debug.Log($"Current stamina: {currentStamina}");
    }

    private void CheckIfPlayerHasEnoughStamina()
    {
        if (currentStamina < minStaminaBeforeSprintingEnabled)
            hasEnoughStamina = false;
        if (currentStamina >= minStaminaBeforeSprintingEnabled)
            hasEnoughStamina = true;
    }
    private void CheckIfPlayerIsSprinting()
    {
        if (GlideController.current.isSprinting)
            IsSprinting = true;
        if (!GlideController.current.isSprinting)
            IsSprinting = false;
    }

    IEnumerator DecreaseStamina()
    {
        if(currentStamina > 0)
        {
            yield return new WaitForSeconds(1);
            currentStamina -= staminaReductionAmount;
        }
            
    }

    IEnumerator IncreaseStamina()
    {
        yield return new WaitForSeconds(timeToWaitBeforeStaminaRecovers);
        if(currentStamina < maxStamina)
        {           
            currentStamina += staminaRecoveryAmount;
        }
            
    }
}

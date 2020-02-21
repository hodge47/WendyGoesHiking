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
    /// It is set to maxStamina at the start of the game.
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

    /// <summary>
    /// Bool used to tell whether the player has stamina above the minStaminaBeforeSprintingEnabled amount
    /// </summary>
    public bool hasEnoughStamina = true;
    /// <summary>
    /// Bool used to tell whether the player is sprinting
    /// It is set based on the GlideController
    /// </summary>
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
        RunCoroutines();
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

    private void RunCoroutines()
    {
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        CheckIfPlayerHasEnoughStamina();
        CheckIfPlayerIsSprinting();

        if (currentStamina == maxStamina)
        {
            //CancelInvoke("IncreaseStamina2");
            StopCoroutine(IncreaseStamina());
        }
        else if (currentStamina == 0 || IsSprinting == false)
        {
            //CancelInvoke("DecreaseStamina2");
            StopCoroutine(DecreaseStamina());
        }

        if (IsSprinting)
        {
            //Invoke("DecreaseStamina2", 1f);
            StartCoroutine(DecreaseStamina());
        }
        else if (IsSprinting == false && currentStamina != maxStamina)
        {
            //Invoke("IncreaseStamina2", 1f);
            StartCoroutine(IncreaseStamina());
        }



        if (currentStamina > maxStamina)
            currentStamina = maxStamina;

        Debug.Log($"Current stamina: {currentStamina}");
    }

    IEnumerator DecreaseStamina()
    {
        if(currentStamina > 0)
        {
            yield return new WaitForSeconds(1);
            currentStamina -= staminaReductionAmount;
        }
            
    }

    private void DecreaseStamina2()
    {
        if (currentStamina > 0)
        {
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

    private void IncreaseStamina2()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryAmount;
        }
    }
}

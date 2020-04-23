using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int MaxHealth = 200;
    public bool DestroyGameObjectOnDeath = false;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public bool IsAlive { get => isAlive; set => isAlive = value; }

    private int currentHealth = 0;
    private bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        // set current health to max health set in the inspector
        CurrentHealth = MaxHealth;
    }

    public void RemoveHealth(int _healthReduction)
    {
        currentHealth -= _healthReduction;
        // Check to see if the player needs to die
        if (currentHealth <= 0) Kill();
        Debug.Log($"Player health: {CurrentHealth}");
    }

    public void AddHealth(int _healthAddition)
    {
        currentHealth += _healthAddition;
    }

    private void Kill()
    {
        this.isAlive = false;
        if(DestroyGameObjectOnDeath == true)
            Destroy(this.gameObject);
    }
}

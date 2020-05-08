using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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

    public void KnockBack(Vector3 _wendigoPosition, float _force)
    {
        Vector3 _moveDirection = _wendigoPosition - this.transform.position;
        this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-_moveDirection.normalized.x, 0, _moveDirection.normalized.z) * _force);
    }

    public void KnockBack(Vector3 _wendigoPosition, float _force, ForceMode _forceMode)
    {
        Vector3 _moveDirection = _wendigoPosition - this.transform.position;
        this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-_moveDirection.normalized.x, 0, _moveDirection.normalized.z) * _force, _forceMode);
    }
    public void RemoveHealth(int _healthReduction)
    {
        currentHealth -= _healthReduction;
        // Check to see if the player needs to die
        if (currentHealth <= 0) Kill();
        //Debug.Log($"Player health: {CurrentHealth}");
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

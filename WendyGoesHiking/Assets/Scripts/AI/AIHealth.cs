using UnityEngine;
using Sirenix.OdinInspector;

public class AIHealth : MonoBehaviour
{
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    public int Health { get => health; set => health = value; }
    public bool DestroyGameObjectOnDeath { get => destroyGameObjectOnDeath; set => destroyGameObjectOnDeath = value; }

    private int health = 0;
    private bool isAlive = true;
    private bool destroyGameObjectOnDeath = false;

    public void Initialize(int _health)
    {
        Health = _health;
    }

    public void RemoveHealth(int _health)
    {
        health -= _health;
        if(health <= 0)
        {
            Kill();
        }
        Debug.Log($"AI health: {health}");
    }

    public void AddHealth(int _health)
    {
        health += _health;
    }

    private void Kill()
    {
        isAlive = false;
        if(destroyGameObjectOnDeath)
            Destroy(this.gameObject);
    }
}

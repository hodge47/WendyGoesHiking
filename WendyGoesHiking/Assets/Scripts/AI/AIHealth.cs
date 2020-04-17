using UnityEngine;
using Sirenix.OdinInspector;

public enum WendigoAliveState { ALIVE, DEAD }
public class AIHealth : MonoBehaviour
{
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    private bool isAlive = true;

    public int Health { get => health; set => health = value; }
    [SerializeField]
    [ReadOnly]
    private int health = 0;

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
        Destroy(this.gameObject);
    }
}

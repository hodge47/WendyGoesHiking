using UnityEngine;
using Sirenix.OdinInspector;

public class AIHealth : MonoBehaviour
{
    [Button(ButtonSizes.Small)]
    [DisableInEditorMode]
    private void ResurrectAI()
    {
        Health = aiManager.Health;
        IsAlive = true;
        this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        aiManager.AnimationControllerAI.SetAnimationState(WendigoAnimationState.IDLE);
    }

    public bool IsAlive { get => isAlive; set => isAlive = value; }
    public int Health { get => health; set => health = value; }
    public bool DestroyGameObjectOnDeath { get => destroyGameObjectOnDeath; set => destroyGameObjectOnDeath = value; }

    [SerializeField]
    [ReadOnly]
    private int health = 0;
    [SerializeField]
    [ReadOnly]
    private bool isAlive = true;
    private bool destroyGameObjectOnDeath = false;
    private AIManager aiManager;

    public void Initialize(AIManager _aim)
    {
        aiManager = _aim;
        Health = _aim.Health;
    }

    public void RemoveHealth(int _health)
    {
        health -= _health;
        if(health <= 0)
        {
            Kill();
        }
        //Debug.Log($"AI health: {health}");
    }

    public void AddHealth(int _health)
    {
        health += _health;
    }

    private void Kill()
    {
        isAlive = false;
        aiManager.NavMeshAgent.speed = 0;
        aiManager.NavMeshAgent.destination = GameObject.FindGameObjectWithTag("Player").transform.position;
        this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        aiManager.AnimationControllerAI.SetAnimationState(WendigoAnimationState.DEAD);
        Debug.Log("Kill AI");
        if(destroyGameObjectOnDeath)
            Destroy(this.gameObject);
    }
}

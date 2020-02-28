using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class AIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject AI;

    private AIGround groundAIScript;
    private AITreeJumping treeJumpingAIScript;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        // Get the necessary components from the AI gameObject
        groundAIScript = AI.GetComponent<AIGround>();
        treeJumpingAIScript = AI.GetComponent<AITreeJumping>();
        navMeshAgent = AI.GetComponent<NavMeshAgent>();
    }

    
}

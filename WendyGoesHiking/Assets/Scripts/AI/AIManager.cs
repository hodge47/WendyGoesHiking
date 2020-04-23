using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class AIManager : MonoBehaviour
{
    [FoldoutGroup("Health")]
    [SerializeField]
    private int health = 100;

    [FoldoutGroup("Testing")]
    [SerializeField]
    private bool isTesting;

    [FoldoutGroup("GameObjects")]
    [SerializeField]
    private GameObject AI;

    public bool AiIsAlive { get => healthAIScript.IsAlive; }

    private AIHealth healthAIScript;
    private AIGround groundAIScript;
    private AITreeJumping treeJumpingAIScript;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        // Create the health script for the AI
        healthAIScript = AI.gameObject.AddComponent<AIHealth>();
        // Get the necessary components from the AI gameObject
        groundAIScript = AI.GetComponent<AIGround>();
        treeJumpingAIScript = AI.GetComponent<AITreeJumping>();
        navMeshAgent = AI.GetComponent<NavMeshAgent>();
        // Give the AI health from variable
        healthAIScript.Initialize(health);
        // Make sure the wendigo is alive
        healthAIScript.IsAlive = true;
        // Hide the AI
        if(!isTesting)
            Invoke(nameof(HideAIOnStart), 0.5f);
    }

    public void TriggerAITreeJumping()
    {
        if (AiIsAlive == false) return;
        // Need to disable the NavMeshAgent so the AI can leave the ground
        treeJumpingAIScript.enabled = true;
        navMeshAgent.enabled = false;
        groundAIScript.enabled = false;
        
        // Tell the AI to start the tree jumping
        treeJumpingAIScript.SetUpAITreeJumping();
    }

    public void TriggerAIGroundDashing(WendigoState _wendigoState)
    {
        if (AiIsAlive == false) return;
        // Need to enable the NavMeshAganet so the AI can use the dash points around the player
        treeJumpingAIScript.enabled = false;
        navMeshAgent.enabled = true;
        groundAIScript.enabled = true;
        
        // Change the wendigo state on the ground AI
        switch(_wendigoState)
        {
            case WendigoState.PASSIVE:
                groundAIScript.wendigoState = WendigoState.PASSIVE;
                break;
            case WendigoState.AGGRESSIVE:
                groundAIScript.wendigoState = WendigoState.AGGRESSIVE;
                break;
        }
        // Tell the AI to start the ground dashing
        groundAIScript.Dash();
    }

    public void ShowAI()
    {
        if (AiIsAlive == false) return;
        AI.gameObject.SetActive(true);
    }

    public void HideAI()
    {
        if (AiIsAlive == false) return;
        AI.gameObject.SetActive(false);
    }

    private void HideAIOnStart()
    {
        if (AiIsAlive == false) return;
        AI.gameObject.SetActive(false);
    }
}

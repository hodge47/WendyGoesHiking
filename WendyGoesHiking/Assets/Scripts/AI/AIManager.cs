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
        // Hide the AI
        Invoke(nameof(HideAIOnStart), 0.5f);
    }

    public void TriggerAITreeJumping()
    {
        // Need to disable the NavMeshAgent so the AI can leave the ground
        treeJumpingAIScript.enabled = true;
        navMeshAgent.enabled = false;
        groundAIScript.enabled = false;
        
        // Tell the AI to start the tree jumping
        treeJumpingAIScript.SetUpAITreeJumping();
    }

    public void TriggerAIGroundDashing(WendigoState _wendigoState)
    {
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
        AI.gameObject.SetActive(true);
    }

    public void HideAI()
    {
        AI.gameObject.SetActive(false);
    }

    private void HideAIOnStart()
    {
        AI.gameObject.SetActive(false);
    }
}

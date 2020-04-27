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
    [FoldoutGroup("Health")]
    [SerializeField]
    private bool destroyGameObjectOnKill = false;

    [FoldoutGroup("Testing")]
    [SerializeField]
    private bool isTesting;

    [FoldoutGroup("GameObjects")]
    [SerializeField]
    private GameObject AI;

    public bool AiIsAlive { get => healthAI.IsAlive; }

    private AIHealth healthAI;
    private AIPassiveDash passiveDashAI;
    private AIAggressiveDash aggressiveDashAI;
    private AITreeJumping treeJumpingAI;
    private NavMeshAgent navMeshAgent;

    private bool activeMovement = false;

    private void Start()
    {
        // Create the health script for the AI
        healthAI = AI.gameObject.AddComponent<AIHealth>();
        // Get the necessary components from the AI gameObject
        passiveDashAI = AI.GetComponent<AIPassiveDash>();
        aggressiveDashAI = AI.GetComponent<AIAggressiveDash>();
        treeJumpingAI= AI.GetComponent<AITreeJumping>();
        navMeshAgent = AI.GetComponent<NavMeshAgent>();
        // Give the AI health from variable
        healthAI.Initialize(health);
        // Make sure the wendigo is alive
        healthAI.IsAlive = true;
        healthAI.DestroyGameObjectOnDeath = destroyGameObjectOnKill;
        // Hide the AI
        if(!isTesting)
            Invoke(nameof(HideAIOnStart), 0.5f);
    }

    private void Update()
    {
        activeMovement = CheckActiveDash();
        if (activeMovement && AI.activeSelf == false)
        {
            ShowAI();
        }
        else if(activeMovement == false && AI.activeSelf == true)
            HideAI();
    }

    private bool CheckActiveDash()
    {
        bool _dashActive = false;
        if (passiveDashAI.DashActive || aggressiveDashAI.DashActive || treeJumpingAI.HasJumpSequence)
        {
            _dashActive = true;
        }
        else
            _dashActive = false;

        return _dashActive;
    }

    public void TriggerAITreeJumping()
    {
        ShowAI();
        // Need to disable the NavMeshAgent so the AI can leave the ground
        treeJumpingAI.enabled = true;
        navMeshAgent.enabled = false;
        passiveDashAI.enabled = false;
        aggressiveDashAI.enabled = false;
        
        //if (AiIsAlive == false) return;
        
        // Tell the AI to start the tree jumping
        treeJumpingAI.SetUpAITreeJumping();
    }

    public void TriggerAIGroundDashing(WendigoState _wendigoState)
    {
        // End tree jumping if in progress
        if (treeJumpingAI.HasJumpSequence)
            treeJumpingAI.HasJumpSequence = false;
        // Need to enable the NavMeshAganet so the AI can use the dash points around the player
        treeJumpingAI.enabled = false;
        navMeshAgent.enabled = true;
        passiveDashAI.enabled = true;
        aggressiveDashAI.enabled = true;
        navMeshAgent.Warp(new Vector3(0, 2, 0));

        if (AiIsAlive == false)
        {
            return;
        }
        else
        {
            ShowAI();
        }

        // Change the wendigo state on the ground AI
        switch (_wendigoState)
        {
            case WendigoState.PASSIVE:
                //groundAI.wendigoState = WendigoState.PASSIVE;
                passiveDashAI.StartPassiveDash();
                break;
            case WendigoState.AGGRESSIVE:
                aggressiveDashAI.StartAggressiveDash();
                break;
        }
        // Tell the AI to start the ground dashing
        //groundAI.Dash();
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

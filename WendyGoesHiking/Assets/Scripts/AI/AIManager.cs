using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public enum SightingType { PASSIVE, AGGRESSIVE, BOTH}
public class AIManager : MonoBehaviour
{
    [FoldoutGroup("Health")]
    [SerializeField]
    private int health = 100;
    [FoldoutGroup("Health")]
    [SerializeField]
    private bool destroyGameObjectOnKill = false;

    [FoldoutGroup("Sightings")]
    [Tooltip("The random time range for when the wendigo shows up in minutes.")]
    [MinMaxSlider(0f, 50f, showFields: true)]
    public Vector2 SightingTimeRange = new Vector2(3f, 5f);
    [FoldoutGroup("Sightings")]
    [Tooltip("This is the type of sighting that could happen - passive, attack, or both.")]
    public SightingType SightingType = SightingType.PASSIVE;

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

    System.Random random;
    private float sightingElapsedTime = 0f;
    private float randomSightingTime = 0f;

    public static AIManager Instance { get => instance; }
    public static AIManager instance;

    private void OnGUI()
    {
        if(isTesting)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 24;
            style.normal.textColor = Color.green;
            GUI.color = Color.green;
            GUI.Label(new Rect(Screen.width - 90, Screen.height - 60, 100, 20), String.Format("{0:00.00}", sightingElapsedTime), style);
        }
    }

    // AIManager Singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("An AIManager already exists in the scene!");
            Destroy(this.gameObject);
        } 
        else
            instance = this;
    }

    private void Start()
    {
        // Create new random
        random = new System.Random();
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
        // Get random sighting time
        randomSightingTime = SetRandomTime();
        // Hide the AI
        if (!isTesting)
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

        // Need to update Random sightings
        CheckElapsedSightingTime();
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
    private float SetRandomTime()
    {
        // Get the random time in minutes from range
        float _randomTime = random.Next((int)SightingTimeRange.x, (int)SightingTimeRange.y + 1);
        // Convert to seconds
        _randomTime *= 60;

        return _randomTime;
    }

    public void AdjustRandomSightings(float _minTime, float _maxTime, SightingType _sightingType)
    {
        SightingTimeRange = new Vector2(_minTime, _maxTime);
        SightingType = _sightingType;
    }

    private void CheckElapsedSightingTime()
    {
        if(sightingElapsedTime > randomSightingTime)
        {
            // Trigger ground dashing
            switch(SightingType)
            {
                case SightingType.PASSIVE:
                    TriggerAIGroundDashing(WendigoState.PASSIVE);
                    break;
                case SightingType.AGGRESSIVE:
                    TriggerAIGroundDashing(WendigoState.AGGRESSIVE);
                    break;
                case SightingType.BOTH:
                    WendigoState _ws = (random.Next(0, 2) == 0) ? WendigoState.PASSIVE : WendigoState.AGGRESSIVE; // 0 for passive, 1 for aggressive;
                    TriggerAIGroundDashing(_ws);
                    break;
            }
            Debug.Log($"{Enum.GetName(typeof(SightingType), SightingType)} sighting was triggered...");
            // Get new random time
            randomSightingTime = SetRandomTime();
            // Set the elapsed time back to 0
            sightingElapsedTime = 0f;
        }
        else
        {
            sightingElapsedTime += Time.deltaTime;
        }
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

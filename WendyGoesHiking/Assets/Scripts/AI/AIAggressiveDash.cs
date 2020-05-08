using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class AIAggressiveDash : MonoBehaviour
{
    [Title("General")]
    [SerializeField]
    [MinMaxSlider(5f, 100f, showFields: true)]
    [Tooltip("This is the speed range [min, max] that the ground AI uses.")]
    private Vector2 dashSpeed = new Vector2(35f, 50f);
    [SerializeField]
    [Tooltip("This is the ground that the raycasting looks for. This is needed for dynamic Y-axis dash points.")]
    private LayerMask groundLayer;

    [Title("Aggressive Settings")]
    [SerializeField]
    [MinMaxSlider(5f, 40f, showFields: true)]
    [Tooltip("This is the random aggressive dash start distance range [min, max].")]
    private Vector2 aggressiveDashStartDistance = new Vector2(15f, 35f);
    [SerializeField]
    [Tooltip("This is the max offset in units that the aggressive dash uses to make sure the dash start position isn't directly in front of the player.")]
    private float maxAggressiveDashStartOffset = 10f;
    [SerializeField]
    [Tooltip("How much the AI damages the player if it comes into contact in an aggressive state")]
    private int damageAggressive = 10;

    [Title("Player Knock-back")]
    [SerializeField]
    private float forceToApplyToPlayer = 1000;
    [SerializeField]
    private bool useForceMode = false;
    [ShowIf("useForceMode")]
    [SerializeField]
    private ForceMode forceMode = ForceMode.Force;

    public bool DashActive { get => dashActive; set => dashActive = value; }
    public Vector3 DashStartPoint { get => dashStartPoint; set => dashStartPoint = value; }
    public Vector3 DashEndPoint { get => dashEndPoint; set => dashEndPoint = value; }
    public float CurrentDashSpeed { get => currentDashSpeed; }
    public bool IsRunAway { get => runAway; set => runAway = value; }

    private GameObject player;
    private PlayerHealth playerHealth;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rb;
    private bool dashActive = false;
    private bool arrivedAtDashEndPoint = false;
    [SerializeField]
    private float currentDashSpeed = 0;
    private Vector3 dashStartPoint = Vector3.zero;
    private Vector3 dashEndPoint = Vector3.zero;
    private RaycastHit raycastHit;
    private System.Random random;
    private bool runAway = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get the player
        player = GameObject.FindGameObjectWithTag("Player");
        // Get the player's health script
        playerHealth = player.GetComponent<PlayerHealth>();
        // Get the navmesh agent
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        // Get the rigidbody
        rb = this.gameObject.GetComponent<Rigidbody>();
        // Check to see if the ground layer mask is set
        int _groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        if (groundLayer.value != _groundLayerMask)
        {
            Debug.LogError("Please make sure to set the ground layer to only 'Ground' on the ground AI script.", this.gameObject);
        }
        // Set up the random class
        random = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        if(dashActive && arrivedAtDashEndPoint == false && IsRunAway == false)
        {
            navMeshAgent.destination = player.transform.position;
        }
        else if(dashActive && arrivedAtDashEndPoint == false && IsRunAway == true)
        {
            CheckRunAwayDashFinished();
        }
    }

    public void StartAggressiveDash()
    {
        if(!dashActive)
        {
            dashActive = true;
            arrivedAtDashEndPoint = false;
            // Set up the dash
            SetUpAggressiveDashPoints();
        }
    }

    // This is the aggressive state dash
    private void SetUpAggressiveDashPoints()
    {
        IsRunAway = false;
        // Get the player's trasnform
        Transform _player = GameObject.FindGameObjectWithTag("Player").transform;
        // Calculate a dash sart distance from a range
        float _dashStartPointDistance = (float)random.Next((int)aggressiveDashStartDistance.x, (int)aggressiveDashStartDistance.y + 1);
        // Get the players position and forward direction in world space
        Vector3 _playerPos = player.transform.position;
        Vector3 _playerDir = player.transform.forward;
        // Calculate a point in front of the player using player world space info and scale by dash point start distance
        Vector3 pointInFrontOfPlayer = _playerPos + _playerDir * _dashStartPointDistance;
        // Calculate an offset so dash point isn't directly in front of player but within a range in the forward direction
        float _randNegate = (random.Next(0, 2) == 0) ? 1 : -1;
        float _offset = random.Next((int)-maxAggressiveDashStartOffset, (int)maxAggressiveDashStartOffset + 1) * _randNegate;
        // Add the offset to the dash start point
        pointInFrontOfPlayer += new Vector3(_offset, 0, _offset);
        // Officially set the dash start point and assign dash end point to the player's position
        dashStartPoint = pointInFrontOfPlayer;
        dashStartPoint = CalculatePointAxisY(dashStartPoint);
        dashEndPoint = _player.position;
        dashEndPoint = CalculatePointAxisY(dashEndPoint);
        // Move the ground AI to the dash start position
        if (Physics.Raycast(new Vector3(this.transform.position.x, 1000, this.transform.position.z), Vector3.down, out raycastHit, Mathf.Infinity, groundLayer))
        {
            navMeshAgent.Warp(new Vector3(dashStartPoint.x, raycastHit.point.y, dashStartPoint.z));
        }
        else
        {
            Debug.Log("You need to assign your ground layer!");
            dashActive = false;
            return;
        }

        // Set the agent's destination to the dash end point
        navMeshAgent.destination = player.gameObject.transform.position;
        // Set the agent's speed from tge speed range
        navMeshAgent.speed = currentDashSpeed = random.Next((int)dashSpeed.x, (int)dashSpeed.y + 1);
    }

    public void RunAway()
    {
        Debug.Log("Run away");
        if(IsRunAway == false)
        {
            IsRunAway = true;
            // Get the player's trasnform
            Transform _player = GameObject.FindGameObjectWithTag("Player").transform;
            // Calculate a dash sart distance from a range
            float _dashStartPointDistance = (float)random.Next((int)aggressiveDashStartDistance.x, (int)aggressiveDashStartDistance.y + 1);
            // Get the players position and forward direction in world space
            Vector3 _playerPos = player.transform.position;
            Vector3 _playerDir = player.transform.forward;
            // Calculate a point in front of the player using player world space info and scale by dash point start distance
            Vector3 pointInFrontOfPlayer = _playerPos + _playerDir * _dashStartPointDistance;
            // Calculate an offset so dash point isn't directly in front of player but within a range in the forward direction
            float _randNegate = (random.Next(0, 2) == 0) ? 1 : -1;
            float _offset = random.Next((int)-maxAggressiveDashStartOffset * 3, (int)maxAggressiveDashStartOffset * 3 + 1) * _randNegate;
            // Add the offset to the dash start point
            pointInFrontOfPlayer += new Vector3(_offset, 0, _offset);
            // Officially set the dash start point and assign dash end point to the player's position
            dashStartPoint = this.transform.position;
            dashStartPoint = CalculatePointAxisY(dashStartPoint);
            dashEndPoint = pointInFrontOfPlayer * 3;
            dashEndPoint = CalculatePointAxisY(dashEndPoint);
            // Move the ground AI to the dash start position
            if (Physics.Raycast(new Vector3(this.transform.position.x, 1000, this.transform.position.z), Vector3.down, out raycastHit, Mathf.Infinity, groundLayer))
            {
                navMeshAgent.Warp(new Vector3(dashStartPoint.x, raycastHit.point.y, dashStartPoint.z));
            }
            else
            {
                Debug.Log("You need to assign your ground layer!");
                dashActive = false;
                return;
            }

            // Set the agent's destination to the dash end point
            navMeshAgent.destination = dashEndPoint;
            // Set the agent's speed from tge speed range
            navMeshAgent.speed = currentDashSpeed = dashSpeed.y * 3;
            // Hide after x seconds
            Invoke(nameof(HideFinishAfterSeconds), 10);
        }
    }

    private void CheckRunAwayDashFinished()
    {
        if ((this.gameObject.transform.position.x == dashEndPoint.x && this.gameObject.transform.position.z == dashEndPoint.z) || navMeshAgent.isPathStale)
        {
            dashActive = false;
            arrivedAtDashEndPoint = true;
        }
    }

    private Vector3 CalculatePointAxisY(Vector3 _point)
    {
        Vector3 _newPoint = Vector3.zero;
        if (Physics.Raycast(new Vector3(_point.x, 1000, _point.z), Vector3.down, out raycastHit, Mathf.Infinity, groundLayer))
        {
            _newPoint = new Vector3(_point.x, raycastHit.point.y, _point.z);
        }
        else
        {
            _newPoint = _point;
        }
        return _newPoint;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Debug.Log("Attacked player!", this.gameObject);
            playerHealth.RemoveHealth(damageAggressive);
            dashActive = false;
            arrivedAtDashEndPoint = true;
            // Add force to player
            if(CameraShake.Instance != null)
                CameraShake.Instance.StartShake();
            if(useForceMode)
            {
                playerHealth.KnockBack(this.transform.position.normalized, forceToApplyToPlayer, forceMode);
            }
            else
            {
                playerHealth.KnockBack(this.transform.position.normalized, forceToApplyToPlayer);
            }
        }
    }

    private void HideFinishAfterSeconds()
    {
        dashActive = false;
        arrivedAtDashEndPoint = true;
        IsRunAway = false;
    }
}

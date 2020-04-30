using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public enum WendigoState {PASSIVE, AGGRESSIVE}

public class AIGround : MonoBehaviour
{

    [Title("Dash - General")]
    [SerializeField]
    [MinMaxSlider(5f, 100f, showFields: true)]
    [Tooltip("This is the speed range [min, max] that the ground AI uses.")]
    private Vector2 dashSpeed = new Vector2(35f, 50f);
    [SerializeField]
    [Tooltip("This is the ground that the raycasting looks for. This is needed for dynamic Y-axis dash points.")]
    private LayerMask groundLayer;

    [Title("Dash - Passive Settings")]
    [SerializeField]
    [MinMaxSlider(0f, 50f, showFields: true)]
    [Tooltip("The random dash radius range [min, max] that the ground AI uses to choose it's start and end points.")]
    private Vector2 dashRadius = new Vector2(5f, 15f);
    [SerializeField]
    [Tooltip("How much the AI damages the player if it comes into contact in a passive state")]
    private int damagePassive = 5;

    [Title("Dash - Aggressive Settings")]
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

    [Title("Dash - Testing")]
    [SerializeField]
    [Tooltip("Check this to test dashing.")]
    private bool isTesting = false;

    [HideInInspector]
    public WendigoState wendigoState = WendigoState.PASSIVE;
    [HideInInspector]
    public bool arrivedAtDashEndPoint = false;

    private GameObject player;
    private PlayerHealth playerHealth;
    private NavMeshAgent agent;
    private bool dashActive = false;
    private Vector3 dashStartPoint = Vector3.zero;
    private Vector3 dashEndPoint = Vector3.zero;
    private RaycastHit raycastHit;
    private System.Random rand = new System.Random();

    [ShowIf("isTesting")]
    [DisableInEditorMode]
    [Button(ButtonSizes.Small)]
    private void TestPassiveDashPoints()
    {
        wendigoState = WendigoState.PASSIVE;
        Dash();
    }

    [ShowIf("isTesting")]
    [DisableInEditorMode]
    [Button(ButtonSizes.Small)]
    private void TestAggressiveDashPoints()
    {
        wendigoState = WendigoState.AGGRESSIVE;
        Dash();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the navMesh agent component
        agent = this.GetComponent<NavMeshAgent>();
        // Find the player object
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.gameObject.GetComponent<PlayerHealth>();

        int _groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        if(groundLayer.value != _groundLayerMask){
            Debug.LogError("Please make sure to set the ground layer to only 'Ground' on the ground AI script.", this.gameObject);
        }
    }

    private void Update()
    {
        // Check to see if a dash is active so we can see if the AI arrived at the dash end point
        if(dashActive)
        {
            if((this.gameObject.transform.position.x == dashEndPoint.x && this.gameObject.transform.position.z == dashEndPoint.z) || agent.isPathStale)
            {   
                dashActive = false;
                arrivedAtDashEndPoint = true;
                HideAI();
            }
        }
    }

    // Call a dash based on AI aggression state
    public void Dash()
    {
        dashActive = true;
        arrivedAtDashEndPoint = false;
        // See what state the wendigo is in
        switch(wendigoState)
        {
            case WendigoState.PASSIVE:
                SetUpPassiveDashPoints();
            break;
            case WendigoState.AGGRESSIVE:
                SetUpAggressiveDashPoints();
            break;
        }

        CalculatePointAxisY(Vector3.zero);
    }

    // This is the passive state dash
    private void SetUpPassiveDashPoints()
    {
        // Get the player's transform
        Transform _player = GameObject.FindGameObjectWithTag("Player").transform;
        // Calculate a dash start point randomly within a radius
        int _binary1 = rand.Next(0,2);
        int _binary2 = rand.Next(0, 2);
        int _randRadiusX = rand.Next((int)dashRadius.x, (int)dashRadius.y + 1);
        int _randRadiusY = rand.Next((int)dashRadius.x, (int)dashRadius.y + 1);
        dashStartPoint = new Vector3(_randRadiusX * ((_binary1 == 0) ? 1 : -1), 0, _randRadiusY * ((_binary2 == 0) ? 1 : -1));
        // Calculate a dash end point randomly within a radius
        int _randRadiusXX = rand.Next((int)dashRadius.x, (int)dashRadius.y + 1);
        int _randRadiusYY = rand.Next((int)dashRadius.x, (int)dashRadius.y + 1);
        dashEndPoint = new Vector3(_randRadiusXX * ((_binary1 == 0) ? -1 : 1), 0, _randRadiusYY * ((_binary2 == 0) ? -1 : 1));
        // Add player position to dash points in world space
        dashStartPoint += _player.position;
        dashStartPoint = CalculatePointAxisY(dashStartPoint);
        dashEndPoint += _player.position;
        dashEndPoint = CalculatePointAxisY(dashEndPoint);
        // Move the ground AI to the dash start position
        this.gameObject.transform.position = new Vector3(dashStartPoint.x, this.gameObject.transform.position.y, dashStartPoint.z);
        // Set the agents destination to the dash end point
        agent.destination = dashEndPoint;
        // Set the agent's speed from the speed range
        agent.speed = rand.Next((int)dashSpeed.x, (int)dashSpeed.y + 1);
    }

    // This is the aggressive state dash
    private void SetUpAggressiveDashPoints()
    {
        // Get the player's trasnform
        Transform _player = GameObject.FindGameObjectWithTag("Player").transform;
        // Calculate a dash sart distance from a range
        float _dashStartPointDistance = (float)rand.Next((int)aggressiveDashStartDistance.x, (int)aggressiveDashStartDistance.y + 1);
        // Get the players position and forward direction in world space
        Vector3 _playerPos = player.transform.position;
        Vector3 _playerDir = player.transform.forward;
        // Calculate a point in front of the player using player world space info and scale by dash point start distance
        Vector3 pointInFrontOfPlayer = _playerPos + _playerDir * _dashStartPointDistance;
        // Calculate an offset so dash point isn't directly in front of player but within a range in the forward direction
        float _randNegate = (rand.Next(0, 2) == 0) ? 1 : -1; 
        float _offset =  rand.Next((int)-maxAggressiveDashStartOffset, (int)maxAggressiveDashStartOffset + 1) * _randNegate;
        // Add the offset to the dash start point
        pointInFrontOfPlayer += new Vector3(_offset, 0, _offset);
        // Officially set the dash start point and assign dash end point to the player's position
        dashStartPoint = pointInFrontOfPlayer;
        dashStartPoint = CalculatePointAxisY(dashStartPoint);
        dashEndPoint = _player.position;
        dashEndPoint = CalculatePointAxisY(dashEndPoint);
        // Move the ground AI to the dash start position
        if(Physics.Raycast(new Vector3(this.transform.position.x, 1000, this.transform.position.z), Vector3.down, out raycastHit, Mathf.Infinity, groundLayer))
        {
             agent.Warp(new Vector3(dashStartPoint.x, raycastHit.point.y, dashStartPoint.z));
        }
        else{
            Debug.Log("You need to assign your ground layer!");
            dashActive = false;
            return;
        }
        
        // Set the agent's destination to the dash end point
        agent.destination = dashEndPoint;
        // Set the agent's speed from tge speed range
        agent.speed = rand.Next((int)dashSpeed.x, (int)dashSpeed.y + 1);
    }

    private Vector3 CalculatePointAxisY(Vector3 _point)
    {
        Vector3 _newPoint = Vector3.zero;
        if(Physics.Raycast(new Vector3(_point.x, 1000, _point.z), Vector3.down, out raycastHit, Mathf.Infinity, groundLayer))
        {
            _newPoint = new Vector3(_point.x, raycastHit.point.y, _point.z);
        }
        else
        {
            _newPoint = _point;
        }
        return _newPoint;
    }

    // This just visualizes the dash start and end points if 'isTesting' is true
    private void OnDrawGizmosSelected()
    {
        if(isTesting && Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(dashStartPoint, 0.5f);
            Gizmos.DrawSphere(dashEndPoint, 0.5f);
            Gizmos.DrawLine(dashStartPoint, dashEndPoint);
        }
    }

    private void HideAI()
    {
        this.gameObject.SetActive(false);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player" || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
    //    {
    //        Debug.Log("Attacked player!", this.gameObject);
    //        switch (wendigoState)
    //        {
    //            case WendigoState.AGGRESSIVE:
    //                playerHealth.RemoveHealth(damageAggressive);
    //                break;
    //            case WendigoState.PASSIVE:
    //                playerHealth.RemoveHealth(damagePassive);
    //                break;
    //        }
    //    }
    //}
}

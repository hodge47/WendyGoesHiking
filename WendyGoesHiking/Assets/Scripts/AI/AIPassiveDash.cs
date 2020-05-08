using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class AIPassiveDash : MonoBehaviour
{

    [Title("General")]
    [SerializeField]
    [MinMaxSlider(5f, 100f, showFields: true)]
    [Tooltip("This is the speed range [min, max] that the ground AI uses.")]
    private Vector2 dashSpeed = new Vector2(35f, 50f);
    [SerializeField]
    [Tooltip("This is the ground that the raycasting looks for. This is needed for dynamic Y-axis dash points.")]
    private LayerMask groundLayer;

    [Title("Passive Settings")]
    [SerializeField]
    [MinMaxSlider(0f, 50f, showFields: true)]
    [Tooltip("The random dash radius range [min, max] that the ground AI uses to choose it's start and end points.")]
    private Vector2 dashRadius = new Vector2(5f, 15f);
    [SerializeField]
    [Tooltip("How much the AI damages the player if it comes into contact in a passive state")]
    private int damagePassive = 5;

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

    private GameObject player;
    private PlayerHealth playerHealth;
    private NavMeshAgent navMeshAgent;
    private bool dashActive = false;
    private bool arrivedAtDashEndPoint = false;
    private float currentDashSpeed = 0;
    private Vector3 dashStartPoint = Vector3.zero;
    private Vector3 dashEndPoint = Vector3.zero;
    private RaycastHit raycastHit;
    private System.Random random;

    // Start is called before the first frame update
    void Start()
    {
        // Get the player
        player = GameObject.FindGameObjectWithTag("Player");
        // Get the player's health script
        playerHealth = player.GetComponent<PlayerHealth>();
        // Get the navmesh agent
        navMeshAgent = this.GetComponent<NavMeshAgent>();
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
        // Check to see if the passive dash finished if the dash is active
        CheckDashFinished();
    }

    private void CheckDashFinished()
    {
        if ((this.gameObject.transform.position.x == dashEndPoint.x && this.gameObject.transform.position.z == dashEndPoint.z) || navMeshAgent.isPathStale)
        {
            dashActive = false;
            arrivedAtDashEndPoint = true;
        }
    }

    public void StartPassiveDash()
    {
        if(!dashActive)
        {
            dashActive = true;
            arrivedAtDashEndPoint = false;
            // Set the dash up
            SetUpPassiveDashPoints();
        }
    }

    // This is the passive state dash
    private void SetUpPassiveDashPoints()
    {
        // Get the player's transform
        Transform _player = GameObject.FindGameObjectWithTag("Player").transform;
        // Calculate a dash start point randomly within a radius
        int _binary1 = random.Next(0, 2);
        int _binary2 = random.Next(0, 2);
        int _randRadiusX = random.Next((int)dashRadius.x, (int)dashRadius.y + 1);
        int _randRadiusY = random.Next((int)dashRadius.x, (int)dashRadius.y + 1);
        dashStartPoint = new Vector3(_randRadiusX * ((_binary1 == 0) ? 1 : -1), 0, _randRadiusY * ((_binary2 == 0) ? 1 : -1));
        // Calculate a dash end point randomly within a radius
        int _randRadiusXX = random.Next((int)dashRadius.x, (int)dashRadius.y + 1);
        int _randRadiusYY = random.Next((int)dashRadius.x, (int)dashRadius.y + 1);
        dashEndPoint = new Vector3(_randRadiusXX * ((_binary1 == 0) ? -1 : 1), 0, _randRadiusYY * ((_binary2 == 0) ? -1 : 1));
        // Add player position to dash points in world space
        dashStartPoint += _player.position;
        dashStartPoint = CalculatePointAxisY(dashStartPoint);
        dashEndPoint += _player.position;
        dashEndPoint = CalculatePointAxisY(dashEndPoint);
        // Move the ground AI to the dash start position
        this.gameObject.transform.position = new Vector3(dashStartPoint.x, this.gameObject.transform.position.y, dashStartPoint.z);
        // Set the agents destination to the dash end point
        navMeshAgent.destination = dashEndPoint;
        // Set the agent's speed from the speed range
        navMeshAgent.speed = currentDashSpeed = random.Next((int)dashSpeed.x, (int)dashSpeed.y + 1);
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
            Debug.Log("Attacked player!", this.gameObject);
            playerHealth.RemoveHealth(damagePassive);
            // Add force to player
            if (CameraShake.Instance != null)
                CameraShake.Instance.StartShake();
            if (useForceMode)
            {
                playerHealth.KnockBack(this.transform.position.normalized, forceToApplyToPlayer, forceMode);
            }
            else
            {
                playerHealth.KnockBack(this.transform.position.normalized, forceToApplyToPlayer);
            }
        }
    }
}

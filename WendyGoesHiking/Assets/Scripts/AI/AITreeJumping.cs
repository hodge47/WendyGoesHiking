using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AITreeJumping : MonoBehaviour
{
    [SerializeField]
    [ReadOnly]
    private float currentArc = 0;
    public AITree nextTree;
    public GameObject startPosition;
    public GameObject destination;
    public GameObject targetPosition;
    [MinMaxSlider(10f, 60f, showFields: true)]
    public Vector2 jumpSpeed = new Vector2(20f, 25f);
    [MinMaxSlider(5f, 35f, showFields: true)]
    public Vector2 arcHeight = new Vector2(15f, 20f);
    [MinMaxSlider(0f, 10f, showFields: true)]
    public Vector2 arrivalPauseTime = new Vector2(1f, 5f);
    public float jumpRange = 20f;

    public float maxTreeFieldRange = 30f;

    [HideInInspector]
    public AITree[] aiTrees;

    public bool isTesting;
    [ShowIf("isTesting")]
    [DisableInEditorMode]
    [Button(ButtonSizes.Small)]
    private void TestAIJumping()
    {
        SetUpAITreeJumping();
    }

    [SerializeField]
    [ReadOnly]
    private bool hasJumpSequence = false;
    private bool arrived = false;
    [SerializeField]
    [ReadOnly]
    private bool jumpPause = false;
    [SerializeField]
    [ReadOnly]
    private bool canMove = true;
    [SerializeField]
    [ReadOnly]
    private float timeSinceJumpPause = 0;
    [SerializeField]
    [ReadOnly]
    private bool hasTimeSinceArrived = false;
    [SerializeField]
    [ReadOnly]
    private bool firstArrived = false;
    [SerializeField]
    [ReadOnly]
    private float pauseTime;
    private Vector3 lastPosition = new Vector3();
    private AITree lastJumpPoint = null; // Need this to see if we are stuck

    private int numberOfJumps = 0;

    private List<AITree> closestAITrees = new List<AITree>();
    [HideInInspector]
    public List<AITree> treesIveBeenTo = new List<AITree>();

    private System.Random rand = new System.Random();

    private int randJumpSpeed = 0;
    private int randArcHeight = 0;
    private bool isStuck = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get our start position
        this.transform.position = startPosition.transform.position;
        // Set first random jump speed and arcHeight
        randJumpSpeed = rand.Next((int)jumpSpeed.x, (int)jumpSpeed.y + 1);
        randArcHeight = rand.Next((int)arcHeight.x, (int)arcHeight.y + 1);

        aiTrees = GameObject.FindObjectsOfType<AITree>();

        foreach(AITree tree in aiTrees)
        {
            if(tree.jumpPoint != startPosition){
            //Debug.Log($"Dist from {startPosition.transform.parent.gameObject.name} to {tree.gameObject.name} is: {Vector3.Distance(startPosition.gameObject.transform.position, tree.transform.position)}");

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(hasJumpSequence){
            InitiateJumpSequence();
        }
    }

    private void SetUpAITreeJumping()
    {
        isStuck = false;
        lastJumpPoint = null;
        arrived = false;
        closestAITrees.Clear();
        var player = GameObject.FindGameObjectWithTag("Player");

        foreach(AITree tree in aiTrees){
            var dist = Vector3.Distance(tree.gameObject.transform.position, player.transform.position);
            //Debug.Log(dist);
            if(dist < maxTreeFieldRange)
            {
                closestAITrees.Add(tree);
            }
        }
        // TODO: Choose randomly start and destination positions that are relatively close to the player
        
        // Define the start position randomly closest to the player
        // startPosition = closestAITrees[rand.Next(0, closestAITrees.Count)].jumpPoint;
        // this.transform.position = startPosition.transform.position;
        // //Debug.Log("Start position: " + startPosition.transform.parent.gameObject.name);
        // // Define destination based on furthest tree from the start point
        // float _fursthestDist = 0;
        // foreach(AITree tree in closestAITrees){
        //     if(tree.jumpPoint != startPosition)
        //     {
        //         var dist = Vector3.Distance(transform.gameObject.transform.position, startPosition.transform.position);
        //         //Debug.Log("Furthest tree from start:" + _fursthestDist);
        //         if(dist >= _fursthestDist){
        //             destination = tree.jumpPoint.gameObject;
        //         }
        //     }
        // }
        //Debug.Log("Destination:" + destination.transform.parent.gameObject.name);
        // Add start tree to trees I've been to so AI can't double back
        treesIveBeenTo.Add(startPosition.transform.parent.GetComponent<AITree>());
        lastJumpPoint = startPosition.transform.parent.GetComponent<AITree>();
        hasJumpSequence = true;
    }

    private void InitiateJumpSequence()
    {
         if(this.transform.position == targetPosition.transform.position){
             firstArrived = true;
             Arrived();
        }
        else{
            firstArrived = false;
            // Rotate to face the next jump position
            RotateTowardsNextJump();
            // Jump to next position
            this.transform.position = ComputeNextJumpPosition();
        }
    }

    private AITree GetNextTreeDestination()
    {
        treesIveBeenTo.Add(this.targetPosition.transform.parent.GetComponent<AITree>());
        List<AITree> nextJumpCanidates = new List<AITree>();
        foreach(AITree tree in aiTrees){
            var dist = Vector3.Distance(tree.gameObject.transform.position, this.transform.position);
            if( dist < jumpRange && dist > 1 && !treesIveBeenTo.Contains(tree))
            {
                nextJumpCanidates.Add(tree);
            }
        }

        numberOfJumps++;

        var _pickJumpCanidate = (nextJumpCanidates.Count > 0) ? nextJumpCanidates[rand.Next(0, nextJumpCanidates.Count)] : null;
        if(_pickJumpCanidate == null){
            var dist = 9999f;
            AITree _nearest = null;
            foreach(AITree tree in closestAITrees){
                if(tree.jumpPoint != nextTree){
                    var _dist = Vector3.Distance(tree.jumpPoint.transform.position, nextTree.jumpPoint.transform.position);
                    if( _dist < dist){
                        dist = _dist;
                        _nearest = tree;
                    }
                }
            }

            if(treesIveBeenTo.Contains(_nearest)){
                treesIveBeenTo.Remove(_nearest);
                _pickJumpCanidate = _nearest;
            }
        }

        return _pickJumpCanidate;
    }

    public void Arrived()
    {
        
        // Set random jump speed and arc height
        randJumpSpeed = rand.Next((int)jumpSpeed.x, (int)jumpSpeed.y + 1);
        randArcHeight = rand.Next((int)arcHeight.x, (int)arcHeight.y + 1);

        if(this.transform.position == destination.transform.position || isStuck){
            arrived = true;
            hasJumpSequence = false;
            treesIveBeenTo.Clear();
            nextTree = null;
            Debug.Log("Jump sequence completed!");
            return;
        }
        else
        {
            if(CheckIfCanMove())
            {
                
                timeSinceJumpPause = 0f;
                lastJumpPoint = nextTree;
                nextTree = GetNextTreeDestination();
                if(lastJumpPoint == nextTree){
                    isStuck = true;
                    return;
                }
                startPosition = targetPosition;
                targetPosition = nextTree.jumpPoint;  
                hasTimeSinceArrived = false;
            }

            
        }
    }

    private bool CheckIfCanMove()
    {
        bool _canMove = false;

        if(firstArrived && !hasTimeSinceArrived)
        {
            timeSinceJumpPause = Time.time;
            hasTimeSinceArrived = true;
            pauseTime = rand.Next((int)arrivalPauseTime.x, (int)arrivalPauseTime.y + 1);

        }

        // See if I can move
        if(hasTimeSinceArrived)
        {
            if(Time.time - timeSinceJumpPause > (float)pauseTime)
            {
                _canMove = true;
                timeSinceJumpPause = 0;
            }
            else{
                _canMove = false;
            }
        }
        return _canMove;
    }

    private void RotateTowardsNextJump()
    {
        var target = targetPosition.transform.position;
        var _lookPos = target - this.transform.position;
        _lookPos.y = 0;
        if(_lookPos!= Vector3.zero){
            var _rotation = Quaternion.LookRotation(_lookPos);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _rotation, Time.deltaTime * randJumpSpeed);
        }
    }

    private Vector3 ComputeNextJumpPosition()
    {
        float x0 = startPosition.transform.position.x;
        float x1 = targetPosition.transform.position.x;
        float z0 = startPosition.transform.position.z;
        float z1 = targetPosition.transform.position.z;
        float dist = Vector3.Distance(new Vector3(x1, targetPosition.transform.position.y, z1), new Vector3(x0, startPosition.transform.position.y, z0));
        float nextX = Mathf.MoveTowards(transform.position.x, x1, Time.deltaTime * jumpSpeed.x);
        float nextZ = Mathf.MoveTowards(transform.position.z, z1, Time.deltaTime * jumpSpeed.x);
        float baseY = Mathf.Abs(Mathf.Lerp(startPosition.transform.position.y, targetPosition.transform.position.y, 1));
        Vector3 moveVector = Vector3.MoveTowards(transform.position, targetPosition.transform.position, Time.deltaTime * jumpSpeed.x);
        float arc = Mathf.Abs((randArcHeight * ((nextX - x0) * (nextX - x1) - (nextZ - z0) * (nextZ - z1)) ) / (-0.25f * dist * dist));
        currentArc = arc;
        Vector3 nextPosition = new Vector3(moveVector.x, baseY + arc, moveVector.z);

        return nextPosition;
    }
}

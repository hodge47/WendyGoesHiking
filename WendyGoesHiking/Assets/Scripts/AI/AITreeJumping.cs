using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AITreeJumping : MonoBehaviour
{
    public bool canMove = false;
    [Title("Tree Jumping - General")]
    [SerializeField]
    [Tooltip("The distance a starting tree is chosen by the player.")]
    private float startPositionDistance = 30f;
    [SerializeField]
    [Tooltip("The range [min, max] of the speed the AI moves along the jump arc.")]
    [MinMaxSlider(10f, 60f, showFields: true)]
    private Vector2 jumpSpeed = new Vector2(20f, 25f);
    [SerializeField]
    [Tooltip("The range [min, max] of the max height the AI reaches during its jump.")]
    [MinMaxSlider(5f, 35f, showFields: true)]
    private Vector2 arcHeight = new Vector2(15f, 20f);    
    [SerializeField]
    [Tooltip("The max distances the AI can jump from tree to tree.")]
    private float jumpRange = 20f;
    [SerializeField]
    [Tooltip("The max radius the script looks for available trees to jump to.")]
    private float maxTreeFieldRange = 30f;
    [SerializeField]
    [Tooltip("If checked, the trees will bend when the player completes a jump.")]
    private bool bendTreeOnArrival = false;

    [HideInInspector]
    public AITree[] aiTrees;
    [HideInInspector]
    public List<AITree> treesIveBeenTo = new List<AITree>();

    [SerializeField]
    [Tooltip("Check this to test tree jumping.")]
    private bool isTesting;

    [ShowIf("isTesting")]
    [DisableInEditorMode]
    [Button(ButtonSizes.Small)]
    private void TestAIJumping()
    {
        SetUpAITreeJumping();
    }

    public AIManager AiManager { get => aiManager; set => aiManager = value; }
    public bool HasJumpSequence { get => hasJumpSequence; set => hasJumpSequence = value; }


    private AIManager aiManager;
    [SerializeField]
    private AITree nextTree;
    private GameObject startPosition;
    private GameObject destination;
    private GameObject targetPosition;
    private float currentArc = 0;
    [SerializeField]
    private bool hasJumpSequence = false;
    private bool arrived = false;
    private bool jumpPause = false;
    private bool canMovee = true;
    private float timeSinceJumpPause = 0f;
    [SerializeField]
    private bool hasTimeSinceArrived = false;
    [SerializeField]
    private bool firstArrived = false;
    private float pauseTime;
    private Vector3 lastPosition = new Vector3();
    private AITree lastJumpPoint = null; // Need this to see if we are stuck
    private int numberOfJumps = 0;
    private List<AITree> closestAITrees = new List<AITree>();
    private System.Random rand = new System.Random();
    private int randJumpSpeed = 0;
    private int randArcHeight = 0;
    private bool isStuck = false;
    private bool bentTree = false;
    private float arrivalTimePause = 0f;
    [SerializeField]
    private bool playingAnim = false;


    // Start is called before the first frame update
    public void Initialize(AIManager _aim)
    {
        // Set the AI Manager
        AiManager = _aim;
        // Set first random jump speed and arcHeight
        randJumpSpeed = rand.Next((int)jumpSpeed.x, (int)jumpSpeed.y + 1);
        randArcHeight = rand.Next((int)arcHeight.x, (int)arcHeight.y + 1);
        // Get all of the AI Trees in the scene
        aiTrees = GameObject.FindObjectsOfType<AITree>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasTimeSinceArrived)
        {
            // Set idle animation
            aiManager.AnimationControllerAI.SetAnimationState(WendigoAnimationState.IDLE);
        }
        else
        {
            // Play jump animation
            aiManager.AnimationControllerAI.SetAnimationState(WendigoAnimationState.JUMP);
        }
        // If the AI has a jump sequence, then it can plan it's next move
        if (hasJumpSequence)
        {
            InitiateJumpSequence();
        }
    }

    // This sets up the jump sequence
    public void SetUpAITreeJumping()
    {
        isStuck = false;
        lastJumpPoint = null;
        arrived = false;
        bentTree = false;
        closestAITrees.Clear();
        var player = GameObject.FindGameObjectWithTag("Player");

        // Get the closest trees to the player in the max tree field distance
        foreach(AITree tree in aiTrees){
            var dist = Vector3.Distance(tree.gameObject.transform.position, player.transform.position);
            //Debug.Log(dist);
            if(dist < maxTreeFieldRange)
            {
                closestAITrees.Add(tree);
            }
        }
        
        // Define the start position randomly closest to the player
        List<AITree> _possibleStartingPositions = new List<AITree>();
        foreach(AITree tree in aiTrees)
        {
            if(Vector3.Distance(player.transform.position, tree.transform.position) < startPositionDistance)
            {
                _possibleStartingPositions.Add(tree);
            }
        }

        if(_possibleStartingPositions.Count > 0){
            startPosition = _possibleStartingPositions[rand.Next(0, _possibleStartingPositions.Count)].jumpPoint;
        }
        else
        {
            float _leastDist = 9999;
            AITree _startTree = null;
            foreach(AITree tree in aiTrees)
            {
                float _dist = Vector3.Distance(player.transform.position, tree.transform.position);
                if(_dist < _leastDist)
                {
                    _leastDist = _dist;
                    _startTree = tree;
                    Debug.Log(_startTree.gameObject.name);
                }
            }

            startPosition = _startTree.jumpPoint;
        }

        this.transform.position = startPosition.transform.position;

        // Define the destination randomly near the player
        float _greatDist = 0;
        AITree _destTree = null;
        foreach(AITree tree in aiTrees)
        {
            if(tree != startPosition.transform.parent.GetComponent<AITree>())
            {
                float _dist = Vector3.Distance(tree.transform.position, player.transform.position);
                if(_dist > _greatDist){
                    _greatDist = _dist;
                    _destTree = tree;
                }
            }
        }
        destination = _destTree.jumpPoint;

        // Get the next jump point
        List<AITree> _possibleTargets = new List<AITree>();
        AITree _targetDest = null;
        foreach(AITree tree in aiTrees){
            if(tree.jumpPoint != startPosition && tree.jumpPoint != destination){
                _possibleTargets.Add(tree);
            }
        }
        targetPosition = _possibleTargets[rand.Next(0, _possibleTargets.Count)].jumpPoint;
        nextTree = targetPosition.transform.parent.GetComponent<AITree>();
        // Add start tree to trees I've been to so AI can't double back
        treesIveBeenTo.Add(startPosition.transform.parent.GetComponent<AITree>());
        lastJumpPoint = startPosition.transform.parent.GetComponent<AITree>();
        hasJumpSequence = true;
    }

    // This starts the jump sequence
    private void InitiateJumpSequence()
    {
        // If we are at the destination, tell the jump sequence we have arrived. If not, continue moving to the next destination
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

    // This gets the next tree destination
    private AITree GetNextTreeDestination()
    {
        // Add this tree to the trees Ive been to
        treesIveBeenTo.Add(this.targetPosition.transform.parent.GetComponent<AITree>());
        // Look for trees that I can jump to next
        List<AITree> nextJumpCanidates = new List<AITree>();
        foreach(AITree tree in aiTrees){
            var dist = Vector3.Distance(tree.gameObject.transform.position, this.transform.position);
            if( dist < jumpRange && dist > 1 && !treesIveBeenTo.Contains(tree))
            {
                nextJumpCanidates.Add(tree);
            }
        }

        numberOfJumps++;

        // Pick a tree to jump to next
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

    // This is called when the AI arrives at a tree
    public void Arrived()
    {
        
        // Set random jump speed and arc height
        randJumpSpeed = rand.Next((int)jumpSpeed.x, (int)jumpSpeed.y + 1);
        randArcHeight = rand.Next((int)arcHeight.x, (int)arcHeight.y + 1);

        //Debug.Log("IDle");

        // Bend the tree if bendTreeOnArrival is true
        if (bendTreeOnArrival && bentTree == false)
        {
            nextTree.TweenBendDeformer(1, Quaternion.Euler(new Vector3(0, this.transform.rotation.eulerAngles.y, 0)));
            bentTree = true;
        }
            
        // Complete the jump sequence if destination reached or the AI is stuck because there are no trees to jump to
        if(this.transform.position == destination.transform.position || isStuck){
            arrived = true;
            hasJumpSequence = false;
            treesIveBeenTo.Clear();
            nextTree = null;
            HideAI();
            Debug.Log("Jump sequence completed!");
            return;
        }
        else
        {
            // Move to the next tree if the jump cool down is over
            if (CheckIfCanMove())
            {
                
                timeSinceJumpPause = 0f;
                lastJumpPoint = nextTree;
                nextTree = GetNextTreeDestination();
                // Rotate the wendigo to match the root motion animation
                //this.gameObject.transform.rotation = Quaternion.LookRotation(transform.position - nextTree.gameObject.transform.position);

                if (lastJumpPoint == nextTree){
                    isStuck = true;
                    return;
                }
                startPosition = targetPosition;
                targetPosition = nextTree.jumpPoint;  
                hasTimeSinceArrived = false;
                bentTree = false;

                Debug.Log("Can move");
            }

            
        }
    }

    // This checks to see if the AI has a jump cool down
    private bool CheckIfCanMove()
    {
        bool _canMove = false;

        if(firstArrived && !hasTimeSinceArrived)
        {
            timeSinceJumpPause = Time.time;
            hasTimeSinceArrived = true;
            //Set the pause time to 60 % of the clip's length
            //if (AiManager.AnimationControllerAI.Animator.GetBool("Jump"))
            //{
            //    foreach (AnimationClip c in AiManager.AnimationControllerAI.Animator.runtimeAnimatorController.animationClips)
            //    {
            //        if (c.name == "Jump")
            //        {
            //            pauseTime = c.length * 0.6f;
            //        }
            //    }
            //}
            pauseTime = 1;
            //Debug.Log($"Arrived {pauseTime}");
        }

        // See if I can move
        if(hasTimeSinceArrived)
        {
            //Debug.Log(Time.time - timeSinceJumpPause);
            if(Time.time - timeSinceJumpPause > (float)pauseTime)// && (bendTreeOnArrival) ? lastJumpPoint.isBending == false : 1 == 1)
            {
                _canMove = true;
                timeSinceJumpPause = 0;
            }
            else{
                _canMove = false;
                this.transform.position = nextTree.jumpPoint.transform.position;
            }
        }
        canMovee = _canMove;
        return _canMove;
    }

    // This rotates the AI to face the next tree
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

    // This gets the next jump position on the tree and moves the AI along an arc
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

    private void HideAI()
    {
        this.gameObject.SetActive(false);
    }
}

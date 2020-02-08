using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AITreeJumping : MonoBehaviour
{
    public AITree nextTree;
    public GameObject startPosition;
    public GameObject destination;
    public GameObject targetPosition;
    [MinMaxSlider(10f, 60f, showFields: true)]
    public Vector2 jumpSpeed = new Vector2(20f, 25f);
    [MinMaxSlider(5f, 35f, showFields: true)]
    public Vector2 arcHeight = new Vector2(15f, 20f);
    public float jumpRange = 20f;

    public float maxTreeFieldRange = 30f;

    [ReadOnly]
    public AITree[] aiTrees;

    public bool isTesting;
    [ShowIf("isTesting")]
    [DisableInEditorMode]
    [Button(ButtonSizes.Small)]
    private void TestAIJumping()
    {
        SetUpAITreeJumping();
    }

    private bool hasJumpSequence = false;
    [ReadOnly]
    private int numberOfJumps = 0;

    [SerializeField]
    [ReadOnly]
    private List<AITree> closestAITrees = new List<AITree>();

    public List<AITree> treesIveBeenTo = new List<AITree>();

    // Start is called before the first frame update
    void Start()
    {
        // Get our start position
        this.transform.position = startPosition.transform.position;

        aiTrees = GameObject.FindObjectsOfType<AITree>();
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
        closestAITrees.Clear();
        var player = GameObject.FindGameObjectWithTag("Player");

        foreach(AITree tree in aiTrees){
            var dist = Vector3.Distance(tree.gameObject.transform.position, player.transform.position);
            Debug.Log(dist);
            if(dist < maxTreeFieldRange)
            {
                closestAITrees.Add(tree);
            }
        }
        System.Random rand = new System.Random();
        startPosition = closestAITrees[rand.Next(0, closestAITrees.Count)].jumpPoint;
        treesIveBeenTo.Add(startPosition.transform.parent.GetComponent<AITree>());
        this.transform.position = startPosition.transform.position;
        hasJumpSequence = true;
    }

    private void InitiateJumpSequence()
    {
         if(this.transform.position == targetPosition.transform.position){
             Arrived();
            
        }
        else{
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
        System.Random rand = new System.Random();

        numberOfJumps++;

        return nextJumpCanidates[rand.Next(0, nextJumpCanidates.Count)];
    }

    public void Arrived()
    {
        if(this.transform.position == destination.transform.position){
            hasJumpSequence = false;
            treesIveBeenTo.Clear();
            return;
        }
        //Debug.Log("Arrived");
        nextTree = GetNextTreeDestination();
        startPosition = targetPosition;
        targetPosition = nextTree.jumpPoint;
    }

    private void RotateTowardsNextJump()
    {
        var target = targetPosition.transform.position;
        var _lookPos = target - this.transform.position;
        _lookPos.y = 0;
        if(_lookPos!= Vector3.zero){
            var _rotation = Quaternion.LookRotation(_lookPos);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _rotation, Time.deltaTime * jumpSpeed.x);
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
        float arc = Mathf.Abs((arcHeight.x * ((nextX - x0) * (nextX - x1) - (nextZ - z0) * (nextZ - z1)) ) / (-0.25f * dist * dist));
        Vector3 nextPosition = new Vector3(moveVector.x, baseY + arc, moveVector.z);

        return nextPosition;
    }
}

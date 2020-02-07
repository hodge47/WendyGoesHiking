using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITreeJumping : MonoBehaviour
{

    public GameObject targetPosition;
    public float jumpSpeed = 3f;
    public float arcHeight = 1f;

    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Get our start position
        startPosition = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /*float x0 = startPosition.x;
        float x1 = targetPosition.transform.position.x;
        float y0 = startPosition.y;
        float y1 = targetPosition.transform.position.y;
        float z0 = startPosition.z;
        float z1 = targetPosition.transform.position.z;

        float distance = Vector3.Distance(new Vector3(x0, y0, z0), new Vector3(x1, y1, z1));

        float nextX = Mathf.MoveTowards(transform.position.x, x1, Time.deltaTime * jumpSpeed);
        float nextZ = Mathf.MoveTowards(transform.position.z, z1, Time.deltaTime * jumpSpeed);

        float baseY = Mathf.Lerp(startPosition.y, targetPosition.transform.position.y, ((nextX - x0) + (nextZ - z0) / distance));
        float arc = (arcHeight * ((nextX - x0) + (nextZ - z0)) * ((nextX - x1) + (nextZ - z1))) / (-0.25f * distance * distance);
        Arc = arc;

        Vector3 nextPosition = new Vector3(nextX, baseY + arc, nextZ);*/

        // Compute the next position
        // Rotate to face the next jump position
        RotateTowardsNextJump();
        // Jump to next position
        //this.transform.position = ComputeNextJumpPosition();
        this.transform.position = ComputeNextJumpPosition();
    }

    private void RotateTowardsNextJump()
    {
        var target = targetPosition.transform.position;
        var _lookPos = target - this.transform.position;
        _lookPos.y = 0;
        if(_lookPos!= Vector3.zero){
            var _rotation = Quaternion.LookRotation(_lookPos);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _rotation, Time.deltaTime * jumpSpeed);
        }
    }

    private Vector3 ComputeNextJumpPosition()
    {
        float x0 = startPosition.x;
        float x1 = targetPosition.transform.position.x;
        float z0 = startPosition.z;
        float z1 = targetPosition.transform.position.z;
        float dist = Vector3.Distance(new Vector3(x1, targetPosition.transform.position.y, z1), new Vector3(x0, startPosition.y, z0));
        float nextX = Mathf.MoveTowards(transform.position.x, x1, Time.deltaTime * jumpSpeed);
        float nextZ = Mathf.MoveTowards(transform.position.z, z1, Time.deltaTime * jumpSpeed);
        float baseY = Mathf.Lerp(startPosition.y, targetPosition.transform.position.y, 1);
        Vector3 moveVector = Vector3.MoveTowards(transform.position, targetPosition.transform.position, Time.deltaTime * jumpSpeed);
        float arc = (arcHeight * ((nextX - x0) * (nextX - x1) - (nextZ - z0) * (nextZ - z1)) ) / (-0.25f * dist * dist);
        Vector3 nextPosition = new Vector3(moveVector.x, baseY + arc, moveVector.z);

        return nextPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTreeJumping : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
   // public GameObject target;

    public float rotateTowardsTreeSpeed = 2f;
    public float moveSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        RotateTowardsNextJump();
        Move(end.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsNextJump();
        Move(end.transform.position);
    }

    private void RotateTowardsNextJump()
    {
        var target = end.transform.position;
        var _lookPos = target - this.transform.position;
        _lookPos.y = 0;
        var _rotation = Quaternion.LookRotation(_lookPos);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _rotation, Time.deltaTime * rotateTowardsTreeSpeed);
    }

    private void Move(Vector3 _destination)
    {
        this.transform.position = Vector3.Lerp(this.transform.position, _destination, Time.deltaTime * moveSpeed);
    }
}

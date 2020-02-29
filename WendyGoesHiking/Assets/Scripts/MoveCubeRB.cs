using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCubeRB : MonoBehaviour
{
    public Vector3 movementForce;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(movementForce, ForceMode.Force);
    }
}

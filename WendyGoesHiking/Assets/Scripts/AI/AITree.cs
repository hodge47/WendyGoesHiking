using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITree : MonoBehaviour
{

    public GameObject jumpPoint;

    // Start is called before the first frame update
    void Start()
    {
        // Get the jump point of this tree
        jumpPoint = this.transform.Find("JumpPoint").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

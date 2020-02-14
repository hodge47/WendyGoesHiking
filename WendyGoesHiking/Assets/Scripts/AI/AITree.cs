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
        if(jumpPoint == null)
        {
            Debug.LogError($"{this.gameObject.name} does not have a jump point Assigned!", this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

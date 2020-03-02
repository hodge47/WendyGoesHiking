using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    ///jpost Audio
    ///a class to check the gameobject tag of objects directly below the player
    ///

    private void FixedUpdate()
    {        
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.25f))
        {             
            if (hit.collider.tag == "Grass")
            {
                print("object tag: " + hit.collider.tag);
                
                Debug.DrawLine(transform.position, hit.point, Color.green);
            }
        }
    }
}

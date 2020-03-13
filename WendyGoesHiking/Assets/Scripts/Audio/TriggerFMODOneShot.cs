using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFMODOneShot : MonoBehaviour
{
    ///jpost Audio
    ///a class dedicated to playing an FMOD event OnTriggerEnter
    ///

    //box collider
    public BoxCollider boxCollider;


    private void Start()
    {
        //initialize boxCollider to be the gameObject's box collider
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }
}

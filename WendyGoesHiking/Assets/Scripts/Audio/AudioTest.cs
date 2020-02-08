using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check if user has pressed the f key
        if (Input.GetMouseButtonDown(0))
        {
            //if they have, play the hello world sound from FMOD
            FMODUnity.RuntimeManager.PlayOneShot("event:/sx_test_hello_world", GetComponent<Transform>().position);
            //debug
            Debug.Log("you should have heard a pop noise");
        }


    }
}

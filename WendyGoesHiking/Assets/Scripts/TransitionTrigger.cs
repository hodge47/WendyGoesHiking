﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTrigger : MonoBehaviour
{

    [SerializeField]
    GameObject BlackAndWhitePPVolume;
    [SerializeField]
    GameObject ColorPPVolume;

    
    

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            ColorPPVolume.SetActive(true);
            BlackAndWhitePPVolume.SetActive(false);

            //jpost Audio
            coll.gameObject.GetComponent<IntroMusicProgress>().ChangeToSafeZoneMusic();
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            BlackAndWhitePPVolume.SetActive(true);
            ColorPPVolume.SetActive(false);

            //jpost Audio
            coll.gameObject.GetComponent<IntroMusicProgress>().ChangeToNightMusic();
        }
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

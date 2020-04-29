﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    [SerializeField]
    GameObject objectToAnimate;

    [SerializeField]
    Transform spawnPoint;

    GameObject player;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            GlideController.current.lockMovement = true;
            GlideController.current.lockCamera = true;
            GlideController.current.playerCamera.enabled = false;
            player = coll.gameObject;
            objectToAnimate.SetActive(true);
            objectToAnimate.GetComponent<Animator>().Play("FallingCutscene", 0, 0);
            StartCoroutine(cutscenePause());

        }
    }



    IEnumerator cutscenePause()
    {
        yield return new WaitForSeconds(objectToAnimate.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        GlideController.current.GetComponentInChildren<StylizerBase>().enabled = true;
        GlideController.current.playerCamera.enabled = true;
        GlideController.current.Teleport(spawnPoint.position);
        GlideController.current.lockMovement = false;
        GlideController.current.lockCamera = false;

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

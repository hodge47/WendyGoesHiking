using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    [SerializeField]
    GameObject objectToAnimate;

    [SerializeField]
    Transform spawnPoint;

    GameObject player;

    [SerializeField]
    GameObject BlackAndWhitePPVolume;
    [SerializeField]
    GameObject IntroPPVolume;

    [SerializeField]
    Material nightTimeSkybox;

    [SerializeField]
    Light sun;

    [SerializeField]
    GameObject blackOverlay;

    [SerializeField]
    GameObject WakeUpDialogueTrigger;

    [SerializeField]
    GameObject FogOutline;
    [SerializeField]
    GameObject FogOutlineNight;



    //jpost Audio
    [SerializeField] GameObject ambienceEmitterHandler;




    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {


            ambienceEmitterHandler.GetComponent<AmbienceEmitterHandler>().ChangeFMODEvent();

            GlideController.current.lockMovement = true;
            GlideController.current.lockCamera = true;
            GlideController.current.playerCamera.enabled = false;
            player = coll.gameObject;
            objectToAnimate.SetActive(true);
            objectToAnimate.GetComponent<Animator>().Play("FallingCutscene", 0, 0);
            
            StartCoroutine(cutscenePause());

            //jpost Audio
            ChangeToNightMusic();
        }
    }



    IEnumerator cutscenePause()
    {
        yield return new WaitForSeconds(objectToAnimate.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length-0.25f);
        blackOverlay.SetActive(true);
        yield return new WaitForSeconds(2.24f);
        blackOverlay.SetActive(false);
        WakeUpDialogueTrigger.SetActive(true);
        RenderSettings.skybox = nightTimeSkybox;
        sun.intensity = 0.25f;
        IntroPPVolume.SetActive(false);
        BlackAndWhitePPVolume.SetActive(true);
        objectToAnimate.SetActive(false);
        FogOutline.SetActive(false);
        FogOutlineNight.SetActive(true);
        GlideController.current.playerCamera.enabled = true;
        GlideController.current.Teleport(spawnPoint.position);
        GlideController.current.sprintMode = GlideController.GlideSprintSetting.normal;
        GlideController.current.lockMovement = false;
        GlideController.current.lockCamera = false;

    }


    //jpost Audio
    private void ChangeToNightMusic()
    {
        player.GetComponent<IntroMusicProgress>().ChangeToNightMusic();
    }


    // Update is called once per frame
    void Update()
    {

    }
}

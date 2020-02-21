using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS IS A CLASS SYSTEM YOU DON'T NEED TO WORRY ABOUT! IT'S INTERNALLY BUILT FOR THE PLAYER SYSTEM TO ALLOW SOUNDS TO PLAY WITHOUT INTERRUPTING ONE ANOTHER.

public class AudioInstance : MonoBehaviour {

    public bool begunPlaying = false;

	// Update is called once per frame
	void Update () {

        if (!begunPlaying)
        {
            if (gameObject.GetComponent<AudioSource>().isPlaying)
            {
                begunPlaying = true;
            }
        }
        else
        {
            if(!gameObject.GetComponent<AudioSource>().isPlaying)
            {
                KillInstance();
            }
        }

	}

    void KillInstance()
    {
        begunPlaying = false;
        gameObject.GetComponent<AudioSource>().clip = null;
        gameObject.GetComponent<AudioSource>().volume = 1f;
        gameObject.GetComponent<AudioSource>().pitch = 1f;
        gameObject.SetActive(false);
    }
}

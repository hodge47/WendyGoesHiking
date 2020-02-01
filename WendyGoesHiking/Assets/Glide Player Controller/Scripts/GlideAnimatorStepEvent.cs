using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlideAnimatorStepEvent : MonoBehaviour
{
    public void PlayStepSound()
    {
        AudioManager.PlaySound(GlideController.current.walkSounds[Random.Range(0,GlideController.current.walkSounds.Length)]);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerJumpAnim : MonoBehaviour
{

    public Animator animator;

    [SerializeField]
    bool playingAnim = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playingAnim = animator.GetCurrentAnimatorStateInfo(0).IsName("Jump");
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(playingAnim == false)
            {
                animator.SetTrigger("Jump");
                animator.SetBool("Idle", false);
                Invoke(nameof(Reset), 5f);
            }
        }
    }

    private void Reset()
    {
        animator.SetBool("Idle", true);
    }
}

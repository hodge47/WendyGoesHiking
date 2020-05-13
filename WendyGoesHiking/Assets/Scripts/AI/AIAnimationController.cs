using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WendigoAnimationState { IDLE, CRAWL, JUMP, AGONY, DEAD}

public class AIAnimationController : MonoBehaviour
{
    public Animator Animator { get => animator; }
    public WendigoAnimationState AnimationState { get => animationState; set => animationState = value; }

    private Animator animator;
    [SerializeField]
    private WendigoAnimationState animationState = WendigoAnimationState.IDLE;

    private void Start()
    {
        // Get the animator
        animator = this.gameObject.GetComponentInChildren<Animator>();
    }

    public void SetAnimationState(WendigoAnimationState _animationState)
    {
        switch(_animationState)
        {
            case WendigoAnimationState.IDLE:
                AnimationState = WendigoAnimationState.IDLE;
                break;
            case WendigoAnimationState.CRAWL:
                AnimationState = WendigoAnimationState.CRAWL;
                break;
            case WendigoAnimationState.JUMP:
                AnimationState = WendigoAnimationState.JUMP;
                break;
            case WendigoAnimationState.AGONY:
                AnimationState = WendigoAnimationState.AGONY;
                break;
            case WendigoAnimationState.DEAD:
                AnimationState = WendigoAnimationState.DEAD;
                break;
            default:
                AnimationState = WendigoAnimationState.IDLE;
                break;
        }
        SetAnimatorParameter(AnimationState);
    }

    private void SetAnimatorParameter(WendigoAnimationState _animationState)
    {
        switch (_animationState)
        {
            case WendigoAnimationState.IDLE:
                animator.SetBool("Idle", true);
                animator.SetBool("Crawl", false);
                animator.SetBool("Jump", false);
                animator.SetBool("Agony", false);
                animator.SetBool("Death", false);
                Debug.Log("Idle");
                break;
            case WendigoAnimationState.CRAWL:
                animator.SetBool("Crawl", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Jump", false);
                animator.SetBool("Agony", false);
                animator.SetBool("Death", false);
                break;
            case WendigoAnimationState.JUMP:
                animator.SetBool("Jump", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Crawl", false);
                animator.SetBool("Agony", false);
                animator.SetBool("Death", false);
                Debug.Log("Jump");
                break;
            case WendigoAnimationState.AGONY:
                animator.SetBool("Agony", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Crawl", false);
                animator.SetBool("Jump", false);
                animator.SetBool("Death", false);
                break;
            case WendigoAnimationState.DEAD:
                animator.SetBool("Idle", false);
                animator.SetBool("Crawl", false);
                animator.SetBool("Jump", false);
                animator.SetBool("Agony", false);
                animator.SetBool("Death", true);
                break;
            default:
                animator.SetBool("Idle", true);
                animator.SetBool("Crawl", false);
                animator.SetBool("Jump", false);
                animator.SetBool("Agony", false);
                animator.SetBool("Death", false);
                break;
        }
    }
}

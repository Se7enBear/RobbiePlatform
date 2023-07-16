using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement movement;
    private Rigidbody2D rb;
    private void Start()
    {
        movement=GetComponentInParent<PlayerMovement>();
        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
    }
    private void Update()
    {
        anim.SetFloat("speed",Mathf.Abs(movement.xVelocity));
        anim.SetBool("isCrouching",movement.isCrouch);
        anim.SetBool("isOnGround", movement.isGround);
        anim.SetBool("isHanging", movement.isHanging);
        anim.SetFloat("verticalVelocity", rb.velocity.y);

    }
    public void StepAudio()
    {
        AudioManager.PlayFootstepAudio();
    }
    public void CrouchStepAudio()
    {
        AudioManager.PlayCrouchFootstepAudio();
    }
}

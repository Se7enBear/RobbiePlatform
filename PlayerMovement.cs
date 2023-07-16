using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll; 
    [Header("移动参数")]
    public float speed;
    private float crouchSpeedDivisor;
    public float xVelocity;
    [Header("跳跃参数")]
    private float jumpForce = 6.3f;
    private float jumpHoldForce = 1.9f;
    private float jumpHoldDuration = 0.1f;
    private float crouchJumpBoost = 2.5f;
    private float hangingJumpForce = 15f;
    float jumpTime;
    [Header("状态")]
    public bool isCrouch;
    public bool isJump;
    public bool isGround;
    public bool isHeadBlocked;
    public bool isHanging;
    [Header("环境检测")]
    public LayerMask groundLayer;
    private float footOffset = 0.4f;
    private float headClearance=0.5f;
    private float groundDistance = 0.2f;
    private float playerHeight;
    private float eyeHeight = 1.5f;
    private float grabDistance = 0.4f;
    private float reachOffset = 0.7f;
     
    //
    bool jumpPressed;
    bool jumpHeld;
    bool crouchHeld;
    bool crouchPressed;
    //
    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        crouchSpeedDivisor = 4f;
        coll = GetComponent<BoxCollider2D>();
        colliderStandSize = coll.size;
        colliderStandOffset=coll.offset;
        colliderCrouchSize=new Vector2(coll.size.x, coll.size.y/2f);
        colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y / 2f);
        playerHeight = coll.size.y;
    }

    private void FixedUpdate()
    {
        if (GameManager.GameOver()) return;
        PhysicsCheck();
        GroundMovement();
        MidAirMovement();

    }
    private void Update()
    {
        if (GameManager.GameOver()) return;
        if(!jumpPressed) { jumpPressed = Input.GetButtonDown("Jump"); }
      
        jumpHeld = Input.GetButton("Jump");
        if (!crouchPressed) { crouchPressed = Input.GetButtonDown("Crouch"); }
        crouchHeld = Input.GetButton("Crouch");

    }

    void MidAirMovement()
    {
        if (isHanging)
        {
            if (jumpPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);
                isHanging = false;
                jumpPressed=false;
            }
            if(crouchHeld)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                isHanging=false;
                crouchPressed=false;
            }
        }

        if (jumpPressed && isGround && !isJump&&!isHeadBlocked)
        {
            if (isCrouch && isGround)
            {
                Standup();
                rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }
            isGround = false;
            isJump = true;
            jumpTime=Time.time+jumpHoldDuration;

            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumpPressed = false;
            AudioManager.PlayJumpAudio();
        }

        else if (isJump)
        {
            if (jumpHeld)
            {
                rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            }
            if (jumpTime < Time.time)
                isJump = false;
        }
    }
    private void GroundMovement()
    {
        if (isHanging) return;
        if (crouchHeld&&!isCrouch&&isGround)
        {
            Crouch();
        }
        else if (!crouchHeld&&isCrouch&&!isHeadBlocked)
        {
            Standup();
        }
        else if(!isGround&&isCrouch) { Standup(); }
       
        xVelocity = Input.GetAxis("Horizontal");
        if(isCrouch)
        {
            xVelocity /= crouchSpeedDivisor;
        }

        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);
        FilpDirction();
    }

    void PhysicsCheck()
    {
     
        RaycastHit2D leftCheck =Raycast(new Vector2(-footOffset,0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        if (leftCheck||rightCheck) { isGround = true; }
        else isGround=false;

        RaycastHit2D headCheck = Raycast(new Vector2(0f, coll.size.y), Vector2.up, headClearance, groundLayer);
        if(headCheck) { isHeadBlocked = true;}
        else isHeadBlocked=false;

        float direction = transform.localScale.x;
        Vector2 grabDir = new Vector2(direction, 0f);
        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset*direction,playerHeight),grabDir,grabDistance,groundLayer);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance, groundLayer);
        RaycastHit2D ledgeCheck= Raycast(new Vector2(reachOffset*direction,playerHeight),Vector2.down,grabDistance,groundLayer);

        if(!isGround&&rb.velocity.y<0&&ledgeCheck&&wallCheck&&!blockedCheck) {
            Vector3 pos = transform.position;
            pos.x +=( wallCheck.distance-0.05f) * direction;
            pos.y -= ledgeCheck.distance;
            transform.position = pos;
            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }

    }
    void FilpDirction()
    {
        if(xVelocity<0) { transform.localScale=new Vector3(-1,1,1); }
        if (xVelocity > 0) { transform.localScale = new Vector3(1, 1,1); }
    }
    void Crouch()
    {
        isCrouch = true;
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }
    void Standup()
    {
        isCrouch = false;
        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
    }
    RaycastHit2D Raycast(Vector2 offset,Vector2 rayDiraction,float length,LayerMask layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset,  rayDiraction*length, color);
        return hit;
    }

}

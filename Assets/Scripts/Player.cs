using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Rigidbody2D body;
    protected BoxCollider2D boxCollider;
    protected SpriteRenderer sprite;
    protected Animator animator;

    public float jumpSpeed;
    public float groundSpeed;
    public float airSpeed;
    public float bufferGroundedTime;

    private float timerBufferGrounded;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        if (animator != null)
            animator.logWarnings = false;
    }

    private bool isGrounded;
    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
        protected set
        {
            isGrounded = value;
        }
    }

    protected bool isJumping;
    protected bool IsJumping
    {
        get
        {
            return isJumping;
        }
        set
        {
            isJumping = value;

            if (value)
            {
                body.velocity = new Vector2(body.velocity.x, jumpSpeed);
                IsGrounded = false;

                if (animator != null)
                    animator.SetTrigger("Jumped");
            }
            else if (body.velocity.y > 0)
            {
                body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
            }

        }
    }

    public bool Jump()
    {
        if (timerBufferGrounded > 0 || IsGrounded)
        {
            IsJumping = true;
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

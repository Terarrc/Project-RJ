using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D body;
    public Vector2 groundSpeed;
    public Vector2 airSpeed;      //not sure if I'll use it for now
    public Vector2 acceleration;
    public float jumpSpeed;
    public float bufferGroundedTime;
    public Rocket rocket;

    protected CapsuleCollider2D capsuleCollider;
    protected SpriteRenderer sprite;
    protected Animator animator;

    private bool verticalMovement;
    private Camera cam;   
    private Vector3 origin;
    private Vector3 direction;
    private float timerBufferGrounded;

    private void Start()
    {
        cam = Camera.main;
    }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
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
                    animator.SetTrigger("Jump");
            }
            else if (body.velocity.y > 0)
            {
                body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
            }
        }
    }

    public bool Jump()
    {
        if (isGrounded)
        {
            IsJumping = true;
            return true;
        }

        return false;
    }

    public bool StopJump()
    {
        return false;
    }

    public bool Move(Vector2 input)
    {
        if (!verticalMovement && isGrounded)
            body.velocity = new Vector2(Mathf.MoveTowards(body.velocity.x, groundSpeed.x * input.x, acceleration.x * Time.deltaTime), body.velocity.y);
        else if(isGrounded)
            body.velocity = new Vector2(Mathf.MoveTowards(body.velocity.x, groundSpeed.x * input.x, acceleration.x * Time.deltaTime), Mathf.MoveTowards(body.velocity.y, groundSpeed.y * input.y, acceleration.y * Time.deltaTime));

        if (input.x != 0)
            sprite.flipX = input.x > 0 ? false : true;

        return true;
    }

    public bool ApplyKnockback(Collider2D collision)
    {
        Vector3 hitDirection = collision.transform.position - transform.position;
        float force = 12;

        body.velocity = -hitDirection.normalized * force;
        //body.AddForce(-hitDirection.normalized * force, ForceMode2D.Force);

        return false;
    }

    public bool LaunchRocket()
    {
        Rocket r = Instantiate(rocket, body.transform.position, Quaternion.identity);
        r.Init(direction);
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePoint = new Vector3();
        Vector2 mousePos = new Vector2();

        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.
        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;
        mousePoint = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

        // Coordinate of the mouse relative to the player
        origin = body.position;
        direction = mousePoint - origin;

        Debug.DrawRay(origin, direction);
    }

    void FixedUpdate()
    {
        isGrounded = false;
    }

    protected void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Ground")))
        {
            ColliderDistance2D colliderDistance = collision.collider.Distance(capsuleCollider);

            float angle = Vector2.Angle(colliderDistance.normal, Vector2.up);

            // Check if the collision is less than 50° with the vertical, and check if the collision is done from the bottom
            if ((angle < 50) && (collision.GetContact(0).point.y - body.position.y < 0))
            {
                isGrounded = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            ApplyKnockback(collision);
        }
    }

}

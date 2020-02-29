using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Rigidbody2D body;
    public Explosion explosion;
    public float speed;
    
    private Player player;
    private Camera cam;
    private Collider2D coll;
    private SpriteRenderer sprite;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    public void Init(Vector3 direction)
    {
        // Apply speed
        direction.z = 0;
        body.velocity = direction.normalized * speed;

        // Rotate object
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Make the rocket explode on surface contact
    void Explode(Vector3 contactPoint)
    {
        // Create and explosion
        Explosion e = Instantiate(explosion, contactPoint, Quaternion.identity);

        //Destroy this rocket
        Destroy(transform.root.gameObject);
        
        //player side :
        //point of collision with the player 
        //distance from the center point : the further away, the less force.
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.15f, 1 << LayerMask.NameToLayer("Ground")))
        {
            Explode(transform.position);
        }

        if (Physics2D.OverlapCircle(transform.position, 0.15f, 1 << LayerMask.NameToLayer("Wall")))
        {
            Explode(transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            
        }
    }
}

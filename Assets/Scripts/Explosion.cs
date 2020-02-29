using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radiusStart;
    public float radiusEnd;
    public float lifeSpan;

    private ParticleSystem partSys;
    private float animationDuration;
    private float hitboxtimer;
    private float destroyTimer;
    private float incr;
    private CircleCollider2D coll;

    private void Awake()
    {
        partSys = GetComponent<ParticleSystem>();
        coll = GetComponent<CircleCollider2D>();
        coll.radius = radiusStart;

        hitboxtimer = 0;
        destroyTimer = 0;
        incr = (radiusEnd - radiusStart) / (lifeSpan);
        animationDuration = partSys.main.duration;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {      
        if(destroyTimer >= animationDuration)
        {
            Destroy(transform.root.gameObject);
        }
        else
        {
            destroyTimer += Time.deltaTime;
        }

        if (hitboxtimer >= lifeSpan)
        {
            Destroy(coll);
        }
        else
        {
            hitboxtimer += Time.deltaTime;

            if(coll.radius < radiusEnd)
            {               
                coll.radius = (radiusEnd - radiusStart) * hitboxtimer / lifeSpan;
            }          
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public float friction, maxSpeed;
    public float speed=1;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Roll(Vector3 direction)
    {
        rb.velocity += direction * speed;
        if (rb.velocity.sqrMagnitude > 0)
        {
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
            rb.velocity -= rb.velocity.normalized * friction;

            if (rb.velocity.sqrMagnitude <= 0.25f * friction * friction)
            {
                rb.velocity = Vector3.zero;
            }

        }
    }

    /*
    protected virtual void ChangeColor(Color c)
    {
        Debug.LogFormat("{0}改变颜色为{1}",gameObject.name, c);

        DestroySelf();

        if(c==Color.red)
        {
            Friend.GenerateSelf(transform.position);
        }
        else if (c == Color.grey)
        {
            Hostage.GenerateSelf(transform.position);
        }
        else
        if (c == Color.red)
        {
            Enemy.GenerateSelf(transform.position);
        }
    }
    
    protected virtual void DestroySelf()
    {
        
    }
    */
}

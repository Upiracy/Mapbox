using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float friction,maxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb= transform.GetComponent<Rigidbody>();
        rb.velocity -= new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (rb.velocity.sqrMagnitude > 0)
        {
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
            rb.velocity -= rb.velocity.normalized * friction;

            if(rb.velocity.sqrMagnitude <= 0.25f * friction * friction)
            {
                rb.velocity = Vector3.zero;
            }
            
        }
        
    }
}

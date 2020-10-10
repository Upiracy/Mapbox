using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Ball
{
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

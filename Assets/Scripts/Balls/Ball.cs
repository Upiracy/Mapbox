using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public float friction,f, maxSpeed;
    public float speed=1;
    public List<Vector3> vecs;
    [SerializeField] float reboundForce = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPath(List<Vector3> path)
    {
        vecs = path;
       // Debug.Log(vecs.Count);
    }

    public void Roll(Vector3 direction)
    {
        rb.freezeRotation = false;
        if((rb.velocity + direction * speed).magnitude < maxSpeed) rb.velocity += direction * speed;
        if (rb.velocity.sqrMagnitude > 0)
        {
            /*
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity -= rb.velocity.normalized * friction;
            }
            */
            rb.velocity -= rb.velocity.normalized * friction;
            rb.velocity -= rb.velocity.sqrMagnitude * rb.velocity.normalized * f;

            if (rb.velocity.sqrMagnitude <= 0.25f * friction * friction)
            {
                rb.velocity = Vector3.zero;
                
            }
            if (rb.velocity == Vector3.zero)
            {
                rb.freezeRotation = true;
                //gameObject.transform.r
            }
            else
            {
                rb.freezeRotation = false;
            }

        }
    }

    protected void Rebound(Collision co)
    {
        //Debug.Log(co.collider.gameObject.name + ";;;;;;;" + (co.collider.GetComponent<Rigidbody>() == null).ToString());
        Vector3 vself = rb.velocity, vother = co.collider.GetComponent<Rigidbody>().velocity, normal = co.contacts[0].normal;
        rb.velocity += normal * reboundForce * Vector3.Project(vother, normal).magnitude;
    }

    protected void CollideWall(Collision co)
    {
        Vector3 vself = co.relativeVelocity, normal = co.contacts[0].normal;
        rb.velocity += normal * reboundForce * Vector3.Project(vself, -normal).magnitude*0.6f;
     //   if(gameObject.name=="PlayerBall")
       // Debug.LogFormat("{0},{1},最后速度{2}",vself,normal,normal * reboundForce * Vector3.Project(vself, -normal).magnitude * 1f);
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

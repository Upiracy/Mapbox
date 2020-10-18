using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] Ball playerBall;
    [SerializeField] JoyStick joyStick;
    [SerializeField] float maxAngle;
    Rigidbody rb;
    public  float maxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = playerBall.GetComponent<Rigidbody>();
        maxSpeed = playerBall.GetComponent<Player>().maxSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 a = new Vector3(joyStick.deltaPos.x, 0, joyStick.deltaPos.y);
        

        playerBall.Roll(Step(rb.velocity,a).normalized*a.magnitude);
    }

    Vector3 Step(Vector3 from, Vector3 to)
    {
        float angle = Vector3.Angle(from, to);
        return Vector3.Slerp(from, to, maxAngle / angle);
    }
    
}

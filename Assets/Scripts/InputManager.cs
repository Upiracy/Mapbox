using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] Rigidbody playerBall;
    [SerializeField] JoyStick joyStick;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerBall.velocity += new Vector3(joyStick.deltaPos.x, 0, joyStick.deltaPos.y)*speed;
    }
}

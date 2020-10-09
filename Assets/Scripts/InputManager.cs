﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] Ball playerBall;
    [SerializeField] JoyStick joyStick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerBall.Roll(new Vector3(joyStick.deltaPos.x, 0, joyStick.deltaPos.y));
    }
}

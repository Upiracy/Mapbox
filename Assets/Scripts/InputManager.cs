using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] Ball playerBall;
    [SerializeField] JoyStick joyStick;
    public float maxAngle;
    Rigidbody rb;

    void Start()
    {
        rb = playerBall.GetComponent<Rigidbody>();
        //maxSpeed = playerBall.GetComponent<Player>().maxSpeed;
    }

    /// <summary>
    /// 获取当前摇杆加速度方向，作为球滚动函数的参数。
    /// </summary>
    void Update()
    {
        Vector3 a = new Vector3(joyStick.deltaPos.x, 0, joyStick.deltaPos.y);
        playerBall.Roll(Step(rb.velocity,a).normalized*a.magnitude);
    }

    /// <summary>
    /// 限定速度改变的角度，对速度方向和加速度方向做差值
    /// </summary>
    /// <param name="from">当前速度方向</param>
    /// <param name="to">加速度方向</param>
    /// <returns></returns>
    Vector3 Step(Vector3 from, Vector3 to)
    {
        float angle = Vector3.Angle(from, to);
        return Vector3.Slerp(from, to, maxAngle / angle);
    }
}

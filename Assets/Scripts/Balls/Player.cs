using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Ball
{  
    public int state = 1; //123对应小中大

    // Start is called before the first frame update
    void Start()
    {
        state = 1;
        rb = transform.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, 0);
    }


    // Update is called once per frame
    void Update()
    {
        
        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "SmallBlackBall" && state == 1) ||
             collision.gameObject.tag == "Boss" )
        {
            //主角变灰
            DestroySelf();

            
        }
    }

    protected void DestroySelf()
    {
        GetComponent<MeshRenderer>().materials[0].color = Color.grey;

        //游戏失败，调用GameManger的函数
    }
}

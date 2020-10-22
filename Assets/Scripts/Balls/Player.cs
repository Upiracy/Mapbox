using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Ball
{
    //[SerializeField] int HP = 1;
    private bool hasCollided = true;
    public int state = 1; //123对应小中大
    GameManager gm;
    public bool union = false;

    // Start is called before the first frame update
    void Start()
    {
        state = 1; 
        rb = transform.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, 0);
        gm = GameObject.Find("Manager").GetComponent<GameManager>();
    }


    private void LateUpdate()
    {
        hasCollided = false;
      //  Debug.Log("主角" + rb.velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;

        if ((collision.gameObject.tag == "SmallBlackBall" && state == 1) ||
            (collision.gameObject.tag == "SmallBlackBall" && collision.transform.GetComponent<Enemy>().fast) ||
             collision.gameObject.tag == "Boss" ||
             collision.gameObject.tag == "Bullet")
        {
            //主角变灰
            Hurt();

            
        }

        if(collision.gameObject.tag == "Boss" || collision.gameObject.tag == "SmallBlackBall" || collision.gameObject.tag == "RedBall" || collision.gameObject.tag == "GreyBall")
        {
            Rebound(collision);
            //Debug.Log(collision.gameObject.tag);
            hasCollided = true;
        }

        if (collision.gameObject.layer == 10)
        {
            CollideWall(collision);
            //GameObject.Find("JoyStick").GetComponent<JoyStick>().ResetPos();
            //Debug.Log("撞墙");
            hasCollided = true;
        }
    }

    protected void Hurt()
    {
        if(union)
        {
            //散开,红球中一个变灰
            gm.DivideRedBalls();
            
        }
        else
        {
            GetComponent<MeshRenderer>().materials[0].color = Color.grey;

            //游戏失败，调用GameManger的函数
        }

    }

}

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

        //小主角（一阶段）碰到大黑，小黑，小黑子弹均死亡
        if (state == 1)
        {
            if (collision.gameObject.tag == "SmallBlackBall" ||
              collision.gameObject.tag == "Boss" ||
              collision.gameObject.tag == "Bullet")
            {
                //主角变灰
                DestroySelf();
            }
        }
        //合体时
        else if (union)
        {
            if (collision.gameObject.tag == "Boss")
            {
                //体积小于boss
                if(transform.localScale.x<collision.transform.localScale.x)
                {
                    int n = 2;
                    gm.DivideRedBalls(n);
                }
                //体积大于boss
                else
                {
                    gm.DivideRedBalls(0);
                }
            }
            //遇到范围内小黑
            else if (collision.gameObject.tag == "SmallBlackBall" && collision.transform.GetComponent<Enemy>().fast)
            {
                gm.DivideRedBalls(1);
            }
            //遇到小黑球子弹
            else if(collision.gameObject.tag == "Bullet")
            {
                int x = 2;
                gm.DivideRedBalls(x);
            }
        }

        if(collision.gameObject.tag == "Boss" || collision.gameObject.tag == "SmallBlackBall" || collision.gameObject.tag == "RedBall" || collision.gameObject.tag == "GreyBall")
        {
            Rebound(collision);
            hasCollided = true;
        }

        if (collision.gameObject.layer == 10)
        {
            CollideWall(collision);
            hasCollided = true;
        }
    }

    protected void Hurt()
    {
        if(union)
        {
            Debug.Log(" 散开,红球中一个变灰");
            gm.DivideRedBalls(1);
            
        }
        else
        {
            
        }

    }
    protected void DestroySelf()
    {
        GetComponent<MeshRenderer>().materials[0].color = Color.grey;

        //游戏失败，调用GameManger的函数
        gm.PlayerLost();
    }
}

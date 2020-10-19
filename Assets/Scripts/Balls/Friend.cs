using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : Ball
{
    private bool hasCollided = false;
    private static Stack<GameObject> friendPool = new Stack<GameObject>();
    public static List<Friend> redBalls = new List<Friend>();
    GameManager gm;
    #region 权重解释
    /*
     1趋向于远离周围红球
Reds[i].weight1 归零
For i
For j
Vector3 delta = (redball[i].transform.position - redball[j].transform.position);
Float dis = 1 / delta.magnitude
Reds[i].weight1 += dis * delta.normalized;
Reds[j].weight1 += dis * -delta.normalized;
For i
Reds[i]. weight1 /= Reds.Length;
Reds[i].tranform.position
速度归一化加起来

2 趋向于靠近所有红球中心
3 趋向于朝向红球群的平均方向
每帧都遍历所有红球

4 越靠近主角越趋向于接近灰球
5 越远离主角越趋向于接近主角与主角的距离
For i in red
Reds[i]与player距离
For j in grey
Red[i] 与 grey[j]距离
得到Red[i]与谁最近
Weight4 = (Grey[j] - Red[i] ) *( 1/与player距离)
Weight5 = Reds[i]与player距离


6 越靠近墙越越趋向于远离墙
暂时先给一个刹车的权重
Spherecast找墙面
遇到墙 权重方向为墙法线方向
     */
    #endregion
    public Vector3 factor1, factor2, factor3, factor4, factor5, factor6,factor7;
    [SerializeField] float weight1=1, weight2=1, weight3=1, weight4=1, weight5=1, weight6=1,weight7=1;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        friendPool.Clear();
        gm = GameObject.Find("Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Roll(weight1 * factor1 + weight2 * factor2 + weight3 * factor3 + weight4 * factor4 + weight5 * factor5 + weight6 * factor6 + weight7 * factor7);
    }
    private void LateUpdate()
    {
        hasCollided = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;

        if (collision.gameObject.tag == "SmallBlackBall")
        {
            //红球变灰
            DestroySelf();
            Hostage.GenerateSelf(transform.position);
            hasCollided = true;
        }
        else if (collision.gameObject.tag == "Boss")
        {
            //红球变黑
            DestroySelf();
            Enemy.GenerateSelf(transform.position);
            hasCollided = true;
        }

        if(collision.gameObject.tag == "Boss" || collision.gameObject.tag == "SmallBlackBall" || collision.gameObject.tag == "RedBall" || collision.gameObject.tag == "GreyBall" || collision.gameObject.tag == "Player")
        {
            //Debug.Log(collision.gameObject.tag);
            Rebound(collision);
           // Debug.Log(rb.velocity);
            hasCollided = true;
        }

        if (collision.gameObject.layer == 10)
        {
            //CollideWall(collision.contacts[0].normal);
            //Debug.Log("撞墙");
            hasCollided = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "AirWall")
        {
            DestroySelf();
            float rangeMin = 40;
            float rangeMax = 60;
            RaycastHit hit;
            Vector3 pos = other.transform.position;
            if (other.transform.position != GameObject.Find("PlayerBall").transform.position)
            {
                Debug.LogErrorFormat("空气墙的位置和主角位置不一样{0},{1}", other.transform.position, GameObject.Find("PlayerBall").transform.position);
            }
            Vector3 dir = new Vector3((Random.Range(-1, 1) + 0.5f) * 2 * Random.Range(rangeMin, rangeMax), 0, (Random.Range(-1, 1) + 0.5f) *2 * Random.Range(rangeMin, rangeMax));
            pos += dir.normalized * 20;
            if (Physics.Raycast(pos, dir, out hit, 50, 1 << 10))
            {
               // Debug.LogFormat("空气墙后重生：射线射中,位置{0}", hit.point - dir.normalized * 0.5f);
                GenerateSelf(hit.point - dir.normalized * 0.5f);
            }
            else
            {
              //  Debug.LogFormat("空气墙后重生：射线没有射中,位置{0}", (pos + dir));
                GenerateSelf(pos + dir);
            }
        }
    }

    protected void DestroySelf()
    {
        rb.velocity = Vector3.zero;
        //Debug.Log("销毁红球");
        gm.SetBallNum("red", false);

        friendPool.Push(gameObject);
        gameObject.SetActive(false);

        redBalls.Remove(this);
    }

    public static void GenerateSelf(Vector3 pos)
    {
        //Debug.Log("生成红球");
        GameObject.Find("Manager").GetComponent<GameManager>().SetBallNum("red", true);

        GameObject go;
        if (friendPool.Count > 0)
        {
            go = friendPool.Pop();
            go.SetActive(true);
        }
        else
        {
            go = Instantiate<GameObject>((GameObject)Resources.Load("Balls/RedBall"));

        }
        go.transform.parent = GameObject.Find("RedBalls").transform;
        go.transform.position = pos;

        redBalls.Add(go.GetComponent<Friend>());
    }
}

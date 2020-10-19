using Mapbox.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 灰球：俘虏类
/// 遇见主角或红球变为红色，遇见黑球变为黑色
/// 移动方式：随机移动（使用寻路AI）
/// </summary>
public class Hostage : Ball
{
    private bool hasCollided = true;
    private static Stack<GameObject> hostagePool = new Stack<GameObject>();
    public static List<Hostage> greyBalls = new List<Hostage>();
    static int GreyMaxNum=20;
    [SerializeField] Vector3 direction;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        gm = GameObject.Find("Manager").GetComponent<GameManager>();
        hostagePool.Clear();
      //  rb.velocity = maxSpeed * transform.position.normalized;
        StartCoroutine(RandomMove());
        
    }

    IEnumerator RandomMove()
    {
        Debug.Log("灰球寻路");
        while (true)
        {

            Vector3 pos = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            GameObject.Find("Manager").GetComponent<FindPath>().FindTarget(transform.position, pos, SetPath);           

            yield return new WaitForSeconds(4);

            foreach (Vector3 v in vecs)
            {
                direction = (v - transform.position).normalized;
                while (true)
                {
                    if ((v-transform.position).sqrMagnitude<=1)
                        break;
                    yield return 0;
                }

                yield return 0;
            }

            yield return 0;
        }
    }


    // Update is called once per frame
    void Update()
    {
        Roll(direction);
    }

    private void LateUpdate()
    {
        hasCollided = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;

        if (collision.gameObject.tag == "RedBall"||
             collision.gameObject.tag == "Player")
        {

            //灰球变红
            DestroySelf();
            Friend.GenerateSelf(transform.position);
            hasCollided = true;

        }
        else if (collision.gameObject.tag == "Boss")
        {
            //灰球变黑
            DestroySelf();
            Enemy.GenerateSelf(transform.position);
            hasCollided = true;
        }
        else if (collision.gameObject.tag == "Wall")
        {
            StopCoroutine(RandomMove());
            StartCoroutine(RandomMove());
        }
        if(collision.gameObject.tag == "Boss" || collision.gameObject.tag == "SmallBlackBall" || collision.gameObject.tag == "RedBall" || collision.gameObject.tag == "GreyBall" || collision.gameObject.tag == "Player")
        {
            Rebound(collision);
            hasCollided = true;
        }

        if (collision.gameObject.layer == 10)
        {
           // CollideWall(collision.contacts[0].normal);
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
            if(other.transform.position!= GameObject.Find("PlayerBall").transform.position)
            {
                Debug.LogErrorFormat("空气墙的位置和主角位置不一样{0},{1}", other.transform.position, GameObject.Find("PlayerBall").transform.position);
            }
            Vector3 dir = new Vector3((Random.Range(-1,1)+0.5f) * 2 * Random.Range(rangeMin, rangeMax), 0, (Random.Range(-1, 1) + 0.5f) * 2 * Random.Range(rangeMin, rangeMax));
            pos += dir.normalized * 20;
            if (Physics.Raycast(pos, dir, out hit, 50, 1 << 10))
            {
               // Debug.LogFormat("空气墙后重生：射线射中,位置{0}", hit.point - dir.normalized * 0.5f);
                GenerateSelf(hit.point - dir.normalized * 0.5f);
            }
            else
            {
               // Debug.LogFormat("空气墙后重生：射线没有射中,位置{0}", (pos + dir));
                GenerateSelf(pos + dir);
            }
        }
    }

    protected void DestroySelf()
    {
        rb.velocity = Vector3.zero;
       // Debug.Log("销毁灰球");
        gm.SetBallNum("grey", false);

        hostagePool.Push(gameObject);
        gameObject.SetActive(false);

        greyBalls.Remove(this);
    }

    public static void GenerateSelf(Vector3 pos)
    {
        if (GreyMaxNum <= greyBalls.Count)
            return;

        //Debug.Log("生成灰球");
        GameObject.Find("Manager").GetComponent<GameManager>().SetBallNum("grey", true);
        GameObject go;
        if (hostagePool.Count>0)
        {
            go = hostagePool.Pop();
            go.SetActive(true);
        }
        else
        {
            go = Instantiate<GameObject>((GameObject)Resources.Load("Balls/GreyBall"));
            
        }

        go.transform.parent = GameObject.Find("GreyBalls").transform;
        go.transform.position = pos;

        greyBalls.Add(go.GetComponent<Hostage>());
    }

}

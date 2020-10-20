using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Examples;
using UnityEngine.SocialPlatforms;

public class Enemy : Ball
{
    GameObject playerBall;
    RaycastHit hit;
    private bool hasCollided = true;
    private static Stack<GameObject> enemyPool = new Stack<GameObject>();
    public static List<Enemy> blackBalls = new List<Enemy>();
    private static int BlackMaxNum=20;
    [SerializeField] float sqrDis;
    [SerializeField] Vector3 direction;
    bool openCorouitine = false;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        playerBall = GameObject.Find("PlayerBall");
        rb = transform.GetComponent<Rigidbody>();
        gm = GameObject.Find("Manager").GetComponent<GameManager>();
        enemyPool.Clear();
        StartCoroutine(RandomMove());
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.SphereCast(transform.position, 1, rb.velocity, out hit, 5, 1 << 10))
            direction += hit.normal * rb.velocity.magnitude * 0.8f + Vector3.Cross(hit.normal, Vector3.up).normalized;

        direction += (playerBall.transform.position - transform.position).normalized +Vector3.Cross(Vector3.up, rb.velocity).normalized * Random.Range(-1f, 1f);

        int index = FindRedBall();
        if (index>=0)
        {
            direction = (Friend.redBalls[index].transform.position-transform.position).normalized;
            StopCoroutine(RandomMove());
            openCorouitine = false;
        }
        else
        {
            if (!openCorouitine)
            {
                StartCoroutine(RandomMove());
                openCorouitine = true;
            }

        }

        Roll(direction.normalized);
;    }

    int FindRedBall()
    {
        for(int i=0;i<Friend.redBalls.Count;i++)
        {
            if((Friend.redBalls[i].transform.position-transform.position).sqrMagnitude<sqrDis)
            {
                return i;
            }
        }
        return -1;
    }

    IEnumerator RandomMove()
    {
        
        while (true)
        {

            Vector3 pos = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            GameObject.Find("Manager").GetComponent<FindPath>().FindTarget(transform.position, pos, SetPath);
            yield return new WaitForSeconds(4);
            foreach (Vector3 v in vecs)
            {
                
                while (true)
                {
                    direction += (v - transform.position).normalized;
                    if ((v - transform.position).sqrMagnitude <= 1)
                        break;
                    yield return 0;
                }

                yield return 0;
            }

            yield return 0;
        }
    }

    private void LateUpdate()
    {
        hasCollided = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;

        if (collision.gameObject.tag == "Player"&&collision.gameObject.GetComponent<Player>().state==2)
        {
            //小黑球与中主角：小黑球变灰，速度加快
            DestroySelf();
            Hostage.GenerateSelf(transform.position);
            hasCollided = true;
        }
        else if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<Player>().state == 3)
        {
            //小黑球与大主角：小黑球变红，速度加快
            DestroySelf();
            Friend.GenerateSelf(transform.position);
            hasCollided = true;
        }

        if(collision.gameObject.tag == "Boss" || collision.gameObject.tag == "SmallBlackBall" || collision.gameObject.tag == "RedBall" || collision.gameObject.tag == "GreyBall" || collision.gameObject.tag == "Player")
        {
            Rebound(collision);
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
        //Debug.Log("销毁黑球");
        gm.SetBallNum("black", false);

        enemyPool.Push(gameObject);
        gameObject.SetActive(false);

        blackBalls.Remove(this);
    }

    public static void GenerateSelf(Vector3 pos)
    {
        if (BlackMaxNum <= blackBalls.Count)
            return;

        //Debug.Log("生成黑球");
        GameObject.Find("Manager").GetComponent<GameManager>().SetBallNum("black", true);
        GameObject go;
        if (enemyPool.Count > 0)
        {
            go = enemyPool.Pop();
            go.SetActive(true);
        }
        else
        {
            go = Instantiate<GameObject>((GameObject)Resources.Load("Balls/SmallBlackBall"));

        }
        go.transform.parent = GameObject.Find("BlackBalls").transform;

        go.transform.position = pos;

        blackBalls.Add(go.GetComponent<Enemy>());
    }
}

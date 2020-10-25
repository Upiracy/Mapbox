using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Examples;
using UnityEngine.SocialPlatforms;

public class Enemy : Ball
{
    GameManager gm;
    GameObject playerBall;
    RaycastHit hit;
    private bool hasCollided = true;
    private static Stack<GameObject> enemyPool = new Stack<GameObject>();
    public static List<Enemy> blackBalls = new List<Enemy>();
    private static int BlackMaxNum=20;
    [SerializeField] int sqrDis = 36;
    [SerializeField] Vector3 direction;
    bool openCorouitine = false;
    public bool fast = false;
    GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        fast = false;
        playerBall = GameObject.Find("PlayerBall");
        rb = transform.GetComponent<Rigidbody>();
        gm = GameObject.Find("Manager").GetComponent<GameManager>();
        enemyPool.Clear();
        StartCoroutine(RandomMove());

        boss = GameObject.Find("RedBalls").GetComponent<RedBallsManager>().boss;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Renderer>().materials[0].name != "C_Black (Instance)")
        {
           // Debug.LogErrorFormat("黑球材质错误！，当前材质是{0}", GetComponent<MeshRenderer>().materials[0].name);
            GetComponent<Renderer>().material = transform.parent.GetComponent<ChangeBallsMat>().mat;
            GetComponent<MeshRenderer>().material = transform.parent.GetComponent<ChangeBallsMat>().mat;
        }

        if (Physics.SphereCast(transform.position, 1, rb.velocity, out hit, 5, 1 << 10))
            direction += hit.normal * rb.velocity.magnitude * 0.8f + Vector3.Cross(hit.normal, Vector3.up).normalized;

        direction += Vector3.Cross(Vector3.up, rb.velocity).normalized * Random.Range(-1f, 1f);

        if(boss != null && boss.activeSelf && (boss.transform.position - transform.position).sqrMagnitude<25)
        {
            direction += (boss.transform.position - transform.position).normalized *2 ;
        }

        if (fast)
        {
            direction += (playerBall.transform.position - transform.position).normalized *2 ;
        }
        else
        {
            int index = FindRedBall();
            if (index >= 0)
            {
                if((Friend.redBalls[index].transform.position - transform.position).sqrMagnitude< (playerBall.transform.position - transform.position).sqrMagnitude)
                    direction = (Friend.redBalls[index].transform.position - transform.position).normalized;
                else
                    direction = (playerBall.transform.position - transform.position).normalized ;
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
            EffectManager.ChangeColor(gameObject, collision, Resources.Load<Material>("C_Grey"));
            this.enabled = false;
            Invoke("Black2Grey", 1);

            
            hasCollided = true;
        }
        else if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<Player>().state == 3)
        {
            //小黑球与大主角：小黑球变红，速度加快
            EffectManager.ChangeColor(gameObject, collision, Resources.Load<Material>("C_Red"));
            this.enabled = false;
            Invoke("Black2Red", 1);

            
            hasCollided = true;
        }

        if(collision.gameObject.tag == "Boss" || collision.gameObject.tag == "SmallBlackBall" || collision.gameObject.tag == "RedBall" || collision.gameObject.tag == "GreyBall" || collision.gameObject.tag == "Player")
        {
            Rebound(collision);
            hasCollided = true;
        }

        if (collision.gameObject.layer == 10)
        {
            CollideWall(collision);
            //Debug.Log("撞墙");
            hasCollided = true;
        }
    }

    void Black2Grey()
    {
        DestroySelf();
        Hostage.GenerateSelf(transform.position);
    }

    void Black2Red()
    {
        DestroySelf();
        Friend.GenerateSelf(transform.position);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "AirWall")
        {
            DestroySelf();
            float rangeMin = 20f;
            float rangeMax = gm.rangeMax;
            RaycastHit hit;
            Vector3 pos = other.transform.position;
            if (other.transform.position != GameObject.Find("PlayerBall").transform.position)
            {
                Debug.LogErrorFormat("空气墙的位置和主角位置不一样{0},{1}", other.transform.position, GameObject.Find("PlayerBall").transform.position);
            }
            Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * Random.Range(rangeMin, rangeMax);
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

        else if (other.gameObject.tag == "Boss")
        {
            //Debug.Log("黑球减速");
            fast = false;
            maxSpeed = 3;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Boss")
        {
            fast = true;
            maxSpeed = 6;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RedColorRange")
        {
            Debug.Log("结束时小球全变红");
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
        }
        else
        {
            go = Instantiate<GameObject>((GameObject)Resources.Load("Balls/SmallBlackBall"));
        }
        go.transform.parent = GameObject.Find("BlackBalls").transform;
        go.transform.position = pos;
        go.GetComponent<Enemy>().enabled = true;
        go.GetComponent<MeshRenderer>().materials[0] = Resources.Load<Material>("C_Black");
        blackBalls.Add(go.GetComponent<Enemy>());
        go.SetActive(true);
    }

    private void OnEnable()
    {
       // rb.velocity = Vector3.zero;
        GetComponent<Enemy>().enabled = true;
        GetComponent<MeshRenderer>().materials[0] = Resources.Load<Material>("C_Black");

        if (GetComponent<MeshRenderer>().materials[0].name != "C_Black (Instance)")
        {
            //Debug.LogErrorFormat("黑球材质错误！，当前材质是{0}", GetComponent<MeshRenderer>().materials[0].name);
        }
    }

    /*
    public static void Bullet2Enemy(Vector3 pos)
    {
        if (BlackMaxNum <= blackBalls.Count)
            return;

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
        go.GetComponent<Enemy>().enabled = true;
        blackBalls.Add(go.GetComponent<Enemy>());

        go.GetComponent<Enemy>().fast = true;
        go.GetComponent<Enemy>().maxSpeed = 6;
    }
    */
}

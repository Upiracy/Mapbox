using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Examples;
using System.Threading;
//using System.Diagnostics;

public class Boss : Ball
{ 
    [SerializeField] GameObject playerBall;
    private bool hasCollided = false;
    [SerializeField] Vector3 direction;
    [SerializeField] int bossHP = 3;
    [SerializeField] int count;
    [SerializeField] float dropGap;
    [SerializeField] float preTime;
    [SerializeField] float dropRange;
    [SerializeField] float sqrShadowRange;
    RaycastHit hit;
    int sumHP;

    // Start is called before the first frame update
    void Start()
    {
        sumHP = bossHP;
        rb = transform.GetComponent<Rigidbody>();
        //playerBall = GameObject.Find("PlayerBall");
        //GameObject.Find("Manager").GetComponent<GameManager>().SetBallNum("black", true);
        StartCoroutine(FindPlayer());

        sqrShadowRange = transform.GetChild(0).transform.localScale.x * transform.GetChild(0).transform.localScale.x;
        //Debug.LogFormat("{0},{1}, sqrShadowRange = {2}", transform.GetChild(0).transform.localScale.x,transform.GetChild(0).transform.localScale.sqrMagnitude,sqrShadowRange);

        //StartCoroutine(StartDropBullet());
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.SphereCast(transform.position, 1, rb.velocity, out hit, 5, 1 << 10))
            direction = (playerBall.transform.position - transform.position).normalized + hit.normal * rb.velocity.magnitude * 0.1f + Vector3.Cross(hit.normal, Vector3.up).normalized;
        else
            direction = (playerBall.transform.position - transform.position).normalized;
        direction += Vector3.Cross(Vector3.up, rb.velocity).normalized * Random.Range(-1f, 1f);
        Roll(direction.normalized );
        //rb.AddForce(Vector3.up * -50);
    }

    IEnumerator FindPlayer()
    {
        while(true)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            Vector3 dir = new Vector3((playerBall.transform.position - transform.position).x, 0, (playerBall.transform.position - transform.position).z).normalized;
            for(int i=0;i<count;i++)
            {
                transform.position += new Vector3(0, Mathf.Max(0, 1f-i*0.01f), 0);
                yield return 0;
            }

            transform.position = new Vector3(playerBall.transform.position.x, 30, playerBall.transform.position.z)-dir*3;

            yield return new WaitForSeconds(0.5f);
            rb.velocity += dir*15;
            yield return new WaitForSeconds(1f);
            transform.GetChild(0).gameObject.SetActive(true);



            yield return new WaitForSeconds(30);
        }

    }

    private void LateUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;

        //遇见第三阶段合体主角且体积大于自己
        if (collision.gameObject.tag == "Player" && collision.transform.localScale.x >= transform.localScale.x && collision.gameObject.GetComponent<Player>().union )
        {
            //体积减小，范围减小，炸出黑球
            bossHP--;
            transform.localScale = new Vector3(1, 1, 1) * Mathf.Lerp(1f, 2.5f, (float)bossHP / sumHP);
            Debug.LogFormat("boss体积减小到{0}",Mathf.Lerp(1f, 2.5f, (float)bossHP / sumHP));
            transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1) * Mathf.Lerp(1f, 5f, (float)bossHP / sumHP);
            transform.GetComponent<SphereCollider>().radius = transform.localScale.x;
            sqrShadowRange = transform.GetChild(0).transform.localScale.x * transform.GetChild(0).transform.localScale.x;

            //炸出黑球在范围边缘？
            int a = 2;
            for(int i=0;i<a;i++)
            {
                Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                Enemy.GenerateSelf(transform.position + dir * transform.GetChild(0).transform.localScale.x * 1.5f );
            }

            if (bossHP<=0)
            {
                Debug.Log("BOSS死亡");
                GameObject.Find("Manager").GetComponent<GameManager>().PlayerWin();

                Destroy(gameObject);              
            }
            hasCollided = true;
            StartCoroutine(AllowCollide());
        }
    }

    IEnumerator AllowCollide()
    {
        yield return new WaitForSeconds(1);
        hasCollided = false;
    }

    public void DropBullet()
    {
        Debug.Log("DropBullet()");
        StartCoroutine(StartDropBullet());
    }

    /// <summary>
    /// 第三阶段：每隔一段时间掉落子弹
    /// </summary>
    /// <returns></returns>
    IEnumerator StartDropBullet()
    {
        Debug.Log("开启携程");
        Debug.Log(playerBall);
        while (true)
        {
            //Debug.LogFormat("主角距离{0}，boss范围{1}", (playerBall.transform.position - transform.position).sqrMagnitude,sqrShadowRange);
            if ((playerBall.transform.position - transform.position).sqrMagnitude <= sqrShadowRange)
            {
                Debug.Log("掉子弹");
                Vector3 pos = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * dropRange + playerBall.transform.position;
                pos += new Vector3(0, 10, 0);
                GameObject shadow = BulletShadow.GenerateShadow(new Vector3(pos.x, 0.05f, pos.z));
                yield return new WaitForSeconds(preTime);
                Bullet.GenerateBullet(pos, shadow);

                yield return new WaitForSeconds(dropGap);
            }
            yield return 0;
        }
    }
}

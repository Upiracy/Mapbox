using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Examples;
using System.Threading;
//using System.Diagnostics;

public class Boss : Ball
{ 
    GameObject playerBall;
    private bool hasCollided = false;
    [SerializeField] Vector3 direction;
    [SerializeField] float scaleDes;
    [SerializeField] int count;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        playerBall = GameObject.Find("PlayerBall");
        GameObject.Find("Manager").GetComponent<GameManager>().SetBallNum("black", true);
        StartCoroutine(FindPlayer());
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
            Vector3 dir = new Vector3((playerBall.transform.position - transform.position).x, 0, (playerBall.transform.position - transform.position).z).normalized;
            for(int i=0;i<count;i++)
            {
                transform.position += new Vector3(0, Mathf.Max(0, 1f-i*0.01f), 0);
                yield return 0;
            }

            transform.position = new Vector3(playerBall.transform.position.x, 30, playerBall.transform.position.z)-dir*3;

            yield return new WaitForSeconds(0.5f);
            rb.velocity += dir*15;



           

            yield return new WaitForSeconds(10);
        }


        /*
        while (true)
        {
            RaycastHit hit;
            Vector3 pos = transform.position;
            Vector3 dir = (playerBall.transform.position - transform.position).normalized;
            if (!Physics.SphereCast(pos,2, dir, out hit, 20))
            {
                direction = dir;
            }
            else
            {

                GameObject.Find("Manager").GetComponent<FindPath>().FindTarget(transform.position, playerBall.transform.position, SetPath);
                //Debug.LogFormat("{0},{1}", transform.position, playerBall.transform.position);
                yield return new WaitForSeconds(1);

                foreach (Vector3 v in vecs)
                {
                    while (true)
                    {
                        //Debug.LogFormat("Boss当前位置{0},目标位置{1},主角位置{2}", transform.position, v, playerBall.transform.position);
                        //Debug.LogFormat("Boss,{0}", (v - transform.position).normalized);
                        Vector3 nowSpeed = new Vector3((v - transform.position).x, 0, (v - transform.position).z);
                        Roll(nowSpeed.normalized);
                        if (nowSpeed.sqrMagnitude <= 0.01)
                            break;
                        yield return 0;
                    }

                    yield return 0;
                }
            }

            yield return 0;
        }

        */
    }

    private void LateUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;

        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<Player>().state == 3)
        {
            //体积减小

            transform.localScale -= new Vector3(1, 1, 1) * scaleDes;
            if(transform.localScale.sqrMagnitude<1)
            {
                Debug.Log("玩家胜利");
                Destroy(gameObject);
                
            }
            hasCollided = true;
            StartCoroutine(AllowCollide());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<Player>().state == 3)
        {
           // hasCollided = false;
        }
    }

    IEnumerator AllowCollide()
    {
        yield return new WaitForSeconds(1);
        hasCollided = false;
    }
}

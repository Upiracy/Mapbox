using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ball
{
    private bool hasCollided = false;
    private static Stack<GameObject> enemyPool = new Stack<GameObject>();
    public static List<Enemy> blackBalls = new List<Enemy>();
    private static int BlackMaxNum=20;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        enemyPool.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    protected void DestroySelf()
    {
        Debug.Log("销毁黑球");
        GameManager.SetBallNum("black", false);

        enemyPool.Push(gameObject);
        gameObject.SetActive(false);

        blackBalls.Remove(this);
    }

    public static void GenerateSelf(Vector3 pos)
    {
        if (BlackMaxNum <= blackBalls.Count)
            return;

        Debug.Log("生成黑球");
        GameManager.SetBallNum("black", true);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostage : Ball
{
    private bool hasCollided = false;
    private static Stack<GameObject> hostagePool = new Stack<GameObject>();
    public static List<Hostage> greyBalls = new List<Hostage>();
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        hostagePool.Clear();
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
    }

    protected void DestroySelf()
    {
        Debug.Log("销毁灰球");
        GameManager.SetBallNum("grey", false);

        hostagePool.Push(gameObject);
        gameObject.SetActive(false);

        greyBalls.Remove(this);
    }

    public static void GenerateSelf(Vector3 pos)
    {
        Debug.Log("生成灰球");
        GameManager.SetBallNum("grey", true);
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

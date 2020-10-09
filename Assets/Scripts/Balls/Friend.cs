using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : Ball
{
    private bool hasCollided = false;
    private static Stack<GameObject> friendPool = new Stack<GameObject>();
    public static List<Friend> redBalls = new List<Friend>();
    // Start is called before the first frame update
    void Start()
    {
        friendPool.Clear();
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
    }

    protected void DestroySelf()
    {
        Debug.Log("销毁红球");
        GameManager.SetBallNum("red", false);

        friendPool.Push(gameObject);
        gameObject.SetActive(false);
    }

    public static void GenerateSelf(Vector3 pos)
    {
        Debug.Log("生成红球");
        GameManager.SetBallNum("red", true);

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
    }
}

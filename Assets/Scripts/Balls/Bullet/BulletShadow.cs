using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShadow
{
    private static Stack<GameObject> shadowPool = new Stack<GameObject>();

    public static void DestroyShadow(GameObject shadow)
    {
        shadow.SetActive(false);
        shadowPool.Push(shadow);
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Debug.LogFormat("销毁子弹影子{0}", other.gameObject);
            gameObject.SetActive(false);
            shadowPool.Push(gameObject);
        }
    }
    
    private void OnTrigerEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.LogFormat("销毁子弹影子{0}", collision.gameObject);
            gameObject.SetActive(false);
            shadowPool.Push(gameObject);
        }
    }

    */

    public static GameObject GenerateShadow(Vector3 pos)
    {
        GameObject shadow;
        if (shadowPool.Count > 0)
        {
            shadow = shadowPool.Pop();
            shadow.SetActive(true);
        }
        else
        {
            shadow = GameObject.Instantiate(Resources.Load<GameObject>("Bullet/BulletShadow"));
        }

        shadow.transform.parent = GameObject.Find("Shadows").transform;
        shadow.transform.position = pos;

        return shadow;
    }
}

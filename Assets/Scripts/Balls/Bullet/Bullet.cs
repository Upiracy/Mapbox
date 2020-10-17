using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject shadow = null;
    private static Stack<GameObject> bulletPool = new Stack<GameObject>();
    bool destroy = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnEnable()
    {
        destroy = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y<-1)
        {
            if (!destroy)
            {
                DestroySelf();
                destroy = true;
            }
        }
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (!destroy)
        {
            DestroySelf();
            destroy = true;
        }
    }
    

    void DestroySelf()
    {
        gameObject.SetActive(false);
        bulletPool.Push(gameObject);
        BulletShadow.DestroyShadow(shadow);
    }

    public static void GenerateBullet(Vector3 pos, GameObject shadow)
    {

       
        GameObject bullet;
        if (bulletPool.Count>0)
        {
            bullet = bulletPool.Pop();
            bullet.SetActive(true);
        }
        else
        {
            bullet = Instantiate(Resources.Load<GameObject>("Bullet/BulletBall"));
        }

        bullet.transform.parent = GameObject.Find("Bullets").transform;
        bullet.transform.position = pos;
        bullet.GetComponent<Bullet>().shadow = shadow;
    }
}

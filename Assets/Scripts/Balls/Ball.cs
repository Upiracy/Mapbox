using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed=1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Roll(Vector3 direction)
    {
        GetComponent<Rigidbody>().velocity += direction * speed;
    }

    /*
    protected virtual void ChangeColor(Color c)
    {
        Debug.LogFormat("{0}改变颜色为{1}",gameObject.name, c);

        DestroySelf();

        if(c==Color.red)
        {
            Friend.GenerateSelf(transform.position);
        }
        else if (c == Color.grey)
        {
            Hostage.GenerateSelf(transform.position);
        }
        else
        if (c == Color.red)
        {
            Enemy.GenerateSelf(transform.position);
        }
    }
    
    protected virtual void DestroySelf()
    {
        
    }
    */
}

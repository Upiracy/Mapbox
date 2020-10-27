using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBall : MonoBehaviour
{
    Quaternion rotation = Quaternion.identity;
    Vector3 scale;

    // Start is called before the first frame update
    void Start()
    {
        rotation = transform.GetChild(0).rotation;
        scale = transform.localScale;
        //transform.localPosition = Vector3.up * 150;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
        transform.GetChild(0).rotation = rotation;
        if(transform.parent.localScale.x != 0) transform.localScale = scale / transform.parent.localScale.x;
    }
}

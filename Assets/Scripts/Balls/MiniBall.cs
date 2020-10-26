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
        scale = transform.lossyScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
        transform.GetChild(0).rotation = rotation;
        transform.localScale = scale / transform.parent.localScale.x;
    }
}

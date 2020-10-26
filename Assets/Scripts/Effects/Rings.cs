using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rings : MonoBehaviour
{
    [SerializeField] Transform target;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        Vector3 scale = target.localScale;
        transform.localScale = scale;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rings : MonoBehaviour
{
    [SerializeField] Transform target;

    private void Start()
    {
        transform.parent = null;
    }

    void Update()
    {
        transform.position = target.position;        
        Vector3 scale = target.localScale;
        transform.localScale = scale;
    }
}

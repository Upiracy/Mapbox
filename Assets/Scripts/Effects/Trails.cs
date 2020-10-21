using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Trails : MonoBehaviour
{
    public Transform start = null, end = null;
    public float height = 0, time = 0;

    [SerializeField] float rotateSpeed = 1;
    [SerializeField] float rangeSpeed = 1, radius = 5;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rotateSpeed, Space.Self);
        float size = Mathf.Sin(rangeSpeed * Time.time) * radius;
        transform.localScale = Vector3.one * size;
    }

    IEnumerator track()
    {
        Vector3 startP = Vector3.zero, endP = Vector3.zero;
        for(float t = 0; t < time; t += Time.deltaTime)
        {
            if(start) startP = start.position;
            if(end) endP = end.position;

            float d = Mathf.Abs(t - time * 0.5f) / (time * 0.5f);
            float h = Mathf.Lerp(height, 0, d * d);

            Vector3 p = Vector3.Lerp(startP, endP, t / time);
            p = new Vector3(p.x, 0, p.z);

            yield return 0;
        }
    }
}

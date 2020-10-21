using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trails : MonoBehaviour
{
    public Transform start = null, end = null;
    public float height = 0, time = 0;

    [SerializeField] float rotateSpeed = 1;
    [SerializeField] float rangeSpeed = 1, radius = 5;


    private void Start()
    {
        StartCoroutine(track());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rotateSpeed, Space.Self);
        float size = Mathf.Sin(rangeSpeed * Time.time) * radius;
        transform.localScale = Vector3.one * size;
    }

    IEnumerator track()
    {
        Vector3 startP = Vector3.zero, endP = Vector3.zero, lastP = Vector3.zero;
        for(float t = 0; t < time; t += Time.deltaTime)
        {
            if(start) startP = start.position;
            if(end) endP = end.position;

            float deltaY = endP.y - startP.y;
            float rate = (height + deltaY) / (height * 2);
            
            float d = Mathf.Abs(t - time * rate) / (time * rate);
            float h = Mathf.Lerp(height, (startP.y + endP.y) * 0.5f, d * d);

            Vector3 p = Vector3.Lerp(startP, endP, t / time);
            p = new Vector3(p.x, h, p.z);

            transform.position = p;

            transform.LookAt(transform.position * 2 - lastP);
            lastP = p;

            yield return 0;
        }
        for(float t = 0; t < 1; t += Time.deltaTime)
        {
            if (end) endP = end.position;
            transform.position = endP;
            yield return 0;
        }
        Destroy(gameObject);
    }
}

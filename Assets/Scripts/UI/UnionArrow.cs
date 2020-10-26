using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnionArrow : MonoBehaviour
{
    [SerializeField] float gapTime;
    [SerializeField] float smallScale = 0.7f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeScale());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ChangeScale()
    {
        while (true)
        {
            for (float t = Time.time; Time.time - t < gapTime;)
            {
                transform.localScale = new Vector3(1, 1, 1) * Mathf.Lerp(smallScale, 1f, (float)(Time.time - t) / gapTime);
                yield return 0;
            }
            for (float t = Time.time; Time.time - t < gapTime;)
            {
                transform.localScale = new Vector3(1, 1, 1) * Mathf.Lerp(1f, smallScale, (float)(Time.time - t) / gapTime);
                yield return 0;
            }

            yield return 0;
        }
    }
}

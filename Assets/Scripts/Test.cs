using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Transform[] balls;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Trail());
    }

    IEnumerator Trail()
    {
        for(; ; )
        {
            foreach(Transform ball in balls)
            {
                EffectManager.GenerateTrail(ball, transform, 3, 2);
            }
            yield return new WaitForSeconds(3);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBornEffect : MonoBehaviour
{
    public GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateBoss());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GenerateBoss()
    {
        boss.transform.position = transform.position + new Vector3(0,1,0);
        boss.transform.localScale = new Vector3(1, 1, 1) * 0.001f;
        boss.GetComponent<Boss>().enabled = false;
        
        yield return new WaitForSeconds(2);
        boss.SetActive(true);
        for (float t=Time.time;Time.time-t<3;)
        {
            boss.transform.localScale = new Vector3(1, 1, 1) * (float)(Time.time - t) / 3 * 2.2f;
            yield return 0;
        }

        boss.GetComponent<Boss>().enabled = true;
    }
}

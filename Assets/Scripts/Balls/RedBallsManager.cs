using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBallsManager : MonoBehaviour
{
    [SerializeField] GameObject playerBall;
    [SerializeField] float sphereCastMaxDis=1;
    [SerializeField] float setTimeGap = 1;
    // Start is called before the first frame update
    void Start()
    {
        Hostage.GenerateSelf(transform.position + new Vector3(0, 0, 5));

        for(int i=0;i<10;i++)
        {
            Friend.GenerateSelf(transform.position + new Vector3(i, 0, 0));
        }
        StartCoroutine(Set());
    }

    IEnumerator Set()
    {
        while(true)
        {
            Debug.Log("set");
            SetFactor();
            yield return new WaitForSeconds(setTimeGap);
        }
    }



    void SetFactor()
    {
        List<Friend> reds = Friend.redBalls;
        List<Hostage> greys = Hostage.greyBalls;

        //因素123清零
        for (int i = 0; i < reds.Count; i++)
        {
            reds[i].factor1 = Vector3.zero;
            reds[i].factor2 = Vector3.zero;
            reds[i].factor3 = Vector3.zero;
        }

        //因素1计算，遍历n(n-1)/2
        //算因素2 因素3的总值
        //因素45
        Vector3 factors2 = Vector3.zero;
        Vector3 factors3 = Vector3.zero;
        for (int i = 0; i < reds.Count; i++)
        {
            for (int j = i + 1; j < reds.Count; j++)
            {
                Vector3 delta = reds[i].transform.position - reds[j].transform.position;
                float dis = 1 / (delta.magnitude);
                reds[i].factor1 += dis * delta.normalized;
                reds[j].factor1 -= dis * delta.normalized;
            }

            //算factor4，对于每一个红球，找最近的灰球
            float minDis = float.MaxValue;
            int minIndex = -1;
            for(int j=0;j<greys.Count;j++)
            {
                if((reds[i].transform.position-greys[j].transform.position).sqrMagnitude<minDis)
                {
                    minDis = (reds[i].transform.position - greys[j].transform.position).sqrMagnitude;
                    minIndex = j;
                }
            }
            Debug.Log(minIndex);
            reds[i].factor5 = playerBall.transform.position - reds[i].transform.position;
            if (greys.Count <= 0)
                reds[i].factor4 = Vector3.zero;
            else
                reds[i].factor4 = (greys[minIndex].transform.position - reds[i].transform.position) * (1 / reds[i].factor5.magnitude);

            factors2 += reds[i].transform.position;
            factors3 += reds[i].GetComponent<Rigidbody>().velocity.normalized;
        }

        factors2 /= reds.Count;
        factors3 /= reds.Count;

        //算出因素1的均值
        //因素2 3 赋值
        float r = reds[0].transform.localScale.x*0.5f;
        RaycastHit hit;
        for (int i = 0; i < reds.Count; i++)
        {
            reds[i].factor1 /= reds.Count;
            reds[i].factor2 = factors2;
            reds[i].factor3 = factors3;

            //因素6 
            if(Physics.SphereCast(reds[i].transform.position, r, reds[i].GetComponent<Rigidbody>().velocity,out hit, sphereCastMaxDis, 1 << 10))
            {
                reds[i].factor6 = hit.normal;
            }
            else
            {
                reds[i].factor6 = Vector3.zero;
            }
        }

        Debug.LogFormat("{0},{1}",reds[0].factor1, Friend.redBalls[0].factor1);
       // Debug.Log(Friend.redBalls[0].factor1);

    }
}
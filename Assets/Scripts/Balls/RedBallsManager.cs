using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBallsManager : MonoBehaviour
{
    [SerializeField] GameObject playerBall;
    [SerializeField] float sphereCastMaxDis=1;
    [SerializeField] float setTimeGap = 1;
    [SerializeField] float factor5555;
    [SerializeField] GameObject boss;
    //[SerializeField] bool set = true;

    #region 权重解释
    /*
     1趋向于远离周围红球
        速度归一化加起来

     2 趋向于靠近所有红球中心

    3 趋向于朝向红球群的平均方向
        每帧都遍历所有红球

    4 越靠近主角越趋向于接近灰球

    5 越远离主角越趋向于接近主角与主角的距离

    6 越靠近墙越越趋向于远离墙
        Spherecast找墙面,遇到墙 权重方向为墙法线方向

    7 左右方向随机扰动

    8 避大黑球
     */
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        //set = true;
        StartCoroutine(Set());
        
    }

    IEnumerator Set()
    {
        while(true)
        {
            //Debug.Log("set");
            SetFactor();
            yield return new WaitForSeconds(setTimeGap);
        }
    }



    /*
    public void StopSetFactor()
    {
        Debug.Log("关红球set");
        set = false;
        StopCoroutine(Set());
        List<Friend> reds = Friend.redBalls;
        for (int i = 0; i < reds.Count; i++)
        {
            //reds[i].BecomeHP();
        }
    }
    */

    /// <summary>
    /// 红球的集群算法
    /// TODO:遇到凹形墙会卡死
    /// </summary>
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
            //reds[i].factor8 = Vector3.zero;
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
            float minDis = 100;
            int minIndex = -1;
            for(int j=0;j<greys.Count;j++)
            {
                if((reds[i].transform.position-greys[j].transform.position).sqrMagnitude<minDis)
                {
                    minDis = (reds[i].transform.position - greys[j].transform.position).sqrMagnitude;
                    minIndex = j;
                }
            }
           // Debug.Log(minIndex);
            reds[i].factor5 = playerBall.transform.position - reds[i].transform.position;
            reds[i].factor5 -= (playerBall.transform.position - reds[i].transform.position).normalized*factor5555;
            if (greys.Count <= 0||minIndex==-1)
                reds[i].factor4 = Vector3.zero;
            else if(minIndex>=0)
            {
                Vector3 v = (greys[minIndex].transform.position - reds[i].transform.position);
                reds[i].factor4 = v.normalized * (1 / v.magnitude);
                reds[i].factor5 = Vector3.zero;
            }
                //reds[i].factor4 = (greys[minIndex].transform.position - reds[i].transform.position) * (1 / reds[i].factor5.magnitude);

            factors2 += reds[i].transform.position;
            factors3 += reds[i].GetComponent<Rigidbody>().velocity.normalized;
        }

        factors2 /= reds.Count;
        factors3 /= reds.Count;

        //算出因素1的均值
        //因素2 3 赋值
        float r = 1;
        RaycastHit hit;
        for (int i = 0; i < reds.Count; i++)
        {
            reds[i].factor1 /= reds.Count;
            reds[i].factor2 = factors2-reds[i].transform.position;
            reds[i].factor3 = factors3;

            //因素6 
            if(Physics.SphereCast(reds[i].transform.position, r, reds[i].GetComponent<Rigidbody>().velocity,out hit, sphereCastMaxDis, 1 << 10))
            {
                reds[i].factor6 += hit.normal * reds[i].rb.velocity.magnitude*0.1f + Vector3.Cross(hit.normal,Vector3.up ).normalized ;
            }
            else
            {
                reds[i].factor6 *= 0.5f;
            }

            //随机扰动
            reds[i].factor7 = Vector3.Cross(Vector3.up, reds[i].GetComponent<Rigidbody>().velocity).normalized * Random.Range(-1f, 1f);


           // Debug.Log("距离" + (transform.position - boss.transform.position).magnitude + ",,," + boss.activeSelf);

            Vector3 dir = transform.position - boss.transform.position;
            dir = new Vector3(dir.x, 0, dir.z);
            if (boss.activeSelf && dir.sqrMagnitude <100)
            {
                reds[i].factor8 = dir.normalized * 1 / dir.sqrMagnitude + Vector3.Cross(Vector3.up, dir).normalized * Random.Range(-1f, 1f);
                //Debug.Log("红球"+ reds[i].factor8);
            }
            else
            {
                reds[i].factor8 = Vector3.zero;
            }
        }

      //  Debug.LogFormat("{0},{1}",reds[0].factor1, Friend.redBalls[0].factor1);
       // Debug.Log(Friend.redBalls[0].factor1);

    }
}
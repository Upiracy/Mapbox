using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GameManager : MonoBehaviour
{
    private static int redNum=1, blackNum, greyNum;
    [SerializeField] float greyBallR, blackBallR;
    // Start is called before the first frame update
    void Start()
    {
       // Enemy.GenerateSelf(new Vector3(0, 3, 0));
        InitializeBalls();
    }

    /// <summary>
    /// 随机生成一批灰球和黑球
    /// TODO:生成的位置随机性不够
    /// </summary>
    private void InitializeBalls()
    {
        //生成一堆灰球黑球
        int newGreyNum = 10;
        int newBalckNum = 10;
        int rangeMin = -50;
        int rangeMax = 50;
        RaycastHit hit;
        for (int i = 0; i < newGreyNum; i++)
        {
            Vector3 pos = new Vector3(Random.Range(rangeMin, rangeMax), 0.5f, Random.Range(rangeMin, rangeMax));
            Vector3 dir = new Vector3(Random.Range(rangeMin, rangeMax), 0.5f, Random.Range(rangeMin, rangeMax));
            if(Physics.Raycast(pos,dir,out hit,100,1<<10))
            {
                Debug.Log("射中");
                Hostage.GenerateSelf(hit.point - dir.normalized*greyBallR);
            }
            else
            {
                Hostage.GenerateSelf(pos + dir);
                Debug.LogFormat("未射中,{0}", pos + dir);
            }
            
        }
        for (int i = 0; i < newBalckNum; i++)
        {
            Vector3 pos = new Vector3(Random.Range(rangeMin, rangeMax), 0.5f, Random.Range(rangeMin, rangeMax));
            Vector3 dir = new Vector3(Random.Range(rangeMin, rangeMax), 0.5f, Random.Range(rangeMin, rangeMax));
            if (Physics.Raycast(pos, dir, out hit, 20, 1 << 10))
            {
                Debug.Log("射中");
                Enemy.GenerateSelf(hit.point - dir.normalized * blackBallR);
            }
            else
            {
                Enemy.GenerateSelf(pos+dir);

                Debug.LogFormat("未射中,{0}",pos + dir);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void SetBallNum(string color,bool add)
    {
        if (add)
        {
            switch (color)
            {
                case "red":
                    redNum++;
                    break;
                case "black":
                    blackNum++;
                    break;
                case "grey":
                    greyNum++;
                    break;
            }
        }
        else
        {
            switch (color)
            {
                case "red":
                    redNum--;
                    break;
                case "black":
                    blackNum--;
                    break;
                case "grey":
                    greyNum--;
                    break;
            }
        }
        //调用uimanager函数改变比例条
    }

}

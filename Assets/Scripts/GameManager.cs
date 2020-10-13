using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GameManager : MonoBehaviour
{
    private static int redNum=1, blackNum, greyNum,sumNum;
    [SerializeField] float greyBallR, blackBallR;
    Player player = null;
    static bool second = false;
    static bool third = false;
    [SerializeField] GameObject boss;
    [SerializeField]
    int newGreyNum = 10;
    [SerializeField]
    int newBlackNum = 2;

    // Start is called before the first frame update
    void Start()
    {
        redNum = 1;
        blackNum = 0;
        greyNum = 0;
        sumNum = redNum + blackNum + greyNum;
        second = false;
        third = false;
        player = GameObject.Find("PlayerBall").GetComponent<Player>();

        InitializeBalls();
    }

    /// <summary>
    /// 随机生成一批灰球和黑球
    /// TODO:生成的位置随机性不够
    /// </summary>
    private void InitializeBalls()
    {
        //生成一堆灰球黑球
        float rangeMin = -45;
        float rangeMax = 45;
        RaycastHit hit;
        UnityEngine.Debug.LogFormat("灰{0}，黑{1}", greyNum, blackNum);
        for (int i = 0; i < newGreyNum; i++)
        {
            Vector3 pos = new Vector3(Random.Range(rangeMin, rangeMax), 0.5f, Random.Range(rangeMin, rangeMax));
            Vector3 dir = new Vector3(Random.Range(rangeMin, rangeMax), 0, Random.Range(rangeMin, rangeMax));
            if(Physics.Raycast(pos,dir,out hit,40,1<<10))
            {
               // Debug.Log("射中");
                Hostage.GenerateSelf(hit.point - dir.normalized*greyBallR);
            }
            else
            {
                Hostage.GenerateSelf(pos + dir);
               // Debug.LogFormat("未射中,{0}", pos + dir);
            }
            
        }
        for (int i = 0; i < newBlackNum; i++)
        {
            Vector3 pos = new Vector3(Random.Range(rangeMin, rangeMax), 0.5f, Random.Range(rangeMin, rangeMax));
            Vector3 dir = new Vector3(Random.Range(rangeMin, rangeMax), 0, Random.Range(rangeMin, rangeMax));
            if (Physics.Raycast(pos, dir, out hit, 40, 1 << 10))
            {
               // Debug.Log("射中");
                Enemy.GenerateSelf(hit.point - dir.normalized * blackBallR);
            }
            else
            {
                Enemy.GenerateSelf(pos+dir);

               // Debug.LogFormat("未射中,{0}",pos + dir);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log((Random.Range(-1, 1) + 0.5f) * 2 );
    }

    public void SetBallNum(string color,bool add)
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
        sumNum = redNum + blackNum + greyNum;
        UnityEngine.Debug.Log((float)redNum / sumNum);
        if (sumNum > newGreyNum+newBlackNum)
        {
            if ((float)redNum / sumNum >= 0.3 && (float)redNum / sumNum <= 0.6)
            {
                if (!second)
                {
                    UnityEngine.Debug.LogFormat("第二阶段,红{0},灰{1},黑{2}，比例{3}", redNum, greyNum, blackNum, (float)redNum / sumNum);
                    player.state = 2;
                    player.transform.localScale = new Vector3(1, 1, 1) * 3;
                    second = true;

                    float rangeMin = 40;
                    float rangeMax = 60;
                    RaycastHit hit;
                    Vector3 pos = new Vector3(player.transform.position.x,5, player.transform.position.z);
                    Vector3 dir = new Vector3((Random.Range(-1, 1) + 0.5f) * 2 * Random.Range(rangeMin, rangeMax), 0, (Random.Range(-1, 1) + 0.5f) * 2 * Random.Range(rangeMin, rangeMax));
                    if (Physics.Raycast(pos, dir, out hit, 60, 1 << 10))
                    {
                        boss.SetActive(true);
                        boss.transform.position = hit.point - dir.normalized * 3;
                    }
                    else
                    {
                        boss.SetActive(true);
                        boss.transform.position = pos + dir;
                        // Debug.LogFormat("未射中,{0}", pos + dir);
                    }

                }
            }
            else if ((float)redNum / sumNum > 0.6)
            {
                if (!third)
                {
                    UnityEngine.Debug.LogFormat("第三阶段,红{0},灰{1},黑{2}，比例{3}", redNum, greyNum, blackNum, (float)redNum / sumNum);
                    player.state = 3;
                    player.transform.localScale = new Vector3(1, 1, 1) * 5;
                    third = true;
                }
            }
        }
        //调用uimanager函数改变比例条
    }

    void GenerateBoss()
    {

    }

}

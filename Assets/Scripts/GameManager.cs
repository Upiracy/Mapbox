using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering.LWRP;
using UnityEngine.SocialPlatforms;

public class GameManager : MonoBehaviour
{
    private static int redNum=1, blackNum, greyNum,sumNum;
    private static bool second = false,third = false;

    [SerializeField] GameObject boss;
    [SerializeField] int newGreyNum = 10;
    [SerializeField] int newBlackNum = 2;
    [SerializeField] int angle1, angle2, angle3;
    [SerializeField] float greyBallR, blackBallR;
    [SerializeField] float dropGap;
    [SerializeField] float preTime;
    [SerializeField] float dropRange;

    public static bool gameOver = false;

    InputManager inputManager;  
    Player player = null;

    void Start()
    {
        redNum = 1;
        blackNum = 0;
        greyNum = 0;
        sumNum = redNum + blackNum + greyNum;
        second = false;
        third = false;
        gameOver = false;
        player = GameObject.Find("PlayerBall").GetComponent<Player>();

        inputManager = gameObject.GetComponent<InputManager>();
        Friend.asHP = false;

        InitializeBalls();
        //StartCoroutine(DropBullet());
    }

    /// <summary>
    /// 初始化：随机生成一批灰球和黑球
    /// TODO:生成的位置随机性不够
    /// </summary>
    private void InitializeBalls()
    {
        //生成一堆灰球黑球
        float rangeMin = -50;
        float rangeMax = 50;
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

    /// <summary>
    /// 修改每种球的个数，并更改UI比例条
    /// </summary>
    /// <param name="color">球的颜色 红灰黑</param>
    /// <param name="add">true为加，false为减</param>
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
        // UnityEngine.Debug.Log((float)redNum / sumNum);
        UnityEngine.Debug.LogFormat("总数{0}，红{1}，灰{2}，黑{3}",sumNum,redNum,greyNum,blackNum);
        if (sumNum > newGreyNum + newBlackNum || third)
        {
            if ((float)redNum / sumNum < 0.3)
            {
                player.state = 1;
                inputManager.maxAngle = angle1;
            }
            else if ((float)redNum / sumNum >= 0.3 && (float)redNum / sumNum <= 0.6)
            {
                player.state = 2;
                inputManager.maxAngle = angle2;
                if (!second)
                {
                    UnityEngine.Debug.LogFormat("第二阶段,红{0},灰{1},黑{2}，比例{3}", redNum, greyNum, blackNum, (float)redNum / sumNum);
                    
                    //player.transform.localScale = new Vector3(1, 1, 1) * 2;
                    second = true;

                    blackNum++;
                    GenerateBoss();

                }
            }
            else if ((float)redNum / sumNum > 0.6)
            {
                player.state = 3;
                inputManager.maxAngle = angle3;
                if (!third)
                {
                    UnityEngine.Debug.LogFormat("第三阶段,红{0},灰{1},黑{2}，比例{3}", redNum, greyNum, blackNum, (float)redNum / sumNum);
                   
                    //player.transform.localScale = new Vector3(1, 1, 1) * 3;
                    third = true;
                    Friend.asHP = true;
                    StartCoroutine(DropBullet());

                    GameObject.Find("RedBalls").GetComponent<RedBallsManager>().StopSetFactor();
                }
            }
            //if(!third)
            UnityEngine.Debug.LogFormat("修改体积{0}", Mathf.Lerp(1f, 2.5f, (float)redNum / sumNum));
                player.transform.localScale = new Vector3(1, 1, 1) * Mathf.Lerp(1f, 2.5f, (float)redNum / sumNum);
        }
        //调用uimanager函数改变比例条
    }

    /// <summary>
    /// 主角附近生成BOSS
    /// TODO: 出生点会卡墙，或者在巷道上方
    /// </summary>
    void GenerateBoss()
    {
        float rangeMin = 10;
        float rangeMax = 20;
        RaycastHit hit;
        Vector3 pos = new Vector3(player.transform.position.x, 5, player.transform.position.z);
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

    /// <summary>
    /// 第三阶段：每隔一段时间掉落子弹
    /// </summary>
    /// <returns></returns>
    IEnumerator DropBullet()
    {
        while(!gameOver)
        {
            Vector3 pos = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * dropRange + player.transform.position;
            pos += new Vector3(0, 10, 0); 
            GameObject shadow = BulletShadow.GenerateShadow(new Vector3(pos.x, 0.05f, pos.z));
            yield return new WaitForSeconds(preTime);
            Bullet.GenerateBullet(pos, shadow);

            yield return new WaitForSeconds(dropGap);
        }
    }
}

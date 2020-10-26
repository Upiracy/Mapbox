using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering.LWRP;
using UnityEngine.SocialPlatforms;

public class GameManager : MonoBehaviour
{
    private static int redNum = 1, blackNum, greyNum;   
    private static bool second = false,third = false;

    [SerializeField] GameObject bossBornEffect;
    [SerializeField] GameObject unionButtonArrow;
    [SerializeField] GameObject holo;
    [SerializeField] int newGreyNum = 10;
    [SerializeField] int newBlackNum = 2;
    [SerializeField] int angle1, angle2, angle3;
    [SerializeField] float greyBallR, blackBallR;
    [SerializeField] float unionTime =10 ;
    [SerializeField] float fadedMusicTime = 3;
    [SerializeField] float unionTimet0 = 0.5f;//技能使用时间与红球数量有关
    [SerializeField] AudioClip music2, music3;
    public float rangeMin = -25;
    public float rangeMax = 25;
    public static int sumNum;

    InputManager inputManager;  
    Player player = null;
    UIManager ui;
    AudioSource audioSource;
    Coroutine coroutine;

    void Start()
    {
        redNum = 1;
        blackNum = 0;
        greyNum = 0;
        sumNum = redNum + blackNum + greyNum;
        second = false;
        third = false;
        //gameOver = false;
        player = GameObject.Find("PlayerBall").GetComponent<Player>();

        inputManager = gameObject.GetComponent<InputManager>();
       // Friend.asHP = false;
        ui = GameObject.Find("Canvas").GetComponent<UIManager>();
        audioSource = GetComponent<AudioSource>();

        InitializeBalls();
    }

    /// <summary>
    /// 初始化：随机生成一批灰球和黑球
    /// TODO:生成的位置随机性不够
    /// </summary>
    private void InitializeBalls()
    {
        //生成一堆灰球黑球
       
        RaycastHit hit;
        int range = 5;
         UnityEngine.Debug.LogFormat("灰{0}，黑{1}", greyNum, blackNum);
        for (int i = 0; i < newGreyNum; i++)
        {
            
            Vector3 pos = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * Random.Range(rangeMin + range, rangeMax - range);
            pos = new Vector3(pos.x, 0.5f, pos.z);
            // Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * (Random.Range(rangeMin+ range, rangeMax- range) + range);
            Vector3 dir = new Vector3(pos.x,0,pos.z).normalized * Random.Range(rangeMax - range, rangeMax);
            if (Physics.Raycast(pos,dir,out hit,40,1<<10))
            {
                UnityEngine.Debug.Log("射中");
                Hostage.GenerateSelf(hit.point - dir.normalized*greyBallR);
            }
            else
            {
                Hostage.GenerateSelf(pos + dir);
              // UnityEngine. Debug.LogFormat("未射中,{0},{1},{2}", pos , dir,pos+dir);
            }
            
        }
        for (int i = 0; i < newBlackNum; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * Random.Range(rangeMin + range, rangeMax - range);
            pos = new Vector3(pos.x, 0.5f, pos.z);
            // Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * (Random.Range(rangeMin+ range, rangeMax- range) + range);
            Vector3 dir = new Vector3(pos.x, 0, pos.z).normalized * Random.Range(rangeMax - range, rangeMax);
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
                default:
                    UnityEngine.Debug.LogErrorFormat("修改小球个数的字符串有误，，{0}", color);
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
                default:
                    UnityEngine.Debug.LogErrorFormat("修改小球个数的字符串有误，，{0}", color);
                    break;
            }
        }
        sumNum = redNum + blackNum + greyNum;
        // UnityEngine.Debug.Log((float)redNum / sumNum);
        //UnityEngine.Debug.LogFormat("总数{0}，红{1}，灰{2}，黑{3}",sumNum,redNum,greyNum,blackNum);
        if (sumNum > newGreyNum + newBlackNum)
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
                    second = true;
                    blackNum++;
                    GenerateBoss();
                    player.state = 2;
                    inputManager.maxAngle = angle2;
                    //切bgm
                    StartCoroutine(Music1TO2());

                    //合体按钮的箭头
                    unionButtonArrow.SetActive(true);
                }
            }
            else if ( third && (float)redNum / sumNum > 0.6)
            {
                player.state = 3;
                inputManager.maxAngle = angle3;
            }

        }
        //调用uimanager函数改变比例条
        ui.ChangeUI(redNum, greyNum, blackNum, player.state);
        
    }

    /// <summary>
    /// 主角附近生成BOSS
    /// TODO: 出生点会卡墙，或者在巷道上方
    /// </summary>
    void GenerateBoss()
    {
        float rangeMin = 3;
        float rangeMax = 5;
        RaycastHit hit;
        Vector3 pos = new Vector3(player.transform.position.x, 0.5f, player.transform.position.z);
        Vector3 dir = new Vector3((Random.Range(-1, 1) + 0.5f) * 2 * Random.Range(rangeMin, rangeMax), 0, (Random.Range(-1, 1) + 0.5f) * 2 * Random.Range(rangeMin, rangeMax));
        if (Physics.Raycast(pos, dir, out hit,3, 1 << 10))
        {
            if (bossBornEffect != null)
            {
                bossBornEffect.SetActive(true);
                bossBornEffect.transform.position = hit.point - dir.normalized * 1.5f;
                
            }
        }
        else
        {
            if (bossBornEffect != null)
            {
                bossBornEffect.SetActive(true);
                bossBornEffect.transform.position = pos + dir;
            }
        }
        UnityEngine.Debug.LogFormat("大黑特效位置{0}", bossBornEffect.transform.position);
    }

    private void checkState()
    {
        if ((float)redNum / sumNum > 0.6)
        {
           /* if (!second)
            {
                UnityEngine.Debug.LogFormat("第二阶段,红{0},灰{1},黑{2}，比例{3}", redNum, greyNum, blackNum, (float)redNum / sumNum);
                second = true;
                blackNum++;
                GenerateBoss();
                player.state = 2;
                inputManager.maxAngle = angle2;

                
            }
           */
            if (!third)
            {
                UnityEngine.Debug.LogFormat("第三阶段,红{0},灰{1},黑{2}，比例{3}", redNum, greyNum, blackNum, (float)redNum / sumNum);
                third = true;
                bossBornEffect.GetComponent<BossBornEffect>().boss.GetComponent<Boss>().DropBullet();

                //StartCoroutine(DropBullet());
                player.state = 3;
                inputManager.maxAngle = angle3;
                ui.ChangeUI(redNum, greyNum, blackNum, player.state);
                //切bgm
                StartCoroutine(Music2TO3());
            }
        }
    }

    IEnumerator Music1TO2()
    {
        
        for(float t =Time.time;Time.time-t<fadedMusicTime;)
        {
            audioSource.volume = 1 - (float)(Time.time - t) / fadedMusicTime;
            yield return 0;
        }

        audioSource.clip = music2;
        audioSource.Play();

        for (float t = Time.time; Time.time - t < fadedMusicTime;)
        {
            audioSource.volume = (float)(Time.time - t) / fadedMusicTime;
            yield return 0;
        }
    }

    IEnumerator Music2TO3()
    {

        for (float t = Time.time; Time.time - t < fadedMusicTime;)
        {
            audioSource.volume = 1 - (float)(Time.time - t) / fadedMusicTime;
            yield return 0;
        }

        audioSource.clip = music3;
        audioSource.Play();

        for (float t = Time.time; Time.time - t < fadedMusicTime;)
        {
            audioSource.volume = (float)(Time.time - t) / fadedMusicTime;
            yield return 0;
        }
    }

    //红球融入主角
    public void UnionRedBalls()
    {
        if (!player.GetComponent<Player>().union)
        {
            checkState();
            List<Friend> reds = Friend.redBalls;
            for (int i = 0; i < reds.Count; i++)
            {
                reds[i].gameObject.SetActive(false);
                EffectManager.GenerateTrail(reds[i].transform, player.transform, 5, 1);
            }
            

            player.transform.localScale = new Vector3(1, 1, 1) * Mathf.Lerp(1f, 3, (float)reds.Count / GameManager.sumNum);
            player.GetComponent<Player>().union = true;
            UnityEngine.Debug.Log("合体,主角" + player.transform.localScale);
            coroutine = StartCoroutine(BeingUnion());
        }
    }

    IEnumerator BeingUnion()
    {
        float sumTime = unionTime + Friend.redBalls.Count * unionTimet0;
        EffectManager.AttachPower(player.transform, sumTime);
        UnityEngine.Debug.LogFormat("等待解除合体，unionTime={0},Friend.redBalls.Count = {1},结果{2}", unionTime, Friend.redBalls.Count, unionTime + Friend.redBalls.Count * unionTimet0);
        // yield return new WaitForSeconds(unionTime + Friend.redBalls.Count * t0);    

        
        for (float t =Time.time;Time.time - t< sumTime;)
        {
            //UnityEngine.Debug.LogFormat("合体中...此时sumTime={0},Time.time={1}, t = {2}",sumTime,Time.time,t);
            yield return 0;
        }
        UnityEngine.Debug.Log("准备执行解除合体函数");


        DivideRedBalls(0);
    }


    public void DivideRedBalls(int changeNum)
    {
        EffectManager.BreakPower();

        StopCoroutine(coroutine);
        player.transform.localScale = new Vector3(1, 1, 1);

        List<Friend> reds = Friend.redBalls;
        RaycastHit hit;
        int redsCount = reds.Count;
        List<GameObject> changeBalls = new List<GameObject>();
        for (int i = 0; i < redsCount; i++)
        {
            
            if (!reds[i].gameObject.activeSelf)
            {
               // UnityEngine.Debug.LogFormat("i={0},是隐藏的,循环总数{1}", i,redsCount);
                //Vector3 pos = new Vector3(Random.Range(rangeMin, rangeMax), 0.5f, Random.Range(rangeMin, rangeMax));
                Vector3 dir = new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f, 1f));
                if (Physics.Raycast(player.transform.position, dir, out hit, 10, 1 << 10))
                {
                    reds[i].transform.position = hit.point - dir.normalized * 0.5f;
                }
                else
                {
                    reds[i].transform.position = player.transform.position + dir * 8;
                }

                if (changeNum > 0)
                {
                    changeBalls.Add(reds[i].gameObject);
                    changeNum--;
                    UnityEngine.Debug.Log("changeNum--;");
                }
                else
                {
                    
                    EffectManager.GenerateTrail(player.transform, reds[i].transform, 5, 1);
                    StartCoroutine(waitRedAppear(reds[i].gameObject, 1));
                }
            }           
        }
        
        for(int i=0;i<changeBalls.Count;i++)
        {
            changeBalls[i].GetComponent<Friend>().ChangeSelf();

            EffectManager.GenerateTrailBlack(player.transform, changeBalls[i].transform, 5, 1);

            StartCoroutine(waitGeneraterEnmey(changeBalls[i].transform.position,1));

            
        }

        
        

        UnityEngine.Debug.Log("解除合体,主角" + player.transform.localScale);
        player.GetComponent<Player>().union = false;
    }

    IEnumerator waitRedAppear(GameObject redBall, float t)
    {
        yield return new WaitForSeconds(t);
        redBall.SetActive(true);
       // UnityEngine.Debug.Log("生成红球");
    }

    IEnumerator waitGeneraterEnmey(Vector3 pos, float t)
    {
        yield return new WaitForSeconds(t);
        Enemy.GenerateSelf(pos);
    }

    public void PlayerWin()
    {
        UnityEngine.Debug.Log("玩家胜利！！！！");

        //Time.timeScale = 0;
        holo.transform.position = player.transform.position;
        holo.SetActive(true);

    }


    public void PlayerLost()
    {
        UnityEngine.Debug.Log("玩家失败……");
        //Time.timeScale = 0;
       // gameOver = true;
        //弹一个ui框

    }

}

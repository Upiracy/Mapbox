using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text ratioRed,ratioGrey,ratioBlack;
    [SerializeField] Image redBar, greyBar, blackBar;
    [SerializeField] Image redPoint, blackPoint;
    [SerializeField] float upMidPer = 0.3f, upBigPer = 0.6f;
    float upBarSize, ratioSize,length,ratioPointWidth;
    private int status = 1;

    [SerializeField] GameObject unionButton;
    [SerializeField] int unionNum;
    [SerializeField] float unionFreezeTime;
    [SerializeField] float freezeTimet1 = 0.1f; //冷却时间受红球数量影响

    //
    [SerializeField] Image mask, barAfter, barBefore,roundEmpty;
    [SerializeField] Image mainLogo1, mainLogo2, mainLogo3;
    [SerializeField] Image logo1, logo2;
    float barSize,roundSize;
    [SerializeField] Image talkingBar;
    [SerializeField] TMP_Text talk;
    [SerializeField] float talkTime=5f;
    float talkPassTime;
    bool talkActive = false;
    bool up1=false, up2=false;
    //
    [SerializeField] Image hint,hintImage;
    [SerializeField] Button hintButton;

    bool isFreeze = false;

    void Start()
    {
        ratioSize = redBar.rectTransform.rect.width;
        length = (redBar.rectTransform.rect.height + redPoint.rectTransform.rect.height) / 2;
        ratioPointWidth = redPoint.rectTransform.rect.width / 2;

        
        barSize = barAfter.rectTransform.rect.width;
        roundSize = roundEmpty.rectTransform.rect.width;

        talkingBar.enabled = false;talk.enabled = false;
    }

    void Update()
    {
        if (talkActive) {
            if (talkPassTime>0) talkPassTime -= Time.deltaTime;
            else { talkActive = false;talkingBar.enabled = false; talk.enabled = false; } 
        }
    }


    public void OnPressStartExit()
    {
        hintImage.enabled = false;
        hint.enabled = false;
        hintButton.image.enabled = false;
        hintButton.enabled = false;
        Time.timeScale = 1;
    }

    public void OnPressRound()
    {
        hintImage.enabled = true;
        hint.enabled = true;
        hintButton.enabled = true;
        hintButton.image.enabled = true;
        Debug.Log("pressRound");
        Time.timeScale = 0;
        
    }
    //status（Small/Mid/Big）为主角状态

    //改变进度条。r,g,b分别为红、灰、黑球数量
    public void SetUpBar(int r,int g,int b) 
    {
        

        barAfter.transform.SetParent(barBefore.transform);
        
        
        if (status == 1)
        {
            mainLogo1.enabled = true;mainLogo2.enabled = false;mainLogo3.enabled = false;
            mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, r / (float)(r + g + b)/upMidPer * (barSize - roundSize) + roundSize);
            logo1.enabled = false;logo2.enabled = false;
        }
        else if (status == 2)
        {
            mainLogo1.enabled = false; mainLogo2.enabled = true; mainLogo3.enabled = false;
            mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, r / (float)(r + g + b) / upBigPer * (barSize - roundSize) + roundSize);
            logo1.enabled = true;logo2.enabled = false;
            logo1.rectTransform.anchoredPosition = new Vector2(upMidPer / upBigPer * (barSize - roundSize) + roundSize, 0);
            
        }
        else if (status == 3)
        {
            mainLogo1.enabled = false; mainLogo2.enabled = false; mainLogo3.enabled = true;
            mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, r / (float)(r + g + b)  * (barSize - roundSize) + roundSize);
            logo1.enabled = true; logo2.enabled = true;
            logo1.rectTransform.anchoredPosition = new Vector2(upMidPer * (barSize - roundSize) + roundSize, 0);
            logo2.rectTransform.anchoredPosition = new Vector2(upBigPer * (barSize - roundSize) + roundSize, 0);
        }
        barAfter.transform.SetParent(mask.transform);
    }
    //改变比例条，r,g,b分别为红、灰、黑球数量
    //
    public void ChangeRatio(int r, int g, int b)
    {
        float value1 = r / (float)(r + g + b);
        float value2 = g / (float)(r + g + b);
        float value3 = b / (float)(r + g + b);
        redBar.rectTransform.SetSizeWithCurrentAnchors
            (RectTransform.Axis.Horizontal, ratioSize * value1);
        greyBar.rectTransform.SetSizeWithCurrentAnchors
            (RectTransform.Axis.Horizontal, ratioSize * value2);
        greyBar.rectTransform.anchoredPosition = new Vector2(ratioSize*value1,0);
        blackBar.rectTransform.SetSizeWithCurrentAnchors
            (RectTransform.Axis.Horizontal, ratioSize * value3);
        blackBar.rectTransform.anchoredPosition=new Vector2(ratioSize*(value1+value2),0);
        redPoint.rectTransform.anchoredPosition = new Vector2(ratioSize * value1-ratioPointWidth, length);
        blackPoint.rectTransform.anchoredPosition = new Vector2(ratioSize * (value1 + value2)-ratioPointWidth, length);
        ratioRed.text = r.ToString();ratioGrey.text = g.ToString();ratioBlack.text = b.ToString();
    }

    public void PopTalk(int i)
    {
        if (i == 2&&up1==false) { 
            talkActive = true;
            talk.text = "level up";
            up1 = true;
        }
        else if (i == 3&&up2==false) { 
            talkActive = true;
            talk.text= "level up";
            up2 = true;
        }
        

        if (talkActive)
        {
            talkingBar.enabled = true; talk.enabled = true;
            talkPassTime = talkTime;
        }
        
    }
    /// <summary>
    /// 由GameManager调用，改变ui
    /// </summary>
    /// <param name="r">红球数</param>
    /// <param name="g">灰球数</param>
    /// <param name="b">黑球数</param>
    /// <param name="state">当前游戏阶段</param>
    public void ChangeUI(int r, int g, int b,int state)
    {
        if(state <1||state >3)
        {
            Debug.LogErrorFormat("游戏阶段参数错误，当前参数阶段为{0}", state);
        }

        if (!isFreeze)
        {
            unionButton.transform.GetChild(0).gameObject.SetActive(false);
            unionButton.transform.GetChild(1).gameObject.SetActive(true);
            //够合体数量且未冷却
            if (r > unionNum) // 是大于不是大于等于，因为热衷包括主角自己
            {
                unionButton.GetComponent<Button>().enabled = true;
                unionButton.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = 1;
                unionButton.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                
            }
            //任何时候个数不够，斗是灰色不可选
            else
            {
                unionButton.GetComponent<Button>().enabled = false;
                unionButton.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = (float)(r - 1) / unionNum;
                unionButton.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
                
            }
        }
        status = state;

        SetUpBar(r, g, b);
        ChangeRatio(r, g, b);
        if (status == 2) PopTalk(2);
        else if (status == 3) PopTalk(3);

    }

    public void ClickUnionButton()
    {
        GameObject.Find("Manager").GetComponent<GameManager>().UnionRedBalls();

        StartCoroutine(UnionButtonFreeze());
    }

    //合体按钮冷却中
    IEnumerator UnionButtonFreeze()
    {
        unionButton.transform.GetChild(2).gameObject.SetActive(false);

        unionButton.transform.GetChild(0).gameObject.SetActive(true);
        unionButton.transform.GetChild(1).gameObject.SetActive(false);
        isFreeze = true;
        unionButton.GetComponent<Button>().enabled = false;

        
        float freezeTime = unionFreezeTime + Friend.redBalls.Count * freezeTimet1;
        for(float t= Time.time; Time.time - t<=freezeTime;)
        {
            unionButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = (Time.time - t) / freezeTime;
            yield return 0;
        }
       
        unionButton.GetComponent<Button>().enabled = true;
        isFreeze = false;
        unionButton.transform.GetChild(0).gameObject.SetActive(false);
        unionButton.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void ClickTryAgain()
    {
        SceneManager.LoadScene(1);
    }

    public void ClickBack()
    {
        SceneManager.LoadScene(0);
    }
}
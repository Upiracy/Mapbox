using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text s, num,ratioRed,ratioGrey,ratioBlack;
    [SerializeField] Image upgradeBar, redBar, greyBar, blackBar, upBarPoint1, upBarPoint2;
    [SerializeField] Image redPoint, blackPoint;
    [SerializeField] float upMidPer = 0.3f, upBigPer = 0.6f;
    float upBarSize, ratioSize,length,ratioPointWidth;
    private int status = 1;

    [SerializeField] GameObject unionButton;
    [SerializeField] int unionNum;
    [SerializeField] float unionFreezeTime;

    bool isFreeze = false;

    void Start()
    {
        upBarSize = upgradeBar.rectTransform.rect.width;
        ratioSize = redBar.rectTransform.rect.width;
        length = (redBar.rectTransform.rect.height + redPoint.rectTransform.rect.height) / 2;
        ratioPointWidth = redPoint.rectTransform.rect.width / 2;
    }

    //status（Small/Mid/Big）为主角状态
    
    //改变进度条。r,g,b分别为红、灰、黑球数量
    public void SetUpBar(int r,int g,int b) 
    {
        if (status==1)
        {
            upgradeBar.rectTransform.SetSizeWithCurrentAnchors
                (RectTransform.Axis.Horizontal, r / (float)(r + g + b)/upMidPer * upBarSize);
            s.text = "Small";
            num.text = (upMidPer*100).ToString() + "%";
            upBarPoint1.enabled = false; upBarPoint2.enabled = false;
        }
        else if (status==2)
        {
            upgradeBar.rectTransform.SetSizeWithCurrentAnchors
                (RectTransform.Axis.Horizontal, r / (float)(r + g + b) / upBigPer * upBarSize);
            s.text = "Mid";
            num.text = (upBigPer * 100).ToString() + "%";
            upBarPoint1.enabled = true; upBarPoint2.enabled = false;
            upBarPoint1.rectTransform.anchoredPosition = new Vector2(upBarSize * upMidPer / upBigPer, 0);
        }
        else if (status==3)
        {
            upgradeBar.rectTransform.SetSizeWithCurrentAnchors
            (RectTransform.Axis.Horizontal, r / (float)(r + g + b) * upBarSize);
            s.text = "Big";
            num.text = "100%";
            upBarPoint1.enabled = true; upBarPoint2.enabled = true;
            upBarPoint1.rectTransform.anchoredPosition = new Vector2(upBarSize*upMidPer , 0);
            upBarPoint2.rectTransform.anchoredPosition = new Vector2(upBarSize*upBigPer, 0);
        }

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


    }

    public void ClickUnionButton()
    {
        GameObject.Find("Manager").GetComponent<GameManager>().UnionRedBalls();

        StartCoroutine(UnionButtonFreeze());
    }

    //合体按钮冷却中
    IEnumerator UnionButtonFreeze()
    {
        unionButton.transform.GetChild(0).gameObject.SetActive(true);
        unionButton.transform.GetChild(1).gameObject.SetActive(false);
        isFreeze = true;
        unionButton.GetComponent<Button>().enabled = false;

        float t1 = 0.01f; //冷却时间受红球数量影响
        float freezeTime = unionFreezeTime + Friend.redBalls.Count * t1;
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
}
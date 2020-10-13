﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_Text s, num;
    [SerializeField] Image upgradeBar, redBar, greyBar, blackBar, upBarPoint1, upBarPoint2;
    [SerializeField] float upMidPer = 0.3f, upBigPer = 0.6f;
    float upBarSize, ratioSize;
    void Start()
    {
        upBarSize = upgradeBar.rectTransform.rect.width;
        ratioSize = redBar.rectTransform.rect.width;
    }

    //改变进度条。r,g,b分别为红、灰、黑球数量，status（Small/Mid/Big）为主角状态
    public void SetUpBar(int r,int g,int b,string status) 
    {
        if (status.Equals("Small"))
        {
            upgradeBar.rectTransform.SetSizeWithCurrentAnchors
                (RectTransform.Axis.Horizontal, r / (float)(r + g + b)/upMidPer * upBarSize);
            s.text = "Small";
            num.text = (upMidPer*100).ToString() + "%";
            upBarPoint1.enabled = false; upBarPoint2.enabled = false;
        }
        if (status.Equals("Mid"))
        {
            upgradeBar.rectTransform.SetSizeWithCurrentAnchors
                (RectTransform.Axis.Horizontal, r / (float)(r + g + b) / upBigPer * upBarSize);
            s.text = "Mid";
            num.text = (upBigPer * 100).ToString() + "%";
            upBarPoint1.enabled = true; upBarPoint2.enabled = false;
            upBarPoint1.rectTransform.anchoredPosition = new Vector2(upBarSize * upMidPer / upBigPer, 0);
        }
        if (status.Equals("Big"))
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
        float value2 = (r + g) / (float)(r + g + b);
        float value3 = (r + g + b) / (float)(r + g + b);
        redBar.rectTransform.SetSizeWithCurrentAnchors
            (RectTransform.Axis.Horizontal, ratioSize * value1);
        greyBar.rectTransform.SetSizeWithCurrentAnchors
            (RectTransform.Axis.Horizontal, ratioSize * value2);
        blackBar.rectTransform.SetSizeWithCurrentAnchors
            (RectTransform.Axis.Horizontal, ratioSize * value3);
    }
}
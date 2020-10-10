using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_Text s, num;
    [SerializeField] Image upgradeBar, redBar, greyBar, blackBar;
    [SerializeField] int upMid = 3, upBig = 5;
    float upBarSize, ratioSize;
    void Start()
    {
        gameObject.GetComponent<Canvas>().enabled = true;
        upBarSize = upgradeBar.rectTransform.rect.width;
        ratioSize = redBar.rectTransform.rect.width;
    }


    public void SetUpBar(int redCount)
    {
        if (redCount < upMid)
        {
            upgradeBar.rectTransform.SetSizeWithCurrentAnchors
                (RectTransform.Axis.Horizontal, redCount / (float)upMid * upBarSize);
            s.text = "Small";
            num.text = redCount.ToString() + '/' + upMid.ToString();
        }
        if (redCount >= upMid && redCount < upBig)
        {
            upgradeBar.rectTransform.SetSizeWithCurrentAnchors
                (RectTransform.Axis.Horizontal, redCount / (float)upBig * upBarSize);
            s.text = "Mid";
            num.text = redCount.ToString() + '/' + upBig.ToString();
        }
        if (redCount >= upBig)
        {
            upgradeBar.rectTransform.SetSizeWithCurrentAnchors
            (RectTransform.Axis.Horizontal, upBarSize);
            s.text = "Big";
            num.text = redCount.ToString() + '/' + upBig.ToString();
        }

    }

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
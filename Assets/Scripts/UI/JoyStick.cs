using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class JoyStick : MonoBehaviour
{
    public Vector2 deltaPos;
    Vector2 startPos;
    [SerializeField] float r;
    [SerializeField] float cutDis= 1;
    bool isMove = false;
    // Start is called before the first frame update
    void Start()
    {
        startPos = GetComponent<RectTransform>().anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isMove)
        {
            if ((GetComponent<RectTransform>().anchoredPosition - startPos).magnitude < cutDis)
                GetComponent<RectTransform>().anchoredPosition = startPos;
            else
            GetComponent<RectTransform>().anchoredPosition -= (GetComponent<RectTransform>().anchoredPosition - startPos).normalized * cutDis * Time.deltaTime * 50;

        }

        deltaPos = (GetComponent<RectTransform>().anchoredPosition - startPos) / r;
    }

    public void ResetPos()
    {
        GetComponent<RectTransform>().anchoredPosition = startPos;
    }

    
    public void OnMouseDrag()
    {
        isMove = true;
        Vector2 delta = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - startPos;
        if(delta.sqrMagnitude>r*r)
        {
            delta.Normalize();
            delta *= r;
        }
        GetComponent<RectTransform>().anchoredPosition = startPos+ delta;
    }
    

    private void OnMouseDown()
    {
        isMove = true;
        //GetComponent<RectTransform>().anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void OnMouseUp()
    {
        isMove = false;
        //GetComponent<RectTransform>().anchoredPosition = startPos;

       // StartCoroutine(SpeedCut());
    }
    /*
    IEnumerator SpeedCut()
    {
        for(float t=Time.time;Time.time-t<=cutTime;)
        {
            GetComponent<RectTransform>().anchoredPosition = startPos+(GetComponent<RectTransform>().anchoredPosition - startPos) * (cutTime - (Time.time - t)) / cutTime;

            yield return 0;
        }

        GetComponent<RectTransform>().anchoredPosition = startPos;
    }

    */
}

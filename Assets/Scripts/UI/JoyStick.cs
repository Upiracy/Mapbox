using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class JoyStick : MonoBehaviour
{
    RectTransform parent = null;
    Vector2 basePosition = Vector2.zero;
    public Vector2 deltaPos;
    //Vector2 startPos;
    //[SerializeField] float amplify = 0.65f;
    [SerializeField] float r;
    [SerializeField] float cutDis= 1;
    bool isMove = false;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.GetComponent<RectTransform>();
        basePosition = new Vector2(parent.anchoredPosition.x + parent.rect.width * 0.5f, parent.anchoredPosition.y + parent.rect.height * 0.5f);
        //Debug.Log(basePosition);
        //startPos = GetComponent<RectTransform>().anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isMove)
        {
            if ((GetComponent<RectTransform>().anchoredPosition).magnitude < cutDis)
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            else
            GetComponent<RectTransform>().anchoredPosition -= (GetComponent<RectTransform>().anchoredPosition).normalized * cutDis * Time.deltaTime * 50;

        }

        deltaPos = (GetComponent<RectTransform>().anchoredPosition) / r;
    }

    /*
    public void ResetPos()
    {
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
    */
    
    public void OnMouseDrag()
    {
        isMove = true;

        Vector2 delta = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - basePosition;
        
        if(delta.sqrMagnitude>r*r)
        {
            delta.Normalize();
            delta *= r;
        }
        
        GetComponent<RectTransform>().anchoredPosition = delta  * 0.5f;
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

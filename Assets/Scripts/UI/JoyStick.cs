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
    [SerializeField] float diameter = 150;
    [SerializeField] float cutDis= 1;
    bool isMove = false;

    RectTransform rt;
    [SerializeField] RectTransform arrow;
    [SerializeField] Image arrowImage;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
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
            if (rt.anchoredPosition.magnitude < cutDis)
                rt.anchoredPosition = Vector2.zero;
            else
            rt.anchoredPosition -= (rt.anchoredPosition).normalized * cutDis * Time.deltaTime * 50;

        }

        deltaPos = (rt.anchoredPosition) / diameter;

        if (rt.anchoredPosition.sqrMagnitude >= 1)
        {
            arrow.anchoredPosition = rt.anchoredPosition.normalized * (diameter + rt.rect.width) * 0.5f;
            float angle = Mathf.Atan2(rt.anchoredPosition.y, rt.anchoredPosition.x) * 180 / Mathf.PI - 40;
            //if (rt.anchoredPosition.y < 0) angle += 180; 
            arrow.localRotation = Quaternion.Euler(0, 0, angle);
            arrowImage.color = new Color(1, 1, 1, rt.anchoredPosition.sqrMagnitude / (diameter * diameter * 0.25f));
        }
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
        
        if(delta.sqrMagnitude>diameter*diameter)
        {
            delta.Normalize();
            delta *= diameter;
        }
        
        rt.anchoredPosition = delta * 0.5f;


    }
    

    private void OnMouseDown()
    {
        isMove = true;
        //GetComponent<RectTransform>().anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void OnMouseUp()
    {
        isMove = false;

    }

}

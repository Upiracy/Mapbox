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
    [SerializeField] float cutTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        startPos = GetComponent<RectTransform>().anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        deltaPos = (GetComponent<RectTransform>().anchoredPosition - startPos) / r;
    }
    public void OnMouseDrag()
    {
        Vector2 delta = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - startPos;
        if(delta.sqrMagnitude>r*r)
        {
            delta.Normalize();
            delta *= r;
        }
        GetComponent<RectTransform>().anchoredPosition = startPos+ delta;
    }

    public void OnMouseUp()
    {
        GetComponent<RectTransform>().anchoredPosition = startPos;

        //StartCoroutine(SpeedCut());
    }

    IEnumerator SpeedCut()
    {
        for(float t=Time.time;Time.time-t<=cutTime;)
        {
            GetComponent<RectTransform>().anchoredPosition = startPos+(GetComponent<RectTransform>().anchoredPosition - startPos) * (3 - (Time.time - t)) / 3;

            yield return 0;
        }

        GetComponent<RectTransform>().anchoredPosition = startPos;
    }
}

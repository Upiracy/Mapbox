using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class MinimapLayer : MonoBehaviour
{
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        ChangeLayer(gameObject.transform, "UI");
    }

    void ChangeLayer(Transform trans,string targetLayer)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(targetLayer);
        foreach(Transform child in trans)
        {
            ChangeLayer(child, targetLayer);
        }
    }
}

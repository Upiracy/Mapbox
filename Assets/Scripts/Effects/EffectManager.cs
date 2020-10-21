using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effect;

public static class EffectManager
{

    public static void ChangeColor(GameObject go, Collision co, Material target, float time = 1)
    {       
        ChangeColor cc = go.AddComponent<ChangeColor>();
        cc.contact = co.contacts[0].point;
        cc.target = target;
        cc.time = time;
    }

    public static void GenerateTrail(Transform origin, Transform target, float height, float time)
    {
        Trails trail = GameObject.Instantiate(Resources.Load<GameObject>("Effects/Trails "), origin.position, Quaternion.identity).GetComponent<Trails>();
        trail.start = origin;
        trail.end = target;
        trail.height = height;
        trail.time = time;
    }
}

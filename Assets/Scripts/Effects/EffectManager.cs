using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effect;
using TMPro.EditorUtilities;

public static class EffectManager
{
    static Shadow shadow;
    static Power power;

    public static void ChangeColor(GameObject go, Collision co, Material target, float time = 1)
    {       
        ChangeColor cc = go.AddComponent<ChangeColor>();
        cc.contact = co.contacts[0].point;
        cc.target = target;
        cc.time = time;
    }

    public static void GenerateTrail(Transform origin, Transform target, float height, float time)
    {
        Trails trail = GameObject.Instantiate(Resources.Load<GameObject>("Effects/Trails"), origin.position, Quaternion.identity).GetComponent<Trails>();
        trail.start = origin;
        trail.end = target;
        trail.height = height;
        trail.time = time;
    }

    public static void GenerateTrailBlack(Transform origin, Transform target, float height, float time)
    {
        Trails trail = GameObject.Instantiate(Resources.Load<GameObject>("Effects/BlackTrails"), origin.position, Quaternion.identity).GetComponent<Trails>();
        trail.start = origin;
        trail.end = target;
        trail.height = height;
        trail.time = time;
    }

    public static void SetShadow(Transform target, float intensity)
    {
        Shadow t_shadow = target.GetComponent<Shadow>();
        if (!t_shadow)
        {
            t_shadow = target.gameObject.AddComponent<Shadow>();
            if (!shadow) shadow = t_shadow;
            else
            {
                shadow.Destroy();
                shadow = target.GetComponent<Shadow>();
            }
        }
        else
        {
            if (!shadow) shadow = t_shadow;
            else if (shadow != t_shadow)
            {
                shadow.Destroy();
                shadow = target.GetComponent<Shadow>();
            }
        }

        foreach(Material mat in shadow.materials)
        {
            mat.SetFloat("_Strength", intensity);
        }
    }

    public static void AttachPower(Transform target, float time)
    {
        power = GameObject.Instantiate(Resources.Load<GameObject>("Effects/Power"), target.position, Quaternion.identity).GetComponent<Power>();
        power.target = target;
        power.Destroy(time);
    }

    public static void BreakPower()
    {
        if(power)
        {
            power.Destroy(0);
            //power.gameObject.SetActive(false);
        }
    }
}

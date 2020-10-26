using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Reflection;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] Material c_red = null;
    [SerializeField] Material c_change = null;
    [SerializeField] Transform greyBalls = null, blackBalls = null;
    [SerializeField] float speed = 5, time = 30;

    List<Renderer> renderers = new List<Renderer>();

    private void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            ParticleSystem particle = transform.GetChild(i).GetComponent<ParticleSystem>();
            Type main = particle.main.GetType();
            PropertyInfo property = main.GetProperty("startSpeed");
            property.SetValue(particle.main, new ParticleSystem.MinMaxCurve(speed, speed), null);
            property = main.GetProperty("startLifetime");
            property.SetValue(particle.main, new ParticleSystem.MinMaxCurve(time, time), null);
        }

        Explode(time);
    }

    public void Explode(float time)
    {
        renderers.Clear();
        for (int i = 0; i < greyBalls.childCount; i++)
        {
            Transform child = greyBalls.GetChild(i);
            child.GetComponent<Hostage>().enabled = false;
            renderers.Add(child.GetComponent<Renderer>());
        }
        for (int i = 0; i < blackBalls.childCount; i++)
        {
            Transform child = blackBalls.GetChild(i);
            child.GetComponent<Enemy>().enabled = false;
            renderers.Add(child.GetComponent<Renderer>());
        }

        foreach (Renderer r in renderers)
        {
            Material temp = r.material;
            r.material = c_change;
            r.material.SetColor("_BaseColor", temp.GetColor("_BaseColor"));
            r.material.SetColor("_EmissionColor", temp.GetColor("_EmissionColor"));
            r.material.SetColor("_BaseE", c_red.GetColor("_BaseColor"));
            r.material.SetColor("_EmissionE", c_red.GetColor("_EmissionColor"));
            r.material.SetVector("_Contact", transform.position);
        }

        StartCoroutine(Explodes(time));
    }

    public IEnumerator Explodes(float time)
    {
        float startTime = Time.time;
        for(;Time.time - startTime <= time;)
        {
            foreach (Renderer r in renderers)
            {
                r.material.SetFloat("_Range", (Time.time - startTime) * speed);
            }
            yield return 0;
        }
    }
}

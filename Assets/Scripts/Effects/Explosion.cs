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
    [SerializeField] GameManager gm; 


    List<Renderer> greys = new List<Renderer>();
    List<Renderer> blacks = new List<Renderer>();

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
        greys.Clear();
        blacks.Clear();
        for (int i = 0; i < greyBalls.childCount; i++)
        {
            Transform child = greyBalls.GetChild(i);
            child.GetComponent<Hostage>().enabled = false;
            greys.Add(child.GetComponent<Renderer>());
        }
        for (int i = 0; i < blackBalls.childCount; i++)
        {
            Transform child = blackBalls.GetChild(i);
            child.GetComponent<Enemy>().enabled = false;
            blacks.Add(child.GetComponent<Renderer>());
        }

        foreach (Renderer r in greys)
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
        for (;Time.time - startTime <= time;)
        {
            float range = (Time.time - startTime) * speed;
            for(int i = 0; i < greys.Count; i++)
            {
                Renderer r = greys[i];
                if (!r.gameObject.activeSelf) break;
                r.material.SetFloat("_Range", range);
                if (range > (transform.position - r.transform.position).magnitude)
                {
                    greys.Remove(r);
                    i--;
                    r.GetComponent<Hostage>().DestroySelf();
                    //gm.SetBallNum("grey", false);
                    Friend.GenerateSelf(r.transform.position);
                }
            }
            for(int i = 0; i < blacks.Count; i++)
            {
                Renderer r = blacks[i];
                if (!r.gameObject.activeSelf) break;
                r.material.SetFloat("_Range", range);
                if (range > (transform.position - r.transform.position).magnitude)
                {
                    blacks.Remove(r);
                    i--;
                    r.GetComponent<Enemy>().DestroySelf();
                    //gm.SetBallNum("black", false);
                    Friend.GenerateSelf(r.transform.position);
                }
            }
            yield return 0;
        }
    }
}

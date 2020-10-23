using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    public Transform target;
    
    public void Destroy(float time)
    {
        StartCoroutine(DestroyEffect(time));
    }

    void Update()
    {
        transform.position = target.position;

        Vector3 scale = target.localScale;
        transform.localScale = scale;
        transform.GetChild(0).localScale = scale;
        for(int i = 1; i < 4; i++)
        {
            transform.GetChild(i).localScale = scale * 0.5f;
        }
    }

    IEnumerator DestroyEffect(float time)
    {
        yield return new WaitForSeconds(time);
        for(int i = 0; i < 4; i++)
        {
            ParticleSystem particle = transform.GetChild(i).GetComponent<ParticleSystem>();
            Type main = particle.main.GetType();
            PropertyInfo property = main.GetProperty("maxParticles");
            property.SetValue(particle.main, 0, null);
            //particle.maxParcles = 0;
        }
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}

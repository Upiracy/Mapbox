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
        transform.GetChild(0).localScale = scale * 0.8f;
        transform.GetChild(1).localScale = scale * 0.4f;
        transform.GetChild(2).localScale = scale * 0.4f;
    }

    IEnumerator DestroyEffect(float time)
    {
        yield return new WaitForSeconds(time);
        for(int i = 0; i < 3; i++)
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

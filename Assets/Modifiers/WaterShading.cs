using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AkilliMum.SRP.Mirror;

public class WaterShading : MonoBehaviour
{
    CameraShade mainCamera = null;
    new Renderer renderer = null;
    Shade shade = null;

    void OnEnable()
    {
        mainCamera = Camera.main.GetComponent<CameraShade>();
        renderer = gameObject.GetComponent<Renderer>();
        List<Shade> shades = new List<Shade>(mainCamera.Shades);

        shade = new Shade();
        shade.ObjectToShade = gameObject;
        shade.MaterialToChange = renderer.sharedMaterial;
        shades.Add(shade);
        mainCamera.Shades = shades.ToArray();

        mainCamera._renderers.Add(renderer);
    }

    void OnDisable()
    {
        List<Shade> shades = new List<Shade>(mainCamera.Shades);

        shades.Remove(shade);
        mainCamera.Shades = shades.ToArray();

        mainCamera._renderers.Remove(renderer);
    }
}

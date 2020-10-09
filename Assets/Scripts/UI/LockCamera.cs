using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class LockCamera : MonoBehaviour
{
    //[SerializeField] float positionFactor, velocityFactor;

    private Vector3 linkedPosition;
    [SerializeField] Rigidbody linkedObject = null;
    [SerializeField] int delay = 1;

    Rigidbody rgb = null;
    Camera cam = null;

    void Start()
    {
        rgb = GetComponent<Rigidbody>();
        cam = GetComponent<Camera>();
        RaycastHit hit;
        if(Physics.Raycast(cam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f)), out hit, cam.farClipPlane, 1 << 8))
        {
            linkedPosition = hit.point - transform.position;
            linkedPosition = new Vector3(linkedPosition.x, 0, linkedPosition.z);
        }
        else
        {
            throw new System.Exception("Can not find a plane.");
        }

        linkedObject.transform.position = linkedPosition;
    }

    void Update()
    {
        /*
        Vector3 deltaPosition = linkedObject.transform.position - linkedPosition - transform.position;
        deltaPosition = new Vector3(deltaPosition.x, 0, deltaPosition.z);

        Vector3 deltaVelocity = linkedObject.velocity - rgb.velocity;

        rgb.AddForce(deltaPosition * positionFactor + deltaVelocity * velocityFactor);
        */

        /*
        Vector3 targetPosition = linkedObject.position - linkedObject.velocity * velocityFactor - linkedPosition;
        transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        */


        //Debug.Log(linkedObject.velocity);
        StartCoroutine(DelayAffect(linkedObject.velocity));
    }

    IEnumerator DelayAffect(Vector3 velocity)
    {
        for(int i = 0; i < delay; i++) yield return 0;

        rgb.velocity = velocity;
    }

}

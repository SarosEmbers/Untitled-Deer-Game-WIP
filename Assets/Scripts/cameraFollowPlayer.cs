using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollowPlayer : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Vector3 rotation;
    public float smoothTime = 0.25f;

    public float smoothWalk;
    public float smoothRun;

    private playerMovment PM;

    Vector3 currentVelocity;
    // Start is called before the first frame update
    void Start()
    {
        PM = target.gameObject.GetComponent<playerMovment>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (PM.isRunning)
        {
            smoothTime = smoothRun;
        }
        else
        {
            smoothTime = smoothWalk;
        }

        if (target != null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref currentVelocity, smoothTime);
            //transform.rotation = Vector3.SmoothDamp(rotation.x, rotation.y, rotation.z, smoothTime);
        }
    }
}

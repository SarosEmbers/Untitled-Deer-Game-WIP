using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField]
    private Transform targetObject;

    [SerializeField]
    private float cutSize = 0.16f;

    [SerializeField]
    private float cutFade = 0.17f;

    [SerializeField]
    private LayerMask wallMask;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        Vector3 lookPoint = new Vector3(targetObject.transform.position.x, targetObject.transform.position.y + 5, targetObject.position.z);
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(lookPoint);
        cutoutPos.y = cutoutPos.y / (Screen.width / Screen.height);

        Vector3 offset = lookPoint - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

        Debug.DrawLine(this.transform.position, lookPoint);

        for (int i = 0; i < hitObjects.Length; ++i)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

            //Debug.Log(hitObjects + " || " + materials);

            for (int m = 0; m < materials.Length; ++m)
            {
                materials[m].SetVector("_CutoutPos", cutoutPos);
                materials[m].SetFloat("_CutoutSize", cutSize);
                materials[m].SetFloat("_FalloffSize", cutFade);
            }
        }
    }
}

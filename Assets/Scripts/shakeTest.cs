using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shakeTest : MonoBehaviour
{
    public Vector3 originPos;
    public float sIntensity;

    Coroutine shakeCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        //shakeCoroutine = StartCoroutine(shake(3, sIntensity));
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float randPosX = Random.Range(-sIntensity, sIntensity);
        float randPosZ = Random.Range(-sIntensity, sIntensity);

        transform.position = new Vector3(originPos.x + randPosX, transform.position.y, originPos.z + randPosZ);
    }

    IEnumerator shake(int time, float intensity)
    {
        int i = 0;

        while (i < time)
        {
            i++;

            yield return null;
        }
    }
}

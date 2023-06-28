using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randRot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float randRot = Random.Range(-180, 180);

        this.transform.rotation = Quaternion.Euler(0, randRot, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

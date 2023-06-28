using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randSize : MonoBehaviour
{
    public float maxSize;
    public float minSize;

    // Start is called before the first frame update
    void Start()
    {
        float randSize = Random.Range(minSize, maxSize);

        this.transform.localScale = Vector3.one * randSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

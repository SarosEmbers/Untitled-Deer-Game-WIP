using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bushHidden : MonoBehaviour
{
    public GameObject player;
    public playerMovment PM;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PM = player.GetComponent<playerMovment>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            PM.inBush = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PM.inBush = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            PM.inBush = true;
        }
    }
}

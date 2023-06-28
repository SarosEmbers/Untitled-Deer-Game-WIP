using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectObject : MonoBehaviour
{
    public int itemIndex;
    public JournalText JT;

    public GameManager GM;

    // Start is called before the first frame update
    void Start()
    {
        //JT = GameObject.Find("Journal Text Panel").GetComponent<JournalText>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            ++GM.journalsCollected;
            JT.activeButton[itemIndex].interactable = true;
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointGlow : MonoBehaviour
{
    public Image img;
    public Transform target;

    public GameObject[] journals;
    public GameObject player;

    // Update is called once per frame
    private void Update()
    {
        //float closestText = Mathf.Infinity;
        /*
        foreach (GameObject JournalObject in journals)
        {
            if (JournalObject != null)
            {
                float distanceFromText;
                distanceFromText = Vector3.Distance(player.transform.position, JournalObject.transform.position);

                if (distanceFromText < closestText)
                {
                    closestText = distanceFromText;

                    target = JournalObject.transform;

                    Vector3 vectorToTarget = target.transform.position - transform.position;
                    float distanceToTarget = vectorToTarget.magnitude;

                    //toEnemy.SetPosition(1, new Vector3(vectorToTarget.x, vectorToTarget.y, 0.0f));
                }
            }
        }
        */
        img.transform.position = Camera.main.WorldToViewportPoint(target.position);
        Debug.DrawLine(this.transform.position, target.position);
        Debug.DrawLine(this.transform.position, img.transform.position);

    }

    public void findNextTarget()
    {

    }
}

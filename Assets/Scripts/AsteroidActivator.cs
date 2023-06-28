using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidActivator : MonoBehaviour
{
    //https://www.youtube.com/watch?v=kvI2NmlkRUo
    [SerializeField]
    private int distanceFromPlayer;

    private GameObject player;

    public List<ActivatorItem> activatorItems;

    public bool unloadScene = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        activatorItems = new List<ActivatorItem>();

        Debug.Log("ActivatorItem: " + activatorItems.Count);
        StartCoroutine("CheckActivation");
    }

    IEnumerator CheckActivation()
    {
        List<ActivatorItem> removeList = new List<ActivatorItem>();

        if (activatorItems.Count > 0)
        {
            foreach (ActivatorItem item in activatorItems)
            {
                if (Vector3.Distance(player.transform.position, item.itemPos) > distanceFromPlayer)
                {
                    if (item.asteroid == null)
                    {
                        removeList.Add(item);
                    }
                    else
                    {
                        item.asteroid.SetActive(false);
                    }
                }
                else
                {
                    if (item.asteroid == null)
                    {
                        removeList.Add(item);
                    }
                    else
                    {
                        item.asteroid.SetActive(true);
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.01f);

        if (removeList.Count > 0)
        {
            foreach (ActivatorItem item in removeList)
            {
                activatorItems.Remove(item);
            }
        }

        yield return new WaitForSeconds(0.01f);
        StartCoroutine("CheckActivation");

        if (unloadScene == true)
        {
            foreach (ActivatorItem item in removeList)
            {
                Destroy(this.gameObject);
            }
            removeList.Clear();
        }
    }

    public void UnloadScene()
    {
        Debug.Log("UnloadScene");

        foreach(ActivatorItem item in activatorItems)
        {
            Destroy(item.asteroid);
        }
        activatorItems.Clear();

        unloadScene = true;
    }
}

public class ActivatorItem
{
    public GameObject asteroid;
    /*public List<GameObject> asteroids = new List<GameObject>
    {

    };*/
    public Vector3 itemPos;

    //Debug.Log("ActivatorItem: " + asteroid);
}

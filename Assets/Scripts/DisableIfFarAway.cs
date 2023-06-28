using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfFarAway : MonoBehaviour
{
    private GameObject ItemActivarotObject;
    private AsteroidActivator activationScript;

    // Start is called before the first frame update
    void Start()
    {
        ItemActivarotObject = GameObject.Find("Tree Activaror Object");
        activationScript = ItemActivarotObject.GetComponent<AsteroidActivator>();

        StartCoroutine("AddToList");
    }

    IEnumerator AddToList()
    {
        yield return new WaitForSeconds(0.5f);

        activationScript.activatorItems.Add(new ActivatorItem { asteroid = this.gameObject, itemPos = transform.position });
    }
}

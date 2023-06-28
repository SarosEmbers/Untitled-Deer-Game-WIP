using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class endGame : MonoBehaviour
{
    public GameManager GM;
    public UIMenus UI;

    public TextMeshProUGUI textComponent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (GM.journalsCollected)
            {
                case 0:
                    textComponent.text = "You've collected 0/8 notes";
                    break;
                case 1:
                    textComponent.text = "You've collected 1/8 notes";
                    break;
                case 2:
                    textComponent.text = "You've collected 2/8 notes";
                    break;
                case 3:
                    textComponent.text = "You've collected 3/8 notes";
                    break;
                case 4:
                    textComponent.text = "You've collected 4/8 notes";
                    break;
                case 5:
                    textComponent.text = "You've collected 5/8 notes";
                    break;
                case 6:
                    textComponent.text = "You've collected 6/8 notes";
                    break;
                case 7:
                    textComponent.text = "You've collected 7/8 notes";
                    break;
                case 8:
                    textComponent.text = "You collected all 8 notes!";
                    break;
            }

            UI.endGameScreen();
        }
    }
}

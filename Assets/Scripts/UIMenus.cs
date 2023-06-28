using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMenus : MonoBehaviour
{
    public bool inventoryScreen = false;

    public GameManager GM;
    public TextMeshProUGUI deathCounterBox;


    public GameObject inventoryUI;
    public GameObject pauseUI;
    public GameObject endGameUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deathCounterBox.text = GM.deathCounter.ToString();


        if (Input.GetKeyDown(KeyCode.P))
        {
            if (inventoryScreen == false)
            {
                inventoryUI.SetActive(true);
                inventoryScreen = true;
                GM.pauseGame(true);
            }
            else
            {
                inventoryUI.SetActive(false);
                inventoryScreen = false;
                GM.pauseGame(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GM.pauseGame(!GM.gamePaused);

            if (GM.gamePaused)
            {
                pauseUI.SetActive(false);
                GM.gamePaused = false;
            }
            else
            {
                pauseUI.SetActive(true);
                GM.gamePaused = true;
            }
        }
    }

    public void endGameScreen()
    {
        endGameUI.SetActive(true);
        GM.pauseGame(true);
    }

    public void unPause()
    {
        GM.pauseGame(!GM.gamePaused);

        pauseUI.SetActive(false);
        GM.gamePaused = false;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game State")]
    [Tooltip("Bushless Mode means you do not to be hiding in a bush to be completely hidden, th player can simply crouch and hide.")]
    public bool bushlessMode = false;
    public bool softDeathMode = false;
    public int deathCounter = 0;

    public GameObject buttonBush;
    public GameObject buttonDeath;

    public bool gamePaused = false;
    public bool gameOver = false;
    public int journalsCollected;

    [Header("Other Scipts")]
    public daddyDeerScript DD;
    public deerScript[] dS;
    public playerMovment PM;
    public GameObject player;
    public GameObject demon;
    public GameObject[] deer;

    public JournalText JT;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PM = player.GetComponent<playerMovment>();
        demon = GameObject.Find("DemonDeer");
        DD = demon.GetComponent<daddyDeerScript>();
        deer = GameObject.FindGameObjectsWithTag("Deer");
        Debug.Log(deer.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            resetPoints();
            PM.inBush = true;
        }
    }

    IEnumerator allDeer()
    {
        for (int i = 0; i < deer.Length; ++i)
        {
            deer[i].GetComponent<deerScript>().StopAllCoroutines();
            deer[i].GetComponent<deerScript>().currentDestination = deer[i].GetComponent<deerScript>().walkPoints[0].transform.position;
            deer[i].GetComponent<deerScript>().stateOfDeer = deerScript.deerState.Roaming;
            deer[i].GetComponent<deerScript>().limitReached = false;
            deer[i].GetComponent<deerScript>().exclaim = false;
            deer[i].GetComponent<deerScript>().alert = false;
        }

        yield return null;
    }

    public void pauseGame(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void restartGame(bool saveProgress)
    {
        if(!saveProgress)
        {
            JT.resetAll();
        }

        Debug.Log("Restart");
        gamePaused = false;
        Time.timeScale = 1f;
        //SceneManager.LoadScene("NintendoDSTestbed");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void closeGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void bushlessAct()
    {
        if(bushlessMode)
        {

            buttonBush.SetActive(false);
            bushlessMode = false;
        }
        else
        {

            buttonBush.SetActive(true);
            bushlessMode = true;
        }
    }

    public void softdeathAct()
    {
        if (softDeathMode)
        {

            buttonDeath.SetActive(false);
            softDeathMode = false;
        }
        else
        {

            buttonDeath.SetActive(true);
            softDeathMode = true;
        }
    }

    public void resetPoints()
    {
        DD.StopAllCoroutines();
        DD.GetComponent<daddyDeerScript>().doomed = false;
        DD.GetComponent<daddyDeerScript>().summon = false;

        demon.transform.position = DD.GetComponent<daddyDeerScript>().homePoint;

        DD.GetComponent<daddyDeerScript>().stateOfDemon = daddyDeerScript.demonState.nothing;

        StartCoroutine("allDeer");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class daddyDeerScript : MonoBehaviour
{
    public GameObject player;
    public playerMovment PM;
    public GameManager GM;

    Coroutine dadCoroutine;
    public bool doomed = false;

    public Animator demonAnims;
    public AudioSource demonStep, demonRoar;
    public AudioClip[] demonSound;
    static bool audioPlayed = false;

    [Header("Spacial Awareness")]
    public float detectRange;

    public enum demonState
    {
        nothing,
        Standby,
        Summoned,
        Searching,
        Doomed,
        Attacking,
        Returning
    }
    public demonState stateOfDemon = demonState.Standby;

    public bool summon = false;
    public Vector3 summonPoint;

    public float searchTimerMax = 4;

    public Vector3 homePoint;

    [Header("Movement")]
    public float movementSpeed = 1;
    public Transform lungePoint;

    public Transform idleLookPoint;

    public Vector3 originPos;
    public float shakeIntensity;

    // Start is called before the first frame update
    void Start()
    {
        demonAnims.Play("Demon Idle");
        homePoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        switch (stateOfDemon)
        {
            case demonState.nothing:

                demonAnims.Play("Demon Idle");

                break;
            case demonState.Standby:

                demonAnims.Play("Demon Idle");

                break;
            case demonState.Summoned:

                demonAnims.Play("Demon Run");

                Vector3 walkTo = summonPoint - transform.position;
                Quaternion rotation = Quaternion.LookRotation(walkTo);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 25 * Time.deltaTime);

                transform.position = Vector3.MoveTowards(transform.position, summonPoint, movementSpeed * Time.deltaTime);

                if (this.transform.position == summonPoint)
                {
                    stateOfDemon = demonState.Searching;
                }

                break;
            case demonState.Searching:

                demonAnims.Play("Demon Idle");

                if (!summon)
                {
                    Invoke("goHome", searchTimerMax);
                    summon = true;
                }

                //in Bushless mode, the player can be hiding in plain sight and not be seen
                //otherwise, the player must be in a bush AND hiding to be seen

                if(!PM.isHiding)
                {
                    demonAnims.Play("Demon Scream");

                    originPos = transform.position;

                    stateOfDemon = demonState.Doomed;
                }
                else if (PM.isHiding && !PM.inBush && !GM.bushlessMode)
                {
                    demonAnims.Play("Demon Scream");

                    originPos = transform.position;

                    stateOfDemon = demonState.Doomed;
                }
                else if (PM.isHiding && PM.inBush && GM.bushlessMode)
                {
                    Debug.Log("All good v1!");
                }
                else if (PM.isHiding && GM.bushlessMode)
                {
                    Debug.Log("All good v2!");
                }

                Debug.Log("Is Hiding = " + PM.isHiding);
                Debug.Log("Is in Bush = " + PM.inBush);
                Debug.Log("Is Bushless = " + GM.bushlessMode);

                break;
            case demonState.Doomed:

                StopAllCoroutines();

                demonAnims.Play("Demon Scream");

                if(!audioPlayed)
                {
                    demonRoar.clip = demonSound[1];
                    demonRoar.Play();
                    audioPlayed = true;
                }

                float randPosX = Random.Range(-shakeIntensity, shakeIntensity);
                float randPosZ = Random.Range(-shakeIntensity, shakeIntensity);

                transform.position = new Vector3(originPos.x + randPosX, transform.position.y, originPos.z + randPosZ);

                Invoke("attackPlayer", 2);

                break;
            case demonState.Attacking:

                audioPlayed = false;

                demonAnims.Play("Demon Run");

                Vector3 attackto = player.transform.position - transform.position;
                Quaternion atkRot = Quaternion.LookRotation(attackto);
                transform.rotation = Quaternion.Lerp(transform.rotation, atkRot, 25 * Time.deltaTime);

                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);

                break;
            case demonState.Returning:

                demonAnims.Play("Demon Run");

                Vector3 timeToGo = homePoint - transform.position;
                Quaternion lookTohome = Quaternion.LookRotation(timeToGo);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookTohome, 25 * Time.deltaTime);

                transform.position = Vector3.MoveTowards(transform.position, homePoint, movementSpeed * Time.deltaTime);

                if (this.transform.position == homePoint)
                {
                    stateOfDemon = demonState.Standby;
                }

                break;
        }
        Debug.Log(stateOfDemon);
    }

    public void goHome()
    {
        if (stateOfDemon == demonState.Searching)
        {
            stateOfDemon = demonState.Returning;
        }
        else
        {
            Debug.Log("SOMETHING ELSE");
        }
        summon = false;
    }

    public void attackPlayer()
    {
        if(stateOfDemon != demonState.nothing)
        {
            stateOfDemon = demonState.Attacking;
        }

        //dadCoroutine = StartCoroutine(attackPoint(player.transform.position));
    }

    IEnumerator attackPoint(Vector3 runTo)
    {

        while (this.transform.position != runTo)
        {
            Vector3 walkTo = runTo - transform.position;
            Quaternion rotation = Quaternion.LookRotation(walkTo);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 25 * Time.deltaTime);

            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(GM.softDeathMode)
            {
                ++GM.deathCounter;
                transform.position = homePoint;

                GM.resetPoints();

                stateOfDemon = demonState.Standby;
            }
            else
            {
                Debug.Log("DEAD");
                GM.gameOver = true;
                stateOfDemon = demonState.nothing;
                GM.restartGame(false);
            }
        }
    }

    public void playSound(int soundToPlay)
    {
        demonStep.clip = demonSound[soundToPlay];
        demonStep.Play();
    }
}

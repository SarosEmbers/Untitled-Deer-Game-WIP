using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deerScript : MonoBehaviour
{
    public GameObject player;
    public GameObject daddyDeer;

    public playerMovment PM;

    public Animator deerAnims;

    public AudioSource deerSource;

    public AudioClip[] deersounds;

    private bool audioPlayed = false;

    [Header("Spacial Awareness")]
    public float minRange;
    public float maxRange;
    public enum deerState
    {
        Vibin,
        Roaming,
        Freeze,
        Investigate,
        Panic,
        Screaming,
        Collapsed
    }
    public deerState stateOfDeer = deerState.Vibin;

    Coroutine myCoroutine;
    public float curiosityTimer;
    public int attentionSpan;
    public int limitcounter;

    public bool limitReached = false;

    public bool exclaim = false;
    public bool alert = false;

    public Light redLight;

    [Header("Navigation")]
    public float walkSpeed;
    public float rotSpeed;
    public bool deerFreeze;

    public float pointRange = 2.2f;
    public Transform[] walkPoints;
    public Vector3 currentDestination;
    public Vector3 investigatonPoint;
    public int lastWalkPoint;

    [Header("Other")]
    public ParticleSystem groundPoff;

    public Vector3 originPos;
    public float shakeIntensity;

    public float sleepytime = 3;

    // Start is called before the first frame update
    void Start()
    {
        currentDestination = walkPoints[1].transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 vectorToTarget = player.transform.position - transform.position;
        float distanceToTarget = vectorToTarget.magnitude;

        if (distanceToTarget <= maxRange && PM.isRunning || distanceToTarget <= minRange && PM.isMoving)
        {
            // if the player is running while within max range
            // if the player is moving within min range

            originPos = transform.position;

            switch (stateOfDeer)
            {
                case deerState.Roaming:

                    if (!alert)
                    {
                        investigatonPoint = player.transform.position;
                        alert = true;
                    }

                    stateOfDeer = deerState.Freeze;

                    deerSource.clip = deersounds[1];
                    deerSource.Play();

                    break;

                case deerState.Investigate:

                    StopAllCoroutines();
                    screamHeadOff();

                    deerSource.clip = deersounds[2];
                    deerSource.Play();

                    stateOfDeer = deerState.Panic;
                    Debug.Log("FILE'S DONE");

                    break;
                case deerState.Panic:


                    limitReached = true;

                    break;
            }
        }
        else if (distanceToTarget <= minRange !& PM.isHiding)
        {

        }

        float randPosX = Random.Range(-shakeIntensity, shakeIntensity);
        float randPosZ = Random.Range(-shakeIntensity, shakeIntensity);

        switch (stateOfDeer)
        {
            case deerState.Vibin:

                break;
            case deerState.Roaming:

                deerAnims.Play("Deer Walk");

                //go through every point in the list
                //start walking towards the first point
                //when reach the first point, set walkTo destination to the next point in the list
                for (int i = 0; i < walkPoints.Length; ++i)
                {
                    Vector3 vectorToDestination = currentDestination - transform.position;
                    float distanceToDestination = vectorToDestination.magnitude;

                    if (currentDestination == walkPoints[i].transform.position)
                    {
                        //if (this.gameObject.transform.position == walkPoints[i].transform.position)
                        if (distanceToDestination <= pointRange)
                        {
                            try
                            {
                                currentDestination = walkPoints[i + 1].transform.position;
                                lastWalkPoint = i;
                            }
                            catch
                            {
                                currentDestination = walkPoints[0].transform.position;
                            }
                        }
                    }
                }

                Vector3 walkTo = currentDestination - transform.position;
                Quaternion rotation = Quaternion.LookRotation(walkTo);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);

                transform.position = Vector3.MoveTowards(transform.position, Vector3.Lerp(transform.position, currentDestination, walkSpeed), walkSpeed);

                break;
            case deerState.Freeze:

                deerAnims.Play("Deer Notice");

                Vector3 investigate = investigatonPoint - transform.position;
                Quaternion turnTo = Quaternion.LookRotation(investigate);
                transform.rotation = Quaternion.Lerp(transform.rotation, turnTo, rotSpeed * 5 * Time.deltaTime);

                //currentDestination = investigatonPoint;
                //play animation
                if (!exclaim)
                {
                    //Debug.Log("ROTATE");

                    Invoke("investPoint", 0.75f);
                    exclaim = true;
                }

                if (limitReached == true)
                {
                    StopAllCoroutines();
                    screamHeadOff();
                }

                break;
            case deerState.Investigate:

                deerAnims.Play("Deer Walk");

                transform.position = Vector3.MoveTowards(transform.position, Vector3.Lerp(transform.position, investigatonPoint, walkSpeed / 2), walkSpeed / 2);

                /*
                while (curiosityTimer > 0)
                {
                    transform.position = Vector3.MoveTowards(transform.position, Vector3.Lerp(transform.position, currentDestination.position, walkSpeed / 2), walkSpeed / 2);
                }

                if (curiosityTimer <= 0)
                {
                    currentDestination = walkPoints[lastWalkPoint];
                    stateOfDeer = deerState.Roaming;
                }

                Debug.Log(curiosityTimer);
                */

                break;
            case deerState.Panic:

                deerAnims.Play("Deer Screm 1");

                transform.position = new Vector3(originPos.x + randPosX, transform.position.y,originPos.z + randPosZ);

                break;
            case deerState.Screaming:

                if (!audioPlayed)
                {
                    deerSource.clip = deersounds[3];
                    deerSource.Play();
                    audioPlayed = true;
                }

                deerAnims.Play("Deer Screm 2");

                transform.position = new Vector3(originPos.x + randPosX + 0.5f, transform.position.y, originPos.z + randPosZ);

                break;
            case deerState.Collapsed:

                deerAnims.Play("Deer Sleepin");
                limitReached = false;

                Invoke("awaken", sleepytime);
                break;
        }

        //Debug.Log(stateOfDeer);
    }

    public void RedLight(float timeToFade, float initIntensity, float lightIntensity)
    {
        StartCoroutine(FlashTorch(timeToFade, initIntensity, lightIntensity));
    }

    private IEnumerator FlashTorch(float timeToFade, float initIntensity, float lightIntensity)
    {
        float timeElapsed = 0;

        while (timeElapsed < timeToFade)
        {
            redLight.intensity = Mathf.Lerp(initIntensity, lightIntensity, timeElapsed / timeToFade);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void randShake(float amount)
    {
        float x = UnityEngine.Random.Range(amount, amount);
        float y = UnityEngine.Random.Range(amount, amount);
        float z = UnityEngine.Random.Range(amount, amount);

        this.transform.position = new Vector3(x, y, z);
    }

    public void awaken()
    {
        stateOfDeer = deerState.Roaming;
        audioPlayed = false;
    }

    public void investPoint()
    {
        myCoroutine = StartCoroutine(MyCoroutine(attentionSpan));
        exclaim = false;
        alert = false;
    }

    public void screamHeadOff()
    {
        myCoroutine = StartCoroutine(screamCountdown(attentionSpan));
    }

    IEnumerator MyCoroutine(int howLong)
    {
        stateOfDeer = deerState.Investigate;

        int i = 0;

        while (i < howLong)
        {
            i++;
            //Debug.Log(i);
            yield return new WaitForSeconds(1);
        }
        currentDestination = walkPoints[0].transform.position;
        stateOfDeer = deerState.Roaming;
        //Debug.Log("FILE'S DONE");
    }

    IEnumerator screamCountdown(int howLong)
    {
        stateOfDeer = deerState.Panic;

        int i = 0;
        while (i < howLong)
        {
            i++;
            //Debug.Log(i);
            yield return new WaitForSeconds(1);
        }

        //Debug.Log("HERE: " + stateOfDeer);
        //If the player runs out of range of deer while it is in panic, it will go back to roaming and panic the NEXT time is sports the player

        if (limitReached)
        {
            originPos = transform.position;
            stateOfDeer = deerState.Screaming;

            if (daddyDeer.GetComponent<daddyDeerScript>().stateOfDemon != daddyDeerScript.demonState.Summoned)
            {
                daddyDeer.GetComponent<daddyDeerScript>().summonPoint = investigatonPoint;
                daddyDeer.GetComponent<daddyDeerScript>().stateOfDemon = daddyDeerScript.demonState.Summoned;
            }

            int j = 0;
            while (j < howLong)
            {
                j++;
                //Debug.Log(j);
                yield return new WaitForSeconds(1);
            }
            stateOfDeer = deerState.Collapsed;
            groundPoff.Play();
        }

        yield return null;
    }

    /*
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            freakOut(3f);
        }
    }

    public void freakOut(float time)
    {
        DeerShake(time);
        Debug.Log("Shook");
    }
    private IEnumerator DeerShake(float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        Debug.Log(elapsedTime);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = startPosition + Random.insideUnitSphere;
            yield return null;
        }
        transform.position = startPosition;
    }*/

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawWireSphere(transform.localPosition, maxRange);

        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.localPosition, minRange);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovment : MonoBehaviour
{
    [Header("Player Movement")]
    public CharacterController controller;

    public float speed = 7f;
    public float sprintSpeed = 9f;

    public bool isMoving = false;
    public bool isRunning = false;
    public bool isHiding = false;
    public bool inBush = false;

    [Header("The Torch")]

    public Light playerTorch;
    public float torchIntensity;
    public float torchRange;
    public float torchSpark;

    public int torchCounter = 0;
    public float torchTimer = 0.5f;
    public float torchTimerMax = 0.5f;

    public bool torchOn = true;

    public GameObject torchLit;
    public ParticleSystem torchUnlit;

    public Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (transform.position.y > 0.3)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);

            //Debug.Log(transform.position.y);
        }

        if (direction.magnitude >= 0.1f && !isHiding)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            playerAnimator.SetBool("isWalking", true);

            isMoving = true;
            if (Input.GetButton("Fire3"))
            {
                isRunning = true;
                playerAnimator.SetBool("isRunning", true);
                controller.Move(direction * sprintSpeed * Time.deltaTime);

                //Debug.Log("IS RUNNING");
            }
            else
            {
                isRunning = false;
                playerAnimator.SetBool("isRunning", false);
                controller.Move(direction * speed * Time.deltaTime);
            }
        }
        else
        {
            playerAnimator.SetBool("isWalking", false);
            isMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isHiding)
            {
                if(torchTimer < 0.0f)
                {
                    playerAnimator.SetTrigger("lightTorch");
                    torchUnlit.Play();

                    if (torchCounter < 2)
                    {
                        ++torchCounter;
                        LightTorch(0.1f, torchIntensity, 1.0f);
                    }
                    else if (torchCounter == 2)
                    {
                        LightTorch(1.0f, 6.0f, torchIntensity);

                        isHiding = false;
                        playerAnimator.SetBool("isHiding", false);
                        playerAnimator.SetBool("torchOn", true);
                        torchCounter = 0;

                        torchLit.SetActive(true);
                        torchOn = true;
                    }
                    torchTimer = torchTimerMax;
                }
            }
            else
            {
                if (torchTimer < 0.0f)
                {
                    LightTorch(0.2f, torchIntensity, 1.0f);

                    isHiding = true;
                    playerAnimator.SetBool("isHiding", true);

                    torchOn = false;
                    playerAnimator.SetBool("torchOn", false);

                    torchLit.SetActive(false);
                }
            }

            /*else
            {
                isHiding = true;
                playerAnimator.SetBool("isHiding", true);
            }*/
        }
        /*
        if (Input.GetKeyDown("e") && isHiding)
        {
            playerAnimator.SetTrigger("lightTorch");

            if (torchTimer < 3)
            {
                ++torchTimer;
            }
            else if (torchTimer == 3)
            {
                isHiding = false;
                playerAnimator.SetBool("isHiding", false);
                torchTimer = 0;
            }
        }*/

        if (torchTimer >= 0)
        {
            torchTimer = torchTimer - Time.deltaTime;
        }
    }

    public void LightTorch(float timeToFade, float initIntensity, float lightIntensity)
    {
        StartCoroutine(FlashTorch(timeToFade, initIntensity, lightIntensity));
    }

    private IEnumerator FlashTorch(float timeToFade, float initIntensity, float lightIntensity)
    {
        float timeElapsed = 0;

        while (timeElapsed < timeToFade)
        {
            playerTorch.intensity = Mathf.Lerp(initIntensity, lightIntensity, timeElapsed / timeToFade);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}

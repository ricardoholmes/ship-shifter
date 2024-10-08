using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public float smallMaxSpeed = 15f;
    public float bigMaxSpeed = 5f;
    public float smallAcceleration = 15f;
    public float bigAcceleration = 5f;
    public float smallDeceleration = 30f;
    public float bigDeceleration = 10f;
    public float smallTurnSpeed = 360f;
    public float bigTurnSpeed = 180f;

    private float maxSpeed;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;

    public float scaleSpeed = 4f;

    public float smallScale = 1f;
    public float bigScale = 2f;

    private bool isSmall = true;
    private float scale;

    private bool isScaling = false;

    private float cameraBaseSize;

    public GameObject bulletPrefab;

    public float shotCooldown = 0.5f;
    private float shotCooldownEndTime = 0f;

    public float maxBoostTime = 1f;
    private float timeBoosted = 0f;
    private bool isBoosting = false;

    public float boostMaxSpeed = 50f;
    public float boostAcceleration = 100f;

    public GameObject cooldownImage;

    public static Transform instance;

    public GameObject thrusterLeft; // for the fire coming out of the thrusters
    public GameObject thrusterRight; // for the fire coming out of the thrusters

    public Transform cannon;
    public Transform cannonCenter;
    public Transform endOfBarrel;

    private AudioSource audioSource;

    public AudioSource cannonFireAudioSource;
    public AudioSource thrusterAudioSource;
    public AudioClip normalThrusterAudioClip;
    public AudioClip boostingThrusterAudioClip;

    public GameObject healthIndicator;
    public int maxHealth = 3;
    private int health;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        cameraBaseSize = Camera.main.orthographicSize;

        instance = transform;

        isScaling = false;
        isSmall = true;
        scale = smallScale;
        RefreshScales();

        health = maxHealth;

        isBoosting = false;
        thrusterAudioSource.clip = normalThrusterAudioClip;
    }

    void Update()
    {
        // turn
        float x = Input.GetAxisRaw("Horizontal");
        if (x != 0)
        {
            float angle = x * turnSpeed * Time.deltaTime;
            transform.Rotate(-Vector3.forward, angle);
        }

        // move
        float y = Input.GetAxisRaw("Vertical");
        if (y > 0)
        {
            float speed = rb2d.velocity.magnitude;
            speed = Mathf.Clamp(speed + acceleration * Time.deltaTime, 0, maxSpeed);
            Vector3 direction = transform.up;
            rb2d.velocity = direction * speed;
        }
        else if (y < 0)
        {
            float speed = rb2d.velocity.magnitude;
            speed = Mathf.Clamp(speed - deceleration * Time.deltaTime, 0, maxSpeed);
            Vector3 direction = transform.up;
            rb2d.velocity = direction * speed;
        }

        thrusterLeft.SetActive(y > 0 || x > 0);
        thrusterRight.SetActive(y > 0 || x < 0);
        thrusterLeft.GetComponent<Animator>().SetBool("isBoosting", isBoosting);
        thrusterRight.GetComponent<Animator>().SetBool("isBoosting", isBoosting);

        if ((x != 0 || y > 0) && !thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
        else if (!(x != 0 || y > 0) && thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Stop();
        }

        // face cannon towards mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toMouse = mousePos - cannonCenter.position;
        if (toMouse.sqrMagnitude > 0)
        {
            float angle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg;
            cannonCenter.rotation = Quaternion.Euler(0, 0, angle - 90);
        }

        // scale
        if (Input.GetButtonDown("ScalePlayer"))
        {
            timeBoosted = 0;
            isBoosting = false;
            thrusterAudioSource.clip = normalThrusterAudioClip;

            shotCooldownEndTime = 0;

            isScaling = true;
        }

        if (isScaling)
        {
            float scaleAmount = scaleSpeed * Time.deltaTime;
            scaleAmount *= isSmall ? 1 : -1;
            scale = Mathf.Clamp(scale + scaleAmount, smallScale, bigScale);
            RefreshScales();

            // will only trigger when complete, since this is after scale has been incremented/decremented
            if (scale == smallScale || scale == bigScale)
            {
                isSmall = scale == smallScale;
                isScaling = false;
            }
        }

        // use ability (dash/fire)
        if (!isScaling && !isSmall && Time.time >= shotCooldownEndTime && Input.GetButtonDown("Fire1"))
        {
            shotCooldownEndTime = Time.time + shotCooldown;
            PewPew();
        }
        else if (!isScaling && isSmall)
        {
            if (Input.GetButtonDown("Fire1") && timeBoosted < maxBoostTime)
            {
                isBoosting = true;
                thrusterAudioSource.clip = boostingThrusterAudioClip;

                maxSpeed = boostMaxSpeed;
                acceleration = boostAcceleration;
            }
            else if (Input.GetButtonUp("Fire1") || (isBoosting && timeBoosted > maxBoostTime))
            {
                isBoosting = false;
                thrusterAudioSource.clip = normalThrusterAudioClip;

                maxSpeed = smallMaxSpeed;
                acceleration = smallAcceleration;
            }
            else if (Input.GetButton("Fire1") && isBoosting && (x != 0 || y > 0)) // only if actually moving
            {
                timeBoosted += Time.deltaTime;
            }
        }

        if (!isBoosting)
        {
            timeBoosted = Mathf.Clamp(timeBoosted - Time.deltaTime, 0, maxBoostTime);
        }

        // visualise cooldown
        if (isSmall && (isBoosting || timeBoosted > 0))
        {
            if (!cooldownImage.activeSelf)
            {
                cooldownImage.SetActive(true);
            }
            float percentageDone = timeBoosted / maxBoostTime;
            cooldownImage.transform.localScale = new(1 - percentageDone, 1);
        }
        else if (!isSmall && shotCooldownEndTime > Time.time)
        {
            if (!cooldownImage.activeSelf)
            {
                cooldownImage.SetActive(true);
            }
            float percentageRemaining = (shotCooldownEndTime - Time.time) / shotCooldown;
            cooldownImage.transform.localScale = new(1 - percentageRemaining, 1);
        }
        else if (cooldownImage.activeSelf)
        {
            cooldownImage.SetActive(false);
        }
    }

    private void PewPew()
    {
        cannon.GetComponent<AudioSource>().Play(); // play fire sound effect

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - cannonCenter.position;
        GameObject bulletObject = Instantiate(bulletPrefab, endOfBarrel.position, Quaternion.identity);
        bulletObject.GetComponent<Bullet>().direction = direction.normalized;
    }

    private void RefreshScales()
    {
        transform.localScale = Vector3.one * scale;

        float transformationCompleted = (scale - smallScale) / (bigScale - smallScale); // [0,1]

        cannon.localScale = Vector3.one * transformationCompleted;

        float cameraScale = 1 + transformationCompleted / 2; // [1,1.5]
        Camera.main.orthographicSize = cameraBaseSize * cameraScale;

        maxSpeed = smallMaxSpeed + (bigMaxSpeed - smallMaxSpeed) * transformationCompleted;
        acceleration = smallAcceleration + (bigAcceleration - smallAcceleration) * transformationCompleted;
        deceleration = smallDeceleration + (bigDeceleration - smallDeceleration) * transformationCompleted;
        turnSpeed = smallTurnSpeed + (bigTurnSpeed - smallTurnSpeed) * transformationCompleted;
    }

    public void Hit()
    {
        health--;
        audioSource.Play();

        if (health > 0)
        {
            float percentageHealth = (float)health / maxHealth;
            healthIndicator.transform.localScale = new(percentageHealth, 1);
        }
        else
        {
            LevelController.GameOver();
        }
    }
}

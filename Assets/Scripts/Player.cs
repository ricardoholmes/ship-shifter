using System.Collections;
using System.Collections.Generic;
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

    public float maxBoostTime = 0.5f;
    private float timeBoosted = 0f;
    private bool isBoosting = false;

    public float boostMaxSpeed = 50f;
    public float boostAcceleration = 100f;

    public GameObject cooldownImage;

    public static Transform instance;

    public GameObject thrusterLeft; // for the fire coming out of the thrusters
    public GameObject thrusterRight; // for the fire coming out of the thrusters

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        cameraBaseSize = Camera.main.orthographicSize;

        instance = transform;

        isScaling = false;
        isSmall = true;
        scale = smallScale;
        RefreshScales();
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

        // scale
        if (Input.GetButtonDown("ScalePlayer"))
        {
            timeBoosted = 0;
            isBoosting = false;

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
                thrusterLeft.GetComponent<Animator>().SetBool("isBoosting", true);
                thrusterRight.GetComponent<Animator>().SetBool("isBoosting", true);

                maxSpeed = boostMaxSpeed;
                acceleration = boostAcceleration;
            }
            else if (Input.GetButtonUp("Fire1") || (isBoosting && timeBoosted > maxBoostTime))
            {
                isBoosting = false;
                thrusterLeft.GetComponent<Animator>().SetBool("isBoosting", false);
                thrusterRight.GetComponent<Animator>().SetBool("isBoosting", false);

                maxSpeed = smallMaxSpeed;
                acceleration = smallAcceleration;
            }
            else if (Input.GetButton("Fire1") && isBoosting)
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
            cooldownImage.transform.localScale = new(percentageRemaining, 1);
        }
        else if (cooldownImage.activeSelf)
        {
            cooldownImage.SetActive(false);
        }
    }

    private void PewPew()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bulletObject.GetComponent<Bullet>().direction = direction.normalized;
    }

    private void RefreshScales()
    {
        transform.localScale = Vector3.one * scale;

        float transformationCompleted = (scale - smallScale) / (bigScale - smallScale); // [0,1]

        float cameraScale = 1 + transformationCompleted / 2; // [1,1.5]
        Camera.main.orthographicSize = cameraBaseSize * cameraScale;

        maxSpeed = smallMaxSpeed + (bigMaxSpeed - smallMaxSpeed) * transformationCompleted;
        acceleration = smallAcceleration + (bigAcceleration - smallAcceleration) * transformationCompleted;
        deceleration = smallDeceleration + (bigDeceleration - smallDeceleration) * transformationCompleted;
        turnSpeed = smallTurnSpeed + (bigTurnSpeed - smallTurnSpeed) * transformationCompleted;
    }

    public static void Kill()
    {
        Debug.Log("ded *skull_emoji*x5");
        LevelController.GameOver();
    }
}

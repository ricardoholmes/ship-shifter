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

    public float smallScale = 1;
    public float bigScale = 2;

    private bool isSmall = true;
    private float scale;

    private bool isScaling = false;

    private float cameraBaseSize;

    public GameObject bulletPrefab;
    public float dashDistance = 5f;

    public float cooldown = 0.5f; // same for dash and fire

    private float cooldownEndTime = 0;

    public GameObject cooldownImage;

    public static Transform instance;

    public GameObject thrusterLeft; // for the fire coming out of the thrusters
    public GameObject thrusterRight; // for the fire coming out of the thrusters

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        cameraBaseSize = Camera.main.orthographicSize;

        instance = transform;

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
        if (!isScaling && Time.time >= cooldownEndTime && Input.GetButtonDown("Fire1"))
        {
            cooldownEndTime = Time.time + cooldown;
            if (isSmall)
            {
                Dash();
            }
            else
            {
                PewPew();
            }
        }

        // visualise cooldown
        if (cooldownEndTime > Time.time)
        {
            if (!cooldownImage.activeSelf)
            {
                cooldownImage.SetActive(true);
            }
            float percentageRemaining = (cooldownEndTime - Time.time) / cooldown;
            cooldownImage.transform.localScale = new(percentageRemaining, 1);
        }
        else if (cooldownImage.activeSelf)
        {
            cooldownImage.SetActive(false);
        }
    }

    private void Dash()
    {
        Vector2 direction = rb2d.velocity;
        if (direction.sqrMagnitude == 0)
        {
            direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        }
        Vector3 dashVector = direction.normalized * dashDistance;
        transform.position += dashVector;
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

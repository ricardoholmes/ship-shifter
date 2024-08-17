using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public float smallSpeed = 20f;
    public float bigSpeed = 10f;
    private float speed;

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

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        cameraBaseSize = Camera.main.orthographicSize;

        instance = transform;

        scale = smallScale;
        transform.localScale = Vector3.one * scale;
        isSmall = true;
        speed = smallSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 direction = new(x, y);
        rb2d.velocity = speed * direction.normalized;

        if (Input.GetButtonDown("ScalePlayer"))
        {
            isScaling = true;
        }

        if (isSmall)
        {
            UpdateSmall();
        }
        else
        {
            UpdateBig();
        }

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

    private void UpdateSmall()
    {
        if (isScaling)
        {
            scale = Mathf.Clamp(scale + (scaleSpeed * Time.deltaTime), smallScale, bigScale);
            RefreshScales();

            if (scale == bigScale)
            {
                isSmall = false;
                isScaling = false;
            }
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= cooldownEndTime)
        {
            Dash();
            cooldownEndTime = Time.time + cooldown;
        }
    }

    private void UpdateBig()
    {
        if (isScaling)
        {
            scale = Mathf.Clamp(scale - (scaleSpeed * Time.deltaTime), smallScale, bigScale);
            RefreshScales();

            if (scale == smallScale)
            {
                isSmall = true;
                isScaling = false;
            }
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= cooldownEndTime)
        {
            PewPew();
            cooldownEndTime = Time.time + cooldown;
        }
    }

    private void Dash()
    {
        Vector2 direction = rb2d.velocity;
        if (direction.sqrMagnitude == 0)
        {
            direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - base.transform.position;
        }
        Vector3 dashVector = direction.normalized * dashDistance;
        base.transform.position += dashVector;
    }

    private void PewPew()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - base.transform.position;
        GameObject bulletObject = Instantiate(bulletPrefab, base.transform.position, Quaternion.identity);
        bulletObject.GetComponent<Bullet>().direction = direction.normalized;
    }

    private void RefreshScales()
    {
        base.transform.localScale = Vector3.one * scale;

        float cameraScale = (scale + smallScale) / 2;
        Camera.main.orthographicSize = cameraBaseSize * cameraScale;

        speed = smallSpeed + (bigSpeed - smallSpeed) * (scale - smallScale) / (bigScale - smallScale);
    }

    public static void Kill()
    {
        Debug.Log("ded *skull_emoji*x5");
    }
}

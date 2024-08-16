using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cameraBaseSize = Camera.main.orthographicSize;

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
        rb.velocity = speed * direction.normalized;

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
    }

    private void RefreshScales()
    {
        transform.localScale = Vector3.one * scale;

        float cameraScale = (scale + smallScale) / 2;
        Camera.main.orthographicSize = cameraBaseSize * cameraScale;

        speed = smallSpeed + (bigSpeed - smallSpeed) * (scale - smallScale) / (bigScale - smallScale);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    public float smallSpeed = 20f;
    public float bigSpeed = 10f;

    public float scaleSpeed = 4f;

    public float smallScale = 1;
    public float bigScale = 2;

    private bool isSmall = true;
    private float scale;

    private bool scaling = false;

    private float cameraBaseSize;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cameraBaseSize = Camera.main.orthographicSize;

        scale = smallScale;
        transform.localScale = Vector3.one * scale;
        isSmall = true;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 direction = new(x, y);
        rb.velocity = (isSmall ? smallSpeed : bigSpeed) * direction.normalized;

        if (Input.GetButtonDown("ScalePlayer"))
        {
            scaling = true;
        }

        if (scaling && isSmall)
        {
            scale = Mathf.Clamp(scale + (scaleSpeed * Time.deltaTime), smallScale, bigScale);
            UpdateScales();

            if (scale == bigScale)
            {
                isSmall = false;
                scaling = false;
            }
        }
        else if (scaling && !isSmall)
        {
            scale = Mathf.Clamp(scale - (scaleSpeed * Time.deltaTime), smallScale, bigScale);
            UpdateScales();

            if (scale == smallScale)
            {
                isSmall = true;
                scaling = false;
            }
        }
    }

    private void UpdateScales()
    {
        transform.localScale = Vector3.one * scale;
        Camera.main.orthographicSize = cameraBaseSize * scale;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed = 10f;
    public float jumpForce = 10f;
    public float fallForce = 10f;

    public float maxJumpTime = 1f;

    private float jumpTime = 0;
    private bool isJumping = false;

    private bool isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal") * speed;
        rb.velocity = new(x, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            jumpTime = 0;
            rb.velocity = new(rb.velocity.x, jumpForce);
        }

        if (isJumping && jumpTime < maxJumpTime && Input.GetKey(KeyCode.Space))
        {
            jumpTime += Time.deltaTime;
            rb.velocity = new(rb.velocity.x, jumpForce);
        }

        if (isJumping && (jumpTime >= maxJumpTime || Input.GetKeyUp(KeyCode.Space)))
        {
            isJumping = false;
            rb.velocity = new(rb.velocity.x, 0);
        }

        if (!isJumping && !isGrounded && Input.GetAxisRaw("Vertical") == -1)
        {
            rb.gravityScale = fallForce;
        }
        else
        {
            rb.gravityScale = 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float maxDistance = 100f; // despawns after this distance
    public float speed = 50f;

    [HideInInspector]
    public Vector2 direction;

    private Vector3 origin;
    private Rigidbody2D rb2d;

    void Awake()
    {
        origin = transform.position;
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb2d.velocity = direction * speed;
        Vector3 fromOrigin = transform.position - origin;
        if (fromOrigin.sqrMagnitude > maxDistance * maxDistance) // squared for efficiency (removes sqrt for euclid dist)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}

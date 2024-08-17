using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    public float speed = 10f;

    private Rigidbody2D rb2d;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 targetPos = Player.instance.position;
        Vector3 direction = targetPos - transform.position;
        rb2d.velocity = direction.normalized * speed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public float speed = 7f;
    public float stopApproachDistance = 10f;

    public float shotCooldown = 1f;
    public GameObject bulletPrefab;

    private Rigidbody2D rb2d;
    private float nextShotTime;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 toPlayer = Player.instance.position - transform.position;
        if (toPlayer.sqrMagnitude > stopApproachDistance * stopApproachDistance)
        {
            nextShotTime = 0;
            rb2d.velocity = toPlayer.normalized * speed;
        }
        else if (nextShotTime == 0)
        {
            nextShotTime = Time.time + shotCooldown;
        }
        else if (Time.time >= nextShotTime)
        {
            ShootAtPlayer();
            nextShotTime = Time.time + shotCooldown;
        }
    }

    private void ShootAtPlayer()
    {
        Vector2 direction = Player.instance.position - transform.position;
        GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bulletObject.GetComponent<Bullet>().direction = direction.normalized;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public float speed = 7f;
    public float stopApproachDistance = 10f;

    public float shotCooldown = 1f;
    public GameObject bulletPrefab;

    private Rigidbody2D rb2d;
    private float nextShotTime;

    public Transform endOfBarrel;
    public AudioSource shotAudioSource;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 toPlayer = Player.instance.position - transform.position;

        float angle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

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
        shotAudioSource.Play();

        Vector2 direction = Player.instance.position - transform.position;
        GameObject bulletObject = Instantiate(bulletPrefab, endOfBarrel.position, Quaternion.identity);
        bulletObject.GetComponent<Bullet>().direction = direction.normalized;
    }
}

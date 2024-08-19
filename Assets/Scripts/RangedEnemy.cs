using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : Enemy
{
    public float speed = 7f;
    public float stopApproachDistance = 10f;
    public float firingDistance = 20f;

    public float shotCooldown = 1f;
    public GameObject bulletPrefab;

    private float nextShotTime = 0;

    public Transform endOfBarrel;
    public AudioSource shotAudioSource;

    void Update()
    {
        agent.SetDestination(Player.instance.position);

        Vector3 toPlayer = Player.instance.position - transform.position;

        float angle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        if (toPlayer.sqrMagnitude > firingDistance * firingDistance)
        {
            nextShotTime = 0;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : Enemy
{
    void Update()
    {
        Vector3 targetPos = Player.instance.position;
        //Vector3 direction = targetPos - transform.position;
        //rb2d.velocity = direction.normalized * speed;
        agent.SetDestination(targetPos);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.Hit();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int pointsOnDeath = 5;

    [Range(0, 1)]
    public float timeDropProbability = 0.05f;
    public GameObject extraTimePickupPrefab;

    public RectTransform healthIndicator;
    private Vector2 healthIndicatorStartSize;

    public int maxHealth = 3;
    private int health;

    protected NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = maxHealth;

        healthIndicatorStartSize = healthIndicator.sizeDelta;
        healthIndicator.gameObject.SetActive(false);
    }

    public void Hit()
    {
        GetComponent<AudioSource>().Play();

        health--;

        if (health > 0)
        {
            StartCoroutine(ShowHealthIndicator());
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        EnemySpawner.DecrementLivingEnemies();

        LevelController.IncrementScore(pointsOnDeath);
        LevelController.ShowScoreGained(pointsOnDeath, transform.position);

        if (Random.value <= timeDropProbability)
        {
            Instantiate(extraTimePickupPrefab, transform.position, Quaternion.identity, null);
        }

        Destroy(gameObject, 1);

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        enabled = false;
    }

    private IEnumerator ShowHealthIndicator()
    {
        healthIndicator.gameObject.SetActive(true);
        healthIndicator.sizeDelta = healthIndicatorStartSize * new Vector2((float)health / maxHealth, 1);

        int h = health;
        yield return new WaitForSeconds(2f);

        if (h == health)
        {
            healthIndicator.gameObject.SetActive(false);
        }
    }
}

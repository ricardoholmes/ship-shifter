using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int pointsOnDeath = 5;

    [Range(0, 1)]
    public float timeDropProbability = 0.05f;
    public GameObject extraTimePickupPrefab;

    public void Kill()
    {
        LevelController.IncrementScore(pointsOnDeath);

        if (Random.value <= timeDropProbability)
        {
            Instantiate(extraTimePickupPrefab, transform.position, Quaternion.identity, null);
        }

        Destroy(gameObject);
    }
}

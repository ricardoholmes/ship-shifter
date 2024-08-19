using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraTimePickup : MonoBehaviour
{
    public int timeIncreaseSeconds = 5;
    public int pointsOnPickup = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelController.IncrementScore(pointsOnPickup);
            LevelController.ShowScoreGained(pointsOnPickup, transform.position);

            LevelController.AddTime(timeIncreaseSeconds);

            GetComponent<AudioSource>().Play();
            Destroy(gameObject, 1);
            enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private int score = 0;

    private static LevelController instance;

    private void Start()
    {
        if (instance != null)
        {
            Debug.Log("Multiple instances of LevelController found");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public static void IncrementScore(int increment)
    {
        instance.score += increment;
        instance.scoreText.text = increment.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeLeftText;

    private int score;

    public float startTimeRemaining = 120;
    private static float timeRemaining;

    private static LevelController instance;

    private void Start()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of LevelController found");
            Destroy(gameObject);
            return;
        }

        instance = this;
        timeRemaining = startTimeRemaining;
        score = 0;
    }

    private void Update()
    {
        timeRemaining -= Time.deltaTime;
        int minutesRemaining = (int)timeRemaining / 60;
        int secondsRemaining = (int)timeRemaining - (minutesRemaining * 60);
        timeLeftText.text = $"{minutesRemaining}:{secondsRemaining:00}";
    }

    public static void IncrementScore(int increment)
    {
        instance.score += increment;
        instance.scoreText.text = instance.score.ToString();
    }

    public static void AddTime(int increment)
    {
        timeRemaining += increment;
    }

    public static void GameOver()
    {
        Debug.Log("game over oopsies :(");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}

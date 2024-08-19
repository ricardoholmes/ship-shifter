using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI recentScoreText;
    public TextMeshProUGUI recentTimeText;

    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI bestTimeText;

    private void Awake()
    {
        int recentScore = PlayerPrefs.GetInt("RecentScore", 0);
        recentScoreText.text = (recentScore * 100).ToString();

        float recentTime = PlayerPrefs.GetFloat("RecentTime", 0);
        int recentTimeMins = (int)recentTime / 60;
        int recentTimeSecs = (int)recentTime - (recentTimeMins * 60);
        recentTimeText.text = $"{recentTimeMins:00}:{recentTimeSecs:00}";

        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestScoreText.text = (bestScore * 100).ToString();

        float bestTime = PlayerPrefs.GetFloat("BestTime", 0);
        int bestTimeMins = (int)bestTime / 60;
        int bestTimeSecs = (int)bestTime - (bestTimeMins * 60);
        bestTimeText.text = $"{bestTimeMins:00}:{bestTimeSecs:00}";
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void Home()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}

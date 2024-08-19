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
        recentScoreText.text = recentScore.ToString();

        int recentTime = PlayerPrefs.GetInt("RecentTime", 0);
        int recentTimeMins = recentTime / 60;
        int recentTimeSecs = recentTime % 60;
        recentTimeText.text = $"{recentTimeMins}:{recentTimeSecs:00}";

        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestScoreText.text = bestScore.ToString();

        int bestTime = PlayerPrefs.GetInt("BestTime", 0);
        int bestTimeMins = bestTime / 60;
        int bestTimeSecs = bestTime % 60;
        bestTimeText.text = $"{bestTimeMins}:{bestTimeSecs:00}";
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

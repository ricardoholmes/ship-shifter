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
    private float timeRemaining;
    private float timeElapsed;

    public Transform worldSpaceCanvas;
    public GameObject scoreGainedTextPrefab;

    private static LevelController instance;

    public GameObject pauseScreen;

    private AudioSource audioSource; // for playing button press sound

    private void Start()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of LevelController found");
            Destroy(gameObject);
            return;
        }

        instance = this;
        audioSource = GetComponent<AudioSource>();
        timeRemaining = startTimeRemaining;
        timeElapsed = 0;
        score = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseScreen.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        timeElapsed += Time.deltaTime;
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            Debug.Log("no more time oopsies :(");
            GameOver();
            return;
        }

        int minutesRemaining = (int)timeRemaining / 60;
        int secondsRemaining = (int)timeRemaining - (minutesRemaining * 60);
        timeLeftText.text = $"{minutesRemaining}:{secondsRemaining:00}";
    }

    public static void ShowScoreGained(int scoreGained, Vector3 worldPosition)
    {
        GameObject prefab = instance.scoreGainedTextPrefab;
        Transform parent = instance.worldSpaceCanvas;
        GameObject pointsGainedObject = Instantiate(prefab, worldPosition, Quaternion.identity, parent);
        TextMeshProUGUI text = pointsGainedObject.GetComponent<TextMeshProUGUI>();
        text.text = (scoreGained * 100).ToString();
    }

    public static void IncrementScore(int increment)
    {
        instance.score += increment;
        instance.scoreText.text = (instance.score * 100).ToString();
    }

    public static void AddTime(int increment)
    {
        instance.timeRemaining += increment;
    }

    public static void GameOver()
    {
        PlayerPrefs.SetInt("RecentScore", instance.score);
        PlayerPrefs.SetFloat("RecentTime", instance.timeElapsed);

        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (instance.score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", instance.score);
        }

        float bestTime = PlayerPrefs.GetFloat("BestTime", 0);
        if (instance.timeElapsed > bestTime)
        {
            PlayerPrefs.SetFloat("BestTime", instance.timeElapsed);
        }

        PlayerPrefs.Save();

        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }

    public void ResumeGame()
    {
        audioSource.Play();

        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    public void Restart()
    {
        audioSource.Play();

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void GoToMainMenu()
    {
        audioSource.Play();

        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}

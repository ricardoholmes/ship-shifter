using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("game");
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }

    public void Exit()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}

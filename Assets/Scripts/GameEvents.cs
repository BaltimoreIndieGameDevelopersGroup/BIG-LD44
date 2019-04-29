using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEvents : MonoBehaviour
{
    public GameObject ScreenPlaying;
    public GameObject ScreenPause;
    public GameObject ScreenLevelComplete;
    public GameObject ScreenGameComplete;
    public GameObject ScreenPlayerDied;

    void Start()
    {
        ScreenPlaying.SetActive(true);
        ScreenPause.SetActive(false);
        ScreenLevelComplete.SetActive(false);
        ScreenGameComplete.SetActive(false);
        ScreenPlayerDied.SetActive(false);
    }

    void Update()
    {
        
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        ScreenPlaying.SetActive(false);
        ScreenPause.SetActive(true);
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        ScreenPause.SetActive(false);
        ScreenPlaying.SetActive(true);
    }

    public void LevelComplete()
    {
        Time.timeScale = 0f;
        ScreenLevelComplete.SetActive(true);
    }

    public void GameComplete()
    {
        Time.timeScale = 0f;
        ScreenGameComplete.SetActive(true);
    }

    public void PlayerDied()
    {
        Time.timeScale = 0f;
        ScreenPause.SetActive(false);
        ScreenPlayerDied.SetActive(true);
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
            SceneManager.LoadScene("PabloScene");
        else if (SceneManager.GetActiveScene().name == "PabloScene")
            SceneManager.LoadScene("TonyScene");
        else if (SceneManager.GetActiveScene().name == "TonyScene")
            SceneManager.LoadScene("RobScene");
        else if (SceneManager.GetActiveScene().name == "RobScene")
            SceneManager.LoadScene("MainScene");
        Time.timeScale = 1f;
    }

    public void LoadSameLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void ChangeToPlayScene()
    {
        SceneManager.LoadScene("PabloScene");
    }
}

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
        if (ScreenPlaying != null) ScreenPlaying.SetActive(true);
        if (ScreenPause != null) ScreenPause.SetActive(false);
        if (ScreenLevelComplete != null) ScreenLevelComplete.SetActive(false);
        if (ScreenGameComplete != null) ScreenGameComplete.SetActive(false);
        if (ScreenPlayerDied != null) ScreenPlayerDied.SetActive(false);
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

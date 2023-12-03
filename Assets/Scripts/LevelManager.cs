using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static Action<bool> onPauseGame;

    // button event
    public void LoadNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            // first scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            // reload scene
            ReloadScene();
        }
    }

    // button event
    public void QuitGame()
    {
        Application.Quit();
    }

    // button event
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // button event
    public void PauseGame()
    {
        Time.timeScale = 0f;
        onPauseGame?.Invoke(true);
    }

    // button event
    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        onPauseGame?.Invoke(false);
    }
}

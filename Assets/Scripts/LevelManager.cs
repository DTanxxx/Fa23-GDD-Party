using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static Action<bool> onPauseGame;
    public static Action<bool> onNotes;
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

    // pause button event
    public void PauseGame()
    {
        Time.timeScale = 0f;
        onPauseGame?.Invoke(true);
    }

    // pause button event
    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        onPauseGame?.Invoke(false);
    }

    // notebook button event
    public void OpenNote()
    {
        Time.timeScale = 0f;
        onNotes?.Invoke(true);
    }

    // notebook button event
    public void CloseNote()
    {
        Time.timeScale = 1f;
        onNotes?.Invoke(false);
    }

}

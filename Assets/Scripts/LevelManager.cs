using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel = null;

    public static Action<bool> onPauseGame;

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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        onPauseGame?.Invoke(true);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        onPauseGame?.Invoke(false);
    }
}

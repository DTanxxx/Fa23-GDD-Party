using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Lurkers.Audio;

namespace Lurkers.Control.Level
{
    public class LevelManager : MonoBehaviour
    {
        public static Action onPauseGame;
        public static Action onUnpausegame;

        // play main menu theme
        private void Awake()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                AudioManager.instance.Play(FMODEvents.instance.mainMenu, transform);
            }
        }

        // button event
        public void LoadNextScene()
        {
            AudioManager.instance.StopAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
            onPauseGame?.Invoke();
        }

        // pause button event
        public void UnpauseGame()
        {
            Time.timeScale = 1f;
            onUnpausegame?.Invoke();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using Lurkers.Audio;

namespace Lurkers.Control.Level
{
    public class LevelManager : MonoBehaviour
    {
        public static Action<bool> onPauseGame;
        public static Action<bool> onNotes;

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
}

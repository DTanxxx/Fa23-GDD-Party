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

                // if no currLevel PlayerPref key, set continue button to inactive
                if (!PlayerPrefs.HasKey("currLevel"))
                {
                    GameObject continueButton = GameObject.Find("Continue Button");
                    continueButton.SetActive(false);
                }
            }
        }

        // button event
        public void LoadNextScene()
        {
            AudioManager.instance.StopAll();
            // adds "currLevel" PlayerPref key, sets to next level
            PlayerPrefs.SetInt("currLevel", SceneManager.GetActiveScene().buildIndex + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        // button event
        public void Continue()
        {
            // checks for existence of "currLevel" key

            // if non-existent (via corruption, deletion, etc.),
            // goes to level 1 (this shouldn't be possible, but is kept as a contingency)
            int sceneIdx = PlayerPrefs.HasKey("currLevel") ? PlayerPrefs.GetInt("currLevel") : 1;
            SceneManager.LoadScene(sceneIdx);
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

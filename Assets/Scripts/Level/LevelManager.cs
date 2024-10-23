using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Lurkers.Audio;
using Unity.VisualScripting;

namespace Lurkers.Control.Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private float ostDur = 47.5f;

        public static Action onPauseGame;
        public static Action onUnpausegame;

        private static bool mainMenu = true;
        private float timer = 0f;
        private FMOD.Studio.EventInstance inst;

        private static LevelManager instance;
        public static LevelManager Instance
        {
            get
            {
                if (LevelManager.instance == null)
                {
                    LevelManager.instance = FindObjectOfType<LevelManager>();
                }

                return instance;
            }
        }

        // play main menu theme
        private void Awake()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                inst = AudioManager.instance.Play(FMODEvents.instance.mainMenu, transform);
                timer = ostDur;
            }
            else
            {
                mainMenu = false;
            }
        }

        private void Update()
        {
            if (!mainMenu)
            {
                return;
            }

            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                // stop OST and restart
                timer = ostDur;
                AudioManager.instance.Stop(inst);
                inst = AudioManager.instance.Play(FMODEvents.instance.mainMenu, transform);
            }
        }

        // button event
        public void LoadNextScene()
        {
            AudioManager.instance.StopAll();
            mainMenu = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        // button event
        public void QuitGame()
        {
            SceneManager.LoadScene(0);
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

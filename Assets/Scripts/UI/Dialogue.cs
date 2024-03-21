using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Lurkers.Control;
using Lurkers.Control.Vision;
using Lurkers.Environment.Vision;
using Lurkers.Camera;

namespace Lurkers.UI
{
    [System.Serializable]
    public enum DialogueType
    {
        FIRST_ENEMY_ENCOUNTER,
        FIRST_ENEMY_FREEZE,
        FIRST_LEVER_PULL,
        INTRO,
        COLOR_TILE_INTRO,

    }

    [System.Serializable]
    public struct DialogueText
    {
        public DialogueType type;
        public string[] lines;
    }

    public class Dialogue : MonoBehaviour
    {
        [SerializeField] private DialogueText[] dialogueOptions;
        [SerializeField] private GameObject uiContainer = null;

        public TextMeshProUGUI text;
        public string[] lines;
        public float textSpeed;
        private int index;
        public PlayerController player;

        public static Action active;
        public static Action unactive;

        private void Update()
        {
            if (!uiContainer.activeInHierarchy)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (text.text == lines[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    text.text = lines[index];
                }
            }
        }

        private void OnEnable()
        {
            LightDirection.onFirstEnemyEncounter += OnFirstEnemyEncounter;
            LightDirection.onFirstFreezeEnemy += OnFirstFreezeEnemy;
            CameraFollow.onCameraRestoreComplete += OnFirstLeverPull;
            ElevatorOpen.onFirstTimeClose += OnFirstTimeElevatorClose;
            ColorTileDialogueTrigger.onColorTileIntro += OnColorTileIntro;
        }

        private void OnDisable()
        {
            LightDirection.onFirstEnemyEncounter -= OnFirstEnemyEncounter;
            LightDirection.onFirstFreezeEnemy -= OnFirstFreezeEnemy;
            CameraFollow.onCameraRestoreComplete -= OnFirstLeverPull;
            ElevatorOpen.onFirstTimeClose -= OnFirstTimeElevatorClose;
            ColorTileDialogueTrigger.onColorTileIntro -= OnColorTileIntro;
        }

        private void DisplayDialogue()
        {
            index = 0;
            if (player)
            {
                player.inDialogue = true;
            }
            active?.Invoke();
            text.text = "";
            StartCoroutine(TypeLine());
        }
        
        private IEnumerator TypeLine()
        {
            foreach (char c in lines[index].ToCharArray())
            {
                text.text += c;
                yield return new WaitForSeconds(1f / textSpeed);
            }
        }

        private void NextLine()
        {
            if (index < lines.Length - 1)
            {
                index++;
                text.text = "";
                StartCoroutine(TypeLine());
            }
            else
            {
                player.inDialogue = false;
                uiContainer.SetActive(false);
                unactive?.Invoke();
            }
        }

        private void OnFirstEnemyEncounter()
        {
            uiContainer.SetActive(true);
            InitDialogueState(DialogueType.FIRST_ENEMY_ENCOUNTER);
            DisplayDialogue();
        }

        private void OnFirstFreezeEnemy()
        {
            uiContainer.SetActive(true);
            InitDialogueState(DialogueType.FIRST_ENEMY_FREEZE);
            DisplayDialogue();
        }

        private void OnFirstLeverPull()
        {
            uiContainer.SetActive(true);
            InitDialogueState(DialogueType.FIRST_LEVER_PULL);
            DisplayDialogue();
        }

        private void OnFirstTimeElevatorClose()
        {
            uiContainer.SetActive(true);
            InitDialogueState(DialogueType.INTRO);
            DisplayDialogue();
        }

        private void OnColorTileIntro()
        {
            uiContainer.SetActive(true);
            InitDialogueState(DialogueType.COLOR_TILE_INTRO);
            DisplayDialogue();
        }

        private void InitDialogueState(DialogueType type)
        {
            foreach (var option in dialogueOptions)
            {
                if (option.type == type)
                {
                    lines = new string[option.lines.Length];
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        lines[i] = option.lines[i];
                    }
                    return;
                }
            }
        }
    }
}

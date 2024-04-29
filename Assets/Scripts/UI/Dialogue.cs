using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Lurkers.Control;
using Lurkers.Control.Vision;
using Lurkers.Environment.Vision;
using Lurkers.Environment.Hearing;
using Lurkers.Cam;
using Lurkers.UI.Hearing;
using Lurkers.UI.Vision;
using Lurkers.Event;

namespace Lurkers.UI
{
    [System.Serializable]
    public enum DialogueType
    {
        NONE,
        FIRST_ENEMY_ENCOUNTER,
        FIRST_ENEMY_FREEZE,
        FIRST_LEVER_PULL,
        INTRO,
        COLOR_TILE_INTRO,
        FIRST_KEY_PICKUP,
        FIRST_FLICKER,
        PICKUP_CASSETTE,
        PICKUP_ECHO,
        PICKUP_NOTEBOOK,
        FLASHLIGHT_BREAK,
        PICKUP_CAM,
        FIRST_TIME_FIND_CASSETTE,
        FIRST_TIME_PLAY_CASSETTE,
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
        private DialogueType curType = DialogueType.NONE;
        public PlayerController player;

        public static Action<DialogueType> active;
        public static Action<DialogueType> unactive;

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
            Vault.onFirstTimeOpen += OnFirstTimeKeyPickup;
            FlickerTrigger.onFlashlightFlickerFirstTime += OnFirstTimeFlicker;
            PlayerAnimationEvents.onFlashlightBreak += OnFlashlightBreak;
            SecurityCamEnabler.onFirstTimeCamPickup += OnPickupSecurityCam;
            CassetteStation.onFirstTimeCassette += OnFirstTimeCassette;
            CassetteAudio.onFirstTimePlayCassette += OnFirstTimePlayCassette;

            AuditoryUIEnabler.onCorpseCollide += OnPickupCassette;
            AuditoryUIEnabler.onCassetteFinish += OnPickupEcholocator;
            AuditoryUIEnabler.onEcholocatorFinish += OnPickupNotebook;
        }

        private void OnDisable()
        {
            LightDirection.onFirstEnemyEncounter -= OnFirstEnemyEncounter;
            LightDirection.onFirstFreezeEnemy -= OnFirstFreezeEnemy;
            CameraFollow.onCameraRestoreComplete -= OnFirstLeverPull;
            ElevatorOpen.onFirstTimeClose -= OnFirstTimeElevatorClose;
            ColorTileDialogueTrigger.onColorTileIntro -= OnColorTileIntro;
            Vault.onFirstTimeOpen -= OnFirstTimeKeyPickup;
            FlickerTrigger.onFlashlightFlickerFirstTime -= OnFirstTimeFlicker;
            PlayerAnimationEvents.onFlashlightBreak -= OnFlashlightBreak;
            SecurityCamEnabler.onFirstTimeCamPickup -= OnPickupSecurityCam;
            CassetteStation.onFirstTimeCassette -= OnFirstTimeCassette;
            CassetteAudio.onFirstTimePlayCassette -= OnFirstTimePlayCassette;

            AuditoryUIEnabler.onCorpseCollide -= OnPickupCassette;
            AuditoryUIEnabler.onCassetteFinish -= OnPickupEcholocator;
            AuditoryUIEnabler.onEcholocatorFinish -= OnPickupNotebook;
        }

        private void DisplayDialogue()
        {
            index = 0;
            if (player)
            {
                player.inDialogue = true;
            }
            active?.Invoke(curType);
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
                unactive?.Invoke(curType);
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

        private void OnFirstTimeKeyPickup()
        {
            uiContainer.SetActive(true);
            InitDialogueState(DialogueType.FIRST_KEY_PICKUP);
            DisplayDialogue();
        }

        private void OnFirstTimeFlicker()
        {
            uiContainer.SetActive(true);
            InitDialogueState(DialogueType.FIRST_FLICKER);
            DisplayDialogue();
        }

        private void OnPickupCassette()
        {
            uiContainer.SetActive(true);
            InitDialogueState(DialogueType.PICKUP_CASSETTE);
            DisplayDialogue();
        }

        private void OnPickupEcholocator()
        {
            uiContainer.SetActive(true);
            InitDialogueState(DialogueType.PICKUP_ECHO);
            DisplayDialogue();
        }

        private void OnPickupNotebook()
        {
            uiContainer.SetActive(true);
            InitDialogueState(DialogueType.PICKUP_NOTEBOOK);
            DisplayDialogue();
        }

        private void OnFlashlightBreak()
        {
            uiContainer.SetActive(true);
            InitDialogueState(DialogueType.FLASHLIGHT_BREAK);
            DisplayDialogue();
        }

        private void OnPickupSecurityCam()
        {
            uiContainer.SetActive(true);
            InitDialogueState(DialogueType.PICKUP_CAM);
            DisplayDialogue();
        }

        private void OnFirstTimeCassette()
        {
            uiContainer.SetActive(true);
            InitDialogueState(DialogueType.FIRST_TIME_FIND_CASSETTE);
            DisplayDialogue();
        }

        private void OnFirstTimePlayCassette()
        {
            uiContainer.SetActive(true);
            InitDialogueState(DialogueType.FIRST_TIME_PLAY_CASSETTE);
            DisplayDialogue();
        }

        private void InitDialogueState(DialogueType type)
        {
            curType = type;

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

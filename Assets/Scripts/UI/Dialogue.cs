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
using Unity.VisualScripting;

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
            if (Input.GetKeyDown(KeyCode.P)) // debugging for setting new game dialogue state 
            {
                PlayerPrefs.DeleteAll(); // deletes all presave prefenece dialogue options 

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

        private string OnFirstEnemyEncounterBoolean = "IntroDialogueState";
        private string OnFirstFreezeEnemyBoolean = "IntroDialogueState";
        private string OnFirstLeverPullBoolean = "IntroDialogueState";
        private string OnFirstTimeElevatorCloseBoolean = "IntroDialogueState";
        private string OnColorTileIntroBoolean = "IntroDialogueState";
        private string OnFirstTimeKeyPickupBoolean = "IntroDialogueState";
        private string OnFirstTimeFlickerBoolean = "IntroDialogueState";
        private string OnPickupCassetteBoolean = "IntroDialogueState";
        private string OnFirstTimePlayCassetteBoolean = "IntroDialogueState";
        private string OnPickupEcholocatorBoolean = "IntroDialogueState";
        private string OnPickupNotebookBoolean = "IntroDialogueState";
        private string OnFlashlightBreakBoolean = "IntroDialogueState";
        private string OnPickupSecurityCamBoolean = "IntroDialogueState";
        private string OnFirstTimeCassetteBoolean = "IntroDialogueState";


        private void OnFirstEnemyEncounter()
        {
            // if not displayed 
            if (PlayerPrefs.GetString(OnFirstEnemyEncounterBoolean, "") == "")
            {
                uiContainer.SetActive(true);
                InitDialogueState(DialogueType.FIRST_ENEMY_ENCOUNTER);
                DisplayDialogue();

                PlayerPrefs.SetString(OnFirstEnemyEncounterBoolean, "true");
                // update displayed = True 
            }
        }

        private void OnFirstFreezeEnemy()
        {
            // if not displayed 
            if (PlayerPrefs.GetString(OnFirstFreezeEnemyBoolean, "") == "")
            {
                uiContainer.SetActive(true);
                InitDialogueState(DialogueType.FIRST_ENEMY_FREEZE);
                DisplayDialogue();
                PlayerPrefs.SetString(OnFirstEnemyEncounterBoolean, "true");
                // update displayed = True 
            }
        }

        private void OnFirstLeverPull()
        {
            if (PlayerPrefs.GetString(OnFirstLeverPullBoolean, "") == "")
            {
                uiContainer.SetActive(true);
                InitDialogueState(DialogueType.FIRST_LEVER_PULL);
                DisplayDialogue();
                PlayerPrefs.SetString(OnFirstLeverPullBoolean, "true");
                // update displayed = True 
            }
        }

        private void OnFirstTimeElevatorClose()
        {
            if (PlayerPrefs.GetString(OnFirstTimeElevatorCloseBoolean, "") == "")
            {
                uiContainer.SetActive(true);
                InitDialogueState(DialogueType.INTRO);
                DisplayDialogue();
                PlayerPrefs.SetString("true", OnFirstTimeElevatorCloseBoolean);
            }
        }

        private void OnColorTileIntro()
        {
            if (PlayerPrefs.GetString(OnColorTileIntroBoolean, "") == "")
            {
                uiContainer.SetActive(true);
                InitDialogueState(DialogueType.COLOR_TILE_INTRO);
                DisplayDialogue();
                PlayerPrefs.SetString(OnColorTileIntroBoolean, "true");
                // update displayed = True 
            }
        }

        private void OnFirstTimeKeyPickup()
        {
            if (PlayerPrefs.GetString(OnFirstTimeKeyPickupBoolean, "") == "")
            {
                uiContainer.SetActive(true);
                InitDialogueState(DialogueType.FIRST_KEY_PICKUP);
                DisplayDialogue();
                PlayerPrefs.SetString(OnFirstTimeKeyPickupBoolean, "true");
                // update displayed = True 
            }
        }

        private void OnFirstTimeFlicker()
        {
            if (PlayerPrefs.GetString(OnFirstTimeFlickerBoolean, "") == "")
            {
                uiContainer.SetActive(true);
                InitDialogueState(DialogueType.FIRST_FLICKER);
                DisplayDialogue();
                PlayerPrefs.SetString(OnFirstTimeFlickerBoolean, "true");
                // update displayed = True 
            }
        }

        private void OnPickupCassette()
        {
            if (PlayerPrefs.GetString(OnPickupCassetteBoolean, "") == "")
            {
                uiContainer.SetActive(true);
                InitDialogueState(DialogueType.PICKUP_CASSETTE);
                DisplayDialogue();
                PlayerPrefs.SetString(OnPickupCassetteBoolean, "true");
                // update displayed = True 
            }
        }

        private void OnPickupEcholocator()
        {
            if (PlayerPrefs.GetString(OnPickupEcholocatorBoolean, "") == "")
            {
                uiContainer.SetActive(true);
                InitDialogueState(DialogueType.PICKUP_ECHO);
                DisplayDialogue();
                PlayerPrefs.SetString(OnPickupEcholocatorBoolean, "true");
                // update displayed = True 
            }
        }

        private void OnPickupNotebook()
        {
            if (PlayerPrefs.GetString(OnPickupNotebookBoolean, "") == "")
            {
                uiContainer.SetActive(true);
                InitDialogueState(DialogueType.PICKUP_NOTEBOOK);
                DisplayDialogue();
                PlayerPrefs.SetString(OnPickupNotebookBoolean, "true");
                // update displayed = True
            }
        }

        private void OnFlashlightBreak()
        {
            if (PlayerPrefs.GetString(OnFlashlightBreakBoolean, "") == "")
            {
                uiContainer.SetActive(true);
                InitDialogueState(DialogueType.FLASHLIGHT_BREAK);
                DisplayDialogue();
                PlayerPrefs.SetString(OnFlashlightBreakBoolean, "true");
                // update displayed = True 
            }
        }

        private void OnPickupSecurityCam()
        {
            if (PlayerPrefs.GetString(OnPickupSecurityCamBoolean, "") == "")
            {
                uiContainer.SetActive(true);
                InitDialogueState(DialogueType.PICKUP_CAM);
                DisplayDialogue();
                PlayerPrefs.SetString(OnPickupSecurityCamBoolean, "true");
                // update displayed = True 
            }
        }

        private void OnFirstTimeCassette()
        {
            if (PlayerPrefs.GetString(OnFirstTimeCassetteBoolean, "") == "")
            {
                uiContainer.SetActive(true);
                InitDialogueState(DialogueType.FIRST_TIME_FIND_CASSETTE);
                DisplayDialogue();
                PlayerPrefs.SetString(OnFirstTimeCassetteBoolean, "true");
                // update displayed = True 
            }
        }

        private void OnFirstTimePlayCassette()
        {
            if (PlayerPrefs.GetString(OnFirstTimePlayCassetteBoolean, "") == "")
            {
                uiContainer.SetActive(true);
                InitDialogueState(DialogueType.FIRST_TIME_PLAY_CASSETTE);
                DisplayDialogue();
                PlayerPrefs.SetString(OnFirstTimePlayCassetteBoolean, "true");
                // update displayed = True 
            }
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

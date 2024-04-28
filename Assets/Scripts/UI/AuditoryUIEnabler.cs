using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Lurkers.UI.Hearing
{
    public class AuditoryUIEnabler : MonoBehaviour
    {
        [SerializeField] private GameObject cassetteButton = null;
        [SerializeField] private GameObject echolocatorButton = null;
        [SerializeField] private GameObject notebookButton = null;

        private bool ended = false;

        public static Action onCorpseCollide;
        public static Action onCassetteFinish;
        public static Action onEcholocatorFinish;

        private void OnEnable()
        {
            Dialogue.unactive += EnableUI;
        }

        private void OnDisable()
        {
            Dialogue.unactive -= EnableUI;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!ended)
            {
                ended = true;
                onCorpseCollide?.Invoke();
            }
        }

        private void ShowEcholocator()
        {
            onCassetteFinish?.Invoke();
        }

        private void ShowNotebook()
        {
            onEcholocatorFinish?.Invoke();
        }

        private void EnableUI(DialogueType type)
        {
            switch (type)
            {
                case DialogueType.PICKUP_CASSETTE:
                    cassetteButton.SetActive(true);
                    Invoke("ShowEcholocator", 1.0f);
                    break;
                case DialogueType.PICKUP_ECHO:
                    echolocatorButton.SetActive(true);
                    Invoke("ShowNotebook", 1.0f);
                    break;
                case DialogueType.PICKUP_NOTEBOOK:
                    notebookButton.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}

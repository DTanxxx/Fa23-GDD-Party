using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lurkers.Audio;

namespace Lurkers.UI.Hearing
{
    public class NotebookPanel : MonoBehaviour
    {
        [SerializeField] private Button openButton = null;
        [SerializeField] private Button closeButton = null;
        [SerializeField] private Animator animator;
        [SerializeField] private int maxPages = 5;
        [SerializeField] private TextMeshProUGUI pageNumber;

        private CanvasGroup canvasGroup;
        public TMP_InputField inputFieldChat;
        private int currPage = 0;
        private Dictionary<int, string> notes = new Dictionary<int, string>();
        private Button[] buttons;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            inputFieldChat = GetComponentInChildren<TMP_InputField>();
            buttons = GetComponentsInChildren<Button>();
            for (int i = 0; i < maxPages; i++)
            {
                notes.Add(i, "");
            }
            pageNumber.text = CurrentPage();
        }

        // notebook button event
        public void OpenNote()
        {
            Time.timeScale = 0f;
            Color bColor = openButton.colors.disabledColor;
            bColor.a = 0f;
            openButton.interactable = false;
            ShowPanel(true);
        }

        // notebook button event
        public void CloseNote()
        {
            Time.timeScale = 1f;
            closeButton.interactable = true;
            openButton.interactable = true;
            ShowPanel(false);
        }

        private void ShowPanel(bool toPause)
        {
            if (toPause)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                animator.SetTrigger("OpenNotes");
                LoadNotes();
                // TO BE FIXED
                //AudioManager.instance.PlayOneShot(FMODEvents.instance.pageFlip, transform);
                animator.SetTrigger("CloseNotes");
            }
            else
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                SaveNotes();
            }
        }

        public void Flip()
        {
            animator.SetTrigger("FlipNotes");
            SaveNotes();
            if (currPage == maxPages - 1)
            {
                currPage = 0;
            }
            else
            {
                currPage++;
            }
            animator.SetTrigger("CloseNotes");
            // TO BE FIXED
            //AudioManager.instance.PlayOneShot(FMODEvents.instance.pageFlip, transform);
        }

        public void SaveNotes()
        {
            if (notes.TryGetValue(currPage, out string currnotes))
            {
                notes[currPage] = inputFieldChat.text;
            }
        }

        public void LoadNotes()
        {
            pageNumber.text = CurrentPage();
            if (notes.TryGetValue(currPage, out string currnotes))
            {
                inputFieldChat.text = currnotes;
            }
        }

        public void ChangeButtonState()
        {
            foreach (var button in buttons)
            {
                button.interactable = !button.interactable;
            }
        }

        private string CurrentPage()
        {
            int displayNumber = currPage + 1;
            return displayNumber.ToString();
        }
    }
}

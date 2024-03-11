using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.UI.Hearing
{
    public class ShowNextNotes : MonoBehaviour
    {
        private NotebookPanel parent;

        private void Start()
        {
            parent = GetComponentInParent<NotebookPanel>();
        }

        public void LoadPage()
        {
            parent.LoadNotes();
        }

        public void PreventSpam()
        {
            parent.ChangeButtonState();
        }
    }
}

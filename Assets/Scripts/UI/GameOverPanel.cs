using Lurkers.Control.Level;
using Lurkers.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lurkers.UI
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private float fadeInRate = 0.25f;
        [SerializeField] private Button restartButton = null;
        [SerializeField] private Button quitButton = null;

        private CanvasGroup canvasGroup;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            PlayerAnimationEvents.onEndPlayerDeathAnim += ShowPanel;

            restartButton.onClick.AddListener(LevelManager.Instance.ReloadScene);
            quitButton.onClick.AddListener(LevelManager.Instance.QuitGame);
        }

        private void OnDisable()
        {
            PlayerAnimationEvents.onEndPlayerDeathAnim -= ShowPanel;

            restartButton.onClick.RemoveListener(LevelManager.Instance.ReloadScene);
            quitButton.onClick.RemoveListener(LevelManager.Instance.QuitGame);
        }

        private void ShowPanel()
        {
            StartCoroutine(FadeInGameOverPanel());
        }

        private IEnumerator FadeInGameOverPanel()
        {
            yield return new WaitForSeconds(1);
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;

            while ((1f - canvasGroup.alpha) > Mathf.Epsilon)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, fadeInRate);
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }
    }
}

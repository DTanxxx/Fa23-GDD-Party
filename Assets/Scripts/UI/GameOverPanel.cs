using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private float fadeInRate = 0.25f;

    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        PlayerAnimationEvents.onEndPlayerDeathAnim += ShowPanel;
    }

    private void OnDisable()
    {
        PlayerAnimationEvents.onEndPlayerDeathAnim -= ShowPanel;
    }

    private void ShowPanel()
    {
        StartCoroutine(FadeInGameOverPanel());
    }

    private IEnumerator FadeInGameOverPanel()
    {
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

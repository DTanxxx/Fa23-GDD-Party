using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerAnimationEvents : MonoBehaviour
{
    public static Action onFootstep;
    public static Action onEndPlayerDeathAnim;
    public static Action onSkullCrush;

    // animation event
    public void OnEndDeathAnimation()
    {
        onEndPlayerDeathAnim?.Invoke();
    }

    // animation event
    public void OnSkullCrush()
    {
        onSkullCrush?.Invoke();
    }

    // animation event
    public void OnFootstep()
    {
        onFootstep?.Invoke();
    }
}

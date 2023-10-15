using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerAnimationEvents : MonoBehaviour
{
    public static Action onEndPlayerDeathAnim;

    // animation event
    public void OnEndDeathAnimation()
    {
        onEndPlayerDeathAnim?.Invoke();
        // SHOULD BE IN ITS OWN SCRIPT
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

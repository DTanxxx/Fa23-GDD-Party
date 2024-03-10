using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string[] lines;
    public float textSpeed;
    private int index;
    public PlayerMovement player;
    public static Action Active;
    public static Action Unactive;
    // Start is called before the first frame update
    void Start()
    {

        //player = (PlayerMovement) GameObject.FindWithTag("player");
    }
    void Update()
    {

        if(Input.GetMouseButtonDown(0)) 
        {
            if (text.text == lines[index])
            {
                NextLine();
            }
            else 
            {
            StopAllCoroutines();
            text.text= lines[index];
            }
        }
        
    }
    void OnEnable() {
        if (player) {
            player.inDialogue = true;
        }
        Active?.Invoke();
        text.text = "";
        StartCoroutine(TypeLine());
    }
    void StartDialogue() {
        index = 0;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine() {
        foreach (char c in lines[index].ToCharArray()) 
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    void NextLine()
    {
        if(index < lines.Length - 1)
        {
            index++;
            text.text = "";
            StartCoroutine(TypeLine());
        }
        else 
        {
            player.inDialogue = false;
            gameObject.SetActive(false);
            Unactive?.Invoke();
        }
    }
}

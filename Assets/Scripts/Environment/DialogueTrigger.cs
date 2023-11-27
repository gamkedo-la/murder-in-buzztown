using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private bool _hasSpoken;
    public string[] dialogue;

    [SerializeField] DialogueManager _dialogueManagerScript;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_hasSpoken)
        {
            _hasSpoken = true;
            GameManager.Instance.gameState = "in dialogue";
            _dialogueManagerScript._dialogue = dialogue;
            _dialogueManagerScript.StartDialogue();

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzztownPoliceScript : MonoBehaviour
{
    private bool spokenYet = false;
    public string myDialogue1 = "Que pedo detective? This one's fresh, so we don't know anything yet.";
    public string myDialogue2 = "But there was some racket over there past the car and steam tower.";

    [SerializeField] GameObject dialogueManager;
    DialogueManager dialogueManagerScript;

    private void Start()
    {
        dialogueManagerScript = dialogueManager.GetComponent<DialogueManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!spokenYet)
        {
            dialogueManagerScript._dialogue[0] = myDialogue1;
            dialogueManagerScript._dialogue[1] = myDialogue2;
            dialogueManagerScript.StartDialogue();
            spokenYet = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocksPolicemanScript : MonoBehaviour
{
    private bool spokenYet = false;
    public string myDialogue1 = "We've already scoped out the scene here detective... We believe the suspect was here, but we don't have any specific clues.";
    public string myDialogue2 = "Try some of this ginseng tea. It's supposed to be very healthy, and the way we steam it enhances it's benefits.";

    [SerializeField] GameObject dialogueManager;
    DialogueManager dialogueManagerScript;

    private GameObject lifeManager;
    private LifeManager lifeManagerScript;

    private void Start()
    {
        dialogueManagerScript = dialogueManager.GetComponent<DialogueManager>();

        lifeManager = GameObject.FindGameObjectWithTag("Lives");
        lifeManagerScript = lifeManager.GetComponent<LifeManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!spokenYet)
            {
                GameManager.Instance.gameState = "in dialogue";
                dialogueManagerScript._dialogue[0] = myDialogue1;
                dialogueManagerScript._dialogue[1] = myDialogue2;
                dialogueManagerScript.StartDialogue();
                spokenYet = true;

                lifeManagerScript.HandleGinsengTeaPowerup();
            }
        }
    }
}

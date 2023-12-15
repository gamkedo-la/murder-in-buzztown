using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocksPolicemanScript : MonoBehaviour
{
    private bool _hasSpoken;
    public string[] dialogue;
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
        if (collision.gameObject.tag == "Player" && !_hasSpoken)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            collision.GetComponent<PlayerController>().anchoredDialogue = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(false);
            collision.GetComponent<PlayerController>().anchoredDialogue = null;
        }
    }

    public void Talk()
    {
        if (!_hasSpoken)
        {
            _hasSpoken = true;
            GameManager.Instance.gameState = "in dialogue";
            dialogueManagerScript._dialogue = dialogue;
            dialogueManagerScript.StartDialogue();
            lifeManagerScript.HandleGinsengTeaPowerup();
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}

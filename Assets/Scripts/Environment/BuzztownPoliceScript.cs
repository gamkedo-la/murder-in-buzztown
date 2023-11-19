using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzztownPoliceScript : MonoBehaviour
{
    private bool _hasSpoken;
    public string[] dialogue;

    [SerializeField] GameObject dialogueManager;
    DialogueManager dialogueManagerScript;

    private void Start()
    {
        dialogueManagerScript = dialogueManager.GetComponent<DialogueManager>();
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
        }
    }
}

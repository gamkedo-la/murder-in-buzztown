using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzztownPoliceScript : MonoBehaviour
{
    private bool _playerInRange;
    private bool _hasSpoken;
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
        if (collision.gameObject.tag == "Player" && !_hasSpoken)
        {
            _playerInRange = true;
            transform.GetChild(0).gameObject.SetActive(true);
            collision.GetComponent<PlayerController>().anchoredDialogue = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _playerInRange = false;
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
            dialogueManagerScript._dialogue[0] = myDialogue1;
            dialogueManagerScript._dialogue[1] = myDialogue2;
            dialogueManagerScript.StartDialogue();
        }
    }
}

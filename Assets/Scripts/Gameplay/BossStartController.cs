using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStartController : MonoBehaviour
{
    private bool _hasTriggered;
    public string[] dialogue;
    [SerializeField] DialogueManager _dialogueManagerScript;
    [SerializeField] BossSM _boss;
    [SerializeField] GameObject _entranceWall;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_hasTriggered)
        {
            _hasTriggered = true;
            GameManager.Instance.gameState = "in dialogue";
            _dialogueManagerScript._dialogue = dialogue;
            _dialogueManagerScript.StartDialogue(gameObject);
            _boss.gameObject.SetActive(true);
            _entranceWall.SetActive(true);
        }
    }

    public void StartBoss()
    {
        _boss.finishedTalking = true;
    }
}

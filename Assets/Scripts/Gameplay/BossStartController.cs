using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStartController : MonoBehaviour
{
    public bool hasTriggered;
    public string[] dialogue;
    [SerializeField] DialogueManager _dialogueManagerScript;
    [SerializeField] BossSM _boss;
    [SerializeField] GameObject _entranceWall;
    [SerializeField] GameObject _exitWall;
    [SerializeField] GameObject _StaticBox1;
    [SerializeField] GameObject _StaticBox2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            GameManager.Instance.gameState = "in dialogue";
            GameManager.Instance.isInBoss = true;
            _dialogueManagerScript._dialogue = dialogue;
            _dialogueManagerScript.StartDialogue(gameObject);
            _boss.gameObject.SetActive(true);
            _entranceWall.SetActive(true);
            _exitWall.SetActive(true);
            _StaticBox1.SetActive(true);
            _StaticBox2.SetActive(true);
        }
    }

    public void StartBoss()
    {
        _boss.finishedTalking = true;
    }
}
